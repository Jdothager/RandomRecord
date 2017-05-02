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
            GetBirthDateTime(record);
            GetFirstName(record);
            record.lastname = GetLastName();
            record.location = GetLocation();
            
            // TODO if I am passing it, why not just make the method return void?
            Tuple<string, string> PhoneZipCode = GetPhoneTimeZone(record.location);
            record.phone = PhoneZipCode.Item1;
            record.location.timezone = PhoneZipCode.Item2;

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

        private void GetBirthDateTime(Record record)
        {
            // TODO add population weighted values to year selector
            // select a birth year
            int selectedYear = RandomObject.Next(1960, 2001);

            DateTime fromDateTime = new DateTime(selectedYear, 1, 1, 0, 0, 0);
            DateTime toDateTime = new DateTime(selectedYear, 12, 31, 23, 59, 59);
            TimeSpan yearRange = toDateTime - fromDateTime;
            DateTime selectedDateTime = fromDateTime + new TimeSpan((long)(RandomObject.NextDouble() * yearRange.Ticks));

            record.dob = selectedDateTime;
        }

        private string GetGender()
        {
            // TODO add realistic historical probabilities
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

        private void GetFirstName(Record record)
        {
            // dictionaries to hold selected data sets base on year and gender
            Dictionary<string, int> femaleDataDict = new Dictionary<string, int>();
            int femaleTotalWeight;
            Dictionary<string, int> maleDataDict = new Dictionary<string, int>();
            int maleTotalWeight;

            // select and set birth year -> select correct datasets and total weight values
            int birthYear = record.dob.Year;
            if (birthYear >= 2010)
            {
                femaleDataDict = CsvData.FemaleFirst2010s;
                femaleTotalWeight = CsvData.FemaleFirst2010sWeight;

                maleDataDict = CsvData.MaleFirst2010s;
                maleTotalWeight = CsvData.MaleFirst2010sWeight;
            }
            else if (birthYear >= 2000)
            {
                femaleDataDict = CsvData.FemaleFirst2000s;
                femaleTotalWeight = CsvData.FemaleFirst2000sWeight;

                maleDataDict = CsvData.MaleFirst2000s;
                maleTotalWeight = CsvData.MaleFirst2000sWeight;
            }
            else if(birthYear >= 1990)
            {
                femaleDataDict = CsvData.FemaleFirst1990s;
                femaleTotalWeight = CsvData.FemaleFirst1990sWeight;

                maleDataDict = CsvData.MaleFirst1990s;
                maleTotalWeight = CsvData.MaleFirst1990sWeight;
            }
            else if(birthYear >= 1980)
            {
                femaleDataDict = CsvData.FemaleFirst1980s;
                femaleTotalWeight = CsvData.FemaleFirst1980sWeight;

                maleDataDict = CsvData.MaleFirst1980s;
                maleTotalWeight = CsvData.MaleFirst1980sWeight;
            }
            else if(birthYear >= 1970)
            {
                femaleDataDict = CsvData.FemaleFirst1970s;
                femaleTotalWeight = CsvData.FemaleFirst1970sWeight;

                maleDataDict = CsvData.MaleFirst1970s;
                maleTotalWeight = CsvData.MaleFirst1970sWeight;
            }
            else
            {
                femaleDataDict = CsvData.FemaleFirst1960s;
                femaleTotalWeight = CsvData.FemaleFirst1960sWeight;

                maleDataDict = CsvData.MaleFirst1960s;
                maleTotalWeight = CsvData.MaleFirst1960sWeight;
            }

            // set gender
            record.gender = GetGender();

            // use predetermined data sets, gender, and weight to choose name -> set firstname to the record
            if (record.gender == "female")
            {
                record.firstname = Randomizer(femaleDataDict, RandomObject.Next(0, femaleTotalWeight));
            }
            else
            {
                record.firstname = Randomizer(maleDataDict, RandomObject.Next(0, maleTotalWeight));
            }
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

        private Tuple<string, string> GetPhoneTimeZone(Location location)
        {
            string targetState = location.state;
            string targetCity = location.city;

            // local variable to manipulate
            // list of string arrays --> ["state", "area code", "time zone", "cities"]
            List<string[]> dataList = CsvData.AreaCodeTimeZones;

            // holds string[] that are within target state
            List<string[]> listContainsState = new List<string[]>();
            // search for listings in the target state
            foreach (string[] listing in dataList)
            {
                if (listing[0].Contains(targetState))
                {
                    listContainsState.Add(listing);
                }
            }

            // holds string[] that contain target city
            List<string[]> listContainsCity = new List<string[]>();
            // separate results that contain target city
            foreach (string[] listing in listContainsState)
            {
                if (listing[3].Contains(targetCity))
                {
                    listContainsCity.Add(listing);
                }
            }

            // enough filtering, time to choose an area code and resulting time zone
            string selectedAreaCode;
            string selectedTimeZone;
            int cityCount = listContainsCity.Count();
            if (cityCount > 0)
            {
                // randomly get listing
                int randomNumber = RandomObject.Next(cityCount);
                selectedAreaCode = listContainsCity[randomNumber][1];
                selectedTimeZone = listContainsCity[randomNumber][2];
            }
            else
            {
                // randomly get listing
                int randomNumber = RandomObject.Next(cityCount);
                selectedAreaCode = listContainsState[randomNumber][1];
                selectedTimeZone = listContainsState[randomNumber][2];
            }

            // add areacode to phone number
            string phone = "(" + selectedAreaCode + ") ";

            /* 
            * nxx refers to the middle three numbers of a phone number:
            * (***) XXX-****
            * convention - allowed ranges:
            * [2-9] for the first digit, and [0-9] for both the second and third digits,
            * but the second digit and the third digit can not be the same,
            * example: not allowed -> (***) *11-****
            * 
            * The last four allow [0-9]
            */
            bool nxxIsFound = false;
            while (!nxxIsFound)
            {
                string randomNumber = RandomObject.Next(200, 999).ToString();
                if (randomNumber[1] != randomNumber[2])
                {
                    phone = phone + randomNumber + "-";
                    nxxIsFound = true;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                string randomNumber = RandomObject.Next(0, 10).ToString();
                phone = phone + randomNumber;
            }

            return Tuple.Create(phone, selectedTimeZone);
        }
    }
}