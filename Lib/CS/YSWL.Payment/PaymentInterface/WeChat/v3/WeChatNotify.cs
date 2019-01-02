using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Xml;
using YSWL.Payment.Core;
using YSWL.Payment.Model;
using YSWL.Payment.PaymentInterface.WeChat.v3.Models.UnifiedMessage;
using YSWL.Payment.PaymentInterface.WeChat.v3.Utils;
using YSWL.Payment.PaymentInterface.WeChat.v3.Utils.WXPay;

namespace YSWL.Payment.PaymentInterface.WeChat.v3
{
    public class WeChatNotify : NotifyQuery
    {
        private Dictionary<string, string> param = new Dictionary<string, string>();
        private string sign = string.Empty;
        private NotifyMessage message = null;
        System.Text.StringBuilder log = new System.Text.StringBuilder();

        public WeChatNotify(NameValueCollection parameters)
        {
            try
            {
                string xmlString = GetXmlString(HttpContext.Current.Request);
                //此处应记录日志
                //Core.Globals.WriteText(new System.Text.StringBuilder(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " xmlString:" + xmlString));
                //Core.Globals.WriteText(new System.Text.StringBuilder(HttpContext.Current.Request.Url.PathAndQuery));

                message = HttpClientHelper.XmlDeserialize<NotifyMessage>(xmlString);

                #region 解析数据
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);

                foreach (XmlNode node in doc.FirstChild.ChildNodes)
                {
                    if (node.Name.ToLower() != "sign")
                        param.Add(node.Name, node.InnerText);
                    else
                        sign = node.InnerText;
                }
                #endregion

            }
            catch (Exception ex)
            {
                //此处记录异常日志
                log.AppendFormat(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " YSWL.Payment.PaymentInterface.WeChat.v3.WeChatNotify 解析xml失败:" + ex.Message);
            }
            if (log.Length > 0)
            {
                Core.Globals.WriteText(log);
            }
        }

        public override decimal GetOrderAmount()
        {
            return message == null ? 0 : (message.Total_Fee / 100M);
        }

        public override string GetOrderId()
        {
            return message == null ? string.Empty : message.Out_Trade_No;
        }

        public override void VerifyNotify(int timeout, PayeeInfo payee, GatewayInfo gateway)
        {
            if (param.Count < 1 ||
                string.IsNullOrWhiteSpace(gateway.Data) ||
                gateway.DataList.Count < 4)
            {
                this.OnNotifyVerifyFaild();
                log.AppendFormat(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " YSWL.Payment.PaymentInterface.WeChat.v3.WeChatNotify CHECK GATEWAY DATA 验证失败:" + gateway.DataList.Count);
                Core.Globals.WriteText(log);
                return;
            }

            //if ((((notify_id == null) || (partner == null) || (trade_state == null)) || ((transaction_id == null) ||
            //    (total_fee == null))) || (((out_trade_no == null) || (trade_mode == null))))
            //{
            //    this.OnNotifyVerifyFaild();

            //}
            //else if (!trade_mode.Equals("1") || !trade_state.Equals("0"))
            //{
            //    this.OnNotifyVerifyFaild();
            //}

            try
            {
                UnifiedWxPayModel model = UnifiedWxPayModel.CreateUnifiedModel(gateway.DataList[3], payee.Partner, payee.PrimaryKey);
                if (!model.ValidateMD5Signature(param, sign))
                {
                    log.AppendFormat(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " YSWL.Payment.PaymentInterface.WeChat.v3.WeChatNotify.OnNotifyVerifyFaild 验证失败:" + param + "|sign:" + sign);
                    this.OnNotifyVerifyFaild();
                }
                else
                {
                    //处理通知
                    this.OnPaidToMerchant();
                }

            }
            catch (Exception ex)
            {
                //此处记录异常日志
                log.AppendFormat(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " YSWL.Payment.PaymentInterface.WeChat.v3.WeChatNotify.VerifyNotify 验证失败:" + ex.Message);
            }
            this.OnNotifyVerifyFaild();
            if (log.Length > 0)
            {
                Core.Globals.WriteText(log);
            }
        }

        public override void WriteBack(HttpContext context, bool success)
        {
            if (context != null)
            {
                context.Response.Clear();
                ReturnMessage returnMsg = new ReturnMessage() { Return_Code = "SUCCESS", Return_Msg = "" };
                if (!success)
                {
                    returnMsg.Return_Code = "FAIL";
                    returnMsg.Return_Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " YSWL.Payment.PaymentInterface.WeChat.v3.WeChatNotify.FAIL";
                }
                context.Response.Write(returnMsg.ToXmlString());
                context.Response.End();
            }
        }

        /// <summary>
        /// 获取Post Xml数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private string GetXmlString(HttpRequest request)
        {
            using (System.IO.Stream stream = request.InputStream)
            {
                Byte[] postBytes = new Byte[stream.Length];
                stream.Read(postBytes, 0, (Int32)stream.Length);
                return System.Text.Encoding.UTF8.GetString(postBytes);
            }
        }

    }
}

