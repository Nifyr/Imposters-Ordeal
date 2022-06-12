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
    public partial class MiscEncounterEditorForm : Form
    {
        EncounterTableFile[] encounterTableFiles;
        List<string> dexEntries;
        EncounterTableFile f;

        private readonly string[] gameVersions = new string[]
        {
            "Diamond", "Pearl"
        };

        public MiscEncounterEditorForm()
        {
            encounterTableFiles = gameData.encounterTableFiles;
            dexEntries = gameData.dexEntries.Select(d => d.GetName()).ToList();

            InitializeComponent();

            gameVersionComboBox.DataSource = gameVersions.ToList();
            gameVersionComboBox.SelectedIndex = 0;
            f = encounterTableFiles[0];

            trophyGardenColumn.DataSource = dexEntries.ToArray();
            rateColumn.ValueType = typeof(int);
            normalColumn.DataSource = dexEntries.ToArray();
            rareColumn.DataSource = dexEntries.ToArray();
            superRareColumn.DataSource = dexEntries.ToArray();
            safariColumn.DataSource = dexEntries.ToArray();

            RefreshFileDisplay();
            ActivateControls();
        }

        private void FileChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            f = encounterTableFiles[gameVersionComboBox.SelectedIndex];
            RefreshFileDisplay();

            ActivateControls();
        }

        private void RefreshFileDisplay()
        {
            trophyGardenDataGridView.Rows.Clear();
            for (int i = 0; i < f.trophyGardenMons.Count; i++)
                trophyGardenDataGridView.Rows.Add(new object[] { dexEntries[f.trophyGardenMons[i]] });

            honeyTreeDataGridView.Rows.Clear();
            for (int i = 0; i < f.honeyTreeEnconters.Count; i++)
                honeyTreeDataGridView.Rows.Add(new object[] { f.honeyTreeEnconters[i].rate, dexEntries[f.honeyTreeEnconters[i].normalDexID],
                    dexEntries[f.honeyTreeEnconters[i].rareDexID], dexEntries[f.honeyTreeEnconters[i].superRareDexID] });

            safariDataGridView.Rows.Clear();
            for (int i = 0; i < f.safariMons.Count; i++)
                safariDataGridView.Rows.Add(new object[] { dexEntries[f.safariMons[i]] });
        }

        private void CommitEdit(object sender, EventArgs e)
        {
            for (int i = 0; i < f.trophyGardenMons.Count; i++)
                f.trophyGardenMons[i] = dexEntries.ToList().IndexOf((string)trophyGardenDataGridView.Rows[i].Cells[0].Value);

            for (int i = 0; i < f.honeyTreeEnconters.Count; i++)
            {
                f.honeyTreeEnconters[i].rate = (int)honeyTreeDataGridView.Rows[i].Cells[0].Value;
                f.honeyTreeEnconters[i].normalDexID = dexEntries.ToList().IndexOf((string)honeyTreeDataGridView.Rows[i].Cells[1].Value);
                f.honeyTreeEnconters[i].rareDexID = dexEntries.ToList().IndexOf((string)honeyTreeDataGridView.Rows[i].Cells[2].Value);
                f.honeyTreeEnconters[i].superRareDexID = dexEntries.ToList().IndexOf((string)honeyTreeDataGridView.Rows[i].Cells[3].Value);
            }

            for (int i = 0; i < f.safariMons.Count; i++)
                f.safariMons[i] = dexEntries.ToList().IndexOf((string)safariDataGridView.Rows[i].Cells[0].Value);
        }

        private void ActivateControls()
        {
            gameVersionComboBox.SelectedIndexChanged += FileChanged;
            trophyGardenDataGridView.CellEndEdit += CommitEdit;
            honeyTreeDataGridView.CellEndEdit += CommitEdit;
            safariDataGridView.CellEndEdit += CommitEdit;
        }

        private void DeactivateControls()
        {
            gameVersionComboBox.SelectedIndexChanged -= FileChanged;
            trophyGardenDataGridView.CellEndEdit -= CommitEdit;
            honeyTreeDataGridView.CellEndEdit -= CommitEdit;
            safariDataGridView.CellEndEdit -= CommitEdit;
        }

        private void DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MainForm.ShowDataError();
        }
    }
}
