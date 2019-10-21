using KCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;

namespace KCore
{
    public static class R
    {
        #region Parameters in the config file
        public static string CustomerName => KCore.Stored.Cache.Parameters.Get("Customer", "Customer name needs to saves in config file");
        #endregion

        #region List of projects
        private static List<Select_v2> _listOfProjects = new List<Select_v2>();
        /// <summary>
        /// Register your project here.
        /// </summary>
        public static void RegisterProject(int id, string name)
        {
            if (_listOfProjects.Where(t => t.value == id).Any())
                return;
            else
                _listOfProjects.Add(new Select_v2(id, name));
        }

        public static string GetProjectName(int id)
        {
            return _listOfProjects.Where(t => t.value == id).Select(t => t.text).FirstOrDefault();
        }
        public static List<Select_v2> ListOfProjects => _listOfProjects;
        #endregion

        #region Database to log files
        public static string MongoUser => KCore.Security.Hash.TokenToValue(Stored.Cache.Parameters.Get("support").ToString());
        #endregion

        #region Debug project

#if DEBUG
        public static bool IsDebugMode = true;
#else
        public static bool IsDebugMode = false;
#endif
        #endregion


        private static string appPath;

        public static String AppPath
        {
            get { if (String.IsNullOrEmpty(appPath)) return System.Environment.CurrentDirectory; else return appPath; }
            set { appPath = value; }
        }
        private static System.Reflection.Assembly Assembly => System.Reflection.Assembly.GetExecutingAssembly();
        private static ResourceManager resx;

        public static string Language = "en-gb";
        public static int ID => Project.ID;
        public static string[] Resources => Assembly.GetManifestResourceNames();

        public static string DataSource => "Teamsoft";
        public static ResourceManager Resx
        {
            get
            {
                if (resx == null)
                    resx = new ResourceManager($"{typeof(R).Namespace}.Content.Location_{R.Project.Language}", Assembly);

                return resx;
            }
        }
        public static string Prefix => $"{KCore.R.Company.Initials}_{Project.Namespace}";
        public static class Project
        {
            public static string Language => R.Language;
            public static string Name => "KCore";
            public static string Namespace => "KC";
            public static Version Version => R.Assembly.GetName().Version;
            public static int ID => Version.Major;

            public static class Folders
            {
                public static string Credential => KCore.Shell.Directory.AppTemp(Project.Name, "credentials");
            }
        }
        public static class Security
        {
            public static string MasterKey => "eyJ1bmlxdWVfbmFtZSI6Im1hbmFnZXIiLCJuYmYiOjE1NjA1MDQxNzYsImV4cCI6MTU2MDUwNTk3NiwiaWF0Ij";
            /// <summary>
            /// Expire in minutes
            /// </summary>
            public static int Expire = 1440;

            /// <summary>
            /// Delete temporary files with N minutes without access.
            /// Default: 7 days.
            /// </summary>
            public static int TempFilesTimeToDelete = 420;
        }
        public static class Company
        {
            public static String FullName => "Teamsoft Limited";
            public static String Name => "Teamsoft";
            public static string ID => "Teamsoft";
            public static String Initials => "TS";
        }
    }
}
