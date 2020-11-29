using System;
using System.Collections.Generic;

namespace School._Distance_Learning.Models
{
    public partial class GroupPupil
    {
        public int GroupPupilId { get; set; }
        public int GroupId { get; set; }
        public int PupilId { get; set; }

        public virtual Groups Group { get; set; }
        public virtual Pupils Pupil { get; set; }
    }
}
