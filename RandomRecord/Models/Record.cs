using RandomRecord.Models;
using System;

namespace RandomRecords.Models
{
    public class Record
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public Location location { get; set; }
        public DateTime dob { get; set; }
        public string gender { get; set; }
        public string phone { get; set; }
    }
}