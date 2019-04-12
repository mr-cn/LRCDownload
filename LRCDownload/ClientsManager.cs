using System.Collections.Generic;

namespace LRCDownload.Clients
{
    public static class ClientsManager
    {
        public static Dictionary<string, IClient> Clients { get; }

        static ClientsManager()
        {
            var interfaces = new List<IClient>()
            {
                new Netease(),
                new Kugou()
            };

            Clients = new Dictionary<string, IClient>();
            foreach (var client in interfaces)
            {
                Clients.Add(client.Name(), client);
            }
        }
    }
}