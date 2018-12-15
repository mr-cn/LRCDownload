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
        private string _deviceDirectory;

        public Form1()
        {
            InitializeComponent();
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK)
                return;
            _deviceDirectory = folderBrowserDialog1.SelectedPath;
            listView1.Items.Clear();
            var theFolder = new DirectoryInfo(_deviceDirectory);
            var files = theFolder.GetFiles("*", SearchOption.AllDirectories)
                .Where(file => file.Name.ToLower().EndsWith(".flac"))
                .Where(file => file.Name.ToLower().EndsWith(".m4a"))
                .Where(file => file.Name.ToLower().EndsWith(".wav"))
                .Where(file => file.Name.ToLower().EndsWith(".mp3"))
                .ToList();
            foreach (var nextItem in files)
            {
                try
                {
                    var tfile = TagLib.File.Create(nextItem.FullName);
                    var artist = tfile.Tag.AlbumArtists.Any() ? tfile.Tag.AlbumArtists[0] : tfile.Tag.Artists[0];

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
                var title = nextItem.SubItems[1].Text;
                var artist = nextItem.SubItems[2].Text;
                tasks.Add(Tuple.Create(Plugins.Netease.getLyric(artist, title), nextItem));
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
