using BusinessApp.Models;
using BusinessApp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessApp.Controllers
{
    public class StatisticsController
    {
        public async Task<Company> GetCompany(string companyNumber)
        {
            FirebaseHelper helper = new FirebaseHelper();
            return await helper.GetCompany(companyNumber);
        }

        public async Task<List<Order>> GetOrders(Company company)
        {
            FirebaseHelper helper = new FirebaseHelper();
            return await helper.GetAllOrders(company.CompanyNumber);
        }

        public async Task<List<StockItem>> GetStockItems(Company company)
        {
            FirebaseHelper helper = new FirebaseHelper();
            return await helper.GetAllStockItems(company.CompanyNumber);
        }

        public async Task<double> CalculateBusinessValue(Company company)
        {
            List<Order> orders = await GetOrders(company);
            List<StockItem> stocks = await GetStockItems(company);

            bool doOrders = true;
            bool doStocks = true;

            if (orders == null)
                doOrders = false;
            if(orders.Count == 0)
                doOrders = false;
            if (stocks == null)            
                doStocks = false;
            if (orders.Count == 0)
                doStocks = false;

            int curYear = DateTime.Now.Year;
            int difference = curYear - company.AccountCreated.Year;

            double cost = 0;
            double earnings = 0;
            double profits = 0;
            double stockValue = 0;

            if (doOrders)
            {                
                for (int i = 0; i < difference + 1; i++)
                {
                    if (orders[i].Date.Year == company.AccountCreated.Year + i)
                    {

                        for (int j = 0; j < orders[i].Items.Count; j++)
                        {
                            if (orders[i].Items[j].Type == ItemType.Basket)
                            {
                                StockItem stock = stocks.Find(a => a.StockNumber == orders[i].Items[j].ItemNumber);
                                cost += stock.Cost * orders[i].Items[j].Quantity;
                                earnings += stock.Price * orders[i].Items[j].Quantity;
                            }
                        }
                    }
                }
                profits = earnings - cost;
            }

            if (doStocks)
            {
                for (int i = 0; i < stocks.Count; i++)
                {
                    if (stocks[i].Type == StockType.Item)
                    {
                        stockValue += stocks[i].Cost * stocks[i].Quantity;
                    }
                }
            }

            return Math.Round(profits + stockValue, 2);
        }
    }
}
