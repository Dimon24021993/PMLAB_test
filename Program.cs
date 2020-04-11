using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PMLAB_test
{
    class Program
    {
        private const string FileUrl = "https://gist.githubusercontent.com/skalinets/23691610f9bbf590b6fba51e373375b4/raw/0b9cc1e97650f5204edbd7b1906e03435506eaf7/mess.txt";
        static void Main(string[] args)
        {
            var fileText = LoadFileContent(FileUrl).Result;

            var charFreq = GetCharsFrequency(fileText);

            var chars = FilterDictionaryByFrequency(charFreq, 100);


            //Console.WriteLine(string.Join(",", chars));
            var time = new Stopwatch();
            time.Start();

            var regex = new Regex($"([{string.Join("", chars)}])");
            var result1 = string.Join("", regex.Matches(fileText).Select(x => x.Value));
            var timeResult1 = time.Elapsed.ToString("G");

            time.Restart();

            var result2 = string.Join("", fileText.Where(x => chars.Contains(x)));

            var timeResult2 = time.Elapsed.ToString("G");
            time.Stop();

            Console.WriteLine($"{timeResult1}\t{result1} with regex");
            Console.WriteLine($"{timeResult2}\t{result2}");
        }

        static async Task<string> LoadFileContent(string fetchUrl)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(fetchUrl);

                return await response.Content.ReadAsStringAsync();
            }
        }

        static Dictionary<char, int> GetCharsFrequency(string text)
        {
            var dictionary = new Dictionary<char, int>();
            var quantity = 0;
            foreach (var @char in text)
            {
                if (dictionary.TryGetValue(@char, out quantity))
                {
                    quantity++;
                    dictionary[@char] = quantity;
                }
                else
                {
                    dictionary.Add(@char, 1);
                }
            }

            return dictionary;
        }

        static List<char> FilterDictionaryByFrequency(Dictionary<char, int> dict, int freq)
        {
            var chars = new List<char>();
            foreach (var pair in dict)
            {
                if (pair.Value < freq) chars.Add(pair.Key);
            }

            return chars;
        }
    }
}
