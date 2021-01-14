using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School._Distance_Learning.Areas.Pupil.ViewModels.Homework;
using School._Distance_Learning.Models;

namespace School._Distance_Learning.Areas.Pupil.Controllers
{
    [Area("Pupil")]
    public class HomeworkController : Controller
    {
        private readonly SchoolDLContext _context;

        public HomeworkController(SchoolDLContext context)
        {
            _context = context;
        }

        // GET: TimetableForPupilsController
        public async Task<ActionResult> IndexAsync(int? pupilid, DateTime? date)
        {
            if (pupilid == null)
            {
                pupilid = _context.Pupils.FirstOrDefault().PupilId;
            }

            if (date == null)
            {
                date = DateTime.Now;
            }

            ViewData["PupilId"] = new SelectList(_context.Pupils,
                "PupilId", "SurName", pupilid);

            if (date != null)
            {
                ViewData["CurrentFilterDate"] = date?.ToString("yyyy-MM-dd");
            }

            #region Homework
            List<IndexViewModel> homework = new List<IndexViewModel>();
            var conn = _context.Database.GetDbConnection();
            try
            {
                await conn.OpenAsync();
                using (var command = conn.CreateCommand())
                {
                    string groups =
                        " SELECT Groups.GroupId FROM Groups " +
                        "RIGHT JOIN GroupPupil gp ON gp.GroupId = Groups.GroupId " +
                        $"WHERE gp.PupilId = {pupilid} ";

                    string query =
                        "SELECT tt.TimetableId, tt.TeacherSubjectGroupId, hw.HomeworkId, tt.LessonNumber, s.SubjectName, hw.Homework " +
                        "FROM Timetables tt " +
                        "LEFT JOIN TeacherSubjectGroup tsg ON tt.TeacherSubjectGroupId = tsg.TeacherSubjectGroupId " +
                        "LEFT JOIN Groups g ON g.GroupId = tsg.GroupId " +
                        "LEFT JOIN TeacherSubject ts ON ts.TeacherSubjectId = tsg.TeacherSubjectId " +
                        "LEFT JOIN Subjects s ON s.SubjectId = ts.SubjectId " +
                        $"LEFT JOIN Homeworks hw ON hw.PassDate = '{date?.ToString("yyyy-MM-dd")}' " +
                            "AND hw.TeacherSubjectGroupId = tt.TeacherSubjectGroupId " +
                        $"WHERE (tsg.GroupId IN ({groups}) OR g.GroupTypeId IS NULL) " +
                            $"AND tt.WeekDayNumber = DATEPART( dw , '{date?.ToString("yyyy-MM-dd")}') " +
                            $"AND g.GradeId = (SELECT GradeId FROM Pupils WHERE PupilId = {pupilid}) " +
                        "ORDER BY tt.LessonNumber;";

                    command.CommandText = query;
                    DbDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new IndexViewModel
                            {
                                timetableId = reader.GetInt32(0),
                                teacherSubjectGroupId = reader.GetInt32(1),
                                lessonNumber = reader.GetInt32(3),
                                subject = reader.GetString(4)
                            };
                            if (!reader.IsDBNull(2))
                            {
                               row.homeworkId = reader.GetInt32(2);
                               row.homework = reader.GetString(5);
                            }
                            homework.Add(row);
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

            return View(homework);
        }
    }
}
