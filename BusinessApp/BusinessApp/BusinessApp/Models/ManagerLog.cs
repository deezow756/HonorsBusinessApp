using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessApp.Models
{
    public enum ManagerLogType { Request, CompanyEdit, EmployeeEdit}
    public class ManagerLog
    {
        public DateTime Date { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }

        public ManagerLogType LogType { get; set; }

        public string LogTypeString
        {
            get
            {
                string msg = "";
                switch (LogType)
                {
                    case ManagerLogType.Request:
                        msg = "Request";
                        break;
                    case ManagerLogType.CompanyEdit:
                        msg = "Company Edit";
                        break;
                    case ManagerLogType.EmployeeEdit:
                        msg = "Employee Edit";
                        break;
                    default:
                        break;
                }
                return msg;
            }
        }

        public string DateString
        {
            get
            {
                if (Date != null)
                {
                    return Date.ToString();
                }
                else
                    return "";
            }
        }
    }
}
