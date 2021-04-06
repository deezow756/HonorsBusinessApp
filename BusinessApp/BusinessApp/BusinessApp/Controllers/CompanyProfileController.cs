using BusinessApp.Models;
using BusinessApp.Utilities;
using BusinessApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessApp.Controllers
{
    public class CompanyProfileController
    {
        public async Task<Company> GetCompany(string companyNumber)
        {
            FirebaseHelper helper = new FirebaseHelper();
            return await helper.GetCompany(companyNumber);
        }

        public async Task<bool> AreYouSure(string title, string msg, string btnYes, string btnNo)
        {
            return await Dialog.Show(title, msg, btnYes, btnNo);
        }

        public async Task DeleteRole(User editor, Company company, Role role)
        {
            await CreateDeleteRoleLog(editor, company, role);
            FirebaseHelper helper = new FirebaseHelper();

            for (int i = 0; i < company.Employees.Count; i++)
            {
                if(company.Employees[i].CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).CurrentRole.Name.Equals(role.Name))
                {
                    company.Employees[i].CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).CurrentRole = new Role() { Name = "Please Add Role" };
                    await helper.UpdateUser(company.Employees[i].Email, company.Employees[i]);
                }
            }

            await helper.UpdateCompany(company);
        }

        public async Task AddRole(User editor, Company company, string role)
        {
            await CreateAddRoleLog(editor, company, role);
            company.Roles.Add(new Role() { Name = role });
            FirebaseHelper helper = new FirebaseHelper();
            await helper.UpdateCompany(company);
        }

        public async Task SaveChanges(User editor, Company company)
        {
            FirebaseHelper helper = new FirebaseHelper();
            Company oldCompany = await helper.GetCompany(company.CompanyNumber);
            await CreateLog(editor, oldCompany, company);
            
            await helper.UpdateCompany(company);
        }

        public async Task CreateDeleteRoleLog(User editor, Company company, Role role)
        {
            ManagerLog log = new ManagerLog() { Date = DateTime.Now, Email = editor.Email, Name = editor.Name, Message = "", LogType = ManagerLogType.CompanyEdit };

            log.Message += log.LogTypeString + "\n" +
                "#" + editor.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).EmployeeNumber + " " + editor.Name + "\n" +
                "Deleted Company Role: " + role.Name;

            FirebaseHelper helper = new FirebaseHelper();
            await helper.AddNewManagerLog(company.CompanyNumber, log);
        }

        public async Task CreateAddRoleLog(User editor, Company company, string role)
        {
            ManagerLog log = new ManagerLog() { Date = DateTime.Now, Email = editor.Email, Name = editor.Name, Message = "", LogType = ManagerLogType.CompanyEdit };

            log.Message += log.LogTypeString + "\n" +
                "#" + editor.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).EmployeeNumber + " " + editor.Name + "\n" +
                "Added Company Role: " + role;

            FirebaseHelper helper = new FirebaseHelper();
            await helper.AddNewManagerLog(company.CompanyNumber, log);
        }

        public async Task CreateLog(User editor, Company oldCompany, Company newCompany)
        {
            bool changedMade = false;
            ManagerLog log = new ManagerLog() { Date = DateTime.Now, Email = editor.Email, Name = editor.Name, Message = "", LogType = ManagerLogType.CompanyEdit };

            log.Message += log.LogTypeString + "\n" +
                "#" + editor.CompanyIDs.Find(a => a.CompanyNumber == newCompany.CompanyNumber).EmployeeNumber + " " + editor.Name;

            if (oldCompany.Name != newCompany.Name)
            {
                changedMade = true;
                log.Message += "\nChanged Company Name From: " + oldCompany.Name + " To: " + newCompany.Name;
            }

            if (changedMade)
            {
                FirebaseHelper helper = new FirebaseHelper();
                await helper.AddNewManagerLog(newCompany.CompanyNumber, log);
            }
        }

        public async void DisplayHelp(Mode mode)
        {
            if(mode == Mode.View)
            {
                await Dialog.Show("Help", "Click the pencil icon to enter edit mode", "Ok");
            }
            else
            {
                await Dialog.Show("Help", "Click the pencil icon to exit edit mode\n\n" +
                    "To remove roles from the list simply click on the remove and click ok to remove it\n" +
                    "To add a new role, enter the name of the role in the text box under the list and click add role\n\n" +
                    "When finshed click save to save the changes made", "Ok");
            }
        }
    }
}
