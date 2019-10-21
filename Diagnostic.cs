using System;

namespace KCore
{
    public static class Diagnostic
    {
        private enum TypeReg
        {
            Info,
            Warn,
            Error,
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
            var values = new string[] { ex.Message, ex.StackTrace, ex.Source };
            return Track(LOG, values);
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
        [Obsolete("Internal error", true)]
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
        /// Register warning
        /// </summary>
        /// <param name="id">Module ID</param>
        /// <param name="log">Class log name</param>
        /// <param name="track">Track name file</param>
        /// <param name="message">Message to save</param>
        public static void Warning(int id, string log, int track, string message)
        {
            Register(id, log, TypeReg.Warn, $"Track {track} - {message}");
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
