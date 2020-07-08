using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CRS.Models;

namespace CRS.Controllers
{
   
    public class RentController : Controller
    {

        supercarEntities db = new supercarEntities();
        // GET: Rent
        public ActionResult Index()
        {
            var result = (from r in db.rents
                          join c in db.carregs on r.carid equals c.carno
                          
                         
                          select new RentalViewModel
                          {
                              id = r.id,
                              carid = r.carid,
                              custid=r.custid,  
                              fee = r.fee,
                              available = c.available,
                              sdate = r.sdate,
                              edate = r.edate
                          }).ToList();
            return View(result);
        }

        [HttpGet]
        public ActionResult Getcar()
        {
            var car = db.carregs.ToList();
            return Json(car, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Getid(int id)
        {
            var customer = (from s in db.customers where s.id == id select s.custname).ToList();
            /*var customer = (from s in db.customers where s.id == id select new { s.custname,s.email}).ToList();*/
            return Json(customer, JsonRequestBehavior.AllowGet);
        }

       [HttpPost]
        public ActionResult Getemail(int id)
        {
            var customer = (from s in db.customers where s.id == id select s.email)
                .ToList();
            return Json(customer, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Getavail(String carno)
        {
            var caravail = (from s in db.carregs where s.carno == carno select s.available).FirstOrDefault();
            return Json(caravail, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Save(rent rent)
        {
            if (ModelState.IsValid)
            {
                db.rents.Add(rent);
                var car = db.carregs.SingleOrDefault(e => e.carno == rent.carid);

                if (car == null)
                    return HttpNotFound("CarNo is not valid!");

                car.available = "No";
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rent);
        }

        

        


    }
}