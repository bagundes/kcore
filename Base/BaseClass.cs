using System;
using System.Collections.Generic;
using System.Text;

namespace KCore.Base
{
    public abstract class BaseClass
    {
        public virtual string LOG => this.GetType().Name;
        public virtual string ObjClass => this.GetType().Name;
    }
}
