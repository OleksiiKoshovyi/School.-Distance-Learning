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
    public class GradeSubjectsController : Controller
    {
        private readonly SchoolDLContext _context;

        public GradeSubjectsController(SchoolDLContext context)
        {
            _context = context;
        }

        // GET: GradeSubjects
        public async Task<IActionResult> Index()
        {
            var schoolDLContext = _context.GradeSubject
                .Include(g => g.Grade)
                .Include(g => g.Subject);
            return View(await schoolDLContext.ToListAsync());
        }

        // GET: GradeSubjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gradeSubject = await _context.GradeSubject
                .Include(g => g.Grade)
                .Include(g => g.Subject)
                .FirstOrDefaultAsync(m => m.GradeSubjectId == id);

            if (gradeSubject == null)
            {
                return NotFound();
            }

            return View(gradeSubject);
        }

        // GET: GradeSubjects/Create
        public IActionResult Create()
        {
            ViewData["GradeId"] = new SelectList(_context.GradesInfo, "GradeId", "GradeName");
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectName");
            return View();
        }

        // POST: GradeSubjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GradeSubjectId,GradeId,SubjectId,HoursNumber")] GradeSubject gradeSubject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gradeSubject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GradeId"] = new SelectList(_context.GradesInfo, "GradeId", "GradeName", gradeSubject.GradeId);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectName", gradeSubject.SubjectId);
            return View(gradeSubject);
        }

        // GET: GradeSubjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gradeSubject = await _context.GradeSubject.FindAsync(id);
            if (gradeSubject == null)
            {
                return NotFound();
            }
            ViewData["GradeId"] = new SelectList(_context.GradesInfo, "GradeId", "GradeName", gradeSubject.GradeId);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectName", gradeSubject.SubjectId);
            return View(gradeSubject);
        }

        // POST: GradeSubjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GradeSubjectId,GradeId,SubjectId,HoursNumber")] GradeSubject gradeSubject)
        {
            if (id != gradeSubject.GradeSubjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gradeSubject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GradeSubjectExists(gradeSubject.GradeSubjectId))
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
            ViewData["GradeId"] = new SelectList(_context.GradesInfo, "GradeId", "GradeName", gradeSubject.GradeId);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectName", gradeSubject.SubjectId);
            return View(gradeSubject);
        }

        // GET: GradeSubjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gradeSubject = await _context.GradeSubject
                .Include(g => g.Grade)
                .Include(g => g.Subject)
                .FirstOrDefaultAsync(m => m.GradeSubjectId == id);
            if (gradeSubject == null)
            {
                return NotFound();
            }

            return View(gradeSubject);
        }

        // POST: GradeSubjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gradeSubject = await _context.GradeSubject.FindAsync(id);
            _context.GradeSubject.Remove(gradeSubject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GradeSubjectExists(int id)
        {
            return _context.GradeSubject.Any(e => e.GradeSubjectId == id);
        }
    }
}
