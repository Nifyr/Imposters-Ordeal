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

namespace ImpostersOrdeal
{
    public partial class PokemonEditorForm : Form
    {
        private List<DexEntry> dexEntries;
        public List<string> pokemon;
        public List<string> moves;
        public List<string> typings;
        public List<string> items;
        public List<string> abilities;
        public Dictionary<int, string> tms;
        public Pokemon p;

        public readonly string[] colors = new string[]
            {
                "Red", "Blue", "Yellow", "Green", "Black",
                "Brown", "Purple", "Gray", "White", "Pink"
            };

        public readonly string[] eggGroups = new string[]
            {
                "", "Monster", "Water 1", "Bug", "Flying", "Field", "Fairy", "Grass",
                "Human-Like", "Water 2", "Mineral", "Amorphous", "Water 2", "Ditto", "Dragon", "Undiscovered"
            };

        public PokemonEditorForm()
        {
            InitializeComponent();

            dexEntries = gameData.dexEntries;
            pokemon = dexEntries.Select(m => m.GetName()).ToList();
            moves = gameData.moves.Select(m => m.GetName()).ToList();
            typings = gameData.typings.Select(t => t.GetName()).ToList();
            items = gameData.items.Select(i => i.GetName()).ToList();
            abilities = gameData.abilities.Select(a => a.GetName()).ToList();
            tms = new();
            for (int tmID = 0; tmID < gameData.tms.Count; tmID++)
                if (gameData.tms[tmID].IsValid() && !tms.ContainsKey(gameData.items[gameData.tms[tmID].itemID].groupID - 1))
                    tms[gameData.items[gameData.tms[tmID].itemID].groupID - 1] = gameData.tms[tmID].GetName() + " " + gameData.moves[gameData.tms[tmID].moveID].GetName();

            dexIDComboBox.DataSource = dexEntries.Select(d => d.GetName()).ToArray();
            dexIDComboBox.SelectedIndex = 0;
            formIDComboBox.DataSource = dexEntries[0].forms.Select((p, i) => i).ToArray();
            formIDComboBox.SelectedIndex = 0;
            p = dexEntries[0].forms[0];

            type1ComboBox.DataSource = typings.ToArray();
            type2ComboBox.DataSource = typings.ToArray();

            item1ComboBox.DataSource = items.ToArray();
            item2ComboBox.DataSource = items.ToArray();
            item3ComboBox.DataSource = items.ToArray();

            ability1ComboBox.DataSource = abilities.ToArray();
            ability2ComboBox.DataSource = abilities.ToArray();
            hiddenAbilityComboBox.DataSource = abilities.ToArray();

            colorComboBox.DataSource = colors;
            eggGroup1ComboBox.DataSource = eggGroups.ToArray();
            eggGroup2ComboBox.DataSource = eggGroups.ToArray();
            growthComboBox.DataSource = gameData.growthRates.Select(g => g.GetName()).ToArray();

            tmCompatibilityCheckedListBox.Items.Clear();
            tmCompatibilityCheckedListBox.Items.AddRange(tms.Values.ToArray());

            levelUpMoveColumn.DataSource = moves.ToArray();
            levelColumn.ValueType = typeof(ushort);
            eggMoveColumn.DataSource = moves.ToArray();

            RefreshPokemonDisplay();

            ActivateControls();
        }

        private void DexIDChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            formIDComboBox.DataSource = dexEntries[dexIDComboBox.SelectedIndex].forms.Select((p, i) => i).ToList();
            formIDComboBox.SelectedIndex = 0;
            p = dexEntries[dexIDComboBox.SelectedIndex].forms[0];
            RefreshPokemonDisplay();

            ActivateControls();
        }

        private void FormIDChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            p = dexEntries[dexIDComboBox.SelectedIndex].forms[formIDComboBox.SelectedIndex];
            RefreshPokemonDisplay();

