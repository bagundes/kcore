using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KCore.Stored
{
    public class PropertiesModel
    {
        private Dictionary<string, dynamic> Properties;


        public PropertiesModel() { }

        public PropertiesModel(string file)
        {
            var json = System.IO.File.ReadAllText(file);
            Properties = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);
        }

        public Dynamic Get(string key)
        {
            if (Properties.Where(t => t.Key.ToUpper() == key.ToUpper()).Any())
                return Properties.Where(t => t.Key.ToUpper() == key.ToUpper()).Select(t => t.Value).FirstOrDefault();
            else
                return Dynamic.Empty;
        }

        public void Set(string key, dynamic value)
        {
            if (Properties == null)
                Properties = new Dictionary<string, dynamic>();

            if (Properties.Where(t => t.Key.ToUpper() == key.ToUpper()).Any())
                Properties[key] = value;
            else
                Properties.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return Properties.ContainsKey(key);
        }
    }
}
