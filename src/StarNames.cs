// Out of Africa - A random world generator for Crusader Kings II
// Copyright (C) 2015--2016 yemmlie101 and nuew
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CrusaderKingsStoryGen
{
    public static class StarNames
    {
        private static string[] s_vowels = new string[] { "a", "e", "i", "o", "u", "ae", "y" };

        private static string[] s_namePartsStart = new string[]
        {
            "a", "aba", "aca", "ada", "ara", "ala", "ama", "ana", "arc", "ard", "arg", "arm", "arn", "art", "ass", "ast", "alb",
            "ald", "alm", "all", "ant", "and", "ar", "an", "am", "as", "at", "al", "att", "bab", "bad", "ban", "bann", "bar", "bra", "can", "car", "cra", "cart", "cars", "cann",
            "dan", "dar", "dart", "darn", "da", "ra", "ran", "ras", "rag", "tar", "tan", "xan", "xar", "pa", "pas", "past", "par", "pra", "part", "pat", "patt", "pac",
            "pad", "pan", "pam", "pag", "pra", "sa", "sad", "san", "sam", "sat", "sap", "sar", "sca", "tra"
        };

        private static string[] s_namePartsEnd = new string[] { "a", "i", "ae", "ia", "ai", "ar", "as", "air", "ain", "ris", "ran", "ras", "ron", "ros", "us", "os", "ous", "on", "uon", "ion", "ios", "iak", "ious", "fi", "ami", "kat", "la", "las", "lon" };

        private static string[] s_nameAdditional = new string[] { "System", "Cluster", "Majoris", "Minoris", "Major", "Minor", "Star", "Primus" };

        public static string Generate(int seed)
        {
            string name = "";
            var rand = Rand.Get();

            int syllabuls = rand.Next(2) + 1;

            for (int n = 0; n < syllabuls; n++)
            {
                string wordPart = s_namePartsStart[rand.Next(s_namePartsStart.Length)];
                foreach (char c in wordPart)
                {
                    if (c == 'a')
                    {
                        string v = s_vowels[rand.Next(s_vowels.Length)];
                        if (v == "y")
                            v = s_vowels[rand.Next(s_vowels.Length)];

                        name += s_vowels[rand.Next(s_vowels.Length)];
                    }
                    else
                    {
                        name += c;
                    }
                }
            }

            name += s_namePartsEnd[rand.Next(s_namePartsEnd.Length)];

            char firstLetter = Char.ToUpper(name[0]);
            name = firstLetter + name.Substring(1);


            return name;
        }

        public static string SafeName(string name)
        {
            name = name.ToLower();
            name = name.Replace(" ", "");
            name = name.Replace("-", "");
            // name += (Rand.Next(10000) + 1).ToString();
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            name = rgx.Replace(name, "");

            return name;
        }

        public static string Generate(string culture)
        {
            return CultureManager.instance.CultureMap[culture].dna.GetPlaceName();
        }
    }
}
