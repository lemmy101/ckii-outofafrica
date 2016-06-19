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
using System.Data.Odbc;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;

namespace CrusaderKingsStoryGen
{
    internal partial class CulturalDna
    {
        public string changeFirstLetter(string input)
        {
            String orig = input;
            String[] sub = input.Split(' ');
            //     if (bVowel)

            String output = "";
            foreach (var s in sub)
            {
                input = s;
                if (input.Trim().Length == 0)
                    return "";
                {
                    bool bVowel = false;
                    if (StartsWithVowel(input))
                        bVowel = true;
                    input = input.Substring(1);

                    String newOne = _firstLetters[Rand.Next(_firstLetters.Count)];
                    if (!EndsWithVowel(newOne))
                        input = stripConsinentsFromStart(input);

                    while (input.Length > 8)
                    {
                        input = stripVowelsFromStart(input);
                        input = stripConsinentsFromStart(input);
                    }

                    if (bVowel)
                    {
                        input = _vowels[Rand.Next(_vowels.Count)] + input;
                    }
                    else
                    {
                        if (Rand.Next(2) == 0)
                            input = _cons[Rand.Next(_cons.Count)] + input;
                    }
                    if (input.Length == 0)
                        return orig;

                    char firstLetter = Char.ToUpper(input[0]);
                    input = firstLetter + input.Substring(1);
                }

                output += input + " ";
            }

            return output.Trim();
        }

        private bool EndsWithVowel(string input)
        {
            for (int n = 0; n < _vowels.Count; n++)
            {
                var vowel = _vowels[n];
                if (input.EndsWith(vowel))
                {
                    return true;
                }
            }

            return false;
        }
        public string ModifyName(string input)
        {
            String[] str = input.Split(' ');
            if (str.Length > 1)
                input = str[str.Length - 1];

            switch (Rand.Next(2))
            {
                case 0:
                    input = changeRandomVowel(input);
                    break;
                case 1:
                    input = changeFirstLetter(input);
                    break;
            }

            return Capitalize(input);
        }

        private static List<String> s_choices = new List<string>();

        private string changeRandomVowel(string input)
        {
            s_choices.Clear();
            for (int n = 0; n < _vowels.Count; n++)
            {
                var vowel = _vowels[n];
                if (input.Contains(vowel))
                {
                    s_choices.Add(vowel);
                }
            }

            if (s_choices.Count == 0)
                return input;

            var vowelc = s_choices[Rand.Next(s_choices.Count)];
            //foreach (var vowel in choices)
            {
                input = input.Replace(vowelc, _vowels[Rand.Next(vowelc.Length)]);
                return input;
            }

            return input;
        }
        public string Capitalize(string toCap)
        {
            if (string.IsNullOrEmpty(toCap))
                return "";
            toCap = toCap.ToLower().Trim();

            int cons = 0;
            int vow = 0;
            string f = _firstLetters[Rand.Next(_firstLetters.Count)];

            if (_vowels.Contains(f))
                toCap = f + stripVowelsFromStart(toCap);
            else
                toCap = f + stripConsinentsFromStart(toCap);

            for (int n = 0; n < toCap.Length; n++)
            {
                if (toCap[n] == ' ' || toCap[n] == '-')
                    continue;

                if (_vowels.Contains(toCap[n].ToString()) && toCap[n].ToString() != "y")
                {
                    vow++;
                    cons = 0;
                }
                else
                {
                    cons++;
                    vow = 0;
                }

                if (cons > 2)
                {
                    toCap = toCap.Substring(0, n) + toCap.Substring(n + 1);
                    cons--;
                }
                if (vow > 2)
                {
                    toCap = toCap.Substring(0, n) + toCap.Substring(n + 1);
                    vow--;
                }
            }

            String[] split = toCap.Split(new char[] { ' ', '-' });

            String outp = "";

            if (split.Length == 1)
            {
                String ss = split[0].Trim();
                char firstLetter = Char.ToUpper(ss[0]);
                ss = firstLetter + ss.Substring(1);

                return ss;
            }
            foreach (var s in split)
            {
                String ss = s;
                if (ss.Length == 0)
                    continue;
                char firstLetter = Char.ToUpper(ss[0]);
                ss = firstLetter + ss.Substring(1);

                outp += ss;
                if (outp.Length < toCap.Length)
                {
                    if (toCap[outp.Length] == ' ')
                        outp += " ";
                    else
                        outp += "-";
                }
            }

            return outp.Trim();
        }

