/**
* SendPaymentHandlerBase.cs
*
* 功 能： 支付请求接口抽象基类
* 类 名： SendPaymentHandlerBase
*
* Ver   变更日期    部门      担当者 变更内容
* ─────────────────────────────────
* V0.01 2012/01/13  研发部    姚远   初版
* V0.02 2012/10/11  研发部    姚远   增加接口实现类, 减少其他模块耦合度
* V0.03 2013/05/07  研发部    姚远   增加测试模式
* V0.04 2014/01/10  研发部    姚远   新增网关自定义(动态)参数功能
*                                    并对网关信息以特殊Base64密文传输
*
* Copyright (c) 2012 YSWL Corporation. All rights reserved.
*┌─────────────────────────────────┐
*│ 此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露． │
*│ 版权所有：云商未来（北京）科技有限公司                           │
*└─────────────────────────────────┘
*/
using System.Web;
using System.Web.SessionState;
using YSWL.Payment.BLL;
using YSWL.Payment.Configuration;
using YSWL.Payment.Core;
using YSWL.Payment.Model;
using System.Collections.Generic;
using System.Linq;

namespace YSWL.Payment.Handler
{
    /// <summary>
    /// 支付请求接口抽象基类
    /// </summary>
    /// <typeparam name="T">订单信息</typeparam>
    public abstract class SendPaymentHandlerBase<T> : IHttpHandler, IRequiresSessionState
        where T : class, IOrderInfo, new()
    {
        protected string HostName = "YSWL ";
        protected IPaymentOption<T> Option;

        [System.Obsolete]
        protected List<string> GatewayDatas = new List<string>();

        //protected Json.JsonObject GatewayDataJson = new Json.JsonObject();

        /// <summary>
        /// 构造支付请求接口
        /// </summary>
        /// <param name="option">支付网关回调URL参数</param>
        protected SendPaymentHandlerBase(IPaymentOption<T> option)
        {
            this.Option = option;
        }

        #region 获取支付网关

        /// <summary>
        /// 获取支付网关
        /// </summary>
        protected virtual GatewayInfo GetGateway(string gatewayName)
        {
            GatewayInfo info = new GatewayInfo();

            //写入网关名称和附加参数
            GatewayDatas.Insert(0, gatewayName);

            #region 网关写入防伪参数
            GatewayDatas.Insert(2, "1");
            #endregion

            //GatewayDataJson.Accumulate("GatewayName", gatewayName);
            //针对URL 特殊Base64方法
            info.Data = Globals.EncodeData4Url(string.Join("|", GatewayDatas));
            info.DataList = GatewayDatas;
            YSWL.Log.LogHelper.AddInfoLog("网关参数", string.Join("|", info.DataList));
            info.ReturnUrl = Globals.FullPath(string.Format(Option.ReturnUrl, info.Data));
            info.NotifyUrl = Globals.FullPath(string.Format(Option.NotifyUrl, info.Data));
            return info;
        }

        #endregion

        #region 获取收款人信息

        /// <summary>
        /// 获取收款人信息
        /// </summary>
        protected virtual PayeeInfo GetPayee(PaymentModeInfo paymode)
        {
            if (paymode == null)
            {
                return null;
            }
            PayeeInfo info = new PayeeInfo();
            info.EmailAddress = paymode.EmailAddress;
            info.Partner = paymode.Partner;
            info.Password = paymode.Password;
            info.PrimaryKey = paymode.SecretKey;
            info.SecondKey = paymode.SecondKey;
            info.SellerAccount = paymode.MerchantCode;
            return info;
        }

        #endregion

        #region 获取交易信息 - 多个订单合并支付处理

        /// <summary>
        /// 获取交易信息
        /// </summary>
        protected virtual TradeInfo GetTrade(string orderIdStr, decimal totalMoney, T order)
        {
            TradeInfo info = new TradeInfo();
            info.Body = HostName + "订单 - [" + orderIdStr + "]";
            info.BuyerEmailAddress = order.BuyerEmail;
            info.Date = order.OrderDate;
            info.OrderId = orderIdStr;
            info.Showurl = Globals.HostPath(HttpContext.Current.Request.Url);
            info.Subject = HostName + "订单 - 订单号: [" + orderIdStr + "] - " +
                           "在线支付 - 订单支付金额" +
                           ": " + totalMoney.ToString("0.00") + " 元";
            info.TotalMoney = totalMoney;

            info.BuyerName = string.IsNullOrWhiteSpace(order.ShipName) ? "SHIPNAMEISNULL" : order.ShipName;
            info.BuyerAddress = order.ShipRegion + order.ShipAddress;
            info.BuyerPhone = order.ShipCellPhone;
            info.BuyerMobile = string.IsNullOrWhiteSpace(order.ShipTelPhone) ?
                order.ShipCellPhone : order.ShipTelPhone;
            return info;
        }

        #endregion

        #region 获取支付信息

        /// <summary>
        /// 获取支付信息
        /// </summary>
        protected virtual PaymentModeInfo GetPaymentMode(T orderInfo)
        {
            return PaymentModeManage.GetPaymentModeById(orderInfo.PaymentTypeId);
        }

        #endregion

        #region 获取订单支付金额

        /// <summary>
        /// 获取订单支付金额
        /// </summary>
        /// <param name="orderIds">订单ID - 合并支付使用</param>
        /// <param name="orderInfo">订单信息</param>
        /// <returns>支付金额</returns>
        protected virtual decimal GetOrderTotalMoney(string[] orderIds, T orderInfo)
        {
            //从订单信息中 返回订单支付金额
            return orderInfo.Amount;
        }

        #endregion

        #region 验证支付请求是否合法

        /// <summary>
        /// 验证支付请求是否合法
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>是否合法</returns>
        protected virtual bool VerifySendPayment(HttpContext context)
        {
            return true;
        }

        #endregion

        #region IHttpHandler 成员

        public virtual bool IsReusable
        {
            get { return false; }
        }

        public virtual void ProcessRequest(HttpContext context)
        {
            //Safe
            if (!VerifySendPayment(context)) return;

            //订单ID字符串
            string orderIdStr = string.Empty;

            //获取全部订单ID
            string[] orderIds = OrderProcessor.GetQueryString4OrderIds(context.Request, out orderIdStr);

            //订单ID NULL ERROR返回首页
            if (orderIds == null || orderIds.Length < 1)
            {
                //Add ErrorLog..
                HttpContext.Current.Response.Redirect("~/");
                return;
            }

            //合并支付 订单支付信息以第一份订单为主
            T orderInfo = Option.GetOrderInfo(orderIds[0]);
            if (orderInfo == null) return;

            //计算订单支付金额
            decimal totalMoney = this.GetOrderTotalMoney(orderIds, orderInfo);
            if (totalMoney < 0) return;

            if (orderInfo.PaymentStatus != PaymentStatus.NotYet)
            {
                //订单已支付
                context.Response.Write(
                    HttpContext.GetGlobalResourceObject("Resources", "IDS_ErrorMessage_SentPayment").ToString());
                return;
            }

            PaymentModeInfo paymentMode = GetPaymentMode(orderInfo);
            if (paymentMode == null || string.IsNullOrWhiteSpace(paymentMode.Gateway))
            {
                //订单历史的支付方式不存在
                context.Response.Write(
                    HttpContext.GetGlobalResourceObject("Resources", "IDS_ErrorMessage_NoPayment").ToString());
                return;
            }

            string getwayName = paymentMode.Gateway.ToLower();

            //获取支付网关
            GatewayProvider provider =
                PayConfiguration.GetConfig().Providers[getwayName] as GatewayProvider;
            if (provider == null) return;

            //支付网关
            GatewayInfo gatewayInfo = this.GetGateway(getwayName);

            //交易信息
            TradeInfo tradeInfo = this.GetTrade(orderIdStr, totalMoney, orderInfo);

            #region 测试模式
            //DONE: 测试模式埋点
            if (Globals.IsPaymentTestMode && !Globals.ExcludeGateway.Contains(getwayName.ToLower()))
            {
                System.Text.StringBuilder url = new System.Text.StringBuilder(gatewayInfo.ReturnUrl);
                url.AppendFormat("&out_trade_no={0}", tradeInfo.OrderId);
                url.AppendFormat("&total_fee={0}", tradeInfo.TotalMoney);
                url.AppendFormat("&sign={0}", Globals.GetMd5(System.Text.Encoding.UTF8, url.ToString()));
                HttpContext.Current.Response.Redirect(
                    gatewayInfo.ReturnUrl.Contains("?")
                        ? url.ToString()
                        : url.ToString().Replace("&out_trade_no", "?out_trade_no"), true);
                return;
            }
            #endregion

            #region 发送支付请求
            //发送支付请求
            PaymentRequest paymentRequest = PaymentRequest.Instance(
                    provider.RequestType,
                    this.GetPayee(paymentMode),
                    gatewayInfo,
                    tradeInfo
                    );
            if (paymentRequest == null) return;
            paymentRequest.SendRequest();
            #endregion
        }

        #endregion

    }
}