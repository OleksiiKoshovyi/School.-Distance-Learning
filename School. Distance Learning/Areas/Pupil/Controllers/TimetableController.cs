using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School._Distance_Learning.Areas.Pupil.ViewModels.Timetable;
using School._Distance_Learning.Models;

namespace School._Distance_Learning.Areas.Pupil.Controllers
{
    [Area("Pupil")]
    public class TimetableController : Controller
    {
        private readonly SchoolDLContext _context;

        public TimetableController(SchoolDLContext context)
        {
            _context = context;
        }

        private IndexViewModel GetIndexViewModel(int pupilid)
        {
            Pupils pupil = _context.Pupils
                .Where(p => p.PupilId == pupilid)
                .FirstOrDefault();

            Grades grade = _context.Pupils
                .Include(p => p.Grade)
                .Where(p => p.PupilId == pupilid)
                .Select(g => g.Grade)
                .FirstOrDefault();

            List<Groups> groups = _context.GroupPupil
                .Include(gp => gp.Group)
                .Include(gp => gp.Pupil)
                .Where(gp => gp.Pupil.PupilId == pupilid)
                .Select(gp => gp.Group)
                .ToList();

            var tt = _context.Timetables
                 .Include(t => t.TeacherSubjectGroup)
                 .Include(t => t.TeacherSubjectGroup.TeacherSubject)
                 .Include(t => t.TeacherSubjectGroup.TeacherSubject.Subject)
                 .Include(t => t.TeacherSubjectGroup.TeacherSubject.Teacher)
                 .Include(t => t.TeacherSubjectGroup.Group)
                 .Include(t => t.TeacherSubjectGroup.Group.GroupType)
                 .Include(t => t.TeacherSubjectGroup.Group.Grade)
                 .Where(t => t.TeacherSubjectGroup.Group.GradeId == grade.GradeId)
                 .Where(t => t.TeacherSubjectGroup.Group.GroupTypeId == null || groups.Contains(t.TeacherSubjectGroup.Group))
                 .OrderBy(t => t.TeacherSubjectGroup.Group.GradeId)
                 .ThenBy(t => t.WeekDayNumber)
                 .ThenBy(t => t.LessonNumber)
                 .ThenBy(t => t.TeacherSubjectGroup.GroupId)
                 .ToList();

            int maxLessonNumber = tt.Select(t => t.LessonNumber).DefaultIfEmpty().Max();

            List<List<List<Timetables>>> lessons =
                new List<List<List<Timetables>>>(maxLessonNumber);

            for (int i = 0; i < maxLessonNumber; i++)
            {
                lessons.Add(new List<List<Timetables>>(5));
                for (int j = 0; j < 5; j++)
                {
                    lessons[i].Add(new List<Timetables>());
                }
            }

            foreach (var lesson in tt)
            {
                lessons[lesson.LessonNumber - 1][lesson.WeekDayNumber - 1].Add(lesson);
            }

            return new IndexViewModel(lessons, pupil, DateTime.Now);
        }

        // GET: TimetableForPupilsController
        public ActionResult Index(int? pupilid)
        {
            if (pupilid == null)
            {
                pupilid = _context.Pupils.FirstOrDefault().PupilId;
            }

            ViewData["PupilId"] = new SelectList(_context.Pupils,
                "PupilId", "SurName", pupilid);

            return View(GetIndexViewModel((int)pupilid));
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
