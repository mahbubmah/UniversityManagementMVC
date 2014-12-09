using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RazorPDF;
using UniversityMnagementSystemMVC.Models;

namespace UniversityMnagementSystemMVC.Controllers
{
    public class ResultsController : Controller
    {
        private UniversityMvcDBEntities db = new UniversityMvcDBEntities();

        [HttpPost]
        public PartialViewResult StudentViewCreate(int id)
        {
            var student = db.Students.SingleOrDefault(x => x.StudentId == id);
            return PartialView("_StudentViewResultPartial", student);
        }
        public ActionResult Pdf(int id)
        {
            var student = db.Students.SingleOrDefault(x => x.StudentId == id);
            var results = db.Results.Where(x => x.StuentId == id).ToList();
            Tuple<Student,IEnumerable<Result>> aTuple=new Tuple<Student, IEnumerable<Result>>(student,results);
            
            return new PdfResult(aTuple,"Pdf");
        }

        public PartialViewResult StudentView()
        {
            return PartialView("_StudentViewPartial");
        }

        [HttpPost]
        public PartialViewResult StudentView(int id)
        {
            var student = db.Students.SingleOrDefault(x => x.StudentId == id);
            return PartialView("_StudentViewPartial", student);
        }
        [HttpPost]
        public PartialViewResult _ResultPartial(int id)
        {
            var results = db.Results.Where(x=>x.StuentId==id);
            return PartialView(results.ToList());
        }


        // GET: Results
        public ActionResult Index()
        {
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "RegNo");
            var results = db.Results.Include(r => r.Course).Include(r => r.Student);
            return View(results.ToList());
        }

        // GET: Results/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        // GET: Results/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Name");
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "RegNo");
            return View();
        }

        // POST: Results/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ResultId,CourseId,StuentId,Grade")] Result result)
        {
            if (ModelState.IsValid)
            {
                db.Results.Add(result);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Name", result.CourseId);
            ViewBag.StuentId = new SelectList(db.Students, "StudentId", "Name", result.StuentId);
            return View(result);
        }

        // GET: Results/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Name", result.CourseId);
            ViewBag.StuentId = new SelectList(db.Students, "StudentId", "Name", result.StuentId);
            return View(result);
        }

        // POST: Results/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ResultId,CourseId,StuentId,Grade")] Result result)
        {
            if (ModelState.IsValid)
            {
                db.Entry(result).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Name", result.CourseId);
            ViewBag.StuentId = new SelectList(db.Students, "StudentId", "Name", result.StuentId);
            return View(result);
        }

        // GET: Results/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        // POST: Results/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Result result = db.Results.Find(id);
            db.Results.Remove(result);
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
