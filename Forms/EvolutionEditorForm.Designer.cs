
namespace ImpostersOrdeal
{
    partial class EvolutionEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EvolutionEditorForm));
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.destinationDexIDColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.methodColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.formIDComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lvReqNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.evoParamComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lvReqNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.destinationDexIDColumn,
            this.methodColumn});
            this.dataGridView.Location = new System.Drawing.Point(12, 12);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersWidth = 20;
            this.dataGridView.RowTemplate.Height = 29;
            this.dataGridView.Size = new System.Drawing.Size(403, 289);
            this.dataGridView.TabIndex = 0;
            // 
            // destinationDexIDColumn
            // 
            this.destinationDexIDColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.destinationDexIDColumn.HeaderText = "Destination";
            this.destinationDexIDColumn.MinimumWidth = 6;
            this.destinationDexIDColumn.Name = "destinationDexIDColumn";
            // 
            // methodColumn
            // 
            this.methodColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.methodColumn.FillWeight = 300F;
            this.methodColumn.HeaderText = "Method";
            this.methodColumn.MinimumWidth = 6;
            this.methodColumn.Name = "methodColumn";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(421, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Destination Form";
            // 
            // formIDComboBox
            // 
            this.formIDComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.formIDComboBox.FormattingEnabled = true;
            this.formIDComboBox.Location = new System.Drawing.Point(421, 32);
            this.formIDComboBox.Name = "formIDComboBox";
            this.formIDComboBox.Size = new System.Drawing.Size(189, 28);
            this.formIDComboBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(421, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Level Requirement";
            // 
            // lvReqNumericUpDown
            // 
            this.lvReqNumericUpDown.Location = new System.Drawing.Point(421, 86);
            this.lvReqNumericUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.lvReqNumericUpDown.Name = "lvReqNumericUpDown";
            this.lvReqNumericUpDown.Size = new System.Drawing.Size(189, 27);
            this.lvReqNumericUpDown.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(421, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Param";
            // 
            // evoParamComboBox
            // 
            this.evoParamComboBox.FormattingEnabled = true;
            this.evoParamComboBox.Location = new System.Drawing.Point(421, 139);
            this.evoParamComboBox.Name = "evoParamComboBox";
            this.evoParamComboBox.Size = new System.Drawing.Size(189, 28);
            this.evoParamComboBox.TabIndex = 6;
            // 
            // EvolutionEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 313);
            this.Controls.Add(this.evoParamComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lvReqNumericUpDown);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.formIDComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EvolutionEditorForm";
            this.Text = "EvolutionEditorForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lvReqNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox formIDComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown lvReqNumericUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox evoParamComboBox;
        private System.Windows.Forms.DataGridViewComboBoxColumn destinationDexIDColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn methodColumn;
    }
}