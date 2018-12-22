using YSWL.Payment.Core;
using YSWL.Payment.Model;

namespace YSWL.Payment.PaymentInterface.WeChat
{
    using System.Web;
    using System.Globalization;
    using System;
    using System.Text;
    using System.IO;

    internal class WeChatRequest : PaymentRequest
    {
        private string gatewayUrl = "https://gw.tenpay.com/gateway/pay.htm";
        private string sign_type = "MD5";
        private string input_charset = "UTF-8";
        private string sign_key_index = "1";
        private string service_version = "1.0";

        //支付参数
        private string partner;//商户号
        private string out_trade_no;// sp_billno);		//商家订单号
        private string total_fee;// (money * 100).ToString()); //商品金额,以分为单位
        private string return_url;//交易完成后跳转的URL
        private string notify_url;//接收财付通通知的URL
        private string body;//商品描述
        private string bank_type = "WX";  //银行类型 固定为 "WX"
        private string spbill_create_ip;  //用户的公网ip，不是商户服务器IP
        private string fee_type = "1";//币种，1人民币


        //业务可选参数
        private string attach = "WeChat";//附加数据，原样返回
        //private string product_fee = "0";//商品费用，必须保证transport_fee + product_fee=total_fee
        //private string transport_fee = "0";               //物流费用，必须保证transport_fee + product_fee=total_fee
        //private string time_start;//订单生成时间，格式为yyyyMMddHHmmss
        //private string time_expire;//订单失效时间，格式为yyyymmddhhmmss
        //private string buyer_id;//买方财付通账号
        //private string goods_tag;//商品标记
        private string trade_mode = "1";//交易模式，1即时到账(默认)，2中介担保，3后台选择（买家进支付中心列表选择）
        //private string transport_desc;              //物流说明
        //private string trans_type = "1";//交易类型，1实物交易，2虚拟交易
        //private string agentid;//平台ID
        //private string agent_type;//代理模式，0无代理(默认)，1表示卡易售模式，2表示网店模式
        //private string seller_id;//卖家商户号，为空则等同于partner

        private string key = "";
        private string key2 = "";
        private string token = "";
        private string appid = "";
        private string partnerId = "";

        private GatewayInfo getGateway;

        public WeChatRequest(PayeeInfo payee, GatewayInfo gateway, TradeInfo trade)
        {
            if (gateway.DataList == null || gateway.DataList.Count < 3)
            {
                throw new ArgumentNullException("GATEWAYINFO:DATALIST NO APPID");
            }
            this.appid = gateway.DataList[3];
            this.token = trade.Token;
            this.key = payee.PrimaryKey;
            this.partner = payee.SellerAccount;
            this.key2 = payee.SecondKey;
            //this.time_start = trade.Date.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            //this.body = Core.Globals.SubString(trade.Body, 62, "..");         //String(64)
            //this.body = trade.Subject;         //String(64)
            this.body = "微信支付";
            this.spbill_create_ip = getRealIp();
            this.out_trade_no = trade.OrderId;
            this.return_url = gateway.ReturnUrl;
            this.notify_url = gateway.NotifyUrl;
            this.total_fee = Convert.ToInt32((decimal)(trade.TotalMoney * 100M)).ToString(CultureInfo.InvariantCulture);

            this.getGateway = gateway;
        }

        private static string getRealIp()
        {
            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }

        public override void SendRequest()
        {
            string action = HttpContext.Current.Request.QueryString["action"];

            if (string.IsNullOrWhiteSpace(action))
            {
                action = "show";
                //HttpContext.Current.Response.Write("NO ACTION");
                //HttpContext.Current.Response.End();
                //return;
            }
            action = action.ToLower();

            Utils.WxPayHelper wxPayHelper = new Utils.WxPayHelper();
            //先设置基本信息
            wxPayHelper.SetAppId(this.appid);
            wxPayHelper.SetAppKey(this.key2);
            wxPayHelper.SetPartnerKey(this.key);
            wxPayHelper.SetSignType("SHA1");
            //设置请求package信息
            wxPayHelper.SetParameter("bank_type", bank_type);
            wxPayHelper.SetParameter("body", body); //商品描述
            wxPayHelper.SetParameter("partner", partner); //partner 商户号
            wxPayHelper.SetParameter("out_trade_no", out_trade_no); //商家订单号
            wxPayHelper.SetParameter("total_fee", total_fee); //商品金额,以分为单位
            wxPayHelper.SetParameter("fee_type", fee_type); //币种，1人民币
            wxPayHelper.SetParameter("notify_url", notify_url);
            wxPayHelper.SetParameter("spbill_create_ip", spbill_create_ip);//用户的公网ip，不是商户服务器IP
            wxPayHelper.SetParameter("input_charset", input_charset);

            //System.Console.Out.WriteLine("生成app支付package:");
            //System.Console.Out.WriteLine(wxPayHelper.CreateAppPackage("test"));
            //System.Console.Out.WriteLine("生成jsapi支付package:");
            //string jsApiPackage = wxPayHelper.CreateBizPackage();
            //System.Console.Out.WriteLine(jsApiPackage);
            //System.Console.Out.WriteLine("生成原生支付url:");
            //System.Console.Out.WriteLine(wxPayHelper.CreateNativeUrl("abc"));
            //System.Console.Out.WriteLine("生成原生支付package:");
            //System.Console.Out.WriteLine(wxPayHelper.CreateNativePackage("0", "ok"));

            //DONE: 输出JS进行网关交互
            string result;
            switch (action)
            {
                case "show":
                    Configuration.GatewayProvider provider = Configuration.PayConfiguration.GetConfig().Providers["wechat"] as Configuration.GatewayProvider;
                    if (provider != null)
                    {
                        this.RedirectToGateway(string.Format(CultureInfo.InvariantCulture,
                            HttpContext.Current.Server.HtmlDecode(provider.Attributes["urlFormat"]),
                            this.out_trade_no, this.getGateway.Data));
                        return;
                    }
                    result = "{\"STATUS\":\"ERROR\",\"DATA\":\"GATEWAY PROVIDER NOT FOND\"}";
                    break;
                case "bizpackage":
                    result = "{\"STATUS\":\"SUCCESS\",\"DATA\":" + wxPayHelper.CreateBizPackage() + "}";
                    break;
                default:
                    result = "{\"STATUS\":\"ERROR\",\"DATA\":\"NotImplemented\"}";
                    break;
            }

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(result);
        }

    }
}

