using MVCBlueRay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCBlueRay.Controllers
{
    public class BluRayController : Controller
    {
        // GET: BluRay
        public ActionResult Index()
        {
            using (MyDbContext db = new MyDbContext())
            {
                return View(db.BluRays.ToList());
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(BluRay bluRay)
        {
            if (ModelState.IsValid)
            {
                using (MyDbContext db = new MyDbContext())
                {
                    db.BluRays.Add(bluRay);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = bluRay.Title + " successfully created.";
            }
            return View();
        }
    }
}