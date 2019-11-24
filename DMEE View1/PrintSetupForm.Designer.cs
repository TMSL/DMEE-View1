namespace DMEEView1
{
    partial class PrintSetupForm
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
            this.AlignTLButton = new System.Windows.Forms.Button();
            this.AlignTMButton = new System.Windows.Forms.Button();
            this.AlignCenterButton = new System.Windows.Forms.Button();
            this.AlignMLButton = new System.Windows.Forms.Button();
            this.AlignTRButton = new System.Windows.Forms.Button();
            this.AlignMRButton = new System.Windows.Forms.Button();
            this.AlignBLButton = new System.Windows.Forms.Button();
            this.buttonBMButton = new System.Windows.Forms.Button();
            this.AlignBRButton = new System.Windows.Forms.Button();
            this.alignLabel = new System.Windows.Forms.Label();
            this.PageSetupButton = new System.Windows.Forms.Button();
            this.pageSetupDialog = new System.Windows.Forms.PageSetupDialog();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.colorCheckBox = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.FitToPageCheckBox = new System.Windows.Forms.CheckBox();
            this.ZoomUpDown = new DMEEView1.MyNumericUpDown();
            this.ZoomLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZoomUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // AlignTLButton
            // 
            this.AlignTLButton.Location = new System.Drawing.Point(284, 41);
            this.AlignTLButton.Name = "AlignTLButton";
            this.AlignTLButton.Size = new System.Drawing.Size(50, 50);
            this.AlignTLButton.TabIndex = 1;
            this.AlignTLButton.Text = "top left";
            this.AlignTLButton.UseVisualStyleBackColor = true;
            this.AlignTLButton.Click += new System.EventHandler(this.AlignTLButton_Click);
            // 
            // AlignTMButton
            // 
            this.AlignTMButton.Location = new System.Drawing.Point(340, 40);
            this.AlignTMButton.Name = "AlignTMButton";
            this.AlignTMButton.Size = new System.Drawing.Size(50, 50);
            this.AlignTMButton.TabIndex = 1;
            this.AlignTMButton.Text = "top middle";
            this.AlignTMButton.UseVisualStyleBackColor = true;
            this.AlignTMButton.Click += new System.EventHandler(this.AlignTMButton_Click);
            // 
            // AlignCenterButton
            // 
            this.AlignCenterButton.Location = new System.Drawing.Point(340, 97);
            this.AlignCenterButton.Name = "AlignCenterButton";
            this.AlignCenterButton.Size = new System.Drawing.Size(50, 50);
            this.AlignCenterButton.TabIndex = 1;
            this.AlignCenterButton.Text = "center";
            this.AlignCenterButton.UseVisualStyleBackColor = true;
            this.AlignCenterButton.Click += new System.EventHandler(this.AlignCenterButton_Click);
            // 
            // AlignMLButton
            // 
            this.AlignMLButton.Location = new System.Drawing.Point(284, 97);
            this.AlignMLButton.Name = "AlignMLButton";
            this.AlignMLButton.Size = new System.Drawing.Size(50, 50);
            this.AlignMLButton.TabIndex = 1;
            this.AlignMLButton.Text = "middle left";
            this.AlignMLButton.UseVisualStyleBackColor = true;
            this.AlignMLButton.Click += new System.EventHandler(this.AlignMLButton_Click);
            // 
            // AlignTRButton
            // 
            this.AlignTRButton.Location = new System.Drawing.Point(396, 40);
            this.AlignTRButton.Name = "AlignTRButton";
            this.AlignTRButton.Size = new System.Drawing.Size(50, 50);
            this.AlignTRButton.TabIndex = 1;
            this.AlignTRButton.Text = "top right";
            this.AlignTRButton.UseVisualStyleBackColor = true;
            this.AlignTRButton.Click += new System.EventHandler(this.AlignTRButton_Click);
            // 
            // AlignMRButton
            // 
            this.AlignMRButton.Location = new System.Drawing.Point(396, 96);
            this.AlignMRButton.Name = "AlignMRButton";
            this.AlignMRButton.Size = new System.Drawing.Size(50, 50);
            this.AlignMRButton.TabIndex = 1;
            this.AlignMRButton.Text = "middle right";
            this.AlignMRButton.UseVisualStyleBackColor = true;
            this.AlignMRButton.Click += new System.EventHandler(this.AlignMRButton_Click);
            // 
            // AlignBLButton
            // 
            this.AlignBLButton.Location = new System.Drawing.Point(284, 153);
            this.AlignBLButton.Name = "AlignBLButton";
            this.AlignBLButton.Size = new System.Drawing.Size(50, 50);
            this.AlignBLButton.TabIndex = 1;
            this.AlignBLButton.Text = "bottom left";
            this.AlignBLButton.UseVisualStyleBackColor = true;
            this.AlignBLButton.Click += new System.EventHandler(this.AlignBLButton_Click);
            // 
            // buttonBMButton
            // 
            this.buttonBMButton.Location = new System.Drawing.Point(340, 153);
            this.buttonBMButton.Name = "buttonBMButton";
            this.buttonBMButton.Size = new System.Drawing.Size(50, 50);
            this.buttonBMButton.TabIndex = 1;
            this.buttonBMButton.Text = "bottom middle";
            this.buttonBMButton.UseVisualStyleBackColor = true;
            this.buttonBMButton.Click += new System.EventHandler(this.buttonBMButton_Click);
            // 
            // AlignBRButton
            // 
            this.AlignBRButton.Location = new System.Drawing.Point(396, 152);
            this.AlignBRButton.Name = "AlignBRButton";
            this.AlignBRButton.Size = new System.Drawing.Size(50, 50);
            this.AlignBRButton.TabIndex = 1;
            this.AlignBRButton.Text = "bottom right";
            this.AlignBRButton.UseVisualStyleBackColor = true;
            this.AlignBRButton.Click += new System.EventHandler(this.AlignBRButton_Click);
            // 
            // alignLabel
            // 
            this.alignLabel.AutoSize = true;
            this.alignLabel.Location = new System.Drawing.Point(284, 22);
            this.alignLabel.Name = "alignLabel";
            this.alignLabel.Size = new System.Drawing.Size(52, 13);
            this.alignLabel.TabIndex = 2;
            this.alignLabel.Text = "alignment";
            // 
            // PageSetupButton
            // 
            this.PageSetupButton.Location = new System.Drawing.Point(284, 236);
            this.PageSetupButton.Name = "PageSetupButton";
            this.PageSetupButton.Size = new System.Drawing.Size(162, 23);
            this.PageSetupButton.TabIndex = 3;
            this.PageSetupButton.Text = "page size and margins";
            this.PageSetupButton.UseVisualStyleBackColor = true;
            this.PageSetupButton.Click += new System.EventHandler(this.PageSetupButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(371, 321);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(284, 321);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 6;
            this.SaveButton.Text = "save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // colorCheckBox
            // 
            this.colorCheckBox.AutoSize = true;
            this.colorCheckBox.Location = new System.Drawing.Point(284, 275);
            this.colorCheckBox.Name = "colorCheckBox";
            this.colorCheckBox.Size = new System.Drawing.Size(49, 17);
            this.colorCheckBox.TabIndex = 8;
            this.colorCheckBox.Text = "color";
            this.colorCheckBox.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pictureBox1.Location = new System.Drawing.Point(19, 19);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(240, 240);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // FitToPageCheckBox
            // 
            this.FitToPageCheckBox.AutoSize = true;
            this.FitToPageCheckBox.Checked = true;
            this.FitToPageCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FitToPageCheckBox.Location = new System.Drawing.Point(107, 324);
            this.FitToPageCheckBox.Name = "FitToPageCheckBox";
            this.FitToPageCheckBox.Size = new System.Drawing.Size(73, 17);
            this.FitToPageCheckBox.TabIndex = 9;
            this.FitToPageCheckBox.Text = "fit to page";
            this.FitToPageCheckBox.UseVisualStyleBackColor = true;
            this.FitToPageCheckBox.CheckedChanged += new System.EventHandler(this.FitToPageCheckBox_CheckedChanged);
            // 
            // ZoomUpDown
            // 
            this.ZoomUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ZoomUpDown.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ZoomUpDown.Location = new System.Drawing.Point(120, 275);
            this.ZoomUpDown.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.ZoomUpDown.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ZoomUpDown.Name = "ZoomUpDown";
            this.ZoomUpDown.Size = new System.Drawing.Size(48, 22);
            this.ZoomUpDown.TabIndex = 10;
            this.ZoomUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ZoomUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.ZoomUpDown.ValueChanged += new System.EventHandler(this.customNumericUpDown1_ValueChanged);
            // 
            // ZoomLabel
            // 
            this.ZoomLabel.AutoSize = true;
            this.ZoomLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ZoomLabel.Location = new System.Drawing.Point(70, 277);
            this.ZoomLabel.Name = "ZoomLabel";
            this.ZoomLabel.Size = new System.Drawing.Size(44, 16);
            this.ZoomLabel.TabIndex = 11;
            this.ZoomLabel.Text = "zoom:";
            // 
            // PrintSetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(469, 356);
            this.Controls.Add(this.ZoomLabel);
            this.Controls.Add(this.ZoomUpDown);
            this.Controls.Add(this.FitToPageCheckBox);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.colorCheckBox);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.PageSetupButton);
            this.Controls.Add(this.alignLabel);
            this.Controls.Add(this.AlignBRButton);
            this.Controls.Add(this.buttonBMButton);
            this.Controls.Add(this.AlignBLButton);
            this.Controls.Add(this.AlignMRButton);
            this.Controls.Add(this.AlignTRButton);
            this.Controls.Add(this.AlignMLButton);
            this.Controls.Add(this.AlignCenterButton);
            this.Controls.Add(this.AlignTMButton);
            this.Controls.Add(this.AlignTLButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PrintSetupForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PrintSetup";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZoomUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button AlignTLButton;
        private System.Windows.Forms.Button AlignTMButton;
        private System.Windows.Forms.Button AlignCenterButton;
        private System.Windows.Forms.Button AlignMLButton;
        private System.Windows.Forms.Button AlignTRButton;
        private System.Windows.Forms.Button AlignMRButton;
        private System.Windows.Forms.Button AlignBLButton;
        private System.Windows.Forms.Button buttonBMButton;
        private System.Windows.Forms.Button AlignBRButton;
        private System.Windows.Forms.Label alignLabel;
        private System.Windows.Forms.Button PageSetupButton;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.CheckBox colorCheckBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox FitToPageCheckBox;
        private MyNumericUpDown ZoomUpDown;
        private System.Windows.Forms.Label ZoomLabel;
    }
}