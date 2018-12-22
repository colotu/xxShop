using System;
using System.Collections.Generic;
using YSWL.TaoBao.Response;
using YSWL.TaoBao.Util;

namespace YSWL.TaoBao.Request
{
    /// <summary>
    /// TOP API: taobao.crm.members.groups.batchdelete
    /// </summary>
    public class CrmMembersGroupsBatchdeleteRequest : ITopRequest<CrmMembersGroupsBatchdeleteResponse>
    {
        /// <summary>
        /// 买家的Id集合
        /// </summary>
        public string BuyerIds { get; set; }

        /// <summary>
        /// 会员需要删除的分组
        /// </summary>
        public string GroupIds { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "taobao.crm.members.groups.batchdelete";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("buyer_ids", this.BuyerIds);
            parameters.Add("group_ids", this.GroupIds);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
            RequestValidator.ValidateRequired("buyer_ids", this.BuyerIds);
            RequestValidator.ValidateMaxListSize("buyer_ids", this.BuyerIds, 10);
            RequestValidator.ValidateRequired("group_ids", this.GroupIds);
            RequestValidator.ValidateMaxListSize("group_ids", this.GroupIds, 10);
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
