using System;
using System.Collections.Generic;
using YSWL.TaoBao.Response;
using YSWL.TaoBao.Util;

namespace YSWL.TaoBao.Request
{
    /// <summary>
    /// TOP API: taobao.simba.keywords.add
    /// </summary>
    public class SimbaKeywordsAddRequest : ITopRequest<SimbaKeywordsAddResponse>
    {
        /// <summary>
        /// 推广组id
        /// </summary>
        public Nullable<long> AdgroupId { get; set; }

        /// <summary>
        /// 关键词，出价字符串和匹配方式字符串数组，最多100个;每个字符串：word+  ”^^”+price+”^^”+matchscope,  Price是整数，以“分”为单位，不能小于5，不能大于日限额;   price为0则设置为使用默认出价；  matchscope只能是1,2,4（1代表精确匹配，2代表子串匹配，4代表广泛匹配）可不传。  关键词不能包含字符”^^”和 ”,”
        /// </summary>
        public string KeywordPrices { get; set; }

        /// <summary>
        /// 主人昵称
        /// </summary>
        public string Nick { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "taobao.simba.keywords.add";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("adgroup_id", this.AdgroupId);
            parameters.Add("keyword_prices", this.KeywordPrices);
            parameters.Add("nick", this.Nick);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
            RequestValidator.ValidateRequired("adgroup_id", this.AdgroupId);
            RequestValidator.ValidateRequired("keyword_prices", this.KeywordPrices);
            RequestValidator.ValidateMaxListSize("keyword_prices", this.KeywordPrices, 100);
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
