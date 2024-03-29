﻿namespace DMEEView1
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuZoom25 = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuZoom50 = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuZoom100 = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuZoom150 = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuZoom200 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuFitToWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.PrintSetupMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.libraryFilesDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorPaletteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.TopFileNameTextBox = new System.Windows.Forms.TextBox();
            this.DrawFileButton = new System.Windows.Forms.Button();
            this.InfoTextBox = new System.Windows.Forms.TextBox();
            this.HideNShowInfoButton = new System.Windows.Forms.Button();
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.DrawPanel = new System.Windows.Forms.Panel();
            this.DrawPictureBox = new System.Windows.Forms.PictureBox();
            this.FitToWindowButton = new System.Windows.Forms.Button();
            this.SplashBox = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            this.DrawPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DrawPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SplashBox)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(22, 22);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.configurationToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(900, 34);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator3,
            this.MenuZoom25,
            this.MenuZoom50,
            this.MenuZoom100,
            this.MenuZoom150,
            this.MenuZoom200,
            this.toolStripMenuFitToWindow,
            this.toolStripSeparator1,
            this.PrintSetupMenuItem,
            this.ToolStripMenuPrint,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(51, 28);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(210, 30);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(207, 6);
            // 
            // MenuZoom25
            // 
            this.MenuZoom25.Name = "MenuZoom25";
            this.MenuZoom25.Size = new System.Drawing.Size(210, 30);
            this.MenuZoom25.Text = "Zoom 25%";
            this.MenuZoom25.Click += new System.EventHandler(this.ToolStripMenuZoom25_Click);
            // 
            // MenuZoom50
            // 
            this.MenuZoom50.Name = "MenuZoom50";
            this.MenuZoom50.Size = new System.Drawing.Size(210, 30);
            this.MenuZoom50.Text = "Zoom 50%";
            this.MenuZoom50.Click += new System.EventHandler(this.ToolStripMenuZoom50_Click);
            // 
            // MenuZoom100
            // 
            this.MenuZoom100.Name = "MenuZoom100";
            this.MenuZoom100.Size = new System.Drawing.Size(210, 30);
            this.MenuZoom100.Text = "Zoom 100%";
            this.MenuZoom100.Click += new System.EventHandler(this.ToolStripMenuZoom100_Click);
            // 
            // MenuZoom150
            // 
            this.MenuZoom150.Name = "MenuZoom150";
            this.MenuZoom150.Size = new System.Drawing.Size(210, 30);
            this.MenuZoom150.Text = "Zoom 150%";
            this.MenuZoom150.Click += new System.EventHandler(this.ToolStripMenuZoom150_Click);
            // 
            // MenuZoom200
            // 
            this.MenuZoom200.Name = "MenuZoom200";
            this.MenuZoom200.Size = new System.Drawing.Size(210, 30);
            this.MenuZoom200.Text = "Zoom 200%";
            this.MenuZoom200.Click += new System.EventHandler(this.ToolStripMenuZoom200_Click);
            // 
            // toolStripMenuFitToWindow
            // 
            this.toolStripMenuFitToWindow.Name = "toolStripMenuFitToWindow";
            this.toolStripMenuFitToWindow.Size = new System.Drawing.Size(210, 30);
            this.toolStripMenuFitToWindow.Text = "Fit to Window";
            this.toolStripMenuFitToWindow.Click += new System.EventHandler(this.ToolStripMenuFitToWindow_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(207, 6);
            // 
            // PrintSetupMenuItem
            // 
            this.PrintSetupMenuItem.Name = "PrintSetupMenuItem";
            this.PrintSetupMenuItem.Size = new System.Drawing.Size(210, 30);
            this.PrintSetupMenuItem.Text = "Print Setup";
            this.PrintSetupMenuItem.Click += new System.EventHandler(this.PrintSetupMenuItem_Click);
            // 
            // ToolStripMenuPrint
            // 
            this.ToolStripMenuPrint.Name = "ToolStripMenuPrint";
            this.ToolStripMenuPrint.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.ToolStripMenuPrint.Size = new System.Drawing.Size(210, 30);
            this.ToolStripMenuPrint.Text = "Print";
            this.ToolStripMenuPrint.Click += new System.EventHandler(this.ToolStripMenuPrint_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(207, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(210, 30);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.libraryFilesDirectoryToolStripMenuItem,
            this.colorPaletteToolStripMenuItem});
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            this.configurationToolStripMenuItem.Size = new System.Drawing.Size(131, 28);
            this.configurationToolStripMenuItem.Text = "Configuration";
            // 
            // libraryFilesDirectoryToolStripMenuItem
            // 
            this.libraryFilesDirectoryToolStripMenuItem.Name = "libraryFilesDirectoryToolStripMenuItem";
            this.libraryFilesDirectoryToolStripMenuItem.Size = new System.Drawing.Size(158, 30);
            this.libraryFilesDirectoryToolStripMenuItem.Text = "Folders";
            this.libraryFilesDirectoryToolStripMenuItem.Click += new System.EventHandler(this.FoldersToolStripMenuItem_Click);
            // 
            // colorPaletteToolStripMenuItem
            // 
            this.colorPaletteToolStripMenuItem.Name = "colorPaletteToolStripMenuItem";
            this.colorPaletteToolStripMenuItem.Size = new System.Drawing.Size(158, 30);
            this.colorPaletteToolStripMenuItem.Text = "Colors";
            this.colorPaletteToolStripMenuItem.Click += new System.EventHandler(this.ColorPaletteToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DereferenceLinks = false;
            this.openFileDialog1.Filter = "DMEE Schematic (*.SCH)|*.sch|All Files (*.*)|*.*";
            this.openFileDialog1.RestoreDirectory = true;
            // 
            // TopFileNameTextBox
            // 
            this.TopFileNameTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.TopFileNameTextBox.Location = new System.Drawing.Point(151, 3);
            this.TopFileNameTextBox.Name = "TopFileNameTextBox";
            this.TopFileNameTextBox.ReadOnly = true;
            this.TopFileNameTextBox.Size = new System.Drawing.Size(481, 22);
            this.TopFileNameTextBox.TabIndex = 2;
            // 
            // DrawFileButton
            // 
            this.DrawFileButton.Location = new System.Drawing.Point(638, 1);
            this.DrawFileButton.Name = "DrawFileButton";
            this.DrawFileButton.Size = new System.Drawing.Size(58, 23);
            this.DrawFileButton.TabIndex = 4;
            this.DrawFileButton.Text = "draw file";
            this.DrawFileButton.UseVisualStyleBackColor = true;
            this.DrawFileButton.Click += new System.EventHandler(this.DrawFileButton_Click);
            // 
            // InfoTextBox
            // 
            this.InfoTextBox.Location = new System.Drawing.Point(471, 29);
            this.InfoTextBox.Multiline = true;
            this.InfoTextBox.Name = "InfoTextBox";
            this.InfoTextBox.ReadOnly = true;
            this.InfoTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.InfoTextBox.Size = new System.Drawing.Size(417, 229);
            this.InfoTextBox.TabIndex = 5;
            // 
            // HideNShowInfoButton
            // 
            this.HideNShowInfoButton.Location = new System.Drawing.Point(702, 1);
            this.HideNShowInfoButton.Name = "HideNShowInfoButton";
            this.HideNShowInfoButton.Size = new System.Drawing.Size(66, 23);
            this.HideNShowInfoButton.TabIndex = 6;
            this.HideNShowInfoButton.Text = "hide info";
            this.HideNShowInfoButton.UseVisualStyleBackColor = true;
            this.HideNShowInfoButton.Click += new System.EventHandler(this.HideNShowInfoButton_Click);
            // 
            // printDocument
            // 
            this.printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.PrintDocument_PrintPage);
            // 
            // printDialog1
            // 
            this.printDialog1.Document = this.printDocument;
            this.printDialog1.UseEXDialog = true;
            // 
            // DrawPanel
            // 
            this.DrawPanel.AutoScroll = true;
            this.DrawPanel.BackColor = System.Drawing.Color.Transparent;
            this.DrawPanel.Controls.Add(this.DrawPictureBox);
            this.DrawPanel.Location = new System.Drawing.Point(0, 28);
            this.DrawPanel.Name = "DrawPanel";
            this.DrawPanel.Size = new System.Drawing.Size(200, 100);
            this.DrawPanel.TabIndex = 7;
            // 
            // DrawPictureBox
            // 
            this.DrawPictureBox.Location = new System.Drawing.Point(13, 4);
            this.DrawPictureBox.Name = "DrawPictureBox";
            this.DrawPictureBox.Size = new System.Drawing.Size(100, 50);
            this.DrawPictureBox.TabIndex = 0;
            this.DrawPictureBox.TabStop = false;
            this.DrawPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawPictureBox_Paint);
            // 
            // FitToWindowButton
            // 
            this.FitToWindowButton.Location = new System.Drawing.Point(774, 1);
            this.FitToWindowButton.Name = "FitToWindowButton";
            this.FitToWindowButton.Size = new System.Drawing.Size(75, 23);
            this.FitToWindowButton.TabIndex = 6;
            this.FitToWindowButton.Text = "fit to window";
            this.FitToWindowButton.UseVisualStyleBackColor = true;
            this.FitToWindowButton.Click += new System.EventHandler(this.FitToWindowButton_Click);
            // 
            // SplashBox
            // 
            this.SplashBox.Image = ((System.Drawing.Image)(resources.GetObject("SplashBox.Image")));
            this.SplashBox.Location = new System.Drawing.Point(267, 127);
            this.SplashBox.Name = "SplashBox";
            this.SplashBox.Size = new System.Drawing.Size(365, 216);
            this.SplashBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.SplashBox.TabIndex = 8;
            this.SplashBox.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(900, 450);
            this.Controls.Add(this.SplashBox);
            this.Controls.Add(this.InfoTextBox);
            this.Controls.Add(this.FitToWindowButton);
            this.Controls.Add(this.HideNShowInfoButton);
            this.Controls.Add(this.DrawFileButton);
            this.Controls.Add(this.TopFileNameTextBox);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.DrawPanel);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "DMEE View";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.DrawPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DrawPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SplashBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox TopFileNameTextBox;
        private System.Windows.Forms.Button DrawFileButton;
        private System.Windows.Forms.TextBox InfoTextBox;
        private System.Windows.Forms.Button HideNShowInfoButton;
        private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem libraryFilesDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colorPaletteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuZoom50;
        private System.Windows.Forms.ToolStripMenuItem MenuZoom100;
        private System.Windows.Forms.ToolStripMenuItem MenuZoom200;
        private System.Windows.Forms.ToolStripMenuItem MenuZoom150;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuPrint;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Drawing.Printing.PrintDocument printDocument;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.Panel DrawPanel;
        private System.Windows.Forms.PictureBox DrawPictureBox;
        private System.Windows.Forms.ToolStripMenuItem MenuZoom25;
        private System.Windows.Forms.Button FitToWindowButton;
        private System.Windows.Forms.ToolStripMenuItem PrintSetupMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuFitToWindow;
        private System.Windows.Forms.PictureBox SplashBox;
    }
}

