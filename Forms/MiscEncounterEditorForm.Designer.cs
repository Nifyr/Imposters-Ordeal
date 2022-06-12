
namespace ImpostersOrdeal
{
    partial class MiscEncounterEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MiscEncounterEditorForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.trophyGardenDataGridView = new System.Windows.Forms.DataGridView();
            this.trophyGardenColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.gameVersionComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.honeyTreeDataGridView = new System.Windows.Forms.DataGridView();
            this.rateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.normalColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.rareColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.superRareColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.safariDataGridView = new System.Windows.Forms.DataGridView();
            this.safariColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trophyGardenDataGridView)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.honeyTreeDataGridView)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.safariDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.trophyGardenDataGridView);
            this.groupBox1.Location = new System.Drawing.Point(12, 46);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(189, 375);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Trophy Garden";
            // 
            // trophyGardenDataGridView
            // 
            this.trophyGardenDataGridView.AllowUserToAddRows = false;
            this.trophyGardenDataGridView.AllowUserToDeleteRows = false;
            this.trophyGardenDataGridView.AllowUserToResizeColumns = false;
            this.trophyGardenDataGridView.AllowUserToResizeRows = false;
            this.trophyGardenDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.trophyGardenDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.trophyGardenDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.trophyGardenColumn});
            this.trophyGardenDataGridView.Location = new System.Drawing.Point(6, 26);
            this.trophyGardenDataGridView.Name = "trophyGardenDataGridView";
            this.trophyGardenDataGridView.RowHeadersVisible = false;
            this.trophyGardenDataGridView.RowHeadersWidth = 51;
            this.trophyGardenDataGridView.RowTemplate.Height = 29;
            this.trophyGardenDataGridView.Size = new System.Drawing.Size(177, 343);
            this.trophyGardenDataGridView.TabIndex = 0;
            this.trophyGardenDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DataError);
            // 
            // trophyGardenColumn
            // 
            this.trophyGardenColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.trophyGardenColumn.HeaderText = "Pokémon";
            this.trophyGardenColumn.MinimumWidth = 6;
            this.trophyGardenColumn.Name = "trophyGardenColumn";
            // 
            // gameVersionComboBox
            // 
            this.gameVersionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gameVersionComboBox.FormattingEnabled = true;
            this.gameVersionComboBox.Location = new System.Drawing.Point(12, 12);
            this.gameVersionComboBox.Name = "gameVersionComboBox";
            this.gameVersionComboBox.Size = new System.Drawing.Size(189, 28);
            this.gameVersionComboBox.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.honeyTreeDataGridView);
            this.groupBox2.Location = new System.Drawing.Point(207, 46);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(384, 375);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Honey Trees";
            // 
            // honeyTreeDataGridView
            // 
            this.honeyTreeDataGridView.AllowUserToAddRows = false;
            this.honeyTreeDataGridView.AllowUserToDeleteRows = false;
            this.honeyTreeDataGridView.AllowUserToResizeColumns = false;
            this.honeyTreeDataGridView.AllowUserToResizeRows = false;
            this.honeyTreeDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.honeyTreeDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.honeyTreeDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.rateColumn,
            this.normalColumn,
            this.rareColumn,
            this.superRareColumn});
            this.honeyTreeDataGridView.Location = new System.Drawing.Point(6, 26);
            this.honeyTreeDataGridView.Name = "honeyTreeDataGridView";
            this.honeyTreeDataGridView.RowHeadersVisible = false;
            this.honeyTreeDataGridView.RowHeadersWidth = 51;
            this.honeyTreeDataGridView.RowTemplate.Height = 29;
            this.honeyTreeDataGridView.Size = new System.Drawing.Size(372, 343);
            this.honeyTreeDataGridView.TabIndex = 0;
            this.honeyTreeDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DataError);
            // 
            // rateColumn
            // 
            this.rateColumn.HeaderText = "Rate";
            this.rateColumn.MinimumWidth = 6;
            this.rateColumn.Name = "rateColumn";
            // 
            // normalColumn
            // 
            this.normalColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.normalColumn.FillWeight = 200F;
            this.normalColumn.HeaderText = "Normal";
            this.normalColumn.MinimumWidth = 6;
            this.normalColumn.Name = "normalColumn";
            // 
            // rareColumn
            // 
            this.rareColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.rareColumn.FillWeight = 200F;
            this.rareColumn.HeaderText = "Rare";
            this.rareColumn.MinimumWidth = 6;
            this.rareColumn.Name = "rareColumn";
            // 
            // superRareColumn
            // 
            this.superRareColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.superRareColumn.FillWeight = 200F;
            this.superRareColumn.HeaderText = "Super Rare";
            this.superRareColumn.MinimumWidth = 6;
            this.superRareColumn.Name = "superRareColumn";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.safariDataGridView);
            this.groupBox3.Location = new System.Drawing.Point(597, 46);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(189, 375);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "The Great Marsh";
            // 
            // safariDataGridView
            // 
            this.safariDataGridView.AllowUserToAddRows = false;
            this.safariDataGridView.AllowUserToDeleteRows = false;
            this.safariDataGridView.AllowUserToResizeColumns = false;
            this.safariDataGridView.AllowUserToResizeRows = false;
            this.safariDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.safariDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.safariDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.safariColumn});
            this.safariDataGridView.Location = new System.Drawing.Point(6, 26);
            this.safariDataGridView.Name = "safariDataGridView";
            this.safariDataGridView.RowHeadersVisible = false;
            this.safariDataGridView.RowHeadersWidth = 51;
            this.safariDataGridView.RowTemplate.Height = 29;
            this.safariDataGridView.Size = new System.Drawing.Size(177, 343);
            this.safariDataGridView.TabIndex = 0;
            this.safariDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DataError);
            // 
            // safariColumn
            // 
            this.safariColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.safariColumn.HeaderText = "Pokémon";
            this.safariColumn.MinimumWidth = 6;
            this.safariColumn.Name = "safariColumn";
            // 
            // MiscEncounterEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 433);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gameVersionComboBox);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MiscEncounterEditorForm";
            this.Text = "Misc. Encounter Editor";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trophyGardenDataGridView)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.honeyTreeDataGridView)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.safariDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox gameVersionComboBox;
        private System.Windows.Forms.DataGridView trophyGardenDataGridView;
        private System.Windows.Forms.DataGridViewComboBoxColumn trophyGardenColumn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView honeyTreeDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn rateColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn normalColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn rareColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn superRareColumn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView safariDataGridView;
        private System.Windows.Forms.DataGridViewComboBoxColumn safariColumn;
    }
}