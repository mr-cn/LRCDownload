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
    public partial class MainForm : Form
    {
        /// <summary>
        ///     将被识别的音乐文件的后缀名
        /// </summary>
        private readonly string[] _exts = {"*.flac", "*.m4a", "*.mp3", "*.wav"};

        /// <summary>
        /// 储存将被处理的音乐
        /// </summary>
        List<Music> Musics = new List<Music>();

        public MainForm()
        {
            InitializeComponent();
            foreach (var client in ClientsManager.Clients)
            {
                comboBox1.Items.Add(client.Key);
                LogView.Items.Add($"[Main|{DateTime.Now.ToString("g")}] 加载插件: {client.Value.Name()}");
            }

            comboBox1.SelectedIndex = 0;
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK) return;

            var deviceDirectory = folderBrowserDialog1.SelectedPath;
            var folder = new DirectoryInfo(deviceDirectory);
            var checkSub = checkBox_searchSubDir.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (var nextItem in _exts.SelectMany(x => folder.EnumerateFiles(x, checkSub)))
            {
                var music = Music.CreateFromFile(nextItem);
                if (!music.HasProperTag()) // 跳过没有元数据的歌曲
                {
                    LogView.Items.Add($"[Main|{DateTime.Now.ToString("g")}] 添加时跳过:\"{nextItem.Name}\" 原因是：没有正确的元数据");
                    continue;
                }
                if (music.ExistLyrics()) // 跳过已经有歌词的歌曲
                {
                    // LogView.Items.Add($"[Main|{DateTime.Now.ToString("g")}] 添加时跳过:\"{nextItem.Name}\" 原因是：已经有对应的歌词文件存在");
                    continue;
                }
                Musics.Add(music);
            }
            LogView.Items.Add($"[Main|{DateTime.Now.ToString("g")}] 已成功添加: {Musics.Count} 项");
        }

        private async Task ProcessTasksAsync(List<Music> tasks)
        {
            // 成功计数器
            var successCount = 0;
            // 使用 SemaphoreSlim 限流，防止访问过快被 Ban
            var throttler = new SemaphoreSlim(5);

            var processingTasks = tasks.Select(async i =>
            {
                try
                {
                    var client = ClientsManager.Clients[comboBox1.SelectedItem.ToString()];
                    var result = await client.GetLyricAsync(i.GetMetadata()).ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        System.IO.File.WriteAllText(Path.ChangeExtension(i.GetPath(), ".lrc"), result);
                        successCount++;
                        Musics.Remove(i);
                    }
                    else
                    {
                        LogView.Items.Add($"[Main|{DateTime.Now.ToString("g")}] 歌词获取失败: 歌曲{i.GetMetadata().Tag.Title}");
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
            LogView.Items.Add($"[Main|{DateTime.Now.ToString("g")}] {successCount} 项歌曲的歌词已成功下载。");
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            btnDown.Enabled = false;
            ProcessTasksAsync(Musics);
        }
    }
}