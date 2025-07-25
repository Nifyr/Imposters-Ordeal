using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static ImpostersOrdeal.GameDataTypes;
using static ImpostersOrdeal.GlobalData;

namespace ImpostersOrdeal
{
    public partial class BattleTowerPokemonShowdownEditorForm : Form
    {
        private List<string> dexEntries;
        private List<string> natures;
        private List<string> abilities;
        private List<string> moves;
        private List<string> items;
        private BattleTowerTrainerPokemon bttMon;

        private readonly string[] genders = new string[]
        {
            "(M)", "(F)", "", "" 
        };

        private readonly string[] stats = new string[]
        {
            "HP", "Atk", "Def", "SpA", "SpD", "Spe"
        };

        private readonly Dictionary<string, int[]> monForms = PokemonFormes.monForms;


        public void SetBTP(BattleTowerTrainerPokemon btp)
        {
            bttMon = btp;
            Text = "Battle Tower Pokémon Editor: " + bttMon.GetID() + " " + bttMon.GetName();
        }

        public BattleTowerPokemonShowdownEditorForm()
        {
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
            richTextBox1.Text = ToShowdownText(bttMon);
            richTextBox2.Text = "Preview should match up with copied text.";
        }

        private void Save_Click(object sender, EventArgs e)
        {
            try
            {
                ShowdownToData(richTextBox1.Text, bttMon);
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
                BattleTowerTrainerPokemon tempMon = new();
                ShowdownToData(richTextBox1.Text, tempMon);
                richTextBox2.Text = ToShowdownText(tempMon);
            }
            catch(Exception excp)
            {
                richTextBox2.Text = "Error in parsing" + excp.ToString();
            }
        }

        private void ShowdownToData(String showdownText, BattleTowerTrainerPokemon tp)
        {
            String[] pokemon = showdownText.Trim('\r', '\n', ' ').Split("\n");
            
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

            String firstline = pokemon[0];
            String mon = firstline.Split(" ")[0];
            if (monForms.ContainsKey(mon))
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
            if (firstline.Contains("("))
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

            for (int i = 1; i < pokemon.Length; i++)
            {
                String data = pokemon[i];

                if (data.ToUpper().StartsWith("-"))
                {
                    newMoves[moveNum] = (ushort) moves.IndexOf(data[2..]);
                    if(data[2..].StartsWith("Hidden Power"))
                    {
                        newMoves[moveNum] = (ushort)237; //Hardcode for hidden power
                    }
                    moveNum++;
                }
                else if (data.ToUpper().StartsWith("ABILITY"))
                {
                    tp.abilityID = (ushort) abilities.IndexOf(data[9..]);
                }
                else if (data.ToUpper().Contains("NATURE"))
                {
                    String nature = data.Split(' ')[0];
                    tp.natureID = (byte) natures.IndexOf(nature);
                }
                else if (data.ToUpper().StartsWith("SHINY"))
                {
                    tp.isRare = 1;
                }
                else if (data.ToUpper().StartsWith("LEVEL"))
                {
                    tp.level = Byte.Parse((data.Split(" ")[1]));
                }
                else if (data.ToUpper().StartsWith("EVS"))
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

        private String ToShowdownText(BattleTowerTrainerPokemon bttp)
        {
            String showdownText = "";

            int[] formArray = new int[] { bttp.dexID, (int) bttp.formID };
            String formName = GetForm(formArray);

            if (bttp.dexID < dexEntries.Count) {
                if (formName != null)
                {
                    showdownText += formName;
                }
                else
                {
                    showdownText += dexEntries[bttp.dexID];
                }
            }
            else
            {
                showdownText += "Unrecognized";
            }
            showdownText += " ";
            if (bttp.sex != 255) //Gender defaults to 255
            {
                showdownText += genders[bttp.sex];

                if (genders[bttp.sex] != "")
                {
                        showdownText += " ";
                }
            }

            if (bttp.itemID != 0)
            {
                showdownText += String.Format("@ {0}", items[bttp.itemID]);
            }

            if (bttp.level != 100) //Level 100 isn't shown
            {
                showdownText += "\n";
                showdownText += String.Format("Level: {0}", bttp.level);
            }

            showdownText += "\n";

            showdownText += "Ability: " + abilities[bttp.abilityID];

            showdownText += "\n";

            if (bttp.isRare == 1)
            {
                showdownText += "Shiny: Yes\n";
            }

            showdownText += GetEVText(new List<byte>() {bttp.hpEV, bttp.atkEV, bttp.defEV, bttp.spAtkEV, bttp.spDefEV, bttp.spdEV});

            showdownText += String.Format("{0} Nature", natures[bttp.natureID]);
            showdownText += "\n";

            showdownText += GetIVText(new List<byte>() {bttp.hpIV, bttp.atkIV, bttp.defIV, bttp.spAtkIV, bttp.spDefIV, bttp.spdIV});
            
            List<int> moveList = new()
            {
                0,
                0,
                0,
                0,
                bttp.moveID1,
                bttp.moveID2,
                bttp.moveID3,
                bttp.moveID4
            };

            foreach (ushort moveID in moveList.Select(v => (ushort)v))
            {
                if (moveID != 0 && moveID != 65535)
                {
                    showdownText += String.Format("- {0}\n", moves[moveID]);
                }
            }

            showdownText += "\n";
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
    }
}
