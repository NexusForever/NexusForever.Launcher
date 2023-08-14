using NexusForever.Launcher.Models;

namespace NexusForever.Launcher.Services
{
    public interface ICustomServerMessageFormatter
    {
        void Format(ServerRepositoryModel serverRepository, ServerModel server);
    }
}