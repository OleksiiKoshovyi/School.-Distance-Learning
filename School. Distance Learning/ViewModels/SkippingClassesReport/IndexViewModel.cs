using School._Distance_Learning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School._Distance_Learning.ViewModels.SkippingClassesReport
{
    public class IndexViewModel
    {
        public List<Models.Pupils> pupils;
        public List<DateTime> dates;
        public Grades grade;
        public DateTime date = DateTime.Now;

        public List<List<List<SkippingClasses>>> classes;

        public IndexViewModel(List<Models.Pupils> p,
            List<DateTime> d, Grades g,
            List<List<List<SkippingClasses>>> c)
        {
            pupils = p;
            dates = d;
            grade = g;
            classes = c;
        }
    }
}
