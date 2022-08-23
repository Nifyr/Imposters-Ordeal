
namespace ImpostersOrdeal
{
    partial class UgEncounterEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UgEncounterEditorForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.levelSetDataGridView = new System.Windows.Forms.DataGridView();
            this.progColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.minColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.listBox = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pokemonDataGridView = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.areaTableComboBox = new System.Windows.Forms.ComboBox();
            this.areaComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rareDataGridView = new System.Windows.Forms.DataGridView();
            this.pokemonColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.versionColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.requirementColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.rareIDColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.rareSpeciesColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.rareVersionColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.rareDRateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rarePRateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.levelSetDataGridView)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pokemonDataGridView)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rareDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.levelSetDataGridView);
            this.groupBox1.Location = new System.Drawing.Point(12, 132);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(189, 529);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Levels";
            // 
            // levelSetDataGridView
            // 
            this.levelSetDataGridView.AllowUserToAddRows = false;
            this.levelSetDataGridView.AllowUserToDeleteRows = false;
            this.levelSetDataGridView.AllowUserToResizeRows = false;
            this.levelSetDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.levelSetDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.levelSetDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.progColumn,
            this.minColumn,
            this.maxColumn});
            this.levelSetDataGridView.Location = new System.Drawing.Point(6, 26);
            this.levelSetDataGridView.Name = "levelSetDataGridView";
            this.levelSetDataGridView.RowHeadersVisible = false;
            this.levelSetDataGridView.RowHeadersWidth = 51;
            this.levelSetDataGridView.RowTemplate.Height = 29;
            this.levelSetDataGridView.Size = new System.Drawing.Size(177, 497);
            this.levelSetDataGridView.TabIndex = 0;
            this.levelSetDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DataError);
            // 
            // progColumn
            // 
            this.progColumn.FillWeight = 200F;
            this.progColumn.HeaderText = "Progression";
            this.progColumn.MinimumWidth = 6;
            this.progColumn.Name = "progColumn";
            this.progColumn.ReadOnly = true;
            // 
            // minColumn
            // 
            this.minColumn.HeaderText = "Min";
            this.minColumn.MinimumWidth = 6;
            this.minColumn.Name = "minColumn";
            this.minColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // maxColumn
            // 
            this.maxColumn.HeaderText = "Max";
            this.maxColumn.MinimumWidth = 6;
            this.maxColumn.Name = "maxColumn";
            this.maxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // listBox
            // 
            this.listBox.FormattingEnabled = true;
            this.listBox.ItemHeight = 20;
            this.listBox.Location = new System.Drawing.Point(6, 26);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(189, 304);
            this.listBox.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pokemonDataGridView);
            this.groupBox2.Controls.Add(this.listBox);
            this.groupBox2.Location = new System.Drawing.Point(207, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(579, 336);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Spawn Tables";
            // 
            // pokemonDataGridView
            // 
            this.pokemonDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.pokemonDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.pokemonDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pokemonColumn,
            this.versionColumn,
            this.requirementColumn});
            this.pokemonDataGridView.Location = new System.Drawing.Point(201, 26);
            this.pokemonDataGridView.Name = "pokemonDataGridView";
            this.pokemonDataGridView.RowHeadersWidth = 20;
            this.pokemonDataGridView.RowTemplate.Height = 29;
            this.pokemonDataGridView.Size = new System.Drawing.Size(372, 304);
            this.pokemonDataGridView.TabIndex = 0;
            this.pokemonDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DataError);
            this.pokemonDataGridView.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.PokemonDataGridViewDefaultValuesNeeded);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.areaTableComboBox);
            this.groupBox3.Controls.Add(this.areaComboBox);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(189, 114);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Underground Areas";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Spawn Table";
            // 
            // areaTableComboBox
            // 
            this.areaTableComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.areaTableComboBox.FormattingEnabled = true;
            this.areaTableComboBox.Location = new System.Drawing.Point(6, 80);
            this.areaTableComboBox.Name = "areaTableComboBox";
            this.areaTableComboBox.Size = new System.Drawing.Size(177, 28);
            this.areaTableComboBox.TabIndex = 1;
            // 
            // areaComboBox
            // 
            this.areaComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.areaComboBox.FormattingEnabled = true;
            this.areaComboBox.Location = new System.Drawing.Point(6, 26);
            this.areaComboBox.Name = "areaComboBox";
            this.areaComboBox.Size = new System.Drawing.Size(177, 28);
            this.areaComboBox.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rareDataGridView);
            this.groupBox4.Location = new System.Drawing.Point(207, 354);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(579, 307);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Rare Spawns";
            // 
            // rareDataGridView
            // 
            this.rareDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.rareDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.rareDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.rareIDColumn,
            this.rareSpeciesColumn,
            this.rareVersionColumn,
            this.rareDRateColumn,
            this.rarePRateColumn});
            this.rareDataGridView.Location = new System.Drawing.Point(6, 26);
            this.rareDataGridView.Name = "rareDataGridView";
            this.rareDataGridView.RowHeadersWidth = 20;
            this.rareDataGridView.RowTemplate.Height = 29;
            this.rareDataGridView.Size = new System.Drawing.Size(567, 275);
            this.rareDataGridView.TabIndex = 0;
            this.rareDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DataError);
            this.rareDataGridView.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.RareDataGridViewDefaultValuesNeeded);
            // 
            // pokemonColumn
            // 
            this.pokemonColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.pokemonColumn.HeaderText = "Species";
            this.pokemonColumn.MinimumWidth = 6;
            this.pokemonColumn.Name = "pokemonColumn";
            this.pokemonColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // versionColumn
            // 
            this.versionColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.versionColumn.HeaderText = "Version";
            this.versionColumn.MinimumWidth = 6;
            this.versionColumn.Name = "versionColumn";
            this.versionColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // requirementColumn
            // 
            this.requirementColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.requirementColumn.HeaderText = "Requirement";
            this.requirementColumn.MinimumWidth = 6;
            this.requirementColumn.Name = "requirementColumn";
            this.requirementColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // rareIDColumn
            // 
            this.rareIDColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.rareIDColumn.FillWeight = 200F;
            this.rareIDColumn.HeaderText = "Area";
            this.rareIDColumn.MinimumWidth = 6;
            this.rareIDColumn.Name = "rareIDColumn";
            this.rareIDColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.rareIDColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // rareSpeciesColumn
            // 
            this.rareSpeciesColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.rareSpeciesColumn.HeaderText = "Species";
            this.rareSpeciesColumn.MinimumWidth = 6;
            this.rareSpeciesColumn.Name = "rareSpeciesColumn";
            this.rareSpeciesColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.rareSpeciesColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // rareVersionColumn
            // 
            this.rareVersionColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.rareVersionColumn.HeaderText = "Version";
            this.rareVersionColumn.MinimumWidth = 6;
            this.rareVersionColumn.Name = "rareVersionColumn";
            this.rareVersionColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.rareVersionColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // rareDRateColumn
            // 
            this.rareDRateColumn.HeaderText = "BD %";
            this.rareDRateColumn.MinimumWidth = 6;
            this.rareDRateColumn.Name = "rareDRateColumn";
            // 
            // rarePRateColumn
            // 
            this.rarePRateColumn.HeaderText = "SP %";
            this.rarePRateColumn.MinimumWidth = 6;
            this.rarePRateColumn.Name = "rarePRateColumn";
            // 
            // UgEncounterEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 673);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UgEncounterEditorForm";
            this.Text = "Underground Encounter Editor";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.levelSetDataGridView)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pokemonDataGridView)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rareDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView levelSetDataGridView;
        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView pokemonDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn progColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn minColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maxColumn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView rareDataGridView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox areaTableComboBox;
        private System.Windows.Forms.ComboBox areaComboBox;
        private System.Windows.Forms.DataGridViewComboBoxColumn pokemonColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn versionColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn requirementColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn rareIDColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn rareSpeciesColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn rareVersionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rareDRateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rarePRateColumn;
    }
}