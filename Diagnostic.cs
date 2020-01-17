using System;
using System.Collections.Generic;

namespace KCore
{
    public static class Diagnostic
    {
        private enum TypeReg
        {
            Info,
            Warn,
            Error,
            Debug,
        }

        public static string LOG => typeof(Diagnostic).Name;

        #region Track
        /// <summary>
        /// Create the specific file information (for tracking)
        /// </summary>
        /// <param name="LOG">log name</param>
        /// <param name="values">Values to save</param>
        /// <returns></returns>
        public static int Track(string LOG, params string[] values)
        {
            var id = Security.Hash.CreateIdNumber(values);
            var url = $"{Shell.Directory.AppTemp("track")}{id.ToString("000000")}_{LOG.ToLower()}.log";
            Shell.File.Save(values, url, true, false);

            return id;
        }

        /// <summary>
        /// Create the specific file information (for tracking)
        /// </summary>
        /// <param name="LOG">log name</param>
        /// <param name="values">Values to save</param>
        /// <returns></returns>
        public static int Track(string LOG, Exception ex)
        {
            var values = new List<string> { ex.Message, ex.StackTrace, ex.Source };
            if(ex.InnerException != null)
            {
                var iex = ex.InnerException;
                values.Add("InnerException");
                values.Add(iex.Message);
                values.Add(iex.StackTrace);
                values.Add(iex.Source);
            }
            return Track(LOG, values.ToArray());
        }
        #endregion

        #region Error
        /// <summary>
        /// Register error
        /// </summary>
        /// <param name="id">Module ID</param>
        /// <param name="log">Class log name</param>
        /// <param name="track">Track name file</param>
        /// <param name="message">Message to save</param>
        public static void Error(int id, string log, int track, string message)
        {
            Register(id, log, TypeReg.Error, $"Track {track} - {message}");
        }

        /// <summary>
        /// Register error
        /// </summary>
        /// <param name="id">Module ID</param>
        /// <param name="log">Class log name</param>
        /// <param name="track">Track name file</param>
        /// <param name="ex">The base exception</param> 
        public static void Error(int id, string log, Exception ex, string message = null)
        {
            Register(id, log, TypeReg.Error, $"Track: {Track(log, ex)}. {message}");
        }

        public static void Error(int id, string log, string message, string stackTrace, string source)
        {
            var values = new string[] { message, stackTrace, source };
            Register(id, log, TypeReg.Error, $"Track: {Track(LOG, values)}. {message}");
        }


        #endregion

        /// <summary>
        /// Debug the libraries. To debug is working you need to enable the KCore.R.IsDebug mode property to true
        /// </summary>
        /// <param name="control">Number of instence or control name</param>
        /// <param name="id">Id of application or library</param>
        /// <param name="log">Log name</param>
        /// <param name="message">Debug message</param>
        /// <param name="args">String format to message</param>
        public static void Debug(string control, int id, string log, string message,params object[] args)
        {
            if (KCore.R.DebugMode)
            {
                try
                {
                    if (args != null && args.Length > 0)
                        Register(id, log, TypeReg.Debug, $"[{control}] {String.Format(message, args)}");
                    else
                        Register(id, log, TypeReg.Debug, $"[{control}] {message}");
                } catch
                {
                    Register(id, log, TypeReg.Debug, $"[{control}] {message} > args:{String.Join(",", args)}");
                }
            }

        }


        /// <summary>
        /// Register warning
        /// </summary>
        /// <param name="id">Module ID</param>
        /// <param name="log">Class log name</param>
        /// <param name="track">Track name file</param>
        /// <param name="message">Message to save</param>
        public static void Warning(int id, string log, int track, string message)
        {
            Register(id, log, TypeReg.Warn, $"Track {track.ToString("000000000")} - {message}");
        }

        private static void Register(int id, string log, TypeReg reg, params string[] lines)
        {
            var type = $"[{reg.ToString() + new string(' ', 5 - reg.ToString().Length)}]";
            var time = DateTime.Now.ToString("HH:mm:ss");
            lines[0] = $"{time} {type} - {id}.{log}: {lines[0]}";

            var url = $"{Shell.Directory.AppTemp("logs")}\\{DateTime.Now.ToString("yyMMdd_HH")}00.log";
            Shell.File.Save(lines, url, false, false);
        }

        private static void RegisterWebTracker()
        {

        }
    }
}
