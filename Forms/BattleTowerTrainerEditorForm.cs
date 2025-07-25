using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ImpostersOrdeal.GameDataTypes;
using static ImpostersOrdeal.GlobalData;

namespace ImpostersOrdeal
{
    public partial class BattleTowerTrainerEditorForm : Form
    {
        public List<BattleTowerTrainer> battleTowertrainers;
        public List<BattleTowerTrainer> battleTowertrainersDoubles;
        private Dictionary<int, string> trainerTypeLabels;
        public Dictionary<int, string> trainerTypeNames;
        private Dictionary<int, int> trainerTypeToCC;
        public List<string> items;
        public BattleTowerTrainer t;
        public List<BattleTowerTrainerPokemon> battleTowerTrainerPokemons;
        private BattleTowerTrainerPokemon trainerPokemon1;
        private BattleTowerTrainerPokemon trainerPokemon2;
        private BattleTowerTrainerPokemon trainerPokemon3;
        private BattleTowerTrainerPokemon trainerPokemon4;
        private TrainerShowdownEditorForm tsef;
        private int mostRecentModifiedRowIndex = -1;
        private bool doubleTrainerMode = false;

        private readonly string[] sortNames = new string[]
        {
            "Sort by ID",
            "Sort by name",
         //   "Sort by internal ID"
        };

        private readonly Comparison<BattleTowerTrainer>[] sortComparisons = new Comparison<BattleTowerTrainer>[]
        {
            (t1, t2) => t1.GetID().CompareTo(t2.GetID()),
            (t1, t2) => t1.GetName().CompareTo(t2.GetName()),
        };

        public BattleTowerTrainerEditorForm()
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
            InitializeComponent();
            tsef = new(this);
            battleTowertrainers = new();
            battleTowertrainers.AddRange(gameData.battleTowerTrainers);
            battleTowertrainersDoubles = new();
            battleTowertrainersDoubles.AddRange(gameData.battleTowerTrainersDouble);
            sortByComboBox.DataSource = sortNames;
            sortByComboBox.SelectedIndex = 0;
            pokemonSelector.DataSource = gameData.battleTowerTrainerPokemons.Select(o => o.GetID() + " - " + String.Join(", ", gameData.dexEntries[o.dexID].GetName())).ToArray();
            battleTowertrainers.Sort(sortComparisons[sortByComboBox.SelectedIndex]);
            battleTowertrainersDoubles.Sort(sortComparisons[sortByComboBox.SelectedIndex]);
            partyDataGridView.AllowUserToAddRows = false;
            partyDataGridView.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            PopulateListBox(false);
            t = battleTowertrainers[0];
            RefreshTrainerDisplay();
            ActivateControls();
        }

        private void TrainerChanged(object sender, EventArgs e)
        {
            DeactivateControls();
            if (doubleTrainerMode == false)
            {
                t = battleTowertrainers[listBox.SelectedIndex];
            }
            else
            {
                t = battleTowertrainersDoubles[listBox.SelectedIndex];
            }
            RefreshTrainerDisplay();

            ActivateControls();
        }

        private void SortChanged(object sender, EventArgs e)
        {
            DeactivateControls();
            if (doubleTrainerMode == false)
            {
                battleTowertrainers.Sort(sortComparisons[sortByComboBox.SelectedIndex]);
                PopulateListBox(false);
                listBox.SelectedIndex = battleTowertrainers.IndexOf(t);
            }
            else
            {
                battleTowertrainersDoubles.Sort(sortComparisons[sortByComboBox.SelectedIndex]);
                PopulateListBox(false);
                listBox.SelectedIndex = battleTowertrainersDoubles.IndexOf(t);
            }

            ActivateControls();
        }

        private void RefreshTrainerDisplay()
        {
            RefreshTextBoxDisplay();
            PopulatePartyDataGridView();
        }

