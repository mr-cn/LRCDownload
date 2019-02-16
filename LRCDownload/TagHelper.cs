using System.Linq;
using System.Text.RegularExpressions;
using TagLib;

namespace LRCDownload
{
    public static class TagHelper
    {
        public static string GetArtist(File tfile)
        {
            if (tfile.Tag.AlbumArtists.Any())
                return process_keywords(tfile.Tag.AlbumArtists[0]);
            return tfile.Tag.Artists.Any() ? process_keywords(tfile.Tag.Artists[0]) : null;
        }

        public static string GetAlbum(File tfile)
        {
            return string.IsNullOrWhiteSpace(tfile.Tag.Album) ? null : process_keywords(tfile.Tag.Album);
        }

        public static string GetTitle(File tfile)
        {
            return process_keywords(tfile.Tag.Title);
        }

        private static string process_keywords(string s)
        {
            s = Regex.Replace(s, @"/\'|·|\$|\&|–/g", string.Empty);
            //s = Regex.Replace(s, @"/\(.*?\)|\[.*?]|{.*?}|（.*?/g", string.Empty);
            s = Regex.Replace(s, @"/[-/:-@[-`{-~]+/g", string.Empty);
            s = Regex.Replace(s,
                @"/[\u2014\u2018\u201c\u2026\u3001\u3002\u300a\u300b\u300e\u300f\u3010\u3011\u30fb\uff01\uff08\uff09\uff0c\uff1a\uff1b\uff1f\uff5e\uffe5]+/g",
                string.Empty);
            return s;
        }
    }
}