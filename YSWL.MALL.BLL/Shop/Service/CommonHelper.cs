using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace YSWL.MALL.BLL.Shop.Service
{
    public class CommonHelper
    {
        /// <summary>
        /// 远程清空缓存
        /// </summary>
        public static void ClearCache()
        {
            //获取远程清空缓存的地址
            string url = Common.ConfigHelper.GetConfigString("Remote_ClearCache_Url");//SysManage.ConfigSystem.GetValueByCache("Remote_ClearCache_Url");
            if (String.IsNullOrWhiteSpace(url))
            {
                return;
            }
            try
            {
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
                System.Net.HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {
                Log.LogHelper.AddWarnLog("清空缓存异常", ex.Message+"---"+ex.StackTrace);
                throw;
            }
        }
        /// <summary>
        /// 先临时处理，（后期整体切换Redis缓存）是否开启对接OMS
        /// </summary>
        /// <returns></returns>
        public static bool ConnectionOMS()
        {
            bool isAuto = Common.ConfigHelper.GetConfigBool("AutoConnection");
            string key = "Shop_ConnectionOMS";
            if (isAuto)
            {
                try
                {
                     bool isOpenOMS = YSWL.DBUtility.SAASInfo.AppIsOpenCache("OMS", Common.Globals.SafeInt(Common.CallContextHelper.GetClearTag(), 0));
                     return  YSWL.DBUtility.SAASInfo.AppIsOpenCache("OMS", Common.Globals.SafeInt(Common.CallContextHelper.GetClearTag(), 0));
                }
                catch (Exception)
                {
                    throw;
                }             
            }
            return YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache(key);
        }
        /// <summary>
        /// 开启多分仓
        /// </summary>
        /// <returns></returns>
        public static bool OpenMultiDepot()
        {
            bool isAuto = Common.ConfigHelper.GetConfigBool("AutoConnection");
            string key = "Shop_OpenMultiDepot";
            if (isAuto)
            {
                return ConnectionOMS();
            }
            return YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache(key);
        }

    }
}
