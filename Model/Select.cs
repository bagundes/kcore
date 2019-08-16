using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace K.Core.Model
{
    [Obsolete]
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class Select : Base.BaseModel
    {
        [JsonProperty]
        public dynamic Value { get; set; }
        [JsonProperty]
        public string Text { get; set; }
        public bool Default { get; set; }

        public Select() { }

        public Select(dynamic value)
        {
            Value = value;
        }

        public Select(dynamic value, string text, bool encrypt = false, bool @default = false)
        {
            Value = encrypt ? K.Core.Security.Hash.Encrypt(value) : value;
            Text = text;
            Default = @default;
        }


        /// <summary>
        /// Load the values in a string.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static Select[] Split(string values, char separator)
        {
            var strArray = values.Split(separator);
            var sel = new List<Select>();

            foreach (var str in strArray)
                sel.Add(new Select(str));

            return sel.ToArray();
        }
    }
}
