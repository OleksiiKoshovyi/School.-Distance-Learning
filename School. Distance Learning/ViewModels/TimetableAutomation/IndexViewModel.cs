using School._Distance_Learning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School._Distance_Learning.ViewModels.TimetableAutomation
{
    public class IndexViewModel
    {
        public List<List<List<Teachers>>> timetable;
        public Grades grade;

        public IndexViewModel(List<List<List<Teachers>>> tt, Grades g) 
        {
            timetable = tt;
            grade = g;
        }
    }
}
