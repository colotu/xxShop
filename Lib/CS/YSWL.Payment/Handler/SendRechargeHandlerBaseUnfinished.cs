/**
* SendRechargeHandlerBaseUnfinished.cs
*
* 功 能： 充值接口 - 包含充值模块版本 封装未完成
* 类 名： SendRechargeHandlerBaseUnfinished
*
* Ver   变更日期    部门      担当者 变更内容
* ─────────────────────────────────
* V0.01 2012/01/13  研发部    姚远   初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌─────────────────────────────────┐
*│ 此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露． │
*│ 版权所有：动软卓越（北京）科技有限公司                           │
*└─────────────────────────────────┘
*/
using System;
using System.Globalization;
using System.Web;
using System.Web.SessionState;
using Maticsoft.Payment.BLL;
using Maticsoft.Payment.Configuration;
using Maticsoft.Payment.Core;
using Maticsoft.Payment.Model;

namespace Maticsoft.Payment.Handler
{
    [Obsolete]
    public abstract class SendRechargeHandlerBaseUnfinished<T> : IHttpHandler, IRequiresSessionState where T : class ,IUserBase
    {
        protected string HostName = string.Empty;
        protected string Recharge_ReturnUrl = string.Empty;
        protected string Recharge_NotifyUrl = string.Empty;

        #region 获取支付网关
        /// <summary>
        /// 获取支付网关
        /// </summary>
        protected virtual GatewayInfo GetGateway(string gatewayName)
        {
            GatewayInfo info = new GatewayInfo();
            info.ReturnUrl = Globals.FullPath(string.Format(string.IsNullOrEmpty(this.Recharge_ReturnUrl)
                                                                ? Globals.RECHARGE_RETURN_URL
                                                                : this.Recharge_ReturnUrl, gatewayName));
            info.NotifyUrl = Globals.FullPath(string.Format(string.IsNullOrEmpty(this.Recharge_NotifyUrl)
                                                                ? Globals.RECHARGE_NOTIFY_URL
                                                                : this.Recharge_NotifyUrl, gatewayName));
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

        #region 获取交易信息
        /// <summary>
        /// 获取交易信息
        /// </summary>
        /// <param name="payCharge">支付手续费</param>
        /// <param name="user">用户</param>
        protected virtual TradeInfo GetTrade(RechargeRequestInfo rechargeRequest, decimal payCharge, T user)
        {
            decimal totalMoney = rechargeRequest.RechargeBlance + payCharge;
            string orderId = rechargeRequest.RechargeId.ToString(CultureInfo.InvariantCulture);
            TradeInfo info = new TradeInfo();
            info.BuyerEmailAddress = user.Email;
            info.Date = rechargeRequest.TradeDate;
            info.OrderId = orderId;
            info.Showurl = Globals.HostPath(HttpContext.Current.Request.Url);
            info.Subject = HostName + "在线充值: " + orderId;
            info.Body = HostName + "在线充值: " + orderId + " 金额: " + totalMoney.ToString(CultureInfo.InvariantCulture);
            info.TotalMoney = totalMoney;
            return info;
        }
        #endregion

        #region IHttpHandler 成员

        public virtual bool IsReusable
        {
            get { return true; }
        }

        public virtual void ProcessRequest(HttpContext context)
        {
            //支付ID
            int paymentModeId = Globals.SafeInt(context.Request.QueryString["modeId"], 0);
            //充值金额
            decimal balance = Globals.SafeDecimal(context.Request.QueryString["blance"], 0M);

            //参数 NULL ERROR返回首页
            if ((paymentModeId == 0) || (balance == 0M))
            {
                //Add ErrorLog..
                return;
            }

            T user = GetCurrentUser(context);
            PayConfiguration config = PayConfiguration.GetConfig();
            PaymentModeInfo paymentMode = PaymentModeManage.GetPaymentModeById(paymentModeId);
            GatewayProvider provider = config.Providers[paymentMode.Gateway.ToLower()] as GatewayProvider;

            //计算支付手续费
            decimal payCharge = paymentMode.CalcPayCharge(balance);
#warning 未支持多币种支付手续费
            //根据多币种货币换算, 计算手续费
            //decimal payCharge = Sales.ScaleMoney(paymentMode.CalcPayCharge(balance));

            if (provider != null)
            {
                RechargeRequestInfo info2 = null;
                info2 = new RechargeRequestInfo
                {
                    TradeDate = DateTime.Now,
                    RechargeBlance = balance,
                    UserId = user.UserId,
                    PaymentGateway = paymentMode.Gateway
                };
                info2.RechargeId = PaymentModeManage.AddRechargeBalance(info2);
                if (info2.RechargeId > 0L)
                {
                    PaymentRequest.Instance(
                        provider.RequestType,
                        this.GetPayee(paymentMode),
                        this.GetGateway(paymentMode.Gateway.ToLower()),
                        this.GetTrade(info2, payCharge, user)
                        ).SendRequest();
                }
            }
        }
        #endregion

        #region 子类实现
        /// <summary>
        /// 获取当前登录(充值)用户
        /// </summary>
        /// <returns></returns>
        protected abstract T GetCurrentUser(HttpContext context);
        #endregion
    }
}