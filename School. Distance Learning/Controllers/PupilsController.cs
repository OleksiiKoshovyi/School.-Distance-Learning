using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School._Distance_Learning.Models;
using School._Distance_Learning.ViewModels;
using School._Distance_Learning.ViewModels.Pupils;

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
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {

            var schoolDLContext = _context.Pupils
                .Include(p => p.Grade).Select(p => p);

            ViewData["CurrentSort"] = sortOrder;
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString??"".ToUpper();
            if (!string.IsNullOrEmpty(searchString))
            {
                schoolDLContext = schoolDLContext
                    .Where(s => s.FirstName.ToUpper().Contains(searchString)
                    || s.SurName.ToUpper().Contains(searchString));
            }

            ViewData["SurNameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "SurNameSortParm_desc" : "";
            ViewData["DobSortParm"] = sortOrder == "Dob" ? "Dob_desc" : "Dob";
            ViewData["LoginSortParm"] = sortOrder == "Login" ? "Login_desc" : "Login";
            ViewData["GradeSortParm"] = sortOrder == "GradeName" ? "GradeName_desc" : "GradeName";

            switch (sortOrder)
            {
                case "SurNameSortParm_desc":
                    schoolDLContext = schoolDLContext.OrderByDescending(s => s.SurName);
                    break;
                case "Dob":
                    schoolDLContext = schoolDLContext.OrderBy(s => s.Dob);
                    break;
                case "Dob_desc":
                    schoolDLContext = schoolDLContext.OrderByDescending(s => s.Dob);
                    break;
                case "GradeName_desc":
                    schoolDLContext = schoolDLContext.OrderBy(s => s.Grade.FirstYear)
                        .ThenByDescending(s => s.Grade.Letter);
                    break;
                case "GradeName":
                    schoolDLContext = schoolDLContext.OrderByDescending(s => s.Grade.FirstYear)
                        .ThenBy(s => s.Grade.Letter);
                    break;
                default:
                    schoolDLContext = schoolDLContext.OrderBy(s => s.SurName);
                    break;
            }

            int pageSize = 5;


            return View(await PaginatedList<Pupils>.CreateAsync(schoolDLContext.AsNoTracking(),
                pageNumber ?? 1, pageSize));

            #region SQL
            /*             
              SELECT FirstName, SurName, Patronymic, DOB, Login, Password, GradeName, g.GradeId, Letter, FirstYear
              FROM Pupils p 
              LEFT JOIN GradesInfo gi ON p.GradeId = gi.GradeId 
              WHERE RowNumber BETWEEN {(page - 1) * pageSize} AND {page  * pageSize} 
              AND UPPER(@item) LIKE @example
              ORDER BY @Order_parametr;
            */
            #endregion

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
            ViewData["GradeId"] = new SelectList(_context.GradesInfo, "GradeId", "GradeName");
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
            ViewData["GradeId"] = new SelectList(_context.GradesInfo, "GradeId", "GradeName", pupils.GradeId);
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
            ViewData["GradeId"] = new SelectList(_context.GradesInfo, "GradeId", "GradeName", pupils.GradeId);
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

        [AcceptVerbs("Get", "Post")]
        public IActionResult IsLoginUnique(Pupils pupil)
        {
            if (_context.Pupils.Any(p => p.Login == pupil.Login && p.PupilId != pupil.PupilId))
            {
                return Json(false);
            }
            return Json(true);
        }
    }
}
