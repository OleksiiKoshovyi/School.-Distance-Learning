﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School._Distance_Learning.Models
{
    public partial class Timetables
    {
        public int TimetableId { get; set; }

        [Required]
        [Range(1,7)]
        public int WeekDayNumber { get; set; }

        [Required]
        [Range(1, 10)]
        public int LessonNumber { get; set; }

        [Required]
        [Range(0, 2)]
        public int Oddness { get; set; }

        [Required]
        [Remote(action: "IsTimetableUnique", controller: "Timetables",
            AdditionalFields = "TimetableId,WeekDayNumber,LessonNumber,Oddness",
            ErrorMessage = "account with this login already exists")]
        public int TeacherSubjectGroupId { get; set; }

        public virtual TeacherSubjectGroup TeacherSubjectGroup { get; set; }
    }
}
