using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using School._Distance_Learning.Models;

namespace School._Distance_Learning.ViewModels.Pupils
{
    public class IndexViewModel
    {
        public IndexViewModel(Models.Pupils pupils, string gradeName)
        {
            Pupils = pupils;
            GradeName = gradeName;
        }

        public Models.Pupils Pupils { get; set; }

        public string GradeName { get; set; }
    }
}