        private string changeRandomLetter(string input)
        {
            {
                int n = Rand.Next(input.Length);

                input = input.Substring(0, n) + _commonStartNames[Rand.Next(_commonStartNames.Count)] + input.Substring(n + 1);
            }

            return input;
        }
        private bool StartsWithVowel(string input)
        {
            for (int n = 0; n < _vowels.Count; n++)
            {
                var vowel = _vowels[n];
                if (input.StartsWith(vowel.ToUpper()) || input.StartsWith(vowel))
                {
                    return true;
                }
            }

            return false;
        }
        public string stripConsinantsFromEnd(String input)
        {
            for (int x = 0; x < input.Length; x++)
            {
                bool bDoIt = true;
                for (int n = 0; n < _vowels.Count; n++)
                {
                    var vowel = _vowels[n];
                    if (input.EndsWith(vowel))
                    {
                        n = 100;
                        bDoIt = false;
                        continue;
                    }
                }

                if (bDoIt)
                {
                    input = input.Substring(0, input.Length - 1);
                    return input;
                }
                else
                {
                    break;
                }
            }


            return input;
        }

        public string stripVowelsFromEnd(String input)
        {
            for (int n = 0; n < _vowels.Count; n++)
            {
                var vowel = _vowels[n];
                if (input.EndsWith(vowel))
                {
                    input = input.Substring(0, input.Length - vowel.Length);
                    n = -1;
                }
            }

            return input;
        }
        public string stripVowelsFromStart(String input)
        {
            for (int n = 0; n < _vowels.Count; n++)
            {
                var vowel = _vowels[n];
                if (input.StartsWith(vowel.ToUpper()) || input.StartsWith(vowel))
                {
                    input = input.Substring(1);
                    n = -1;
                }
            }

            return input;
        }
        public string stripConsinentsFromStart(String input)
        {
            for (int x = 0; x < input.Length; x++)
            {
                bool bDoIt = true;
                for (int n = 0; n < _vowels.Count; n++)
                {
                    var vowel = _vowels[n];
                    if (input.StartsWith(vowel.ToUpper()) || input.StartsWith(vowel))
                    {
                        n = 100;
                        bDoIt = false;
                        continue;
                    }
                }

                if (bDoIt)
                {
                    input = input.Substring(1);
                    x = -1;
                }
                else
                {
                    break;
                }
            }

            return input;
        }

        public string GetStart(string str, int min)
        {
            bool bDone = false;
            while (!bDone)
            {
                string last = str;

                if (EndsWithVowel(str))
                {
                    str = stripVowelsFromEnd(str);
                }
                else
                {
                    str = stripConsinantsFromEnd(str);
                    str = stripVowelsFromEnd(str);
                }
                if (str.Length < min)
                {
                    str = last;
                    bDone = true;
                }
            }

            return str;
        }

        public string GetEnd(string str, int min)
        {
            String orig = str;
            bool bDone = false;
            while (!bDone)
            {
                string last = str;

                if (StartsWithVowel(str))
                {
                    str = stripVowelsFromStart(str);
                    str = stripConsinentsFromStart(str);
                }
                else
                {
                    str = stripConsinentsFromStart(str);
                }
                if (str.Length < min)
                {
                    str = last;
                    bDone = true;
                }
            }

            if (str == orig)
                return "";

            return str;
        }

