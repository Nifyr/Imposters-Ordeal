using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpostersOrdeal
{
    /// <summary>
    ///  Container for game-related data types for use in the application.
    /// </summary>
    public static class GameDataTypes
    {
        public class EvScript
        {
            public string mName;
            public List<Script> scripts;
            public List<string> strList;
        }

        public class Script
        {
            public string evLabel;
            public List<Command> commands;
        }

        public class Command
        {
            public int cmdType;
            public List<Argument> args;
        }

        public class Argument
        {
            public int argType;
            public float data;
        }

        public class MapWarpAsset
        {
            public string mName;
            public List<MapWarp> mapWarps;
            public List<int> zoneIDs;
        }

        public class MapWarp
        {
            public int groupId;
            public int destWarpZone;
            public int destWarpIndex;
            public int inputDir;
            public int flagIndex;
            public int scriptLabel;
            public int exitLabel;
            public string connectionName;

            //Readonly
            public int currentWarpIndex;
            public MapWarpAsset destination;
        }

        public class PickupItem
        {
            public ushort itemID;
            public List<byte> ratios;
        }

        public class ShopTables
        {
            public List<MartItem> martItems;
            public List<FixedShopItem> fixedShopItems;
            public List<BpShopItem> bpShopItems;
        }

        public class MartItem
        {
            public ushort itemID;
            public int badgeNum;
            public int zoneID;
        }

        public class FixedShopItem
        {
            public ushort itemID;
            public int shopID;
        }

        public class BpShopItem
        {
            public ushort itemID;
            public int npcID;
        }

        public class Trainer : INamedEntity
        {
            public int trainerTypeID;
            public byte colorID;
            public byte fightType;
            public int arenaID;
            public int effectID;
            public byte gold;
            public ushort useItem1;
            public ushort useItem2;
            public ushort useItem3;
            public ushort useItem4;
            public byte hpRecoverFlag;
            public ushort giftItem;
            public string nameLabel;
            public uint aiBit;
            public List<TrainerPokemon> trainerPokemon;

            //Readonly
            public int trainerID;
            public string name;

            public Trainer() { }

            public Trainer(Trainer t)
            {
                SetAll(t);
            }

            public void SetAll(Trainer t)
            {
                trainerTypeID = t.trainerTypeID;
                colorID = t.colorID;
                fightType = t.fightType;
                arenaID = t.arenaID;
                effectID = t.effectID;
                gold = t.gold;
                useItem1 = t.useItem1;
                useItem2 = t.useItem2;
                useItem3 = t.useItem3;
                useItem4 = t.useItem4;
                hpRecoverFlag = t.hpRecoverFlag;
                giftItem = t.giftItem;
                nameLabel = t.nameLabel;
                aiBit = t.aiBit;
                trainerPokemon = new();
                foreach (TrainerPokemon tp in t.trainerPokemon)
                    trainerPokemon.Add(new(tp));
                trainerID = t.trainerID;
                name = t.name;
            }

            public List<int> GetItems()
            {
                List<int> items = new();
                if (useItem1 > 0)
                    items.Add(useItem1);
                if (useItem2 > 0)
                    items.Add(useItem2);
                if (useItem3 > 0)
                    items.Add(useItem3);
                if (useItem4 > 0)
                    items.Add(useItem4);
                return items;
            }

            public void SetItems(List<int> items)
            {
                useItem1 = (ushort)(items.Count > 0 ? items[0] : 0);
                useItem2 = (ushort)(items.Count > 1 ? items[1] : 0);
                useItem3 = (ushort)(items.Count > 2 ? items[2] : 0);
                useItem4 = (ushort)(items.Count > 3 ? items[3] : 0);
            }

            public void SetItemFlag()
            {
                aiBit |= 1 << 5;
            }

            public int GetTypeTheme()
            {
                return trainerTypeID switch
                {
                    80 => 1,
                    69 => 4,
                    65 => 5,
                    68 => 6,
                    81 => 7,
                    67 => 8,
                    70 => 9,
                    79 => 10,
                    78 => 11,
                    83 => 12,
                    71 => 13,
                    82 => 14,
                    _ => -1,
                };
            }

            public double GetAvgLevel()
            {
                if (trainerPokemon.Count == 0)
                    return 0;
                return trainerPokemon.Select(p => (int)p.level).Average();
            }

            public bool[] GetAIFlags()
            {
                bool[] flags = new bool[32];
                for (int i = 0; i < 32; i++)
                    flags[i] = (aiBit & ((uint)1 << i)) != 0;
                return flags;
            }

            public void SetAIFlags(bool[] flagArray)
            {
                aiBit = 0;
                for (int i = 0; i < 32; i++)
                    aiBit |= flagArray[i] ? (uint)1 << i : 0;
            }

            public int GetID()
            {
                return trainerID;
            }

            public string GetName()
            {
                return name;
            }

            public bool IsValid()
            {
                return true;
            }
        }

        public class TrainerPokemon
        {
            public ushort dexID;
            public ushort formID;
            public byte isRare;
            public byte level;
            public byte sex;
            public byte natureID;
            public ushort abilityID;
            public ushort moveID1;
            public ushort moveID2;
            public ushort moveID3;
            public ushort moveID4;
            public ushort itemID;
            public byte ballID;
            public int seal;
            public byte hpIV;
            public byte atkIV;
            public byte defIV;
            public byte spAtkIV;
            public byte spDefIV;
            public byte spdIV;
            public byte hpEV;
            public byte atkEV;
            public byte defEV;
            public byte spAtkEV;
            public byte spDefEV;
            public byte spdEV;

            public TrainerPokemon() { }

            public TrainerPokemon(TrainerPokemon tp)
            {
                dexID = tp.dexID;
                formID = tp.formID;
                isRare = tp.isRare;
                level = tp.level;
                sex = tp.sex;
                natureID = tp.natureID;
                abilityID = tp.abilityID;
                moveID1 = tp.moveID1;
                moveID2 = tp.moveID2;
                moveID3 = tp.moveID3;
                moveID4 = tp.moveID4;
                itemID = tp.itemID;
                ballID = tp.ballID;
                seal = tp.seal;
                hpIV = tp.hpIV;
                atkIV = tp.atkIV;
                defIV = tp.defIV;
                spdIV = tp.spdIV;
                spAtkIV = tp.spAtkIV;
                spDefIV = tp.spDefIV;
                hpEV = tp.hpEV;
                atkEV = tp.atkEV;
                defEV = tp.defEV;
                spdEV = tp.spdEV;
                spAtkEV = tp.spAtkEV;
                spDefEV = tp.spDefEV;
            }

            public List<ushort> GetMoves()
            {
                List<ushort> moves = new();
                if (moveID1 > 0)
                    moves.Add(moveID1);
                if (moveID2 > 0)
                    moves.Add(moveID2);
                if (moveID3 > 0)
                    moves.Add(moveID3);
                if (moveID4 > 0)
                    moves.Add(moveID4);
                return moves;
            }

            public void SetMoves(List<ushort> moves)
            {
                moveID1 = (ushort)(moves.Count > 0 ? moves[0] : 0);
                moveID2 = (ushort)(moves.Count > 1 ? moves[1] : 0);
                moveID3 = (ushort)(moves.Count > 2 ? moves[2] : 0);
                moveID4 = (ushort)(moves.Count > 3 ? moves[3] : 0);
            }

            public int[] GetIVs()
            {
                return new int[]
                {
                    hpIV,
                    atkIV,
                    defIV,
                    spAtkIV,
                    spDefIV,
                    spdIV
                };
            }

            public int[] GetEVs()
            {
                return new int[]
                {
                    hpEV,
                    atkEV,
                    defEV,
                    spAtkEV,
                    spDefEV,
                    spdEV
                };
            }

            public void SetEVs(int[] evs)
            {
                hpEV = (byte)evs[0];
                atkEV = (byte)evs[1];
                defEV = (byte)evs[2];
                spAtkEV = (byte)evs[3];
                spDefEV = (byte)evs[4];
                spdEV = (byte)evs[5];
            }
        }

        public class TrainerType : INamedEntity
        {
            public int trainerTypeID;
            public string label;
            public string name;

            public int GetID()
            {
                return trainerTypeID;
            }

            public string GetName()
            {
                return name;
            }

            public bool IsValid()
            {
                return label != "";
            }
        }

        public class EncounterTableFile
        {
            public string mName;
            public List<EncounterTable> encounterTables;
            public List<int> trophyGardenMons;
            public List<HoneyTreeEncounter> honeyTreeEnconters;
            public List<int> safariMons;
        }

        public class EncounterTable
        {
            public ZoneID zoneID;
            public int encRateGround;
            public List<Encounter> groundMons;
            public List<Encounter> tairyo;
            public List<Encounter> day;
            public List<Encounter> night;
            public List<Encounter> swayGrass;
            public int formProb;
            public int unownTable;
            public List<Encounter> gbaRuby;
            public List<Encounter> gbaSapphire;
            public List<Encounter> gbaEmerald;
            public List<Encounter> gbaFire;
            public List<Encounter> gbaLeaf;
            public int encRateWater;
            public List<Encounter> waterMons;
            public int encRateOldRod;
            public List<Encounter> oldRodMons;
            public int encRateGoodRod;
            public List<Encounter> goodRodMons;
            public int encRateSuperRod;
            public List<Encounter> superRodMons;

            public List<List<Encounter>> GetAllTables()
            {
                return new List<List<Encounter>>()
                {
                    groundMons, tairyo, day, night, swayGrass,
                    gbaRuby, gbaSapphire, gbaEmerald, gbaFire, gbaLeaf,
                    waterMons, oldRodMons, goodRodMons, superRodMons
                };
            }

            public double GetAvgLevel()
            {
                return GetAllTables()
                    .Take(5)
                    .SelectMany(l => l)
                    .Where(e => e.dexID != 0)
                    .Select(e => e.GetAvgLevel())
                    .DefaultIfEmpty()
                    .Average();
            }
        }

        public class Encounter
        {
            public int maxLv;
            public int minLv;
            public int dexID;

            public double GetAvgLevel()
            {
                return (minLv + maxLv) / 2.0;
            }
        }

        public class HoneyTreeEncounter
        {
            public int rate;
            public int normalDexID;
            public int rareDexID;
            public int superRareDexID;
        }

        public class MessageFileSet
        {
            public int langID;
            public List<MessageFile> messageFiles;

            public List<LabelData> GetStrings()
            {
                List<LabelData> strings = new();
                for (int i = 0; i < messageFiles.Count; i++)
                    strings.AddRange(messageFiles[i].GetStrings());
                return strings;
            }

            public void SetStrings(List<LabelData> strings)
            {
                for (int i = messageFiles.Count - 1; i >= 0; i--)
                    messageFiles[i].SetStrings(strings);
            }
        }

        public class MessageFile
        {
            public string mName;
            public int langID;
            public byte isKanji;
            public List<LabelData> labelDatas;

            public List<LabelData> GetStrings()
            {
                List<LabelData> strings = new();
                for (int i = 0; i < labelDatas.Count; i++)
                    if (labelDatas[i].IsValidString())
                        strings.Add(labelDatas[i]);
                return strings;
            }

            public void SetStrings(List<LabelData> strings)
            {
                for (int i = labelDatas.Count - 1; i >= 0; i--)
                    if (labelDatas[i].IsValidString())
                    {
                        labelDatas[i].wordDatas = strings[^1].wordDatas;
                        strings.RemoveAt(strings.Count - 1);
                    }
            }
        }
        public class TagData : ICloneable
        {
            public int tagIndex;
            public int groupID;
            public int tagID;
            public int tagPatternID;
            public int forceArticle;
            public int tagParameter;
            public List<string> tagWordArray;
            public int forceGrmID;

            public object Clone()
            {
                TagData td = (TagData)MemberwiseClone();
                td.tagWordArray = new();
                td.tagWordArray.AddRange(tagWordArray);
                return td;
            }
        }

        public class LabelData : ICloneable
        {
            public int labelIndex;
            public int arrayIndex;
            public string labelName;
            public int styleIndex;
            public int colorIndex;
            public int fontSize;
            public int maxWidth;
            public int controlID;
            public List<int> attributeValues;
            public List<TagData> tagDatas;
            public List<WordData> wordDatas;
            public object Clone()
            {
                LabelData ld = (LabelData)MemberwiseClone();
                ld.attributeValues = new();
                ld.attributeValues.AddRange(attributeValues);
                ld.tagDatas = new();
                foreach (TagData td in tagDatas)
                    ld.tagDatas.Add((TagData)td.Clone());
                ld.wordDatas = new();
                foreach (WordData wd in wordDatas)
                    ld.wordDatas.Add((WordData)wd.Clone());

                return ld;
            }

            public string GetString()
            {
                string str = "";
                for (int i = 0; i < wordDatas.Count; i++)
                    str += wordDatas[i].str + wordDatas[i].GetEndChar();
                return str;
            }

            public bool IsValidString()
            {
                if (GetString().Length < 1)
                    return false;
                for (int i = 0; i < wordDatas.Count; i++)
                    if (wordDatas[i].tagIndex >= 0 || wordDatas[i].eventID == 5)
                        return false;
                return true;
            }

            public bool IsDialogString()
            {
                for (int i = 0; i < wordDatas.Count; i++)
                    if (wordDatas[i].eventID == 3)
                        return true;
                return false;
            }
        }

        public class WordData : ICloneable
        {
            public int patternID;
            public int eventID;
            public int tagIndex;
            public float tagValue;
            public string str;
            public float strWidth;

            public string GetEndChar()
            {
                return eventID switch
                {
                    0 => "", //No marker
                    1 => "\n", //New line marker
                    2 => "", //Wait marker
                    3 => "\n", //New textbox marker
                    4 => "\n", //Scroll textbox marker
                    5 => "", //Start/join event marker?
                    7 => "", //End of message
                    _ => "\0", //Unknown
                };
            }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }

        public class Pokemon : INamedEntity, ICloneable, IComparable<Pokemon>
        {
            public byte validFlag;
            public ushort personalID;
            public ushort dexID;
            public ushort formIndex;
            public byte formMax;
            public byte color;
            public ushort graNo;
            public byte basicHp;
            public byte basicAtk;
            public byte basicDef;
            public byte basicSpd;
            public byte basicSpAtk;
            public byte basicSpDef;
            public byte typingID1;
            public byte typingID2;
            public byte getRate;
            public byte rank;
            public ushort expValue;
            public ushort item1;
            public ushort item2;
            public ushort item3;
            public byte sex;
            public byte eggBirth;
            public byte initialFriendship;
            public byte eggGroup1;
            public byte eggGroup2;
            public byte grow;
            public ushort abilityID1;
            public ushort abilityID2;
            public ushort abilityID3;
            public ushort giveExp;
            public ushort height;
            public ushort weight;
            public ushort chihouZukanNo;
            public uint machine1;
            public uint machine2;
            public uint machine3;
            public uint machine4;
            public uint hiddenMachine;
            public ushort eggMonsno;
            public ushort eggFormno;
            public ushort eggFormnoKawarazunoishi;
            public byte eggFormInheritKawarazunoishi;

            public List<LevelUpMove> levelUpMoves;
            public List<ushort> eggMoves;
            public List<EvolutionPath> evolutionPaths;

            //Readonly
            public string name;
            public int formID;
            public (ushort, ushort) pastEvoLvs;
            public (ushort, ushort) nextEvoLvs;
            public List<Pokemon> pastPokemon;
            public List<Pokemon> nextPokemon;
            public List<Pokemon> inferiorForms;
            public List<Pokemon> superiorForms;
            public bool legendary;

            public object Clone()
            {
                Pokemon p = (Pokemon)MemberwiseClone();
                p.levelUpMoves = new();
                foreach (LevelUpMove lum in levelUpMoves)
                    p.levelUpMoves.Add((LevelUpMove)lum.Clone());
                p.eggMoves = new();
                p.eggMoves.AddRange(eggMoves);
                p.evolutionPaths = new();
                foreach (EvolutionPath ep in evolutionPaths)
                    p.evolutionPaths.Add((EvolutionPath)ep.Clone());

                return p;
            }

            public int GetBST()
            {
                return basicHp + basicAtk + basicDef + basicSpAtk + basicSpDef + basicSpd;
            }

            public byte[] GetStats()
            {
                return new byte[]
                {
                    basicHp,
                    basicAtk,
                    basicDef,
                    basicSpAtk,
                    basicSpDef,
                    basicSpd
                };
            }

            public void SetStats(byte[] stats)
            {
                basicHp = stats[0];
                basicAtk = stats[1];
                basicDef = stats[2];
                basicSpAtk = stats[3];
                basicSpDef = stats[4];
                basicSpd = stats[5];
            }

            public List<int> GetTyping()
            {
                List<int> l = new();
                l.Add(typingID1);
                if (typingID1 != typingID2)
                    l.Add(typingID2);
                return l;
            }

            public void SetTyping(List<int> l)
            {
                typingID1 = (byte)l[0];
                typingID2 = (byte)l[0];
                if (l.Count > 1)
                    typingID2 = (byte)l[1];
            }

            public bool[] GetTMCompatibility()
            {
                bool[] tmCompatibility = new bool[128];
                for (int i = 0; i < 32; i++)
                    tmCompatibility[i] = (machine1 & ((uint)1 << i)) != 0;
                for (int i = 0; i < 32; i++)
                    tmCompatibility[i + 32] = (machine2 & ((uint)1 << i)) != 0;
                for (int i = 0; i < 32; i++)
                    tmCompatibility[i + 64] = (machine3 & ((uint)1 << i)) != 0;
                for (int i = 0; i < 32; i++)
                    tmCompatibility[i + 96] = (machine4 & ((uint)1 << i)) != 0;
                return tmCompatibility;
            }

            public void SetTMCompatibility(bool[] tmCompatibility)
            {
                machine1 = 0;
                for (int i = 0; i < 32; i++)
                    machine1 |= tmCompatibility[i] ? (uint)1 << i : 0;
                machine2 = 0;
                for (int i = 0; i < 32; i++)
                    machine2 |= tmCompatibility[i + 32] ? (uint)1 << i : 0;
                machine3 = 0;
                for (int i = 0; i < 32; i++)
                    machine3 |= tmCompatibility[i + 64] ? (uint)1 << i : 0;
                machine4 = 0;
                for (int i = 0; i < 32; i++)
                    machine4 |= tmCompatibility[i + 96] ? (uint)1 << i : 0;
            }

            public List<int> GetCompatibleTMs()
            {
                List<int> compatibleTMs = new();
                bool[] tmCompatibility = GetTMCompatibility();
                for (int tmID = 0; tmID < tmCompatibility.Length; tmID++)
                    if (tmCompatibility[tmID])
                        compatibleTMs.Add(tmID);
                return compatibleTMs;
            }

            public int[] GetWildHeldItems()
            {
                return new int[3]
                {
                    item1,
                    item2,
                    item3
                };
            }

            public int[] GetAbilities()
            {
                return new int[3]
                {
                    abilityID1,
                    abilityID2,
                    abilityID3
                };
            }

            public int[] GetEvYield()
            {
                int[] evYield = new int[6];
                for (int i = 0; i < 6; i++)
                    evYield[i] = (expValue & (3 << (2 * i))) >> (2 * i);
                return evYield;
            }

            public void SetEvYield(int[] evYield)
            {
                expValue = 0;
                for (int i = 0; i < 6; i++)
                    expValue |= (ushort)(evYield[i] << (2 * i));
            }

            public int GetID()
            {
                return personalID;
            }

            public string GetName()
            {
                return name;
            }

            public bool IsValid()
            {
                return validFlag != 0 && personalID > 0;
            }

            public int CompareTo(Pokemon other)
            {
                if (formID.CompareTo(other.formID) != 0)
                {
                    if (formID == 0)
                        return -1;
                    if (other.formID == 0)
                        return 1;
                }

                int i = dexID.CompareTo(other.dexID);
                if (i == 0)
                    i = formID.CompareTo(other.formID);
                return i;
            }
        }

        public class DexEntry : INamedEntity
        {
            public int dexID;
            public List<Pokemon> forms;
            public string name;

            public List<DexEntry> GetPastEntries()
            {
                List<DexEntry> past = new();
                foreach (Pokemon pokemon in forms)
                    past = past.Union(pokemon.pastPokemon.Select(p => GlobalData.gameData.dexEntries[p.dexID])).ToList();

                return past;
            }

            public List<DexEntry> GetNextEntries()
            {
                List<DexEntry> next = new();
                foreach (Pokemon pokemon in forms)
                    next = next.Union(pokemon.nextPokemon.Select(p => GlobalData.gameData.dexEntries[p.dexID])).ToList();

                return next;
            }

            public int GetID()
            {
                return dexID;
            }

            public string GetName()
            {
                return name;
            }

            public bool IsValid()
            {
                return forms[0].IsValid();
            }
        }

        public class EvolutionPath : ICloneable
        {
            public ushort method;
            public ushort parameter;
            public ushort destDexID;
            public ushort destFormID;
            public ushort level;

            public object Clone()
            {
                return MemberwiseClone();
            }
        }

        public class LevelUpMove : ICloneable
        {
            public ushort level;
            public ushort moveID;

            public object Clone()
            {
                return MemberwiseClone();
            }
        }

        public class Item : GameDataTypes.INamedEntity
        {
            public short itemID;
            public byte type;
            public int iconID;
            public int price;
            public int bpPrice;
            public byte nageAtc;
            public byte sizenAtc;
            public byte sizenType;
            public byte tuibamuEff;
            public byte sort;
            public byte group;
            public byte groupID;
            public byte fldPocket;
            public byte fieldFunc;
            public byte battleFunc;
            public byte criticalRanks;
            public byte atkStages;
            public byte defStages;
            public byte spdStages;
            public byte accStages;
            public byte spAtkStages;
            public byte spDefStages;
            public byte ppRestoreAmount;
            public sbyte hpEvIncrease;
            public sbyte atkEvIncrease;
            public sbyte defEvIncrease;
            public sbyte spdEvIncrease;
            public sbyte spAtkEvIncrease;
            public sbyte spDefEvIncrease;
            public sbyte friendshipIncrease1; //0-99
            public sbyte friendshipIncrease2; //100-199
            public sbyte friendshipIncrease3; //200-255
            public byte hpRestoreAmount;
            public uint flags0;

            //Readonly
            public string name;

            public bool IsActive()
            {
                return !GetFlags()[31];
            }

            public bool[] GetFlags()
            {
                bool[] flags = new bool[32];
                for (int i = 0; i < 32; i++)
                    flags[i] = (flags0 & ((uint)1 << i)) != 0;
                return flags;
            }

            public void SetFlags(bool[] flagArray)
            {
                flags0 = 0;
                for (int i = 0; i < 32; i++)
                    flags0 |= flagArray[i] ? (uint)1 << i : 0;
            }

            public bool IsPurchasable()
            {
                return IsActive() && price > 0;
            }

            public int GetID()
            {
                return itemID;
            }

            public string GetName()
            {
                return name;
            }

            public bool IsValid()
            {
                return IsPurchasable();
                //Not completely accurate, but randomization is easier this way.
            }
        }

        public class TM : GameDataTypes.INamedEntity
        {
            public int itemID;
            public int machineNo;
            public int moveID;

            //Readonly
            public int tmID;
            public string name;

            public string GetFullName()
            {
                return GetName() + " " + GlobalData.gameData.moves[moveID].GetName();
            }

            public int GetID()
            {
                return tmID;
            }

            public string GetName()
            {
                return name;
            }

            public bool IsValid()
            {
                return GlobalData.gameData.items[itemID].IsActive() &&
                    GlobalData.gameData.items[itemID].fieldFunc == 2 &&
                    GlobalData.gameData.items[itemID].groupID <= 128 &&
                    GlobalData.gameData.items[itemID].groupID > 0;
            }
        }

        public class Move : GameDataTypes.INamedEntity
        {
            public int moveID;
            public byte isValid;
            public byte typingID;
            public byte category;
            public byte damageCategoryID;
            public byte power;
            public byte hitPer;
            public byte basePP;
            public sbyte priority;
            public byte hitCountMax;
            public byte hitCountMin;
            public ushort sickID;
            public byte sickPer;
            public byte sickCont;
            public byte sickTurnMin;
            public byte sickTurnMax;
            public byte criticalRank;
            public byte shrinkPer;
            public ushort aiSeqNo;
            public sbyte damageRecoverRatio;
            public sbyte hpRecoverRatio;
            public byte target;
            public byte rankEffType1;
            public byte rankEffType2;
            public byte rankEffType3;
            public sbyte rankEffValue1;
            public sbyte rankEffValue2;
            public sbyte rankEffValue3;
            public byte rankEffPer1;
            public byte rankEffPer2;
            public byte rankEffPer3;
            public uint flags;
            public uint contestWazaNo;

            public string cmdSeqName;
            public string cmdSeqNameLegend;
            public string notShortenTurnType0;
            public string notShortenTurnType1;
            public string turnType1;
            public string turnType2;
            public string turnType3;
            public string turnType4;

            //Readonly
            public string name;

            public bool[] GetFlags()
            {
                bool[] flagArray = new bool[32];
                for (int i = 0; i < 32; i++)
                    flagArray[i] = (flags & ((uint)1 << i)) != 0;
                return flagArray;
            }

            public void SetFlags(bool[] flagArray)
            {
                flags = 0;
                for (int i = 0; i < 32; i++)
                    flags |= flagArray[i] ? (uint)1 << i : 0;
            }

            public int GetID()
            {
                return moveID;
            }

            public string GetName()
            {
                return name;
            }

            public bool IsValid()
            {
                return isValid != 0 && moveID > 0;
            }
        }

        public class GrowthRate : INamedEntity
        {
            public int growthID;
            public List<uint> expRequirements;

            public int GetID()
            {
                return growthID;
            }

            public string GetName()
            {
                return growthID switch
                {
                    0 => "Medium Fast",
                    1 => "Erratic",
                    2 => "Fluctuating",
                    3 => "Medium Slow",
                    4 => "Fast",
                    5 => "Slow",
                    _ => ""
                };
            }

            public bool IsValid()
            {
                return growthID <= 5;
            }
        }

        public class UgArea
        {
            public int id;
            public string fileName;
        }

        public class UgEncounterFile
        {
            public string mName;
            public List<UgEncounter> ugEncounters;
        }

        public class UgEncounter
        {
            public int dexID;
            public int version;
            public int zukanFlag;
        }

        public class UgEncounterLevelSet
        {
            public int minLv;
            public int maxLv;

            public double GetAvgLevel()
            {
                return (minLv + maxLv) / 2.0;
            }
        }

        public class UgSpecialEncounter
        {
            public int id;
            public int dexID;
            public int version;
            public int dRate;
            public int pRate;
        }

        public class Ability : INamedEntity
        {
            public int abilityID;
            public string name;

            public int GetID()
            {
                return abilityID;
            }

            public string GetName()
            {
                return name;
            }

            public bool IsValid()
            {
                return abilityID > 0;
            }
        }

        public class Typing : INamedEntity
        {
            public int typingID;
            public string name;

            public int GetID()
            {
                return typingID;
            }

            public string GetName()
            {
                return name;
            }

            public bool IsValid()
            {
                return true;
            }
        }

        public class DamageCategory : INamedEntity
        {
            public int damageCategoryID;

            public int GetID()
            {
                return damageCategoryID;
            }

            public string GetName()
            {
                switch (damageCategoryID)
                {
                    case 0:
                        return "Status";
                    case 1:
                        return "Physical";
                    case 2:
                        return "Special";
                }
                return null;
            }

            public bool IsValid()
            {
                return damageCategoryID > 0;
            }
        }

        public class Nature : INamedEntity
        {
            public int natureID;
            public string name;

            public int GetID()
            {
                return natureID;
            }

            public string GetName()
            {
                return name;
            }

            public bool IsValid()
            {
                return true;
            }
        }

        public class GlobalMetadata
        {
            public byte[] buffer;

            //Readonly
            public uint stringOffset;
            public uint defaultValuePtrOffset;
            public uint defaultValuePtrSecSize;
            public uint defaultValueOffset;
            public uint defaultValueSecSize;
            public uint fieldOffset;
            public uint typeOffset;
            public uint imageOffset;
            public uint imageSecSize;

            public Dictionary<uint, FieldDefaultValue> defaultValueDic;
            public List<ImageDefinition> images;

            public long[] typeMatchupOffsets;

            public byte GetTypeMatchup(int off, int def)
            {
                return buffer[typeMatchupOffsets[off] + def];
            }

            public void SetTypeMatchup(int off, int def, byte aff)
            {
                buffer[typeMatchupOffsets[off] + def] = aff;
            }
        }

        public class ImageDefinition : IGMObject
        {
            public string name;
            public uint typeStart;
            public uint typeCount;
            public List<TypeDefinition> types;

            public override string ToString()
            {
                return name;
            }

            public bool HasDefault()
            {
                return types.Any(t => t.HasDefault());
            }
        }

        public class TypeDefinition : IGMObject
        {
            public string name;
            public int fieldStart;
            public ushort fieldCount;
            public List<FieldDefinition> fields;

            public override string ToString()
            {
                return name;
            }

            public bool HasDefault()
            {
                return fields.Any(f => f.HasDefault());
            }
        }

        public class FieldDefinition : IGMObject
        {
            public string name;
            public FieldDefaultValue defautValue;

            public override string ToString()
            {
                return name;
            }

            public bool HasDefault()
            {
                return defautValue != null;
            }
        }

        public class FieldDefaultValue
        {
            public long offset;
            public int length;
        }

        public interface IGMObject
        {
            public bool HasDefault();
            public string ToString();
        }

        public interface INamedEntity
        {
            public int GetID();
            public string GetName();
            public bool IsValid();
        }
    }
}
