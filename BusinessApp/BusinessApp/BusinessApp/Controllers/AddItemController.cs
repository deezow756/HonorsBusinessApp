using BusinessApp.Models;
using BusinessApp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessApp.Controllers
{
    public class AddItemController
    {
        public async Task<List<StockItem>> GetAllStockItems(Company company, List<ItemListEntry> exclude)
        {
            FirebaseHelper helper = new FirebaseHelper();
            List<StockItem> items = await helper.GetAllStockItems(company.CompanyNumber);

            for (int i = 0; i < exclude.Count; i++)
            {
                if (exclude[i].Type == ItemType.Basket)
                {
                    items.RemoveAll(a => a.StockNumber == exclude[i].ItemNumber);
                }
            }

            return items;
        }
        public List<ItemListEntry> GetLabourList(Company company, List<ItemListEntry> exclude)
        {
            List<ItemListEntry> items = new List<ItemListEntry>();            
            for (int i = 0; i < company.Employees.Count; i++)
            {
                if (company.Employees[i].CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).Access < 3)
                {
                    ItemListEntry item = new ItemListEntry();
                    item.Name = company.Employees[i].Name;
                    item.Amount = company.Employees[i].CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).HourlyRate;
                    item.ItemNumber = company.Employees[i].Email;
                    items.Add(item);
                }
            }
            for (int i = 0; i < exclude.Count; i++)
            {
                if (exclude[i].Type == ItemType.Labour)
                {
                    items.RemoveAll(a => a.ItemNumber == exclude[i].ItemNumber);
                }
            }
            return items;
        }

        public async Task<List<StockItem>> GetStockItems(Company company, string category, List<ItemListEntry> exclude)
        {
            List<StockItem> items = new List<StockItem>();
            FirebaseHelper helper = new FirebaseHelper();
            List<StockItem> stockItems = await helper.GetAllStockItems(company.CompanyNumber);

            for (int i = 0; i < exclude.Count; i++)
            {
                if (exclude[i].Type == ItemType.Basket)
                {
                    items.RemoveAll(a => a.StockNumber == exclude[i].ItemNumber);
                }
            }

            for (int i = 0; i < stockItems.Count; i++)
            {
                if(stockItems[i].Catergory == category)
                {
                    items.Add(stockItems[i]);
                }
            }

            return items;
        }

        public async Task<StockItem> GetStock(Company company, string itemNumber)
        {
            FirebaseHelper helper = new FirebaseHelper();
            return await helper.GetStockItem(company.CompanyNumber, itemNumber);
        }

        public async Task<ItemListEntry> GetItem(Company company, string email)
        {
            FirebaseHelper helper = new FirebaseHelper();
            User user = await helper.GetUser(email);
            ItemListEntry item = new ItemListEntry();
            item.Name = user.Name;
            item.Amount = user.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).HourlyRate;
            item.ItemNumber = user.Email;
            return item;
        }

        public bool CheckQuantity(string val)
        {
            int temp1;
            try
            {
                temp1 = int.Parse(val);
                if(temp1 < 1)
                {
                    Dialog.Show("Warning", "Quantity Must Be Higher Than 0", "Ok");
                    return false; 
                }
                return true;
            }
            catch (Exception)
            {
                Dialog.Show("Warning", "Quantity Is Not In The Correct Format", "Ok");
                return false;
            }
        }

        public bool CheckPerHour(string val)
        {
            double temp1;
            try
            {
                temp1 = double.Parse(val, System.Globalization.CultureInfo.InvariantCulture);
                return true;
            }
            catch(Exception)
            {
                Dialog.Show("Warning", "Entered Hours Not In Correct Format\nExample: 4.5 = 4 Hours, 30 Minutes", "Ok");
                return false;
            }
        }

        public double Multiply(string val1S, string val2S)
        {
            double temp1 = double.Parse(val1S, System.Globalization.CultureInfo.InvariantCulture);
            double temp2 = double.Parse(val2S, System.Globalization.CultureInfo.InvariantCulture);

            return temp1 * temp2;
        }

        public async void DisplayHelp()
        {
            await Dialog.Show("Help", "By clicking the icon in the top right will change between adding a stock item and specific employee labour\n\n" +
                "Simply select the stock or employee and add either the quantity of the stock or the hours of labour and click add", "Ok");
        }
    }
}
