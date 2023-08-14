using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using NexusForever.Launcher.Models;

namespace NexusForever.Launcher.Services
{
    public class CustomServerMessageFormatter : ICustomServerMessageFormatter
    {
        public void Format(ServerRepositoryModel serverRepository, ServerModel server)
        {
            if (string.IsNullOrEmpty(server.CustomMessage))
            {
                var sb = new StringBuilder();
                sb.AppendLine(serverRepository.CustomMessage);

                if (server.Discord != null)
                    sb.AppendLine($"Discord: {server.Discord}");
                if (server.Website != null)
                    sb.AppendLine($"Website: {server.Website}");

                server.CustomMessage = sb.ToString();
            }

            // do variable replacement
            MatchCollection matches = Regex.Matches(server.CustomMessage, "#{([A-Za-z]+)}");
            foreach (Match match in matches)
            {
                PropertyInfo property = server.GetType().GetProperty(match.Groups[1].Value);
                if (property == null)
                    continue;

                object value = property.GetValue(server);
                if (value == null)
                    continue;

                server.CustomMessage = server.CustomMessage.Replace(match.Value, value.ToString());
            }
        }
    }
}
