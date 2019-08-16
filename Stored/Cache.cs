using System;
using System.Collections.Generic;
using System.Text;

namespace KCore.Stored
{
    public static class Cache
    {
        public static PropertiesModel Parameters{get; internal set; }

        public static void LoadParameters()
        {
#if DEBUG
            var file = System.IO.Path.Combine(System.Environment.CurrentDirectory,"config.dev.json");
#else
            var file = System.IO.Path.Combine(System.Environment.CurrentDirectory,"config.json");
#endif
            if (System.IO.File.Exists(file))
                Parameters = new PropertiesModel(file);
            else
                Parameters = new PropertiesModel();
        }
    }
}
