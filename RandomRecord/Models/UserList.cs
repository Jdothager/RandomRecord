using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace RandomRecords.Models
{
    public class UserList
    {
        // creates a class to build a list of users and returns that list

        public IEnumerable<User> GetList()
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

            List<User> usersList = new List<User>();
            for (int i = 0; i < 1; i++)
            {
                User extraBody = new User(dict, randomObject.Next(0, totalWeight));
                usersList.Add(extraBody);
            }

            return usersList;
        }
    }
}