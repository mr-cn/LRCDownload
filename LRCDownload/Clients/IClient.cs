using System.Threading.Tasks;
using TagLib;

namespace LRCDownload.Clients
{
    public interface IClient
    {
        /// <summary>
        ///     Returns the client name
        /// </summary>
        /// <returns>client name</returns>
        string Name();

        /// <summary>
        ///     Gets the lyrics
        /// </summary>
        /// <returns>
        /// lyrics (task)
        /// if it is null, it means it failed.
        /// </returns>
        Task<string> GetLyricAsync(File metadata);
    }
}