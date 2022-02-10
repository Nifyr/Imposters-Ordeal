
namespace BDSP_Randomizer
{
    partial class ItemDistributionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemDistributionForm));
            this.label1 = new System.Windows.Forms.Label();
            this.distributionSelectComboBox = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.pNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.argLabel1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Mode:";
            // 
            // distributionSelectComboBox
            // 
            this.distributionSelectComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.distributionSelectComboBox.FormattingEnabled = true;
            this.distributionSelectComboBox.Location = new System.Drawing.Point(12, 32);
            this.distributionSelectComboBox.Name = "distributionSelectComboBox";
            this.distributionSelectComboBox.Size = new System.Drawing.Size(189, 28);
            this.distributionSelectComboBox.TabIndex = 2;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ColumnHeadersVisible = false;
            this.dataGridView1.Location = new System.Drawing.Point(207, 32);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridView1.RowTemplate.Height = 29;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.Size = new System.Drawing.Size(189, 609);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.CommitEdit);
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DataError);
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(12, 117);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(85, 20);
            this.descriptionLabel.TabIndex = 10;
            this.descriptionLabel.Text = "Description";
            // 
            // pNumericUpDown
            // 
            this.pNumericUpDown.DecimalPlaces = 3;
            this.pNumericUpDown.Location = new System.Drawing.Point(12, 87);
            this.pNumericUpDown.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.pNumericUpDown.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.pNumericUpDown.Name = "pNumericUpDown";
            this.pNumericUpDown.Size = new System.Drawing.Size(189, 27);
            this.pNumericUpDown.TabIndex = 12;
            // 
            // argLabel1
            // 
            this.argLabel1.AutoSize = true;
            this.argLabel1.Location = new System.Drawing.Point(12, 63);
            this.argLabel1.Name = "argLabel1";
            this.argLabel1.Size = new System.Drawing.Size(186, 20);
            this.argLabel1.TabIndex = 11;
            this.argLabel1.Text = "Randomization Probability";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(111, 577);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 29);
            this.button1.TabIndex = 13;
            this.button1.Text = "Empty";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Empty_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(111, 612);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(90, 29);
            this.button2.TabIndex = 14;
            this.button2.Text = "Fill";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Fill_Click);
            // 
            // ItemDistributionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 653);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pNumericUpDown);
            this.Controls.Add(this.argLabel1);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.distributionSelectComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ItemDistributionForm";
            this.Text = "ItemDistributionForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox distributionSelectComboBox;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.NumericUpDown pNumericUpDown;
        private System.Windows.Forms.Label argLabel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}