using System;
using System.Collections.Generic;
using System.Linq;

namespace KCore.Base
{
    /// <summary>
    /// Base to construct the model table
    /// </summary>
    public abstract class BaseTable_v1 : BaseModel_v1
    {
        #region Properties
        /// <summary>
        /// Information about the table
        /// </summary>
        public TableInformation TableInfo { get; private set; }
        
        /// <summary>
        /// Fields
        /// </summary>
        public Dictionary<string, KCore.Dynamic> Fields { get; internal set; }
        public bool IsUpdate => Fields != null && Fields.Count > 0; 

        private String[] _virtualPK;
        /// <summary>
        /// Internal Primary Key. If the PK is not defined the model will get PK in TableInfo properties.
        /// </summary>
        public String[] VirtualPK
        {
            get
            {
                if (_virtualPK == null)
                    return TableInfo.PKey;
                else
                    return _virtualPK;
            }
            protected set
            {
                _virtualPK = value;
            }
        }
        #endregion

        /// <summary>
        /// Base table
        /// </summary>
        /// <param name="dbase">Database</param>
        /// <param name="table">table</param>
        /// <param name="pKey">primary keys</param>
        public BaseTable_v1(string dbase, string table, string[] pKey)
        {
            TableInfo = new TableInformation(table, pKey, dbase);
        }

        #region Primary key

        /// <summary>
        /// Get virtual primary key value
        /// </summary>
        /// <param name="pk">primary key index</param>
        /// <returns>Value of primary key</returns>
        public dynamic GetPKeyValue(int pk = 0)
        {
            return Reflection.GetValue(this, TableInfo.PKey[pk]);
        }
        public dynamic GetVirtualPKeyValue(int pk = 0)
        {
            return Reflection.GetValue(this, VirtualPK[pk]);
        }

        public void SetVirtualPKeyValue(dynamic value, int pk = 0)
        {
            Reflection.SetValue(this, VirtualPK[pk], value);
        }

        /// <summary>
        /// Set primary key value
        /// </summary>
        /// <param name="val">Value</param>
        /// <param name="pk">Index</param>
        public void SetPKeyValue(dynamic val, int pk = 0)
        {
            Reflection.SetValue(this, TableInfo.PKey[pk], val);
        }
        #endregion

        public virtual bool Get(dynamic pKeyValue)
        {
            throw new NotImplementedException();
        }

        public virtual void Save()
        {
            throw new NotImplementedException();
        }

        public virtual void Delete()
        {
            throw new NotImplementedException();
        }

        public override void Load(Dictionary<string, KCore.Dynamic> fields)
        {
            this.Fields = fields;
            foreach(var prop in KCore.Reflection.FilterOnlySetProperties(this))
            {
                var value = this.Fields.Where(t => t.Key.ToUpper() == prop.Name.ToUpper()).Select(t => t.Value.Value).FirstOrDefault();
                if (value != null)
                    KCore.Reflection.SetValue(this, prop.Name, value);
            }
        }

        public virtual void UpdatePK() {}

        #region sub-classes

        /// <summary>
        /// Table infomation
        /// </summary>
        public class TableInformation
        {
            /// <summary>
            /// Table name
            /// </summary>
            public string Name { get; private set; }
            
            /// <summary>
            /// Virtual PK
            /// </summary>
            public string[] PKey { get; private set; }
            
            /// <summary>
            /// Database name
            /// </summary>
            public string DBase { get; internal set; }

            /// <summary>
            /// Table information
            /// </summary>
            /// <param name="table">Table name</param>
            /// <param name="pKey">The virtual pk</param>
            public TableInformation(string table, string[] pKey, string dbase)
            {
                this.DBase = dbase;
                this.Name = table;
                this.PKey = pKey;
            }

            /// <summary>
            /// Change the database information
            /// </summary>
            /// <param name="newdbase"></param>
            public void ChangeDatabase(string newdbase)
            {
                this.DBase = newdbase;
            }
        }
        #endregion

    }
}
