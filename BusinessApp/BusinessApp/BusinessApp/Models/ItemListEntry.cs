using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BusinessApp.Models
{
    public enum ItemType { Basket, Labour}
    public class ItemListEntry
    {
        public string Name { get; set; }
        public ItemType Type { get; set; }
        public string ItemNumber { get; set; }
        public double Amount { get; set; }
        public double Quantity { get; set; }

        public string AmountString
        {
            get
            {
                if (Type == ItemType.Basket)
                {
                    return "£" + Amount.ToString();
                }
                else
                {
                    return "£" + Amount + " Per Hour";
                }
            }
        }

        public string QuantityString
        {
            get
            {
                if (Quantity > 0)
                {
                    return Quantity.ToString();
                }
                else { return ""; }
            }
        }

        public string TypeString
        {
            get
            {
                if (Type == ItemType.Basket)
                {
                    return "Stock Item";
                }
                else
                {
                    return "Labour";
                }
            }
        }
        public string QuantityType
        {
            get
            {
                if (Type == ItemType.Basket)
                {
                    return "x";
                }
                else
                {
                    return "Hours";
                }
            }
        }
        public double TotalPrice
        {
            get
            {
                return Math.Round(Amount * Quantity, 2);
            }
        }

        public string TotalPriceString
        {
            get
            {
                return "£" + TotalPrice.ToString();
            }
        }
    }
}
