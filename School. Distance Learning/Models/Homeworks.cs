using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School._Distance_Learning.Models
{
    public partial class Homeworks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HomeworkId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Remote(action: "IsHomeworkUnique", controller: "Homeworks",
          AdditionalFields = "HomeworkId,TeacherSubjectGroupId",
          ErrorMessage = "account with this login already exists")]
        public DateTime PassDate { get; set; }

        [Required]
        [Remote(action: "IsHomeworkUnique", controller: "Homeworks",
          AdditionalFields = "HomeworkId,PassDate",
          ErrorMessage = "account with this login already exists")]
        public int TeacherSubjectGroupId { get; set; }

        [Required]
        public string Homework { get; set; }

        public virtual TeacherSubjectGroup TeacherSubjectGroup { get; set; }
    }
}
