/**
* SendPaymentHandler.cs
*
* 功 能： 发送支付请求
* 类 名： SendPaymentHandler
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/05/14 17:28:15  Ben    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：小鸟科技（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using YSWL.MALL.Model.Shop.Order;
using YSWL.Web;
#pragma warning disable 612

namespace YSWL.MALL.Web.Handlers.Shop.Pay
{
    /// <summary>
    /// 发送支付请求
    /// </summary>
    public class SendPaymentHandler : Payment.Handler.SendPaymentHandlerBase<OrderInfo>
    {
        #region 成员
        private readonly BLL.Shop.Order.Orders _orderManage = new BLL.Shop.Order.Orders();
        private const string MSG_ERRORLOG =
            "SendPaymentHandler >> Verificationn[{0}] 操作用户[{1}] 已阻止非法方式支付订单!";
        public const string KEY_ORDERID = "SendPayment_OrderId";
        public const string KEY_ACCESSMETHOD = "SendPayment_AccessMethod";//访问方式  手机还是电脑

        #endregion

        #region 构造
        public SendPaymentHandler()
            : base(new PaymentOption())
        {
            #region 设置网站名称
            BLL.SysManage.WebSiteSet webSiteSet = new BLL.SysManage.WebSiteSet(
                Model.SysManage.ApplicationKeyType.System);
            base.HostName = webSiteSet.WebName;
            #endregion
        }
        #endregion

        #region 验证请求是否合法
        /// <summary>
        /// 验证请求是否合法
        /// </summary>
        protected override bool VerifySendPayment(System.Web.HttpContext context)
        {
            #region 验证请求是否合法
            string[] orderIds = Payment.OrderProcessor.GetQueryString4OrderIds(context.Request);
            if (orderIds == null || orderIds.Length < 1) return false;
            long orderId = Common.Globals.SafeLong(orderIds[0], -1);
            if (orderId < -1) return false;

            if (!context.User.Identity.IsAuthenticated)
            {
                //未登录
                context.Response.Redirect("/Account/Login");
                return false;
            }
            YSWL.Accounts.Bus.User currentUser;
            if (context.Session[Common.Globals.SESSIONKEY_USER] == null)
            {
                currentUser = new YSWL.Accounts.Bus.User(
                    new YSWL.Accounts.Bus.AccountsPrincipal(context.User.Identity.Name));
                context.Session[Common.Globals.SESSIONKEY_USER] = currentUser;
            }
            else
            {
                currentUser = (YSWL.Accounts.Bus.User)context.Session[Common.Globals.SESSIONKEY_USER];
            }

            Model.Shop.Order.OrderInfo orderInfo = _orderManage.GetModel(orderId);
            if (orderInfo == null)
            {
                Web.LogHelp.AddErrorLog(string.Format(MSG_ERRORLOG, orderId, currentUser.UserID),
                    "非法操作订单", "Shop >> SendPaymentHandler >> Verification >> OrderInfo Is NULL");
                context.Response.Redirect("/");
                return false;
            }
            if (orderInfo.BuyerID != currentUser.UserID)
            {
                Web.LogHelp.AddErrorLog(string.Format(MSG_ERRORLOG, orderId, currentUser.UserID),
                    "非法操作订单", "Shop >> SendPaymentHandler >> Verification >> Check BuyerID");
                context.Response.Redirect("/");
                return false;
            }

            Payment.Model.PaymentModeInfo paymentMode =
                Payment.BLL.PaymentModeManage.GetPaymentModeById(orderInfo.PaymentTypeId);
            if (paymentMode == null)
            {
                Web.LogHelp.AddErrorLog(string.Format(MSG_ERRORLOG, orderId, currentUser.UserID),
                    "非法操作订单", "Shop >> SendPaymentHandler >> Verification >> PaymentModeInfo Is NULL");
                context.Response.Redirect("/");
                return false;
            }
            #endregion

            string basePath = "/";
            string u = context.Request.ServerVariables["HTTP_USER_AGENT"];

            string area = context.Request.QueryString["Area"];
            if (!string.IsNullOrWhiteSpace(area))
            {
                basePath = string.Format("/{0}/", area);
            }
            //向网关写入请求发起源的Area
            this.GatewayDatas.Add(area);

            #region 支付宝银联

            if (paymentMode.Gateway == "alipaybank")
            {
                /** 
                * 关于银行编码：
                * 如： 招商银行【CMB】、中国建设银行【CCB】、中国工商银行【ICBCB2C】
                * 注意：优先使用B2C通道
                * 混合渠道: https://doc.open.alipay.com/doc2/detail.htm?spm=0.0.0.0.Nz80L8&treeId=63&articleId=103763&docType=1 
                * 纯借记卡渠道: https://doc.open.alipay.com/doc2/detail.htm?spm=0.0.0.0.1NpxKf&treeId=63&articleId=103764&docType=1
                **/

                string bankCode = context.Request.QueryString["BankCode"];
                if (!string.IsNullOrWhiteSpace(bankCode))
                {
                    //向网关写入用户选择的银行编码
                    this.GatewayDatas.Add(bankCode);
                }

            }

            #endregion 

            
            //微信支付 向网关写入 APPID OPENID
            if (paymentMode.Gateway.StartsWith("wechat"))
            {
                YSWL.Log.LogHelper.AddErrorLog("wechat", "进入微信支付");
                string weChatAppId = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_AppId", -1, "AA");
                if (string.IsNullOrWhiteSpace(weChatAppId))
                {
                    context.Response.Clear();
                    context.Response.Write("NO WECHAT_APPID > WECHAT APPID IS NULL!");
                    YSWL.Log.LogHelper.AddErrorLog("wechat", "NO WECHAT_APPID > WECHAT APPID IS NULL!");
                    return false;
                }
                this.GatewayDatas.Add(weChatAppId);
                string weChatOpenId = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_OpenId", -1, "AA");
                string weChatAppSercet = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_AppSercet", -1, "AA");
                if (string.IsNullOrWhiteSpace(weChatOpenId) || string.IsNullOrWhiteSpace(weChatAppSercet))
                {
                    context.Response.Clear();
                    context.Response.Write("NO WECHATINFO > WECHAT WECHAT_OPENID OR WECHAT_APPSERCET IS NULL!");
                    YSWL.Log.LogHelper.AddErrorLog("wechat", "NO WECHATINFO > WECHAT WECHAT_OPENID OR WECHAT_APPSERCET IS NULL!");
                    return false;
                }
                string authorizeCode = context.Request.QueryString["code"];
                if (string.IsNullOrWhiteSpace(authorizeCode))
                {
                    string authorizeUrl =
                       string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state={2}#wechat_redirect"
                       , weChatAppId, Common.Globals.UrlEncode(context.Request.Url.ToString()), "MATICSOFTBEN");
                    YSWL.Log.LogHelper.AddErrorLog("wechat-->authorizeUrl", authorizeUrl);
                    context.Response.Redirect(authorizeUrl);
                    return false;
                }

                string userOpenId = YSWL.WeChat.BLL.Core.Utils.GetUserOpenId(weChatAppId, weChatAppSercet, authorizeCode);
                if (string.IsNullOrWhiteSpace(userOpenId))
                {
                    context.Response.Clear();
                    context.Response.Write("NO USEROPENID > WECHAT USEROPENID IS NULL!");
                    return false;
                }
                this.GatewayDatas.Add(userOpenId);
            }

            if (u.ToLower().Contains("android") || u.ToLower().Contains("mobile"))//手机访问
            {
                if (!paymentMode.DrivePath.Contains("|2|"))//不能手机支付
                {
                    context.Session[KEY_ORDERID] = orderInfo.OrderId.ToString();
                    context.Response.Redirect("/m/PayResult/MFail");
                    return false;
                }
            }
            else//电脑访问
            {
                if (!paymentMode.DrivePath.Contains("|1|")) //不能电脑支付
                {
                    context.Session[KEY_ORDERID] = orderInfo.OrderId.ToString();
                    context.Response.Redirect(MvcApplication.GetCurrentRoutePath(AreaRoute.Shop) + "PayResult/MFail");
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region 美化交易信息

        protected override Payment.Model.TradeInfo GetTrade(string orderIdStr,
            decimal orderTotal, OrderInfo order)
        {
            //return base.GetTrade(orderIdStr, orderTotal, order);

            Payment.Model.TradeInfo info = new Payment.Model.TradeInfo();
            info.Body = HostName + "订单-订单号：[" + order.OrderCode + "]";
            info.BuyerEmailAddress = order.BuyerEmail;
            info.Date = order.OrderDate;
            info.OrderId = orderIdStr;
            info.Showurl = Common.Globals.HostPath(System.Web.HttpContext.Current.Request.Url);
            info.Subject = HostName + "订单-订单号：[" + order.OrderCode + "]-" +
                           "在线支付-订单支付金额" +
                           "：" + orderTotal.ToString("0.00") + "元";
            info.TotalMoney = orderTotal;
            return info;
        }

        #endregion
    }
}