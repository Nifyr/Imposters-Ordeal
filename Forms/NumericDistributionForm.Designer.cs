
namespace BDSP_Randomizer
{
    partial class NumericDistributionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NumericDistributionForm));
            this.distributionSelectComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.argLabel1 = new System.Windows.Forms.Label();
            this.argNumericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.argLabel2 = new System.Windows.Forms.Label();
            this.argNumericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.argNumericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.argLabel3 = new System.Windows.Forms.Label();
            this.descriptionLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.argNumericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.argNumericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.argNumericUpDown3)).BeginInit();
            this.SuspendLayout();
            // 
            // distributionSelectComboBox
            // 
            this.distributionSelectComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.distributionSelectComboBox.FormattingEnabled = true;
            this.distributionSelectComboBox.Location = new System.Drawing.Point(12, 32);
            this.distributionSelectComboBox.Name = "distributionSelectComboBox";
            this.distributionSelectComboBox.Size = new System.Drawing.Size(189, 28);
            this.distributionSelectComboBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Probability Distribution:";
            // 
            // argLabel1
            // 
            this.argLabel1.AutoSize = true;
            this.argLabel1.Location = new System.Drawing.Point(208, 9);
            this.argLabel1.Name = "argLabel1";
            this.argLabel1.Size = new System.Drawing.Size(41, 20);
            this.argLabel1.TabIndex = 2;
            this.argLabel1.Text = "Arg1";
            // 
            // argNumericUpDown1
            // 
            this.argNumericUpDown1.DecimalPlaces = 3;
            this.argNumericUpDown1.Location = new System.Drawing.Point(208, 33);
            this.argNumericUpDown1.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.argNumericUpDown1.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.argNumericUpDown1.Name = "argNumericUpDown1";
            this.argNumericUpDown1.Size = new System.Drawing.Size(189, 27);
            this.argNumericUpDown1.TabIndex = 3;
            // 
            // argLabel2
            // 
            this.argLabel2.AutoSize = true;
            this.argLabel2.Location = new System.Drawing.Point(208, 63);
            this.argLabel2.Name = "argLabel2";
            this.argLabel2.Size = new System.Drawing.Size(41, 20);
            this.argLabel2.TabIndex = 4;
            this.argLabel2.Text = "Arg2";
            // 
            // argNumericUpDown2
            // 
            this.argNumericUpDown2.DecimalPlaces = 3;
            this.argNumericUpDown2.Location = new System.Drawing.Point(208, 86);
            this.argNumericUpDown2.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.argNumericUpDown2.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.argNumericUpDown2.Name = "argNumericUpDown2";
            this.argNumericUpDown2.Size = new System.Drawing.Size(189, 27);
            this.argNumericUpDown2.TabIndex = 5;
            // 
            // argNumericUpDown3
            // 
            this.argNumericUpDown3.DecimalPlaces = 3;
            this.argNumericUpDown3.Location = new System.Drawing.Point(208, 139);
            this.argNumericUpDown3.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.argNumericUpDown3.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.argNumericUpDown3.Name = "argNumericUpDown3";
            this.argNumericUpDown3.Size = new System.Drawing.Size(189, 27);
            this.argNumericUpDown3.TabIndex = 7;
            // 
            // argLabel3
            // 
            this.argLabel3.AutoSize = true;
            this.argLabel3.Location = new System.Drawing.Point(208, 116);
            this.argLabel3.Name = "argLabel3";
            this.argLabel3.Size = new System.Drawing.Size(41, 20);
            this.argLabel3.TabIndex = 6;
            this.argLabel3.Text = "Arg3";
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(12, 63);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(85, 20);
            this.descriptionLabel.TabIndex = 9;
            this.descriptionLabel.Text = "Description";
            // 
            // NumericDistributionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 183);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.argNumericUpDown3);
            this.Controls.Add(this.argLabel3);
            this.Controls.Add(this.argNumericUpDown2);
            this.Controls.Add(this.argLabel2);
            this.Controls.Add(this.argNumericUpDown1);
            this.Controls.Add(this.argLabel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.distributionSelectComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NumericDistributionForm";
            this.Text = "NumericDistributionForm";
            ((System.ComponentModel.ISupportInitialize)(this.argNumericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.argNumericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.argNumericUpDown3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox distributionSelectComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label argLabel1;
        private System.Windows.Forms.NumericUpDown argNumericUpDown1;
        private System.Windows.Forms.Label argLabel2;
        private System.Windows.Forms.NumericUpDown argNumericUpDown2;
        private System.Windows.Forms.NumericUpDown argNumericUpDown3;
        private System.Windows.Forms.Label argLabel3;
        private System.Windows.Forms.Label descriptionLabel;
    }
}