using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using THanh1.Data;
using THanh1.Models;

namespace THanh1.Controllers
{
    public class LearnerController : Controller
    {
        
            private SchoolContext db;
            public LearnerController(SchoolContext context)
            {
                db = context;
            }
            public IActionResult Index()
        {
            var learners = db.Learners.Include(m => m.Major).ToList();
            return View(learners);
        }
        // GET: Learner/Create
        public IActionResult Create()
        {
            // Dùng cách 2 để tạo SelectList gửi về View qua ViewBag để hiển thị danh sách chuyên ngành (Majors)
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName");
            return View();
        }

        // POST: Learner/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FirstMidName,LastName,MajorID,EnrollmentDate")] Learner learner)
        {
            if (ModelState.IsValid)
            {
                db.Learners.Add(learner);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            // Nếu dữ liệu không hợp lệ, tải lại SelectList để hiển thị lại dropdown chuyên ngành
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }
        // GET: Learner/Edit/5
        public IActionResult Edit(int id)
        {
            if (id == null || db.Learners == null)
            {
                return NotFound();
            }

            var learner = db.Learners.Find(id);
            if (learner == null)
            {
                return NotFound();
            }
            ViewBag.MajorId = new SelectList(db.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }

        // POST: Learner/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("LearnerID,FirstMidName,LastName,MajorID,EnrollmentDate")] Learner learner)
        {
            if (id != learner.LearnerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(learner);
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearnerExists(learner.LearnerID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MajorId = new SelectList(db.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }

        private bool LearnerExists(int id)
        {
            return (db.Learners?.Any(e => e.LearnerID == id)).GetValueOrDefault();
        }

        // GET: Learner/Delete/5
        public IActionResult Delete(int id)
        {
            if (id == null || db.Learners == null)
            {
                return NotFound();
            }

            var learner = db.Learners.Include(l => l.Major)
                                     .Include(e => e.Enrollments)
                                     .FirstOrDefault(m => m.LearnerID == id);
            if (learner == null)
            {
                return NotFound();
            }
            if (learner.Enrollments.Count() > 0)
            {
                return Content("This learner has some enrollments, can't delete!");
            }
            return View(learner);
        }

        // POST: Learner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (db.Learners == null)
            {
                return Problem("Entity set 'Learners' is null.");
            }
            var learner = db.Learners.Find(id);
            if (learner != null)
            {
                db.Learners.Remove(learner);
            }

            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
