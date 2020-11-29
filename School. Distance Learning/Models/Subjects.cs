using System;
using System.Collections.Generic;

namespace School._Distance_Learning.Models
{
    public partial class Subjects
    {
        public Subjects()
        {
            GradeSubject = new HashSet<GradeSubject>();
            GroupTypeSubject = new HashSet<GroupTypeSubject>();
            TeacherSubject = new HashSet<TeacherSubject>();
        }

        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int Complexity { get; set; }

        public virtual ICollection<GradeSubject> GradeSubject { get; set; }
        public virtual ICollection<GroupTypeSubject> GroupTypeSubject { get; set; }
        public virtual ICollection<TeacherSubject> TeacherSubject { get; set; }
    }
}
