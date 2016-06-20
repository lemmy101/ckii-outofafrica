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
    internal class Globals
    {
        public static string SrcTraitIconDir =
             "C:/Users/LEMMYMAIN/Documents/Paradox Interactive/Crusader Kings II/mod/storygen/_data/";
        public static string GameDir =
             "C:/Program Files (x86)/Steam/steamapps/common/Crusader Kings II/";
        //     public static string MapDir =
        //   "C:/Users/LEMMYMAIN/Documents/Paradox Interactive/Crusader Kings II/mod/A Game of Thrones/";
        //   public static string MapDir =
        //     "C:/Users/LEMMYMAIN/Documents/Paradox Interactive/Crusader Kings II/mod/Historical Immersion Project/";
        // C:\Users\LEMMYMAIN\Documents\Paradox Interactive\Crusader Kings II\mod\Historical Immersion Project
        public static string MapDir =
          "C:/Program Files (x86)/Steam/steamapps/common/Crusader Kings II/";
        public static string ModDir = "C:/Users/LEMMYMAIN/Documents/Paradox Interactive/Crusader Kings II/mod/storygen/";
        public static string OModDir = "C:/Users/LEMMYMAIN/Documents/Paradox Interactive/Crusader Kings II/mod/storygen/";
        public static string UserDir = "C:/Users/LEMMYMAIN/Documents/Paradox Interactive/Crusader Kings II/storygen/";
        public static int OneInChanceOfReligionSplinter = 1;
        // public static int OneInChanceOfReligionReformed = 2;
        //  public static int OneInChanceOfReligionModern = 2;
        public static float BaseChanceOfRevolt = 1000.0f;
        //     public static float BaseChanceOfRevolt = 70000.0f;

        public static int OneInChanceOfCultureSplinter = 1;
        public static string ModRoot { get; set; }
        public static string ModName { get; set; }

        public static int StartYear { get; set; } = 1066;
    }
}
