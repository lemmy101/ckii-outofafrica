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
    internal class GovernmentManager
    {
        public static GovernmentManager instance = new GovernmentManager();
        public List<Government> governments = new List<Government>();
        public void Init()
        {
        }

        public Government BranchGovernment(Government gov, CultureParser culture)
        {
            var newGov = gov.Mutate(10);

            {
                if (newGov.type == "nomadic")
                {
                    newGov.type = "tribal";
                    newGov.SetType(newGov.type);
                }
            }

            do
            {
                newGov.name = culture.dna.GetMaleName();
            } while (LanguageManager.instance.Get(StarNames.SafeName(newGov.name) + "_government") != null);

            string s = newGov.name;
            newGov.name = StarNames.SafeName(newGov.name) + "_government";
            LanguageManager.instance.Add(newGov.name, s);
            culture.Governments.Add(newGov);
            if (!newGov.cultureAllow.Contains(culture.Name))
                newGov.cultureAllow.Add(culture.Name);
            return newGov;
        }
        public void Save()
        {
            return;
            foreach (var cultureParser in CultureManager.instance.AllCultures)
            {
                foreach (var government in cultureParser.Governments)
                {
                    if (government.cultureAllow.Count == 0)
                    {
                        government.cultureAllow.Add(cultureParser.Name);
                    }
                }

                if (!Government.cultureDone.Contains(cultureParser.Name))
                {
                    cultureParser.Governments.Add(GovernmentManager.instance.CreateNewGovernment(cultureParser));
                }
            }

            if (!Directory.Exists(Globals.ModDir + "gfx/interface/"))
                Directory.CreateDirectory(Globals.ModDir + "gfx/interface/");
            var files = Directory.GetFiles(Globals.ModDir + "gfx/interface/");
            foreach (var file in files)
            {
                File.Delete(file);
            }


            foreach (var government in governments)
            {
                switch (government.type)
                {
                    case "nomadic":
                        try
                        {
                            File.Copy(Globals.GameDir + "gfx/interface/government_icon_nomadic.dds", Globals.ModDir + "gfx/interface/government_icon_" + government.name.Replace("government_", "") + ".dds");
                        }
                        catch (Exception)
                        {
                        }

                        SpriteManager.instance.AddGovernment(government);
                        break;
                    case "tribal":
                        try
                        {
                            File.Copy(Globals.GameDir + "gfx/interface/government_icon_tribal.dds", Globals.ModDir + "gfx/interface/government_icon_" + government.name.Replace("government_", "") + ".dds");
                        }
                        catch (Exception)
                        {
                        }
                        SpriteManager.instance.AddGovernment(government);

                        break;
                    case "feudal":
                        try
                        {
                            File.Copy(Globals.GameDir + "gfx/interface/government_icon_feudal.dds", Globals.ModDir + "gfx/interface/government_icon_" + government.name.Replace("government_", "") + ".dds");
                        }
                        catch (Exception)
                        {
                        }
                        SpriteManager.instance.AddGovernment(government);

                        break;
                    case "theocracy":
                        try
                        {
                            File.Copy(Globals.GameDir + "gfx/interface/government_icon_theocracy.dds", Globals.ModDir + "gfx/interface/government_icon_" + government.name.Replace("government_", "") + ".dds");
                        }
                        catch (Exception)
                        {
                        }

                        SpriteManager.instance.AddGovernment(government);

                        break;
                    case "republic":
                        try
                        {
                            File.Copy(Globals.GameDir + "gfx/interface/government_icon_republic.dds", Globals.ModDir + "gfx/interface/government_icon_" + government.name.Replace("government_", "") + ".dds");
                        }
                        catch (Exception)
                        {
                        }
                        SpriteManager.instance.AddGovernment(government);

                        break;
                }
            }


            Script s = new Script();

            s.Name = Globals.ModDir + "common/governments/nomadic_governments.txt";

            s.Root = new ScriptScope();

            var scope = new ScriptScope();
            scope.Name = "nomadic_governments";
            s.Root.Add(scope);
            foreach (var government in governments)
            {
                if (government.type == "nomadic" && government.cultureAllow.Count > 0)
                {
                    var g = new ScriptScope();
                    g.Name = government.name;
                    government.Save(g);
                    scope.Add(g);
                }
            }

            s.Save();

            s = new Script();

            s.Name = Globals.ModDir + "common/governments/feudal_governments.txt";

            s.Root = new ScriptScope();

            scope = new ScriptScope();
            scope.Name = "feudal_governments";
            s.Root.Add(scope);
            foreach (var government in governments)
            {
                if (government.type == "feudal" && government.cultureAllow.Count > 0)
                {
                    var g = new ScriptScope();
                    g.Name = government.name;
                    government.Save(g);
                    scope.Add(g);
                }
            }

            s.Save();
            s = new Script();

            s.Name = Globals.ModDir + "common/governments/theocracy_governments.txt";

            s.Root = new ScriptScope();

            scope = new ScriptScope();
            scope.Name = "theocracy_governments";
            s.Root.Add(scope);
            foreach (var government in governments)
            {
                if (government.type == "theocracy" && government.cultureAllow.Count > 0)
                {
                    var g = new ScriptScope();
                    g.Name = government.name;
                    government.Save(g);
                    scope.Add(g);
                }
            }

            s.Save();
            s = new Script();

            s.Name = Globals.ModDir + "common/governments/republic_governments.txt";

            s.Root = new ScriptScope();

            scope = new ScriptScope();
            scope.Name = "republic_governments";
            s.Root.Add(scope);
            foreach (var government in governments)
            {
                if (government.type == "republic" && government.cultureAllow.Count > 0)
                {
                    var g = new ScriptScope();
                    g.Name = government.name;
                    government.Save(g);
                    scope.Add(g);
                }
            }

            s.Save();
            s = new Script();

            s.Name = Globals.ModDir + "common/governments/tribal_governments.txt";

            s.Root = new ScriptScope();

            scope = new ScriptScope();
            scope.Name = "tribal_governments";
            s.Root.Add(scope);
            foreach (var government in governments)
            {
                if (government.type == "tribal" && government.cultureAllow.Count > 0)
                {
                    var g = new ScriptScope();
                    g.Name = government.name;
                    government.Save(g);
                    scope.Add(g);
                }
            }

            s.Save();
        }

        public int numNomadic = 0;
        public int numTribal = 0;

        public Government CreateNewGovernment(CultureParser culture)
        {
            Government g = new Government();
            g.type = "tribal";
            Government r = g.Mutate(8);
            r.name = culture.dna.GetMaleName();
            string s = r.name;
            r.name = StarNames.SafeName(r.name) + "_government";
            LanguageManager.instance.Add(r.name, s);
            culture.Governments.Add(r);
            r.SetType(r.type);
            if (!r.cultureAllow.Contains(culture.Name))
                r.cultureAllow.Add(culture.Name);   //    governments.Add(r);
            return r;
        }
    }
}
