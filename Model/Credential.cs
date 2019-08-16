using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace KCore.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class Credential : Base.BaseModel
    {
        [JsonProperty]
        public string User { get; internal set; }

        [JsonProperty]
        /// <summary>
        /// User Id - This is optional field
        /// </summary>
        public int UserId { get; set; }

        private string InternalKey => Security.Hash.MD5(User, Uid, Instance);
  
        /// <summary>
        /// Password encrypted
        /// </summary>
        public string PwdKey { get; internal set; }

        #region Optinal properties
        [JsonProperty]
        public string Uid { get; internal set; }
        public string Token
        {
            get
            {
                if (Expire < DateTime.Now)
                    throw new KCoreException(LOG, C.MessageEx.LoginExpired6_0);
                else
                    return KCore.Security.Hash.PasswdToKey(Instance, ToJson());
            }
        }
        [JsonProperty]
        public DateTime Expire { get; set; } = DateTime.Now.AddMinutes(R.Security.Expire);
        public string Instance { internal set;  get; }
        public string Host { get; internal set; }


        #endregion
        /// <summary>
        /// Credential
        /// </summary>
        /// <param name="user">User name</param>
        /// <param name="mkey">Master key to encrypt (example OUSR.PASSWD)</param>
        /// <param name="host">The company/database/server</param>
        /// <param name="instance">Machine name or ip</param>
        public Credential(string user, string mkey, string host, string instance = null)
        {
            this.User = user;
            this.Instance = instance ?? Dns.GetHostEntry(Environment.MachineName).HostName;
            this.Uid = SetUid(user + mkey + host);
            this.Host = host;
        }

        /// <summary>
        /// Credential
        /// </summary>
        /// <param name="token"></param>
        /// <param name="instance"></param>
        public Credential(string token, string instance)
        {
            try
            {
                var obj = KCore.Security.Hash.KeyToPasswd(instance, token);
                var val = Newtonsoft.Json.JsonConvert.DeserializeObject<Serialize>(obj);

                if (this.Expire < DateTime.Now)
                    throw new KCoreException(LOG, C.MessageEx.LoginExpired6_0);

                this.User = val.user;
                SetPwdKey(val.pwdkey);
                this.Uid = val.uid;
                this.Expire = val.expire;
                this.Instance = val.instance;
                this.Host = val.host;
                this.UserId = val.userid;
                
            }catch(Exception ex)
            {
                var id = KCore.Diagnostic.Track(LOG, $"Machine:{instance}", $"Token:{token}", ex.Message);
                KCore.Diagnostic.Error(R.ID, LOG, id, "Error when application tried to read token.");
                throw new KCoreException(LOG, C.MessageEx.LoginExpired6_0);
            }

        }

        /// <summary>
        /// Set the password.
        /// </summary>
        /// <param name="passwd">Password</param>
        /// <returns>Key</returns>
        public string SetPasswd(string passwd)
        {
            this.PwdKey = Security.Hash.PasswdToKey(InternalKey, passwd);
            return this.PwdKey;
        }

        public string GetPasswd()
        {
            if (Expire > DateTime.Now)
                return Security.Hash.KeyToPasswd(InternalKey, PwdKey);
            else
                throw new KCoreException(LOG, C.MessageEx.LoginExpired6_0);
        }

        public void SetPwdKey(string pwdkey)
        {
            if (!pwdkey.EndsWith("="))
                throw new KCoreException(LOG, C.MessageEx.InvalidPwdKey7_0);
            else
                this.PwdKey = pwdkey;
        }

        /// <summary>
        /// Create the unique identification
        /// </summary>
        /// <param name="key">Fixed value (example: OUSR.PASSWORD)</param>
        private string SetUid(string id)
        {
            this.Uid = Security.Hash.MD5(User, id);
            return this.Uid;
        }

        public override string ToJson()
        {
            var foo = new Serialize
            {
                user = User,
                pwdkey = PwdKey,
                uid = Uid,
                expire = Expire,
                instance = Instance,
                host = Host,
                userid = UserId,
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(foo);
        }

        class Serialize
        {
            public string user;
            public string pwdkey;
            public string uid;
            public DateTime expire;
            public string instance;
            public string host;
            public int userid;

        }
    }
}
