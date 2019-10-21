using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace KCore.Model
{
    public class Select : Base.BaseModel
    {
        public dynamic value;     
        public string text;        
        public bool flag;

        [JsonIgnoreAttribute]
        public bool encrypt;

        public Select() { }

        public Select(object value, bool encrypt = false)
        {
            this.value = value;
            this.text = value.ToString();
            this.encrypt = encrypt;

            if (encrypt)
                EncryptValue();
        }

        /// <summary>
        /// Select model
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="text">Text</param>
        /// <param name="flag">Flag for any detail</param>
        /// <param name="encrypt">Encrypt the value?</param>
        public Select(object value, string text, bool flag = false, bool encrypt = false)
        {
            this.value = value;
            this.text = text;
            this.flag = flag;
            this.encrypt = encrypt;

            if (encrypt)
                EncryptValue();
        }

        public void EncryptValue()
        {
            KCore.Security.Hash.Encrypt(value.ToString(), null);
            this.encrypt = true;
        }

        public Select_v2 ToV2()
        {
            return new Select_v2(value, text, encrypt, flag);
        }
    }
}
