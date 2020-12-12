using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School._Distance_Learning.Models
{
    public partial class TeacherSubject
    {
        public TeacherSubject()
        {
            TeacherSubjectGroup = new HashSet<TeacherSubjectGroup>();
        }

        public int TeacherSubjectId { get; set; }

        [Required]
        [Remote(action: "IsTeacherSubjectsUnique", controller: "TeacherSubjects",
            AdditionalFields = "SubjectId",
            ErrorMessage = "similar conformity already exists")]
        public int TeacherId { get; set; }

        [Required]
        [Remote(action: "IsTeacherSubjectsUnique", controller: "TeacherSubjects",
            AdditionalFields = "TeacherId",
            ErrorMessage = "similar conformity already exists")]
        public int SubjectId { get; set; }

        [Required]
        [Range(1, 40)]
        public int HoursNumber { get; set; }

        public string TeacherSubjectName 
        {
            get
            {
                return $"{Teacher?.SurName}: {Subject?.SubjectName}; ";
            }
        }

        public virtual Subjects Subject { get; set; }
        public virtual Teachers Teacher { get; set; }
        public virtual ICollection<TeacherSubjectGroup> TeacherSubjectGroup { get; set; }
    }
}
