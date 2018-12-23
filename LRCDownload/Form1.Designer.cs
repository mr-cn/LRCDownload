namespace LRCDownload
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnCheck = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnArtist = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.checkBox_searchSubDir = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnCheck,
            this.columnTitle,
            this.columnArtist,
            this.columnFile});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(887, 502);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnCheck
            // 
            this.columnCheck.Text = "";
            this.columnCheck.Width = 37;
            // 
            // columnTitle
            // 
            this.columnTitle.Text = "Title";
            this.columnTitle.Width = 236;
            // 
            // columnArtist
            // 
            this.columnArtist.Text = "Artist";
            this.columnArtist.Width = 129;
            // 
            // columnFile
            // 
            this.columnFile.Text = "Path";
            this.columnFile.Width = 437;
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "Choose a directory. All files under the direcrory (including subdirectories) will" +
    " be added.";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkBox_searchSubDir);
            this.panel1.Controls.Add(this.btnDown);
            this.panel1.Controls.Add(this.btnSelect);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(887, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(125, 502);
            this.panel1.TabIndex = 5;
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(6, 100);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(111, 58);
            this.btnDown.TabIndex = 6;
            this.btnDown.Text = "开始下载";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.BtnDown_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(6, 12);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(111, 60);
            this.btnSelect.TabIndex = 5;
            this.btnSelect.Text = "选择歌曲目录 ..";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.BtnSelect_Click);
            // 
            // checkBox_searchSubDir
            // 
            this.checkBox_searchSubDir.AutoSize = true;
            this.checkBox_searchSubDir.Checked = true;
            this.checkBox_searchSubDir.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_searchSubDir.Location = new System.Drawing.Point(6, 78);
            this.checkBox_searchSubDir.Name = "checkBox_searchSubDir";
            this.checkBox_searchSubDir.Size = new System.Drawing.Size(84, 16);
            this.checkBox_searchSubDir.TabIndex = 7;
            this.checkBox_searchSubDir.Text = "查找子目录";
            this.checkBox_searchSubDir.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 502);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "LRCDownload";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnCheck;
        private System.Windows.Forms.ColumnHeader columnTitle;
        private System.Windows.Forms.ColumnHeader columnArtist;
        private System.Windows.Forms.ColumnHeader columnFile;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.CheckBox checkBox_searchSubDir;
    }
}

