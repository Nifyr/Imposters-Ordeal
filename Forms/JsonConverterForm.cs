using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static ImpostersOrdeal.GlobalData;

namespace ImpostersOrdeal
{
    public partial class JsonConverterForm : Form
    {
        private const string pokemonFileName = "IOPokémon.json";
        private const string trainersFileName = "IOTrainers.json";
        private const string textFileName = "IOText.json";
        private readonly List<string> typings;
        private readonly List<string> items;
        private readonly List<string> growthRates;
        private readonly List<string> abilities;
        private readonly List<string> pokemon;
        private readonly List<string> moves;
        private readonly List<string> natures;
        public Dictionary<int, string> tms;

        private readonly string[] colors = new string[]
        {
            "Red", "Blue", "Yellow", "Green", "Black",
            "Brown", "Purple", "Gray", "White", "Pink"
        };

        public readonly string[] eggGroups = new string[]
        {
            "", "Monster", "Water 1", "Bug", "Flying", "Field", "Fairy", "Grass",
            "Human-Like", "Water 2", "Mineral", "Amorphous", "Water 2", "Ditto", "Dragon", "Undiscovered"
        };

        public readonly string[] sexOptions = new string[]
        {
            "Male", "Female", "Genderless", "Random"
        };

        public readonly string[] modes = new string[]
        {
            "Pokémon", "Trainers", "Text"
        };

        public JsonConverterForm()
        {
            typings = MakeDistinct(gameData.typings.Select(n => n.GetName()));
            items = MakeDistinct(gameData.items.Select(n => n.GetName()));
            growthRates = MakeDistinct(gameData.growthRates.Select(n => n.GetName()));
            abilities = MakeDistinct(gameData.abilities.Select(n => n.GetName()));
            pokemon = MakeDistinct(gameData.dexEntries.Select(n => n.GetName()));
            moves = MakeDistinct(gameData.moves.Select(n => n.GetName()));
            natures = MakeDistinct(gameData.natures.Select(n => n.GetName()));
            tms = new();
            for (int tmID = 0; tmID < gameData.tms.Count; tmID++)
                if (gameData.tms[tmID].IsValid() && !tms.ContainsKey(gameData.items[gameData.tms[tmID].itemID].groupID - 1))
                    tms[gameData.items[gameData.tms[tmID].itemID].groupID - 1] = gameData.tms[tmID].GetFullName();

            InitializeComponent();

            modeComboBox.DataSource = modes;
            modeComboBox.SelectedIndex = 0;
        }

        private static List<string> MakeDistinct(IEnumerable<string> l)
        {
            HashSet<string> items = new();
            foreach (string s in l)
            {
                string item = s;
                while (items.Contains(item))
                    item = IncrementString(item);
                items.Add(item);
            }
            return items.ToList();
        }

        private static string IncrementString(string s)
        {
            Regex r = new(@"\d+\z");
            Match m = r.Match(s);
            if (!m.Success)
                return s + 1;
            int n = int.Parse(m.Value);
            return r.Replace(s, (n + 1).ToString());
        }

        private void ExportJson(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new()
            {
                DefaultExt = "json",
                Filter = "(*.json)|*.json",
                RestoreDirectory = true
            };
            object data = null;
            switch (modeComboBox.SelectedIndex)
            {
                case 0:
                    sfd.FileName = pokemonFileName;
                    data = GeneratePokemonStructs(gameData.personalEntries);
                    break;
                case 1:
                    sfd.FileName = trainersFileName;
                    data = GenerateTrainerStructs(gameData.trainers);
                    break;
                case 2:
                    sfd.FileName = textFileName;
                    data = GenerateTextStructs(gameData.messageFileSets.ToList());
                    break;
            }
            if (data == null)
                return;
            if (sfd.ShowDialog() == DialogResult.Cancel)
                return;
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(sfd.FileName, json);
        }

