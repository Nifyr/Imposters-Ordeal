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
    public partial class TrainerEditorForm : Form
    {
        public List<Trainer> trainers;
        public Dictionary<int, string> trainerTypeLabels;
        public Dictionary<int, int> trainerTypeToCC;
        public List<string> items;
        public Trainer t;
        private TrainerPokemonEditorForm tpef;
        private Forms.TrainerShowdownEditorForm tsef;
        private List<TrainerPokemon> partyClipboard;

        public TrainerEditorForm()
        {
            trainerTypeLabels = new();
            trainerTypeToCC = new();
            trainerTypeLabels.Add(-1, "None");
            trainerTypeToCC.Add(-1, 0);
            for (int i = 0; i < gameData.trainerTypes.Count; i++)
            {
                TrainerType tt = gameData.trainerTypes[i];
                trainerTypeLabels.Add(tt.GetID(), tt.GetName());
                trainerTypeToCC.Add(tt.GetID(), i + 1);
            }
            items = gameData.items.Select(o => o.GetName()).ToList();

            InitializeComponent();
            tpef = new(this);
            tsef = new(this);

            trainers = new();
            trainers.AddRange(gameData.trainers);

            //trainers.Sort((t1, t2) => (int)(t1.aiBit - t2.aiBit));

            listBox.DataSource = trainers.Select(o => o.GetID() + " - " + o.GetName()).ToArray();
            listBox.SelectedIndex = 0;
            t = trainers[0];

            trainerTypeComboBox.DataSource = trainerTypeLabels.Values.ToArray();

            item1ComboBox.DataSource = items.ToArray();
            item2ComboBox.DataSource = items.ToArray();
            item3ComboBox.DataSource = items.ToArray();
            item4ComboBox.DataSource = items.ToArray();

            RefreshTrainerDisplay();
            ActivateControls();
        }

        private void TrainerChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            t = trainers[listBox.SelectedIndex];
            RefreshTrainerDisplay();

            ActivateControls();
        }

        private void RefreshTrainerDisplay()
        {
            RefreshTextBoxDisplay();

            trainerTypeComboBox.SelectedIndex = trainerTypeToCC[t.trainerTypeID];
            arenaIDNumericUpDown.Value = t.arenaID;
            effectIDNumericUpDown.Value = t.effectID;

            doubleBattleCheckBox.Checked = t.fightType == 1;
            prizeMoneyNumericUpDown.Value = t.gold;
            item1ComboBox.SelectedIndex = t.useItem1;
            item2ComboBox.SelectedIndex = t.useItem2;
            item3ComboBox.SelectedIndex = t.useItem3;
            item4ComboBox.SelectedIndex = t.useItem4;

            bool[] aiFlags = t.GetAIFlags();
            checkBox1.Checked = aiFlags[0];
            checkBox2.Checked = aiFlags[1];
            checkBox3.Checked = aiFlags[2];
            checkBox4.Checked = aiFlags[3];
            checkBox5.Checked = aiFlags[4];
            checkBox6.Checked = aiFlags[5];
            checkBox7.Checked = aiFlags[6];

            PopulatePartyDataGridView();
        }

        private void CommitEdit(object sender, EventArgs e)
        {
            t.trainerTypeID = trainerTypeLabels.Keys.ToArray()[trainerTypeComboBox.SelectedIndex];
            t.arenaID = (int)arenaIDNumericUpDown.Value;
            t.effectID = (int)effectIDNumericUpDown.Value;

            t.fightType = (byte)(doubleBattleCheckBox.Checked ? 1 : 0);
            t.gold = (byte)prizeMoneyNumericUpDown.Value;
            t.useItem1 = (ushort)(item1ComboBox.SelectedIndex == -1 ? 0 : item1ComboBox.SelectedIndex);
            t.useItem2 = (ushort)(item2ComboBox.SelectedIndex == -1 ? 0 : item2ComboBox.SelectedIndex);
            t.useItem3 = (ushort)(item3ComboBox.SelectedIndex == -1 ? 0 : item3ComboBox.SelectedIndex);
            t.useItem4 = (ushort)(item4ComboBox.SelectedIndex == -1 ? 0 : item4ComboBox.SelectedIndex);

            bool[] aiFlags = new bool[32];
            aiFlags[0] = checkBox1.Checked;
            aiFlags[1] = checkBox2.Checked;
            aiFlags[2] = checkBox3.Checked;
            aiFlags[3] = checkBox4.Checked;
            aiFlags[4] = checkBox5.Checked;
            aiFlags[5] = checkBox6.Checked;
            aiFlags[6] = checkBox7.Checked;
            t.SetAIFlags(aiFlags);

            RefreshTextBoxDisplay();
        }

        private void ActivateControls()
        {
            listBox.SelectedIndexChanged += TrainerChanged;

            trainerTypeComboBox.SelectedIndexChanged += CommitEdit;
            arenaIDNumericUpDown.ValueChanged += CommitEdit;
            effectIDNumericUpDown.ValueChanged += CommitEdit;

            doubleBattleCheckBox.CheckedChanged += CommitEdit;
            prizeMoneyNumericUpDown.ValueChanged += CommitEdit;
            item1ComboBox.SelectedIndexChanged += CommitEdit;
            item2ComboBox.SelectedIndexChanged += CommitEdit;
            item3ComboBox.SelectedIndexChanged += CommitEdit;
            item4ComboBox.SelectedIndexChanged += CommitEdit;

            checkBox1.CheckedChanged += CommitEdit;
            checkBox2.CheckedChanged += CommitEdit;
            checkBox3.CheckedChanged += CommitEdit;
            checkBox4.CheckedChanged += CommitEdit;
            checkBox5.CheckedChanged += CommitEdit;
            checkBox6.CheckedChanged += CommitEdit;
            checkBox7.CheckedChanged += CommitEdit;

            partyDataGridView.CellContentClick += ConfigureTP;
        }

        private void DeactivateControls()
        {
            listBox.SelectedIndexChanged -= TrainerChanged;

            trainerTypeComboBox.SelectedIndexChanged -= CommitEdit;
            arenaIDNumericUpDown.ValueChanged -= CommitEdit;
            effectIDNumericUpDown.ValueChanged -= CommitEdit;

            doubleBattleCheckBox.CheckedChanged -= CommitEdit;
            prizeMoneyNumericUpDown.ValueChanged -= CommitEdit;
            item1ComboBox.SelectedIndexChanged -= CommitEdit;
            item2ComboBox.SelectedIndexChanged -= CommitEdit;
            item3ComboBox.SelectedIndexChanged -= CommitEdit;
            item4ComboBox.SelectedIndexChanged -= CommitEdit;

            checkBox1.CheckedChanged -= CommitEdit;
            checkBox2.CheckedChanged -= CommitEdit;
            checkBox3.CheckedChanged -= CommitEdit;
            checkBox4.CheckedChanged -= CommitEdit;
            checkBox5.CheckedChanged -= CommitEdit;
            checkBox6.CheckedChanged -= CommitEdit;
            checkBox7.CheckedChanged -= CommitEdit;

            partyDataGridView.CellContentClick -= ConfigureTP;
        }

        private void RefreshTextBoxDisplay()
        {
            trainerDisplayTextBox.Text = t.GetID() + " - " + trainerTypeLabels[t.trainerTypeID] + " " + t.GetName();
        }

        private void PopulatePartyDataGridView()
        {
            partyDataGridView.Rows.Clear();
            foreach (TrainerPokemon tp in t.trainerPokemon)
            {
                partyDataGridView.Rows.Add(new object[] { gameData.GetTPDisplayName(tp), "Configure" });
            }
        }

        private void ConfigureTP(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                if (e.RowIndex == t.trainerPokemon.Count)
                    t.trainerPokemon.Add(t.trainerPokemon.Count > 0 ? new(t.trainerPokemon.Last()) : new());
                else
                {
                    tpef.SetTP(t, e.RowIndex);
                    tpef.ShowDialog();
                }

                PopulatePartyDataGridView();
            }
        }

        private void UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            t.trainerPokemon.RemoveAt(e.Row.Index);
        }

        private void CopyButtonClick(object sender, EventArgs e)
        {
            partyClipboard = new();
            foreach (TrainerPokemon tp in t.trainerPokemon)
                partyClipboard.Add(new(tp));
        }

        private void PasteButtonClick(object sender, EventArgs e)
        {
            if (partyClipboard == null)
                return;

            List<TrainerPokemon> newParty = new();
            foreach (TrainerPokemon tp in partyClipboard)
                newParty.Add(new(tp));
            t.trainerPokemon = newParty;

            PopulatePartyDataGridView();
        }

        private void ShowdownButtonClick(object sender, EventArgs e)
        {
            tsef.SetTP(t);
            tsef.ShowDialog();
        }
    }
}