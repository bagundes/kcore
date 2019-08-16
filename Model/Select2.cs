using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace KCore.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class Select2 : Base.BaseModel
    {
        [JsonProperty]
        public dynamic value;
        [JsonProperty]
        public string text;
        public bool def;

        public Select2() { }

        public Select2(dynamic value)
        {
            this.value = value;
        }

        public Select2(dynamic value, string text, bool encrypt = false, bool @default = false)
        {
            this.value = encrypt ? KCore.Security.Hash.Encrypt(value) : value;
            this.text = text;
            this.def = @default;
        }


        /// <summary>
        /// Load the values in a string.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static Select2[] Split(string values, char separator)
        {
            var strArray = values.Split(separator);
            var sel = new List<Select2>();

            foreach (var str in strArray)
                sel.Add(new Select2(str));

            return sel.ToArray();
        }
    }
}
