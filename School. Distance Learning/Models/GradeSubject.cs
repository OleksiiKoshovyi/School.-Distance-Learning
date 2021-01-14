using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School._Distance_Learning.Models
{
    public partial class GradeSubject
    {
        public int GradeSubjectId { get; set; }
        [Required]
        public int GradeId { get; set; }
        [Required]
        public int SubjectId { get; set; }

        [Required]
        [Range(1, 40)]
        public int HoursNumber { get; set; }

        public virtual Grades Grade { get; set; }
        public virtual Subjects Subject { get; set; }
    }
}
