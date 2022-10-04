using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ImpostersOrdeal.GlobalData;

namespace ImpostersOrdeal
{
    public partial class JsonConverterForm : Form
    {
        private const string pokemonFileName = "IOPokémon.json";
        private List<string> typings;
        private List<string> items;
        private List<string> growthRates;
        private List<string> abilities;
        private List<string> pokemon;
        private List<string> moves;
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

        public readonly string[] modes = new string[]
        {
            "Pokémon"
        };

        public JsonConverterForm()
        {
            typings = MakeDistinct(gameData.typings.Select(n => n.GetName()));
            items = MakeDistinct(gameData.items.Select(n => n.GetName()));
            growthRates = MakeDistinct(gameData.growthRates.Select(n => n.GetName()));
            abilities = MakeDistinct(gameData.abilities.Select(n => n.GetName()));
            pokemon = MakeDistinct(gameData.dexEntries.Select(n => n.GetName()));
            moves = MakeDistinct(gameData.moves.Select(n => n.GetName()));
            tms = new();
            for (int tmID = 0; tmID < gameData.tms.Count; tmID++)
                if (gameData.tms[tmID].IsValid() && !tms.ContainsKey(gameData.items[gameData.tms[tmID].itemID].groupID - 1))
                    tms[gameData.items[gameData.tms[tmID].itemID].groupID - 1] = gameData.tms[tmID].GetFullName();

            InitializeComponent();

            modeComboBox.DataSource = modes;
            modeComboBox.SelectedIndex = 0;
        }

        private List<string> MakeDistinct(IEnumerable<string> l)
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

        private string IncrementString(string s)
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
            switch (modeComboBox.SelectedIndex)
            {
                case 0:
                    ExportPokemon();
                    break;
            }
        }

        private void ExportPokemon()
        {
            SaveFileDialog sfd = new()
            {
                FileName = pokemonFileName,
                DefaultExt = "json",
                Filter = "(*.json)|*.json",
                RestoreDirectory = true
            };
            if (sfd.ShowDialog() == DialogResult.Cancel)
                return;
            List<JsonConverterStructs.Pokemon> data = GeneratePokemonStructs(gameData.personalEntries);
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
                    compatibleTMs = p.GetCompatibleTMs().Select(i => tms[i]).ToList(),
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

        private void ImportJson(object sender, EventArgs e)
        {
            try
            {
                switch (modeComboBox.SelectedIndex)
                {
                    case 0:
                        ImportPokemon();
                        break;
                }
            }
            catch (Exception ex)
            {
                MainForm.ShowParserError(ex.Message);
            }
        }

        private void ImportPokemon()
        {
            OpenFileDialog ofd = new()
            {
                DefaultExt = "json",
                Filter = "(*.json)|*.json",
                RestoreDirectory = true
            };
            if (ofd.ShowDialog() == DialogResult.Cancel)
                return;
            List<JsonConverterStructs.Pokemon> data =
                JsonConvert.DeserializeObject<List<JsonConverterStructs.Pokemon>>
                (File.ReadAllText(ofd.FileName));
            (List<GameDataTypes.Pokemon>, List<GameDataTypes.DexEntry>) newEntries = ParsePokemonSructs(data, gameData.personalEntries);
            (gameData.personalEntries, gameData.dexEntries) = newEntries;
            DataParser.SetFamilies();
            gameData.SetModified(GameDataSet.DataField.PersonalEntries);
            gameData.SetModified(GameDataSet.DataField.DexEntries);
            ShowSuccessMessage();
        }

        private (List<GameDataTypes.Pokemon>, List<GameDataTypes.DexEntry>) ParsePokemonSructs(List<JsonConverterStructs.Pokemon> data, List<GameDataTypes.Pokemon> oldEntries)
        {
            if (data.Count != oldEntries.Count)
                throw new ArgumentException("Incorrect number of Pokémon the in json, friend.");
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
                    throw new ArgumentException("Incorrect order. The dexID of " + gp.dexID + " shows up a little early, my main man.");

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

        private int GetIndex(string[] source, string s) => GetIndex(source.ToList(), s);

        private int GetIndex(List<string> source, string s)
        {
            string u = s.ToUpper();
            int index = -1;
            for (int i = 0; index == -1 && i < source.Count; i++)
                if (source[i].ToUpper() == u)
                    index = i;
            if (index == -1)
                throw new ArgumentException("Invalid input. \"" + s + "\" is not something that exists, pal.");
            return index;
        }

        private int GetIndex(Dictionary<int, string> source, string s)
        {
            string u = s.ToUpper();
            int index = -1;
            for (int i = 0; i < source.Count && index == -1; i++)
            {
                KeyValuePair<int, string> pair = source.ElementAt(i);
                if (pair.Value == s)
                    index = pair.Key;
            }
            if (index == -1)
                throw new ArgumentException("Invalid input. \"" + s + "\" is not something that exists, chum.");
            return index;
        }

        private void ShowSuccessMessage()
        {
            MessageBox.Show("Hey, what do you know, it actually worked.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
