using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School._Distance_Learning.Models;
using School._Distance_Learning.ViewModels.Automation;

namespace School._Distance_Learning.Controllers
{
    public class AutomationController : Controller
    {
        private readonly SchoolDLContext _context;

        public AutomationController(SchoolDLContext context)
        {
            _context = context;
        }

        private bool CheckTeacherCollision(
           List<List<List<List<TeacherSubjectGroup>>>> tsg,
           int lnum, int day, Teachers teacher)
        {
            foreach (var tt in tsg)
            {
                if (tt[lnum][day]
                    .Select(tsg => tsg.TeacherSubject.TeacherId).Contains(teacher.TeacherId))
                {
                    return false;
                }
            }
            return true;
        }

        private bool CheckGradeCollision(
           List<List<List<List<TeacherSubjectGroup>>>> tsg,
           int lnum, int day, int gradeIndex)
        {
            return tsg[gradeIndex][lnum][day].Count() == 0;
        }

        private bool CheckCollision(
            List<List<List<List<TeacherSubjectGroup>>>> tsg, 
            int lnum, int day, int gradeIndedx, List<Teachers> teachers)
        {
            if (!CheckGradeCollision(tsg, lnum, day, gradeIndedx))
            {
                return false;
            }

            foreach (var teacher in teachers)
            {
                if (!CheckTeacherCollision(tsg, lnum, day, teacher))
                {
                    return false;
                }
            }

            return true;
        }

        private int SetPartTimetable(List<List<List<List<TeacherSubjectGroup>>>> timetables,
            int gradeIndex, int hoursNumber,
            List<TeacherSubjectGroup> currentTeacherSubjectGroups,
            Random random, int maxHoursPerDay, int mask = 0)
        {
            // true if visited
            List<bool> visitedDays = new List<bool>(5);
            for (int i = 0; i < 5; i++)
            {
                visitedDays.Add(false || (mask & (1 << i)) != 0);
            }

            List<Teachers> teachers = 
                currentTeacherSubjectGroups
                .Select(tsg => tsg.TeacherSubject.Teacher)
                .ToList();

            int maxPosition = maxHoursPerDay * 5;

            int prevDay = random.Next(0, maxPosition) / maxHoursPerDay % 5;

            while (hoursNumber != 0)
            {
                int position = random.Next(0, maxPosition);
                int day = position / maxHoursPerDay % 5;
                int lnum = (position - day * maxHoursPerDay) % maxHoursPerDay;

                // Jumping optimization
                if (hoursNumber == 1)
                {
                    day = (prevDay + 2 + position % 2) % 5;
                }

                // Check visited days
                for (int i = 0; i < 5 && visitedDays[day]; i++)
                {
                    day = (day + 1) % 5;
                }

                // If all days are visited, reload visited list
                if (visitedDays[day])
                {
                    for (int i = 0; i < 5; i++)
                    {
                        visitedDays[i] = false;
                    }
                    mask = 0;
                    continue;
                }

                for (int i = 0; i < maxPosition; i++)
                {
                    if (CheckCollision(timetables, lnum, day, gradeIndex, teachers))
                    {
                        foreach (var curr in currentTeacherSubjectGroups)
                        {
                            timetables[gradeIndex][lnum][day].Add(curr);
                        }
                        visitedDays[day] = true;
                        mask += (1 << day);
                        prevDay = day;
                        break;
                    }
                    else
                    {
                        lnum += 1;

                        if (lnum == maxHoursPerDay)
                        {
                            lnum = 0;
                            day = (day + 1) % 5;
                        }
                    }
                }

                hoursNumber -= 1;
            }

            return mask;
        }

