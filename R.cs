using System;
using System.Collections.Generic;
using System.Resources;
using System.Text;

namespace KCore
{
    public static class R
    {
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
        public static string Prefix => $"{KCore.R.Company.Namespace}_{Project.Namespace}";
        public static class Project
        {
            public static string Language => R.Language;
            public static string Name => "KCore";
            public static string Namespace => "KC";
            public static Version Version => R.Assembly.GetName().Version;
            public static int ID => Version.Major;
            
            public static class Folders
            {
                public static string Credential => KCore.Shell.Directory.Temp(Project.Name, "credentials");
            }
        }
        public static class Security
        {
            public static string MasterKey => "eyJ1bmlxdWVfbmFtZSI6Im1hbmFnZXIiLCJuYmYiOjE1NjA1MDQxNzYsImV4cCI6MTU2MDUwNTk3NiwiaWF0Ij";
            /// <summary>
            /// Expire in minutes
            /// </summary>
            public const int Expire = 1440;
            /// <summary>
            /// Due date in months
            /// </summary>
            public const int DueDate = 12; 
        }
        public static class Company
        {
            public static String FullName => "Teamsoft Limited";
            public static String Name => "Teamsoft";
            public static String Namespace => "TS";
        }
    }
}
