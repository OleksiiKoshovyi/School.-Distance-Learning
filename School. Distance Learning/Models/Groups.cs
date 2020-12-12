using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School._Distance_Learning.Models
{
    public partial class Groups
    {
        public Groups()
        {
            GroupPupil = new HashSet<GroupPupil>();
            TeacherSubjectGroup = new HashSet<TeacherSubjectGroup>();
        }

        public int GroupId { get; set; }
        public int? GroupTypeId { get; set; }

        [Required]
        public int GradeId { get; set; }

        public string GroupName 
        {
            get
            {
                if (GroupType != null)
                {
                    return $"{Grade.GradeName}: [{GroupType.GroupTypeName}]({GroupId})";
                }
                else 
                {
                    return Grade.GradeName;
                }

                /*List<string> names = new List<string>();
                foreach (var item in TeacherSubjectGroup ?? new List<TeacherSubjectGroup>())
                {

                }*/
            }     
        }

        public virtual Grades Grade { get; set; }
        public virtual GroupTypes GroupType { get; set; }
        public virtual ICollection<GroupPupil> GroupPupil { get; set; }
        public virtual ICollection<TeacherSubjectGroup> TeacherSubjectGroup { get; set; }
    }
}
