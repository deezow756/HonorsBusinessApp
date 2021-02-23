using BusinessApp.Models;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessApp.Utilities
{
    public class FirebaseHelper
    {

        FirebaseClient firebase = new FirebaseClient("https://businessapp-2de82-default-rtdb.europe-west1.firebasedatabase.app/");

        #region UserEdit
        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                return (await firebase
                  .Child("Users")
                  .OnceAsync<User>()).Select(item => new User
                  {
                      Email = item.Object.Email,
                      Password = item.Object.Password,
                      FirstName = item.Object.FirstName,
                      Surname = item.Object.Surname,
                      CompanyIDs = item.Object.CompanyIDs,
                      AccountCreated = item.Object.AccountCreated
                  }).ToList();
            }
            catch(FirebaseException)
            {
                return null;
            }
            
        }

        public async Task AddNewUser(User user)
        {
            await firebase
              .Child("Users")
              .PostAsync(user);
        }

        public async Task<User> GetUser(string email)
        {
            var allUsers = await GetAllUsers();
            if (allUsers != null)
            {
                if (allUsers.Count > 0)
                {
                    await firebase
                      .Child("Users")
                      .OnceAsync<User>();
                    return allUsers.Where(a => a.Email == email).FirstOrDefault();
                }
                else { return null; }
            }
            return null;
        }

        public async Task UpdateUser(string email, User newInfo)
        {
            var toUpdateUser = (await firebase
              .Child("Users")
              .OnceAsync<User>()).Where(a => a.Object.Email == email).FirstOrDefault();

            await firebase
              .Child("Users")
              .Child(toUpdateUser.Key)
              .PutAsync(newInfo);
        }

        public async Task DeleteUser(User user)
        {
            var toDeletePerson = (await firebase
              .Child("Users")
              .OnceAsync<User>()).Where(a => a.Object.Email == user.Email).FirstOrDefault();
            await firebase.Child("Users").Child(toDeletePerson.Key).DeleteAsync();

            List<Company> companies = await GetAllCompanies();

            for (int i = 0; i < user.CompanyIDs.Count; i++)
            {
                for (int j = 0; j < companies.Count; j++)
                {
                    Company company = null;
                    if(user.CompanyIDs[i].CompanyNumber == companies[j].CompanyNumber)
                    {
                        company = companies[j];
                    }

                    if (company != null)
                    {
                        company.Employees.RemoveAll(a => a.Email == user.Email);
                        await UpdateCompany(company);
                        break;
                    }
                }
            }           
        }

        #endregion

        #region CompanyEdit

        public async Task<List<Company>> GetAllCompanies()
        {
            return (await firebase
              .Child("Companies")
              .OnceAsync<Company>()).Select(item => new Company
              {
                  Name = item.Object.Name,
                  CompanyNumber = item.Object.CompanyNumber,
                  Employees = item.Object.Employees,
                  Roles = item.Object.Roles,
                  AccountCreated = item.Object.AccountCreated
              }).ToList();
        }
        public async Task AddNewCompany(Company company)
        {
            await firebase
              .Child("Companies")
              .PostAsync(company);
        }

        public async Task<Company> GetCompany(string companyId)
        {
            var allCompanies = await GetAllCompanies();
            await firebase
              .Child("Companies")
              .OnceAsync<Company>();
            return allCompanies.Where(a => a.CompanyNumber == companyId).FirstOrDefault();
        }

        public async Task UpdateCompany(Company company)
        {
            var toUpdateCompany = (await firebase
              .Child("Companies")
              .OnceAsync<Company>()).Where(a => a.Object.CompanyNumber == company.CompanyNumber).FirstOrDefault();

            await firebase
              .Child("Companies")
              .Child(toUpdateCompany.Key)
              .PutAsync(company);
        }

        public async Task DeleteCompany(string companyId)
        {
            var toDeleteCompany = (await firebase
              .Child("Companies")
              .OnceAsync<Company>()).Where(a => a.Object.CompanyNumber == companyId).FirstOrDefault();
            await firebase.Child("Companies").Child(toDeleteCompany.Key).DeleteAsync();
        }

        #endregion

        #region ManagerLogsEdit

        public async Task<List<ManagerLog>> GetAllManagerLogs(string companyNumber)
        {
            return (await firebase
              .Child(companyNumber)
              .Child("ManagerLogs")
              .OnceAsync<ManagerLog>()).Select(item => new ManagerLog
              {
                  Name = item.Object.Name,
                  Email = item.Object.Email,
                  Date = item.Object.Date,
                  Message = item.Object.Message,
                  LogType = item.Object.LogType
              }).ToList();
        }

        public async Task AddNewManagerLog(string companyNumber, ManagerLog log)
        {
            await firebase
              .Child(companyNumber)
              .Child("ManagerLogs")
              .PostAsync(log);
        }

        #endregion

        #region StockEdit

        public async Task<List<StockItem>> GetAllStockItems(string companyId)
        {
            return (await firebase
              .Child(companyId)
              .Child("Stocks")
              .OnceAsync<StockItem>()).Select(item => new StockItem
              {
                  Name = item.Object.Name,
                  Catergory = item.Object.Catergory,
                  Quantity = item.Object.Quantity,
                  Description = item.Object.Description,
                  Type = item.Object.Type,
                  Price = item.Object.Price,
                  Cost = item.Object.Cost,
                  StockNumber = item.Object.StockNumber,
                  CompanyNumber = item.Object.CompanyNumber,
                  Date = item.Object.Date,
                  Active = item.Object.Active
              }).ToList();
        }

        public async Task<StockItem> GetStockItem(string companyId, string stockNumber)
        {
            var allStockItems = await GetAllStockItems(companyId);
            await firebase
              .Child(companyId)
              .Child("Stocks")
              .OnceAsync<StockItem>();
            return allStockItems.Where(a => a.StockNumber == stockNumber).FirstOrDefault();
        }

        public async Task AddNewStockItem(StockItem stock)
        {
            await firebase
              .Child(stock.CompanyNumber)
              .Child("Stocks")
              .PostAsync(stock);
        }

        public async Task UpdateStockItem(StockItem newInfo)
        {
            var toUpdateStock = (await firebase
              .Child(newInfo.CompanyNumber)
              .Child("Stocks")
              .OnceAsync<StockItem>()).Where(a => a.Object.StockNumber == newInfo.StockNumber).FirstOrDefault();

            await firebase
              .Child(newInfo.CompanyNumber)
              .Child("Stocks")
              .Child(toUpdateStock.Key)
              .PutAsync(newInfo);
        }

        public async Task DeleteStockItems(List<StockItem> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                var toDeletePerson = (await firebase
                  .Child(items[0].CompanyNumber)
                  .Child("Stocks")
                  .OnceAsync<StockItem>()).Where(a => a.Object.StockNumber == items[i].StockNumber).FirstOrDefault();
                await firebase.Child(items[i].CompanyNumber).Child("Stocks").Child(toDeletePerson.Key).DeleteAsync();
            }
        }

        #endregion

        #region StockLogs

        public async Task<List<StockLog>> GetAllStockLogs(string companyId)
        {
            return (await firebase
              .Child(companyId)
              .Child("StockLogs")
              .OnceAsync<StockLog>()).Select(item => new StockLog
              {
                  Date = item.Object.Date,
                  Name = item.Object.Name,
                  Email = item.Object.Email,
                  Message = item.Object.Message,
                  LogType = item.Object.LogType
              }).ToList();
        }

        public async Task AddNewStockLog(string companyId, StockLog log)
        {
            await firebase
              .Child(companyId)
              .Child("StockLogs")
              .PostAsync(log);
        }

        #endregion

        #region OrderEdit

        public async Task<List<Order>> GetAllOrders(string companyId)
        {
            return (await firebase
              .Child(companyId)
              .Child("Orders")
              .OnceAsync<Order>()).Select(item => new Order
              {
                  OrderNumber = item.Object.OrderNumber,
                  Date = item.Object.Date,
                  Email = item.Object.Email,
                  ContactNumber = item.Object.ContactNumber,
                  Items = item.Object.Items,
                  TotalPrice = item.Object.TotalPrice,
                  CompanyNumber = item.Object.CompanyNumber,
                  Paid = item.Object.Paid
              }).ToList();
        }

        public async Task<Order> GetOrder(string companyId, string orderNumber)
        {
            var allOrders = await GetAllOrders(companyId);
            await firebase
              .Child(companyId)
              .Child("Orders")
              .OnceAsync<Order>();
            return allOrders.Where(a => a.OrderNumber == orderNumber).FirstOrDefault();
        }

        public async Task AddNewOrder(Order order)
        {
            await firebase
              .Child(order.CompanyNumber)
              .Child("Orders")
              .PostAsync(order);
        }

        public async Task UpdateOrder(Order order)
        {
            var toUpdateOrder = (await firebase
              .Child(order.CompanyNumber)
              .Child("Orders")
              .OnceAsync<Order>()).Where(a => a.Object.OrderNumber == order.OrderNumber).FirstOrDefault();

            await firebase
              .Child(order.CompanyNumber)
              .Child("Orders")
              .Child(toUpdateOrder.Key)
              .PutAsync(order);
        }

        public async Task DeleteOrder(List<Order> orders)
        {
            for (int i = 0; i < orders.Count; i++)
            {
                var toDeleteOrder = (await firebase
                  .Child(orders[0].CompanyNumber)
                  .Child("Orders")
                  .OnceAsync<Order>()).Where(a => a.Object.OrderNumber == orders[i].OrderNumber).FirstOrDefault();
                await firebase.Child("Companies").Child(orders[0].CompanyNumber).Child("Orders").Child(toDeleteOrder.Key).DeleteAsync();
            }
        }

        #endregion

        #region OrderLog

        public async Task<List<OrderLog>> GetAllOrderLogs(string companyId)
        {
            return (await firebase
              .Child(companyId)
              .Child("OrderLogs")
              .OnceAsync<OrderLog>()).Select(item => new OrderLog
              {
                  Date = item.Object.Date,
                  Name = item.Object.Name,
                  Email = item.Object.Email,
                  Message = item.Object.Message
              }).ToList();
        }

        public async Task AddNewOrderLog(string companyId, OrderLog log)
        {
            await firebase
              .Child(companyId)
              .Child("OrderLogs")
              .PostAsync(log);
        }

        #endregion
    }
}
