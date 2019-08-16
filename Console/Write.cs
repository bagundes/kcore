using System;
using System.Collections.Generic;
using System.Text;

namespace KCore.Console
{
    public static class Write
    {
        private const string formatDate = "hh:mm:ss";
        private const int limitLog = 10;
        private const int limitType = 8;
        public static string Message(string log, C.StatusType type, string info, params object[] vals)
        {

            var date = DateTime.Now.ToString(formatDate);
            log = log.Length > limitLog ? log.Substring(0, limitLog) : log;

            return String.Format("{0} {1," + -limitLog + "} {2," + -limitType + "} - {3}", date, log, type.ToString(), String.Format(info, vals));
        }

        public static string Message(string log, Exception ex)
        {
            var date = DateTime.Now.ToString(formatDate);
            log = log.Length > limitLog ? log.Substring(0, limitLog) : log;

            return String.Format("{0} {1," + -limitLog + "} {2," + -limitType + "} - {3}", date, log, C.StatusType.Error, ex.Message);
        }
    }
}
