using System;

namespace KCore.Model
{
    public sealed class DataInfo : Base.BaseModel_v1
    {
        public string Server { get; }
        public string Schema { get; set; }
        public string User { get; }
        public string Password { get; }
        public int Port { get; }
        public string Driver { get; }
        public bool Default { get; set; } = false;
        public C.Database.DBaseType DBaseType { get; }


        public DataInfo() { }
        public DataInfo(string server, string schema, string user, string passwd, C.Database.DBaseType type)
        {
            Server = server;
            Schema = schema;
            User = user;
            Password = passwd;
            DBaseType = type;

            switch (type)
            {
                case C.Database.DBaseType.Hana:
                    Port = 30015; break;
                case C.Database.DBaseType.MSQL: Port = 1433; break;
            }
        }

        public DataInfo(string server, string schema, string user, string passwd, int port, string driver, C.Database.DBaseType type)
        {
            Server = server;
            Schema = schema;
            User = user;
            Password = passwd;
            DBaseType = type;
            Port = port;
            Driver = driver;
        }

        public override string ToString()
        {
            if (R.IsDebugMode)
                return ToConnString(false);
            else
                return ToConnString(true);

        }

        public string URL()
        {
            return $"{Server}.{Schema}/user:{User}";
        }

        /// <summary>
        /// Create the connection string
        /// </summary>
        /// <param name="hidden">Hidden the informations</param>
        /// <returns></returns>
        public string ToConnString(bool hidden = false)
        {
            var password = hidden ? new string('*', Password.Length) : Password;


            switch (DBaseType)
            {
                case C.Database.DBaseType.Hana:
                    return String.Format("DRIVER={0};SERVERNODE={1}:{2};UID={3};PWD={4};CS={5}", Driver, Server, Port, User, password, Schema);
                case C.Database.DBaseType.MSQL:
                    return String.Format("server={0};initial catalog={1};user id={2};password={3};", Server, Schema, User, password);
                default:
                    throw new NotImplementedException();
            }
        }

        public DataInfo Clone()
        {
            return new DataInfo(
                Server,
                Schema,
                User,
                Password,
                Port,
                Driver,
                DBaseType);
        }
    }
}
