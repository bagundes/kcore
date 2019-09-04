using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KCore.Shell
{
    public static class File
    {
        public static string LOG => typeof(File).Name;

        public static string SaveTempDir(string prjName, string line, string filename, bool ovride = false, bool wait = true)
        {
            filename = $"{KCore.Shell.Directory.Temp(prjName)}/{filename}";
            Save(line, filename, ovride, wait);
            return filename;
        }

        #region Save
        /// <summary>
        /// Save the lines in text file
        /// </summary>
        /// <param name="lines">Values to save</param>
        /// <param name="filename">URL (full path file)</param>
        /// <param name="ovride">Override the file</param>
        /// <param name="wait">Wait the file unlock</param>
        public static void Save(string[] lines, string filename, bool ovride = false, bool wait = false)
        {
            var fileInfo = new FileInfo(filename);
            System.IO.Directory.CreateDirectory(fileInfo.DirectoryName);

                var exists = System.IO.File.Exists(filename);

            while (wait == true && IsLocked(filename) && exists)
                System.Threading.Thread.Sleep(2000);

            if (ovride)
            {
                if(WaitUnlocked(filename))
                    System.IO.File.WriteAllLines(filename, lines);
            }
            else
            {
                using (var file = new System.IO.StreamWriter(filename, true))
                {
                    for (int i = 0; i < lines.Length; i++)
                    {
                        var tab = i > 0 ? "\t" : "";
                        file.WriteLine(tab + lines[i]);
                    }
                }
            }
        }
        public static void Save(string line, string filename, bool ovride = false, bool wait = false)
        {
            Save(new string[] { line }, filename, ovride, wait);
        }
        #endregion

        #region Convert
        public static void LoadHex(string hexString, string filename, bool ovride = false)
        {
            if (!ovride && System.IO.File.Exists(filename))
                return;

            int bytesCount = (hexString.Length) / 2;
            byte[] bytes = new byte[bytesCount];
            for (int x = 0; x < bytesCount; ++x)
            {
                bytes[x] = Convert.ToByte(hexString.Substring(x * 2, 2), 16);
            }

            System.IO.File.WriteAllBytes(filename, bytes);
        }

        public static string ConvertToBase64(string filename)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(filename);
           return Convert.ToBase64String(imageArray);
        }

        #endregion

        #region Status
        /// <summary>
        /// Verify if the file is locked
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsLocked(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            return IsLocked(fileInfo);
        }

        public static bool WaitUnlocked(string filename)
        {
            while (IsLocked(filename))
                System.Threading.Thread.Sleep(1000);

            return true;
        }

        public static bool IsLocked(FileInfo file)
        {
            file.Refresh();
            FileStream stream = null;

            try
            {
                if (file.Exists)
                    stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //o arquivo está indisponível pelas seguintes causas:
                //está sendo escrito
                //utilizado por uma outra thread
                //não existe ou sendo criado
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //arquivo está disponível
            return false;
        }
        #endregion

    }
}
