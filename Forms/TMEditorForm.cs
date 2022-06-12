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
    public partial class TMEditorForm : Form
    {
        List<TM> tms;
        List<Item> items;
        TM t;
        Item i;
        public TMEditorForm()
        {
            tms = gameData.tms;
            items = gameData.items;
            InitializeComponent();

            PopulateListBox();
            listBox.SelectedIndex = 0;
            t = tms[0];
            i = items[t.itemID];

            itemComboBox.DataSource = items.Select(i => i.GetName()).ToArray();
            moveComboBox.DataSource = gameData.moves.Select(m => m.GetName()).ToArray();

            RefreshTMDisplay();
            ActivateControls();
        }

        private void TMChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            t = tms[listBox.SelectedIndex];
            i = items[t.itemID];
            RefreshTMDisplay();

            ActivateControls();
        }

        private void RefreshTMDisplay()
        {
            itemComboBox.SelectedIndex = t.itemID;
            moveComboBox.SelectedIndex = t.moveID;
            compatibilityNumericUpDown.Value = i.groupID;
        }

        private void CommitEdit(object sender, EventArgs e)
        {
            DeactivateControls();

            if (t.itemID != itemComboBox.SelectedIndex)
            {
                t.itemID = itemComboBox.SelectedIndex == -1 ? 0 : itemComboBox.SelectedIndex;
                t.name = gameData.items[t.itemID].GetName();
                PopulateListBox();
                i = items[t.itemID];
                RefreshTMDisplay();
            }

            if (t.moveID != moveComboBox.SelectedIndex)
            {
                t.moveID = moveComboBox.SelectedIndex == -1 ? 0 : moveComboBox.SelectedIndex;
                PopulateListBox();
            }

            i.groupID = (byte)compatibilityNumericUpDown.Value;

            ActivateControls();
        }

        private void ActivateControls()
        {
            listBox.SelectedIndexChanged += TMChanged;
            itemComboBox.SelectedIndexChanged += CommitEdit;
            moveComboBox.SelectedIndexChanged += CommitEdit;
            compatibilityNumericUpDown.ValueChanged += CommitEdit;
        }

        private void DeactivateControls()
        {
            listBox.SelectedIndexChanged -= TMChanged;
            itemComboBox.SelectedIndexChanged -= CommitEdit;
            moveComboBox.SelectedIndexChanged -= CommitEdit;
            compatibilityNumericUpDown.ValueChanged -= CommitEdit;
        }

        private void PopulateListBox()
        {
            int i = listBox.SelectedIndex;
            listBox.DataSource = tms.Select(t => t.GetFullName()).ToArray();
            listBox.SelectedIndex = i;
        }
    }
}
