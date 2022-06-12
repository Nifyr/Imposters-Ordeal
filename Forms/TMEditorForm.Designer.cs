
namespace ImpostersOrdeal
{
    partial class TMEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TMEditorForm));
            this.listBox = new System.Windows.Forms.ListBox();
            this.itemComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.moveComboBox = new System.Windows.Forms.ComboBox();
            this.compatibilityNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.compatibilityNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // listBox
            // 
            this.listBox.FormattingEnabled = true;
            this.listBox.ItemHeight = 20;
            this.listBox.Location = new System.Drawing.Point(12, 12);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(189, 164);
            this.listBox.TabIndex = 0;
            // 
            // itemComboBox
            // 
            this.itemComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.itemComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.itemComboBox.FormattingEnabled = true;
            this.itemComboBox.Location = new System.Drawing.Point(296, 12);
            this.itemComboBox.Name = "itemComboBox";
            this.itemComboBox.Size = new System.Drawing.Size(100, 28);
            this.itemComboBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(240, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Target";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(244, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Move";
            // 
            // moveComboBox
            // 
            this.moveComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.moveComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.moveComboBox.FormattingEnabled = true;
            this.moveComboBox.Location = new System.Drawing.Point(296, 46);
            this.moveComboBox.Name = "moveComboBox";
            this.moveComboBox.Size = new System.Drawing.Size(100, 28);
            this.moveComboBox.TabIndex = 3;
            // 
            // compatibilityNumericUpDown
            // 
            this.compatibilityNumericUpDown.Location = new System.Drawing.Point(346, 80);
            this.compatibilityNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.compatibilityNumericUpDown.Name = "compatibilityNumericUpDown";
            this.compatibilityNumericUpDown.Size = new System.Drawing.Size(50, 27);
            this.compatibilityNumericUpDown.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(216, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Compatibility Set";
            // 
            // TMEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 193);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.compatibilityNumericUpDown);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.moveComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.itemComboBox);
            this.Controls.Add(this.listBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TMEditorForm";
            this.Text = "TM Editor";
            ((System.ComponentModel.ISupportInitialize)(this.compatibilityNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.ComboBox itemComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox moveComboBox;
        private System.Windows.Forms.NumericUpDown compatibilityNumericUpDown;
        private System.Windows.Forms.Label label3;
    }
}