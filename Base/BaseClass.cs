using Newtonsoft.Json;

namespace KCore.Base
{
    public abstract class BaseClass
    {
        [JsonIgnoreAttribute]
        /// <summary>
        /// Log class name
        /// </summary>
        public virtual string LOG => this.GetType().Name;

        [JsonIgnoreAttribute]
        /// <summary>
        /// Object class name
        /// </summary>
        public virtual string ObjClass => this.GetType().Name;

        [JsonIgnoreAttribute]
        /// <summary>
        /// Class version
        /// </summary>
        public virtual int Version => 1;

    }
}
