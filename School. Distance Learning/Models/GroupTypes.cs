using System;
using System.Collections.Generic;
using System.Linq;

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

        public string GroupTypeRealName
        { 
            get 
            {
                if (GroupTypeSubject.Count != 0)
                {
                    return string.Join(" ", GroupTypeSubject.Select(gts => gts.Subject.SubjectName));
                }
                else
                {
                    return "main";
                }
            } 
        }

        public virtual ICollection<GroupTypeSubject> GroupTypeSubject { get; set; }
        public virtual ICollection<Groups> Groups { get; set; }
    }
}
