using System;
using System.Collections.Generic;
using System.Text;

namespace SampleSorter.Workers
{
    class SampleDataStringGenerator
    {

        public string RandomString(Random random)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var randomString = new String(stringChars);

            return randomString;
        }

        public IList<string> StringsBatch(int batchSize) //generates a list of strings used for insert into sql
        {
            var random = new Random();
            List<string> stringsBatch = new List<string>();
            for (int i = 0; i < batchSize; i++)
            {
                stringsBatch.Add(RandomString(random));
            }
            return stringsBatch;
        }
    }
}
