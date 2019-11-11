namespace DMEEView1
{
    partial class ColorConfigForm
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
            this.pinsCheckBox = new System.Windows.Forms.CheckBox();
            this.pinsLabel = new System.Windows.Forms.Label();
            this.textLabel = new System.Windows.Forms.Label();
            this.wiresLabel = new System.Windows.Forms.Label();
            this.linesLabel = new System.Windows.Forms.Label();
            this.pinsColorBox = new System.Windows.Forms.PictureBox();
            this.textColorBox = new System.Windows.Forms.PictureBox();
            this.wiresColorBox = new System.Windows.Forms.PictureBox();
            this.linesColorBox = new System.Windows.Forms.PictureBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.colorRadioButton = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.blackAndWhiteRadioButton = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.pinsColorBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textColorBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wiresColorBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.linesColorBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pinsCheckBox
            // 
            this.pinsCheckBox.AutoSize = true;
            this.pinsCheckBox.Location = new System.Drawing.Point(93, 14);
            this.pinsCheckBox.Name = "pinsCheckBox";
            this.pinsCheckBox.Size = new System.Drawing.Size(51, 17);
            this.pinsCheckBox.TabIndex = 0;
            this.pinsCheckBox.Text = "show";
            this.pinsCheckBox.UseVisualStyleBackColor = true;
            this.pinsCheckBox.CheckedChanged += new System.EventHandler(this.pinsCheckBox_CheckedChanged);
            this.pinsCheckBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pinsCheckBox_Paint);
            // 
            // pinsLabel
            // 
            this.pinsLabel.AutoSize = true;
            this.pinsLabel.Location = new System.Drawing.Point(29, 15);
            this.pinsLabel.Name = "pinsLabel";
            this.pinsLabel.Size = new System.Drawing.Size(26, 13);
            this.pinsLabel.TabIndex = 1;
            this.pinsLabel.Text = "pins";
            this.pinsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textLabel
            // 
            this.textLabel.AutoSize = true;
            this.textLabel.Location = new System.Drawing.Point(31, 37);
            this.textLabel.Name = "textLabel";
            this.textLabel.Size = new System.Drawing.Size(24, 13);
            this.textLabel.TabIndex = 1;
            this.textLabel.Text = "text";
            this.textLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // wiresLabel
            // 
            this.wiresLabel.AutoSize = true;
            this.wiresLabel.Location = new System.Drawing.Point(24, 81);
            this.wiresLabel.Name = "wiresLabel";
            this.wiresLabel.Size = new System.Drawing.Size(31, 13);
            this.wiresLabel.TabIndex = 1;
            this.wiresLabel.Text = "wires";
            this.wiresLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // linesLabel
            // 
            this.linesLabel.AutoSize = true;
            this.linesLabel.Location = new System.Drawing.Point(27, 59);
            this.linesLabel.Name = "linesLabel";
            this.linesLabel.Size = new System.Drawing.Size(28, 13);
            this.linesLabel.TabIndex = 1;
            this.linesLabel.Text = "lines";
            this.linesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pinsColorBox
            // 
            this.pinsColorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pinsColorBox.Location = new System.Drawing.Point(61, 14);
            this.pinsColorBox.Name = "pinsColorBox";
            this.pinsColorBox.Size = new System.Drawing.Size(21, 16);
            this.pinsColorBox.TabIndex = 2;
            this.pinsColorBox.TabStop = false;
            this.pinsColorBox.Click += new System.EventHandler(this.pinsColorBox_Click);
            this.pinsColorBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pinsColorBox_Paint);
            // 
            // textColorBox
            // 
            this.textColorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textColorBox.Location = new System.Drawing.Point(61, 36);
            this.textColorBox.Name = "textColorBox";
            this.textColorBox.Size = new System.Drawing.Size(21, 16);
            this.textColorBox.TabIndex = 2;
            this.textColorBox.TabStop = false;
            this.textColorBox.Click += new System.EventHandler(this.textColorBox_Click);
            this.textColorBox.Paint += new System.Windows.Forms.PaintEventHandler(this.textColorBox_Paint);
            // 
            // wiresColorBox
            // 
            this.wiresColorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.wiresColorBox.Location = new System.Drawing.Point(61, 80);
            this.wiresColorBox.Name = "wiresColorBox";
            this.wiresColorBox.Size = new System.Drawing.Size(21, 16);
            this.wiresColorBox.TabIndex = 2;
            this.wiresColorBox.TabStop = false;
            this.wiresColorBox.Click += new System.EventHandler(this.wiresColorBox_Click);
            this.wiresColorBox.Paint += new System.Windows.Forms.PaintEventHandler(this.wiresColorBox_Paint);
            // 
            // linesColorBox
            // 
            this.linesColorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.linesColorBox.Location = new System.Drawing.Point(61, 58);
            this.linesColorBox.Name = "linesColorBox";
            this.linesColorBox.Size = new System.Drawing.Size(21, 16);
            this.linesColorBox.TabIndex = 2;
            this.linesColorBox.TabStop = false;
            this.linesColorBox.Click += new System.EventHandler(this.linesColorBox_Click);
            this.linesColorBox.Paint += new System.Windows.Forms.PaintEventHandler(this.linesColorBox_Paint);
            // 
            // saveButton
            // 
            this.saveButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.saveButton.Location = new System.Drawing.Point(50, 117);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 3;
            this.saveButton.Text = "save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(151, 117);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // colorRadioButton
            // 
            this.colorRadioButton.AutoSize = true;
            this.colorRadioButton.Location = new System.Drawing.Point(3, 3);
            this.colorRadioButton.Name = "colorRadioButton";
            this.colorRadioButton.Size = new System.Drawing.Size(48, 17);
            this.colorRadioButton.TabIndex = 4;
            this.colorRadioButton.Text = "color";
            this.colorRadioButton.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.blackAndWhiteRadioButton);
            this.panel1.Controls.Add(this.colorRadioButton);
            this.panel1.Location = new System.Drawing.Point(151, 57);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(114, 46);
            this.panel1.TabIndex = 5;
            // 
            // blackAndWhiteRadioButton
            // 
            this.blackAndWhiteRadioButton.AutoSize = true;
            this.blackAndWhiteRadioButton.Checked = true;
            this.blackAndWhiteRadioButton.Location = new System.Drawing.Point(3, 23);
            this.blackAndWhiteRadioButton.Name = "blackAndWhiteRadioButton";
            this.blackAndWhiteRadioButton.Size = new System.Drawing.Size(103, 17);
            this.blackAndWhiteRadioButton.TabIndex = 5;
            this.blackAndWhiteRadioButton.TabStop = true;
            this.blackAndWhiteRadioButton.Text = "black and  white";
            this.blackAndWhiteRadioButton.UseVisualStyleBackColor = true;
            this.blackAndWhiteRadioButton.CheckedChanged += new System.EventHandler(this.blackAndWhiteRadioButton_CheckedChanged);
            // 
            // ColorConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(277, 151);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.linesColorBox);
            this.Controls.Add(this.wiresColorBox);
            this.Controls.Add(this.textColorBox);
            this.Controls.Add(this.pinsColorBox);
            this.Controls.Add(this.linesLabel);
            this.Controls.Add(this.wiresLabel);
            this.Controls.Add(this.textLabel);
            this.Controls.Add(this.pinsLabel);
            this.Controls.Add(this.pinsCheckBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColorConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Color / Layer Configuration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ColorConfigForm_FormClosing);
            this.Shown += new System.EventHandler(this.ColorConfigForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pinsColorBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textColorBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wiresColorBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.linesColorBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox pinsCheckBox;
        private System.Windows.Forms.Label pinsLabel;
        private System.Windows.Forms.Label textLabel;
        private System.Windows.Forms.Label wiresLabel;
        private System.Windows.Forms.Label linesLabel;
        private System.Windows.Forms.PictureBox pinsColorBox;
        private System.Windows.Forms.PictureBox textColorBox;
        private System.Windows.Forms.PictureBox wiresColorBox;
        private System.Windows.Forms.PictureBox linesColorBox;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.RadioButton colorRadioButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton blackAndWhiteRadioButton;
    }
}