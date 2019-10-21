using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace KCore.Model
{
    public class VersionControlModel : Base.BaseModel_v1
    {
        public string Host { get; set; } = KCore.Shell.MachineInfo.IP();
        public string Instance { get; set; } = KCore.Shell.MachineInfo.Name();
        public Version Version { get; set; } 
        public string Details { get; set; }

        public VersionControlModel()
        {

        }

        public VersionControlModel(int id, Version version)
        {
            Version = version;
        }
    }
}
