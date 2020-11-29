using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School._Distance_Learning.Models
{
    public partial class Teachers
    {
        public Teachers()
        {
            TeacherSubject = new HashSet<TeacherSubject>();
        }

        public int TeacherId { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Patronymic { get; set; }

        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }

        [DataType(DataType.Date)]
        public DateTime RecruitmentDate { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public virtual ICollection<TeacherSubject> TeacherSubject { get; set; }
    }
}
