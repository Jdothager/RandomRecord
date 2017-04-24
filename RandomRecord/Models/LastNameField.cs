using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RandomRecord.Models
{
    public class LastNameField : NameField
    {
        public Dictionary<string, int> LastNameDataDict { get; set; }

        public LastNameField()
        {
            //LastNameDataDict = new Dictionary<string, int>();
            //LastNameDataDict["Johnson"] = 10;
            //LastNameDataDict["Smith"] = 20;
            //LastNameDataDict["Baker"] = 20;
            //LastNameDataDict["Savage"] = 10;


            //Name = Randomizer(LastNameDataDict, randomNumber);
            Name = "Johnson";
        }
    }
}