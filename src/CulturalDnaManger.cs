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
using System.Threading.Tasks;

namespace CrusaderKingsStoryGen
{
    internal class CulturalDnaManager
    {
        public static CulturalDnaManager instance = new CulturalDnaManager();
        public Dictionary<String, CulturalDna> dna = new Dictionary<string, CulturalDna>();
        public List<string> dnaTypes = new List<string>();
        public CulturalDna GetVanillaCulture(String culture)
        {
            if (culture == null)
            {
                culture = dnaTypes[Rand.Next(dnaTypes.Count)];
            }

            return this.dna[culture]; ;
        }
        public CulturalDna GetVanillaCulture(CulturalDna not)
        {
            String culture = dnaTypes[Rand.Next(dnaTypes.Count)];

            while (this.dna[culture] == not)
            {
                culture = dnaTypes[Rand.Next(dnaTypes.Count)];
            }
            return this.dna[culture]; ;
        }
        public CulturalDna GetNewFromVanillaCulture(String culture = null)
        {
            if (culture == null)
            {
                culture = dnaTypes[Rand.Next(dnaTypes.Count)];
            }
            CulturalDna dna = this.dna[culture];
            dna.culture = null;
            CulturalDna dna2 = dna.Mutate(256);
            dna2.DoRandom();
            return dna2;
        }

        public void Init()
        {
            Script s = ScriptLoader.instance.Load(Globals.GameDir + "common/cultures/00_cultures.txt");
            foreach (var child in s.Root.Children)
            {
                if (child is ScriptScope)
                {
                    ScriptScope cc = (ScriptScope)child;

                    foreach (var scriptScope in cc.Scopes)
                    {
                        if (scriptScope.Name == "graphical_cultures")
                            continue;

                        CulturalDna dna = new CulturalDna();
                        foreach (var scope in scriptScope.Scopes)
                        {
                            if (scope.Name == "male_names" || scope.Name == "female_names")
                            {
                                String[] male_names = scope.Data.Split(new[] { ' ', '_', '\t' });
                                foreach (var maleName in male_names)
                                {
                                    var mName = maleName.Trim();
                                    if (mName.Length > 0)
                                        dna.Cannibalize(mName);
                                }
                            }
                        }

                        this.dna[scriptScope.Name] = dna;
                        dnaTypes.Add(scriptScope.Name);

                        {
                        }

                        dna.Name = scriptScope.Name;
                        dna.ShortenWordCounts();
                    }
                }
            }
        }
    }
}
