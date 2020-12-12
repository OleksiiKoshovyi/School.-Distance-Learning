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
    public class TimetablesController : Controller
    {
        private readonly SchoolDLContext _context;

        public TimetablesController(SchoolDLContext context)
        {
            _context = context;
        }

        // GET: Timetables
        public async Task<IActionResult> Index()
        {
            var schoolDLContext = _context.Timetables.Include(t => t.TeacherSubjectGroup);
            return View(await schoolDLContext.ToListAsync());
        }

        // GET: Timetables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timetables = await _context.Timetables
                .Include(t => t.TeacherSubjectGroup)
                .FirstOrDefaultAsync(m => m.TimetableId == id);
            if (timetables == null)
            {
                return NotFound();
            }

            return View(timetables);
        }

        // GET: Timetables/Create
        public IActionResult Create()
        {
            ViewData["TeacherSubjectGroupId"] = new SelectList(_context.TeacherSubjectGroup
                .Include(t => t.TeacherSubject)
                .Include(t => t.TeacherSubject.Subject)
                .Include(t => t.TeacherSubject.Teacher)
                .Include(t => t.Group)
                .Include(t => t.Group.GroupType)
                .Include(t => t.Group.Grade),
                "TeacherSubjectGroupId",
                "TeacherSubjectGroupName");

            return View();
        }

        // POST: Timetables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TimetableId,WeekDayNumber,LessonNumber,Oddness,TeacherSubjectGroupId")] Timetables timetables)
        {
            if (ModelState.IsValid)
            {
                _context.Add(timetables);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["TeacherSubjectGroupId"] = new SelectList(_context.TeacherSubjectGroup
                .Include(t => t.TeacherSubject)
                .Include(t => t.TeacherSubject.Subject)
                .Include(t => t.TeacherSubject.Teacher)
                .Include(t => t.Group)
                .Include(t => t.Group.GroupType)
                .Include(t => t.Group.Grade),
                "TeacherSubjectGroupId",
                "TeacherSubjectGroupName",
                timetables.TeacherSubjectGroupId);

            return View(timetables);
        }

        // GET: Timetables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timetables = await _context.Timetables.FindAsync(id);
            if (timetables == null)
            {
                return NotFound();
            }
            ViewData["TeacherSubjectGroupId"] = new SelectList(_context.TeacherSubjectGroup, "TeacherSubjectGroupId", "TeacherSubjectGroupId", timetables.TeacherSubjectGroupId);
            return View(timetables);
        }

        // POST: Timetables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TimetableId,WeekDayNumber,LessonNumber,Oddness,TeacherSubjectGroupId")] Timetables timetables)
        {
            if (id != timetables.TimetableId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timetables);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimetablesExists(timetables.TimetableId))
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
            ViewData["TeacherSubjectGroupId"] = new SelectList(_context.TeacherSubjectGroup, "TeacherSubjectGroupId", "TeacherSubjectGroupId", timetables.TeacherSubjectGroupId);
            return View(timetables);
        }

        // GET: Timetables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timetables = await _context.Timetables
                .Include(t => t.TeacherSubjectGroup)
                .FirstOrDefaultAsync(m => m.TimetableId == id);
            if (timetables == null)
            {
                return NotFound();
            }

            return View(timetables);
        }

        // POST: Timetables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var timetables = await _context.Timetables.FindAsync(id);
            _context.Timetables.Remove(timetables);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TimetablesExists(int id)
        {
            return _context.Timetables.Any(e => e.TimetableId == id);
        }
    }
}