        public string GetMiddle(string str, int min)
        {
            bool bDone = false;
            while (!bDone)
            {
                string last = str;

                if (EndsWithVowel(str))
                {
                    str = stripVowelsFromEnd(str);
                }
                else
                {
                    str = stripConsinantsFromEnd(str);
                }
                if (StartsWithVowel(str))
                {
                    str = stripVowelsFromStart(str);
                }
                else
                {
                    str = stripConsinentsFromStart(str);
                }
                if (str.Length < min)
                {
                    str = last;
                    bDone = true;
                }
            }

            return str;
        }
        public void Cannibalize(string str)
        {
            int min = (str.Length / 2) - 1;
            if (min < 3)
                min = 3;

            String start = GetStart(str, min - 1);
            if (start.Trim().Length > 0 && !_commonStartNames.Contains(start))
                _commonStartNames.Add(start);
            String end = GetEnd(str, min);

            if (end.Length > 5)
            {
                String mid = end;
                mid = GetStart(mid, end.Length / 3);
                //         CommonMiddleNames.Add(mid);
                end = end.Substring(mid.Length);
            }

            if (end.Trim().Length > 0 && !_commonEndNames.Contains(end))
                _commonEndNames.Add(end);
        }

        public String GetMaleCharacterName()
        {
            String word = ConstructWord(5, 10);
            //return Capitalize(GetCommonStartName() + GetCommonMiddleName() + GetCommonEndName());
            return word;
        }

        public string ConstructWord(float min, float max)
        {
            String str = "";
            do
            {
                String start = GetCommonStartName();
                String mid = GetCommonMiddleName();
                String end = GetCommonEndName();

                if (EndsWithVowel(start) && StartsWithVowel(mid))
                {
                    mid = stripVowelsFromStart(mid);
                }
                if (!EndsWithVowel(start) && !StartsWithVowel(mid))
                {
                    mid = stripConsinentsFromStart(mid);
                }
                int tot = start.Length + mid.Length + end.Length;

                if (tot > max)
                {
                    if (start.Length + end.Length < max && start.Length + end.Length > min)
                    {
                        if (EndsWithVowel(start) && StartsWithVowel(end))
                        {
                            end = stripVowelsFromStart(end);
                        }
                        if (!EndsWithVowel(start) && !StartsWithVowel(end))
                        {
                            end = stripConsinentsFromStart(end);
                        }
                    }
                    else
                        while (!(start.Length + end.Length < max && start.Length + end.Length >= min) && start.Length > 0 && end.Length > 0)
                        {
                            if (start.Length > end.Length)
                            {
                                if (EndsWithVowel(start))
                                {
                                    start = stripVowelsFromEnd(start);
                                }
                                else
                                {
                                    start = stripConsinantsFromEnd(start);
                                }
                            }
                            else
                            {
                                if (StartsWithVowel(end))
                                {
                                    end = stripVowelsFromStart(end);
                                }
                                else
                                {
                                    end = stripConsinentsFromStart(end);
                                }
                            }
                        }
                    str = start + end;
                }
                else
                {
                    if (EndsWithVowel(mid) && StartsWithVowel(end))
                    {
                        end = stripVowelsFromStart(end);
                    }
                    if (!EndsWithVowel(mid) && !StartsWithVowel(end))
                    {
                        end = stripConsinentsFromStart(end);
                    }

                    str = start + mid + end;
                }
            } while (str.Length < min || str.Length >= max);

            str = str.Replace('"'.ToString(), "");

            if (str.Length > min)
            {
                int rem = Rand.Next(str.Length - (int)min);

                while (rem > 0)
                {
                    if (Rand.Next(2) == 0)
                    {
                        if (StartsWithVowel(str))
                            str = stripVowelsFromStart(str);
                        else
                            str = stripConsinentsFromStart(str);
                    }
                    else
                    {
                        if (StartsWithVowel(str))
                            str = stripVowelsFromEnd(str);
                        else
                            str = stripConsinantsFromEnd(str);
                    }

                    rem--;
                }
            }

            if (str.Trim().Length == 1)
                return ConstructWord(min, max);

            return Capitalize(str);
        }

        private String GetCommonStartName()
        {
            return _commonStartNames[Rand.Next(_commonStartNames.Count)];
        }

        private String GetCommonMiddleName()
        {
            if (_commonMiddleNames.Count == 0)
                return "";
            return _commonMiddleNames[Rand.Next(_commonMiddleNames.Count)];
        }
        private String GetCommonEndName()
        {
            return _commonEndNames[Rand.Next(_commonEndNames.Count)];
        }

