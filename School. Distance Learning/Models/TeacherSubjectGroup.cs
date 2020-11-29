using System;
using System.Collections.Generic;

namespace School._Distance_Learning.Models
{
    public partial class TeacherSubjectGroup
    {
        public TeacherSubjectGroup()
        {
            Homeworks = new HashSet<Homeworks>();
            Timetables = new HashSet<Timetables>();
        }

        public int TeacherSubjectGroupId { get; set; }
        public int TeacherSubjectId { get; set; }
        public int GroupId { get; set; }

        public virtual Groups Group { get; set; }
        public virtual TeacherSubject TeacherSubject { get; set; }
        public virtual ICollection<Homeworks> Homeworks { get; set; }
        public virtual ICollection<Timetables> Timetables { get; set; }
    }
}