        private List<JsonConverterStructs.Pokemon> GeneratePokemonStructs(List<GameDataTypes.Pokemon> data)
        {
            List<JsonConverterStructs.Pokemon> output = new();
            foreach (GameDataTypes.Pokemon p in data)
            {
                int[] evYield = p.GetEvYield();
                output.Add(new()
                {
                    readOnly = new()
                    {
                        personalID = p.personalID,
                        dexID = p.dexID,
                        formIndex = p.formIndex,
                        formMax = p.formMax,
                        name = p.name,
                        formID = p.formID,
                        legendaryMythical = p.legendary
                    },
                    validFlag = p.validFlag != 0,
                    color = colors[p.color],
                    graNo = p.graNo,
                    baseStats = new()
                    {
                        hp = p.basicHp,
                        atk = p.basicAtk,
                        def = p.basicDef,
                        spAtk = p.basicSpAtk,
                        spDef = p.basicSpDef,
                        spd = p.basicSpd
                    },
                    typing = p.GetTyping().Select(i => typings[i]).ToList(),
                    catchRate = p.getRate,
                    rank = p.rank,
                    evYield = new()
                    {
                        hp = (byte)evYield[0],
                        atk = (byte)evYield[1],
                        def = (byte)evYield[2],
                        spAtk = (byte)evYield[4],
                        spDef = (byte)evYield[5],
                        spd = (byte)evYield[3]
                    },
                    heldItems = new()
                    {
                        item1 = items[p.item1],
                        item2 = items[p.item2],
                        item3 = items[p.item3]
                    },
                    sexValue = p.sex,
                    eggCycles = p.eggBirth,
                    initialFriendship = p.initialFriendship,
                    eggGroups = p.GetEggGroups().Select(i => eggGroups[i]).ToList(),
                    growthRate = growthRates[p.grow],
                    abilities = new()
                    {
                        ability1 = abilities[p.abilityID1],
                        ability2 = abilities[p.abilityID2],
                        hiddenAbility = abilities[p.abilityID3]
                    },
                    expYield = p.giveExp,
                    cmHeight = p.height,
                    hgWeight = p.weight,
                    sinnohDexNumber = p.chihouZukanNo,
                    compatibleTMs = p.GetCompatibleTMs().Select(i => tms.TryGetValue(i, out string s) ? s : null).Where(s => s != null).ToList(),
                    eggTarget = pokemon[p.eggMonsno],
                    eggTargetFormno = p.eggFormno,
                    eggFormnoKawarazunoishi = p.eggFormnoKawarazunoishi,
                    eggFormInheritKawarazunoishi = p.eggFormInheritKawarazunoishi,
                    levelUpMoves = p.levelUpMoves.Select(lum => new JsonConverterStructs.LevelUpMove()
                    {
                        level = lum.level,
                        move = moves[lum.moveID]
                    }).ToList(),
                    eggMoves = p.eggMoves.Select(i => moves[i]).ToList(),
                    evolutions = p.evolutionPaths.Select(ep => new JsonConverterStructs.EvolutionPath()
                    {
                        method = ep.method,
                        parameter = ep.parameter,
                        target = pokemon[ep.destDexID],
                        targetFormID = ep.destFormID,
                        level = ep.level
                    }).ToList()
                });
            }
            return output;
        }

