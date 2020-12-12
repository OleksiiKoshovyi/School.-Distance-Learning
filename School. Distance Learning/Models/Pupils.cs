using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        public int GradeId { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 5)]
        [Remote(action: "IsLoginUnique", controller: "Pupils",
            AdditionalFields = "PupilId",
            ErrorMessage = "account with this login already exists")]
        public string Login { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 6)]
        public string Password { get; set; }

        public string FullName
        {
            get
            {
                return $"{SurName} {FirstName} {Patronymic}";
            }
        }

        public string PartName
        {
            get
            {
                return $"{SurName} {(FirstName ?? " ")[0]}. {(Patronymic ?? " ")[0]}.";
            }
        }

        public override bool Equals(object obj)
        {
            return ((Pupils)obj).PupilId == PupilId;
        }

        public virtual Grades Grade { get; set; }
        public virtual ICollection<GroupPupil> GroupPupil { get; set; }
        public virtual ICollection<SkippingClasses> SkippingClasses { get; set; }


    }
}
