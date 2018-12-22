using System.Globalization;
using YSWL.Payment.Core;
using YSWL.Payment.Model;

namespace YSWL.Payment.PaymentInterface.AlipayBank
{
    internal class AlipayBankRequest : PaymentRequest
    {
        private string _input_charset = "utf-8";
        private string body;
        private string extend_param = "isv^sh31";
        private string gateway = "https://mapi.alipay.com/gateway.do?";
        private string key;
        private string notify_url;
        private string out_trade_no;
        private string partner;
        private string payment_type = "1";
        private string return_url;
        private string seller_email;
        private string service = "create_direct_pay_by_user";
        private string show_url;
        private string sign_type = "MD5";
        private string subject;
        private string token;
        private string total_fee;
        private string paymethod = "bankPay";
        private string defaultbank = "CMB"; //默认招商银行

        public AlipayBankRequest(PayeeInfo payee, GatewayInfo gateway, TradeInfo trade)
        {
            this.partner = payee.SellerAccount;
            this.key = payee.PrimaryKey;
            this.seller_email = payee.EmailAddress;
            this.body = trade.Body;
            this.out_trade_no = trade.OrderId;
            this.subject = trade.Subject;
            this.total_fee = trade.TotalMoney.ToString("F", CultureInfo.InvariantCulture);
            this.return_url = gateway.ReturnUrl;
            this.notify_url = gateway.NotifyUrl;
            this.show_url = trade.Showurl;
            this.token = trade.Token;

            if (gateway.DataList != null && gateway.DataList.Count >= 4)
            {
                /**
                 * Handlers.Shop.Pay.SendPaymentHandler
                 * 发起支付前先给用户一个选择银行的页面, 可利用现有的提交订单成功页面加入针对不同网关开启银行卡选择的功能
                 * VerifySendPayment方法中加入判断 paymentMode.Gateway == "alipaybank"
                 * 如果是alipaybank向网关[GatewayDatas]加入选择的银行编码
                 * this.GatewayDatas.Add(area); 代码后加入 this.GatewayDatas.Add([SelectBankCode]);
                */
                this.defaultbank = gateway.DataList[3];
            }
        }

        public override void SendRequest()
        {
            this.RedirectToGateway(Globals.CreatDirectUrl(this.gateway, this.service, this.partner, this.sign_type, this.out_trade_no, this.subject, this.body, this.payment_type, this.total_fee, this.show_url, this.seller_email, this.key, this.return_url, this._input_charset, this.notify_url, this.extend_param, this.token, this.paymethod, this.defaultbank));
        }
    }
}
