﻿using System.Net.Http;
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

            var stringContent = new StringContent($"s={Artist}+{Title}+{Album}&limit=15&type=1&offset=0");
            stringContent.Headers.Clear();
            stringContent.Headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");
            var response = await client.PostAsync("http://music.163.com/api/search/get/", stringContent);

            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(responseText);

            if (!jObject.ContainsKey("result")) // Whether the music is blocked or uncollected, we shouldn't go on.
                return null;

            foreach (var result in jObject["result"]["songs"].Children())
            {
                response = await client.GetAsync(
                    $"http://music.163.com/api/song/lyric?os=pc&id={result["id"]}&lv=-1&kv=-1&tv=-1");
                response.EnsureSuccessStatusCode();
                var resultText = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(resultText);

                if (json.ContainsKey("nolyric")) // Means the music doesn't have lyric. (Normally pure music)
                    return "纯音乐";
                if (json.ContainsKey("uncollected")) // Means there's no lyric in the database.
                    continue;
                return (string) json["lrc"]["lyric"];
            }

            return null;
        }
    }
}