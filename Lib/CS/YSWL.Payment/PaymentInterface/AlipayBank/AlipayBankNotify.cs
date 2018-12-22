using System.Collections.Specialized;
using System.Globalization;
using System.Web;
using YSWL.Payment.Core;
using YSWL.Payment.Model;

namespace YSWL.Payment.PaymentInterface.AlipayBank
{
    internal class AlipayBankNotify : NotifyQuery
    {
        private string input_charset = "utf-8";
        private NameValueCollection parameters;

        public AlipayBankNotify(NameValueCollection parameters)
        {
            this.parameters = parameters;
        }

        private string CreateUrl(PayeeInfo payee)
        {
            return string.Format(CultureInfo.InvariantCulture, "https://mapi.alipay.com/gateway.do?service=notify_verify&partner={0}&notify_id={1}", new object[] { payee.SellerAccount, this.parameters["notify_id"] });
        }

        public override string GetGatewayOrderId()
        {
            return this.parameters["trade_no"];
        }

        public override decimal GetOrderAmount()
        {
            return decimal.Parse(this.parameters["total_fee"]);
        }

        public override string GetOrderId()
        {
            return this.parameters["out_trade_no"];
        }

        public override void VerifyNotify(int timeout, PayeeInfo payee, GatewayInfo gateway)
        {
            bool flag;
            try
            {
                flag = bool.Parse(this.GetResponse(this.CreateUrl(payee), timeout));
            }
            catch
            {
                flag = false;
            }
            //System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //sb.AppendFormat("flag={0}|", flag);
            foreach (string tmpParameter in Core.Globals.AlipayOtherParamKeys)
            {
                if (!string.IsNullOrEmpty(tmpParameter))
                {
                    this.parameters.Remove(tmpParameter);
                }
            }
            string[] strArray2 = Globals.BubbleSort(this.parameters.AllKeys);
            string s = "";
            for (int i = 0; i < strArray2.Length; i++)
            {
                if ((!string.IsNullOrEmpty(this.parameters[strArray2[i]]) && (strArray2[i] != "sign")) && (strArray2[i] != "sign_type"))
                {
                    if (i == (strArray2.Length - 1))
                    {
                        s = s + strArray2[i] + "=" + this.parameters[strArray2[i]];
                        //sb.AppendFormat("{0}={1}|", strArray2[i], this.parameters[strArray2[i]]);
                    }
                    else
                    {
                        s = s + strArray2[i] + "=" + this.parameters[strArray2[i]] + "&";
                        //sb.AppendFormat("{0}={1}&", strArray2[i], this.parameters[strArray2[i]]);
                    }
                }
            }
            s = s + payee.PrimaryKey;
            flag = flag && this.parameters["sign"].Equals(Globals.GetMD5(s, this.input_charset));
            string str2 = this.parameters["trade_status"];

            //sb.AppendFormat("|sign={0}|", this.parameters["sign"]);
            //sb.AppendFormat("|trade_status={0}|", this.parameters["trade_status"]);
            //Core.Globals.WriteText(sb);
            if (flag && ((str2 == "TRADE_SUCCESS") || (str2 == "TRADE_FINISHED")))
            {
                this.OnPaidToMerchant();
            }
            else
            {
                this.OnNotifyVerifyFaild();
            }
        }

        public override void WriteBack(HttpContext context, bool success)
        {
            if (context != null)
            {
                context.Response.Clear();
                context.Response.Write(success ? "success" : "fail");
                context.Response.End();
            }
        }
    }
}
