using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BDSP_Randomizer
{
    public partial class FileSelectForm : Form
    {
        private int[] fileIndexes;
        private string[] fileNames;
        private List<int> overwrites;

        public FileSelectForm(List<(int, string)> conflicts, List<int> overwrites)
        {
            fileIndexes = conflicts.Select(c => c.Item1).ToArray();
            fileNames = conflicts.Select(c => c.Item2).ToArray();
            this.overwrites = overwrites;
            InitializeComponent();

            checkedListBox1.Items.Clear();
            checkedListBox1.Items.AddRange(fileNames);
        }

        private void Confirm(object sender, EventArgs e)
        {
            Close();
        }

        private void OnClose(object sender, FormClosingEventArgs e)
        {
            for (int i = 0; i < fileNames.Length; i++)
                if (checkedListBox1.GetItemChecked(i))
                    overwrites.Add(fileIndexes[i]);
        }
    }
}
