using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School._Distance_Learning.Models;
using School._Distance_Learning.ViewModels.TimetableForTeachers;

namespace School._Distance_Learning.Controllers
{
    public class TimetableForTeachersController : Controller
    {
        private readonly SchoolDLContext _context;

        public TimetableForTeachersController(SchoolDLContext context)
        {
            _context = context;
        }

        private IndexViewModel GetIndexViewModel(int teacherid)
        {
            var tt = _context.Timetables
                 .Include(t => t.TeacherSubjectGroup)
                 .Include(t => t.TeacherSubjectGroup.TeacherSubject)
                 .Include(t => t.TeacherSubjectGroup.TeacherSubject.Subject)
                 .Include(t => t.TeacherSubjectGroup.TeacherSubject.Teacher)
                 .Include(t => t.TeacherSubjectGroup.Group)
                 .Include(t => t.TeacherSubjectGroup.Group.GroupType)
                 .Include(t => t.TeacherSubjectGroup.Group.Grade)
                 .Where(t => t.TeacherSubjectGroup.TeacherSubject.TeacherId == teacherid)
                 .OrderBy(t => t.TeacherSubjectGroup.Group.GradeId)
                 .ThenBy(t => t.WeekDayNumber)
                 .ThenBy(t => t.LessonNumber)
                 .ThenBy(t => t.TeacherSubjectGroup.GroupId)
                 .ToList();

            Teachers teacher = _context.Teachers
                .Where(t => t.TeacherId == teacherid)
                .ToList()
                .FirstOrDefault();

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

            return new IndexViewModel(lessons, teacher, DateTime.Now);
        }

        // GET: TimetableForPupilsController
        public ActionResult Index(int? teacherid)
        {
            if (teacherid == null)
            {
                teacherid = _context.Teachers.FirstOrDefault().TeacherId;
            }

            ViewData["TeacherId"] = new SelectList(_context.TeachersInfo,
                "TeacherId", "TeacherFullName", teacherid);

            return View(GetIndexViewModel((int)teacherid));
        }

        [HttpPost]
        public FileResult ExportCsv(int teacherid)
        {
            IndexViewModel indexViewModel = GetIndexViewModel(teacherid);
            return File(indexViewModel);
        }

        public virtual CsvResult File(IndexViewModel timetableData)
        {
            return new CsvResult(timetableData);
        }

    }
}
