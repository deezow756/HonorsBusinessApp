using BusinessApp.Utilities;
using BusinessApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessApp.Controllers
{
    public class StockInfoController
    {
        public async Task<StockItem> GetStockItem(StockItem item)
        {
            FirebaseHelper helper = new FirebaseHelper();
            return await helper.GetStockItem(item.CompanyNumber, item.StockNumber);
        }
        public async Task<bool> StockItemCheck(StockItem item)
        {
            FirebaseHelper helper = new FirebaseHelper();
            StockItem temp = await helper.GetStockItem(item.CompanyNumber, item.StockNumber);

            if(temp != null)
            {
                if(temp.Active)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }
        public async Task SetActive(StockItem item, bool state)
        {
            item.Active = state;
            FirebaseHelper helper = new FirebaseHelper();
            await helper.UpdateStockItem(item);
        }
        public async Task<StockItem> GetStockItem(string companyNumber, string stockNumber)
        {
            FirebaseHelper helper = new FirebaseHelper();
            StockItem temp = await helper.GetStockItem(companyNumber, stockNumber);
            if(temp != null)
            {
                return temp;
            }
            else
            {
                return null;
            }
        }

        public async Task SaveChanges(User user, Company company, StockItem item, int amountChanged)
        {
            item.Quantity += amountChanged;
            await CreateStockLog(user, company, item, amountChanged);
            FirebaseHelper helper = new FirebaseHelper();
            await helper.UpdateStockItem(item);
        }

        public async Task CreateStockLog(User user, Company company, StockItem item, int amountChanged)
        {
            StockLog log = new StockLog() { Date = DateTime.Now, Email = user.Email, Name = user.Name, Message = "", LogType = StockLogType.StockEdit };

            log.Message += log.LogTypeString + "\n" +
                "#" + user.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).EmployeeNumber + " " + user.Name + "\n" +
                "Edited Stock Item: " + item.StockNumber + " " + item.Name + "\n" +
                "Quantity From: " + (item.Quantity - amountChanged).ToString() + " To " + item.Quantity.ToString();

            FirebaseHelper helper = new FirebaseHelper();
            await helper.AddNewStockLog(company.CompanyNumber, log);
        }
    }
}
