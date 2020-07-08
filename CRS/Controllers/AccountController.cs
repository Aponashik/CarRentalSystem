using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRS.Models;
using System.Web.Security;

namespace CRS.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            using (var db=new supercarEntities())
            {
                bool isValid = db.Users.Any(x => x.UserName == user.UserName && x.Password == user.Password);
                if (isValid)
                {
                    FormsAuthentication.SetAuthCookie(user.UserName, false);
                    return RedirectToAction("Index", "Car");
                }
                ModelState.AddModelError("", "Invalied UserName and Password");
                return View();
            }
                
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(User user)
        {
            using (var db = new supercarEntities())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
            return RedirectToAction("Login");
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Home"); 
        }
    }
}