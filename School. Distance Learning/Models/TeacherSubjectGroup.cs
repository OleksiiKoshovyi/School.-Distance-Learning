using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Required]
        [Remote(action: "IsTeacherSubjectGroupUnique", controller: "TeacherSubjectGroups",
            AdditionalFields = "GroupId,TeacherSubject",
            ErrorMessage = "similar conformity already exists")]
        public int TeacherSubjectId { get; set; }
        public int GroupId { get; set; }

        public string TeacherSubjectGroupName 
        {
            get
            {
                return $"{Group?.GroupName} : {TeacherSubject?.Subject?.SubjectName} : {TeacherSubject?.Teacher?.PartName}";
            }
        }

        public virtual Groups Group { get; set; }
        public virtual TeacherSubject TeacherSubject { get; set; }
        public virtual ICollection<Homeworks> Homeworks { get; set; }
        public virtual ICollection<Timetables> Timetables { get; set; }

        public override string ToString()
        {
            return TeacherSubjectGroupName; 
        }
    }
}
