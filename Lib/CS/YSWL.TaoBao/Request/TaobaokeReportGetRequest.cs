using System;
using System.Collections.Generic;
using YSWL.TaoBao.Response;
using YSWL.TaoBao.Util;

namespace YSWL.TaoBao.Request
{
    /// <summary>
    /// TOP API: taobao.taobaoke.report.get
    /// </summary>
    public class TaobaokeReportGetRequest : ITopRequest<TaobaokeReportGetResponse>
    {
        /// <summary>
        /// 需要查询报表的日期，有效的日期为最近3个月内的某一天，格式为:yyyyMMdd,如20090520.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 需返回的字段列表.可选值:TaobaokeReportMember淘宝客报表成员结构体中的所有字段;字段之间用","分隔.
        /// </summary>
        public string Fields { get; set; }

        /// <summary>
        /// 当前页数.只能获取1-499页数据.
        /// </summary>
        public Nullable<long> PageNo { get; set; }

        /// <summary>
        /// 每页返回结果数,默认是40条.最大每页100
        /// </summary>
        public Nullable<long> PageSize { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "taobao.taobaoke.report.get";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("date", this.Date);
            parameters.Add("fields", this.Fields);
            parameters.Add("page_no", this.PageNo);
            parameters.Add("page_size", this.PageSize);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
            RequestValidator.ValidateRequired("date", this.Date);
            RequestValidator.ValidateRequired("fields", this.Fields);
            RequestValidator.ValidateMaxValue("page_no", this.PageNo, 499);
            RequestValidator.ValidateMinValue("page_no", this.PageNo, 1);
            RequestValidator.ValidateMaxValue("page_size", this.PageSize, 100);
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
