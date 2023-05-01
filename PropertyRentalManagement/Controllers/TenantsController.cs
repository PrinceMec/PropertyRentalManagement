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
    public class TenantsController : Controller
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


        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Tenant tenant)
        {
            if (ModelState.IsValid)
            {

                using (PropertyRentalManagementEntities1 db = new PropertyRentalManagementEntities1())
                {

                    db.Tenants.Add(tenant);
                    db.SaveChanges();

                }

                ModelState.Clear();
                ViewBag.Message = tenant.FirstName + " " + tenant.LastName + " Successfully registered!";
                

            }

            return View();
        }


        public ActionResult Login()
        {
            

            return View();
        }

        [HttpPost]
        public ActionResult Login(Tenant tenantObj)
        {

            

            if (tenantObj != null)
            {
                using (PropertyRentalManagementEntities1 db = new PropertyRentalManagementEntities1())
                {
                    //var list = db.admin.ToList();
                    var tenant = db.Tenants.FirstOrDefault(u => u.Username == tenantObj.Username && u.Password == tenantObj.Password);

                    if (tenant != null)
                    {
                        Session["TenantId"] = tenant.TenantId.ToString();
                        Session["TenantUsername"] = tenant.Username.ToString();
                        return RedirectToAction("DashboardForTenant", "Properties");
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


            if (Session["TenantId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }




        // GET: Tenants
        public ActionResult Index()
        {
            if(Session["AdminId"] == null)
            {
                return RedirectToAction("Login");
            }

            return View(db.Tenants.ToList());
        }

        // GET: Tenants/Details/5
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
            Tenant tenant = db.Tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            return View(tenant);
        }

        // GET: Tenants/Create
        public ActionResult Create()
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        // POST: Tenants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TenantId,FirstName,LastName,Email,Username,Password")] Tenant tenant)
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                db.Tenants.Add(tenant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tenant);
        }

        // GET: Tenants/Edit/5
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
            Tenant tenant = db.Tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            return View(tenant);
        }

        // POST: Tenants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TenantId,FirstName,LastName,Email,Username,Password")] Tenant tenant)
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                db.Entry(tenant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tenant);
        }

        // GET: Tenants/Delete/5
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
            Tenant tenant = db.Tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            return View(tenant);
        }

        // POST: Tenants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login");
            }

            Tenant tenant = db.Tenants.Find(id);
            db.Tenants.Remove(tenant);
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
