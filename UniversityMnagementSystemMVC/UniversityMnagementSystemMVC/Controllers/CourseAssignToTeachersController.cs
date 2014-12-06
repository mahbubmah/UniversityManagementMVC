using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UniversityMnagementSystemMVC.Models;

namespace UniversityMnagementSystemMVC.Controllers
{
    public class CourseAssignToTeachersController : Controller
    {
        private UniversityMvcDBEntities db = new UniversityMvcDBEntities();

        public PartialViewResult TeacherView()
        {
            var teacher = db.Teachers.First();
            return PartialView("_TeacherViewPartial");
        }

        [HttpPost]
        public PartialViewResult TeacherView(int id)
        {
            int creditRemain = db.Teachers.Where(x => x.TeacherId == id).SingleOrDefault().CreditTobeTaken;
            var courseTakenByTeacher = db.CourseAssignToTeachers.Where(x => x.TeacherId == id).ToList();

            creditRemain = courseTakenByTeacher.Aggregate(creditRemain, (current, x) => current - x.Course.Credit);

            ViewBag.creditRemain = creditRemain;
            var teacher = db.Teachers.SingleOrDefault(x => x.TeacherId == id);
            return PartialView("_TeacherViewPartial",teacher);
        }
        public PartialViewResult CourseView()
        {
            var course = db.Courses.First();
            return PartialView("_CourseViewPartial");
        }

        [HttpPost]
        public PartialViewResult CourseView(int id)
        {
            var course = db.Courses.SingleOrDefault(x => x.CourseId == id);

            return PartialView("_CourseViewPartial",course);
        }

        // GET: CourseAssignToTeachers
        public ActionResult Index()
        {
            var courseAssignToTeachers = db.CourseAssignToTeachers.Include(c => c.Course).Include(c => c.Department).Include(c => c.Teacher);
            return View(courseAssignToTeachers.ToList());
        }

        // GET: CourseAssignToTeachers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseAssignToTeacher courseAssignToTeacher = db.CourseAssignToTeachers.Find(id);
            if (courseAssignToTeacher == null)
            {
                return HttpNotFound();
            }
            return View(courseAssignToTeacher);
        }

        // GET: CourseAssignToTeachers/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Name");
            ViewBag.DeptId = new SelectList(db.Departments, "DeptId", "Code");
            ViewBag.TeacherId = new SelectList(db.Teachers, "TeacherId", "Name");
            return View();
        }

        // POST: CourseAssignToTeachers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseAssignToTeacherId,CourseId,DeptId,TeacherId")] CourseAssignToTeacher courseAssignToTeacher)
        {
            if (ModelState.IsValid)
            {
                db.CourseAssignToTeachers.Add(courseAssignToTeacher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Name", courseAssignToTeacher.CourseId);
            ViewBag.DeptId = new SelectList(db.Departments, "DeptId", "Code", courseAssignToTeacher.DeptId);
            ViewBag.TeacherId = new SelectList(db.Teachers, "TeacherId", "Name", courseAssignToTeacher.TeacherId);
            return View(courseAssignToTeacher);
        }

        // GET: CourseAssignToTeachers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseAssignToTeacher courseAssignToTeacher = db.CourseAssignToTeachers.Find(id);
            if (courseAssignToTeacher == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Name", courseAssignToTeacher.CourseId);
            ViewBag.DeptId = new SelectList(db.Departments, "DeptId", "Code", courseAssignToTeacher.DeptId);
            ViewBag.TeacherId = new SelectList(db.Teachers, "TeacherId", "Name", courseAssignToTeacher.TeacherId);
            return View(courseAssignToTeacher);
        }

        // POST: CourseAssignToTeachers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseAssignToTeacherId,CourseId,DeptId,TeacherId")] CourseAssignToTeacher courseAssignToTeacher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courseAssignToTeacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Name", courseAssignToTeacher.CourseId);
            ViewBag.DeptId = new SelectList(db.Departments, "DeptId", "Code", courseAssignToTeacher.DeptId);
            ViewBag.TeacherId = new SelectList(db.Teachers, "TeacherId", "Name", courseAssignToTeacher.TeacherId);
            return View(courseAssignToTeacher);
        }

        // GET: CourseAssignToTeachers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseAssignToTeacher courseAssignToTeacher = db.CourseAssignToTeachers.Find(id);
            if (courseAssignToTeacher == null)
            {
                return HttpNotFound();
            }
            return View(courseAssignToTeacher);
        }

        // POST: CourseAssignToTeachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CourseAssignToTeacher courseAssignToTeacher = db.CourseAssignToTeachers.Find(id);
            db.CourseAssignToTeachers.Remove(courseAssignToTeacher);
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
