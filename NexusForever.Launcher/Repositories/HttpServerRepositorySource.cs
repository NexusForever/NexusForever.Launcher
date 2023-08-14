using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using NexusForever.Launcher.Models;

namespace NexusForever.Launcher.Repositories
{
    public class HttpServerRepositorySource : IServerRepositorySource
    {
        #region Dependency Injection

        private readonly IHttpClientFactory _httpClientFactory;

        public HttpServerRepositorySource(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        #endregion

        public async Task<ServerRepositoryModel> GetRepository(string url)
        {
            using HttpClient client = _httpClientFactory.CreateClient();
            return await client.GetFromJsonAsync<ServerRepositoryModel>($"{url.TrimEnd('/')}/Servers.json");
        }
    }
}
