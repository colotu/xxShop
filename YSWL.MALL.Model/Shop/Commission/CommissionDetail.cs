/**  版本信息模板在安装目录下，可自行修改。
* CommissionDetail.cs
*
* 功 能： N/A
* 类 名： CommissionDetail
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/2/26 16:51:35   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace YSWL.MALL.Model.Shop.Commission
{
	/// <summary>
	/// CommissionDetail:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class CommissionDetail
	{
		public CommissionDetail()
		{}
        #region Model
        private long _detailid;
        private int _ruleid;
        private string _rulename;
        private int _userid;
        private string _username;
        private int _rulelevel;
        private DateTime _tradedate;
        private int _tradetype;
        private decimal _fee;
        private long _orderid;
        private string _ordercode;
        private int _buyerid;
        private string _buyername;
        private decimal _orderamount;
        private int _referid;
        private int? _refertype = 0;
        private long _productid;
        private int _quantity;
        private string _name;
        private int _supplierid;
        private string _suppliername;
        private int _brandid;
        private string _brandname;
        private string _remark;
        /// <summary>
        /// 
        /// </summary>
        public long DetailId
        {
            set { _detailid = value; }
            get { return _detailid; }
        }
        /// <summary>
        /// 佣金规则ID
        /// </summary>
        public int RuleId
        {
            set { _ruleid = value; }
            get { return _ruleid; }
        }
        /// <summary>
        /// 佣金规则名
        /// </summary>
        public string RuleName
        {
            set { _rulename = value; }
            get { return _rulename; }
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 拥挤规则级别  
        /// </summary>
        public int RuleLevel
        {
            set { _rulelevel = value; }
            get { return _rulelevel; }
        }
        /// <summary>
        /// 交易日期
        /// </summary>
        public DateTime TradeDate
        {
            set { _tradedate = value; }
            get { return _tradedate; }
        }
        /// <summary>
        /// 交易类型   1:收入 2:支出
        /// </summary>
        public int TradeType
        {
            set { _tradetype = value; }
            get { return _tradetype; }
        }
        /// <summary>
        /// 佣金
        /// </summary>
        public decimal Fee
        {
            set { _fee = value; }
            get { return _fee; }
        }
        /// <summary>
        /// 订单ID
        /// </summary>
        public long OrderId
        {
            set { _orderid = value; }
            get { return _orderid; }
        }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderCode
        {
            set { _ordercode = value; }
            get { return _ordercode; }
        }
        /// <summary>
        /// 买家ID
        /// </summary>
        public int BuyerID
        {
            set { _buyerid = value; }
            get { return _buyerid; }
        }
        /// <summary>
        /// 买家名称
        /// </summary>
        public string BuyerName
        {
            set { _buyername = value; }
            get { return _buyername; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal OrderAmount
        {
            set { _orderamount = value; }
            get { return _orderamount; }
        }
        /// <summary>
        /// 推广来源ID 默认为邀请用户ID
        /// </summary>
        public int ReferID
        {
            set { _referid = value; }
            get { return _referid; }
        }
        /// <summary>
        /// 订单来源类型 1：表示PC下单，2：表示微信下单，3：表示业务员待下单
        /// </summary>
        public int? ReferType
        {
            set { _refertype = value; }
            get { return _refertype; }
        }
        /// <summary>
        /// 商品ID
        /// </summary>
        public long ProductId
        {
            set { _productid = value; }
            get { return _productid; }
        }
        /// <summary>
        /// 商品数量
        /// </summary>
        public int Quantity
        {
            set { _quantity = value; }
            get { return _quantity; }
        }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 供货商ID
        /// </summary>
        public int SupplierId
        {
            set { _supplierid = value; }
            get { return _supplierid; }
        }
        /// <summary>
        /// 供货商名称
        /// </summary>
        public string SupplierName
        {
            set { _suppliername = value; }
            get { return _suppliername; }
        }
        /// <summary>
        /// 品牌Id
        /// </summary>
        public int BrandId
        {
            set { _brandid = value; }
            get { return _brandid; }
        }
        /// <summary>
        /// 品牌名称
        /// </summary>
        public string BrandName
        {
            set { _brandname = value; }
            get { return _brandname; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        #endregion Model

        #region 扩展属性
        /// <summary>
        /// 一级佣金
        /// </summary>
        public decimal FirstFee;
        /// <summary>
        /// 二级佣金
        /// </summary>
        public decimal SecondFee;
        /// <summary>
        /// 推广链接码
        /// </summary>
	    public string PromoCode;

	    #endregion
	}
}

