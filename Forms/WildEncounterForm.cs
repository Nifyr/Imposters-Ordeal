using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ImpostersOrdeal.GlobalData;

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
            etef.Show();
            gameData.SetModified(GameDataSet.DataField.EncounterTableFiles);
        }

        private void OpenUndergroundEncounterEditor(object sender, EventArgs e)
        {
            UgEncounterEditorForm ueef = new();
            ueef.Show();
            gameData.SetModified(GameDataSet.DataField.UgAreas);
            gameData.SetModified(GameDataSet.DataField.UgEncounterFiles);
            gameData.SetModified(GameDataSet.DataField.UgEncounterLevelSets);
            gameData.SetModified(GameDataSet.DataField.UgSpecialEncounters);
        }

        private void OpenMiscEncounterEditor(object sender, EventArgs e)
        {
            MiscEncounterEditorForm meef = new();
            meef.Show();
            gameData.SetModified(GameDataSet.DataField.EncounterTableFiles);
        }
    }
}
