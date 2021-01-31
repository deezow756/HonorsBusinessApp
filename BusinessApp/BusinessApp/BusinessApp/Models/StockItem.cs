using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessApp.Models
{
    public class StockItem
    {
        public string CompanyID { get; set; }
        public string StockNumber { get; set; }
        public string Catergory { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Cost { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
    }
}
