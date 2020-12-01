using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string SubjectName { get; set; }

        [Required]
        [Range(1, 10)]
        public int Complexity { get; set; }

        public virtual ICollection<GradeSubject> GradeSubject { get; set; }
        public virtual ICollection<GroupTypeSubject> GroupTypeSubject { get; set; }
        public virtual ICollection<TeacherSubject> TeacherSubject { get; set; }
    }
}