        private List<JsonConverterStructs.Trainer> GenerateTrainerStructs(List<GameDataTypes.Trainer> data)
        {
            List<JsonConverterStructs.Trainer> output = new();
            foreach (GameDataTypes.Trainer t in data)
            {
                bool[] aiFlags = t.GetAIFlags();
                output.Add(new()
                {
                    readOnly = new()
                    {
                        trainerID = t.trainerID,
                        name = t.name
                    },
                    trainerTypeID = t.trainerTypeID,
                    colorID = t.colorID,
                    fightType = t.fightType,
                    arenaID = t.arenaID,
                    gold = t.gold,
                    useItems = new int[]
                    {
                        t.useItem1,
                        t.useItem2,
                        t.useItem3,
                        t.useItem4,
                    }.Where(i => i > 0).Select(i => items[i]).ToList(),
                    hpRecoverFlag = t.hpRecoverFlag != 0,
                    giftItem = t.giftItem,
                    nameLabel = t.nameLabel,
                    aiFlags = new()
                    {
                        basicAI = aiFlags[0],
                        strongAI = aiFlags[1],
                        expertAI = aiFlags[2],
                        doubleBattleAI = aiFlags[3],
                        mercifulAI = aiFlags[4],
                        canUseItems = aiFlags[5],
                        canSwitchPokemon = aiFlags[6]
                    },
                    party = t.trainerPokemon.Select(tp => new JsonConverterStructs.TrainerPokemon()
                    {
                        species = pokemon[tp.dexID],
                        formID = tp.formID,
                        shiny = tp.isRare != 0,
                        level = tp.level,
                        sex = sexOptions.ElementAtOrDefault(tp.sex),
                        nature = natures[tp.natureID],
                        ability = abilities[tp.abilityID],
                        moveset = new int[]
                        {
                            tp.moveID1,
                            tp.moveID2,
                            tp.moveID3,
                            tp.moveID4
                        }.Where(i => i > 0).Select(i => moves[i]).ToList(),
                        heldItem = items[tp.itemID],
                        ballID = tp.ballID,
                        seal = tp.seal,
                        ivs = new()
                        {
                            hp = tp.hpIV,
                            atk = tp.atkIV,
                            def = tp.defIV,
                            spAtk = tp.spAtkIV,
                            spDef = tp.spDefIV,
                            spd = tp.spdIV
                        },
                        evs = new()
                        {
                            hp = tp.hpEV,
                            atk = tp.atkEV,
                            def = tp.defEV,
                            spAtk = tp.spAtkEV,
                            spDef = tp.spDefEV,
                            spd = tp.spdEV
                        }
                    }).ToList()
                });
            }
            return output;
        }

        private static List<JsonConverterStructs.MessageFileSet> GenerateTextStructs(List<GameDataTypes.MessageFileSet> data)
        {
            List<JsonConverterStructs.MessageFileSet> output = new();
            for (int languageIdx = 0; languageIdx < data.Count; languageIdx++)
            {
                GameDataTypes.MessageFileSet gmfs = data[languageIdx];
                JsonConverterStructs.MessageFileSet jmfs = new()
                {
                    textAssets = gmfs.messageFiles.Select(gmf => new JsonConverterStructs.MessageFile()
                    {
                        assetName = gmf.mName,
                        strings = gmf.labelDatas.Select(ld => ld.GetMacroString()).ToList()
                    }).ToList()
                };

                output.Add(jmfs);
            }
            return output;
        }

        private void ImportJson(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new()
            {
                DefaultExt = "json",
                Filter = "(*.json)|*.json",
                RestoreDirectory = true
            };
            if (ofd.ShowDialog() == DialogResult.Cancel)
                return;
            try
            {
                switch (modeComboBox.SelectedIndex)
                {
                    case 0:
                        List<JsonConverterStructs.Pokemon> p =
                            JsonConvert.DeserializeObject<List<JsonConverterStructs.Pokemon>>
                            (File.ReadAllText(ofd.FileName));
                        (List<GameDataTypes.Pokemon>, List<GameDataTypes.DexEntry>) newEntries = ParsePokemonSructs(p, gameData.personalEntries);
                        (gameData.personalEntries, gameData.dexEntries) = newEntries;
                        DataParser.SetFamilies();
                        gameData.SetModified(GameDataSet.DataField.PersonalEntries);
                        gameData.SetModified(GameDataSet.DataField.DexEntries);
                        break;
                    case 1:
                        List<JsonConverterStructs.Trainer> t =
                            JsonConvert.DeserializeObject<List<JsonConverterStructs.Trainer>>
                            (File.ReadAllText(ofd.FileName));
                        gameData.trainers = ParseTrainerStructs(t, gameData.trainers);
                        gameData.SetModified(GameDataSet.DataField.Trainers);
                        break;
                    case 2:
                        List<JsonConverterStructs.MessageFileSet> mfs =
                            JsonConvert.DeserializeObject<List<JsonConverterStructs.MessageFileSet>>
                            (File.ReadAllText(ofd.FileName));
                        gameData.messageFileSets = ParseTextStructs(mfs, gameData.messageFileSets.ToList()).ToArray();
                        gameData.SetModified(GameDataSet.DataField.MessageFileSets);
                        break;
                }
                ShowSuccessMessage();
            }
            catch (Exception ex)
            {
                MainForm.ShowParserError(ex.Message);
            }
        }

