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
using System.Text;
using System.Windows.Forms.VisualStyles;

namespace CrusaderKingsStoryGen
{
    internal partial class CulturalDna
    {
        private List<String> _vowels = new List<string>() { "a", "e", "i", "o", "u", "ae", "y" };
        public CultureParser culture;

        public static string[] placeFormatOptions = new[] { "{1}{0}" };
        public string placeFormat = null;
        public float wordLengthBias = 1.0f;

        public String empTitle = "";
        public String kingTitle = "";
        public String dukeTitle = "";
        public String countTitle = "";
        public String baronTitle = "";
        public String mayorTitle = "";
        public String femaleEnd = "";
        private List<String> _wordsForLand = new List<string>()
        {
        };

        public static List<String> CommonWordFormats = new List<string>()
        {
            "{0}-{1}-{2}",
            "{0} {1}-{2}",
            "{0} {1} {2}",
            "{0}-{1} {2}",
            "{0}'{1}'{2}",
            "{0}-{1}'{2}",
            "{0}{1}'{2}",
            "{0}'{1}{2}",
            "{0}'{1}-{2}",
        };

        public List<String> WordFormats = new List<string>()
        {
        };

        public CulturalDna()
        {
            wordLengthBias = 1.0f;
        }
        private List<String> _firstLetters = new List<string>()
        {
            "a", "e", "d", "k", "q", "w", "r", "t", "r", "s", "t", "l", "n", "k", "b",
        };

        private List<String> _cons = new List<String> { "q", "w", "r", "t", "r", "s", "t", "l", "n", "k", "b" };

        private List<String> _commonStartNames = new List<string>();
        private List<String> _commonMiddleNames = new List<string>();
        private List<String> _commonEndNames = new List<string>();
        public List<string> portraitPool = new List<string>();
        public bool dukes_called_kings = false;
        public bool baron_titles_hidden = false;
        public bool count_titles_hidden = false;
        public bool allow_looting = false;
        public bool seafarer = false;
        public string male_patronym = "son";
        public string female_patronym = "daughter";
        public bool patronym_prefix = false;
        public bool founder_named_dynasties = false;
        public bool dynasty_title_names = false;
        public string Name { get; set; }
        public string from_dynasty_prefix = "af ";
        public bool horde = false;
        public CultureParser dna;
        private bool _tribal;

        public void CreateFromCulture(CultureParser c)
        {
            dna = c;
        }

        public CulturalDna MutateSmall(int numChanges)
        {
            var c = new CulturalDna();

            c.empTitle = empTitle;
            c.kingTitle = kingTitle;
            c.dukeTitle = dukeTitle;
            c.countTitle = countTitle;
            c.baronTitle = baronTitle;
            c.mayorTitle = mayorTitle;

            c._tribal = _tribal;
            c._wordsForLand.AddRange(_wordsForLand);
            //c.cons.AddRange(cons);
            c.WordFormats.AddRange(CommonWordFormats);
            //  c.vowels.Clear();
            //c.vowels.AddRange(vowels);
            c._commonStartNames.AddRange(_commonStartNames);
            c._commonMiddleNames.AddRange(_commonMiddleNames);
            c._commonEndNames.AddRange(_commonEndNames);
            c.portraitPool.AddRange(portraitPool);
            c.placeFormat = placeFormat;
            c._firstLetters.AddRange(_firstLetters);
            c.wordLengthBias = wordLengthBias;
            c.from_dynasty_prefix = from_dynasty_prefix;
            c.count_titles_hidden = count_titles_hidden;
            c.baron_titles_hidden = baron_titles_hidden;
            c.allow_looting = allow_looting;
            c.female_patronym = female_patronym;
            c.male_patronym = male_patronym;
            c.founder_named_dynasties = founder_named_dynasties;
            c.dynasty_title_names = dynasty_title_names;
            c.horde = horde;
            c.culture = culture;
            for (int n = 0; n < numChanges; n++)
            {
                c.DoRandomSmallChange();
            }

            return c;
        }

