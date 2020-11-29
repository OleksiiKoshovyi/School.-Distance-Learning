using System;
using System.Collections.Generic;

namespace School._Distance_Learning.Models
{
    public partial class TeacherSubject
    {
        public TeacherSubject()
        {
            TeacherSubjectGroup = new HashSet<TeacherSubjectGroup>();
        }

        public int TeacherSubjectId { get; set; }
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }
        public int HoursNumber { get; set; }

        public virtual Subjects Subject { get; set; }
        public virtual Teachers Teacher { get; set; }
        public virtual ICollection<TeacherSubjectGroup> TeacherSubjectGroup { get; set; }
    }
}
