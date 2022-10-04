namespace ImpostersOrdeal
{
    partial class JsonConverterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JsonConverterForm));
            this.ImportPokemonButton = new System.Windows.Forms.Button();
            this.exportPokemonButton = new System.Windows.Forms.Button();
            this.modeComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // ImportPokemonButton
            // 
            this.ImportPokemonButton.Location = new System.Drawing.Point(12, 81);
            this.ImportPokemonButton.Name = "ImportPokemonButton";
            this.ImportPokemonButton.Size = new System.Drawing.Size(189, 29);
            this.ImportPokemonButton.TabIndex = 1;
            this.ImportPokemonButton.Text = "Import";
            this.ImportPokemonButton.UseVisualStyleBackColor = true;
            this.ImportPokemonButton.Click += new System.EventHandler(this.ImportJson);
            // 
            // exportPokemonButton
            // 
            this.exportPokemonButton.Location = new System.Drawing.Point(12, 46);
            this.exportPokemonButton.Name = "exportPokemonButton";
            this.exportPokemonButton.Size = new System.Drawing.Size(189, 29);
            this.exportPokemonButton.TabIndex = 0;
            this.exportPokemonButton.Text = "Export";
            this.exportPokemonButton.UseVisualStyleBackColor = true;
            this.exportPokemonButton.Click += new System.EventHandler(this.ExportJson);
            // 
            // modeComboBox
            // 
            this.modeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modeComboBox.FormattingEnabled = true;
            this.modeComboBox.Location = new System.Drawing.Point(12, 12);
            this.modeComboBox.Name = "modeComboBox";
            this.modeComboBox.Size = new System.Drawing.Size(189, 28);
            this.modeComboBox.TabIndex = 2;
            // 
            // JsonConverterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(213, 122);
            this.Controls.Add(this.modeComboBox);
            this.Controls.Add(this.ImportPokemonButton);
            this.Controls.Add(this.exportPokemonButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "JsonConverterForm";
            this.Text = "JSON Converter";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button ImportPokemonButton;
        private System.Windows.Forms.Button exportPokemonButton;
        private System.Windows.Forms.ComboBox modeComboBox;
    }
}