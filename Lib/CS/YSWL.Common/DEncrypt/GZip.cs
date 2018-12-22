/**
* GZip.cs
*
* 功 能： GZip压缩类
* 类 名： GZip
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/5/10 17:50:45  Ben    初版
*
* Copyright (c) 2013 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.IO;
using System.IO.Compression;

namespace YSWL.Common.DEncrypt
{
    public static class GZip
    {
        #region 方案一 针对JavaScript混合压缩算法
        /// <summary>
        /// GZip解压函数
        /// </summary>
        /// <remarks>针对JavaScript混合压缩算法</remarks>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] JsDecompress(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (GZipStream gZipStream = new GZipStream(new MemoryStream(data), CompressionMode.Decompress))
                {
                    byte[] bytes = new byte[40960];
                    int n;
                    while ((n = gZipStream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        stream.Write(bytes, 0, n);
                    }
                    gZipStream.Close();
                }

                return stream.ToArray();
            }
        }

        /// <summary>
        /// GZip压缩函数
        /// </summary>
        /// <remarks>针对JavaScript混合压缩算法</remarks>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] JsCompress(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (GZipStream gZipStream = new GZipStream(stream, CompressionMode.Compress))
                {
                    gZipStream.Write(data, 0, data.Length);
                    gZipStream.Close();
                }

                return stream.ToArray();
            }
        }

        /// <summary>
        /// Deflate解压函数
        /// JS:var details = eval('(' + utf8to16(zip_depress(base64decode(hidEnCode.value))) + ')')对应的C#压缩方法
        /// </summary>
        /// <remarks>针对JavaScript混合压缩算法</remarks>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public static string DeflateDecompress(string strSource)
        {
            byte[] buffer = Convert.FromBase64String(strSource);

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                ms.Write(buffer, 0, buffer.Length);
                ms.Position = 0;

                using (System.IO.Compression.DeflateStream stream = new System.IO.Compression.DeflateStream(ms, System.IO.Compression.CompressionMode.Decompress))
                {
                    stream.Flush();

                    int nSize = 16 * 1024 + 256;    //假设字符串不会超过16K
                    byte[] decompressBuffer = new byte[nSize];
                    int nSizeIncept = stream.Read(decompressBuffer, 0, nSize);
                    stream.Close();

                    return System.Text.Encoding.UTF8.GetString(decompressBuffer, 0, nSizeIncept);   //转换为普通的字符串
                }
            }
        }

        /// <summary>
        /// Deflate压缩函数
        /// </summary>
        /// <remarks>针对JavaScript混合压缩算法</remarks>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public static string DeflateCompress(string strSource)
        {
            if (string.IsNullOrWhiteSpace(strSource)) return string.Empty;

            if (strSource.Length > 8 * 1024)
                throw new System.ArgumentException("YSWL.Common.DEncrypt.GZip.DeflateCompress 字符串长度超过上限");
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(strSource);

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                using (System.IO.Compression.DeflateStream stream = new System.IO.Compression.DeflateStream(ms, System.IO.Compression.CompressionMode.Compress, true))
                {
                    stream.Write(buffer, 0, buffer.Length);
                    stream.Close();
                }

                byte[] compressedData = ms.ToArray();
                ms.Close();

                return Convert.ToBase64String(compressedData);      //将压缩后的byte[]转换为Base64String
            }
        }
        #endregion

        #region 方案二 通用算法
        /// <summary>
        /// 压缩字符串
        /// </summary>
        /// <param name="context">要压缩的内容</param>
        /// <returns>使用base64的方式输出压缩后的内容</returns>
        public static string Compress(string context)
        {
            return Convert.ToBase64String(Compress(System.Text.Encoding.UTF8.GetBytes(context)));
        }
        /// <summary>
        /// 压缩二进制
        /// </summary>
        /// <param name="context">要压缩的内容</param>
        /// <returns>输出压缩后的内容</returns>
        public static byte[] Compress(byte[] context)
        {
            MemoryStream ms = new MemoryStream();
            GZipStream gzs = new GZipStream(ms, CompressionMode.Compress, true);
            gzs.Write(context, 0, context.Length);
            gzs.Close();
            return ms.ToArray();
        }
        /// <summary>
        /// 解压一个base64格式的字符串
        /// </summary>
        /// <param name="context">内容</param>
        /// <returns>返回解压后的内容</returns>
        public static string Decompress(string context)
        {
            return System.Text.Encoding.UTF8.GetString(Decompress(Convert.FromBase64String(context)));
        }
        /// <summary>
        /// 解压二进制
        /// </summary>
        /// <param name="context">内容</param>
        /// <returns>返回解压后的内容</returns>
        public static byte[] Decompress(byte[] context)
        {
            byte[] result = null;
            if (context != null)
            {
                byte[] bytes = new byte[1024];
                MemoryStream ms = new MemoryStream();
                GZipStream gzs = new GZipStream(new MemoryStream(context), CompressionMode.Decompress, true);
                int i; while ((i = gzs.Read(bytes, 0, bytes.Length)) != 0)
                {
                    ms.Write(bytes, 0, i);
                }
                result = ms.ToArray();
                gzs.Close();
                ms.Close();
                ms.Dispose();
                gzs.Dispose();
            } return result;
        }
        #endregion

    }
}
