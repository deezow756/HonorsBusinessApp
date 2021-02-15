using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessApp.Models
{
    public enum StockLogType { StockAdd, StockEdit, StockDelete }
    public class StockLog
    {
        public DateTime Date { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }

        public StockLogType LogType { get; set; }

        public string LogTypeString
        {
            get
            {
                string msg = "";
                switch (LogType)
                {
                    case StockLogType.StockAdd:
                        msg = "Stock Add";
                        break;
                    case StockLogType.StockEdit:
                        msg = "Stock Edit";
                        break;
                    case StockLogType.StockDelete:
                        msg = "Stock Delete";
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
