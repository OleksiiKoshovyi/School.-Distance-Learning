using School._Distance_Learning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School._Distance_Learning.ViewModels.TimetableForPupils
{
    public class IndexViewModel
    {
        public List<List<List<Timetables>>> timetable;
        public Grades grade;
        public DateTime date = DateTime.Now;

        public IndexViewModel(List<List<List<Timetables>>> tt,
            Grades g, DateTime d)
        {
            timetable = tt;
            grade = g;
            date = d;
        }

        public IndexViewModel() {}
    }
}
