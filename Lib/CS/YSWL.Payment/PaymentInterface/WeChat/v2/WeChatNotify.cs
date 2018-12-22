using YSWL.Payment.Core;
using YSWL.Payment.Model;

namespace YSWL.Payment.PaymentInterface.WeChat
{
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web;
    using System.Xml;
    using System.IO;
    using System.Net;
    using System.Text;
    using System;

    public class WeChatNotify : NotifyQuery
    {
        private NameValueCollection parameters;

        public WeChatNotify(NameValueCollection parameters)
        {
            this.parameters = parameters;
        }

        public override decimal GetOrderAmount()
        {
            return (decimal.Parse(this.parameters["total_fee"], CultureInfo.InvariantCulture) / 100M);
        }

        public override string GetOrderId()
        {
            return this.parameters["out_trade_no"];
        }

        protected string key2 = "";

        public override void VerifyNotify(int timeout, PayeeInfo payee, GatewayInfo gateway)
        {
            this.parameters["key"] = payee.PrimaryKey;
            this.key2 = payee.SecondKey;

            string notify_id = this.parameters["notify_id"];//通知id
            string partner = this.parameters["partner"];//商户号
            string trade_state = this.parameters["trade_state"];//支付结果
            string transaction_id = this.parameters["transaction_id"];//财付通订单号
            string total_fee = this.parameters["total_fee"];//金额,以分为单位
            string out_trade_no = this.parameters["out_trade_no"];//商户订单号
            string trade_mode = this.parameters["trade_mode"];//交易模式，1即时到账，2中介担保
            //string attach = this.parameters["attach"];


            if ((((notify_id == null) || (partner == null) || (trade_state == null)) || ((transaction_id == null) ||
                (total_fee == null))) || (((out_trade_no == null) || (trade_mode == null))))
            {
                this.OnNotifyVerifyFaild();

            }
            else if (!trade_mode.Equals("1") || !trade_state.Equals("0"))
            {
                this.OnNotifyVerifyFaild();
            }
            else
            {
                if (!Globals.isTenpaySign(this.parameters, "utf-8"))
                {
                    this.OnNotifyVerifyFaild();
                }
                else
                {
                    try
                    {
                        this.AutoDeliverNotify();
                    }
                    catch (Exception)
                    {
                        //Core.Globals.WriteText(new System.Text.StringBuilder(ex.Message + "|" + ex.StackTrace));
                    }
                    this.OnPaidToMerchant();
                }
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

        /// <summary>
        /// 自动发货通知
        /// </summary>
        protected virtual void AutoDeliverNotify()
        {

        }

        protected virtual string SetPayHelperBase(Utils.WxPayHelper wxPayHelper)
        {
            NameValueCollection postData = LoadPostData();
            if (postData == null) return string.Empty;

            if (string.IsNullOrWhiteSpace(postData["openid"]) ||
                string.IsNullOrWhiteSpace(parameters["transaction_id"]) ||
                string.IsNullOrWhiteSpace(parameters["out_trade_no"]))
            {
                return string.Empty;
            }

            System.Collections.Generic.Dictionary<string, string> bizObj = new System.Collections.Generic.Dictionary<string, string>();

            bizObj.Add("openid", postData["openid"]);
            bizObj.Add("transid", parameters["transaction_id"]);
            bizObj.Add("out_trade_no", parameters["out_trade_no"]);

            bizObj.Add("deliver_status", "1");
            bizObj.Add("deliver_msg", "OK");

            //先设置基本信息
            wxPayHelper.SetAppId(postData["appid"]);
            wxPayHelper.SetAppKey(this.key2);
            wxPayHelper.SetSignType("SHA1");

            string tmp = wxPayHelper.CreateDeliverNotifyXml(bizObj);
            //Core.Globals.WriteText(new System.Text.StringBuilder(tmp));
            return tmp;
        }

        protected static NameValueCollection LoadPostData()
        {
            NameValueCollection postData = new NameValueCollection();
            if (HttpContext.Current.Request.InputStream.Length < 1) return null;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Request.InputStream);
            XmlNode root = xmlDoc.SelectSingleNode("xml");
            if (root == null) return null;
            XmlNodeList xnl = root.ChildNodes;

            foreach (XmlNode xnf in xnl)
            {
                //StringBuilder sb = new System.Text.StringBuilder();
                //Core.Globals.WriteText(sb.AppendFormat("{0}:{1},", xnf.Name, xnf.InnerText));
                postData.Add(xnf.Name, xnf.InnerText);
            }
            return postData;
        }

        protected string GetResponse(string url, string data, int? timeout = null)
        {
            string str;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                if (timeout.HasValue)
                {
                    request.Timeout = timeout.Value;
                }

                if (!string.IsNullOrWhiteSpace(data))
                {
                    request.Method = "POST";
                    //request.ServicePoint.Expect100Continue = false;
                    request.ContentType = "application/x-www-form-urlencoded";
                    using (StreamWriter requestWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        requestWriter.Write(data);
                    }
                }

                using (Stream responseStream = ((HttpWebResponse)request.GetResponse()).GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    StringBuilder builder = new StringBuilder();
                    while (-1 != reader.Peek())
                    {
                        builder.Append(reader.ReadLine());
                    }
                    str = builder.ToString();
                    reader.Close();
                    responseStream.Close();
                }
            }
            catch (Exception exception)
            {
                str = "Error:" + exception.Message;
            }
            return str;
        }
    }
}

