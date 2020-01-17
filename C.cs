namespace KCore
{
    public static class C
    {

        public enum StatusType
        {
            Success,
            Error,
            Warning,
            Info,
            Fatal
        }


        public enum MessageEx
        {
            FatalError1_1 = 1,
            KeyIsNotCorrectFormat3_0 = 3,
            InvalidValueToFormat4_2 = 4,
            DirectoryInvalid5_1 = 5,
            LoginExpired6_0 = 6,
            InvalidPwdKey7_0 = 7,
            ItNotPossibleDecrypt_8_1 = 8,
            E_InvalidKey9 = 9,
            StoredCacheError10_1 = 10,
            InvalidCredential11_0 = 11,
            GlobalConfigError12_1 = 12,
        }

        /// <summary>
        /// Type of databases
        /// </summary>
        public class Database
        {
            public enum DBaseType
            {
                MSQL,
                Hana,
                None,
            }

            public enum ColumnType
            {
                Text,
                LongText,
                Number,
                Decimal,
                Date,
                DateTime,
                Char,
                None,
                Int,
            }

            public enum TypeID
            {
                Number = 'N',
                Date = 'D',
                Alpha = 'A',
                Price = 'P',
                None = '0',
            }

            public enum Actions
            {
                Add = Added,
                Added = 1,
                Update = Updated,
                Updated = 2,
                Delete = Deleted,
                Deleted = 3,
                Error = -1,
            }

            public enum ClientType
            {
                SQLClient,
                SAPClient,
                HanaClient,
            }

        }

        public enum Language
        {
            en_IE = 1,
            en_UK = 2,
            en_US = 3,
        }

        public static class RegexMask
        {
            /// <summary>
            /// Only letters and underscore
            /// </summary>
            public static string OnlyLettersAndUnderscore => "[^a-zA-Z_]+";
            /// <summary>
            /// Only letters and numbers
            /// </summary>
            public static string OnlyLetterAndNumbers => "[^0-9a-zA-Z]+";
            /// <summary>
            /// Only decimal number
            /// </summary>
            public static string OnlyDecimal => @"^[\d,.?!]+$";
            /// <summary>
            /// Only e-mail
            /// </summary>
            public static string Email => @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
        }
    }
}
