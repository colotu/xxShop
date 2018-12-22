using YSWL.Payment.Core;
using YSWL.Payment.Model;

namespace YSWL.Payment.PaymentInterface.Chinabank
{
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web;
    using System.Web.Security;

    internal class ChinabankNotify : NotifyQuery
    {
        private NameValueCollection parameters;

        public ChinabankNotify(NameValueCollection parameters)
        {
            this.parameters = parameters;
        }

        public override decimal GetOrderAmount()
        {
            return decimal.Parse(this.parameters["v_amount"]);
        }

        public override string GetOrderId()
        {
            return this.parameters["v_oid"];
        }

        public override void VerifyNotify(int timeout, PayeeInfo payee, GatewayInfo gateway)
        {
            string str = this.parameters["v_oid"];
            string str2 = this.parameters["v_pstatus"];
            string str3 = this.parameters["v_pstring"];
            string str4 = this.parameters["v_pmode"];
            string str5 = this.parameters["v_md5str"];
            string str6 = this.parameters["v_amount"];
            string str7 = this.parameters["v_moneytype"];
            string str8 = this.parameters["remark1"];
            if ((((str == null) || (str2 == null)) || ((str3 == null) || (str4 == null))) || (((str5 == null) || (str6 == null)) || ((str8 == null) || (str7 == null))))
            {
                this.OnNotifyVerifyFaild();
            }
            else if (!str2.Equals("20"))
            {
                this.OnNotifyVerifyFaild();
            }
            else if (!str5.Equals(FormsAuthentication.HashPasswordForStoringInConfigFile(str + str2 + str6 + str7 + payee.PrimaryKey, "MD5").ToUpper(CultureInfo.InvariantCulture)))
            {
                this.OnNotifyVerifyFaild();
            }
            else
            {
                this.OnPaidToMerchant();
            }
        }

        public override void WriteBack(HttpContext context, bool success)
        {
            if (context != null)
            {
                context.Response.Clear();
                context.Response.Write(success ? "ok" : "error");
                context.Response.End();
            }
        }
    }
}

