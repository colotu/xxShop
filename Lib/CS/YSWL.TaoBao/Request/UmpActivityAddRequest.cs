using System;
using System.Collections.Generic;
using YSWL.TaoBao.Response;
using YSWL.TaoBao.Util;

namespace YSWL.TaoBao.Request
{
    /// <summary>
    /// TOP API: taobao.ump.activity.add
    /// </summary>
    public class UmpActivityAddRequest : ITopRequest<UmpActivityAddResponse>
    {
        /// <summary>
        /// 活动内容，通过ump sdk里面的MarkeitngTool来生成
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 工具id
        /// </summary>
        public Nullable<long> ToolId { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "taobao.ump.activity.add";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("content", this.Content);
            parameters.Add("tool_id", this.ToolId);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
            RequestValidator.ValidateRequired("content", this.Content);
            RequestValidator.ValidateRequired("tool_id", this.ToolId);
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
