using System;
using System.Collections.Generic;
using System.Text;

namespace KCore.Model
{
    public sealed class ResultSet3
    {
        private List<string> Columns = new List<string>();
        private List<Dynamic> Datas = new List<Dynamic>();
        public int LinesTotal => Datas.Count / Columns.Count;
        public int ColumnsTotal => Columns.Count;
        public Dynamic this[string key, int line]
        { get
            {
                if (!Columns.Exists(t => t.ToUpper() == key.ToUpper()))
                    throw new KeyNotFoundException($"Column {key} is not exists");

                var foo = (Columns.Count * line) - Columns.Count;
                var pos = foo < 0 ? 0 : foo;
                var index = Columns.FindIndex(a => a.ToUpper() == key.ToUpper()); //.IndexOf(key);

                return Datas[pos + index];
            }
        }

        /// <summary>
        /// Get fields in the line
        /// </summary>
        /// <param name="line">line to reads</param>
        /// <returns></returns>
        public Dictionary<string, Dynamic> GetFields(int line)
        {
            var list = new Dictionary<string, Dynamic>();
            var start = Columns.Count * line;
            var end = (start + Columns.Count);
            var col = 0;

            for (int pos = start; pos < end; pos++)
                list.Add(Columns[col++], Datas[pos]);

            return list;
        }


        public void AddColumn(string col)
        {
            Columns.Add(col);
        }

        public void AddData(object val)
        {
            Datas.Add(new Dynamic(val));
        }

        /// <summary>
        /// Verify if column name exists.
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public bool HasColumn(string col)
        {
            return Columns.Exists(t => t.ToUpper() == col.ToUpper());
        }



        /// <summary>
        /// Save the result in the file
        /// </summary>
        /// <param name="dest"></param>
        /// <returns></returns>
        public void SaveToFile(string dest, string delimited = ";", bool addColumName = false)
        {
            var lines = new List<string>();

            // fix
            switch (delimited)
            {
                case "\\t": delimited = "\t"; break;
                case "\\n": delimited = "\n"; break;
            }


            if(addColumName)
            {
                var val = String.Empty;
                foreach (var col in Columns)
                {
                    val += $"{col}{delimited}";
                }
                lines.Add(val.Substring(0, val.Length - delimited.Length));
                lines.Add(val.Substring(0, val.Length - delimited.Length));
            }


            for(int i = 0; i < LinesTotal; i++)
            {
                var val = String.Empty;
                foreach (var line in GetFields(i))
                {
                    val += $"{line.Value}{delimited}";
                }

                lines.Add(val.Substring(0, val.Length - delimited.Length));
                
            }

            
            KCore.Shell.File.Save(lines.ToArray(), dest, true, true);

        }
    }
}
