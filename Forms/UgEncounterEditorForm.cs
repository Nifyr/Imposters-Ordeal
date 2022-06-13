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
        private List<UgEncounterFile> encounterFiles;
        private List<UgEncounterLevelSet> levelSets;
        private List<string> dexEntries;
        private bool versionUnbounded;
        private string[] versionNames;
        private UgEncounterFile ef;

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
            encounterFiles = gameData.ugEncounterFiles;
            levelSets = gameData.ugEncounterLevelSets;
            dexEntries = gameData.dexEntries.Select(d => d.GetName()).ToList();
            versionNames = versions;
            versionUnbounded = encounterFiles.SelectMany(o => o.ugEncounter).Any(e => e.version < 1 || e.version > 3);
            if (versionUnbounded)
            {
                versionNames = new string[256];
                for (int i = 0; i < versionNames.Length; i++)
                    versionNames[i] = i.ToString();
            }

            InitializeComponent();

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

            RefreshEncounterFileDisplay();
            ActivateControls();
        }

        private void EncounterFileChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            ef = encounterFiles[listBox.SelectedIndex];
            RefreshEncounterFileDisplay();

            ActivateControls();
        }

        private void RefreshEncounterFileDisplay()
        {
            pokemonDataGridView.Rows.Clear();
            for (int i = 0; i < ef.ugEncounter.Count; i++)
                pokemonDataGridView.Rows.Add(new object[] { dexEntries[ef.ugEncounter[i].dexID], versionNames[ef.ugEncounter[i].version], requirements[ef.ugEncounter[i].zukanFlag] });
        }

        private void CommitEdit(object sender, EventArgs e)
        {
            for (int i = 0; i < levelSets.Count; i++)
            {
                levelSets[i].minLv = (int)levelSetDataGridView.Rows[i].Cells[1].Value;
                levelSets[i].maxLv = (int)levelSetDataGridView.Rows[i].Cells[2].Value;
            }

            for (int i = 0; i < ef.ugEncounter.Count; i++)
            {
                ef.ugEncounter[i].dexID = dexEntries.IndexOf((string)pokemonDataGridView.Rows[i].Cells[0].Value);
                ef.ugEncounter[i].version = versions.ToList().IndexOf((string)pokemonDataGridView.Rows[i].Cells[1].Value);
                if (ef.ugEncounter[i].version <= 0 && !versionUnbounded)
                    ef.ugEncounter[i].version = 1;
                ef.ugEncounter[i].zukanFlag = requirements.ToList().IndexOf((string)pokemonDataGridView.Rows[i].Cells[2].Value);
                if (ef.ugEncounter[i].zukanFlag <= 0)
                    ef.ugEncounter[i].zukanFlag = 1;
            }
        }

        private void ActivateControls()
        {
            levelSetDataGridView.CellEndEdit += CommitEdit;
            listBox.SelectedIndexChanged += EncounterFileChanged;
            pokemonDataGridView.CellEndEdit += CommitEdit;
        }

        private void DeactivateControls()
        {
            levelSetDataGridView.CellEndEdit -= CommitEdit;
            listBox.SelectedIndexChanged -= EncounterFileChanged;
            pokemonDataGridView.CellEndEdit -= CommitEdit;
        }
    }
}
