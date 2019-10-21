using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace KCore.Security
{
    public static class Hash
    {
        private static string LOG => typeof(Hash).Name;
        private const string SIGN = "@";

        /// <summary>
        /// Create Unique Identify to values.
        /// </summary>
        /// <param name="values">Values</param>
        /// <returns>Unique Id</returns>
        public static int CreateIdNumber(params object[] values)
        {
            var strInfo = String.Join("", values);
            var id = 0;

            for (int i = 0; i < strInfo.Length; i++)
                id += Convert.ToInt32(strInfo[i]);

            return id;
        }

        #region Crypt
        /// <summary>
        /// Transform value in Token using the masterkey default.
        /// </summary>
        /// <param name="value">Convert value to token</param>
        /// <returns>Token encoded url string</returns>
        public static string ValueToToken(string value)
        {
            var token = Encrypt(value, R.Security.MasterKey);
            return HttpUtility.UrlEncode(token);
        }

        /// <summary>
        /// Restore the token to value.
        /// </summary>
        /// <param name="token">Convert token to value</param>
        /// <returns>Value</returns>
        public static string TokenToValue(string token)
        {
            if (!token.StartsWith(SIGN) || !token.EndsWith(SIGN))
                token = HttpUtility.UrlDecode(token);
            return Decrypt(token, R.Security.MasterKey);
        }
        #endregion

        #region Simple crypto
        /// <summary>
        /// Encrypt the value using the key.
        /// </summary>
        /// <param name="value">Value to encrypt</param>
        /// <param name="key"><Key to encrypt</param>
        /// <returns>The value encrypted will be url enconde</returns>
        public static string Encrypt(string value, string key)
        {
            return $"{SIGN}{Encrypt1(value, key)}{SIGN}";
        }

        /// <summary>
        /// Decrypt the value using the key.
        /// </summary>
        /// <param name="value">Value to encrypt</param>
        /// <param name="key"><Key to encrypt</param>
        /// <returns></returns>
        public static string Decrypt(string value, string key)
        {
            if (value.Contains(System.Environment.NewLine))
                value = value.Replace(System.Environment.NewLine, null);

            if (!value.StartsWith(SIGN) || !value.EndsWith(SIGN))
                throw new System.FormatException($"The value needs to starting and ending with \"{SIGN}\" sign");
            else
                value = value.Substring(SIGN.Length, value.Length - SIGN.Length - 1);

            return Decrypt1(value, key);            
        }
        #endregion

        #region One way
        /// <summary>
        /// Create unique hash with 128 bits (32 characteres)
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string UniqueHash(params object[] values)
        {
            return HttpUtility.UrlEncode(MD5(values, R.Security.MasterKey));
        }

        public static string UniqueToken(params object[] values)
        {
            return HttpUtility.UrlEncode(SHA512(values, R.Security.MasterKey));
        }

        /// <summary>
        /// Create number id to values
        /// </summary>
        /// <param name="values">If this values are empty, the method will be return random number</param>
        /// <returns></returns>
        public static int Id(params dynamic[] values)
        {
            var value = values.Length > 0 ? String.Join("", values) : MD5(DateTime.Now);
            var res = 0;
            for (int i = 0; i < value.Length; i++)
                res += value[i];

            return res;
        }
        #endregion

        #region Hashs
        /// <summary>
        /// Hash with 32-bits (8 characters)
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string Crc32(params object[] values)
        {
            throw new NotImplementedException();
            //var crc32 = new Crc32();
            //var hash = String.Empty;

            //    foreach (byte b in crc32.ComputeHash(String.Join("", values))) hash += b.ToString("x2").ToLower();

            //Console.WriteLine("CRC-32 is {0}", hash);

        }

        /// <summary>
        /// Hash with 512-bits (128 characters)
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string SHA512(params object[] values)
        {
            var rawData = String.Join("", values);
            var bytes = System.Text.Encoding.UTF8.GetBytes(rawData);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);

                // Convert to text
                // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
                var hashedInputStringBuilder = new System.Text.StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
            }
        }

        /// <summary>
        /// Hash with 256-bits (64 characteres)
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string Sha256(params object[] values)
        {
            var rawData = String.Join("", values);
            // Create a SHA256   
            using (var sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Hash with 128-bits (32 characteres)
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string MD5(params object[] values)
        {
            var value = String.Join("", values);
            using (var md5Hash = System.Security.Cryptography.MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(value));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString().ToUpper();
            }
        }
        #endregion

        #region Encrypton Expert
        /// <summary>
        /// Encrypt the input value.
        /// </summary>
        /// <param name="input">Value to encrypt</param>
        /// <param name="key">Key to decrypt</param>
        /// <returns>The value will return with start and end at sign</returns>
        private static string Encrypt1(object input, string key)
        {
            if (input == null)
                return null;

            key = key ?? R.Security.MasterKey;
            var byte24 = MD5(key).Substring(0, 24);
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input.ToString());
            var tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(byte24);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            var cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// Decrypto the string.
        /// </summary>
        /// <param name="input">Encrypt value. The value needs start and end with sign</param>
        /// <param name="key">Key to decrypt</param>
        /// <returns></returns>
        private static string Decrypt1(string input, string key)
        {
            key = key ?? R.Security.MasterKey;
            var byte24 = MD5(key).Substring(0, 24);
            byte[] inputArray = Convert.FromBase64String(input.ToString());
            var tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(byte24);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            var cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        #endregion
    }
}
