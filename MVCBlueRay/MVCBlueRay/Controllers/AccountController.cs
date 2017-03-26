using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCBlueRay.Models;

namespace MVCBlueRay.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            using (MyDbContext db = new MyDbContext())
            {
                return View(db.User.ToList());
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            if(ModelState.IsValid)
            {
                using (MyDbContext db = new MyDbContext())
                {
                    db.User.Add(user);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = user.Username + " successfully registred."; 
            }
            return View();
        }

        //Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            using (MyDbContext db = new MyDbContext())
            {
                User userFound = db.User.FirstOrDefault(u => u.Username == user.Username);
                if(userFound != null)
                {
                    if(userFound.Password == user.Password)
                    {
                        Session["UserID"] = userFound.Id.ToString();
                        Session["Username"] = userFound.Username.ToString();
                        return RedirectToAction("LoggedIn");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Password is wrong.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Username does not exist.");
                }
            }
            return View();
        }

        public ActionResult LoggedIn()
        {
            if(Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
    }
}