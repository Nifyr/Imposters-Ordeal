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
using static ImpostersOrdeal.Masterdatas;
using static ImpostersOrdeal.GlobalData;

namespace ImpostersOrdeal
{
    public partial class PokemonInserterForm : Form
    {
        private List<DexEntry> dexEntries;
        private DexEntry srcDE;
        private DexEntry dstDE;
        private InserterMode inserterMode;

        private enum InserterMode
        {
            Form, Species
        }

        private enum GenderConfig
        {
            None, MaleOnly, FemaleOnly, Normal, Variations, Genderless
        }

        public PokemonInserterForm()
        {
            dexEntries = gameData.dexEntries;
            inserterMode = InserterMode.Form;

            InitializeComponent();

            srcDE = dexEntries[0];
            srcDexIDComboBox.DataSource = dexEntries.Select(o => o.GetName()).ToArray();
            srcDexIDComboBox.SelectedIndex = 0;

            dstDE = dexEntries[0];
            dstDexIDComboBox.DataSource = dexEntries.Select(o => o.GetName()).ToArray();
            dstDexIDComboBox.SelectedIndex = 0;

            newFormRadioButton.Checked = true;
            label2.Text = "Pokémon " + dexEntries.Count + " Name:";

            RefreshSrcDexEntryDisplay();
            RefreshGenderInfoDisplay();
            RefreshDstDexEntryDisplay();
            RefreshModeDisplay();
            ActivateControls();
        }

        private void SrcDexEntryChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            srcDE = dexEntries[srcDexIDComboBox.SelectedIndex];
            RefreshSrcDexEntryDisplay();
            RefreshGenderInfoDisplay();

            ActivateControls();
        }

        private void FormIDChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            RefreshGenderInfoDisplay();

