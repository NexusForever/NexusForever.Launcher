namespace NexusForever.Launcher.Repositories
{
    public interface IServerRepositorySourceFactory
    {
        IServerRepositorySource Create(string url);
    }
}