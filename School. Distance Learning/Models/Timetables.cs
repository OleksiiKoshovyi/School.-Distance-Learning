using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School._Distance_Learning.Models
{
    public partial class Timetables
    {
        public Timetables()
        {
            SkippingClasses = new HashSet<SkippingClasses>();
        }

        public int TimetableId { get; set; }

        [Required]
        [Range(1,7)]
        public int WeekDayNumber { get; set; }

        [Required]
        [Range(1, 10)]
        public int LessonNumber { get; set; }

        [Required]
        [Range(0, 2)]
        public int Oddness { get; set; }

        [Required]
        // Remote
        public int TeacherSubjectGroupId { get; set; }

        public virtual TeacherSubjectGroup TeacherSubjectGroup { get; set; }
        public virtual ICollection<SkippingClasses> SkippingClasses { get; set; }
    }
}
