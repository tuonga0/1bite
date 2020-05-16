using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _1bite.Models
{
    public class Account
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public DateTime date { get; set; }
        public int rank { get; set; }

        public string role { get; set; }
    }
}