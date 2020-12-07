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
    public class HomeworksController : Controller
    {
        private readonly SchoolDLContext _context;

        public HomeworksController(SchoolDLContext context)
        {
            _context = context;
        }

        // GET: Homeworks
        public async Task<IActionResult> Index()
        {
            var schoolDLContext = _context.Homeworks.Include(h => h.TeacherSubjectGroup);
            return View(await schoolDLContext.ToListAsync());
        }

        // GET: Homeworks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeworks = await _context.Homeworks
                .Include(h => h.TeacherSubjectGroup)
                .FirstOrDefaultAsync(m => m.HomeworkId == id);
            if (homeworks == null)
            {
                return NotFound();
            }

            return View(homeworks);
        }

        // GET: Homeworks/Create
        public IActionResult Create()
        {
            ViewData["TeacherSubjectGroupId"] = new SelectList(_context.TeacherSubjectGroup, "TeacherSubjectGroupId", "TeacherSubjectGroupId");
            return View();
        }

        // POST: Homeworks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HomeworkId,PassDate,TeacherSubjectGroupId")] Homeworks homeworks)
        {
            if (ModelState.IsValid)
            {
                _context.Add(homeworks);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeacherSubjectGroupId"] = new SelectList(_context.TeacherSubjectGroup, "TeacherSubjectGroupId", "TeacherSubjectGroupId", homeworks.TeacherSubjectGroupId);
            return View(homeworks);
        }

        // GET: Homeworks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeworks = await _context.Homeworks.FindAsync(id);
            if (homeworks == null)
            {
                return NotFound();
            }
            ViewData["TeacherSubjectGroupId"] = new SelectList(_context.TeacherSubjectGroup, "TeacherSubjectGroupId", "TeacherSubjectGroupId", homeworks.TeacherSubjectGroupId);
            return View(homeworks);
        }

        // POST: Homeworks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HomeworkId,PassDate,TeacherSubjectGroupId")] Homeworks homeworks)
        {
            if (id != homeworks.HomeworkId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(homeworks);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomeworksExists(homeworks.HomeworkId))
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
            ViewData["TeacherSubjectGroupId"] = new SelectList(_context.TeacherSubjectGroup, "TeacherSubjectGroupId", "TeacherSubjectGroupId", homeworks.TeacherSubjectGroupId);
            return View(homeworks);
        }

        // GET: Homeworks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeworks = await _context.Homeworks
                .Include(h => h.TeacherSubjectGroup)
                .FirstOrDefaultAsync(m => m.HomeworkId == id);
            if (homeworks == null)
            {
                return NotFound();
            }

            return View(homeworks);
        }

        // POST: Homeworks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var homeworks = await _context.Homeworks.FindAsync(id);
            _context.Homeworks.Remove(homeworks);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomeworksExists(int id)
        {
            return _context.Homeworks.Any(e => e.HomeworkId == id);
        }
    }
}