        private (List<GameDataTypes.Pokemon>, List<GameDataTypes.DexEntry>) ParsePokemonSructs(List<JsonConverterStructs.Pokemon> data, List<GameDataTypes.Pokemon> oldEntries)
        {
            if (data.Count == 0)
                throw new ArgumentException("You sure this was the right JSON? This one didn't have any of the data I expected...");
            if (data.Count != oldEntries.Count)
                throw new ArgumentException("Incorrect number of Pokémon in the json, friend.");
            List<GameDataTypes.Pokemon> personalEntries = new();
            List<GameDataTypes.DexEntry> dexEntries = new();
            int i = 0;
            foreach (JsonConverterStructs.Pokemon jp in data)
            {
                GameDataTypes.Pokemon gp = new()
                {
                    validFlag = (byte)(jp.validFlag ? 1 : 0),
                    personalID = oldEntries[i].personalID,
                    dexID = oldEntries[i].dexID,
                    formIndex = oldEntries[i].formIndex,
                    formMax = oldEntries[i].formMax,
                    color = (byte)GetIndex(colors, jp.color),
                    graNo = jp.graNo,
                    basicHp = jp.baseStats.hp,
                    basicAtk = jp.baseStats.atk,
                    basicDef = jp.baseStats.def,
                    basicSpd = jp.baseStats.spd,
                    basicSpAtk = jp.baseStats.spAtk,
                    basicSpDef = jp.baseStats.spDef,
                    getRate = jp.catchRate,
                    rank = jp.rank,
                    item1 = (ushort)GetIndex(items, jp.heldItems.item1),
                    item2 = (ushort)GetIndex(items, jp.heldItems.item2),
                    item3 = (ushort)GetIndex(items, jp.heldItems.item3),
                    sex = jp.sexValue,
                    eggBirth = jp.eggCycles,
                    initialFriendship = jp.initialFriendship,
                    grow = (byte)GetIndex(growthRates, jp.growthRate),
                    abilityID1 = (ushort)GetIndex(abilities, jp.abilities.ability1),
                    abilityID2 = (ushort)GetIndex(abilities, jp.abilities.ability2),
                    abilityID3 = (ushort)GetIndex(abilities, jp.abilities.hiddenAbility),
                    giveExp = jp.expYield,
                    height = jp.cmHeight,
                    weight = jp.hgWeight,
                    chihouZukanNo = jp.sinnohDexNumber,
                    eggMonsno = (ushort)GetIndex(pokemon, jp.eggTarget),
                    eggFormno = jp.eggTargetFormno,
                    eggFormnoKawarazunoishi = jp.eggFormnoKawarazunoishi,
                    eggFormInheritKawarazunoishi = jp.eggFormInheritKawarazunoishi,
                    levelUpMoves = jp.levelUpMoves.Select(lum => new GameDataTypes.LevelUpMove()
                    {
                        level = lum.level,
                        moveID = (ushort)GetIndex(moves, lum.move)
                    }).ToList(),
                    eggMoves = jp.eggMoves.Select(s => (ushort)GetIndex(moves, s)).ToList(),
                    evolutionPaths = jp.evolutions.Select(ep => new GameDataTypes.EvolutionPath()
                    {
                        method = ep.method,
                        parameter = ep.parameter,
                        destDexID = (ushort)GetIndex(pokemon, ep.target),
                        destFormID = ep.targetFormID,
                        level = ep.level
                    }).ToList(),
                    name = oldEntries[i].name,
                    formID = oldEntries[i].formID,
                    legendary = oldEntries[i].legendary
                };

                if (!new int[] { 1, 2 }.Contains(jp.typing.Count))
                    throw new ArgumentException("Invalid input. You gave " + gp.name + " " + jp.typing.Count + " typings, mate.");
                gp.SetTyping(jp.typing.Select(s => GetIndex(typings, s)).ToList());

                gp.SetEvYield(new int[]
                {
                    jp.evYield.hp,
                    jp.evYield.atk,
                    jp.evYield.def,
                    jp.evYield.spd,
                    jp.evYield.spAtk,
                    jp.evYield.spDef
                });

                if (!new int[] { 1, 2 }.Contains(jp.eggGroups.Count))
                    throw new ArgumentException("Invalid input. You gave " + gp.name + " " + jp.eggGroups.Count + " egg groups, buddy.");
                gp.SetEggGroups(jp.eggGroups.Select(s => GetIndex(eggGroups, s)).ToList());

                gp.SetCompatibleTMs(jp.compatibleTMs.Select(s => GetIndex(tms, s)).ToList());

                gp.pastPokemon = new();
                gp.nextPokemon = new();
                gp.inferiorForms = new();
                gp.superiorForms = new();

                personalEntries.Add(gp);

                if (gp.dexID > dexEntries.Count)
                    throw new ArgumentException("Incorrect order. The dexID of " + gp.dexID + " shows up a little early, my man.");

                if (dexEntries.Count == gp.dexID)
                {
                    dexEntries.Add(new());
                    dexEntries[gp.dexID].dexID = gp.dexID;
                    dexEntries[gp.dexID].forms = new();
                    dexEntries[gp.dexID].name = gp.name;
                }

                if (gp.formID > dexEntries[gp.dexID].forms.Count)
                    throw new ArgumentException("Incorrect order. For " + gp.name + ", its form" + gp.formID + " shows up a little early, homeboy.");

                dexEntries[gp.dexID].forms.Add(gp);
                i++;
            }
            return (personalEntries, dexEntries);
        }

