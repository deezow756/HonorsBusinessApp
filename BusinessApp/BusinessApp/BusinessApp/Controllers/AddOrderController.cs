using BusinessApp.Utilities;
using BusinessApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BusinessApp.Controllers
{
    public class AddOrderController
    {
        public async Task<StockItem> GetStockItem(Company company, string itemNumber)
        {
            FirebaseHelper helper = new FirebaseHelper();
            return await helper.GetStockItem(company.CompanyNumber, itemNumber);
        }
        public async Task<bool> CheckValues(Company company, List<ItemListEntry> items, string phoneNumber, string email )
        {
            if(items.Count == 0)
            {
                Dialog.Show("Warning", "You Must Have At Least 1 Item Added", "Ok");
                return false;
            }

            FirebaseHelper helper = new FirebaseHelper();

            List<StockItem> stocks = await helper.GetAllStockItems(company.CompanyNumber);

            for (int i = 0; i < items.Count; i++)
            {
                if(items[i].Type == ItemType.Basket)
                {
                    StockItem stock = stocks.Find(a => a.StockNumber == items[i].ItemNumber);
                    if(items[i].Quantity > stock.Quantity)
                    {
                        Dialog.Show("Warning", stock.Name + ": Only Has " + stock.Quantity + " Left In Stock, You Have Requested: " + items[i].Quantity + 
                            "\nPlease Change The Amount You Are Wanting To The Same As The Stock Or Less" , "Ok");
                        return false;
                    }
                }
            }

            bool checkPhoneNumber = true;
            bool checkEmail = true;

            if(string.IsNullOrWhiteSpace(phoneNumber) && string.IsNullOrWhiteSpace(email))
            {
                Dialog.Show("Warning", "You Must Enter A Phone Number And/Or Email", "Ok");
                return false;
            }
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                checkPhoneNumber = false;
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                checkEmail = false;
            }


            if (checkPhoneNumber)
            {
                try
                {
                    long val = long.Parse(phoneNumber);
                }
                catch (Exception)
                {
                    Dialog.Show("Warning", "Please Enter A Valid Phone Number", "Ok");
                    return false;
                }

                if (phoneNumber.Length == 11)
                {
                    string check = "";
                    for (int i = 0; i < 2; i++)
                    {
                        check += phoneNumber[i];
                    }

                    if (!check.Equals("07"))
                    {
                        Dialog.Show("Warning", "Please Enter A Valid Phone Number", "Ok");
                        return false;
                    }
                }
                else if (phoneNumber.Length == 13)
                {
                    string check = "";
                    for (int i = 0; i < 4; i++)
                    {
                        check += phoneNumber[i];
                    }

                    if (!check.Equals("+447"))
                    {
                        Dialog.Show("Warning", "Please Enter A Valid Phone Number", "Ok");
                        return false;
                    }
                }
                else
                {
                    Dialog.Show("Warning", "Please Enter A Valid Phone Number", "Ok");
                    return false;
                }
            }
            if (checkEmail)
            {
                try
                {
                    var mail = new MailAddress(email);
                    bool isValidEmail = mail.Host.Contains(".");
                    if (!isValidEmail)
                    {
                        Dialog.Show("Warning", "Please Enter A Vaild Email", "Ok");
                        return false;
                    }
                }
                catch (Exception)
                {
                    Dialog.Show("Warning", "Please Enter A Vaild Email", "Ok");
                    return false;
                }
            }

            return true;
        }
        public bool CheckQuantity(string val)
        {
            int temp1;
            try
            {
                temp1 = int.Parse(val);
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
            catch (Exception)
            {
                Dialog.Show("Warning", "Entered Hours Not In Correct Format\nExample: 4.5 = 4 Hours, 30 Minutes", "Ok");
                return false;
            }
        }

        public async Task AddOrder(User user, Company company, List<ItemListEntry> items, string phoneNumber, string email, string totalPrice)
        {
            string temp = "";
            for (int i = 1; i < totalPrice.Length; i++)
            {
                temp += totalPrice[i];
            }
            Order order = new Order()
            {
                Items = items,
                ContactNumber = phoneNumber,
                Email = email,
                Date = DateTime.Now,
                CompanyNumber = company.CompanyNumber,
                TotalPrice = double.Parse(temp, System.Globalization.CultureInfo.InvariantCulture),
                Paid = false
            };

            order.OrderNumber = await GenerateRandomOrderNumber(company);
            await CreateLog(user, company, order);

            FirebaseHelper helper = new FirebaseHelper();

            for (int i = 0; i < items.Count; i++)
            {
                if(items[i].Type == ItemType.Basket)
                {
                    StockItem stock = await helper.GetStockItem(company.CompanyNumber, items[i].ItemNumber);
                    stock.Quantity -= int.Parse(items[i].Quantity.ToString());
                    await helper.UpdateStockItem(stock);
                }
            }
            
            await helper.AddNewOrder(order);
        }

        public async Task CreateLog(User user, Company company, Order order)
        {
            OrderLog log = new OrderLog() { Date = DateTime.Now, Email = user.Email, Name = user.Name, Message = "", LogType = OrderLogType.OrderAdd };

            log.Message += log.LogTypeString + "\n" +
                "#" + user.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).EmployeeNumber + " " + user.Name + "\n";

            log.Message += "Added A New Order: " + order.OrderNumber;
            log.Message += "\nDate: " + order.Date;
            if(!string.IsNullOrWhiteSpace(order.ContactNumber))
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

        private async Task<string> GenerateRandomOrderNumber(Company company)
        {
            string randomString;
            bool match = false;
            do
            {
                match = false;
                randomString = RandomGenerator.GenerateString(8);

                FirebaseHelper helper = new FirebaseHelper();
                List<Order> orders = await helper.GetAllOrders(company.CompanyNumber);

                if (orders != null)
                {
                    if (orders.Count > 0)
                    {
                        for (int i = 0; i < orders.Count; i++)
                        {
                            if (orders[i].OrderNumber == randomString)
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

        public async Task<bool> AreYouSure(string title, string msg, string yes, string no)
        {
            return await Dialog.Show(title, msg, yes, no);
        }
    }
}
