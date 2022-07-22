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
        List<DexEntry> dexEntries;
        DexEntry d;

        public PokemonInserterForm()
        {
            dexEntries = gameData.dexEntries;

            InitializeComponent();

            d = dexEntries[0];
            dexIDComboBox.DataSource = dexEntries.Select(o => o.GetName()).ToArray();
            dexIDComboBox.SelectedIndex = 0;

            RefreshDexEntryDisplay();
            ActivateControls();
        }

        private void DexEntryChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            d = dexEntries[dexIDComboBox.SelectedIndex];
            RefreshDexEntryDisplay();

            ActivateControls();
        }

        private void RefreshDexEntryDisplay()
        {
            formIDComboBox.DataSource = d.forms.Select((o, i) => i).ToArray();
            formIDComboBox.SelectedIndex = 0;
        }

        private void ActivateControls()
        {
            dexIDComboBox.SelectedIndexChanged += DexEntryChanged;
        }

        private void DeactivateControls()
        {
            dexIDComboBox.SelectedIndexChanged -= DexEntryChanged;
        }

        private void AddFormClick(object sender, EventArgs e)
        {
            if (d.dexID == 0)
            {
                MessageBox.Show("I'm not gonna insert a damn egg.\nAnd I couldn't even if I wanted to.",
                    "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            /* for testing mass insertion
            for (int i = 1; i <= 151; i++)
                AssetInserter.GetInstance().InsertPokemon(i, i, 0, dexEntries[i].forms.Count, "Ditto");
            */
            AssetInserter.GetInstance().InsertPokemon(d.dexID, d.dexID, (int)formIDComboBox.SelectedItem, d.forms.Count, formNameTextBox.Text);
            DexEntryChanged(null, null);
            MessageBox.Show("Data for " + d.GetName() + " " + formNameTextBox.Text + " has been inserted!",
                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
