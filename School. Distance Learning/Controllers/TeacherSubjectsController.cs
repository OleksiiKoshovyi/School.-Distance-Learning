using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School._Distance_Learning.Models;

namespace School._Distance_Learning.Controllers
{
    public class TeacherSubjectsController : Controller
    {
        private readonly SchoolDLContext _context;

        public TeacherSubjectsController(SchoolDLContext context)
        {
            _context = context;
        }

        // GET: TeacherSubjects
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var schoolDLContext = _context.TeacherSubject.Include(t => t.Subject).Include(t => t.Teacher)
                .Skip((page - 1) * pageSize)
                .Take(page * pageSize);
            return View(await schoolDLContext.ToListAsync());
        }

        // GET: TeacherSubjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherSubject = await _context.TeacherSubject
                .Include(t => t.Subject)
                .Include(t => t.Teacher)
                .FirstOrDefaultAsync(m => m.TeacherSubjectId == id);
            if (teacherSubject == null)
            {
                return NotFound();
            }

            return View(teacherSubject);
        }

        // GET: TeacherSubjects/Create
        public IActionResult Create()
        {
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectName");
            ViewData["TeacherId"] = new SelectList(_context.TeachersInfo, "TeacherId", "TeacherFullName");
            return View();
        }

        // POST: TeacherSubjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeacherSubjectId,TeacherId,SubjectId,HoursNumber")] TeacherSubject teacherSubject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacherSubject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectName", teacherSubject.SubjectId);
            ViewData["TeacherId"] = new SelectList(_context.TeachersInfo, "TeacherId", "TeacherFullName", teacherSubject.TeacherId);
            return View(teacherSubject);
        }

        // GET: TeacherSubjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherSubject = await _context.TeacherSubject.FindAsync(id);
            if (teacherSubject == null)
            {
                return NotFound();
            }
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectName", teacherSubject.SubjectId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FirstName", teacherSubject.TeacherId);
            return View(teacherSubject);
        }

        // POST: TeacherSubjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeacherSubjectId,TeacherId,SubjectId,HoursNumber")] TeacherSubject teacherSubject)
        {
            if (id != teacherSubject.TeacherSubjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacherSubject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherSubjectExists(teacherSubject.TeacherSubjectId))
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
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectName", teacherSubject.SubjectId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FirstName", teacherSubject.TeacherId);
            return View(teacherSubject);
        }

        // GET: TeacherSubjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherSubject = await _context.TeacherSubject
                .Include(t => t.Subject)
                .Include(t => t.Teacher)
                .FirstOrDefaultAsync(m => m.TeacherSubjectId == id);
            if (teacherSubject == null)
            {
                return NotFound();
            }

            return View(teacherSubject);
        }

        // POST: TeacherSubjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacherSubject = await _context.TeacherSubject.FindAsync(id);
            _context.TeacherSubject.Remove(teacherSubject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherSubjectExists(int id)
        {
            return _context.TeacherSubject.Any(e => e.TeacherSubjectId == id);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult IsTeacherSubjectsUnique(TeacherSubject tsub)
        {
            if (_context.TeacherSubject.Any(ts => ts.TeacherId == tsub.TeacherId 
                && ts.SubjectId == tsub.SubjectId 
                && ts.TeacherSubjectId != tsub.TeacherSubjectId))
            {
                return Json(false);
            }
            return Json(true);
        }
    }
}
