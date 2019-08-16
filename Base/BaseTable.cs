using System;
using System.Collections.Generic;
using System.Text;

namespace KCore.Base
{
    public abstract class BaseTable : BaseModel
    {
        public TableInfo TabInfo { get; private set; }
        public Dictionary<string, KCore.Dynamic> Fields { get; set; }



        //public KCore.Base.BaseTable() { }
        public BaseTable(string dsource, string table, string[] pKey, bool ai)
        {
            TabInfo = new TableInfo(dsource, table, pKey, ai);
        }

        public dynamic GetPKeyValue(int pk = 0)
        {
            return Reflection.GetValue(this, TabInfo.PKey[pk]);
        }

        public void SetPKeyValue(dynamic val, int pk = 0)
        {
            Reflection.SetValue(this, TabInfo.PKey[pk], val);
        }

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

        public void QFields(Dictionary<string,KCore.Dynamic> fields)
        {
            this.Fields = fields;
        }

        #region sub-classes
        public class TableInfo
        {
            public string DataSource { get; private set; }
            public string Table { get; private set; }
            public string[] PKey { get; private set; }
            public bool AutoIncrement { get; private set; }

            public TableInfo(string dsource, string table, string[] pKey, bool ai = false)
            {
                this.Table = table;
                this.PKey = pKey;
                this.DataSource = dsource;
                AutoIncrement = ai;
            }
        }
        #endregion

}
}
