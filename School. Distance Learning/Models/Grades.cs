﻿using Microsoft.AspNetCore.Mvc;
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
            AdditionalFields = "Letter",
            ErrorMessage = "a grade with the same first year and a letter already exists")]
        public int FirstYear { get; set; }

        [Required]
        [StringLength(1, MinimumLength = 1)]
        [Remote(action: "IsGradeUnique", controller: "Grades",
            AdditionalFields = "FirstYear",
            ErrorMessage = "a grade with the same first year and a letter already exists")]
        public string Letter { get; set; }

        public string GradeName 
        { 
            get 
            {
                int number = DateTime.Now.Year - FirstYear;
                if (DateTime.Now.Month >= 9)
                {
                    number++;
                }
                return $"{number} {Letter}";
            } 
        }

        public virtual ICollection<GradeSubject> GradeSubject { get; set; }
        public virtual ICollection<Groups> Groups { get; set; }
        public virtual ICollection<Pupils> Pupils { get; set; }
    }
}