        private List<GameDataTypes.Trainer> ParseTrainerStructs(List<JsonConverterStructs.Trainer> data, List<GameDataTypes.Trainer> oldEntries)
        {
            if (data.Count == 0)
                throw new ArgumentException("You sure this was the right JSON? This one didn't have any of the data I expected...");
            if (data.Count != oldEntries.Count)
                throw new ArgumentException("Incorrect number of trainers in the json, friend.");
            List<GameDataTypes.Trainer> trainers = new();
            for (int i = 0; i < oldEntries.Count; i++)
            {
                JsonConverterStructs.Trainer jt = data[i];
                GameDataTypes.Trainer oldT = oldEntries[i];
                GameDataTypes.Trainer gt = new()
                {
                    trainerID = oldT.trainerID,
                    name = oldT.name,
                    trainerTypeID = jt.trainerTypeID,
                    colorID = jt.colorID,
                    fightType = jt.fightType,
                    arenaID = jt.arenaID,
                    effectID = jt.effectID,
                    gold = jt.gold,
                    hpRecoverFlag = (byte)(jt.hpRecoverFlag ? 1 : 0),
                    giftItem = jt.giftItem,
                    nameLabel = jt.nameLabel,
                    trainerPokemon = jt.party.Select(tp => new GameDataTypes.TrainerPokemon()
                    {
                        dexID = (ushort)GetIndex(pokemon, tp.species),
                        formID = tp.formID,
                        isRare = (byte)(tp.shiny ? 1 : 0),
                        level = tp.level,
                        sex = (byte)GetIndex(sexOptions, tp.sex),
                        natureID = (byte)GetIndex(natures, tp.nature),
                        abilityID = (ushort)GetIndex(abilities, tp.ability),
                        itemID = (ushort)GetIndex(items, tp.heldItem),
                        ballID = tp.ballID,
                        seal = tp.seal,
                        hpIV = tp.ivs.hp,
                        atkIV = tp.ivs.atk,
                        defIV = tp.ivs.def,
                        spAtkIV = tp.ivs.spAtk,
                        spDefIV = tp.ivs.spDef,
                        spdIV = tp.ivs.spd,
                        hpEV = tp.evs.hp,
                        atkEV = tp.evs.atk,
                        defEV = tp.evs.def,
                        spAtkEV = tp.evs.spAtk,
                        spDefEV = tp.evs.spDef,
                        spdEV = tp.evs.spd
                    }).ToList()
                };

                if (jt.useItems.Count > 4)
                    throw new ArgumentException("Invalid input. You gave trainerID " + gt.trainerID + " " + jt.useItems.Count + " items, mate.");
                gt.SetItems(jt.useItems.Select(s => GetIndex(items, s)).ToList());

                gt.SetAIFlags(new bool[]
                {
                    jt.aiFlags.basicAI,
                    jt.aiFlags.strongAI,
                    jt.aiFlags.expertAI,
                    jt.aiFlags.doubleBattleAI,
                    jt.aiFlags.mercifulAI,
                    jt.aiFlags.canUseItems,
                    jt.aiFlags.canSwitchPokemon,
                    false,
                    false, false, false, false, false, false, false, false,
                    false, false, false, false, false, false, false, false,
                    false, false, false, false, false, false, false, false
                });

                for (int tpIdx = 0; tpIdx < jt.party.Count; tpIdx++)
                {
                    JsonConverterStructs.TrainerPokemon jtp = jt.party[tpIdx];
                    GameDataTypes.TrainerPokemon gtp = gt.trainerPokemon[tpIdx];
                    if (jtp.moveset.Count > 4)
                        throw new ArgumentException("Invalid input. You gave a pokémon of trainer ID " + gt.trainerID + " " + jtp.moveset.Count + " moves, buddy.");
                    gtp.SetMoves(jt.party[tpIdx].moveset.Select(s => (ushort)GetIndex(moves, s)).ToList());
                    if (jtp.formID >= gameData.dexEntries[gtp.dexID].forms.Count)
                        throw new ArgumentException("Invalid input. You gave a " + jtp.species + " of trainer ID " + gt.trainerID + " a formID of " + jtp.formID + ", chompsky honk.");
                }

                trainers.Add(gt);
            }
            return trainers;
        }

