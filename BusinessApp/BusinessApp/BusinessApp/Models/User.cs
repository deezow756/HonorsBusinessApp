using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessApp.Models
{
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public List<string> CompanyIDs { get; set; }
        public int Access { get; set; }
        public bool Accepted { get; set; }
        public Role CurrentRole { get; set; }
        public DateTime AccountCreated { get; set; }


        public User(string firstName, string surname, string email, string password)
        {
            FirstName = firstName;
            Surname = surname;
            Email = email;
            Password = password;
            CurrentRole = null;
            Access = 0;
            CompanyIDs = new List<string>();
            AccountCreated = DateTime.Now;
            Accepted = false;            
        }
    }
}
