using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _1bite.Models
{
    public class Import
    {
        public DateTime date { get; set; }
        public int overallDiscount { get; set; }
        public int id { get; set; }
        public int shipFee { get; set; }
        public int total { get; set; }
        public string source { get; set; }
        public int sourceid { get; set; }
        public int staffId { get; set; }
        public string staffname { get; set; }
    }
}