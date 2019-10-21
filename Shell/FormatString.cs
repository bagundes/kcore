using System;
using System.Text.RegularExpressions;

namespace KCore.Shell
{
    public static class FormatString
    {
        /// <summary>
        /// Create the Initials name.
        /// Examples: 
        /// Dana Willy: return DW
        /// Maria: return Ma
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string Initials(string name)
        {
            var names = name.Split(' ');

            if (names.Length > 1)
            {
                return names[0].ToString().ToUpper() + names[names.Length - 1].ToString().ToUpper();
            }
            else
                return name[0].ToString().ToUpper() + name[1].ToString().ToLower();
        }

        /// <summary>
        /// Convert class name to javascript variable name, example:
        /// class HelloWorld = hello_world
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string JavaScriptFormat(object obj)
        {
            var name = obj.GetType() == typeof(string) ? obj.ToString() : obj.GetType().Name;
            var res = String.Empty;

            for (int i = 0; i < name.Length; i++)
            {
                var c = name[i].ToString().ToLower();

                if (i == 0)
                    res = c;
                else if (char.IsUpper(name[i]))
                    res += $"_{c}";
                else
                    res += c;
            }

            return res;

        }

        /// <summary>
        /// Find and return the hexadecimal value in the text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Hexadecimal(string text)
        {
            var start = text.IndexOf("0x");
            if (start < 0) return null;

            var hex = String.Empty;
            var regex = new Regex("[0-9ABCDEFabcdefXx]");

            for (int i = start; i < text.Length; i++)
            {
                if (regex.IsMatch(text[i].ToString()))
                    hex += text[i];
                else
                    break;
            }

            return hex;

        }

        //public static string RemoveDuplicateChars(string key, char character)
        //{
        //    string res = String.Empty;
        //    bool ischaracter = false;
        //    for(int i = 0; i < key.Length; i++)
        //    {
        //        if(key[i] == character)
        //    }

        //    foreach (char value in key)
        //    {
        //        // See if character is in the table.
        //        if (table.IndexOf(value) == -1)
        //        {
        //            // Append to the table and the result.
        //            table += value;
        //            result += value;
        //        }
        //    }
        //    return result;
        //}
    }
}
