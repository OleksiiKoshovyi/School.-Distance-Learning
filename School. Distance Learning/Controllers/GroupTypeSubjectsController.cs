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
    public class GroupTypeSubjectsController : Controller
    {
        private readonly SchoolDLContext _context;

        public GroupTypeSubjectsController(SchoolDLContext context)
        {
            _context = context;
        }

        // GET: GroupTypeSubjects
        public async Task<IActionResult> Index()
        {
            var schoolDLContext = _context.GroupTypeSubject.Include(g => g.GroupType).Include(g => g.Subject);
            return View(await schoolDLContext.ToListAsync());
        }

        // GET: GroupTypeSubjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupTypeSubject = await _context.GroupTypeSubject
                .Include(g => g.GroupType)
                .Include(g => g.Subject)
                .FirstOrDefaultAsync(m => m.GroupTypeSubjectId == id);
            if (groupTypeSubject == null)
            {
                return NotFound();
            }

            return View(groupTypeSubject);
        }

        // GET: GroupTypeSubjects/Create
        public IActionResult Create()
        {
            ViewData["GroupTypeId"] = new SelectList(_context.GroupTypes, "GroupTypeId", "GroupTypeName");
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectName");
            return View();
        }

        // POST: GroupTypeSubjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupTypeSubjectId,GroupTypeId,SubjectId")] GroupTypeSubject groupTypeSubject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(groupTypeSubject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupTypeId"] = new SelectList(_context.GroupTypes, "GroupTypeId", "GroupTypeName", groupTypeSubject.GroupTypeId);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectName", groupTypeSubject.SubjectId);
            return View(groupTypeSubject);
        }

        // GET: GroupTypeSubjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupTypeSubject = await _context.GroupTypeSubject.FindAsync(id);
            if (groupTypeSubject == null)
            {
                return NotFound();
            }
            ViewData["GroupTypeId"] = new SelectList(_context.GroupTypes, "GroupTypeId", "GroupTypeName", groupTypeSubject.GroupTypeId);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectName", groupTypeSubject.SubjectId);
            return View(groupTypeSubject);
        }

        // POST: GroupTypeSubjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GroupTypeSubjectId,GroupTypeId,SubjectId")] GroupTypeSubject groupTypeSubject)
        {
            if (id != groupTypeSubject.GroupTypeSubjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groupTypeSubject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupTypeSubjectExists(groupTypeSubject.GroupTypeSubjectId))
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
            ViewData["GroupTypeId"] = new SelectList(_context.GroupTypes, "GroupTypeId", "GroupTypeName", groupTypeSubject.GroupTypeId);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectName", groupTypeSubject.SubjectId);
            return View(groupTypeSubject);
        }

        // GET: GroupTypeSubjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupTypeSubject = await _context.GroupTypeSubject
                .Include(g => g.GroupType)
                .Include(g => g.Subject)
                .FirstOrDefaultAsync(m => m.GroupTypeSubjectId == id);
            if (groupTypeSubject == null)
            {
                return NotFound();
            }

            return View(groupTypeSubject);
        }

        // POST: GroupTypeSubjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groupTypeSubject = await _context.GroupTypeSubject.FindAsync(id);
            _context.GroupTypeSubject.Remove(groupTypeSubject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupTypeSubjectExists(int id)
        {
            return _context.GroupTypeSubject.Any(e => e.GroupTypeSubjectId == id);
        }
        
        public IActionResult IsGroupTypeSubjectUnique(GroupTypeSubject g)
        {
            if (_context.GroupTypeSubject.Any(gts => gts.GroupTypeId == g.GroupTypeId 
                && gts.SubjectId == gts.SubjectId 
                && gts.GroupTypeSubjectId != g.GroupTypeSubjectId))
            {
                return Json(false);
            }
            return Json(true);
        }
    }
}
