/**
* FileManage.cs
*
* 功 能： [文件操作类]
* 类 名： FileManage
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/10/16 14:34:37  Rock    初版
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
using System.Collections;
using System.Reflection;
using System.Text;
using System.Web;

namespace YSWL.Common
{
    /// <summary>
    /// 文件操作类
    /// </summary>
    public class FileManage
    {
        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="oldPath">源文件路径</param>
        /// <param name="newPath">目标文件路径</param>
        /// <param name="fileName">文件名称</param>
        public static void MoveFile(string oldPath, string newPath, string fileName)
        {
            if (!Directory.Exists(newPath))
            {
                //不存在则自动创建文件夹
                Directory.CreateDirectory(newPath);
            }
            File.Move(oldPath + fileName, newPath + fileName);
        }

        /// <summary>
        /// 批量移动文件
        /// </summary>
        /// <param name="oldPath">源文件路径</param>
        /// <param name="newPath">目标文件路径</param>
        /// <param name="fileNameList">文件名称</param>
        public static void MoveFile(string oldPath, string newPath, ArrayList fileNameList)
        {
            if (!Directory.Exists(newPath))
            {
                //不存在则自动创建文件夹
                Directory.CreateDirectory(newPath);
            }
            for (int i = 0; i < fileNameList.Count; i++)
            {
                File.Move(oldPath + fileNameList[i], newPath + fileNameList[i]);
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>删除结果，成功或失败</returns>
        public static bool DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <returns>删除结果，成功或失败</returns>
        public static bool DeleteFolder(string path)
        {
            try
            {
                Directory.Delete(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 移动文件夹
        /// </summary>
        /// <param name="oldPath">源文件夹路径</param>
        /// <param name="newPath">目标文件夹路径</param>
        /// <returns>移动结果</returns>
        public static bool MoveFolder(string oldPath, string newPath)
        {
            try
            {
                Directory.Move(oldPath, newPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #region 检测文件

        /// <summary>
        /// 判断是否为安全文件名
        /// </summary>
        /// <param name="str">文件名</param>
        public static bool IsSafeName(string strExtension)
        {
            strExtension = strExtension.ToLower();
            if (strExtension.LastIndexOf(".") >= 0)
            {
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
            }
            else
            {
                strExtension = ".txt";
            }
            string[] arrExtension =
            {
                ".htm", ".html", ".txt", ".js", ".css", ".xml", ".sitemap", ".jpg", ".gif", ".png",
                ".rar", ".zip"
            };
            for (int i = 0; i < arrExtension.Length; i++)
            {
                if (strExtension.Equals(arrExtension[i]))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///  判断是否为不安全文件名
        /// </summary>
        /// <param name="str">文件名、文件夹名</param>
        public static bool IsUnsafeName(string strExtension)
        {
            strExtension = strExtension.ToLower();
            if (strExtension.LastIndexOf(".") >= 0)
            {
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
            }
            else
            {
                strExtension = ".txt";
            }
            string[] arrExtension =
            {
                ".", ".asp", ".aspx", ".cs", ".net", ".dll", ".config", ".ascx", ".master", ".asmx",
                ".asax", ".cd", ".browser", ".rpt", ".ashx", ".xsd", ".mdf", ".resx", ".xsd"
            };
            for (int i = 0; i < arrExtension.Length; i++)
            {
                if (strExtension.Equals(arrExtension[i]))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///  判断是否为可编辑文件
        /// </summary>
        /// <param name="str">文件名、文件夹名</param>
        public static bool IsCanEdit(string strExtension)
        {
            strExtension = strExtension.ToLower();

            if (strExtension.LastIndexOf(".") >= 0)
            {
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
            }
            else
            {
                strExtension = ".txt";
            }
            string[] arrExtension = { ".htm", ".html", ".txt", ".js", ".css", ".xml", ".sitemap" };
            for (int i = 0; i < arrExtension.Length; i++)
            {
                if (strExtension.Equals(arrExtension[i]))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion 检测文件

        public static void WriteText(StringBuilder log, string fileName = "Log")
        {
            string path = GetWebAssemblyPath();
            string dir = string.Format("{0}/log/", path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            fileName = string.Format("{0}{1}_{2}.txt", dir, fileName,
                DateTime.Now.ToString("yyyyMMdd"));
            try
            {
                if (File.Exists(fileName))
                {
                    using (StreamWriter sw = File.AppendText(fileName))
                    {
                        sw.WriteLine(log);
                        sw.Flush();
                        sw.Close();
                    }
                }
                else
                {
                    using (StreamWriter sw = File.CreateText(fileName))
                    {
                        sw.WriteLine(log);
                        sw.Flush();
                        sw.Close();
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 获取Assembly的运行路径
        /// </summary>
        /// <returns></returns>
        public static string GetWebAssemblyPath()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            DirectoryInfo dr = new DirectoryInfo(Path.GetDirectoryName(path));
            if (dr.Parent != null) path = dr.Parent.FullName; //当前目录的上一级目录
            return path;
        }
    }
}