using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessApp.Models
{
    public class CompanyID
    {
        public string CompanyNumber { get; set; }
        public string EmployeeNumber { get; set; }
        public int Access { get; set; }
        public Role CurrentRole { get; set; }
        public double HourlyRate { get; set; }
        public bool Approved { get; set; }
    }
}
