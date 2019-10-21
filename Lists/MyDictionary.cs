using System;
using System.Collections.Generic;
using System.Linq;

namespace KCore.Lists
{
    /// <summary>
    /// Global config list.
    /// </summary>
    public class MyDictionary : Dictionary<string, dynamic>
    {
        public string LOG => this.GetType().Name;

        public MyDictionary() { }
        public MyDictionary(Dictionary<string, dynamic> d)
        {
            foreach (var v in d)
                Add(v.Key, v.Value);
        }
        public MyDictionary(string filename)
        {
            var json = System.IO.File.ReadAllText(filename);
            var d = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);
            foreach (var v in d)
                Add(v.Key, v.Value);
        }

        /// <summary>
        /// Get the parameter value
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="msgerror">Create exception when the Key is not exists</param>
        /// <returns></returns>
        public Dynamic Get(string key, string msgerror = null)
        {

            if (this.Count > 0 && this.Where(t => t.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase)).Any())
                return new Dynamic(this.Where(t => t.Key.ToUpper() == key.ToUpper()).Select(t => t.Value).FirstOrDefault());
            else
            {
                if (!String.IsNullOrEmpty(msgerror))
                    throw new KCoreException(LOG, C.MessageEx.StoredCacheError10_1, msgerror);
                else
                    return Dynamic.Empty;
            }
        }

        /// <summary>
        /// Add or update the value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, dynamic value)
        {
            if (this.Count < 1)
                Add(key, value);
            else if (ContainsKey(key))
                this[key] = value;
            else
                this.Add(key, value);
        }

        /// <summary>
        /// Verify if key exists (it is ignoring case)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public new bool ContainsKey(string key)
        {
            return this.Where(t => t.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)).Any();
        }

        public IReadOnlyDictionary<string, dynamic> AsReadOnly()
        {
            return this;
        }
    }
}
