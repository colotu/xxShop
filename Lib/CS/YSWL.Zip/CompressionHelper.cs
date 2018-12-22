/**
* CompressionHelper.cs
*
* 功 能： 字符串的压缩和解压缩
* 类 名： CompressionHelper
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/12/11 17:56:27  Ben    初版
*
* Copyright (c) 2012 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.IO;
using System.Text;
using YSWL.ZipLib.BZip2;
using YSWL.ZipLib.GZip;
using YSWL.ZipLib.Zip;

namespace YSWL.ZipLib
{
    /// <summary>
    /// 压缩方式。
    /// </summary>
    public enum CompressionType
    {
        /// <summary>
        /// GZip 压缩格式
        /// </summary>
        GZip,
        /// <summary>
        /// BZip2 压缩格式
        /// </summary>
        BZip2,
        /// <summary>
        /// Zip 压缩格式
        /// </summary>
        Zip
    }
    /// <summary>
    /// 使用 SharpZipLib 进行压缩的辅助类，简化对字节数组和字符串进行压缩的操作。
    /// </summary>
    public class CompressionHelper
    {
        /// <summary>
        /// 压缩供应者，默认为 GZip。
        /// </summary>
        public static CompressionType CompressionProvider = CompressionType.GZip;
        /// <summary>
        /// 从原始字节数组生成已压缩的字节数组。
        /// </summary>
        /// <param name="bytesToCompress">原始字节数组。</param>
        /// <returns>返回已压缩的字节数组</returns>
        public static byte[] Compress(byte[] bytesToCompress)
        {
            MemoryStream ms = new MemoryStream();
            Stream s = OutputStream(ms);
            s.Write(bytesToCompress, 0, bytesToCompress.Length);
            s.Close();
            return ms.ToArray();
        }
        /// <summary>
        /// 从原始字符串生成已压缩的字符串。
        /// </summary>
        /// <param name="stringToCompress">原始字符串。</param>
        /// <returns>返回已压缩的字符串。</returns>
        public static string Compress(string stringToCompress)
        {
            byte[] compressedData = CompressToByte(stringToCompress);
            string strOut = Convert.ToBase64String(compressedData);
            return strOut;
        }
        /// <summary>
        /// 从原始字符串生成已压缩的字节数组。
        /// </summary>
        /// <param name="stringToCompress">原始字符串。</param>
        /// <returns>返回已压缩的字节数组。</returns>
        public static byte[] CompressToByte(string stringToCompress)
        {
            byte[] bytData = Encoding.Unicode.GetBytes(stringToCompress);
            return Compress(bytData);
        }
        /// <summary>
        /// 从已压缩的字符串生成原始字符串。
        /// </summary>
        /// <param name="stringToDecompress">已压缩的字符串。</param>
        /// <returns>返回原始字符串。</returns>
        public string DeCompress(string stringToDecompress)
        {
            string outString = string.Empty;
            if (stringToDecompress == null)
            {
                throw new ArgumentNullException("stringToDecompress", "You tried to use an empty string");
            }
            try
            {
                byte[] inArr = Convert.FromBase64String(stringToDecompress.Trim());
                outString = Encoding.Unicode.GetString(DeCompress(inArr));
            }
            catch (NullReferenceException nEx)
            {
                return nEx.Message;
            }
            return outString;
        }
        /// <summary>
        /// 从已压缩的字节数组生成原始字节数组。
        /// </summary>
        /// <param name="bytesToDecompress">已压缩的字节数组。</param>
        /// <returns>返回原始字节数组。</returns>
        public static byte[] DeCompress(byte[] bytesToDecompress)
        {
            byte[] writeData = new byte[4096];
            Stream s2 = InputStream(new MemoryStream(bytesToDecompress));
            MemoryStream outStream = new MemoryStream();
            while (true)
            {
                int size = s2.Read(writeData, 0, writeData.Length);
                if (size > 0)
                {
                    outStream.Write(writeData, 0, size);
                }
                else
                {
                    break;
                }
            }
            s2.Close();
            byte[] outArr = outStream.ToArray();
            outStream.Close();
            return outArr;
        }
        /// <summary>
        /// 从给定的流生成压缩输出流。
        /// </summary>
        /// <param name="inputStream">原始流。</param>
        /// <returns>返回压缩输出流。</returns>
        private static Stream OutputStream(Stream inputStream)
        {
            switch (CompressionProvider)
            {
                case CompressionType.BZip2:
                    return new BZip2OutputStream(inputStream);
                case CompressionType.GZip:
                    return new GZipOutputStream(inputStream);
                case CompressionType.Zip:
                    return new ZipOutputStream(inputStream);
                default:
                    return new GZipOutputStream(inputStream);
            }
        }
        /// <summary>
        /// 从给定的流生成压缩输入流。
        /// </summary>
        /// <param name="inputStream">原始流。</param>
        /// <returns>返回压缩输入流。</returns>
        private static Stream InputStream(Stream inputStream)
        {
            switch (CompressionProvider)
            {
                case CompressionType.BZip2:
                    return new BZip2InputStream(inputStream);
                case CompressionType.GZip:
                    return new GZipInputStream(inputStream);
                case CompressionType.Zip:
                    return new ZipInputStream(inputStream);
                default:
                    return new GZipInputStream(inputStream);
            }
        }
    }

}
