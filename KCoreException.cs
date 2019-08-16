using System;
using System.Collections.Generic;
using System.Resources;
using System.Runtime.Serialization;
using System.Text;

namespace K.Core
{
    public class KCoreException : Base.BaseException
    {
        protected override ResourceManager Resx => R.Resx;
        protected override int Id => R.ID;

        public KCoreException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public KCoreException(string log, C.MessageEx code, params dynamic[] values) : base(log, code, values)
        {
        }

        public KCoreException(string log, C.MessageEx code, Exception innerException) : base(log, code, innerException)
        {
        }
    }
}
