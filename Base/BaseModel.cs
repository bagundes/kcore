using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KCore.Base
{
    public interface IBaseModel : ICloneable
    {
        string ToXml(string filename = null, bool encrypt = false);
        string ToJson();
        string Serialize();

        Lists.SelectList ToSelect();
    }


    [Serializable()]
    [JsonObject(MemberSerialization.OptOut)]
    public abstract class BaseModel : BaseClass, IBaseModel
    {
        public virtual string Id => KCore.Security.Hash.MD5(this);
        public int Updated { get; set; }

        /// <summary>
        /// Transform all model properties and fields to select model
        /// </summary>
        /// <returns></returns>
        public virtual Lists.SelectList ToSelect()
        {
            var list = new Lists.SelectList();

            foreach (var proper in Reflection.GetFields(this))
            {
                var val = proper.GetValue(this);
                if (val != null && val.GetType() == typeof(Lists.MyDictionary))
                {
                    var mydic = (Lists.MyDictionary)val;
                    foreach (var v in mydic)
                        list.Add(v.Value, $"{this.ObjClass}.p.{v.Key}".ToLower());
                }
                else
                    list.Add(proper.GetValue(this), $"{this.LOG}.{proper.Name}".ToLower());
            }

            foreach (var proper in Reflection.FilterOnlyGetProperties(this))
                list.Add(proper.GetValue(this), $"{this.LOG}.{proper.Name}".ToLower());

            return list;
        }

        /// <summary>
        /// Serialze the object to json.
        /// You can overrite this method and serealize with your format.
        /// </summary>
        /// <returns></returns>
        public virtual string Serialize()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Serialize the model to xml file.
        /// </summary>
        /// <param name="filename">Save in file</param>
        /// <param name="encrypt">Encrypt file</param>
        /// <returns>XML format</returns>
        public virtual string ToXml(string filename = null, bool encrypt = false)
        {

            using (var ms = new MemoryStream())
            {
                using (var wfile = new StreamWriter(ms))
                {
                    var writer = new System.Xml.Serialization.XmlSerializer(this.GetType());

                    writer.Serialize(wfile, this);
                    wfile.Close();

                    if (String.IsNullOrEmpty(filename))
                        System.IO.File.WriteAllBytes(filename, ms.ToArray());

                    if (encrypt)
                        throw new NotImplementedException("encrypt file was not implemented");

                    return wfile.ToString();
                }
            }
        }

        /// <summary>
        /// Transform the model to json format.
        /// </summary>
        /// <returns>model json format</returns>
        public virtual string ToJson()
        {
            var formatting = Formatting.None;

            if (R.DebugMode)
                formatting = Formatting.Indented;

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(this,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = formatting
                });

            return json;
        }

        /// <summary>
        /// Clone the model
        /// </summary>
        /// <returns>A new model</returns>
        public virtual object Clone()
        {
            throw new NotImplementedException();
        }
                       
        #region Statics method
        public static T LoadXml<T>(string filename, bool encrypt = false) where T : BaseModel
        {
            System.Xml.Serialization.XmlSerializer reader =
                new System.Xml.Serialization.XmlSerializer(typeof(T));
            System.IO.StreamReader file = new System.IO.StreamReader(filename);
            var t = (T)reader.Deserialize(file);
            file.Close();

            return t;
        }

        public static T Deserialize<T>(string filename) where T : BaseModel
        {
            return LoadXml<T>(filename, true);
        }
        #endregion
    }
}
