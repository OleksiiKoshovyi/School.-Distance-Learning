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
    public class SkippingClassesController : Controller
    {
        private readonly SchoolDLContext _context;

        public SkippingClassesController(SchoolDLContext context)
        {
            _context = context;
        }

        // GET: SkippingClasses
        public async Task<IActionResult> Index()
        {
            var schoolDLContext = _context.SkippingClasses.Include(s => s.Pupil).Include(s => s.Timetable);
            return View(await schoolDLContext.ToListAsync());
        }

        // GET: SkippingClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skippingClasses = await _context.SkippingClasses
                .Include(s => s.Pupil)
                .Include(s => s.Timetable)
                .FirstOrDefaultAsync(m => m.SkippingClassId == id);
            if (skippingClasses == null)
            {
                return NotFound();
            }

            return View(skippingClasses);
        }

        // GET: SkippingClasses/Create
        public IActionResult Create()
        {
            ViewData["PupilId"] = new SelectList(_context.Pupils, "PupilId", "FirstName");
            ViewData["TimetableId"] = new SelectList(_context.Timetables, "TimetableId", "TimetableId");
            return View();
        }

        // POST: SkippingClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SkippingClassId,TimetableId,PupilId,WeekNumber")] SkippingClasses skippingClasses)
        {
            if (ModelState.IsValid)
            {
                _context.Add(skippingClasses);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PupilId"] = new SelectList(_context.Pupils, "PupilId", "FirstName", skippingClasses.PupilId);
            ViewData["TimetableId"] = new SelectList(_context.Timetables, "TimetableId", "TimetableId", skippingClasses.TimetableId);
            return View(skippingClasses);
        }

        // GET: SkippingClasses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skippingClasses = await _context.SkippingClasses.FindAsync(id);
            if (skippingClasses == null)
            {
                return NotFound();
            }
            ViewData["PupilId"] = new SelectList(_context.Pupils, "PupilId", "FirstName", skippingClasses.PupilId);
            ViewData["TimetableId"] = new SelectList(_context.Timetables, "TimetableId", "TimetableId", skippingClasses.TimetableId);
            return View(skippingClasses);
        }

        // POST: SkippingClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SkippingClassId,TimetableId,PupilId,WeekNumber")] SkippingClasses skippingClasses)
        {
            if (id != skippingClasses.SkippingClassId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(skippingClasses);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SkippingClassesExists(skippingClasses.SkippingClassId))
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
            ViewData["PupilId"] = new SelectList(_context.Pupils, "PupilId", "FirstName", skippingClasses.PupilId);
            ViewData["TimetableId"] = new SelectList(_context.Timetables, "TimetableId", "TimetableId", skippingClasses.TimetableId);
            return View(skippingClasses);
        }

        // GET: SkippingClasses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skippingClasses = await _context.SkippingClasses
                .Include(s => s.Pupil)
                .Include(s => s.Timetable)
                .FirstOrDefaultAsync(m => m.SkippingClassId == id);
            if (skippingClasses == null)
            {
                return NotFound();
            }

            return View(skippingClasses);
        }

        // POST: SkippingClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var skippingClasses = await _context.SkippingClasses.FindAsync(id);
            _context.SkippingClasses.Remove(skippingClasses);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SkippingClassesExists(int id)
        {
            return _context.SkippingClasses.Any(e => e.SkippingClassId == id);
        }
    }
}
