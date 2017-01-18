﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HospitalManagament.Controllers
{
    public class CareGiverController : Controller
    {
        private HospitalManagementContext db = new HospitalManagementContext();

        // GET: Patient
        public ActionResult Index()
        {
            if (HttpContext.Session["LoggedInUser"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            else
            {
                return View(HttpContext.Session["LoggedInUser"] as User);
            }
        }

        // POST: ManagePatients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                User oldUser = db.Users.FirstOrDefault(u => u.Id == user.Id);

                // temp sol
                //oldUser.FullName = user.FullName;
                //oldUser.UserName = user.UserName;
                //oldUser.Caregiver.NRIC = user.Caregiver.NRIC;
                //oldUser.Caregiver.Age = user.Caregiver.Age;
                //oldUser.Caregiver.ContactNo = user.Caregiver.ContactNo;
                //oldUser.Email = user.Email;
                //oldUser.Caregiver.Occupation = user.Caregiver.Occupation;
                //oldUser.Caregiver.Gender = user.Caregiver.Gender;
                //oldUser.Caregiver.Address = user.Caregiver.Address;
                //oldUser.Caregiver.PatientId = user.PatientId.Value;

                oldUser.FullName = user.FullName;
                oldUser.UserName = user.UserName;
                oldUser.NRIC = user.NRIC;
                oldUser.Age = user.Age;
                oldUser.ContactNo = user.ContactNo;
                oldUser.Email = user.Email;
                oldUser.Gender = user.Gender;
                oldUser.Address = user.Address;
                oldUser.Caregiver.Patient = db.Patients.ToList().Where(u => u.Id == user.Patient.Id).FirstOrDefault();

                db.SaveChanges();

                HttpContext.Session["LoggedInUser"] = oldUser;

                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: ManageCareGivers/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            Caregiver careGiver = user.Caregiver;
            if (careGiver == null)
            {
                return HttpNotFound();
            }
            ViewBag.PatientId = new SelectList(db.Patients.Include(a => a.User).Select(a => new { a.User.FullName, a.Id }), "Id", "FullName", careGiver.Patient.Id);
            return View(db.Users.FirstOrDefault(a => a.Id == id));
        }
    }
}