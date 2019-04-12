namespace LRCDownload
{
    partial class MainForm
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
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnAddFolder = new System.Windows.Forms.Button();
            this.checkBox_searchSubDir = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox_replaceAlbumArtist = new System.Windows.Forms.CheckBox();
            this.btnDown = new System.Windows.Forms.Button();
            this.LogView = new System.Windows.Forms.ListBox();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "Choose a directory. All files under the direcrory (including subdirectories) will" +
    " be added.";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.checkBox_replaceAlbumArtist);
            this.panel1.Controls.Add(this.btnDown);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(747, 156);
            this.panel1.TabIndex = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(335, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 100);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "数据源";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(27, 51);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(147, 20);
            this.comboBox1.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "数据源";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.btnAddFolder);
            this.groupBox1.Controls.Add(this.checkBox_searchSubDir);
            this.groupBox1.Location = new System.Drawing.Point(21, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(291, 100);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "添加歌曲";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(17, 26);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(115, 38);
            this.button1.TabIndex = 10;
            this.button1.Text = "添加文件";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnAddFolder
            // 
            this.btnAddFolder.Location = new System.Drawing.Point(148, 26);
            this.btnAddFolder.Name = "btnAddFolder";
            this.btnAddFolder.Size = new System.Drawing.Size(111, 38);
            this.btnAddFolder.TabIndex = 5;
            this.btnAddFolder.Text = "添加目录";
            this.btnAddFolder.UseVisualStyleBackColor = true;
            this.btnAddFolder.Click += new System.EventHandler(this.BtnSelect_Click);
            // 
            // checkBox_searchSubDir
            // 
            this.checkBox_searchSubDir.AutoSize = true;
            this.checkBox_searchSubDir.Checked = true;
            this.checkBox_searchSubDir.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_searchSubDir.Location = new System.Drawing.Point(17, 72);
            this.checkBox_searchSubDir.Name = "checkBox_searchSubDir";
            this.checkBox_searchSubDir.Size = new System.Drawing.Size(84, 16);
            this.checkBox_searchSubDir.TabIndex = 7;
            this.checkBox_searchSubDir.Text = "查找子目录";
            this.checkBox_searchSubDir.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 132);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "日志";
            // 
            // checkBox_replaceAlbumArtist
            // 
            this.checkBox_replaceAlbumArtist.Location = new System.Drawing.Point(570, 84);
            this.checkBox_replaceAlbumArtist.Name = "checkBox_replaceAlbumArtist";
            this.checkBox_replaceAlbumArtist.Size = new System.Drawing.Size(147, 28);
            this.checkBox_replaceAlbumArtist.TabIndex = 8;
            this.checkBox_replaceAlbumArtist.Text = "附加:使用专辑演唱者替换演唱者";
            this.checkBox_replaceAlbumArtist.UseVisualStyleBackColor = true;
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(570, 28);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(147, 38);
            this.btnDown.TabIndex = 6;
            this.btnDown.Text = "开始下载";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.BtnDown_Click);
            // 
            // LogView
            // 
            this.LogView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogView.FormattingEnabled = true;
            this.LogView.ItemHeight = 12;
            this.LogView.Location = new System.Drawing.Point(0, 156);
            this.LogView.Name = "LogView";
            this.LogView.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.LogView.Size = new System.Drawing.Size(747, 202);
            this.LogView.TabIndex = 6;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 358);
            this.Controls.Add(this.LogView);
            this.Controls.Add(this.panel1);
            this.Name = "MainForm";
            this.Text = "LRCDownload";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnAddFolder;
        private System.Windows.Forms.CheckBox checkBox_searchSubDir;
        private System.Windows.Forms.CheckBox checkBox_replaceAlbumArtist;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox LogView;
    }
}

