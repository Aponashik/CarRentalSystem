using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRS.Models;

namespace CRS.Controllers
{
   
    public class returncarController : Controller
    {
        supercarEntities db = new supercarEntities();
        // GET: returncar
        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult Return(returncar rcar)
        {
            if (ModelState.IsValid)
            {
                db.returncars.Add(rcar);
                var car = db.carregs.SingleOrDefault(e => e.carno == rcar.carno);
                if (car == null)
                    return HttpNotFound("CarNo Not Found!");
                car.available = "Yes";
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rcar);
        }


        [HttpPost]
        public ActionResult Getid(String carno)
        {
            var carn = (from s in db.rents
                        where s.carid == carno
                        select new
                        {
                            StartDate = s.sdate,
                            EndDate = s.edate,
                            Custid = s.custid,
                            CarNo = s.carid,
                            Fee = s.fee,
                            ElapsedDays = SqlFunctions.DateDiff("day", s.edate, DateTime.Now)
                        }).ToArray();
            return Json(carn, JsonRequestBehavior.AllowGet);
        }
    }
}