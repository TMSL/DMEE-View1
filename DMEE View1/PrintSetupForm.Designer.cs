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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
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
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.pageSetupDialog = new System.Windows.Forms.PageSetupDialog();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.FitToPageButton = new System.Windows.Forms.Button();
            this.colorCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(18, 19);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(240, 240);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // AlignTLButton
            // 
            this.AlignTLButton.Location = new System.Drawing.Point(284, 41);
            this.AlignTLButton.Name = "AlignTLButton";
            this.AlignTLButton.Size = new System.Drawing.Size(50, 50);
            this.AlignTLButton.TabIndex = 1;
            this.AlignTLButton.Text = "top left";
            this.AlignTLButton.UseVisualStyleBackColor = true;
            // 
            // AlignTMButton
            // 
            this.AlignTMButton.Location = new System.Drawing.Point(340, 40);
            this.AlignTMButton.Name = "AlignTMButton";
            this.AlignTMButton.Size = new System.Drawing.Size(50, 50);
            this.AlignTMButton.TabIndex = 1;
            this.AlignTMButton.Text = "top middle";
            this.AlignTMButton.UseVisualStyleBackColor = true;
            // 
            // AlignCenterButton
            // 
            this.AlignCenterButton.Location = new System.Drawing.Point(340, 97);
            this.AlignCenterButton.Name = "AlignCenterButton";
            this.AlignCenterButton.Size = new System.Drawing.Size(50, 50);
            this.AlignCenterButton.TabIndex = 1;
            this.AlignCenterButton.Text = "center";
            this.AlignCenterButton.UseVisualStyleBackColor = true;
            // 
            // AlignMLButton
            // 
            this.AlignMLButton.Location = new System.Drawing.Point(284, 97);
            this.AlignMLButton.Name = "AlignMLButton";
            this.AlignMLButton.Size = new System.Drawing.Size(50, 50);
            this.AlignMLButton.TabIndex = 1;
            this.AlignMLButton.Text = "middle left";
            this.AlignMLButton.UseVisualStyleBackColor = true;
            // 
            // AlignTRButton
            // 
            this.AlignTRButton.Location = new System.Drawing.Point(396, 40);
            this.AlignTRButton.Name = "AlignTRButton";
            this.AlignTRButton.Size = new System.Drawing.Size(50, 50);
            this.AlignTRButton.TabIndex = 1;
            this.AlignTRButton.Text = "top right";
            this.AlignTRButton.UseVisualStyleBackColor = true;
            // 
            // AlignMRButton
            // 
            this.AlignMRButton.Location = new System.Drawing.Point(396, 96);
            this.AlignMRButton.Name = "AlignMRButton";
            this.AlignMRButton.Size = new System.Drawing.Size(50, 50);
            this.AlignMRButton.TabIndex = 1;
            this.AlignMRButton.Text = "middle right";
            this.AlignMRButton.UseVisualStyleBackColor = true;
            // 
            // AlignBLButton
            // 
            this.AlignBLButton.Location = new System.Drawing.Point(284, 153);
            this.AlignBLButton.Name = "AlignBLButton";
            this.AlignBLButton.Size = new System.Drawing.Size(50, 50);
            this.AlignBLButton.TabIndex = 1;
            this.AlignBLButton.Text = "bottom left";
            this.AlignBLButton.UseVisualStyleBackColor = true;
            // 
            // buttonBMButton
            // 
            this.buttonBMButton.Location = new System.Drawing.Point(340, 153);
            this.buttonBMButton.Name = "buttonBMButton";
            this.buttonBMButton.Size = new System.Drawing.Size(50, 50);
            this.buttonBMButton.TabIndex = 1;
            this.buttonBMButton.Text = "bottom middle";
            this.buttonBMButton.UseVisualStyleBackColor = true;
            // 
            // AlignBRButton
            // 
            this.AlignBRButton.Location = new System.Drawing.Point(396, 152);
            this.AlignBRButton.Name = "AlignBRButton";
            this.AlignBRButton.Size = new System.Drawing.Size(50, 50);
            this.AlignBRButton.TabIndex = 1;
            this.AlignBRButton.Text = "bottom right";
            this.AlignBRButton.UseVisualStyleBackColor = true;
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
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(19, 275);
            this.trackBar1.Maximum = 30;
            this.trackBar1.Minimum = 10;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(239, 45);
            this.trackBar1.TabIndex = 4;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackBar1.Value = 10;
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
            // 
            // FitToPageButton
            // 
            this.FitToPageButton.Location = new System.Drawing.Point(101, 321);
            this.FitToPageButton.Name = "FitToPageButton";
            this.FitToPageButton.Size = new System.Drawing.Size(75, 23);
            this.FitToPageButton.TabIndex = 7;
            this.FitToPageButton.Text = "fit to page";
            this.FitToPageButton.UseVisualStyleBackColor = true;
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
            // PrintSetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(469, 356);
            this.Controls.Add(this.colorCheckBox);
            this.Controls.Add(this.FitToPageButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.trackBar1);
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
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PrintSetupForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PrintSetup";
            this.Shown += new System.EventHandler(this.PrintSetupForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
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
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button FitToPageButton;
        private System.Windows.Forms.CheckBox colorCheckBox;
    }
}