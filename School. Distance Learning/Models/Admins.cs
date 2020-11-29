using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School._Distance_Learning.Models
{
    public partial class Admins
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdminId { get; set; }

        [StringLength(25, MinimumLength = 5, ErrorMessage = "Must be at least 5 characters long.")]
        public string Login { get; set; }

        [StringLength(25, MinimumLength = 6, ErrorMessage = "Must be at least 6 characters long.")]
        public string Password { get; set; }
    }
}
