using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
            return View(db.Takeaway.ToList());
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
                return RedirectToAction("Index");
            }

            return View(takeaway);
        }

        // GET: Takeaways/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Takeaways/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TakeawayID,Fullnames,Email,OrderDate,OrderStatus,TotalAmount,Quantity,ItemPrice,OrderItem")] Takeaway takeaway)
        {
            if (ModelState.IsValid)
            {
                db.Entry(takeaway).State = EntityState.Modified;
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

        // POST: Takeaways/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Takeaway takeaway = db.Takeaway.Find(id);
            db.Takeaway.Remove(takeaway);
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
