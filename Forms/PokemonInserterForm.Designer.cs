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
            this.formNameTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.newFormRadioButton = new System.Windows.Forms.RadioButton();
            this.newSpeciesRadioButton = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.speciesNameTextBox = new System.Windows.Forms.TextBox();
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
            this.button1.Location = new System.Drawing.Point(12, 246);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(189, 29);
            this.button1.TabIndex = 1;
            this.button1.Text = "Clone and Insert Form";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.AddFormClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "FormID to Clone";
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
            // formNameTextBox
            // 
            this.formNameTextBox.Location = new System.Drawing.Point(12, 213);
            this.formNameTextBox.Name = "formNameTextBox";
            this.formNameTextBox.Size = new System.Drawing.Size(189, 27);
            this.formNameTextBox.TabIndex = 7;
            this.formNameTextBox.Text = "New Form";
            this.formNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 190);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Form YYY Name:";
            // 
            // newFormRadioButton
            // 
            this.newFormRadioButton.AutoSize = true;
            this.newFormRadioButton.Location = new System.Drawing.Point(103, 80);
            this.newFormRadioButton.Name = "newFormRadioButton";
            this.newFormRadioButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.newFormRadioButton.Size = new System.Drawing.Size(98, 24);
            this.newFormRadioButton.TabIndex = 9;
            this.newFormRadioButton.Text = "New Form";
            this.newFormRadioButton.UseVisualStyleBackColor = true;
            // 
            // newSpeciesRadioButton
            // 
            this.newSpeciesRadioButton.AutoSize = true;
            this.newSpeciesRadioButton.Location = new System.Drawing.Point(87, 110);
            this.newSpeciesRadioButton.Name = "newSpeciesRadioButton";
            this.newSpeciesRadioButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.newSpeciesRadioButton.Size = new System.Drawing.Size(114, 24);
            this.newSpeciesRadioButton.TabIndex = 10;
            this.newSpeciesRadioButton.Text = "New Species";
            this.newSpeciesRadioButton.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 137);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(148, 20);
            this.label2.TabIndex = 12;
            this.label2.Text = "Pokémon XXX Name:";
            // 
            // speciesNameTextBox
            // 
            this.speciesNameTextBox.Location = new System.Drawing.Point(12, 160);
            this.speciesNameTextBox.Name = "speciesNameTextBox";
            this.speciesNameTextBox.Size = new System.Drawing.Size(189, 27);
            this.speciesNameTextBox.TabIndex = 11;
            this.speciesNameTextBox.Text = "New Species";
            this.speciesNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // PokemonInserterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(213, 287);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.speciesNameTextBox);
            this.Controls.Add(this.newSpeciesRadioButton);
            this.Controls.Add(this.newFormRadioButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.formNameTextBox);
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
        private System.Windows.Forms.TextBox formNameTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton newFormRadioButton;
        private System.Windows.Forms.RadioButton newSpeciesRadioButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox speciesNameTextBox;
    }
}