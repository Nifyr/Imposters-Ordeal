using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImpostersOrdeal
{
    public partial class WildEncounterForm : Form
    {
        public WildEncounterForm()
        {
            InitializeComponent();
        }

        private void OpenEcounterTableEditor(object sender, EventArgs e)
        {
            EncounterTableEditorForm etef = new();
            etef.ShowDialog();
            GlobalData.gameData.SetModified(GlobalData.GameDataSet.DataField.EncounterTableFiles);
        }

        private void OpenUndergroundEncounterEditor(object sender, EventArgs e)
        {
            UgEncounterEditorForm ueef = new();
            ueef.ShowDialog();
            GlobalData.gameData.SetModified(GlobalData.GameDataSet.DataField.UgEncounterFiles);
            GlobalData.gameData.SetModified(GlobalData.GameDataSet.DataField.UgEncounterLevelSets);
        }

        private void OpenMiscEncounterEditor(object sender, EventArgs e)
        {
            MiscEncounterEditorForm meef = new();
            meef.ShowDialog();
            GlobalData.gameData.SetModified(GlobalData.GameDataSet.DataField.EncounterTableFiles);
        }
    }
}
