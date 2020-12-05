using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace School._Distance_Learning.Models
{
    public partial class GroupTypeSubject
    {
        public int GroupTypeSubjectId { get; set; }

        [Remote(action: "IsGroupTypeSubjectUnique", controller: "GroupTypeSubjects",
            AdditionalFields = "SubjectId",
            ErrorMessage = "account with this login already exists")]
        public int GroupTypeId { get; set; }
        public int SubjectId { get; set; }

        public string GroupName
        { 
            get 
            {
                return $"{GroupType?.GroupTypeName}: {Subject?.SubjectName}";
            }
        }

        public virtual GroupTypes GroupType { get; set; }
        public virtual Subjects Subject { get; set; }
    }
}
