using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static ImpostersOrdeal.GameDataTypes;
using static ImpostersOrdeal.GlobalData;

namespace ImpostersOrdeal
{
    public partial class TrainerShowdownEditorForm : Form
    {
        private TrainerEditorForm tef;
        private BattleTowerTrainerEditorForm bttef;
        private List<string> dexEntries;
        private List<string> natures;
        private List<string> abilities;
        private List<string> moves;
        private List<string> items;
        private List<TrainerPokemon> tMons;
        private List<BattleTowerTrainerPokemon> bttMons;

        private readonly string[] genders = new string[]
        {
            "(M)", "(F)", "", "" 
        };

        private readonly string[] stats = new string[]
        {
            "HP", "Atk", "Def", "SpA", "SpD", "Spe"
        };

        private readonly Dictionary<string, int[]> monForms = PokemonFormes.monForms;


        public void SetTP(Trainer t)
        {
            tMons = t.trainerPokemon;
            Text = "Trainer Pokémon Editor: " + tef.trainerTypeNames[t.trainerTypeID] +
                " " + t.GetName();
        }

        public void SetBTTP(BattleTowerTrainer btt)
        {
            BattleTowerTrainerPokemon pokemon1 = gameData.battleTowerTrainerPokemons.FirstOrDefault(t1 => t1.pokemonID == btt.battleTowerPokemonID1);
            BattleTowerTrainerPokemon pokemon2 = gameData.battleTowerTrainerPokemons.FirstOrDefault(t1 => t1.pokemonID == btt.battleTowerPokemonID2);
            BattleTowerTrainerPokemon pokemon3 = gameData.battleTowerTrainerPokemons.FirstOrDefault(t1 => t1.pokemonID == btt.battleTowerPokemonID3);
            BattleTowerTrainerPokemon pokemon4 = gameData.battleTowerTrainerPokemons.FirstOrDefault(t1 => t1.pokemonID == btt.battleTowerPokemonID4);
            bttMons = new() {pokemon1, pokemon2, pokemon3};
            if (pokemon4 != null) bttMons.Add(pokemon4);
            Text = "Battle Tower Trainer Pokémon Editor: " + " " + btt.GetName();
        }

        public TrainerShowdownEditorForm(TrainerEditorForm tef)
        {
            this.tef = tef;
            Init();
        }

        public TrainerShowdownEditorForm(BattleTowerTrainerEditorForm bttef)
        {
            this.bttef = bttef;
            Init();
        }

        private void Init()
        {
            dexEntries = gameData.dexEntries.Select(p => p.GetName()).ToList();
            natures = gameData.natures.Select(n => n.GetName()).ToList();
            abilities = gameData.abilities.Select(a => a.GetName()).ToList();
            moves = gameData.moves.Select(m => m.GetName()).ToList();
            items = gameData.items.Select(m => m.GetName()).ToList();

            InitializeComponent();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            if (tef != null) richTextBox1.Text = TpToShowdownText(tMons);
            else richTextBox1.Text = BttpToShowdownText(bttMons);
            richTextBox2.Text = "Preview should match up with copied text.";
        }

