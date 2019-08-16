using System;
using System.Collections.Generic;
using System.Text;

namespace K.Core.Config
{
    public static class Init
    {
        /// <summary>
        /// Init will execute in the order:
        /// 10 - Construct
        /// 20 - Populate
        /// 30 - Configure
        /// 40 - Register
        /// 90 - Destruct
        /// </summary>
        public static bool Execute(Base.IBaseInit init)
        {
            init.Destruct();
            init.Construct();
            init.Populate();
            init.Configure();
            init.Register();
            

            return true;
        }
    }
}
