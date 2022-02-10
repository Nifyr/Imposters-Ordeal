using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BDSP_Randomizer.Distributions;

namespace BDSP_Randomizer
{
    public partial class ItemDistributionForm : Form
    {
        private MainForm.ItemDistributionControl idc;

        public ItemDistributionForm(MainForm.ItemDistributionControl idc)
        {
            this.idc = idc;

            InitializeComponent();
            Text = idc.Text;
            distributionSelectComboBox.DataSource = itemDistributionNames;
            distributionSelectComboBox.SelectedIndex = idc.idx;
            pNumericUpDown.ValueChanged += CommitEdit;
            RefreshDistributionDisplay();

            distributionSelectComboBox.SelectedIndexChanged += SelectedDistributionChanged;
        }

        private void SelectedDistributionChanged(object sender, EventArgs e)
        {
            idc.idx = ((ComboBox)sender).SelectedIndex;
            RefreshDistributionDisplay();
        }

        /// <summary>
        ///  Fills the form with data from the currently selected distribution.
        /// </summary>
        private void RefreshDistributionDisplay()
        {
            descriptionLabel.Text = itemDistributionDescriptions[idc.idx];

            pNumericUpDown.ValueChanged -= CommitEdit;
            pNumericUpDown.Value = (decimal)idc.Get().GetConfig()[1];
            pNumericUpDown.ValueChanged += CommitEdit;

            DataTable dataTable = new();
            DataColumn[] columns = { new DataColumn("Name"), new DataColumn() };
            columns[0].ReadOnly = true;
            dataTable.Columns.AddRange(columns);
            switch (idc.Get().GetConfig().First())
            {
                case 6:
                    columns[1].ColumnName = "Weight";
                    columns[1].DataType = typeof(int);
                    List<int> intValues = idc.Get().GetConfig().Skip(2).Select(d => (int)d).ToList();
                    for (int i = 0; i < idc.itemNames.Count; i++)
                        dataTable.Rows.Add(new Object[] { idc.itemNames[i], intValues[i] });
                    break;
                case 7:
                    columns[1].ColumnName = "Included";
                    columns[1].DataType = typeof(bool);
                    List<bool> boolValues = idc.Get().GetConfig().Skip(2).Select(d => d == 1).ToList();
                    for (int i = 0; i < idc.itemNames.Count; i++)
                        dataTable.Rows.Add(new Object[] { idc.itemNames[i], boolValues[i] });
                    break;
            }
            dataGridView1.DataSource = dataTable;
        }

        private void DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Yeah, no. That's not gonna fly buster.\nInput some actual valid data please.",
                "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void CommitEdit(object sender, EventArgs e)
        {
            DataGridViewRowCollection dgvrc = dataGridView1.Rows;
            List<Object> data = new();
            for (int row = 0; row < dgvrc.Count; row++)
                data.Add(dgvrc[row].Cells[1].Value);
            List<double> args = new();
            args.Add(idc.Get().GetConfig()[0]);
            args.Add((double)pNumericUpDown.Value);
            switch (args[0])
            {
                case 6:
                    args.AddRange(data.Select(o => (double)(int)o));
                    break;
                case 7:
                    args.AddRange(data.Select(o => (bool)o ? 1.0 : 0.0));
                    break;
            }
            idc.SetCurrent(CreateDistribution(args));
        }

        private void Empty_Click(object sender, EventArgs e)
        {
            if (idc.idx == 0)
                SetAll(0);
            else if (idc.idx == 1)
                SetAll(false);
        }

        private void Fill_Click(object sender, EventArgs e)
        {
            if (idc.idx == 0)
                SetAll(12);
            else if (idc.idx == 1)
                SetAll(true);
        }

        private void SetAll(object o)
        {
            for (int itemID = 0; itemID < dataGridView1.RowCount; itemID++)
                dataGridView1.Rows[itemID].Cells[1].Value = o;
            CommitEdit(null, null);
        }
    }
}
