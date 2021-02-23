using BusinessApp.Models;
using BusinessApp.Utilities;
using BusinessApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessApp.Controllers
{
    public class OrdersController
    {
        public async Task<List<Order>> GetAllOrders(Company company)
        {
            FirebaseHelper helper = new FirebaseHelper();
            return await helper.GetAllOrders(company.CompanyNumber);
        }

        public List<Order> FilterListByDate(List<Order> orders, int month, int year)
        {
            List<Order> temp = new List<Order>();
            for (int i = 0; i < orders.Count; i++)
            {
                if(orders[i].Date.Year == year)
                {
                    if(orders[i].Date.Month == month)
                    {
                        temp.Add(orders[i]);
                    }
                }
            }
            return temp;
        }

        public List<Order> FilterListBySearch(SearchType type, List<Order> orders, string search)
        {
            List<Order> temp = new List<Order>();

            if(type == SearchType.OrderNumber)
            {
                for (int i = 0; i < orders.Count; i++)
                {
                    if(orders[i].OrderNumber.Contains(search))
                    {
                        temp.Add(orders[i]);
                    }
                }
            }
            else if(type == SearchType.Email)
            {
                for (int i = 0; i < orders.Count; i++)
                {
                    if (orders[i].Email != null)
                    {
                        if (orders[i].Email.ToLower().Contains(search.ToLower()))
                        {
                            temp.Add(orders[i]);
                        }
                    }
                }
            }
            else if(type == SearchType.ContactNumber)
            {
                for (int i = 0; i < orders.Count; i++)
                {
                    if (orders[i].ContactNumber != null)
                    {
                        if (orders[i].ContactNumber.ToLower().Contains(search.ToLower()))
                        {
                            temp.Add(orders[i]);
                        }
                    }
                }
            }

            return temp;
        }
    }
}
