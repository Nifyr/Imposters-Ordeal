using AssetsTools.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ImpostersOrdeal.GlobalData;
using static ImpostersOrdeal.GameDataTypes;

namespace ImpostersOrdeal
{
    /// <summary>
    ///  Responsible for converting AssetsTools.NET objects into easier to work with objects and back.
    /// </summary>
    static public class DataParser
    {
        /// <summary>
        ///  Parses all files necessary for analysis and configuration.
        /// </summary>
        public static void PrepareAnalysis()
        {
            ParseNatures();
            ParseEvScripts();
            //ParseMapWarpAssets();
            //ParseMessageFiles();
            ParseGrowthRates();
            ParseItems();
            ParsePickupItems();
            ParseShopTables();
            ParseMoves();
            ParseTMs();
            ParsePokemon();
            ParseEncounterTables();
            ParseTrainers();
            ParseUgEncounterTables();
            ParseAbilities();
            ParseTypings();
            ParseDamagaCategories();
            ParseTrainerTypes();
            ParseGlobalMetadata();
            ParseBattleMasterDatas();
            ParseMasterDatas();
            ParsePersonalMasterDatas();
            InsertAVulpix();
        }
        private static void InsertAVulpix()
        {
            AssetInsertor.getInstance().InsertPokemon(37, 37, 0, 1, "Alolan Vulpix");
        }

        /// <summary>
        ///  Overwrites GlobalData with parsed TrainerTypes.
        /// </summary>
        private static void ParseTrainerTypes()
        {
            gameData.trainerTypes = new();
            AssetTypeValueField monoBehaviour = fileManager.GetMonoBehaviours(PathEnum.DprMasterdatas).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "TrainerTable");
            AssetTypeValueField nameData = fileManager.GetMonoBehaviours(PathEnum.English).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "english_dp_trainers_type");

            AssetTypeValueField[] nameFields = nameData.children[8].children[0].children;
            Dictionary<string, string> trainerTypeNames = new();
            foreach (AssetTypeValueField label in nameFields)
                if (label.children[6].children[0].childrenCount > 0)
                    trainerTypeNames[label.children[2].GetValue().AsString()] = label.children[6].children[0].children[0].children[4].GetValue().AsString();

