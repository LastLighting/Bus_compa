using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bus_company.Models;
using Microsoft.AspNet.Identity;

namespace Bus_company.Controllers
{
    public class TimetablesController : Controller
    {
        private TimeTableEntities db = new TimeTableEntities();
        private TimeTableEntities ent = new TimeTableEntities();
        // GET: Timetables
        public ActionResult Index()
        {
            var timetables = db.Timetables.Include(t => t.AspNetUser).Include(t => t.Bus1).Include(t => t.Trip1).OrderBy(t => t.DateTime);
            if (User.IsInRole("driver"))
            {
                ViewBag.Driver = db.Timetables.Include(t => t.AspNetUser).Include(t => t.Bus1).Include(t => t.Trip1).Where(n => n.Driver.Equals(User.Identity.GetUserId()));
            }
            
            return View(timetables.ToList());
        }

        // GET: Timetables/Details/5
        public ActionResult Details(int? id)
        {                    
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Timetable timetable = db.Timetables.Find(id);
            if (timetable == null)
            {
                return HttpNotFound();
            }
            return View(timetable);
        }
       
        public ActionResult Oplata(int id)
        {
            WebServicePay web = new WebServicePay();
            string otvet = null;            
            Timetable timetable = db.Timetables.Find(id);          
            ViewBag.Web = otvet;
            return View(timetable);
        }

        [HttpPost]
        public ActionResult Oplata(int? id, int idcard, int summ)
        {
            WebServicePay web = new WebServicePay();
            string otvet = web.operation(idcard, summ);
            Timetable timetable = db.Timetables.Find(id);
            if (otvet == "Недостаточно средств"| otvet == "Такой карточки нет")
            {
                
                ViewBag.Web = otvet;
                return View(timetable);
            }
            else
            {
                timetable.Free_paces -= 1;
                db.Entry(timetable).State = EntityState.Modified;
                db.SaveChanges();
                Order order = new Order();
                order.IdTimeTable = id;
                order.IdUser = User.Identity.GetUserId();
                order.Date = DateTime.Now;
                order.Money = summ;
                ent.Orders.Add(order);
                ent.SaveChanges();
                var timetables = db.Timetables.Include(t => t.AspNetUser).Include(t => t.Bus1).Include(t => t.Trip1);
                return RedirectToAction("Index", "Manage");
            }           
        }


        ApplicationDbContext context = new ApplicationDbContext();
        // GET: Timetables/Create
        public ActionResult Create()
        {
            var thisRole = context.Roles.Where(r => r.Name.Equals("driver", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            string[] memberIDs = thisRole.Users.Select(x => x.UserId).ToArray();
            ViewBag.Driver = new SelectList(db.AspNetUsers.Where(x => memberIDs.Any(y => y == x.Id)), "Id", "Name");
            ViewBag.Bus = new SelectList(db.Buses, "Id", "Model");
            ViewBag.Trip = new SelectList(db.Trips, "Id", "begin");
            return View();
        }

        // POST: Timetables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DateTime,Trip,Bus,Driver,Free_paces,Price_place")] Timetable timetable)
        {
            if (ModelState.IsValid)
            {
                db.Timetables.Add(timetable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Driver = new SelectList(db.AspNetUsers, "Id", "Name", timetable.Driver);
            ViewBag.Bus = new SelectList(db.Buses, "Id", "Model", timetable.Bus);
            ViewBag.Trip = new SelectList(db.Trips, "Id", "begin", timetable.Trip);
            return View(timetable);
        }

        // GET: Timetables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Timetable timetable = db.Timetables.Find(id);
            if (timetable == null)
            {
                return HttpNotFound();
            }
            ViewBag.Driver = new SelectList(db.AspNetUsers, "Id", "Name", timetable.Driver);
            ViewBag.Bus = new SelectList(db.Buses, "Id", "Model", timetable.Bus);
            ViewBag.Trip = new SelectList(db.Trips, "Id", "begin", timetable.Trip);
            return View(timetable);
        }

        // POST: Timetables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DateTime,Trip,Bus,Driver,Free_paces,Price_place")] Timetable timetable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(timetable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Driver = new SelectList(db.AspNetUsers, "Id", "Name", timetable.Driver);
            ViewBag.Bus = new SelectList(db.Buses, "Id", "Model", timetable.Bus);
            ViewBag.Trip = new SelectList(db.Trips, "Id", "begin", timetable.Trip);
            return View(timetable);
        }

        // GET: Timetables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Timetable timetable = db.Timetables.Find(id);
            if (timetable == null)
            {
                return HttpNotFound();
            }
            return View(timetable);
        }

        // POST: Timetables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Timetable timetable = db.Timetables.Find(id);
            db.Timetables.Remove(timetable);
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
