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
    public class RequestsController : Controller
    {
        private PropertyRentalManagementEntities1 db = new PropertyRentalManagementEntities1();

        // GET: Requests

        public ActionResult GetRequests()
        {
            if (Session["ManagerId"] == null)
            {
                return RedirectToAction("Login");
            }
            int id = Int32.Parse(Session["ManagerId"].ToString());
            var requests = db.Requests.Where(x => x.ManagerId == id).ToList();
            return View(requests);
            
        }

        public ActionResult Index()
        {
            var requests = db.Requests.Include(r => r.Manager).Include(r => r.Tenant);
            return View(requests.ToList());
        }

        // GET: Requests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // GET: Requests/Create
        public ActionResult Create()
        {
            ViewBag.ManagerId = new SelectList(db.Managers, "ManagerId", "FirstName");
            ViewBag.TenantId = new SelectList(db.Tenants, "TenantId", "FirstName");
            

            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RequestId,TenantId,ManagerId,Appointment")] Request request)
        {
            if (ModelState.IsValid)
            {
                db.Requests.Add(request);
                db.SaveChanges();
                return RedirectToAction("Index", "Properties");
            }

            ViewBag.ManagerId = new SelectList(db.Managers, "ManagerId", "FirstName", request.ManagerId);
            ViewBag.TenantId = new SelectList(db.Tenants, "TenantId", "FirstName", request.TenantId);
            
            return View(request);
        }

        // GET: Requests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            ViewBag.ManagerId = new SelectList(db.Managers, "ManagerId", "FirstName", request.ManagerId);
            ViewBag.TenantId = new SelectList(db.Tenants, "TenantId", "FirstName", request.TenantId);
            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RequestId,TenantId,ManagerId,Appointment")] Request request)
        {
            if (ModelState.IsValid)
            {
                db.Entry(request).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ManagerId = new SelectList(db.Managers, "ManagerId", "FirstName", request.ManagerId);
            ViewBag.TenantId = new SelectList(db.Tenants, "TenantId", "FirstName", request.TenantId);
            return View(request);
        }

        // GET: Requests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Request request = db.Requests.Find(id);
            db.Requests.Remove(request);
            db.SaveChanges();
            return RedirectToAction("GetRequests", "Requests");
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
