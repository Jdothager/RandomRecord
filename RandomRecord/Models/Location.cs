using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RandomRecord.Models
{
    public class Location
    {
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public int zipcode { get; set; }
    }
}