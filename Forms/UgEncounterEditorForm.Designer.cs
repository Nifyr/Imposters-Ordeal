
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
            this.pokemonColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.versionColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.requirementColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.levelSetDataGridView)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pokemonDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.levelSetDataGridView);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(189, 404);
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
            this.levelSetDataGridView.Size = new System.Drawing.Size(177, 372);
            this.levelSetDataGridView.TabIndex = 0;
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
            this.listBox.Location = new System.Drawing.Point(207, 12);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(189, 404);
            this.listBox.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pokemonDataGridView);
            this.groupBox2.Location = new System.Drawing.Point(402, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(384, 404);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Pokémon";
            // 
            // pokemonDataGridView
            // 
            this.pokemonDataGridView.AllowUserToAddRows = false;
            this.pokemonDataGridView.AllowUserToDeleteRows = false;
            this.pokemonDataGridView.AllowUserToResizeColumns = false;
            this.pokemonDataGridView.AllowUserToResizeRows = false;
            this.pokemonDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.pokemonDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.pokemonDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pokemonColumn,
            this.versionColumn,
            this.requirementColumn});
            this.pokemonDataGridView.Location = new System.Drawing.Point(6, 26);
            this.pokemonDataGridView.Name = "pokemonDataGridView";
            this.pokemonDataGridView.RowHeadersVisible = false;
            this.pokemonDataGridView.RowHeadersWidth = 51;
            this.pokemonDataGridView.RowTemplate.Height = 29;
            this.pokemonDataGridView.Size = new System.Drawing.Size(372, 372);
            this.pokemonDataGridView.TabIndex = 0;
            // 
            // pokemonColumn
            // 
            this.pokemonColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.pokemonColumn.HeaderText = "Species";
            this.pokemonColumn.MinimumWidth = 6;
            this.pokemonColumn.Name = "pokemonColumn";
            // 
            // versionColumn
            // 
            this.versionColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.versionColumn.HeaderText = "Version";
            this.versionColumn.MinimumWidth = 6;
            this.versionColumn.Name = "versionColumn";
            // 
            // requirementColumn
            // 
            this.requirementColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.requirementColumn.HeaderText = "Requirement";
            this.requirementColumn.MinimumWidth = 6;
            this.requirementColumn.Name = "requirementColumn";
            // 
            // UgEncounterEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 433);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.listBox);
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
        private System.Windows.Forms.DataGridViewComboBoxColumn pokemonColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn versionColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn requirementColumn;
    }
}