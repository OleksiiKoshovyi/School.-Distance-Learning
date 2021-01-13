using School._Distance_Learning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School._Distance_Learning.ViewModels.Automation
{
    public class TimetableViewModel
    {
        public Grades grade;
        public int Complexity;
        public int HoursNumber;
        public List<List<List<TeacherSubjectGroup>>> timetable;

        public double MeanComplexity
        { 
            get => (HoursNumber == 0) ?
                0 :
                (double)Complexity / (double)HoursNumber; 
        }
    }
}
