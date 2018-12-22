/**
* SqlExecutor.cs
*
* 功 能： [N/A]
* 类 名： SqlExecutor
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/4/28 17:08:43  Ben    初版
*
* Copyright (c) 2013 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;

namespace YSWL.Common
{
    /// <summary>
    /// Sql执行器
    /// </summary>
    public static class SqlExecutor
    {
        #region 执行Sql文件
        /// <summary>
        /// 执行Sql文件
        /// </summary>
        /// <param name="connectionString">ConnectionString</param>
        /// <param name="fileMapPath">FileMapPath</param>
        /// <exception cref="FileNotFoundException">FileNotFound</exception>
        public static void ExecuteScriptFile(string connectionString, string fileMapPath)
        {
            if (!File.Exists(fileMapPath))
            {
                throw new FileNotFoundException("ExecuteScriptFile: " + fileMapPath + " FileNotFound !");
            }
            using (StreamReader reader = new StreamReader(fileMapPath, System.Text.Encoding.Default))
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlTransaction transaction = null;
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandTimeout = 120;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    command.Transaction = transaction;
                    while (!reader.EndOfStream)
                    {
                        string str = NextSqlFromStream(reader);
                        if (!string.IsNullOrWhiteSpace(str))
                        {
                            command.CommandText = str;
                            command.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    if (transaction != null) transaction.Rollback();
                    throw;
                }
                finally
                {
                    reader.Close();
                    connection.Close();
                }
            }
        }

        private static string NextSqlFromStream(StreamReader reader)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                string readLine = reader.ReadLine();

                if (readLine == null) return string.Empty;

                string strA = readLine.Trim();
                while (!reader.EndOfStream && (string.Compare(strA, "GO", true, CultureInfo.InvariantCulture) != 0))
                {
                    builder.Append(strA + Environment.NewLine);
                    strA = readLine;
                }
                if (string.Compare(strA, "GO", true, CultureInfo.InvariantCulture) != 0)
                {
                    builder.Append(strA + Environment.NewLine);
                }
                return builder.ToString();
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 执行Sql语句
        /// <summary>
        /// 执行Sql文件
        /// </summary>
        /// <param name="connectionString">ConnectionString</param>
        /// <param name="safeSql">SafeSql</param>
        /// <exception cref="FileNotFoundException">FileNotFound</exception>
        public static int ExecuteSql(string connectionString, string safeSql)
        {
            if (string.IsNullOrWhiteSpace(safeSql))
            {
                throw new ArgumentNullException("ExecuteSql: SafeSql IsNULL !");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandTimeout = 120;
                command.CommandText = safeSql;
                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// DB服务器版本 暂未启用
    /// </summary>
    internal enum DBServerType
    {
        SQLServer,
        MySql,
        Oracle
    }
}
