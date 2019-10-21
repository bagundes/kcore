using System;

namespace KCore.Shell
{
    public static class Directory
    {
        /// <summary>
        /// Create a new folder in temporary application folder.
        /// The old files will be deleted automatically.
        /// </summary>
        /// <param name="prjName">Project name</param>
        /// <param name="folders">sub folders to create</param>
        /// <returns>path</returns>
        public static string AppTemp(string prjName, params string[] folders)
        {
            var dir = $"{System.IO.Path.GetTempPath()}{R.Company.Name}\\{prjName.ToString()}\\{System.IO.Path.Combine(folders)}";
            KCore.Shell.File.Delete(DateTime.Now.AddMinutes(-R.Security.TempFilesTimeToDelete), dir);
            return System.IO.Directory.CreateDirectory(dir).FullName;
        }

        public static string Create(params string[] dir)
        {
            var foo = String.Join("\\", dir);
            //foo = foo.Replace("/", "\\");
            //foo = 

            System.IO.Directory.CreateDirectory(foo);
            return foo;
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
