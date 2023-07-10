namespace ImpostersOrdeal.Forms
{
    partial class BattleTowerTrainerEditorForm
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
            listBox = new System.Windows.Forms.ListBox();
            trainerDisplayTextBox = new System.Windows.Forms.TextBox();
            groupBox4 = new System.Windows.Forms.GroupBox();
            partyDataGridView = new System.Windows.Forms.DataGridView();
            tpDisplayColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            pokemonSelector = new System.Windows.Forms.DataGridViewComboBoxColumn();
            button1 = new System.Windows.Forms.Button();
            sortByComboBox = new System.Windows.Forms.ComboBox();
            button2 = new System.Windows.Forms.Button();
            Column1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)partyDataGridView).BeginInit();
            SuspendLayout();
            // 
            // listBox
            // 
            listBox.FormattingEnabled = true;
            listBox.ItemHeight = 20;
            listBox.Location = new System.Drawing.Point(12, 45);
            listBox.Name = "listBox";
            listBox.Size = new System.Drawing.Size(189, 404);
            listBox.TabIndex = 1;
            // 
            // trainerDisplayTextBox
            // 
            trainerDisplayTextBox.Location = new System.Drawing.Point(208, 12);
            trainerDisplayTextBox.Name = "trainerDisplayTextBox";
            trainerDisplayTextBox.ReadOnly = true;
            trainerDisplayTextBox.Size = new System.Drawing.Size(189, 27);
            trainerDisplayTextBox.TabIndex = 0;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(partyDataGridView);
            groupBox4.Location = new System.Drawing.Point(207, 45);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new System.Drawing.Size(753, 352);
            groupBox4.TabIndex = 7;
            groupBox4.TabStop = false;
            groupBox4.Text = "Party";
            // 
            // partyDataGridView
            // 
            partyDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            partyDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            partyDataGridView.ColumnHeadersVisible = false;
            partyDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { tpDisplayColumn, pokemonSelector });
            partyDataGridView.Location = new System.Drawing.Point(6, 27);
            partyDataGridView.Name = "partyDataGridView";
            partyDataGridView.RowHeadersWidth = 20;
            partyDataGridView.RowTemplate.Height = 29;
            partyDataGridView.Size = new System.Drawing.Size(741, 285);
            partyDataGridView.TabIndex = 0;
            partyDataGridView.CellContentClick += partyDataGridView_CellContentClick;
            // 
            // tpDisplayColumn
            // 
            tpDisplayColumn.FillWeight = 300F;
            tpDisplayColumn.HeaderText = "TPDisplay";
            tpDisplayColumn.MinimumWidth = 6;
            tpDisplayColumn.Name = "tpDisplayColumn";
            tpDisplayColumn.ReadOnly = true;
            tpDisplayColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // pokemonSelector
            // 
            pokemonSelector.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            pokemonSelector.FillWeight = 200F;
            pokemonSelector.HeaderText = "Column3";
            pokemonSelector.MinimumWidth = 6;
            pokemonSelector.Name = "pokemonSelector";
            pokemonSelector.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(12, 491);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(189, 29);
            button1.TabIndex = 3;
            button1.Text = "Double Battle Trainers";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // sortByComboBox
            // 
            sortByComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            sortByComboBox.FormattingEnabled = true;
            sortByComboBox.Location = new System.Drawing.Point(12, 12);
            sortByComboBox.Name = "sortByComboBox";
            sortByComboBox.Size = new System.Drawing.Size(189, 28);
            sortByComboBox.TabIndex = 8;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(12, 456);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(189, 29);
            button2.TabIndex = 2;
            button2.Text = "Single Battle Trainers";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // Column1
            // 
            Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            Column1.FillWeight = 200F;
            Column1.HeaderText = "Change Pokemon";
            Column1.MinimumWidth = 6;
            Column1.Name = "Column1";
            Column1.Width = 125;
            // 
            // BattleTowerTrainerEditorForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(972, 532);
            Controls.Add(sortByComboBox);
            Controls.Add(button1);
            Controls.Add(button2);
            Controls.Add(groupBox4);
            Controls.Add(trainerDisplayTextBox);
            Controls.Add(listBox);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "BattleTowerTrainerEditorForm";
            Text = "Battle Tower Trainer Editor";
            Load += BattleTowerTrainerEditorForm_Load;
            groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)partyDataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.TextBox trainerDisplayTextBox;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView partyDataGridView;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox sortByComboBox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.DataGridViewButtonColumn tpButtonColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column1;
        private System.Windows.Forms.DataGridViewButtonColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn tpDisplayColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn pokemonSelector;
    }
}