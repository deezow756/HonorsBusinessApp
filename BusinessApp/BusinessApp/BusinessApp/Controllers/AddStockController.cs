using BusinessApp.Models;
using BusinessApp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessApp.Controllers
{
    public class AddStockController
    {
        public bool CheckDetails(string name, string price, string cost, string quantity, string description)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                Dialog.Show("Warning", "Please Enter A Name For The Item", "Ok");
                return false;
            }
            if (string.IsNullOrWhiteSpace(price))
            {
                Dialog.Show("Warning", "Please Enter A Price For The Item", "Ok");
                return false;
            }
            if (string.IsNullOrWhiteSpace(cost))
            {
                Dialog.Show("Warning", "Please Enter A Cost For The Item", "Ok");
                return false;
            }
            if (string.IsNullOrWhiteSpace(quantity))
            {
                Dialog.Show("Warning", "Please Enter A Quantity For The Item", "Ok");
                return false;
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                Dialog.Show("Warning", "Please Enter A Description For The Item", "Ok");
                return false;
            }
            try
            {
                double temp = double.Parse(price, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                Dialog.Show("Warning", "Please Enter A Valid Price", "Ok");
                return false;
            }
            try
            {
                double temp = double.Parse(cost, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                Dialog.Show("Warning", "Please Enter A Valid Cost", "Ok");
                return false;
            }
            try
            {
                int temp = int.Parse(quantity);
            }
            catch
            {
                Dialog.Show("Warning", "Please Enter A Valid Quantity", "Ok");
                return false;
            }
            return true;
        }

        public bool CheckDetails(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Dialog.Show("Warning", "Please Enter A Name For The Item", "Ok");
                return false;
            }
            return true;
        }

        public async Task AddStockItem(User user, Company company, string name, string price, string cost, string quantity, string description, string category)
        {
            StockItem stockItem = new StockItem()
            {
                Name = name,
                Price = double.Parse(price, System.Globalization.CultureInfo.InvariantCulture),
                Cost = double.Parse(cost, System.Globalization.CultureInfo.InvariantCulture),
                Quantity = int.Parse(quantity),
                Description = description,
                Catergory = category,
                CompanyNumber = company.CompanyNumber,
                Type = StockType.Item,
                Date = DateTime.Now,
                Active = false
            };
            stockItem.StockNumber = await GenerateRandomStockNumber(company);

            await CreateStockLog(user, company, stockItem);

            FirebaseHelper helper = new FirebaseHelper();
            await helper.AddNewStockItem(stockItem);
        }

        public async Task AddStockCategory(User user, Company company, string name, string category)
        {
            StockItem stockItem = new StockItem()
            {
                Name = name,
                Catergory = category,
                CompanyNumber = company.CompanyNumber,
                Type = StockType.Category
            };
            stockItem.StockNumber = await GenerateRandomStockNumber(company);

            await CreateStockLog(user, company, stockItem);

            FirebaseHelper helper = new FirebaseHelper();
            await helper.AddNewStockItem(stockItem);
        }

        private async Task CreateStockLog(User user, Company company, StockItem stockItem)
        {
            StockLog log = new StockLog() { Date = DateTime.Now, Email = user.Email, Name = user.Name, Message = "", LogType = StockLogType.StockAdd };

            log.Message += log.LogTypeString + "\n" +
                "#" + user.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).EmployeeNumber + " " + user.Name + "\n";
               
            if(stockItem.Type == StockType.Item)
            {
                log.Message += "Added A New StockItem: " + stockItem.StockNumber;
                if(!string.IsNullOrWhiteSpace(stockItem.Catergory))
                { log.Message += " In Category: " + stockItem.Catergory; }

                log.Message += "\nName: " + stockItem.Name +
                    "\nPrice: " + stockItem.Price +
                    "\nCost: " + stockItem.Cost +
                    "\nQuantity: " + stockItem.Quantity +
                    "\nDescription: " + stockItem.Description;
;           }
            else
            {
                log.Message += "Added A New Category: " + stockItem.Name;
                if (!string.IsNullOrWhiteSpace(stockItem.Catergory))
                { log.Message += " In Category: " + stockItem.Catergory; }
            }

            FirebaseHelper helper = new FirebaseHelper();
            await helper.AddNewStockLog(company.CompanyNumber, log);
        }

        private async Task<string> GenerateRandomStockNumber(Company company)
        {
            string randomString;
            bool match = false;
            do
            {
                match = false;
                randomString = RandomGenerator.GenerateString(8);

                FirebaseHelper helper = new FirebaseHelper();
                List<StockItem> stocks = await helper.GetAllStockItems(company.CompanyNumber);

                if (stocks != null)
                {
                    if (stocks.Count > 0)
                    {
                        for (int i = 0; i < stocks.Count; i++)
                        {
                            if (stocks[i].StockNumber == randomString)
                            {
                                match = true;
                                break;
                            }
                        }
                    }
                }
            } while (match);


            return randomString;
        }
    }
}
