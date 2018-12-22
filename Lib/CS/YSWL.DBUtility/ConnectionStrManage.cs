using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Web.Management;
using YSWL.Common;
using YSWL.Log;

namespace YSWL.DBUtility
{
    /// <summary>
    /// 链接字符串管理
    /// </summary>
    public class ConnectionStrManage
    {
        /// <summary>
        /// 锁的对象
        /// </summary>
        public static object obj = new object();

        /// <summary>
        /// 是否开启自动连接标识key，通过配置文件获取
        /// </summary>
        protected const string KEY_AUTOCONNECTION = "AutoConnection";
        /// <summary>
        /// SAAS基础连接字符串
        /// </summary>
        protected const string KEY_BASECONNECTION = "BaseConnectionStr";
        /// <summary>
        /// 系统标识key，从配置文件中获取系统标识
        /// </summary>
        protected const string KEY_SYSTEM_FLAG = "SystemFlag";


        public static YSWL.Common.DataCacheCore dataCache = null;

        /// <summary>
        /// 是否开启动态连接数据库
        /// </summary>
        public static bool isAutoConn
        {
            get
            {
                string isAuto = ConfigurationManager.AppSettings[KEY_AUTOCONNECTION];
                return String.IsNullOrWhiteSpace(isAuto) ? false : (isAuto.ToLower() == "true");
            }
        }

        /// <summary>
        /// 初始化集合字典
        /// </summary>
        static ConnectionStrManage()
        {
            // connectionStrs = new Dictionary<string, string>();
            //applicationIds = new Dictionary<string, int>();
        }


        /// <summary>
        /// 链接串内存管理保存字典
        /// </summary>
        //private static Dictionary<string, string> connectionStrs;


        /// <summary>
        /// 应用标识字典
        /// </summary>
        //private static Dictionary<string, int> applicationIds;

        /// <summary>
        /// 获取数据库链接串
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionStr()
        {
            if (isAutoConn)
            {
                lock (obj)
                {
                    //获取企业标识
                    string businessTag = Common.CallContextHelper.GetAutoTag();
                    if (string.IsNullOrEmpty(businessTag))
                    {
                        return null;
                    }
                    //if (connectionStrs != null && connectionStrs.Any(k => k.Key == businessTag))
                    //{
                    //    return connectionStrs[businessTag];
                    //}
                    //获取企业数据库配置信息
                    return GetBusinnessConStr(GetSystemFlag());
                }
            }
            //更新缓存
            ConfigurationManager.RefreshSection("appSettings");
            string connectionString = ConfigurationManager.AppSettings[PubConstant.KEY_CONNECTION];
            string conStringEncrypt = ConfigurationManager.AppSettings[PubConstant.KEY_ENCRYPT];
            if (conStringEncrypt == "true")
            {
                connectionString = DESEncrypt.Decrypt(connectionString);
            }
            return connectionString;
        }


        /// <summary>
        /// 企业的数据库配置数据库链接串
        /// </summary>
        public static string BaseDBStr
        {
            get
            {
                if (isAutoConn)
                {
                    //更新缓存
                    ConfigurationManager.RefreshSection("appSettings");
                    string connectionString = ConfigurationManager.AppSettings[KEY_BASECONNECTION];
                    return connectionString;
                }
                else
                {
                    ConfigurationManager.RefreshSection("appSettings");
                    string connectionString = ConfigurationManager.AppSettings[PubConstant.KEY_CONNECTION];
                    string conStringEncrypt = ConfigurationManager.AppSettings[PubConstant.KEY_ENCRYPT];
                    if (conStringEncrypt == "true")
                    {
                        connectionString = DESEncrypt.Decrypt(connectionString);
                    }
                    return connectionString;
                }
            }
        }
        /// <summary>
        /// 获取应用Id
        /// </summary>
        /// <returns></returns>
        public static int GetApplicationId()
        {
            if (isAutoConn)
            {
                lock (obj)
                {
                    //获取企业标识
                    string businessTag = Common.CallContextHelper.GetAutoTag();
                    if (string.IsNullOrEmpty(businessTag))
                    {
                        return 0;
                    }
                    string applicationTag = GetSystemFlag();
                    if (GetBusinnessConStr(applicationTag) == null)
                    {
                        return 0;
                    }

                    return GetApplicationId(applicationTag);
                }
            }
            return 0;
        }

        /// <summary>
        /// 获取系统的标识
        /// </summary>
        /// <returns></returns>
        public static string GetSystemFlag()
        {
            return ConfigurationManager.AppSettings[KEY_SYSTEM_FLAG];
        }

