using System.Collections.Generic;
using System.Linq;

namespace KCore.Config
{
    public static class Init
    {
        public static List<string> Ran { get; internal set; } = new List<string>();
        /// <summary>
        /// Init will execute in the order:
        /// 00 - Dependencies
        /// 10 - Destruct
        /// 20 - Construct
        /// 30 - Configure
        /// 40 - Populate
        /// 50 - Register

        /// </summary>
        public static bool Execute(Base.IBaseInit init)
        {
            var name = init.GetType().FullName;
            if (Ran.Where(t => t == name).Any())
                return true;


            init.Dependencies();
            init.Destruct();
            init.Construct();
            init.Configure();
            init.Populate();
            init.Register();

            Ran.Add(name);


            return true;
        }
    }
}
