﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School._Distance_Learning.Models;

namespace School._Distance_Learning.Controllers
{
    public class TeacherSubjectGroupsController : Controller
    {
        private readonly SchoolDLContext _context;

        public TeacherSubjectGroupsController(SchoolDLContext context)
        {
            _context = context;
        }

        // GET: TeacherSubjectGroups
        public async Task<IActionResult> Index()
        {
            var schoolDLContext = _context.TeacherSubjectGroup.Include(t => t.Group).Include(t => t.TeacherSubject);
            return View(await schoolDLContext.ToListAsync());
        }

        // GET: TeacherSubjectGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherSubjectGroup = await _context.TeacherSubjectGroup
                .Include(t => t.Group)
                .Include(t => t.TeacherSubject)
                .FirstOrDefaultAsync(m => m.TeacherSubjectGroupId == id);
            if (teacherSubjectGroup == null)
            {
                return NotFound();
            }

            return View(teacherSubjectGroup);
        }

        // GET: TeacherSubjectGroups/Create
        public IActionResult Create()
        {
            ViewData["GroupId"] = new SelectList(_context.Groups, "GroupId", "GroupId");
            ViewData["TeacherSubjectId"] = new SelectList(_context.TeacherSubject, "TeacherSubjectId", "TeacherSubjectId");
            return View();
        }

        // POST: TeacherSubjectGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeacherSubjectGroupId,TeacherSubjectId,GroupId")] TeacherSubjectGroup teacherSubjectGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacherSubjectGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupId"] = new SelectList(_context.Groups, "GroupId", "GroupId", teacherSubjectGroup.GroupId);
            ViewData["TeacherSubjectId"] = new SelectList(_context.TeacherSubject, "TeacherSubjectId", "TeacherSubjectId", teacherSubjectGroup.TeacherSubjectId);
            return View(teacherSubjectGroup);
        }

        // GET: TeacherSubjectGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherSubjectGroup = await _context.TeacherSubjectGroup.FindAsync(id);
            if (teacherSubjectGroup == null)
            {
                return NotFound();
            }
            ViewData["GroupId"] = new SelectList(_context.Groups, "GroupId", "GroupId", teacherSubjectGroup.GroupId);
            ViewData["TeacherSubjectId"] = new SelectList(_context.TeacherSubject, "TeacherSubjectId", "TeacherSubjectId", teacherSubjectGroup.TeacherSubjectId);
            return View(teacherSubjectGroup);
        }

        // POST: TeacherSubjectGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeacherSubjectGroupId,TeacherSubjectId,GroupId")] TeacherSubjectGroup teacherSubjectGroup)
        {
            if (id != teacherSubjectGroup.TeacherSubjectGroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacherSubjectGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherSubjectGroupExists(teacherSubjectGroup.TeacherSubjectGroupId))
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
            ViewData["GroupId"] = new SelectList(_context.Groups, "GroupId", "GroupId", teacherSubjectGroup.GroupId);
            ViewData["TeacherSubjectId"] = new SelectList(_context.TeacherSubject, "TeacherSubjectId", "TeacherSubjectId", teacherSubjectGroup.TeacherSubjectId);
            return View(teacherSubjectGroup);
        }

        // GET: TeacherSubjectGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherSubjectGroup = await _context.TeacherSubjectGroup
                .Include(t => t.Group)
                .Include(t => t.TeacherSubject)
                .FirstOrDefaultAsync(m => m.TeacherSubjectGroupId == id);
            if (teacherSubjectGroup == null)
            {
                return NotFound();
            }

            return View(teacherSubjectGroup);
        }

        // POST: TeacherSubjectGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacherSubjectGroup = await _context.TeacherSubjectGroup.FindAsync(id);
            _context.TeacherSubjectGroup.Remove(teacherSubjectGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherSubjectGroupExists(int id)
        {
            return _context.TeacherSubjectGroup.Any(e => e.TeacherSubjectGroupId == id);
        }
    }
}
