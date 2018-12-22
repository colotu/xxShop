using System;
using System.Collections.Generic;
using YSWL.TaoBao.Response;
using YSWL.TaoBao.Util;

namespace YSWL.TaoBao.Request
{
    /// <summary>
    /// TOP API: taobao.hotel.type.add
    /// </summary>
    public class HotelTypeAddRequest : ITopRequest<HotelTypeAddResponse>
    {
        /// <summary>
        /// 酒店id。必须为数字
        /// </summary>
        public Nullable<long> Hid { get; set; }

        /// <summary>
        /// 房型名称。长度不能超过30
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 接入卖家数据主键
        /// </summary>
        public string SiteParam { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "taobao.hotel.type.add";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("hid", this.Hid);
            parameters.Add("name", this.Name);
            parameters.Add("site_param", this.SiteParam);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
            RequestValidator.ValidateRequired("hid", this.Hid);
            RequestValidator.ValidateMinValue("hid", this.Hid, 0);
            RequestValidator.ValidateRequired("name", this.Name);
            RequestValidator.ValidateMaxLength("name", this.Name, 30);
            RequestValidator.ValidateMaxLength("site_param", this.SiteParam, 100);
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
