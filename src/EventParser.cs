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
    internal class EventParser : Parser
    {
        public List<EventParser> LinkedEvents = new List<EventParser>();
        public List<String> LinkedEventIDs = new List<String>();
        public EventParser(ScriptScope scope)
            : base(scope)
        {
            Trigger = scope.Find("trigger") as ScriptScope;
            MeanTime = scope.Find("mean_time_to_happen") as ScriptScope;
        }

        public void FindLinks()
        {
            ExamineEvent(Scope);
        }
        private void ExamineEvent(ScriptScope node)
        {
            for (int index = 0; index < node.Children.Count; index++)
            {
                var child = node.Children[index];
                if (child is ScriptScope)
                    ExamineEvent(child as ScriptScope);
                if (child is ScriptCommand)
                {
                    ScriptCommand c = child as ScriptCommand;
                    if (c.Name == "id" && node.Name == "character_event")
                    {
                        if (!LinkedEventIDs.Contains(c.Value.ToString()))
                        {
                            var e = (EventManager.instance.GetEvent(c.Value.ToString()));
                            if (e != null)
                            {
                                LinkedEvents.Add(e);
                                LinkedEventIDs.Add(c.Value.ToString());
                            }
                        }
                    }
                }
            }
        }

        public ScriptScope Trigger { get; set; }
        public ScriptScope MeanTime { get; set; }

        public override ScriptScope CreateScope()
        {
            return null;
        }
    }
}
