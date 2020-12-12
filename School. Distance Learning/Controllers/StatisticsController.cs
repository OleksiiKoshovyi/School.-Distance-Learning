using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School._Distance_Learning.Models;
using School._Distance_Learning.ViewModels.Statistics;

namespace School._Distance_Learning.Controllers
{
    public class StatisticsController : Controller
    {

        private readonly SchoolDLContext _context;

        public StatisticsController(SchoolDLContext context)
        {
            _context = context;
        }

        // GET: StatisticsController
        public async Task<ActionResult> IndexAsync()
        {
            IndexViewModel indexViewModel = new IndexViewModel();

            #region TeacherWorkload
            List<TeacherWorkload> teacherWorkloads = new List<TeacherWorkload>();
            var conn = _context.Database.GetDbConnection();
            try
            {
                await conn.OpenAsync();
                using (var command = conn.CreateCommand())
                {
                    string workinghours = "SELECT teacherid, SUM(hoursnumber) AS workinghours "
                       + "FROM teacherSubject "
                       + "GROUP BY teacherid ";

                    string realhours = "SELECT ts.teacherid, SUM(gs.hoursnumber) AS realhours FROM teacherSubjectGroup tsg "
                        + "LEFT JOIN groups g ON g.groupid = tsg.groupid "
                        + "LEFT JOIN grades ON grades.gradeid = g.gradeid "
                        + "LEFT JOIN teachersubject ts ON ts.teachersubjectid = tsg.teachersubjectid "
                        + "LEFT JOIN gradeSubject gs ON gs.gradeid = g.gradeid AND gs.SubjectId = ts.SubjectId "
                        + "LEFT JOIN teachers t ON ts.teacherid = t.teacherid "
                        + "GROUP BY ts.teacherid ";
                    
                    string query =
                        "SELECT TOP(5) CONCAT(surname, ' ', firstname, ' ', patronymic) AS fullname, w.workinghours, r.realhours "
                        + "FROM teachers t "
                        +$"LEFT JOIN ({workinghours}) AS w ON w.teacherid = t.teacherid "
                        +$"LEFT JOIN ({realhours}) AS r ON r.teacherid = t.teacherid "
                        + "WHERE w.workinghours != 0 AND r.realhours IS NOT NULL "
                        + "ORDER BY CAST(r.realhours AS Float) / w.workinghours desc;";
                    command.CommandText = query;
                    DbDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new TeacherWorkload {
                                FullName = reader.GetString(0),
                                WorkingHoursNumber = reader.GetInt32(1),
                                RealWorkingHoursNumber = reader.GetInt32(2)};
                            teacherWorkloads.Add(row);
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

            #region SubjectWorkload
            List<SubjectWorkload> subjectWorkloads = new List<SubjectWorkload>();
            var conn1 = _context.Database.GetDbConnection();
            try
            {
                await conn1.OpenAsync();
                using (var command = conn1.CreateCommand())
                {
                    string workinghours = "SELECT subjectid, SUM(hoursnumber) AS tt "
                       + "FROM gradeSubject "
                       + "GROUP BY subjectid ";

                    string realhours = "SELECT s.subjectid, COUNT(homeworkid) AS hw FROM homeworks h "
                        + "LEFT JOIN teacherSubjectGroup tsg ON tsg.teacherSubjectGroupId = h.teacherSubjectGroupId "
                        + "LEFT JOIN teachersubject ts ON ts.teachersubjectid = tsg.teachersubjectid "
                        + "LEFT JOIN subjects s ON ts.subjectid = s.subjectid "
                        + "GROUP BY s.subjectid ";

                    string query =
                        "SELECT TOP(5) subjectname, w.tt, r.hw "
                        + "FROM subjects s "
                        + $"LEFT JOIN ({workinghours}) AS w ON w.subjectid = s.subjectid "
                        + $"LEFT JOIN ({realhours}) AS r ON r.subjectid = s.subjectid "
                        + "WHERE w.tt != 0 AND r.hw IS NOT NULL "
                        + "ORDER BY CAST(r.hw AS Float) / w.tt desc ;";
                    command.CommandText = query;
                    DbDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new SubjectWorkload
                            {
                                SubjectName = reader.GetString(0),
                                TimetableHoursNumber = reader.GetInt32(1),
                                HomeworkHoursNumber = reader.GetInt32(2)
                            };
                            subjectWorkloads.Add(row);
                        }
                    }
                    reader.Dispose();
                }
            }
            finally
            {
                conn1.Close();
            }
            #endregion

            #region Turant
            List<Turant> turants = new List<Turant>();
            var conn2 = _context.Database.GetDbConnection();
            try
            {
                await conn2.OpenAsync();
                using (var command = conn2.CreateCommand())
                {
                    string skipping = "SELECT pupilid, COUNT(skippingclassid) AS skippinghours "
                       + "FROM skippingclasses "
                       + "GROUP BY pupilid ";

                    string tthours = "SELECT gradeid, SUM(hoursnumber) * 7 AS tthours "
                        + "FROM gradesubject "
                        + "GROUP BY gradeid ";

                    string query =
                        "SELECT TOP(5) CONCAT(firstname, ' ',surname, ' ',patronymic) AS fullname, gradename, skippinghours, tthours "
                        + "FROM pupils p "
                        + "LEFT JOIN gradesinfo gi ON gi.gradeid = p.gradeid "
                        + $"LEFT JOIN ({skipping}) AS w ON w.pupilid = p.pupilid "
                        + $"LEFT JOIN ({tthours}) AS r ON r.gradeid = p.gradeid "
                        + "WHERE tthours != 0 AND skippinghours IS NOT NULL "
                        + "ORDER BY (CAST(skippinghours AS Float) / tthours) desc;";
                    command.CommandText = query;
                    DbDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new Turant
                            {
                                FullName = reader.GetString(0),
                                GradeName = reader.GetString(1),
                                SkippingClassesNumber = reader.GetInt32(2),
                                TimetableHoursNumber = reader.GetInt32(3)
                            };
                            turants.Add(row);
                        }
                    }
                    reader.Dispose();
                }
            }
            finally
            {
                conn2.Close();
            }
            #endregion

