using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using INFM201.Models;
using System.Diagnostics;
using System.Data.Entity.Infrastructure;

namespace INFM201.Controllers
{
    public class ReservationsController : Controller
    {
        private RendevousResturantContext db = new RendevousResturantContext();

        // GET: Reservations

      //  [Authorize]
        public ActionResult Index()
        {
            var confirmedReservations = db.Reservations
                .Where(r => r.IsConfirmed && !r.IsDeleted) // Show only confirmed and not deleted reservations
                .Include(r => r.Table) // Include related Table data
                .ToList();
            return View(confirmedReservations);
        }
        public JsonResult Filter(string status)
        {
            // Start with the reservations that are not deleted
            var reservations = db.Reservations.Include(r => r.Table).Where(r => !r.IsDeleted);

            if (!string.IsNullOrEmpty(status))
            {
                switch (status)
                {
                    case "confirmed": reservations = reservations.Where(r => r.IsConfirmed && !r.IsCompleted); break;
                    case "completed":reservations = reservations.Where(r => r.IsCompleted); break;
                    default:break;
                }
            }
            // Retrieve the raw data first
            var reservationList = reservations.ToList();

            var result = reservationList.Select(r => new
            {
                r.ReservationID, r.CFullnames, r.CEmail,
                Date = r.Date.ToString("yyyy-MM-dd"), // Format date after retrieval
                r.Time, r.NumberOfGuests, r.SeatingPreference,
                SpecialRequests = string.IsNullOrEmpty(r.SpecialRequests) ? "None" : r.SpecialRequests,
                r.IsConfirmed, r.IsCompleted, r.TableID, TableNumber = r.Table != null ? r.Table.TableNumber : "N/A" // Handle TableNumber appropriately
            }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: Reservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null || reservation.IsDeleted)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: Reservations/Create
        public ActionResult Create()
        {
            var reservation = new Reservation();
            ViewBag.TableID = new SelectList(db.Tables, "TableID", "TableNumber"); // Populate available tables
            return View(reservation);
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReservationID,CFullnames,CEmail,Date,Time,NumberOfGuests,SeatingPreference,SpecialRequests,TableID")] Reservation reservation, bool? hasSpecialRequests)
        {
            if (hasSpecialRequests == true && string.IsNullOrWhiteSpace(reservation.SpecialRequests))
                ModelState.AddModelError("SpecialRequests", "Please enter your special requests.");

            if (hasSpecialRequests != true || string.IsNullOrWhiteSpace(reservation.SpecialRequests))
                reservation.SpecialRequests = "None";
            else
                reservation.SpecialRequests = reservation.SpecialRequests.Trim();

            if (ModelState.IsValid)
            {
                int totalBookings = db.Reservations.Where(r => r.Date == reservation.Date && r.Time == reservation.Time && !r.IsDeleted).Sum(r => (int?)r.NumberOfGuests) ?? 0;
                int maxCapacity = 26;

                if (totalBookings + reservation.NumberOfGuests > maxCapacity)
                {
                    ModelState.AddModelError("", "Sorry, the total capacity for this time slot has been reached. Please select a different option.");
                    ViewBag.TableID = new SelectList(db.Tables, "TableID", "TableNumber");
                    return View(reservation);
                }

                var availableTables = db.Tables.Where(t => t.MaxGuests >= reservation.NumberOfGuests && t.IsAvailable)
                    .Where(t => !db.Reservations.Any(r => r.TableID == t.TableID && r.Date == reservation.Date && r.Time == reservation.Time && !r.IsDeleted))
                    .Where(t => t.SeatingType == reservation.SeatingPreference).ToList();

                if (!availableTables.Any())
                {
                    ModelState.AddModelError("", "Sorry, no tables are available for the selected seating preference and time.");
                    ViewBag.TableID = new SelectList(db.Tables, "TableID", "TableNumber");
                    return View(reservation);
                }

                reservation.TableID = availableTables.First().TableID;
                reservation.IsConfirmed = true;
                db.Reservations.Add(reservation);
                db.SaveChanges();

                var confirmation = new Confirmation
                {
                    ReservationID = reservation.ReservationID,
                    Name = reservation.CFullnames,
                    EmailAddress = reservation.CEmail,
                    SpecialRequests = reservation.SpecialRequests,
                };

                var table = db.Tables.Find(reservation.TableID);
                if (table != null)
                    table.IsAvailable = false;

                db.Confirmation.Add(confirmation);
                db.SaveChanges();
                SendConfirmationEmail(reservation.CEmail, reservation.CFullnames, reservation.Date, reservation.Time, reservation.SpecialRequests, reservation.TableID);
                TempData["ReservationSuccess"] = true;
                TempData["TableNumber"] = availableTables.First().TableNumber;
                return RedirectToAction("Create");
            }

            ViewBag.TableID = new SelectList(db.Tables, "TableID", "TableNumber", reservation.TableID);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var reservation = db.Reservations.Include(r => r.Table).FirstOrDefault(r => r.ReservationID == id && !r.IsDeleted);

            if (reservation == null)
                return HttpNotFound();

            ViewBag.Tables = db.Tables
            .Where(t => t.IsAvailable && t.SeatingType == reservation.SeatingPreference)
            .ToList();
            ViewBag.TableID = new SelectList(ViewBag.Tables, "TableID", "TableNumber", reservation.TableID);

            return View(reservation);
        }

        // POST: Reservations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReservationID,CFullnames,CEmail,Date,Time,NumberOfGuests,SeatingPreference,SpecialRequests,IsConfirmed")] Reservation reservation)
        {
            if (!ModelState.IsValid) return View(reservation);

            var currentReservation = db.Reservations.Include(r => r.Table).FirstOrDefault(r => r.ReservationID == reservation.ReservationID);
            if (currentReservation == null)
            {
                ModelState.AddModelError("", "The reservation does not exist."); return View(reservation);
            }

            bool tableChanged = currentReservation.NumberOfGuests != reservation.NumberOfGuests ||
                                currentReservation.SeatingPreference != reservation.SeatingPreference ||
                                currentReservation.Date != reservation.Date ||
                                currentReservation.Time != reservation.Time;

            currentReservation.Date = reservation.Date;
            currentReservation.Time = reservation.Time;
            currentReservation.NumberOfGuests = reservation.NumberOfGuests;
            currentReservation.SpecialRequests = reservation.SpecialRequests;
            currentReservation.SeatingPreference = reservation.SeatingPreference;

            if (tableChanged)
            {
                if (currentReservation.TableID >= 1)
                {
                    var previousTable = db.Tables.Find(currentReservation.TableID);
                    if (previousTable != null)
                    {
                        previousTable.IsAvailable = true;
                    }
                }
                var availableTables = db.Tables
                    .Where(t => t.MaxGuests >= reservation.NumberOfGuests && t.IsAvailable &&
                                !db.Reservations.Any(r => r.TableID == t.TableID && r.Date == reservation.Date && r.Time == reservation.Time && !r.IsDeleted) &&
                                t.SeatingType == reservation.SeatingPreference)
                    .ToList();


                if (!availableTables.Any())
                {
                    ModelState.AddModelError("", "Sorry, no tables are available for the selected seating preference and time.");
                    ViewBag.Tables = db.Tables.Where(t => t.IsAvailable).ToList();
                    ViewBag.TableID = new SelectList(ViewBag.Tables, "TableID", "TableNumber", currentReservation.TableID);
                    return View(currentReservation);
                }

                currentReservation.TableID = availableTables.First().TableID;
                var newTable = db.Tables.Find(currentReservation.TableID);
                if (newTable != null)
                {
                    newTable.IsAvailable = false; // Mark the new table as unavailable
                }
            }
            try
            {
                db.SaveChanges(); // Save changes to the database
                SendEditedReservationEmail(currentReservation.CEmail, currentReservation.CFullnames, currentReservation.Date, currentReservation.Time, currentReservation.SpecialRequests, currentReservation.TableID);
                TempData["Message"] = $"Reservation updated successfully! Table assigned: {currentReservation.TableID}.";
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the reservation. Please try again.");
                System.Diagnostics.Debug.WriteLine($"DbUpdateException: {ex.InnerException?.Message}");
                return View(currentReservation);
            }
            return RedirectToAction("Index");
        }

        // GET: Reservations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null || reservation.IsDeleted)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var reservation = db.Reservations.Find(id);
            if (reservation != null)
            {
                var table = db.Tables.Find(reservation.TableID);
                if (table != null)
                {
                    table.IsAvailable = true; // Make the table available again
                    db.Entry(table).State = EntityState.Modified; // Track the change
                }

                reservation.IsDeleted = true; // Mark as deleted
                db.Entry(reservation).State = EntityState.Modified; // Mark entity as modified
                TempData["SuccessMessage"] = "Reservation deleted successfully.";
                db.SaveChanges(); // Save changes to the database
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Complete(int id)
        {
            var reservation = db.Reservations.Find(id);
            if (reservation != null)
            {
                if (!reservation.IsCompleted)
                {
                    reservation.IsCompleted = true; var table = db.Tables.Find(reservation.TableID);
                    if (table != null) { table.IsAvailable = true; db.Entry(table).State = EntityState.Modified; }
                    db.Entry(reservation).State = EntityState.Modified;
                    try { db.SaveChanges(); TempData["SuccessMessage"] = "Reservation marked as complete successfully."; }
                    catch (Exception ex) { TempData["ErrorMessage"] = $"An error occurred while marking the reservation as complete: {ex.Message}"; }
                }
                else { TempData["ErrorMessage"] = "Reservation is already marked as complete."; }
            }
            else { TempData["ErrorMessage"] = "Reservation not found."; }
            return RedirectToAction("Index");
        }

        private void SendConfirmationEmail(string email, string fullnames, DateTime date, TimeSpan time, string specialRequests, int tableID)
        {
            var tableNumber = db.Tables.Find(tableID)?.TableNumber;
            string body = $"Dear {fullnames},\n\nYour booking for {date.ToShortDateString()} at {time.ToString(@"hh\:mm")} has been confirmed.\n\nYou have been assigned: {tableNumber}.\n\n";
            if (!string.IsNullOrEmpty(specialRequests) && specialRequests != "None") body += $"Special Requests: {specialRequests}\n\n";
            body += "Thank you!";
            MailMessage message = new MailMessage
            {
                From = new MailAddress("chettyelizabeth79@gmail.com"),
                Subject = "Reservation Confirmation",
                Body = body
            };
            message.To.Add(email);
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("chettyelizabeth79@gmail.com", "oufd qrwh pbkl dpnl"),
                EnableSsl = true
            };
            try { smtpClient.Send(message); } catch (SmtpException ex) { System.Diagnostics.Debug.WriteLine($"Error sending email: {ex.Message}"); }
        }
        private void SendEditedReservationEmail(string email, string fullnames, DateTime date, TimeSpan time, string specialRequests, int tableID)
        {
            var tableNumber = db.Tables.Find(tableID)?.TableNumber;
            string body = $"Dear {fullnames},\n\nYour booking for {date.ToShortDateString()} at {time.ToString(@"hh\:mm")} has been successfully updated.\n\nYou have been assigned: {tableNumber}.\n\n";
            if (!string.IsNullOrEmpty(specialRequests) && specialRequests != "None") body += $"Special Requests: {specialRequests}\n\n";
            body += "If you need any further assistance or wish to make additional changes, please do not hesitate to reach out.\n\nThank you!";
            MailMessage message = new MailMessage
            {
                From = new MailAddress("chettyelizabeth79@gmail.com"),
                Subject = "Reservation Update Confirmation",
                Body = body
            };
            message.To.Add(email);
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("chettyelizabeth79@gmail.com", "oufd qrwh pbkl dpnl"),
                EnableSsl = true
            };
            try { smtpClient.Send(message); } catch (SmtpException ex) { System.Diagnostics.Debug.WriteLine($"Error sending email: {ex.Message}"); }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
