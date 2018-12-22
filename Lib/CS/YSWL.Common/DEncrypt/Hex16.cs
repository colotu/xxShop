/**
* Hex16.cs
*
* 功 能： 16进制可逆操作类
* 类 名： Hex16
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/4/16 17:43:35  Ben    初版
*
* Copyright (c) 2013 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/


namespace YSWL.Common.DEncrypt
{
    /// <summary>
    /// 16进制可逆操作类
    /// </summary>
    public static class Hex16
    {
        /// <summary>
        /// 作用：将字符串内容转化为16进制数据编码，其逆过程是Decode
        /// 参数说明：
        /// strEncode 需要转化的原始字符串
        /// 转换的过程是直接把字符转换成Unicode字符,比如数字"3"-->0033,汉字"我"-->U+6211
        /// 函数decode的过程是encode的逆过程.
        /// </summary>
        public static string Encode(string strEncode)
        {
            string strReturn = "";//  存储转换后的编码
            try
            {
                foreach (short shortx in strEncode.ToCharArray())
                {
                    strReturn += shortx.ToString("X4");
                }
            }
            catch { }
            return strReturn;
        }

        /// <summary>
        /// 作用：将16进制数据编码转化为字符串，是Encode的逆过程
        /// </summary>
        public static string Decode(string strDecode)
        {
            string sResult = "";
            try
            {
                for (int i = 0; i < strDecode.Length / 4; i++)
                {
                    sResult += (char)short.Parse(strDecode.Substring(i * 4, 4),
                        global::System.Globalization.NumberStyles.HexNumber);
                }
            }
            catch { }
            return sResult;
        }

        /// <summary>
        /// 将byte[]转换为16进制字符串
        /// </summary>
        /// <param name="bytes">二进制数组</param>
        /// <returns></returns>
        public static string ToString(byte[] bytes)
        {
            string hex = System.BitConverter.ToString(bytes);
            return hex.Replace("-", "");
        }

        /// <summary>
        /// 将16进制字符串转换为byte[]
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(string hex)
        {
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
                bytes[i / 2] = System.Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}
