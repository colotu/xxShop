using System;
using System.Collections.Generic;
using YSWL.TaoBao.Response;
using YSWL.TaoBao.Util;

namespace YSWL.TaoBao.Request
{
    /// <summary>
    /// TOP API: taobao.logistics.trace.search
    /// </summary>
    public class LogisticsTraceSearchRequest : ITopRequest<LogisticsTraceSearchResponse>
    {
        /// <summary>
        /// 卖家昵称
        /// </summary>
        public string SellerNick { get; set; }

        /// <summary>
        /// 淘宝交易号，请勿传非淘宝交易号
        /// </summary>
        public Nullable<long> Tid { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "taobao.logistics.trace.search";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("seller_nick", this.SellerNick);
            parameters.Add("tid", this.Tid);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
            RequestValidator.ValidateRequired("seller_nick", this.SellerNick);
            RequestValidator.ValidateRequired("tid", this.Tid);
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
