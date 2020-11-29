using System;
using System.Collections.Generic;

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
        public int GradeId { get; set; }

        public virtual Grades Grade { get; set; }
        public virtual GroupTypes GroupType { get; set; }
        public virtual ICollection<GroupPupil> GroupPupil { get; set; }
        public virtual ICollection<TeacherSubjectGroup> TeacherSubjectGroup { get; set; }
    }
}
