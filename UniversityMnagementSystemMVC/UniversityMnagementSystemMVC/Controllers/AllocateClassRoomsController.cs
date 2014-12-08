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
    public class AllocateClassRoomsController : Controller
    {
        private UniversityMvcDBEntities db = new UniversityMvcDBEntities();

        [HttpPost]
        public ActionResult DeleteAll()
        {
            var allocateClassRoom = db.AllocateClassRooms.ToList();
            foreach (var allocate in allocateClassRoom)
            {
                db.AllocateClassRooms.Remove(allocate);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public PartialViewResult _AllocateClassRoomPartial()
        {
            return PartialView();
        }

        [HttpPost]
        public PartialViewResult _AllocateClassRoomPartial(int id)
        {
            var allocateClassRoomList = db.AllocateClassRooms.Where(x => x.DeptId == id).ToList();
            return PartialView(allocateClassRoomList);
        }

        // GET: AllocateClassRooms
        public ActionResult Index()
        {
            ViewBag.DeptId = new SelectList(db.Departments, "DeptId", "Code");
            var allocateClassRooms = db.AllocateClassRooms.Include(a => a.Course).Include(a => a.Department);
            return View(allocateClassRooms.ToList());
        }

        // GET: AllocateClassRooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AllocateClassRoom allocateClassRoom = db.AllocateClassRooms.Find(id);
            if (allocateClassRoom == null)
            {
                return HttpNotFound();
            }
            return View(allocateClassRoom);
        }

        // GET: AllocateClassRooms/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Name");
            ViewBag.DeptId = new SelectList(db.Departments, "DeptId", "Code");
            return View();
        }

        // POST: AllocateClassRooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AllocateClassRoomId,DeptId,CourseId,FromTime,ToTime,RoomNo")] AllocateClassRoom allocateClassRoom)
        {
            if (ModelState.IsValid)
            {
                var allocate = db.AllocateClassRooms.Where(x=>x.RoomNo==allocateClassRoom.RoomNo).ToList();
                bool IsTimeAvailable = true;

                foreach (var source in allocate)
                {
                        if (!(((allocateClassRoom.FromTime > source.FromTime) & (allocateClassRoom.ToTime > source.ToTime)) | ((allocateClassRoom.FromTime < source.FromTime) & (allocateClassRoom.ToTime < source.ToTime))))
                        {
                            IsTimeAvailable = false;
                            ViewBag.noTime = "This time is not available";
                            break;
                        }
                }

                if (IsTimeAvailable)
                {
                    db.AllocateClassRooms.Add(allocateClassRoom);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Name", allocateClassRoom.CourseId);
            ViewBag.DeptId = new SelectList(db.Departments, "DeptId", "Code", allocateClassRoom.DeptId);
            return View(allocateClassRoom);
        }

        // GET: AllocateClassRooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AllocateClassRoom allocateClassRoom = db.AllocateClassRooms.Find(id);
            if (allocateClassRoom == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Name", allocateClassRoom.CourseId);
            ViewBag.DeptId = new SelectList(db.Departments, "DeptId", "Code", allocateClassRoom.DeptId);
            return View(allocateClassRoom);
        }

        // POST: AllocateClassRooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AllocateClassRoomId,DeptId,CourseId,FromTime,ToTime,RoomNo")] AllocateClassRoom allocateClassRoom)
        {
            if (ModelState.IsValid)
            {
                db.Entry(allocateClassRoom).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Name", allocateClassRoom.CourseId);
            ViewBag.DeptId = new SelectList(db.Departments, "DeptId", "Code", allocateClassRoom.DeptId);
            return View(allocateClassRoom);
        }

        // GET: AllocateClassRooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AllocateClassRoom allocateClassRoom = db.AllocateClassRooms.Find(id);
            if (allocateClassRoom == null)
            {
                return HttpNotFound();
            }
            return View(allocateClassRoom);
        }

        // POST: AllocateClassRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            
            AllocateClassRoom allocateClassRoom = db.AllocateClassRooms.Find(id);
            db.AllocateClassRooms.Remove(allocateClassRoom);
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