        public CulturalDna Mutate(int numChanges)
        {
            var c = new CulturalDna();
            c.empTitle = empTitle;
            c.kingTitle = kingTitle;
            c.dukeTitle = dukeTitle;
            c.countTitle = countTitle;
            c.baronTitle = baronTitle;
            c.mayorTitle = mayorTitle;

            c._tribal = _tribal;
            c._wordsForLand.AddRange(_wordsForLand);
            //c.cons.AddRange(cons);
            //   c.vowels.AddRange(vowels);
            c._commonStartNames.AddRange(_commonStartNames);
            c._commonMiddleNames.AddRange(_commonMiddleNames);
            c._commonEndNames.AddRange(_commonEndNames);
            c.WordFormats.AddRange(CommonWordFormats);
            c.portraitPool.AddRange(portraitPool);
            c.placeFormat = placeFormat;
            c._firstLetters.AddRange(_firstLetters);
            c.wordLengthBias = wordLengthBias;
            c.from_dynasty_prefix = from_dynasty_prefix;
            c.count_titles_hidden = count_titles_hidden;
            c.baron_titles_hidden = baron_titles_hidden;
            c.allow_looting = allow_looting;
            c.female_patronym = female_patronym;
            c.male_patronym = male_patronym;
            c.founder_named_dynasties = founder_named_dynasties;
            c.dynasty_title_names = dynasty_title_names;
            c.horde = horde;
            c.culture = culture;
            for (int n = 0; n < numChanges; n++)
            {
                c.DoRandomChange();
            }

            return c;
        }

