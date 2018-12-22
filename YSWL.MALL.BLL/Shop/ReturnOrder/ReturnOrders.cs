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
using System.Data;
using System.Collections.Generic;
using YSWL.MALL.BLL.Members;
using YSWL.MALL.BLL.Shop.Order;
using YSWL.Common;
using YSWL.MALL.Model.Shop.ReturnOrder;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Shop.ReturnOrder;
using System.Text;
namespace YSWL.MALL.BLL.Shop.ReturnOrder
{
	/// <summary>
	/// ReturnOrders
	/// </summary>
	public partial class ReturnOrders
	{
        private readonly IReturnOrders dal = DAShopReturnOrder.CreateReturnOrders();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
		public ReturnOrders()
		{}
		#region  BasicMethod
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(long ReturnOrderId)
		{
			return dal.Exists(ReturnOrderId);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public long Add(YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(long ReturnOrderId)
		{
			
			return dal.Delete(ReturnOrderId);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string ReturnOrderIdlist )
		{
            return dal.DeleteList(YSWL.Common.Globals.SafeLongFilter(ReturnOrderIdlist, 0));
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders GetModel(long ReturnOrderId)
		{
			
			return dal.GetModel(ReturnOrderId);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders GetModelByCache(long ReturnOrderId)
		{
			
			string CacheKey = "ReturnOrdersModel-" + ReturnOrderId;
			object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(ReturnOrderId);
					if (objModel != null)
					{
						int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("CacheTime");
						YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders)objModel;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders> DataTableToList(DataTable dt)
		{
			List<YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders> modelList = new List<YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = dal.DataRowToModel(dt.Rows[n]);
					if (model != null)
					{
						modelList.Add(model);
					}
				}
			}
			return modelList;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			return dal.GetRecordCount(strWhere);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			return dal.GetListByPage( strWhere,  orderby,  startIndex,  endIndex);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  BasicMethod
		#region  ExtensionMethod
       /// <summary>
        /// 根据订单号获取退货记录总数 (不包含已取消的记录和已拒绝的)
       /// </summary>
       /// <param name="orderId"></param>
       /// <returns></returns>
        public int GetCountByOrderId(long orderId)
        {
            return dal.GetCountByOrderId(orderId);
        }
        /// <summary>
        /// 是否满足退单条件 (已完成的订单并且未申请过退单)
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <param name="orderStatus">订单状态</param>
        /// <returns></returns>
        public bool IsMeetCondition(long orderId, int orderStatus)
        {
            if (orderStatus != (int)YSWL.MALL.Model.Shop.Order.EnumHelper.OrderStatus.Complete)
            { return false; }
            if (GetCountByOrderId(orderId) > 0)
            { return false; }
            return true;
        }
        /// <summary>
        /// 是否满足退单条件 (已完成的订单并且未申请过退单)
        /// </summary>
        /// <param name="orderModel"></param>
        /// <param name="returnUserId">退单用户id</param>
        /// <returns></returns>
        public bool IsMeetCondition(Model.Shop.Order.OrderInfo orderModel , int returnUserId) 
        {
            if (orderModel == null)
            {
                return false;
            }
            if (orderModel.BuyerID != returnUserId || orderModel.OrderStatus != (int)YSWL.MALL.Model.Shop.Order.EnumHelper.OrderStatus.Complete || GetCountByOrderId(orderModel.OrderId) > 0)
            {
                return false;
            } 
            return true;
        }
        /// <summary>
        /// 创建退单
        /// </summary>
        /// <param name="returnOrders"></param>
        /// <returns></returns>
        public long CreateReturnOrder(YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders returnOrders, Accounts.Bus.User currentUser )
        {
            return dal.CreateReturnOrder(returnOrders,currentUser);
        }
        /// <summary>
        /// 得到一个对象实体(包括退单项)
        /// </summary>
        public YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders GetModelInfo(long ReturnOrderId)
        {
            YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model = GetModel(ReturnOrderId);
            if (model != null)
            {
                YSWL.MALL.BLL.Shop.ReturnOrder.ReturnOrderItems itemBll = new ReturnOrderItems();
                model.Items = itemBll.GetModelList(ReturnOrderId);
            }
            return model;
        }
    /// <summary>
    /// 分页获取数据列表
    /// </summary>
    /// <param name="ReturnUserId"></param>
    /// <param name="startIndex"></param>
    /// <param name="endIndex"></param>
    /// <param name="toalCount"></param>
    /// <returns></returns>
        public List<YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders> GetListByPage(int ReturnUserId, int startIndex, int endIndex, out int toalCount)
        {
            string strWhere = string.Format(" ReturnUserId={0} ", ReturnUserId);
            toalCount = GetRecordCount(strWhere);
            DataSet ds = dal.GetListByPage(strWhere, "  ReturnOrderId desc  ", startIndex, endIndex);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获取返回方式 字符串
        /// </summary>
        /// <param name="returntype"></param>
        /// <returns></returns>
        public string GetReturnTypeStr(int returntype) 
        {
            switch (returntype) { 
                case 1:
                    return "上门取货";
                default:
                    return "";
            }
        }
        /// <summary>
        /// 获取服务类型 字符串
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public string GetServiceTypeStr(int serviceType)
        {
            switch (serviceType)
            {
                case 1:
                    return "退货";
                case 2:
                    return "换货";
                case 3:
                    return "维修";
                default:
                    return "";
            }
        }
        /// <summary>
        /// 获取退货类型 字符串
        /// </summary>
        /// <param name="returnGoodsType"></param>
        /// <returns></returns>
        public string GetReturnGoodsTypeStr(int returnGoodsType)
        {
            switch (returnGoodsType)
            {
                case (int)Model.Shop.ReturnOrder.EnumHelper.ReturngoodsType.All:
                    return "整单退";
                case (int)Model.Shop.ReturnOrder.EnumHelper.ReturngoodsType.Part:
                    return "部分退";
                default:
                    return "";
            }
        }
        public void RemoveModelCache(long ReturnOrderId)
        {
            YSWL.Common.DataCache.DeleteCache("ReturnOrdersModel-" + ReturnOrderId);
        }
         /// <summary>
        /// 审核通过 修改数据
     /// </summary>
     /// <param name="model"></param>
     /// <param name="IsReturnGoods">是否需要退货</param>
     /// <returns></returns>
        public bool AuditPass(YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model, bool IsReturnGoods)
         {
             BLL.Shop.Order.Orders orderBll = new Orders();
            return dal.AuditPass(model, IsReturnGoods, orderBll.GetModel(model.OrderId));
        }
       /// <summary>
        /// 审核未通过
       /// </summary>
       /// <param name="ReturnOrderId"></param>
       /// <param name="refuseReason">原因</param>
       /// <param name="UpdateUserId"></param>
       /// <param name="Remark"></param>
       /// <returns></returns>
        public bool AuditFail(long ReturnOrderId, string refuseReason, int UpdateUserId, string Remark)
        {
            return dal.AuditFail(ReturnOrderId, refuseReason, UpdateUserId, Remark);
        }
       /// <summary>
        /// 获取组合状态
       /// </summary>
       /// <param name="Status"></param>
       /// <param name="LogisticStatus"></param>
       /// <param name="RefundStatus"></param>
       /// <returns></returns>
        public static string GetMainStatusStr(int Status, int LogisticStatus, int RefundStatus)
        {
            EnumHelper.MainStatus type = GetMainStatus(Status, LogisticStatus, RefundStatus);
            switch (type)
            {
                case EnumHelper.MainStatus.Auditing:
                    return  "等待审核";
                case EnumHelper.MainStatus.Cancel:
                    return "已取消";
                case EnumHelper.MainStatus.Refuse:
                    return "审核未通过";
                case EnumHelper.MainStatus.Handling:
                    return "正在处理";
                case EnumHelper.MainStatus.Packing:
                    return "取货中";
                case EnumHelper.MainStatus.Returning:
                    return "返程中";
                case EnumHelper.MainStatus.WaitingRefund:
                    return "等待退款";
                case EnumHelper.MainStatus.Refunding:
                    return "退款中";
                case EnumHelper.MainStatus.Complete:
                    return "已完成";
            }
            return "";
        }
        /// <summary>
        /// 根据各种状态返回组合状态
        /// </summary>
        /// <returns></returns>
        public static EnumHelper.MainStatus GetMainStatus(int Status, int LogisticStatus, int RefundStatus)
        {
             EnumHelper.MainStatus type= EnumHelper.MainStatus.Auditing;
             #region 未审核
             //等待审核
             if (Status == (int)EnumHelper.Status.UnHandle &&
                 LogisticStatus == (int)EnumHelper.LogisticStatus.UnPick &&
                 RefundStatus == (int)EnumHelper.RefundStatus.UnRefund)
             {
                return  type = EnumHelper.MainStatus.Auditing;
             }
             //取消申请
             if (Status == (int)EnumHelper.Status.Cancel &&
                 LogisticStatus == (int)EnumHelper.LogisticStatus.UnPick &&
                 RefundStatus == (int)EnumHelper.RefundStatus.UnRefund)
             {
                return type = EnumHelper.MainStatus.Cancel;
             }
             #endregion

             #region 审核未通过
              //拒绝
             if (Status == (int)EnumHelper.Status.Refuse &&
                 LogisticStatus == (int)EnumHelper.LogisticStatus.UnPick &&
                 RefundStatus == (int)EnumHelper.RefundStatus.UnRefund)
             {
                 return type = EnumHelper.MainStatus.Refuse;
             }
             #endregion


             #region 已审核并通过
             if (IsReturnGoods(LogisticStatus))
             {//需要取货
                 //正在处理
                 if (Status == (int)EnumHelper.Status.Handling &&
                     LogisticStatus == (int)EnumHelper.LogisticStatus.UnPrint &&
                     RefundStatus == (int)EnumHelper.RefundStatus.UnRefund)
                 {
                     return type = EnumHelper.MainStatus.Handling;
                 }
                 //取货中
                 if (Status == (int)EnumHelper.Status.Handling &&
                     LogisticStatus == (int)EnumHelper.LogisticStatus.Packing &&
                     RefundStatus == (int)EnumHelper.RefundStatus.UnRefund)
                 {
                     return type = EnumHelper.MainStatus.Packing;
                 }
                 //返程中
                 if (Status == (int)EnumHelper.Status.Handling &&
                     LogisticStatus == (int)EnumHelper.LogisticStatus.Returning &&
                     RefundStatus == (int)EnumHelper.RefundStatus.UnRefund)
                 {
                     return type = EnumHelper.MainStatus.Returning;
                 }
                 //等待退款(已入库)
                 if (Status == (int)EnumHelper.Status.Handling &&
                     LogisticStatus == (int)EnumHelper.LogisticStatus.Storaged &&
                     RefundStatus == (int)EnumHelper.RefundStatus.Apply)
                 {
                     return type = EnumHelper.MainStatus.WaitingRefund;
                 }
                 //退款中
                 if (Status == (int)EnumHelper.Status.Handling &&
                     LogisticStatus == (int)EnumHelper.LogisticStatus.Storaged &&
                     RefundStatus == (int)EnumHelper.RefundStatus.Refunding)
                 {
                     return type = EnumHelper.MainStatus.Refunding;
                 }
                 //已完成
                 if (Status == (int)EnumHelper.Status.Complete &&
                     LogisticStatus == (int)EnumHelper.LogisticStatus.Storaged &&
                     RefundStatus == (int)EnumHelper.RefundStatus.Refunds)
                 {
                     return type = EnumHelper.MainStatus.Complete;
                 }

             }
             else  //不需要取货
             {
                 //等待退款(未取货)
                 if (Status == (int)EnumHelper.Status.Handling &&
                     LogisticStatus == (int)EnumHelper.LogisticStatus.UnPick &&
                     RefundStatus == (int)EnumHelper.RefundStatus.Apply)
                 {
                     return type = EnumHelper.MainStatus.WaitingRefund;
                 }
                 //退款中(未取货)
                 if (Status == (int)EnumHelper.Status.Handling &&
                     LogisticStatus == (int)EnumHelper.LogisticStatus.UnPick &&
                     RefundStatus == (int)EnumHelper.RefundStatus.Refunding)
                 {
                     return type = EnumHelper.MainStatus.Refunding;
                 }
                 //已完成
                 if (Status == (int)EnumHelper.Status.Complete &&
                     LogisticStatus == (int)EnumHelper.LogisticStatus.UnPick &&
                     RefundStatus == (int)EnumHelper.RefundStatus.Refunds)
                 {
                     return type = EnumHelper.MainStatus.Complete;
                 }
             }
             #endregion

            return type;
        }

        /// <summary>
        /// 根据状态获取是否需要取货 (当状态大于【未处理】时再调用此方法) Status > (int)EnumHelper.Status.UnHandle
        /// </summary>
        /// <param name="LogisticStatus">取货状态</param>
        /// <returns></returns>
        public static bool IsReturnGoods(int LogisticStatus)
        {
            if (LogisticStatus == (int)EnumHelper.LogisticStatus.UnPick)//不需要取货
            {
                return false;
            }
            else
            {//需要取货
                return true;
            }
        }

        /// <summary>
        /// 取消退单申请
        /// </summary>
        /// <param name="ReturnOrderId"></param>
        /// <param name="ReturnOrderCode"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public bool CancelReturnOrder(long ReturnOrderId, string ReturnOrderCode, int  userId) 
        {
            bool  returnVal=dal.CancelReturnOrder(ReturnOrderId, ReturnOrderCode, userId);
            if (returnVal)
            {
                RemoveModelCache(ReturnOrderId);
            }
            return returnVal;
        }
        /// <summary>
        /// 确认取货
        /// </summary>
        /// <param name="returnId"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public bool PackedOrder(YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders info, Accounts.Bus.User currentUser)
        {
            return dal.PackedOrder(info, currentUser);
        }
       /// <summary>
         ///  完成退款
       /// </summary>
       /// <param name="model"></param>
         /// <param name="currentUser">当前用户</param>
         /// <param name="IsReturnCoupon">是否退优惠劵</param>
        /// <param name="suppUserId">商家的用户Id</param>
        /// <param name="deductionSuppAmount">扣除商家的金额</param>
        /// <param name="supplierId">商家id</param>
       /// <returns></returns>
         public bool Refunds(Model.Shop.ReturnOrder.ReturnOrders info, Accounts.Bus.User currentUser, bool IsReturnCoupon, int suppUserId,decimal deductionSuppAmount,int supplierId=0)
        {
            if (info ==null || currentUser == null)
            {
                return false;
            }
            //管理员/商家 完成退款 
            if ((currentUser.UserType == "SP" && info.SupplierId != supplierId) && currentUser.UserType != "AA")
            {
                YSWL.MALL.Model.SysManage.ErrorLog errorlogmodel = new YSWL.MALL.Model.SysManage.ErrorLog();
                errorlogmodel.Loginfo =
                    string.Format("入侵拦截:[非法完成退货单][YSWL.MALL.BLL.Shop.ReturnOrder.ReturnOrders.Refunds] IP:[{0}]",
                        System.Web.HttpContext.Current.Request.UserHostAddress);
                errorlogmodel.StackTrace = string.Empty;
                errorlogmodel.Url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
                YSWL.MALL.BLL.SysManage.ErrorLog.Add(errorlogmodel);
                return false;
            }

            #region 检查状态，防止一个退货单完成退款两次
            if (info.RefundStatus >= (int)Model.Shop.ReturnOrder.EnumHelper.RefundStatus.Refunds)
            {
                return false;
            }
            #endregion

            if (IsReturnGoods(info.LogisticStatus))
            {
                info.LogisticStatus = (int)Model.Shop.ReturnOrder.EnumHelper.LogisticStatus.Storaged;
            }


            #region 是否符合退优惠劵的条件
            if (!IsReturnCoupon || String.IsNullOrWhiteSpace(info.CouponCode) || info.ReturnGoodsType != (int)Model.Shop.ReturnOrder.EnumHelper.ReturngoodsType.All)
            {
                IsReturnCoupon = false;
            }
            else
            {
                //选中了退优惠劵checkbox      且原订单含优惠劵信息     且是整单退, 
                IsReturnCoupon = true;
            }
            #endregion

            #region 扣除购物成长值

            //是否开启会员等级
            bool isEnable = SysManage.ConfigSystem.GetBoolValueByCache("RankScoreEnable");
            //购物成长值比例 
            decimal rankScoreRatio = SysManage.ConfigSystem.GetDecimalValueByCache("Shop_ShoppingRankScoreRatio");
            //计算成长值
            int rankScore = (int)(info.AmountActual * rankScoreRatio);
            if (isEnable && rankScoreRatio > 0 && rankScore > 0)
            {
                BLL.Members.RankDetail rankDetailBll = new RankDetail();
                //扣除购物成长值   
                rankDetailBll.DeductScore(info.ReturnUserId, rankScore, string.Format("退货单号：{0}", info.ReturnOrderCode));
            }
            #endregion

            return dal.Refunds(info, currentUser, IsReturnCoupon,suppUserId,deductionSuppAmount);
        }

         /// <summary>
         ///  修改应退金额
         /// </summary>
         /// <param name="ReturnOrderId"></param>
         /// <param name="ReturnOrderCode"></param>
         /// <param name="oldAmountAdjusted">修改前的值(数据库中的值)</param>
         /// <param name="newAmountAdjusted">修改后的值</param>
         /// <param name="currentUser"></param>
         /// <returns></returns>
         public bool UpdateAmountAdjusted(long ReturnOrderId, string ReturnOrderCode, decimal oldAmountAdjusted, decimal newAmountAdjusted, Accounts.Bus.User currentUser)
         {
             return dal.UpdateAmountAdjusted(ReturnOrderId, ReturnOrderCode, oldAmountAdjusted, newAmountAdjusted, currentUser);
         }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders> GetModelList(int Top, string strWhere, string filedOrder)
        {
            DataSet ds = dal.GetList(Top, strWhere, filedOrder);
            return DataTableToList(ds.Tables[0]);
        }
		#endregion  ExtensionMethod
        /// <summary>
        /// 获得商家数据列表
        /// </summary>
        /// <param name="supplierid">商家supplierId</param>
        /// <param name="rordercode">退货单号</param>
        /// <param name="ordercode">订单号</param>
        /// <param name="contactname">联系人</param>
        /// <param name="username">用户名</param>
        /// <param name="startdate">开始日期</param>
        /// <param name="enddate">结束日期</param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="toalCount">总条数</param>
        /// <returns></returns>
        public List<YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders> GetListByPage(int supplierid,string rordercode, string ordercode, string contactname, string username, DateTime? startdate,DateTime? enddate,  int startIndex, int endIndex, out int toalCount)
        {
            StringBuilder strWhere = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(rordercode))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" ReturnOrderCode like '%{0}%'", InjectionFilter.QuoteFilter(rordercode.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(ordercode))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" OrderCode like '%{0}%'", InjectionFilter.QuoteFilter(ordercode));
            }

            if (!string.IsNullOrWhiteSpace(contactname))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" ContactName like '%{0}%'", InjectionFilter.QuoteFilter(contactname));
            }
            if (!string.IsNullOrWhiteSpace(username))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" ReturnUserName like '%{0}%'", InjectionFilter.QuoteFilter(username));
            }

