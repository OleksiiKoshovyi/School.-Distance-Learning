using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School._Distance_Learning.Models;
using School._Distance_Learning.ViewModels.SkippingClassesReport;

namespace School._Distance_Learning.Controllers
{
    public class SkippingClassesReportController : Controller
    {
        private readonly SchoolDLContext _context;

        public SkippingClassesReportController(SchoolDLContext context)
        {
            _context = context;
        }

        private IndexViewModel GetIndexViewModel(int? gradeid)
        {
            var tt = _context.SkippingClasses
                 .Include(t => t.Pupil)
                 .Where(t => t.Pupil.GradeId == gradeid)
                 .OrderBy(t => t.Pupil.SurName)
                 .ThenBy(t => t.Pupil.FirstName)
                 .ThenBy(t => t.Pupil.Patronymic)
                 .ToList();

            var pupils = _context.Pupils
                .Where(p => p.GradeId == gradeid)
                .ToList();

            var dates = new List<DateTime>() {
                new DateTime(2020, 12, 7),
                new DateTime(2020, 12, 8),
                new DateTime(2020, 12, 9),
                new DateTime(2020, 12, 10),
                new DateTime(2020, 12, 11),
                new DateTime(2020, 12, 12),
                new DateTime(2020, 12, 13)};

            List<List<List<SkippingClasses>>> lessons =
                new List<List<List<SkippingClasses>>>(pupils.Count());

            for (int i = 0; i < pupils.Count(); i++)
            {
                lessons.Add(new List<List<SkippingClasses>>(pupils.Count()));
                for (int j = 0; j < 7; j++)
                {
                    lessons[i].Add(new List<SkippingClasses>());
                }
            }

            foreach (var cl in tt)
            {
                if (cl.SkippingDate >= new DateTime(2020, 12, 7) && cl.SkippingDate <= new DateTime(2020, 12, 13))
                    lessons[pupils.IndexOf(cl.Pupil)][(int)cl.SkippingDate.DayOfWeek].Add(cl);
            }

            Grades grade = _context.Grades.Where(g => g.GradeId == gradeid).FirstOrDefault();

            return new IndexViewModel(pupils, dates, grade, lessons);
        }

        // GET: TimetableForPupilsController
        public ActionResult Index(int? gradeid)
        {
            if (gradeid == null)
            {
                gradeid = _context.Grades.FirstOrDefault().GradeId;
            }

            if (gradeid == null)
            {
                gradeid = _context.GradeSubject
                    .Where(gs => gs.GradeId == gradeid)
                    .FirstOrDefault()
                    .SubjectId;
            }

            ViewData["GradeId"] = new SelectList(_context.GradesInfo,
                "GradeId", "GradeName", gradeid);

            return View(GetIndexViewModel(gradeid));
        }

        [HttpPost]
        public FileResult ExportCsv(int gradeid)
        {
            IndexViewModel indexViewModel = GetIndexViewModel(gradeid);
            return File(indexViewModel);
        }

        public virtual CsvResult File(IndexViewModel timetableData)
        {
            return new CsvResult(timetableData);
        }
    }
}
