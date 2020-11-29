using System;
using System.Collections.Generic;

namespace School._Distance_Learning.Models
{
    public partial class GradeSubject
    {
        public int GradeSubjectId { get; set; }
        public int GradeId { get; set; }
        public int SubjectId { get; set; }
        public int HoursNumber { get; set; }

        public virtual Grades Grade { get; set; }
        public virtual Subjects Subject { get; set; }
    }
}
