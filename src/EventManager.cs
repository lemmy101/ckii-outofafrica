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
    internal class EventNamespace
    {
        public String name;
        public List<EventParser> events = new List<EventParser>();
        public Dictionary<String, EventParser> eventmap = new Dictionary<String, EventParser>();

        public EventNamespace(String name)
        {
            this.name = name;
        }
    }
    internal class EventManager
    {
        public static EventManager instance = new EventManager();
        public List<Script> Scripts = new List<Script>();
        public void AddToNamespace(String names, EventParser ev)
        {
            EventNamespace e = null;
            if (!NamespaceMap.ContainsKey(names))
            {
                e = new EventNamespace(names);
                NamespaceMap[names] = e;
            }
            else
            {
                e = NamespaceMap[names];
            }

            e.events.Add(ev);
            e.eventmap[ev.GetProperty("id").Value.ToString().Replace(names + ".", "")] = ev;
        }
        public void Load()
        {
            var files = Directory.GetFiles(Globals.GameDir + "events");

            foreach (var file in files)
            {
                Script s = ScriptLoader.instance.Load(file);
                string names = null;
                foreach (var child in s.Root.Children)
                {
                    if (child is ScriptCommand)
                    {
                        if (child.ToString().Contains("namespace"))
                        {
                            names = (child as ScriptCommand).Value.ToString();
                        }
                        continue;
                    }
                    var sc = (child as ScriptScope);
                    EventParser ee = new EventParser(sc);
                    Events.Add(ee);

                    EventMap[(sc.Find("id") as ScriptCommand).Value.ToString()] = ee;
                    if (names != null)
                        AddToNamespace(names, ee);
                }
                Scripts.Add(s);
            }

            foreach (var scriptScope in Events)
            {
                scriptScope.FindLinks();
            }

            var ev = EventMap["TOG.1200"];
        }

        public void Save()
        {
            foreach (var script in Scripts)
            {
                //    RemoveAllReligionTests(script.Root);
                //        script.Save();
            }
        }

        public List<EventParser> Events = new List<EventParser>();
        public Dictionary<string, EventParser> EventMap = new Dictionary<string, EventParser>();
        public Dictionary<string, EventNamespace> NamespaceMap = new Dictionary<string, EventNamespace>();

        private void RemoveAllReligionTests(ScriptScope node)
        {
            for (int index = 0; index < node.Children.Count; index++)
            {
                var child = node.Children[index];
                if (child is ScriptScope)
                    RemoveAllReligionTests(child as ScriptScope);
                if (child is ScriptCommand)
                {
                    ScriptCommand c = (ScriptCommand)child;
                    if (c.Name == "religion" || c.Name == "religion_group")
                    {
                        if (c.Value.ToString().ToUpper() == "FROM" || c.Value.ToString().ToUpper() == "ROOT" || c.Value.ToString().ToUpper().Contains("PREV"))
                        {
                        }
                        else
                        {
                            node.Remove(child);
                            index--;
                        }
                        continue;
                    }
                    if (c.Name == "culture" || c.Name == "culture_group")
                    {
                        if (c.Value.ToString().ToUpper() == "FROM" || c.Value.ToString().ToUpper() == "ROOT" || c.Value.ToString().ToUpper().Contains("PREV"))
                        {
                        }
                        else
                        {
                            node.Remove(child);
                            index--;
                        }
                        continue;
                    }
                }
            }
        }

        public EventParser GetEvent(string id)
        {
            if (!EventMap.ContainsKey(id))
                return null;

            return EventMap[id];
        }
    }
}
