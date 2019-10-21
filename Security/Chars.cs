using System;
using System.Linq;

namespace KCore.Security
{
    public static class Chars
    {
        private static string LOG => typeof(Chars).Name;

        /// <summary>
        /// Return random string.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="special">Add special chars</param>
        /// <returns></returns>
        public static string RandomChars(int size, bool special = false)
        {
            var chars = System.String.Empty;

            if (special)
                chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!£$%^&*(){}-_+=[]:;@~#";
            else
                chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, size)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }

        /// <summary>
        /// Mixer the values
        /// </summary>
        /// <param name="def">Mixer default or random</param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string StringMixer(bool def, params object[] values)
        {
            var totalSize = 0;
            var biggerString = 0;
            var works = values.Length;
            var result = System.String.Empty;

            foreach (string value in values)
            {
                var foo = value == null ? 0 : value.Length;
                totalSize += foo;
                biggerString = biggerString < foo ? foo : biggerString;
            }


            for (int i = 0; i < biggerString; i++)
            {
                foreach (string value in values)
                {
                    if (value.Length >= (i + 1))
                    {
                        if (def)
                            result = value[i] + result;
                        else
                        {
                            Random random = new Random();
                            var rnd = random.Next(0, biggerString - 1);
                            result = value[rnd] + result;
                        }
                    }

                }
            }

            if (result.Length == totalSize)
                return result;
            else
                throw new KCoreException(LOG, C.MessageEx.FatalError1_1, "The mixer string result lenght wrong");

        }
    }
}
