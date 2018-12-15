using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace LRCDownload.Plugins
{
    internal class Netease
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

        public static async Task<string> GetLyricAsync(string artist, string title)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Referer", "http://music.163.com/");
            client.DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36");
            client.DefaultRequestHeaders.Add("Cookie", "appver=2.0.2");

            var stringContent = new StringContent(get_search_params(artist, title));
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
                    //若没有歌词，则查找下一个
                }

            throw new Exception();
        }

        private static string process_keywords(string s)
        {
            s = s.ToLower();
            s = Regex.Replace(s, @"/\'|·|\$|\&|–/g", string.Empty);
            s = Regex.Replace(s, @"/\(.*?\)|\[.*?]|{.*?}|（.*?/g", string.Empty);
            s = Regex.Replace(s, @"/[-/:-@[-`{-~]+/g", string.Empty);
            s = Regex.Replace(s,
                @"/[\u2014\u2018\u201c\u2026\u3001\u3002\u300a\u300b\u300e\u300f\u3010\u3011\u30fb\uff01\uff08\uff09\uff0c\uff1a\uff1b\uff1f\uff5e\uffe5]+/g",
                string.Empty);
            return s;
        }

        private static string get_search_params(string artist, string title)
        {
            var limit = 10;
            var type = 1;
            var offset = 0;
            artist = process_keywords(artist);
            title = process_keywords(title);
            return "s=" + artist + "+" + title + "&limit=" + limit + "&type=" + type + "&offset=" + offset;
        }
    }
}