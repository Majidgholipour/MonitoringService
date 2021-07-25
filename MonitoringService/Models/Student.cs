using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MonitoringService.Models
{
    public class Student
    {
        [DisplayName("شماره")]

        public int StudentID { get; set; }
        [DisplayName("نام")]
        public string StudentName { get; set; }
        [DisplayName("زمان")]
        public DateTime DOB { get; set; }
        [DisplayName("وزن")]
        public int Weight { get; set; }
    }
}