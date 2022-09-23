using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static ImpostersOrdeal.GameDataTypes;
using static ImpostersOrdeal.GlobalData;

namespace ImpostersOrdeal
{
    public partial class UgEncounterEditorForm : Form
    {
        private List<UgArea> areas;
        private List<UgEncounterFile> encounterFiles;
        private List<UgEncounterLevelSet> levelSets;
        private List<UgSpecialEncounter> specialEncounters;
        private List<string> dexEntries;
        private Mode mode;
        private string[] versionNames;
        private UgEncounterFile ef;
        private UgArea a;

        public enum Mode
        {
            Normal,
            VersionUnbounded,
            Uint16DexID
        }

        private readonly string[] areaNames = new string[]
        {
            "unk0",
            "unk1",
            "Spacious Cave",
            "Grassland Cave",
            "Fountainspring Cave",
            "Rocky Cave",
            "Volcanic Cave",
            "Swampy Cave",
            "Dazzling Cave",
            "Whiteout Cave",
            "Icy Cave",
            "Riverbank Cave",
            "Sandsear Cave",
            "Still-Water Cavern",
            "Sunlit Cavern",
            "Big Bluff Cavern",
            "Stargleam Cavern",
            "Glacial Cavern",
            "Bogsunk Cavern",
            "Typhlo Cavern",
            "unk20"
        };

        private readonly string[] progSteps = new string[]
        {
            "1 Badge",
            "2 Bagdes",
            "3 Bagdes",
            "4 Bagdes",
            "5 Bagdes",
            "6 Bagdes",
            "7 Bagdes",
            "8 Bagdes",
            "Post-game"
        };

        private readonly string[] versions = new string[]
        {
            "",
            "Both",
            "Diamond",
            "Pearl"
        };

        private readonly string[] requirements = new string[]
        {
            "",
            "None",
            "Strength",
            "Defog",
            "7 Badges",
            "Waterfall",
            "Nat Dex"
        };

        public UgEncounterEditorForm()
        {
            areas = gameData.ugAreas;
            encounterFiles = gameData.ugEncounterFiles;
            levelSets = gameData.ugEncounterLevelSets;
            specialEncounters = gameData.ugSpecialEncounters;
            dexEntries = gameData.dexEntries.Select(d => d.GetName()).ToList();
            versionNames = versions;

            InitializeComponent();

            //encounterFiles[0].ugEncounters[0].version = 0;
            //encounterFiles[0].ugEncounters[0].dexID = 0xFFFF + 1;
            mode = Mode.Normal;
            if (gameData.Uint16UgTables())
                mode = Mode.Uint16DexID;
            else if (gameData.UgVersionsUnbounded())
                mode = Mode.VersionUnbounded;

            if (mode == Mode.Uint16DexID)
            {
                DataGridViewTextBoxColumn formColumn = new();
                string formIDColumnName = "FormID";
                formColumn.Name = formIDColumnName;
                formColumn.ValueType = typeof(ushort);
                pokemonDataGridView.Columns.Add(formColumn);
            }

            if (mode == Mode.VersionUnbounded)
            {
                versionNames = new string[256];
                for (int i = 0; i < versionNames.Length; i++)
                    versionNames[i] = i.ToString();
            }

            areaComboBox.DataSource = areas.Select(o => o.id).OrderBy(i => i).Select(i => areaNames[i]).ToArray();
            areaComboBox.SelectedIndex = 0;
            areaTableComboBox.DataSource = encounterFiles.Select(e => e.mName).ToArray();
            a = areas[0];

            minColumn.ValueType = typeof(int);
            maxColumn.ValueType = typeof(int);
            pokemonColumn.DataSource = dexEntries.ToArray();
            versionColumn.DataSource = versionNames.ToList();
            requirementColumn.DataSource = requirements.ToList();

            for (int i = 0; i < levelSets.Count; i++)
                levelSetDataGridView.Rows.Add(new object[] { progSteps[i], levelSets[i].minLv, levelSets[i].maxLv });

            listBox.DataSource = encounterFiles.Select(e => e.mName).ToArray();
            listBox.SelectedIndex = 0;
            ef = encounterFiles[0];

            rareDRateColumn.ValueType = typeof(double);
            rarePRateColumn.ValueType = typeof(double);
            rareIDColumn.DataSource = areaNames.ToList();
            rareSpeciesColumn.DataSource = dexEntries.ToArray();
            rareVersionColumn.DataSource = versionNames.ToList();

            for (int i = 0; i < specialEncounters.Count; i++)
                rareDataGridView.Rows.Add(new object[] { areaNames[specialEncounters[i].id],
                    dexEntries[specialEncounters[i].dexID], versionNames[specialEncounters[i].version],
                    specialEncounters[i].dRate / 10.0, specialEncounters[i].pRate / 10.0});

            RefreshAreaDisplay();
            RefreshEncounterFileDisplay();
            ActivateControls();
        }

        private void AreaChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            a = areas[areaComboBox.SelectedIndex];
            RefreshAreaDisplay();

            ActivateControls();
        }

        private void EncounterFileChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            ef = encounterFiles[listBox.SelectedIndex];
            RefreshEncounterFileDisplay();

