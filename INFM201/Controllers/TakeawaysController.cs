using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using INFM201.Models;

namespace INFM201.Controllers
{
    public class TakeawaysController : Controller
    {
        private RendevousResturantContext db = new RendevousResturantContext();

        // GET: Takeaways
        public ActionResult Index()
        {
            return View(db.Takeaway.Where( x => x.IsDelete == false).ToList());
        }

        // GET: Takeaways/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Takeaway takeaway = db.Takeaway.Find(id);
            if (takeaway == null)
            {
                return HttpNotFound();
            }
            return View(takeaway);
        }

        // GET: Takeaways/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Takeaways/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TakeawayID,Fullnames,Email,OrderDate,OrderStatus,TotalAmount,Quantity,ItemPrice")] Takeaway takeaway, string OrderItems)
        {
            if (ModelState.IsValid)
            {
                takeaway.OrderStatus = 0;
                takeaway.OrderItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OrderItems>>(OrderItems);
                takeaway.TotalAmount = takeaway.GetPrice();
                db.Takeaway.Add(takeaway);
                db.SaveChanges();

                SendConfirmationEmail(takeaway.Email, takeaway.Fullnames, DateTime.Now);

            }


            //  return RedirectToAction("Index");
            //return View(takeaway);

            return RedirectToAction("Details", new { id = takeaway.TakeawayID });
        }


        // GET: Takeaways/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var takeaway = db.Takeaway.Include(t => t.OrderItems).SingleOrDefault(t => t.TakeawayID == id);
            if (takeaway == null)
            {
                return HttpNotFound();
            }
            return View(takeaway);
        }

        // POST: Takeaways/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TakeawayID,Fullnames,Email,OrderDate,OrderStatus,TotalAmount,Quantity,ItemPrice,OrderItem")] Takeaway takeaway, string OrderItems)
        {
            if (ModelState.IsValid)
            {
                var dbtakeaway = db.Takeaway.Include(t => t.OrderItems).SingleOrDefault(t => t.TakeawayID == takeaway.TakeawayID);

                dbtakeaway.OrderItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OrderItems>>(OrderItems);
                dbtakeaway.TotalAmount = dbtakeaway.GetPrice();
                dbtakeaway.Email = takeaway.Email;
                dbtakeaway.Fullnames = takeaway.Fullnames;
                dbtakeaway.OrderDate = takeaway.OrderDate;
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(takeaway);
        }

        // GET: Takeaways/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Takeaway takeaway = db.Takeaway.Find(id);
            if (takeaway == null)
            {
                return HttpNotFound();
            }
            return View(takeaway);
        }


        private void SendConfirmationEmail(string email, string fullnames, DateTime date)
        {
            string body = $"Dear {fullnames},\n\nYour Order has been placed. {date.ToShortDateString()} at {date.ToString(@"hh\:mm")} you can collect in the next 30 mins.\n\nThank you!";
            MailMessage message = new MailMessage
            {
                From = new MailAddress("chettyelizabeth79@gmail.com"), // Your Gmail address
                Subject = "Reservation Confirmation",
                Body = body
            };
            message.To.Add(email);

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("chettyelizabeth79@gmail.com", "oufd qrwh pbkl dpnl"), // Use a config file for security
                EnableSsl = true
            };

            try
            {
                smtpClient.Send(message);
            }
            catch (SmtpException ex)
            {
                // Handle the exception
                System.Diagnostics.Debug.WriteLine($"Error sending email: {ex.Message}");
            }
        }


        // POST: Takeaways/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Takeaway takeaway = db.Takeaway.Find(id);
            takeaway.IsDelete = true;
            db.SaveChanges();
            return RedirectToAction("Index");
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
