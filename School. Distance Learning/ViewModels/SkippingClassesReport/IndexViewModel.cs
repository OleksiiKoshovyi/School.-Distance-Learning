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

        public List<List<List<Models.SkippingClasses>>> classes;

        public IndexViewModel(List<Models.Pupils> p,
            List<DateTime> d,
            List<List<List<Models.SkippingClasses>>> c)
        {
            pupils = p;
            dates = d;
            classes = c;
        }
    }
}