        /// <summary>
        /// 获取企业的数据库配置信息
        /// </summary>
        /// <returns></returns>
        public static string GetBusinnessConStr(string applicationTag)
        {
            applicationTag = applicationTag.ToUpper();
            string CacheKey = "SAAS_ConnectionString_" + applicationTag+"_"+ Common.CallContextHelper.GetAutoTag();

            dataCache = new DataCacheCore(
                new CacheOption
                {
                    CacheType = CacheType.Redis,
                    CancelProductKey = true,
                    CancelEnterpriseKey = true,
                    DefaultDb = 0,
                    ReadWriteHosts = SAASInfo.GetSystemValue("RedisCacheReadWriteHosts"),
                    ReadOnlyHosts = SAASInfo.GetSystemValue("RedisCacheReadOnlyHosts"),
                });

           object objModel = dataCache.GetCache(CacheKey);
            if (objModel == null || String.IsNullOrWhiteSpace(objModel.ToString()))
            {
                try
                {
                    DataSet ds =
                        Query(
                            "select * from  SA_EnterpriseDBConfig  where EnterpriseId=@EnterpriseId  and [State]=1 and ApplicationTag=@ApplicationTag",
                            new SqlParameter[]
                            {
                                new SqlParameter("@EnterpriseId", Common.CallContextHelper.GetAutoTag()),
                                new SqlParameter("@ApplicationTag", applicationTag)
                            });
                    if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count > 1)
                    {
                        return null;
                    }
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        objModel = "server=" + item["RemoteDB_IP"] + ";database=" + item["DBName"] + ";uid=" +
                                   item["UserInstance"] + ";pwd=" + item["Passworld"];
                        break;
                    }
                    if (objModel != null && !String.IsNullOrWhiteSpace(objModel.ToString()))
                    {
                        dataCache.SetCache(CacheKey, objModel, DateTime.MaxValue, TimeSpan.Zero);
                    }
                }
                catch (Exception ex)
                {
                    Log.LogHelper.AddErrorLog("获取连接地址失败：" + ex.Message, "详细错误为：" + ex.StackTrace);
                    throw ex;
                }
            }
            return Common.Globals.SafeString(objModel, "");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool IsDBExists()
        {
            string CacheKey = "YSWL_IsDBExists_ConnectionString";
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    string ConnectionString = GetConnectionStr();//GetBusinnessConStr(GetSystemFlag());
                    SqlConnection connection = new SqlConnection(ConnectionString);
                    connection.Open();
                    connection.Close();
                    objModel = true;
                }
                catch (Exception exception)
                {
                    return false;
                }
                if (objModel != null)
                {
                    int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("ModelCache");
                    YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                }
            }
            return Common.Globals.SafeBool(objModel, false);
        }

        /// <summary>
        /// 查询企业数据库配置
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static DataSet Query(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(BaseDBStr))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds;
                }
            }
        }

        public static object GetSingle4Trans(CommandInfo commandInfo, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand();
            string cmdText = commandInfo.CommandText;
            SqlParameter[] cmdParms = (SqlParameter[])commandInfo.Parameters;
            PrepareCommand(cmd, trans.Connection, trans, cmdText, cmdParms);
            object obj = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                return null;
            }
            else
            {
                return obj;
            }
        }


        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(BaseDBStr))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }


        public static bool Exists(string strSql, params SqlParameter[] cmdParms)
        {
            object obj = GetSingle(strSql, cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public static object GetSingle(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(BaseDBStr))
            {

                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        throw e;
                    }
                }
            }
        }


        public static int ExecuteSql(string SQLString, params SqlParameter[] cmdParms)
        {
            using (
                SqlConnection connection = new SqlConnection(BaseDBStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="cmdList">SQL语句的CommandInfo</param>
        /// <param name="trans">外部事务对象</param>
        /// <remarks>警告:内部不触发事务的提交和回滚</remarks>
        /// <remarks>只使用了CommandInfo-EffentNextType.ExcuteEffectRows, 其它项暂不支持</remarks>
        public static int ExecuteSqlTran4Indentity(System.Collections.Generic.List<CommandInfo> cmdList, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand();
            int count = 0;
            int indentity = 0;
            //循环
            foreach (CommandInfo commandInfo in cmdList)
            {
                string cmdText = commandInfo.CommandText;
                SqlParameter[] cmdParms = (SqlParameter[])commandInfo.Parameters;
                foreach (SqlParameter parameter in cmdParms)
                {
                    if (parameter.Direction == ParameterDirection.InputOutput)
                    {
                        parameter.Value = indentity;
                    }
                }
                PrepareCommand(cmd, trans.Connection, trans, cmdText, cmdParms);
                //执行 并计数
                int val = cmd.ExecuteNonQuery();
                count += val;
                if (commandInfo.EffentNextType == EffentNextType.ExcuteEffectRows && val == 0)
                {
                    //未执行成功抛出异常, 由外部进行事务回滚
                    throw new SqlExecutionException("DbHelperSQL.ExecuteSqlTran4Indentity - [" + cmd.CommandText + "] 未执行成功!");
                }
                foreach (SqlParameter parameter in cmdParms)
                {
                    if (parameter.Direction == ParameterDirection.Output)
                    {
                        indentity = Convert.ToInt32(parameter.Value);
                    }
                }
                cmd.Parameters.Clear();
            }
            return count;
        }

        /// <summary>
        /// 组织参数
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        ///// <summary>
        ///// 将企业链接串保存到内存字典中
        ///// </summary>
        ///// <param name="businnessTag">企业标识</param>
        ///// <param name="cnnStr">链接串</param>
        ///// <param name="applicationId">应用Id</param>
        //public static void SaveConnectionStr(string businnessTag, string cnnStr, int applicationId)
        //{
        //    if (connectionStrs != null && !connectionStrs.Any(k => k.Key == businnessTag))
        //    {
        //        connectionStrs.Add(businnessTag, cnnStr);
        //        applicationIds.Add(businnessTag, applicationId);
        //    }
        //}
        /// <summary>
        /// 获取应用ID
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static int GetApplicationId(string tag)
        {
            if (String.IsNullOrWhiteSpace(tag))
            {
                tag = Common.ConfigHelper.GetConfigString("SystemFlag");
            }
            tag = tag.ToUpper();
            switch (tag)
            {
                case "WMS":
                    return 1;
                case "OMS":
                    return 2;
                case "SCM":
                    return 3;
                case "PMS":
                    return 4;
                case "MALLB":
                    return 5;
                case "ERP":
                    return 6;
                case "BI":
                    return 7;
                case "CRM":
                    return 8;
                case "SALES":
                    return 9;
                default:
                    return 0;
            }
        }

    }

}
