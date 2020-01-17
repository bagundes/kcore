using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KCore
{
    public class Reflection
    {
        /// <summary>
        /// Set value in the property.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="model">Object model</param>
        /// <param name="name">Property name</param>
        /// <param name="value">Value</param>
        /// <returns>Accepted</returns>
        public static bool SetValue<T>(T model, string name, object value) where T : Base.BaseModel_v1
        {
            try
            {
                name = (model.GetType().GetProperties()).Where(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).Select(t => t.Name).FirstOrDefault();

                if (name is null) return false;
                if (String.IsNullOrEmpty(value.ToString())) return true;
                var proper = model.GetType().GetProperty(name);

                if (proper is null)
                    return false;

                proper.SetValue(model, value);

                return true;
            }
            catch (Exception ex)
            {
                KCore.Diagnostic.Error(R.ID, model.LOG, ex.Message, ex.StackTrace, ex.Source + $"\nParameters:\n\tmodel:{typeof(T).Name}, name:{name} and value:{value}");
                return false;
            }
        }

        public static object GetValue<T>(T obj, string name)
        {
            var p = obj.GetType();

            // Properties
            var key = p.GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (key != null)
                return key.GetValue(obj);

            // Fields
            var key1 = p.GetField(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (key1 != null)
                return key1.GetValue(obj);
            else
                return null;
        }

        /// <summary>
        /// Verify if the property or field exists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns>-1 none, 1 property and 2 field</returns>
        public static int HasPropertyOrField<T>(T obj, string name)
        {
            var p = obj.GetType();

            // Properties
            var key = p.GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (key != null)
                return 1;

            // Fields
            var key1 = p.GetField(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (key1 != null)
                return 2;
            else
                return -1;
        }

        public static object GetMember<T>(T obj, string name)
        {
            var m = obj.GetType();
            var key = m.GetMember(name);
            var foo = key.GetValue(0);

            return foo;

        }

        public static PropertyInfo[] FilterOnlySetProperties(Object obj)
        {
            var p = obj.GetType();
            var res = new List<PropertyInfo>();

            foreach (var proper in p.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var setter = proper.GetSetMethod();

                if (setter != null)
                    res.Add(proper);
            }

            return res.ToArray();
        }


        public static FieldInfo[] GetFields(Object obj)
        {
            var p = obj.GetType();
            var res = new List<FieldInfo>();

            foreach (var proper in p.GetFields())
            {

                if (proper != null && proper.Name != "get_LOG")
                    res.Add(proper);
            }

            return res.ToArray();
        }

        public static MemberInfo[] GetMembers(Object obj)
        {
            var p = obj.GetType();
            var res = new List<MemberInfo>();
            var ignore = new string[] { "Equals", "GetHashCode", "GetType", "ToString", ".ctor" };

            foreach (var member in p.GetMembers())
            {
                if (!ignore.Contains(member.Name))
                    res.Add(member);
            }


            return res.ToArray();
        }



        public static PropertyInfo[] FilterOnlyGetProperties(Object obj)
        {
            var p = obj.GetType();
            var res = new List<PropertyInfo>();

            foreach (var proper in p.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var getter = proper.GetGetMethod();

                if (getter != null && getter.Name != "get_LOG")
                    res.Add(proper);
            }

            return res.ToArray();
        }
    }
}
