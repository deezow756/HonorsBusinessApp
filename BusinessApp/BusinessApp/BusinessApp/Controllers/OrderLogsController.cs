using BusinessApp.Models;
using BusinessApp.Utilities;
using BusinessApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessApp.Controllers
{
    public class OrderLogsController
    {
        public async Task<List<OrderLog>> GetAllOrderLogs(Company company)
        {
            FirebaseHelper helper = new FirebaseHelper();
            return await helper.GetAllOrderLogs(company.CompanyNumber);
        }

        public List<OrderLog> FilterList(DateMode mode, List<OrderLog> logs, int number)
        {
            if (mode == DateMode.Month)
            {
                logs.RemoveAll(a => a.Date.Month != number);
            }
            else if (mode == DateMode.Year)
            {
                logs.RemoveAll(a => a.Date.Year != number);
            }

            return logs;
        }

        public List<OrderLog> FilterList(List<OrderLog> logs, string search)
        {
            List<OrderLog> lstLogs = new List<OrderLog>();
            for (int i = 0; i < logs.Count; i++)
            {
                if (logs[i].Message.ToLower().Contains(search))
                {
                    lstLogs.Add(logs[i]);
                }
            }

            return lstLogs;
        }

        public async void DisplayHelp()
        {
            await Dialog.Show("Help", "You can filter order logs by month, year, and search\n\n" +
                "Month and Year: clicking the tick box will enable and disable the month or year filter\n\n" +
                "Search: simply start typing and the order logs will automatically start being filtered by what you are entering", "Ok");
        }
    }
}
