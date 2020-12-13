using School._Distance_Learning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School._Distance_Learning.ViewModels.TimetableForTeachers
{
    public class IndexViewModel
    {
        public List<List<List<Timetables>>> timetable;
        public Teachers teacher;
        public DateTime date = DateTime.Now;

        public IndexViewModel(List<List<List<Timetables>>> tt,
            Teachers t, DateTime d)
        {
            timetable = tt;
            teacher = t;
            date = d;
        }

        public IndexViewModel() {}
    }
}
