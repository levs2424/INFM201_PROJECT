using INFM201.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace INFM201.Controllers
{
    public class AuthenticationController : Controller
    {
        // Hardcoded list of staff members
        private static List<Staff> staffList = new List<Staff>
        {
            new Staff { StaffId = 1, EmployeeID = 22360612, Password = "password1", StaffEmail = "staff1@example.com", IsActive = true },
            new Staff { StaffId = 2, EmployeeID = 22345788, Password = "password1", StaffEmail = "staff2@example.com", IsActive = true },
            new Staff { StaffId = 3, EmployeeID = 22239021, Password = "password1", StaffEmail = "staff3@example.com", IsActive = true }
        };

        // GET: Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Staff staff)
        {
        

            if (IsValid(staff.EmployeeID, staff.Password))
            {
                FormsAuthentication.SetAuthCookie(staff.EmployeeID.ToString(), false);
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ModelState.AddModelError("", "Invalid Employee ID or Password");
                return View(staff);
            }
        }

        // GET: Account/Logout

        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Authentication");
        }

        // Method to validate staff credentials
        private bool IsValid(int employeeID, string password)
        {
            return staffList.Any(s => s.EmployeeID == employeeID && s.Password == password && s.IsActive);
        }
    }
}
