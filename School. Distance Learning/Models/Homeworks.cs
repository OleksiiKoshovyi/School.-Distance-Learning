using System;
using System.Collections.Generic;

namespace School._Distance_Learning.Models
{
    public partial class Homeworks
    {
        public int HomeworkId { get; set; }
        public DateTime PassDate { get; set; }
        public int TeacherSubjectGroupId { get; set; }

        public virtual TeacherSubjectGroup TeacherSubjectGroup { get; set; }
    }
}
