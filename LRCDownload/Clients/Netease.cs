using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TagLib;

namespace LRCDownload.Clients
{
    class Netease : IClient
    {
        public Netease(File metadata)
        {
            Metadata = metadata;
        }

        public File Metadata { get; }

        private string Artist => TagHelper.GetArtist(Metadata);
        private string Title => TagHelper.GetTitle(Metadata);
        private string Album => TagHelper.GetAlbum(Metadata);

        public string Name()
        {
            return "网易云音乐";
        }

        public async Task<string> GetLyricAsync()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Referer", "http://music.163.com/");
            client.DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36");
            client.DefaultRequestHeaders.Add("Cookie", "appver=2.0.2");

            var stringContent = new StringContent($"s={Artist}+{Title}+{Album}&limit=15&type=1&offset=0");
            stringContent.Headers.Clear();
            stringContent.Headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");
            var response = await client.PostAsync("http://music.163.com/api/search/get/", stringContent);

            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(responseText);

            Trace.WriteLine($"[Netease][{Title}] Find {jObject["result"]["songs"].Count()} results.");

            foreach (var result in jObject["result"]["songs"].Children())
                try
                {
                    response = await client.GetAsync(
                        $"http://music.163.com/api/song/lyric?os=pc&id={result["id"]}&lv=-1&kv=-1&tv=-1");
                    response.EnsureSuccessStatusCode();
                    var resultText = await response.Content.ReadAsStringAsync();
                    var lyrics = JObject.Parse(resultText)["lrc"]["lyric"];
                    Trace.WriteLine($"[Netease][{Title}] Used lyric ID {result["id"]}.");
                    return (string) lyrics;
                }
                catch (NullReferenceException)
                {
                    // Use next candidate if failed
                }

            return null;
        }
    }
}