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
    public class AdminsController : Controller
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
        public ActionResult Login(Admin adminObj)
        {
            

            

            if (adminObj != null)
            {
                using (PropertyRentalManagementEntities1 db = new PropertyRentalManagementEntities1())
                {
                    
                    //var list = db.admin.ToList();
                    var admn = db.Admins.FirstOrDefault(u => u.Username == adminObj.Username && u.Password == adminObj.Password);

                    if(admn != null)
                    {
                        Session["AdminId"] = admn.AdminId.ToString();
                        Session["AdminUserName"] = admn.Username.ToString();
                        return RedirectToAction("Index", "Properties");
                        
                    }
                    else if (admn == null)
                    {
                        ModelState.AddModelError("", "Username or Password is wrong.");
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
            
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
            
        }

        // GET: Admins
        public ActionResult Index()
        {
            return View(db.Admins.ToList());
        }

        // GET: Admins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // GET: Admins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AdminId,FirstName,LastName,Email,Username,Password")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        // GET: Admins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AdminId,FirstName,LastName,Email,Username,Password")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin);
        }

        // GET: Admins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Admin admin = db.Admins.Find(id);
            db.Admins.Remove(admin);
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
