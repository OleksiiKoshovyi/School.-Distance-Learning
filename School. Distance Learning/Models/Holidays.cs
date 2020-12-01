using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School._Distance_Learning.Models
{
    public partial class Holidays
    {
        public int HolidayId { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 4)]
        public string HolidayName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [Range(1, 13)]
        public int Duration { get; set; }
    }
}
