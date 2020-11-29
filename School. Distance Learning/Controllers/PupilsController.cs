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
    public class PupilsController : Controller
    {
        private readonly SchoolDLContext _context;

        public PupilsController(SchoolDLContext context)
        {
            _context = context;
        }

        // GET: Pupils
        public async Task<IActionResult> Index()
        {
            var schoolDLContext = _context.Pupils.Include(p => p.Grade);
            return View(await schoolDLContext.ToListAsync());
        }

        // GET: Pupils/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pupils = await _context.Pupils
                .Include(p => p.Grade)
                .FirstOrDefaultAsync(m => m.PupilId == id);
            if (pupils == null)
            {
                return NotFound();
            }

            return View(pupils);
        }

        // GET: Pupils/Create
        public IActionResult Create()
        {
            ViewData["GradeId"] = new SelectList(_context.Grades, "GradeId", "Letter");
            return View();
        }

        // POST: Pupils/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PupilId,FirstName,SurName,Patronymic,Dob,GradeId,Login,Password")] Pupils pupils)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pupils);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GradeId"] = new SelectList(_context.Grades, "GradeId", "Letter", pupils.GradeId);
            return View(pupils);
        }

        // GET: Pupils/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pupils = await _context.Pupils.FindAsync(id);
            if (pupils == null)
            {
                return NotFound();
            }
            ViewData["GradeId"] = new SelectList(_context.Grades, "GradeId", "Letter", pupils.GradeId);
            return View(pupils);
        }

        // POST: Pupils/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PupilId,FirstName,SurName,Patronymic,Dob,GradeId,Login,Password")] Pupils pupils)
        {
            if (id != pupils.PupilId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pupils);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PupilsExists(pupils.PupilId))
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
            ViewData["GradeId"] = new SelectList(_context.Grades, "GradeId", "Letter", pupils.GradeId);
            return View(pupils);
        }

        // GET: Pupils/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pupils = await _context.Pupils
                .Include(p => p.Grade)
                .FirstOrDefaultAsync(m => m.PupilId == id);
            if (pupils == null)
            {
                return NotFound();
            }

            return View(pupils);
        }

        // POST: Pupils/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pupils = await _context.Pupils.FindAsync(id);
            _context.Pupils.Remove(pupils);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PupilsExists(int id)
        {
            return _context.Pupils.Any(e => e.PupilId == id);
        }
    }
}
