using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TagLib;

namespace LRCDownload.Clients
{
    internal class Netease : IClient
    {
        public Netease(File metadata)
        {
            Metadata = metadata;
        }

        private string Artist => TagHelper.GetArtist(Metadata);
        private string Title => TagHelper.GetTitle(Metadata);
        private string Album => TagHelper.GetAlbum(Metadata);

        public File Metadata { get; }

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

            var keyValues = new List<KeyValuePair<string, string>>(); // POST Send Params
            keyValues.Add(new KeyValuePair<string, string>("s", $"{Title} - {Album} - {Artist}")); // set keywords
            keyValues.Add(new KeyValuePair<string, string>("limit", "15")); // receive 15 results
            keyValues.Add(new KeyValuePair<string, string>("type", "1"));
            keyValues.Add(new KeyValuePair<string, string>("offset", "0"));
            var postContent = new FormUrlEncodedContent(keyValues);

            var response = await client.PostAsync("http://music.163.com/api/search/get/", postContent);

            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(responseText);

            if (!jObject.ContainsKey("result")) // Whether the music is blocked or uncollected, we shouldn't go on.
                return null;
            if ((int) jObject["result"]["songCount"] == 0) // There's no result.
                return null;

            foreach (var result in jObject["result"]["songs"].Children())
            {
                response = await client.GetAsync(
                    $"http://music.163.com/api/song/lyric?os=pc&id={result["id"]}&lv=-1&kv=-1&tv=-1");
                response.EnsureSuccessStatusCode();
                var resultText = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(resultText);

                if (json.ContainsKey("nolyric")) // The music itself doesn't have lyric. (Normally pure music)
                    return "纯音乐";
                if (json.ContainsKey("uncollected")) // There's no lyric so far.
                    continue;
                return (string) json["lrc"]["lyric"];
            }

            return null;
        }
    }
}