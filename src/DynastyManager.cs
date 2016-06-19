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
    internal class Dynasty
    {
        public int ID;
        public List<CharacterParser> Members = new List<CharacterParser>();
        public ScriptScope Scope;
    }
    internal class DynastyManager
    {
        public static DynastyManager instance = new DynastyManager();
        public int ID = 1;
        public void Init()
        {
            Script s = new Script();
            script = s;
            s.Name = Globals.ModDir + "common/dynasties/dynasties.txt";
            s.Root = new ScriptScope();
        }
        public Dictionary<int, Dynasty> DynastyMap = new Dictionary<int, Dynasty>();
        public void Save()
        {
            script.Save();
        }
        public Dynasty GetDynasty(CultureParser culture)
        {
            ScriptScope scope = new ScriptScope();
            scope.Name = ID.ToString();
            ID++;
            scope.Add(new ScriptCommand("name", culture.dna.GetDynastyName(), scope));
            scope.Add(new ScriptCommand("culture", culture.Name, scope));
            script.Root.Add(scope);
            var d = new Dynasty() { ID = ID - 1, Scope = scope };
            DynastyMap[ID - 1] = d;
            culture.Dynasties.Add(d);
            return d;
        }
        public Script script { get; set; }
    }
}
