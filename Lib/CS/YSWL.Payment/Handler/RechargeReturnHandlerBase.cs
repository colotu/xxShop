﻿/**
* RechargeReturnHandlerBase.cs
*
* 功 能： 支付模块回调/通知抽象基类
* 类 名： RechargeReturnHandlerBase
*
* Ver   变更日期    部门      担当者 变更内容
* ─────────────────────────────────
* V0.01 2012/01/13  研发部    姚远   初版
* V0.02 2012/10/11  研发部    姚远   增加接口实现类, 减少其他模块耦合度
* V0.03 2013/09/03  研发部    姚远   增加测试模式
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
    /// 支付模块回调/通知抽象基类
    /// </summary>
    public abstract class RechargeReturnHandlerBase :
        RechargeReturnHandlerBase<RechargeRequestInfo, UserInfo>
    {
        /// <summary>
        /// 构造充值请求接口
        /// </summary>
        /// <param name="option">充值网关回调URL参数</param>
        /// <param name="isNotify">是否为异步通知模式</param>
        protected RechargeReturnHandlerBase(
            IRechargeOption<RechargeRequestInfo, UserInfo> option,
            bool isNotify)
            : base(option, isNotify)
        {
        }
    }

    /// <summary>
    /// 支付模块回调/通知抽象基类
    /// </summary>
    public abstract class RechargeReturnHandlerBase<T, U> : IHttpHandler, IRequiresSessionState
        where T : class, IRechargeRequest, new()
        where U : class, IUserInfo, new()
    {
        protected string GatewayName;
        protected string[] GetwayDatas;
        private long _rechargeId;
        private decimal _amount;

        /// <summary>
        /// 充值流水号
        /// </summary>
        public long RechargeId
        {
            get { return _rechargeId; }
            set { _rechargeId = value; }
        }
        /// <summary>
        /// 充值金额
        /// </summary>
        public decimal Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        protected RechargeRequestInfo RechargeRequest;
        private readonly bool _isNotify;
        protected NotifyQuery Notify;
        protected PaymentModeInfo paymode;

        protected IRechargeOption<T, U> Option;

        /// <summary>
        /// 构造充值请求接口
        /// </summary>
        /// <param name="option">充值网关回调URL参数</param>
        /// <param name="isNotify">是否为异步通知模式</param>
        protected RechargeReturnHandlerBase(IRechargeOption<T, U> option, bool isNotify)
        {
            this.Option = option;
            this._isNotify = isNotify;
        }

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
                Core.Globals.WriteText(new System.Text.StringBuilder(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " gatewaydatanotfound PROVIDER IS NULL"));
                this.ResponseStatus(false, "gatewaynotfound");
                return;
            }
            if (string.IsNullOrWhiteSpace(provider.NotifyType))
            {
                Core.Globals.WriteText(new System.Text.StringBuilder(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " gatewaydatanotfound PROVIDER.NOTIFYTYPE IS NULL"));
                this.ResponseStatus(false, "notifytypenotfound");
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
                this.Notify.ReturnUrl = Globals.FullPath(string.Format(Option.ReturnUrl, this.GatewayName));
            }

            #region 测试模式
            if (Globals.IsRechargeTestMode)
            {
                this.RechargeId = Globals.SafeLong(parameters["out_trade_no"], -1);
            }
            #endregion
            else
            {
                this.RechargeId = Globals.SafeLong(this.Notify.GetOrderId(), -1);
            }
            this.RechargeRequest = PaymentModeManage.GetRechargeRequest(this.RechargeId);
            //this.RechargeRequest = Option.GetRechargeRequest(this.RechargeId);
            if (this.RechargeRequest == null)
            {
                Core.Globals.WriteText(new System.Text.StringBuilder(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " RECHARGEREQUEST IS NULL| RechargeId:" + this.RechargeId));
                this.ResponseStatus(true, "fail");
                return;
            }

            this.Amount = this.RechargeRequest.RechargeBlance;
            this.paymode = this.GetPaymentMode(this.RechargeRequest.PaymentTypeId);
            if (this.paymode == null)
            {
                Core.Globals.WriteText(new System.Text.StringBuilder(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " PAYMENTMODE IS NULL| PaymentTypeId:" + this.RechargeRequest.PaymentTypeId));
                this.ResponseStatus(false, "gatewaynotfound");
                return;
            }

            //DONE: 充值订单状态反复通知问题 BEN ADD 20131023
            if (this.RechargeRequest.PaymentStatus == PaymentStatus.Prepaid)
            {
                this.ResponseStatus(true, "success");
            }
            else
            {
                #region 测试模式
                //DONE: 测试模式埋点
                if (Globals.IsRechargeTestMode)
                {
                    string sign = HttpContext.Current.Request.QueryString["sign"];
                    if (string.IsNullOrWhiteSpace(sign))
                        this.ResponseStatus(false, "<TestMode> no sign");

                    System.Text.StringBuilder url = new System.Text.StringBuilder(
                        Globals.FullPath(string.Format(Option.ReturnUrl, this.GatewayName)));
                    url.AppendFormat("&out_trade_no={0}", this._rechargeId);
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
                    EmailAddress = this.paymode.EmailAddress,
                    Partner = this.paymode.Partner,
                    Password = this.paymode.Password,
                    PrimaryKey = this.paymode.SecretKey,
                    SecondKey = this.paymode.SecondKey,
                    SellerAccount = this.paymode.MerchantCode
                };

                GatewayInfo getway = new GatewayInfo
                {
                    Data = gatewayData,
                    DataList = GetwayDatas.ToList(),
                    ReturnUrl = Option.ReturnUrl,
                    NotifyUrl = Option.NotifyUrl
                };
                this.Notify.PaidToIntermediary += new NotifyEventHandler(this.notify_PaidToIntermediary);
                this.Notify.PaidToMerchant += new NotifyEventHandler(this.notify_PaidToMerchant);
                this.Notify.NotifyVerifyFaild += new NotifyEventHandler(this.notify_NotifyVerifyFaild);
                this.Notify.VerifyNotify(0x7530, payee, getway);
            }
        }

        private void PaidToSite()
        {
            if (Option.PayForRechargeRequest(RechargeRequest))
            {
                this.ResponseStatus(true, "success");
            }
            else
            {
                this.ResponseStatus(false, "fail");
            }
        }

        private void notify_NotifyVerifyFaild(NotifyQuery sender)
        {
            this.ResponseStatus(false, "verifyfaild");
        }

        private void notify_PaidToIntermediary(NotifyQuery sender)
        {
            this.ResponseStatus(false, "waitconfirm");
        }

        private void notify_PaidToMerchant(NotifyQuery sender)
        {
            PaidToSite();
        }

        private void ResponseStatus(bool success, string status)
        {
            //Core.Globals.WriteText(new System.Text.StringBuilder(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ResponseStatus|success:"+ success+ "|status:" + status));
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

