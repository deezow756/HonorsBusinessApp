using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessApp.Models
{
    public enum OrderLogType { OrderAdd, OrderEdit, OrderDelete }
    public class OrderLog
    {
        public DateTime Date { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }

        public OrderLogType LogType { get; set; }

        public string LogTypeString
        {
            get
            {
                string msg = "";
                switch (LogType)
                {
                    case OrderLogType.OrderAdd:
                        msg = "Order Add";
                        break;
                    case OrderLogType.OrderEdit:
                        msg = "Order Edit";
                        break;
                    case OrderLogType.OrderDelete:
                        msg = "Order Delete";
                        break;
                    default:
                        break;
                }
                return msg;
            }
        }

        public string DateString
        {
            get
            {
                if (Date != null)
                {
                    return Date.ToString();
                }
                else
                    return "";
            }
        }
    }
}
