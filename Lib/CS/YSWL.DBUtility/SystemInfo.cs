using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using YSWL.Common;

namespace YSWL.DBUtility
{
  public  class SystemInfo
    {
        /// <summary>
        /// 设置企业数据流向
        /// </summary>
        /// <param name="enterpriseId"></param>
        /// <returns></returns>
        public static string GetDataConn(int enterpriseId)
        {
            YSWL.Common.DataCacheCore coreBll = new DataCacheCore(new CacheOption
            {
                CacheType = CacheType.Redis,
                CancelProductKey = true,
                CancelEnterpriseKey = true,
                DefaultDb = 0,
                ReadWriteHosts = SAASInfo.GetSystemValue("RedisCacheReadWriteHosts"),
                ReadOnlyHosts = SAASInfo.GetSystemValue("RedisCacheReadOnlyHosts"),
            });
            string BaseKey = "OMS_ConnectionData";
            string CacheKey = BaseKey+"_" + enterpriseId;
            object objModel = coreBll.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    Common.CallContextHelper.SetAutoTag(enterpriseId);
                    objModel = GetSysValue(BaseKey);
                    if (objModel != null)
                    {
                        coreBll.SetCache(CacheKey, objModel, DateTime.MaxValue, TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return Common.Globals.SafeString(objModel,"ERP") ;
        }

        private static string GetSysValue(string key)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Value from SA_Config_System ");
            strSql.Append(" where Keyname=@Keyname ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Keyname", SqlDbType.NVarChar)};
            parameters[0].Value = key;

            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(), parameters);
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                return "";
            }
            else
            {
                return obj.ToString();
            }
        }
    }
}
