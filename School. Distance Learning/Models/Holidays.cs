using System;
using System.Collections.Generic;

namespace School._Distance_Learning.Models
{
    public partial class Holidays
    {
        public int HolidayId { get; set; }
        public string HolidayName { get; set; }
        public DateTime StartDate { get; set; }
        public int Duration { get; set; }
    }
}
