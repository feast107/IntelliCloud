
namespace IntelliTool
{
    partial class WordForm
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
            this.DataView = new System.Windows.Forms.DataGridView();
            this.FileView = new System.Windows.Forms.DataGridView();
            this.ExportButton = new System.Windows.Forms.Button();
            this.ImportButtton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.选项ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TopPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClosePic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinimunPic)).BeginInit();
            this.MainPanel.SuspendLayout();
            this.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FileView)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TopPanel
            // 
            this.TopPanel.Size = new System.Drawing.Size(1054, 30);
            // 
            // ClosePic
            // 
            this.ClosePic.Location = new System.Drawing.Point(1001, 0);
            // 
            // MinimunPic
            // 
            this.MinimunPic.Location = new System.Drawing.Point(950, 0);
            // 
            // MainPanel
            // 
            this.MainPanel.Size = new System.Drawing.Size(1052, 533);
            // 
            // ContentPanel
            // 
            this.ContentPanel.Controls.Add(this.ExportButton);
            this.ContentPanel.Controls.Add(this.ImportButtton);
            this.ContentPanel.Controls.Add(this.DataView);
            this.ContentPanel.Controls.Add(this.menuStrip1);
            this.ContentPanel.Location = new System.Drawing.Point(0, 30);
            this.ContentPanel.Size = new System.Drawing.Size(1054, 504);
            // 
            // DataView
            // 
            this.DataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataView.Location = new System.Drawing.Point(12, 196);
            this.DataView.Name = "DataView";
            this.DataView.RowTemplate.Height = 25;
            this.DataView.Size = new System.Drawing.Size(1029, 295);
            this.DataView.TabIndex = 0;
            // 
            // FileView
            // 
            this.FileView.AllowUserToAddRows = false;
            this.FileView.AllowUserToDeleteRows = false;
            this.FileView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.FileView.CausesValidation = false;
            this.FileView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FileView.Location = new System.Drawing.Point(638, 59);
            this.FileView.Name = "FileView";
            this.FileView.ReadOnly = true;
            this.FileView.RowTemplate.Height = 25;
            this.FileView.Size = new System.Drawing.Size(404, 162);
            this.FileView.TabIndex = 2;
            this.FileView.TabStop = false;
            // 
            // ExportButton
            // 
            this.ExportButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ExportButton.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ExportButton.Location = new System.Drawing.Point(374, 57);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(166, 107);
            this.ExportButton.TabIndex = 5;
            this.ExportButton.Text = "导出";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // ImportButtton
            // 
            this.ImportButtton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ImportButtton.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ImportButtton.Location = new System.Drawing.Point(87, 57);
            this.ImportButtton.Name = "ImportButtton";
            this.ImportButtton.Size = new System.Drawing.Size(166, 107);
            this.ImportButtton.TabIndex = 4;
            this.ImportButtton.Text = "导入";
            this.ImportButtton.UseVisualStyleBackColor = true;
            this.ImportButtton.Click += new System.EventHandler(this.ImportButtton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.选项ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1054, 25);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 选项ToolStripMenuItem
            // 
            this.选项ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.重置ToolStripMenuItem});
            this.选项ToolStripMenuItem.Name = "选项ToolStripMenuItem";
            this.选项ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.选项ToolStripMenuItem.Text = "选项";
            // 
            // 重置ToolStripMenuItem
            // 
            this.重置ToolStripMenuItem.Name = "重置ToolStripMenuItem";
            this.重置ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.重置ToolStripMenuItem.Text = "重置";
            this.重置ToolStripMenuItem.Click += new System.EventHandler(this.Reset_Click);
            // 
            // WordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1053, 534);
            this.Controls.Add(this.FileView);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "WordForm";
            this.Text = "WordTool";
            this.Controls.SetChildIndex(this.FileView, 0);
            this.Controls.SetChildIndex(this.MainPanel, 0);
            this.TopPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ClosePic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinimunPic)).EndInit();
            this.MainPanel.ResumeLayout(false);
            this.ContentPanel.ResumeLayout(false);
            this.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FileView)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DataView;
        private System.Windows.Forms.DataGridView FileView;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.Button ImportButtton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 选项ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 重置ToolStripMenuItem;
    }
}