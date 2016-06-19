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
    internal class LanguageManager
    {
        public static LanguageManager instance = new LanguageManager();
        private Dictionary<String, String> _english = new Dictionary<string, string>();

        public LanguageManager()
        {
        }
        public String Add(String key, String english)
        {
            _english[key] = english;
            return english;
        }

        public void Save()
        {
            String filename = "localisation/aaa_genLanguage.csv";

            filename = Globals.ModDir + filename;

            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(filename, false, Encoding.GetEncoding(1252)))
            {
                foreach (var entry in _english)
                {
                    file.Write(entry.Key + ";" + entry.Value + ";;;;;;;;;;;;;\n");
                }

                file.Close();
            }

            //thing;eng;;;;;;;;;;;;;
        }

        public void Remove(string name)
        {
            _english.Remove(name);
        }

        public string Get(string name)
        {
            if (!_english.ContainsKey(name))
                return null;
            return _english[name];
        }

        public string AddSafe(string name)
        {
            String safe = StarNames.SafeName(name);
            Add(safe, name);
            return safe;
        }
    }
}
