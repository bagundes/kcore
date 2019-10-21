using KCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KCore.Stored
{
    public static class Cache
    {
        private static int  block = 0;

        #region Global config

        private static KCore.Lists.MyDictionary _globalConfig;
        public static IReadOnlyDictionary<string, dynamic> GlobalConfig
        {
            get
            {
                if (_globalConfig == null)
                {
                    string filename;

                    if (R.IsDebugMode)
                    {
                        filename = System.IO.Path.Combine(R.AppPath, "config.dev.json");
                        if (!System.IO.File.Exists(filename) && R.IsDebugMode)
                        {
                            System.IO.File.Copy(System.IO.Path.Combine(R.AppPath, "config.json"),
                                System.IO.Path.Combine(R.AppPath, "config.dev.json"));
                        }
                    }
                    else
                    {
                        filename = System.IO.Path.Combine(R.AppPath, "config.json");
                    }

                    if (System.IO.File.Exists(filename))
                        _globalConfig = new Lists.MyDictionary(filename);
                    else
                        _globalConfig = new Lists.MyDictionary();
                }

                return _globalConfig.AsReadOnly();
            }
        }

        #endregion

        public static List<ColumnStruct> ColumnsStruct { get; internal set; }





        private static PropertiesList_v1 parameters;
        [Obsolete("Use GlobalConfig")]
        public static PropertiesList_v1 Parameters
        {
            get
            {
                if (parameters == null)
                    LoadParameters();

                return parameters;
            }
        }

        private static void LoadParameters()
        {
            string file;


            if (R.IsDebugMode)
            {
                file = System.IO.Path.Combine(R.AppPath, "config.dev.json");
                if (!System.IO.File.Exists(file) && R.IsDebugMode)
                {
                    System.IO.File.Copy(System.IO.Path.Combine(R.AppPath, "config.json"),
                        System.IO.Path.Combine(R.AppPath, "config.dev.json"));
                }
            }
            else
            {
                file = System.IO.Path.Combine(R.AppPath, "config.json");
            }

            if (System.IO.File.Exists(file))
                parameters = new PropertiesList_v1(file);
            else
                parameters = new PropertiesList_v1();
        }

        public static void LoadColumnsStruct(List<ColumnStruct> columnsStruct)
        {
            

          while(block++ > 0)
                System.Threading.Thread.Sleep(1000);         


            if (ColumnsStruct == null)
                ColumnsStruct = new List<ColumnStruct>();

            var foo = columnsStruct[0];

            if (!KCore.Stored.Cache.ColumnsStruct.Where(t => t.DBase.Equals(foo.DBase, StringComparison.InvariantCultureIgnoreCase)
                && t.Table.Equals(foo.Table, StringComparison.InvariantCultureIgnoreCase)).Any())
                ColumnsStruct.AddRange(columnsStruct);

            block = 0;
        }

        public static void ClearCache()
        {
            //foreach (var p in KCore.Reflection.FilterOnlySetProperties(this))
            //    KCore.Reflection.SetValue(this, p.Name, null);
            ColumnsStruct = null;
            parameters = null;
        }
    }
}