        private void Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (tef != null) ShowdownToTpData(richTextBox1.Text, tMons);
                else ShowdownToBttpData(richTextBox1.Text, bttMons);
            }
            catch (Exception)
            {
                richTextBox2.Text = "Error in parsing";
            }
        }

        private void Preview_Click(object sender, EventArgs e)
        {
            try
            {
                if (tef != null) 
                {
                    List<TrainerPokemon> trainerMons = new();
                    for(int i = 0; i < tMons.Count; i++)
                    {
                        trainerMons.Add(new TrainerPokemon());
                    }
                    ShowdownToTpData(richTextBox1.Text, trainerMons);
                    richTextBox2.Text = TpToShowdownText(trainerMons);
                }
                else
                {
                    List<BattleTowerTrainerPokemon> trainerMons = new();
                    for(int i = 0; i < bttMons.Count; i++)
                    {
                        trainerMons.Add(new BattleTowerTrainerPokemon());
                    }
                    ShowdownToBttpData(richTextBox1.Text, trainerMons);
                    richTextBox2.Text = BttpToShowdownText(trainerMons);
                }
            }
            catch(Exception excp)
            {
                richTextBox2.Text = "Error in parsing" + excp.ToString();
            }
        }

        private void ShowdownToTpData(String showdownText, List<TrainerPokemon> trainerMons)
        {
            String[] team = showdownText.Trim('\r', '\n', ' ').Split("\n");
            List<int> breakpoints = GetShowdownBreakpoints(team);
            int pokeNum = 0;
            int point;

            for(point = 0; point < breakpoints.Count - 1; point++)
            {
                while (trainerMons.Count <= point)
                {
                    trainerMons.Add(new TrainerPokemon());
                }
                TrainerPokemon tp = trainerMons[pokeNum];
                //Default values
                tp.isRare = 0;
                tp.level = 100;
                tp.natureID = 0;
                tp.sex = 3; //Default to random

                //Counter Values
                int moveNum = 0;

                List<ushort> newMoves = new() { 0, 0, 0, 0 };
                List<byte> EVs = new() { 0, 0, 0, 0, 0, 0 };
                List<byte> IVs = new() { 31, 31, 31, 31, 31, 31 };

                pokeNum++;

                String firstline = team[breakpoints[point]];


                String mon = firstline.Split(" ")[0];
                if(monForms.ContainsKey(mon))
                {
                    tp.dexID = (ushort) monForms[mon][0];
                    tp.formID = (ushort) monForms[mon][1];
                }

                else
                {
                    tp.dexID = (ushort) dexEntries.IndexOf(mon);
                    tp.formID = 0;
                }
                //Mr. Mime and Mime Jr. are both two word pokemon
                //-1 is 65535 for ushort numbers
                if (tp.dexID == 65535)
                {
                    mon = firstline.Split(" ")[0] + " " + firstline.Split(" ")[1];
                    tp.dexID = (ushort) dexEntries.IndexOf(mon);
                    tp.formID = 0;
                }

                //Gender
                if(firstline.Contains("("))
                {
                    int index = firstline.IndexOf("(");
                    String genderStr = firstline.Substring(index, 3);
                    tp.sex = (byte) Array.IndexOf(genders, genderStr);
                }
                else
                {
                    tp.sex = 3; //Random Gender
                }

                //Item
                if (firstline.Contains("@"))
                {
                    int Index = firstline.IndexOf("@");

                    String itemStr = firstline[(Index + 1)..].Trim();
                    tp.itemID = (ushort) items.IndexOf(itemStr);
                }

                for (int i = breakpoints[point] + 1; i < breakpoints[point + 1]; i++)
                {
                    String data = team[i];

                    if(data.ToUpper().StartsWith("-"))
                    {
                        newMoves[moveNum] = (ushort) moves.IndexOf(data[2..]);
                        if(data[2..].StartsWith("Hidden Power"))
                        {
                            newMoves[moveNum] = (ushort)237; //Hardcode for hidden power
                        }
                        moveNum++;
                    }
                    else if(data.ToUpper().StartsWith("ABILITY"))
                    {
                        tp.abilityID = (ushort) abilities.IndexOf(data[9..]);
                    }
                    else if(data.ToUpper().Contains("NATURE"))
                    {
                        String nature = data.Split(' ')[0];
                        tp.natureID = (byte) natures.IndexOf(nature);
                    }
                    else if(data.ToUpper().StartsWith("SHINY"))
                    {
                        tp.isRare = 1;
                    }
                    else if(data.ToUpper().StartsWith("LEVEL"))
                    {
                        tp.level = Byte.Parse((data.Split(" ")[1]));
                    }
                    else if(data.ToUpper().StartsWith("EVS"))
                    {
                        int statNum = 0;
                        foreach (int stat in FormatStats(data, 0))
                        {
                            EVs[statNum] = (byte) stat;
                            statNum++;
                        }
                    }
                    else if (data.ToUpper().StartsWith("IVS"))
                    {
                        int statNum = 0;
                        foreach (int stat in FormatStats(data, 31))
                        {
                            IVs[statNum] = (byte) stat;
                            statNum++;
                        }
                    } 
                    else if (data.ToUpper().StartsWith("TERA"))
                    {
                        //Tera types are not supported in Imposters Ordeal
                        //This is just to skip the line
                        continue;
                    }
                }

                //Repack into tp
                tp.moveID1 = newMoves[0];
                tp.moveID2 = newMoves[1];
                tp.moveID3 = newMoves[2];
                tp.moveID4 = newMoves[3];

                tp.hpIV = IVs[0];
                tp.atkIV = IVs[1];
                tp.defIV = IVs[2];
                tp.spAtkIV = IVs[3];
                tp.spDefIV = IVs[4];
                tp.spdIV = IVs[5];

                tp.hpEV = EVs[0];
                tp.atkEV = EVs[1];
                tp.defEV = EVs[2];
                tp.spAtkEV = EVs[3];
                tp.spDefEV = EVs[4];
                tp.spdEV = EVs[5];
            }

            while(point < trainerMons.Count)
            {
                trainerMons.RemoveAt(trainerMons.Count - 1);
            }
        }

        private void ShowdownToBttpData(String showdownText, List<BattleTowerTrainerPokemon> trainerMons)
        {
            String[] team = showdownText.Trim('\r', '\n', ' ').Split("\n");
            List<int> breakpoints = GetShowdownBreakpoints(team);
            int pokeNum = 0;
            int point;

            for(point = 0; point < breakpoints.Count - 1; point++)
            {
                while (trainerMons.Count <= point)
                {
                    trainerMons.Add(new BattleTowerTrainerPokemon());
                }
                BattleTowerTrainerPokemon tp = trainerMons[pokeNum];
                //Default values
                tp.isRare = 0;
                tp.level = 100;
                tp.natureID = 0;
                tp.sex = 3; //Default to random

                //Counter Values
                int moveNum = 0;

                List<ushort> newMoves = new() { 0, 0, 0, 0 };
                List<byte> EVs = new() { 0, 0, 0, 0, 0, 0 };
                List<byte> IVs = new() { 31, 31, 31, 31, 31, 31 };

                pokeNum++;

                String firstline = team[breakpoints[point]];


                String mon = firstline.Split(" ")[0];
                if(monForms.ContainsKey(mon))
                {
                    tp.dexID = (ushort) monForms[mon][0];
                    tp.formID = (ushort) monForms[mon][1];
                }

                else
                {
                    tp.dexID = (ushort) dexEntries.IndexOf(mon);
                    tp.formID = 0;
                }
                //Mr. Mime and Mime Jr. are both two word pokemon
                //-1 is 65535 for ushort numbers
                if (tp.dexID == 65535)
                {
                    mon = firstline.Split(" ")[0] + " " + firstline.Split(" ")[1];
                    tp.dexID = (ushort) dexEntries.IndexOf(mon);
                    tp.formID = 0;
                }

                //Gender
                if(firstline.Contains("("))
                {
                    int index = firstline.IndexOf("(");
                    String genderStr = firstline.Substring(index, 3);
                    tp.sex = (byte) Array.IndexOf(genders, genderStr);
                }
                else
                {
                    tp.sex = 3; //Random Gender
                }

                //Item
                if (firstline.Contains("@"))
                {
                    int Index = firstline.IndexOf("@");

                    String itemStr = firstline[(Index + 1)..].Trim();
                    tp.itemID = (ushort) items.IndexOf(itemStr);
                }

                for (int i = breakpoints[point] + 1; i < breakpoints[point + 1]; i++)
                {
                    String data = team[i];

                    if(data.ToUpper().StartsWith("-"))
                    {
                        newMoves[moveNum] = (ushort) moves.IndexOf(data[2..]);
                        if(data[2..].StartsWith("Hidden Power"))
                        {
                            newMoves[moveNum] = (ushort)237; //Hardcode for hidden power
                        }
                        moveNum++;
                    }
                    else if(data.ToUpper().StartsWith("ABILITY"))
                    {
                        tp.abilityID = (ushort) abilities.IndexOf(data[9..]);
                    }
                    else if(data.ToUpper().Contains("NATURE"))
                    {
                        String nature = data.Split(' ')[0];
                        tp.natureID = (byte) natures.IndexOf(nature);
                    }
                    else if(data.ToUpper().StartsWith("SHINY"))
                    {
                        tp.isRare = 1;
                    }
                    else if(data.ToUpper().StartsWith("LEVEL"))
                    {
                        tp.level = Byte.Parse((data.Split(" ")[1]));
                    }
                    else if(data.ToUpper().StartsWith("EVS"))
                    {
                        int statNum = 0;
                        foreach (int stat in FormatStats(data, 0))
                        {
                            EVs[statNum] = (byte) stat;
                            statNum++;
                        }
                    }
                    else if (data.ToUpper().StartsWith("IVS"))
                    {
                        int statNum = 0;
                        foreach (int stat in FormatStats(data, 31))
                        {
                            IVs[statNum] = (byte) stat;
                            statNum++;
                        }
                    }
                }

                //Repack into tp
                tp.moveID1 = newMoves[0];
                tp.moveID2 = newMoves[1];
                tp.moveID3 = newMoves[2];
                tp.moveID4 = newMoves[3];

                tp.hpIV = IVs[0];
                tp.atkIV = IVs[1];
                tp.defIV = IVs[2];
                tp.spAtkIV = IVs[3];
                tp.spDefIV = IVs[4];
                tp.spdIV = IVs[5];

                tp.hpEV = EVs[0];
                tp.atkEV = EVs[1];
                tp.defEV = EVs[2];
                tp.spAtkEV = EVs[3];
                tp.spDefEV = EVs[4];
                tp.spdEV = EVs[5];
            }

            while(point < trainerMons.Count)
            {
                trainerMons.RemoveAt(trainerMons.Count - 1);
            }
        }

        private List<int> GetShowdownBreakpoints(String[] team)
        {
            List<int> breakpoints = new();
            if (team.Length > 1) //Makes sure it isn't just newlines
            {
                breakpoints.Add(0);
                for (int i = 0; i < team.Length; i++)
                {
                    team[i] = team[i].Trim();
                    if (team[i] == "")
                    {
                        breakpoints.Add(i + 1);
                    }
                }
            }

            if (breakpoints.Count > 0)
            {
                breakpoints.Add(team.Length);
            }

            return breakpoints;
        }

        private String GetForm(int[] formArray)
        {
            foreach (KeyValuePair<String, int[]> entry in monForms)
            {
                if (Enumerable.SequenceEqual(formArray, entry.Value))
                {
                    return entry.Key;
                }
            }
            return null;
        }

        private String TpToShowdownText(List<TrainerPokemon> trainerMons)
        {
            String showdownText = "";
            foreach (TrainerPokemon tp in trainerMons)
            {
                int[] formArray = new int[] { tp.dexID, tp.formID };

                String formName = GetForm(formArray);

                if(tp.dexID < dexEntries.Count) {
                    if (formName != null)
                    {
                        showdownText += formName;
                    }
                    else
                    {
                        showdownText += dexEntries[tp.dexID];
                    }
                }
                else
                {
                    showdownText += "Unrecognized";
                }
                showdownText += " ";
                if(tp.sex != 255) //Gender defaults to 255
                {
                    showdownText += genders[tp.sex];

                    if (genders[tp.sex] != "")
                    {
                        showdownText += " ";
                    }
                }

                if (tp.itemID != 0)
                {
                    showdownText += String.Format("@ {0}", items[tp.itemID]);
                }

                if(tp.level != 100) //Level 100 isn't shown
                {
                    showdownText += "\n";
                    showdownText += String.Format("Level: {0}", tp.level);
                }

                showdownText += "\n";

                showdownText += "Ability: " + abilities[tp.abilityID];

                showdownText += "\n";

                if(tp.isRare == 1)
                {
                    showdownText += "Shiny: Yes\n";
                }

                showdownText += GetEVText(new List<byte>() {tp.hpEV, tp.atkEV, tp.defEV, tp.spAtkEV, tp.spDefEV, tp.spdEV});

                showdownText += String.Format("{0} Nature", natures[tp.natureID]);
                showdownText += "\n";

                showdownText += GetIVText(new List<byte>() {tp.hpIV, tp.atkIV, tp.defIV, tp.spAtkIV, tp.spDefIV, tp.spdIV});
            
                List<ushort> moveList = new() {0, 0, 0, 0};
                moveList.Add(tp.moveID1);
                moveList.Add(tp.moveID2);
                moveList.Add(tp.moveID3);
                moveList.Add(tp.moveID4);

                foreach (ushort moveID in moveList)
                {
                    if (moveID != 0 && moveID != 65535)
                    {
                        showdownText += String.Format("- {0}\n", moves[moveID]);
                    }
                }

                showdownText += "\n";
            }

            return showdownText;
        }

        private String BttpToShowdownText(List<BattleTowerTrainerPokemon> trainerMons)
        {
            String showdownText = "";
            foreach (BattleTowerTrainerPokemon tp in trainerMons)
            {
                int[] formArray = new int[] { tp.dexID, (int) tp.formID };

                String formName = GetForm(formArray);

                if(tp.dexID < dexEntries.Count) {
                    if (formName != null)
                    {
                        showdownText += formName;
                    }
                    else
                    {
                        showdownText += dexEntries[tp.dexID];
                    }
                }
                else
                {
                    showdownText += "Unrecognized";
                }
                showdownText += " ";
                if(tp.sex != 255) //Gender defaults to 255
                {
                    showdownText += genders[tp.sex];

                    if (genders[tp.sex] != "")
                    {
                        showdownText += " ";
                    }
                }

                if (tp.itemID != 0)
                {
                    showdownText += String.Format("@ {0}", items[tp.itemID]);
                }

                if(tp.level != 100) //Level 100 isn't shown
                {
                    showdownText += "\n";
                    showdownText += String.Format("Level: {0}", tp.level);
                }

                showdownText += "\n";

                showdownText += "Ability: " + abilities[tp.abilityID];

                showdownText += "\n";

                if(tp.isRare == 1)
                {
                    showdownText += "Shiny: Yes\n";
                }

                showdownText += GetEVText(new List<byte>() {tp.hpEV, tp.atkEV, tp.defEV, tp.spAtkEV, tp.spDefEV, tp.spdEV});

                showdownText += String.Format("{0} Nature", natures[tp.natureID]);
                showdownText += "\n";

                showdownText += GetIVText(new List<byte>() {tp.hpIV, tp.atkIV, tp.defIV, tp.spAtkIV, tp.spDefIV, tp.spdIV});
            
                List<int> moveList = new() {0, 0, 0, 0};
                moveList.Add(tp.moveID1);
                moveList.Add(tp.moveID2);
                moveList.Add(tp.moveID3);
                moveList.Add(tp.moveID4);

                foreach (ushort moveID in moveList)
                {
                    if (moveID != 0 && moveID != 65535)
                    {
                        showdownText += String.Format("- {0}\n", moves[moveID]);
                    }
                }

                showdownText += "\n";
            }

            return showdownText;
        }

        //0 Is implied EVs
        private String GetEVText(List<Byte> EVList)
        {
            bool hasEV = false;
            String returnString = "";
            byte b;

            //Format a HP / b Atk / c Def / d SpA / e SpD / f Spe  
            for (int i = 0; i < EVList.Count; i++)
            {
                b = EVList[i];
                if(b != 0)
                {
                    if(!hasEV)
                    {
                        returnString += "EVs: ";
                        hasEV = true;
                    }
                    returnString = returnString += String.Format("{0} {1} / ", b, stats[i]);
                }
            }

            if(hasEV)
            {
                returnString = returnString.Remove(returnString.Length - 3 , 3);
                returnString += "\n";
            }

            return returnString;
        }

        //31 Is implied IVs
        private String GetIVText(List<Byte> IVList)
        {
            bool hasIV = false;
            String returnString = "";
            byte b;

            //Format a HP / b Atk / c Def / d SpA / e SpD / f Spe  
            for (int i = 0; i < IVList.Count; i++)
            {
                b = IVList[i];
                if (b != 31)
                {
                    if (!hasIV)
                    {
                        returnString += "IVs: ";
                        hasIV = true;
                    }
                    returnString += String.Format("{0} {1} / ", b, stats[i]);
                }
            }

            if (hasIV)
            {
                returnString = returnString.Remove(returnString.Length - 3, 3);
                returnString += "\n";
            }

            return returnString;
        }

        private List<int> FormatStats(String statString, int defaultVal)
        {
            List<int> returnList = new();

            for(int i = 0; i < 6; i++)
            {
                returnList.Add(defaultVal);
            }

            String[] statList = statString[4..].TrimEnd('/', ' ').Split("/");
            foreach (String stat in statList)
            {
                String[] statSplit = stat.Trim().Split(" ");
                int statVal = int.Parse(statSplit[0]);
                String statName = statSplit[1];

                int index = Array.IndexOf(stats, statName);

                returnList[index] = statVal;
            }

            return returnList;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
