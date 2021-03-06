﻿// Out of Africa - A random world generator for Crusader Kings II
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
    public class Rand
    {
        internal static Random rand = new Random();

        public static int Next(int max)
        {
            return rand.Next(max);
        }

        public static int Next(int minSize, int maxSize)
        {
            return rand.Next(minSize, maxSize);
        }

        public static void SetSeed(int i)
        {
            rand = new Random(i);
        }
        public static void SetSeed()
        {
            rand = new Random();
        }

        public static Random Get()
        {
            return rand;
        }
    }
}