            #region DobPerMonth
            List<DobPerMonth> dobPerMonths = new List<DobPerMonth>();
            var conn3 = _context.Database.GetDbConnection();
            try
            {
                await conn3.OpenAsync();
                using (var command = conn3.CreateCommand())
                {
                    string dobs = " SELECT MONTH(dob) AS monthnumber, COUNT(pupilid) AS dobnumber " +
                        "FROM pupils " +
                        "GROUP BY MONTH(dob) ";

                    string query =
                        "SELECT number AS monthnumber, FORMAT(DateAdd( month , number, -1 ), 'MMMM', 'en-US') as month, COALESCE(r.dobnumber, 0) AS dobnumber " +
                        "FROM master..spt_values " +
                        $"LEFT JOIN({dobs}) AS r ON r.monthnumber = number " +
                        "WHERE Type = 'P' and number between 1 and 12 " +
                        "ORDER BY number";
                    command.CommandText = query;
                    DbDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new DobPerMonth
                            {
                                Month = reader.GetString(1),
                                DobNumber = reader.GetInt32(2)
                            };
                            dobPerMonths.Add(row);
                        }
                    }
                    reader.Dispose();
                }
            }
            finally
            {
                conn3.Close();
            }
            #endregion

            indexViewModel.TeacherWorkloads = teacherWorkloads;
            indexViewModel.SubjectWorkloads = subjectWorkloads;
            indexViewModel.Turants = turants;
            indexViewModel.DobPerMonths = dobPerMonths;

            return View(indexViewModel);
        }
    }
}
