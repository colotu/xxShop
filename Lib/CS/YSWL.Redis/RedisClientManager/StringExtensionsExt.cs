using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.RedisClientManager
{
    /// <summary>
    /// string扩展
    /// </summary>
    public static class StringExtensionsExt
    {
        public static string BaseConvert(this string source, int from, int to)
        {
            int num4;
            string str = "";
            int length = source.Length;
            int[] numArray = new int[length];
            for (int i = 0; i < length; i++)
            {
                numArray[i] = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(source[i]);
            }
            do
            {
                int num3 = 0;
                num4 = 0;
                for (int j = 0; j < length; j++)
                {
                    num3 = (num3 * from) + numArray[j];
                    if (num3 >= to)
                    {
                        numArray[num4++] = num3 / to;
                        num3 = num3 % to;
                    }
                    else if (num4 > 0)
                    {
                        numArray[num4++] = 0;
                    }
                }
                length = num4;
                str = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"[num3] + str;
            }
            while (num4 != 0);
            return str;
        }


        public static bool IsEmptyOrNull(this string source)
        {
            return string.IsNullOrEmpty(source);
        }

        public static string EncodeJson(this string value)
        {
            return ("\"" + value.Replace(@"\", @"\\").Replace("\"", "\\\"").Replace("\r", "").Replace("\n", @"\n") + "\"");
        }

        public static string EncodeJsv(this string value)
        {
            return value.ToCsvField();
        }

        public static string EncodeXml(this string value)
        {
            return value.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;");
        }

        private static byte[] FastToUtf8Bytes(string strVal)
        {
            byte[] buffer = new byte[strVal.Length];
            for (int i = 0; i < strVal.Length; i++)
            {
                buffer[i] = (byte)strVal[i];
            }
            return buffer;
        }

        public static string FromUtf8Bytes(this byte[] bytes)
        {
            if (bytes != null)
            {
                return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            }
            return null;
        }

        public static string HexEscape(this string text, params char[] anyCharOf)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            if ((anyCharOf == null) || (anyCharOf.Length == 0))
            {
                return text;
            }
            HashSet<char> set = new HashSet<char>(anyCharOf);
            StringBuilder builder = new StringBuilder();
            int length = text.Length;
            for (int i = 0; i < length; i++)
            {
                char item = text[i];
                if (set.Contains(item))
                {
                    builder.Append('%' + ((int)item).ToString("x"));
                }
                else
                {
                    builder.Append(item);
                }
            }
            return builder.ToString();
        }

        public static string HexUnescape(this string text, params char[] anyCharOf)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            if ((anyCharOf == null) || (anyCharOf.Length == 0))
            {
                return text;
            }
            StringBuilder builder = new StringBuilder();
            int length = text.Length;
            for (int i = 0; i < length; i++)
            {
                string str = text.Substring(i, 1);
                if (str == "%")
                {
                    int num3 = Convert.ToInt32(text.Substring(i + 1, 2), 0x10);
                    builder.Append((char)num3);
                    i += 2;
                }
                else
                {
                    builder.Append(str);
                }
            }
            return builder.ToString();
        }

        public static T To<T>(this string value)
        {
            return TypeSerializer.DeserializeFromString<T>(value);
        }

        public static object To(this string value, Type type)
        {
            return TypeSerializer.DeserializeFromString(value, type);
        }

        public static T To<T>(this string value, T defaultValue)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return TypeSerializer.DeserializeFromString<T>(value);
            }
            return defaultValue;
        }

        public static T ToOrDefaultValue<T>(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return TypeSerializer.DeserializeFromString<T>(value);
            }
            return default(T);
        }

        public static string ToRot13(this string value)
        {
            char[] chArray = value.ToCharArray();
            for (int i = 0; i < chArray.Length; i++)
            {
                int num2 = chArray[i];
                if ((num2 >= 0x61) && (num2 <= 0x7a))
                {
                    num2 += (num2 > 0x6d) ? -13 : 13;
                }
                else if ((num2 >= 0x41) && (num2 <= 90))
                {
                    num2 += (num2 > 0x4d) ? -13 : 13;
                }
                chArray[i] = (char)num2;
            }
            return new string(chArray);
        }

        public static byte[] ToUtf8Bytes(this double doubleVal)
        {
            return FastToUtf8Bytes(doubleVal.ToString());
        }

        public static byte[] ToUtf8Bytes(this int intVal)
        {
            return FastToUtf8Bytes(intVal.ToString());
        }

        public static byte[] ToUtf8Bytes(this long longVal)
        {
            return FastToUtf8Bytes(longVal.ToString());
        }

        public static byte[] ToUtf8Bytes(this string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        public static string UrlDecode(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            StringBuilder builder = new StringBuilder();
            int length = text.Length;
            for (int i = 0; i < length; i++)
            {
                string str = text.Substring(i, 1);
                if (str == "+")
                {
                    builder.Append(" ");
                }
                else if (str == "%")
                {
                    int num3 = Convert.ToInt32(text.Substring(i + 1, 2), 0x10);
                    builder.Append((char)num3);
                    i += 2;
                }
                else
                {
                    builder.Append(str);
                }
            }
            return builder.ToString();
        }

        public static string UrlEncode(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            StringBuilder builder = new StringBuilder();
            int length = text.Length;
            for (int i = 0; i < length; i++)
            {
                string str = text.Substring(i, 1);
                int num3 = text[i];
                if ((((num3 >= 0x41) && (num3 <= 90)) || ((num3 >= 0x61) && (num3 <= 0x7a))) || (((num3 >= 0x30) && (num3 <= 0x39)) || ((num3 == 0x2d) || (num3 == 0x2e))))
                {
                    builder.Append(str);
                }
                else
                {
                    builder.Append('%' + num3.ToString("x"));
                }
            }
            return builder.ToString();
        }

        public static string UrlFormat(this string url, params string[] urlComponents)
        {
            string[] strArray = new string[urlComponents.Length];
            for (int i = 0; i < urlComponents.Length; i++)
            {
                strArray[i] = urlComponents[i].UrlEncode();
            }
            return string.Format(url, (object[])strArray);
        }

        public static string WithTrailingSlash(this string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }
            if (path[path.Length - 1] != '/')
            {
                return (path + "/");
            }
            return path;
        }
    }
}
