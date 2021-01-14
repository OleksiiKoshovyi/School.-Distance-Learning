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
    public class GroupPupilsController : Controller
    {
        private readonly SchoolDLContext _context;

        public GroupPupilsController(SchoolDLContext context)
        {
            _context = context;
        }

        // GET: GroupPupils
        public async Task<IActionResult> Index()
        {
            var schoolDLContext = _context.GroupPupil
                .Include(g => g.Group)
                .Include(g => g.Pupil)
                .Include(g => g.Group.Grade)
                .Include(g => g.Group.GroupType)
                .Where(g => g.Group.GroupTypeId != null);

            return View(await schoolDLContext.ToListAsync());
        }

        // GET: GroupPupils/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupPupil = await _context.GroupPupil
                .Include(g => g.Group)
                .Include(g => g.Pupil)
                .FirstOrDefaultAsync(m => m.GroupPupilId == id);

            if (groupPupil == null)
            {
                return NotFound();
            }

            return View(groupPupil);
        }

        // GET: GroupPupils/Create
        public IActionResult Create()
        {
            ViewData["GroupId"] = new SelectList(_context.Groups.Where(g => g.GroupTypeId != null), "GroupId", "GroupId");
            ViewData["PupilId"] = new SelectList(_context.Pupils, "PupilId", "FirstName");
            return View();
        }

        // POST: GroupPupils/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupPupilId,GroupId,PupilId")] GroupPupil groupPupil)
        {
            if (ModelState.IsValid)
            {
                _context.Add(groupPupil);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupId"] = new SelectList(_context.Groups.Where(g => g.GroupTypeId != null), "GroupId", "GroupId", groupPupil.GroupId);
            ViewData["PupilId"] = new SelectList(_context.Pupils, "PupilId", "FirstName", groupPupil.PupilId);
            return View(groupPupil);
        }

        // GET: GroupPupils/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupPupil = await _context.GroupPupil.FindAsync(id);
            if (groupPupil == null)
            {
                return NotFound();
            }
            ViewData["GroupId"] = new SelectList(_context.Groups.Where(g => g.GroupTypeId != null), "GroupId", "GroupId", groupPupil.GroupId);
            ViewData["PupilId"] = new SelectList(_context.Pupils, "PupilId", "FirstName", groupPupil.PupilId);
            return View(groupPupil);
        }

        // POST: GroupPupils/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GroupPupilId,GroupId,PupilId")] GroupPupil groupPupil)
        {
            if (id != groupPupil.GroupPupilId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groupPupil);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupPupilExists(groupPupil.GroupPupilId))
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
            ViewData["GroupId"] = new SelectList(_context.Groups.Where(g => g.GroupTypeId != null), "GroupId", "GroupId", groupPupil.GroupId);
            ViewData["PupilId"] = new SelectList(_context.Pupils, "PupilId", "FirstName", groupPupil.PupilId);
            return View(groupPupil);
        }

        // GET: GroupPupils/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupPupil = await _context.GroupPupil
                .Include(g => g.Group)
                .Include(g => g.Pupil)
                .FirstOrDefaultAsync(m => m.GroupPupilId == id);

            if (groupPupil == null)
            {
                return NotFound();
            }

            return View(groupPupil);
        }

        // POST: GroupPupils/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groupPupil = await _context.GroupPupil.FindAsync(id);
            _context.GroupPupil.Remove(groupPupil);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupPupilExists(int id)
        {
            return _context.GroupPupil.Any(e => e.GroupPupilId == id);
        }
    }
}
