using RandomRecord.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RandomRecords.Models
{
    public class Record
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string dob { get; set; }
        public Location location { get; set; }
    }
}