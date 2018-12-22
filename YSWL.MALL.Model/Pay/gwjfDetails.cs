/**  版本信息模板在安装目录下，可自行修改。
* BalanceDetails.cs
*
* 功 能： N/A
* 类 名： BalanceDetails
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/9/3 14:45:12   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace YSWL.MALL.Model.Pay
{
    /// <summary>
    /// BalanceDetails:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class GwjfDetails
    {
        public GwjfDetails()
        { }
        #region Model
        private long _gwjfid;
        private int? _userid;
        private DateTime? _tradedate;
        private int? _tradetype;
        private decimal? _income;
        private string _expenses;
        private decimal? _gwjf;
        private string _remarkc;
        private string _remarkctwo;
        private string _remarkcthree;
        /// <summary>
        /// 
        /// </summary>
        public long gwjfid
        {
            set { _gwjfid = value; }
            get { return _gwjfid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? UserId
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? TradeDate
        {
            set { _tradedate = value; }
            get { return _tradedate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? TradeType
        {
            set { _tradetype = value; }
            get { return _tradetype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Income
        {
            set { _income = value; }
            get { return _income; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Expenses
        {
            set { _expenses = value; }
            get { return _expenses; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? gwjf
        {
            set { _gwjf = value; }
            get { return _gwjf; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string remarkC
        {
            set { _remarkc = value; }
            get { return _remarkc; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string remarkCtwo
        {
            set { _remarkctwo = value; }
            get { return _remarkctwo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string remarkCthree
        {
            set { _remarkcthree = value; }
            get { return _remarkcthree; }
        }
        #endregion Model

    }
}


