using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PropertyRentalManagement.Models;

namespace PropertyRentalManagement.Controllers
{
    public class ManagersController : Controller
    {
        private PropertyRentalManagementEntities1 db = new PropertyRentalManagementEntities1();



        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            Session.Clear();

            Session.Abandon();

            Session.RemoveAll();

            return RedirectToAction("Login");
        }
        public ActionResult Login()
        {
            

            return View();
        }

        [HttpPost]
        public ActionResult Login(Manager managerObj)
        {

            

            if (managerObj != null)
            {
                using (PropertyRentalManagementEntities1 db = new PropertyRentalManagementEntities1())
                {
                    //var list = db.admin.ToList();
                    var admn = db.Managers.FirstOrDefault(u => u.Username == managerObj.Username && u.Password == managerObj.Password);

                    if (admn != null)
                    {
                        Session["ManagerId"] = admn.ManagerId.ToString();
                        Session["ManagerUsername"] = admn.Username.ToString();
                        return RedirectToAction("Index", "Properties");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Username or Password is Wrong.");
                    }
                }
                //ModelState.Clear();
                return View();
            }
            else
            {
                return View();
            }
        }

        public ActionResult Dashboard()
        {
            if (Session["ManagerId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        // GET: Managers
        

       public ActionResult Index()
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login");
            }

            return View(db.Managers.ToList());
        }

        // GET: Managers/Details/5
        public ActionResult Details(int? id)
        {

            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = db.Managers.Find(id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            return View(manager);
        }

        // GET: Managers/Create
        public ActionResult Create()
        {

            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        // POST: Managers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ManagerId,FirstName,LastName,Email,Username,Password")] Manager manager)
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login");
            }


            if (ModelState.IsValid)
            {
                db.Managers.Add(manager);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(manager);
        }

        // GET: Managers/Edit/5
        public ActionResult Edit(int? id)
        {

            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = db.Managers.Find(id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            return View(manager);
        }

        // POST: Managers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ManagerId,FirstName,LastName,Email,Username,Password")] Manager manager)
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                db.Entry(manager).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(manager);
        }

        // GET: Managers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = db.Managers.Find(id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            return View(manager);
        }

        // POST: Managers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login");
            }

            Manager manager = db.Managers.Find(id);
            /*Property property = db.Properties.Find(id);
            TenantMessage tenanatmsg = db.TenantMessages.Find(id);
            ManagerMessage managermsg = db.ManagerMessages.Find(id);
            Request request = db.Requests.Find(id);
            Appointment appointment = db.Appointments.Find(id);
            db.Appointments.Remove(appointment);
            db.Requests.Remove(request);
            db.ManagerMessages.Remove(managermsg);
            db.TenantMessages.Remove(tenanatmsg);db.
            db.Properties.Remove(property);*/

            /*List<Appointment> appointments = db.Appointments.Where(x => x.Manager.ManagerId == manager.ManagerId).ToList();

            foreach(Appointment appointment in appointments)
            {
               db.Appointments.Remove(appointment);
            }

            List<Request> requests = db.Requests.Where(x => x.Manager.ManagerId == manager.ManagerId).ToList();

            foreach (Request request in requests)
            {
                db.Requests.Remove(request);
            }

            List<ManagerMessage> requests = db.Requests.Where(x => x.Manager.ManagerId == manager.ManagerId).ToList();

            foreach (Request request in requests)
            {
                db.Requests.Remove(request);
            }*/

            db.Managers.Remove(manager);
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
