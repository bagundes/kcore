using System;
using System.Collections.Generic;
using System.Text;

namespace KCore.Lists
{
    public class DynamicList : List<Dynamic>
    {
        public void Add(dynamic value)
        {
            base.Add(new Dynamic(value));
        }

        public void Add(object value, string text)
        {
            base.Add(new Dynamic(value, text));
        }
    }

    public class DynamicDic : Dictionary<string, Dynamic>
    {
        public void Add(object value)
        {
            var text = Dynamic.From(value).ToString();
            base.Add(text, Dynamic.From(value));
        }

        public void Add(object value, string text)
        {
            base.Add(text, new Dynamic(value));
        }
    }
}