        public String GetPlaceName()
        {
            if (WordFormats.Count == 0)
            {
                for (int n = 0; n < 1; n++)
                {
                    WordFormats.Add(CommonWordFormats[Rand.Next(CommonWordFormats.Count)]);
                }
            }
            if (_wordsForLand.Count == 0)
            {
                string s = ConstructWord(2 * wordLengthBias, 4 * wordLengthBias);
                _wordsForLand.Add(s);
            }

            if (placeFormat == null)
            {
                placeFormat = placeFormatOptions[Rand.Next(placeFormatOptions.Count())];
            }

            string format = this.WordFormats[Rand.Next(WordFormats.Count)];

            int nWords = 1;

            if (Rand.Next(3) == 0)
            {
                nWords++;
            }

            if (Rand.Next(3) == 0)
            {
                nWords++;
            }

            int index = 0;

            if (nWords == 1)
            {
                index = format.IndexOf("{0}") + 3;
                format = format.Substring(0, index);
            }
            if (nWords == 2)
            {
                index = format.IndexOf("{1}") + 3;
                format = format.Substring(0, index);
            }
            String[] strs = new String[nWords];
            for (int n = 0; n < nWords; n++)
            {
                if (nWords == 1)
                    strs[n] = ConstructWord(5 * wordLengthBias, 8 * wordLengthBias);
                else if (nWords == 2)
                {
                    strs[n] = ConstructWord(3 * wordLengthBias, 5 * wordLengthBias);
                }
                else if (nWords == 3)
                {
                    strs[n] = ConstructWord(2 * wordLengthBias, 4 * wordLengthBias);
                }
            }
            if (nWords == 1)
                return Capitalize(String.Format(format, strs[0]));
            else if (nWords == 2)
                return Capitalize(String.Format(format, strs[0], strs[1]));
            else
                return Capitalize(String.Format(format, strs[0], strs[1], strs[2]));

            if (Rand.Next(2) == 0)
                return
                    Capitalize(String.Format(placeFormat, _wordsForLand[Rand.Next(_wordsForLand.Count)],
                        ConstructWord(3 * wordLengthBias, 7 * wordLengthBias)));
            else
            {
                if (Rand.Next(4) == 0)
                {
                    return ConstructWord(6 * wordLengthBias, 8 * wordLengthBias);
                }
                else
                {
                    return Capitalize(ConstructWord(3 * wordLengthBias, 5 * wordLengthBias) + " " + ConstructWord(3 * wordLengthBias, 4 * wordLengthBias));
                }
            }
        }

        public string GetMaleName()
        {
            var name = ConstructWord(3 * wordLengthBias, 8 * wordLengthBias);

            name = stripVowelsFromEnd(name);

            return name;
        }
        public string GetDynastyName()
        {
            var name = ConstructWord(6 * wordLengthBias, 8 * wordLengthBias);
            if (Rand.Next(3) == 0)
                ConstructWord(6 * wordLengthBias, 10 * wordLengthBias);

            return name;
        }

        public string GetFemaleName()
        {
            var name = ConstructWord(3 * wordLengthBias, 8 * wordLengthBias);

            name = stripConsinantsFromEnd(name);

            return name;
        }

        public string GetGodName()
        {
            var name = ConstructWord(3 * wordLengthBias, 5 * wordLengthBias);

            if (Rand.Next(3) == 0)
                name = stripConsinantsFromEnd(name);

            return name;
        }

        public void ShortenWordCounts()
        {
            int targ = _commonStartNames.Count / 4;
            int targend = _commonEndNames.Count / 4;

            targ = Math.Min(20, targ);
            targend = Math.Min(20, targend);

            while (_commonStartNames.Count > targ)
                _commonStartNames.RemoveAt(Rand.Next(_commonStartNames.Count));
            while (_commonEndNames.Count > targend)
                _commonEndNames.RemoveAt(Rand.Next(_commonEndNames.Count));
        }
    }
}
