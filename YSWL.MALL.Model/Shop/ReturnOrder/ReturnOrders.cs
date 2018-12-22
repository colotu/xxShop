/**  版本信息模板在安装目录下，可自行修改。
* ReturnOrders.cs
*
* 功 能： N/A
* 类 名： ReturnOrders
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/7/2 11:50:36   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
namespace YSWL.MALL.Model.Shop.ReturnOrder
{
	/// <summary>
	/// ReturnOrders:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class ReturnOrders
	{
		public ReturnOrders()
		{}
        #region Model
        private long _returnorderid;
        private string _returnordercode;
        private long _orderid;
        private string _ordercode;
        private int _returnuserid = 0;
        private string _returnusername;
        private int _createuserid = -1;
        private DateTime _createddate;
        private int _updateduserid = 0;
        private DateTime? _updateddate;
        private int _supplierid = 0;
        private string _suppliername;
        private string _couponcode;
        private string _couponname;
        private decimal? _couponamount;
        private decimal? _couponvalue;
        private int? _couponvaluetype;
        private int _returngoodstype = 2;
        private int _returncoupon = 0;
        private decimal _actualsalestotal;
        private decimal _amount = 0M;
        private decimal _amountadjusted;
        private decimal _amountactual=0;
        private int _servicetype;
        private string _credential;
        private string _description;
        private string _imageurl;
        private int _returntype;
        private int _pickregionid = 0;
        private string _pickregion;
        private string _pickaddress;
        private string _pickzipcode;
        private string _pickname;
        private string _picktelphone;
        private string _pickcellphone;
        private string _pickemail;
        private int _shippingmodeid = 0;
        private string _shippingmodename;
        private string _shipordernumber;
        private string _expresscompanyname;
        private string _expresscompanyabb;
        private string _returntruename;
        private string _returnbankname;
        private string _returncard;
        private int _returncardtype = 3;
        private string _contactname;
        private string _contactphone;
        private int _status = 0;
        private int _refundstatus = 0;
        private int _logisticstatus = 0;
        private string _refusereason;
        private int _customerreview = 0;
        private string _remark;
        /// <summary>
        /// 退货流水id
        /// </summary>
        public long ReturnOrderId
        {
            set { _returnorderid = value; }
            get { return _returnorderid; }
        }
        /// <summary>
        /// 退货自定义编号
        /// </summary>
        public string ReturnOrderCode
        {
            set { _returnordercode = value; }
            get { return _returnordercode; }
        }
        /// <summary>
        /// 订单Id
        /// </summary>
        public long OrderId
        {
            set { _orderid = value; }
            get { return _orderid; }
        }
        /// <summary>
        /// 订单自定义单号
        /// </summary>
        public string OrderCode
        {
            set { _ordercode = value; }
            get { return _ordercode; }
        }
        /// <summary>
        /// 退货人Id
        /// </summary>
        public int ReturnUserId
        {
            set { _returnuserid = value; }
            get { return _returnuserid; }
        }
        /// <summary>
        /// 退货人用户名
        /// </summary>
        public string ReturnUserName
        {
            set { _returnusername = value; }
            get { return _returnusername; }
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public int CreateUserId
        {
            set { _createuserid = value; }
            get { return _createuserid; }
        }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreatedDate
        {
            set { _createddate = value; }
            get { return _createddate; }
        }
        /// <summary>
        /// 更新人
        /// </summary>
        public int UpdatedUserId
        {
            set { _updateduserid = value; }
            get { return _updateduserid; }
        }
        /// <summary>
        /// 更新日期
        /// </summary>
        public DateTime? UpdatedDate
        {
            set { _updateddate = value; }
            get { return _updateddate; }
        }
        /// <summary>
        /// 商家ID 
        /// </summary>
        public int SupplierId
        {
            set { _supplierid = value; }
            get { return _supplierid; }
        }
        /// <summary>
        /// 商家名称
        /// </summary>
        public string SupplierName
        {
            set { _suppliername = value; }
            get { return _suppliername; }
        }
        /// <summary>
        /// 优惠码
        /// </summary>
        public string CouponCode
        {
            set { _couponcode = value; }
            get { return _couponcode; }
        }
        /// <summary>
        /// 优惠卷名称
        /// </summary>
        public string CouponName
        {
            set { _couponname = value; }
            get { return _couponname; }
        }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal? CouponAmount
        {
            set { _couponamount = value; }
            get { return _couponamount; }
        }
        /// <summary>
        /// 优惠值
        /// </summary>
        public decimal? CouponValue
        {
            set { _couponvalue = value; }
            get { return _couponvalue; }
        }
        /// <summary>
        /// 优惠值类型
        /// </summary>
        public int? CouponValueType
        {
            set { _couponvaluetype = value; }
            get { return _couponvaluetype; }
        }
        /// <summary>
        /// 退货类型   1:整单退    2.部分退
        /// </summary>
        public int ReturnGoodsType
        {
            set { _returngoodstype = value; }
            get { return _returngoodstype; }
        }
        /// <summary>
        /// 默认值 0   未设置      1:退优惠劵   2:不退优惠劵
        /// </summary>
        public int ReturnCoupon
        {
            set { _returncoupon = value; }
            get { return _returncoupon; }
        }
        /// <summary>
        /// 商品实际出售总价
        /// </summary>
        public decimal ActualSalesTotal
        {
            set { _actualsalestotal = value; }
            get { return _actualsalestotal; }
        }
        /// <summary>
        /// 实付金额
        /// </summary>
        public decimal Amount
        {
            set { _amount = value; }
            get { return _amount; }
        }
        /// <summary>
        /// 调整后金额(应退金额)
        /// </summary>
        public decimal AmountAdjusted
        {
            set { _amountadjusted = value; }
            get { return _amountadjusted; }
        }
        /// <summary>
        /// 实际金额(实退金额)
        /// </summary>
        public decimal AmountActual
        {
            set { _amountactual = value; }
            get { return _amountactual; }
        }
        /// <summary>
        /// 服务类型: 1 退货  2 换货  3 维修
        /// </summary>
        public int ServiceType
        {
            set { _servicetype = value; }
            get { return _servicetype; }
        }
        /// <summary>
        /// 申请凭据   多种凭据用|分割    1.带有发票     2.有检测报告  例：|1|2|3|4|
        /// </summary>
        public string Credential
        {
            set { _credential = value; }
            get { return _credential; }
        }
        /// <summary>
        /// 问题描述
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 图片信息   多张图片用|分割   相对路径
        /// </summary>
        public string ImageUrl
        {
            set { _imageurl = value; }
            get { return _imageurl; }
        }
        /// <summary>
        /// 退回方式   | 1上门取件  
        /// </summary>
        public int ReturnType
        {
            set { _returntype = value; }
            get { return _returntype; }
        }
        /// <summary>
        /// 取货省市区ID
        /// </summary>
        public int PickRegionId
        {
            set { _pickregionid = value; }
            get { return _pickregionid; }
        }
        /// <summary>
        /// 取货地区
        /// </summary>
        public string PickRegion
        {
            set { _pickregion = value; }
            get { return _pickregion; }
        }
        /// <summary>
        /// 取货地址
        /// </summary>
        public string PickAddress
        {
            set { _pickaddress = value; }
            get { return _pickaddress; }
        }
        /// <summary>
        /// 取货邮编
        /// </summary>
        public string PickZipCode
        {
            set { _pickzipcode = value; }
            get { return _pickzipcode; }
        }
        /// <summary>
        /// 取货人
        /// </summary>
        public string PickName
        {
            set { _pickname = value; }
            get { return _pickname; }
        }
        /// <summary>
        /// 取货人座机
        /// </summary>
        public string PickTelPhone
        {
            set { _picktelphone = value; }
            get { return _picktelphone; }
        }
        /// <summary>
        /// 取货人手机
        /// </summary>
        public string PickCellPhone
        {
            set { _pickcellphone = value; }
            get { return _pickcellphone; }
        }
        /// <summary>
        /// 取货人Email
        /// </summary>
        public string PickEmail
        {
            set { _pickemail = value; }
            get { return _pickemail; }
        }
        /// <summary>
        /// 配送ID
        /// </summary>
        public int ShippingModeId
        {
            set { _shippingmodeid = value; }
            get { return _shippingmodeid; }
        }
        /// <summary>
        /// 配送名称
        /// </summary>
        public string ShippingModeName
        {
            set { _shippingmodename = value; }
            get { return _shippingmodename; }
        }
        /// <summary>
        /// 配送单号
        /// </summary>
        public string ShipOrderNumber
        {
            set { _shipordernumber = value; }
            get { return _shipordernumber; }
        }
        /// <summary>
        /// 快递公司名称
        /// </summary>
        public string ExpressCompanyName
        {
            set { _expresscompanyname = value; }
            get { return _expresscompanyname; }
        }
        /// <summary>
        /// 快递公司缩写
        /// </summary>
        public string ExpressCompanyAbb
        {
            set { _expresscompanyabb = value; }
            get { return _expresscompanyabb; }
        }
        /// <summary>
        /// 开户姓名
        /// </summary>
        public string ReturnTrueName
        {
            set { _returntruename = value; }
            get { return _returntruename; }
        }
        /// <summary>
        /// 开户银行名称
        /// </summary>
        public string ReturnBankName
        {
            set { _returnbankname = value; }
            get { return _returnbankname; }
        }
        /// <summary>
        /// 银行卡号 或 支付宝号
        /// </summary>
        public string ReturnCard
        {
            set { _returncard = value; }
            get { return _returncard; }
        }
        /// <summary>
        /// 卡号类型    默认为3     1: 银行卡号   2:支付宝帐号  3:账户余额
        /// </summary>
        public int ReturnCardType
        {
            set { _returncardtype = value; }
            get { return _returncardtype; }
        }
        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactName
        {
            set { _contactname = value; }
            get { return _contactname; }
        }
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string ContactPhone
        {
            set { _contactphone = value; }
            get { return _contactphone; }
        }
        /// <summary>
        /// 状态  |-3拒绝 | -2锁定 |  -1取消申请  |   0未处理  |  1处理中| 2 完成|
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 退款状态 0 未退款 | 1 申请退款 | 2 退款中 | 3 已退款 | 4 拒绝退款
        /// </summary>
        public int RefundStatus
        {
            set { _refundstatus = value; }
            get { return _refundstatus; }
        }
        /// <summary>
        /// 物流状态:   0 未取货  |    1 未打印   |   2 取货中  |   3 返程中  |  4 已入库
        /// </summary>
        public int LogisticStatus
        {
            set { _logisticstatus = value; }
            get { return _logisticstatus; }
        }
        /// <summary>
        /// 拒绝原因  (当申请被拒绝时使用)
        /// </summary>
        public string RefuseReason
        {
            set { _refusereason = value; }
            get { return _refusereason; }
        }
        /// <summary>
        /// 客户评价      0 未填写  1  已解决  2未解决
        /// </summary>
        public int CustomerReview
        {
            set { _customerreview = value; }
            get { return _customerreview; }
        }
        /// <summary>
        /// 退货备注
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        #endregion Model
 
    }
}

