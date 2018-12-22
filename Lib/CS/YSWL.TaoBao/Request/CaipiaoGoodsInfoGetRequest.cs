using System;
using System.Collections.Generic;
using YSWL.TaoBao.Response;
using YSWL.TaoBao.Util;

namespace YSWL.TaoBao.Request
{
    /// <summary>
    /// TOP API: taobao.caipiao.goods.info.get
    /// </summary>
    public class CaipiaoGoodsInfoGetRequest : ITopRequest<CaipiaoGoodsInfoGetResponse>
    {
        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "taobao.caipiao.goods.info.get";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
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
