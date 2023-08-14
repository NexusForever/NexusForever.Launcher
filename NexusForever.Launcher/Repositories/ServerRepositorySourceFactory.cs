using System;
using Microsoft.Extensions.DependencyInjection;

namespace NexusForever.Launcher.Repositories
{
    public class ServerRepositorySourceFactory : IServerRepositorySourceFactory
    {
        #region Dependency Injection

        private IServiceProvider _serviceProvider;

        public ServerRepositorySourceFactory(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #endregion

        public IServerRepositorySource Create(string url)
        {
            return new Uri(url).Scheme switch
            {
                "file" => _serviceProvider.GetRequiredService<FileServerRepositorySource>(),
                _      => _serviceProvider.GetRequiredService<HttpServerRepositorySource>()
            };
        }
    }
}
