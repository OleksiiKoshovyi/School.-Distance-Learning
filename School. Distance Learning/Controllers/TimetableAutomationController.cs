using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School._Distance_Learning.Models;
using School._Distance_Learning.ViewModels.TimetableAutomation;

namespace School._Distance_Learning.Controllers
{
    public class TimetableAutomationController : Controller
    {
        private readonly SchoolDLContext _context;

        public TimetableAutomationController(SchoolDLContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var grades = _context.Grades
                .OrderBy(g => g.FirstYear)
                .ThenBy(g => g.Letter)
                .ToList();

            var groupSubject = _context.TeacherSubjectGroup
                .Include(g => g.TeacherSubject)
                .Include(g => g.TeacherSubject.Subject)
                .Include(g => g.TeacherSubject.Teacher)
                .Include(g => g.Group)
                .Include(g => g.Group.Grade)
                .Where(g => g.Group.GroupType != null);

            var gradeSubject = _context.GradeSubject
                .Include(g => g.Grade)
                .Include(g => g.Subject)/*
                .Where(g => 
                    !(groupSubject.Where(p => p.Group.GradeId == g.GradeId)
                    .Select(t => t.TeacherSubject.Subject.SubjectId))
                    .Contains(g.Subject.SubjectId))*/;


            List<IndexViewModel> models = new List<IndexViewModel>();

            for (int i = 0; i < grades.Count(); i++)
            {
                var currGradeSub = gradeSubject.Where(gs => gs.GradeId == grades[i].GradeId);
                var currGroupSub = groupSubject.Where(gs => gs.Group.GradeId == grades[i].GradeId);

                double hours = currGradeSub.Select(gs => gs.HoursNumber).Sum();
                double mean = (currGradeSub.Select(gs => gs.HoursNumber * gs.Subject.Complexity)
                    .Sum() -
                    currGradeSub
                    .Where(g => (currGroupSub
                    .Select(s => s.TeacherSubject.SubjectId))
                    .Contains(g.SubjectId)).Select(gs => gs.HoursNumber * gs.Subject.Complexity)
                    .Sum() / 2) / hours * 5;

                double maxHoursPerDay = Math.Ceiling(hours / 5);

                int currMaxHoursPerDay = (int)Math.Min(7, maxHoursPerDay);

                List<List<List<Teachers>>> timetable = new List<List<List<Teachers>>>();
                IndexViewModel currTT = new IndexViewModel(timetable, grades[i]);

                for (int lnum = 0; lnum < currMaxHoursPerDay; lnum++)
                {
                    timetable.Add(new List<List<Teachers>>());

                    for (int wd = 0; wd < 5; wd++)
                    {
                        timetable[lnum].Add(new List<Teachers>());
                    }
                }



                // Groups

                List<int> used = new List<int>();

                foreach (var gs in groupSubject)
                {
                    if (used.Contains(gs.GroupId))
                    {
                        continue;
                    }

                    int currGroupHours = gradeSubject.Where(g => g.SubjectId == g.SubjectId).FirstOrDefault().HoursNumber;
                    int otherId = currGroupSub.Where(t => t.Group.GroupTypeId == gs.Group.GroupTypeId && t.GroupId != gs.GroupId && t.TeacherSubject.SubjectId == gs.TeacherSubject.SubjectId).FirstOrDefault().GroupId;
                    used.Add(gs.GroupId);
                    used.Add(otherId);

                    Random random = new Random();
                    int day = random.Next(1, 6);
                    int lnum = random.Next(1, currMaxHoursPerDay + 1);

                }

            }

            return View(models);
        }
    }
}
