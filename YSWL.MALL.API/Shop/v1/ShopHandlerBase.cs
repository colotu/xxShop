/**
* ShopHandlerBase.cs
*
* 功 能： Shop Json.RPC API 基类
* 类 名： ShopHandlerBase
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/12/17 17:04:23  Ben    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.SessionState;
using YSWL.Components;
using YSWL.Json.RPC.Web;

namespace YSWL.MALL.API.Shop.v1
{
    public partial class ShopHandler : YSWL.Components.Handlers.API.HandlerBase
    {
        public ShopHandler(bool _requestSecurity) : base(_requestSecurity) { }

        public ShopHandler() : base(false)
        {
            
        }

        //protected override bool RequestSecurity(HttpContext context)
        //{

        //    #region  判断是否为自动链接数据库
        //    if (MvcApplication.IsAutoConn)
        //    {
        //        string tag = context.Request.Headers["enterpriseStr"];
        //        if (!String.IsNullOrWhiteSpace(tag))
        //        {
        //            long enterpriseId = YSWL.Common.DEncrypt.DEncrypt.ConvertToNumber(tag);
        //            YSWL.Common.CallContextHelper.SetAutoTag(enterpriseId);
        //        }
        //    }
        //    #endregion 

        //    bool securityParam = Common.ConfigHelper.GetConfigBool("API_Security");
        //    int intervalSeconds = Common.ConfigHelper.GetConfigInt("API_TimeInterval");
        //    string apiKey = Common.ConfigHelper.GetConfigString("API_Key");
        //    if ((HttpContext.Current != null && HttpContext.Current.IsDebuggingEnabled) || !securityParam)//没开启安全验证直接请求
        //    {
        //        return true;
        //    }
        //    string requestId = context.Request.Headers["requestId"];
        //    string timespan = context.Request.Headers["timespan"];
        //    if (string.IsNullOrWhiteSpace(timespan)) return false;
        //    DateTime receiveTime = DateTime.ParseExact(timespan, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
        //    string md5Value = context.Request.Headers["md5hex"];
        //    if (string.IsNullOrWhiteSpace(md5Value)) return false;

        //    if (receiveTime.AddSeconds(intervalSeconds) > DateTime.Now)//在20s时间内
        //    {

        //        MD5 md5 = MD5.Create();
        //        string firstMd5;
        //        string secondMd5;
        //        byte[] bs = Encoding.UTF8.GetBytes(requestId + timespan + apiKey);
        //        byte[] hs = md5.ComputeHash(bs);
        //        StringBuilder sb = new StringBuilder();
        //        foreach (byte b in hs)
        //        {
        //            // 以十六进制格式格式化
        //            sb.Append(b.ToString("x2"));
        //        }
        //        firstMd5 = sb.ToString();
        //        sb.Clear();
        //        byte[] bss = Encoding.UTF8.GetBytes(requestId + timespan + firstMd5);
        //        byte[] hss = md5.ComputeHash(bss);
        //        // StringBuilder sb = new StringBuilder();
        //        foreach (byte b in hss)
        //        {
        //            // 以十六进制格式格式化
        //            sb.Append(b.ToString("x2"));
        //        }
        //        secondMd5 = sb.ToString();
        //        if (!string.Equals(secondMd5, md5Value))
        //        {
        //            return false;
        //        }
        //        return base.RequestSecurity(context);
        //    }
        //    return false;
        //}
    }
}