            if (startdate.HasValue)
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and  ");
                }
                strWhere.AppendFormat(" CreatedDate >='" + startdate + "' ");
            }
            //时间段
            if (enddate.HasValue)
            {
                string endTime = Common.Globals.SafeDateTime(enddate, DateTime.Now).AddDays(1).ToString("yyyy-MM-dd");
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and  ");
                }
                strWhere.AppendFormat(" CreatedDate <='" + endTime + "' ");
            }

            #region 商家
            if (strWhere.Length > 1) strWhere.Append(" and ");
            strWhere.AppendFormat(" SupplierId = {0}", supplierid);
            #endregion

            toalCount = GetRecordCount(strWhere.ToString());
            DataSet ds = dal.GetListByPage(strWhere.ToString(), " ReturnOrderId desc ", startIndex, endIndex);
            return DataTableToList(ds.Tables[0]);
        }


         /// <summary>
        /// 更新备注
        /// </summary>
        /// <param name="ReturnOrderId"></param>
        /// <param name="Remark"></param>
        /// <param name="SupplierId"></param>
        /// <returns></returns>
        public bool UpdateRemark(long ReturnOrderId, string Remark, int SupplierId)
        {
            return dal.UpdateRemark(ReturnOrderId, InjectionFilter.SqlFilter(Remark), SupplierId);
        }
        /// <summary>
        /// 获取应扣除商家的金额
        /// </summary>
        /// <returns></returns>
        public decimal GetDeductionSuppAmount(Model.Shop.ReturnOrder.ReturnOrders model)
        {
            if (model == null || model.SupplierId<=0)
            {
                return 0;
            }
            
            int supplierSettleType = BLL.SysManage.ConfigSystem.GetIntValueByCache("Shop_Supplier_SettleType");
            // 0 实退金额    1 成本价
            if (supplierSettleType == 1)
            {
                decimal result = 0;
                ReturnOrderItems itemBll = new ReturnOrderItems();
                List<YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderItems> itemList= itemBll.GetModelList(model.ReturnOrderId);
                if (itemList == null || itemList.Count == 0)
                {
                    return 0;
                }
                foreach (Model.Shop.ReturnOrder.ReturnOrderItems itemInfo in itemList)
                {
                    result += (itemInfo.CostPrice * itemInfo.ShipmentQuantity);
                }
                return result;
            }
            else
            {
                return model.AmountActual;
            }
        } 
	 
	}
}