            ActivateControls();
        }

        private void RefreshPokemonDisplay()
        {
            personalIDTextBox.Text = p.personalID.ToString();

            numericUpDown1.Value = p.basicHp;
            numericUpDown2.Value = p.basicAtk;
            numericUpDown3.Value = p.basicDef;
            numericUpDown4.Value = p.basicSpAtk;
            numericUpDown5.Value = p.basicSpDef;
            numericUpDown6.Value = p.basicSpd;
            bstTextBox.Text = p.GetBST().ToString();

            int[] evYield = p.GetEvYield();
            numericUpDown7.Value = evYield[0];
            numericUpDown8.Value = evYield[1];
            numericUpDown9.Value = evYield[2];
            numericUpDown10.Value = evYield[4];
            numericUpDown11.Value = evYield[5];
            numericUpDown12.Value = evYield[3];

            type1ComboBox.SelectedIndex = p.typingID1;
            type2ComboBox.SelectedIndex = p.typingID2;

            item1ComboBox.SelectedIndex = p.item1;
            item2ComboBox.SelectedIndex = p.item2;
            item3ComboBox.SelectedIndex = p.item3;

            ability1ComboBox.SelectedIndex = p.abilityID1;
            ability2ComboBox.SelectedIndex = p.abilityID2;
            hiddenAbilityComboBox.SelectedIndex = p.abilityID3;

            colorComboBox.SelectedIndex = p.color;
            numericUpDown13.Value = p.graNo;
            numericUpDown14.Value = p.getRate;
            numericUpDown15.Value = p.rank;
            numericUpDown16.Value = p.sex;
            numericUpDown17.Value = p.eggBirth;
            numericUpDown18.Value = p.initialFriendship;
            eggGroup1ComboBox.SelectedIndex = p.eggGroup1;
            eggGroup2ComboBox.SelectedIndex = p.eggGroup2;
            growthComboBox.SelectedIndex = p.grow;
            numericUpDown19.Value = p.giveExp;
            numericUpDown20.Value = p.height;
            numericUpDown21.Value = p.weight;

            bool[] tmCompatibility = p.GetTMCompatibility();
            for (int i = 0; i < tmCompatibilityCheckedListBox.Items.Count; i++)
                tmCompatibilityCheckedListBox.SetItemChecked(i, tmCompatibility[tms.Keys.ToArray()[i]]);

            levelUpMoveDataGridView.Rows.Clear();
            for (int i = 0; i < p.levelUpMoves.Count; i++)
                levelUpMoveDataGridView.Rows.Add(new object[] { p.levelUpMoves[i].level, moves[p.levelUpMoves[i].moveID] });

            eggMoveDataGridView.Rows.Clear();
            for (int i = 0; i < p.eggMoves.Count; i++)
                eggMoveDataGridView.Rows.Add(new object[] { moves[p.eggMoves[i]] });
        }

        private void CommitEdit(object sender, EventArgs e)
        {
            p.basicHp = (byte)numericUpDown1.Value;
            p.basicAtk = (byte)numericUpDown2.Value;
            p.basicDef = (byte)numericUpDown3.Value;
            p.basicSpAtk = (byte)numericUpDown4.Value;
            p.basicSpDef = (byte)numericUpDown5.Value;
            p.basicSpd = (byte)numericUpDown6.Value;
            bstTextBox.Text = p.GetBST().ToString();

            p.SetEvYield(new int[]
            {
                (int)numericUpDown7.Value,
                (int)numericUpDown8.Value,
                (int)numericUpDown9.Value,
                (int)numericUpDown11.Value,
                (int)numericUpDown12.Value,
                (int)numericUpDown10.Value
            });

            p.typingID1 = (byte)type1ComboBox.SelectedIndex;
            p.typingID2 = (byte)type2ComboBox.SelectedIndex;

            p.item1 = (ushort)item1ComboBox.SelectedIndex;
            p.item2 = (ushort)item2ComboBox.SelectedIndex;
            p.item3 = (ushort)item3ComboBox.SelectedIndex;

            p.abilityID1 = (ushort)ability1ComboBox.SelectedIndex;
            p.abilityID2 = (ushort)ability2ComboBox.SelectedIndex;
            p.abilityID3 = (ushort)hiddenAbilityComboBox.SelectedIndex;

            p.color = (byte)colorComboBox.SelectedIndex;
            p.graNo = (ushort)numericUpDown13.Value;
            p.getRate = (byte)numericUpDown14.Value;
            p.rank = (byte)numericUpDown15.Value;
            p.sex = (byte)numericUpDown16.Value;
            p.eggBirth = (byte)numericUpDown17.Value;
            p.initialFriendship = (byte)numericUpDown18.Value;
            p.eggGroup1 = (byte)eggGroup1ComboBox.SelectedIndex;
            p.eggGroup2 = (byte)eggGroup2ComboBox.SelectedIndex;
            p.grow = (byte)growthComboBox.SelectedIndex;
            p.giveExp = (byte)numericUpDown19.Value;
            p.height = (byte)numericUpDown20.Value;
            p.weight = (byte)numericUpDown21.Value;

            bool[] tmCompatibility = new bool[128];
            for (int i = 0; i < tmCompatibilityCheckedListBox.Items.Count; i++)
                tmCompatibility[tms.Keys.ToArray()[i]] = tmCompatibilityCheckedListBox.GetItemChecked(i);
            p.SetTMCompatibility(tmCompatibility);

            List<LevelUpMove> levelUpMoves = new();
            for (int i = 0; i < levelUpMoveDataGridView.Rows.Count; i++)
            {
                if (levelUpMoveDataGridView.Rows[i].Cells[0].Value == null ||
                    levelUpMoveDataGridView.Rows[i].Cells[1].Value == null ||
                    moves.IndexOf((string)levelUpMoveDataGridView.Rows[i].Cells[1].Value) == 0)
                    continue;
                LevelUpMove l = new();
                l.level = (ushort)levelUpMoveDataGridView.Rows[i].Cells[0].Value;
                l.moveID = (ushort)moves.IndexOf((string)levelUpMoveDataGridView.Rows[i].Cells[1].Value);
                levelUpMoves.Add(l);
            }
            p.levelUpMoves = levelUpMoves;

            List<ushort> eggMoves = new();
            for (int i = 0; i < eggMoveDataGridView.Rows.Count; i++)
            {
                if (eggMoveDataGridView.Rows[i].Cells[0].Value == null ||
                    moves.IndexOf((string)eggMoveDataGridView.Rows[i].Cells[0].Value) == 0)
                    continue;
                eggMoves.Add((ushort)moves.IndexOf((string)eggMoveDataGridView.Rows[i].Cells[0].Value));
            }
            p.eggMoves = eggMoves;
        }

        private void ActivateControls()
        {
            dexIDComboBox.SelectedIndexChanged += DexIDChanged;
            formIDComboBox.SelectedIndexChanged += FormIDChanged;
            numericUpDown1.ValueChanged += CommitEdit;
            numericUpDown2.ValueChanged += CommitEdit;
            numericUpDown3.ValueChanged += CommitEdit;
            numericUpDown4.ValueChanged += CommitEdit;
            numericUpDown5.ValueChanged += CommitEdit;
            numericUpDown6.ValueChanged += CommitEdit;
            numericUpDown12.ValueChanged += CommitEdit;
            numericUpDown11.ValueChanged += CommitEdit;
            numericUpDown10.ValueChanged += CommitEdit;
            numericUpDown9.ValueChanged += CommitEdit;
            numericUpDown8.ValueChanged += CommitEdit;
            numericUpDown7.ValueChanged += CommitEdit;
            type1ComboBox.SelectedIndexChanged += CommitEdit;
            type2ComboBox.SelectedIndexChanged += CommitEdit;
            item1ComboBox.SelectedIndexChanged += CommitEdit;
            item2ComboBox.SelectedIndexChanged += CommitEdit;
            item3ComboBox.SelectedIndexChanged += CommitEdit;
            ability1ComboBox.SelectedIndexChanged += CommitEdit;
            ability2ComboBox.SelectedIndexChanged += CommitEdit;
            hiddenAbilityComboBox.SelectedIndexChanged += CommitEdit;
            colorComboBox.SelectedIndexChanged += CommitEdit;
            numericUpDown13.ValueChanged += CommitEdit;
            numericUpDown14.ValueChanged += CommitEdit;
            numericUpDown15.ValueChanged += CommitEdit;
            numericUpDown16.ValueChanged += CommitEdit;
            numericUpDown17.ValueChanged += CommitEdit;
            numericUpDown18.ValueChanged += CommitEdit;
            eggGroup1ComboBox.SelectedIndexChanged += CommitEdit;
            eggGroup2ComboBox.SelectedIndexChanged += CommitEdit;
            growthComboBox.SelectedIndexChanged += CommitEdit;
            numericUpDown19.ValueChanged += CommitEdit;
            numericUpDown20.ValueChanged += CommitEdit;
            numericUpDown21.ValueChanged += CommitEdit;
            tmCompatibilityCheckedListBox.Leave += CommitEdit;
            levelUpMoveDataGridView.CellEndEdit += CommitEdit;
            levelUpMoveDataGridView.UserDeletedRow += CommitEdit;
            eggMoveDataGridView.CellEndEdit += CommitEdit;
            eggMoveDataGridView.UserDeletedRow += CommitEdit;
        }

        private void DeactivateControls()
        {
            dexIDComboBox.SelectedIndexChanged -= DexIDChanged;
            formIDComboBox.SelectedIndexChanged -= FormIDChanged;
            numericUpDown1.ValueChanged -= CommitEdit;
            numericUpDown2.ValueChanged -= CommitEdit;
            numericUpDown3.ValueChanged -= CommitEdit;
            numericUpDown4.ValueChanged -= CommitEdit;
            numericUpDown5.ValueChanged -= CommitEdit;
            numericUpDown6.ValueChanged -= CommitEdit;
            numericUpDown12.ValueChanged -= CommitEdit;
            numericUpDown11.ValueChanged -= CommitEdit;
            numericUpDown10.ValueChanged -= CommitEdit;
            numericUpDown9.ValueChanged -= CommitEdit;
            numericUpDown8.ValueChanged -= CommitEdit;
            numericUpDown7.ValueChanged -= CommitEdit;
            type1ComboBox.SelectedIndexChanged -= CommitEdit;
            type2ComboBox.SelectedIndexChanged -= CommitEdit;
            item1ComboBox.SelectedIndexChanged -= CommitEdit;
            item2ComboBox.SelectedIndexChanged -= CommitEdit;
            item3ComboBox.SelectedIndexChanged -= CommitEdit;
            ability1ComboBox.SelectedIndexChanged -= CommitEdit;
            ability2ComboBox.SelectedIndexChanged -= CommitEdit;
            hiddenAbilityComboBox.SelectedIndexChanged -= CommitEdit;
            colorComboBox.SelectedIndexChanged -= CommitEdit;
            numericUpDown13.ValueChanged -= CommitEdit;
            numericUpDown14.ValueChanged -= CommitEdit;
            numericUpDown15.ValueChanged -= CommitEdit;
            numericUpDown16.ValueChanged -= CommitEdit;
            numericUpDown17.ValueChanged -= CommitEdit;
            numericUpDown18.ValueChanged -= CommitEdit;
            eggGroup1ComboBox.SelectedIndexChanged -= CommitEdit;
            eggGroup2ComboBox.SelectedIndexChanged -= CommitEdit;
            growthComboBox.SelectedIndexChanged -= CommitEdit;
            numericUpDown19.ValueChanged -= CommitEdit;
            numericUpDown20.ValueChanged -= CommitEdit;
            numericUpDown21.ValueChanged -= CommitEdit;
            tmCompatibilityCheckedListBox.Leave -= CommitEdit;
            levelUpMoveDataGridView.CellEndEdit -= CommitEdit;
            levelUpMoveDataGridView.UserDeletedRow -= CommitEdit;
            eggMoveDataGridView.CellEndEdit -= CommitEdit;
            eggMoveDataGridView.UserDeletedRow -= CommitEdit;
        }

        private void DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Yeah, no. That's not gonna fly buster.\nInput some actual valid data please.",
                "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void LevelUpMoveDefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.SetValues(new object[] { (ushort)0, moves[0] });
        }

        private void EggMoveDefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.SetValues(new object[] { moves[0] });
        }

        private void OpenEvolutionEditor(object sender, EventArgs e)
        {
            EvolutionEditorForm eef = new(this);
            eef.ShowDialog();
        }
    }
}