        public async Task<IActionResult> IndexAsync()
        {
            Random random = new Random(DateTime.Now.Second + DateTime.Now.Millisecond);

            List<Grades> grades = 
                await _context.Grades
                .OrderBy(g => g.GradeId)
                .ToListAsync();

            List<List<List<List<TeacherSubjectGroup>>>> timetables = new List<List<List<List<TeacherSubjectGroup>>>>();

            // Generation blank timetables
            for (int i = 0; i < grades.Count; i++)
            {
                timetables.Add(new List<List<List<TeacherSubjectGroup>>>());
                for (int j = 0; j < 7; j++)
                {
                    timetables[i].Add(new List<List<TeacherSubjectGroup>>());
                    for (int k = 0; k < 5; k++)
                    {
                        timetables[i][j].Add(new List<TeacherSubjectGroup>());
                    }
                }
            }

            List<GradeSubject> gradeSubjects = await _context.GradeSubject.ToListAsync();

            #region SingleGroupTypes
            List<int> singleGroupTypes = new List<int>();
            var conn = _context.Database.GetDbConnection();
            try
            {
                await conn.OpenAsync();
                using (var command = conn.CreateCommand())
                {
                    string query =
                        "SELECT gt.GroupTypeId, COUNT(SubjectId) FROM GroupTypes gt " +
                        "RIGHT JOIN GroupTypeSubject gts ON gts.GroupTypeId = gt.GroupTypeId " +
                        "GROUP BY gt.GroupTypeId " +
                        "HAVING COUNT(SubjectId) = 1;";
                    command.CommandText = query;
                    DbDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            singleGroupTypes.Add(reader.GetInt32(0));
                        }
                    }
                    reader.Dispose();
                }
            }
            finally
            {
                conn.Close();
            }
            #endregion

            // TeacherSubjectGroups for real group
            List<TeacherSubjectGroup> tsubGroups =
                await _context.TeacherSubjectGroup
                .Include(tsg => tsg.Group)
                .Include(tsg => tsg.Group.Grade)
                .Include(tsg => tsg.TeacherSubject)
                .Include(tsg => tsg.TeacherSubject.Teacher)
                .Include(tsg => tsg.TeacherSubject.Subject)
                .Where(tsg => tsg.Group.GroupTypeId != null)
                .ToListAsync();

            #region Groups: double

            // TeacherSubjectGroup for group with couple subjects
            List<TeacherSubjectGroup> tsubgroupsDouble =
                tsubGroups.Where(
                    tsg => !singleGroupTypes.Contains((int)tsg.Group.GroupTypeId))
                .ToList();

            // Operating by grade
            for (int gradeIndex = 0; gradeIndex < grades.Count(); gradeIndex++)
            {
                // TeacherSubjectGroup for current grade
                List<TeacherSubjectGroup> currentGroups =
                    tsubgroupsDouble.Where(tsg => tsg.Group.GradeId == grades[gradeIndex].GradeId)
                    .ToList();

                List<int> currentTypeGroupsId =
                    currentGroups.Select(tsg => (int)tsg.Group.GroupTypeId)
                    .Distinct()
                    .ToList();

                List<GradeSubject> currentGradeSubjects =
                    gradeSubjects
                    .Where(gs => gs.GradeId == grades[gradeIndex].GradeId)
                    .ToList();

                // Operating by TypeGroup
                foreach (var currentGroupTypeId in currentTypeGroupsId)
                {
                    List<TeacherSubjectGroup> currentTypeGroup =
                        currentGroups.Where(tsg => (int)tsg.Group.GroupTypeId == currentGroupTypeId)
                        .OrderBy(tsg => tsg.TeacherSubject.SubjectId)
                        .ThenBy(tsg => tsg.GroupId)
                        .ToList();

                    int firstTime =
                        currentGradeSubjects.Find(
                            gs => gs.SubjectId == currentTypeGroup[0]
                            .TeacherSubject.SubjectId)?.HoursNumber ?? 0;
                    
                    int secondTime =
                        currentGradeSubjects.Find(
                            gs => gs.SubjectId == currentTypeGroup[2]
                            .TeacherSubject.SubjectId)?.HoursNumber ?? 0;

                    int minHoursNumber = Math.Min(firstTime, secondTime);

                    // Different Subjects
                    List<TeacherSubjectGroup> firstSet =
                        new List<TeacherSubjectGroup> { currentTypeGroup[0], currentTypeGroup[3] };

                    List<TeacherSubjectGroup> secondSet =
                        new List<TeacherSubjectGroup> { currentTypeGroup[1], currentTypeGroup[2] };

                    int mask = SetPartTimetable(timetables, gradeIndex, minHoursNumber, firstSet, random, 7);        
                    mask = SetPartTimetable(timetables, gradeIndex, minHoursNumber, secondSet, random, 7, mask);

                    // Same Subjects
                    firstSet =
                        new List<TeacherSubjectGroup> { currentTypeGroup[0], currentTypeGroup[1] };

                    secondSet =
                        new List<TeacherSubjectGroup> { currentTypeGroup[2], currentTypeGroup[3] };

                    mask = SetPartTimetable(timetables, gradeIndex, firstTime - minHoursNumber, firstSet, random, 7, mask);
                    mask = SetPartTimetable(timetables, gradeIndex, secondTime - minHoursNumber, secondSet, random, 7, mask);
                }
                
            }

