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

        //Edit
        public ActionResult Edit(int id)
        {
            using (MyDbContext db = new MyDbContext())
            {
                BluRay blueRay = db.BluRays.FirstOrDefault(u => u.Id == id);
                if (blueRay != null)
                {
                    return View(blueRay);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        public ActionResult Edit(BluRay blueRay)
        {
            if (ModelState.IsValid)
            {
                using (MyDbContext db = new MyDbContext())
                {
                    db.Entry(blueRay).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(blueRay);
        }

        //Details
        public ActionResult Details(int id)
        {
            using (MyDbContext db = new MyDbContext())
            {
                BluRay blueRay = db.BluRays.FirstOrDefault(u => u.Id == id);
                if (blueRay != null)
                {
                    return View(blueRay);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        //Delete
        public ActionResult Delete(int id)
        {
            using (MyDbContext db = new MyDbContext())
            {
                BluRay blueRay = db.BluRays.FirstOrDefault(u => u.Id == id);
                if (blueRay != null)
                {
                    db.Entry(blueRay).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
    }
}