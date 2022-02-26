using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ImpostersOrdeal.Distributions;

namespace ImpostersOrdeal
{
    public partial class NumericDistributionForm : Form
    {
        private MainForm.NumericDistributionControl ndc;
        public NumericDistributionForm(MainForm.NumericDistributionControl ndc)
        {
            this.ndc = ndc;

            InitializeComponent();

            Text = ndc.Text;
            distributionSelectComboBox.DataSource = numericDistributionNames;
            distributionSelectComboBox.SelectedIndex = ndc.idx;
            RefreshDistributionDisplay();

            distributionSelectComboBox.SelectedIndexChanged += SelectedDistributionChanged;
            argNumericUpDown1.ValueChanged += CommitEdit;
            argNumericUpDown2.ValueChanged += CommitEdit;
            argNumericUpDown3.ValueChanged += CommitEdit;
        }

        private void SelectedDistributionChanged(object sender, EventArgs e)
        {
            ndc.idx = ((ComboBox)sender).SelectedIndex;
            RefreshDistributionDisplay();
        }

        /// <summary>
        ///  Fills the form with data from the currently selected distribution.
        /// </summary>
        private void RefreshDistributionDisplay()
        {
            descriptionLabel.Text = numericDistributionDescriptions[ndc.idx];
            List<double> distributionConfig = ndc.Get().GetConfig();
            if (distributionConfig.Count > 1)
            {
                argLabel1.Visible = true;
                argNumericUpDown1.Visible = true;
                argLabel1.Text = numericDistributionArgNames[ndc.idx].Item1;
                argNumericUpDown1.Value = (decimal)distributionConfig[1];
            }
            else
            {
                argLabel1.Visible = false;
                argNumericUpDown1.Visible = false;
            }
            if (distributionConfig.Count > 2)
            {
                argLabel2.Visible = true;
                argNumericUpDown2.Visible = true;
                argLabel2.Text = numericDistributionArgNames[ndc.idx].Item2;
                argNumericUpDown2.Value = (decimal)distributionConfig[2];
            }
            else
            {
                argLabel2.Visible = false;
                argNumericUpDown2.Visible = false;
            }
            if (distributionConfig.Count > 3)
            {
                argLabel3.Visible = true;
                argNumericUpDown3.Visible = true;
                argLabel3.Text = numericDistributionArgNames[ndc.idx].Item3;
                argNumericUpDown3.Value = (decimal)distributionConfig[3];
            }
            else
            {
                argLabel3.Visible = false;
                argNumericUpDown3.Visible = false;
            }
        }

        private void CommitEdit(object sender, EventArgs e)
        {
            List<double> args = new();
            args.Add(ndc.idx);
            args.Add((double)argNumericUpDown1.Value);
            args.Add((double)argNumericUpDown2.Value);
            args.Add((double)argNumericUpDown3.Value);
            ndc.SetCurrent(CreateDistribution(args));
        }
    }
}
