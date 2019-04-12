using System.IO;
using File = TagLib.File;

namespace LRCDownload
{
    public class Music
    {
        private File tag;
        private FileInfo file;

        private Music(File tag, FileInfo file)
        {
            this.tag = tag;
            this.file = file;
        }

        public static Music CreateFromPath(string path)
        {
            var tfile = File.Create(path);
            var finfo = new FileInfo(path);
            return new Music(tfile,finfo);
        }

        public static Music CreateFromFile(FileInfo file)
        {
            var tfile = File.Create(file.FullName);
            return new Music(tfile,file);
        }

        public bool HasProperTag()
        {
            return !string.IsNullOrWhiteSpace(tag.Tag.Title);
        }

        public bool ExistLyrics()
        {
            return new FileInfo(Path.ChangeExtension(file.FullName, ".lrc")).Exists;
        }

        public File GetMetadata()
        {
            return tag;
        }

        public string GetPath()
        {
            return file.FullName;
        }
    }
}