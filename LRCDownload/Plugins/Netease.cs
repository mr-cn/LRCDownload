using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TagLib;

namespace LRCDownload.Plugins
{
    public class Netease
    {
        public string Name()
        {
            return "网易云音乐";
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
            var album = TagHelper.GetAlbum(tfile);

            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Referer", "http://music.163.com/");
            client.DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36");
            client.DefaultRequestHeaders.Add("Cookie", "appver=2.0.2");

            var stringContent = new StringContent($"s={artist}+{title}+{album}&limit=15&type=1&offset=0");
            stringContent.Headers.Clear();
            stringContent.Headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");
            var response = await client.PostAsync("http://music.163.com/api/search/get/", stringContent);

            response.EnsureSuccessStatusCode();
            var responseBodyAsText = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(responseBodyAsText);
            IList<JToken> results = jObject["result"]["songs"].Children().ToList();
            foreach (var result in results)
                try
                {
                    response = await client.GetAsync(
                        $"http://music.163.com/api/song/lyric?os=pc&id={result["id"]}&lv=-1&kv=-1&tv=-1");
                    response.EnsureSuccessStatusCode();
                    var lyricsAsText = await response.Content.ReadAsStringAsync();
                    var lyricsAsJson = JObject.Parse(lyricsAsText);
                    return (string) lyricsAsJson["lrc"]["lyric"];
                }
                catch (NullReferenceException)
                {
                    // 若没有歌词，则查找下一个
                }

            throw new Exception(); // 找遍了都没有
        }
    }
}