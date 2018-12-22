/**  版本信息模板在安装目录下，可自行修改。
* PreOrder.cs
*
* 功 能： N/A
* 类 名： PreOrder
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/8/24 16:08:38   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace YSWL.MALL.Model.Shop.PrePro
{
	/// <summary>
	/// 预定
	/// </summary>
	[Serializable]
	public partial class PreOrder
	{
		public PreOrder()
		{}
        #region Model
        private long _preorderid;
        private string _precode;
        private int _preproid;
        private long _productid;
        private string _productname;
        private int _count;
        private string _sku;
        private int _paymenttypeid;
        private string _paymenttypename;
        private string _paymentgateway;
        private int _paymentstatus = 0;
        private decimal _amount;
        private string _phone;
        private int _userid;
        private string _username;
        private DateTime _createddate;
        private int _handleuserid = 0;
        private DateTime? _handledate;
        private string _deliverytip;
        private int _status;
        private string _remark;
        /// <summary>
        /// 预定ID
        /// </summary>
        public long PreOrderId
        {
            set { _preorderid = value; }
            get { return _preorderid; }
        }
        /// <summary>
        /// 预订码
        /// </summary>
        public string PreCode
        {
            set { _precode = value; }
            get { return _precode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int PreProId
        {
            set { _preproid = value; }
            get { return _preproid; }
        }
        /// <summary>
        /// 产品ID
        /// </summary>
        public long ProductId
        {
            set { _productid = value; }
            get { return _productid; }
        }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName
        {
            set { _productname = value; }
            get { return _productname; }
        }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count
        {
            set { _count = value; }
            get { return _count; }
        }
        /// <summary>
        /// SKU
        /// </summary>
        public string SKU
        {
            set { _sku = value; }
            get { return _sku; }
        }
        /// <summary>
        /// 支付类型接口ID
        /// </summary>
        public int PaymentTypeId
        {
            set { _paymenttypeid = value; }
            get { return _paymenttypeid; }
        }
        /// <summary>
        /// 支付类型名称
        /// </summary>
        public string PaymentTypeName
        {
            set { _paymenttypename = value; }
            get { return _paymenttypename; }
        }
        /// <summary>
        /// 支付网关名称(用于辨识付款类型)  cod 货到付款| bank 银行汇款 | [..] 在线支付
        /// </summary>
        public string PaymentGateway
        {
            set { _paymentgateway = value; }
            get { return _paymentgateway; }
        }
        /// <summary>
        /// 支付状态 0 未支付 | 1 等待确认 | 2 已支付 | 3 处理中(预留) | 4 支付异常(预留)
        /// </summary>
        public int PaymentStatus
        {
            set { _paymentstatus = value; }
            get { return _paymentstatus; }
        }
        /// <summary>
        /// 预订金额
        /// </summary>
        public decimal Amount
        {
            set { _amount = value; }
            get { return _amount; }
        }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone
        {
            set { _phone = value; }
            get { return _phone; }
        }
        /// <summary>
        /// 创建者
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
        /// 创建时间
        /// </summary>
        public DateTime CreatedDate
        {
            set { _createddate = value; }
            get { return _createddate; }
        }
        /// <summary>
        /// 处理人
        /// </summary>
        public int HandleUserId
        {
            set { _handleuserid = value; }
            get { return _handleuserid; }
        }
        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? HandleDate
        {
            set { _handledate = value; }
            get { return _handledate; }
        }
        /// <summary>
        /// 预计配送时间
        /// </summary>
        public string DeliveryTip
        {
            set { _deliverytip = value; }
            get { return _deliverytip; }
        }
        /// <summary>
        /// 预定状态   -1：取消  0：未处理  1：已处理（已支付）2：已完成
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
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

	}
}

