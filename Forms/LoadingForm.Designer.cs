
namespace BDSP_Randomizer
{
    partial class LoadingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadingForm));
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.mainTaskLabel = new System.Windows.Forms.Label();
            this.subTaskLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 52);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(382, 29);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 0;
            // 
            // mainTaskLabel
            // 
            this.mainTaskLabel.AutoSize = true;
            this.mainTaskLabel.Location = new System.Drawing.Point(12, 9);
            this.mainTaskLabel.Name = "mainTaskLabel";
            this.mainTaskLabel.Size = new System.Drawing.Size(73, 20);
            this.mainTaskLabel.TabIndex = 1;
            this.mainTaskLabel.Text = "Main Task";
            // 
            // subTaskLabel
            // 
            this.subTaskLabel.AutoSize = true;
            this.subTaskLabel.Location = new System.Drawing.Point(12, 29);
            this.subTaskLabel.Name = "subTaskLabel";
            this.subTaskLabel.Size = new System.Drawing.Size(66, 20);
            this.subTaskLabel.TabIndex = 2;
            this.subTaskLabel.Text = "Sub-task";
            // 
            // LoadingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 98);
            this.Controls.Add(this.subTaskLabel);
            this.Controls.Add(this.mainTaskLabel);
            this.Controls.Add(this.progressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoadingForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Loading";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label mainTaskLabel;
        private System.Windows.Forms.Label subTaskLabel;
    }
}