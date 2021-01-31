using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessApp.Models
{
    public class Order
    {
        public string CompanyID { get; set; }
        public string OrderNumber { get; set; }
        public DateTime Date { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public List<StockItem> StockItems { get; set; }
        public double TotalPrice { get; set; }
    }
}
