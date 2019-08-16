using KCore.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace KCore.Base
{
    public interface IBaseClient : System.IDisposable
    {
        #region Properties
        bool IsFirstLine { get; }
        int FieldCount { get; }
        string LastCommand { get; }
        DataInfo DataInfo { get; }
        #endregion

        void Connect();
        void Connect(string dbase);
        void Connect(DataInfo connDetails);
        void Connect(Credential2 cred);

        #region Execute
        bool DoQuery(string sql, params dynamic[] values);
        bool NoQuery(string sql, params dynamic[] values);
        bool Procedure(string name, params object[] values);
        bool Next(int limit = -1);
        #endregion

        #region Result
        Dynamic Field(object index);
        Dictionary<string, Dynamic> Fields(bool upper = true);
        Dynamic[] Line1();

        /// <summary>
        /// return values of column 
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <returns></returns>
        T[] Column<T>(object index);

        Dictionary<string, Dynamic> Line2();
        #endregion

        #region Check
        bool HasDatabase(string name);
        bool HasTable(string database, string table);
        bool HasColumn(string database, string table, string column);
        #endregion

        #region Properties
        int Version();
        ColumnStruct[] Columns(string dsource, string table);
        #endregion
    }
}
