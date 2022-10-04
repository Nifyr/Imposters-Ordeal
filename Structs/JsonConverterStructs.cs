using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpostersOrdeal
{
    /// <summary>
    /// Container for more human-readable structs to make json editing easier.
    /// </summary>
    public static class JsonConverterStructs
    {
        public struct Pokemon
        {
            public PokemonReadOnly readOnly;
            public bool validFlag;
            public string color;
            public ushort graNo;
            public BaseStats baseStats;
            public List<string> typing;
            public byte catchRate;
            public byte rank;
            public EvYield evYield;
            public HeldItems heldItems;
            public byte sexValue;
            public byte eggCycles;
            public byte initialFriendship;
            public List<string> eggGroups;
            public string growthRate;
            public Abilities abilities;
            public ushort expYield;
            public ushort cmHeight;
            public ushort hgWeight;
            public ushort sinnohDexNumber;
            public List<string> compatibleTMs;
            public string eggTarget;
            public ushort eggTargetFormno;
            public ushort eggFormnoKawarazunoishi;
            public byte eggFormInheritKawarazunoishi;
            public List<LevelUpMove> levelUpMoves;
            public List<string> eggMoves;
            public List<EvolutionPath> evolutions;
        }

        public struct PokemonReadOnly
        {
            public ushort personalID;
            public ushort dexID;
            public ushort formIndex;
            public byte formMax;
            public string name;
            public int formID;
            public bool legendaryMythical;
        }

        public struct BaseStats
        {
            public byte hp;
            public byte atk;
            public byte def;
            public byte spAtk;
            public byte spDef;
            public byte spd;
        }

        public struct EvYield
        {
            public byte hp;
            public byte atk;
            public byte def;
            public byte spAtk;
            public byte spDef;
            public byte spd;
        }

        public struct HeldItems
        {
            public string item1;
            public string item2;
            public string item3;
        }

        public struct Abilities
        {
            public string ability1;
            public string ability2;
            public string hiddenAbility;
        }

        public struct LevelUpMove
        {
            public ushort level;
            public string move;
        }

        public struct EvolutionPath
        {
            public ushort method;
            public ushort parameter;
            public string target;
            public ushort targetFormID;
            public ushort level;
        }

        public struct Trainer
        {
            public TrainerReadOnly readOnly;
            public int trainerTypeID;
            public byte colorID;
            public byte fightType;
            public int arenaID;
            public int effectID;
            public byte gold;
            public List<string> useItems;
            public bool hpRecoverFlag;
            public ushort giftItem;
            public string nameLabel;
            public AIFlags aiFlags;
            public List<TrainerPokemon> party;
        }

        public struct TrainerReadOnly
        {
            public int trainerID;
            public string name;
        }

        public struct AIFlags
        {
            public bool basicAI;
            public bool strongAI;
            public bool expertAI;
            public bool doubleBattleAI;
            public bool mercifulAI;
            public bool canUseItems;
            public bool canSwitchPokemon;
        }

        public struct TrainerPokemon
        {
            public string species;
            public ushort formID;
            public bool shiny;
            public byte level;
            public string sex;
            public string nature;
            public string ability;
            public List<string> moveset;
            public string heldItem;
            public byte ballID;
            public int seal;
            public IVs ivs;
            public EVs evs;
        }

        public struct IVs
        {
            public byte hp;
            public byte atk;
            public byte def;
            public byte spAtk;
            public byte spDef;
            public byte spd;
        }

        public struct EVs
        {
            public byte hp;
            public byte atk;
            public byte def;
            public byte spAtk;
            public byte spDef;
            public byte spd;
        }
    }
}