        public void DoRandom()
        {
            from_dynasty_prefix = ConstructWord(2, 4).ToLower() + " ";
            this.female_patronym = ConstructWord(3, 4).ToLower();
            this.male_patronym = ConstructWord(3, 4).ToLower();

            allow_looting = Rand.Next(2) == 0;
            if (allow_looting && GovernmentManager.instance.numNomadic < 10)
                horde = Rand.Next(2) == 0;
            else
            {
                horde = false;
            }

            empTitle = ConstructWord(2, 5);
            kingTitle = ConstructWord(2, 5);
            dukeTitle = ConstructWord(2, 5);
            countTitle = ConstructWord(2, 5);
            baronTitle = ConstructWord(2, 5);
            mayorTitle = ConstructWord(2, 5);

            empTitle = LanguageManager.instance.AddSafe(empTitle);
            kingTitle = LanguageManager.instance.AddSafe(kingTitle);
            dukeTitle = LanguageManager.instance.AddSafe(dukeTitle);
            countTitle = LanguageManager.instance.AddSafe(countTitle);
            baronTitle = LanguageManager.instance.AddSafe(baronTitle);
            mayorTitle = LanguageManager.instance.AddSafe(mayorTitle);
        }
        private void DoRandomChange()
        {
            switch (Rand.Next(4))
            {
                case 0:
                    // replace 1/3 of the Start words and replace with random dna culture....
                    {
                        int count = _commonStartNames.Count / 2;
                        WordFormats.Clear();
                        ReplaceStartNames(count);
                        {
                            _wordsForLand.Clear();
                            int create = 3;

                            for (int n = 0; n < create; n++)
                            {
                                String a = "";
                                a = ConstructWord(2 * wordLengthBias, 4 * wordLengthBias);

                                if (!_wordsForLand.Contains(a))
                                {
                                    _wordsForLand.Add(a);
                                    continue;
                                }
                            }
                        }
                    }
                    break;

                case 1:
                    {
                        int count = _commonEndNames.Count / 2;
                        ReplaceEndNames(count);
                        WordFormats.Clear();
                        {
                            _wordsForLand.Clear();
                            int create = 3;

                            for (int n = 0; n < create; n++)
                            {
                                String a = "";
                                a = ConstructWord(2 * wordLengthBias, 4 * wordLengthBias);

                                if (!_wordsForLand.Contains(a))
                                {
                                    _wordsForLand.Add(a);
                                    continue;
                                }
                            }
                        }
                    }
                    break;

                case 2:

                    // replace 1/3 of the words for land
                    {
                        WordFormats.Clear();
                        if (portraitPool.Count == 0)
                        {
                            return;
                        }

                        int x = Rand.Next(portraitPool.Count);

                        var por = portraitPool[x];
                        portraitPool.RemoveAt(x);
                        portraitPool.Add(culture.GetRelatedCultureGfx(por));

                        if (Rand.Next(6) == 0)
                        {
                            if (portraitPool.Count == 1)
                            {
                                portraitPool.Add(culture.GetRelatedCultureGfx(por));
                            }
                            else if (portraitPool.Count == 2)
                            {
                                portraitPool.RemoveAt(Rand.Next(2));
                            }
                        }

                        if (Rand.Next(6) == 0)
                        {
                            portraitPool.RemoveAt(0);
                            portraitPool.Add(CultureParser.GetRandomCultureGraphics());
                        }
                    }

                    break;

                case 3:

                    //   for(int n=0;n<3;n++)
                    {
                        // switch (Rand.Next(6))
                        {
                            //    case 0:
                            empTitle = ConstructWord(2, 5);
                            empTitle = LanguageManager.instance.AddSafe(empTitle);

                            //       break;
                            //    case 1:
                            kingTitle = ConstructWord(2, 5);
                            kingTitle = LanguageManager.instance.AddSafe(kingTitle);

                            //      break;
                            //  case 2:
                            dukeTitle = ConstructWord(2, 5);
                            dukeTitle = LanguageManager.instance.AddSafe(dukeTitle);

                            //       break;
                            //   case 3:
                            countTitle = ConstructWord(2, 5);
                            countTitle = LanguageManager.instance.AddSafe(countTitle);

                            //     break;
                            //    case 4:
                            baronTitle = ConstructWord(2, 5);
                            baronTitle = LanguageManager.instance.AddSafe(baronTitle);

                            //        break;
                            //    case 5:
                            mayorTitle = ConstructWord(2, 5);
                            mayorTitle = LanguageManager.instance.AddSafe(mayorTitle);

                            //       break;
                        }
                    }

                    break;
            }

            if (culture != null)
                culture.DoDetailsForCulture();
        }
        private void DoRandomSmallChange()
        {
            {
                switch (Rand.Next(11))
                {
                    case 0:
                        founder_named_dynasties = !founder_named_dynasties;
                        break;
                    case 1:
                        dynasty_title_names = !dynasty_title_names;
                        break;
                    case 2:
                        baron_titles_hidden = !baron_titles_hidden;
                        break;
                    case 3:
                        count_titles_hidden = !count_titles_hidden;
                        break;
                    case 4:
                        dukes_called_kings = !dukes_called_kings;
                        break;
                    case 5:
                        from_dynasty_prefix = ConstructWord(2, 3);
                        break;
                    case 6:
                        this.female_patronym = ConstructWord(3, 4).ToLower();
                        this.male_patronym = ConstructWord(3, 4).ToLower();
                        if (Rand.Next(10) == 0)
                        {
                            this.female_patronym = ConstructWord(3, 4);
                            this.male_patronym = ConstructWord(3, 4);
                        }
                        break;
                    case 7:
                        // Change place format
                        {
                            this.placeFormat = null;
                        }
                        break;
                    case 8:
                        WordFormats.RemoveAt(Rand.Next(WordFormats.Count));
                        WordFormats.Add(CommonWordFormats[Rand.Next(CommonWordFormats.Count)]);
                        break;
                    case 9:
                        _tribal = !_tribal;
                        break;
                    case 10:

                        //    for (int n = 0; n < 3; n++)
                        {
                            switch (Rand.Next(6))
                            {
                                case 0:
                                    empTitle = ConstructWord(2, 5);
                                    empTitle = LanguageManager.instance.AddSafe(empTitle);

                                    break;
                                case 1:
                                    kingTitle = ConstructWord(2, 5);
                                    kingTitle = LanguageManager.instance.AddSafe(kingTitle);

                                    break;
                                case 2:
                                    dukeTitle = ConstructWord(2, 5);
                                    dukeTitle = LanguageManager.instance.AddSafe(dukeTitle);

                                    break;
                                case 3:
                                    countTitle = ConstructWord(2, 5);
                                    countTitle = LanguageManager.instance.AddSafe(countTitle);

                                    break;
                                case 4:
                                    baronTitle = ConstructWord(2, 5);
                                    baronTitle = LanguageManager.instance.AddSafe(baronTitle);

                                    break;
                                case 5:
                                    mayorTitle = ConstructWord(2, 8);
                                    mayorTitle = LanguageManager.instance.AddSafe(mayorTitle);

                                    break;
                            }
                        }

                        break;
                }
            }

            if (culture != null)
                culture.DoDetailsForCulture();
        }

