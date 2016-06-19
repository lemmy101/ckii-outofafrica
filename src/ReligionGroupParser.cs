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

namespace CrusaderKingsStoryGen
{
    internal class ReligionGroupParser : Parser
    {
        public List<ReligionParser> Religions = new List<ReligionParser>();

        public ReligionGroupParser(ScriptScope scope)
            : base(scope)
        {
            foreach (var scriptScope in scope.Scopes)
            {
                if (ReligionManager.instance.ReligionMap.ContainsKey(scriptScope.Name))
                    Religions.Add(ReligionManager.instance.ReligionMap[scriptScope.Name]);
            }
        }

        public override ScriptScope CreateScope()
        {
            return null;
        }

        public void RemoveReligion(String name)
        {
            var r = ReligionManager.instance.ReligionMap[name];
            Religions.Remove(r);
            Scope.Remove(r);
        }
        public void AddReligion(ReligionParser r)
        {
            if (r.Group != null)
            {
                r.Group.Scope.Remove(r.Scope);
            }
            Scope.Add(r.Scope);
            Religions.Add(r);
        }
        public ReligionParser AddReligion(String name, String orig = null)
        {
            if (name != "pagan")
            {
                String oname = name;
                name = StarNames.SafeName(name);
                LanguageManager.instance.Add(name, oname);
                orig = oname;
            }



            ScriptScope scope = new ScriptScope();
            scope.Name = name;
            Scope.Add(scope);
            ReligionParser r = new ReligionParser(scope);
            ReligionManager.instance.AllReligions.Add(r);
            if (orig != null)
            {
                r.LanguageName = orig;
            }
            Religions.Add(r);
            ReligionManager.instance.ReligionMap[name] = r;
            return r;
        }
        private static string[] s_gfx = new string[]
            {
                "muslimgfx",
                "westerngfx",
                "norsegfx",
                "mongolgfx",
                "mesoamericangfx",
                "africangfx",
                "persiangfx",
                "jewishgfx",
                "indiangfx",
                "hindugfx",
                "buddhistgfx",
                "jaingfx",
            };

        public bool hostile_within_group = false;
        public void Init()
        {
            hostile_within_group = Rand.Next(2) == 0;
            String g = s_gfx[Rand.Next(s_gfx.Count())];
            Scope.Clear();
            Scope.Do(@"

	            has_coa_on_barony_only = yes
	            graphical_culture = " + g + @"
	            playable = yes
	            hostile_within_group = " + (hostile_within_group ? "yes" : "no") + @"
	
	            # Names given only to Pagan characters (base names)
	            male_names = {
		            Anund Asbjörn Aslak Audun Bagge Balder Brage Egil Emund Frej Gnupa Gorm Gudmund Gudröd Hardeknud Helge Odd Orm 
		            Orvar Ottar Rikulfr Rurik Sigbjörn Styrbjörn Starkad Styrkar Sämund Sölve Sörkver Thorolf Tjudmund Toke Tolir 
		            Torbjörn Torbrand Torfinn Torgeir Toste Tyke
	            }
	            female_names = {
		            Aslaug Bothild Björg Freja Grima Gytha Kráka Malmfrid Thora Thordis Thyra Ragnfrid Ragnhild Svanhild Ulvhilde
	            }

");
        }
    }
}