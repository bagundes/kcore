using System;
using System.Collections.Generic;
using System.Text;

namespace KCore
{
    public class Debug
    {
        /// <summary>
        /// Save the details of exception on extern database
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="table"></param>
        public static void Save(Exception ex, string table = null)
        {
            var model = new DebugModel(ex);
            if (!String.IsNullOrEmpty(table))
                model.table = table;

            KCore.Stored.Online.Save(model, model.table);
        }

        public static void Save(string message)
        {
            
        }
      
    }

    public sealed class DebugModel : KCore.Base.BaseModel
    {
#if DEBUG
        public bool DebugMode = true;
#else
        public bool DebugMode = false;
#endif

        public string table;
        public DebugModel inner;
        public string message;
        public string trace;
        public int hresult;
        public string source;
        //public string target;

        public DebugModel(Exception ex)
        {
            this.table = ex.GetType().Name;
            this.message = ex.Message;
            this.trace = ex.StackTrace;
            this.hresult = ex.HResult;
            this.source = ex.Source;
            //this.target = ex.TargetSitetarget;

            if (ex.InnerException != null)
                this.inner = InnerException(ex.InnerException);

            this.Id = KCore.Security.Hash.MD5(this);
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
