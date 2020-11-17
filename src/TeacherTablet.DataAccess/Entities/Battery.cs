using System;
using System.Collections.Generic;
using System.Text;

namespace TeacherTablet.DataAccess.Entities
{
    public class Battery
    {
        public int AcademyId { get; set; }

        public decimal BatteryLevel { get; set; }

        public string EmployeeId { get; set; }

        public string SerialNumber { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
