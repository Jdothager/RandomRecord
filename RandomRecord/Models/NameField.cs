using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RandomRecords.Models
{
    public class NameField
    {
        public string Name { get; set; }
        public decimal Weight { get; set; }

        public NameField()
        {

        }

        public string Randomizer(Dictionary<string, int> dataDict, int randomNumber)
        {
            string selectedEntry = null;


            // where the magic happens...
            foreach (KeyValuePair<string, int> entry in dataDict)
            {
                if (randomNumber < entry.Value)
                {
                    selectedEntry = entry.Key;
                    break;
                }

                randomNumber = randomNumber - entry.Value;
            }

            return selectedEntry;
        }
    }
}