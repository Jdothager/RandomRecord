using RandomRecord.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;


namespace RandomRecords.Models
{
    public class ResultCreator
    {
        /* This is the class that builds the records using weighted random values
         * - all of the random magic happens here, mostly in the Randomizer() method
         * - retrieves data from the RecordRepository class which stores all the data dictionaries
         * - RecordReposity class is instantiated in the RecordController and passed in when this class is created
        */

        // Random object that is shared throughout the class methods
        private static Random RandomObject = new Random();
        // Copy of the RecordRepo data
        private RecordRepository CsvData { get; set; }

        public ResultCreator(RecordRepository data)
        {
            CsvData = data;
        }

        public IEnumerable<Record> GetRecords()
        {
            List<Record> RecordsList = new List<Record>();
            for (int i = 0; i < 1; i++)
            {
                Record newRecord = GetRecord();
                RecordsList.Add(newRecord);
            }

            return RecordsList;
        }


        public Record GetRecord()
        {
            Record record = new Record();
            record.dob = GetBirthDateTime();
            record.firstname = GetFirstName();
            record.lastname = GetLastName();
            record.location = GetLocation();
            record.phone = GetPhone();

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

        private string GetBirthDateTime()
        {
            DateTime from1960 = new DateTime(1960, 1, 1, 0, 0, 0);
            DateTime to2000 = new DateTime(2000, 12, 31, 0, 0, 0);
            TimeSpan yearRange = to2000 - from1960;
            DateTime randTimeSpan = from1960 + new TimeSpan((long)(RandomObject.NextDouble() * yearRange.Ticks));
            string formatted = string.Format("{0:yyyy-MM-ddTHH:mm:ss}", randTimeSpan);

            return formatted;
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

        private string GetFirstName()
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

        private string GetLastName()
        {
            Dictionary<string, int> dataDict = CsvData.LastNames;

            // get total weight from the dictionary
            int totalWeight = CsvData.LastNamesWeight;

            string selectedName = Randomizer(dataDict, RandomObject.Next(0, totalWeight));

            return selectedName;
        }

        private Location GetLocation()
        {
            Location selectedLocation = new Location();
            int randomNumber = RandomObject.Next(0, CsvData.ZipCityStatLatLongPopWeight);

            // determine zip, city, state, lat and long
            foreach (KeyValuePair<Location, int> loc in CsvData.ZipCityStateLatLongPop)
            {
                if (randomNumber < loc.Value)
                {
                    selectedLocation = loc.Key;
                    break;
                }

                randomNumber = randomNumber - loc.Value;
            }

            // build the street address
            StringBuilder valueBuilder = new StringBuilder();

            // generate a house number
            valueBuilder.Append(RandomObject.Next(1000, 15000));

            // choose a street
            string street = CsvData.StreetNames[RandomObject.Next(CsvData.StreetNames.Count())];

            // build the string and set it to the Location object
            valueBuilder.Append(" " + street);
            selectedLocation.street = valueBuilder.ToString();

            return selectedLocation;
        }

        private string GetPhone()
        {
            // TODO everything...
            string phone = "(314) 867-5309";

            return phone;
        }
    }
}