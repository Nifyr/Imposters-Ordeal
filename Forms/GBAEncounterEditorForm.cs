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

            if (etef.uint16DexID)
            {
                DataGridViewTextBoxColumn rubyFormIDColumn = new();
                DataGridViewTextBoxColumn sapphireFormIDColumn = new();
                DataGridViewTextBoxColumn emeraldFormIDColumn = new();
                DataGridViewTextBoxColumn fireFormIDColumn = new();
                DataGridViewTextBoxColumn leafFormIDColumn = new();
                rubyFormIDColumn.Name = "FormID";
                sapphireFormIDColumn.Name = "FormID";
                emeraldFormIDColumn.Name = "FormID";
                fireFormIDColumn.Name = "FormID";
                leafFormIDColumn.Name = "FormID";
                rubyFormIDColumn.ValueType = typeof(ushort);
                sapphireFormIDColumn.ValueType = typeof(ushort);
                emeraldFormIDColumn.ValueType = typeof(ushort);
                fireFormIDColumn.ValueType = typeof(ushort);
                leafFormIDColumn.ValueType = typeof(ushort);
                rubyDataGridView.Columns.Add(rubyFormIDColumn);
                sapphireDataGridView.Columns.Add(sapphireFormIDColumn);
                emeraldDataGridView.Columns.Add(emeraldFormIDColumn);
                fireDataGridView.Columns.Add(fireFormIDColumn);
                leafDataGridView.Columns.Add(leafFormIDColumn);
            }

            RefreshDisplay();
            ActivateControls();
        }

        private void RefreshDisplay()
        {
            // Ruby GBA slot
            UpdateTable(rubyDataGridView, etef.encounterTable.gbaRuby);

            // Sapphire GBA slot
            UpdateTable(sapphireDataGridView, etef.encounterTable.gbaSapphire);

            // Emerald GBA slot
            UpdateTable(emeraldDataGridView, etef.encounterTable.gbaEmerald);

            // Fire Red GBA slot
            UpdateTable(fireDataGridView, etef.encounterTable.gbaFire);

            // Leaf Green GBA slot
            UpdateTable(leafDataGridView, etef.encounterTable.gbaLeaf);
        }

        private void UpdateTable(DataGridView dgv, List<Encounter> es)
        {
            dgv.Rows.Clear();
            for (int i = 0; i < es.Count; i++)
            {
                Encounter e = es[i];
                if (etef.uint16DexID)
                    dgv.Rows.Add(new object[] { etef.pokemon[(ushort)e.dexID],
                        e.minLv, e.maxLv, gbaSlotRates[i], (ushort)(e.dexID >> 16) });
                else
                    dgv.Rows.Add(new object[] { etef.pokemon[(ushort)e.dexID],
                        e.minLv, e.maxLv, gbaSlotRates[i] });
            }
        }

        private void CommitEdit(object sender, EventArgs e)
        {
            // Ruby GBA slot
            CommitTable(rubyDataGridView, etef.encounterTable.gbaRuby);

            // Sapphire GBA slot
            CommitTable(sapphireDataGridView, etef.encounterTable.gbaSapphire);

            // Emerald GBA slot
            CommitTable(emeraldDataGridView, etef.encounterTable.gbaEmerald);

            // Fire Red GBA slot
            CommitTable(fireDataGridView, etef.encounterTable.gbaFire);

            // Leaf Green GBA slot
            CommitTable(leafDataGridView, etef.encounterTable.gbaLeaf);
        }

        private void CommitTable(DataGridView dgv, List<Encounter> es)
        {
            for (int i = 0; i < es.Count; i++)
            {
                es[i].dexID = etef.pokemon.IndexOf((string)dgv.Rows[i].Cells[0].Value);
                es[i].minLv = (int)dgv.Rows[i].Cells[1].Value;
                es[i].maxLv = (int)dgv.Rows[i].Cells[2].Value;
                if (etef.uint16DexID)
                    es[i].dexID += (ushort)dgv.Rows[i].Cells[4].Value << 16;
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