            AssetTypeValueField[] trainerTypeFields = monoBehaviour.children[4].children[0].children;
            for (int trainerTypeIdx = 0; trainerTypeIdx < trainerTypeFields.Length; trainerTypeIdx++)
            {
                if (trainerTypeFields[trainerTypeIdx].children[0].GetValue().AsInt() == -1)
                    continue;

                TrainerType trainerType = new();
                trainerType.trainerTypeID = trainerTypeIdx;
                trainerType.label = trainerTypeFields[trainerTypeIdx].children[1].GetValue().AsString();

                trainerType.name = !trainerTypeNames.ContainsKey(trainerType.label) ? "" : trainerTypeNames[trainerType.label];

                gameData.trainerTypes.Add(trainerType);
            }
        }

        /// <summary>
        ///  Overwrites GlobalData with parsed Natures.
        /// </summary>
        private static void ParseNatures()
        {
            gameData.natures = new();
            AssetTypeValueField[] natureFields = fileManager.GetMonoBehaviours(PathEnum.English).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "english_ss_seikaku").children[8].children[0].children;

            for (int natureID = 0; natureID < natureFields.Length; natureID++)
            {
                Nature nature = new();
                nature.natureID = natureID;
                nature.name = Encoding.UTF8.GetString(natureFields[natureID].children[6].children[0].children[0].children[4].value.value.asString);

                gameData.natures.Add(nature);
            }
        }

        /// <summary>
        ///  Overwrites GlobalData with parsed DamageCategories.
        /// </summary>
        private static void ParseDamagaCategories()
        {
            gameData.damageCategories = new();
            for (int i = 0; i < 3; i++)
            {
                DamageCategory damageCategory = new();
                damageCategory.damageCategoryID = i;
                gameData.damageCategories.Add(damageCategory);
            }
        }

        /// <summary>
        ///  Overwrites GlobalData with parsed Typings.
        /// </summary>
        private static void ParseTypings()
        {
            gameData.typings = new();
            AssetTypeValueField[] typingFields = fileManager.GetMonoBehaviours(PathEnum.English).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "english_ss_typename").children[8].children[0].children;

            for (int typingID = 0; typingID < typingFields.Length; typingID++)
            {
                Typing typing = new();
                typing.typingID = typingID;
                typing.name = Encoding.UTF8.GetString(typingFields[typingID].children[6].children[0].children[0].children[4].value.value.asString);

                gameData.typings.Add(typing);
            }
        }

        /// <summary>
        ///  Overwrites GlobalData with parsed Abilities.
        /// </summary>
        private static void ParseAbilities()
        {
            gameData.abilities = new();
            AssetTypeValueField[] abilityFields = fileManager.GetMonoBehaviours(PathEnum.English).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "english_ss_tokusei").children[8].children[0].children;

            for (int abilityID = 0; abilityID < abilityFields.Length; abilityID++)
            {
                Ability ability = new();
                ability.abilityID = abilityID;
                ability.name = Encoding.UTF8.GetString(abilityFields[abilityID].children[6].children[0].children[0].children[4].value.value.asString);

                gameData.abilities.Add(ability);
            }
        }

        /// <summary>
        ///  Overwrites GlobalData with parsed UgEncounterTables.
        /// </summary>
        private static void ParseUgEncounterTables()
        {
            gameData.ugEncounterFiles = new();
            gameData.ugEncounterLevelSets = new();
            List<AssetTypeValueField> monoBehaviours = fileManager.GetMonoBehaviours(PathEnum.Ugdata);

            List<AssetTypeValueField> ugEncounterMonobehaviours = monoBehaviours.Where(m => Encoding.Default.GetString(m.children[3].value.value.asString).StartsWith("UgEncount_")).ToList();
            for (int ugEncounterFileIdx = 0; ugEncounterFileIdx < ugEncounterMonobehaviours.Count; ugEncounterFileIdx++)
            {
                UgEncounterFile ugEncounterFile = new();
                ugEncounterFile.mName = Encoding.Default.GetString(ugEncounterMonobehaviours[ugEncounterFileIdx].children[3].value.value.asString);

                ugEncounterFile.ugEncounter = new();
                AssetTypeValueField[] ugMonFields = ugEncounterMonobehaviours[ugEncounterFileIdx].children[4].children[0].children;
                for (int ugMonIdx = 0; ugMonIdx < ugMonFields.Length; ugMonIdx++)
                {
                    UgEncounter ugEncounter = new();
                    ugEncounter.dexID = ugMonFields[ugMonIdx].children[0].value.value.asInt32;
                    ugEncounter.version = ugMonFields[ugMonIdx].children[1].value.value.asInt32;
                    ugEncounter.zukanFlag = ugMonFields[ugMonIdx].children[2].value.value.asInt32;

                    ugEncounterFile.ugEncounter.Add(ugEncounter);
                }

                gameData.ugEncounterFiles.Add(ugEncounterFile);
            }

            AssetTypeValueField[] ugEncounterLevelFields = monoBehaviours.Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "UgEncountLevel").children[4].children[0].children;
            for (int ugEncouterLevelIdx = 0; ugEncouterLevelIdx < ugEncounterLevelFields.Length; ugEncouterLevelIdx++)
            {
                UgEncounterLevelSet ugLevels = new();
                ugLevels.minLv = ugEncounterLevelFields[ugEncouterLevelIdx].children[0].value.value.asInt32;
                ugLevels.maxLv = ugEncounterLevelFields[ugEncouterLevelIdx].children[1].value.value.asInt32;

                gameData.ugEncounterLevelSets.Add(ugLevels);
            }
        }

        /// <summary>
        ///  Overwrites GlobalData with a parsed Trainer table.
        /// </summary>
        private static void ParseTrainers()
        {
            gameData.trainers = new();
            AssetTypeValueField monoBehaviour = fileManager.GetMonoBehaviours(PathEnum.DprMasterdatas).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "TrainerTable");
            AssetTypeValueField nameData = fileManager.GetMonoBehaviours(PathEnum.English).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "english_dp_trainers_name");

            AssetTypeValueField[] nameFields = nameData.children[8].children[0].children;
            Dictionary<string, string> trainerNames = new();
            gameData.trainerNames = trainerNames;
            foreach (AssetTypeValueField label in nameFields)
                if (label.children[6].children[0].childrenCount > 0)
                    trainerNames[label.children[2].GetValue().AsString()] = label.children[6].children[0].children[0].children[4].GetValue().AsString();

            AssetTypeValueField[] trainerFields = monoBehaviour.children[5].children[0].children;
            AssetTypeValueField[] trainerPokemonFields = monoBehaviour.children[6].children[0].children;
            for (int trainerIdx = 0; trainerIdx < Math.Min(trainerFields.Length, trainerPokemonFields.Length); trainerIdx++)
            {
                Trainer trainer = new();
                trainer.trainerTypeID = trainerFields[trainerIdx].children[0].value.value.asInt32;
                trainer.colorID = trainerFields[trainerIdx].children[1].value.value.asUInt8;
                trainer.fightType = trainerFields[trainerIdx].children[2].value.value.asUInt8;
                trainer.arenaID = trainerFields[trainerIdx].children[3].value.value.asInt32;
                trainer.effectID = trainerFields[trainerIdx].children[4].value.value.asInt32;
                trainer.gold = trainerFields[trainerIdx].children[5].value.value.asUInt8;
                trainer.useItem1 = trainerFields[trainerIdx].children[6].value.value.asUInt16;
                trainer.useItem2 = trainerFields[trainerIdx].children[7].value.value.asUInt16;
                trainer.useItem3 = trainerFields[trainerIdx].children[8].value.value.asUInt16;
                trainer.useItem4 = trainerFields[trainerIdx].children[9].value.value.asUInt16;
                trainer.hpRecoverFlag = trainerFields[trainerIdx].children[10].value.value.asUInt8;
                trainer.giftItem = trainerFields[trainerIdx].children[11].value.value.asUInt16;
                trainer.nameLabel = trainerFields[trainerIdx].children[12].GetValue().AsString();
                trainer.aiBit = trainerFields[trainerIdx].children[19].value.value.asUInt32;

                trainer.trainerID = trainerIdx;
                trainer.name = trainerNames[trainer.nameLabel];

                //Parse trainer pokemon
                trainer.trainerPokemon = new();
                AssetTypeValueField[] pokemonFields = trainerPokemonFields[trainerIdx].children;
                for (int pokemonIdx = 1; pokemonIdx < pokemonFields.Length && pokemonFields[pokemonIdx].value.value.asUInt16 != 0; pokemonIdx += 26)
                {
                    TrainerPokemon pokemon = new();
                    pokemon.dexID = pokemonFields[pokemonIdx + 0].value.value.asUInt16;
                    pokemon.formID = pokemonFields[pokemonIdx + 1].value.value.asUInt16;
                    pokemon.isRare = pokemonFields[pokemonIdx + 2].value.value.asUInt8;
                    pokemon.level = pokemonFields[pokemonIdx + 3].value.value.asUInt8;
                    pokemon.sex = pokemonFields[pokemonIdx + 4].value.value.asUInt8;
                    pokemon.natureID = pokemonFields[pokemonIdx + 5].value.value.asUInt8;
                    pokemon.abilityID = pokemonFields[pokemonIdx + 6].value.value.asUInt16;
                    pokemon.moveID1 = pokemonFields[pokemonIdx + 7].value.value.asUInt16;
                    pokemon.moveID2 = pokemonFields[pokemonIdx + 8].value.value.asUInt16;
                    pokemon.moveID3 = pokemonFields[pokemonIdx + 9].value.value.asUInt16;
                    pokemon.moveID4 = pokemonFields[pokemonIdx + 10].value.value.asUInt16;
                    pokemon.itemID = pokemonFields[pokemonIdx + 11].value.value.asUInt16;
                    pokemon.ballID = pokemonFields[pokemonIdx + 12].value.value.asUInt8;
                    pokemon.seal = pokemonFields[pokemonIdx + 13].value.value.asInt32;
                    pokemon.hpIV = pokemonFields[pokemonIdx + 14].value.value.asUInt8;
                    pokemon.atkIV = pokemonFields[pokemonIdx + 15].value.value.asUInt8;
                    pokemon.defIV = pokemonFields[pokemonIdx + 16].value.value.asUInt8;
                    pokemon.spdIV = pokemonFields[pokemonIdx + 17].value.value.asUInt8;
                    pokemon.spAtkIV = pokemonFields[pokemonIdx + 18].value.value.asUInt8;
                    pokemon.spDefIV = pokemonFields[pokemonIdx + 19].value.value.asUInt8;
                    pokemon.hpEV = pokemonFields[pokemonIdx + 20].value.value.asUInt8;
                    pokemon.atkEV = pokemonFields[pokemonIdx + 21].value.value.asUInt8;
                    pokemon.defEV = pokemonFields[pokemonIdx + 22].value.value.asUInt8;
                    pokemon.spdEV = pokemonFields[pokemonIdx + 23].value.value.asUInt8;
                    pokemon.spAtkEV = pokemonFields[pokemonIdx + 24].value.value.asUInt8;
                    pokemon.spDefEV = pokemonFields[pokemonIdx + 25].value.value.asUInt8;

                    if (pokemon.dexID >= gameData.dexEntries.Count)
                        MainForm.ShowParserError("Oh my, this monsNo's way outta bounds...\n" +
                            "I don't feel so good...\n" +
                            "trainerID: " + trainerIdx + "\n" +
                            "monsNo: " + pokemon.dexID + "??");

                    if (pokemon.formID >= gameData.dexEntries[pokemon.dexID].forms.Count)
                        MainForm.ShowParserError("Oh my, this formNo's way outta bounds...\n" +
                            "I don't feel so good...\n" +
                            "trainerID: " + trainerIdx + "\n" +
                            "monsNo: " + pokemon.dexID + "\n" +
                            "formNo: " + pokemon.formID + "??");

                    if (pokemon.natureID >= gameData.natures.Count)
                        MainForm.ShowParserError("Oh my, this nature's way outta bounds...\n" +
                            "I don't feel so good...\n" +
                            "trainerID: " + trainerIdx + "\n" +
                            "monsNo: " + pokemon.dexID + "\n" +
                            "natureID: " + pokemon.natureID + "??");

                    trainer.trainerPokemon.Add(pokemon);
                }

                gameData.trainers.Add(trainer);
            }
        }

        /// <summary>
        ///  Overwrites GlobalData with parsed EncounterTables.
        /// </summary>
        private static void ParseEncounterTables()
        {
            gameData.encounterTableFiles = new EncounterTableFile[2];

            List<AssetTypeValueField> monoBehaviours = fileManager.GetMonoBehaviours(PathEnum.Gamesettings);
            AssetTypeValueField[] encounterTableMonoBehaviours = new AssetTypeValueField[2];
            encounterTableMonoBehaviours[0] = monoBehaviours.Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "FieldEncountTable_d");
            encounterTableMonoBehaviours[1] = monoBehaviours.Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "FieldEncountTable_p");
            for (int encounterTableFileIdx = 0; encounterTableFileIdx < encounterTableMonoBehaviours.Length; encounterTableFileIdx++)
            {
                EncounterTableFile encounterTableFile = new();
                encounterTableFile.mName = Encoding.Default.GetString(encounterTableMonoBehaviours[encounterTableFileIdx].children[3].value.value.asString);

                //Parse wild encounter tables
                encounterTableFile.encounterTables = new();
                AssetTypeValueField[] encounterTableFields = encounterTableMonoBehaviours[encounterTableFileIdx].children[4].children[0].children;
                for (int encounterTableIdx = 0; encounterTableIdx < encounterTableFields.Length; encounterTableIdx++)
                {
                    EncounterTable encounterTable = new();
                    encounterTable.zoneID = (ZoneID) encounterTableFields[encounterTableIdx].children[0].value.value.asInt32;
                    encounterTable.encRateGround = encounterTableFields[encounterTableIdx].children[1].value.value.asInt32;
                    encounterTable.formProb = encounterTableFields[encounterTableIdx].children[7].children[0].children[0].value.value.asInt32;
                    encounterTable.unownTable = encounterTableFields[encounterTableIdx].children[9].children[0].children[1].value.value.asInt32;
                    encounterTable.encRateWater = encounterTableFields[encounterTableIdx].children[15].value.value.asInt32;
                    encounterTable.encRateOldRod = encounterTableFields[encounterTableIdx].children[17].value.value.asInt32;
                    encounterTable.encRateGoodRod = encounterTableFields[encounterTableIdx].children[19].value.value.asInt32;
                    encounterTable.encRateSuperRod = encounterTableFields[encounterTableIdx].children[21].value.value.asInt32;

                    //Parse ground tables
                    encounterTable.groundMons = GetParsedEncounters(encounterTableFields[encounterTableIdx].children[2].children[0].children);

                    //Parse morning tables
                    encounterTable.tairyo = GetParsedEncounters(encounterTableFields[encounterTableIdx].children[3].children[0].children);

                    //Parse day tables
                    encounterTable.day = GetParsedEncounters(encounterTableFields[encounterTableIdx].children[4].children[0].children);

                    //Parse night tables
                    encounterTable.night = GetParsedEncounters(encounterTableFields[encounterTableIdx].children[5].children[0].children);

                    //Parse pokefinder tables
                    encounterTable.swayGrass = GetParsedEncounters(encounterTableFields[encounterTableIdx].children[6].children[0].children);

                    //Parse surfing tables
                    encounterTable.waterMons = GetParsedEncounters(encounterTableFields[encounterTableIdx].children[16].children[0].children);

                    //Parse old rod tables
                    encounterTable.oldRodMons = GetParsedEncounters(encounterTableFields[encounterTableIdx].children[18].children[0].children);

                    //Parse good rod tables
                    encounterTable.goodRodMons = GetParsedEncounters(encounterTableFields[encounterTableIdx].children[20].children[0].children);

                    //Parse super rod tables
                    encounterTable.superRodMons = GetParsedEncounters(encounterTableFields[encounterTableIdx].children[22].children[0].children);

                    encounterTableFile.encounterTables.Add(encounterTable);
                }

                //Parse trophy garden table
                encounterTableFile.trophyGardenMons = new();
                AssetTypeValueField[] trophyGardenMonFields = encounterTableMonoBehaviours[encounterTableFileIdx].children[5].children[0].children;
                for (int trophyGardenMonIdx = 0; trophyGardenMonIdx < trophyGardenMonFields.Length; trophyGardenMonIdx++)
                    encounterTableFile.trophyGardenMons.Add(trophyGardenMonFields[trophyGardenMonIdx].children[0].value.value.asInt32);

                //Parse honey tree tables
                encounterTableFile.honeyTreeEnconters = new();
                AssetTypeValueField[] honeyTreeEncounterFields = encounterTableMonoBehaviours[encounterTableFileIdx].children[6].children[0].children;
                for (int honeyTreeEncounterIdx = 0; honeyTreeEncounterIdx < honeyTreeEncounterFields.Length; honeyTreeEncounterIdx++)
                {
                    HoneyTreeEncounter honeyTreeEncounter = new();
                    honeyTreeEncounter.rate = honeyTreeEncounterFields[honeyTreeEncounterIdx].children[0].value.value.asInt32;
                    honeyTreeEncounter.normalDexID = honeyTreeEncounterFields[honeyTreeEncounterIdx].children[1].value.value.asInt32;
                    honeyTreeEncounter.rareDexID = honeyTreeEncounterFields[honeyTreeEncounterIdx].children[2].value.value.asInt32;
                    honeyTreeEncounter.superRareDexID = honeyTreeEncounterFields[honeyTreeEncounterIdx].children[3].value.value.asInt32;

                    encounterTableFile.honeyTreeEnconters.Add(honeyTreeEncounter);
                }

                //Parse safari table
                encounterTableFile.safariMons = new();
                AssetTypeValueField[] safariMonFields = encounterTableMonoBehaviours[encounterTableFileIdx].children[8].children[0].children;
                for (int safariMonIdx = 0; safariMonIdx < safariMonFields.Length; safariMonIdx++)
                    encounterTableFile.safariMons.Add(safariMonFields[safariMonIdx].children[0].value.value.asInt32);

                gameData.encounterTableFiles[encounterTableFileIdx] = encounterTableFile;
            }
        }

        /// <summary>
        ///  Parses an array of encounters in a monobehaviour into a list of Encounters.
        /// </summary>
        private static List<Encounter> GetParsedEncounters(AssetTypeValueField[] encounterFields)
        {
            List<Encounter> encounters = new();
            for (int encounterIdx = 0; encounterIdx < encounterFields.Length; encounterIdx++)
            {
                Encounter encounter = new();
                encounter.maxLv = encounterFields[encounterIdx].children[0].value.value.asInt32;
                encounter.minLv = encounterFields[encounterIdx].children[1].value.value.asInt32;
                encounter.dexID = encounterFields[encounterIdx].children[2].value.value.asInt32;

                encounters.Add(encounter);
            }
            return encounters;
        }

        /// <summary>
        ///  Overwrites GlobalData with parsed DexEntries and PersonalEntries.
        /// </summary>
        private static void ParsePokemon()
        {
            gameData.dexEntries = new();
            gameData.personalEntries = new();
            List<AssetTypeValueField> monoBehaviours = fileManager.GetMonoBehaviours(PathEnum.PersonalMasterdatas);
            AssetTypeValueField textData = fileManager.GetMonoBehaviours(PathEnum.CommonMsbt).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "english_ss_monsname");

            AssetTypeValueField[] levelUpMoveFields = monoBehaviours.Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "WazaOboeTable").children[4].children[0].children;
            AssetTypeValueField[] eggMoveFields = monoBehaviours.Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "TamagoWazaTable").children[4].children[0].children;
            AssetTypeValueField[] evolveFields = monoBehaviours.Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "EvolveTable").children[4].children[0].children;
            AssetTypeValueField[] personalFields = monoBehaviours.Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "PersonalTable").children[4].children[0].children;
            AssetTypeValueField[] textFields = textData.children[8].children[0].children;

            if (levelUpMoveFields.Length < personalFields.Length)
                MainForm.ShowParserError("Oh my, this WazaOboeTable is missing some stuff...\n" +
                    "I don't feel so good...\n" +
                    "PersonalTable entries: " + personalFields.Length + "\n" +
                    "WazaOboeTable entries: " + levelUpMoveFields.Length + "??");
            if (eggMoveFields.Length < personalFields.Length)
                MainForm.ShowParserError("Oh my, this TamagoWazaTable is missing some stuff...\n" +
                    "I don't feel so good...\n" +
                    "PersonalTable entries: " + personalFields.Length + "\n" +
                    "TamagoWazaTable entries: " + eggMoveFields.Length + "??");
            if (evolveFields.Length < personalFields.Length)
                MainForm.ShowParserError("Oh my, this EvolveTable is missing some stuff...\n" +
                    "I don't feel so good...\n" +
                    "PersonalTable entries: " + personalFields.Length + "\n" +
                    "EvolveTable entries: " + evolveFields.Length + "??");

            for (int personalID = 0; personalID < personalFields.Length; personalID++)
            {
                Pokemon pokemon = new();
                pokemon.validFlag = personalFields[personalID].children[0].value.value.asUInt8;
                pokemon.personalID = personalFields[personalID].children[1].value.value.asUInt16;
                pokemon.dexID = personalFields[personalID].children[2].value.value.asUInt16;
                pokemon.formIndex = personalFields[personalID].children[3].value.value.asUInt16;
                pokemon.formMax = personalFields[personalID].children[4].value.value.asUInt8;
                pokemon.color = personalFields[personalID].children[5].value.value.asUInt8;
                pokemon.graNo = personalFields[personalID].children[6].value.value.asUInt16;
                pokemon.basicHp = personalFields[personalID].children[7].value.value.asUInt8;
                pokemon.basicAtk = personalFields[personalID].children[8].value.value.asUInt8;
                pokemon.basicDef = personalFields[personalID].children[9].value.value.asUInt8;
                pokemon.basicSpd = personalFields[personalID].children[10].value.value.asUInt8;
                pokemon.basicSpAtk = personalFields[personalID].children[11].value.value.asUInt8;
                pokemon.basicSpDef = personalFields[personalID].children[12].value.value.asUInt8;
                pokemon.typingID1 = personalFields[personalID].children[13].value.value.asUInt8;
                pokemon.typingID2 = personalFields[personalID].children[14].value.value.asUInt8;
                pokemon.getRate = personalFields[personalID].children[15].value.value.asUInt8;
                pokemon.rank = personalFields[personalID].children[16].value.value.asUInt8;
                pokemon.expValue = personalFields[personalID].children[17].value.value.asUInt16;
                pokemon.item1 = personalFields[personalID].children[18].value.value.asUInt16;
                pokemon.item2 = personalFields[personalID].children[19].value.value.asUInt16;
                pokemon.item3 = personalFields[personalID].children[20].value.value.asUInt16;
                pokemon.sex = personalFields[personalID].children[21].value.value.asUInt8;
                pokemon.eggBirth = personalFields[personalID].children[22].value.value.asUInt8;
                pokemon.initialFriendship = personalFields[personalID].children[23].value.value.asUInt8;
                pokemon.eggGroup1 = personalFields[personalID].children[24].value.value.asUInt8;
                pokemon.eggGroup2 = personalFields[personalID].children[25].value.value.asUInt8;
                pokemon.grow = personalFields[personalID].children[26].value.value.asUInt8;
                pokemon.abilityID1 = personalFields[personalID].children[27].value.value.asUInt16;
                pokemon.abilityID2 = personalFields[personalID].children[28].value.value.asUInt16;
                pokemon.abilityID3 = personalFields[personalID].children[29].value.value.asUInt16;
                pokemon.giveExp = personalFields[personalID].children[30].value.value.asUInt16;
                pokemon.height = personalFields[personalID].children[31].value.value.asUInt16;
                pokemon.weight = personalFields[personalID].children[32].value.value.asUInt16;
                pokemon.chihouZukanNo = personalFields[personalID].children[33].value.value.asUInt16;
                pokemon.machine1 = personalFields[personalID].children[34].value.value.asUInt32;
                pokemon.machine2 = personalFields[personalID].children[35].value.value.asUInt32;
                pokemon.machine3 = personalFields[personalID].children[36].value.value.asUInt32;
                pokemon.machine4 = personalFields[personalID].children[37].value.value.asUInt32;
                pokemon.hiddenMachine = personalFields[personalID].children[38].value.value.asUInt32;
                pokemon.eggMonsno = personalFields[personalID].children[39].value.value.asUInt16;
                pokemon.eggFormno = personalFields[personalID].children[40].value.value.asUInt16;
                pokemon.eggFormnoKawarazunoishi = personalFields[personalID].children[41].value.value.asUInt16;
                pokemon.eggFormInheritKawarazunoishi = personalFields[personalID].children[42].value.value.asUInt8;

                pokemon.formID = 0;
                if (pokemon.personalID != pokemon.dexID)
                    pokemon.formID = pokemon.personalID - pokemon.formIndex + 1;
                pokemon.name = "";
                if (textFields[pokemon.dexID].children[6].children[0].childrenCount > 0)
                    pokemon.name = Encoding.UTF8.GetString(textFields[pokemon.dexID].children[6].children[0].children[0].children[4].value.value.asString);
                pokemon.nextEvoLvs = (ushort.MaxValue, ushort.MaxValue); //(wildLevel, trainerLevel)
                pokemon.pastPokemon = new();
                pokemon.nextPokemon = new();
                pokemon.inferiorForms = new();
                pokemon.superiorForms = new();

                //Parse level up moves
                pokemon.levelUpMoves = new();
                for (int levelUpMoveIdx = 0; levelUpMoveIdx < levelUpMoveFields[personalID].children[1].children[0].childrenCount; levelUpMoveIdx += 2)
                {
                    LevelUpMove levelUpMove = new();
                    levelUpMove.level = levelUpMoveFields[personalID].children[1].children[0].children[levelUpMoveIdx].value.value.asUInt16;
                    levelUpMove.moveID = levelUpMoveFields[personalID].children[1].children[0].children[levelUpMoveIdx + 1].value.value.asUInt16;

                    pokemon.levelUpMoves.Add(levelUpMove);
                }

                //Parse egg moves
                pokemon.eggMoves = new();
                for (int eggMoveIdx = 0; eggMoveIdx < eggMoveFields[personalID].children[2].children[0].childrenCount; eggMoveIdx++)
                    pokemon.eggMoves.Add(eggMoveFields[personalID].children[2].children[0].children[eggMoveIdx].value.value.asUInt16);

                //Parse evolutions
                pokemon.evolutionPaths = new();
                for (int evolutionIdx = 0; evolutionIdx < evolveFields[personalID].children[1].children[0].childrenCount; evolutionIdx += 5)
                {
                    EvolutionPath evolution = new();
                    evolution.method = evolveFields[personalID].children[1].children[0].children[evolutionIdx].value.value.asUInt16;
                    evolution.parameter = evolveFields[personalID].children[1].children[0].children[evolutionIdx + 1].value.value.asUInt16;
                    evolution.destDexID = evolveFields[personalID].children[1].children[0].children[evolutionIdx + 2].value.value.asUInt16;
                    evolution.destFormID = evolveFields[personalID].children[1].children[0].children[evolutionIdx + 3].value.value.asUInt16;
                    evolution.level = evolveFields[personalID].children[1].children[0].children[evolutionIdx + 4].value.value.asUInt16;

                    pokemon.evolutionPaths.Add(evolution);
                }

                gameData.personalEntries.Add(pokemon);

                if (gameData.dexEntries.Count == pokemon.dexID)
                {
                    gameData.dexEntries.Add(new());
                    gameData.dexEntries[pokemon.dexID].dexID = pokemon.dexID;
                    gameData.dexEntries[pokemon.dexID].forms = new();
                    gameData.dexEntries[pokemon.dexID].name = pokemon.name;
                }

                gameData.dexEntries[pokemon.dexID].forms.Add(pokemon);
            }

            SetFamilies();
            SetLegendaries();
        }

        private static void SetLegendaries()
        {
            AssetTypeValueField[] legendFields = fileManager.GetMonoBehaviours(PathEnum.Gamesettings).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "FieldEncountTable_d").children[10].children[0].children;
            for (int legendEntryIdx = 0; legendEntryIdx < legendFields.Length; legendEntryIdx++)
            {
                List<Pokemon> forms = gameData.dexEntries[legendFields[legendEntryIdx].children[0].value.value.asInt32].forms;
                for (int formID = 0; formID < forms.Count; formID++)
                    forms[formID].legendary = true;
            }
        }

        /// <summary>
        ///  Overwrites and updates all pokemons' evolution info for easier BST logic.
        /// </summary>
        public static void SetFamilies()
        {
            for (int dexID = 0; dexID < gameData.dexEntries.Count; dexID++)
            {
                for (int formID = 0; formID < gameData.dexEntries[dexID].forms.Count; formID++)
                {
                    Pokemon pokemon = gameData.dexEntries[dexID].forms[formID];
                    for (int evolutionIdx = 0; evolutionIdx < gameData.dexEntries[dexID].forms[formID].evolutionPaths.Count; evolutionIdx++)
                    {
                        EvolutionPath evo = pokemon.evolutionPaths[evolutionIdx];
                        Pokemon next = gameData.dexEntries[evo.destDexID].forms[evo.destFormID];

                        if (pokemon.dexID == next.dexID)
                            continue;

                        pokemon.nextPokemon.Add(next);
                        next.pastPokemon.Add(pokemon);
                    }

                    for (int formID2 = 0; formID2 < gameData.dexEntries[dexID].forms.Count; formID2++)
                    {
                        Pokemon pokemon2 = gameData.dexEntries[dexID].forms[formID2];
                        if (pokemon2.GetBST() - pokemon.GetBST() >= 30)
                        {
                            pokemon2.inferiorForms.Add(pokemon);
                            pokemon.superiorForms.Add(pokemon2);
                        }
                    }
                }
            }

            for (int dexID = 0; dexID < gameData.dexEntries.Count; dexID++)
            {
                for (int formID = 0; formID < gameData.dexEntries[dexID].forms.Count; formID++)
                {
                    Pokemon pokemon = gameData.dexEntries[dexID].forms[formID];
                    (ushort, ushort) evoLvs = GetEvoLvs(pokemon);
                    if (evoLvs == (0, 0))
                        continue;

                    pokemon.nextEvoLvs = evoLvs;

                    for (int evolutionIdx = 0; evolutionIdx < gameData.dexEntries[dexID].forms[formID].evolutionPaths.Count; evolutionIdx++)
                    {
                        EvolutionPath evo = pokemon.evolutionPaths[evolutionIdx];
                        Pokemon next = gameData.dexEntries[evo.destDexID].forms[evo.destFormID];

                        if (pokemon.dexID == next.dexID)
                            continue;

                        next.pastEvoLvs.Item1 = Math.Max(next.pastEvoLvs.Item1, evoLvs.Item1);
                        next.pastEvoLvs.Item2 = Math.Max(next.pastEvoLvs.Item2, evoLvs.Item2);
                    }
                }
            }
        }

        /// <summary>
        ///  Finds the levels the specified pokemon is likely to evolve: (wildLevel, trainerLevel).
        /// </summary>
        private static (ushort, ushort) GetEvoLvs(Pokemon pokemon)
        {
            (ushort, ushort) evoLvs = (0, 0);
            for (int evolutionIdx = 0; evolutionIdx < pokemon.evolutionPaths.Count; evolutionIdx++)
            {
                EvolutionPath evo = pokemon.evolutionPaths[evolutionIdx];
                if (pokemon.dexID == evo.destDexID)
                    continue;

                switch (evo.method)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 21:
                    case 29:
                    case 43:
                        for (int pastEvos = 0; pastEvos < pokemon.pastPokemon.Count; pastEvos++)
                        {
                            (ushort, ushort) pastPokemonEvoLvs = GetEvoLvs(pokemon.pastPokemon[pastEvos]);
                            evoLvs.Item1 = Math.Max(evoLvs.Item1, pastPokemonEvoLvs.Item1);
                            evoLvs.Item2 = Math.Max(evoLvs.Item2, pastPokemonEvoLvs.Item2);
                        }
                        if (evoLvs == (0, 0))
                            evoLvs = (1, 1);
                        evoLvs.Item1 += 16;
                        evoLvs.Item2 += 16;
                        break;
                    case 4:
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                    case 23:
                    case 24:
                    case 28:
                    case 32:
                    case 33:
                    case 34:
                    case 36:
                    case 37:
                    case 38:
                    case 40:
                    case 41:
                    case 46:
                    case 47:
                        evoLvs.Item1 = evo.level;
                        evoLvs.Item2 = evo.level;
                        break;
                    case 5:
                    case 8:
                    case 17:
                    case 18:
                    case 22:
                    case 25:
                    case 26:
                    case 27:
                    case 39:
                    case 42:
                    case 44:
                    case 45:
                        for (int pastEvos = 0; pastEvos < pokemon.pastPokemon.Count; pastEvos++)
                        {
                            (ushort, ushort) pastPokemonEvoLvs = GetEvoLvs(pokemon.pastPokemon[pastEvos]);
                            evoLvs.Item1 = Math.Max(evoLvs.Item1, pastPokemonEvoLvs.Item1);
                            evoLvs.Item2 = Math.Max(evoLvs.Item2, pastPokemonEvoLvs.Item2);
                        }
                        if (evoLvs == (0, 0))
                            evoLvs = (1, 1);
                        evoLvs.Item1 += 32;
                        evoLvs.Item2 += 16;
                        break;
                    case 6:
                    case 7:
                        for (int pastEvos = 0; pastEvos < pokemon.pastPokemon.Count; pastEvos++)
                        {
                            (ushort, ushort) pastPokemonEvoLvs = GetEvoLvs(pokemon.pastPokemon[pastEvos]);
                            evoLvs.Item1 = Math.Max(evoLvs.Item1, pastPokemonEvoLvs.Item1);
                            evoLvs.Item2 = Math.Max(evoLvs.Item2, pastPokemonEvoLvs.Item2);
                        }
                        if (evoLvs == (0, 0))
                            evoLvs = (1, 1);
                        evoLvs.Item1 += 48;
                        evoLvs.Item2 += 16;
                        break;
                    case 16:
                        for (int pastEvos = 0; pastEvos < pokemon.pastPokemon.Count; pastEvos++)
                        {
                            (ushort, ushort) pastPokemonEvoLvs = GetEvoLvs(pokemon.pastPokemon[pastEvos]);
                            evoLvs.Item1 = Math.Max(evoLvs.Item1, pastPokemonEvoLvs.Item1);
                            evoLvs.Item2 = Math.Max(evoLvs.Item2, pastPokemonEvoLvs.Item2);
                        }
                        if (evoLvs == (0, 0))
                            evoLvs = (1, 1);
                        evoLvs.Item1 += 48;
                        evoLvs.Item2 += 32;
                        break;
                    case 19:
                    case 20:
                    case 30:
                    case 31:
                        evoLvs.Item1 = (ushort)(evo.level + 16);
                        evoLvs.Item2 = evo.level;
                        break;
                }
            }
            return evoLvs;
        }

        /// <summary>
        ///  Overwrites GlobalData with parsed TMs.
        /// </summary>
        private static void ParseTMs()
        {
            gameData.tms = new();
            AssetTypeValueField monoBehaviour = fileManager.GetMonoBehaviours(PathEnum.PersonalMasterdatas).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "ItemTable");
            AssetTypeValueField textData = fileManager.GetMonoBehaviours(PathEnum.English).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "english_ss_itemname");

            AssetTypeValueField[] tmFields = monoBehaviour.children[5].children[0].children;
            AssetTypeValueField[] textFields = textData.children[8].children[0].children;
            for (int tmID = 0; tmID < tmFields.Length; tmID++)
            {
                TM tm = new();
                tm.itemID = tmFields[tmID].children[0].value.value.asInt32;
                tm.machineNo = tmFields[tmID].children[1].value.value.asInt32;
                tm.moveID = tmFields[tmID].children[2].value.value.asInt32;

                tm.tmID = tmID;
                tm.name = "";
                if (textFields[tm.itemID].children[6].children[0].childrenCount > 0)
                    tm.name = Encoding.UTF8.GetString(textFields[tm.itemID].children[6].children[0].children[0].children[4].value.value.asString);

                gameData.tms.Add(tm);
            }
        }
        private static void ParsePersonalMasterDatas()
        {
            // AssetTypeValueField addPersonalTable = fileManager.GetMonoBehaviours(PathEnum.PersonalMasterdatas).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "AddPersonalTable");
            // AssetTypeValueField[] addPersonalTableArray = addPersonalTable["AddPersonalTable"].children[0].children;
        }

        private static void ParseMasterDatas()
        {
            gameData.pokemonInfos = new();
            AssetTypeValueField pokemonInfo = fileManager.GetMonoBehaviours(PathEnum.DprMasterdatas).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "PokemonInfo");
            AssetTypeValueField[] catalogArray = pokemonInfo["Catalog"].children[0].children;

            for (int i = 0; i < catalogArray.Length; i++)
            {
                Masterdatas.PokemonInfoCatalog catalog = new();
                catalog.UniqueID = catalogArray[i]["UniqueID"].value.value.asInt32;
                catalog.No = catalogArray[i]["No"].value.value.asInt32;
                catalog.SinnohNo = catalogArray[i]["SinnohNo"].value.value.asInt32;
                catalog.MonsNo = catalogArray[i]["MonsNo"].value.value.asInt32;
                catalog.FormNo = catalogArray[i]["FormNo"].value.value.asInt32;
                catalog.Sex = catalogArray[i]["Sex"].value.value.asUInt8;
                catalog.Rare = catalogArray[i]["Rare"].value.value.asUInt8 == 0;
                catalog.AssetBundleName = catalogArray[i]["AssetBundleName"].GetValue().AsString();
                catalog.BattleScale = catalogArray[i]["BattleScale"].value.value.asFloat;
                catalog.ContestScale = catalogArray[i]["ContestScale"].value.value.asFloat;
                catalog.ContestSize = (Masterdatas.Size) catalogArray[i]["ContestSize"].value.value.asInt32;
                catalog.FieldScale = catalogArray[i]["FieldScale"].value.value.asFloat;
                catalog.FieldChikaScale = catalogArray[i]["FieldChikaScale"].value.value.asFloat;
                catalog.StatueScale = catalogArray[i]["StatueScale"].value.value.asFloat;
                catalog.FieldWalkingScale = catalogArray[i]["FieldWalkingScale"].value.value.asFloat;
                catalog.FieldFureaiScale = catalogArray[i]["FieldFureaiScale"].value.value.asFloat;
                catalog.MenuScale = catalogArray[i]["MenuScale"].value.value.asFloat;
                catalog.ModelMotion = catalogArray[i]["ModelMotion"].GetValue().AsString();

                catalog.ModelOffset = new();
                catalog.ModelOffset.X = catalogArray[i]["ModelOffset"].children[0].value.value.asFloat;
                catalog.ModelOffset.Y = catalogArray[i]["ModelOffset"].children[1].value.value.asFloat;
                catalog.ModelOffset.Z = catalogArray[i]["ModelOffset"].children[2].value.value.asFloat;

                catalog.ModelRotationAngle = new();
                catalog.ModelRotationAngle.X = catalogArray[i]["ModelRotationAngle"].children[0].value.value.asFloat;
                catalog.ModelRotationAngle.Y = catalogArray[i]["ModelRotationAngle"].children[1].value.value.asFloat;
                catalog.ModelRotationAngle.Z = catalogArray[i]["ModelRotationAngle"].children[2].value.value.asFloat;

                catalog.DistributionScale = catalogArray[i]["DistributionScale"].value.value.asFloat;
                catalog.DistributionModelMotion = catalogArray[i]["DistributionModelMotion"].GetValue().AsString();

                catalog.DistributionModelOffset = new();
                catalog.DistributionModelOffset.X = catalogArray[i]["DistributionModelOffset"].children[0].value.value.asFloat;
                catalog.DistributionModelOffset.Y = catalogArray[i]["DistributionModelOffset"].children[1].value.value.asFloat;
                catalog.DistributionModelOffset.Z = catalogArray[i]["DistributionModelOffset"].children[2].value.value.asFloat;

                catalog.DistributionModelRotationAngle = new();
                catalog.DistributionModelRotationAngle.X = catalogArray[i]["DistributionModelRotationAngle"].children[0].value.value.asFloat;
                catalog.DistributionModelRotationAngle.Y = catalogArray[i]["DistributionModelRotationAngle"].children[1].value.value.asFloat;
                catalog.DistributionModelRotationAngle.Z = catalogArray[i]["DistributionModelRotationAngle"].children[2].value.value.asFloat;

                catalog.VoiceScale = catalogArray[i]["VoiceScale"].value.value.asFloat;
                catalog.VoiceModelMotion = catalogArray[i]["VoiceModelMotion"].GetValue().AsString();

                catalog.VoiceModelOffset = new();
                catalog.VoiceModelOffset.X = catalogArray[i]["VoiceModelOffset"].children[0].value.value.asFloat;
                catalog.VoiceModelOffset.Y = catalogArray[i]["VoiceModelOffset"].children[1].value.value.asFloat;
                catalog.VoiceModelOffset.Z = catalogArray[i]["VoiceModelOffset"].children[2].value.value.asFloat;

                catalog.VoiceModelRotationAngle = new();
                catalog.VoiceModelRotationAngle.X = catalogArray[i]["VoiceModelRotationAngle"].children[0].value.value.asFloat;
                catalog.VoiceModelRotationAngle.Y = catalogArray[i]["VoiceModelRotationAngle"].children[1].value.value.asFloat;
                catalog.VoiceModelRotationAngle.Z = catalogArray[i]["VoiceModelRotationAngle"].children[2].value.value.asFloat;

                catalog.CenterPointOffset = new();
                catalog.CenterPointOffset.X = catalogArray[i]["CenterPointOffset"].children[0].value.value.asFloat;
                catalog.CenterPointOffset.Y = catalogArray[i]["CenterPointOffset"].children[1].value.value.asFloat;
                catalog.CenterPointOffset.Z = catalogArray[i]["CenterPointOffset"].children[2].value.value.asFloat;

                catalog.RotationLimitAngle = new();
                catalog.RotationLimitAngle.X = catalogArray[i]["RotationLimitAngle"].children[0].value.value.asFloat;
                catalog.RotationLimitAngle.Y = catalogArray[i]["RotationLimitAngle"].children[1].value.value.asFloat;

                catalog.StatusScale = catalogArray[i]["StatusScale"].value.value.asFloat;
                catalog.StatusModelMotion = catalogArray[i]["StatusModelMotion"].GetValue().AsString();

                catalog.StatusModelOffset = new();
                catalog.StatusModelOffset.X = catalogArray[i]["StatusModelOffset"].children[0].value.value.asFloat;
                catalog.StatusModelOffset.Y = catalogArray[i]["StatusModelOffset"].children[1].value.value.asFloat;
                catalog.StatusModelOffset.Z = catalogArray[i]["StatusModelOffset"].children[2].value.value.asFloat;

                catalog.StatusModelRotationAngle = new();
                catalog.StatusModelRotationAngle.X = catalogArray[i]["StatusModelRotationAngle"].children[0].value.value.asFloat;
                catalog.StatusModelRotationAngle.Y = catalogArray[i]["StatusModelRotationAngle"].children[1].value.value.asFloat;
                catalog.StatusModelRotationAngle.Z = catalogArray[i]["StatusModelRotationAngle"].children[2].value.value.asFloat;

                catalog.BoxScale = catalogArray[i]["BoxScale"].value.value.asFloat;
                catalog.BoxModelMotion = catalogArray[i]["BoxModelMotion"].GetValue().AsString();

                catalog.BoxModelOffset = new();
                catalog.BoxModelOffset.X = catalogArray[i]["BoxModelOffset"].children[0].value.value.asFloat;
                catalog.BoxModelOffset.Y = catalogArray[i]["BoxModelOffset"].children[1].value.value.asFloat;
                catalog.BoxModelOffset.Z = catalogArray[i]["BoxModelOffset"].children[2].value.value.asFloat;

                catalog.BoxModelRotationAngle = new();
                catalog.BoxModelRotationAngle.X = catalogArray[i]["BoxModelRotationAngle"].children[0].value.value.asFloat;
                catalog.BoxModelRotationAngle.Y = catalogArray[i]["BoxModelRotationAngle"].children[1].value.value.asFloat;
                catalog.BoxModelRotationAngle.Z = catalogArray[i]["BoxModelRotationAngle"].children[2].value.value.asFloat;

                catalog.CompareScale = catalogArray[i]["CompareScale"].value.value.asFloat;
                catalog.CompareModelMotion = catalogArray[i]["CompareModelMotion"].GetValue().AsString();

                catalog.CompareModelOffset = new();
                catalog.CompareModelOffset.X = catalogArray[i]["CompareModelOffset"].children[0].value.value.asFloat;
                catalog.CompareModelOffset.Y = catalogArray[i]["CompareModelOffset"].children[1].value.value.asFloat;
                catalog.CompareModelOffset.Z = catalogArray[i]["CompareModelOffset"].children[2].value.value.asFloat;

                catalog.CompareModelRotationAngle = new();
                catalog.CompareModelRotationAngle.X = catalogArray[i]["CompareModelRotationAngle"].children[0].value.value.asFloat;
                catalog.CompareModelRotationAngle.Y = catalogArray[i]["CompareModelRotationAngle"].children[1].value.value.asFloat;
                catalog.CompareModelRotationAngle.Z = catalogArray[i]["CompareModelRotationAngle"].children[2].value.value.asFloat;

                catalog.BrakeStart = catalogArray[i]["BrakeStart"].value.value.asFloat;
                catalog.BrakeEnd = catalogArray[i]["BrakeEnd"].value.value.asFloat;
                catalog.WalkSpeed = catalogArray[i]["WalkSpeed"].value.value.asFloat;
                catalog.RunSpeed = catalogArray[i]["RunSpeed"].value.value.asFloat;
                catalog.WalkStart = catalogArray[i]["WalkStart"].value.value.asFloat;
                catalog.RunStart = catalogArray[i]["RunStart"].value.value.asFloat;
                catalog.BodySize = catalogArray[i]["BodySize"].value.value.asFloat;
                catalog.AppearLimit = catalogArray[i]["AppearLimit"].value.value.asFloat;
                catalog.MoveType = (Masterdatas.MoveType) catalogArray[i]["MoveType"].value.value.asInt32;

                catalog.GroundEffect = catalogArray[i]["GroundEffect"].value.value.asUInt8 == 0;
                catalog.Waitmoving = catalogArray[i]["Waitmoving"].value.value.asUInt8 == 0;
                catalog.BattleAjustHeight = catalogArray[i]["BattleAjustHeight"].value.value.asInt32;

                gameData.pokemonInfos.Add(catalog);
            }
        }
        private static void ParseBattleMasterDatas()
        {
            gameData.motionTimingData = new();
            AssetTypeValueField battleDataTable = fileManager.GetMonoBehaviours(PathEnum.BattleMasterdatas).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "BattleDataTable");
            AssetTypeValueField[] motionTimingDataArray = battleDataTable["MotionTimingData"].children[0].children;

            for (int i = 0; i < motionTimingDataArray.Length; i++)
            {
                BattleMasterdatas.MotionTimingData motionTimingData = new();
                motionTimingData.MonsNo = motionTimingDataArray[i]["MonsNo"].value.value.asInt32;
                motionTimingData.FormNo = motionTimingDataArray[i]["FormNo"].value.value.asInt32;
                motionTimingData.Sex = motionTimingDataArray[i]["Sex"].value.value.asInt32;
                motionTimingData.Buturi01 = motionTimingDataArray[i]["Buturi01"].value.value.asInt32;
                motionTimingData.Buturi02 = motionTimingDataArray[i]["Buturi02"].value.value.asInt32;
                motionTimingData.Buturi03 = motionTimingDataArray[i]["Buturi03"].value.value.asInt32;
                motionTimingData.Tokusyu01 = motionTimingDataArray[i]["Tokusyu01"].value.value.asInt32;
                motionTimingData.Tokusyu02 = motionTimingDataArray[i]["Tokusyu02"].value.value.asInt32;
                motionTimingData.Tokusyu03 = motionTimingDataArray[i]["Tokusyu03"].value.value.asInt32;
                motionTimingData.BodyBlow = motionTimingDataArray[i]["BodyBlow"].value.value.asInt32;
                motionTimingData.Punch = motionTimingDataArray[i]["Punch"].value.value.asInt32;
                motionTimingData.Kick = motionTimingDataArray[i]["Kick"].value.value.asInt32;
                motionTimingData.Tail = motionTimingDataArray[i]["Tail"].value.value.asInt32;
                motionTimingData.Bite = motionTimingDataArray[i]["Bite"].value.value.asInt32;
                motionTimingData.Peck = motionTimingDataArray[i]["Peck"].value.value.asInt32;
                motionTimingData.Radial = motionTimingDataArray[i]["Radial"].value.value.asInt32;
                motionTimingData.Cry = motionTimingDataArray[i]["Cry"].value.value.asInt32;
                motionTimingData.Dust = motionTimingDataArray[i]["Dust"].value.value.asInt32;
                motionTimingData.Shot = motionTimingDataArray[i]["Shot"].value.value.asInt32;
                motionTimingData.Guard = motionTimingDataArray[i]["Guard"].value.value.asInt32;
                motionTimingData.LandingFall = motionTimingDataArray[i]["LandingFall"].value.value.asInt32;
                motionTimingData.LandingFallEase = motionTimingDataArray[i]["LandingFallEase"].value.value.asInt32;

                gameData.motionTimingData.Add(motionTimingData);
            }
        }

        /// <summary>
        ///  Overwrites GlobalData with parsed Moves.
        /// </summary>
        private static void ParseMoves()
        {
            gameData.moves = new();
            AssetTypeValueField monoBehaviour = fileManager.GetMonoBehaviours(PathEnum.PersonalMasterdatas).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "WazaTable");
            AssetTypeValueField animationData = fileManager.GetMonoBehaviours(PathEnum.BattleMasterdatas).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "BattleDataTable");
            AssetTypeValueField textData = fileManager.GetMonoBehaviours(PathEnum.English).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "english_ss_wazaname");

            AssetTypeValueField[] moveFields = monoBehaviour.children[4].children[0].children;
            AssetTypeValueField[] animationFields = animationData.children[8].children[0].children;
            AssetTypeValueField[] textFields = textData.children[8].children[0].children;
            for (int moveID = 0; moveID < moveFields.Length; moveID++)
            {
                Move move = new();
                move.moveID = moveFields[moveID].children[0].value.value.asInt32;
                move.isValid = moveFields[moveID].children[1].value.value.asUInt8;
                move.typingID = moveFields[moveID].children[2].value.value.asUInt8;
                move.category = moveFields[moveID].children[3].value.value.asUInt8;
                move.damageCategoryID = moveFields[moveID].children[4].value.value.asUInt8;
                move.power = moveFields[moveID].children[5].value.value.asUInt8;
                move.hitPer = moveFields[moveID].children[6].value.value.asUInt8;
                move.basePP = moveFields[moveID].children[7].value.value.asUInt8;
                move.priority = moveFields[moveID].children[8].value.value.asInt8;
                move.hitCountMax = moveFields[moveID].children[9].value.value.asUInt8;
                move.hitCountMin = moveFields[moveID].children[10].value.value.asUInt8;
                move.sickID = moveFields[moveID].children[11].value.value.asUInt16;
                move.sickPer = moveFields[moveID].children[12].value.value.asUInt8;
                move.sickCont = moveFields[moveID].children[13].value.value.asUInt8;
                move.sickTurnMin = moveFields[moveID].children[14].value.value.asUInt8;
                move.sickTurnMax = moveFields[moveID].children[15].value.value.asUInt8;
                move.criticalRank = moveFields[moveID].children[16].value.value.asUInt8;
                move.shrinkPer = moveFields[moveID].children[17].value.value.asUInt8;
                move.aiSeqNo = moveFields[moveID].children[18].value.value.asUInt16;
                move.damageRecoverRatio = moveFields[moveID].children[19].value.value.asInt8;
                move.hpRecoverRatio = moveFields[moveID].children[20].value.value.asInt8;
                move.target = moveFields[moveID].children[21].value.value.asUInt8;
                move.rankEffType1 = moveFields[moveID].children[22].value.value.asUInt8;
                move.rankEffType2 = moveFields[moveID].children[23].value.value.asUInt8;
                move.rankEffType3 = moveFields[moveID].children[24].value.value.asUInt8;
                move.rankEffValue1 = moveFields[moveID].children[25].value.value.asInt8;
                move.rankEffValue2 = moveFields[moveID].children[26].value.value.asInt8;
                move.rankEffValue3 = moveFields[moveID].children[27].value.value.asInt8;
                move.rankEffPer1 = moveFields[moveID].children[28].value.value.asUInt8;
                move.rankEffPer2 = moveFields[moveID].children[29].value.value.asUInt8;
                move.rankEffPer3 = moveFields[moveID].children[30].value.value.asUInt8;
                move.flags = moveFields[moveID].children[31].value.value.asUInt32;
                move.contestWazaNo = moveFields[moveID].children[32].value.value.asUInt32;

                move.cmdSeqName = animationFields[moveID].children[1].GetValue().AsString();
                move.cmdSeqNameLegend = animationFields[moveID].children[2].GetValue().AsString();
                move.notShortenTurnType0 = animationFields[moveID].children[3].GetValue().AsString();
                move.notShortenTurnType1 = animationFields[moveID].children[4].GetValue().AsString();
                move.turnType1 = animationFields[moveID].children[5].GetValue().AsString();
                move.turnType2 = animationFields[moveID].children[6].GetValue().AsString();
                move.turnType3 = animationFields[moveID].children[7].GetValue().AsString();
                move.turnType4 = animationFields[moveID].children[8].GetValue().AsString();

                move.name = "";
                if (textFields[moveID].children[6].children[0].childrenCount > 0)
                    move.name = Encoding.UTF8.GetString(textFields[moveID].children[6].children[0].children[0].children[4].value.value.asString);

                gameData.moves.Add(move);
            }
        }

        /// <summary>
        ///  Overwrites GlobalData with parsed ShopTables.
        /// </summary>
        private static void ParseShopTables()
        {
            gameData.shopTables = new();
            AssetTypeValueField monoBehaviour = fileManager.GetMonoBehaviours(PathEnum.DprMasterdatas).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "ShopTable");

            gameData.shopTables.martItems = new();
            AssetTypeValueField[] martItemFields = monoBehaviour.children[4].children[0].children;
            for (int martItemIdx = 0; martItemIdx < martItemFields.Length; martItemIdx++)
            {
                MartItem martItem = new();
                martItem.itemID = martItemFields[martItemIdx].children[0].value.value.asUInt16;
                martItem.badgeNum = martItemFields[martItemIdx].children[1].value.value.asInt32;
                martItem.zoneID = martItemFields[martItemIdx].children[2].value.value.asInt32;

                gameData.shopTables.martItems.Add(martItem);
            }

            gameData.shopTables.fixedShopItems = new();
            AssetTypeValueField[] fixedShopItemFields = monoBehaviour.children[5].children[0].children;
            for (int fixedShopItemIdx = 0; fixedShopItemIdx < fixedShopItemFields.Length; fixedShopItemIdx++)
            {
                FixedShopItem fixedShopItem = new();
                fixedShopItem.itemID = fixedShopItemFields[fixedShopItemIdx].children[0].value.value.asUInt16;
                fixedShopItem.shopID = fixedShopItemFields[fixedShopItemIdx].children[1].value.value.asInt32;

                gameData.shopTables.fixedShopItems.Add(fixedShopItem);
            }

            gameData.shopTables.bpShopItems = new();
            AssetTypeValueField[] bpShopItemFields = monoBehaviour.children[9].children[0].children;
            for (int bpShopItemIdx = 0; bpShopItemIdx < bpShopItemFields.Length; bpShopItemIdx++)
            {
                BpShopItem bpShopItem = new();
                bpShopItem.itemID = bpShopItemFields[bpShopItemIdx].children[0].value.value.asUInt16;
                try
                {
                    bpShopItem.npcID = bpShopItemFields[bpShopItemIdx].children[1].value.value.asInt32;
                }
                catch (IndexOutOfRangeException e)
                {
                    MainForm.ShowParserError("Oh my, this dump might be a bit outdated...\n" +
                        "Please input at least the v1.1.3 version of BDSP.\n" +
                        "I don't feel so good...");
                    throw;
                }

                gameData.shopTables.bpShopItems.Add(bpShopItem);
            }
        }

        /// <summary>
        ///  Overwrites GlobalData with parsed PickupItems.
        /// </summary>
        private static void ParsePickupItems()
        {
            gameData.pickupItems = new();
            AssetTypeValueField monoBehaviour = fileManager.GetMonoBehaviours(PathEnum.DprMasterdatas).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "MonohiroiTable");

            AssetTypeValueField[] pickupItemFields = monoBehaviour.children[4].children[0].children;
            for (int pickupItemIdx = 0; pickupItemIdx < pickupItemFields.Length; pickupItemIdx++)
            {
                PickupItem pickupItem = new();
                pickupItem.itemID = pickupItemFields[pickupItemIdx].children[0].value.value.asUInt16;

                //Parse item probabilities
                pickupItem.ratios = new();
                for (int ratio = 0; ratio < pickupItemFields[pickupItemIdx].children[1].children[0].childrenCount; ratio++)
                    pickupItem.ratios.Add(pickupItemFields[pickupItemIdx].children[1].children[0].children[ratio].value.value.asUInt8);

                gameData.pickupItems.Add(pickupItem);
            }
        }

        /// <summary>
        ///  Overwrites GlobalData with parsed Items.
        /// </summary>
        private static void ParseItems()
        {
            gameData.items = new();
            AssetTypeValueField monoBehaviour = fileManager.GetMonoBehaviours(PathEnum.PersonalMasterdatas).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "ItemTable");
            AssetTypeValueField textData = fileManager.GetMonoBehaviours(PathEnum.English).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "english_ss_itemname");


            AssetTypeValueField[] itemFields = monoBehaviour.children[4].children[0].children;
            AssetTypeValueField[] textFields = textData.children[8].children[0].children;
            for (int itemIdx = 0; itemIdx < itemFields.Length; itemIdx++)
            {
                Item item = new();
                item.itemID = itemFields[itemIdx].children[0].value.value.asInt16;
                item.type = itemFields[itemIdx].children[1].value.value.asUInt8;
                item.iconID = itemFields[itemIdx].children[2].value.value.asInt32;
                item.price = itemFields[itemIdx].children[3].value.value.asInt32;
                item.bpPrice = itemFields[itemIdx].children[4].value.value.asInt32;
                item.nageAtc = itemFields[itemIdx].children[7].value.value.asUInt8;
                item.sizenAtc = itemFields[itemIdx].children[8].value.value.asUInt8;
                item.sizenType = itemFields[itemIdx].children[9].value.value.asUInt8;
                item.tuibamuEff = itemFields[itemIdx].children[10].value.value.asUInt8;
                item.sort = itemFields[itemIdx].children[11].value.value.asUInt8;
                item.group = itemFields[itemIdx].children[12].value.value.asUInt8;
                item.groupID = itemFields[itemIdx].children[13].value.value.asUInt8;
                item.fldPocket = itemFields[itemIdx].children[14].value.value.asUInt8;
                item.fieldFunc = itemFields[itemIdx].children[15].value.value.asUInt8;
                item.battleFunc = itemFields[itemIdx].children[16].value.value.asUInt8;
                item.criticalRanks = itemFields[itemIdx].children[18].value.value.asUInt8;
                item.atkStages = itemFields[itemIdx].children[19].value.value.asUInt8;
                item.defStages = itemFields[itemIdx].children[20].value.value.asUInt8;
                item.spdStages = itemFields[itemIdx].children[21].value.value.asUInt8;
                item.accStages = itemFields[itemIdx].children[22].value.value.asUInt8;
                item.spAtkStages = itemFields[itemIdx].children[23].value.value.asUInt8;
                item.spDefStages = itemFields[itemIdx].children[24].value.value.asUInt8;
                item.ppRestoreAmount = itemFields[itemIdx].children[25].value.value.asUInt8;
                item.hpEvIncrease = itemFields[itemIdx].children[26].value.value.asInt8;
                item.atkEvIncrease = itemFields[itemIdx].children[27].value.value.asInt8;
                item.defEvIncrease = itemFields[itemIdx].children[28].value.value.asInt8;
                item.spdEvIncrease = itemFields[itemIdx].children[29].value.value.asInt8;
                item.spAtkEvIncrease = itemFields[itemIdx].children[30].value.value.asInt8;
                item.spDefEvIncrease = itemFields[itemIdx].children[31].value.value.asInt8;
                item.friendshipIncrease1 = itemFields[itemIdx].children[32].value.value.asInt8;
                item.friendshipIncrease2 = itemFields[itemIdx].children[33].value.value.asInt8;
                item.friendshipIncrease3 = itemFields[itemIdx].children[34].value.value.asInt8;
                item.hpRestoreAmount = itemFields[itemIdx].children[35].value.value.asUInt8;
                item.flags0 = itemFields[itemIdx].children[36].value.value.asUInt32;

                item.name = "";
                if (textFields[itemIdx].children[6].children[0].childrenCount > 0)
                    item.name = Encoding.UTF8.GetString(textFields[itemIdx].children[6].children[0].children[0].children[4].value.value.asString);

                gameData.items.Add(item);
            }
        }

        /// <summary>
        ///  Overwrites GlobalData with parsed growth rates.
        /// </summary>
        private static void ParseGrowthRates()
        {
            gameData.growthRates = new();
            AssetTypeValueField monoBehaviour = fileManager.GetMonoBehaviours(PathEnum.PersonalMasterdatas).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "GrowTable");

            AssetTypeValueField[] growthRateFields = monoBehaviour.children[4].children[0].children;
            for (int growthRateIdx = 0; growthRateIdx < growthRateFields.Length; growthRateIdx++)
            {
                GrowthRate growthRate = new();
                growthRate.growthID = growthRateIdx;

                //Parse exp requirement
                growthRate.expRequirements = new();
                for (int level = 0; level < growthRateFields[growthRateIdx].children[0].children[0].childrenCount; level++)
                    growthRate.expRequirements.Add(growthRateFields[growthRateIdx].children[0].children[0].children[level].value.value.asUInt32);

                gameData.growthRates.Add(growthRate);
            }
        }

        /// <summary>
        ///  Overwrites GlobalData with parsed MessageFiles.
        /// </summary>
        public static void ParseAllMessageFiles()
        {
            gameData.messageFileSets = new MessageFileSet[10];
            for (int i = 0; i < gameData.messageFileSets.Length; i++)
            {
                gameData.messageFileSets[i] = new();
                gameData.messageFileSets[i].messageFiles = new();
            }
            gameData.messageFileSets[0].langID = 1;
            gameData.messageFileSets[1].langID = 1;
            gameData.messageFileSets[2].langID = 2;
            gameData.messageFileSets[3].langID = 3;
            gameData.messageFileSets[4].langID = 4;
            gameData.messageFileSets[5].langID = 5;
            gameData.messageFileSets[6].langID = 7;
            gameData.messageFileSets[7].langID = 8;
            gameData.messageFileSets[8].langID = 9;
            gameData.messageFileSets[9].langID = 10;

            List<AssetTypeValueField> monoBehaviours = fileManager.GetMonoBehaviours(PathEnum.CommonMsbt);
            monoBehaviours.AddRange(fileManager.GetMonoBehaviours(PathEnum.English));
            monoBehaviours.AddRange(fileManager.GetMonoBehaviours(PathEnum.French));
            monoBehaviours.AddRange(fileManager.GetMonoBehaviours(PathEnum.German));
            monoBehaviours.AddRange(fileManager.GetMonoBehaviours(PathEnum.Italian));
            monoBehaviours.AddRange(fileManager.GetMonoBehaviours(PathEnum.Jpn));
            monoBehaviours.AddRange(fileManager.GetMonoBehaviours(PathEnum.JpnKanji));
            monoBehaviours.AddRange(fileManager.GetMonoBehaviours(PathEnum.Korean));
            monoBehaviours.AddRange(fileManager.GetMonoBehaviours(PathEnum.SimpChinese));
            monoBehaviours.AddRange(fileManager.GetMonoBehaviours(PathEnum.Spanish));
            monoBehaviours.AddRange(fileManager.GetMonoBehaviours(PathEnum.TradChinese));

            for (int mIdx = 0; mIdx < monoBehaviours.Count; mIdx++)
            {
                MessageFile messageFile = new();
                messageFile.mName = Encoding.Default.GetString(monoBehaviours[mIdx].children[3].value.value.asString);
                messageFile.langID = monoBehaviours[mIdx].children[5].value.value.asInt32;
                messageFile.isKanji = monoBehaviours[mIdx].children[7].value.value.asUInt8;

                //Parse LabelData
                messageFile.labelDatas = new();
                AssetTypeValueField[] labelDataFields = monoBehaviours[mIdx].children[8].children[0].children;
                for (int labelDataIdx = 0; labelDataIdx < labelDataFields.Length; labelDataIdx++)
                {
                    LabelData labelData = new();
                    labelData.labelIndex = labelDataFields[labelDataIdx].children[0].value.value.asInt32;
                    labelData.arrayIndex = labelDataFields[labelDataIdx].children[1].value.value.asInt32;
                    labelData.labelName = Encoding.Default.GetString(labelDataFields[labelDataIdx].children[2].value.value.asString);
                    labelData.styleIndex = labelDataFields[labelDataIdx].children[3].children[0].value.value.asInt32;
                    labelData.colorIndex = labelDataFields[labelDataIdx].children[3].children[1].value.value.asInt32;
                    labelData.fontSize = labelDataFields[labelDataIdx].children[3].children[2].value.value.asInt32;
                    labelData.maxWidth = labelDataFields[labelDataIdx].children[3].children[3].value.value.asInt32;
                    labelData.controlID = labelDataFields[labelDataIdx].children[3].children[4].value.value.asInt32;

                    //Parse WordData
                    labelData.wordDatas = new();
                    AssetTypeValueField[] wordDataFields = labelDataFields[labelDataIdx].children[6].children[0].children;
                    for (int wordDataIdx = 0; wordDataIdx < wordDataFields.Length; wordDataIdx++)
                    {
                        WordData wordData = new();
                        wordData.patternID = wordDataFields[wordDataIdx].children[0].value.value.asInt32;
                        wordData.eventID = wordDataFields[wordDataIdx].children[1].value.value.asInt32;
                        wordData.tagIndex = wordDataFields[wordDataIdx].children[2].value.value.asInt32;
                        wordData.tagValue = wordDataFields[wordDataIdx].children[3].value.value.asFloat;
                        wordData.str = Encoding.UTF8.GetString(wordDataFields[wordDataIdx].children[4].value.value.asString);
                        wordData.strWidth = wordDataFields[wordDataIdx].children[5].value.value.asFloat;

                        labelData.wordDatas.Add(wordData);
                    }

                    messageFile.labelDatas.Add(labelData);
                }

                switch (messageFile.langID)
                {
                    case 1:
                        if (messageFile.isKanji == 0)
                            gameData.messageFileSets[0].messageFiles.Add(messageFile);
                        else
                            gameData.messageFileSets[1].messageFiles.Add(messageFile);
                        break;
                    case 2:
                        gameData.messageFileSets[2].messageFiles.Add(messageFile);
                        break;
                    case 3:
                        gameData.messageFileSets[3].messageFiles.Add(messageFile);
                        break;
                    case 4:
                        gameData.messageFileSets[4].messageFiles.Add(messageFile);
                        break;
                    case 5:
                        gameData.messageFileSets[5].messageFiles.Add(messageFile);
                        break;
                    case 7:
                        gameData.messageFileSets[6].messageFiles.Add(messageFile);
                        break;
                    case 8:
                        gameData.messageFileSets[7].messageFiles.Add(messageFile);
                        break;
                    case 9:
                        gameData.messageFileSets[8].messageFiles.Add(messageFile);
                        break;
                    case 10:
                        gameData.messageFileSets[9].messageFiles.Add(messageFile);
                        break;
                }
            }
        }

        /// <summary>
        ///  Overwrites GlobalData with parsed MapWarpAssets.
        /// </summary>
        private static void ParseMapWarpAssets()
        {
            gameData.mapWarpAssets = new();
            List<int> areaIDs = GenerateAreaIDList();
            List<AssetTypeValueField> monoBehaviours = fileManager.GetMonoBehaviours(PathEnum.DprMasterdatas).Where(m => Encoding.Default.GetString(m.children[3].value.value.asString).StartsWith("MapWarp")).ToList();
            Dictionary<int, MapWarpAsset> links = new();

            for (int mIdx = monoBehaviours.Count - 1; mIdx >= 0; mIdx--)
            {
                MapWarpAsset mapWarpAsset = new();
                mapWarpAsset.mName = Encoding.Default.GetString(monoBehaviours[mIdx].children[3].value.value.asString);

                int areaID = GetAreaID(mapWarpAsset.mName.Replace("MapWarp_", ""));
                List<int> zoneIDs = FindAllIndexes(areaIDs, areaID);
                if (zoneIDs.Count == 0)
                    continue;
                for (int i = 0; i < zoneIDs.Count; i++)
                    links[zoneIDs[i]] = mapWarpAsset;
                mapWarpAsset.zoneIDs = new();
                mapWarpAsset.zoneIDs.AddRange(zoneIDs);

                //Parse MapWarp
                mapWarpAsset.mapWarps = new();
                AssetTypeValueField[] mapWarpFields = monoBehaviours[mIdx].children[4].children[0].children;
                for (int mapWarpIdx = 0; mapWarpIdx < mapWarpFields.Length; mapWarpIdx++)
                {
                    MapWarp mapWarp = new();
                    mapWarp.groupId = mapWarpFields[mapWarpIdx].children[1].value.value.asInt32;
                    mapWarp.destWarpZone = mapWarpFields[mapWarpIdx].children[3].value.value.asInt32;
                    mapWarp.destWarpIndex = mapWarpFields[mapWarpIdx].children[4].value.value.asInt32;
                    mapWarp.inputDir = mapWarpFields[mapWarpIdx].children[5].value.value.asInt32;
                    mapWarp.flagIndex = mapWarpFields[mapWarpIdx].children[6].value.value.asInt32;
                    mapWarp.scriptLabel = mapWarpFields[mapWarpIdx].children[7].value.value.asInt32;
                    mapWarp.exitLabel = mapWarpFields[mapWarpIdx].children[8].value.value.asInt32;
                    mapWarp.connectionName = Encoding.Default.GetString(mapWarpFields[mapWarpIdx].children[9].value.value.asString);
                    mapWarp.currentWarpIndex = mapWarpIdx;

                    mapWarpAsset.mapWarps.Add(mapWarp);
                }

                gameData.mapWarpAssets.Add(mapWarpAsset);
            }

            //Link MapWarps
            for (int mIdx = 0; mIdx < gameData.mapWarpAssets.Count; mIdx++)
                for (int mapWarpIdx = 0; mapWarpIdx < gameData.mapWarpAssets[mIdx].mapWarps.Count; mapWarpIdx++)
                    if (gameData.mapWarpAssets[mIdx].mapWarps[mapWarpIdx].destWarpZone != -1)
                        try
                        {
                            MapWarp mapWarp = gameData.mapWarpAssets[mIdx].mapWarps[mapWarpIdx];
                            mapWarp.destination = links[mapWarp.destWarpZone];
                        }
                        catch (KeyNotFoundException)
                        {
                            //Some areas don't have warp data because their warps are handled by evScripts for some reason...
                        }
        }

        /// <summary>
        ///  Finds the indexes of all instances of the number in the specified list.
        /// </summary>
        private static List<int> FindAllIndexes(List<int> list, int number)
        {
            List<int> dummy = new();
            dummy.AddRange(list);
            List<int> indexes = new();
            while (true)
            {
                int index = dummy.IndexOf(number);
                if (index == -1)
                    break;
                indexes.Add(index);
                dummy[index] = number + 1;
            }
            return indexes;
        }

        /// <summary>
        ///  Generates a list of AreaIDs ordered by ZoneIDs.
        /// </summary>
        private static List<int> GenerateAreaIDList()
        {
            return fileManager.GetMonoBehaviours(PathEnum.Gamesettings).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "MapInfo").children[4].children[0].children.Select(f => f.children[17].value.value.asInt32).ToList();
        }

        /// <summary>
        ///  Returns the areaID of an area name.
        /// </summary>
        public static int GetAreaID(string areaName)
        {
            try
            {
                return (int)(AreaName)Enum.Parse(typeof(AreaName), areaName);
            }
            catch (ArgumentException)
            {
                return -1;
            }
        }

        /// <summary>
        ///  Overwrites GlobalData with parsed EvScripts.
        /// </summary>
        private static void ParseEvScripts()
        {
            gameData.evScripts = new();
            List<AssetTypeValueField> monoBehaviours = fileManager.GetMonoBehaviours(PathEnum.EvScript).Where(m => m.children[4].GetName() == "Scripts").ToList();

            for (int mIdx = 0; mIdx < monoBehaviours.Count; mIdx++)
            {
                EvScript evScript = new();
                evScript.mName = Encoding.Default.GetString(monoBehaviours[mIdx].children[3].value.value.asString);

                //Parse Scripts
                evScript.scripts = new();
                AssetTypeValueField[] scriptFields = monoBehaviours[mIdx].children[4].children[0].children;
                for (int scriptIdx = 0; scriptIdx < scriptFields.Length; scriptIdx++)
                {
                    Script script = new();
                    script.evLabel = Encoding.Default.GetString(scriptFields[scriptIdx].children[0].value.value.asString);

                    //Parse Commands
                    script.commands = new();
                    AssetTypeValueField[] commandFields = scriptFields[scriptIdx].children[1].children[0].children;
                    for (int commandIdx = 0; commandIdx < commandFields.Length; commandIdx++)
                    {
                        Command command = new();

                        //Check for commands without data, because those exist for some reason.
                        if (commandFields[commandIdx].children[0].children[0].children.Length == 0)
                        {
                            command.cmdType = -1;
                            script.commands.Add(command);
                            continue;
                        }
                        command.cmdType = commandFields[commandIdx].children[0].children[0].children[0].children[1].value.value.asInt32;

                        //Parse Arguments
                        command.args = new();
                        AssetTypeValueField[] argumentFields = commandFields[commandIdx].children[0].children[0].children;
                        for (int argIdx = 1; argIdx < argumentFields.Length; argIdx++)
                        {
                            Argument arg = new();
                            arg.argType = argumentFields[argIdx].children[0].value.value.asInt32;
                            arg.data = argumentFields[argIdx].children[1].value.value.asInt32;
                            if (arg.argType == 1)
                                arg.data = ConvertToFloat((int)arg.data);

                            command.args.Add(arg);
                        }

                        script.commands.Add(command);
                    }

                    evScript.scripts.Add(script);
                }

                //Parse StrLists
                evScript.strList = new();
                AssetTypeValueField[] stringFields = monoBehaviours[mIdx].children[5].children[0].children;
                for (int stringIdx = 0; stringIdx < stringFields.Length; stringIdx++)
                    evScript.strList.Add(Encoding.Default.GetString(stringFields[stringIdx].value.value.asString));

                gameData.evScripts.Add(evScript);
            }
        }

        /// <summary>
        ///  Interprets bytes of an int32 as a float.
        /// </summary>
        private static float ConvertToFloat(int n)
        {
            return BitConverter.ToSingle(BitConverter.GetBytes(n));
        }

        /// <summary>
        ///  Overwrites GlobalData with a parsed AudioCollection.
        /// </summary>
        public static void ParseAudioCollection()
        {
            gameData.audioCollection = new();
            gameData.audioCollection.itemsByIDs = new();
            gameData.audioCollection.mrsBySourceIDs = new();
            byte[] buffer = fileManager.GetDelphisMainBuffer();
            gameData.audioCollection.delphisMainBuffer = buffer;

            long offset = 4;
            offset += BitConverter.ToUInt32(buffer, (int)offset);
            offset += 8;
            offset += BitConverter.ToUInt32(buffer, (int)offset);
            offset += 8;
            offset += BitConverter.ToUInt32(buffer, (int)offset);
            offset += 12;
            uint hircItemCount = BitConverter.ToUInt32(buffer, (int)offset);
            offset += 4;

            //Parse itemsByIDs
            for (int i = 0; i < hircItemCount; i++)
            {
                HircItem h = new();
                h.hircType = buffer[offset];
                offset++;
                long nextItemOffset = offset + 4 + BitConverter.ToUInt32(buffer, (int)offset);
                offset += 4;
                h.id = BitConverter.ToUInt32(buffer, (int)offset);
                h.idOffset = offset;
                offset += 4;

                switch (h.hircType)
                {
                    case 11: //Music Track
                        offset++;
                        offset += 4 + BitConverter.ToUInt32(buffer, (int)offset) * 14;

                        uint playlistItemCount = BitConverter.ToUInt32(buffer, (int)offset);
                        offset += 4;
                        for (int j = 0; j < playlistItemCount; j++)
                        {
                            if (BitConverter.ToUInt32(buffer, (int)offset + 8) == 0)
                            {
                                h.sourceID = BitConverter.ToUInt32(buffer, (int)offset + 4);
                                h.sourceDuration = BitConverter.ToDouble(buffer, (int)offset + 36);
                            }
                            offset += 44;
                        }
                        offset += 4;

                        uint clipAutomationCount = BitConverter.ToUInt32(buffer, (int)offset);
                        offset += 4;
                        for (int j = 0; j < clipAutomationCount; j++)
                        {
                            offset += 8;
                            offset += 4 + BitConverter.ToUInt32(buffer, (int)offset) * 12;
                        }
                        h.parentID = BitConverter.ToUInt32(buffer, (int)offset + 7);
                        break;
                    case 10: //Music Segment
                        h.parentID = BitConverter.ToUInt32(buffer, (int)offset + 8);
                        h.parentIDOffset = offset + 8;
                        break;
                    case 13: //Music Random Sequence
                        h.parentID = BitConverter.ToUInt32(buffer, (int)offset + 8);
                        h.parentIDOffset = offset + 8;
                        h.idReferenceOffsets = new();
                        break;
                    case 12:
                        h.childReferences = new();
                        offset += 13;
                        offset += 1 + buffer[offset] * 5;
                        offset += 1 + buffer[offset] * 5;
                        if (buffer[offset] > 0)
                            offset++;
                        offset += 14;

                        ushort rtpcCount = BitConverter.ToUInt16(buffer, (int)offset);
                        offset += 2;
                        for (int j = 0; j < rtpcCount; j++)
                        {
                            offset += 12;
                            offset += 2 + BitConverter.ToUInt16(buffer, (int)offset) * 12;
                        }

                        uint childCount1 = BitConverter.ToUInt32(buffer, (int)offset);
                        offset += 4;
                        for (int j = 0; j < childCount1; j++)
                        {
                            h.childReferences.Add((BitConverter.ToUInt32(buffer, (int)offset), offset));
                            offset += 4;
                        }
                        offset += 27;

                        uint ruleCount = BitConverter.ToUInt32(buffer, (int)offset);
                        offset += 4;
                        for (int j = 0; j < ruleCount; j++)
                        {
                            uint srcCount = BitConverter.ToUInt32(buffer, (int)offset);
                            offset += 4;
                            for (int k = 0; k < srcCount; k++)
                            {
                                uint id = BitConverter.ToUInt32(buffer, (int)offset);
                                if (id != 0 && id != 4294967295)
                                    h.childReferences.Add((id, offset));
                                offset += 4;
                            }

                            uint dstCount = BitConverter.ToUInt32(buffer, (int)offset);
                            offset += 4;
                            for (int k = 0; k < dstCount; k++)
                            {
                                uint id = BitConverter.ToUInt32(buffer, (int)offset);
                                if (id != 0 && id != 4294967295)
                                    h.childReferences.Add((id, offset));
                                offset += 4;
                            }
                            offset += 47;
                            offset += 1 + buffer[offset] * 30;
                        }
                        offset++;

                        //THERE WAS A DECISION TREE HERE. ARE YOU KIDDING ME.
                        //OH LORD THE TREE STRUCTURE HAS NO POINTERS. I SWEAR THE THINGS I GOTTA PUT UP WITH.
                        uint treeDepth = BitConverter.ToUInt32(buffer, (int)offset);
                        offset += 9 + treeDepth * 5;
                        AddChildReferencesTree(buffer, h.childReferences, ref offset, treeDepth);
                        break;
                }

                gameData.audioCollection.itemsByIDs[h.id] = h;
                offset = nextItemOffset;
            }

            Dictionary<uint, HircItem> itemsByIDs = gameData.audioCollection.itemsByIDs;
            foreach (HircItem item in gameData.audioCollection.itemsByIDs.Values)
            {
                if (item.hircType == 11)
                {
                    HircItem ms = itemsByIDs[item.parentID];
                    if (ms.hircType != 10 || ms.parentID == 0)
                        continue;
                    HircItem mrs = itemsByIDs[ms.parentID];
                    if (mrs.hircType != 13)
                        continue;

                    mrs.sourceID = item.sourceID;
                    mrs.sourceDuration = item.sourceDuration;
                    if (!gameData.audioCollection.mrsBySourceIDs.ContainsKey(item.sourceID))
                        gameData.audioCollection.mrsBySourceIDs[item.sourceID] = new();
                    gameData.audioCollection.mrsBySourceIDs[item.sourceID].Add(mrs);
                }

                if (item.hircType == 10 && item.parentID != 0)
                {
                    HircItem mrs = itemsByIDs[item.parentID];
                    if (mrs.hircType == 13)
                        mrs.idReferenceOffsets.Add(item.parentIDOffset);
                }
                /*
                if (item.hircType == 12)
                {
                    for (int i = 0; i < item.childReferences.Count; i++)
                    {
                        if (!itemsByIDs.ContainsKey(item.childReferences[i].Item1))
                            continue;
                        HircItem mrs = itemsByIDs[item.childReferences[i].Item1];
                        if (mrs.hircType == 13)
                            mrs.idReferenceOffsets.Add(item.childReferences[i].Item2);
                    }
                }
                */
            }
        }

        /// <summary>
        ///  Adds child references to list from tree at specified offset
        /// </summary>
        private static void AddChildReferencesTree(byte[] buffer, List<(uint, long)> childReferences, ref long offset, uint level)
        {
            ushort childCount = BitConverter.ToUInt16(buffer, (int)offset + 6);
            offset += 12;
            List<ushort> childCounts = new();
            for (int i = 0; i < childCount; i++)
                childCounts.Add(GetTreeChildCount(buffer, childReferences, ref offset, level - 1));
            for (int i = 0; i < childCounts.Count; i++)
                SearchChildren(buffer, childReferences, ref offset, level - 1, childCounts[i]);
        }

        /// <summary>
        ///  Reads a node, moving the offset.
        /// </summary>
        private static ushort GetTreeChildCount(byte[] buffer, List<(uint, long)> childReferences, ref long offset, uint level)
        {
            if (level == 0)
            {
                childReferences.Add((BitConverter.ToUInt32(buffer, (int)offset + 4), offset + 4));
                offset += 12;
                return 0;
            }

            ushort childCount = BitConverter.ToUInt16(buffer, (int)offset + 6);
            offset += 12;
            return childCount;
        }

        /// <summary>
        ///  Recursive traversal of tree.
        /// </summary>
        private static void SearchChildren(byte[] buffer, List<(uint, long)> childReferences, ref long offset, uint level, ushort childCount)
        {
            List<ushort> childCounts = new();
            for (int i = 0; i < childCount; i++)
                childCounts.Add(GetTreeChildCount(buffer, childReferences, ref offset, level - 1));
            for (int i = 0; i < childCounts.Count; i++)
                SearchChildren(buffer, childReferences, ref offset, level - 1, childCounts[i]);
        }

        /// <summary>
        ///  Overwrites GlobalData with a parsed GlobalMetadata.
        /// </summary>
        private static void ParseGlobalMetadata()
        {
            gameData.globalMetadata = new();
            byte[] buffer = fileManager.GetGlobalMetadataBuffer();
            gameData.globalMetadata.buffer = buffer;

            gameData.globalMetadata.stringOffset = BitConverter.ToUInt32(buffer, 0x18);

            gameData.globalMetadata.defaultValuePtrOffset = BitConverter.ToUInt32(buffer, 0x40);
            gameData.globalMetadata.defaultValuePtrSecSize = BitConverter.ToUInt32(buffer, 0x44);
            uint defaultValuePtrSize = 0xC;
            uint defaultValuePtrCount = gameData.globalMetadata.defaultValuePtrSecSize / defaultValuePtrSize;

            gameData.globalMetadata.defaultValueOffset = BitConverter.ToUInt32(buffer, 0x48);
            gameData.globalMetadata.defaultValueSecSize = BitConverter.ToUInt32(buffer, 0x4C);

            gameData.globalMetadata.fieldOffset = BitConverter.ToUInt32(buffer, 0x60);
            uint fieldSize = 0xC;

            gameData.globalMetadata.typeOffset = BitConverter.ToUInt32(buffer, 0xA0);
            uint typeSize = 0x5C;

            gameData.globalMetadata.imageOffset = BitConverter.ToUInt32(buffer, 0xA8);
            gameData.globalMetadata.imageSecSize = BitConverter.ToUInt32(buffer, 0xAC);
            uint imageSize = 0x28;
            uint imageCount = gameData.globalMetadata.imageSecSize / imageSize;

            gameData.globalMetadata.defaultValueDic = new();
            uint defaultValuePtrOffset = gameData.globalMetadata.defaultValuePtrOffset;
            for (int defaultValuePtrIdx = 0; defaultValuePtrIdx < defaultValuePtrCount; defaultValuePtrIdx++)
            {
                FieldDefaultValue fdv = new();
                fdv.offset = gameData.globalMetadata.defaultValueOffset + BitConverter.ToUInt32(buffer, (int)defaultValuePtrOffset + 8);
                long nextOffset = gameData.globalMetadata.defaultValueOffset + gameData.globalMetadata.defaultValueSecSize;
                if (defaultValuePtrIdx < defaultValuePtrCount - 1)
                    nextOffset = gameData.globalMetadata.defaultValueOffset + BitConverter.ToUInt32(buffer, (int)defaultValuePtrOffset + 20);
                fdv.length = (int)(nextOffset - fdv.offset);
                uint fieldIdx = BitConverter.ToUInt32(buffer, (int)defaultValuePtrOffset + 0);

                gameData.globalMetadata.defaultValueDic[fieldIdx] = fdv;
                defaultValuePtrOffset += defaultValuePtrSize;
            }

            gameData.globalMetadata.images = new();
            uint imageOffset = gameData.globalMetadata.imageOffset;
            for (int imageIdx = 0; imageIdx < imageCount; imageIdx++)
            {
                ImageDefinition id = new();
                uint imageNameIdx = BitConverter.ToUInt32(buffer, (int)imageOffset + 0);
                id.name = ReadNullTerminatedString(buffer, gameData.globalMetadata.stringOffset + imageNameIdx);
                id.typeStart = BitConverter.ToUInt32(buffer, (int)imageOffset + 8);
                id.typeCount = BitConverter.ToUInt32(buffer, (int)imageOffset + 12);

                id.types = new();
                uint typeOffset = gameData.globalMetadata.typeOffset + id.typeStart * typeSize;
                for (uint typeIdx = id.typeStart; typeIdx < id.typeStart + id.typeCount; typeIdx++)
                {
                    TypeDefinition td = new();
                    uint typeNameIdx = BitConverter.ToUInt32(buffer, (int)typeOffset + 0);
                    uint namespaceNameIdx = BitConverter.ToUInt32(buffer, (int)typeOffset + 4);
                    td.name = ReadNullTerminatedString(buffer, gameData.globalMetadata.stringOffset + namespaceNameIdx);
                    td.name += td.name.Length > 0 ? "." : "";
                    td.name += ReadNullTerminatedString(buffer, gameData.globalMetadata.stringOffset + typeNameIdx);
                    td.fieldStart = BitConverter.ToInt32(buffer, (int)typeOffset + 36);
                    td.fieldCount = BitConverter.ToUInt16(buffer, (int)typeOffset + 72);

                    td.fields = new();
                    uint fieldOffset = (uint)(gameData.globalMetadata.fieldOffset + td.fieldStart * fieldSize);
                    for (uint fieldIdx = (uint)td.fieldStart; fieldIdx < td.fieldStart + td.fieldCount; fieldIdx++)
                    {
                        FieldDefinition fd = new();
                        uint fieldNameIdx = BitConverter.ToUInt32(buffer, (int)fieldOffset + 0);
                        fd.name = ReadNullTerminatedString(buffer, gameData.globalMetadata.stringOffset + fieldNameIdx);
                        if (gameData.globalMetadata.defaultValueDic.TryGetValue(fieldIdx, out FieldDefaultValue fdv))
                            fd.defautValue = fdv;

                        td.fields.Add(fd);
                        fieldOffset += fieldSize;
                    }

                    id.types.Add(td);
                    typeOffset += typeSize;
                }

                gameData.globalMetadata.images.Add(id);
                imageOffset += imageSize;
            }

            string[] typeArrayNames = new string[]
            {
                "A3758C06C7FB42A47D220A11FBA532C6E8C62A77",
                "4B289ECFF3C0F0970CFBB23E3106E05803CB0010",
                "B9D3FD531E1A63CC167C4B98C0EC93F0249D9944",
                "347E5A9763B5C5AD3094AEC4B91A98983001E87D",
                "C089A0863406C198B5654996536BAC473C816234",
                "BCEEC8610D8506C3EDAC1C28CED532E5E2D8AD32",
                "A6F987666C679A4472D8CD64F600B501D2241486",
                "ACBC28AD33161A13959E63783CBFC94EB7FB2D90",
                "0459498E9764395D87F7F43BE89CCE657C669BFC",
                "C4215116A59F8DBC29910FA47BFBC6A82702816F",
                "AEDBD0B97A96E5BDD926058406DB246904438044",
                "DF2387E4B816070AE396698F2BD7359657EADE81",
                "64FFED43123BBC9517F387412947F1C700527EB4",
                "B5D988D1CB442CF60C021541BF2DC2A008819FD4",
                "D64329EA3A838F1B4186746A734070A5DFDA4983",
                "37DF3221C4030AC4E0EB9DD64616D020BB628CC1",
                "B2DD1970DDE852F750899708154090300541F4DE",
                "F774719D6A36449B152496136177E900605C9778"
            };

            TypeDefinition privateImplementationDetails = gameData.globalMetadata.images
                .Where(i => i.name == "Assembly-CSharp.dll").SelectMany(i => i.types)
                .First(t => t.name == "<PrivateImplementationDetails>");

            gameData.globalMetadata.typeMatchupOffsets = typeArrayNames
                .Select(s => privateImplementationDetails.fields.First(f => f.name == s).defautValue.offset).ToArray();
        }

        /// <summary>
        ///  Returns the null terminated UTF8 string starting at the specified offset.
        /// </summary>
        private static string ReadNullTerminatedString(byte[] buffer, long offset)
        {
            long endOffset = offset;
            while (buffer[endOffset] != 0)
                endOffset++;
            return Encoding.UTF8.GetString(buffer, (int)offset, (int)(endOffset - offset));
        }

        /// <summary>
        ///  Commits all modified files and prepares them for exporting.
        /// </summary>
        public static void CommitChanges()
        {
            if (gameData.IsModified(GameDataSet.DataField.EvScripts))
                CommitEvScripts();
            //if (gameData.IsModified(GameDataSet.DataField.MapWarpAssets))
            //    CommitMapWarpAssets();
            if (gameData.IsModified(GameDataSet.DataField.PickupItems))
                CommitPickupItems();
            if (gameData.IsModified(GameDataSet.DataField.ShopTables))
                CommitShopTables();
            if (gameData.IsModified(GameDataSet.DataField.Trainers))
                CommitTrainers();
            if (gameData.IsModified(GameDataSet.DataField.EncounterTableFiles))
                CommitEncounterTables();
            if (gameData.IsModified(GameDataSet.DataField.MessageFileSets))
                CommitMessageFileSets();
            //if (gameData.IsModified(GameDataSet.DataField.GrowthRates))
            //    CommitGrowthRates();
            if (gameData.IsModified(GameDataSet.DataField.UgEncounterFiles))
                CommitUgEncounters();
            if (gameData.IsModified(GameDataSet.DataField.PersonalEntries))
                CommitPokemon();
            if (gameData.IsModified(GameDataSet.DataField.Items))
                CommitItems();
            if (gameData.IsModified(GameDataSet.DataField.TMs))
                CommitTMs();
            if (gameData.IsModified(GameDataSet.DataField.Moves))
                CommitMoves();
            if (gameData.IsModified(GameDataSet.DataField.AudioCollection))
                CommitAudio();
            if (gameData.IsModified(GameDataSet.DataField.GlobalMetadata))
                CommitGlobalMetadata();
        }

        private static void CommitGlobalMetadata()
        {
            fileManager.CommitGlobalMetadata();
        }

        private static void CommitAudio()
        {
            fileManager.CommitAudio();
        }

        private static void CommitMoves()
        {
            AssetTypeValueField monoBehaviour = fileManager.GetMonoBehaviours(PathEnum.PersonalMasterdatas).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "WazaTable");
            AssetTypeValueField animationData = fileManager.GetMonoBehaviours(PathEnum.BattleMasterdatas).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "BattleDataTable");
            AssetTypeValueField textData = fileManager.GetMonoBehaviours(PathEnum.English).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "english_ss_wazaname");

            AssetTypeValueField[] moveFields = monoBehaviour.children[4].children[0].children;
            AssetTypeValueField[] animationFields = animationData.children[8].children[0].children;
            AssetTypeValueField[] textFields = textData.children[8].children[0].children;
            for (int moveID = 0; moveID < moveFields.Length; moveID++)
            {
                Move move = gameData.moves[moveID];
                moveFields[moveID].children[0].GetValue().Set(move.moveID);
                moveFields[moveID].children[1].GetValue().Set(move.isValid);
                moveFields[moveID].children[2].GetValue().Set(move.typingID);
                moveFields[moveID].children[3].GetValue().Set(move.category);
                moveFields[moveID].children[4].GetValue().Set(move.damageCategoryID);
                moveFields[moveID].children[5].GetValue().Set(move.power);
                moveFields[moveID].children[6].GetValue().Set(move.hitPer);
                moveFields[moveID].children[7].GetValue().Set(move.basePP);
                moveFields[moveID].children[8].GetValue().Set(move.priority);
                moveFields[moveID].children[9].GetValue().Set(move.hitCountMax);
                moveFields[moveID].children[10].GetValue().Set(move.hitCountMin);
                moveFields[moveID].children[11].GetValue().Set(move.sickID);
                moveFields[moveID].children[12].GetValue().Set(move.sickPer);
                moveFields[moveID].children[13].GetValue().Set(move.sickCont);
                moveFields[moveID].children[14].GetValue().Set(move.sickTurnMin);
                moveFields[moveID].children[15].GetValue().Set(move.sickTurnMax);
                moveFields[moveID].children[16].GetValue().Set(move.criticalRank);
                moveFields[moveID].children[17].GetValue().Set(move.shrinkPer);
                moveFields[moveID].children[18].GetValue().Set(move.aiSeqNo);
                moveFields[moveID].children[19].GetValue().Set(move.damageRecoverRatio);
                moveFields[moveID].children[20].GetValue().Set(move.hpRecoverRatio);
                moveFields[moveID].children[21].GetValue().Set(move.target);
                moveFields[moveID].children[22].GetValue().Set(move.rankEffType1);
                moveFields[moveID].children[23].GetValue().Set(move.rankEffType2);
                moveFields[moveID].children[24].GetValue().Set(move.rankEffType3);
                moveFields[moveID].children[25].GetValue().Set(move.rankEffValue1);
                moveFields[moveID].children[26].GetValue().Set(move.rankEffValue2);
                moveFields[moveID].children[27].GetValue().Set(move.rankEffValue3);
                moveFields[moveID].children[28].GetValue().Set(move.rankEffPer1);
                moveFields[moveID].children[29].GetValue().Set(move.rankEffPer2);
                moveFields[moveID].children[30].GetValue().Set(move.rankEffPer3);
                moveFields[moveID].children[31].GetValue().Set(move.flags);
                moveFields[moveID].children[32].GetValue().Set(move.contestWazaNo);
                animationFields[moveID].children[1].GetValue().Set(move.cmdSeqName);
                animationFields[moveID].children[2].GetValue().Set(move.cmdSeqNameLegend);
                animationFields[moveID].children[3].GetValue().Set(move.notShortenTurnType0);
                animationFields[moveID].children[4].GetValue().Set(move.notShortenTurnType1);
                animationFields[moveID].children[5].GetValue().Set(move.turnType1);
                animationFields[moveID].children[6].GetValue().Set(move.turnType2);
                animationFields[moveID].children[7].GetValue().Set(move.turnType3);
                animationFields[moveID].children[8].GetValue().Set(move.turnType4);
            }

            fileManager.WriteMonoBehaviour(PathEnum.PersonalMasterdatas, monoBehaviour);
            fileManager.WriteMonoBehaviour(PathEnum.BattleMasterdatas, animationData);
        }

        /// <summary>
        ///  Updates loaded bundle with TMs.
        /// </summary>
        private static void CommitTMs()
        {
            AssetTypeValueField monoBehaviour = fileManager.GetMonoBehaviours(PathEnum.PersonalMasterdatas).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "ItemTable");
            AssetTypeValueField textData = fileManager.GetMonoBehaviours(PathEnum.English).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "english_ss_itemname");

            AssetTypeValueField[] tmFields = monoBehaviour.children[5].children[0].children;
            AssetTypeValueField[] textFields = textData.children[8].children[0].children;
            for (int tmID = 0; tmID < tmFields.Length; tmID++)
            {
                TM tm = gameData.tms[tmID];
                tmFields[tmID].children[0].GetValue().Set(tm.itemID);
                tmFields[tmID].children[1].GetValue().Set(tm.machineNo);
                tmFields[tmID].children[2].GetValue().Set(tm.moveID);
            }

            fileManager.WriteMonoBehaviour(PathEnum.PersonalMasterdatas, monoBehaviour);
        }

        /// <summary>
        ///  Updates loaded bundle with Items.
        /// </summary>
        private static void CommitItems()
        {
            AssetTypeValueField monoBehaviour = fileManager.GetMonoBehaviours(PathEnum.PersonalMasterdatas).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "ItemTable");
            AssetTypeValueField textData = fileManager.GetMonoBehaviours(PathEnum.English).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "english_ss_itemname");

            AssetTypeValueField[] itemFields = monoBehaviour.children[4].children[0].children;
            AssetTypeValueField[] textFields = textData.children[8].children[0].children;
            for (int itemIdx = 0; itemIdx < itemFields.Length; itemIdx++)
            {
                Item item = gameData.items[itemIdx];
                itemFields[itemIdx].children[0].GetValue().Set(item.itemID);
                itemFields[itemIdx].children[1].GetValue().Set(item.type);
                itemFields[itemIdx].children[2].GetValue().Set(item.iconID);
                itemFields[itemIdx].children[3].GetValue().Set(item.price);
                itemFields[itemIdx].children[4].GetValue().Set(item.bpPrice);
                itemFields[itemIdx].children[7].GetValue().Set(item.nageAtc);
                itemFields[itemIdx].children[8].GetValue().Set(item.sizenAtc);
                itemFields[itemIdx].children[9].GetValue().Set(item.sizenType);
                itemFields[itemIdx].children[10].GetValue().Set(item.tuibamuEff);
                itemFields[itemIdx].children[11].GetValue().Set(item.sort);
                itemFields[itemIdx].children[12].GetValue().Set(item.group);
                itemFields[itemIdx].children[13].GetValue().Set(item.groupID);
                itemFields[itemIdx].children[14].GetValue().Set(item.fldPocket);
                itemFields[itemIdx].children[15].GetValue().Set(item.fieldFunc);
                itemFields[itemIdx].children[16].GetValue().Set(item.battleFunc);
                itemFields[itemIdx].children[18].GetValue().Set(item.criticalRanks);
                itemFields[itemIdx].children[19].GetValue().Set(item.atkStages);
                itemFields[itemIdx].children[20].GetValue().Set(item.defStages);
                itemFields[itemIdx].children[21].GetValue().Set(item.spdStages);
                itemFields[itemIdx].children[22].GetValue().Set(item.accStages);
                itemFields[itemIdx].children[23].GetValue().Set(item.spAtkStages);
                itemFields[itemIdx].children[24].GetValue().Set(item.spDefStages);
                itemFields[itemIdx].children[25].GetValue().Set(item.ppRestoreAmount);
                itemFields[itemIdx].children[26].GetValue().Set(item.hpEvIncrease);
                itemFields[itemIdx].children[27].GetValue().Set(item.atkEvIncrease);
                itemFields[itemIdx].children[28].GetValue().Set(item.defEvIncrease);
                itemFields[itemIdx].children[29].GetValue().Set(item.spdEvIncrease);
                itemFields[itemIdx].children[30].GetValue().Set(item.spAtkEvIncrease);
                itemFields[itemIdx].children[31].GetValue().Set(item.spDefEvIncrease);
                itemFields[itemIdx].children[32].GetValue().Set(item.friendshipIncrease1);
                itemFields[itemIdx].children[33].GetValue().Set(item.friendshipIncrease2);
                itemFields[itemIdx].children[34].GetValue().Set(item.friendshipIncrease3);
                itemFields[itemIdx].children[35].GetValue().Set(item.hpRestoreAmount);
                itemFields[itemIdx].children[36].GetValue().Set(item.flags0);
            }

            fileManager.WriteMonoBehaviour(PathEnum.PersonalMasterdatas, monoBehaviour);
        }

        /// <summary>
        ///  Updates loaded bundle with Pokemon.
        /// </summary>
        private static void CommitPokemon()
        {
            List<AssetTypeValueField> monoBehaviours = fileManager.GetMonoBehaviours(PathEnum.PersonalMasterdatas);
            AssetTypeValueField textData = fileManager.GetMonoBehaviours(PathEnum.CommonMsbt).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "english_ss_monsname");

            AssetTypeValueField wazaOboeTable = monoBehaviours.Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "WazaOboeTable");
            AssetTypeValueField tamagoWazaTable = monoBehaviours.Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "TamagoWazaTable");
            AssetTypeValueField evolveTable = monoBehaviours.Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "EvolveTable");
            AssetTypeValueField personalTable = monoBehaviours.Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "PersonalTable");
            monoBehaviours = new();
            monoBehaviours.Add(wazaOboeTable);
            monoBehaviours.Add(tamagoWazaTable);
            monoBehaviours.Add(evolveTable);
            monoBehaviours.Add(personalTable);

            AssetTypeValueField[] levelUpMoveFields = wazaOboeTable.children[4].children[0].children;
            AssetTypeValueField[] eggMoveFields = tamagoWazaTable.children[4].children[0].children;
            AssetTypeValueField[] evolveFields = evolveTable.children[4].children[0].children;
            AssetTypeValueField[] personalFields = personalTable.children[4].children[0].children;
            AssetTypeValueField[] textFields = textData.children[8].children[0].children;

            AssetTypeTemplateField levelUpMoveEntryTemplate = null;
            AssetTypeTemplateField eggMoveEntryTemplate = null;
            AssetTypeTemplateField evolveEntryTemplate = null;

            for (int personalID = 1; personalID < personalFields.Length; personalID++)
            {
                Pokemon pokemon = gameData.personalEntries[personalID];
                personalFields[personalID].children[0].GetValue().Set(pokemon.validFlag);
                personalFields[personalID].children[1].GetValue().Set(pokemon.personalID);
                personalFields[personalID].children[2].GetValue().Set(pokemon.dexID);
                personalFields[personalID].children[3].GetValue().Set(pokemon.formIndex);
                personalFields[personalID].children[4].GetValue().Set(pokemon.formMax);
                personalFields[personalID].children[5].GetValue().Set(pokemon.color);
                personalFields[personalID].children[6].GetValue().Set(pokemon.graNo);
                personalFields[personalID].children[7].GetValue().Set(pokemon.basicHp);
                personalFields[personalID].children[8].GetValue().Set(pokemon.basicAtk);
                personalFields[personalID].children[9].GetValue().Set(pokemon.basicDef);
                personalFields[personalID].children[10].GetValue().Set(pokemon.basicSpd);
                personalFields[personalID].children[11].GetValue().Set(pokemon.basicSpAtk);
                personalFields[personalID].children[12].GetValue().Set(pokemon.basicSpDef);
                personalFields[personalID].children[13].GetValue().Set(pokemon.typingID1);
                personalFields[personalID].children[14].GetValue().Set(pokemon.typingID2);
                personalFields[personalID].children[15].GetValue().Set(pokemon.getRate);
                personalFields[personalID].children[16].GetValue().Set(pokemon.rank);
                personalFields[personalID].children[17].GetValue().Set(pokemon.expValue);
                personalFields[personalID].children[18].GetValue().Set(pokemon.item1);
                personalFields[personalID].children[19].GetValue().Set(pokemon.item2);
                personalFields[personalID].children[20].GetValue().Set(pokemon.item3);
                personalFields[personalID].children[21].GetValue().Set(pokemon.sex);
                personalFields[personalID].children[22].GetValue().Set(pokemon.eggBirth);
                personalFields[personalID].children[23].GetValue().Set(pokemon.initialFriendship);
                personalFields[personalID].children[24].GetValue().Set(pokemon.eggGroup1);
                personalFields[personalID].children[25].GetValue().Set(pokemon.eggGroup2);
                personalFields[personalID].children[26].GetValue().Set(pokemon.grow);
                personalFields[personalID].children[27].GetValue().Set(pokemon.abilityID1);
                personalFields[personalID].children[28].GetValue().Set(pokemon.abilityID2);
                personalFields[personalID].children[29].GetValue().Set(pokemon.abilityID3);
                personalFields[personalID].children[30].GetValue().Set(pokemon.giveExp);
                personalFields[personalID].children[31].GetValue().Set(pokemon.height);
                personalFields[personalID].children[32].GetValue().Set(pokemon.weight);
                personalFields[personalID].children[33].GetValue().Set(pokemon.chihouZukanNo);
                personalFields[personalID].children[34].GetValue().Set(pokemon.machine1);
                personalFields[personalID].children[35].GetValue().Set(pokemon.machine2);
                personalFields[personalID].children[36].GetValue().Set(pokemon.machine3);
                personalFields[personalID].children[37].GetValue().Set(pokemon.machine4);
                personalFields[personalID].children[38].GetValue().Set(pokemon.hiddenMachine);
                personalFields[personalID].children[39].GetValue().Set(pokemon.eggMonsno);
                personalFields[personalID].children[40].GetValue().Set(pokemon.eggFormno);
                personalFields[personalID].children[41].GetValue().Set(pokemon.eggFormnoKawarazunoishi);
                personalFields[personalID].children[42].GetValue().Set(pokemon.eggFormInheritKawarazunoishi);

                //Write level up moves
                if (levelUpMoveFields[personalID].children[1].children[0].GetChildrenCount() > 0)
                    levelUpMoveEntryTemplate = levelUpMoveFields[personalID].children[1].children[0].children[0].GetTemplateField();
                List<AssetTypeValue> levelUpMoveEntries = new();
                for (int levelUpMoveIdx = 0; levelUpMoveIdx < pokemon.levelUpMoves.Count; levelUpMoveIdx++)
                {
                    LevelUpMove levelUpMove = pokemon.levelUpMoves[levelUpMoveIdx];
                    levelUpMoveEntries.Add(new AssetTypeValue(EnumValueTypes.UInt16, levelUpMove.level));
                    levelUpMoveEntries.Add(new AssetTypeValue(EnumValueTypes.UInt16, levelUpMove.moveID));
                }
                List<AssetTypeValueField> levelUpMoveAtvfs = new();
                for (int itemIdx = 0; itemIdx < levelUpMoveEntries.Count; itemIdx++)
                {
                    AssetTypeValue item = levelUpMoveEntries[itemIdx];
                    AssetTypeValueField atvf = new();
                    AssetTypeValueField[] children = new AssetTypeValueField[0];

                    atvf.Read(item, levelUpMoveEntryTemplate, children);
                    levelUpMoveAtvfs.Add(atvf);
                }
                levelUpMoveFields[personalID].children[1].children[0].SetChildrenList(levelUpMoveAtvfs.ToArray());

                //Write egg moves
                if (eggMoveFields[personalID].children[2].children[0].GetChildrenCount() > 0)
                    eggMoveEntryTemplate = eggMoveFields[personalID].children[2].children[0].children[0].GetTemplateField();
                List<AssetTypeValueField> eggMoveAtvfs = new();
                for (int itemIdx = 0; itemIdx < pokemon.eggMoves.Count; itemIdx++)
                {
                    AssetTypeValue item = new AssetTypeValue(EnumValueTypes.UInt16, pokemon.eggMoves[itemIdx]);
                    AssetTypeValueField atvf = new();
                    AssetTypeValueField[] children = new AssetTypeValueField[0];

                    atvf.Read(item, levelUpMoveEntryTemplate, children);
                    eggMoveAtvfs.Add(atvf);
                }
                eggMoveFields[personalID].children[2].children[0].SetChildrenList(eggMoveAtvfs.ToArray());

                //Write evolutions
                if (evolveFields[personalID].children[1].children[0].GetChildrenCount() > 0)
                    evolveEntryTemplate = evolveFields[personalID].children[1].children[0].children[0].GetTemplateField();
                List<AssetTypeValue> evolveEntries = new();
                for (int evolveIdx = 0; evolveIdx < pokemon.evolutionPaths.Count; evolveIdx++)
                {
                    EvolutionPath evolutionPath = pokemon.evolutionPaths[evolveIdx];
                    evolveEntries.Add(new AssetTypeValue(EnumValueTypes.UInt16, evolutionPath.method));
                    evolveEntries.Add(new AssetTypeValue(EnumValueTypes.UInt16, evolutionPath.parameter));
                    evolveEntries.Add(new AssetTypeValue(EnumValueTypes.UInt16, evolutionPath.destDexID));
                    evolveEntries.Add(new AssetTypeValue(EnumValueTypes.UInt16, evolutionPath.destFormID));
                    evolveEntries.Add(new AssetTypeValue(EnumValueTypes.UInt16, evolutionPath.level));
                }
                List<AssetTypeValueField> evolveAtvfs = new();
                for (int itemIdx = 0; itemIdx < evolveEntries.Count; itemIdx++)
                {
                    AssetTypeValue item = evolveEntries[itemIdx];
                    AssetTypeValueField atvf = new();
                    AssetTypeValueField[] children = new AssetTypeValueField[0];

                    atvf.Read(item, evolveEntryTemplate, children);
                    evolveAtvfs.Add(atvf);
                }
                evolveFields[personalID].children[1].children[0].SetChildrenList(evolveAtvfs.ToArray());
            }

            fileManager.WriteMonoBehaviours(PathEnum.PersonalMasterdatas, monoBehaviours.ToArray());
        }

        /// <summary>
        ///  Updates loaded bundle with UgEncounters.
        /// </summary>
        private static void CommitUgEncounters()
        {
            List<AssetTypeValueField> monoBehaviours = fileManager.GetMonoBehaviours(PathEnum.Ugdata);

            List<AssetTypeValueField> ugEncounterMonobehaviours = monoBehaviours.Where(m => Encoding.Default.GetString(m.children[3].value.value.asString).StartsWith("UgEncount_")).ToList();
            for (int ugEncounterFileIdx = 0; ugEncounterFileIdx < ugEncounterMonobehaviours.Count; ugEncounterFileIdx++)
            {
                UgEncounterFile ugEncounterFile = gameData.ugEncounterFiles[ugEncounterFileIdx];
                ugEncounterMonobehaviours[ugEncounterFileIdx].children[3].GetValue().Set(ugEncounterFile.mName);

                List<List<AssetTypeValue>> ugEncounters = new();
                for (int ugEncounterIdx = 0; ugEncounterIdx < ugEncounterFile.ugEncounter.Count; ugEncounterIdx++)
                {
                    UgEncounter ugEncounter = ugEncounterFile.ugEncounter[ugEncounterIdx];
                    List<AssetTypeValue> atvs = new();
                    atvs.Add(new AssetTypeValue(EnumValueTypes.Int32, ugEncounter.dexID));
                    atvs.Add(new AssetTypeValue(EnumValueTypes.Int32, ugEncounter.version));
                    atvs.Add(new AssetTypeValue(EnumValueTypes.Int32, ugEncounter.zukanFlag));
                    ugEncounters.Add(atvs);
                }
                AssetTypeValueField ugEncounterReference = ugEncounterMonobehaviours[ugEncounterFileIdx].children[4].children[0].children[0];
                ugEncounterMonobehaviours[ugEncounterFileIdx].children[4].children[0].SetChildrenList(GetATVFs(ugEncounterReference, ugEncounters));
            }

            AssetTypeValueField ugEncounterLevelMonobehaviour = monoBehaviours.Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "UgEncountLevel");
            AssetTypeValueField[] ugEncounterLevelFields = ugEncounterLevelMonobehaviour.children[4].children[0].children;
            for (int ugEncouterLevelIdx = 0; ugEncouterLevelIdx < ugEncounterLevelFields.Length; ugEncouterLevelIdx++)
            {
                UgEncounterLevelSet ugLevels = gameData.ugEncounterLevelSets[ugEncouterLevelIdx];
                ugEncounterLevelFields[ugEncouterLevelIdx].children[0].GetValue().Set(ugLevels.minLv);
                ugEncounterLevelFields[ugEncouterLevelIdx].children[1].GetValue().Set(ugLevels.maxLv);
            }
            ugEncounterMonobehaviours.Add(ugEncounterLevelMonobehaviour);

            fileManager.WriteMonoBehaviours(PathEnum.Ugdata, ugEncounterMonobehaviours.ToArray());
        }

        /// <summary>
        ///  Updates loaded bundle with EncounterTables.
        /// </summary>
        private static void CommitMessageFileSets()
        {
            List<MessageFile> messageFiles = gameData.messageFileSets.SelectMany(mfs => mfs.messageFiles).ToList();

            List<AssetTypeValueField> monoBehaviours = fileManager.GetMonoBehaviours(PathEnum.CommonMsbt);
            CommitMessageFiles(monoBehaviours, messageFiles);
            fileManager.WriteMonoBehaviours(PathEnum.CommonMsbt, monoBehaviours.ToArray());

            monoBehaviours = fileManager.GetMonoBehaviours(PathEnum.English);
            CommitMessageFiles(monoBehaviours, messageFiles);
            fileManager.WriteMonoBehaviours(PathEnum.English, monoBehaviours.ToArray());

            monoBehaviours = fileManager.GetMonoBehaviours(PathEnum.French);
            CommitMessageFiles(monoBehaviours, messageFiles);
            fileManager.WriteMonoBehaviours(PathEnum.French, monoBehaviours.ToArray());

            monoBehaviours = fileManager.GetMonoBehaviours(PathEnum.German);
            CommitMessageFiles(monoBehaviours, messageFiles);
            fileManager.WriteMonoBehaviours(PathEnum.German, monoBehaviours.ToArray());

            monoBehaviours = fileManager.GetMonoBehaviours(PathEnum.Italian);
            CommitMessageFiles(monoBehaviours, messageFiles);
            fileManager.WriteMonoBehaviours(PathEnum.Italian, monoBehaviours.ToArray());

            monoBehaviours = fileManager.GetMonoBehaviours(PathEnum.Jpn);
            CommitMessageFiles(monoBehaviours, messageFiles);
            fileManager.WriteMonoBehaviours(PathEnum.Jpn, monoBehaviours.ToArray());

            monoBehaviours = fileManager.GetMonoBehaviours(PathEnum.JpnKanji);
            CommitMessageFiles(monoBehaviours, messageFiles);
            fileManager.WriteMonoBehaviours(PathEnum.JpnKanji, monoBehaviours.ToArray());

            monoBehaviours = fileManager.GetMonoBehaviours(PathEnum.Korean);
            CommitMessageFiles(monoBehaviours, messageFiles);
            fileManager.WriteMonoBehaviours(PathEnum.Korean, monoBehaviours.ToArray());

            monoBehaviours = fileManager.GetMonoBehaviours(PathEnum.SimpChinese);
            CommitMessageFiles(monoBehaviours, messageFiles);
            fileManager.WriteMonoBehaviours(PathEnum.SimpChinese, monoBehaviours.ToArray());

            monoBehaviours = fileManager.GetMonoBehaviours(PathEnum.Spanish);
            CommitMessageFiles(monoBehaviours, messageFiles);
            fileManager.WriteMonoBehaviours(PathEnum.Spanish, monoBehaviours.ToArray());

            monoBehaviours = fileManager.GetMonoBehaviours(PathEnum.TradChinese);
            CommitMessageFiles(monoBehaviours, messageFiles);
            fileManager.WriteMonoBehaviours(PathEnum.TradChinese, monoBehaviours.ToArray());
        }

        /// <summary>
        ///  Writes all data into monobehaviors from a superset of MessageFiles.
        /// </summary>
        private static void CommitMessageFiles(List<AssetTypeValueField> monoBehaviours, List<MessageFile> messageFiles)
        {
            for (int mIdx = 0; mIdx < monoBehaviours.Count; mIdx++)
            {
                MessageFile messageFile = messageFiles.Find(mf => mf.mName == monoBehaviours[mIdx].children[3].GetValue().AsString());
                monoBehaviours[mIdx].children[5].GetValue().Set(messageFile.langID);
                monoBehaviours[mIdx].children[7].GetValue().Set(messageFile.isKanji);

                //Write LabelData
                AssetTypeValueField[] labelDataFields = monoBehaviours[mIdx].children[8].children[0].children;
                for (int labelDataIdx = 0; labelDataIdx < labelDataFields.Length; labelDataIdx++)
                {
                    LabelData labelData = messageFile.labelDatas[labelDataIdx];
                    labelDataFields[labelDataIdx].children[0].GetValue().Set(labelData.labelIndex);
                    labelDataFields[labelDataIdx].children[1].GetValue().Set(labelData.arrayIndex);
                    labelDataFields[labelDataIdx].children[2].GetValue().Set(labelData.labelName);
                    labelDataFields[labelDataIdx].children[3].children[0].GetValue().Set(labelData.styleIndex);
                    labelDataFields[labelDataIdx].children[3].children[1].GetValue().Set(labelData.colorIndex);
                    labelDataFields[labelDataIdx].children[3].children[2].GetValue().Set(labelData.fontSize);
                    labelDataFields[labelDataIdx].children[3].children[3].GetValue().Set(labelData.maxWidth);
                    labelDataFields[labelDataIdx].children[3].children[4].GetValue().Set(labelData.controlID);

                    //Write WordData
                    List<List<AssetTypeValue>> wordDatas = new();
                    for (int wordDataIdx = 0; wordDataIdx < labelData.wordDatas.Count; wordDataIdx++)
                    {
                        WordData wordData = labelData.wordDatas[wordDataIdx];
                        List<AssetTypeValue> atvs = new();
                        atvs.Add(new AssetTypeValue(EnumValueTypes.Int32, wordData.patternID));
                        atvs.Add(new AssetTypeValue(EnumValueTypes.Int32, wordData.eventID));
                        atvs.Add(new AssetTypeValue(EnumValueTypes.Int32, wordData.tagIndex));
                        atvs.Add(new AssetTypeValue(EnumValueTypes.Float, wordData.tagValue));
                        atvs.Add(new AssetTypeValue(EnumValueTypes.String, wordData.str));
                        atvs.Add(new AssetTypeValue(EnumValueTypes.Float, wordData.strWidth));
                        wordDatas.Add(atvs);
                    }
                    AssetTypeValueField wordDataReference = monoBehaviours[0].children[8].children[0].children[0].children[6].children[0].children[0];
                    labelDataFields[labelDataIdx].children[6].children[0].SetChildrenList(GetATVFs(wordDataReference, wordDatas));
                }
            }
        }

        /// <summary>
        ///  Updates loaded bundle with EncounterTables.
        /// </summary>
        private static void CommitEncounterTables()
        {
            List<AssetTypeValueField> monoBehaviours = fileManager.GetMonoBehaviours(PathEnum.Gamesettings);
            AssetTypeValueField[] encounterTableMonoBehaviours = new AssetTypeValueField[2];
            encounterTableMonoBehaviours[0] = monoBehaviours.Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "FieldEncountTable_d");
            encounterTableMonoBehaviours[1] = monoBehaviours.Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "FieldEncountTable_p");
            for (int encounterTableFileIdx = 0; encounterTableFileIdx < encounterTableMonoBehaviours.Length; encounterTableFileIdx++)
            {
                EncounterTableFile encounterTableFile = gameData.encounterTableFiles[encounterTableFileIdx];
                encounterTableMonoBehaviours[encounterTableFileIdx].children[3].GetValue().Set(encounterTableFile.mName);

                //Write wild encounter tables
                AssetTypeValueField[] encounterTableFields = encounterTableMonoBehaviours[encounterTableFileIdx].children[4].children[0].children;
                for (int encounterTableIdx = 0; encounterTableIdx < encounterTableFields.Length; encounterTableIdx++)
                {
                    EncounterTable encounterTable = encounterTableFile.encounterTables[encounterTableIdx];
                    encounterTableFields[encounterTableIdx].children[0].GetValue().Set(encounterTable.zoneID);
                    encounterTableFields[encounterTableIdx].children[1].GetValue().Set(encounterTable.encRateGround);
                    encounterTableFields[encounterTableIdx].children[7].children[0].children[0].GetValue().Set(encounterTable.formProb);
                    encounterTableFields[encounterTableIdx].children[9].children[0].children[1].GetValue().Set(encounterTable.unownTable);
                    encounterTableFields[encounterTableIdx].children[15].GetValue().Set(encounterTable.encRateWater);
                    encounterTableFields[encounterTableIdx].children[17].GetValue().Set(encounterTable.encRateOldRod);
                    encounterTableFields[encounterTableIdx].children[19].GetValue().Set(encounterTable.encRateGoodRod);
                    encounterTableFields[encounterTableIdx].children[21].GetValue().Set(encounterTable.encRateSuperRod);

                    //Write ground tables
                    SetEncounters(encounterTableFields[encounterTableIdx].children[2].children[0], encounterTable.groundMons);

                    //Write morning tables
                    SetEncounters(encounterTableFields[encounterTableIdx].children[3].children[0], encounterTable.tairyo);

                    //Write day tables
                    SetEncounters(encounterTableFields[encounterTableIdx].children[4].children[0], encounterTable.day);

                    //Write night tables
                    SetEncounters(encounterTableFields[encounterTableIdx].children[5].children[0], encounterTable.night);

                    //Write pokefinder tables
                    SetEncounters(encounterTableFields[encounterTableIdx].children[6].children[0], encounterTable.swayGrass);

                    //Write surfing tables
                    SetEncounters(encounterTableFields[encounterTableIdx].children[16].children[0], encounterTable.waterMons);

                    //Write old rod tables
                    SetEncounters(encounterTableFields[encounterTableIdx].children[18].children[0], encounterTable.oldRodMons);

                    //Write good rod tables
                    SetEncounters(encounterTableFields[encounterTableIdx].children[20].children[0], encounterTable.goodRodMons);

                    //Write super rod tables
                    SetEncounters(encounterTableFields[encounterTableIdx].children[22].children[0], encounterTable.superRodMons);
                }

                //Write trophy garden table
                AssetTypeValueField[] trophyGardenMonFields = encounterTableMonoBehaviours[encounterTableFileIdx].children[5].children[0].children;
                for (int trophyGardenMonIdx = 0; trophyGardenMonIdx < trophyGardenMonFields.Length; trophyGardenMonIdx++)
                    trophyGardenMonFields[trophyGardenMonIdx].children[0].GetValue().Set(encounterTableFile.trophyGardenMons[trophyGardenMonIdx]);

                //Write honey tree tables
                AssetTypeValueField[] honeyTreeEncounterFields = encounterTableMonoBehaviours[encounterTableFileIdx].children[6].children[0].children;
                for (int honeyTreeEncounterIdx = 0; honeyTreeEncounterIdx < honeyTreeEncounterFields.Length; honeyTreeEncounterIdx++)
                {
                    HoneyTreeEncounter honeyTreeEncounter = encounterTableFile.honeyTreeEnconters[honeyTreeEncounterIdx];
                    honeyTreeEncounterFields[honeyTreeEncounterIdx].children[0].GetValue().Set(honeyTreeEncounter.rate);
                    honeyTreeEncounterFields[honeyTreeEncounterIdx].children[1].GetValue().Set(honeyTreeEncounter.normalDexID);
                    honeyTreeEncounterFields[honeyTreeEncounterIdx].children[2].GetValue().Set(honeyTreeEncounter.rareDexID);
                    honeyTreeEncounterFields[honeyTreeEncounterIdx].children[3].GetValue().Set(honeyTreeEncounter.superRareDexID);
                }

                //Write safari table
                AssetTypeValueField[] safariMonFields = encounterTableMonoBehaviours[encounterTableFileIdx].children[8].children[0].children;
                for (int safariMonIdx = 0; safariMonIdx < safariMonFields.Length; safariMonIdx++)
                    safariMonFields[safariMonIdx].children[0].GetValue().Set(encounterTableFile.safariMons[safariMonIdx]);
            }

            fileManager.WriteMonoBehaviours(PathEnum.Gamesettings, encounterTableMonoBehaviours);
            return;
        }

        /// <summary>
        ///  Updates loaded bundle with Trainers.
        /// </summary>
        private static void CommitTrainers()
        {
            AssetTypeValueField monoBehaviour = fileManager.GetMonoBehaviours(PathEnum.DprMasterdatas).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "TrainerTable");

            AssetTypeValueField[] trainerFields = monoBehaviour.children[5].children[0].children;
            for (int trainerIdx = 0; trainerIdx < gameData.trainers.Count; trainerIdx++)
            {
                Trainer trainer = gameData.trainers[trainerIdx];
                trainerFields[trainerIdx].children[0].GetValue().Set(trainer.trainerTypeID);
                trainerFields[trainerIdx].children[1].GetValue().Set(trainer.colorID);
                trainerFields[trainerIdx].children[2].GetValue().Set(trainer.fightType);
                trainerFields[trainerIdx].children[3].GetValue().Set(trainer.arenaID);
                trainerFields[trainerIdx].children[4].GetValue().Set(trainer.effectID);
                trainerFields[trainerIdx].children[5].GetValue().Set(trainer.gold);
                trainerFields[trainerIdx].children[6].GetValue().Set(trainer.useItem1);
                trainerFields[trainerIdx].children[7].GetValue().Set(trainer.useItem2);
                trainerFields[trainerIdx].children[8].GetValue().Set(trainer.useItem3);
                trainerFields[trainerIdx].children[9].GetValue().Set(trainer.useItem4);
                trainerFields[trainerIdx].children[10].GetValue().Set(trainer.hpRecoverFlag);
                trainerFields[trainerIdx].children[11].GetValue().Set(trainer.giftItem);
                trainerFields[trainerIdx].children[12].GetValue().Set(trainer.nameLabel);
                trainerFields[trainerIdx].children[19].GetValue().Set(trainer.aiBit);

                //Write trainer pokemon
                List<List<AssetTypeValue>> tranierPokemons = new();
                List<AssetTypeValue> atvs = new();
                atvs.Add(monoBehaviour.children[6].children[0].children[trainerIdx].children[0].GetValue());
                for (int trainerPokemonIdx = 0; trainerPokemonIdx < (int)GetBoundaries(AbsoluteBoundary.TrainerPokemonCount)[2]; trainerPokemonIdx++)
                {
                    TrainerPokemon trainerPokemon = new();
                    if (gameData.trainers[trainerIdx].trainerPokemon.Count > trainerPokemonIdx)
                        trainerPokemon = gameData.trainers[trainerIdx].trainerPokemon[trainerPokemonIdx];
                    atvs.Add(new AssetTypeValue(EnumValueTypes.UInt16, trainerPokemon.dexID));
                    atvs.Add(new AssetTypeValue(EnumValueTypes.UInt16, trainerPokemon.formID));
                    atvs.Add(new AssetTypeValue(EnumValueTypes.UInt8, trainerPokemon.isRare));
                    atvs.Add(new AssetTypeValue(EnumValueTypes.UInt8, trainerPokemon.level));
                    atvs.Add(new AssetTypeValue(EnumValueTypes.UInt8, trainerPokemon.sex));
                    atvs.Add(new AssetTypeValue(EnumValueTypes.UInt8, trainerPokemon.natureID));
                    atvs.Add(new AssetTypeValue(EnumValueTypes.UInt16, trainerPokemon.abilityID));
                    atvs.Add(new AssetTypeValue(EnumValueTypes.UInt16, trainerPokemon.moveID1));
                    atvs.Add(new AssetTypeValue(EnumValueTypes.UInt16, trainerPokemon.moveID2));
                    atvs.Add(new AssetTypeValue(EnumValueTypes.UInt16, trainerPokemon.moveID3));
                    atvs.Add(new AssetTypeValue(EnumValueTypes.UInt16, trainerPokemon.moveID4));
                    atvs.Add(new AssetTypeValue(EnumValueTypes.UInt16, trainerPokemon.itemID));
                    atvs.Add(new AssetTypeValue(EnumValueTypes.UInt8, trainerPokemon.ballID));
                    atvs.Add(new AssetTypeValue(EnumValueTypes.Int32, trainerPokemon.seal));
                    atvs.Add(new AssetTypeValue(EnumValueTypes.UInt8, trainerPokemon.hpIV));
                    atvs.Add(new AssetTypeValue(EnumValueTypes.UInt8, trainerPokemon.atkIV));
                    atvs.Add(new AssetTypeValue(EnumValueTypes.UInt8, trainerPokemon.defIV));
                    atvs.Add(new AssetTypeValue(EnumValueTypes.UInt8, trainerPokemon.spdIV));
                    atvs.Add(new AssetTypeValue(EnumValueTypes.UInt8, trainerPokemon.spAtkIV));
                    atvs.Add(new AssetTypeValue(EnumValueTypes.UInt8, trainerPokemon.spDefIV));
                    atvs.Add(new AssetTypeValue(EnumValueTypes.UInt8, trainerPokemon.hpEV));
                    atvs.Add(new AssetTypeValue(EnumValueTypes.UInt8, trainerPokemon.atkEV));
                    atvs.Add(new AssetTypeValue(EnumValueTypes.UInt8, trainerPokemon.defEV));
                    atvs.Add(new AssetTypeValue(EnumValueTypes.UInt8, trainerPokemon.spdEV));
                    atvs.Add(new AssetTypeValue(EnumValueTypes.UInt8, trainerPokemon.spAtkEV));
                    atvs.Add(new AssetTypeValue(EnumValueTypes.UInt8, trainerPokemon.spDefEV));
                }
                tranierPokemons.Add(atvs);
                AssetTypeValueField trainerPokemonsReference = monoBehaviour.children[6].children[0].children[trainerIdx];
                monoBehaviour.children[6].children[0].children[trainerIdx] = GetATVFs(trainerPokemonsReference, tranierPokemons)[0];
            }

            fileManager.WriteMonoBehaviour(PathEnum.DprMasterdatas, monoBehaviour);
        }

        /// <summary>
        ///  Updates loaded bundle with ShopTables.
        /// </summary>
        private static void CommitShopTables()
        {
            AssetTypeValueField monoBehaviour = fileManager.GetMonoBehaviours(PathEnum.DprMasterdatas).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "ShopTable");

            List<List<AssetTypeValue>> martItems = new();
            for (int martItemIdx = 0; martItemIdx < gameData.shopTables.martItems.Count; martItemIdx++)
            {
                MartItem martItem = gameData.shopTables.martItems[martItemIdx];
                List<AssetTypeValue> atvs = new();
                atvs.Add(new AssetTypeValue(EnumValueTypes.UInt16, martItem.itemID));
                atvs.Add(new AssetTypeValue(EnumValueTypes.Int32, martItem.badgeNum));
                atvs.Add(new AssetTypeValue(EnumValueTypes.Int32, martItem.zoneID));
                martItems.Add(atvs);
            }
            AssetTypeValueField martItemReference = monoBehaviour.children[4].children[0].children[0];
            monoBehaviour.children[4].children[0].SetChildrenList(GetATVFs(martItemReference, martItems));

            List<List<AssetTypeValue>> fixedShopItems = new();
            for (int fixedShopItemIdx = 0; fixedShopItemIdx < gameData.shopTables.fixedShopItems.Count; fixedShopItemIdx++)
            {
                FixedShopItem fixedShopItem = gameData.shopTables.fixedShopItems[fixedShopItemIdx];
                List<AssetTypeValue> atvs = new();
                atvs.Add(new AssetTypeValue(EnumValueTypes.UInt16, fixedShopItem.itemID));
                atvs.Add(new AssetTypeValue(EnumValueTypes.Int32, fixedShopItem.shopID));
                fixedShopItems.Add(atvs);
            }
            AssetTypeValueField fixedShopItemReference = monoBehaviour.children[5].children[0].children[0];
            monoBehaviour.children[5].children[0].SetChildrenList(GetATVFs(fixedShopItemReference, fixedShopItems));

            List<List<AssetTypeValue>> bpShopItems = new();
            for (int bpShopItemIdx = 0; bpShopItemIdx < gameData.shopTables.bpShopItems.Count; bpShopItemIdx++)
            {
                BpShopItem bpShopItem = gameData.shopTables.bpShopItems[bpShopItemIdx];
                List<AssetTypeValue> atvs = new();
                atvs.Add(new AssetTypeValue(EnumValueTypes.UInt16, bpShopItem.itemID));
                atvs.Add(new AssetTypeValue(EnumValueTypes.Int32, bpShopItem.npcID));
                bpShopItems.Add(atvs);
            }
            AssetTypeValueField bpShopItemReference = monoBehaviour.children[9].children[0].children[0];
            monoBehaviour.children[9].children[0].SetChildrenList(GetATVFs(bpShopItemReference, bpShopItems));

            fileManager.WriteMonoBehaviour(PathEnum.DprMasterdatas, monoBehaviour);
        }

        /// <summary>
        ///  Updates loaded bundle with PickupItems.
        /// </summary>
        private static void CommitPickupItems()
        {
            AssetTypeValueField monoBehaviour = fileManager.GetMonoBehaviours(PathEnum.DprMasterdatas).Find(m => Encoding.Default.GetString(m.children[3].value.value.asString) == "MonohiroiTable");

            AssetTypeValueField[] pickupItemFields = monoBehaviour.children[4].children[0].children;
            for (int pickupItemIdx = 0; pickupItemIdx < pickupItemFields.Length; pickupItemIdx++)
            {
                PickupItem pickupItem = gameData.pickupItems[pickupItemIdx];
                pickupItemFields[pickupItemIdx].children[0].GetValue().Set(pickupItem.itemID);

                //Write item probabilities
                for (int ratio = 0; ratio < pickupItemFields[pickupItemIdx].children[1].children[0].childrenCount; ratio++)
                    pickupItemFields[pickupItemIdx].children[1].children[0].children[ratio].GetValue().Set(pickupItem.ratios[ratio]);
            }

            fileManager.WriteMonoBehaviour(PathEnum.DprMasterdatas, monoBehaviour);
        }

        /// <summary>
        ///  Updates loaded bundle with EvScripts.
        /// </summary>
        private static void CommitEvScripts()
        {
            List<AssetTypeValueField> monoBehaviours = fileManager.GetMonoBehaviours(PathEnum.EvScript).Where(m => m.children[4].GetName() == "Scripts").ToList();

            for (int mIdx = 0; mIdx < monoBehaviours.Count; mIdx++)
            {
                EvScript evScript = gameData.evScripts[mIdx];
                monoBehaviours[mIdx].children[3].GetValue().Set(evScript.mName);

                //Write Scripts
                AssetTypeValueField[] scriptFields = monoBehaviours[mIdx].children[4].children[0].children;
                for (int scriptIdx = 0; scriptIdx < scriptFields.Length; scriptIdx++)
                {
                    Script script = evScript.scripts[scriptIdx];
                    scriptFields[scriptIdx].children[0].GetValue().Set(script.evLabel);

                    //Write Commands
                    AssetTypeValueField[] commandFields = scriptFields[scriptIdx].children[1].children[0].children;
                    for (int commandIdx = 0; commandIdx < commandFields.Length; commandIdx++)
                    {
                        Command command = script.commands[commandIdx];

                        //Check for commands without data, because those exist for some reason.
                        if (commandFields[commandIdx].children[0].children[0].children.Length == 0)
                            continue;

                        commandFields[commandIdx].children[0].children[0].children[0].children[1].GetValue().Set(command.cmdType);

                        //Write Arguments
                        AssetTypeValueField[] argumentFields = commandFields[commandIdx].children[0].children[0].children;
                        for (int argIdx = 1; argIdx < argumentFields.Length; argIdx++)
                        {
                            Argument arg = command.args[argIdx - 1];
                            argumentFields[argIdx].children[0].GetValue().Set(arg.argType);
                            argumentFields[argIdx].children[1].GetValue().Set(arg.data);
                            if (arg.argType == 1)
                                argumentFields[argIdx].children[1].GetValue().Set(ConvertToInt((int)arg.data));
                        }
                    }
                }

                //Write StrLists
                AssetTypeValueField[] stringFields = monoBehaviours[mIdx].children[5].children[0].children;
                for (int stringIdx = 0; stringIdx < stringFields.Length; stringIdx++)
                    stringFields[stringIdx].GetValue().Set(evScript.strList[stringIdx]);
            }

            fileManager.WriteMonoBehaviours(PathEnum.EvScript, monoBehaviours.ToArray());
        }

        /// <summary>
        ///  Converts a List of Encounters into a AssetTypeValueField array.
        /// </summary>
        private static void SetEncounters(AssetTypeValueField encounterSetAtvf, List<Encounter> encounters)
        {
            List<List<AssetTypeValue>> encounterAtvs = new();
            for (int encounterIdx = 0; encounterIdx < encounterSetAtvf.GetChildrenCount(); encounterIdx++)
            {
                Encounter encounter = encounters[encounterIdx];
                List<AssetTypeValue> atvs = new();
                atvs.Add(new AssetTypeValue(EnumValueTypes.Int32, encounter.maxLv));
                atvs.Add(new AssetTypeValue(EnumValueTypes.Int32, encounter.minLv));
                atvs.Add(new AssetTypeValue(EnumValueTypes.Int32, encounter.dexID));
                encounterAtvs.Add(atvs);
            }
            AssetTypeValueField martItemReference = encounterSetAtvf.children[0];
            encounterSetAtvf.SetChildrenList(GetATVFs(martItemReference, encounterAtvs));
        }

        /// <summary>
        ///  Updates loaded bundle with PickupItems.
        /// </summary>
        private static AssetTypeValueField[] GetATVFs(AssetTypeValueField reference, List<List<AssetTypeValue>> items)
        {
            List<AssetTypeValueField> atvfs = new();
            for (int itemIdx = 0; itemIdx < items.Count; itemIdx++)
            {
                List<AssetTypeValue> item = items[itemIdx];
                AssetTypeValueField atvf = new();
                AssetTypeValueField[] children = new AssetTypeValueField[item.Count];

                for (int i = 0; i < children.Length; i++)
                {
                    children[i] = new AssetTypeValueField();
                    children[i].Read(item[i], reference.children[i].templateField, new AssetTypeValueField[0]);
                }

                atvf.Read(null, reference.templateField, children);
                atvfs.Add(atvf);
            }
            return atvfs.ToArray();
        }

        /// <summary>
        ///  Interprets bytes of a float as an int32.
        /// </summary>
        private static int ConvertToInt(float n)
        {
            return BitConverter.ToInt32(BitConverter.GetBytes(n));
        }
    }
}
