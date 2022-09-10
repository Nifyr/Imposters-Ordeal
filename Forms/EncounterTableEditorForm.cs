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
    public partial class EncounterTableEditorForm : Form
    {
        public List<string> pokemon;
        public List<string> zoneIDs;
        public List<EncounterTable> encounterTables;
        public EncounterTable encounterTable;
        public GBAEncounterEditorForm gbaeef;
        public bool formIDEnabled;

        public BindingSource pokemonSource = new BindingSource();

        public readonly string[] gameVersions = new string[]
        {
            "Diamond", "Pearl"
        };

        private readonly string[] groundRates = new string[]
        {
            "20%", "20%", "10%", "10%", "10%", "10%", "5%", "5%", "4%", "4%", "1%", "1%"
        };

        private readonly string[] pokeradarRates = new string[]
        {
            "10%", "10%", "1%", "1%"
        };

        private readonly string[] waterRates = new string[]
        {
            "60%", "30%", "5%", "4%", "1%"
        };

        public EncounterTableEditorForm()
        {
            InitializeComponent();

            pokemon = gameData.dexEntries.Select(m => m.GetName()).ToList();
            encounterTables = gameData.encounterTableFiles[0].encounterTables;
            zoneIDs = encounterTables.Select(e => GetZoneName((int)e.zoneID)).ToList();

            versionComboBox.DataSource = gameVersions;
            zoneIDListBox.DataSource = zoneIDs;
            pokemonSource.DataSource = pokemon.ToArray();
            monsNoMorning.DataSource = pokemonSource;
            monsNoDay.DataSource = pokemonSource;
            monsNoNight.DataSource = pokemonSource;
            monsNoPokeradar.DataSource = pokemonSource;
            monsNoSurf.DataSource = pokemonSource;
            monsNoOldRod.DataSource = pokemonSource;
            monsNoGoodRod.DataSource = pokemonSource;
            monsNoSuperRod.DataSource = pokemonSource;

            dataGridView1.Columns[1].ValueType = typeof(int);
            dataGridView1.Columns[2].ValueType = typeof(int);
            dataGridView2.Columns[1].ValueType = typeof(int);
            dataGridView2.Columns[2].ValueType = typeof(int);
            dataGridView3.Columns[1].ValueType = typeof(int);
            dataGridView3.Columns[2].ValueType = typeof(int);
            dataGridView4.Columns[1].ValueType = typeof(int);
            dataGridView4.Columns[2].ValueType = typeof(int);
            dataGridView5.Columns[1].ValueType = typeof(int);
            dataGridView5.Columns[2].ValueType = typeof(int);
            dataGridView6.Columns[1].ValueType = typeof(int);
            dataGridView6.Columns[2].ValueType = typeof(int);
            dataGridView7.Columns[1].ValueType = typeof(int);
            dataGridView7.Columns[2].ValueType = typeof(int);
            dataGridView8.Columns[1].ValueType = typeof(int);
            dataGridView8.Columns[2].ValueType = typeof(int);

            encounterTables[0].groundMons[0].dexID = ushort.MaxValue + 1;
            formIDEnabled = encounterTables.Any(o => o.groundMons.Any(e => (uint)e.dexID > 0xFFFF));
            if (formIDEnabled)
            {
                DataGridViewTextBoxColumn dgvtbc1 = new();
                DataGridViewTextBoxColumn dgvtbc2 = new();
                DataGridViewTextBoxColumn dgvtbc3 = new();
                DataGridViewTextBoxColumn dgvtbc4 = new();
                DataGridViewTextBoxColumn dgvtbc5 = new();
                DataGridViewTextBoxColumn dgvtbc6 = new();
                DataGridViewTextBoxColumn dgvtbc7 = new();
                DataGridViewTextBoxColumn dgvtbc8 = new();
                dgvtbc1.Name = "FormID";
                dgvtbc2.Name = "FormID";
                dgvtbc3.Name = "FormID";
                dgvtbc4.Name = "FormID";
                dgvtbc5.Name = "FormID";
                dgvtbc6.Name = "FormID";
                dgvtbc7.Name = "FormID";
                dgvtbc8.Name = "FormID";
                dgvtbc1.ValueType = typeof(ushort);
                dgvtbc2.ValueType = typeof(ushort);
                dgvtbc3.ValueType = typeof(ushort);
                dgvtbc4.ValueType = typeof(ushort);
                dgvtbc5.ValueType = typeof(ushort);
                dgvtbc6.ValueType = typeof(ushort);
                dgvtbc7.ValueType = typeof(ushort);
                dgvtbc8.ValueType = typeof(ushort);
                dataGridView1.Columns.Add(dgvtbc1);
                dataGridView2.Columns.Add(dgvtbc2);
                dataGridView3.Columns.Add(dgvtbc3);
                dataGridView4.Columns.Add(dgvtbc4);
                dataGridView5.Columns.Add(dgvtbc5);
                dataGridView6.Columns.Add(dgvtbc6);
                dataGridView7.Columns.Add(dgvtbc7);
                dataGridView8.Columns.Add(dgvtbc8);
            }

            dataGridView1.Rows.Add(12);
            dataGridView2.Rows.Add(12);
            dataGridView3.Rows.Add(12);
            dataGridView4.Rows.Add(4);
            dataGridView5.Rows.Add(5);
            dataGridView6.Rows.Add(5);
            dataGridView7.Rows.Add(5);
            dataGridView8.Rows.Add(5);

            dataGridView1.DataError += DataError;
            dataGridView2.DataError += DataError;
            dataGridView3.DataError += DataError;
            dataGridView4.DataError += DataError;
            dataGridView5.DataError += DataError;
            dataGridView6.DataError += DataError;
            dataGridView7.DataError += DataError;
            dataGridView8.DataError += DataError;

            encounterTable = encounterTables[0];

            RefreshDisplay();

            ActivateControls();
        }

        private void RefreshTimeDisplay()
        {
            // Ground Mons
            for (int i = 0; i < 12; i++)
            {
                Encounter encounter = encounterTable.groundMons[i];
                DataGridViewRow iRow;
                if (i != 2 && i != 3)
                {
                    // Morning
                    iRow = dataGridView1.Rows[i];
                    iRow.Cells[0].Value = pokemon[(ushort)encounter.dexID];
                    iRow.Cells[1].Value = encounter.minLv;
                    iRow.Cells[2].Value = encounter.maxLv;
                    iRow.Cells[3].Value = groundRates[i];
                    if (formIDEnabled)
                        iRow.Cells[4].Value = (ushort)(encounter.dexID >> 16);
                    // Day
                    iRow = dataGridView2.Rows[i];
                    iRow.Cells[0].Value = pokemon[(ushort)encounter.dexID];
                    iRow.Cells[1].Value = encounter.minLv;
                    iRow.Cells[2].Value = encounter.maxLv;
                    iRow.Cells[3].Value = groundRates[i];
                    if (formIDEnabled)
                        iRow.Cells[4].Value = (ushort)(encounter.dexID >> 16);
                    // Night
                    iRow = dataGridView3.Rows[i];
                    iRow.Cells[0].Value = pokemon[(ushort)encounter.dexID];
                    iRow.Cells[1].Value = encounter.minLv;
                    iRow.Cells[2].Value = encounter.maxLv;
                    iRow.Cells[3].Value = groundRates[i];
                    if (formIDEnabled)
                        iRow.Cells[4].Value = (ushort)(encounter.dexID >> 16);
                }
                else
                {
                    Encounter morning = encounterTable.tairyo[i - 2];
                    Encounter day = encounterTable.day[i - 2];
                    Encounter night = encounterTable.night[i - 2];
                    // Morning
                    iRow = dataGridView1.Rows[i];
                    iRow.Cells[0].Value = pokemon[(ushort)morning.dexID];
                    iRow.Cells[1].Value = morning.minLv;
                    iRow.Cells[2].Value = morning.maxLv;
                    iRow.Cells[3].Value = groundRates[i];
                    if (formIDEnabled)
                        iRow.Cells[4].Value = (ushort)(morning.dexID >> 16);
                    // Day
                    iRow = dataGridView2.Rows[i];
                    iRow.Cells[0].Value = pokemon[(ushort)day.dexID];
                    iRow.Cells[1].Value = day.minLv;
                    iRow.Cells[2].Value = day.maxLv;
                    iRow.Cells[3].Value = groundRates[i];
                    if (formIDEnabled)
                        iRow.Cells[4].Value = (ushort)(day.dexID >> 16);
                    // Night
                    iRow = dataGridView3.Rows[i];
                    iRow.Cells[0].Value = pokemon[(ushort)night.dexID];
                    iRow.Cells[1].Value = night.minLv;
                    iRow.Cells[2].Value = night.maxLv;
                    iRow.Cells[3].Value = groundRates[i];
                    if (formIDEnabled)
                        iRow.Cells[4].Value = (ushort)(night.dexID >> 16);
                }
            }
        }

        private void RefreshDisplay()
        {
            zoneIDLabel.Text = zoneIDs[zoneIDListBox.SelectedIndex];
            
            RefreshTimeDisplay();

            // Pokeradar Mons
            for (int i = 0; i < encounterTable.swayGrass.Count; i++)
            {
                Encounter encounter = encounterTable.swayGrass[i];
                DataGridViewRow iRow = dataGridView4.Rows[i];
                iRow.Cells[0].Value = pokemon[(ushort)encounter.dexID];
                iRow.Cells[1].Value = encounter.minLv;
                iRow.Cells[2].Value = encounter.maxLv;
                iRow.Cells[3].Value = pokeradarRates[i];
                if (formIDEnabled)
                    iRow.Cells[4].Value = (ushort)(encounter.dexID >> 16);
            }

            // Water Mons
            for (int i = 0; i < encounterTable.waterMons.Count; i++)
            {
                Encounter encounter = encounterTable.waterMons[i];
                DataGridViewRow iRow = dataGridView5.Rows[i];
                iRow.Cells[0].Value = pokemon[(ushort)encounter.dexID];
                iRow.Cells[1].Value = encounter.minLv;
                iRow.Cells[2].Value = encounter.maxLv;
                iRow.Cells[3].Value = waterRates[i];
                if (formIDEnabled)
                    iRow.Cells[4].Value = (ushort)(encounter.dexID >> 16);
            }

            // Old Rod
            for (int i = 0; i < encounterTable.oldRodMons.Count; i++)
            {
                Encounter encounter = encounterTable.oldRodMons[i];
                DataGridViewRow iRow = dataGridView6.Rows[i];
                iRow.Cells[0].Value = pokemon[(ushort)encounter.dexID];
                iRow.Cells[1].Value = encounter.minLv;
                iRow.Cells[2].Value = encounter.maxLv;
                iRow.Cells[3].Value = waterRates[i];
                if (formIDEnabled)
                    iRow.Cells[4].Value = (ushort)(encounter.dexID >> 16);
            }
            
            // Good Rod
            for (int i = 0; i < encounterTable.goodRodMons.Count; i++)
            {
                Encounter encounter = encounterTable.goodRodMons[i];
                DataGridViewRow iRow = dataGridView7.Rows[i];
                iRow.Cells[0].Value = pokemon[(ushort)encounter.dexID];
                iRow.Cells[1].Value = encounter.minLv;
                iRow.Cells[2].Value = encounter.maxLv;
                iRow.Cells[3].Value = waterRates[i];
                if (formIDEnabled)
                    iRow.Cells[4].Value = (ushort)(encounter.dexID >> 16);
            }

            // Super Rod
            for (int i = 0; i < encounterTable.superRodMons.Count; i++)
            {
                Encounter encounter = encounterTable.superRodMons[i];
                DataGridViewRow iRow = dataGridView8.Rows[i];
                iRow.Cells[0].Value = pokemon[(ushort)encounter.dexID];
                iRow.Cells[1].Value = encounter.minLv;
                iRow.Cells[2].Value = encounter.maxLv;
                iRow.Cells[3].Value = waterRates[i];
                if (formIDEnabled)
                    iRow.Cells[4].Value = (ushort)(encounter.dexID >> 16);
            }

            encRateGround.Value = encounterTable.encRateGround;
            encRateWater.Value = encounterTable.encRateWater;
            encRateOldRod.Value = encounterTable.encRateOldRod;
            encRateGoodRod.Value = encounterTable.encRateGoodRod;
            encRateSuperRod.Value = encounterTable.encRateSuperRod;

            formProbNumericUpDown.Value = encounterTable.formProb;
            unownTableNumericUpDown.Value = encounterTable.unownTable;

            if (gbaeef != null)
                gbaeef.ZoneChanged();
        }

        private void CommitEdit(object sender, EventArgs e)
        {
            List<Encounter> swayGrass = new();
            List<Encounter> waterMons = new();
            List<Encounter> oldRodMons = new();
            List<Encounter> goodRodMons = new();
            List<Encounter> superRodMons = new();

            // Pokeradar Mons
            for (int i = 0; i < encounterTable.swayGrass.Count; i++)
            {
                DataGridViewRow iRow = dataGridView4.Rows[i];
                Encounter swayGrassEnc = new();
                swayGrassEnc.dexID = pokemon.IndexOf((string)iRow.Cells[0].Value);
                swayGrassEnc.minLv = (int)iRow.Cells[1].Value;
                swayGrassEnc.maxLv = (int)iRow.Cells[2].Value;
                if (formIDEnabled)
                    swayGrassEnc.dexID += (ushort)iRow.Cells[4].Value << 16;
                swayGrass.Add(swayGrassEnc);
            }

            // Water Mons
            for (int i = 0; i < encounterTable.waterMons.Count; i++)
            {
                DataGridViewRow iRow = dataGridView5.Rows[i];
                Encounter waterMon = new();
                waterMon.dexID = pokemon.IndexOf((string)iRow.Cells[0].Value);
                waterMon.minLv = (int)iRow.Cells[1].Value;
                waterMon.maxLv = (int)iRow.Cells[2].Value;
                if (formIDEnabled)
                    waterMon.dexID += (ushort)iRow.Cells[4].Value << 16;
                waterMons.Add(waterMon);
            }

            // Old Rod
            for (int i = 0; i < encounterTable.oldRodMons.Count; i++)
            {
                DataGridViewRow iRow = dataGridView6.Rows[i];
                Encounter oldRodMon = new();
                oldRodMon.dexID = pokemon.IndexOf((string)iRow.Cells[0].Value);
                oldRodMon.minLv = (int)iRow.Cells[1].Value;
                oldRodMon.maxLv = (int)iRow.Cells[2].Value;
                if (formIDEnabled)
                    oldRodMon.dexID += (ushort)iRow.Cells[4].Value << 16;
                oldRodMons.Add(oldRodMon);
            }

            // Good Rod
            for (int i = 0; i < encounterTable.goodRodMons.Count; i++)
            {
                DataGridViewRow iRow = dataGridView7.Rows[i];
                Encounter goodRodMon = new();
                goodRodMon.dexID = pokemon.IndexOf((string)iRow.Cells[0].Value);
                goodRodMon.minLv = (int)iRow.Cells[1].Value;
                goodRodMon.maxLv = (int)iRow.Cells[2].Value;
                if (formIDEnabled)
                    goodRodMon.dexID += (ushort)iRow.Cells[4].Value << 16;
                goodRodMons.Add(goodRodMon);
            }

            // Super Rod
            for (int i = 0; i < encounterTable.superRodMons.Count; i++)
            {
                DataGridViewRow iRow = dataGridView8.Rows[i];
                Encounter superRodMon = new();
                superRodMon.dexID = pokemon.IndexOf((string)iRow.Cells[0].Value);
                superRodMon.minLv = (int)iRow.Cells[1].Value;
                superRodMon.maxLv = (int)iRow.Cells[2].Value;
                if (formIDEnabled)
                    superRodMon.dexID += (ushort)iRow.Cells[4].Value << 16;
                superRodMons.Add(superRodMon);
            }

            encounterTable.swayGrass = swayGrass;
            encounterTable.waterMons = waterMons;
            encounterTable.oldRodMons = oldRodMons;
            encounterTable.goodRodMons = goodRodMons;
            encounterTable.superRodMons = superRodMons;

            encounterTable.encRateGround = (int) encRateGround.Value;
            encounterTable.encRateWater = (int) encRateWater.Value;
            encounterTable.encRateOldRod = (int) encRateOldRod.Value;
            encounterTable.encRateGoodRod = (int) encRateGoodRod.Value;
            encounterTable.encRateSuperRod = (int) encRateSuperRod.Value;

            encounterTable.formProb = (int)formProbNumericUpDown.Value;
            encounterTable.unownTable = (int)unownTableNumericUpDown.Value;
        }
        private void CommitMorningEdit(object sender, EventArgs e)
        {
            List<Encounter> groundMons = new();
            List<Encounter> morning = new();
            for (int i = 0; i < 12; i++)
            {
                DataGridViewRow iRow = dataGridView1.Rows[i];
                Encounter groundMon = new();
                groundMon.dexID = pokemon.IndexOf((string)iRow.Cells[0].Value);
                groundMon.minLv = (int)iRow.Cells[1].Value;
                groundMon.maxLv = (int)iRow.Cells[2].Value;
                if (formIDEnabled)
                    groundMon.dexID += (ushort)iRow.Cells[4].Value << 16;
                groundMons.Add(groundMon);
                if (i == 2 || i == 3)
                {
                    Encounter morningEnc = new();
                    morningEnc.dexID = pokemon.IndexOf((string)iRow.Cells[0].Value);
                    morningEnc.minLv = (int)iRow.Cells[1].Value;
                    morningEnc.maxLv = (int)iRow.Cells[2].Value;
                    if (formIDEnabled)
                        morningEnc.dexID += (ushort)iRow.Cells[4].Value << 16;
                    morning.Add(morningEnc);
                }
            }

            encounterTable.groundMons = groundMons;
            encounterTable.tairyo = morning;

            DeactivateControls();

            RefreshTimeDisplay();

            ActivateControls();
        }

        private void CommitDayEdit(object sender, EventArgs e)
        {
            List<Encounter> groundMons = new();
            List<Encounter> day = new();
            for (int i = 0; i < 12; i++)
            {
                DataGridViewRow iRow = dataGridView2.Rows[i];
                Encounter groundMon = new();
                groundMon.dexID = pokemon.IndexOf((string)iRow.Cells[0].Value);
                groundMon.minLv = (int)iRow.Cells[1].Value;
                groundMon.maxLv = (int)iRow.Cells[2].Value;
                if (formIDEnabled)
                    groundMon.dexID += (ushort)iRow.Cells[4].Value << 16;
                groundMons.Add(groundMon);
                if (i == 2 || i == 3)
                {
                    Encounter dayEnc = new();
                    dayEnc.dexID = pokemon.IndexOf((string)iRow.Cells[0].Value);
                    dayEnc.minLv = (int)iRow.Cells[1].Value;
                    dayEnc.maxLv = (int)iRow.Cells[2].Value;
                    if (formIDEnabled)
                        dayEnc.dexID += (ushort)iRow.Cells[4].Value << 16;
                    day.Add(groundMon);
                }
            }

            encounterTable.groundMons = groundMons;
            encounterTable.day = day;

            DeactivateControls();

            RefreshTimeDisplay();

            ActivateControls();
        }

        private void CommitNightEdit(object sender, EventArgs e)
        {
            List<Encounter> groundMons = new();
            List<Encounter> night = new();
            for (int i = 0; i < 12; i++)
            {
                DataGridViewRow iRow = dataGridView3.Rows[i];
                Encounter groundMon = new();
                groundMon.dexID = pokemon.IndexOf((string)iRow.Cells[0].Value);
                groundMon.minLv = (int)iRow.Cells[1].Value;
                groundMon.maxLv = (int)iRow.Cells[2].Value;
                if (formIDEnabled)
                    groundMon.dexID += (ushort)iRow.Cells[4].Value << 16;
                groundMons.Add(groundMon);
                if (i == 2 || i == 3)
                {
                    Encounter nightEnc = new();
                    nightEnc.dexID = pokemon.IndexOf((string)iRow.Cells[0].Value);
                    nightEnc.minLv = (int)iRow.Cells[1].Value;
                    nightEnc.maxLv = (int)iRow.Cells[2].Value;
                    if (formIDEnabled)
                        nightEnc.dexID += (ushort)iRow.Cells[4].Value << 16;
                    night.Add(groundMon);
                }
            }

            encounterTable.groundMons = groundMons;
            encounterTable.night = night;

            DeactivateControls();

            RefreshTimeDisplay();

            ActivateControls();
        }

        private void ZoneIDChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            encounterTable = encounterTables[zoneIDListBox.SelectedIndex];
            RefreshDisplay();

            ActivateControls();
        }

        private void VersionChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            encounterTables = gameData.encounterTableFiles[versionComboBox.SelectedIndex].encounterTables;
            zoneIDs = encounterTables.Select(e => e.zoneID.ToString()).ToList();
            encounterTable = encounterTables[zoneIDListBox.SelectedIndex];

            RefreshDisplay();

            ActivateControls();
        }

        private void ActivateControls()
        {
            zoneIDListBox.SelectedIndexChanged += ZoneIDChanged;
            versionComboBox.SelectedIndexChanged += VersionChanged;

            encRateGround.ValueChanged += CommitEdit;
            encRateWater.ValueChanged += CommitEdit;
            encRateOldRod.ValueChanged += CommitEdit;
            encRateGoodRod.ValueChanged += CommitEdit;
            encRateSuperRod.ValueChanged += CommitEdit;

            formProbNumericUpDown.ValueChanged += CommitEdit;
            unownTableNumericUpDown.ValueChanged += CommitEdit;

            dataGridView1.CellEndEdit += CommitMorningEdit;
            dataGridView2.CellEndEdit += CommitDayEdit;
            dataGridView3.CellEndEdit += CommitNightEdit;
            dataGridView4.CellEndEdit += CommitEdit;
            dataGridView5.CellEndEdit += CommitEdit;
            dataGridView6.CellEndEdit += CommitEdit;
            dataGridView7.CellEndEdit += CommitEdit;
            dataGridView8.CellEndEdit += CommitEdit;
        }

        private void DeactivateControls()
        {
            zoneIDListBox.SelectedIndexChanged -= ZoneIDChanged;
            versionComboBox.SelectedIndexChanged -= VersionChanged;

            encRateGround.ValueChanged -= CommitEdit;
            encRateWater.ValueChanged -= CommitEdit;
            encRateOldRod.ValueChanged -= CommitEdit;
            encRateGoodRod.ValueChanged -= CommitEdit;
            encRateSuperRod.ValueChanged -= CommitEdit;

            formProbNumericUpDown.ValueChanged -= CommitEdit;
            unownTableNumericUpDown.ValueChanged -= CommitEdit;

            dataGridView1.CellEndEdit -= CommitMorningEdit;
            dataGridView2.CellEndEdit -= CommitDayEdit;
            dataGridView3.CellEndEdit -= CommitNightEdit;
            dataGridView4.CellEndEdit -= CommitEdit;
            dataGridView5.CellEndEdit -= CommitEdit;
            dataGridView6.CellEndEdit -= CommitEdit;
            dataGridView7.CellEndEdit -= CommitEdit;
            dataGridView8.CellEndEdit -= CommitEdit;
        }

        private void DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MainForm.ShowDataError();
        }

        private void OpenGBAEncounterEditor(object sender, EventArgs e)
        {
            gbaeef ??= new(this);
            gbaeef.Show();
        }
    }
}
