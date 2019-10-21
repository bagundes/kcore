using System;
using System.Resources;
using System.Runtime.Serialization;

namespace KCore
{
    public class KCoreException : Base.BaseException
    {
        protected override ResourceManager Resx => R.Resx;
        protected override int Id => R.ID;

        public KCoreException(string log, C.MessageEx code, params dynamic[] values) : base(log, code, values)
        {
        }

        public KCoreException(string log, C.MessageEx code, Exception innerException) : base(log, code, innerException)
        {
        }
    }
}
