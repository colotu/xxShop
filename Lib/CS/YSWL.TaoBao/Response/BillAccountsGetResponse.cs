using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// BillAccountsGetResponse.
    /// </summary>
    public class BillAccountsGetResponse : TopResponse
    {
        /// <summary>
        /// 返回的科目信息
        /// </summary>
        [XmlArray("accounts")]
        [XmlArrayItem("account")]
        public List<Account> Accounts { get; set; }

        /// <summary>
        /// 返回记录行数
        /// </summary>
        [XmlElement("total_results")]
        public long TotalResults { get; set; }
    }
}
