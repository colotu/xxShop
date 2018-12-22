using YSWL.Payment.Core;
using YSWL.Payment.Model;

namespace YSWL.Payment.PaymentInterface.IPS
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using System.Web.Security;

    internal class IpsRequest : PaymentRequest
    {
        private string Amount = "";
        private string Attach = "IPS";
        private string Billno = "";
        private string MER_CERT = "";
        private string Currency_Type = "156";
        private string Date = "";
        private string Gateway_Type = "01";
        private string Mer_Code = "";
        private string Account = "";
        private string Merchanturl = "";
        private string OrderEncodeType = "5";
        private string PostUrl = "https://newpay.ips.com.cn/psfp-entry/gateway/payment.do";
        private string RetEncodeType = "17";
        private string Rettype = "1";
        private string Lang = "GB";
        private string FailUrl = "";

        /// <summary>
        /// 请求报文头信息
        /// </summary>
        public Head objHead = null;
        /// <summary>
        /// 请求报文体信息
        /// </summary>
        public Body objBody = null;

        public IpsRequest(PayeeInfo payee, GatewayInfo gateway, TradeInfo trade)
        {
            objHead = new Head();
            objHead.Version = "v1.0.0";
            objHead.MsgId = "";
         
            objHead.ReqDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            objHead.MerCode = payee.SellerAccount;
            objHead.Account = payee.EmailAddress;
            objHead.MerName = "";

            this.MER_CERT = payee.PrimaryKey;

            objBody = new Body();
            objBody.MerBillno = trade.OrderId;
            objBody.GatewayType = this.Gateway_Type;
            objBody.Date = Convert.ToDateTime(trade.Date).ToString("yyyyMMdd", CultureInfo.InvariantCulture); ;
            objBody.CurrencyType = this.Currency_Type;
            objBody.Amount = trade.TotalMoney.ToString("F", CultureInfo.InvariantCulture);
            objBody.Lang = this.Lang;
            objBody.Merchanturl = gateway.ReturnUrl;
            objBody.FailUrl = this.FailUrl;
            objBody.Attach = this.Attach;
            objBody.OrderEncodeType = this.OrderEncodeType;
            objBody.RetEncodeType = this.RetEncodeType;
            objBody.RetType = this.Rettype;
            objBody.ServerUrl = gateway.ReturnUrl;
            objBody.BillEXP = "";
            objBody.GoodsName = trade.OrderId;
            objBody.IsCredit ="";
            objBody.BankCode = "";
            objBody.ProductType = "";

            //this.Mer_Code = payee.SellerAccount;
          
            //this.Merchanturl = gateway.ReturnUrl;
            //this.Billno = trade.OrderId;
            //this.Amount = trade.TotalMoney.ToString("F", CultureInfo.InvariantCulture);
       
        }

        public override void SendRequest()
        {
            //string strValue = FormsAuthentication.HashPasswordForStoringInConfigFile(this.Billno + this.Amount + this.Date + this.Currency_Type + this.Cert, "MD5").ToLower(CultureInfo.InvariantCulture);
            //StringBuilder builder = new StringBuilder();
            //builder.Append(this.CreateField("mer_code", this.Mer_code));
            //builder.Append(this.CreateField("Billno", this.Billno));
            //builder.Append(this.CreateField("Gateway_type", this.Gateway_Type));
            //builder.Append(this.CreateField("Currency_Type", this.Currency_Type));
            //builder.Append(this.CreateField("Amount", this.Amount));
            //builder.Append(this.CreateField("Date", this.Date));
            //builder.Append(this.CreateField("Merchanturl", this.Merchanturl));
            //builder.Append(this.CreateField("Attach", this.Attach));
            //builder.Append(this.CreateField("OrderEncodeType", this.OrderEncodeType));
            //builder.Append(this.CreateField("RetEncodeType", this.RetEncodeType));
            //builder.Append(this.CreateField("RetType", this.Rettype));
            //builder.Append(this.CreateField("SignMD5", strValue));
            string reqXml = GetReqXml();
            try
            {
                StringBuilder postForm = new StringBuilder();
                postForm.AppendFormat("<form id=\"payform\" name=\"payform\" action=\"{0}\" method=\"POST\">", PostUrl);
                postForm.AppendFormat("<input type=\"hidden\" name=\"pGateWayReq\" value=\"{0}\" />", reqXml);
                postForm.Append("<input type='submit' style='display: none'/>");
                postForm.Append("</form>");
                postForm.Append("  <script language=\"javascript\">document.forms['payform'].submit();</script>");
                HttpContext.Current.Response.Write(postForm.ToString());
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
           

            //this.SubmitPaymentForm(this.CreateForm(builder.ToString(), this.PostUrl));
        }


        #region 请求报文组装
        /// <summary>
        /// 获取请求报文信息
        /// </summary>
        /// <returns></returns>
        private string GetReqXml()
        {
            StringBuilder sbReturnXML = new StringBuilder();
            sbReturnXML.Append("<Ips>");
            sbReturnXML.Append("<GateWayReq>");
            sbReturnXML.Append(GetReqHead());
            sbReturnXML.Append(GetReqBody());
            sbReturnXML.Append("</GateWayReq>");
            sbReturnXML.Append("</Ips>");
            return sbReturnXML.ToString();
        }
        /// <summary>
        /// 请求报文头
        /// </summary>
        /// <returns></returns>
        private string GetReqHead()
        {
            //YSWL.Log.LogHelper.AddErrorLog("IPS","")
            StringBuilder sbReturnHead = new StringBuilder();
            sbReturnHead.Append("<head>");
            sbReturnHead.Append("<Version>").Append(objHead.Version).Append("</Version>");
            sbReturnHead.Append("<MerCode>").Append(objHead.MerCode).Append("</MerCode>");
            sbReturnHead.Append("<MerName>").Append(objHead.MerName).Append("</MerName>");
            sbReturnHead.Append("<Account>").Append(objHead.Account).Append("</Account>");
            sbReturnHead.Append("<MsgId>").Append(objHead.MsgId).Append("</MsgId>");
            sbReturnHead.Append("<ReqDate>").Append(objHead.ReqDate).Append("</ReqDate>");
            sbReturnHead.Append("<Signature>").Append(IpsVerify.MD5Sign(GetReqBody() + objHead.MerCode + MER_CERT)).Append("</Signature>");
            sbReturnHead.Append("</head>");
            return sbReturnHead.ToString();
        }
        /// <summary>
        /// 请求报文体
        /// </summary>
        /// <returns></returns>
        private string GetReqBody()
        {
            StringBuilder sbReturnBody = new StringBuilder();
            sbReturnBody.Append("<body>");
            sbReturnBody.Append("<MerBillNo>").Append(objBody.MerBillno).Append("</MerBillNo>");
            sbReturnBody.Append("<Amount>").Append(objBody.Amount).Append("</Amount>");
            sbReturnBody.Append("<Date>").Append(objBody.Date).Append("</Date>");
            sbReturnBody.Append("<CurrencyType>").Append(objBody.CurrencyType).Append("</CurrencyType>");
            sbReturnBody.Append("<GatewayType>").Append(objBody.GatewayType).Append("</GatewayType>");
            sbReturnBody.Append("<Lang>").Append(objBody.Lang).Append("</Lang>");
            sbReturnBody.Append("<Merchanturl><![CDATA[").Append(objBody.Merchanturl).Append("]]></Merchanturl>");
            sbReturnBody.Append("<FailUrl><![CDATA[").Append(objBody.FailUrl).Append("]]></FailUrl>");
            sbReturnBody.Append("<Attach><![CDATA[").Append(objBody.Attach).Append("]]></Attach>");
            sbReturnBody.Append("<OrderEncodeType>").Append(objBody.OrderEncodeType).Append("</OrderEncodeType>");
            sbReturnBody.Append("<RetEncodeType>").Append(objBody.RetEncodeType).Append("</RetEncodeType>");
            sbReturnBody.Append("<RetType>").Append(objBody.RetType).Append("</RetType>");
            sbReturnBody.Append("<ServerUrl><![CDATA[").Append(objBody.ServerUrl).Append("]]></ServerUrl>");
            sbReturnBody.Append("<BillEXP>").Append(objBody.BillEXP).Append("</BillEXP>");
            sbReturnBody.Append("<GoodsName>").Append(objBody.GoodsName).Append("</GoodsName>");
            sbReturnBody.Append("<IsCredit>").Append(objBody.IsCredit).Append("</IsCredit>");
            sbReturnBody.Append("<BankCode>").Append(objBody.BankCode).Append("</BankCode>");
            sbReturnBody.Append("<ProductType>").Append(objBody.ProductType).Append("</ProductType>");
            sbReturnBody.Append("</body>");
            return sbReturnBody.ToString();
        }

        #endregion
    }
}

