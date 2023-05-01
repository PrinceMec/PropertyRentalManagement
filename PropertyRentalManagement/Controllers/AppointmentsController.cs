﻿using System;
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
    public class AppointmentsController : Controller
    {
        private PropertyRentalManagementEntities1 db = new PropertyRentalManagementEntities1();

        public ActionResult GetAppointments()
        {
            if (Session["TenantId"] == null)
            {
                return RedirectToAction("Login");
            }
            int id = Int32.Parse(Session["TenantId"].ToString());
            var appointments = db.Appointments.Where(x => x.TenantId == id).ToList();
            return View(appointments);
        }


        // GET: Appointments
        public ActionResult Index()
        {
            var appointments = db.Appointments.Include(a => a.Manager).Include(a => a.Property).Include(a => a.Tenant);
            return View(appointments.ToList());
        }

        // GET: Appointments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // GET: Appointments/Create
        public ActionResult Create()
        {
            ViewBag.PropertyId = new SelectList(db.Managers, "ManagerId", "FirstName");
            ViewBag.PropertyId = new SelectList(db.Properties, "PropertyId", "PropertyName");
            ViewBag.TenantId = new SelectList(db.Tenants, "TenantId", "FirstName");
            ViewBag.ManagerId = new SelectList(db.Managers, "ManagerId", "FirstName");
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AppointmentId,ManagerId,TenantId,PropertyId,AppointmentDate")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Dashboard", "Managers");
            }

            ViewBag.PropertyId = new SelectList(db.Managers, "ManagerId", "FirstName", appointment.PropertyId);
            ViewBag.PropertyId = new SelectList(db.Properties, "PropertyId", "PropertyName", appointment.PropertyId);
            ViewBag.TenantId = new SelectList(db.Tenants, "TenantId", "FirstName", appointment.TenantId);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            ViewBag.PropertyId = new SelectList(db.Managers, "ManagerId", "FirstName", appointment.PropertyId);
            ViewBag.PropertyId = new SelectList(db.Properties, "PropertyId", "PropertyName", appointment.PropertyId);
            ViewBag.TenantId = new SelectList(db.Tenants, "TenantId", "FirstName", appointment.TenantId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AppointmentId,ManagerId,TenantId,PropertyId,AppointmentDate")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PropertyId = new SelectList(db.Managers, "ManagerId", "FirstName", appointment.PropertyId);
            ViewBag.PropertyId = new SelectList(db.Properties, "PropertyId", "PropertyName", appointment.PropertyId);
            ViewBag.TenantId = new SelectList(db.Tenants, "TenantId", "FirstName", appointment.TenantId);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            db.Appointments.Remove(appointment);
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