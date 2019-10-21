using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KCore.Stored
{
    

    [Obsolete("Use global config list")]
    public class PropertiesList_v1 : KCore.Base.BaseModel_v1
    {
        public string LOG => this.GetType().Name;

        private Dictionary<string, dynamic> properties;
        public Dictionary<string, dynamic> Properties
        {
            get
            {

                if(properties != null)
                    return properties.ToDictionary(p => p.Key, p => p.Value);
                else
                    return new Dictionary<string, dynamic>();
            }
        }

        public PropertiesList_v1() { }

        public PropertiesList_v1(Dictionary<string, dynamic> dic)
        {
            properties = dic;
        }

        public PropertiesList_v1(string file)
        {
            var json = System.IO.File.ReadAllText(file);
            properties = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);
        }

        /// <summary>
        /// Get the parameter value
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="msgerror">Create exception when the Key is not exists</param>
        /// <returns></returns>
        public Dynamic Get(string key, string msgerror = null)
        {

            if (properties != null && properties.Where(t => t.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase)).Any())
                return new Dynamic(properties.Where(t => t.Key.ToUpper() == key.ToUpper()).Select(t => t.Value).FirstOrDefault());
            else
            {
                if (!String.IsNullOrEmpty(msgerror))
                    throw new KCoreException(LOG, C.MessageEx.StoredCacheError10_1, msgerror);
                else
                    return Dynamic.Empty;
            }
        }

        public Dictionary<string, dynamic> GetList()
        {
            return new Dictionary<string, dynamic>(properties);
        }

        public void Set(string key, dynamic value)
        {
            if (properties == null)
                properties = new Dictionary<string, dynamic>();

            if (properties.Where(t => t.Key.ToUpper() == key.ToUpper()).Any())
                properties[key] = value;
            else
                properties.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return properties.ContainsKey(key);
        }
    }
}
