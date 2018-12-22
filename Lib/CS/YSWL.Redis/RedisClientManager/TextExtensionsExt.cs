using ServiceStack.Text;
using ServiceStack.Text.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.RedisClientManager
{
    /// <summary>
    /// 文本操作扩展
    /// </summary>
    public static class TextExtensionsExt
    {
        public static string FromCsvField(this string text)
        {
            if (!string.IsNullOrEmpty(text) && (text[0] == '"'))
            {
                return text.Substring(1, text.Length - 2).Replace("\"\"", "\"");
            }
            return text;
        }

        public static List<string> FromCsvFields(this IEnumerable<string> texts)
        {
            List<string> list = new List<string>();
            foreach (string str in texts)
            {
                list.Add(str.FromCsvField());
            }
            return list;
        }

        public static string[] FromCsvFields(params string[] texts)
        {
            int length = texts.Length;
            string[] strArray = new string[length];
            for (int i = 0; i < length; i++)
            {
                strArray[i] = texts[i].FromCsvField();
            }
            return strArray;
        }

        public static string SerializeToString<T>(this T value)
        {
            return JsonSerializer.SerializeToString<T>(value);
        }

        public static string ToCsvField(this string text)
        {
            if (!string.IsNullOrEmpty(text) && JsWriter.HasAnyEscapeChars(text))
            {
                return ("\"" + text.Replace("\"", "\"\"") + "\"");
            }
            return text;
        }
    }
}
