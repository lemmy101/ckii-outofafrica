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
using CrusaderKingsStoryGen.Simulation;

namespace CrusaderKingsStoryGen
{
    internal class ReligionParser : Parser
    {
        public ReligionGroupParser Group
        {
            get
            {
                return new ReligionGroupParser(Scope.Parent);
            }
        }
        public List<ProvinceParser> Believers = new List<ProvinceParser>();
        private ProvinceParser _capital;
        public bool Modern { get; set; }
        public HashSet<ProvinceParser> holySites = new HashSet<ProvinceParser>();
        public ReligionParser(ScriptScope scope) : base(scope)
        {
            Name = scope.Name;
        }

        public override ScriptScope CreateScope()
        {
            return null;
        }

        public ReligionParser BranchReligion(String name)
        {
            ScriptScope scope = new ScriptScope();
            scope.Name = name;
            Group.Scope.Add(scope);
            ReligionParser r = new ReligionParser(scope);
            ReligionManager.instance.AllReligions.Add(r);
            return r;
        }


        public void RandomReligionProperties()
        {
            _divine_blood = Rand.Next(2) == 0; ;
            _female_temple_holders = Rand.Next(2) == 0;
            _priests_can_inherit = Rand.Next(2) == 0;



            _matrilineal_marriages = Rand.Next(4) != 0;

            bool warLike = Rand.Next(2) == 0;
            if (Rand.Next(2) == 0)
                Resilience = Rand.Next(2);
            else
                Resilience = Rand.Next(5);
            if (warLike)
            {
                _allow_looting = true;
                _allow_viking_invasion = true;
                _can_call_crusade = true;
                if (Rand.Next(2) == 0)
                {
                    _peace_prestige_loss = true;
                }
            }
            else
            {
                if (Rand.Next(5) == 0)
                {
                    _pacifist = true;
                }
                if (Rand.Next(2) == 0)
                    _can_call_crusade = false;
            }

            _polytheism = Rand.Next(2) == 0;
            //   this.polytheism = true;
            if (_polytheism)
                this.hasLeader = false;//Rand.Next(3) == 0;
            else
                this.hasLeader = true;

            _can_grant_claim = Rand.Next(3) != 0;
            _can_grant_divorce = Rand.Next(2) != 0;
            _can_excommunicate = Rand.Next(2) != 0;
            _can_hold_temples = Rand.Next(3) != 0;
            _can_retire_to_monastery = Rand.Next(2) != 0;
            _can_have_antipopes = Rand.Next(2) != 0 && hasLeader;
            _autocephaly = false;//Rand.Next(3) == 0;
            _investiture = Rand.Next(2) == 0 && hasLeader;
            this.icon = Rand.Next(52) + 1;
            this.heresy_icon = Rand.Next(52) + 1;
            if (Rand.Next(2) == 0)
                this.ai_convert_other_group = 0;
            else
                this.ai_convert_other_group = 2;

            _has_heir_designation = Rand.Next(4) == 0;

            if (Rand.Next(2) == 0)
            {
                if (Rand.Next(2) == 0)
                {
                    max_consorts = 1 + Rand.Next(5);
                }
                else
                {
                    {
                        max_wives = 2 + Rand.Next(4);
                    }
                }
            }


            if (Rand.Next(6) == 0)
            {
                _bs_marriage = true;
                if (Rand.Next(3) == 0)
                    _pc_marriage = true;
            }



            religious_clothing_head = Rand.Next(4);
            religious_clothing_priest = Rand.Next(4);
        }

        public void TryFillHolySites()
        {
            if (Believers.Count <= holySites.Count * 3)
                return;

            if (holySites.Count >= 5)
            {
                while (holySites.Count > 5)
                {
                    holySites.ToArray()[0].Title.Scope.Remove(holySites.ToArray()[0].Title.Scope.Find("holy_site"));
                    holySites.Remove(holySites.ToArray()[0]);
                }
                return;
            }

            {
                var chosen = Believers[Rand.Next(Believers.Count)];
                chosen.AddTemple(chosen.Title.Culture);
                chosen.Title.Scope.Add(new ScriptCommand("holy_site", Name, chosen.Scope));
                holySites.Add(chosen);
            }
        }
        private List<String> _gods = new List<string>();
        private List<String> _evilgods = new List<string>();

