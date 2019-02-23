using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LRCDownload.Clients;
using File = TagLib.File;

namespace LRCDownload
{
    public partial class MainForm: Form
    {
        /// <summary>
        ///     将被识别的音乐文件的后缀名
        /// </summary>
        private readonly string[] _exts = {"*.flac", "*.m4a", "*.mp3", "*.wav"};

        public MainForm()
        {
            InitializeComponent();
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK) return;

            var deviceDirectory = folderBrowserDialog1.SelectedPath;
            var folder = new DirectoryInfo(deviceDirectory);
            var checkSub = checkBox_searchSubDir.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            listView.Items.Clear();
            foreach (var nextItem in _exts.SelectMany(x => folder.EnumerateFiles(x, checkSub)))
            {
                var tagFile = File.Create(nextItem.FullName);
                if (string.IsNullOrWhiteSpace(tagFile.Tag.Title)) // 跳过没有元数据的歌曲
                    continue;
                if (new FileInfo(Path.ChangeExtension(nextItem.FullName, ".lrc")).Exists) // 跳过已经有歌词的歌曲
                    continue;
                var artist = TagHelper.GetArtist(tagFile);
                var item = new ListViewItem("");
                item.SubItems.Add(tagFile.Tag.Title);
                item.SubItems.Add(artist);
                item.SubItems.Add(nextItem.FullName);
                listView.Items.Add(item);
            }
        }

        private async Task ProcessTasksAsync(List<TaskStruct> tasks)
        {
            // 使用 SemaphoreSlim 限流，防止访问过快被 Ban
            var throttler = new SemaphoreSlim(5);

            var processingTasks = tasks.Select(async i =>
            {
                try
                {
                    var result = await i.Client.GetLyricAsync();
                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        var lrcFile = new FileInfo(i.ViewItem.SubItems[3].Text);
                        System.IO.File.WriteAllText(
                            $"{lrcFile.DirectoryName}/{Path.GetFileNameWithoutExtension(lrcFile.Name)}.lrc",
                            result);
                        i.ViewItem.SubItems[0].Text = "✔";
                    }
                    else
                    {
                        i.ViewItem.SubItems[0].Text = "✖";
                        i.ViewItem.BackColor = Color.Red;
                        i.ViewItem.ForeColor = Color.White;
                    }
                }
                finally
                {
                    throttler.Release();
                }
            }).ToArray();

            // 等待全部处理过程的完成。
            await Task.WhenAll(processingTasks);
            btnDown.Enabled = true;
            MessageBox.Show("下载完毕", "Finished.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            btnDown.Enabled = false;
            var tasks = new List<TaskStruct>();
            foreach (ListViewItem nextItem in listView.Items)
            {
                var tagFile = File.Create(nextItem.SubItems[3].Text);
                IClient client = new Netease(tagFile);
                tasks.Add(new TaskStruct(nextItem, client));
            }

            ProcessTasksAsync(tasks);
        }

        private struct TaskStruct
        {
            public readonly ListViewItem ViewItem;
            public readonly IClient Client;

            public TaskStruct(ListViewItem item, IClient client)
            {
                ViewItem = item;
                Client = client;
            }
        }
    }
}