
namespace ImpostersOrdeal
{
    partial class ShopEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShopEditorForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.martDataGridView = new System.Windows.Forms.DataGridView();
            this.commonItemColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.badgeCountColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.zoneIDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.fixedShopDataGridView = new System.Windows.Forms.DataGridView();
            this.fixedItemColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.shopColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.bpShopDataGridView = new System.Windows.Forms.DataGridView();
            this.bpItemColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.npcColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.martDataGridView)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fixedShopDataGridView)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bpShopDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.martDataGridView);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(390, 409);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Normal Shop Items";
            // 
            // martDataGridView
            // 
            this.martDataGridView.AllowUserToResizeRows = false;
            this.martDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.martDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.martDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.commonItemColumn,
            this.badgeCountColumn,
            this.zoneIDColumn});
            this.martDataGridView.Location = new System.Drawing.Point(6, 26);
            this.martDataGridView.Name = "martDataGridView";
            this.martDataGridView.RowHeadersWidth = 20;
            this.martDataGridView.RowTemplate.Height = 29;
            this.martDataGridView.Size = new System.Drawing.Size(378, 377);
            this.martDataGridView.TabIndex = 0;
            this.martDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DataError);
            this.martDataGridView.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.MartDefaultValuesNeeded);
            // 
            // commonItemColumn
            // 
            this.commonItemColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.commonItemColumn.FillWeight = 200F;
            this.commonItemColumn.HeaderText = "Item";
            this.commonItemColumn.MinimumWidth = 6;
            this.commonItemColumn.Name = "commonItemColumn";
            // 
            // badgeCountColumn
            // 
            this.badgeCountColumn.HeaderText = "Badges";
            this.badgeCountColumn.MinimumWidth = 6;
            this.badgeCountColumn.Name = "badgeCountColumn";
            this.badgeCountColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // zoneIDColumn
            // 
            this.zoneIDColumn.HeaderText = "zoneID";
            this.zoneIDColumn.MinimumWidth = 6;
            this.zoneIDColumn.Name = "zoneIDColumn";
            this.zoneIDColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.fixedShopDataGridView);
            this.groupBox2.Location = new System.Drawing.Point(408, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(189, 409);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Fixed Shop Items";
            // 
            // fixedShopDataGridView
            // 
            this.fixedShopDataGridView.AllowUserToResizeRows = false;
            this.fixedShopDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.fixedShopDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.fixedShopDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.fixedItemColumn,
            this.shopColumn});
            this.fixedShopDataGridView.Location = new System.Drawing.Point(6, 26);
            this.fixedShopDataGridView.Name = "fixedShopDataGridView";
            this.fixedShopDataGridView.RowHeadersWidth = 20;
            this.fixedShopDataGridView.RowTemplate.Height = 29;
            this.fixedShopDataGridView.Size = new System.Drawing.Size(177, 377);
            this.fixedShopDataGridView.TabIndex = 1;
            this.fixedShopDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DataError);
            this.fixedShopDataGridView.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.FixedShopDefaultValuesNeeded);
            // 
            // fixedItemColumn
            // 
            this.fixedItemColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.fixedItemColumn.FillWeight = 200F;
            this.fixedItemColumn.HeaderText = "Item";
            this.fixedItemColumn.MinimumWidth = 6;
            this.fixedItemColumn.Name = "fixedItemColumn";
            // 
            // shopColumn
            // 
            this.shopColumn.HeaderText = "Shop";
            this.shopColumn.MinimumWidth = 6;
            this.shopColumn.Name = "shopColumn";
            this.shopColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.shopColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.bpShopDataGridView);
            this.groupBox3.Location = new System.Drawing.Point(603, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(189, 409);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "BP Shop Items";
            // 
            // bpShopDataGridView
            // 
            this.bpShopDataGridView.AllowUserToResizeRows = false;
            this.bpShopDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.bpShopDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.bpShopDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.bpItemColumn,
            this.npcColumn});
            this.bpShopDataGridView.Location = new System.Drawing.Point(6, 26);
            this.bpShopDataGridView.Name = "bpShopDataGridView";
            this.bpShopDataGridView.RowHeadersWidth = 20;
            this.bpShopDataGridView.RowTemplate.Height = 29;
            this.bpShopDataGridView.Size = new System.Drawing.Size(177, 377);
            this.bpShopDataGridView.TabIndex = 1;
            this.bpShopDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DataError);
            this.bpShopDataGridView.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.BpShopDefaultValuesNeeded);
            // 
            // bpItemColumn
            // 
            this.bpItemColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.bpItemColumn.FillWeight = 200F;
            this.bpItemColumn.HeaderText = "Item";
            this.bpItemColumn.MinimumWidth = 6;
            this.bpItemColumn.Name = "bpItemColumn";
            // 
            // npcColumn
            // 
            this.npcColumn.HeaderText = "NPC";
            this.npcColumn.MinimumWidth = 6;
            this.npcColumn.Name = "npcColumn";
            this.npcColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.npcColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ShopEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 433);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ShopEditorForm";
            this.Text = "Shop Editor";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.martDataGridView)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fixedShopDataGridView)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bpShopDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView martDataGridView;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView fixedShopDataGridView;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView bpShopDataGridView;
        private System.Windows.Forms.DataGridViewComboBoxColumn commonItemColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn badgeCountColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn zoneIDColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn fixedItemColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn shopColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn bpItemColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn npcColumn;
    }
}