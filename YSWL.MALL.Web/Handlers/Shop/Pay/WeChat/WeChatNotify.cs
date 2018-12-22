/**
* WeChatNotify.cs
*
* 功 能： 微信支付通知扩展
* 类 名： WeChatNotify
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/5/21 3:24:17  Ben    初版
*
* Copyright (c) 2014 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：小鸟科技（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System.Collections.Specialized;
using YSWL.Payment.PaymentInterface.WeChat.Utils;

namespace YSWL.MALL.Web.Handlers.Shop.Pay.WeChat
{
    public class WeChatNotify : YSWL.Payment.PaymentInterface.WeChat.WeChatNotify
    {
        private const string URL_DELIVER = "https://api.weixin.qq.com/pay/delivernotify?access_token={0}";

        public WeChatNotify(NameValueCollection parameters)
            : base(parameters)
        {
        }

        protected override void AutoDeliverNotify()
        {
            //TODO: 是否开启自动发货, 默认开启

            WxPayHelper wxPayHelper = new WxPayHelper();
            string wechatData = SetPayHelperBase(wxPayHelper);

            string AppId = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_AppId", -1, "AA");
            string AppSecret = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_AppSercet", -1, "AA");
            string token = YSWL.MALL.Web.Components.PostMsgHelper.GetToken(AppId, AppSecret);
            if (string.IsNullOrWhiteSpace(token)) return;


            string jsonStr = GetResponse(string.Format(URL_DELIVER, token), wechatData);
            //LogHelp.AddErrorLog(jsonStr, "", "AutoDeliverNotify");
            #region TODO: 自动发货成功相关操作
            if (string.IsNullOrWhiteSpace(jsonStr)) return;

            //try
            //{
            //    YSWL.Json.JsonObject jsonObject = JsonConvert.Import<JsonObject>(jsonStr);
            //    if (jsonObject["errcode"] != null && jsonObject["errcode"].ToString() == "0")
            //    {
            //        //发货成功
            //    }
            //}
            //catch (Exception)
            //{
            //} 
            #endregion
        }
    }
}