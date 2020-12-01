using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School._Distance_Learning.Models
{
    public partial class Grades
    {
        public Grades()
        {
            GradeSubject = new HashSet<GradeSubject>();
            Groups = new HashSet<Groups>();
            Pupils = new HashSet<Pupils>();
        }

        public int GradeId { get; set; }

        [Required]
        [Remote(action: "IsGradeUnique", controller: "Grades",
            ErrorMessage = "a grade with the same first year and a letter already exists")]
        public int FirstYear { get; set; }

        [Required]
        [StringLength(1, MinimumLength = 1)]
        [Remote(action: "IsGradeUnique", controller: "Grades",
            ErrorMessage = "a grade with the same first year and a letter already exists")]
        public string Letter { get; set; }

        public virtual ICollection<GradeSubject> GradeSubject { get; set; }
        public virtual ICollection<Groups> Groups { get; set; }
        public virtual ICollection<Pupils> Pupils { get; set; }
    }
}
