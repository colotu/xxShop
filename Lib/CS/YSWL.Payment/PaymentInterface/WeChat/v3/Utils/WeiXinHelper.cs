using YSWL.Payment.PaymentInterface.WeChat.v3.Models.Message;
using YSWL.Payment.PaymentInterface.WeChat.v3.Models.UnifiedMessage;
using YSWL.Payment.PaymentInterface.WeChat.v3.Consts;
namespace YSWL.Payment.PaymentInterface.WeChat.v3.Utils
{
    public class WeiXinHelper
    {

        #region 生成预支付账单 V2 App支付

        ///// <summary>
        ///// 生成预支付订单
        ///// </summary>
        ///// <param name="postData">请求参数</param>
        ///// <returns></returns>
        //public static PrePayMessage PreWXPay(string postData)
        //{
        //    var accessToken = AccessToken.Instance;
        //    string url = string.Format(WeiXinConst.WeiXin_Pay_PrePayUrl, accessToken.Access_Token);
        //    PrePayMessage result = WeiXin.Lib.Core.Helper.HttpClientHelper.PostResponse<PrePayMessage>(url, postData);

        //    if (result.TokenExpired)
        //        return PreWXPayByNewAccessToken(postData);

        //    return result;
        //}

        //private static PrePayMessage PreWXPayByNewAccessToken(string postData)
        //{
        //    var accessToken = AccessToken.NewInstance;
        //    string url = string.Format(WeiXinConst.WeiXin_Pay_PrePayUrl, accessToken.Access_Token);
        //    PrePayMessage result = WeiXin.Lib.Core.Helper.HttpClientHelper.PostResponse<PrePayMessage>(url, postData);
        //    return result;
        //}

        #endregion

        #region V3 统一支付接口

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postData"></param>
        /// <param name="isApp"></param>
        /// <returns></returns>
        public static UnifiedPrePayMessage UnifiedPrePay(string postData)
        {
            string url = WeiXinConst.WeiXin_Pay_UnifiedPrePayUrl;
            var message = HttpClientHelper.PostXmlResponse<UnifiedPrePayMessage>(url, postData);
            return message;
        }

        #endregion

        #region V2&V3 订单查询

        #region V2 订单查询

        //public static OrderQueryMessage OrderQuery(string postData)
        //{
        //    var accessToken = AccessToken.Instance;
        //    string url = string.Format(WeiXinConst.WeiXin_Pay_OrderQueryUrl, accessToken.Access_Token);
        //    OrderQueryMessage result = WeiXin.Lib.Core.Helper.HttpClientHelper.PostResponse<OrderQueryMessage>(url, postData);
        //    if (result.TokenExpired)
        //    {
        //        return OrderQueryByNewAccessToken(postData);
        //    }
        //    return result;
        //}

        //private static OrderQueryMessage OrderQueryByNewAccessToken(string postData)
        //{
        //    var accessToken = AccessToken.NewInstance;
        //    string url = string.Format(WeiXinConst.WeiXin_Pay_OrderQueryUrl, accessToken.Access_Token);
        //    OrderQueryMessage result = WeiXin.Lib.Core.Helper.HttpClientHelper.PostResponse<OrderQueryMessage>(url, postData);
        //    return result;
        //}

        #endregion

        #region V3 订单查询

        /// <summary>
        /// V3 统一订单查询接口
        /// </summary>
        /// <param name="postXml"></param>
        /// <returns></returns>
        public static UnifiedOrderQueryMessage UnifiedOrderQuery(string postXml)
        {
            string url = WeiXinConst.WeiXin_Pay_UnifiedOrderQueryUrl;
            UnifiedOrderQueryMessage message = HttpClientHelper.PostXmlResponse<UnifiedOrderQueryMessage>(url, postXml);
            return message;
        }

        #endregion


        #endregion

        #region V2 发货通知

        ///// <summary>
        ///// 发货通知
        ///// </summary>
        ///// <param name="postData">请求数据</param>
        ///// <param name="isApp">是否为App</param>
        //public static void DeliverNotify(string postData, bool isApp)
        //{
        //    var accessToken = isApp ? AccessToken.Instance : AccessToken.Instance;
        //    string url = string.Format(WeiXinConst.WeiXin_Pay_DeliverNotifyUrl, accessToken.Access_Token);
        //    ErrorMessage msg = WeiXin.Lib.Core.Helper.HttpClientHelper.PostResponse<ErrorMessage>(url, postData);
        //    if (msg.TokenExpired)
        //    {
        //        DeliverNotifyByNewAccessToken(postData, isApp);
        //    }
        //}

        //private static void DeliverNotifyByNewAccessToken(string postData, bool isApp)
        //{
        //    var accessToken = isApp ? AccessToken.NewInstance : AccessToken.NewInstance;
        //    string url = string.Format(WeiXinConst.WeiXin_Pay_DeliverNotifyUrl, accessToken.Access_Token);
        //    WeiXin.Lib.Core.Helper.HttpClientHelper.PostResponse(url, postData);
        //}

        #endregion

        #region V3 申请退款

        ///// <summary>
        ///// 申请退款（V3接口）
        ///// </summary>
        ///// <param name="postData">请求参数</param>
        ///// <param name="certPath">证书路径</param>
        ///// <param name="certPwd">证书密码</param>
        //public static bool Refund(string postData, string certPath, string certPwd)
        //{
        //    string url = WeiXinConst.WeiXin_Pay_UnifiedOrderRefundUrl;
        //    RefundMessage message = WeiXin.Lib.Core.Helper.RefundHelper.PostXmlResponse<RefundMessage>(url, postData, certPath, certPwd);
        //    return message.Success;
        //}

        #endregion
    }
}
