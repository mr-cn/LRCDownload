using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TagLib;

namespace LRCDownload.Clients
{
    internal class Kugou : IClient
    {
        public Kugou(File metadata)
        {
            Metadata = metadata;
        }

        private string Artist => TagHelper.GetArtist(Metadata);
        private string Title => TagHelper.GetTitle(Metadata);
        private double Length => Metadata.Properties.Duration.TotalMilliseconds;

        public File Metadata { get; }

        public string Name()
        {
            return "酷狗音乐";
        }

        public async Task<string> GetLyricAsync()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36");

            var response =
                await client.GetAsync(
                    $"http://lyrics.kugou.com/search?ver=1&man=yes&client=pc&keyword={Uri.EscapeDataString(Title)}-{Uri.EscapeDataString(Artist)}&duration={Length}&hash=");

            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(responseText);
            foreach (var result in jObject["candidates"].Children())
                try
                {
                    response = await client.GetAsync(
                        $"http://lyrics.kugou.com/download?ver=1&client=pc&id={result["id"]}&accesskey={result["accesskey"]}&fmt=lrc&charset=utf8");
                    response.EnsureSuccessStatusCode();
                    var resultText = await response.Content.ReadAsStringAsync();
                    var lyrics = JObject.Parse(resultText)["content"];
                    return UnBase64String((string) lyrics);
                }
                catch (NullReferenceException)
                {
                    // 使用下一个候选歌曲
                }

            return null;
        }

        private string UnBase64String(string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            var bytes = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}