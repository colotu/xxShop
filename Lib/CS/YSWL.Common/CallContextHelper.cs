using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Messaging;

namespace YSWL.Common
{
    /// <summary>
    /// 通话线程帮助类，辅助动态连接数据库
    /// </summary>
    public class CallContextHelper
    {
        /// <summary>
        /// 获取指定Key 的线程值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            return Common.Globals.SafeString(CallContext.GetData(key), "");
        }
        /// <summary>
        /// 是否在Cookie中获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValueAndCookie(string key)
        {
            string objStr = Common.Globals.SafeString(CallContext.GetData(key), "");
            if (String.IsNullOrWhiteSpace(objStr))//Cookie缓存中获取
            {
                string cookieStr = Common.Cookies.getKeyCookie(key);
                objStr = Common.DEncrypt.DEncrypt.ConvertToNumber(cookieStr).ToString();
            }
            return objStr;
        }

        /// <summary>
        /// 设置key 线程值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="objStr"></param>
        public static void SetValue(string key, string objStr)
        {
            CallContext.SetData(key, objStr);
        }

        /// <summary>
        /// 设置key 线程值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="objStr"></param>
        public static void SetValueAndCookie(string key, string objStr, double time)
        {
            CallContext.SetData(key, objStr);
            CallContext.SetData("YSWL_WCF_EnterpriseID", Common.DEncrypt.DEncrypt.GetEncryptionStr(Common.Globals.SafeLong(objStr, 0)));//WCF 专用

            //获取配置的domin信息
            string domin = Common.ConfigHelper.GetConfigString("RootDomain");
            domin=!string.IsNullOrEmpty(domin) ? $".{domin}" : "";
            // 设置缓存
            Common.Cookies.setKeyCookie(key, Common.DEncrypt.DEncrypt.GetEncryptionStr(Common.Globals.SafeLong(objStr, 0)), domin, time);
        }
        /// <summary>
        /// 获取动态企业线程ID (未加密)
        /// </summary>
        /// <returns></returns>
        public static string GetAutoTag()
        {
            return GetValueAndCookie("YSWL_SAAS_EnterpriseID");
        }
        /// <summary>
        /// 获取企业ID
        /// </summary>
        /// <returns></returns>
        public static long GetEnterpriseId()
        {
            return Common.Globals.SafeInt(GetAutoTag(), 0);
        }

        /// <summary>
        /// 获取动态企业线程ID （未加密）
        /// </summary>
        /// <returns></returns>
        public static string GetClearTag()
        {
            return GetValue("YSWL_SAAS_EnterpriseID");
        }
        /// <summary>
        /// 设置动态企业线程ID （未加密）
        /// </summary>
        /// <param name="enterpriseId"></param>
        public static void SetAutoTag(long enterpriseId)
        {
            SetValueAndCookie("YSWL_SAAS_EnterpriseID", enterpriseId.ToString(), 1440);
        }

        #region WCF 线程用
        /// <summary>
        /// WCF 专用线程用
        /// </summary>
        /// <returns></returns>
        public static string GetWCFTag()
        {
            return GetValue("YSWL_WCF_EnterpriseID");
        }
        #endregion
        /// <summary>
        ///  获取动态企业线程ID （已加密成字符串）
        /// </summary>
        /// <returns></returns>
        public static string GetDEncrypTag()
        {
            long enterpriseStr = Common.Globals.SafeLong(GetAutoTag(), 0);
            return  YSWL.Common.DEncrypt.DEncrypt.GetEncryptionStr(enterpriseStr);
        }

    }
}
