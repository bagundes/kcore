using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace KCore.Base
{
    public interface IBaseModel
    {
        Model.Select[] ToSelect();
        object ToJsonObject();
        string ToJson();
        string ToXML();
        object Response();
        T Clone<T>() where T : BaseModel;
    }

    public abstract class BaseModel : BaseClass, IBaseModel
    {
        public string Id;
        public int updated;
        public DateTime? lastModified;

        /// <summary>
        /// Transform all model properties to select model
        /// </summary>
        /// <returns></returns>
        public virtual Model.Select[] ToSelect()
        {
            //TODO - @blf - I need to create the recursive method when the property is other object
            var res = new List<Model.Select>();
            var name = this.GetType().Name;

            foreach (var proper in Reflection.FilterOnlyGetProperties(this))
                res.Add(new Model.Select(proper.GetValue(this), $"{this.LOG}.{proper.Name}".ToLower()));


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
#if DEBUG
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, 
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.None
                });

#else
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, 
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.None
                });
#endif
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

        ///// <summary>
        ///// Load this object with same informartion other object
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="model"></param>
        //public virtual void Clone<T>(T model) where T : BaseModel
        //{
        //    if (!this.Equals(model))
        //        throw new System.Exception();

        //    foreach (var p in KCore.Reflection.FilterOnlySetProperties(model))
        //        KCore.Reflection.SetValue(this, p.Name, p.GetValue(model));
        //}

        //public virtual T Clone<T>() where T : BaseModel, new()
        //{
        //    throw new NotImplementedException();

            //var t = (T) Activator.CreateInstance(this.GetType(), new object[] { });

            //foreach (var p in KCore.Reflection.FilterOnlySetProperties(this))
            //    KCore.Reflection.SetValue(t, p.Name, p.GetValue(p.Name));

            //return new T();
        //}

        public virtual string Serialize()
        {
            throw new System.NotImplementedException(this.LOG);
        }

        public T Clone<T>() where T : BaseModel
        {
            throw new NotImplementedException();
        }
    }
}
