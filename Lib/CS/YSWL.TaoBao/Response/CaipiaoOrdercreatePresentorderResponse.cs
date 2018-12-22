using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// CaipiaoOrdercreatePresentorderResponse.
    /// </summary>
    public class CaipiaoOrdercreatePresentorderResponse : TopResponse
    {
        /// <summary>
        /// 是否是赠送
        /// </summary>
        [XmlElement("is_present")]
        public string IsPresent { get; set; }

        /// <summary>
        /// 彩期编号
        /// </summary>
        [XmlElement("issue_id")]
        public string IssueId { get; set; }

        /// <summary>
        /// 彩种编号
        /// </summary>
        [XmlElement("lottery_type_id")]
        public string LotteryTypeId { get; set; }

        /// <summary>
        /// 问题错误码
        /// </summary>
        [XmlElement("msg_code")]
        public string MsgCode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        [XmlElement("msg_info")]
        public string MsgInfo { get; set; }

        /// <summary>
        /// 返回订单编号
        /// </summary>
        [XmlElement("order_id")]
        public string OrderId { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        [XmlElement("out_order_type")]
        public string OutOrderType { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        [XmlElement("pay_type")]
        public string PayType { get; set; }

        /// <summary>
        /// 赠送编号
        /// </summary>
        [XmlElement("present_order_id")]
        public string PresentOrderId { get; set; }

        /// <summary>
        /// 是否执行成功
        /// </summary>
        [XmlElement("status")]
        public bool Status { get; set; }

        /// <summary>
        /// 交易编号
        /// </summary>
        [XmlElement("trade_number")]
        public string TradeNumber { get; set; }
    }
}
