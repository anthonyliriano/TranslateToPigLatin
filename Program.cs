using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace EnglishToPigLatin
{
    class Program
    {
        private const string FILE_PATH = "https://ocw.mit.edu/ans7870/6/6.006/s08/lecturenotes/files/t8.shakespeare.txt";
        private static readonly List<char> VOWELS = new List<char> { 'a', 'e', 'i', 'o', 'u', };

        static void Main(string[] args)
        {
            StringBuilder translation = new StringBuilder();
            string translateText = GetShakespeareWork().ToLower();

            string[] wordsToTranslate = translateText.Split(' ', ',');

            foreach(string word in wordsToTranslate)
                translation.Append(TranslateToPigLatin(word));


            WriteToFile(translation);
        }


        static string TranslateToPigLatin(string input)
        {
            char[] vowelsExtended = VOWELS.Concat(new[] { 'y' }).ToArray();
            string result = input;

            if (!string.IsNullOrEmpty(input) && input.Length > 0)
            {
                if (VOWELS.Contains(input[0]))
                    result = input + "way ";
                else
                {
                    int indexOfVowel = input.IndexOfAny(vowelsExtended, 1);
                    if (indexOfVowel == -1)
                        result = input + "ay ";
                    else
                        result = input.Substring(indexOfVowel) + input.Substring(0, indexOfVowel) + "ay ";
                }
            }

            return result;
        }

        static string GetShakespeareWork()
        {
            try
            {
                using HttpClient client = new HttpClient();
                return client.GetAsync(FILE_PATH).Result.Content.ReadAsStringAsync().Result;

            }catch(Exception ex)
            {
                return "";
            }
        }

        static void WriteToFile(StringBuilder sb)
        {
            using StreamWriter file = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "translation.txt");
            file.Write(sb.ToString());
        }
    }
}
