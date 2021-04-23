using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _1bite.Models
{
    public class Order
    {
        public int id { get; set; }
        public int staffid { get; set; }
        public int statusId {get;set;}
        public string status { get; set; }
        public string staffName { get; set; }
        public DateTime Date { get; set; }
        public int discount { get; set; }
        public string note { get; set;  }
        public string address { get; set; }
        public int Total { get; set; }
    }
}