using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NexusForever.Launcher.Configuration;
using NexusForever.Launcher.Models;

namespace NexusForever.Launcher.Services
{
    public class UpdateService : IUpdateService
    {
        #region Dependency Injection

        private readonly IOptions<LauncherConfiguration> _options;
        private readonly IHttpClientFactory _httpClientFactory;

        public UpdateService(
            IOptions<LauncherConfiguration> options,
            IHttpClientFactory httpClientFactory)
        {
            _options           = options;
            _httpClientFactory = httpClientFactory;
        }

        #endregion

        public string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public async Task<UpdateModel> GetUpdate()
        {
            if (!_options.Value.CheckForUpdates || string.IsNullOrEmpty(_options.Value.UpdateUrl))
                return null;

            using HttpClient httpClient = _httpClientFactory.CreateClient();
            return await httpClient.GetFromJsonAsync<UpdateModel>(_options.Value.UpdateUrl);
        }
    }
}
