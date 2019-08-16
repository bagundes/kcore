using System;
using System.Collections.Generic;
using System.Text;

namespace K.Core.Shell
{
    public static class FormatString
    {
        /// <summary>
        /// Create the Initials name.
        /// Examples: 
        /// Dana Willy: return DW
        /// Maria: return Ma
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string Initials(string name)
        {
            var names = name.Split(' ');

            if(names.Length > 1)
            {
                return names[0].ToString().ToUpper() + names[names.Length - 1].ToString().ToUpper();
            } else
                return name[0].ToString().ToUpper() + name[1].ToString().ToLower();
        }
    }
}
