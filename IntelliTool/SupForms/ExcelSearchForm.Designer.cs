
using System.Drawing;

namespace IntelliTool.SupForms
{
    partial class ExcelSearchForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TitleLable = new System.Windows.Forms.Label();
            this.TitleEndLable = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.Page = new System.Windows.Forms.NumericUpDown();
            this.DataEndRow = new System.Windows.Forms.NumericUpDown();
            this.TitleRow = new System.Windows.Forms.NumericUpDown();
            this.TitleEndRow = new System.Windows.Forms.NumericUpDown();
            this.DataStartRow = new System.Windows.Forms.NumericUpDown();
            this.StartCol = new System.Windows.Forms.NumericUpDown();
            this.EndCol = new System.Windows.Forms.NumericUpDown();
            this.FileDialog = new System.Windows.Forms.OpenFileDialog();
            this.FileUpLoad = new System.Windows.Forms.Button();
            this.KeyCol = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.AddBox = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.MainTable = new System.Windows.Forms.CheckBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.TopPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClosePic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinimunPic)).BeginInit();
            this.MainPanel.SuspendLayout();
            this.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Page)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataEndRow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TitleRow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TitleEndRow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataStartRow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartCol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndCol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.KeyCol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // TopPanel
            // 
            this.TopPanel.Size = new System.Drawing.Size(549, 30);
            // 
            // ClosePic
            // 
            this.ClosePic.Location = new System.Drawing.Point(496, 0);
            // 
            // MinimunPic
            // 
            this.MinimunPic.Location = new System.Drawing.Point(445, 0);
            // 
            // MainPanel
            // 
            this.MainPanel.Size = new System.Drawing.Size(549, 328);
            // 
            // ContentPanel
            // 
            this.ContentPanel.Controls.Add(this.panel1);
            this.ContentPanel.Location = new System.Drawing.Point(0, 30);
            this.ContentPanel.Size = new System.Drawing.Size(551, 299);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(34, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "起始列：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(34, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 21);
            this.label2.TabIndex = 2;
            this.label2.Text = "结束列：";
            // 
            // TitleLable
            // 
            this.TitleLable.AutoSize = true;
            this.TitleLable.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TitleLable.Location = new System.Drawing.Point(259, 107);
            this.TitleLable.Name = "TitleLable";
            this.TitleLable.Size = new System.Drawing.Size(104, 21);
            this.TitleLable.TabIndex = 4;
            this.TitleLable.Text = "      标题行：";
            // 
            // TitleEndLable
            // 
            this.TitleEndLable.AutoSize = true;
            this.TitleEndLable.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TitleEndLable.Location = new System.Drawing.Point(259, 142);
            this.TitleEndLable.Name = "TitleEndLable";
            this.TitleEndLable.Size = new System.Drawing.Size(106, 21);
            this.TitleEndLable.TabIndex = 6;
            this.TitleEndLable.Text = "标题结束行：";
            this.TitleEndLable.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::IntelliTool.Properties.Resources.icon_small;
            this.pictureBox1.Location = new System.Drawing.Point(477, 107);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(25, 25);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.PictureBox1_Click);
            this.pictureBox1.MouseLeave += new System.EventHandler(this.SwitchBox_Leave);
            this.pictureBox1.MouseHover += new System.EventHandler(this.SwitchBox_Hover);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(259, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 21);
            this.label5.TabIndex = 10;
            this.label5.Text = "数据起始行：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(259, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 21);
            this.label6.TabIndex = 12;
            this.label6.Text = "数据结束行：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label7.Location = new System.Drawing.Point(34, 67);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 21);
            this.label7.TabIndex = 14;
            this.label7.Text = "所在页：";
            // 
            // Page
            // 
            this.Page.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.Page.Location = new System.Drawing.Point(114, 66);
            this.Page.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Page.Name = "Page";
            this.Page.Size = new System.Drawing.Size(100, 23);
            this.Page.TabIndex = 1;
            this.Page.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // DataEndRow
            // 
            this.DataEndRow.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.DataEndRow.Location = new System.Drawing.Point(371, 68);
            this.DataEndRow.Name = "DataEndRow";
            this.DataEndRow.Size = new System.Drawing.Size(100, 23);
            this.DataEndRow.TabIndex = 5;
            this.DataEndRow.Maximum = int.MaxValue;
            // 
            // TitleRow
            // 
            this.TitleRow.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.TitleRow.Location = new System.Drawing.Point(371, 107);
            this.TitleRow.Name = "TitleRow";
            this.TitleRow.Size = new System.Drawing.Size(100, 23);
            this.TitleRow.TabIndex = 6;
            // 
            // TitleEndRow
            // 
            this.TitleEndRow.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.TitleEndRow.Location = new System.Drawing.Point(371, 144);
            this.TitleEndRow.Name = "TitleEndRow";
            this.TitleEndRow.Size = new System.Drawing.Size(100, 23);
            this.TitleEndRow.TabIndex = 7;
            this.TitleEndRow.Visible = false;
            // 
            // DataStartRow
            // 
            this.DataStartRow.Cursor = System.Windows.Forms.Cursors.Default;
            this.DataStartRow.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.DataStartRow.Location = new System.Drawing.Point(371, 30);
            this.DataStartRow.Name = "DataStartRow";
            this.DataStartRow.Size = new System.Drawing.Size(100, 23);
            this.DataStartRow.TabIndex = 4;
            // 
            // StartCol
            // 
            this.StartCol.Location = new System.Drawing.Point(114, 103);
            this.StartCol.Name = "StartCol";
            this.StartCol.Size = new System.Drawing.Size(100, 23);
            this.StartCol.TabIndex = 2;
            // 
            // EndCol
            // 
            this.EndCol.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.EndCol.Location = new System.Drawing.Point(114, 139);
            this.EndCol.Name = "EndCol";
            this.EndCol.Size = new System.Drawing.Size(100, 23);
            this.EndCol.TabIndex = 3;
            // 
            // FileDialog
            // 
            this.FileDialog.Filter = "(全部Excel)|*.xlsx;*.xls|(*.xlsx)|*.xlsx|(*.xls)|*.xls";
            this.FileDialog.RestoreDirectory = true;
            // 
            // FileUpLoad
            // 
            this.FileUpLoad.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FileUpLoad.Location = new System.Drawing.Point(295, 180);
            this.FileUpLoad.Name = "FileUpLoad";
            this.FileUpLoad.Size = new System.Drawing.Size(176, 93);
            this.FileUpLoad.TabIndex = 15;
            this.FileUpLoad.Text = "选择文件";
            this.FileUpLoad.UseVisualStyleBackColor = true;
            this.FileUpLoad.Click += new System.EventHandler(this.FileUpLoad_Click);
            // 
            // KeyCol
            // 
            this.KeyCol.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.KeyCol.Location = new System.Drawing.Point(114, 175);
            this.KeyCol.Name = "KeyCol";
            this.KeyCol.Size = new System.Drawing.Size(100, 23);
            this.KeyCol.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(34, 177);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 21);
            this.label3.TabIndex = 17;
            this.label3.Text = "主键列：";
            // 
            // AddBox
            // 
            this.AddBox.BackColor = System.Drawing.Color.Transparent;
            this.AddBox.BackgroundImage = global::IntelliTool.Properties.Resources.AddIcon_small1;
            this.AddBox.Location = new System.Drawing.Point(220, 173);
            this.AddBox.Name = "AddBox";
            this.AddBox.Size = new System.Drawing.Size(25, 25);
            this.AddBox.TabIndex = 18;
            this.AddBox.TabStop = false;
            this.AddBox.Click += new System.EventHandler(this.AddBox_Click);
            this.AddBox.MouseLeave += new System.EventHandler(this.AddBox_Leave);
            this.AddBox.MouseHover += new System.EventHandler(this.AddBox_Hover);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(50, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 21);
            this.label4.TabIndex = 19;
            this.label4.Text = "主表：";
            // 
            // MainTable
            // 
            this.MainTable.Font = new System.Drawing.Font("Microsoft YaHei UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MainTable.Location = new System.Drawing.Point(114, 27);
            this.MainTable.MaximumSize = new System.Drawing.Size(30, 30);
            this.MainTable.Name = "MainTable";
            this.MainTable.Size = new System.Drawing.Size(30, 30);
            this.MainTable.TabIndex = 0;
            this.MainTable.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::IntelliTool.Properties.Resources.blank;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox2.Image = global::IntelliTool.Properties.Resources.Alert_small2;
            this.pictureBox2.Location = new System.Drawing.Point(1, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(200, 97);
            this.pictureBox2.TabIndex = 22;
            this.pictureBox2.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.MainTable);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.AddBox);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.KeyCol);
            this.panel1.Controls.Add(this.EndCol);
            this.panel1.Controls.Add(this.StartCol);
            this.panel1.Controls.Add(this.DataStartRow);
            this.panel1.Controls.Add(this.TitleEndRow);
            this.panel1.Controls.Add(this.TitleRow);
            this.panel1.Controls.Add(this.DataEndRow);
            this.panel1.Controls.Add(this.Page);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.TitleEndLable);
            this.panel1.Controls.Add(this.TitleLable);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.FileUpLoad);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(527, 276);
            this.panel1.TabIndex = 24;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.pictureBox3);
            this.panel3.Location = new System.Drawing.Point(329, 47);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(200, 53);
            this.panel3.TabIndex = 24;
            this.panel3.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.White;
            this.label9.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label9.Location = new System.Drawing.Point(29, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(135, 20);
            this.label9.TabIndex = 1;
            this.label9.Text = "当有多行标题时切换";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::IntelliTool.Properties.Resources.Alert_flat;
            this.pictureBox3.Location = new System.Drawing.Point(0, 3);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(200, 54);
            this.pictureBox3.TabIndex = 0;
            this.pictureBox3.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.pictureBox2);
            this.panel2.Location = new System.Drawing.Point(108, 69);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 100);
            this.panel2.TabIndex = 23;
            this.panel2.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label8.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label8.Location = new System.Drawing.Point(23, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(149, 60);
            this.label8.TabIndex = 21;
            this.label8.Text = "主键列支持多列主键，\r\n降序检索，适用于主键\r\n可能重复的情况\r\n";
            this.label8.UseMnemonic = false;
            // 
            // ExcelSearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 329);
            this.Name = "ExcelSearchForm";
            this.Text = "ExcelSearchForm";
            this.TransparencyKey = System.Drawing.Color.Tan;
            this.TopPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ClosePic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinimunPic)).EndInit();
            this.MainPanel.ResumeLayout(false);
            this.ContentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Page)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataEndRow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TitleRow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TitleEndRow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataStartRow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartCol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndCol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.KeyCol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label TitleLable;
        private System.Windows.Forms.Label TitleEndLable;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown Page;
        private System.Windows.Forms.NumericUpDown DataEndRow;
        private System.Windows.Forms.NumericUpDown TitleRow;
        private System.Windows.Forms.NumericUpDown TitleEndRow;
        private System.Windows.Forms.NumericUpDown DataStartRow;
        private System.Windows.Forms.NumericUpDown StartCol;
        private System.Windows.Forms.NumericUpDown EndCol;
        private System.Windows.Forms.OpenFileDialog FileDialog;
        private System.Windows.Forms.Button FileUpLoad;
        private System.Windows.Forms.NumericUpDown KeyCol;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox AddBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox MainTable;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.PictureBox pictureBox3;
    }
}