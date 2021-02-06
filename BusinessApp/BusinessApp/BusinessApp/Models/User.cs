using System;
using System.Collections.Generic;
using System.Text;
using Firebase.Database;

namespace BusinessApp.Models
{
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public List<CompanyID> CompanyIDs { get; set; }     
        
        public DateTime AccountCreated { get; set; }
        
        public string Name
        {
            get { return FirstName + " " + Surname; }
        }

        public User() { }
        public User(string firstName, string surname, string email, string password)
        {
            FirstName = firstName;
            Surname = surname;
            Email = email;
            Password = password;
            CompanyIDs = new List<CompanyID>();
            AccountCreated = DateTime.Now;         
        }
    }
}
