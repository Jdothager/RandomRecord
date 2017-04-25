using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace RandomRecords.Models
{
    public class ResultCreator
    {
        private static Random RandomObject = new Random();

        // creates a class to build a list of Records and returns that list
        // this will be the magic of the randomness happens

        public IEnumerable<Record> GetRecords(RecordRepository CsvData)
        {

            List<Record> RecordsList = new List<Record>();
            for (int i = 0; i < 1; i++)
            {
                Record extraBody = GetRecord(CsvData);
                RecordsList.Add(extraBody);
            }

            return RecordsList;
        }


        public Record GetRecord(RecordRepository CsvData)
        {
            // get gender and access dictionary
            Dictionary<string, int> dataDict = new Dictionary<string, int>();
            string gender = GetGender();
            if (gender == "female")
            {
                dataDict = CsvData.FemaleFirst_2010_2015;
            }
            else
            {
                dataDict = CsvData.MaleFirst_2010_2015;

            }

            // get total weight from the dictionary
            int totalWeight = GetTotalWeight(dataDict);

            Record record = new Record();
            record.FirstName = Randomizer(dataDict, RandomObject.Next(0, totalWeight));
            record.LastName = "Washington";

            return record;
        }

        private string Randomizer(Dictionary<string, int> dataDict, int randomNumber)
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

        private string GetGender()
        {
            int coin = RandomObject.Next(0, 2);
            if (coin % 2 == 0)
            {
                return "female";
            }
            else
            {
                return "male";
            }
        }

        private int GetTotalWeight(Dictionary<string, int> dataDict)
        {
            // get total weight from the dictionary
            int totalWeight = 0;
            foreach (KeyValuePair<string, int> entry in dataDict)
            {
                totalWeight = totalWeight + entry.Value;
            }
            return totalWeight;
        }
    }
}