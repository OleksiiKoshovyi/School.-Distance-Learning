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
        public DateTime PassDate { get; set; }

        [Required]
        // Remote
        public int TeacherSubjectGroupId { get; set; }

        public virtual TeacherSubjectGroup TeacherSubjectGroup { get; set; }
    }
}
