using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KCore.Model
{
    [Obsolete]
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class Credential_v2 : Base.BaseModel_v1
    {
        public override int Version => 2;
        public override string LOG => "Credential";
        public static string Folder => R.Project.Folders.Credential;
        public static int Expire => R.Security.Expire;

        #region Information about location
        [JsonProperty]
        /// <summary>
        /// The host is connected the credential
        /// </summary>
        public string Host { get; internal set; }
        [JsonProperty]
        /// <summary>
        /// The instance name or location the application is working
        /// </summary>
        public string Instance { get; internal set; }
        #endregion

        #region Login
        [JsonProperty]
        /// <summary>
        /// User name to login
        /// </summary>
        public string User { get; internal set; }

        [JsonProperty]
        /// <summary>
        /// Unique Identification
        /// </summary>
        public int UId { get; internal set; }

        [JsonProperty]
        /// <summary>
        /// Key to access the Credential
        /// </summary>
        public string UserKey => key ?? Security.Hash.MD5(User, Host, Instance);
        private string key = null;
        #endregion

        #region Properties
        [JsonProperty]
        public DateTime Created;
        [JsonProperty]
        public DateTime LastUpdate { get; internal set; }
        #endregion

        #region Security
        [JsonProperty]
        public string CryptPasswd { get; internal set; }
        #endregion



        [JsonProperty]
        /// <summary>
        /// Add personal properties
        /// </summary>
        public Dictionary<string, Dynamic> Properties { get; internal set; }


        

        

        




        public string Token
        {
            get
            {
                if (LastUpdate.AddMinutes(Expire) < DateTime.Now)
                    throw new KCoreException(LOG, C.MessageEx.LoginExpired6_0);
                else
                    return KCore.Security.Hash.Encrypt(Serialize(), key);
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
        public Credential_v2(string user, string host, string instance, string userKey = null, int uid = -1)
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
        public Credential_v2(string userKey, string host)
        {
            Load(userKey, host);
        }

        public string SetPassword(string passwd)
        {
            CryptPasswd = Security.Hash.Encrypt(passwd, UserKey);
            return UserKey;
        }

        public string GetPasswd()
        {
            return Security.Hash.Decrypt(CryptPasswd, UserKey);
        }

        public string Save()
        {
            var file = $"{Folder}\\{key}.credential";


            KCore.Shell.File.Save(Token, file, true, true);
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
            var foo = KCore.Security.Hash.MD5(key, R.Security.MasterKey);
            var file = new FileInfo($"{Folder}/{foo}.credential");

            if (!file.Exists)
                throw new KCoreException(LOG, C.MessageEx.InvalidCredential11_0, file);

            this.key = key;
            LastUpdate = file.LastAccessTime;

            if (LastUpdate.AddMinutes(Expire) < DateTime.Now)
                throw new KCoreException(LOG, C.MessageEx.LoginExpired6_0);

            string text = System.IO.File.ReadAllText(file.ToString());
            var json = KCore.Security.Hash.Decrypt(text, UserKey);
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
            var foo = KCore.Security.Hash.MD5(UserKey, R.Security.MasterKey);
            var file = new FileInfo($"{Folder}/{foo}.credential");
            System.IO.File.Delete(file.ToString());
        }

        /// <summary>
        /// Clone the credential with new key and host
        /// </summary>
        /// <param name="userKey">New key</param>
        public Credential_v2 Clone(string userKey)
        {
            return new Credential_v2(this.User, this.Host, this.Instance, userKey, this.UId);
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
