using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KCore.Base;
using KCore.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace KCore.Lists
{
    /// <summary>
    /// List of selects object
    /// </summary>
    public partial class SelectList : List<Select>//, Base.ISelects
    {
        /// <summary>
        /// Default select
        /// </summary>
        public string Default;

        public void Add(object value)
        {
          base.Add(new Select(value, false));
        }

        public void Add(object value, string text, bool flag = false, bool encrypt = false)
        {
            base.Add(new Select(value, text, flag, encrypt));
        }

        public void AddEncrypted(object value)
        {
            base.Add(new Select(value, true));
        }

        /// <summary>
        /// Convert select list to json format
        /// </summary>
        /// <param name="encrypt">Encrypt the json?</param>
        /// <returns></returns>
        public string ToJson(bool encrypt = false)
        {
            var formatting = Formatting.None;

            if (R.IsDebugMode)
                formatting = Formatting.Indented;

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(this,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = formatting
                });


            return encrypt ? KCore.Security.Hash.ValueToToken(json) : json;
        }

        /// <summary>
        /// Convert select to dictionary.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, dynamic> ToList()
        {
            var res = new Dictionary<string, dynamic>();

            foreach (var l in this)
                res.Add(l.text, l.value);

            return res;
        }

        /// <summary>
        /// Encrypt all values of select.
        /// </summary>
        public void EncryptAll()
        {
            foreach (var sel in this)
                sel.EncryptValue();
        }

        public dynamic GetValue(string text)
        {
            return this.Where(t => t.text.Equals(text, StringComparison.InvariantCultureIgnoreCase)).Select(t => t.value).FirstOrDefault();
        }

        public string GetText(dynamic value)
        {
            return this.Where(t => t.value == value).Select(t => t.text).FirstOrDefault();
        }
    }

    public partial class SelectList
    {
        /// <summary>
        /// Loading the json (encrypted or not).
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static SelectList Load(string json)
        {
            try
            {
                var foo = KCore.Security.Hash.TokenToValue(json);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<SelectList>(foo);
            }
            catch
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<SelectList>(json);
            }
        }
    }
}
