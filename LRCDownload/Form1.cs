using LRCDownload.Clients;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LRCDownload
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Extensions which represents a music file
        /// </summary>
        private readonly string[] _exts = { "*.flac", "*.m4a", "*.mp3", "*.wav" };

        public Form1()
        {
            InitializeComponent();
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var deviceDirectory = folderBrowserDialog1.SelectedPath;
            var folder = new DirectoryInfo(deviceDirectory);
            var checkSub = checkBox_searchSubDir.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            listView.Items.Clear();
            foreach (var nextItem in _exts.SelectMany(x => folder.EnumerateFiles(x, checkSub)))
            {
                var tagFile = TagLib.File.Create(nextItem.FullName);
                var artist = TagHelper.GetArtist(tagFile);
                var item = new ListViewItem("");
                item.SubItems.Add(tagFile.Tag.Title);
                item.SubItems.Add(artist);
                item.SubItems.Add(nextItem.FullName);
                listView.Items.Add(item);
            }
        }

        struct TaskStruct
        {
            public ListViewItem view;
            public Task<string> task;

            public TaskStruct(ListViewItem item, Task<string> task)
            {
                view = item;
                this.task = task;
            }
        }

        async Task ProcessTasksAsync(List<TaskStruct> tasks)
        {
            var processingTasks = tasks.Select(async s =>
            {
                var result = await s.task;

                var lrcFile = new FileInfo(s.view.SubItems[3].Text);
                File.WriteAllText($"{lrcFile.DirectoryName}/{Path.GetFileNameWithoutExtension(lrcFile.Name)}.lrc", result);
                s.view.SubItems[0].Text = "✔";
               
            }).ToArray();

            // 等待全部处理过程的完成。
            await Task.WhenAll(processingTasks);
            btnDown.Enabled = true;
            MessageBox.Show("The process has been done.", "Finished.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            btnDown.Enabled = false;
            var tasks = new List<TaskStruct>();
            foreach (ListViewItem nextItem in listView.Items)
            {
                var tagFile = TagLib.File.Create(nextItem.SubItems[3].Text);
                var client = new Netease(tagFile);
                tasks.Add(new TaskStruct(nextItem, client.GetLyricAsync()));
            }
            ProcessTasksAsync(tasks);
        }
    }
}
