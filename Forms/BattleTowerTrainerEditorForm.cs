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

namespace ImpostersOrdeal.Forms
{
    public partial class BattleTowerTrainerEditorForm : Form
    {
        public List<BattleTowerTrainer> battleTowertrainers;
        private Dictionary<int, string> trainerTypeLabels;
        public Dictionary<int, string> trainerTypeNames;
        private Dictionary<int, int> trainerTypeToCC;
        private Dictionary<string, string> labelToTrainerName;
        public List<string> items;
        public BattleTowerTrainer t;
        private BattleTowerTrainerEditorForm tpef;
        private BattleTowerTrainer trainerClipboard;
        private List<TrainerPokemon> tpClipboard;
        private BattleTowerTrainerPokemon tp;
        public List<BattleTowerTrainerPokemon> battleTowerTrainerPokemons;
        private BattleTowerTrainerPokemon trainerPokemon1;
        private BattleTowerTrainerPokemon trainerPokemon2;
        private BattleTowerTrainerPokemon trainerPokemon3;
        private int mostRecentModifiedRowIndex = -1;

        private readonly string[] sortNames = new string[]
        {
            "Sort by ID",
            "Sort by name",
        };

        private readonly string[] pokemonNames = new string[]
       {
            "Sort by ID",
            "Sort by pokedex order",
            "Sort by species name"
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
            labelToTrainerName = gameData.trainerNames;
            InitializeComponent();
            battleTowertrainers = new();
            battleTowertrainers.AddRange(gameData.battleTowerTrainers);

            sortByComboBox.DataSource = sortNames;
            sortByComboBox.SelectedIndex = 0;
            pokemonSelector.DataSource = gameData.battleTowerTrainerPokemons.Select(o => o.GetID() + " - " + String.Join(", ", gameData.dexEntries[o.dexID].GetName())).ToArray();


            battleTowertrainers.Sort(sortComparisons[sortByComboBox.SelectedIndex]);
            partyDataGridView.AllowUserToAddRows = false;
            partyDataGridView.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            PopulateListBox();
            t = battleTowertrainers[0];
            RefreshTrainerDisplay();
            ActivateControls();
        }

        private void TrainerChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            t = battleTowertrainers[listBox.SelectedIndex];
            RefreshTrainerDisplay();

            ActivateControls();
        }

        private void SortChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            battleTowertrainers.Sort(sortComparisons[sortByComboBox.SelectedIndex]);
            PopulateListBox();
            listBox.SelectedIndex = battleTowertrainers.IndexOf(t);

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
            Debug.WriteLine(numericValue + pokemonNumber);
            switch (rowIndex)
            {
                case 0:
                    t.battleTowerPokemonID1 = (uint)pokemonNumber;
                    Debug.WriteLine(t.battleTowerPokemonID1);
                    break;
                case 1:
                    t.battleTowerPokemonID2 = (uint)pokemonNumber;
                    break;
                case 2:
                    t.battleTowerPokemonID3 = (uint)pokemonNumber;
                    break;
            }
            RefreshTextBoxDisplay();
            PopulatePartyDataGridView();
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            mostRecentModifiedRowIndex = e.RowIndex;
        }

        private void CommitNameEdit(object sender, EventArgs e)
        {
            DeactivateControls();
            PopulateListBox();
            RefreshTextBoxDisplay();

            ActivateControls();
        }

        private void ActivateControls()
        {
            sortByComboBox.SelectedIndexChanged += SortChanged;
            listBox.SelectedIndexChanged += TrainerChanged;
            partyDataGridView.CellContentClick += ConfigureTP;
            partyDataGridView.CellValueChanged += dataGridView1_CellValueChanged;
            partyDataGridView.CellValueChanged += CommitEdit;

        }

        private void DeactivateControls()
        {
            sortByComboBox.SelectedIndexChanged -= SortChanged;
            listBox.SelectedIndexChanged -= TrainerChanged;
            partyDataGridView.CellValueChanged -= dataGridView1_CellValueChanged;
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
        }

        private void ConfigureTP(object sender, DataGridViewCellEventArgs e)
        {
            // DataGridView senderGrid = (DataGridView)sender;

            /*  if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
              {
                  if (e.RowIndex == t.trainerPokemon.Count)
                      t.trainerPokemon.Add(t.trainerPokemon.Count > 0 ? new(t.trainerPokemon.Last()) : new());
                  else
                  {
                      //       tpef.SetTP(t, e.RowIndex);
                      tpef.ShowDialog();
                  }

                  PopulatePartyDataGridView();
              }*/
        }

        /*   private void UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
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
           }*/


          /* private void CopyTrainerButtonClick(object sender, EventArgs e)
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
           }*/

        private void PopulateListBox()
        {
            int index = listBox.SelectedIndex;
            if (index < 0)
                index = 0;
            listBox.DataSource = battleTowertrainers.Select(o => o.GetID() + " - " + o.GetName()).ToArray();
            listBox.SelectedIndex = index;
        }

        private void partyDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void BattleTowerTrainerEditorForm_Load(object sender, EventArgs e)
        {

        }
    }
}
