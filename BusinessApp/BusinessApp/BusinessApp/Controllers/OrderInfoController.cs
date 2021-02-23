using BusinessApp.Models;
using BusinessApp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessApp.Controllers
{
    public class OrderInfoController
    {
        public async Task<bool> AreYouSure(string title, string msg, string btnYes, string btnNo)
        {
            return await Dialog.Show(title, msg, btnYes, btnNo);
        }
        public async Task ChangePaidStatus(Company company, Order order)
        {
            order.Paid = true;
            FirebaseHelper helper = new FirebaseHelper();
            await helper.UpdateOrder(order);
        }

        public async Task<Order> GetOrder(User user, Company company, Order order)
        {
            await CreateLog(user, company, order);
            FirebaseHelper helper = new FirebaseHelper();
            return await helper.GetOrder(company.CompanyNumber, order.OrderNumber);
        }

        private async Task CreateLog(User user, Company company, Order order)
        {
            OrderLog log = new OrderLog() { Date = DateTime.Now, Email = user.Email, Name = user.Name, Message = "", LogType = OrderLogType.OrderEdit };

            log.Message += log.LogTypeString + "\n" +
                "#" + user.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).EmployeeNumber + " " + user.Name + "\n";

            log.Message += "Changed Paid Status To : " + order.PaidString;            

            FirebaseHelper helper = new FirebaseHelper();
            await helper.AddNewOrderLog(company.CompanyNumber, log);
        }

        public async Task DeleteOrder(User user, Company company, Order order, bool ReturnStock = false)
        {
            FirebaseHelper helper = new FirebaseHelper();
            if(ReturnStock)
            {
                for (int i = 0; i < order.Items.Count; i++)
                {
                    if(order.Items[i].Type == ItemType.Basket)
                    {
                        StockItem stock = await helper.GetStockItem(company.CompanyNumber, order.Items[i].ItemNumber);
                        if(stock != null)
                        {
                            stock.Quantity += int.Parse(order.Items[i].Quantity.ToString());
                            await helper.UpdateStockItem(stock);
                        }
                    }
                }
            }

            await CreateDeleteLog(user, company, order);

            await helper.DeleteOrder(new List<Order>() { order });
        }

        private async Task CreateDeleteLog(User user, Company company, Order order)
        {
            OrderLog log = new OrderLog() { Date = DateTime.Now, Email = user.Email, Name = user.Name, Message = "", LogType = OrderLogType.OrderDelete };

            log.Message += log.LogTypeString + "\n" +
                "#" + user.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).EmployeeNumber + " " + user.Name + "\n";

            log.Message += "Deleted Order: " + order.OrderNumber;
            log.Message += "\nDate: " + order.Date;
            if (!string.IsNullOrWhiteSpace(order.ContactNumber))
            {
                log.Message += "\nContact Number: " + order.ContactNumber;
            }
            if (!string.IsNullOrWhiteSpace(order.Email))
            {
                log.Message += "\nEmail: " + order.Email;
            }
            log.Message += "\nItems:";
            for (int i = 0; i < order.Items.Count; i++)
            {
                log.Message += "\nName: " + order.Items[i].Name +
                    " Type: " + order.Items[i].TypeString + " " +
                    order.Items[i].QuantityType + order.Items[i].QuantityString +
                    " Total: " + order.Items[i].TotalPriceString;
            }

            FirebaseHelper helper = new FirebaseHelper();
            await helper.AddNewOrderLog(company.CompanyNumber, log);
        }
    }
}
