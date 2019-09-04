using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace KCore.Stored
{
    public static class Cache
    {
        public static PropertiesModel Parameters{get; internal set; }

        public static void LoadParameters()
        {
            string file;


#if DEBUG
            file = System.IO.Path.Combine(R.AppPath,"config.dev.json");
            if (!System.IO.File.Exists(file) && R.IsDebugMode)
            {
                System.IO.File.Copy(System.IO.Path.Combine(R.AppPath, "config.json"),
                    System.IO.Path.Combine(R.AppPath, "config.dev.json"));                
            }

#else
            file = System.IO.Path.Combine(R.AppPath,"config.json");
#endif
            if (System.IO.File.Exists(file))
                Parameters = new PropertiesModel(file);
            else
                Parameters = new PropertiesModel();
        }
    }
}
