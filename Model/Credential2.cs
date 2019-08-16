using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace K.Core.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class Credential2 : Base.BaseModel
    {
        public override string LOG => "Credential";
        public static string Folder => R.Project.Folders.Credential;
        public static int Expire => R.Security.Expire;

    [JsonProperty]
        /// <summary>
        /// The instance name or location the application is working
        /// </summary>
        public string Instance { get;  internal set; }

        [JsonProperty]
        /// <summary>
        /// The host is connected the application
        /// </summary>
        public string Host { get; internal set; }

        [JsonProperty]
        /// <summary>
        /// User name to login
        /// </summary>
        public string User { get; internal set; }

        [JsonProperty]
        /// <summary>
        /// User id
        /// </summary>
        public int UId { get; internal set; }

        [JsonProperty]
        /// <summary>
        /// Key to access the Credential
        /// </summary>
        public string UserKey => key ?? Security.Hash.MD5(User, Host, Instance);
        private string key = null;

        [JsonProperty]
        /// <summary>
        /// Add personal properties
        /// </summary>
        public Dictionary<string,Dynamic> Properties { get; internal set; }



        [JsonProperty]
        public DateTime Created;

        [JsonProperty]
        public string CryptPasswd { get; internal set; }

        public DateTime LastUpdate { get; internal set; }
        public string Token
        {
            get
            {
                if (LastUpdate.AddMinutes(Expire) < DateTime.Now)
                    throw new KCoreException(LOG, C.MessageEx.LoginExpired6_0);
                else
                    return K.Core.Security.Hash.PasswdToKey(key, Serialize());
            }
        }

        /// <summary>
        /// Create the credential
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="host">host</param>
        /// <param name="instance">local machine</param>
        /// <param name="userKey">specific user key</param>
        /// <param name="uid">unique id</param>
        public Credential2(string user, string host, string instance, string userKey = null, int uid = -1)
        {
            this.User = user;
            this.Host = host;
            this.Instance = instance;
            this.key = userKey;
            this.Created = DateTime.Now;
            this.LastUpdate = DateTime.Now;
            this.UId = uid;
        }

        [JsonConstructor]
        public Credential2(string userKey, string host)
        {
            Load(userKey, host);
        }

        public string SetPassword(string passwd)
        {
            CryptPasswd = Security.Hash.PasswdToKey(UserKey, passwd);
            return UserKey;
        }

        public string GetPasswd()
        {
            return Security.Hash.KeyToPasswd(UserKey, CryptPasswd);
        }

        public string Save()
        {
            var foo = K.Core.Security.Hash.MD5(key, R.Security.MasterKey);
            var file = $"{Folder}\\{foo}.credential";
            
            
            K.Core.Shell.File.Save(Token, file, true, true);
            return Token;
        }

        public override string Serialize()
        {
            
            var bar = new PersonalSerialize
            {
                user = this.User,
                instance = this.Instance,
                host = this.Host,
                key = this.UserKey,
                uid = this.UId,
                cryppasswd = this.CryptPasswd,
                created = this.Created,
                properties = this.Properties,
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(bar);
        }

        public void Load(string key, string host)
        {
            var foo = K.Core.Security.Hash.MD5(key, R.Security.MasterKey);
            var file = new FileInfo($"{Folder}/{foo}.credential");
            this.key = key;
            LastUpdate = file.LastAccessTime;

            if (LastUpdate.AddMinutes(Expire) < DateTime.Now)
                throw new KCoreException(LOG, C.MessageEx.LoginExpired6_0);

            string text = System.IO.File.ReadAllText(file.ToString());
            var json = K.Core.Security.Hash.KeyToPasswd(UserKey, text);
            var cred = Newtonsoft.Json.JsonConvert.DeserializeObject<PersonalSerialize>(json);

#if !DEBUG
            if(cred.host != host)
                throw new KCoreException(LOG, C.MessageEx.E_InvalidKey9);
#endif
            this.User = cred.user;
            this.Instance = cred.instance;
            this.Host = cred.host;
            this.key = cred.key;
            this.UId = cred.uid;
            this.CryptPasswd = cred.cryppasswd;
            this.Created = cred.created;
            this.Properties = cred.properties;
        }

        public Dynamic GetProperty(string key)
        {
            if (Properties.Where(t => t.Key.ToUpper() == key.ToUpper()).Any())
                return Properties.Where(t => t.Key.ToUpper() == key.ToUpper()).Select(t => t.Value).FirstOrDefault();
            else
                return Dynamic.Empty;
        }

        public void SetProperty(string key, dynamic value)
        {
            if (Properties == null)
                Properties = new Dictionary<string, Dynamic>();

            if (Properties.Where(t => t.Key.ToUpper() == key.ToUpper()).Any())
                Properties[key] = value;
            else
                Properties.Add(key, new Dynamic(value));
        }

        public void DestroySession()
        {
            var foo = K.Core.Security.Hash.MD5(UserKey, R.Security.MasterKey);
            var file = new FileInfo($"{Folder}/{foo}.credential");
            System.IO.File.Delete(file.ToString());
        }
        private class PersonalSerialize
        {
            public string user;
            public string instance;
            public string host;
            public string key;
            public string cryppasswd;
            public int uid;
            public DateTime created;
            public Dictionary<string, Dynamic> properties;
        }

    }
}
