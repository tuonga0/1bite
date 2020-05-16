using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _1bite.Models
{
    public class Dish
    {
        public int id { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public string des { get; set; }
        public string dishtype { get; set; }
        public int orderedAmount { get; set; }
    }
}