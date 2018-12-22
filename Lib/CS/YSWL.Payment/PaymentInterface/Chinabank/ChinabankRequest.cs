using YSWL.Payment.Core;
using YSWL.Payment.Model;

namespace YSWL.Payment.PaymentInterface.Chinabank
{
    using System.Globalization;
    using System.Text;
    using System.Web.Security;

    internal class ChinabankRequest : PaymentRequest
    {
        private string gateway = "https://pay3.chinabank.com.cn/PayGate";
        private string key = "";
        private string remark1 = "Chinabank";
        private string v_amount = "";
        private string v_mid = "";
        private string v_moneytype = "CNY";
        private string v_oid = "";
        private string v_url = "";

        public ChinabankRequest(PayeeInfo payee, GatewayInfo gateway, TradeInfo trade)
        {
            this.v_oid = trade.OrderId;
            this.v_amount = trade.TotalMoney.ToString("F", CultureInfo.InvariantCulture);
            this.v_url = gateway.ReturnUrl;
            this.v_mid = payee.SellerAccount;
            this.key = payee.PrimaryKey;
        }

        public override void SendRequest()
        {
            string strValue = FormsAuthentication.HashPasswordForStoringInConfigFile(this.v_amount + this.v_moneytype + this.v_oid + this.v_mid + this.v_url + this.key, "MD5").ToUpper(CultureInfo.InvariantCulture);
            StringBuilder builder = new StringBuilder();
            builder.Append(this.CreateField("v_mid", this.v_mid));
            builder.Append(this.CreateField("v_oid", this.v_oid));
            builder.Append(this.CreateField("v_amount", this.v_amount));
            builder.Append(this.CreateField("v_moneytype", this.v_moneytype));
            builder.Append(this.CreateField("v_url", this.v_url));
            builder.Append(this.CreateField("remark1", this.remark1));
            builder.Append(this.CreateField("v_md5info", strValue));
            this.SubmitPaymentForm(this.CreateForm(builder.ToString(), this.gateway));
        }
    }
}

