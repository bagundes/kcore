using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;

namespace KCore.Base
{
    [Obsolete]
    public interface IBaseModel_v1
    {
        Model.Select_v1[] ToSelect();
        object ToJsonObject();
        string ToJson();
        string ToXML();
        object Response();
        T Clone<T>() where T : BaseModel_v1;
    }

    [Obsolete]
    public abstract class BaseModel_v1 : BaseClass, IBaseModel_v1
    {
        public string Id;
        public int updated;
        public DateTime? lastModified;

        /// <summary>
        /// Transform all model properties to select model
        /// </summary>
        /// <returns></returns>
        public virtual Model.Select_v1[] ToSelect()
        {
            //TODO - @blf - I need to create the recursive method when the property is other object
            var res = new List<Model.Select_v1>();
            var name = this.GetType().Name;

            foreach (var proper in Reflection.GetFields(this))
            {
                var val = proper.GetValue(this);
                if (val != null && val.GetType() == typeof(KCore.Stored.PropertiesList_v1))
                {
                    var foo = (KCore.Stored.PropertiesList_v1)val;
                    foreach (var v in foo.GetList())
                        res.Add(new Model.Select_v1(v.Value, $"{this.ObjClass}.p.{v.Key}".ToLower()));
                }
                else
                {
                    res.Add(new Model.Select_v1(proper.GetValue(this), $"{this.LOG}.{proper.Name}".ToLower()));
                }
            }

            foreach(var proper in Reflection.FilterOnlyGetProperties(this))
                res.Add(new Model.Select_v1(proper.GetValue(this), $"{this.LOG}.{proper.Name}".ToLower()));


            return res.ToArray();
        }

        public virtual object ToJsonObject()
        {
            var foo = Newtonsoft.Json.JsonConvert.SerializeObject(this,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.None
                });

            return JsonConvert.DeserializeObject(foo);
        }

        public virtual string ToJson()
        {
            if (R.DebugMode)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(this,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Formatting = Formatting.None
                    });
            }
            else
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(this,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Formatting = Formatting.None
                    });
            }
        }
        public virtual string ToXML()
        {
            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(stringwriter, this);
                return stringwriter.ToString();
            }
        }

        public virtual object Response()
        {
            return this;
        }

        public virtual void Load(Dictionary<string, Dynamic> line)
        {

            foreach (var p in KCore.Reflection.FilterOnlySetProperties(this))
            {
                if (line.Where(t => t.Key.ToUpper() == p.Name.ToUpper()).Any())
                {
                    var val = line.Where(t => t.Key.ToUpper() == p.Name.ToUpper()).Select(t => t.Value).FirstOrDefault();
                    KCore.Reflection.SetValue(this, p.Name, val);
                }
            }
        }

        /// <summary>
        /// Loading the model using json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        protected void Load<T>(string json) where T : BaseModel_v1
        {
            var model = JsonConvert.DeserializeObject<T>(json);
            foreach (var p in KCore.Reflection.FilterOnlySetProperties(this))
            {
                KCore.Reflection.SetValue(this, p.Name, model.GetFieldValue(p.Name));
            }
        }

        /// <summary>
        /// Get value of field
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetFieldValue(string name)
        {
            return KCore.Reflection.GetValue(this, name);
        }

        /// <summary>
        /// Set value in the field
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        protected void SetFieldValue(string name, object val)
        {
            KCore.Reflection.SetValue(this, name, val);
        }

        public virtual string Serialize()
        {
            throw new System.NotImplementedException(this.LOG);
        }

        public T Clone<T>() where T : BaseModel_v1
        {
            throw new NotImplementedException();
        }
    }
}
