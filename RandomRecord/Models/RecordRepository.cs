using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Text;




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



        public RecordRepository()
        {
            MaleFirst2010_2015 = GetDict("~/App_Data/2010_2015_MaleFirst.csv");
            MaleFirst2010_2015Weight = GetTotalWeight(MaleFirst2010_2015);

            FemaleFirst2010_2015 = GetDict("~/App_Data/2010_2015_FemaleFirst.csv");
            FemaleFirst2010_2015Weight = GetTotalWeight(FemaleFirst2010_2015);

            LastNames = GetDict("~/App_Data/LastNames.csv");
            LastNamesWeight = GetTotalWeight(LastNames);
        }

        private Dictionary<string, int> GetDict(string file)
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
                    string[] rowArrray = CSVRowToStringArray(line);
                    if (rowArrray.Length > 0)
                    {
                        rows.Add(rowArrray);
                    }
                }
            }

            // Parse each row array into a more friendly Dictionary
            Dictionary<string, int> dataDict = new Dictionary<string, int>();
            foreach (string[] row in rows)
            {
                row[0] = row[0].ToLower();
                dataDict[row[0]] = int.Parse(row[1]);
            }
            return dataDict;
        }

        // Parse a single line of a CSV file into a string array
        private static string[] CSVRowToStringArray(string row, char fieldSeparator = ',', char stringSeparator = '\"')
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
    }
}
