namespace NexusForever.Launcher.Models.Native;

public struct SECURITY_ATTRIBUTES
{
    public int length;
    public nint lpSecurityDescriptor;
    public bool bInheritHandle;
}
