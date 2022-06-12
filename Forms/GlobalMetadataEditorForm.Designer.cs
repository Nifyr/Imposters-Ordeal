namespace ImpostersOrdeal
{
    partial class GlobalMetadataEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlobalMetadataEditorForm));
            this.imageListBox = new System.Windows.Forms.ListBox();
            this.typeListBox = new System.Windows.Forms.ListBox();
            this.fieldListBox = new System.Windows.Forms.ListBox();
            this.defaultValueRichTextBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.uIntTextBox = new System.Windows.Forms.TextBox();
            this.intTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.decimalTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.stringTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // imageListBox
            // 
            this.imageListBox.FormattingEnabled = true;
            this.imageListBox.ItemHeight = 20;
            this.imageListBox.Location = new System.Drawing.Point(12, 12);
            this.imageListBox.Name = "imageListBox";
            this.imageListBox.Size = new System.Drawing.Size(250, 644);
            this.imageListBox.TabIndex = 0;
            // 
            // typeListBox
            // 
            this.typeListBox.FormattingEnabled = true;
            this.typeListBox.ItemHeight = 20;
            this.typeListBox.Location = new System.Drawing.Point(268, 12);
            this.typeListBox.Name = "typeListBox";
            this.typeListBox.Size = new System.Drawing.Size(250, 644);
            this.typeListBox.TabIndex = 1;
            // 
            // fieldListBox
            // 
            this.fieldListBox.FormattingEnabled = true;
            this.fieldListBox.ItemHeight = 20;
            this.fieldListBox.Location = new System.Drawing.Point(524, 12);
            this.fieldListBox.Name = "fieldListBox";
            this.fieldListBox.Size = new System.Drawing.Size(250, 644);
            this.fieldListBox.TabIndex = 2;
            // 
            // defaultValueRichTextBox
            // 
            this.defaultValueRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.defaultValueRichTextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.defaultValueRichTextBox.Location = new System.Drawing.Point(780, 12);
            this.defaultValueRichTextBox.Name = "defaultValueRichTextBox";
            this.defaultValueRichTextBox.Size = new System.Drawing.Size(212, 644);
            this.defaultValueRichTextBox.TabIndex = 3;
            this.defaultValueRichTextBox.Text = "00 11 22 33 44 55 66 77 88 99 AA BB CC DD EE FF";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(998, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Unsigned Integer";
            // 
            // uIntTextBox
            // 
            this.uIntTextBox.Location = new System.Drawing.Point(998, 32);
            this.uIntTextBox.Name = "uIntTextBox";
            this.uIntTextBox.Size = new System.Drawing.Size(200, 27);
            this.uIntTextBox.TabIndex = 5;
            // 
            // intTextBox
            // 
            this.intTextBox.Location = new System.Drawing.Point(998, 85);
            this.intTextBox.Name = "intTextBox";
            this.intTextBox.Size = new System.Drawing.Size(200, 27);
            this.intTextBox.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(998, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Signed Integer";
            // 
            // decimalTextBox
            // 
            this.decimalTextBox.Location = new System.Drawing.Point(998, 138);
            this.decimalTextBox.Name = "decimalTextBox";
            this.decimalTextBox.Size = new System.Drawing.Size(200, 27);
            this.decimalTextBox.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(998, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Decimal Number";
            // 
            // stringTextBox
            // 
            this.stringTextBox.Location = new System.Drawing.Point(998, 191);
            this.stringTextBox.Name = "stringTextBox";
            this.stringTextBox.Size = new System.Drawing.Size(200, 27);
            this.stringTextBox.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(998, 168);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 20);
            this.label4.TabIndex = 10;
            this.label4.Text = "Text";
            // 
            // GlobalMetadataEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1210, 668);
            this.Controls.Add(this.stringTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.decimalTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.intTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.uIntTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.defaultValueRichTextBox);
            this.Controls.Add(this.fieldListBox);
            this.Controls.Add(this.typeListBox);
            this.Controls.Add(this.imageListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GlobalMetadataEditorForm";
            this.Text = "Global Metadata Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox imageListBox;
        private System.Windows.Forms.ListBox typeListBox;
        private System.Windows.Forms.ListBox fieldListBox;
        private System.Windows.Forms.RichTextBox defaultValueRichTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox uIntTextBox;
        private System.Windows.Forms.TextBox intTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox decimalTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox stringTextBox;
        private System.Windows.Forms.Label label4;
    }
}