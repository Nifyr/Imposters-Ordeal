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

namespace ImpostersOrdeal
{
    public partial class GBAEncounterEditorForm : Form
    {
        private readonly EncounterTableEditorForm etef;

        private readonly string[] gbaSlotRates = new string[]
        {
            "4%", "4%"
        };

        public GBAEncounterEditorForm(EncounterTableEditorForm etef)
        {
            this.etef = etef;
            InitializeComponent();

            rubyDexIDColumn.DataSource = etef.pokemon.ToArray();
            sapphireDexIDColumn.DataSource = etef.pokemon.ToArray();
            emeraldDexIDColumn.DataSource = etef.pokemon.ToArray();
            fireDexIDColumn.DataSource = etef.pokemon.ToArray();
            leafDexIDColumn.DataSource = etef.pokemon.ToArray();
            rubyMinLvlColumn.ValueType = typeof(int);
            rubyMaxLvlColumn.ValueType = typeof(int);
            sapphireMinLvlColumn.ValueType = typeof(int);
            sapphireMaxLvlColumn.ValueType = typeof(int);
            emeraldMinLvlColumn.ValueType = typeof(int);
            emeraldMaxLvlColumn.ValueType = typeof(int);
            fireMinLvlColumn.ValueType = typeof(int);
            fireMaxLvlColumn.ValueType = typeof(int);
            leafMinLvlColumn.ValueType = typeof(int);
            leafMaxLvlColumn.ValueType = typeof(int);

            RefreshDisplay();
            ActivateControls();
        }

        private void RefreshDisplay()
        {
            // Ruby GBA slot
            rubyDataGridView.Rows.Clear();
            for (int i = 0; i < etef.encounterTable.gbaRuby.Count; i++)
            {
                Encounter encounter = etef.encounterTable.gbaRuby[i];
                rubyDataGridView.Rows.Add(new object[] { etef.pokemon[encounter.dexID],
                    encounter.minLv, encounter.maxLv, gbaSlotRates[i] });
            }

            // Sapphire GBA slot
            sapphireDataGridView.Rows.Clear();
            for (int i = 0; i < etef.encounterTable.gbaSapphire.Count; i++)
            {
                Encounter encounter = etef.encounterTable.gbaSapphire[i];
                sapphireDataGridView.Rows.Add(new object[] { etef.pokemon[encounter.dexID],
                    encounter.minLv, encounter.maxLv, gbaSlotRates[i] });
            }

            // Emerald GBA slot
            emeraldDataGridView.Rows.Clear();
            for (int i = 0; i < etef.encounterTable.gbaEmerald.Count; i++)
            {
                Encounter encounter = etef.encounterTable.gbaEmerald[i];
                emeraldDataGridView.Rows.Add(new object[] { etef.pokemon[encounter.dexID],
                    encounter.minLv, encounter.maxLv, gbaSlotRates[i] });
            }

            // Fire Red GBA slot
            fireDataGridView.Rows.Clear();
            for (int i = 0; i < etef.encounterTable.gbaFire.Count; i++)
            {
                Encounter encounter = etef.encounterTable.gbaFire[i];
                fireDataGridView.Rows.Add(new object[] { etef.pokemon[encounter.dexID],
                    encounter.minLv, encounter.maxLv, gbaSlotRates[i] });
            }

            // Leaf Green GBA slot
            leafDataGridView.Rows.Clear();
            for (int i = 0; i < etef.encounterTable.gbaLeaf.Count; i++)
            {
                Encounter encounter = etef.encounterTable.gbaLeaf[i];
                leafDataGridView.Rows.Add(new object[] { etef.pokemon[encounter.dexID],
                    encounter.minLv, encounter.maxLv, gbaSlotRates[i] });
            }
        }

        private void CommitEdit(object sender, EventArgs e)
        {
            // Ruby GBA slot
            for (int i = 0; i < etef.encounterTable.gbaRuby.Count; i++)
            {
                etef.encounterTable.gbaRuby[i].dexID = etef.pokemon.IndexOf((string)rubyDataGridView.Rows[i].Cells[0].Value);
                etef.encounterTable.gbaRuby[i].minLv = (int)rubyDataGridView.Rows[i].Cells[1].Value;
                etef.encounterTable.gbaRuby[i].maxLv = (int)rubyDataGridView.Rows[i].Cells[2].Value;
            }

            // Sapphire GBA slot
            for (int i = 0; i < etef.encounterTable.gbaSapphire.Count; i++)
            {
                etef.encounterTable.gbaSapphire[i].dexID = etef.pokemon.IndexOf((string)sapphireDataGridView.Rows[i].Cells[0].Value);
                etef.encounterTable.gbaSapphire[i].minLv = (int)sapphireDataGridView.Rows[i].Cells[1].Value;
                etef.encounterTable.gbaSapphire[i].maxLv = (int)sapphireDataGridView.Rows[i].Cells[2].Value;
            }

            // Emerald GBA slot
            for (int i = 0; i < etef.encounterTable.gbaEmerald.Count; i++)
            {
                etef.encounterTable.gbaEmerald[i].dexID = etef.pokemon.IndexOf((string)emeraldDataGridView.Rows[i].Cells[0].Value);
                etef.encounterTable.gbaEmerald[i].minLv = (int)emeraldDataGridView.Rows[i].Cells[1].Value;
                etef.encounterTable.gbaEmerald[i].maxLv = (int)emeraldDataGridView.Rows[i].Cells[2].Value;
            }

            // Fire Red GBA slot
            for (int i = 0; i < etef.encounterTable.gbaFire.Count; i++)
            {
                etef.encounterTable.gbaFire[i].dexID = etef.pokemon.IndexOf((string)fireDataGridView.Rows[i].Cells[0].Value);
                etef.encounterTable.gbaFire[i].minLv = (int)fireDataGridView.Rows[i].Cells[1].Value;
                etef.encounterTable.gbaFire[i].maxLv = (int)fireDataGridView.Rows[i].Cells[2].Value;
            }

            // Leaf Green GBA slot
            for (int i = 0; i < etef.encounterTable.gbaLeaf.Count; i++)
            {
                etef.encounterTable.gbaLeaf[i].dexID = etef.pokemon.IndexOf((string)leafDataGridView.Rows[i].Cells[0].Value);
                etef.encounterTable.gbaLeaf[i].minLv = (int)leafDataGridView.Rows[i].Cells[1].Value;
                etef.encounterTable.gbaLeaf[i].maxLv = (int)leafDataGridView.Rows[i].Cells[2].Value;
            }
        }

        private void ActivateControls()
        {
            rubyDataGridView.CellEndEdit += CommitEdit;
            sapphireDataGridView.CellEndEdit += CommitEdit;
            emeraldDataGridView.CellEndEdit += CommitEdit;
            fireDataGridView.CellEndEdit += CommitEdit;
            leafDataGridView.CellEndEdit += CommitEdit;
        }

        private void DeactivateControls()
        {
            rubyDataGridView.CellEndEdit -= CommitEdit;
            sapphireDataGridView.CellEndEdit -= CommitEdit;
            emeraldDataGridView.CellEndEdit -= CommitEdit;
            fireDataGridView.CellEndEdit -= CommitEdit;
            leafDataGridView.CellEndEdit -= CommitEdit;
        }

        public void ZoneChanged()
        {
            DeactivateControls();
            RefreshDisplay();
            ActivateControls();
        }

        private void DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MainForm.ShowDataError();
        }

        private void GBAEncounterEditorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            etef.gbaeef = null;
        }
    }
}
