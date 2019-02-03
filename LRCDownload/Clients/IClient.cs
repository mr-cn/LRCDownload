using System.Threading.Tasks;
using TagLib;

namespace LRCDownload.Clients
{
    internal interface IClient
    {
        /// <summary>
        ///     Returns the client name
        /// </summary>
        /// <returns>client name</returns>
        string Name();

        /// <summary>
        ///     Gets the lyrics
        /// </summary>
        /// <returns>lyrics (task)</returns>
        Task<string> GetLyricAsync();

        /// <summary>
        /// Stores the metadata
        /// </summary>
        File Metadata { get; }
    }
}