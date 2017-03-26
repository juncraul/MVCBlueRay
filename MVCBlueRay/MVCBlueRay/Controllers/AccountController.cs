using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCBlueRay.Models;
using System.Net;

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

            if (Session["UserID"] == null)
                return RedirectToAction("Login");

            int id = int.Parse(Session["UserID"].ToString());
            using (MyDbContext db = new MyDbContext())
            {
                User userFound = db.Users.FirstOrDefault(u => u.Id == id);
                List<BluRay> userBlueRay = db.UserBlueRays.Where(u => u.UserId == id).Select(a=>a.BluRay).ToList();
                return View(userBlueRay);
            }
        }

        //Edit
        public ActionResult Edit(int id)
        {
            using (MyDbContext db = new MyDbContext())
            {
                User user = db.Users.FirstOrDefault(u => u.Id == id);
                if(user != null)
                {
                    return View(user);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if(ModelState.IsValid)
            {
                using (MyDbContext db = new MyDbContext())
                {
                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(user);
        }

        public ActionResult AddBluRay()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login");
            
            int id = int.Parse(Session["UserID"].ToString());
            using (MyDbContext db = new MyDbContext())
            {
                User user = db.Users.FirstOrDefault(u => u.Id == id);
                List<CheckBloxViewModel> bluRay = db.BluRays
                    .Select(a => new CheckBloxViewModel
                    {
                        Id = a.Id,
                        Name = a.Title,
                        Checked = db.UserBlueRays.Count(u=>u.BluRayId == a.Id && u.UserId == id) > 0
                    }).ToList();

                UserViewModel userViewModel = new UserViewModel()
                {
                    BluRays = bluRay,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Id = user.Id
                };

                return View(userViewModel);
            }
        }

        [HttpPost]
        public ActionResult AddBluRay(UserViewModel userView)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login");
            
            int id = int.Parse(Session["UserID"].ToString());
            using (MyDbContext db = new MyDbContext())
            {
                User user = db.Users.FirstOrDefault(u => u.Id == id);

                //Delete any uncheked bluerays
                foreach(CheckBloxViewModel c in userView.BluRays.Where(b=>!b.Checked))
                {
                    //check if blueray exists in user's collection
                    UserBlueRay userBlueRayToDelete = user.UserBlueRays.FirstOrDefault(u => u.BluRayId == c.Id);
                    if (userBlueRayToDelete == null) continue;

                    //delete the use-bluray connection
                    db.Entry(userBlueRayToDelete).State = System.Data.Entity.EntityState.Deleted;
                }

                //Loop through checked bluerays and add them to the user
                foreach (CheckBloxViewModel c in userView.BluRays.Where(b=>b.Checked))
                {
                    //Blueray already exists in user's collection
                    if (db.UserBlueRays.FirstOrDefault(a => a.BluRayId == c.Id) != null) continue;

                    //Create new user-bluray connection and add it 
                    UserBlueRay entity = db.UserBlueRays.Create();
                    entity.BluRayId = c.Id;
                    entity.UserId = id;
                    db.UserBlueRays.Add(entity);
                }
                db.SaveChanges();
                return RedirectToAction("LoggedIn");
            }
        }
    }
}