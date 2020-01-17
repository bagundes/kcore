using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace KCore
{
    [JsonObject(MemberSerialization.OptIn)]
    public partial class Dynamic : Base.BaseModel_v1
    {
        [JsonProperty]
        public readonly dynamic Value;

        [JsonProperty]
        /// <summary>
        /// Information about the value
        /// </summary>
        public string Text { get; protected set; }

        [JsonProperty]
        /// <summary>
        /// This flag it's only for you control (Example to define a special or defaul value).
        /// </summary>
        public dynamic Flag { get; set; }

        private int _lenght;
        
        /// <summary>
        /// Inform type o value. It's possible to force the type using the FormceType method
        /// </summary>
        public TypeCode Type => System.Type.GetTypeCode(Value.GetType());

        #region Constructions
        public Dynamic(dynamic obj)
        {
            this.Value = obj;
        }

        public Dynamic(byte[] obj)
        {
            this.Value = obj;
        }

        public Dynamic(dynamic obj, string text)
        {
            this.Value = obj;
            this.Text = text;
        }
        #endregion

        #region Implicit
        // input
        public static implicit operator Dynamic(string v) => new Dynamic(v);
        public static implicit operator Dynamic(int v) => new Dynamic(v);
        public static implicit operator Dynamic(char v) => new Dynamic(v);
        public static implicit operator Dynamic(double v) => new Dynamic(v);
        public static implicit operator Dynamic(DateTime v) => new Dynamic(v);
        public static implicit operator Dynamic(decimal v) => new Dynamic(v);
        // output
        public static implicit operator string(Dynamic v) => (string)v.Value;
        public static implicit operator int(Dynamic v) => v.ToInt();
        public static implicit operator DateTime(Dynamic v) => v.ToDateTime();
        public static implicit operator byte[](Dynamic v) => new Dynamic(v);
        #endregion

        #region Explicit
        /// <summary>
        /// Try to convert the value in many format to find the original type.
        /// </summary>
        /// <returns></returns>
        public TypeCode ForceType()
        {
            try
            {
                // Trying to convert the value to DateTime
                var foo = DateTime.ParseExact(Value.ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture).GetType();
                return TypeCode.DateTime;
            }
            catch
            {
                return Type;
            }
        }

        #endregion

        #region Transform with masks
        /// <summary>
        /// Return the match value
        /// </summary>
        /// <param name="pattern">Example: </param>
        /// <returns></returns>
        public Dynamic RegexMatch(string pattern)
        {
            return Regex.Match(ToString(), pattern).Value;
        }

        /// <summary>
        /// Valid value is e-mail value.
        /// </summary>
        /// <param name="mail_alt">If e-mail invalid the method return email alt or create a exception</param>
        /// <returns>e-mail valid</returns>
        public string ToEmail(string mail_alt = null)
        {
            if (Dynamic.IsEmail(Value.ToString()))
                return ToString();
            else
            {
                if (!string.IsNullOrEmpty(mail_alt))
                    return mail_alt;
                else
                    throw new KCoreException(this, C.MessageEx.InvalidValueToFormat4_2, Value.ToString(), "e-mail");
            }
        }

        public Dynamic OnlyNumbers(int def = 0)
        {
            if (IsEmpty())
                return new Dynamic(def);

            var val = RegexMatch(@"\d+").Value;// Regex.Match(ToString(), @"\d+").Value;

            if (String.IsNullOrEmpty(val))
                return new Dynamic(def);
            else
                return new Dynamic(val);

        }

        /// <summary>
        /// Replace or remove the information using regex. Exemple:
        /// Only char and number: "[^0-9a-zA-Z]+" to $
        /// eXAMPLE: !2eF6^^5 = !$$$$^^$
        /// </summary>
        /// <param name="regex"></param>
        /// <returns></returns>
        public string RegexReplace(string pattern, string to = null)
        {
            return Regex.Replace(Value.ToString(), pattern, to ?? "");
        }

        public string StringFormat(params object[] values)
        {
            if (values.Length > 0)
                return String.Format(Value.ToString(), values);
            else
                return Value.ToString();
        }
        #endregion

        #region Validations
        /// <summary>
        /// The value is null or empty
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty() => Value == null
            || String.IsNullOrEmpty(Value.ToString())
            || String.IsNullOrWhiteSpace(Value.ToString());

        public bool HasValue() => !IsEmpty();
        public int Length()
        {
            if (_lenght == 0)
            {
                if (Value.GetType() == typeof(Array))
                    _lenght = ((Array)Value).Length;
                else
                    _lenght = Value.ToString().Length;
            }

            return _lenght;
        }

        /// <summary>
        /// Return the match value
        /// </summary>
        /// <param name="pattern">See options in kcore.C.RegexMask</param>
        /// <returns></returns>
        public bool IsValid(string pattern)
        {
            Regex regex = new Regex(pattern);
            Match match = regex.Match(Value.ToString());
            return match.Success;
        }

        /// <summary>
        /// checks whether the value is a number.
        /// </summary>
        /// <returns></returns>
        public bool IsNumber()
        {
            try
            {
                var foo = int.Parse(Value.ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Transform
        public override string ToString()
        {
            if (IsEmpty())
                return String.Empty;
            else
                return Value.ToString();
        }

        public double ToDouble()
        {
            return (double)Value;
        }

        #endregion

        #region Format
        public string ToDateString(string format = null)
        {
            format = format ?? "dd.MM.yyyy";
            return ((DateTime)Value).ToString(format);
        }
        public string ToNumberString(string format = null)
        {
            var foo = ToInt();
            return foo.ToString(format ?? "0");
        }

        /// <summary>
        /// Convert value to price
        /// </summary>
        /// <param name="ci">CultureInfo</param>
        /// <param name="symbol">Symbol</param>
        /// <param name="symbolend">Put the symbol in the end?</param>
        /// <returns></returns>
        public string ToPriceString(KCore.C.Language lng, string symbol = "€", bool symbolend = false)
        {
            var ci = CultureInfo.GetCultureInfo(lng.ToString().Replace('_', '-'));

            var price = ToDouble();
            if (symbolend)
                return price.ToString("#,##0.00" + symbol, ci);
            else
                return price.ToString(symbol + " #,##0.00", ci);
        }

        public string[] Split(params char[] separators)
        {
            return Value.ToString().Split(separators);
        }

        #region Enums
        public T GetEnumByName<T>() where T : Enum
        {
            return Dynamic.GetEnumByName<T>(Value.ToString());
        }


        public T GetEnumByIndex<T>() where T : Enum
        {
            return (T)Enum.ToObject(typeof(T), ToInt());
        }

        public string GetEnumDescription<T>() where T : Enum
        {
            var enum1 = GetEnumByIndex<T>();
            return Dynamic.GetEnumDescription(enum1);
        }
        #endregion


        /// <summary>
        /// Check the value is true
        /// </summary>
        /// <param name="totrue"></param>
        /// <returns></returns>
        public bool ToBool(string totrue = null)
        {
            var foo = Value.ToString().ToUpper();
            if (foo != null && totrue != null && foo == totrue.ToUpper())
                return true;

            switch (foo)
            {
                case "1":
                case "Y":
                case "YES":
                case "S":
                case "SI":
                case "SIM":
                case "T":
                case "TRUE":
                    return true;
                default:
                    return false;

            }
        }

        public T ToEnum<T>() where T : System.Enum
        {
            if (((int)Type) > 6 && ((int)Type) < 13)
                return (T)Enum.ToObject(typeof(T), ToInt());
            else
                return (T)Enum.Parse(typeof(T), ToString(), true);
        }
        public char ToBoolChar(char yes = 'Y', char no = 'N')
        {
            return ToBool(yes.ToString()) ? yes : no;
        }

        public int? ToNInt()
        {
            try
            {
                return int.Parse(Value.ToString());
            }
            catch
            {
                return null;
            }
        }

        public int ToInt(int ifnull = 0)
        {
            try
            {
                int val;
                if (!int.TryParse(Value.ToString(), out val))
                    val = OnlyNumbers(ifnull);

                return val;
            }
            catch
            {
                return ifnull;
            }
        }

        public DirectoryInfo ToDirectory()
        {
            switch (Value.ToString().ToUpper())
            {
                case "%TEMP%":
                    throw new NotImplementedException();
                case "%USER%":
                    throw new NotImplementedException();
                case "%APP%":
                    throw new NotImplementedException();
                default:
                    try
                    {
                        System.IO.Directory.CreateDirectory(Value.ToString());
                        return new DirectoryInfo(Value.ToString());
                    }
                    catch
                    {
                        throw new KCoreException(this, C.MessageEx.DirectoryInvalid5_1, Value.ToString());
                    }
            }
        }

        public byte[] ToByte()
        {
            int NumberChars = Value.ToString().Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(Value.ToString().Substring(i, 2), 16);
            return bytes;
        }

        public string ToMD5Hash()
        {
            if (IsEmpty())
                return null;
            else
                return Security.Hash.MD5(ToString());
        }

        public decimal ToNumber()
        {
            try
            {
                return Decimal.Parse(Value.ToString());
            }
            catch (Exception)
            {
                throw new KCoreException(this, C.MessageEx.InvalidValueToFormat4_2, Value.ToString(), "decimal");
            }
        }

        public DateTime ToDateTime(string format = "dd/MM/yyyy HH:mm:ss")
        {
            if (Type == TypeCode.DateTime)
                return Value;
            else
                return DateTime.ParseExact(Value.ToString(), format, System.Globalization.CultureInfo.InvariantCulture);
        }
        public Model.Select_v1 Select(bool @default)
        {
            return new Model.Select_v1(Value, Text, @default);
        }
        #endregion

    }

    public partial class Dynamic
    {
        public static Dynamic Empty => new Dynamic(null);

        public static Dynamic From(dynamic value) => new Dynamic(value);

        /// <summary>
        /// Transform the Dynaminc object to CSV file
        /// </summary>
        /// <param name="delimeted"></param>
        /// <param name="escape"></param>
        /// <param name="values">Flags: STRING (force string)</param>
        /// <returns></returns>
        public static string CSV(string delimeted, string escape, Dynamic[] values)
        {
            var line = new List<string>();

            foreach (var value in values)
            {
                var forceString = value.Flag.Contains("STRING");

                if (forceString || !value.IsNumber())
                {
                    var val = value.ToString();

                    if (val.Contains(escape))
                        val = val.Replace(escape, $"{escape}{escape}");

                    line.Add($"{escape}{val}{escape}");
                }
                else
                    line.Add(value.ToNumber().ToString());
            }

            return String.Join(delimeted, line.ToArray());
        }

        #region Enums
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        /// <summary>
        /// Get Enum by name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetEnumByName<T>(string name) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), name);
        }
        #endregion

        public static bool IsEmail(string val)
        {
            var regex = new Regex(C.RegexMask.Email);
            return regex.Match(val).Success;
        }
    }
}
