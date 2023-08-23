using NexusForever.Launcher.Models;
using System.Threading.Tasks;

namespace NexusForever.Launcher.Services
{
    public interface IUpdateService
    {
        string GetVersion();

        Task<UpdateModel> GetUpdate();
    }
}
