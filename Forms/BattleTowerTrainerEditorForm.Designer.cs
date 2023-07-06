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
            ShowdownButton = new System.Windows.Forms.Button();
            pasteButton = new System.Windows.Forms.Button();
            copyButton = new System.Windows.Forms.Button();
            partyDataGridView = new System.Windows.Forms.DataGridView();
            tpDisplayColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            tpButtonColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            button1 = new System.Windows.Forms.Button();
            sortByComboBox = new System.Windows.Forms.ComboBox();
            button2 = new System.Windows.Forms.Button();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)partyDataGridView).BeginInit();
            SuspendLayout();
            // 
            // listBox
            // 
            listBox.FormattingEnabled = true;
            listBox.ItemHeight = 15;
            listBox.Location = new System.Drawing.Point(10, 34);
            listBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            listBox.Name = "listBox";
            listBox.Size = new System.Drawing.Size(166, 304);
            listBox.TabIndex = 1;
            // 
            // trainerDisplayTextBox
            // 
            trainerDisplayTextBox.Location = new System.Drawing.Point(181, 9);
            trainerDisplayTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            trainerDisplayTextBox.Name = "trainerDisplayTextBox";
            trainerDisplayTextBox.ReadOnly = true;
            trainerDisplayTextBox.Size = new System.Drawing.Size(166, 23);
            trainerDisplayTextBox.TabIndex = 0;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(ShowdownButton);
            groupBox4.Controls.Add(pasteButton);
            groupBox4.Controls.Add(copyButton);
            groupBox4.Controls.Add(partyDataGridView);
            groupBox4.Location = new System.Drawing.Point(352, 126);
            groupBox4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            groupBox4.Size = new System.Drawing.Size(336, 264);
            groupBox4.TabIndex = 7;
            groupBox4.TabStop = false;
            groupBox4.Text = "Party";
            // 
            // ShowdownButton
            // 
            ShowdownButton.Location = new System.Drawing.Point(0, 0);
            ShowdownButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            ShowdownButton.Name = "ShowdownButton";
            ShowdownButton.Size = new System.Drawing.Size(66, 17);
            ShowdownButton.TabIndex = 0;
            // 
            // pasteButton
            // 
            pasteButton.Location = new System.Drawing.Point(0, 0);
            pasteButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            pasteButton.Name = "pasteButton";
            pasteButton.Size = new System.Drawing.Size(66, 17);
            pasteButton.TabIndex = 1;
            // 
            // copyButton
            // 
            copyButton.Location = new System.Drawing.Point(0, 0);
            copyButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            copyButton.Name = "copyButton";
            copyButton.Size = new System.Drawing.Size(66, 17);
            copyButton.TabIndex = 2;
            // 
            // partyDataGridView
            // 
            partyDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            partyDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            partyDataGridView.ColumnHeadersVisible = false;
            partyDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { tpDisplayColumn, tpButtonColumn });
            partyDataGridView.Location = new System.Drawing.Point(5, 20);
            partyDataGridView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            partyDataGridView.Name = "partyDataGridView";
            partyDataGridView.RowHeadersWidth = 20;
            partyDataGridView.RowTemplate.Height = 29;
            partyDataGridView.Size = new System.Drawing.Size(326, 214);
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
            // 
            // tpButtonColumn
            // 
            tpButtonColumn.HeaderText = "TPButton";
            tpButtonColumn.MinimumWidth = 6;
            tpButtonColumn.Name = "tpButtonColumn";
            tpButtonColumn.ReadOnly = true;
            tpButtonColumn.Text = "Configure";
            tpButtonColumn.UseColumnTextForButtonValue = true;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(10, 368);
            button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(165, 22);
            button1.TabIndex = 3;
            button1.Text = "Paste Trainer";
            button1.UseVisualStyleBackColor = true;
            // 
            // sortByComboBox
            // 
            sortByComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            sortByComboBox.FormattingEnabled = true;
            sortByComboBox.Location = new System.Drawing.Point(10, 9);
            sortByComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            sortByComboBox.Name = "sortByComboBox";
            sortByComboBox.Size = new System.Drawing.Size(166, 23);
            sortByComboBox.TabIndex = 8;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(10, 342);
            button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(165, 22);
            button2.TabIndex = 2;
            button2.Text = "Copy Trainer";
            button2.UseVisualStyleBackColor = true;
            // 
            // BattleTowerTrainerEditorForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(698, 398);
            Controls.Add(sortByComboBox);
            Controls.Add(button1);
            Controls.Add(button2);
            Controls.Add(groupBox4);
            Controls.Add(trainerDisplayTextBox);
            Controls.Add(listBox);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
        private System.Windows.Forms.DataGridViewTextBoxColumn tpDisplayColumn;
        private System.Windows.Forms.DataGridViewButtonColumn tpButtonColumn;
        private System.Windows.Forms.Button pasteButton;
        private System.Windows.Forms.Button copyButton;
        private System.Windows.Forms.Button ShowdownButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox sortByComboBox;
        private System.Windows.Forms.Button button2;
    }
}