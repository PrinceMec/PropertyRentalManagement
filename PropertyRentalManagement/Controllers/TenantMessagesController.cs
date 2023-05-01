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
    public class TenantMessagesController : Controller
    {
        private PropertyRentalManagementEntities1 db = new PropertyRentalManagementEntities1();

        public ActionResult GetLists()
        {
            if (Session["TenantId"] == null)
            {
                return RedirectToAction("Login");
            }
            int id = Int32.Parse(Session["TenantId"].ToString());
            var messages = db.TenantMessages.Where(x => x.Receiver == id).ToList();
            return View(messages);
        }

        // GET: TenantMessages
        public ActionResult Index()
        {
            var tenantMessages = db.TenantMessages.Include(t => t.Manager).Include(t => t.Tenant);
            return View(tenantMessages.ToList());
        }

        // GET: TenantMessages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TenantMessage tenantMessage = db.TenantMessages.Find(id);
            if (tenantMessage == null)
            {
                return HttpNotFound();
            }
            return View(tenantMessage);
        }

        // GET: TenantMessages/Create
        public ActionResult Create()
        {
            ViewBag.Sender = new SelectList(db.Managers, "ManagerId", "FirstName");
            ViewBag.Receiver = new SelectList(db.Tenants, "TenantId", "FirstName");
            return View();
        }

        // POST: TenantMessages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Sender,Receiver,Message")] TenantMessage tenantMessage)
        {
            if (ModelState.IsValid)
            {
                db.TenantMessages.Add(tenantMessage);
                db.SaveChanges();
                return RedirectToAction("GetLists", "ManagerMessages");
            }

            ViewBag.Sender = new SelectList(db.Managers, "ManagerId", "FirstName", tenantMessage.Sender);
            ViewBag.Receiver = new SelectList(db.Tenants, "TenantId", "FirstName", tenantMessage.Receiver);
            return View(tenantMessage);
        }

        // GET: TenantMessages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TenantMessage tenantMessage = db.TenantMessages.Find(id);
            if (tenantMessage == null)
            {
                return HttpNotFound();
            }
            ViewBag.Sender = new SelectList(db.Managers, "ManagerId", "FirstName", tenantMessage.Sender);
            ViewBag.Receiver = new SelectList(db.Tenants, "TenantId", "FirstName", tenantMessage.Receiver);
            return View(tenantMessage);
        }

        // POST: TenantMessages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Sender,Receiver,Message")] TenantMessage tenantMessage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tenantMessage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Sender = new SelectList(db.Managers, "ManagerId", "FirstName", tenantMessage.Sender);
            ViewBag.Receiver = new SelectList(db.Tenants, "TenantId", "FirstName", tenantMessage.Receiver);
            return View(tenantMessage);
        }

        // GET: TenantMessages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TenantMessage tenantMessage = db.TenantMessages.Find(id);
            if (tenantMessage == null)
            {
                return HttpNotFound();
            }
            return View(tenantMessage);
        }

        // POST: TenantMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TenantMessage tenantMessage = db.TenantMessages.Find(id);
            db.TenantMessages.Remove(tenantMessage);
            db.SaveChanges();
            return RedirectToAction("GetLists", "ManagerMessages");
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
