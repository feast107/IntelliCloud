
namespace IntelliTool.SupForms
{
    partial class WordSearchForm
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
            this.AddPic = new System.Windows.Forms.PictureBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.RemovePic = new System.Windows.Forms.PictureBox();
            this.RulesView = new System.Windows.Forms.TreeView();
            this.SwitchPic = new System.Windows.Forms.PictureBox();
            this.InputBox = new System.Windows.Forms.TextBox();
            this.TargText = new System.Windows.Forms.Label();
            this.RuleBox = new System.Windows.Forms.Label();
            this.RulesCombo = new System.Windows.Forms.ComboBox();
            this.Numberbox = new System.Windows.Forms.NumericUpDown();
            this.FileButton = new System.Windows.Forms.Button();
            this.DirButton = new System.Windows.Forms.Button();
            this.TopPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClosePic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinimunPic)).BeginInit();
            this.MainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AddPic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RemovePic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SwitchPic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Numberbox)).BeginInit();
            this.SuspendLayout();
            // 
            // TopPanel
            // 
            this.TopPanel.Size = new System.Drawing.Size(621, 53);
            // 
            // ClosePic
            // 
            this.ClosePic.Location = new System.Drawing.Point(568, 0);
            this.ClosePic.Click += new System.EventHandler(this.ClosePic_Click_1);
            // 
            // MinimunPic
            // 
            this.MinimunPic.Location = new System.Drawing.Point(517, 0);
            // 
            // MainPanel
            // 
            this.MainPanel.Size = new System.Drawing.Size(621, 377);
            // 
            // ContentPanel
            // 
            this.ContentPanel.Location = new System.Drawing.Point(0, 30);
            this.ContentPanel.Size = new System.Drawing.Size(621, 347);
            this.ContentPanel.Visible = false;
            // 
            // AddPic
            // 
            this.AddPic.BackColor = System.Drawing.Color.Transparent;
            this.AddPic.BackgroundImage = global::IntelliTool.Properties.Resources.AddIcon_small1;
            this.AddPic.Location = new System.Drawing.Point(29, 87);
            this.AddPic.Name = "AddPic";
            this.AddPic.Size = new System.Drawing.Size(25, 25);
            this.AddPic.TabIndex = 19;
            this.AddPic.TabStop = false;
            this.AddPic.Click += new System.EventHandler(this.AddBox_Click);
            // 
            // RemovePic
            // 
            this.RemovePic.BackgroundImage = global::IntelliTool.Properties.Resources.removeicon_small;
            this.RemovePic.Location = new System.Drawing.Point(29, 191);
            this.RemovePic.Name = "RemovePic";
            this.RemovePic.Size = new System.Drawing.Size(25, 25);
            this.RemovePic.TabIndex = 20;
            this.RemovePic.TabStop = false;
            this.RemovePic.Click += new System.EventHandler(this.RemovePic_Click);
            // 
            // RulesView
            // 
            this.RulesView.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.RulesView.LineColor = System.Drawing.Color.SlateGray;
            this.RulesView.Location = new System.Drawing.Point(80, 34);
            this.RulesView.Name = "RulesView";
            this.RulesView.Size = new System.Drawing.Size(530, 235);
            this.RulesView.TabIndex = 22;
            // 
            // SwitchPic
            // 
            this.SwitchPic.BackColor = System.Drawing.Color.Transparent;
            this.SwitchPic.BackgroundImage = global::IntelliTool.Properties.Resources.icon_small;
            this.SwitchPic.Location = new System.Drawing.Point(29, 282);
            this.SwitchPic.Name = "SwitchPic";
            this.SwitchPic.Size = new System.Drawing.Size(25, 25);
            this.SwitchPic.TabIndex = 23;
            this.SwitchPic.TabStop = false;
            this.SwitchPic.Click += new System.EventHandler(this.SwitchPic_Click);
            // 
            // InputBox
            // 
            this.InputBox.Location = new System.Drawing.Point(220, 285);
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(121, 23);
            this.InputBox.TabIndex = 24;
            // 
            // TargText
            // 
            this.TargText.AutoSize = true;
            this.TargText.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TargText.Location = new System.Drawing.Point(124, 284);
            this.TargText.Name = "TargText";
            this.TargText.Size = new System.Drawing.Size(90, 21);
            this.TargText.TabIndex = 25;
            this.TargText.Text = "前置字符：";
            // 
            // RuleBox
            // 
            this.RuleBox.AutoSize = true;
            this.RuleBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.RuleBox.Location = new System.Drawing.Point(371, 284);
            this.RuleBox.Name = "RuleBox";
            this.RuleBox.Size = new System.Drawing.Size(58, 21);
            this.RuleBox.TabIndex = 26;
            this.RuleBox.Text = "规则：";
            this.RuleBox.Visible = false;
            // 
            // RulesCombo
            // 
            this.RulesCombo.FormattingEnabled = true;
            this.RulesCombo.Location = new System.Drawing.Point(435, 283);
            this.RulesCombo.Name = "RulesCombo";
            this.RulesCombo.Size = new System.Drawing.Size(121, 25);
            this.RulesCombo.TabIndex = 27;
            this.RulesCombo.Visible = false;
            // 
            // Numberbox
            // 
            this.Numberbox.Location = new System.Drawing.Point(220, 285);
            this.Numberbox.Name = "Numberbox";
            this.Numberbox.Size = new System.Drawing.Size(120, 23);
            this.Numberbox.TabIndex = 28;
            this.Numberbox.Visible = false;
            // 
            // FileButton
            // 
            this.FileButton.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FileButton.Location = new System.Drawing.Point(106, 323);
            this.FileButton.Name = "FileButton";
            this.FileButton.Size = new System.Drawing.Size(161, 44);
            this.FileButton.TabIndex = 29;
            this.FileButton.Text = "选择文件";
            this.FileButton.UseVisualStyleBackColor = true;
            this.FileButton.Click += new System.EventHandler(this.FileButton_Click);
            // 
            // DirButton
            // 
            this.DirButton.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.DirButton.Location = new System.Drawing.Point(371, 323);
            this.DirButton.Name = "DirButton";
            this.DirButton.Size = new System.Drawing.Size(161, 44);
            this.DirButton.TabIndex = 30;
            this.DirButton.Text = "选择文件夹";
            this.DirButton.UseVisualStyleBackColor = true;
            this.DirButton.Click += new System.EventHandler(this.DirButton_Click);
            // 
            // WordSearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 379);
            this.Controls.Add(this.DirButton);
            this.Controls.Add(this.FileButton);
            this.Controls.Add(this.Numberbox);
            this.Controls.Add(this.RulesCombo);
            this.Controls.Add(this.RuleBox);
            this.Controls.Add(this.TargText);
            this.Controls.Add(this.InputBox);
            this.Controls.Add(this.SwitchPic);
            this.Controls.Add(this.RulesView);
            this.Controls.Add(this.RemovePic);
            this.Controls.Add(this.AddPic);
            this.Name = "WordSearchForm";
            this.Text = "WordSearchForm";
            this.Controls.SetChildIndex(this.MainPanel, 0);
            this.Controls.SetChildIndex(this.AddPic, 0);
            this.Controls.SetChildIndex(this.RemovePic, 0);
            this.Controls.SetChildIndex(this.RulesView, 0);
            this.Controls.SetChildIndex(this.SwitchPic, 0);
            this.Controls.SetChildIndex(this.InputBox, 0);
            this.Controls.SetChildIndex(this.TargText, 0);
            this.Controls.SetChildIndex(this.RuleBox, 0);
            this.Controls.SetChildIndex(this.RulesCombo, 0);
            this.Controls.SetChildIndex(this.Numberbox, 0);
            this.Controls.SetChildIndex(this.FileButton, 0);
            this.Controls.SetChildIndex(this.DirButton, 0);
            this.TopPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ClosePic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinimunPic)).EndInit();
            this.MainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AddPic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RemovePic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SwitchPic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Numberbox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox AddPic;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.PictureBox RemovePic;
        private System.Windows.Forms.TreeView RulesView;
        private System.Windows.Forms.PictureBox SwitchPic;
        private System.Windows.Forms.TextBox InputBox;
        private System.Windows.Forms.Label TargText;
        private System.Windows.Forms.Label RuleBox;
        private System.Windows.Forms.ComboBox RulesCombo;
        private System.Windows.Forms.NumericUpDown Numberbox;
        private System.Windows.Forms.Button FileButton;
        private System.Windows.Forms.Button DirButton;
    }
}