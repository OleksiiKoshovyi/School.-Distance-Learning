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
    public class GroupTypesController : Controller
    {
        private readonly SchoolDLContext _context;

        public GroupTypesController(SchoolDLContext context)
        {
            _context = context;
        }

        // GET: GroupTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.GroupTypes.ToListAsync());
        }

        // GET: GroupTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupTypes = await _context.GroupTypes
                .FirstOrDefaultAsync(m => m.GroupTypeId == id);
            if (groupTypes == null)
            {
                return NotFound();
            }

            return View(groupTypes);
        }

        // GET: GroupTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GroupTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupTypeId,GroupTypeName")] GroupTypes groupTypes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(groupTypes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(groupTypes);
        }

        // GET: GroupTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupTypes = await _context.GroupTypes.FindAsync(id);
            if (groupTypes == null)
            {
                return NotFound();
            }
            return View(groupTypes);
        }

        // POST: GroupTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GroupTypeId,GroupTypeName")] GroupTypes groupTypes)
        {
            if (id != groupTypes.GroupTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groupTypes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupTypesExists(groupTypes.GroupTypeId))
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
            return View(groupTypes);
        }

        // GET: GroupTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupTypes = await _context.GroupTypes
                .FirstOrDefaultAsync(m => m.GroupTypeId == id);
            if (groupTypes == null)
            {
                return NotFound();
            }

            return View(groupTypes);
        }

        // POST: GroupTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groupTypes = await _context.GroupTypes.FindAsync(id);
            _context.GroupTypes.Remove(groupTypes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupTypesExists(int id)
        {
            return _context.GroupTypes.Any(e => e.GroupTypeId == id);
        }
    }
}
