using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Text;
using RandomRecord.Models;

namespace RandomRecords.Models
{
    public class RecordRepository
    {
        public Dictionary<string, int> FemaleFirst2010s { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> FemaleFirst2000s { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> FemaleFirst1990s { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> FemaleFirst1980s { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> FemaleFirst1970s { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> FemaleFirst1960s { get; set; } = new Dictionary<string, int>();

        public int FemaleFirst2010sWeight { get; set; }
        public int FemaleFirst2000sWeight { get; set; }
        public int FemaleFirst1990sWeight { get; set; }
        public int FemaleFirst1980sWeight { get; set; }
        public int FemaleFirst1970sWeight { get; set; }
        public int FemaleFirst1960sWeight { get; set; }

        public Dictionary<string, int> MaleFirst2010s { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> MaleFirst2000s { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> MaleFirst1990s { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> MaleFirst1980s { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> MaleFirst1970s { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> MaleFirst1960s { get; set; } = new Dictionary<string, int>();

        public int MaleFirst2010sWeight { get; set; }
        public int MaleFirst2000sWeight { get; set; }
        public int MaleFirst1990sWeight { get; set; }
        public int MaleFirst1980sWeight { get; set; }
        public int MaleFirst1970sWeight { get; set; }
        public int MaleFirst1960sWeight { get; set; }

        public Dictionary<string, int> LastNames { get; set; } = new Dictionary<string, int>();
        public int LastNamesWeight { get; set; }
        public Dictionary<Location, int> ZipCityStateLatLongPop { get; set; }
        public int ZipCityStatLatLongPopWeight { get; set; }
        public List<string> StreetNames { get; set; }
        public List<string[]> AreaCodeTimeZones { get; set; }

        public RecordRepository()
        {
            LoadFirstNameDicts("~/App_Data/FirstNames1960_2015.csv");
            LoadFirstNameWeights();

            LoadLastNamesDicts("~/App_Data/LastNames.csv");
            LastNamesWeight = GetTotalWeight(LastNames);

            ZipCityStateLatLongPop = GetLocDict("~/App_Data/ZipCityStateLatLongPop.csv");
            ZipCityStatLatLongPopWeight = GetLocTotalWeight(ZipCityStateLatLongPop);

            StreetNames = GetStreetNamesList("~/App_Data/StreetNames.csv");

            AreaCodeTimeZones = GetAreaCodeTimeZoneDict("~/App_Data/StateAreaCodeTZCity.csv");
        }

        private void LoadFirstNameDicts(string file)
        {
            // List of string arrays to hold rows
            List<string[]> rows = CsvToListOfStringArrays(file);

            // Parse each row array into corresponding dictionary property
            foreach (string[] row in rows)
            {
                MaleFirst1960s[row[0]] = int.Parse(row[1]);
                FemaleFirst1960s[row[2]] = int.Parse(row[3]);

                MaleFirst1970s[row[4]] = int.Parse(row[5]);
                FemaleFirst1970s[row[6]] = int.Parse(row[7]);

                MaleFirst1980s[row[8]] = int.Parse(row[9]);
                FemaleFirst1980s[row[10]] = int.Parse(row[11]);

                MaleFirst1990s[row[12]] = int.Parse(row[13]);
                FemaleFirst1990s[row[14]] = int.Parse(row[15]);

                MaleFirst2000s[row[16]] = int.Parse(row[17]);
                FemaleFirst2000s[row[18]] = int.Parse(row[19]);

                MaleFirst2010s[row[20]] = int.Parse(row[21]);
                FemaleFirst2010s[row[22]] = int.Parse(row[23]);
            }
        }

        private void LoadFirstNameWeights()
        {
            MaleFirst2010sWeight = GetTotalWeight(MaleFirst2010s);
            MaleFirst2000sWeight = GetTotalWeight(MaleFirst2000s);
            MaleFirst1990sWeight = GetTotalWeight(MaleFirst1990s);
            MaleFirst1980sWeight = GetTotalWeight(MaleFirst1980s);
            MaleFirst1970sWeight = GetTotalWeight(MaleFirst1970s);
            MaleFirst1960sWeight = GetTotalWeight(MaleFirst1960s);

            FemaleFirst2010sWeight = GetTotalWeight(FemaleFirst2010s);
            FemaleFirst2000sWeight = GetTotalWeight(FemaleFirst2000s);
            FemaleFirst1990sWeight = GetTotalWeight(FemaleFirst1990s);
            FemaleFirst1980sWeight = GetTotalWeight(FemaleFirst1980s);
            FemaleFirst1970sWeight = GetTotalWeight(FemaleFirst1970s);
            FemaleFirst1960sWeight = GetTotalWeight(FemaleFirst1960s);
        }

        private void LoadLastNamesDicts(string file)
        {
            // List of string arrays to hold rows
            List<string[]> rows = CsvToListOfStringArrays(file);

            // Parse each row array into a more friendly Dictionary
            foreach (string[] row in rows)
            {
                LastNames[row[0]] = int.Parse(row[1]);
            }

        }

        private Dictionary<Location, int> GetLocDict(string file)
        {
            // List of string arrays to hold rows
            List<string[]> rows = CsvToListOfStringArrays(file);

            // Parse each row array into a location Dictionary
            Dictionary<Location, int> locDict = new Dictionary<Location, int>();
            foreach (string[] row in rows)
            {
                Location loc = new Location();
                loc.zipcode = row[0];
                loc.city = row[1];
                loc.state = row[2];
                loc.latitude = double.Parse(row[3]);
                loc.longitude = double.Parse(row[4]);
                locDict[loc] = int.Parse(row[5]);
            }

            return locDict;
        }

        private List<string> GetStreetNamesList(string file)
        {
            // List of strings to return
            List<string> names = new List<string>();

            // Read the file and convert each row to string and add to list
            string path = HttpContext.Current.Server.MapPath(file);
            using (StreamReader reader = File.OpenText(path))
            {
                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                    if (line.Length > 0)
                    {
                        names.Add(line);
                    }
                }
            }

            return names;
        }

        private List<string[]> GetAreaCodeTimeZoneDict(string file)
        {
            /*
            * I left this as a separate method, rather than just calling CsvToListOfStringArrays()
            * because I may refactor the csv files
            */

            // create list that holds a string[] of each line of file
            List<string[]> rows = CsvToListOfStringArrays(file);

            return rows;
        }

        private static List<string[]> CsvToListOfStringArrays(string file)
        {
            // List of string arrays to hold rows
            List<string[]> rows = new List<string[]>();

            // Read the file and convert to string array
            string path = HttpContext.Current.Server.MapPath(file);
            using (StreamReader reader = File.OpenText(path))
            {
                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                    string[] rowArrray = CsvRowToStringArray(line);
                    if (rowArrray.Length > 0)
                    {
                        rows.Add(rowArrray);
                    }
                }
            }

            return rows;
        }

        private static string[] CsvRowToStringArray(string row, char fieldSeparator = ',', char stringSeparator = '\"')
        {
            bool isBetweenQuotes = false;
            StringBuilder valueBuilder = new StringBuilder();
            List<string> rowValues = new List<string>();

            // Loop through the row string one char at a time
            foreach (char c in row.ToCharArray())
            {
                if ((c == fieldSeparator && !isBetweenQuotes))
                {
                    rowValues.Add(valueBuilder.ToString());
                    valueBuilder.Clear();
                }
                else
                {
                    if (c == stringSeparator)
                    {
                        isBetweenQuotes = !isBetweenQuotes;
                    }
                    else
                    {
                        valueBuilder.Append(c);
                    }
                }
            }

            // Add the final value
            rowValues.Add(valueBuilder.ToString());
            valueBuilder.Clear();

            return rowValues.ToArray();
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

        private int GetLocTotalWeight(Dictionary<Location, int> dataDict)
        {
            // get total weight from the dictionary
            int totalWeight = 0;
            foreach (KeyValuePair<Location, int> entry in dataDict)
            {
                totalWeight = totalWeight + entry.Value;
            }

            return totalWeight;
        }
    }
}
