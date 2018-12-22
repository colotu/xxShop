using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Domain
{
    /// <summary>
    /// LotteryWangcaiPresent Data Structure.
    /// </summary>
    [Serializable]
    public class LotteryWangcaiPresent : TopObject
    {
        /// <summary>
        /// 彩种名称
        /// </summary>
        [XmlElement("lottery_name")]
        public string LotteryName { get; set; }

        /// <summary>
        /// 赠送时间
        /// </summary>
        [XmlElement("present_date")]
        public string PresentDate { get; set; }

        /// <summary>
        /// 彩票注数
        /// </summary>
        [XmlElement("stake_count")]
        public long StakeCount { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        [XmlElement("user_nick")]
        public string UserNick { get; set; }

        /// <summary>
        /// 中奖金额，以分为单位
        /// </summary>
        [XmlElement("win_fee")]
        public long WinFee { get; set; }
    }
}
