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
            Record record = new Record();
            record.firstname = GetFirstName(CsvData);
            record.lastname = GetLastName(CsvData);

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

        private string GetFirstName(RecordRepository CsvData)
        {
            // get gender, access dictionary and get total weight from the dictionary
            Dictionary<string, int> dataDict = new Dictionary<string, int>();
            int totalWeight;
            string gender = GetGender();
            if (gender == "female")
            {
                dataDict = CsvData.FemaleFirst2010_2015;
                totalWeight = CsvData.FemaleFirst2010_2015Weight;
            }
            else
            {
                dataDict = CsvData.MaleFirst2010_2015;
                totalWeight = CsvData.MaleFirst2010_2015Weight;
            }

            return Randomizer(dataDict, RandomObject.Next(0, totalWeight));
        }

        private string GetLastName(RecordRepository CsvData)
        {
            Dictionary<string, int> dataDict = CsvData.LastNames;

            // get total weight from the dictionary
            int totalWeight = CsvData.LastNamesWeight;

            string selectedName = Randomizer(dataDict, RandomObject.Next(0, totalWeight));

            return selectedName;
        }
    }
}