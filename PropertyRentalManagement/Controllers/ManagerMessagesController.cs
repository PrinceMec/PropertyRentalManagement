using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PropertyRentalManagement.Models;

namespace PropertyRentalManagement.Controllers
{
    public class ManagerMessagesController : Controller
    {
        private PropertyRentalManagementEntities1 db = new PropertyRentalManagementEntities1();

        // GET: ManagerMessages
        public ActionResult Index()
        {
            var managerMessages = db.ManagerMessages.Include(m => m.Manager).Include(m => m.Tenant);
            return View(managerMessages.ToList());
        }

        // GET: ManagerMessages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ManagerMessage managerMessage = db.ManagerMessages.Find(id);
            if (managerMessage == null)
            {
                return HttpNotFound();
            }
            return View(managerMessage);
        }

        // GET: ManagerMessages/Create
        public ActionResult Create()
        {
            ViewBag.Receiver = new SelectList(db.Managers, "ManagerId", "FirstName");
            ViewBag.Sender = new SelectList(db.Tenants, "TenantId", "FirstName");
            return View();
        }

        // POST: ManagerMessages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Sender,Receiver,Message")] ManagerMessage managerMessage)
        {
            if (ModelState.IsValid)
            {
                db.ManagerMessages.Add(managerMessage);
                db.SaveChanges();
                return RedirectToAction("Index", "Properties");
            }

            ViewBag.Receiver = new SelectList(db.Managers, "ManagerId", "FirstName", managerMessage.Receiver);
            ViewBag.Sender = new SelectList(db.Tenants, "TenantId", "FirstName", managerMessage.Sender);
            return View(managerMessage);
        }

        // GET: ManagerMessages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ManagerMessage managerMessage = db.ManagerMessages.Find(id);
            if (managerMessage == null)
            {
                return HttpNotFound();
            }
            ViewBag.Receiver = new SelectList(db.Managers, "ManagerId", "FirstName", managerMessage.Receiver);
            ViewBag.Sender = new SelectList(db.Tenants, "TenantId", "FirstName", managerMessage.Sender);
            return View(managerMessage);
        }

        // POST: ManagerMessages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Sender,Receiver,Message")] ManagerMessage managerMessage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(managerMessage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Receiver = new SelectList(db.Managers, "ManagerId", "FirstName", managerMessage.Receiver);
            ViewBag.Sender = new SelectList(db.Tenants, "TenantId", "FirstName", managerMessage.Sender);
            return View(managerMessage);
        }

        // GET: ManagerMessages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ManagerMessage managerMessage = db.ManagerMessages.Find(id);
            if (managerMessage == null)
            {
                return HttpNotFound();
            }
            return View(managerMessage);
        }

        // POST: ManagerMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ManagerMessage managerMessage = db.ManagerMessages.Find(id);
            db.ManagerMessages.Remove(managerMessage);
            db.SaveChanges();
            return RedirectToAction("GetLists", "TenantMessages");
        }

        public ActionResult GetLists()
        {
            if (Session["ManagerId"] == null)
            {
                return RedirectToAction("Login");
            }
            int id = Int32.Parse(Session["ManagerId"].ToString());
            var messages = db.ManagerMessages.Where(x=>x.Receiver == id).ToList();
            return View(messages);
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
