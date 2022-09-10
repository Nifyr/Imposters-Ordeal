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
    public partial class PokemonInserterForm : Form
    {
        private List<DexEntry> dexEntries;
        private DexEntry d;
        private InserterMode inserterMode;

        private enum InserterMode
        {
            Form, Species
        }

        public PokemonInserterForm()
        {
            dexEntries = gameData.dexEntries;
            inserterMode = InserterMode.Form;

            InitializeComponent();

            d = dexEntries[0];
            dexIDComboBox.DataSource = dexEntries.Select(o => o.GetName()).ToArray();
            dexIDComboBox.SelectedIndex = 0;

            newFormRadioButton.Checked = true;
            label2.Text = "Pokémon " + dexEntries.Count + " Name:";

            RefreshDexEntryDisplay();
            RefreshModeDisplay();
            ActivateControls();
        }

        private void DexEntryChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            d = dexEntries[dexIDComboBox.SelectedIndex];
            RefreshDexEntryDisplay();

            ActivateControls();
        }

        private void ModeChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            inserterMode = newFormRadioButton.Checked ? InserterMode.Form : InserterMode.Species;
            RefreshModeDisplay();

            ActivateControls();
        }

        private void RefreshDexEntryDisplay()
        {
            formIDComboBox.DataSource = d.forms.Select((o, i) => i).ToArray();
            formIDComboBox.SelectedIndex = 0;
            label3.Text = inserterMode == InserterMode.Form ? "Form " + d.forms.Count + " Name:" : "Form 0 Name:";
        }

        private void RefreshModeDisplay()
        {
            speciesNameTextBox.Enabled = inserterMode == InserterMode.Species;
            label2.Enabled = inserterMode == InserterMode.Species;
            label3.Text = inserterMode == InserterMode.Form ? "Form " + d.forms.Count + " Name:" : "Form 0 Name:";
            if (inserterMode == InserterMode.Species)
                formNameTextBox.Text = "";
            else
                formNameTextBox.Text = "New Form";
        }

        private void ActivateControls()
        {
            dexIDComboBox.SelectedIndexChanged += DexEntryChanged;
            newFormRadioButton.CheckedChanged += ModeChanged;
            newSpeciesRadioButton.CheckedChanged += ModeChanged;
        }

        private void DeactivateControls()
        {
            dexIDComboBox.SelectedIndexChanged -= DexEntryChanged;
            newFormRadioButton.CheckedChanged -= ModeChanged;
            newSpeciesRadioButton.CheckedChanged -= ModeChanged;
        }

        private void AddFormClick(object sender, EventArgs e)
        {
            if (d.dexID == 0)
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
                PokemonInserter.GetInstance().InsertPokemon(d.dexID, d.dexID, (int)formIDComboBox.SelectedItem, d.forms.Count, speciesNameTextBox.Text, formNameTextBox.Text);
                MessageBox.Show("Data for " + d.GetName() + " " + formNameTextBox.Text + " has been inserted!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                PokemonInserter.GetInstance().InsertPokemon(d.dexID, dexEntries.Count, (int)formIDComboBox.SelectedItem, 0, speciesNameTextBox.Text, formNameTextBox.Text);
                MessageBox.Show("Data for " + speciesNameTextBox.Text + " has been inserted!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            label2.Text = "Pokémon " + dexEntries.Count + " Name:";
            speciesNameTextBox.Text = "New Species";
            dexIDComboBox.DataSource = dexEntries.Select(o => o.GetName()).ToArray();
        }
    }
}
