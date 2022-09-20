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
            "20%", "20%", "10%", "10%", "5%", "5%", "4%", "4%", "1%", "1%"
        };

        private readonly string[] swarmRates = new string[]
        {
            "20%", "20%"
        };

        private readonly string[] timeRates = new string[]
        {
            "10%", "10%"
        };

        private readonly string[] pokeradarRates = new string[]
        {
            "10%", "10%", "1%", "1%"
        };

        private readonly string[] waterRates = new string[]
        {
            "60%", "30%", "5%", "4%", "1%"
        };

        private readonly string[] sortNames = new string[]
        {
            "Sort by zoneID",
            "Sort by name",
            "Sort by level"
        };

        private readonly Comparison<EncounterTable>[] sortComparisons = new Comparison<EncounterTable>[]
        {
            (e0, e1) => e0.zoneID.CompareTo(e1.zoneID),
            (e0, e1) => GetZoneName((int)e0.zoneID).CompareTo(GetZoneName((int)e1.zoneID)),
            (e0, e1) => e0.GetAvgLevel().CompareTo(e1.GetAvgLevel())
        };

        public EncounterTableEditorForm()
        {
            InitializeComponent();

            pokemon = gameData.dexEntries.Select(m => m.GetName()).ToList();
            encounterTables = new();
            encounterTables.AddRange(gameData.encounterTableFiles[0].encounterTables);

            sortComboBox.DataSource = sortNames;
            sortComboBox.SelectedIndex = 0;
            encounterTables.Sort(sortComparisons[sortComboBox.SelectedIndex]);

            PopulateListBox();

            versionComboBox.DataSource = gameVersions;
            pokemonSource.DataSource = pokemon.ToArray();
            monsNoGround.DataSource = pokemonSource;
            monsNoSwarm.DataSource = pokemonSource;
            monsNoMorning.DataSource = pokemonSource;
            monsNoDay.DataSource = pokemonSource;
            monsNoNight.DataSource = pokemonSource;
            monsNoPokeradar.DataSource = pokemonSource;
            monsNoSurf.DataSource = pokemonSource;
            monsNoOldRod.DataSource = pokemonSource;
            monsNoGoodRod.DataSource = pokemonSource;
            monsNoSuperRod.DataSource = pokemonSource;

            groundMonsDataGridView.Columns[1].ValueType = typeof(int);
            groundMonsDataGridView.Columns[2].ValueType = typeof(int);
            swarmDataGridView.Columns[1].ValueType = typeof(int);
            swarmDataGridView.Columns[2].ValueType = typeof(int);
            morningDataGridView.Columns[1].ValueType = typeof(int);
            morningDataGridView.Columns[2].ValueType = typeof(int);
            dayDataGridView.Columns[1].ValueType = typeof(int);
            dayDataGridView.Columns[2].ValueType = typeof(int);
            nightDataGridView.Columns[1].ValueType = typeof(int);
            nightDataGridView.Columns[2].ValueType = typeof(int);
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

            //encounterTables[0].groundMons[0].dexID = ushort.MaxValue + 1;
            formIDEnabled = encounterTables.Any(o => o.GetAllTables().Any(l => l.Any(e => (uint)e.dexID > 0xFFFF)));
            if (formIDEnabled)
            {
                DataGridViewTextBoxColumn dgvtbc1 = new();
                DataGridViewTextBoxColumn fcSwarm = new();
                DataGridViewTextBoxColumn fcMorning = new();
                DataGridViewTextBoxColumn fcDay = new();
                DataGridViewTextBoxColumn fcNight = new();
                DataGridViewTextBoxColumn dgvtbc4 = new();
                DataGridViewTextBoxColumn dgvtbc5 = new();
                DataGridViewTextBoxColumn dgvtbc6 = new();
                DataGridViewTextBoxColumn dgvtbc7 = new();
                DataGridViewTextBoxColumn dgvtbc8 = new();
                string formIDColumnName = "FormID";
                dgvtbc1.Name = formIDColumnName;
                fcSwarm.Name = formIDColumnName;
                fcMorning.Name = formIDColumnName;
                fcDay.Name = formIDColumnName;
                fcNight.Name = formIDColumnName;
                dgvtbc4.Name = formIDColumnName;
                dgvtbc5.Name = formIDColumnName;
                dgvtbc6.Name = formIDColumnName;
                dgvtbc7.Name = formIDColumnName;
                dgvtbc8.Name = formIDColumnName;
                dgvtbc1.ValueType = typeof(ushort);
                fcSwarm.ValueType = typeof(ushort);
                fcMorning.ValueType = typeof(ushort);
                fcDay.ValueType = typeof(ushort);
                fcNight.ValueType = typeof(ushort);
                dgvtbc4.ValueType = typeof(ushort);
                dgvtbc5.ValueType = typeof(ushort);
                dgvtbc6.ValueType = typeof(ushort);
                dgvtbc7.ValueType = typeof(ushort);
                dgvtbc8.ValueType = typeof(ushort);
                groundMonsDataGridView.Columns.Add(dgvtbc1);
                swarmDataGridView.Columns.Add(fcSwarm);
                morningDataGridView.Columns.Add(fcMorning);
                dayDataGridView.Columns.Add(fcDay);
                nightDataGridView.Columns.Add(fcNight);
                dataGridView4.Columns.Add(dgvtbc4);
                dataGridView5.Columns.Add(dgvtbc5);
                dataGridView6.Columns.Add(dgvtbc6);
                dataGridView7.Columns.Add(dgvtbc7);
                dataGridView8.Columns.Add(dgvtbc8);
            }

            groundMonsDataGridView.Rows.Add(10);
            swarmDataGridView.Rows.Add(2);
            morningDataGridView.Rows.Add(2);
            dayDataGridView.Rows.Add(2);
            nightDataGridView.Rows.Add(2);
            dataGridView4.Rows.Add(4);
            dataGridView5.Rows.Add(5);
            dataGridView6.Rows.Add(5);
            dataGridView7.Rows.Add(5);
            dataGridView8.Rows.Add(5);

            groundMonsDataGridView.DataError += DataError;
            swarmDataGridView.DataError += DataError;
            morningDataGridView.DataError += DataError;
            dayDataGridView.DataError += DataError;
            nightDataGridView.DataError += DataError;
            dataGridView4.DataError += DataError;
            dataGridView5.DataError += DataError;
            dataGridView6.DataError += DataError;
            dataGridView7.DataError += DataError;
            dataGridView8.DataError += DataError;

            encounterTable = encounterTables[0];

            RefreshDisplay();

            ActivateControls();
        }

        private void RefreshGroundMonsDisplay()
        {
            // Ground Mons
            for (int i = 0; i < 10; i++)
            {
                int index = i < 2 ? i : i + 2;
                Encounter encounter = encounterTable.groundMons[index];
                DataGridViewRow iRow = groundMonsDataGridView.Rows[i];
                iRow.Cells[0].Value = pokemon[(ushort)encounter.dexID];
                iRow.Cells[1].Value = encounter.minLv;
                iRow.Cells[2].Value = encounter.maxLv;
                iRow.Cells[3].Value = groundRates[i];
                if (formIDEnabled)
                    iRow.Cells[4].Value = (ushort)(encounter.dexID >> 16);
            }
        }

        private void RefreshDisplay()
        {
            RefreshGroundMonsDisplay();

            // Swarm
            for (int i = 0; i < encounterTable.tairyo.Count; i++)
            {
                Encounter encounter = encounterTable.tairyo[i];
                DataGridViewRow iRow = swarmDataGridView.Rows[i];
                iRow.Cells[0].Value = pokemon[(ushort)encounter.dexID];
                iRow.Cells[1].Value = encounter.minLv;
                iRow.Cells[2].Value = encounter.maxLv;
                iRow.Cells[3].Value = swarmRates[i];
                if (formIDEnabled)
                    iRow.Cells[4].Value = (ushort)(encounter.dexID >> 16);
            }

            // Morning
            for (int i = 0; i < 2; i++)
            {
                int index = i + 2;
                Encounter encounter = encounterTable.groundMons[index];
                DataGridViewRow iRow = morningDataGridView.Rows[i];
                iRow.Cells[0].Value = pokemon[(ushort)encounter.dexID];
                iRow.Cells[1].Value = encounter.minLv;
                iRow.Cells[2].Value = encounter.maxLv;
                iRow.Cells[3].Value = timeRates[i];
                if (formIDEnabled)
                    iRow.Cells[4].Value = (ushort)(encounter.dexID >> 16);
            }

            // Day
            for (int i = 0; i < encounterTable.day.Count; i++)
            {
                Encounter encounter = encounterTable.day[i];
                DataGridViewRow iRow = dayDataGridView.Rows[i];
                iRow.Cells[0].Value = pokemon[(ushort)encounter.dexID];
                iRow.Cells[1].Value = encounter.minLv;
                iRow.Cells[2].Value = encounter.maxLv;
                iRow.Cells[3].Value = timeRates[i];
                if (formIDEnabled)
                    iRow.Cells[4].Value = (ushort)(encounter.dexID >> 16);
            }

            // Night
            for (int i = 0; i < encounterTable.night.Count; i++)
            {
                Encounter encounter = encounterTable.night[i];
                DataGridViewRow iRow = nightDataGridView.Rows[i];
                iRow.Cells[0].Value = pokemon[(ushort)encounter.dexID];
                iRow.Cells[1].Value = encounter.minLv;
                iRow.Cells[2].Value = encounter.maxLv;
                iRow.Cells[3].Value = timeRates[i];
                if (formIDEnabled)
                    iRow.Cells[4].Value = (ushort)(encounter.dexID >> 16);
            }

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
            CommitGroundAndMorning();

            List<Encounter> swarm = new();
            List<Encounter> swayGrass = new();
            List<Encounter> day = new();
            List<Encounter> night = new();
            List<Encounter> waterMons = new();
            List<Encounter> oldRodMons = new();
            List<Encounter> goodRodMons = new();
            List<Encounter> superRodMons = new();

            // Swarm
            for (int i = 0; i < encounterTable.tairyo.Count; i++)
            {
                DataGridViewRow iRow = swarmDataGridView.Rows[i];
                Encounter enc = new();
                enc.dexID = pokemon.IndexOf((string)iRow.Cells[0].Value);
                enc.minLv = (int)iRow.Cells[1].Value;
                enc.maxLv = (int)iRow.Cells[2].Value;
                if (formIDEnabled)
                    enc.dexID += (ushort)iRow.Cells[4].Value << 16;
                swarm.Add(enc);
            }

            // Day
            for (int i = 0; i < encounterTable.day.Count; i++)
            {
                DataGridViewRow iRow = dayDataGridView.Rows[i];
                Encounter enc = new();
                enc.dexID = pokemon.IndexOf((string)iRow.Cells[0].Value);
                enc.minLv = (int)iRow.Cells[1].Value;
                enc.maxLv = (int)iRow.Cells[2].Value;
                if (formIDEnabled)
                    enc.dexID += (ushort)iRow.Cells[4].Value << 16;
                day.Add(enc);
            }

            // Night
            for (int i = 0; i < encounterTable.night.Count; i++)
            {
                DataGridViewRow iRow = nightDataGridView.Rows[i];
                Encounter enc = new();
                enc.dexID = pokemon.IndexOf((string)iRow.Cells[0].Value);
                enc.minLv = (int)iRow.Cells[1].Value;
                enc.maxLv = (int)iRow.Cells[2].Value;
                if (formIDEnabled)
                    enc.dexID += (ushort)iRow.Cells[4].Value << 16;
                night.Add(enc);
            }

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

            encounterTable.tairyo = swarm;
            encounterTable.day = day;
            encounterTable.night = night;
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

        private void CommitGroundAndMorning()
        {
            Encounter[] groundMons = new Encounter[12];
            for (int i = 0; i < 10; i++)
            {
                int index = i < 2 ? i : i + 2;
                DataGridViewRow iRow = groundMonsDataGridView.Rows[i];
                Encounter groundMon = new();
                groundMon.dexID = pokemon.IndexOf((string)iRow.Cells[0].Value);
                groundMon.minLv = (int)iRow.Cells[1].Value;
                groundMon.maxLv = (int)iRow.Cells[2].Value;
                if (formIDEnabled)
                    groundMon.dexID += (ushort)iRow.Cells[4].Value << 16;
                groundMons[index] = groundMon;
            }

            for (int i = 0; i < 2; i++)
            {
                int index = i + 2;

                
                DataGridViewRow iRow = morningDataGridView.Rows[i];

                Encounter morningEnc = new();
                morningEnc.dexID = pokemon.IndexOf((string)iRow.Cells[0].Value);
                morningEnc.minLv = (int)iRow.Cells[1].Value;
                morningEnc.maxLv = (int)iRow.Cells[2].Value;
                if (formIDEnabled)
                    morningEnc.dexID += (ushort)iRow.Cells[4].Value << 16;

                groundMons[index] = morningEnc;
            }

            encounterTable.groundMons = groundMons.ToList();
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
            encounterTable = encounterTables[zoneIDListBox.SelectedIndex];

            RefreshDisplay();

            ActivateControls();
        }

        private void SortChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            encounterTables.Sort(sortComparisons[sortComboBox.SelectedIndex]);
            PopulateListBox();
            zoneIDListBox.SelectedIndex = encounterTables.IndexOf(encounterTable);

            ActivateControls();
        }

        private void ActivateControls()
        {
            sortComboBox.SelectedIndexChanged += SortChanged;
            zoneIDListBox.SelectedIndexChanged += ZoneIDChanged;
            versionComboBox.SelectedIndexChanged += VersionChanged;

            encRateGround.ValueChanged += CommitEdit;
            encRateWater.ValueChanged += CommitEdit;
            encRateOldRod.ValueChanged += CommitEdit;
            encRateGoodRod.ValueChanged += CommitEdit;
            encRateSuperRod.ValueChanged += CommitEdit;

            formProbNumericUpDown.ValueChanged += CommitEdit;
            unownTableNumericUpDown.ValueChanged += CommitEdit;

            groundMonsDataGridView.CellEndEdit += CommitEdit;
            swarmDataGridView.CellEndEdit += CommitEdit;
            morningDataGridView.CellEndEdit += CommitEdit;
            dayDataGridView.CellEndEdit += CommitEdit;
            nightDataGridView.CellEndEdit += CommitEdit;
            dataGridView4.CellEndEdit += CommitEdit;
            dataGridView5.CellEndEdit += CommitEdit;
            dataGridView6.CellEndEdit += CommitEdit;
            dataGridView7.CellEndEdit += CommitEdit;
            dataGridView8.CellEndEdit += CommitEdit;
        }

        private void DeactivateControls()
        {
            sortComboBox.SelectedIndexChanged -= SortChanged;
            zoneIDListBox.SelectedIndexChanged -= ZoneIDChanged;
            versionComboBox.SelectedIndexChanged -= VersionChanged;

            encRateGround.ValueChanged -= CommitEdit;
            encRateWater.ValueChanged -= CommitEdit;
            encRateOldRod.ValueChanged -= CommitEdit;
            encRateGoodRod.ValueChanged -= CommitEdit;
            encRateSuperRod.ValueChanged -= CommitEdit;

            formProbNumericUpDown.ValueChanged -= CommitEdit;
            unownTableNumericUpDown.ValueChanged -= CommitEdit;

            groundMonsDataGridView.CellEndEdit -= CommitEdit;
            swarmDataGridView.CellEndEdit -= CommitEdit;
            morningDataGridView.CellEndEdit -= CommitEdit;
            dayDataGridView.CellEndEdit -= CommitEdit;
            nightDataGridView.CellEndEdit -= CommitEdit;
            dataGridView4.CellEndEdit -= CommitEdit;
            dataGridView5.CellEndEdit -= CommitEdit;
            dataGridView6.CellEndEdit -= CommitEdit;
            dataGridView7.CellEndEdit -= CommitEdit;
            dataGridView8.CellEndEdit -= CommitEdit;
        }

        private void PopulateListBox()
        {
            int index = zoneIDListBox.SelectedIndex;
            if (index < 0)
                index = 0;
            zoneIDListBox.DataSource = encounterTables.Select(e => GetZoneName((int)e.zoneID)).ToList();
            zoneIDListBox.SelectedIndex = index;
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
