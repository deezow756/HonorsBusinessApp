using BusinessApp.Models;
using BusinessApp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessApp.Controllers
{
    public class StocksController
    {
        public async Task<List<StockItem>> GetStocks(Company company)
        {
            FirebaseHelper helper = new FirebaseHelper();
            List<StockItem> items = await helper.GetAllStockItems(company.CompanyNumber);

            if (items != null)
            { return items; }
            else
            { return items = new List<StockItem>(); }
        }

        public List<StockItem> GetStockInCategory(List<StockItem> items, string category)
        {
            List<StockItem> lstItems = new List<StockItem>();
            for (int i = 0; i < items.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(category))
                {
                    if (string.IsNullOrWhiteSpace(items[i].Catergory))
                    {
                        lstItems.Add(items[i]);
                    }
                }
                else
                {
                    if (items[i].Catergory == category)
                    {
                        lstItems.Add(items[i]);
                    }
                }
            }

            return lstItems;
        }

        public async Task<bool> DeleteItems(User user, Company company, List<StockItem> items)
        {
            string message = "Are You Sure You Want To Delete Selected Items?";

            for (int i = 0; i < items.Count; i++)
            {
                if (!string.IsNullOrEmpty(items[i].Catergory))
                {
                    message += "\n" + items[i].Name + " In Catergory " + items[i].Catergory;
                }
                else
                {
                    message += "\n" + items[i].Name;
                }
            }

            var result = await Dialog.Show("Warning", message, "Yes", "No");
            if (result)
            {
                await CreateDeleteLog(user, company, items);
                FirebaseHelper helper = new FirebaseHelper();
                await helper.DeleteStockItems(items);
                return true;
            }
            else
            { return false; }
        }

        private async Task CreateDeleteLog(User user, Company company, List<StockItem> items)
        {
            StockLog log = new StockLog() { Name = user.Name, Email = user.Email, Date = DateTime.Now, Message = "", LogType = StockLogType.StockDelete };

            log.Message += log.LogTypeString + "\n" +
                "#" + user.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).EmployeeNumber + " " + user.Name;

            for (int i = 0; i < items.Count; i++)
            {
                if (!string.IsNullOrEmpty(items[i].Catergory))
                {
                    log.Message += "\nDeleted: " + items[i].Name + " In Catergory " + items[i].Catergory;
                }
                else
                {
                    log.Message += "\nDeleted: " + items[i].Name;
                }
            }

            FirebaseHelper helper = new FirebaseHelper();
            await helper.AddNewStockLog(company.CompanyNumber, log);
        }

        public async void DisplayHelp()
        {
            await Dialog.Show("Help", "You can refresh the page by clicking refresh in the top right corner\n\n" +
                "To delete stock/stocks you can click delete and then select the stocks you want to delete and then click delete again and then ok, to exit delete mode you can click the back button or deselect the stocks and click delete", "Ok");
        }
    }
}
