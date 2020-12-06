using System;
using System.Collections.Generic;

namespace School._Distance_Learning.Models
{
    public partial class SkippingClasses
    {
        public int SkippingClassId { get; set; }
        public int TimetableId { get; set; }
        public int PupilId { get; set; }
        public int WeekNumber { get; set; }

        public virtual Pupils Pupil { get; set; }
        public virtual Timetables Timetable { get; set; }
    }
}
