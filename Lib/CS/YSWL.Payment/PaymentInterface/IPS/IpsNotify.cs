using YSWL.Payment.Core;
using YSWL.Payment.Model;

namespace YSWL.Payment.PaymentInterface.IPS
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using System.Web.Security;
    using System.Xml;

    internal class IpsNotify : NotifyQuery
    {
       // private NameValueCollection parameters;
        private string paymentResult;

        public IpsNotify(NameValueCollection parameters)
        {
            //this.parameters = parameters;
            this.paymentResult = parameters["paymentResult"];
        }

        public override decimal GetOrderAmount()
        {
            return decimal.Parse(
                XmlStrUtil.GetXmlValue(paymentResult, "Ips/GateWayRsp/body/Amount ")
                );
        }

        public override string GetOrderId()
        {
            return   XmlStrUtil.GetXmlValue(paymentResult, "Ips/GateWayRsp/body/MerBillNo") ;
            
        }

        public override void VerifyNotify(int timeout, PayeeInfo payee, GatewayInfo gateway)
        {
            string rspCode = XmlStrUtil.GetXmlValue(paymentResult, "Ips/GateWayRsp/head/RspCode");
            string retEncodeType = XmlStrUtil.GetXmlValue(paymentResult, "Ips/GateWayRsp/body/RetEncodeType");


            //验证判断交易是否成功
            if (rspCode == "000000")
            {
                //Md5的摘要认证方式 
                if (retEncodeType == "17" && verifySign(paymentResult, payee))
                {
                    string result = XmlStrUtil.GetXmlValue(paymentResult, "Ips/GateWayRsp/body/Status");
                    if (result == "Y")
                    {
                        //1、为安全和验证数据的真实性，建议商户做如下操作：

                        //a、将自己数据库中存放的订单信息和IPS返回的订单信息进行比对，其对比项目有订单编号、订单金额和订单日期，以便做相应的交易防重处理
                        this.OnPaidToMerchant();

                    }
                }
                else
                {
                    this.OnNotifyVerifyFaild();
                }
            }
            else
            {
                this.OnNotifyVerifyFaild();
            }
        }

        /// <summary>
        /// 验签
        /// </summary>
        /// <param name="rspXml">请求返回报文</param>
        /// <returns>验签结果</returns>
        private bool verifySign(string rspXml, PayeeInfo payee)
        {
            try
            {
                StringBuilder sbReturnXML = new StringBuilder();
                sbReturnXML.Append(rspXml);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sbReturnXML.ToString());

                string strSignature = XmlStrUtil.GetXmlValue(rspXml, "Ips/GateWayRsp/head/Signature");
                XmlNode xnNodes = doc.DocumentElement.SelectSingleNode("GateWayRsp/body");

                string strBody = "<body>" + xnNodes.InnerXml + "</body>";

                string strContent = strBody + payee.SellerAccount+ payee.PrimaryKey;
                //验签
                bool result = IpsVerify.IsMD5Sign(strSignature, strContent);
                if (!result)
                {
                    //验签失败
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
               // Log.LogHelper.AddErrorLog("验证异常", ex.Message + "-----" + ex.StackTrace);
            }
            return true;
        }

        public override void WriteBack(HttpContext context, bool success)
        {
        }
    }
}

