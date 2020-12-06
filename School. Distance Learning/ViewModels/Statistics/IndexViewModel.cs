using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School._Distance_Learning.ViewModels.Statistics
{
    public class IndexViewModel
    {
        public IEnumerable<TeacherWorkload> TeacherWorkloads { get; set; }
        public IEnumerable<SubjectWorkload> SubjectWorkloads { get; set; }
        public IEnumerable<Turant> Turants { get; set; }
        public IEnumerable<DobPerMonth> DobPerMonths { get; set; }

    }

    /// <summary>
    /// Загруженность учителя относительно заявленной
    /// </summary>
    public class TeacherWorkload
    {
        public TeacherWorkload() { }

        public TeacherWorkload(string name, int hours, int real)
        {
            FullName = name;
            WorkingHoursNumber = hours;
            RealWorkingHoursNumber = real;
        }

        public string FullName { get; set; }
        public int WorkingHoursNumber { get; set; }
        public int RealWorkingHoursNumber { get; set; }

        public double Workload
        {
            get
            {
                if (WorkingHoursNumber != 0)
                {
                    return RealWorkingHoursNumber / WorkingHoursNumber;
                }

                return 0;
            }
        }
    }

    /// <summary>
    /// Домашние задания по предмету относительно выделенных на него часов.
    /// </summary>
    public class SubjectWorkload
    {
        public SubjectWorkload() { }
        public SubjectWorkload(string name, int timetable, int homework)
        {
            SubjectName = name;
            HomeworkHoursNumber = homework;
            TimetableHoursNumber = timetable;
        }

        public string SubjectName { get; set; }
        public int TimetableHoursNumber { get; set; }
        public int HomeworkHoursNumber { get; set; }

        public double Workload
        {
            get
            {
                if (TimetableHoursNumber != 0)
                {
                    return HomeworkHoursNumber / TimetableHoursNumber;
                }

                return 0;
            }
        }
    }

    /// <summary>
    /// Прогулы учеников относительно количеству уроков в неделю
    /// </summary>
    public class Turant
    {
        public string FullName { get; set; }
        public string GradeName { get; set; }
        public int SkippingClassesNumber { get; set; }
        public int TimetableHoursNumber { get; set; }

        public double RelateNumber
        {
            get
            {
                if (TimetableHoursNumber != 0)
                {
                    return SkippingClassesNumber / TimetableHoursNumber;
                }

                return 0;
            }
        }
    }

    /// <summary>
    /// Количество дней рождений по месяцам
    /// </summary>
    public class DobPerMonth
    {
        public int DobNumber { get; set; }
        public string Month { get; set; }
    }
    
}
