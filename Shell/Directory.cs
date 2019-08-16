using System;
using System.Collections.Generic;
using System.Text;

namespace K.Core.Shell
{
    public static class Directory
    {
        public static string Temp(string prjName, params string[] folders)
        {
            var dir = $"{System.IO.Path.GetTempPath()}{R.Company.Name}\\{prjName.ToString()}\\{System.IO.Path.Combine(folders)}";
            return System.IO.Directory.CreateDirectory(dir).FullName;
        }

        public static string AppData(string prjName, params string[] folders)
        {
            throw new NotImplementedException();
            //var dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //var path = System.IO.Path.Combine(dir, klib.R.Company.AliasName, prj_name, System.IO.Path.Combine(folders));
            //if (!Directory.Exists(path))
            //    Directory.CreateDirectory(path);

            //return new System.IO.DirectoryInfo(path);
        }
    }
}
