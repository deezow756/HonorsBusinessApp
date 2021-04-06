using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace BusinessApp.Models
{
    public enum StockType { Item, Category}
    public class StockItem
    {
        public string CompanyNumber { get; set; }
        public string StockNumber { get; set; }
        public string Catergory { get; set; }
        public StockType Type { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Cost { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public bool Active { get; set; }
        public DateTime DateActive { get; set; }

        public bool SelectionMode = false;
        public bool Selected = false;

        public Style Icon
        {
            get
            {
                if (SelectionMode)
                {
                    if (Selected)
                    {
                        return Application.Current.Resources["RadioChecked"] as Style;
                    }
                    else
                    {
                        return Application.Current.Resources["RadioUnchecked"] as Style;
                    }
                }
                else
                { return null; }
            }
        }
        public bool IconVisible
        {
            get
            {
                if (SelectionMode)
                { return true;  }
                else
                { return false; }
            }
        }

        public string ItemPriceString
        {
            get
            {
                if (Price > 0)
                {
                    string specifier = "C";
                    CultureInfo culture = CultureInfo.CreateSpecificCulture("en-GB");
                    return Price.ToString(specifier, culture);
                }
                else
                {
                    return "";
                }
            }
        }

        public string QuantityString
        {
            get
            {
                if(Type == StockType.Item)
                {
                    return "x" + Quantity.ToString();
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