        private static List<GameDataTypes.MessageFileSet> ParseTextStructs(List<JsonConverterStructs.MessageFileSet> data, List<GameDataTypes.MessageFileSet> oldEntries)
        {
            if (data.Count == 0)
                throw new ArgumentException("You sure this was the right JSON? This one didn't have any of the data I expected...");
            if (data.Count != oldEntries.Count)
                throw new ArgumentException("Incorrect number of languages in the json, friend.");
            for (int languageIdx = 0; languageIdx < oldEntries.Count; languageIdx++)
            {
                JsonConverterStructs.MessageFileSet jmfs = data[languageIdx];
                GameDataTypes.MessageFileSet gmfs = oldEntries[languageIdx];
                if (jmfs.textAssets.Count != gmfs.messageFiles.Count)
                    throw new ArgumentException("Incorrect number of text assets in the json, my man.");
                for (int assetIdx = 0; assetIdx < gmfs.messageFiles.Count; assetIdx++)
                {
                    JsonConverterStructs.MessageFile jmf = jmfs.textAssets[assetIdx];
                    GameDataTypes.MessageFile gmf = gmfs.messageFiles[assetIdx];
                    if (jmf.strings.Count != gmf.labelDatas.Count)
                        throw new ArgumentException("Incorrect number of strings in " + jmf.assetName + ", homeboy.");
                    for (int stringIdx = 0; stringIdx < gmf.labelDatas.Count; stringIdx++)
                        gmf.labelDatas[stringIdx].SetMacroString(jmf.strings[stringIdx]);
                }
            }

            return oldEntries;
        }

        private static int GetIndex(string[] source, string s) => GetIndex(source.ToList(), s);

        private static int GetIndex(List<string> source, string s)
        {
            if (s == null)
                throw new ArgumentException("Invalid input. I won't accept \"null\" as input, champ.");
            string u = s.ToUpper();
            int index = -1;
            for (int i = 0; index == -1 && i < source.Count; i++)
                if (source[i].ToUpper() == u)
                    index = i;
            if (index == -1)
                throw new ArgumentException("Invalid input. \"" + s + "\" is not something that exists, pal.");
            return index;
        }

        private static int GetIndex(Dictionary<int, string> source, string s)
        {
            if (s == null)
                throw new ArgumentException("Invalid input. I would never accept \"null\" as input, broski.");
            string u = s.ToUpper();
            int index = -1;
            for (int i = 0; i < source.Count && index == -1; i++)
            {
                KeyValuePair<int, string> pair = source.ElementAt(i);
                if (pair.Value.ToUpper() == u)
                    index = pair.Key;
            }
            if (index == -1)
                throw new ArgumentException("Invalid input. \"" + s + "\" is not something that exists, chum.");
            return index;
        }

        private static void ShowSuccessMessage()
        {
            MessageBox.Show("Hey, what do you know, it actually worked.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
