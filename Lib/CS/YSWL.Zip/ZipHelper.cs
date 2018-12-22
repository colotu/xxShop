/**
* ZipHelper.cs
*
* 功 能： 文件的压缩和解压缩
* 类 名： ZipHelper
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/12/11 17:47:32  Ben    初版
*
* Copyright (c) 2012 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.IO;
using YSWL.ZipLib.Checksums;
using YSWL.ZipLib.Zip;

namespace YSWL.ZipLib
{
    /// <summary>
    /// 文件的压缩和解压缩 支持Zip, GZip, BZip2 和Tar格式
    /// </summary>
    public static class ZipHelper
    {
        /// <summary>
        /// 压缩单个文件
        /// </summary>
        /// <param name="fileToZip">要压缩的文件</param>
        /// <param name="zipedFile">压缩后的文件</param>
        /// <param name="compressionLevel">压缩等级</param>
        /// <param name="blockSize">每次写入大小</param>
        public static void ZipFile(string fileToZip, string zipedFile, int compressionLevel, int blockSize)
        {
            //如果文件没有找到，则报错
            if (!System.IO.File.Exists(fileToZip))
            {
                throw new System.IO.FileNotFoundException("指定要压缩的文件: " + fileToZip + " 不存在!");
            }

            using (System.IO.FileStream ZipFile = System.IO.File.Create(zipedFile))
            {
                using (ZipOutputStream ZipStream = new ZipOutputStream(ZipFile))
                {
                    using (System.IO.FileStream StreamToZip = new System.IO.FileStream(fileToZip, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        string fileName = fileToZip.Substring(fileToZip.LastIndexOf("\\") + 1);

                        ZipEntry ZipEntry = new ZipEntry(fileName);

                        ZipStream.PutNextEntry(ZipEntry);

                        ZipStream.SetLevel(compressionLevel);

                        byte[] buffer = new byte[blockSize];

                        int sizeRead = 0;

                        try
                        {
                            do
                            {
                                sizeRead = StreamToZip.Read(buffer, 0, buffer.Length);
                                ZipStream.Write(buffer, 0, sizeRead);
                            }
                            while (sizeRead > 0);
                        }
                        catch (System.Exception ex)
                        {
                            throw ex;
                        }

                        StreamToZip.Close();
                    }

                    ZipStream.Finish();
                    ZipStream.Close();
                }

                ZipFile.Close();
            }
        }

        /// <summary>
        /// 压缩单个文件
        /// </summary>
        /// <param name="fileToZip">要进行压缩的文件名</param>
        /// <param name="zipedFile">压缩后生成的压缩文件名</param>
        public static void ZipFile(string fileToZip, string zipedFile)
        {
            //如果文件没有找到，则报错
            if (!File.Exists(fileToZip))
            {
                throw new System.IO.FileNotFoundException("指定要压缩的文件: " + fileToZip + " 不存在!");
            }

            using (FileStream fs = File.OpenRead(fileToZip))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();

                using (FileStream ZipFile = File.Create(zipedFile))
                {
                    using (ZipOutputStream ZipStream = new ZipOutputStream(ZipFile))
                    {
                        string fileName = fileToZip.Substring(fileToZip.LastIndexOf("\\") + 1);
                        ZipEntry ZipEntry = new ZipEntry(fileName);
                        ZipStream.PutNextEntry(ZipEntry);
                        ZipStream.SetLevel(5);

                        ZipStream.Write(buffer, 0, buffer.Length);
                        ZipStream.Finish();
                        ZipStream.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 压缩多层目录
        /// </summary>
        /// <param name="strDirectory">The directory.</param>
        /// <param name="zipedFile">The ziped file.</param>
        public static void ZipFileDirectory(string strDirectory, string zipedFile)
        {
            using (System.IO.FileStream ZipFile = System.IO.File.Create(zipedFile))
            {
                using (ZipOutputStream s = new ZipOutputStream(ZipFile))
                {
                    ZipSetp(strDirectory, s, "");
                }
            }
        }

        /// <summary>
        /// 检测压缩文件中的目录
        /// </summary>
        /// <param name="directories">The directory.</param>
        /// <param name="zipedFile">The ziped file.</param>
        /// <param name="isExactlyMatch">是否严格匹配</param>
        public static bool ExistsZipFileDirectory(string[] directories, string zipedFile, bool isExactlyMatch = true)
        {
            List<bool> flags = new List<bool>();
            //如果文件没有找到，则报错
            if (!File.Exists(zipedFile))
                throw new System.IO.FileNotFoundException("检测压缩文件: " + zipedFile + " 不存在!");

            using (ZipFile zipFile = new ZipFile(zipedFile))
            {
                return zipFile.FindEntry(directories[0], false) > -1;
                //foreach (ZipEntry zipEntry in zipFile)
                //{
                //    if (zipEntry.IsDirectory)
                //    {
                //        foreach (string directory in directories)
                //        {
                //            flags.Add(zipEntry.Name == directory);
                //        }
                //    }
                //}
            }

            if (isExactlyMatch)
            {
                List<bool> tmpFlag = flags.FindAll(xx => xx);
                return tmpFlag != null && tmpFlag.Count == directories.Length;
            }
            return flags.Count == directories.Length;
        }

        /// <summary>
        /// 递归遍历目录
        /// </summary>
        /// <param name="strDirectory">The directory.</param>
        /// <param name="s">The ZipOutputStream Object.</param>
        /// <param name="parentPath">The parent path.</param>
        private static void ZipSetp(string strDirectory, ZipOutputStream s, string parentPath)
        {
            if (strDirectory[strDirectory.Length - 1] != Path.DirectorySeparatorChar)
            {
                strDirectory += Path.DirectorySeparatorChar;
            }
            Crc32 crc = new Crc32();

            string[] filenames = Directory.GetFileSystemEntries(strDirectory);

            foreach (string file in filenames)// 遍历所有的文件和目录
            {

                if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {
                    string pPath = parentPath;
                    pPath += file.Substring(file.LastIndexOf("\\") + 1);
                    pPath += "\\";
                    ZipSetp(file, s, pPath);
                }

                else // 否则直接压缩文件
                {
                    //打开压缩文件
                    using (FileStream fs = File.OpenRead(file))
                    {

                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);

                        string fileName = parentPath + file.Substring(file.LastIndexOf("\\") + 1);
                        ZipEntry entry = new ZipEntry(fileName);

                        entry.DateTime = DateTime.Now;
                        entry.Size = fs.Length;

                        fs.Close();

                        crc.Reset();
                        crc.Update(buffer);

                        entry.Crc = crc.Value;
                        s.PutNextEntry(entry);

                        s.Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }

        /// <summary>
        /// 解压缩一个 zip 文件。
        /// </summary>
        /// <param name="zipedFile">The ziped file.</param>
        /// <param name="strDirectory">The STR directory.</param>
        /// <param name="password">zip 文件的密码。</param>
        /// <param name="overWrite">是否覆盖已存在的文件。</param>
        public static void UnZip(string zipedFile, string strDirectory, string password, bool overWrite)
        {

            if (strDirectory == "")
                strDirectory = Directory.GetCurrentDirectory();
            if (!strDirectory.EndsWith("\\"))
                strDirectory = strDirectory + "\\";

            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipedFile)))
            {
                s.Password = password;
                ZipEntry theEntry;

                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = "";
                    string pathToZip = "";
                    pathToZip = theEntry.Name;

                    if (pathToZip != "")
                        directoryName = Path.GetDirectoryName(pathToZip) + "\\";

                    string fileName = Path.GetFileName(pathToZip);

                    Directory.CreateDirectory(strDirectory + directoryName);

                    if (fileName != "")
                    {
                        if ((File.Exists(strDirectory + directoryName + fileName) && overWrite) || (!File.Exists(strDirectory + directoryName + fileName)))
                        {
                            using (FileStream streamWriter = File.Create(strDirectory + directoryName + fileName))
                            {
                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);

                                    if (size > 0)
                                        streamWriter.Write(data, 0, size);
                                    else
                                        break;
                                }
                                streamWriter.Close();
                            }
                        }
                    }
                }

                s.Close();
            }
        }


        #region FastZip
        //(new FastZip()).ExtractEntry(@"E:\001a.zip", "a.txt");  //解压单个文件
        //(new FastZip()).ExtractEntry(@"E:\001a.zip", "test/a.dat"); //解压单个文件
        //(new FastZip()).ExtractEntry(@"E:\001a.zip", "images/");    //解压文件夹
        #endregion
    }
}
