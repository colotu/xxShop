/**
* RechargeReturnTemplatedPageUnfinished.cs
*
* 功 能： 充值回调/通知 - 包含充值模块版本 封装未完成
* 类 名： RechargeReturnTemplatedPageUnfinished
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
using System.Collections.Specialized;
using System.Globalization;
using System.Web;
using System.Web.UI;
using Maticsoft.Payment.BLL;
using Maticsoft.Payment.Configuration;
using Maticsoft.Payment.Core;
using Maticsoft.Payment.Handler;
using Maticsoft.Payment.Model;

namespace Maticsoft.Payment.Web
{
    [Obsolete]
    public abstract class RechargeReturnTemplatedPageUnfinished : Page
    {
        protected decimal Amount;
        protected string GatewayName;
        protected long RechargeId;
        protected RechargeRequestInfo RechargeRequest;
        private bool isBackRequest;
        protected NotifyQuery Notify;
        protected PaymentModeInfo paymode;

        protected RechargeReturnTemplatedPageUnfinished(bool _isBackRequest)
        {
            this.isBackRequest = _isBackRequest;
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            this.DoValidate();
        }

        protected abstract void DisplayMessage(string status);
        private void DoValidate()
        {
            PayConfiguration config = PayConfiguration.GetConfig();
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add(this.Page.Request.Form);
            parameters.Add(this.Page.Request.QueryString);
            string tmpGatewayName = this.Page.Request.QueryString[Globals.GATEWAY_KEY];
            if (string.IsNullOrEmpty(tmpGatewayName))
            {
                this.ResponseStatus(true, "gatewaynotfound");
                return;
            }
            this.GatewayName = tmpGatewayName.ToLower();
            GatewayProvider provider = config.Providers[this.GatewayName] as GatewayProvider;
            if (provider == null)
            {
                this.ResponseStatus(true, "gatewaynotfound");
                return;
            }
            this.Notify = NotifyQuery.Instance(provider.NotifyType, parameters);
            if (this.isBackRequest)
            {
                this.Notify.ReturnUrl = Globals.FullPath(string.Format(Globals.PAYMENT_RETURN_URL, this.GatewayName));
            }
            this.RechargeId = long.Parse(this.Notify.GetOrderId(), CultureInfo.InvariantCulture);
            this.Amount = this.Notify.GetOrderAmount();
            this.RechargeRequest = PaymentModeManage.GetRechargeRequest(this.RechargeId);
            if (this.RechargeRequest == null)
            {
                this.ResponseStatus(true, "success");
            }
            else
            {
                this.Amount = this.RechargeRequest.RechargeBlance;
                this.paymode = PaymentModeManage.GetPaymentModeByName(this.RechargeRequest.PaymentGateway);
                if (this.paymode == null)
                {
                    this.ResponseStatus(true, "gatewaynotfound");
                }
                else
                {
                    PayeeInfo payee = new PayeeInfo
                    {
                        EmailAddress = this.paymode.EmailAddress,
                        Partner = this.paymode.Partner,
                        Password = this.paymode.Password,
                        PrimaryKey = this.paymode.SecretKey,
                        SecondKey = this.paymode.SecondKey,
                        SellerAccount = this.paymode.MerchantCode
                    };
                    this.Notify.PaidToIntermediary += new NotifyEventHandler(this.notify_PaidToIntermediary);
                    this.Notify.PaidToMerchant += new NotifyEventHandler(this.notify_PaidToMerchant);
                    this.Notify.NotifyVerifyFaild += new NotifyEventHandler(this.notify_NotifyVerifyFaild);
                    this.Notify.VerifyNotify(0x7530, payee);
                }
            }
        }

        private void notify_NotifyVerifyFaild(NotifyQuery sender)
        {
            PaymentModeManage.RemoveRechargeRequest(this.RechargeId);
            this.ResponseStatus(false, "verifyfaild");
        }

        private void notify_PaidToIntermediary(NotifyQuery sender)
        {
            this.ResponseStatus(false, "waitconfirm");
        }

        private void notify_PaidToMerchant(NotifyQuery sender)
        {
            DateTime now = DateTime.Now;
            TradeTypes selfHelpRecharge = TradeTypes.SelfHelpRecharge;
            decimal num = PaymentModeManage.GetAccountSummary(this.RechargeRequest.UserId).
                AccountAmount + this.RechargeRequest.RechargeBlance;
            BalanceDetailInfo balanceDetails = new BalanceDetailInfo
            {
                JournalNumber = this.RechargeRequest.RechargeId,
                UserId = this.RechargeRequest.UserId,
                TradeDate = now,
                TradeType = selfHelpRecharge,
                Income = this.RechargeRequest.RechargeBlance,
                Balance = num
            };
            if (this.paymode != null)
            {
                balanceDetails.Remark = "充值支付方式：" + this.paymode.Name;
            }
            if (PaymentModeManage.AddBalanceDetail(balanceDetails))
            {
                this.ResponseStatus(true, "success");
            }
            else
            {
                PaymentModeManage.RemoveRechargeRequest(this.RechargeId);
                this.ResponseStatus(false, "fail");
            }
        }

        private void ResponseStatus(bool success, string status)
        {
            if (this.isBackRequest)
            {
                this.Notify.WriteBack(HttpContext.Current, success);
            }
            else
            {
                this.DisplayMessage(status);
            }
        }
    }
}

