using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School._Distance_Learning.Models;
using School._Distance_Learning.ViewModels;

namespace School._Distance_Learning.Controllers
{
    public class TeachersController : Controller
    {
        private readonly SchoolDLContext _context;

        public TeachersController(SchoolDLContext context)
        {
            _context = context;
        }

        // GET: Teachers
        public async Task<IActionResult> Index(
             string sortOrder,
             string currentFilter,
             string searchString,
             int? pageNumber,
            string name,
            DateTime? dobstart, DateTime? dobend,
            DateTime? recdstart,  DateTime? recdend,
            string login)
        {

            var teachers = from t in _context.Teachers
                           select t;

            #region searching
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }


            ViewData["CurrentFilter"] = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToUpper();
                teachers = teachers.Where(
                    s => s.SurName.ToUpper().Contains(searchString)
                    || s.FirstName.ToUpper().Contains(searchString)
                    || s.Patronymic.ToUpper().Contains(searchString)
                    || s.Login.ToUpper().Contains(searchString));
            }
            #endregion  

            #region OrderBy
            if (string.IsNullOrEmpty(sortOrder))
            {
                sortOrder = "SurName";
            }

            ViewData["SurNameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "SurName_desc" : "";
            ViewData["FirstNameSortParm"] = sortOrder == "FirstName" ? "FirstName_desc" : "FirstName";
            ViewData["PatronymicSortParm"] = sortOrder == "Patronymic" ? "Patronymic_desc" : "Patronymic";
            ViewData["DobSortParm"] = sortOrder == "Dob" ? "Dob_desc" : "Dob";
            ViewData["RecruitmentDateSortParm"] = sortOrder == "RecruitmentDate" ? "RecruitmentDate_desc" : "RecruitmentDate";
            ViewData["LoginSortParm"] = sortOrder == "Login" ? "Login_desc" : "Login";

            bool descending = false;
            if (sortOrder.EndsWith("_desc"))
            {
                sortOrder = sortOrder[0..^5];
                descending = true;
            }

            if (descending)
            {
                teachers = teachers.OrderByDescending(e => EF.Property<object>(e, sortOrder));
            }
            else
            {
                teachers = teachers.OrderBy(e => EF.Property<object>(e, sortOrder));
            }
            #endregion

            #region filtering

            if (!string.IsNullOrEmpty(name))
            {
                ViewData["CurrentFilterName"] = name;
                name = name.ToUpper();
                teachers = teachers
                    .Where(p => p.FirstName.ToUpper().Contains(name)
                    || p.SurName.ToUpper().Contains(name)
                    || p.Patronymic.ToUpper().Contains(name));

            }
            if (!string.IsNullOrEmpty(login))
            {
                ViewData["CurrentFilterLogin"] = login;
                login = login.ToUpper();
                teachers = teachers
                    .Where(s => s.Login.ToUpper().Contains(login));

            }
            if (dobstart != null)
            {
                ViewData["CurrentFilterDobStart"] = dobstart?.ToString("yyyy-MM-dd");
                teachers = teachers.Where(p => p.Dob.Date >= ((DateTime)dobstart).Date);
            }
            if (dobend != null)
            {
                ViewData["CurrentFilterDobEnd"] = dobend?.ToString("yyyy-MM-dd");
                teachers = teachers.Where(p => p.Dob.Date <= ((DateTime)dobend).Date);
            }
            if (recdstart != null)
            {
                ViewData["CurrentFilterRecDStart"] = recdstart?.ToString("yyyy-MM-dd");
                teachers = teachers.Where(p => p.RecruitmentDate.Date >= ((DateTime)recdstart).Date);
            }
            if (recdend != null)
            {
                ViewData["CurrentFilterRecDEnd"] = recdend?.ToString("yyyy-MM-dd");
                teachers = teachers.Where(p => p.RecruitmentDate.Date <= ((DateTime)recdend).Date);
            }

            #endregion

            int pageSize = 5;
            return View(await PaginatedList<Teachers>.CreateAsync(teachers.AsNoTracking(),
                pageNumber ?? 1, pageSize));

            #region SQL
            /*             
              SELECT TeacherId, FirstName, SurName, Patronymic, Dob, RecruitmentDate, Login
              FROM Teachers t
              WHERE RowNumber BETWEEN {(page - 1) * pageSize} AND {page  * pageSize} 
              AND UPPER(@item) LIKE @example
              ORDER BY @Order_parametr;

              filter:
              WHERE UPPER(surname) LIKE UPPER(@surname)
                AND UPPER(firstname) LIKE UPPER(@firstname) 
                AND UPPER(patronymic) LIKE UPPER(@patronymic)
                AND UPPER(login) LIKE UPPER(@login)
                AND dob BETWEEN @dobstart AND @dobend
                AND RecruitmentDate BETWEEN @recdstart AND @recdend;
            */
            #endregion
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teachers = await _context.Teachers
                .Include(t => t.TeacherSubject)
                .ThenInclude(ts => ts.Subject)
                .FirstOrDefaultAsync(m => m.TeacherId == id);

            if (teachers == null)
            {
                return NotFound();
            }

            return View(teachers);
        }

        // GET: Teachers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeacherId,FirstName,SurName,Patronymic,Dob,RecruitmentDate,Login,Password")] Teachers teachers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teachers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teachers);

            #region SQL
            /*

             INSERT INTO teachers(FirstName,SurName,Patronymic,Dob,RecruitmentDate,Login,Password) 
                VALUES(@FirstName,@SurName,@Patronymic,@Dob,@RecD,@Login,@Password)

             */
            #endregion
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teachers = await _context.Teachers.FindAsync(id);
            if (teachers == null)
            {
                return NotFound();
            }
            return View(teachers);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeacherId,FirstName,SurName,Patronymic,Dob,RecruitmentDate,Login,Password")] Teachers teachers)
        {
            if (id != teachers.TeacherId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teachers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeachersExists(teachers.TeacherId))
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
            return View(teachers);

            #region SQL
            /*

             UPDATE INTO teachers SET FirstName = @FirstName, SurName = @SurName, Patronymic = @Patronymic, Dob = @Dob, RecruitmentDate = @RecD, Login = @Login, Password = @Password
                WHERE TeacherId = @TeacherId;

             */
            #endregion
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teachers = await _context.Teachers
                .FirstOrDefaultAsync(m => m.TeacherId == id);
            if (teachers == null)
            {
                return NotFound();
            }

            return View(teachers);

            #region SQL
            /*

             DELETE teachers WHERE TeacherId = @TeacherId;

             */
            #endregion
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teachers = await _context.Teachers.FindAsync(id);
            _context.Teachers.Remove(teachers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeachersExists(int id)
        {
            return _context.Teachers.Any(e => e.TeacherId == id);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult IsLoginUnique(Teachers teacher)
        {
            if (_context.Teachers.Any(t => t.Login == teacher.Login && t.TeacherId != teacher.TeacherId))
            {
                return Json(false);
            }
            return Json(true);
        }
    }
}
