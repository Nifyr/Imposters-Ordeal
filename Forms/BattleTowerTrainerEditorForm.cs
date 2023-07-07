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
        //   private TrainerShowdownEditorForm tsef;
        private BattleTowerTrainer trainerClipboard;
        private List<TrainerPokemon> tpClipboard;

        private readonly string[] sortNames = new string[]
        {
            "Sort by ID",
            "Sort by name",
           // "Sort by level"
        };

        private readonly Comparison<BattleTowerTrainer>[] sortComparisons = new Comparison<BattleTowerTrainer>[]
        {
            (t1, t2) => t1.GetID().CompareTo(t2.GetID()),
            (t1, t2) => t1.GetName().CompareTo(t2.GetName()),
        //    (t1, t2) => t1.GetAvgLevel().CompareTo(t2.GetAvgLevel())
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
            //  items = gameData.items.Select(o => o.GetName()).ToList();

            InitializeComponent();
            //   tpef = new(this);
            //     tsef = new(this);

            battleTowertrainers = new();
            battleTowertrainers.AddRange(gameData.battleTowerTrainers);

            sortByComboBox.DataSource = sortNames;
            sortByComboBox.SelectedIndex = 0;
            battleTowertrainers.Sort(sortComparisons[sortByComboBox.SelectedIndex]);

            PopulateListBox();
            t = battleTowertrainers[0];

            // trainerTypeComboBox.DataSource = trainerTypeLabels.Values.ToArray();
            // trainerNameComboBox.DataSource = labelToTrainerName.Values.ToArray();
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

            //    trainerTypeComboBox.SelectedIndex = trainerTypeToCC[t.trainerTypeID];
            PopulatePartyDataGridView();
        }

        private void CommitEdit(object sender, EventArgs e)
        {
            //   t.trainerTypeID = trainerTypeNames.Keys.ToArray()[trainerTypeComboBox.SelectedIndex];
            RefreshTextBoxDisplay();
        }

        private void CommitNameEdit(object sender, EventArgs e)
        {
            DeactivateControls();

            //   t.nameLabel = labelToTrainerName.Keys.ToArray()[trainerNameComboBox.SelectedIndex];
            //  t.name = (string)trainerNameComboBox.SelectedItem;
            PopulateListBox();
            RefreshTextBoxDisplay();

            ActivateControls();
        }

        private void ActivateControls()
        {
            sortByComboBox.SelectedIndexChanged += SortChanged;
            listBox.SelectedIndexChanged += TrainerChanged;

            /*  trainerTypeComboBox.SelectedIndexChanged += CommitEdit;
              trainerNameComboBox.SelectedIndexChanged += CommitNameEdit;
              arenaIDNumericUpDown.ValueChanged += CommitEdit;
              effectIDNumericUpDown.ValueChanged += CommitEdit;*/

            partyDataGridView.CellContentClick += ConfigureTP;
        }

        private void DeactivateControls()
        {
            sortByComboBox.SelectedIndexChanged -= SortChanged;
            listBox.SelectedIndexChanged -= TrainerChanged;

            /*        trainerTypeComboBox.SelectedIndexChanged -= CommitEdit;
                    trainerNameComboBox.SelectedIndexChanged -= CommitNameEdit;
                    arenaIDNumericUpDown.ValueChanged -= CommitEdit;
                    effectIDNumericUpDown.ValueChanged -= CommitEdit;*/
            partyDataGridView.CellContentClick -= ConfigureTP;
        }

        private void RefreshTextBoxDisplay()
        {
            //    trainerDisplayTextBox.Text = t.GetID() + " - " + trainerTypeNames[t.trainerTypeID] + " " + t.GetName();
        }

        private void PopulatePartyDataGridView()
        {
            partyDataGridView.Rows.Clear();
            /*  foreach (TrainerPokemon tp in t.trainerPokemon)
             
              {
                  partyDataGridView.Rows.Add(new object[] { gameData.GetTPDisplayName(tp), "Configure" });
              }*/
            partyDataGridView.Rows.Add(new object[] { t.battleTowerPokemonID1, "Configure" });
            partyDataGridView.Rows.Add(new object[] { t.battleTowerPokemonID2, "Configure" });
            partyDataGridView.Rows.Add(new object[] { t.battleTowerPokemonID3, "Configure" });
        }

        private void ConfigureTP(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView senderGrid = (DataGridView)sender;

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

        /*   private void ShowdownButtonClick(object sender, EventArgs e)
           {
               tsef.SetTP(t);
               //tsef.ShowDialog();
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
