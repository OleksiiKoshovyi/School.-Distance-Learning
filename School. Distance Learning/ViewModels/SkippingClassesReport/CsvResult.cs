using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace School._Distance_Learning.ViewModels.SkippingClassesReport
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
                  ";"+_timetableData.dates.Select(d => d.ToString("dd.MM.yyyy")).Aggregate((i,j) => i+$";{j}")
                );

                for (int i = 0; i < _timetableData.classes.Count(); ++i)
                {
                    await streamWriter.WriteLineAsync(
                        _timetableData.pupils[i].PartName + delimiter + string.Join(delimiter,
                        _timetableData.classes[i].Select(
                            t => string.Join(" / ", t.Select(
                                l => "▼"))))
                    );
                    await streamWriter.FlushAsync();
                }
                await streamWriter.FlushAsync();

                await streamWriter.WriteLineAsync(
                  ""
                );

                await streamWriter.WriteLineAsync(
                  $"Created at:;{_timetableData.date}"
                );

            }
        }

    }
}