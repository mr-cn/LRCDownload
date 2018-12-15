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
        private string deviceDirectory;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                deviceDirectory = folderBrowserDialog1.SelectedPath;
                listView1.Items.Clear();
                DirectoryInfo TheFolder = new DirectoryInfo(deviceDirectory);
                var files = TheFolder.GetFiles("*", SearchOption.AllDirectories)
                    .Where(file => file.Name.ToLower().EndsWith(".flac") || file.Name.ToLower().EndsWith(".m4a") || file.Name.ToLower().EndsWith(".wav") || file.Name.ToLower().EndsWith(".mp3"))
                    .ToList();
                foreach (FileInfo NextItem in files)
                {
                    try
                    {
                        var tfile = TagLib.File.Create(NextItem.FullName);
                        string artist = tfile.Tag.AlbumArtists.Count() > 0 ? tfile.Tag.AlbumArtists[0] : tfile.Tag.Artists[0];

                        var item = new ListViewItem("");
                        item.SubItems.Add(tfile.Tag.Title);
                        item.SubItems.Add(artist);
                        item.SubItems.Add(NextItem.FullName);
                        this.listView1.Items.Add(item);
                    }
                    catch (IndexOutOfRangeException ex)
                    {

                    }
                }
            }
        }

        private async void btnDown_Click(object sender, EventArgs e)
        {
            btnDown.Enabled = false;
            var tasks = new List<Tuple<Task<string>, ListViewItem>>();
            foreach (ListViewItem NextItem in listView1.Items)
            {
                string title = NextItem.SubItems[1].Text;
                string artist = NextItem.SubItems[2].Text;
                tasks.Add(Tuple.Create(Plugins.Netease.getLyric(artist, title), NextItem));
            }
            while (tasks.Count > 0)
            {
                var currentCompleted = await Task.WhenAny(tasks.Select(x => x.Item1).ToList());
                var task = tasks.First(x => x.Item1.Equals(currentCompleted));
                ListViewItem listViewItem = task.Item2;
                try
                {
                    string lryrics = currentCompleted.Result;
                    FileInfo mfile = new FileInfo(listViewItem.SubItems[3].Text);
                    File.WriteAllText(string.Format(@"{0}/{1}.lrc", mfile.DirectoryName, Path.GetFileNameWithoutExtension(mfile.Name)), lryrics);
                    listViewItem.SubItems[0].Text = "✔";
                }
                catch (AggregateException ex)
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
