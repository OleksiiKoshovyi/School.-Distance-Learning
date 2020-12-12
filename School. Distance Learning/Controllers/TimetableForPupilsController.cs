using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School._Distance_Learning.Models;

namespace School._Distance_Learning.Controllers
{
    public class TimetableForPupilsController : Controller
    {
        private readonly SchoolDLContext _context;

        public TimetableForPupilsController(SchoolDLContext context)
        {
            _context = context;
        }

        // GET: TimetableForPupilsController
        public ActionResult Index(int? gradeid)
        {
            if (gradeid == null)
            {
                gradeid = _context.Grades.FirstOrDefault().GradeId;
            }

            ViewData["GradeId"] = new SelectList(_context.GradesInfo,
                "GradeId", "GradeName", gradeid);

            /*ViewData["CurrentGradeId"] = new SelectList(_context.GradesInfo, 
                "GradeId", "GradeName", gradeid);*/


            var tt =  _context.Timetables
                 .Include(t => t.TeacherSubjectGroup)
                 .Include(t => t.TeacherSubjectGroup.TeacherSubject)
                 .Include(t => t.TeacherSubjectGroup.TeacherSubject.Subject)
                 .Include(t => t.TeacherSubjectGroup.TeacherSubject.Teacher)
                 .Include(t => t.TeacherSubjectGroup.Group)
                 .Include(t => t.TeacherSubjectGroup.Group.GroupType)
                 .Include(t => t.TeacherSubjectGroup.Group.Grade)
                 .Where(t => t.TeacherSubjectGroup.Group.GradeId == gradeid)
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

            return View(lessons);
        }

        public static Byte[] PdfSharpConvert(String html)
        {
            Byte[] res = null;
            using (MemoryStream ms = new MemoryStream())
            {
                var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4);
                pdf.Save(ms);
                res = ms.ToArray();
            }
            return res;
        }
    }
}
