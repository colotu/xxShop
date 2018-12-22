 using System;

namespace YSWL.Common
{
    /// <summary>
    /// web.config操作类
    /// Copyright (C) 云商未来
    /// </summary>
    public sealed class ConfigHelper
    {
        /// <summary>
        /// 得到AppSettings中的配置字符串信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigString(string key)
        {
            string cacheKey = "AppSettings-" + key;
            object objModel = YSWL.Common.DataCache.GetCache(cacheKey);
            if (objModel == null)
            {
                try
                {
                    //TODO: 应兼容exe时的config文件读取 BEN ADD 2013-03-18 11:54
                    //DONE: 更正配置文件读取缓存BUG BEN MODIFY 2013-03-05 20:07
                    System.Configuration.Configuration rootWebConfig =
                    System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                    if (0 < rootWebConfig.AppSettings.Settings.Count &&
                        rootWebConfig.AppSettings.Settings[key] != null)
                    {
                        objModel = rootWebConfig.AppSettings.Settings[key].Value;
                        if (objModel == null || String.IsNullOrWhiteSpace(objModel.ToString()))//兼容exe时的config文件读取
                        {
                            objModel = System.Configuration.ConfigurationManager.AppSettings[key];
                        }
                        YSWL.Common.DataCache.SetCache(cacheKey, objModel, DateTime.Now.AddMinutes(180), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return objModel != null ? objModel.ToString() : null;
        }

        /// <summary>
        /// 得到exe AppSettings中的配置字符串信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetExeConfigString(string key)
        {
               return      System.Configuration.ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// 得到AppSettings中的配置Bool信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetConfigBool(string key)
        {
            bool result = false;
            string cfgVal = GetConfigString(key);
            if (!string.IsNullOrWhiteSpace(cfgVal))
            {
                try
                {
                    result = bool.Parse(cfgVal);
                }
                catch (FormatException)
                {
                    // Ignore format exceptions.
                }
            }
            return result;
        }
        /// <summary>
        /// 得到AppSettings中的配置Decimal信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static decimal GetConfigDecimal(string key)
        {
            decimal result = 0;
            string cfgVal = GetConfigString(key);
            if (!string.IsNullOrWhiteSpace(cfgVal))
            {
                try
                {
                    result = decimal.Parse(cfgVal);
                }
                catch (FormatException)
                {
                    // Ignore format exceptions.
                }
            }

            return result;
        }
        /// <summary>
        /// 得到AppSettings中的配置int信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetConfigInt(string key)
        {
            int result = 0;
            string cfgVal = GetConfigString(key);
            if (!string.IsNullOrWhiteSpace(cfgVal))
            {
                try
                {
                    result = int.Parse(cfgVal);
                }
                catch (FormatException)
                {
                    // Ignore format exceptions.
                }
            }

            return result;
        }
    }
}
