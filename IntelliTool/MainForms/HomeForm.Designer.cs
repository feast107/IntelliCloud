
namespace IntelliTool
{
    partial class HomeForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            Program.Release();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Excel = new System.Windows.Forms.Button();
            this.Word = new System.Windows.Forms.Button();
            this.TopPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClosePic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinimunPic)).BeginInit();
            this.MainPanel.SuspendLayout();
            this.ContentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // TopPanel
            // 
            this.TopPanel.Size = new System.Drawing.Size(1056, 30);
            // 
            // ClosePic
            // 
            this.ClosePic.Location = new System.Drawing.Point(1003, 0);
            // 
            // MinimunPic
            // 
            this.MinimunPic.Location = new System.Drawing.Point(952, 0);
            // 
            // MainPanel
            // 
            this.MainPanel.Size = new System.Drawing.Size(1054, 564);
            // 
            // ContentPanel
            // 
            this.ContentPanel.Controls.Add(this.Word);
            this.ContentPanel.Controls.Add(this.Excel);
            this.ContentPanel.Location = new System.Drawing.Point(0, 30);
            this.ContentPanel.Size = new System.Drawing.Size(1056, 535);
            // 
            // Excel
            // 
            this.Excel.Image = global::IntelliTool.Properties.Resources.ExceIcon;
            this.Excel.Location = new System.Drawing.Point(12, 12);
            this.Excel.Name = "Excel";
            this.Excel.Size = new System.Drawing.Size(512, 512);
            this.Excel.TabIndex = 0;
            this.Excel.UseVisualStyleBackColor = true;
            this.Excel.Click += new System.EventHandler(this.Excel_Click);
            // 
            // Word
            // 
            this.Word.Image = global::IntelliTool.Properties.Resources.Wordcon;
            this.Word.Location = new System.Drawing.Point(530, 12);
            this.Word.Name = "Word";
            this.Word.Size = new System.Drawing.Size(512, 512);
            this.Word.TabIndex = 1;
            this.Word.UseVisualStyleBackColor = true;
            this.Word.Click += new System.EventHandler(this.Word_Click);
            // 
            // HomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1053, 564);
            this.Name = "HomeForm";
            this.Text = "IntelliTool";
            this.Load += new System.EventHandler(this.HomeForm_Load);
            this.TopPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ClosePic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinimunPic)).EndInit();
            this.MainPanel.ResumeLayout(false);
            this.ContentPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button Excel;
        private System.Windows.Forms.Button Word;
    }
}

