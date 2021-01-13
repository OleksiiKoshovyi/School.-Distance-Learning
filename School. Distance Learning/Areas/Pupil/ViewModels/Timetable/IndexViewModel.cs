using School._Distance_Learning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School._Distance_Learning.Areas.Pupil.ViewModels.Timetable
{
    public class IndexViewModel
    {
        public List<List<List<Timetables>>> timetable;
        public Pupils pupil;
        public DateTime date = DateTime.Now;

        public IndexViewModel(List<List<List<Timetables>>> tt,
            Pupils p,
            DateTime d)
        {
            timetable = tt;
            pupil = p;
            date = d;
        }

        public IndexViewModel() { }
    }
}
