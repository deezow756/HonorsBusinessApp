﻿using BusinessApp.Models;
using BusinessApp.Utilities;
using BusinessApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BusinessApp.Controllers
{
    public class ManagerLogsController
    {
        public async Task<List<ManagerLog>> GetAllManagerLogs(Company company)
        {
            FirebaseHelper helper = new FirebaseHelper();
            return await helper.GetAllManagerLogs(company.CompanyNumber);
        }

        public List<ManagerLog> FilterList(DateMode mode, List<ManagerLog> logs, int number)
        {
            if (mode == DateMode.Month)
            {
                logs.RemoveAll(a => a.Date.Month != number);
            }
            else if(mode == DateMode.Year)
            {
                logs.RemoveAll(a => a.Date.Year != number);
            }

            return logs;
        }

        public List<ManagerLog> FilterList(List<ManagerLog> logs, string search)
        {
            List<ManagerLog> lstLogs = new List<ManagerLog>();
            for (int i = 0; i < logs.Count; i++)
            {
                if (logs[i].Message.ToLower().Contains(search))
                {
                    lstLogs.Add(logs[i]);
                }
            }

            return lstLogs;
        }
    }
}
