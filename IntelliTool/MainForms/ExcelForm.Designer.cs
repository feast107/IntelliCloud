
namespace IntelliTool
{
    partial class ExcelForm
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
            this.ImportButtton = new System.Windows.Forms.Button();
            this.ExportButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.MenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.TopPanel.Size = new System.Drawing.Size(1052, 30);
            // 
            // ClosePic
            // 
            this.ClosePic.Location = new System.Drawing.Point(999, 0);
            // 
            // MinimunPic
            // 
            this.MinimunPic.Location = new System.Drawing.Point(948, 0);
            // 
            // MainPanel
            // 
            this.MainPanel.Size = new System.Drawing.Size(1050, 530);
            // 
            // ContentPanel
            // 
            this.ContentPanel.Controls.Add(this.ExportButton);
            this.ContentPanel.Controls.Add(this.ImportButtton);
            this.ContentPanel.Controls.Add(this.FileView);
            this.ContentPanel.Controls.Add(this.DataView);
            this.ContentPanel.Controls.Add(this.menuStrip1);
            this.ContentPanel.Location = new System.Drawing.Point(0, 30);
            this.ContentPanel.Size = new System.Drawing.Size(1052, 501);
            // 
            // DataView
            // 
            this.DataView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataView.Location = new System.Drawing.Point(12, 209);
            this.DataView.Name = "DataView";
            this.DataView.RowTemplate.Height = 25;
            this.DataView.Size = new System.Drawing.Size(1027, 279);
            this.DataView.TabIndex = 0;
            // 
            // FileView
            // 
            this.FileView.AllowUserToAddRows = false;
            this.FileView.AllowUserToDeleteRows = false;
            this.FileView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FileView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.FileView.CausesValidation = false;
            this.FileView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FileView.Location = new System.Drawing.Point(635, 28);
            this.FileView.Name = "FileView";
            this.FileView.ReadOnly = true;
            this.FileView.RowTemplate.Height = 25;
            this.FileView.Size = new System.Drawing.Size(404, 178);
            this.FileView.TabIndex = 1;
            this.FileView.TabStop = false;
            // 
            // ImportButtton
            // 
            this.ImportButtton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ImportButtton.FlatAppearance.BorderSize = 0;
            this.ImportButtton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ImportButtton.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ImportButtton.Location = new System.Drawing.Point(87, 57);
            this.ImportButtton.MaximumSize = new System.Drawing.Size(166, 107);
            this.ImportButtton.Name = "ImportButtton";
            this.ImportButtton.Size = new System.Drawing.Size(164, 104);
            this.ImportButtton.TabIndex = 2;
            this.ImportButtton.Text = "导入";
            this.ImportButtton.UseVisualStyleBackColor = true;
            this.ImportButtton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // ExportButton
            // 
            this.ExportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExportButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ExportButton.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ExportButton.Location = new System.Drawing.Point(374, 57);
            this.ExportButton.MaximumSize = new System.Drawing.Size(166, 107);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(164, 104);
            this.ExportButton.TabIndex = 3;
            this.ExportButton.Text = "导出";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1052, 25);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // MenuItem
            // 
            this.MenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.重置ToolStripMenuItem});
            this.MenuItem.Name = "MenuItem";
            this.MenuItem.Size = new System.Drawing.Size(44, 21);
            this.MenuItem.Text = "选项";
            // 
            // 重置ToolStripMenuItem
            // 
            this.重置ToolStripMenuItem.Name = "重置ToolStripMenuItem";
            this.重置ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.重置ToolStripMenuItem.Text = "重置";
            this.重置ToolStripMenuItem.Click += new System.EventHandler(this.Reset_Click);
            // 
            // ExcelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1053, 534);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ExcelForm";
            this.Text = "ExcelTool";
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
        private System.Windows.Forms.Button ImportButtton;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuItem;
        private System.Windows.Forms.ToolStripMenuItem 重置ToolStripMenuItem;
    }
}