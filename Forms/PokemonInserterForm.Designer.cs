namespace ImpostersOrdeal
{
    partial class PokemonInserterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PokemonInserterForm));
            this.dexIDComboBox = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.formIDComboBox = new System.Windows.Forms.ComboBox();
            this.genderIDComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.formNameTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // dexIDComboBox
            // 
            this.dexIDComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.dexIDComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.dexIDComboBox.FormattingEnabled = true;
            this.dexIDComboBox.Location = new System.Drawing.Point(12, 12);
            this.dexIDComboBox.Name = "dexIDComboBox";
            this.dexIDComboBox.Size = new System.Drawing.Size(189, 28);
            this.dexIDComboBox.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 167);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(189, 29);
            this.button1.TabIndex = 1;
            this.button1.Text = "Copy and Insert Form";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.AddFormClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "FormID to Copy";
            // 
            // formIDComboBox
            // 
            this.formIDComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.formIDComboBox.FormattingEnabled = true;
            this.formIDComboBox.Location = new System.Drawing.Point(151, 46);
            this.formIDComboBox.Name = "formIDComboBox";
            this.formIDComboBox.Size = new System.Drawing.Size(50, 28);
            this.formIDComboBox.TabIndex = 4;
            // 
            // genderIDComboBox
            // 
            this.genderIDComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.genderIDComboBox.FormattingEnabled = true;
            this.genderIDComboBox.Location = new System.Drawing.Point(151, 80);
            this.genderIDComboBox.Name = "genderIDComboBox";
            this.genderIDComboBox.Size = new System.Drawing.Size(50, 28);
            this.genderIDComboBox.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "GenderID to Copy";
            // 
            // formNameTextBox
            // 
            this.formNameTextBox.Location = new System.Drawing.Point(12, 134);
            this.formNameTextBox.Name = "formNameTextBox";
            this.formNameTextBox.Size = new System.Drawing.Size(189, 27);
            this.formNameTextBox.TabIndex = 7;
            this.formNameTextBox.Text = "New Form";
            this.formNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "New Form Name";
            // 
            // PokemonInserterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(213, 208);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.formNameTextBox);
            this.Controls.Add(this.genderIDComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.formIDComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dexIDComboBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PokemonInserterForm";
            this.Text = "Pokémon Inserter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox dexIDComboBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox formIDComboBox;
        private System.Windows.Forms.ComboBox genderIDComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox formNameTextBox;
        private System.Windows.Forms.Label label3;
    }
}