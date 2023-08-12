using System.Threading.Tasks;
using NexusForever.Launcher.Models;

namespace NexusForever.Launcher.Services;

public interface ICustomDialogService
{
    /// <summary>
    /// Show a custom dialog to add a new server.
    /// </summary>
    Task<ServerModel> ShowAddServerDialog(object context);
}