        private void ReplaceStartNames(int count)
        {
            int i = _commonStartNames.Count;
            int c = count;
            for (int n = 0; n < c; n++)
                _commonStartNames.RemoveAt(Rand.Next(_commonStartNames.Count));

            while (_commonStartNames.Count < i)
                AddRandomStartNames(i - _commonStartNames.Count);
        }

        private void ReplaceMiddleNames(int count)
        {
            return;
        }

        private void ReplaceEndNames(int count)
        {
            int i = _commonEndNames.Count;
            int c = count;
            for (int n = 0; n < c; n++)
                _commonEndNames.RemoveAt(Rand.Next(_commonEndNames.Count));

            while (_commonEndNames.Count < i)
                AddRandomEndNames(i - _commonEndNames.Count);
        }

        private int AddRandomStartNames(int count)
        {
            int c = count;
            CulturalDna dna = CulturalDnaManager.instance.GetVanillaCulture((string)null);
            List<String> choices = new List<string>();
            int added = 0;
            for (int n = 0; n < c; n++)
            {
                String str = dna._commonStartNames[Rand.Next(dna._commonStartNames.Count)];
                if (_commonStartNames.Contains(str))
                {
                }
                else
                {
                    choices.Add(str);
                }
            }


            if (choices.Count > 0)
            {
                c = Math.Min(choices.Count, c);
                for (int n = 0; n < c; n++)
                {
                    var cc = choices[Rand.Next(choices.Count)];
                    _commonStartNames.Add(cc);
                    choices.Remove(cc);
                    n--;
                    c = Math.Min(choices.Count, c);
                    added++;
                }
            }


            return added;
        }
        private int AddRandomMiddleNames(int count)
        {
            int c = count;
            CulturalDna dna = CulturalDnaManager.instance.GetVanillaCulture((string)null);
            if (dna._commonMiddleNames.Count == 0)
                return 0;
            List<String> choices = new List<string>();
            int added = 0;
            for (int n = 0; n < c; n++)
            {
                String str = dna._commonMiddleNames[Rand.Next(dna._commonMiddleNames.Count)];
                if (_commonMiddleNames.Contains(str))
                {
                }
                else
                {
                    choices.Add(str);
                }
            }


            if (choices.Count > 0)
            {
                c = Math.Min(choices.Count, c);
                for (int n = 0; n < c; n++)
                {
                    var cc = choices[Rand.Next(choices.Count)];
                    _commonMiddleNames.Add(cc);
                    choices.Remove(cc);
                    n--;
                    c = Math.Min(choices.Count, c);
                    added++;
                }
            }


            return added;
        }

        private int AddRandomEndNames(int count)
        {
            int c = count;
            CulturalDna dna = CulturalDnaManager.instance.GetVanillaCulture((string)null);
            List<String> choices = new List<string>();
            int added = 0;
            for (int n = 0; n < c; n++)
            {
                String str = dna._commonEndNames[Rand.Next(dna._commonEndNames.Count)];
                if (_commonEndNames.Contains(str))
                {
                }
                else
                {
                    choices.Add(str);
                }
            }


            if (choices.Count > 0)
            {
                c = Math.Min(choices.Count, c);
                for (int n = 0; n < c; n++)
                {
                    var cc = choices[Rand.Next(choices.Count)];
                    _commonEndNames.Add(cc);
                    choices.Remove(cc);
                    n--;
                    c = Math.Min(choices.Count, c);
                    added++;
                }
            }


            return added;
        }

        public string GetMaleNameBlock()
        {
            StringBuilder b = new StringBuilder();
            for (int n = 0; n < 200; n++)
            {
                b.Append(GetMaleName() + " ");
            }
            return b.ToString();
        }

        public string GetFemaleNameBlock()
        {
            StringBuilder b = new StringBuilder();
            for (int n = 0; n < 200; n++)
            {
                b.Append(GetFemaleName() + " ");
            }
            return b.ToString();
        }
    }
}
