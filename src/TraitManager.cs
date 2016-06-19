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
    internal class TraitManager
    {
        public static TraitManager instance = new TraitManager();

        public void Init()
        {
            if (!Directory.Exists(Globals.ModDir + "gfx/traits"))
                Directory.CreateDirectory(Globals.ModDir + "gfx/traits");
            var files = Directory.GetFiles(Globals.ModDir + "gfx/traits");
            foreach (var file in files)
            {
                File.Delete(file);
            }
        }
        public void AddTrait(String name, String srcFilename)
        {
            String srcend = srcFilename.Substring(srcFilename.LastIndexOf('.'));
            File.Copy(Globals.SrcTraitIconDir + srcFilename, Globals.ModDir + "gfx/traits/" + name + srcend);
            SpriteManager.instance.AddTraitSprite(name, "gfx/traits/" + name + srcend);
        }

        public void AddTrait(string safeName)
        {
            var files = Directory.GetFiles(Globals.SrcTraitIconDir);


            String s = files[Rand.Next(files.Length)];

            String srcend = s.Substring(s.LastIndexOf('/') + 1);
            AddTrait(safeName, srcend);
        }
    }
}
