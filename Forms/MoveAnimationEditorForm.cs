using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BDSP_Randomizer.GameDataTypes;
using static BDSP_Randomizer.GlobalData;

namespace BDSP_Randomizer
{
    public partial class MoveAnimationEditorForm : Form
    {
        private readonly Move m;
        public MoveAnimationEditorForm(Move m, List<string> moveSequences)
        {
            this.m = m;
            InitializeComponent();
            Text = "Move Animation Editor: " + m.GetName();

            comboBox1.DataSource = moveSequences.ToArray();
            comboBox1.SelectedItem = m.cmdSeqName;
            comboBox2.DataSource = moveSequences.ToArray();
            comboBox2.SelectedItem = m.cmdSeqNameLegend;
            comboBox3.DataSource = moveSequences.ToArray();
            comboBox3.SelectedItem = m.notShortenTurnType0;
            comboBox4.DataSource = moveSequences.ToArray();
            comboBox4.SelectedItem = m.notShortenTurnType1;
            comboBox5.DataSource = moveSequences.ToArray();
            comboBox5.SelectedItem = m.turnType1;
            comboBox6.DataSource = moveSequences.ToArray();
            comboBox6.SelectedItem = m.turnType2;
            comboBox7.DataSource = moveSequences.ToArray();
            comboBox7.SelectedItem = m.turnType3;
            comboBox8.DataSource = moveSequences.ToArray();
            comboBox8.SelectedItem = m.turnType4;

            ActivateControls();
        }

        private void CommitEdit(object sender, EventArgs e)
        {
            m.cmdSeqName = (string)comboBox1.SelectedItem;
            m.cmdSeqNameLegend = (string)comboBox2.SelectedItem;
            m.notShortenTurnType0 = (string)comboBox3.SelectedItem;
            m.notShortenTurnType1 = (string)comboBox4.SelectedItem;
            m.turnType1 = (string)comboBox5.SelectedItem;
            m.turnType2 = (string)comboBox6.SelectedItem;
            m.turnType3 = (string)comboBox7.SelectedItem;
            m.turnType4 = (string)comboBox8.SelectedItem;
        }

        private void ActivateControls()
        {
            comboBox1.SelectedIndexChanged += CommitEdit;
            comboBox2.SelectedIndexChanged += CommitEdit;
            comboBox3.SelectedIndexChanged += CommitEdit;
            comboBox4.SelectedIndexChanged += CommitEdit;
            comboBox5.SelectedIndexChanged += CommitEdit;
            comboBox6.SelectedIndexChanged += CommitEdit;
            comboBox7.SelectedIndexChanged += CommitEdit;
            comboBox8.SelectedIndexChanged += CommitEdit;
        }

        private void DeactivateControls()
        {
            comboBox1.SelectedIndexChanged -= CommitEdit;
            comboBox2.SelectedIndexChanged -= CommitEdit;
            comboBox3.SelectedIndexChanged -= CommitEdit;
            comboBox4.SelectedIndexChanged -= CommitEdit;
            comboBox5.SelectedIndexChanged -= CommitEdit;
            comboBox6.SelectedIndexChanged -= CommitEdit;
            comboBox7.SelectedIndexChanged -= CommitEdit;
            comboBox8.SelectedIndexChanged -= CommitEdit;
        }
    }
}
