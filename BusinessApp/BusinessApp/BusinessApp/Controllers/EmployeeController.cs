using BusinessApp.Models;
using BusinessApp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessApp.Controllers
{
    public class EmployeeController
    {
        public async Task<User> GetUser(string email)
        {
            FirebaseHelper helper = new FirebaseHelper();
            return await helper.GetUser(email);
        }
        public List<string> GetRolesString(List<Role> roles)
        {
            List<string> sRoles = new List<string>();

            foreach (var role in roles)
            {
                sRoles.Add(role.Name);
            }

            return sRoles;
        }

        public async Task SaveChanges(User editor, User user, Company company)
        {
            FirebaseHelper helper = new FirebaseHelper();
            await CreateLog(editor, await helper.GetUser(user.Email), user, company);
            company.Employees.RemoveAll(a => a.AccountCreated == user.AccountCreated && a.Email == user.Email);
            company.Employees.Add(user);

            
            await helper.UpdateUser(user.Email, user);
            await helper.UpdateCompany(company);
        }

        public async Task<bool> AreYouSure(string title, string msg, string btnYes, string btnNo)
        {
            return await Dialog.Show(title, msg, btnYes, btnNo);
        }

        public async Task RemoveEmployee(User editor, User user, Company company)
        {
            await CreateRemoveLog(editor, user, company);
            company.Employees.RemoveAll(a => a.AccountCreated == user.AccountCreated);
            user.CompanyIDs.RemoveAll(a => a.CompanyNumber == company.CompanyNumber);

            FirebaseHelper helper = new FirebaseHelper();
            await helper.UpdateUser(user.Email, user);
            await helper.UpdateCompany(company);
        }

        public async Task CreateLog(User editor, User oldUser, User newUser, Company company)
        {
            bool changeMade = false;
            ManagerLog log = new ManagerLog() { Date = DateTime.Now, Email = editor.Email, Name = editor.Name, Message = "", LogType = ManagerLogType.EmployeeEdit };

            log.Message += log.LogTypeString + "\n" +
                "#" + editor.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).EmployeeNumber + " " + editor.Name;

            CompanyID oldID = oldUser.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber);
            CompanyID newID = newUser.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber);

            int old = oldID.Access;
            int New = newID.Access;

            if (oldID.CurrentRole.Name != newID.CurrentRole.Name)
            {
                changeMade = true;
                log.Message += "\nChanged Employees Company Role From: " + oldID.CurrentRole.Name + " To: " + newID.CurrentRole.Name;
            }

            if (oldID.Access != newID.Access)
            {
                changeMade = true;
                string oldTemp;
                string newTemp;
                if(oldID.Access == 1) { oldTemp = "No"; }
                else { oldTemp = "Yes"; }
                if (newID.Access == 1) { newTemp = "No"; }
                else { newTemp = "Yes"; }
                log.Message += "\nChanged Employees Manager Access From: " + oldTemp + " To: " + newTemp;
            }

            if (changeMade)
            {
                FirebaseHelper helper = new FirebaseHelper();
                await helper.AddNewManagerLog(company.CompanyNumber, log);
            }
        }

        public async Task CreateRemoveLog(User editor, User user, Company company)
        {
            ManagerLog log = new ManagerLog() { Date = DateTime.Now, Email = editor.Email, Name = editor.Name, Message = "", LogType = ManagerLogType.EmployeeEdit };

            log.Message += log.LogTypeString + "\n" +
                "#" + editor.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).EmployeeNumber + " " + editor.Name + "\n" +
                "Removed Employee From Company: " + user.Name + " " + user.Email;

            FirebaseHelper helper = new FirebaseHelper();
            await helper.AddNewManagerLog(company.CompanyNumber, log);
        }

        public bool CheckHourlyRate(string val)
        {
            try
            {
                double temp = double.Parse(val, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                Dialog.Show("Wanring", "Please Enter A Valid Hourly Rate\nExample: 7.85", "Ok");
                return false;
            }
            return true;
        }
    }
}
