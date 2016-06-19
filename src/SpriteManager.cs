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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusaderKingsStoryGen
{
    internal class SpriteManager
    {
        public static SpriteManager instance = new SpriteManager();
        public Script script = new Script();

        public ScriptScope spriteTypes = new ScriptScope();

        public void Init()
        {
            if (!Directory.Exists(Globals.ModDir + "interface"))
                Directory.CreateDirectory(Globals.ModDir + "interface");
            var files = Directory.GetFiles(Globals.ModDir + "interface");
            foreach (var file in files)
            {
                File.Delete(file);
            }

            script.Root = new ScriptScope();
            var s = new ScriptScope();
            s.Name = "spriteTypes";
            spriteTypes = s;
            script.Root.Add(s);
            script.Name = Globals.ModDir + "interface/genGraphics.gfx";
        }

        public void AddTraitSprite(String name, String relFilename)
        {
            var scope = new ScriptScope();

            scope.Name = "spriteType";

            scope.Do(@"
                	        name = ""GFX_trait_" + name + @"
		                    texturefile = " + relFilename + @"
		                    noOfFrames = 1
		                    norefcount = yes
		                    effectFile = ""gfx/FX/buttonstate.lua""");

            spriteTypes.Add(scope);
        }

        public void Save()
        {
            script.Save();
        }

        public void AddGovernment(Government government)
        {
            var scope = new ScriptScope();

            scope.Name = "spriteType";

            scope.Do(@"
                	        name = ""GFX_icon_" + government.name + @"
		                    texturefile = gfx/interface/government_icon_" + government.name.Replace("_government", "") + ".dds");

            spriteTypes.Add(scope);
        }
    }
}
