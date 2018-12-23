using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TagLib;

namespace LRCDownload.Plugins
{
    public class Kugou
    {
        public string Name()
        {
            return "酷狗音乐";
        }
        public string Author()
        {
            return "built-in";
        }
        public string Version()
        {
            return "0.1";
        }

        public static async Task<string> GetLyricAsync(File tfile)
        {
            var artist = TagHelper.GetArtist(tfile);
            var title = TagHelper.GetTitle(tfile);
            var length = Math.Round(tfile.Properties.Duration.TotalMilliseconds);

            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36");

            Console.WriteLine($"http://lyrics.kugou.com/search?ver=1&man=yes&client=pc&keyword={Uri.EscapeDataString(artist)}-{Uri.EscapeDataString(title)}&duration={length}&hash=");

            var response = await client.GetAsync($"http://lyrics.kugou.com/search?ver=1&man=yes&client=pc&keyword={Uri.EscapeDataString(artist)}-{Uri.EscapeDataString(title)}&duration={length}&hash=");
            response.EnsureSuccessStatusCode();
            var responseBodyAsText = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(responseBodyAsText);
            IList<JToken> results = jObject["candidates"].Children().ToList();
            foreach (var result in results)
                try
                {
                    response = await client.GetAsync(
                        $"http://lyrics.kugou.com/download?ver=1&client=pc&id={result["id"]}&accesskey={result["accesskey"]}&fmt=lrc&charset=utf8");
                    response.EnsureSuccessStatusCode();
                    var lyricsAsText = await response.Content.ReadAsStringAsync();
                    var lyricsAsJson = JObject.Parse(lyricsAsText);
                    return UnBase64String((string) lyricsAsJson["content"]);
                }
                catch (NullReferenceException)
                {
                    //若没有歌词，则查找下一个
                }

            throw new Exception();
        }

        public static string UnBase64String(string value)
        {
            if (value == null || value == "")
            {
                return "";
            }
            byte[] bytes = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}