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
        private Dictionary<int, string> trainerTypeLabels;
        public Dictionary<int, string> trainerTypeNames;
        private Dictionary<int, int> trainerTypeToCC;
        private Dictionary<string, string> labelToTrainerName;
        public List<string> items;
        public Trainer t;
        private TrainerPokemonEditorForm tpef;
        private TrainerShowdownEditorForm tsef;
        private Trainer trainerClipboard;
        private List<TrainerPokemon> tpClipboard;

        private readonly string[] sortNames = new string[]
        {
            "Sort by ID",
            "Sort by name",
            "Sort by level"
        };
        private readonly Comparison<Trainer>[] sortComparisons = new Comparison<Trainer>[]
        {
            (t1, t2) => t1.GetID().CompareTo(t2.GetID()),
            (t1, t2) => t1.GetName().CompareTo(t2.GetName()),
            (t1, t2) => t1.GetAvgLevel().CompareTo(t2.GetAvgLevel())
        };

        public TrainerEditorForm()
        {
            trainerTypeLabels = new();
            trainerTypeNames = new();
            trainerTypeToCC = new();
            trainerTypeLabels.Add(-1, "None");
            trainerTypeNames.Add(-1, "None");
            trainerTypeToCC.Add(-1, 0);
            for (int i = 0; i < gameData.trainerTypes.Count; i++)
            {
                TrainerType tt = gameData.trainerTypes[i];
                trainerTypeLabels.Add(tt.GetID(), tt.label);
                trainerTypeNames.Add(tt.GetID(), tt.GetName());
                trainerTypeToCC.Add(tt.GetID(), i + 1);
            }
            labelToTrainerName = gameData.trainerNames;
            items = gameData.items.Select(o => o.GetName()).ToList();

            InitializeComponent();
            tpef = new(this);
            tsef = new(this);

            trainers = new();
            trainers.AddRange(gameData.trainers);

            sortByComboBox.DataSource = sortNames;
            sortByComboBox.SelectedIndex = 0;
            trainers.Sort(sortComparisons[sortByComboBox.SelectedIndex]);

            PopulateListBox();
            t = trainers[0];

            trainerTypeComboBox.DataSource = trainerTypeLabels.Values.ToArray();
            trainerNameComboBox.DataSource = labelToTrainerName.Values.ToArray();

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

        private void SortChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            trainers.Sort(sortComparisons[sortByComboBox.SelectedIndex]);
            PopulateListBox();
            listBox.SelectedIndex = trainers.IndexOf(t);

            ActivateControls();
        }

        private void RefreshTrainerDisplay()
        {
            RefreshTextBoxDisplay();

            trainerTypeComboBox.SelectedIndex = trainerTypeToCC[t.trainerTypeID];
            trainerNameComboBox.SelectedItem = labelToTrainerName[t.nameLabel];
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
            t.trainerTypeID = trainerTypeNames.Keys.ToArray()[trainerTypeComboBox.SelectedIndex];
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

        private void CommitNameEdit(object sender, EventArgs e)
        {
            DeactivateControls();

            t.nameLabel = labelToTrainerName.Keys.ToArray()[trainerNameComboBox.SelectedIndex];
            t.name = (string)trainerNameComboBox.SelectedItem;
            PopulateListBox();
            RefreshTextBoxDisplay();

            ActivateControls();
        }

        private void ActivateControls()
        {
            sortByComboBox.SelectedIndexChanged += SortChanged;
            listBox.SelectedIndexChanged += TrainerChanged;

            trainerTypeComboBox.SelectedIndexChanged += CommitEdit;
            trainerNameComboBox.SelectedIndexChanged += CommitNameEdit;
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
            sortByComboBox.SelectedIndexChanged -= SortChanged;
            listBox.SelectedIndexChanged -= TrainerChanged;

            trainerTypeComboBox.SelectedIndexChanged -= CommitEdit;
            trainerNameComboBox.SelectedIndexChanged -= CommitNameEdit;
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
            trainerDisplayTextBox.Text = t.GetID() + " - " + trainerTypeNames[t.trainerTypeID] + " " + t.GetName();
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

        private void CopyTPButtonClick(object sender, EventArgs e)
        {
            tpClipboard = new();
            foreach (DataGridViewRow row in partyDataGridView.SelectedRows)
                if (row.Index >= 0 && row.Index < t.trainerPokemon.Count)
                    tpClipboard.Add(new(t.trainerPokemon[row.Index]));
        }

        private void PasteTPButtonClick(object sender, EventArgs e)
        {
            if (tpClipboard == null)
                return;

            List<TrainerPokemon> newParty = new();
            List<DataGridViewRow> selection = new();
            foreach (DataGridViewRow row in partyDataGridView.SelectedRows)
                selection.Add(row);

            int firstIndex = selection.Count > 0 ? selection.Select(r => r.Index).Min() : t.trainerPokemon.Count;
            int lastIndex = selection.Count > 0 ? selection.Select(r => r.Index).Max() : t.trainerPokemon.Count;
            for (int i = 0; i < firstIndex; i++)
                newParty.Add(new(t.trainerPokemon[i]));
            foreach (TrainerPokemon tp in tpClipboard)
                newParty.Add(new(tp));
            for (int i = lastIndex + 1; i < t.trainerPokemon.Count; i++)
                newParty.Add(new(t.trainerPokemon[i]));
            t.trainerPokemon = newParty;

            PopulatePartyDataGridView();
        }

        private void ShowdownButtonClick(object sender, EventArgs e)
        {
            tsef.SetTP(t);
            tsef.ShowDialog();
            PopulatePartyDataGridView();
        }

        private void CopyTrainerButtonClick(object sender, EventArgs e)
        {
            trainerClipboard = new(t);
        }

        private void PasteTrainerButtonClick(object sender, EventArgs e)
        {
            if (trainerClipboard != null)
            {
                DeactivateControls();
                int id = trainers[listBox.SelectedIndex].trainerID;
                trainers[listBox.SelectedIndex].SetAll(trainerClipboard);
                trainers[listBox.SelectedIndex].trainerID = id;
                PopulateListBox();
                ActivateControls();

                TrainerChanged(null, null);
            }
        }

        private void PopulateListBox()
        {
            int index = listBox.SelectedIndex;
            if (index < 0)
                index = 0;
            listBox.DataSource = trainers.Select(o => o.GetID() + " - " + o.GetName()).ToArray();
            listBox.SelectedIndex = index;
        }
    }
}