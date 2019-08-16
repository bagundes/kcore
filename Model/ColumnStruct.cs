using System;
using System.Collections.Generic;
using System.Text;

namespace K.Core.Model
{
    public class ColumnStruct : Base.BaseModel
    {
        public string DBase { get; private set; }
        public string Table { get; private set; }
        public string Name { get; private set; }
        public bool PK { get; private set; }
        public int? Size { get; private set; }
        public bool Required { get; private set; }
        public C.Database.ColumnType ColType { get; private set; }
        public dynamic Default { get; private set; }


        public ColumnStruct(string dBase, string table, string name, C.Database.ColumnType type, bool required, dynamic def, int? size, bool pk = false)
        {
            DBase = dBase;
            Table = table;
            Name = name;
            Required = required;
            ColType = type;
            Default = def;
            PK = pk;

            if (type == C.Database.ColumnType.Text && (size == null || size < 1))
                Size = 150;
            else
                Size = null;
        }
    }
}
