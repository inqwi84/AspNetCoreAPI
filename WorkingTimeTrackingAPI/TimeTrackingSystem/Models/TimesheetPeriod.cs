using System;
using System.ComponentModel.DataAnnotations;

namespace TimeTrackingSystem.Models
{
    public class TimesheetPeriod
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime? DateFrom { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH.mm}")]
        public DateTime? DateTo { get; set; }
    }
}
