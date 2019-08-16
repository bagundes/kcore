﻿using System;
using System.Collections.Generic;
using System.Text;

namespace K.Core.Model
{
    public sealed class DataInfo : Base.BaseModel
    {
        public string Server { get; }
        public string Schema { get; set; }
        public string User { get; }
        public string Password { get; }
        public int Port { get; }
        public string Driver { get; }
        public bool Default { get; set; } = false;
        public C.Database.ServerType ServerType { get; }


        public DataInfo() { }
        public DataInfo(string server, string schema, string user, string passwd, C.Database.ServerType type)
        {
            Server = server;
            Schema = schema;
            User = user;
            Password = passwd;
            ServerType = type;

            switch(type)
            {
                case C.Database.ServerType.Hana:
                    Port = 30015; break;
                case C.Database.ServerType.MSQL: Port = 1433; break;
            }
        }

        public DataInfo(string server, string schema, string user, string passwd, int port, string driver, C.Database.ServerType type)
        {
            Server = server;
            Schema = schema;
            User = user;
            Password = passwd;
            ServerType = type;
            Port = port;
            Driver = driver;
        }

        public override string ToString()
        {
#if DEBUG
            return ToConnString(false);
#else
            return ToConnString(true);
#endif
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


            switch (ServerType)
            {
                case C.Database.ServerType.Hana:
                    return String.Format("DRIVER={0};SERVERNODE={1}:{2};UID={3};PWD={4};CS={5}", Driver, Server, Port, User, password, Schema);
                case C.Database.ServerType.MSQL:
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
                ServerType);
        }
    }
}
