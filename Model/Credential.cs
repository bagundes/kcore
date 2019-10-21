using KCore.Lists;
using KCore.Stored;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KCore.Model
{
    /// <summary>
    /// Credential - Version 3.
    /// Credential may expire in two ways, time of uselessness or due date.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class Credential : Base.BaseModel
    {
        #region Headers
        [JsonIgnoreAttribute]
        public override int Version => 3;
        [JsonIgnoreAttribute]
        public new string LOG => "Credential";
        private string Folder => R.Project.Folders.Credential;
        #endregion

        public string Id { get; internal set; }
        private string extension => $"crdv{Version}".ToLower();
        private List<string> role;

        /// <summary>
        /// Expire after N minutes
        /// </summary>
        private int Expire = R.Security.Expire;       

        #region Information about location
        /// <summary>
        /// The host is connected the credential
        /// </summary>
        public string Host { get; internal set; }

        /// <summary>
        /// The instance name or location the application is working
        /// </summary>
        public string Instance { get; internal set; }
        #endregion

        #region Login
        /// <summary>
        /// User name to login
        /// </summary>
        public string User { get; internal set; }
        #endregion

        #region Properties
        /// <summary>
        /// Add personal properties
        /// </summary>
        public MyDictionary Properties = new MyDictionary();

        /// <summary>
        /// Due date
        /// </summary>
        public DateTime DueDate { get; internal set; }

        /// <summary>
        /// Roles to this credentials.
        /// </summary>
        public string[] Roles => role != null ? role.ToArray() : new string[0] { };

        /// <summary>
        /// The key to encrypt the information about Credential.
        /// </summary>
        private string Security => KCore.Security.Hash.MD5(User, Host, Instance, KCore.R.Security.MasterKey);
        #endregion

        #region Security
        /// <summary>
        /// Token created using the internal information.
        /// If you add or change the any information in the credential.
        /// </summary>
        private string InternalToken;
        #endregion

        /// <summary>
        /// Create the credential with random password.
        /// </summary>
        /// <param name="host">host</param>
        /// <param name="instance">local machine</param>
        /// /// <param name="user">user</param>
        public Credential(string host, string instance, string user)
        {
            this.User = user;
            this.Host = host;
            this.Instance = instance;
            //this.lastModified = DateTime.Now;
            this.SetPassword(KCore.Security.Chars.RandomChars(8, true));
            this.Id = Security;
            this.DueDate = DateTime.Now.AddMinutes(Expire);
        }

        /// <summary>
        /// Load the credential.
        /// </summary>
        /// <param name="key">Key to access the credential</param>
        public Credential(string key)
        {
            string id = null;
            if (key.StartsWith("@") && key.EndsWith("@"))
                id = KCore.Security.Hash.TokenToValue(key);
            else
                id = IdToKey(key);

            var file = new FileInfo($"{Folder}/{id}.{extension}");

            if (!file.Exists)
                throw new KCoreException(LOG, C.MessageEx.InvalidCredential11_0);

            string text = System.IO.File.ReadAllText(file.ToString());
            string json;
            if (!KCore.R.IsDebugMode)
                json = KCore.Security.Hash.Decrypt(text, id);
            else
                json = text;

            #region Loading
            var cred = Newtonsoft.Json.JsonConvert.DeserializeObject<PersonalSerialize>(json);
            this.Id = cred.id;
            this.Host = cred.host;
            this.Instance = cred.instance;
            this.User = cred.user;
            this.InternalToken = cred.internalToken;
            this.DueDate = cred.duedate;
            this.Expire = cred.expire;
            this.Properties =  new MyDictionary(cred.properties);
            this.role = cred.roles;
            #endregion

            var lastAccess = file.LastAccessTime;
            if (Expire == -1)
            {
                if (DueDate < DateTime.Now)
                {
                    DestroySession();
                    throw new KCoreException(LOG, C.MessageEx.LoginExpired6_0);
                }
            }
            else if (lastAccess.AddMinutes(Expire) < DateTime.Now)
            {
                DestroySession();
                throw new KCoreException(LOG, C.MessageEx.LoginExpired6_0);
            }
        }

        /// <summary>
        /// Add due date on credential
        /// </summary>
        public void AddDueDate(DateTime duedate)
        {
            Expire = -1;
            DueDate = duedate;
        }

        public string GetKey() => KCore.Security.Hash.ValueToToken(Id);

        #region Password
        /// <summary>
        /// Set the password
        /// </summary>
        /// <param name="passwd">Password</param>
        public void SetPassword(string passwd)
        {

            InternalToken = KCore.Security.Hash.Encrypt(passwd, Security);
        }

        /// <summary>
        /// Get the password
        /// </summary>
        /// <returns></returns>
        public string GetPasswd()
        {
            return KCore.Security.Hash.Decrypt(InternalToken, Security);
        }
        #endregion

        #region Save      
        /// <summary>
        /// Save the credential using Id.
        /// </summary>
        /// <returns>Key</returns>
        public string Save()
        {
            var file = $"{Folder}\\{Id}.{extension}";
            string json;
            if (!KCore.R.IsDebugMode)
                json = KCore.Security.Hash.Encrypt(Serialize(), Id);
            else
                json = Serialize();

            KCore.Shell.File.Save(json, file, true, true);
            return GetKey();//KCore.Security.Hash.ValueToToken(Id);
        }

        /// <summary>
        /// Add role name
        /// </summary>
        /// <param name="name"></param>
        public void AddRole(string name)
        {
            name = name.ToLower().Trim();
            if (role == null)
                role = new List<string>();

            if (!ExistsRole(name))
                role.Add(name);
        }

        /// <summary>
        /// Verify if role exists
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ExistsRole(string name)
        {
            name = name.ToLower().Trim();
            if (role == null)
                return false;
            else
                return role.Where(t => t.Equals(name, StringComparison.InvariantCultureIgnoreCase)).Any();
        }

        /// <summary>
        /// Serealize this object
        /// </summary>
        /// <returns></returns>
        private new string Serialize()
        {
            var ps = new PersonalSerialize
            {
                id = Id,
                host = Host,
                instance = Instance,
                user = User,
                internalToken = InternalToken,
                properties = Properties,
                duedate = DueDate,
                roles = role,
                expire = Expire,
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(ps);
        }
        #endregion

        /// <summary>
        /// Destroy the sassion saved
        /// </summary>
        public void DestroySession()
        {
            var file = new FileInfo($"{Folder}/{Id}.credential");
            System.IO.File.Delete(file.ToString());
        }

        #region Convert
#pragma warning disable CS0612 // Type or member is obsolete
        /// <summary>
        /// Convert to old class version
        /// </summary>
        public Credential_v2 ToV2()

        {
            var credv2 = new Credential_v2(User, Host, Instance, Security, Properties.Get("OUSR.USERID").ToInt(-1));
            credv2.SetPassword(GetPasswd());
            foreach (var p in Properties)
                credv2.SetProperty(p.Key, p.Value);

            return credv2;
        }
#pragma warning restore CS0612 // Type or member is obsolete

        /// <summary>
        /// Create datainfo model.
        /// Host => Server
        /// Instance => Schema
        /// User => User
        /// Password => Password
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataInfo ToDataInfo(C.Database.DBaseType type)
        {
            return new DataInfo(Host, Instance, User, GetPasswd(), type);
        }

        /// <summary>
        /// Force a new Id.
        /// </summary>
        /// <param name="new_id">new id will create using md5 hash</param>
        public void SetId(string new_id)
        {
            this.Id = KCore.Security.Hash.MD5(new_id);
        }

        /// <summary>
        /// Convert you Id to key
        /// </summary>
        /// <param name="my_id">Id setted setId method</param>
        /// <returns></returns>
        public static string IdToKey(string my_id)
        {
            var id = KCore.Security.Hash.MD5(my_id);
            return KCore.Security.Hash.ValueToToken(id);
        }

        #endregion
        private class PersonalSerialize
        {
            public string id;
            public string user;
            public string instance;
            public string host;
            public string internalToken;
            public DateTime duedate;
            public int expire;
            public Dictionary<string, dynamic> properties;
            public List<string> roles;
        }

    }
}
