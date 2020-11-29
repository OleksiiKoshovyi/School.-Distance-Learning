using System;
using System.Collections.Generic;

namespace School._Distance_Learning.Models
{
    public partial class GroupTypeSubject
    {
        public int GroupTypeSubjectId { get; set; }
        public int GroupTypeId { get; set; }
        public int SubjectId { get; set; }

        public virtual GroupTypes GroupType { get; set; }
        public virtual Subjects Subject { get; set; }
    }
}
