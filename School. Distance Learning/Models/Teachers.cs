using Microsoft.AspNetCore.Mvc;
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

        [Required]
        [StringLength(25, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 2)]
        public string SurName { get; set; }

        [StringLength(25)]
        public string Patronymic { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime RecruitmentDate { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 5)]
        [Remote(action: "IsLoginUnique", controller: "Teachers",
            AdditionalFields = "TeacherId",
            ErrorMessage = "account with this login already exists")]
        public string Login { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 6)]
        public string Password { get; set; }

        public virtual ICollection<TeacherSubject> TeacherSubject { get; set; }
    }
}
