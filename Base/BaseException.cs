using System;
using System.Resources;
using System.Runtime.Serialization;

namespace KCore.Base
{
    public abstract class BaseException : Exception
    {
        public string LOG => this.GetType().Name;


        public int Code { get; protected set; }
        public readonly string LogName;
        public override string Message => Dscription();
        public readonly string Tag;
        private dynamic[] Values = new dynamic[0];
        
        protected abstract ResourceManager Resx { get; }
        protected abstract int Id { get; }


        public BaseException(string log, Enum code, params dynamic[] values) : base(log)
        {
            this.LogName = log;
            this.Code = Convert.ToInt32(code);
            this.Values = values;
            Tag = code.ToString();
            Save();

        }

        //public BaseException(string log, Enum code, Exception innerException) : base(log, innerException)
        //{
        //    this.LogName = log;
        //    this.Code = Convert.ToInt32(code);
        //    Tag = code.ToString();
        //    Save();
        //}

        //protected BaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        //{
        //    Save();
        //}

        protected string Dscription()
        {
            if (Code < 0)
                return $"({Code * -1}) {String.Join(". ", Values)}";


            var ResxName = $"M{Code.ToString("00000")}_{Values.Length}";

            //klib.content.Location.resources
            var msg = String.Empty;

            try
            {
                msg = Resx.GetString(ResxName);
                msg = msg ?? $"Args ({Values.Length}): {String.Join(", ", Values)}";
                msg = String.Format($"[{Code:X}] " + msg, Values);
            }
            catch (ArgumentException ex)
            {
                Diagnostic.Error(R.ID, LOG, ex.Message, ex.StackTrace, ex.Source);
                msg = $"{msg} - Args ({Values.Length}): {String.Join(", ", Values)}";
            }

            return msg;
        }

        public override string ToString()
        {
            return base.Message;
        }

        private void Save()
        {

            var debug = new KCore.DebugModel(this);
            //KCore.Stored.Online.Save(debug);
        }
    }
}
