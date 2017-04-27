using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Text;
using RandomRecord.Models;

namespace RandomRecords.Models
{
    public class RecordRepository
    {
        public Dictionary<string, int> MaleFirst2010_2015 { get; set; }
        public int MaleFirst2010_2015Weight { get; set; }
        public Dictionary<string, int> FemaleFirst2010_2015 { get; set; }
        public int FemaleFirst2010_2015Weight { get; set; }
        public Dictionary<string, int> LastNames { get; set; }
        public int LastNamesWeight { get; set; }
        public Dictionary<Location, int> ZipCityStateLatLongPop { get; set; }
        public int ZipCityStatLatLongPopWeight { get; set; }
        public List<string> StreetNames { get; set; }



        public RecordRepository()
        {
            MaleFirst2010_2015 = GetDict("~/App_Data/2010_2015_MaleFirst.csv");
            MaleFirst2010_2015Weight = GetTotalWeight(MaleFirst2010_2015);

            FemaleFirst2010_2015 = GetDict("~/App_Data/2010_2015_FemaleFirst.csv");
            FemaleFirst2010_2015Weight = GetTotalWeight(FemaleFirst2010_2015);

            LastNames = GetDict("~/App_Data/LastNames.csv");
            LastNamesWeight = GetTotalWeight(LastNames);

            ZipCityStateLatLongPop = GetLocDict("~/App_Data/ZipCityStateLatLongPop.csv");
            ZipCityStatLatLongPopWeight = GetLocTotalWeight(ZipCityStateLatLongPop);

            StreetNames = GetNamesList("~/App_Data/StreetNames.csv");
        }

        private Dictionary<string, int> GetDict(string file)
        {
            // List of string arrays to hold rows
            List<string[]> rows = CsvToListOfStringArrays(file);

            // Parse each row array into a more friendly Dictionary
            Dictionary<string, int> dataDict = new Dictionary<string, int>();
            foreach (string[] row in rows)
            {
                dataDict[row[0]] = int.Parse(row[1]);
            }
            return dataDict;
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

        private List<string> GetNamesList(string file)
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
