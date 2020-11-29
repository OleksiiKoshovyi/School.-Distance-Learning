using System;
using System.Collections.Generic;

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
        public int FirstYear { get; set; }
        public string Letter { get; set; }

        public virtual ICollection<GradeSubject> GradeSubject { get; set; }
        public virtual ICollection<Groups> Groups { get; set; }
        public virtual ICollection<Pupils> Pupils { get; set; }
    }
}
