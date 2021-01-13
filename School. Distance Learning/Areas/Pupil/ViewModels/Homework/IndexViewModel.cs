﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School._Distance_Learning.Areas.Pupil.ViewModels.Homework
{
    public class IndexViewModel
    {
        public int timetableId;
        public int teacherSubjectGroupId;
        public int? homeworkId;
        public int lessonNumber;
        public string subject;
        public string homework;

        public IndexViewModel(int timetableId, int teacherSubjectGroupId, int? homeworkId,
            int lessonNumber, string subject, string homework)
        {
            this.timetableId = timetableId;
            this.teacherSubjectGroupId = teacherSubjectGroupId;
            this.homeworkId = homeworkId;
            this.lessonNumber = lessonNumber;
            this.subject = subject;
            this.homework = homework;
        }

        public IndexViewModel() { }
    }
}
