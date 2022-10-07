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
            this.srcDexIDComboBox = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.formIDComboBox = new System.Windows.Forms.ComboBox();
            this.formNameTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.newFormRadioButton = new System.Windows.Forms.RadioButton();
            this.newSpeciesRadioButton = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.speciesNameTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.genderConfigTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dstDexIDComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // srcDexIDComboBox
            // 
            this.srcDexIDComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.srcDexIDComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.srcDexIDComboBox.FormattingEnabled = true;
            this.srcDexIDComboBox.Location = new System.Drawing.Point(83, 26);
            this.srcDexIDComboBox.Name = "srcDexIDComboBox";
            this.srcDexIDComboBox.Size = new System.Drawing.Size(100, 28);
            this.srcDexIDComboBox.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 429);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(189, 29);
            this.button1.TabIndex = 3;
            this.button1.Text = "Clone and Insert";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.AddFormClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(69, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "FormID";
            // 
            // formIDComboBox
            // 
            this.formIDComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.formIDComboBox.FormattingEnabled = true;
            this.formIDComboBox.Location = new System.Drawing.Point(133, 60);
            this.formIDComboBox.Name = "formIDComboBox";
            this.formIDComboBox.Size = new System.Drawing.Size(50, 28);
            this.formIDComboBox.TabIndex = 4;
            // 
            // formNameTextBox
            // 
            this.formNameTextBox.Location = new System.Drawing.Point(6, 133);
            this.formNameTextBox.Name = "formNameTextBox";
            this.formNameTextBox.Size = new System.Drawing.Size(177, 27);
            this.formNameTextBox.TabIndex = 16;
            this.formNameTextBox.Text = "New Form";
            this.formNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Form YYY Name:";
            // 
            // newFormRadioButton
            // 
            this.newFormRadioButton.AutoSize = true;
            this.newFormRadioButton.Location = new System.Drawing.Point(85, 26);
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
            this.newSpeciesRadioButton.Location = new System.Drawing.Point(69, 56);
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
            this.label2.Location = new System.Drawing.Point(6, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(148, 20);
            this.label2.TabIndex = 12;
            this.label2.Text = "Pokémon XXX Name:";
            // 
            // speciesNameTextBox
            // 
            this.speciesNameTextBox.Location = new System.Drawing.Point(6, 80);
            this.speciesNameTextBox.Name = "speciesNameTextBox";
            this.speciesNameTextBox.Size = new System.Drawing.Size(177, 27);
            this.speciesNameTextBox.TabIndex = 15;
            this.speciesNameTextBox.Text = "New Species";
            this.speciesNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 20);
            this.label4.TabIndex = 13;
            this.label4.Text = "Species";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.genderConfigTextBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.srcDexIDComboBox);
            this.groupBox1.Controls.Add(this.formIDComboBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 104);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(189, 147);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Source Pokémon";
            // 
            // genderConfigTextBox
            // 
            this.genderConfigTextBox.Location = new System.Drawing.Point(6, 114);
            this.genderConfigTextBox.Name = "genderConfigTextBox";
            this.genderConfigTextBox.ReadOnly = true;
            this.genderConfigTextBox.Size = new System.Drawing.Size(177, 27);
            this.genderConfigTextBox.TabIndex = 15;
            this.genderConfigTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 91);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 20);
            this.label6.TabIndex = 14;
            this.label6.Text = "Gender Config";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.newFormRadioButton);
            this.groupBox2.Controls.Add(this.newSpeciesRadioButton);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(189, 86);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mode";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.dstDexIDComboBox);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.formNameTextBox);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.speciesNameTextBox);
            this.groupBox3.Location = new System.Drawing.Point(12, 257);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(189, 166);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Target";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 20);
            this.label5.TabIndex = 15;
            this.label5.Text = "Species";
            // 
            // dstDexIDComboBox
            // 
            this.dstDexIDComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.dstDexIDComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.dstDexIDComboBox.FormattingEnabled = true;
            this.dstDexIDComboBox.Location = new System.Drawing.Point(83, 26);
            this.dstDexIDComboBox.Name = "dstDexIDComboBox";
            this.dstDexIDComboBox.Size = new System.Drawing.Size(100, 28);
            this.dstDexIDComboBox.TabIndex = 14;
            // 
            // PokemonInserterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(213, 470);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PokemonInserterForm";
            this.Text = "Pokémon Inserter";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox srcDexIDComboBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox formIDComboBox;
        private System.Windows.Forms.TextBox formNameTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton newFormRadioButton;
        private System.Windows.Forms.RadioButton newSpeciesRadioButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox speciesNameTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox dstDexIDComboBox;
        private System.Windows.Forms.TextBox genderConfigTextBox;
        private System.Windows.Forms.Label label6;
    }
}