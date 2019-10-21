using System.Net;
using System.Net.Sockets;

namespace KCore.Shell
{
    public static class MachineInfo
    {
        public static string Name()
        {
            return System.Environment.MachineName;
        }

        public static string IP()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return null;
        }

    }
}
