using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionSnoop.Enums
{
    using System.Runtime.Serialization;

    public enum BozjaActions
    {
        Paralyze = 1,
        Banish = 2,
        Manawall = 3,
        Dispel = 4,
        Stealth = 5,
        Spellforge = 6,
        Steelsting = 7,
        Swift = 8,

        [EnumMember(Value = "Protect I")]
        Protect_I = 9,

        [EnumMember(Value = "Shell I")]
        Shell_I = 10,
        Reflect = 11,
        Stoneskin = 12,
        Bravery = 13,
        Focus = 14,

        [EnumMember(Value = "Font of Magic")]
        Font_of_Magic = 15,

        [EnumMember(Value = "Font of Skill")]
        Font_of_Skill = 16,

        [EnumMember(Value = "Font of Power")]
        Font_of_Power = 17,
        Slash = 18,
        Death = 19,

        [EnumMember(Value = "Noble Ends")]
        Noble_Ends = 20,

        [EnumMember(Value = "Honored Sacrifice")]
        Honored_Sacrifice = 21,

        [EnumMember(Value = "Tireless Conviction")]
        Tireless_Conviction = 22,

        [EnumMember(Value = "Firm Resolve")]
        Firm_Resolve = 23,

        [EnumMember(Value = "Solemn Clarity")]
        Solemn_Clarity = 24,

        [EnumMember(Value = "Honed Acuity")]
        Honed_Acuity = 25,

        [EnumMember(Value = "Cure I")]
        Cure_I = 26,

        [EnumMember(Value = "Cure II")]
        Cure_II = 27,

        [EnumMember(Value = "Cure III")]
        Cure_III = 28,

        [EnumMember(Value = "Cure IV")]
        Cure_IV = 29,
        Arise = 30,
        Incense = 31,

        [EnumMember(Value = "Fair Trade")]
        Fair_Trade = 32,
        Mimic = 33,
        Percept = 71,
        Sacrifice = 72,

        [EnumMember(Value = "Flare Star")]
        Flare_Star = 79,

        [EnumMember(Value = "Rend Armor")]
        Rend_Armor = 80,

        [EnumMember(Value = "Seraph Strike")]
        Seraph_Strike = 81,
        Aethershield = 82,
        Dervish = 83,

        [EnumMember(Value = "Stoneskin II")]
        Stoneskin_II = 85,
        Burst = 86,
        Rampage = 87,
        Reraise = 89,
        Chainspell = 90,
        Assassinate = 91,

        [EnumMember(Value = "Protect II")]
        Protect_II = 92,

        [EnumMember(Value = "Shell II")]
        Shell_II = 93,
        Bubble = 94,
        Impetus = 95,
        Excellence = 96,

        [EnumMember(Value = "Full Cure")]
        Full_Cure = 97,
        Bloodrage = 98
    }

}
