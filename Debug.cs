using System;

namespace KCore
{
    public class Debug
    {
        /// <summary>
        /// Save the details of exception on extern database
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="table"></param>
        public static void Save(Exception ex)
        {
            var model = new DebugModel(ex);
            KCore.Stored.Online.Save(model);
        }

        public static void Save(string message)
        {
            var model = new DebugModel(message);
            KCore.Stored.Online.Save(model);
        }

    }

    public sealed class DebugModel : KCore.Base.BaseModel
    {
        #region properties
        private string _logName;
        public override string LOG => _logName;
#if DEBUG
        public bool DebugMode = true;
#else
        public bool DebugMode = false;
#endif
        public DebugModel inner;
        public string message;
        public string trace;
        public int hresult;
        public string source;
        //public string target;
        #endregion

        public DebugModel(string message)
        {
            this.message = message;
        }

        public DebugModel(Exception ex)
        {
            _logName = ex.GetType().FullName;
            this.message = ex.Message;
            this.trace = ex.StackTrace;
            this.hresult = ex.HResult;
            this.source = ex.Source;
            //this.target = ex.TargetSitetarget;

            if (ex.InnerException != null)
                this.inner = InnerException(ex.InnerException);
        }

        private DebugModel InnerException(Exception inner)
        {
            if (inner != null)
                return new DebugModel(inner);
            else
                return null;
        }
    }
}
