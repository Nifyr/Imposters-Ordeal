using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ImpostersOrdeal.GameDataTypes;
using static ImpostersOrdeal.GlobalData;

namespace ImpostersOrdeal.Forms
{
    public partial class TrainerShowdownEditorForm : Form
    {
        private TrainerEditorForm tef;
        private List<string> dexEntries;
        private List<string> natures;
        private List<string> abilities;
        private List<string> moves;
        private List<string> items;
        private List<TrainerPokemon> t;

        private readonly string[] genders = new string[]
        {
            "(M)", "(F)", "", "" 
        };

        private readonly string[] stats = new string[]
        {
            "HP", "Atk", "Def", "SpA", "SpD", "Spe"
        };

        private readonly Dictionary<string, int[]> monForms = new Dictionary<string, int[]>
        {
            {"Unown-B", new int[] {201,1}},
            {"Unown-C", new int[] {201,2}},
            {"Unown-D", new int[] {201,3}},
            {"Unown-E", new int[] {201,4}},
            {"Unown-F", new int[] {201,5}},
            {"Unown-G", new int[] {201,6}},
            {"Unown-H", new int[] {201,7}},
            {"Unown-I", new int[] {201,8}},
            {"Unown-J", new int[] {201,9}},
            {"Unown-K", new int[] {201,10}},
            {"Unown-L", new int[] {201,11}},
            {"Unown-M", new int[] {201,12}},
            {"Unown-N", new int[] {201,13}},
            {"Unown-O", new int[] {201,14}},
            {"Unown-P", new int[] {201,15}},
            {"Unown-Q", new int[] {201,16}},
            {"Unown-R", new int[] {201,17}},
            {"Unown-S", new int[] {201,18}},
            {"Unown-T", new int[] {201,19}},
            {"Unown-U", new int[] {201,20}},
            {"Unown-V", new int[] {201,21}},
            {"Unown-W", new int[] {201,22}},
            {"Unown-X", new int[] {201,23}},
            {"Unown-Y", new int[] {201,24}},
            {"Unown-Z", new int[] {201,25}},
            {"Unown-Exclamation", new int[] {201,26}},
            {"Unown-Question", new int[] {201,27}},
            {"Castform-Sunny", new int[] {351,1}},
            {"Castform-Rainy", new int[] {351,2}},
            {"Castform-Snowy", new int[] {351,3}},
            {"Deoxys-Attack", new int[] {386,1}},
            {"Deoxys-Defense", new int[] {386,2}},
            {"Deoxys-Speed", new int[] {386,3}},
            {"Burmy-Sandy", new int[] {412,1}},
            {"Burmy-Trash", new int[] {412,1} },
            {"Wormadam-Sandy", new int[] {413,1}},
            {"Wormadam-Trash", new int[] {413,2}},
            {"Cherrim-Sunshine", new int[] {421,1}},
            {"Shellos-East", new int[] {422,1}},
            {"Gastrodon-East", new int[] {423,1}},
            {"Rotom-Heat", new int[] {479,1}},
            {"Rotom-Wash", new int[] {479,2}},
            {"Rotom-Frost", new int[] {479,3}},
            {"Rotom-Fan", new int[] {479,4}},
            {"Rotom-Mow", new int[] {479,5}},
            {"Giratina-Origin", new int[] {487,1}},
            {"Shaymin-Sky", new int[] {492,1}},
            {"Arceus-Fighting", new int[] {493,1}},
            {"Arceus-Flying", new int[] {493,2}},
            {"Arceus-Poison", new int[] {493,3}},
            {"Arceus-Ground", new int[] {493,4}},
            {"Arceus-Rock", new int[] {493,5}},
            {"Arceus-Bug", new int[] {493,6}},
            {"Arceus-Ghost", new int[] {493,7}},
            {"Arceus-Steel", new int[] {493,8}},
            {"Arceus-Fire", new int[] {493,9}},
            {"Arceus-Water", new int[] {493,10}},
            {"Arceus-Grass", new int[] {493,11}},
            {"Arceus-Electric", new int[] {493,12}},
            {"Arceus-Psychic", new int[] {493,13}},
            {"Arceus-Ice", new int[] {493,14}},
            {"Arceus-Dragon", new int[] {493,15}},
            {"Arceus-Dark", new int[] {493,16}},
            {"Arceus-Fairy", new int[] {493,17}}
        };


        public void SetTP(Trainer t)
        {
            this.t = t.trainerPokemon;
            Text = "Trainer Pokémon Editor: " + tef.trainerTypeLabels[t.trainerTypeID] +
                " " + t.GetName();
        }

        public TrainerShowdownEditorForm(TrainerEditorForm tef)
        {
            this.tef = tef;
            dexEntries = gameData.dexEntries.Select(p => p.GetName()).ToList();
            natures = gameData.natures.Select(n => n.GetName()).ToList();
            abilities = gameData.abilities.Select(a => a.GetName()).ToList();
            moves = gameData.moves.Select(m => m.GetName()).ToList();
            items = gameData.items.Select(m => m.GetName()).ToList();

            InitializeComponent();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            richTextBox1.Text = ToShowdownText(t);
            richTextBox2.Text = "Preview should match up with copied text";
        }

