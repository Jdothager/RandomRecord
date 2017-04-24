using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace RandomRecords.Models
{
    public class ResultCreator
    {
        // creates a class to build a list of Records and returns that list

        public IEnumerable<Record> GetRecords()
        {
            // create dictionary --- TEMP ---
            MaleFirst_2010_2015 dataGetter = new MaleFirst_2010_2015();
            Dictionary<string, int> dict = dataGetter.GetDict();

            // get total weight from the dictionary
            int totalWeight = 0;
            foreach (KeyValuePair<string, int> entry in dict)
            {
                totalWeight = totalWeight + entry.Value;
            }

            // create random object
            Random randomObject = new Random();

            List<Record> RecordsList = new List<Record>();
            for (int i = 0; i < 1; i++)
            {
                Record extraBody = new Record(dict, randomObject.Next(0, totalWeight));
                RecordsList.Add(extraBody);
            }

            return RecordsList;
        }
    }
}