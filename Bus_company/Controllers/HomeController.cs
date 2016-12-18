using Bus_company.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bus_company.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
        private Models.TimeTableEntities db = new Models.TimeTableEntities();
        public ActionResult Bus()
        {
           
            var Items = db.Buses;
            return View(Items);
        }

        public ActionResult CreateBus()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateBus([Bind(Include = "Id,Model,Description,Preview,Rashod_many,Rashod_fuel,Quantity_of_seats")] Bus bus)
        {            
            db.Buses.Add(bus);
            db.SaveChanges();
            var Items = db.Buses;
            return RedirectToAction("Bus",Items);
        }

        public ActionResult DeleteBus(int item_id)
        {
            var Item = db.Buses.FirstOrDefault(x => x.Id == item_id);
            db.Buses.Remove(Item);
            db.SaveChanges();
            var Items = db.Buses;
            return RedirectToAction("Bus", Items);
        }

        [HttpGet]
        public ActionResult BusPage(int item_id)
        {
            var Item = db.Buses.FirstOrDefault(x => x.Id == item_id);
            return View(Item);
        }
        [HttpPost]
        public ActionResult BusPage(Bus Item)
        {
            Bus bus =new Bus();
            bus=db.Buses.Find(Item.Id);
            db.Entry(bus).State = EntityState.Modified;
            bus.Model = Item.Model;
            bus.Preview = Item.Preview;
            bus.Rashod_fuel = Item.Rashod_fuel;
            bus.Rashod_many = Item.Rashod_many;
            bus.Quantity_of_seats = Item.Quantity_of_seats;
            db.SaveChanges();
            return View(Item);
        }
        

        public ActionResult Company()
        {
            return View();
        }
        ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult Employee()
        {
            var RoleDriver = context.Roles.Where(r => r.Name.Equals("driver", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            string[] IDDrivers = RoleDriver.Users.Select(x => x.UserId).ToArray();
            ViewBag.Driver = context.Users.Where(x => IDDrivers.Any(y => y == x.Id));
            var RoleUser = context.Roles.Where(r => r.Name.Equals("user", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            string[] IDUsers = RoleUser.Users.Select(x => x.UserId).ToArray();
            ViewBag.User = db.AspNetUsers.Where(x => IDUsers.Any(y => y == x.Id));
            var RoleOperator = context.Roles.Where(r => r.Name.Equals("operator", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            string[] IDOperators = RoleOperator.Users.Select(x => x.UserId).ToArray();
            ViewBag.Operator = db.AspNetUsers.Where(x => IDOperators.Any(y => y == x.Id));
            var Items = context.Users;
            return View(Items);
        }

        public ActionResult Delete(string id)
        {
            ApplicationUser user = context.Users.Find(id);
            context.Users.Remove(user);
            context.SaveChanges();
            var Items = context.Users;
            return View("Employee",Items);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                ViewBag.RolesForThisUser = um.GetRoles(user.Id);

                // prepopulat roles for the view dropdown
                var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                ViewBag.Roles = list;
            }

            return View("Employee");
        }




    }
}