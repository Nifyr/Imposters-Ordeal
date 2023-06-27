
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
            dataGridView = new System.Windows.Forms.DataGridView();
            destinationDexIDColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            methodColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            label1 = new System.Windows.Forms.Label();
            formIDComboBox = new System.Windows.Forms.ComboBox();
            label2 = new System.Windows.Forms.Label();
            lvReqNumericUpDown = new System.Windows.Forms.NumericUpDown();
            label3 = new System.Windows.Forms.Label();
            evoParamComboBox = new System.Windows.Forms.ComboBox();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lvReqNumericUpDown).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView
            // 
            dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { destinationDexIDColumn, methodColumn });
            dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridView.Location = new System.Drawing.Point(3, 3);
            dataGridView.Name = "dataGridView";
            dataGridView.RowHeadersWidth = 20;
            dataGridView.RowTemplate.Height = 29;
            dataGridView.Size = new System.Drawing.Size(416, 307);
            dataGridView.TabIndex = 0;
            // 
            // destinationDexIDColumn
            // 
            destinationDexIDColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            destinationDexIDColumn.HeaderText = "Destination";
            destinationDexIDColumn.MinimumWidth = 6;
            destinationDexIDColumn.Name = "destinationDexIDColumn";
            // 
            // methodColumn
            // 
            methodColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            methodColumn.FillWeight = 300F;
            methodColumn.HeaderText = "Method";
            methodColumn.MinimumWidth = 6;
            methodColumn.Name = "methodColumn";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(3, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(123, 20);
            label1.TabIndex = 1;
            label1.Text = "Destination Form";
            // 
            // formIDComboBox
            // 
            formIDComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            formIDComboBox.FormattingEnabled = true;
            formIDComboBox.Location = new System.Drawing.Point(3, 23);
            formIDComboBox.Name = "formIDComboBox";
            formIDComboBox.Size = new System.Drawing.Size(189, 28);
            formIDComboBox.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(3, 54);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(132, 20);
            label2.TabIndex = 3;
            label2.Text = "Level Requirement";
            // 
            // lvReqNumericUpDown
            // 
            lvReqNumericUpDown.Location = new System.Drawing.Point(3, 77);
            lvReqNumericUpDown.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            lvReqNumericUpDown.Name = "lvReqNumericUpDown";
            lvReqNumericUpDown.Size = new System.Drawing.Size(189, 27);
            lvReqNumericUpDown.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(3, 107);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(50, 20);
            label3.TabIndex = 5;
            label3.Text = "Param";
            // 
            // evoParamComboBox
            // 
            evoParamComboBox.FormattingEnabled = true;
            evoParamComboBox.Location = new System.Drawing.Point(3, 130);
            evoParamComboBox.Name = "evoParamComboBox";
            evoParamComboBox.Size = new System.Drawing.Size(189, 28);
            evoParamComboBox.TabIndex = 6;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            tableLayoutPanel1.Controls.Add(dataGridView, 0, 0);
            tableLayoutPanel1.Controls.Add(panel1, 1, 0);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new System.Drawing.Size(622, 313);
            tableLayoutPanel1.TabIndex = 7;
            // 
            // panel1
            // 
            panel1.Controls.Add(label1);
            panel1.Controls.Add(evoParamComboBox);
            panel1.Controls.Add(formIDComboBox);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(lvReqNumericUpDown);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(425, 3);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(194, 307);
            panel1.TabIndex = 1;
            // 
            // EvolutionEditorForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(622, 313);
            Controls.Add(tableLayoutPanel1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EvolutionEditorForm";
            Text = "EvolutionEditorForm";
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)lvReqNumericUpDown).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
    }
}