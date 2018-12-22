using System;
using System.Collections.Generic;
using YSWL.TaoBao.Response;
using YSWL.TaoBao.Util;

namespace YSWL.TaoBao.Request
{
    /// <summary>
    /// TOP API: taobao.item.get
    /// </summary>
    public class ItemGetRequest : ITopRequest<ItemGetResponse>
    {
        /// <summary>
        /// 需要返回的商品对象字段。可选值：Item商品结构体中所有字段均可返回；多个字段用“,”分隔。如果想返回整个子对象，那字段为item_img，如果是想返回子对象里面的字段，那字段为item_img.url。新增返回字段：second_kill（是否秒杀商品）、auto_fill（代充商品类型）,props_name（商品属性名称）
        /// </summary>
        public string Fields { get; set; }

        /// <summary>
        /// 商品数字ID
        /// </summary>
        public Nullable<long> NumIid { get; set; }

        /// <summary>
        /// 商品数字ID(带有跟踪效果)
        /// </summary>
        public string TrackIid { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "taobao.item.get";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("fields", this.Fields);
            parameters.Add("num_iid", this.NumIid);
            parameters.Add("track_iid", this.TrackIid);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
            RequestValidator.ValidateRequired("fields", this.Fields);
            RequestValidator.ValidateMinValue("num_iid", this.NumIid, 1);
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
