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
using CrusaderKingsStoryGen.Simulation;

namespace CrusaderKingsStoryGen
{
    internal class ReligionManager
    {
        public static ReligionManager instance = new ReligionManager();
        private Script _script;
        public List<ReligionParser> AllReligions = new List<ReligionParser>();
        public Dictionary<String, ReligionParser> ReligionMap = new Dictionary<String, ReligionParser>();
        public List<ReligionGroupParser> AllReligionGroups = new List<ReligionGroupParser>();

        public ReligionManager()
        {
        }

        public Script Script
        {
            get { return _script; }
            set { _script = value; }
        }

        public Dictionary<String, ReligionGroupParser> GroupMap = new Dictionary<string, ReligionGroupParser>();

        public ReligionGroupParser AddReligionGroup(String name)
        {
            ScriptScope scope = new ScriptScope();
            scope.Name = name;

            _script.Root.Add(scope);

            ReligionGroupParser r = new ReligionGroupParser(scope);

            r.Init();
            GroupMap[name] = r;
            AllReligionGroups.Add(r);
            return r;
        }
        public void Save()
        {
            foreach (var religionParser in AllReligions)
            {
                if (religionParser.ReligiousHeadTitle != null)
                {
                    CharacterParser chr = CharacterManager.instance.GetNewCharacter();
                    chr.GiveTitle(religionParser.ReligiousHeadTitle);

                    chr.religion = religionParser.Name;
                }
            }

            _script.Save();
        }

        public void Init()
        {
            LanguageManager.instance.Add("norse", StarNames.Generate(Rand.Next(1000000)));
            LanguageManager.instance.Add("pagan", StarNames.Generate(Rand.Next(1000000)));
            LanguageManager.instance.Add("christian", StarNames.Generate(Rand.Next(1000000)));

            Script s = new Script();
            _script = s;
            s.Name = Globals.ModDir + "common/religions/00_religions.txt";
            s.Root = new ScriptScope();
            ReligionGroupParser r = AddReligionGroup("pagan");
            r.Init();
            var pagan = r.AddReligion("pagan");

            pagan.CreateRandomReligion(null);

            AllReligionGroups.Add(r);
            s.Save();
        }

        public ReligionParser BranchReligion(string religion, ProvinceParser capital = null)
        {
            var rel = this.ReligionMap[religion];
            var group = rel.Group;

            if (rel.Group.Religions.Count > 3 && Rand.Next(2) == 0)
            {
                String name = StarNames.Generate(capital.Title.culture);
                String safe = StarNames.SafeName(name);
                LanguageManager.instance.Add(safe, name);
                group = AddReligionGroup(safe);
                var rel2 = group.AddReligion(StarNames.Generate(capital.Title.culture));
                rel2.RandomReligionProperties();
                //rel2.Init(Rand.Next(255), Rand.Next(255), Rand.Next(255));
                rel2.CreateRandomReligion(group);
                return rel2;
            }
            else
            {
                var rel2 = group.AddReligion(StarNames.Generate(capital.Title.culture));
                //rel2.Init(Rand.Next(255), Rand.Next(255), Rand.Next(255));
                rel2.RandomReligionProperties();
                rel2.CreateRandomReligion(group);
                rel2.Mutate(rel, capital.Title.Culture, 6);
                return rel2;
            }
        }
    }
}
