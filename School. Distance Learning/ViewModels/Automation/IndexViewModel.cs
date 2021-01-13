using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School._Distance_Learning.ViewModels.Automation
{
    public class IndexViewModel
    {
        public List<TimetableViewModel> timetables;
        public DateTime generationTime = DateTime.Now;
    }
}
