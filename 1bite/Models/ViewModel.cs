using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _1bite.Models
{
    public class ViewModel
    {
        public List<OrderDetails> OrderDetails { get; set; }
        public List<Order> Order { get; set; }
        public int total { get; set; }
        public int profit { get; set; }
        public List<Dish> Dish { get; set; }
        public List<Shipper> Shipper { get; set; }
        public List<DishType> DishType { get; set; }
        public List<Account> Account { get; set; }
        public List<Staff> Staff { get; set; }
        public List<Product> Product { get; set; }
        public List<ImportDetail> importDetail { get; set; }
        public List<Import> Import { get; set; }
        public List<Source> Source { get; set; }
    }
}