            ActivateControls();
        }

        private void RefreshAreaDisplay()
        {
            areaTableComboBox.Text = a.fileName;
        }

        private void RefreshEncounterFileDisplay()
        {
            pokemonDataGridView.Rows.Clear();
            for (int i = 0; i < ef.ugEncounters.Count; i++)
            {
                object[] rowItems = new object[] { dexEntries[(ushort)ef.ugEncounters[i].dexID],
                    versionNames[ef.ugEncounters[i].version], requirements[ef.ugEncounters[i].zukanFlag] };
                if (mode == Mode.Uint16DexID)
                    rowItems = rowItems.Append((ushort)(ef.ugEncounters[i].dexID >> 16)).ToArray();
                pokemonDataGridView.Rows.Add(rowItems);
            }
        }

        private void CommitEdit(object sender, EventArgs e)
        {
            a.fileName = areaTableComboBox.Text;

            for (int i = 0; i < levelSets.Count; i++)
            {
                levelSets[i].minLv = (int)levelSetDataGridView.Rows[i].Cells[1].Value;
                levelSets[i].maxLv = (int)levelSetDataGridView.Rows[i].Cells[2].Value;
            }

            List<UgEncounter> newEncounters = new();
            for (int i = 0; i < pokemonDataGridView.Rows.Count; i++)
            {
                if (pokemonDataGridView.Rows[i].Cells[0].Value == null ||
                    pokemonDataGridView.Rows[i].Cells[1].Value == null ||
                    pokemonDataGridView.Rows[i].Cells[2].Value == null)
                    continue;
                UgEncounter ue = new();
                ue.dexID = dexEntries.IndexOf((string)pokemonDataGridView.Rows[i].Cells[0].Value);
                ue.version = versions.ToList().IndexOf((string)pokemonDataGridView.Rows[i].Cells[1].Value);
                if (ue.version <= 0 && mode != Mode.VersionUnbounded)
                    ue.version = 1;
                ue.zukanFlag = requirements.ToList().IndexOf((string)pokemonDataGridView.Rows[i].Cells[2].Value);
                if (ue.zukanFlag <= 0)
                    ue.zukanFlag = 1;
                if (mode == Mode.Uint16DexID)
                    ue.dexID += (ushort)pokemonDataGridView.Rows[i].Cells[3].Value << 16;
                newEncounters.Add(ue);
            }
            newEncounters.Sort((ue0, ue1) => ue0.dexID.CompareTo(ue1.dexID));
            ef.ugEncounters = newEncounters;

            List<UgSpecialEncounter> newSpecialEncounters = new();
            for (int i = 0; i < rareDataGridView.Rows.Count; i++)
            {
                if (rareDataGridView.Rows[i].Cells[0].Value == null ||
                    rareDataGridView.Rows[i].Cells[0].Value == null ||
                    rareDataGridView.Rows[i].Cells[0].Value == null ||
                    rareDataGridView.Rows[i].Cells[0].Value == null ||
                    rareDataGridView.Rows[i].Cells[0].Value == null)
                    continue;
                UgSpecialEncounter use = new();
                use.id = areaNames.ToList().IndexOf((string)rareDataGridView.Rows[i].Cells[0].Value);
                use.dexID = dexEntries.IndexOf((string)rareDataGridView.Rows[i].Cells[1].Value);
                use.version = versions.ToList().IndexOf((string)rareDataGridView.Rows[i].Cells[2].Value);
                if (use.version <= 0 && mode != Mode.VersionUnbounded)
                    use.version = 1;
                use.dRate = (int)((double)rareDataGridView.Rows[i].Cells[3].Value * 10);
                use.pRate = (int)((double)rareDataGridView.Rows[i].Cells[4].Value * 10);
                newSpecialEncounters.Add(use);
            }
            newSpecialEncounters.Sort((use0, use1) => use0.id.CompareTo(use1.id));
            gameData.ugSpecialEncounters = newSpecialEncounters;
            specialEncounters = newSpecialEncounters;
        }

        private void ActivateControls()
        {
            areaComboBox.SelectedIndexChanged += AreaChanged;
            areaTableComboBox.SelectedIndexChanged += CommitEdit;
            levelSetDataGridView.CellEndEdit += CommitEdit;
            listBox.SelectedIndexChanged += EncounterFileChanged;
            pokemonDataGridView.CellEndEdit += CommitEdit;
            pokemonDataGridView.UserDeletedRow += CommitEdit;
            rareDataGridView.CellEndEdit += CommitEdit;
            rareDataGridView.UserDeletedRow += CommitEdit;
        }

        private void DeactivateControls()
        {
            areaComboBox.SelectedIndexChanged -= AreaChanged;
            areaTableComboBox.SelectedIndexChanged -= CommitEdit;
            levelSetDataGridView.CellEndEdit -= CommitEdit;
            listBox.SelectedIndexChanged -= EncounterFileChanged;
            pokemonDataGridView.CellEndEdit -= CommitEdit;
            pokemonDataGridView.UserDeletedRow -= CommitEdit;
            rareDataGridView.CellEndEdit -= CommitEdit;
            rareDataGridView.UserDeletedRow -= CommitEdit;
        }

        private void DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MainForm.ShowDataError();
        }

        private void PokemonDataGridViewDefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.SetValues(new object[] { dexEntries[0], versionNames[0], requirements[0] });
        }

        private void RareDataGridViewDefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.SetValues(new object[] { areaNames[0], dexEntries[0], versionNames[0], 0.0, 0.0 });
        }
    }
}
