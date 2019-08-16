using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace KCore.Web
{
    public class Ftp
    {

        public double Percent { get; internal set; }
        public KCore.Model.Credential2 cred { get; internal set; }

        public Ftp(KCore.Model.Credential2 cred)
        {
            this.cred = cred;
        }

        /// <summary>
        /// Send the file via ftp. You can get the progressbar using Percent properties 
        /// </summary>
        /// <param name="subpath">Inform the sub path. The uri is need to inform in credential</param>
        /// <param name="fileName">path of file</param>
        /// <param name="cred"></param>
        public void Send(string subpath, string fileName)
        {
            this.cred = cred;
            var file = new FileInfo(fileName);
            if (!file.Exists)
                return;

            if (subpath.Contains("ftp://"))
                return;

            var  request = (FtpWebRequest)WebRequest.Create($"ftp://{cred.Host}/{subpath}/{file.Name}.{file.Extension}");
            

            request.Credentials = new NetworkCredential(cred.User, cred.GetPasswd());
            request.Method = WebRequestMethods.Ftp.UploadFile;

            using (var fileStream = File.OpenRead(file.FullName))
            using (var ftpStream = request.GetRequestStream())
            {
                var a = fileStream.Length;

                byte[] buffer = new byte[10240];
                int read;

                Percent = 0;

                while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    Percent = fileStream.Position;
                    ftpStream.Write(buffer, 0, read);

                    Percent = (double.Parse(fileStream.Position.ToString()) / double.Parse(fileStream.Length.ToString()));
                }

                Percent = 1;
            }
        }


        public static bool TestConn(Model.Credential2 cred)
        {
            var request = (FtpWebRequest)WebRequest.Create($"ftp://{cred.Host}/");
            request.Credentials = new NetworkCredential(cred.User, cred.GetPasswd());
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                return true;
            
        }
    }
}
