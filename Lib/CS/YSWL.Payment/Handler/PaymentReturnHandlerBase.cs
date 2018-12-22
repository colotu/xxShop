

/**
* PaymentReturnHandlerBase.cs
*
* 功 能： 支付回调/通知抽象Handler
* 类 名： PaymentReturnHandlerBase
*
* Ver   变更日期    部门      担当者 变更内容
* ─────────────────────────────────
* V0.01 2012/01/13  研发部    姚远   初版
* V0.02 2012/10/11  研发部    姚远   增加接口实现类, 减少其他模块耦合度
* V0.03 2013/05/07  研发部    姚远   增加测试模式
* V0.04 2014/01/14  研发部    姚远   新增网关自定义(动态)参数功能
*
* Copyright (c) 2012 YSWL Corporation. All rights reserved.
*┌─────────────────────────────────┐
*│ 此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露． │
*│ 版权所有：云商未来（北京）科技有限公司                           │
*└─────────────────────────────────┘
*/
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using YSWL.Payment.BLL;
using YSWL.Payment.Configuration;
using YSWL.Payment.Core;
using YSWL.Payment.Model;

namespace YSWL.Payment.Handler
{
    /// <summary>
    /// 支付回调/通知抽象Handler
    /// </summary>
    /// <typeparam name="T">订单信息</typeparam>
    public abstract class PaymentReturnHandlerBase<T> : IHttpHandler, IRequiresSessionState
        where T : class, IOrderInfo
    {
        protected string GatewayName;
        protected string[] GetwayDatas;
        protected NotifyQuery Notify;
        protected T Order;

        private readonly bool _isNotify;
        private decimal _amount;
        private string _orderId;

        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal Amount
        {
            get { return _amount; }
        }

        /// <summary>
        /// 订单ID
        /// </summary>
        protected string OrderId
        {
            get { return _orderId; }
        }

        protected IPaymentOption<T> Option;

        /// <summary>
        /// 构造支付回调/通知Handler
        /// </summary>
        /// <param name="option">支付网关回调URL参数</param>
        /// <param name="isNotify">是否为支付网关异步回调请求</param>
        protected PaymentReturnHandlerBase(IPaymentOption<T> option, bool isNotify)
        {
            Option = option;
            this._isNotify = isNotify;
        }

        protected PaymentModeInfo PaymentMode;

        protected virtual void ExecuteResult(bool success, string status) { }
        protected virtual void DisplayMessage(string status) { }
        protected void DoValidate()
        {
            PayConfiguration config = PayConfiguration.GetConfig();
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add(HttpContext.Current.Request.QueryString);
            parameters.Add(HttpContext.Current.Request.Form);
            string gatewayData = parameters[Globals.GATEWAY_KEY];

            if (string.IsNullOrEmpty(gatewayData))
            {
                Core.Globals.WriteText(new System.Text.StringBuilder(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " gatewaydatanotfound GATEWAYDATA IS NULL"));
                this.ResponseStatus(false, "gatewaydatanotfound");
                return;
            }
            parameters.Remove(Globals.GATEWAY_KEY);

            //获取GetwayData特殊Base64数据
            GetwayDatas = Globals.DecodeData4Url(gatewayData).Split(new[] { '|' }, System.StringSplitOptions.None);
            if (GetwayDatas.Length < 1)
            {
                Core.Globals.WriteText(new System.Text.StringBuilder(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " gatewaydatanotfound GetwayDatas.Length < 1"));
                this.ResponseStatus(false, "gatewaydatanotfound");
                return;
            }
            this.GatewayName = GetwayDatas[0].ToLower();
            GatewayProvider provider = config.Providers[this.GatewayName] as GatewayProvider;
            if (provider == null)
            {
                this.ResponseStatus(false, "gatewaynotfound");
                Core.Globals.WriteText(new System.Text.StringBuilder(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " gatewaydatanotfound PROVIDER IS NULL"));
                return;
            }
            if (string.IsNullOrWhiteSpace(provider.NotifyType))
            {
                this.ResponseStatus(false, "notifytypenotfound");
                Core.Globals.WriteText(new System.Text.StringBuilder(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " gatewaydatanotfound PROVIDER.NOTIFYTYPE IS NULL"));
                return;
            }

            if (provider.UseNotifyMode)     //DONE: 使用差异通知模式 BEN NEW MODE 20131016
            {
                this.Notify = NotifyQuery.Instance(provider.NotifyType, parameters,
                    _isNotify ? NotifyMode.Notify : NotifyMode.Callback);
            }
            else                            //回调和通知均使用同一种模式
            {
                this.Notify = NotifyQuery.Instance(provider.NotifyType, parameters);
            }
            if (this._isNotify)
            {
                this.Notify.ReturnUrl = Globals.FullPath(string.Format(Option.ReturnUrl, gatewayData));
            }

            #region 测试模式
            if (Globals.IsPaymentTestMode)
            {
                this._orderId = parameters["out_trade_no"];
            }
            #endregion
            else
            {
                this._orderId = this.Notify.GetOrderId();
            }

            this.Order = Option.GetOrderInfo(this._orderId);
            if (this.Order == null)
            {
                Core.Globals.WriteText(new System.Text.StringBuilder(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GETORDERINFO IS NULL| ORDERID:" + this._orderId));
                this.ResponseStatus(false, "ordernotfound");
                return;
            }

            #region 测试模式
            if (Globals.IsPaymentTestMode)
            {
                this._amount = Globals.SafeDecimal(parameters["total_fee"], -1);
            }
            #endregion
            else
            {
                this._amount = this.Notify.GetOrderAmount() == decimal.Zero
                    ? this.Order.Amount
                    : this.Notify.GetOrderAmount();
            }

            //设置支付网关生成的订单ID
            this.Order.GatewayOrderId = this.Notify.GetGatewayOrderId();
            PaymentMode = this.GetPaymentMode(this.Order.PaymentTypeId);
            if (PaymentMode == null)
            {
                Core.Globals.WriteText(new System.Text.StringBuilder(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " PAYMENTMODE IS NULL| PaymentTypeId:" + this.Order.PaymentTypeId));
                this.ResponseStatus(false, "gatewaynotfound");
                return;
            }

            if (this.Order.PaymentStatus == PaymentStatus.Prepaid)
            {
                this.ResponseStatus(true, "success");
            }
            else
            {
                #region 测试模式
                //DONE: 测试模式埋点
                if (Globals.IsPaymentTestMode)
                {
                    string sign = HttpContext.Current.Request.QueryString["sign"];
                    if (string.IsNullOrWhiteSpace(sign))
                        this.ResponseStatus(false, "<TestMode> no sign");

                    System.Text.StringBuilder url = new System.Text.StringBuilder(
                        Globals.FullPath(string.Format(Option.ReturnUrl, gatewayData)));
                    url.AppendFormat("&out_trade_no={0}", this._orderId);
                    url.AppendFormat("&total_fee={0}", this._amount);

                    if (sign != Globals.GetMd5(System.Text.Encoding.UTF8, url.ToString()))
                        this.ResponseStatus(false, "<TestMode> Unauthorized sign");

                    //效验通过
                    PaidToSite();
                    return;
                }
                #endregion

                PayeeInfo payee = new PayeeInfo
                {
                    EmailAddress = PaymentMode.EmailAddress,
                    Partner = PaymentMode.Partner,
                    Password = PaymentMode.Password,
                    PrimaryKey = PaymentMode.SecretKey,
                    SecondKey = PaymentMode.SecondKey,
                    SellerAccount = PaymentMode.MerchantCode
                };
                GatewayInfo getway = new GatewayInfo
                {
                    Data = gatewayData,
                    DataList = GetwayDatas.ToList(),
                    ReturnUrl = Option.ReturnUrl,
                    NotifyUrl = Option.NotifyUrl
                };
                this.Notify.NotifyVerifyFaild += new NotifyEventHandler(this.notify_NotifyVerifyFaild);
                this.Notify.PaidToIntermediary += new NotifyEventHandler(this.notify_PaidToIntermediary);
                this.Notify.PaidToMerchant += new NotifyEventHandler(this.notify_PaidToMerchant);

                this.Notify.VerifyNotify(0x7530, payee, getway);

            }
        }

        private void notify_NotifyVerifyFaild(NotifyQuery sender)
        {
            this.ResponseStatus(false, "verifyfaild");
        }

        private void PaidToSite()
        {
            if (this.Order.PaymentStatus == PaymentStatus.Prepaid)
            {
                this.ResponseStatus(true, "success");
            }
            else
            {
                if (OrderProcessor.CheckAction(this.Order, OrderActions.BUYER_PAY) && Option.PayForOrder(this.Order))
                {
                    this.ResponseStatus(true, "success");
                }
                else
                {
                    this.ResponseStatus(false, "fail");
                }
            }
        }

        private void notify_PaidToIntermediary(NotifyQuery sender)
        {
            PaidToSite();
        }

        private void notify_PaidToMerchant(NotifyQuery sender)
        {
            PaidToSite();
        }

        private void ResponseStatus(bool success, string status)
        {
            ExecuteResult(success, status);
            //Clear Page
            if (this._isNotify || !success)
            {
                HttpContext.Current.Response.Clear();
            }
            if (this._isNotify)
            {
                this.Notify.WriteBack(HttpContext.Current, success);
            }
            else
            {
                this.DisplayMessage(status);
            }
        }

        #region 获取支付信息
        /// <summary>
        /// 根据支付ID 获取支付信息
        /// </summary>
        /// <param name="paymentTypeId"></param>
        /// <returns></returns>
        protected virtual PaymentModeInfo GetPaymentMode(int paymentTypeId)
        {
            return PaymentModeManage.GetPaymentModeById(paymentTypeId);
        }
        #endregion

        #region IHttpHandler 成员

        public virtual bool IsReusable
        {
            get { return false; }
        }

        public virtual void ProcessRequest(HttpContext context)
        {
            //try
            //{
                this.DoValidate();
            //}
            //catch (Exception ex)
            //{
            //    Core.Globals.WriteText(new System.Text.StringBuilder(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ProcessRequest失败!" + ex.Message + "|StackTrace" + ex.StackTrace));
            //}
        }

        #endregion
    }
}

