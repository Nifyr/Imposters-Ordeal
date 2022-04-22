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
            richTextBox1.Text = showdownConverter();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private String showdownConverter()
        {
            String showdownText = "";
            foreach (TrainerPokemon tp in t)
            {
                showdownText += dexEntries[tp.dexID];
                showdownText += " ";
                showdownText += genders[tp.sex];
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
            
                List<ushort> moveList = new List<ushort>();
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
                returnString.Remove(returnString.Length - 3 , 3);
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

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
