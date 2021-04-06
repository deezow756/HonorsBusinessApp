using BusinessApp.Models;
using BusinessApp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessApp.Controllers
{
    public class EmployeeRequestController
    {
        public List<string> GetRolesString(List<Role> roles)
        {
            List<string> sRoles = new List<string>();

            foreach (var role in roles)
            {
                sRoles.Add(role.Name);
            }

            return sRoles;
        }

        public async Task<bool> AreYouSure()
        {
            return await Dialog.Show("Warning", "Are You Sure You Want To Reject The Request", "Yes", "No");
        }

        public async Task RejectRequest(User editor, User user, Company company)
        {
            await CreateRejectLog(editor, user, company);
            user.CompanyIDs.RemoveAll(a => a.CompanyNumber == company.CompanyNumber);
            company.Employees.RemoveAll(a => a.AccountCreated == user.AccountCreated);

            FirebaseHelper helper = new FirebaseHelper();
            await helper.UpdateUser(user.Email, user);
            await helper.UpdateCompany(company);
        }

        public async Task AcceptRequest(User editor, User user, Company company)
        {
            await CreateAcceptLog(editor, user, company);
            user.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).Approved = true;
            company.Employees.RemoveAll(a => a.AccountCreated == user.AccountCreated && a.Email == user.Email);
            company.Employees.Add(user);

            FirebaseHelper helper = new FirebaseHelper();
            await helper.UpdateUser(user.Email, user);
            await helper.UpdateCompany(company);
        }

        public bool CheckHourlyRate(string val)
        {
            try
            {
                double temp = double.Parse(val, System.Globalization.CultureInfo.InvariantCulture);
                if (temp < 0)
                {
                    Dialog.Show("Warning", "Quantity Must Be 0 Or Higher", "Ok");
                    return false;
                }
            }
            catch(Exception)
            {
                Dialog.Show("Wanring", "Please Enter A Valid Hourly Rate\nExample: 7.85", "Ok");
                return false;
            }
            return true;
        }

        public bool CheckCompanyID(CompanyID companyID)
        {
            if(companyID.CurrentRole == null)
            {
                Dialog.Show("Warning", "Please Select A Role", "Ok");
                return false;
            }

            if(companyID.Access == 0)
            {
                Dialog.Show("Warning", "Please Select An Access", "Ok");
                return false;
            }

            return true;
        }

        private async Task CreateAcceptLog(User editor, User user, Company company)
        {
            ManagerLog log = new ManagerLog() { Date = DateTime.Now, Email = editor.Email, Name = editor.Name, Message = "" , LogType = ManagerLogType.Request};

            log.Message += log.LogTypeString + "\n" +
                "#" + editor.CompanyIDs.Find(a=> a.CompanyNumber == company.CompanyNumber).EmployeeNumber + " " + editor.Name + "\n" +
                "Accepted Request From: " + "#" + user.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).EmployeeNumber + " " + user.Name + "\n" +
                "Role: " + user.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).CurrentRole.Name + "\n" +
                "Manage Access: ";
            if(user.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).Access == 1)
            {
                log.Message += "No";
            }
            else
            {
                log.Message += "Yes";
            }

            FirebaseHelper helper = new FirebaseHelper();
            await helper.AddNewManagerLog(company.CompanyNumber, log);
        }

        private async Task CreateRejectLog(User editor, User user, Company company)
        {
            ManagerLog log = new ManagerLog() { Date = DateTime.Now, Email = editor.Email, Name = editor.Name, Message = "", LogType = ManagerLogType.Request };

            log.Message += log.LogTypeString + "\n" +
                "#" + editor.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).EmployeeNumber + " " + editor.Name + "\n" +
                "Rejected Request From: " + user.Name + " " + user.Email;

            FirebaseHelper helper = new FirebaseHelper();
            await helper.AddNewManagerLog(company.CompanyNumber, log);
        }

        public async void Displayhelp()
        {

            await Dialog.Show("Help", "To decline the request click the cross icon\n\n" +
                "To accect the request fill in the empty boxes and click the tick icon", "Ok");
        }
    }
}
