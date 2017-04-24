using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RandomRecord.Models
{
    public class FirstNameField : NameField
    {
        public string Gender { get; set; }
        public Dictionary<string, int> FirstNameDataDict { get; set; } 

        public FirstNameField(Dictionary<string, int> dict, int randomNumber)
        {
            //FirstNameDataDict = new Dictionary<string, int>();
            //FirstNameDataDict["Dave"] = 10;
            //FirstNameDataDict["Bob"] = 20;
            //FirstNameDataDict["Charlie"] = 20;
            //FirstNameDataDict["John"] = 10;


            Name = Randomizer(dict, randomNumber);
        }


    }
}