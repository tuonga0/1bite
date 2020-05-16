using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _1bite.Models
{
    public class OrderDetails
    {
        public int id { get; set; }
        public int orderId { get; set; }
        public string name { get; set; }
        public int amount { get; set; }
        public int price { get; set; }
        public int total { get; set; }
        public int orderedAmount { get; set; }
        public int moneyReceived { get; set; }
    }
}