        private void CommitEdit(object sender, EventArgs e)
        {
            //Get the values needed
            int rowIndex = mostRecentModifiedRowIndex; ;
            int columnIndex = partyDataGridView.Columns["pokemonSelector"].Index;
            DataGridViewComboBoxCell comboBoxCell1 = (DataGridViewComboBoxCell)partyDataGridView.Rows[rowIndex].Cells[columnIndex];
            string selectedValue = comboBoxCell1.Value.ToString();
            string numericValue = Regex.Replace(selectedValue, @"[^0-9]", "");
            int pokemonNumber = int.Parse(numericValue);
            switch (rowIndex)
            {
                case 0:
                    t.battleTowerPokemonID1 = (uint)pokemonNumber;
                    break;
                case 1:
                    t.battleTowerPokemonID2 = (uint)pokemonNumber;
                    break;
                case 2:
                    t.battleTowerPokemonID3 = (uint)pokemonNumber;
                    break;
                case 3:
                    t.battleTowerPokemonID4 = (uint)pokemonNumber;
                    break;
            }
            RefreshTextBoxDisplay();
            PopulatePartyDataGridView();
        }
        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            mostRecentModifiedRowIndex = e.RowIndex;
        }

        private void CommitNameEdit(object sender, EventArgs e)
        {
            DeactivateControls();
            PopulateListBox(false);
            RefreshTextBoxDisplay();

            ActivateControls();
        }

        private void ActivateControls()
        {
            sortByComboBox.SelectedIndexChanged += SortChanged;
            listBox.SelectedIndexChanged += TrainerChanged;
            partyDataGridView.CellContentClick += ConfigureTP;
            partyDataGridView.CellValueChanged += DataGridView1_CellValueChanged;
            partyDataGridView.CellValueChanged += CommitEdit;

        }

        private void DeactivateControls()
        {
            sortByComboBox.SelectedIndexChanged -= SortChanged;
            listBox.SelectedIndexChanged -= TrainerChanged;
            partyDataGridView.CellValueChanged -= DataGridView1_CellValueChanged;
            partyDataGridView.CellValueChanged -= CommitEdit;
            partyDataGridView.CellContentClick -= ConfigureTP;
        }

        private void RefreshTextBoxDisplay()
        {
            trainerDisplayTextBox.Text = t.GetID() + " - " + t.GetName();
        }

        private void PopulatePartyDataGridView()
        {
            partyDataGridView.Rows.Clear();

            trainerPokemon1 = gameData.battleTowerTrainerPokemons.FirstOrDefault(t1 => t1.pokemonID == t.battleTowerPokemonID1);
            trainerPokemon2 = gameData.battleTowerTrainerPokemons.FirstOrDefault(t1 => t1.pokemonID == t.battleTowerPokemonID2);
            trainerPokemon3 = gameData.battleTowerTrainerPokemons.FirstOrDefault(t1 => t1.pokemonID == t.battleTowerPokemonID3);
            string nameTrainerPokemon1 = gameData.dexEntries[trainerPokemon1.dexID].GetName();
            string nameTrainerPokemon2 = gameData.dexEntries[trainerPokemon2.dexID].GetName();
            string nameTrainerPokemon3 = gameData.dexEntries[trainerPokemon3.dexID].GetName();
            partyDataGridView.Rows.Add(new object[] { t.battleTowerPokemonID1 + " - " + nameTrainerPokemon1 });
            partyDataGridView.Rows.Add(new object[] { t.battleTowerPokemonID2 + " - " + nameTrainerPokemon2 });
            partyDataGridView.Rows.Add(new object[] { t.battleTowerPokemonID3 + " - " + nameTrainerPokemon3 });
            if (t.isDouble == true)
            {
                trainerPokemon4 = gameData.battleTowerTrainerPokemons.FirstOrDefault(t1 => t1.pokemonID == t.battleTowerPokemonID4);
                string nameTrainerPokemon4 = gameData.dexEntries[trainerPokemon4.dexID].GetName();
                partyDataGridView.Rows.Add(new object[] { t.battleTowerPokemonID4 + " - " + nameTrainerPokemon4 });
            }
        }

        private void ConfigureTP(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void ShowdownButtonClick(object sender, EventArgs e)
        {
            tsef.SetBTTP(t);
            tsef.ShowDialog();
            PopulatePartyDataGridView();
        }

        private void PopulateListBox(bool resetIndex)
        {
            int index = 0;
            if (resetIndex == false)
            {
                index = listBox.SelectedIndex;
            }
            if (index < 0)
                index = 0;
            if (doubleTrainerMode == false)
            {
                listBox.DataSource = battleTowertrainers.Select(o => o.GetID() + " - " + o.GetName()).ToArray();
                listBox.SelectedIndex = index;
            }
            else
            {
                listBox.DataSource = battleTowertrainersDoubles.Select(o => o.GetID() + " - " + o.GetName()).ToArray();
                listBox.SelectedIndex = index;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            doubleTrainerMode = true;
            PopulateListBox(true);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            doubleTrainerMode = false;
            //Todo
            //Add trainer type to editor
            PopulateListBox(true);
        }
    }
}
