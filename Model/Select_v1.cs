using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace KCore.Model
{
    [Obsolete]
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class Select_v1 : Base.BaseModel_v1
    {
        [JsonProperty]
        public dynamic Value { get; set; }
        [JsonProperty]
        public string Text { get; set; }
        public bool Default { get; set; }

        public Select_v1() { }

        public Select_v1(dynamic value)
        {
            Value = value;
        }

        public Select_v1(dynamic value, string text, bool encrypt = false, bool @default = false)
        {
            Value = encrypt ? KCore.Security.Hash.ValueToToken(value) : value;
            Text = text;
            Default = @default;
        }


        /// <summary>
        /// Load the values in a string.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static Select_v1[] Split(string values, char separator)
        {
            var strArray = values.Split(separator);
            var sel = new List<Select_v1>();

            foreach (var str in strArray)
                sel.Add(new Select_v1(str));

            return sel.ToArray();
        }
    }
}
