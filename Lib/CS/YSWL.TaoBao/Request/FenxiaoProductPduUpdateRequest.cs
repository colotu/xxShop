using System;
using System.Collections.Generic;
using YSWL.TaoBao.Response;
using YSWL.TaoBao.Util;

namespace YSWL.TaoBao.Request
{
    /// <summary>
    /// TOP API: taobao.fenxiao.product.pdu.update
    /// </summary>
    public class FenxiaoProductPduUpdateRequest : ITopRequest<FenxiaoProductPduUpdateResponse>
    {
        /// <summary>
        /// 分销商ID
        /// </summary>
        public Nullable<long> DistributorId { get; set; }

        /// <summary>
        /// 是否删除，删除指定分销商的数据
        /// </summary>
        public Nullable<bool> IsDelete { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public Nullable<long> ProductId { get; set; }

        /// <summary>
        /// 库存是追加还是覆盖；删除操作可不传  append - 追加、overwrite - 覆盖
        /// </summary>
        public string QuantityType { get; set; }

        /// <summary>
        /// 0-999999的整数，可传入多个，以逗号隔开，顺序与属性列表保持一致；删除操作可不传
        /// </summary>
        public string Quantitys { get; set; }

        /// <summary>
        /// 产品包含sku时必选，可传入多个，以逗号隔开；删除操作可不传
        /// </summary>
        public string SkuProperties { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "taobao.fenxiao.product.pdu.update";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("distributor_id", this.DistributorId);
            parameters.Add("is_delete", this.IsDelete);
            parameters.Add("product_id", this.ProductId);
            parameters.Add("quantity_type", this.QuantityType);
            parameters.Add("quantitys", this.Quantitys);
            parameters.Add("sku_properties", this.SkuProperties);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
            RequestValidator.ValidateRequired("distributor_id", this.DistributorId);
            RequestValidator.ValidateRequired("product_id", this.ProductId);
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
