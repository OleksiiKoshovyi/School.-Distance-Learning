using System;
using System.Collections.Generic;

namespace School._Distance_Learning.Models
{
    public partial class GroupTypes
    {
        public GroupTypes()
        {
            GroupTypeSubject = new HashSet<GroupTypeSubject>();
            Groups = new HashSet<Groups>();
        }

        public int GroupTypeId { get; set; }
        public string GroupTypeName { get; set; }

        public virtual ICollection<GroupTypeSubject> GroupTypeSubject { get; set; }
        public virtual ICollection<Groups> Groups { get; set; }
    }
}
