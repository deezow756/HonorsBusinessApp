using BusinessApp.Models;
using BusinessApp.Utilities;
using BusinessApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BusinessApp.Controllers
{
    public class OverallController
    {
        public async Task<List<Order>> GetOrders(Company company)
        {
            FirebaseHelper helper = new FirebaseHelper();
            return await helper.GetAllOrders(company.CompanyNumber);
        }

        public async Task<List<StockItem>> GetStocks(Company company)
        {
            FirebaseHelper helper = new FirebaseHelper();
            return await helper.GetAllStockItems(company.CompanyNumber);
        }

        public void RefreshDetails(OverallSection section, Label txtCost, Label txtEarnings, Label txtProfit, List<Order> orders, List<StockItem> stocks, int month, int year)
        {
            List<Order> Orders = new List<Order>();

            if (section == OverallSection.Month)
            {
                for (int i = 0; i < orders.Count; i++)
                {
                    if (orders[i].Date.Year == year)
                    {
                        if (orders[i].Date.Month == month)
                        {
                            Orders.Add(orders[i]);
                        }
                    }
                }
                
            }
            else
            {
                for (int i = 0; i < orders.Count; i++)
                {
                    if (orders[i].Date.Year == year)
                    {
                        Orders.Add(orders[i]);
                    }
                }                
            }

            double cost = 0;
            double earnings = 0;
            for (int i = 0; i < Orders.Count; i++)
            {
                if (Orders[i].Paid)
                {
                    for (int j = 0; j < Orders[i].Items.Count; j++)
                    {
                        if (Orders[i].Items[j].Type == ItemType.Basket)
                        {
                            StockItem stock = stocks.Find(a => a.StockNumber == Orders[i].Items[j].ItemNumber);
                            cost += stock.Cost * Orders[i].Items[j].Quantity;
                            earnings += stock.Price * Orders[i].Items[j].Quantity;
                        }
                        else
                        {
                            earnings += Orders[i].Items[j].TotalPrice;
                        }
                    }
                }
            }

            txtCost.Text = cost.ToString();
            txtEarnings.Text = earnings.ToString();
            txtProfit.Text = (earnings - cost).ToString();
        }
    }
}
