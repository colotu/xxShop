using System;
using System.Collections.Generic;
using YSWL.TaoBao.Response;
using YSWL.TaoBao.Util;

namespace YSWL.TaoBao.Request
{
    /// <summary>
    /// TOP API: taobao.trip.jipiao.agent.order.get
    /// </summary>
    public class TripJipiaoAgentOrderGetRequest : ITopRequest<TripJipiaoAgentOrderGetResponse>
    {
        /// <summary>
        /// 淘宝政策id列表，当前支持列表长度为1，即当前只支持单个订单详情查询
        /// </summary>
        public string OrderIds { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "taobao.trip.jipiao.agent.order.get";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("order_ids", this.OrderIds);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
            RequestValidator.ValidateRequired("order_ids", this.OrderIds);
            RequestValidator.ValidateMaxListSize("order_ids", this.OrderIds, 1);
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
