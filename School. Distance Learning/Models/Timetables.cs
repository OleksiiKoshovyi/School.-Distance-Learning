using System;
using System.Collections.Generic;

namespace School._Distance_Learning.Models
{
    public partial class Timetables
    {
        public Timetables()
        {
            SkippingClasses = new HashSet<SkippingClasses>();
        }

        public int TimetableId { get; set; }
        public int WeekDayNumber { get; set; }
        public int LessonNumber { get; set; }
        public int Oddness { get; set; }
        public int TeacherSubjectGroupId { get; set; }

        public virtual TeacherSubjectGroup TeacherSubjectGroup { get; set; }
        public virtual ICollection<SkippingClasses> SkippingClasses { get; set; }
    }
}
