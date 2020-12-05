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
    public class GroupsController : Controller
    {
        private readonly SchoolDLContext _context;

        public GroupsController(SchoolDLContext context)
        {
            _context = context;
        }

        // GET: Groups
        public async Task<IActionResult> Index()
        {
            var schoolDLContext = _context.Groups
                .Include(g => g.Grade)
                .Include(g => g.GroupType)
                .Where(g => g.GroupType != null);
            return View(await schoolDLContext.ToListAsync());

            #region SQL
                /*"SELECT g.GroupTypeId, g.GradeId FROM Groups g "
                + "LEFT JOIN GroupTypes gt ON gt.GroupTypeId = g.GroupTypeId "
                + "LEFT JOIN Grades gr ON g.GradeId = gr.GradeId "
                + "WHERE g.GroupTypeId IS NOT NULL"*/
            #endregion
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groups = await _context.Groups
                .Include(g => g.Grade)
                .Include(g => g.GroupType)
                .FirstOrDefaultAsync(m => m.GroupId == id);
            if (groups == null)
            {
                return NotFound();
            }

            return View(groups);
        }

        // GET: Groups/Create
        public IActionResult Create()
        {
            ViewData["GradeId"] = new SelectList(_context.GradesInfo, "GradeId", "GradeName");
            ViewData["GroupTypeId"] = new SelectList(_context.GroupTypes, "GroupTypeId", "GroupTypeName");
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupId,GroupTypeId,GradeId")] Groups groups)
        {
            if (ModelState.IsValid)
            {
                _context.Add(groups);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GradeId"] = new SelectList(_context.GradesInfo, "GradeId", "GradeName", groups.GradeId);
            ViewData["GroupTypeId"] = new SelectList(_context.GroupTypes, "GroupTypeId", "GroupTypeName", groups.GroupTypeId);
            return View(groups);
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groups = await _context.Groups.FindAsync(id);
            if (groups == null)
            {
                return NotFound();
            }
            ViewData["GradeId"] = new SelectList(_context.GradesInfo, "GradeId", "GradeName", groups.GradeId);
            ViewData["GroupTypeId"] = new SelectList(_context.GroupTypes, "GroupTypeId", "GroupTypeName", groups.GroupTypeId);
            return View(groups);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GroupId,GroupTypeId,GradeId")] Groups groups)
        {
            if (id != groups.GroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groups);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupsExists(groups.GroupId))
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
            ViewData["GradeId"] = new SelectList(_context.Grades, "GradeId", "GradeName", groups.GradeId);
            ViewData["GroupTypeId"] = new SelectList(_context.GroupTypes, "GroupTypeId", "GroupTypeName", groups.GroupTypeId);
            return View(groups);
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groups = await _context.Groups
                .Include(g => g.Grade)
                .Include(g => g.GroupType)
                .FirstOrDefaultAsync(m => m.GroupId == id);
            if (groups == null)
            {
                return NotFound();
            }

            return View(groups);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groups = await _context.Groups.FindAsync(id);
            _context.Groups.Remove(groups);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupsExists(int id)
        {
            return _context.Groups.Any(e => e.GroupId == id);
        }
    }
}