        public void CreateRandomReligion(ReligionGroupParser group)
        {
            String culture = "";
            CulturalDna dna = null;
            if (_capital == null)
            {
                dna = CulturalDnaManager.instance.GetVanillaCulture((string)null);
            }
            else
            {
                culture = _capital.Title.Holder.culture;
                dna = CultureManager.instance.CultureMap[culture].dna;
            }
            RandomReligionProperties();
            //     Modern = true;
            int r = Rand.Next(255);
            int g = Rand.Next(255);
            int b = Rand.Next(255);
            string god = dna.GetGodName();
            string devil = dna.GetPlaceName();
            string priest = dna.GetPlaceName();
            high_god_name = dna.GetGodName();
            string scripture_name = dna.GetPlaceName();
            string crusade_name = dna.GetPlaceName();

            this.high_god_name = god;

            _devil = devil;
            _priest = priest;
            _scripture_name = scripture_name;
            _crusade_name = crusade_name;

            DoReligionScope(god, devil, priest, scripture_name, crusade_name, dna, r, g, b);
        }

        private void DoReligionScope(string god, string devil, string priest, string scripture_name, string crusade_name,
            CulturalDna dna, int r, int g, int b)
        {
            string safegod = StarNames.SafeName(god);
            string safedevil = StarNames.SafeName(devil);
            string safepriest = StarNames.SafeName(priest);
            string safehigh_god_name = StarNames.SafeName(high_god_name);
            string safescripture_name = StarNames.SafeName(scripture_name);
            string safecrusade_name = StarNames.SafeName(crusade_name);
            String desc = "";

            if (!_polytheism)
                desc = "All praise the almighty " + high_god_name + "!";
            else
                desc = "The Gods smile upon you.";

            LanguageManager.instance.Add(this.Name + "_DESC", desc);

            LanguageManager.instance.Add(safegod, god);
            LanguageManager.instance.Add(safedevil, devil);
            LanguageManager.instance.Add(safepriest, priest);
            LanguageManager.instance.Add(safehigh_god_name, high_god_name);
            LanguageManager.instance.Add(safescripture_name, scripture_name);
            LanguageManager.instance.Add(safecrusade_name, crusade_name);

            this.r = r;
            this.g = g;
            this.b = b;

            String gods = "";
            {
                for (int n = 0; n < 10; n++)
                {
                    string go = dna.GetGodName();
                    var sg = StarNames.SafeName(go);
                    LanguageManager.instance.Add(sg, go);

                    _gods.Add(sg);
                    gods += sg + " ";
                }
            }

            String egods = "";

            for (int n = 0; n < 5; n++)
            {
                string go = dna.GetGodName();
                var sg = StarNames.SafeName(go);
                LanguageManager.instance.Add(sg, go);
                _evilgods.Add(sg);

                egods += sg + " ";
            }


            if (!_polytheism)
            {
                gods = safegod;
                egods = safedevil;
            }
            else
            {
                gods = safehigh_god_name + " " + gods;
            }


            if (max_wives > 1 && max_consorts > 0)
                max_consorts = 0;

            Scope.Do(@"

	            graphical_culture = westerngfx

		            icon = " + icon + @"
		            heresy_icon = " + heresy_icon + @"
		         
		            ai_convert_other_group = " + ai_convert_other_group + @" # always try to convert
	
		            color = { " + r + " " + g + " " + b + @" }
		
		            crusade_name = " + safecrusade_name + @"
		            scripture_name = " + safescripture_name + @"
		            priest_title = " + safepriest + @"
		
		            high_god_name = " + safehigh_god_name + @"
		
		            god_names = {
			            " + gods + @"
		            }
		
		            evil_god_names = {
			           " + egods + @"
		            }
		
		            investiture = " + (_investiture ? "yes" : "no") + @"
		            can_have_antipopes  = " + (_can_have_antipopes ? "yes" : "no") + @"
		            can_excommunicate  = " + (_can_excommunicate ? "yes" : "no") + @"
		            can_grant_divorce  = " + (_can_grant_divorce ? "yes" : "no") + @"
		            can_grant_claim  = " + (_can_grant_claim ? "yes" : "no") + @"
		            can_call_crusade  = " + (_can_call_crusade ? "yes" : "no") + @"
		            can_retire_to_monastery  = " + (_can_retire_to_monastery ? "yes" : "no") + @"
		            priests_can_inherit  = " + (_priests_can_inherit ? "yes" : "no") + @"
		            can_hold_temples  = " + (_can_hold_temples ? "yes" : "no") + @"
		            pacifist  = " + (_pacifist ? "yes" : "no") + @"
		           
                    bs_marriage  = " + (_bs_marriage ? "yes" : "no") + @"
		            pc_marriage  = " + (_pc_marriage ? "yes" : "no") + @"
		            psc_marriage  = " + (_psc_marriage ? "yes" : "no") + @"
		            cousin_marriage  = " + (_psc_marriage ? "yes" : "no") + @"
		            matrilineal_marriages  = " + (_matrilineal_marriages ? "yes" : "no") + @"
		            allow_viking_invasion  = " + (_allow_viking_invasion ? "yes" : "no") + @"
		            allow_looting  = " + (_allow_looting ? "yes" : "no") + @"
		            allow_rivermovement  = " + (_allow_rivermovement ? "yes" : "no") + @"
		            female_temple_holders  = " + (_female_temple_holders ? "yes" : "no") + @"
		            autocephaly  = " + (_autocephaly ? "yes" : "no") + @"
		            divine_blood  = " + (_divine_blood ? "yes" : "no") + @"
		            has_heir_designation  = " + (_has_heir_designation ? "yes" : "no") + @"
		            peace_prestige_loss  = " + (_peace_prestige_loss ? "yes" : "no") + @"
		         
		            " + (max_consorts > 0 ? ("max_consorts = " + max_consorts.ToString()) : "") + @"
		
                    max_wives  = " + max_wives + @"
		            uses_decadence = " + (uses_decadence ? "yes" : "no") + @"
                    uses_jizya_tax = " + (_uses_jizya_tax ? "yes" : "no") + @"
          
                    can_grant_invasion_cb = invasion
		            
		            religious_clothing_head = " + religious_clothing_head + @"
		            religious_clothing_priest = " + religious_clothing_priest + @"
");
        }

        public int icon = 0;
        public int heresy_icon = 0;
        public int ai_convert_other_group = 0;
        public int religious_clothing_head = 0;
        public int religious_clothing_priest = 1;
        public int max_wives = 1;
        public int max_consorts = 0;
        private bool _investiture = true;
        private bool _can_have_antipopes = true;
        private bool _can_excommunicate = true;
        private bool _can_grant_divorce = true;
        private bool _can_grant_claim = true;
        private bool _can_call_crusade = true;
        private bool _can_retire_to_monastery = true;
        private bool _priests_can_inherit = false;
        private bool _can_hold_temples = true;
        private bool _pacifist = false;
        private bool _bs_marriage = false;
        private bool _pc_marriage = false;
        private bool _psc_marriage = false;
        private bool _matrilineal_marriages = true;


        private bool _allow_viking_invasion = false;
        private bool _allow_looting = false;
        private bool _allow_rivermovement = false;
        private bool _female_temple_holders = false;
        private bool _autocephaly = false;
        private bool _divine_blood = false;
        private bool _has_heir_designation = false;
        private bool _peace_prestige_loss = false;
        public bool hasLeader = true;


        public void DoLeader(ProvinceParser capital)
        {
            String popeName = StarNames.SafeName(StarNames.Generate(10000000));

            LanguageManager.instance.Add(popeName, StarNames.Generate(10000000));

            TitleParser title = null;

            switch (Rand.Next(3))
            {
                case 0:
                    title = TitleManager.instance.CreateKingScriptScope(capital, Name);
                    break;
                case 1:
                    title = TitleManager.instance.CreateEmpireScriptScope(capital, Name);
                    break;
                case 2:
                    title = TitleManager.instance.CreateDukeScriptScope(capital, Name);
                    break;
            }

            ReligiousHeadTitle = title;
            ReligiousHeadTitle.Religious = true;
            ReligiousHeadTitle.Active = true;
            //  ch.UpdateCultural();
            var ch = SimulationManager.instance.AddCharacterForTitle(ReligiousHeadTitle, true);
            ch.religion = Name;
            ch.UpdateCultural();
            var liege = ReligiousHeadTitle.CapitalProvince.Title;
            if (Rand.Next(3) == 0)
                ch.GiveTitle(ReligiousHeadTitle.CapitalProvince.Title);
            String religious_names = "";

            for (int n = 0; n < 40; n++)
            {
                religious_names = CultureManager.instance.CultureMap[ch.culture].dna.GetMaleName() + " ";
            }
            title.Scope.Do(@"

	        title = """ + popeName + @"""
	        foa = ""POPE_FOA""
	        short_name = yes
	        location_ruler_title = yes
	        landless = yes
	        controls_religion = """ + Name + @"""
	        religion = """ + Name + @"""
	        primary = yes
	        dynasty_title_names = no
	        male_names = {
			        " + religious_names + @"
        }

");



            LanguageManager.instance.Add(title.Name, this.LanguageName);
        }
        public TitleParser ReligiousHeadTitle { get; set; }

        public string LanguageName { get; set; }
        public int Resilience = 0;
        private bool _polytheism = true;
        public string high_god_name;
        private string _devil;
        private string _priest;
        private string _scripture_name;
        private string _crusade_name;
        public int r;
        public int g;
        public int b;
        private CulturalDna _dna;
        public bool uses_decadence;
        private bool _uses_jizya_tax;

        public void MakeChange()
        {
            switch (Rand.Next(24))
            {
                case 0:
                    {
                        bool warLike = Rand.Next(2) == 0;
                        if (Rand.Next(2) == 0)
                            Resilience = Rand.Next(2);
                        else
                            Resilience = Rand.Next(5);
                        if (warLike)
                        {
                            _allow_looting = true;
                            _allow_viking_invasion = true;
                            _can_call_crusade = true;
                            if (Rand.Next(2) == 0)
                            {
                                _peace_prestige_loss = true;
                            }
                        }
                        else
                        {
                            if (Rand.Next(5) == 0)
                            {
                                _pacifist = true;
                            }
                            if (Rand.Next(2) == 0)
                                _can_call_crusade = false;
                        }
                    }
                    break;
                case 1:
                    _can_grant_claim = Rand.Next(3) != 0;
                    break;
                case 2:
                    _can_grant_divorce = Rand.Next(2) != 0;
                    break;
                case 3:
                    _can_excommunicate = Rand.Next(2) != 0;
                    break;
                case 4:
                    _can_hold_temples = Rand.Next(3) != 0;
                    break;
                case 5:
                    _can_retire_to_monastery = Rand.Next(2) != 0;
                    break;
                case 6:
                    _can_have_antipopes = Rand.Next(2) != 0 && hasLeader;
                    break;
                case 7:
                    _investiture = Rand.Next(2) == 0 && hasLeader;
                    break;
                case 8:
                    if (Rand.Next(2) == 0)
                        this.ai_convert_other_group = 0;
                    else
                        this.ai_convert_other_group = 2;
                    break;
                case 9:

                    _has_heir_designation = Rand.Next(4) == 0;

                    break;
                case 10:

                    if (Rand.Next(2) == 0)
                    {
                        if (Rand.Next(2) == 0)
                        {
                            max_consorts = 1 + Rand.Next(5);
                        }
                        else
                        {
                            {
                                max_wives = 2 + Rand.Next(4);
                            }
                        }
                    }


                    break;
                case 11:

                    if (Rand.Next(6) == 0)
                    {
                        _bs_marriage = true;
                        if (Rand.Next(3) == 0)
                            _pc_marriage = true;
                    }


                    break;
                case 12:

                    religious_clothing_head = Rand.Next(4);
                    religious_clothing_priest = Rand.Next(4);



                    break;
                case 13:

                    high_god_name = _dna.GetGodName();

                    break;
                case 14:

                    _devil = _dna.GetGodName();

                    break;
                case 15:

                    _scripture_name = _dna.GetGodName();

                    break;
                case 16:

                    _crusade_name = _dna.GetGodName();

                    break;
                case 17:

                    _priest = _dna.GetGodName();

                    break;
                case 18:

                    _matrilineal_marriages = !_matrilineal_marriages;

                    break;
                case 19:

                    _divine_blood = Rand.Next(2) == 0; ;

                    break;
                case 20:

                    _female_temple_holders = !_female_temple_holders;

                    break;
                case 21:
                    _priests_can_inherit = !_priests_can_inherit;

                    break;
                case 22:
                    this.uses_decadence = !uses_decadence;

                    break;
                case 23:
                    _uses_jizya_tax = !_uses_jizya_tax;

                    break;
            }
        }
        public void Mutate(ReligionParser rel, CultureParser culture, int nChanges)
        {
            _dna = culture.dna;
            this.ai_convert_other_group = rel.ai_convert_other_group;

            this.max_consorts = rel.max_consorts;
            this.max_wives = rel.max_wives;
            this.religious_clothing_head = rel.religious_clothing_head;
            this.religious_clothing_priest = rel.religious_clothing_priest;
            _allow_looting = rel._allow_looting;
            _allow_rivermovement = rel._allow_rivermovement;
            _allow_viking_invasion = rel._allow_viking_invasion;
            _autocephaly = rel._autocephaly;
            _bs_marriage = rel._bs_marriage;
            _can_call_crusade = rel._can_call_crusade;
            _can_excommunicate = rel._can_excommunicate;
            _can_grant_claim = rel._can_grant_claim;
            _can_grant_divorce = rel._can_grant_divorce;
            _can_have_antipopes = rel._can_have_antipopes;
            _can_hold_temples = rel._can_hold_temples;
            _can_retire_to_monastery = rel._can_retire_to_monastery;
            _divine_blood = rel._divine_blood;
            _female_temple_holders = rel._female_temple_holders;
            this.hasLeader = rel.hasLeader;
            _has_heir_designation = rel._has_heir_designation;
            _investiture = rel._investiture;
            _matrilineal_marriages = rel._matrilineal_marriages;
            _pacifist = rel._pacifist;
            _pc_marriage = rel._pc_marriage;
            _peace_prestige_loss = rel._peace_prestige_loss;
            _polytheism = rel._polytheism;
            _priests_can_inherit = rel._priests_can_inherit;
            _psc_marriage = rel._psc_marriage;

            high_god_name = rel.high_god_name;
            _devil = rel._devil;
            _priest = rel._priest;

            _gods.AddRange(rel._gods);


            int r = Rand.Next(255);
            int g = Rand.Next(255);
            int b = Rand.Next(255);

            r = rel.r;
            g = rel.g;
            b = rel.b;
            switch (Rand.Next(3))
            {
                case 0:
                    r += Rand.Next(-45, 45);
                    g += Rand.Next(-25, 25);
                    b += Rand.Next(-15, 15);

                    break;
                case 1:
                    g += Rand.Next(-45, 45);
                    r += Rand.Next(-25, 25);
                    b += Rand.Next(-15, 15);

                    break;
                case 2:
                    b += Rand.Next(-45, 45);
                    g += Rand.Next(-25, 25);
                    r += Rand.Next(-15, 15);

                    break;
            }
            if (r > 255)
                r = 255;
            if (g > 255)
                g = 255;
            if (b > 255)
                b = 255;

            if (r < 0)
                r = 0;
            if (g < 0)
                g = 0;
            if (b < 0)
                b = 0;

            for (int n = 0; n < nChanges; n++)
                MakeChange();

            DoReligionScope(high_god_name, _devil, _priest, _scripture_name, _crusade_name, culture.dna, r, g, b);
        }
    }
}