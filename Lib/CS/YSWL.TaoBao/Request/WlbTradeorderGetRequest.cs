using System;
using System.Collections.Generic;
using YSWL.TaoBao.Response;
using YSWL.TaoBao.Util;

namespace YSWL.TaoBao.Request
{
    /// <summary>
    /// TOP API: taobao.wlb.tradeorder.get
    /// </summary>
    public class WlbTradeorderGetRequest : ITopRequest<WlbTradeorderGetResponse>
    {
        /// <summary>
        /// 指定交易类型的交易号
        /// </summary>
        public string TradeId { get; set; }

        /// <summary>
        /// 交易类型:  TAOBAO--淘宝交易  PAIPAI--拍拍交易  YOUA--有啊交易
        /// </summary>
        public string TradeType { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "taobao.wlb.tradeorder.get";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("trade_id", this.TradeId);
            parameters.Add("trade_type", this.TradeType);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
            RequestValidator.ValidateRequired("trade_id", this.TradeId);
            RequestValidator.ValidateRequired("trade_type", this.TradeType);
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
