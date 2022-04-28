
namespace IntelliTool
{
    abstract partial class BaseForm 
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
            this.TopPanel = new IntelliTool.SupForms.TransparentPanel();
            this.MinimunPic = new System.Windows.Forms.PictureBox();
            this.ClosePic = new System.Windows.Forms.PictureBox();
            this.MainPanel = new IntelliTool.SupForms.TransparentPanel();
            this.ContentPanel = new IntelliTool.SupForms.TransparentPanel();
            this.TopPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinimunPic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClosePic)).BeginInit();
            this.MainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // TopPanel
            // 
            this.TopPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TopPanel.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.TopPanel.Controls.Add(this.MinimunPic);
            this.TopPanel.Controls.Add(this.ClosePic);
            this.TopPanel.Location = new System.Drawing.Point(0, 0);
            this.TopPanel.Margin = new System.Windows.Forms.Padding(0);
            this.TopPanel.MaximumSize = new System.Drawing.Size(1920, 53);
            this.TopPanel.MinimumSize = new System.Drawing.Size(50, 30);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(559, 53);
            this.TopPanel.TabIndex = 0;
            this.TopPanel.Text = "Top";
            // 
            // MinimunPic
            // 
            this.MinimunPic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MinimunPic.BackColor = System.Drawing.SystemColors.Control;
            this.MinimunPic.BackgroundImage = global::IntelliTool.Properties.Resources.MINIMUN_Button_Small;
            this.MinimunPic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.MinimunPic.Location = new System.Drawing.Point(455, 0);
            this.MinimunPic.Margin = new System.Windows.Forms.Padding(0);
            this.MinimunPic.Name = "MinimunPic";
            this.MinimunPic.Size = new System.Drawing.Size(52, 23);
            this.MinimunPic.TabIndex = 3;
            this.MinimunPic.TabStop = false;
            this.MinimunPic.Click += new System.EventHandler(this.MinimunPic_Click);
            this.MinimunPic.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MinimunPic_Prepare);
            this.MinimunPic.MouseEnter += new System.EventHandler(this.MinimunPic_Hover);
            this.MinimunPic.MouseLeave += new System.EventHandler(this.MinimunPic_Leave);
            // 
            // ClosePic
            // 
            this.ClosePic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ClosePic.BackColor = System.Drawing.SystemColors.Control;
            this.ClosePic.BackgroundImage = global::IntelliTool.Properties.Resources.CLOSE_Button_Small;
            this.ClosePic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClosePic.Location = new System.Drawing.Point(506, 0);
            this.ClosePic.Margin = new System.Windows.Forms.Padding(0);
            this.ClosePic.Name = "ClosePic";
            this.ClosePic.Size = new System.Drawing.Size(52, 23);
            this.ClosePic.TabIndex = 2;
            this.ClosePic.TabStop = false;
            this.ClosePic.Click += new System.EventHandler(this.ClosePic_Click);
            this.ClosePic.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ClosePic_Prepare);
            this.ClosePic.MouseEnter += new System.EventHandler(this.ClosePic_Hover);
            this.ClosePic.MouseLeave += new System.EventHandler(this.ClosePic_Leave);
            // 
            // MainPanel
            // 
            this.MainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainPanel.Controls.Add(this.ContentPanel);
            this.MainPanel.Controls.Add(this.TopPanel);
            this.MainPanel.Location = new System.Drawing.Point(1, 1);
            this.MainPanel.Margin = new System.Windows.Forms.Padding(1);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(557, 390);
            this.MainPanel.TabIndex = 5;
            this.MainPanel.Text = "MainPanel";
            // 
            // ContentPanel
            // 
            this.ContentPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ContentPanel.Location = new System.Drawing.Point(0, 53);
            this.ContentPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ContentPanel.Name = "ContentPanel";
            this.ContentPanel.Size = new System.Drawing.Size(559, 338);
            this.ContentPanel.TabIndex = 4;
            this.ContentPanel.Text = "transparentPanel1";
            // 
            // BaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 392);
            this.Controls.Add(this.MainPanel);
            this.Name = "BaseForm";
            this.Text = "TestForm";
            this.TopPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MinimunPic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClosePic)).EndInit();
            this.MainPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        protected SupForms.TransparentPanel TopPanel;
        protected System.Windows.Forms.PictureBox ClosePic;
        protected System.Windows.Forms.PictureBox MinimunPic;
        protected SupForms.TransparentPanel MainPanel;
        protected SupForms.TransparentPanel ContentPanel;
    }
}