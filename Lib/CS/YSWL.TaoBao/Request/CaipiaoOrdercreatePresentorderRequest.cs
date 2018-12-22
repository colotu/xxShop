using System;
using System.Collections.Generic;
using YSWL.TaoBao.Response;
using YSWL.TaoBao.Util;

namespace YSWL.TaoBao.Request
{
    /// <summary>
    /// TOP API: taobao.caipiao.ordercreate.presentorder
    /// </summary>
    public class CaipiaoOrdercreatePresentorderRequest : ITopRequest<CaipiaoOrdercreatePresentorderResponse>
    {
        /// <summary>
        /// 大乐购追加
        /// </summary>
        public Nullable<long> DltPursue { get; set; }

        /// <summary>
        /// 费用
        /// </summary>
        public string Fees { get; set; }

        /// <summary>
        /// 彩期
        /// </summary>
        public string Issue { get; set; }

        /// <summary>
        /// 彩种编号
        /// </summary>
        public Nullable<long> LotteryTypeId { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobiles { get; set; }

        /// <summary>
        /// 倍数
        /// </summary>
        public string Multis { get; set; }

        /// <summary>
        /// 选号
        /// </summary>
        public string Numbers { get; set; }

        /// <summary>
        /// 来源 wap和客户端都是1，只有pc是0
        /// </summary>
        public Nullable<long> RewardId { get; set; }

        /// <summary>
        /// 电商id
        /// </summary>
        public Nullable<long> ShopId { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public string Stakes { get; set; }

        /// <summary>
        /// 赠送总金额
        /// </summary>
        public Nullable<long> TotalFee { get; set; }

        /// <summary>
        /// 渠道编号
        /// </summary>
        public string Ttid { get; set; }

        /// <summary>
        /// 赠送语
        /// </summary>
        public string Words { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "taobao.caipiao.ordercreate.presentorder";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("dlt_pursue", this.DltPursue);
            parameters.Add("fees", this.Fees);
            parameters.Add("issue", this.Issue);
            parameters.Add("lottery_type_id", this.LotteryTypeId);
            parameters.Add("mobiles", this.Mobiles);
            parameters.Add("multis", this.Multis);
            parameters.Add("numbers", this.Numbers);
            parameters.Add("reward_id", this.RewardId);
            parameters.Add("shop_id", this.ShopId);
            parameters.Add("stakes", this.Stakes);
            parameters.Add("total_fee", this.TotalFee);
            parameters.Add("ttid", this.Ttid);
            parameters.Add("words", this.Words);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
            RequestValidator.ValidateRequired("dlt_pursue", this.DltPursue);
            RequestValidator.ValidateRequired("fees", this.Fees);
            RequestValidator.ValidateRequired("issue", this.Issue);
            RequestValidator.ValidateRequired("lottery_type_id", this.LotteryTypeId);
            RequestValidator.ValidateRequired("mobiles", this.Mobiles);
            RequestValidator.ValidateRequired("multis", this.Multis);
            RequestValidator.ValidateRequired("numbers", this.Numbers);
            RequestValidator.ValidateRequired("reward_id", this.RewardId);
            RequestValidator.ValidateRequired("shop_id", this.ShopId);
            RequestValidator.ValidateRequired("stakes", this.Stakes);
            RequestValidator.ValidateRequired("total_fee", this.TotalFee);
            RequestValidator.ValidateRequired("ttid", this.Ttid);
            RequestValidator.ValidateRequired("words", this.Words);
        }

        #endregion

        public void AddOtherParameter(string key, string value)
        {
            if (this.otherParameters == null)
            {
                this.otherParameters = new TopDictionary();
            }
            this.otherParameters.Add(key, value);
        }
    }
}
