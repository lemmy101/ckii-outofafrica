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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusaderKingsStoryGen
{
    internal class RegionManager
    {
        public static RegionManager instance = new RegionManager();

        internal class Region
        {
            public String name;
            public List<TitleParser> duchies = new List<TitleParser>();
        }
        public List<TitleParser> duchiesAssigned = new List<TitleParser>();

        public List<Region> regions = new List<Region>();
        public void AddRegion(String name, List<TitleParser> duchies)
        {
            String safeName = StarNames.SafeName(name);

            LanguageManager.instance.Add(safeName, name);
            Region r = new Region();
            for (int index = 0; index < duchies.Count; index++)
            {
                var titleParser = duchies[index];
                if (duchiesAssigned.Contains(titleParser))
                {
                    duchies.Remove(titleParser);
                    index--;
                }
            }
            r.name = name;
            r.duchies.AddRange(duchies);
            duchiesAssigned.AddRange(duchies);
            regions.Add(r);
        }

        public void Save()
        {
            Script s = new Script();
            s.Name = Globals.GameDir + "map/geographical_region.txt";
            s.Root = new ScriptScope();
            foreach (var region in regions)
            {
                ScriptScope ss = new ScriptScope();

                String duchieList = "";

                foreach (var titleParser in region.duchies)
                {
                    duchieList = duchieList + " " + titleParser.Name;
                }
                ss.Name = StarNames.SafeName(region.name);
                ss.Do(@"
                    duchies = {
                        " + duchieList + @"
                    }
");
                s.Root.Add(ss);
            }

            s.Save();
        }
    }
}
