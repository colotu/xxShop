using System;
using System.Collections.Generic;
using YSWL.TaoBao.Response;
using YSWL.TaoBao.Util;

namespace YSWL.TaoBao.Request
{
    /// <summary>
    /// TOP API: taobao.simba.adgroup.adgroupcatmatchs.get
    /// </summary>
    public class SimbaAdgroupAdgroupcatmatchsGetRequest : ITopRequest<SimbaAdgroupAdgroupcatmatchsGetResponse>
    {
        /// <summary>
        /// 推广组Id列表
        /// </summary>
        public string AdgroupIds { get; set; }

        /// <summary>
        /// 主人昵称
        /// </summary>
        public string Nick { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "taobao.simba.adgroup.adgroupcatmatchs.get";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("adgroup_ids", this.AdgroupIds);
            parameters.Add("nick", this.Nick);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
            RequestValidator.ValidateRequired("adgroup_ids", this.AdgroupIds);
            RequestValidator.ValidateMaxListSize("adgroup_ids", this.AdgroupIds, 200);
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
