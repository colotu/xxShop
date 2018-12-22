using System;
using System.Collections.Generic;
using YSWL.TaoBao.Response;
using YSWL.TaoBao.Util;

namespace YSWL.TaoBao.Request
{
    /// <summary>
    /// TOP API: taobao.shop.update
    /// </summary>
    public class ShopUpdateRequest : ITopRequest<ShopUpdateResponse>
    {
        /// <summary>
        /// 店铺公告。不超过1024个字符
        /// </summary>
        public string Bulletin { get; set; }

        /// <summary>
        /// 店铺描述。10～2000个字符以内
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 店铺标题。不超过30个字符；过滤敏感词，如淘咖啡、阿里巴巴等。title, bulletin和desc至少必须传一个
        /// </summary>
        public string Title { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "taobao.shop.update";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("bulletin", this.Bulletin);
            parameters.Add("desc", this.Desc);
            parameters.Add("title", this.Title);
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
