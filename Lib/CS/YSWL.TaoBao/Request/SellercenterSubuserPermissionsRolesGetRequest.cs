using System;
using System.Collections.Generic;
using YSWL.TaoBao.Response;
using YSWL.TaoBao.Util;

namespace YSWL.TaoBao.Request
{
    /// <summary>
    /// TOP API: taobao.sellercenter.subuser.permissions.roles.get
    /// </summary>
    public class SellercenterSubuserPermissionsRolesGetRequest : ITopRequest<SellercenterSubuserPermissionsRolesGetResponse>
    {
        /// <summary>
        /// 子账号昵称(子账号标识)
        /// </summary>
        public string Nick { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "taobao.sellercenter.subuser.permissions.roles.get";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("nick", this.Nick);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
            RequestValidator.ValidateRequired("nick", this.Nick);
            RequestValidator.ValidateMaxLength("nick", this.Nick, 100);
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
