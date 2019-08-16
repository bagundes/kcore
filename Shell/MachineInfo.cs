using System;
using System.Collections.Generic;
using System.Text;

namespace KCore.Shell
{
    public static class MachineInfo
    {
        public static string Name()
        {
            return System.Environment.MachineName;
        }

    }
}
