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
            int? pageNumber,
            string name,
            DateTime? dobstart,
            DateTime? dobend,
            string login,
            int? gradeid)
        {

            var pupils = _context.Pupils
                .Include(p => p.Grade).Select(p => p);

            #region Search

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
                searchString = searchString.ToUpper();
                pupils = pupils
                    .Where(p => p.FirstName.ToUpper().Contains(searchString)
                    || p.SurName.ToUpper().Contains(searchString)
                    || p.Patronymic.ToUpper().Contains(searchString)
                    || p.Login.ToUpper().Contains(searchString));
            }

            #endregion

            #region OrderBy

            ViewData["CurrentSort"] = sortOrder;

            ViewData["SurNameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "SurNameSortParm_desc" : "";
            ViewData["FirstNameSortParm"] = sortOrder == "FirstName" ? "FirstName_desc" : "FirstName";
            ViewData["PatronymicSortParm"] = sortOrder == "Patronymic" ? "Patronymic_desc" : "Patronymic";
            ViewData["DobSortParm"] = sortOrder == "Dob" ? "Dob_desc" : "Dob";
            ViewData["LoginSortParm"] = sortOrder == "Login" ? "Login_desc" : "Login";
            ViewData["GradeSortParm"] = sortOrder == "GradeName" ? "GradeName_desc" : "GradeName";

            pupils = sortOrder switch
            {
                "SurNameSortParm_desc" => pupils.OrderByDescending(s => s.SurName),
                "FirstName" => pupils.OrderBy(s => s.FirstName),
                "FirstName_desc" => pupils.OrderByDescending(s => s.FirstName),
                "Patronymic" => pupils.OrderBy(s => s.Patronymic),
                "Patronymic_desc" => pupils.OrderByDescending(s => s.Patronymic),
                "Login" => pupils.OrderBy(s => s.Login),
                "Login_desc" => pupils.OrderByDescending(s => s.Login),
                "Dob" => pupils.OrderBy(s => s.Dob),
                "Dob_desc" => pupils.OrderByDescending(s => s.Dob),
                "GradeName_desc" => pupils.OrderBy(s => s.Grade.FirstYear)
                    .ThenByDescending(s => s.Grade.Letter),
                "GradeName" => pupils.OrderByDescending(s => s.Grade.FirstYear)
                    .ThenBy(s => s.Grade.Letter),
                _ => pupils.OrderBy(s => s.SurName),
            };

            #endregion

            #region filter

            ViewData["GradeId"] = new SelectList(_context.GradesInfo, "GradeId", "GradeName");

            if (!string.IsNullOrEmpty(name))
            {
                ViewData["CurrentFilterName"] = name;
                name = name.ToUpper();
                pupils = pupils
                    .Where(p => p.FirstName.ToUpper().Contains(name)
                    || p.SurName.ToUpper().Contains(name)
                    || p.Patronymic.ToUpper().Contains(name));

            }
            if (!string.IsNullOrEmpty(login))
            {
                ViewData["CurrentFilterLogin"] = login;
                login = login.ToUpper();
                pupils = pupils
                    .Where(s => s.Login.ToUpper().Contains(login));

            }
            if (gradeid != null && gradeid != 0)
            {
                ViewData["CurrentFilterGradeId"] = new SelectList(_context.GradesInfo, "GradeId", "GradeName", gradeid);
                pupils = pupils
                    .Where(p => p.GradeId.Equals(gradeid));
            }
            if (dobstart != null)
            {
                ViewData["CurrentFilterDobStart"] = dobstart?.ToString("yyyy-MM-dd");
                pupils = pupils.Where(p => p.Dob.Date >= ((DateTime)dobstart).Date);
            }
            if (dobend != null)
            {
                ViewData["CurrentFilterDobEnd"] = dobend?.ToString("yyyy-MM-dd");
                pupils = pupils.Where(p => p.Dob.Date <= ((DateTime)dobend).Date);
            }

            #endregion

            int pageSize = 5;

            return View(await PaginatedList<Pupils>.CreateAsync(pupils.AsNoTracking(),
                pageNumber ?? 1, pageSize));

            #region SQL
            /*             
              SELECT FirstName, SurName, Patronymic, DOB, Login, Password, GradeName, gi.GradeId
              FROM Pupils p 
              LEFT JOIN GradesInfo gi ON p.GradeId = gi.GradeId 
              WHERE UPPER(@item) LIKE UPPER(@example)
              ORDER BY @Order_parametr
              OFFSET     ((page - 1) * pageSize) ROWS       
              FETCH NEXT (pageSize)      ROWS ONLY; 

              filter:
              WHERE UPPER(surname) LIKE UPPER(@surname)
                AND UPPER(firstname) LIKE UPPER(@firstname) 
                AND UPPER(patronymic) LIKE UPPER(@patronymic)
                AND UPPER(login) LIKE UPPER(@login)
                AND dob BETWEEN @dobstart AND @dobend
                AND gradeid = @gradeid;
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

            #region SQL
            /*

             INSERT INTO pupils(FirstName,SurName,Patronymic,Dob,GradeId,Login,Password) 
                VALUES(@FirstName,@SurName,@Patronymic,@Dob,@GradeId,@Login,@Password)

             */
            #endregion
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

            #region SQL
            /*

             UPDATE INTO pupils SET FirstName = @FirstName, SurName = @SurName, Patronymic = @Patronymic, Dob = @Dob, GradeId = @GradeId, Login = @Login, Password = @Password
                WHERE PupilId = @PupilId;

             */
            #endregion
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

            #region SQL
            /*

             DELETE pupils WHERE PupilId = @PupilId;

             */
            #endregion
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
