using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School._Distance_Learning.Models
{
    public partial class SkippingClasses
    {
        public int SkippingClassId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime SkippingDate { get; set; }
        public int PupilId { get; set; }

        public virtual Pupils Pupil { get; set; }
    }
}
