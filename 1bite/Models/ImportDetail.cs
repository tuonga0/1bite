using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _1bite.Models
{
    public class ImportDetail
    {
        public int importId { get; set; }

        public int productId { get; set; }
        public string productName { get; set; }
        public int unitPrice { get; set; }
        public string Unit { get; set; }
        public int discounted { get; set; }
        public int amount { get; set; }
        public int paid { get; set; }
        
    }
}