using System;
using System.Collections.Generic;
using YSWL.TaoBao.Response;
using YSWL.TaoBao.Util;

namespace YSWL.TaoBao.Request
{
    /// <summary>
    /// TOP API: taobao.caipiao.issue.get
    /// </summary>
    public class CaipiaoIssueGetRequest : ITopRequest<CaipiaoIssueGetResponse>
    {
        /// <summary>
        /// 彩种的id
        /// </summary>
        public string LotteryTypeIds { get; set; }

        /// <summary>
        /// 渠道编号
        /// </summary>
        public string Ttid { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "taobao.caipiao.issue.get";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("lottery_type_ids", this.LotteryTypeIds);
            parameters.Add("ttid", this.Ttid);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
            RequestValidator.ValidateRequired("lottery_type_ids", this.LotteryTypeIds);
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