        private void save_Click(object sender, EventArgs e)
        {
            try
            {
                ShowdownToData(richTextBox1.Text, t);
            }
            catch (Exception ex)
            {
                richTextBox2.Text = "Error in parsing";
            }
        }

        private void preview_Click(object sender, EventArgs e)
        {
            List<TrainerPokemon> trainer = new List<TrainerPokemon>();

            for(int i = 0; i < t.Count; i++)
            {
                trainer.Add(new TrainerPokemon());
            }

            try
            {
                ShowdownToData(richTextBox1.Text, trainer);
                richTextBox2.Text = ToShowdownText(trainer);
            }
            catch(Exception ex)
            {
                richTextBox2.Text = "Error in parsing";
            }
        }

        private void ShowdownToData(String showdownText, List<TrainerPokemon> trainer)
        {
            String[] team = showdownText.Trim('\r', '\n', ' ').Split("\n");
            List<int> breakpoints = new List<int>();

            if(team.Length > 1) //Makes sure it isn't just newlines
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

            if(breakpoints.Count > 0)
            {
                breakpoints.Add(team.Length);
            }

            int pokeNum = 0;

            int point;

            for(point = 0; point < breakpoints.Count - 1; point++)
            {
                while (trainer.Count < point)
                {
                    trainer.Add(new TrainerPokemon());
                }
                TrainerPokemon tp = trainer[pokeNum];
                //Default values
                tp.isRare = 0;
                tp.level = 100;
                tp.natureID = 0;

                //Counter Values
                int moveNum = 0;

                List<ushort> newMoves = new List<ushort>() { 0, 0, 0, 0 };
                List<byte> EVs = new List<byte>() { 0, 0, 0, 0, 0, 0 };
                List<byte> IVs = new List<byte>() { 31, 31, 31, 31, 31, 31 };

                pokeNum++;

                String firstline = team[breakpoints[point]];

                try
                {
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
                }
                //Mr. Mime and Mime Jr. are both two word pokemon
                catch(Exception ex)
                {
                    String mon = firstline.Split(" ")[0] + " " + firstline.Split(" ")[1];
                    tp.dexID = (ushort) dexEntries.IndexOf(mon);
                    tp.formID = 0;
                }

                //Gender
                if(firstline.Contains("("))
                {
                    int index = firstline.IndexOf("(");
                    String genderStr = firstline.Substring(index, index + 3);
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

                    String itemStr = firstline.Substring(Index + 1).Trim();
                    tp.itemID = (ushort) items.IndexOf(itemStr);
                }

                for (int i = breakpoints[point] + 1; i < breakpoints[point + 1]; i++)
                {
                    String data = team[i];

                    if(data.ToUpper().StartsWith("-"))
                    {
                        newMoves[moveNum] = (ushort) moves.IndexOf(data[2..]);
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
                        foreach (int stat in formatStats(data, 0))
                        {
                            EVs[statNum] = (byte) stat;
                            statNum++;
                        }
                    }
                    else if (data.ToUpper().StartsWith("IVS"))
                    {
                        int statNum = 0;
                        foreach (int stat in formatStats(data, 31))
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

            while(point < trainer.Count)
            {
                trainer.RemoveAt(trainer.Count - 1);
            }
        }

        private String ToShowdownText(List<TrainerPokemon> trainer)
        {
            String showdownText = "";
            foreach (TrainerPokemon tp in trainer)
            {
                showdownText += dexEntries[tp.dexID];
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

                showdownText += "\n";

                showdownText += "Ability: " + abilities[tp.abilityID];

                showdownText += "\n";

                if(tp.isRare == 1)
                {
                    showdownText += "Shiny: Yes\n";
                }

                showdownText += getEVText(tp);

                showdownText += String.Format("{0} Nature", natures[tp.natureID]);
                showdownText += "\n";

                showdownText += getIVText(tp);
            
                List<ushort> moveList = new List<ushort>() {0, 0, 0, 0};
                moveList.Add(tp.moveID1);
                moveList.Add(tp.moveID2);
                moveList.Add(tp.moveID3);
                moveList.Add(tp.moveID4);

                foreach (ushort moveID in moveList)
                {
                    if (moveID != 0)
                    {
                        showdownText += String.Format("- {0}\n", moves[moveID]);
                    }
                }

                showdownText += "\n";
            }

            return showdownText;
        }  

        //0 Is implied EVs
        private String getEVText(TrainerPokemon tp)
        {
            List<Byte> EVList = new List<Byte>();
            EVList.Add(tp.hpEV);
            EVList.Add(tp.atkEV);
            EVList.Add(tp.defEV);
            EVList.Add(tp.spAtkEV);
            EVList.Add(tp.spDefEV);
            EVList.Add(tp.spdEV);

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
        private String getIVText(TrainerPokemon tp)
        {
            List<Byte> IVList = new List<Byte>();
            IVList.Add(tp.hpIV);
            IVList.Add(tp.atkIV);
            IVList.Add(tp.defIV);
            IVList.Add(tp.spAtkIV);
            IVList.Add(tp.spDefIV);
            IVList.Add(tp.spdIV);

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

        private List<int> formatStats(String statString, int defaultVal)
        {
            List<int> returnList = new List<int>();

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

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
