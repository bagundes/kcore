using System;
using System.Collections.Generic;
using System.Text;

namespace KCore.Config
{
    public static class Init
    {
        /// <summary>
        /// Init will execute in the order:
        /// 10 - Configure
        /// 20 - Construct
        /// 30 - Populate
        /// 40 - Register
        /// 90 - Destruct
        /// </summary>
        public static bool Execute(Base.IBaseInit init)
        {
            init.Configure();
            init.Destruct();
            init.Construct();
            init.Populate();
            init.Register();
            

            return true;
        }
    }
}
