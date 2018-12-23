using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LRCDownload
{
    public partial class Form1 : Form
    {
        private string[] _exts = { "*.flac", "*.m4a", "*.mp3", "*.wav" };

        public Form1()
        {
            InitializeComponent();
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK)
                return;
            var deviceDirectory = folderBrowserDialog1.SelectedPath;
            var folder = new DirectoryInfo(deviceDirectory);
            var sub = checkBox_searchSubDir.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            listView1.Items.Clear();
            var files = _exts.SelectMany(x => folder.EnumerateFiles(x,sub));
            foreach (var nextItem in files)
            {
                try
                {
                    var tfile = TagLib.File.Create(nextItem.FullName);
                    var artist = TagHelper.GetArtist(tfile);

                    var item = new ListViewItem("");
                    item.SubItems.Add(tfile.Tag.Title);
                    item.SubItems.Add(artist);
                    item.SubItems.Add(nextItem.FullName);
                    listView1.Items.Add(item);
                }
                catch (IndexOutOfRangeException)
                {  }
            }
        }

        private async void BtnDown_Click(object sender, EventArgs e)
        {
            btnDown.Enabled = false;
            var tasks = new List<Tuple<Task<string>, ListViewItem>>();
            foreach (ListViewItem nextItem in listView1.Items)
            {
                var tfile = TagLib.File.Create(nextItem.SubItems[3].Text);
                tasks.Add(Tuple.Create(Plugins.Kugou.GetLyricAsync(tfile), nextItem));
            }
            while (tasks.Count > 0)
            {
                var currentCompleted = await Task.WhenAny(tasks.Select(x => x.Item1).ToList());
                var task = tasks.First(x => x.Item1.Equals(currentCompleted));
                var listViewItem = task.Item2;
                try
                {
                    var lryrics = currentCompleted.Result;
                    var mfile = new FileInfo(listViewItem.SubItems[3].Text);
                    File.WriteAllText($"{mfile.DirectoryName}/{Path.GetFileNameWithoutExtension(mfile.Name)}.lrc", lryrics);
                    listViewItem.SubItems[0].Text = "✔";
                }
                catch (AggregateException)
                {
                    listViewItem.BackColor = System.Drawing.Color.Red;
                    listViewItem.ForeColor = System.Drawing.Color.White;
                }
                tasks.Remove(task);
            }
            btnDown.Enabled = true;
            MessageBox.Show("The process has been done.", "Finished.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
    }
}
