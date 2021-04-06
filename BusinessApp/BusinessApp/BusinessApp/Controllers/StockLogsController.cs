using BusinessApp.Models;
using BusinessApp.Utilities;
using BusinessApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessApp.Controllers
{
    public class StockLogsController
    {
        public async Task<List<StockLog>> GetAllStockLogs(Company company)
        {
            FirebaseHelper helper = new FirebaseHelper();
            return await helper.GetAllStockLogs(company.CompanyNumber);
        }

        public List<StockLog> FilterList(DateMode mode, List<StockLog> logs, int number)
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

        public List<StockLog> FilterList(List<StockLog> logs, string search)
        {
            List<StockLog> lstLogs = new List<StockLog>();
            for (int i = 0; i < logs.Count; i++)
            {
                if(logs[i].Message.ToLower().Contains(search))
                {
                    lstLogs.Add(logs[i]);
                }
            }

            return lstLogs;
        }

        public async void DisplayHelp()
        {
            await Dialog.Show("Help", "You can filter stock logs by month, year, and search\n\n" +
                "Month and Year: clicking the tick box will enable and disable the month or year filter\n\n" +
                "Search: simply start typing and the stock logs will automatically start being filtered by what you are entering", "Ok");
        }
    }
}