            ActivateControls();
        }

        private void DstDexEntryChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            dstDE = dexEntries[dstDexIDComboBox.SelectedIndex];
            RefreshDstDexEntryDisplay();

            ActivateControls();
        }

        private void ModeChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            inserterMode = newFormRadioButton.Checked ? InserterMode.Form : InserterMode.Species;
            RefreshModeDisplay();

            ActivateControls();
        }

        private void RefreshSrcDexEntryDisplay()
        {
            formIDComboBox.DataSource = srcDE.forms.Select((o, i) => i).ToArray();
            formIDComboBox.SelectedIndex = 0;
        }

        private void RefreshGenderInfoDisplay()
        {
            switch (GetGenderConfig(srcDE.dexID, formIDComboBox.SelectedIndex))
            {
                case GenderConfig.None: genderConfigTextBox.Text = "None"; break;
                case GenderConfig.MaleOnly: genderConfigTextBox.Text = "Male only"; break;
                case GenderConfig.FemaleOnly: genderConfigTextBox.Text = "Female only"; break;
                case GenderConfig.Normal: genderConfigTextBox.Text = "Male/Female"; break;
                case GenderConfig.Variations: genderConfigTextBox.Text = "Male/Female variants"; break;
                case GenderConfig.Genderless: genderConfigTextBox.Text = "Genderless"; break;
            }
        }

        private void RefreshDstDexEntryDisplay()
        {
            label3.Text = inserterMode == InserterMode.Form ? "Form " + dstDE.forms.Count + " Name:" : "Form 0 Name:";
        }

        private void RefreshModeDisplay()
        {
            dstDexIDComboBox.Enabled = inserterMode == InserterMode.Form;
            label5.Enabled = inserterMode == InserterMode.Form;
            speciesNameTextBox.Enabled = inserterMode == InserterMode.Species;
            label2.Enabled = inserterMode == InserterMode.Species;
            label3.Text = inserterMode == InserterMode.Form ? "Form " + dstDE.forms.Count + " Name:" : "Form 0 Name:";
            if (inserterMode == InserterMode.Species)
                formNameTextBox.Text = "";
            else
                formNameTextBox.Text = "New Form";
        }

        private void ActivateControls()
        {
            srcDexIDComboBox.SelectedIndexChanged += SrcDexEntryChanged;
            formIDComboBox.SelectedIndexChanged += FormIDChanged;
            dstDexIDComboBox.SelectedIndexChanged += DstDexEntryChanged;
            newFormRadioButton.CheckedChanged += ModeChanged;
            newSpeciesRadioButton.CheckedChanged += ModeChanged;
        }

        private void DeactivateControls()
        {
            srcDexIDComboBox.SelectedIndexChanged -= SrcDexEntryChanged;
            formIDComboBox.SelectedIndexChanged -= FormIDChanged;
            dstDexIDComboBox.SelectedIndexChanged -= DstDexEntryChanged;
            newFormRadioButton.CheckedChanged -= ModeChanged;
            newSpeciesRadioButton.CheckedChanged -= ModeChanged;
        }

        private void AddFormClick(object sender, EventArgs e)
        {
            if (srcDexIDComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("I have never heard of this species...\n\"" + srcDexIDComboBox.Text + "\"",
                    "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (inserterMode == InserterMode.Form && dstDexIDComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("I have never heard of this species...\n\"" + dstDexIDComboBox.Text + "\"",
                    "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (srcDE.dexID == 0 || inserterMode == InserterMode.Form && dstDE.dexID == 0)
            {
                MessageBox.Show("I'm not gonna insert a damn egg.\nAnd I couldn't even if I wanted to.",
                    "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (inserterMode == InserterMode.Species &&
                MessageBox.Show("Note that expanding the pokédex will require\nadditional exefs changes to function properly.",
                    "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                return;

            if (inserterMode == InserterMode.Form)
            {
                PokemonInserter.GetInstance().InsertPokemon(srcDE.dexID, dstDE.dexID, (int)formIDComboBox.SelectedItem, dstDE.forms.Count, speciesNameTextBox.Text, formNameTextBox.Text);
                MessageBox.Show("Data for " + dstDE.GetName() + " " + formNameTextBox.Text + " has been inserted!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                PokemonInserter.GetInstance().InsertPokemon(srcDE.dexID, dexEntries.Count, (int)formIDComboBox.SelectedItem, 0, speciesNameTextBox.Text, formNameTextBox.Text);
                MessageBox.Show("Data for " + speciesNameTextBox.Text + " has been inserted!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            label2.Text = "Pokémon " + dexEntries.Count + " Name:";
            speciesNameTextBox.Text = "New Species";
            int srcIdx = srcDexIDComboBox.SelectedIndex;
            int dstIdx = dstDexIDComboBox.SelectedIndex;
            srcDexIDComboBox.DataSource = dexEntries.Select(o => o.GetName()).ToArray();
            dstDexIDComboBox.DataSource = dexEntries.Select(o => o.GetName()).ToArray();
            srcDexIDComboBox.SelectedIndex = srcIdx;
            dstDexIDComboBox.SelectedIndex = dstIdx;
        }

        private GenderConfig GetGenderConfig(int dexID, int formID)
        {
            List<PokemonInfoCatalog> pics = gameData.pokemonInfos.Where(pic => pic.MonsNo == dexID && pic.FormNo == formID && !pic.Rare).ToList();
            if (pics.Count == 0)
                return GenderConfig.None;
            if (pics.Count > 2)
                throw new ArgumentException("Too many PokemonInfo entries found for dexID " + dexID + " and formID " + formID);
            if (pics.Count == 1)
                switch (pics.First().Sex)
                {
                    case 0: return GenderConfig.MaleOnly;
                    case 1: return GenderConfig.FemaleOnly;
                    case 2: return GenderConfig.Genderless;
                }
            if (pics[0].AssetBundleName == pics[1].AssetBundleName)
                return GenderConfig.Normal;
            return GenderConfig.Variations;
        }
    }
}
