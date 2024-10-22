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
    public class AdminController : Controller
    {
        private RendevousResturantContext db = new RendevousResturantContext();

        // GET: Admin
        public ActionResult Index()
        {
            return View(db.Staff.ToList());
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staff.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StaffId,EmployeeID,Password,StaffEmail,DateCreated,IsActive,IsManager")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                db.Staff.Add(staff);
                db.SaveChanges();

                // Send email with link to set new password
                string resetLink = Url.Action("SetNewPassword", "Admin", new { id = staff.StaffId }, Request.Url.Scheme);
                SetNewPasswordEmail(staff.StaffEmail, "New User", DateTime.Now, TimeSpan.Zero, null, resetLink);

                return RedirectToAction("Index");
            }
            return View(staff);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staff.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StaffId,EmployeeID,Password,StaffEmail,DateCreated,IsActive,IsManager")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(staff);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staff.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Staff staff = db.Staff.Find(id);
            db.Staff.Remove(staff);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void SetNewPasswordEmail(string email, string fullnames, DateTime date, TimeSpan time, string specialRequests, string resetLink)
        {
            string body = $"Dear {fullnames},\n\nTo create your password, please click the following link:\n{resetLink}\n\nThank you!";
            MailMessage message = new MailMessage
            {
                From = new MailAddress("chettyelizabeth79@gmail.com"),
                Subject = "Password Reset",
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

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Staff staff)
        {
            System.Diagnostics.Debug.WriteLine($"EmployeeID: {staff.EmployeeID}, Password: {staff.Password}");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
                }
                return View(staff);
            }

            var loggedInStaff = db.Staff
               .Where(s => s.EmployeeID == staff.EmployeeID && s.Password == staff.Password && s.IsActive)
               .FirstOrDefault();

            if (loggedInStaff != null)
            {
                // Set session or cookie for logged in user
                Session["EmployeeID"] = loggedInStaff.EmployeeID;

                if (loggedInStaff.IsManager)
                {
                    return RedirectToAction("Create", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Reservations");
                }
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(staff);
        }

        // GET: Reset Password
        public ActionResult ResetPassword()
        {
            return View();
        }

        // POST: Reset Password (direct link)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string email)
        {
            var staff = db.Staff.FirstOrDefault(s => s.StaffEmail == email && s.IsActive);
            if (staff != null)
            {
                // Send the reset password link to the email
                // You can use the SendConfirmationEmail method here
                string resetLink = Url.Action("SetNewPassword", "Admin", new { id = staff.StaffId }, Request.Url.Scheme);
                SendResetPasswordEmail(staff.StaffEmail, "User", DateTime.Now, TimeSpan.Zero, null, resetLink);
            }
            return RedirectToAction("Login"); // Redirect to login after sending email
        }

        // GET: Set New Password
        public ActionResult SetNewPassword(int id)
        {
            var staff = db.Staff.Find(id);
            return View(staff);
        }

        // POST: Set New Password
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetNewPassword(Staff staff, string newPassword)
        {
            if (ModelState.IsValid)
            {
                var existingStaff = db.Staff.Find(staff.StaffId);
                if (existingStaff != null)
                {
                    existingStaff.Password = newPassword; // Directly set new password
                    db.SaveChanges();
                }
                return RedirectToAction("Login");
            }
            return View(staff);
        }

        private void SendResetPasswordEmail(string email, string fullnames, DateTime date, TimeSpan time, string specialRequests, string resetLink)
        {
            string body = $"Dear {fullnames},\n\nTo reset your password, please click the following link:\n{resetLink}\n\nThank you!";
            MailMessage message = new MailMessage
            {
                From = new MailAddress("chettyelizabeth79@gmail.com"),
                Subject = "Password Reset",
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
