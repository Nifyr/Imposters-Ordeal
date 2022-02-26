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
    /// <summary>
    ///  Display for use while the application is busy with operations.
    /// </summary>
    public partial class LoadingForm : Form
    {
        public LoadingForm(string mainTask, string subTask)
        {
            InitializeComponent();
            mainTaskLabel.Text = mainTask;
            subTaskLabel.Text = subTask;
        }

        /// <summary>
        ///  Updates the subtask label to the specified string.
        /// </summary>
        public void UpdateSubTask(string subTask)
        {
            subTaskLabel.Invoke((MethodInvoker)delegate {
                subTaskLabel.Text = subTask;
            });
        }

        public void Finish()
        {
            subTaskLabel.Invoke((MethodInvoker)delegate {
                this.Close();
            });
        }
    }
}
