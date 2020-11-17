using System;
using System.Collections.Generic;
using System.Text;

namespace TeacherTablet.Business.Models
{
    public class BatteryUsage
    {
        public string SerialNumber { get; set; }

        public string AverageDailyBatteryUsage { get; set; }

        public bool NeedsReplacement { get; set; }
    }
}
