using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BusinessApp.Models
{
    public class Order
    {
        public string CompanyNumber { get; set; }
        public string OrderNumber { get; set; }
        public DateTime Date { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public List<ItemListEntry> Items { get; set; }
        public double TotalPrice { get; set; }
        public bool Paid { get; set; }

        public string PaidString
        {
            get
            {
                if(Paid)
                {
                    return "Paid";
                }
                else
                {
                    return "Unpaid";
                }
            }
        }

        public string OrderNumberString
        {
            get
            {
                return "#" + OrderNumber;
            }
        }

        public string DateString
        {
            get
            {
                return Date.ToString("dddd, dd MMMM yyyy");
            }
        }

        public string TotalString
        {
            get
            {
                return "£" + TotalPrice.ToString();
            }
        }

        public Color PaidColour
        {
            get
            {
                if(Paid)
                {
                    return Color.Green;
                }
                else
                {
                    return Color.Red;
                }
            }
        }

    }
}
