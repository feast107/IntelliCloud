
namespace IntelliTool.TestForm
{
    partial class TestFormHigher
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
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.TopPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClosePic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinimunPic)).BeginInit();
            this.MainPanel.SuspendLayout();
            this.ContentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // TopPanel
            // 
            this.TopPanel.Size = new System.Drawing.Size(710, 53);
            // 
            // ClosePic
            // 
            this.ClosePic.Location = new System.Drawing.Point(658, 0);
            // 
            // MinimunPic
            // 
            this.MinimunPic.Location = new System.Drawing.Point(607, 0);
            // 
            // MainPanel
            // 
            this.MainPanel.Size = new System.Drawing.Size(710, 461);
            // 
            // ContentPanel
            // 
            this.ContentPanel.Controls.Add(this.button1);
            this.ContentPanel.Location = new System.Drawing.Point(0, 65);
            this.ContentPanel.Size = new System.Drawing.Size(710, 396);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(248, 205);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(208, 137);
            this.button1.TabIndex = 6;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.MouseEnter += new System.EventHandler(this.Button_Hover);
            this.button1.MouseLeave += new System.EventHandler(this.Button_Leave);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(298, 207);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(116, 25);
            this.comboBox1.TabIndex = 6;
            // 
            // TestFormHigher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 461);
            this.Controls.Add(this.comboBox1);
            this.MaximizeBox = false;
            this.Name = "TestFormHigher";
            this.Text = "TestFormHigher";
            this.Controls.SetChildIndex(this.MainPanel, 0);
            this.Controls.SetChildIndex(this.comboBox1, 0);
            this.TopPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ClosePic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinimunPic)).EndInit();
            this.MainPanel.ResumeLayout(false);
            this.ContentPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}