using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessApp.Models
{
    public class Company
    {
        public string Name { get; set; }
        public string CompanyID { get; set; }
        public List<User> Employees { get; set; }
        public List<Role> Roles { get; set; }
        public DateTime AccountCreated { get; set; }
    }
}
