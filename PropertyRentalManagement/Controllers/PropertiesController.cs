using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PropertyRentalManagement.Models;

namespace PropertyRentalManagement.Controllers
{
    public class PropertiesController : Controller
    {   
        private PropertyRentalManagementEntities1 db = new PropertyRentalManagementEntities1();

        

        public ActionResult DashboardForTenant(string city)
        {

            if (Session["TenantId"] == null)
            {
                return RedirectToAction("Login", "Tenants");
            }
            
            return View(db.Properties.Where(x => x.City.StartsWith(city) || city == null || city == "").ToList());
        }

        public ActionResult ShowAllProperties()
        {
            if (Session["TenantUsername"] == null || Session["ManagerUsername"] == null)
            {
                return RedirectToAction("Login", "Tenants");
            }
            //var properties = db.Properties.Include(p => p.Manager);
            
            return RedirectToAction("DashboardForTenant", new { city = "" });
        }

        // GET: Properties
        public ActionResult Index()
        {
            if (Session["AdminId"] == null && Session["ManagerId"] == null && Session["TenantId"] == null)
            {
                return RedirectToAction("Login");
            }


            var properties = db.Properties.Include(p => p.Manager);
            return View(properties.ToList());
        }

        // GET: Properties/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["AdminId"] == null && Session["ManagerId"] == null && Session["TenantId"] == null)
            {
                return RedirectToAction("Login");
            }


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Property property = db.Properties.Find(id);
            if (property == null)
            {
                return HttpNotFound();
            }
            return View(property);
        }

        // GET: Properties/Create
        public ActionResult Create()
        {

            if (Session["AdminId"] == null && Session["ManagerId"] == null && Session["TenantId"] == null)
            {
                return RedirectToAction("Login");
            }

            ViewBag.ManagerId = new SelectList(db.Managers, "ManagerId", "FirstName");
            
            return View();
        }

        // POST: Properties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PropertyId,PropertyName,Address,Type,Bedroom,Bathroom,Rent,Utilities,Lease,Status,ManagerId")] Property property)
        {
            if (Session["AdminId"] == null && Session["ManagerId"] == null && Session["TenantId"] == null)
            {
                return RedirectToAction("Login");
            }


            if (ModelState.IsValid)
            {
                db.Properties.Add(property);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ManagerId = new SelectList(db.Managers, "ManagerId", "FirstName", property.ManagerId);
            return View(property);
        }

        // GET: Properties/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["AdminId"] == null && Session["ManagerId"] == null && Session["TenantId"] == null)
            {
                return RedirectToAction("Login");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Property property = db.Properties.Find(id);
            if (property == null)
            {
                return HttpNotFound();
            }
            ViewBag.ManagerId = new SelectList(db.Managers, "ManagerId", "FirstName", property.ManagerId);
            return View(property);
        }

        // POST: Properties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PropertyId,PropertyName,Address,Type,Bedroom,Bathroom,Rent,Utilities,Lease,Status,ManagerId")] Property property)
        {

            if (Session["AdminId"] == null && Session["ManagerId"] == null && Session["TenantId"] == null)
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                db.Entry(property).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ManagerId = new SelectList(db.Managers, "ManagerId", "FirstName", property.ManagerId);
            return View(property);
        }

        // GET: Properties/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["AdminId"] == null && Session["ManagerId"] == null && Session["TenantId"] == null)
            {
                return RedirectToAction("Login");
            }


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Property property = db.Properties.Find(id);
            if (property == null)
            {
                return HttpNotFound();
            }
            return View(property);
        }

        // POST: Properties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["AdminId"] == null && Session["ManagerId"] == null && Session["TenantId"] == null)
            {
                return RedirectToAction("Login");
            }

            Property property = db.Properties.Find(id);
            db.Properties.Remove(property);
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
