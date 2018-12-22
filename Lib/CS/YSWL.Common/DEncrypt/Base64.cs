/**
* Base64.cs
*
* 功 能： Base64可逆操作类
* 类 名： Base64
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/01/10 19:32:16  Ben    初版
*
* Copyright (c) 2014 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/


using System.Text;
using System;
namespace YSWL.Common.DEncrypt
{
    /// <summary>
    /// Base64可逆操作类
    /// </summary>
    public static class Base64
    {
        public static string Encode(string strEncode)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(strEncode);
            return Convert.ToBase64String(bytes);
        }

        public static string Decode(string strDecode)
        {
            byte[] bytes = Convert.FromBase64String(strDecode);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 从适用于URL的Base64编码字符串转换为普通字符串
        /// </summary>
        public static string Decode4Url(string base64String)
        {
            base64String = Globals.UrlDecode(base64String);
            string temp = base64String.Replace('_', '=').Replace('.', '+').Replace('-', '/');
            return Encoding.UTF8.GetString(Convert.FromBase64String(temp));
        }

        /// <summary>
        /// 从普通字符串转换为适用于URL的Base64编码字符串
        /// </summary>
        public static string Encode4Url(string normalString)
        {
            return
                Globals.UrlEncode(
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(normalString))
                        .Replace('+', '.')
                        .Replace('/', '-')
                        .Replace('=', '_'));
        }
    }
}