            #endregion

            #region Groups: single

            // TeacherSubjectGroup for group with single subject
            List<TeacherSubjectGroup> tsubgroupsSingle =
                tsubGroups.Where(
                    tsg => singleGroupTypes.Contains((int)tsg.Group.GroupTypeId))
                .ToList();

            // Operating by grade
            for (int gradeIndex = 0; gradeIndex < grades.Count(); gradeIndex++)
            {
                // TeacherSubjectGroup for current grade
                List<TeacherSubjectGroup> currentGroups =
                    tsubgroupsSingle.Where(tsg => tsg.Group.GradeId == grades[gradeIndex].GradeId)
                    .ToList();

                List<int> currentTypeGroupsId =
                    currentGroups.Select(tsg => (int)tsg.Group.GroupTypeId)
                    .Distinct()
                    .ToList();

                List<GradeSubject> currentGradeSubjects =
                    gradeSubjects
                    .Where(gs => gs.GradeId == grades[gradeIndex].GradeId)
                    .ToList();

                // Operating by TypeGroup
                foreach (var currentGroupTypeId in currentTypeGroupsId)
                {
                    List<TeacherSubjectGroup> currentTypeGroup =
                        currentGroups.Where(tsg => (int)tsg.Group.GroupTypeId == currentGroupTypeId)
                        .OrderBy(tsg => tsg.TeacherSubject.SubjectId)
                        .ThenBy(tsg => tsg.GroupId)
                        .ToList();

                    int time =
                        currentGradeSubjects
                        .Find(gs => gs.SubjectId == currentTypeGroup[0]
                        .TeacherSubject.SubjectId)
                        ?.HoursNumber ?? 0;

                    // Same Subjects
                    SetPartTimetable(timetables, gradeIndex, time, currentTypeGroup, random, 7);
                }

            }

            #endregion

            #region Simple
            List <TeacherSubjectGroup> tsubGrades =
                await _context.TeacherSubjectGroup
                .Include(tsg => tsg.Group)
                .Include(tsg => tsg.Group.Grade)
                .Include(tsg => tsg.TeacherSubject)
                .Include(tsg => tsg.TeacherSubject.Teacher)
                .Include(tsg => tsg.TeacherSubject.Subject)
                .Where(tsg => tsg.Group.GroupTypeId == null)
                .OrderBy(tsg => tsg.Group.GradeId)
                .ToListAsync();

            // Operating by grade
            for (int gradeIndex = 0; gradeIndex < grades.Count(); gradeIndex++)
            {
                // TeacherSubjectGroup for current grade
                // Teacher - Subject - Group (Grade)
                List<TeacherSubjectGroup> currentGroups =
                    tsubGrades.Where(tsg => tsg.Group.GradeId == grades[gradeIndex].GradeId)
                    .ToList();

                // HoursNumber for each subject
                List<GradeSubject> currentGradeSubjects =
                    gradeSubjects
                    .Where(gs => gs.GradeId == grades[gradeIndex].GradeId)
                    .ToList();

                // Operating by TypeGroup
                foreach (var currentTeacherSubjectGroup in currentGroups)
                {
                    int time =
                        currentGradeSubjects
                        .Find(gs => gs.SubjectId == currentTeacherSubjectGroup
                        .TeacherSubject.SubjectId)
                        ?.HoursNumber ?? 0;

                    // Same Subjects
                    SetPartTimetable(timetables, gradeIndex, time, new List<TeacherSubjectGroup>(){currentTeacherSubjectGroup}, random, 7);
                }
            }
            #endregion

            List<TimetableViewModel> result = new List<TimetableViewModel>();

            for (int i = 0; i < grades.Count(); ++i)
            {
                TimetableViewModel timetableViewModel =
                    new TimetableViewModel
                    {
                        timetable = timetables[i],
                        grade = grades[i]
                    };
                result.Add(timetableViewModel);
            }

            IndexViewModel indexViewModel = new IndexViewModel()
            {
                timetables = result,
                generationTime = DateTime.Now
            };

            return View(indexViewModel);
        }
    }
}
