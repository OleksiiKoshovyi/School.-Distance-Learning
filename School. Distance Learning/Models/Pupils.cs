using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School._Distance_Learning.Models
{
    public partial class Pupils
    {
        public Pupils()
        {
            GroupPupil = new HashSet<GroupPupil>();
            SkippingClasses = new HashSet<SkippingClasses>();
        }

        public int PupilId { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Patronymic { get; set; }

        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }
        public int GradeId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public virtual Grades Grade { get; set; }
        public virtual ICollection<GroupPupil> GroupPupil { get; set; }
        public virtual ICollection<SkippingClasses> SkippingClasses { get; set; }
    }
}
