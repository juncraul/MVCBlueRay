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
                return View(db.Users.ToList());
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
                    db.Users.Add(user);
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
                User userFound = db.Users.FirstOrDefault(u => u.Username == user.Username);
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
                int id = int.Parse(Session["UserID"].ToString());
                using (MyDbContext db = new MyDbContext())
                {
                    User userFound = db.Users.FirstOrDefault(u => u.Id == id);
                    List<BluRay> userBlueRay = db.UserBlueRays.Where(u => u.UserId == id).Select(a=>a.BluRay).ToList();
                    return View(userBlueRay);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult AddBluRay()
        {
            if (Session["UserID"] != null)
            {
                int id = int.Parse(Session["UserID"].ToString());
                using (MyDbContext db = new MyDbContext())
                {
                    List<CheckBloxViewModel> bluRay = db.BluRays
                        .Select(a => new CheckBloxViewModel
                        {
                            Id = a.Id,
                            Name = a.Title,
                            Checked = db.UserBlueRays.Count(u=>u.BluRayId == a.Id && u.UserId == id) == 0
                        }).ToList();

                    return View(bluRay);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public ActionResult AddBluRay(List<CheckBloxViewModel> checkBoxs)
        {
            if (Session["UserID"] != null)
            {
                int id = int.Parse(Session["UserID"].ToString());
                using (MyDbContext db = new MyDbContext())
                {
                    foreach(CheckBloxViewModel c in checkBoxs)
                    {
                        db.UserBlueRays.Add(new UserBlueRay()
                        {
                            BluRayId = c.Id,
                            UserId = id
                        });
                    }
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
    }
}