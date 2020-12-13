using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace School._Distance_Learning.ViewModels.TimetableForPupils
{
    public class CsvResult : FileResult
    {
        private readonly IndexViewModel _timetableData;

        public CsvResult(IndexViewModel timetableData) : base("text/csv")
        {
            _timetableData = timetableData;
            FileDownloadName = $"{timetableData.grade.GradeName}:{timetableData.date}.csv";
        }

        public async override Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;

            context.HttpContext.Response.Headers
                .Add("Content-Disposition", new[] { "attachment; filename=" + FileDownloadName });

            string delimiter = ";";

            using (var streamWriter = new StreamWriter(response.Body, System.Text.Encoding.UTF8))
            {

                await streamWriter.WriteLineAsync(
                  $"{_timetableData.grade.GradeName}"
                );

                await streamWriter.WriteLineAsync(
                  ";Monday;Tuesday;Wednesday;Thursday;Friday"
                );

                for (int i = 0; i < _timetableData.timetable.Count(); ++i)
                {
                    await streamWriter.WriteLineAsync(
                        (i+1).ToString()+delimiter+string.Join(delimiter,
                        _timetableData.timetable[i].Select(
                            t => string.Join(" / ", t.Select(
                                l => l.TeacherSubjectGroup.TeacherSubject.Subject.SubjectName))))
                    );
                    await streamWriter.FlushAsync();
                }
                await streamWriter.FlushAsync();

                await streamWriter.WriteLineAsync(
                  ""
                );

                await streamWriter.WriteLineAsync(
                  $";;;;Created at:;{_timetableData.date}"
                );

            }
        }

    }
}