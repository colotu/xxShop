using System;
using System.Collections.Generic;
using YSWL.TaoBao.Response;
using YSWL.TaoBao.Util;

namespace YSWL.TaoBao.Request
{
    /// <summary>
    /// TOP API: tmall.product.spec.pic.upload
    /// </summary>
    public class TmallProductSpecPicUploadRequest : ITopUploadRequest<TmallProductSpecPicUploadResponse>
    {
        /// <summary>
        /// 上传的认证图片文件
        /// </summary>
        public FileItem CertifyPic { get; set; }

        /// <summary>
        /// 上传的认证图片的认证类型
        /// </summary>
        public Nullable<long> CertifyType { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "tmall.product.spec.pic.upload";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("certify_type", this.CertifyType);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
            RequestValidator.ValidateRequired("certify_pic", this.CertifyPic);
            RequestValidator.ValidateRequired("certify_type", this.CertifyType);
        }

        #endregion

        #region ITopUploadRequest Members

        public IDictionary<string, FileItem> GetFileParameters()
        {
            IDictionary<string, FileItem> parameters = new Dictionary<string, FileItem>();
            parameters.Add("certify_pic", this.CertifyPic);
            return parameters;
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
