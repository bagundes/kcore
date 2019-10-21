using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace KCore.Model
{
    [Obsolete]
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class Select_v2 : Base.BaseModel_v1
    {
        [JsonProperty]
        public dynamic value;
        [JsonProperty]
        public string text;
        public bool encrypt;
        public bool def;

        public Select_v2() { }

        public Select_v2(object value, bool encrypt = false)
        {
            this.value = encrypt ? KCore.Security.Hash.ValueToToken(value.ToString()) : value;
            this.text = value.ToString();
            this.encrypt = encrypt;
        }

        public Select_v2(object value, string text, bool encrypt = false, bool @default = false)
        {
            this.value = encrypt ? KCore.Security.Hash.Encrypt(value.ToString(), null) : value;
            this.text = text;
            this.def = @default;
            this.encrypt = encrypt;
        }


        /// <summary>
        /// Load the values in a string.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static Select_v2[] Split(string values, char separator)
        {
            var strArray = values.Split(separator);
            var sel = new List<Select_v2>();

            foreach (var str in strArray)
                sel.Add(new Select_v2(str));

            return sel.ToArray();
        }

        public Select_v1 ToV1()
        {
            return new Select_v1(value, text, encrypt, def);
        }
    }
}
