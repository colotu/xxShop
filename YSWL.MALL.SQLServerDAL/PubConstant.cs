using System;
using System.Configuration;
namespace YSWL.MALL.SQLServerDAL
{
    public class PubConstant
    {
        /// <summary>
        /// 得到web.config里配置项的数据库连接字符串。
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static string GetConnectionString(string configName)
        {
            string connectionString = YSWL.Common.ConfigHelper.GetConfigString(configName);
            string ConStringEncrypt = YSWL.Common.ConfigHelper.GetConfigString("ConStringEncrypt");
            if (ConStringEncrypt == "true")
            {
                connectionString = YSWL.Common.DEncrypt.DESEncrypt.Decrypt(connectionString);
            }
            return connectionString;
        }

    }
}
