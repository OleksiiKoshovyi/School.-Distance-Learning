using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School._Distance_Learning.Areas.Teacher.ViewModels.Homework;
using School._Distance_Learning.Models;

namespace School._Distance_Learning.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    public class HomeworkController : Controller
    {
        private readonly SchoolDLContext _context;

        public HomeworkController(SchoolDLContext context)
        {
            _context = context;
        }

        // GET: TimetableForPupilsController
        public async Task<ActionResult> IndexAsync(int? teacherid, DateTime? date)
        {
            if (teacherid == null)
            {
                teacherid = _context.Teachers.FirstOrDefault().TeacherId;
            }

            if (date == null)
            {
                date = DateTime.Now;
            }

            ViewData["TeacherId"] = new SelectList(_context.TeachersInfo,
                "TeacherId", "TeacherFullName", teacherid);

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
                    string query =
                        "SELECT tt.TimetableId, tt.TeacherSubjectGroupId, hw.HomeworkId, tt.LessonNumber, s.SubjectName, gi.GradeName, hw.Homework " +
                        "FROM Timetables tt " +
                        "LEFT JOIN TeacherSubjectGroup tsg ON tt.TeacherSubjectGroupId = tsg.TeacherSubjectGroupId " +
                        "LEFT JOIN Groups g ON g.GroupId = tsg.GroupId " +
                        "LEFT JOIN GradesInfo gi ON gi.GradeId = g.GradeId " +
                        "LEFT JOIN TeacherSubject ts ON ts.TeacherSubjectId = tsg.TeacherSubjectId " +
                        "LEFT JOIN Subjects s ON s.SubjectId = ts.SubjectId " +
                        $"LEFT JOIN Homeworks hw ON hw.PassDate = '{date?.ToString("yyyy-MM-dd")}' " +
                            "AND hw.TeacherSubjectGroupId = tt.TeacherSubjectGroupId " +
                        $"WHERE tt.WeekDayNumber = DATEPART( dw , '{date?.ToString("yyyy-MM-dd")}') " +
                            $"AND ts.TeacherId = {teacherid} " +
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
                               row.grade = reader.GetString(5);
                               row.homework = reader.GetString(6);
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
