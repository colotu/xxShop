/**  版本信息模板在安装目录下，可自行修改。
* ReturnOrderAction.cs
*
* 功 能： N/A
* 类 名： ReturnOrderAction
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/7/2 11:50:35   N/A    初版
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
using YSWL.Common;
using YSWL.MALL.Model.Shop.ReturnOrder;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Shop.ReturnOrder;
namespace YSWL.MALL.BLL.Shop.ReturnOrder
{
	/// <summary>
	/// ReturnOrderAction
	/// </summary>
	public partial class ReturnOrderAction
	{
		private readonly IReturnOrderAction dal=DAShopReturnOrder.CreateReturnOrderAction();
		public ReturnOrderAction()
		{}
		#region  BasicMethod
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(long ActionId)
		{
			return dal.Exists(ActionId);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public long Add(YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderAction model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderAction model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(long ActionId)
		{
			
			return dal.Delete(ActionId);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string ActionIdlist )
		{
            return dal.DeleteList(YSWL.Common.Globals.SafeLongFilter(ActionIdlist, 0));
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderAction GetModel(long ActionId)
		{
			
			return dal.GetModel(ActionId);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderAction GetModelByCache(long ActionId)
		{
			
			string CacheKey = "ReturnOrderActionModel-" + ActionId;
			object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(ActionId);
					if (objModel != null)
					{
						int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("CacheTime");
						YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderAction)objModel;
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
		public List<YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderAction> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderAction> DataTableToList(DataTable dt)
		{
			List<YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderAction> modelList = new List<YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderAction>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderAction model;
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
        public static  string GetActionCode(string actionCode)
        {
            switch (Globals.SafeInt(actionCode, 0))
            {
                case (int)EnumHelper.ActionCode.CustomersCancel:
                    return "客户取消";
                case (int)EnumHelper.ActionCode.CustomersCreate:
                    return "客户创建";
                case (int)EnumHelper.ActionCode.SystemCreate:
                    return "系统创建";    
                case (int)EnumHelper.ActionCode.SystemCancel:
                    return "系统取消";
                case (int)EnumHelper.ActionCode.SystemUpdateAmountAdjusted:
                    return "系统修改应退金额";
                case (int)EnumHelper.ActionCode.SystemAudit:
                    return "系统审核";
                case (int)EnumHelper.ActionCode.SystemUpdatePick:
                    return "系统修改取货信息";
                case (int)EnumHelper.ActionCode.SystemPicked:
                    return "系统确认取货";
                case (int)EnumHelper.ActionCode.SystemUpdateAmountActual:
                    return "系统修改实退金额";
                case (int)EnumHelper.ActionCode.SystemReturnCoupon:
                    return "系统返还优惠劵";
                case (int)EnumHelper.ActionCode.SystemComplete:
                    return "系统完成(确认退款)";
                case (int)EnumHelper.ActionCode.SellerCreate:
                    return "商家创建";
                case (int)EnumHelper.ActionCode.SellerCancel:
                    return "商家取消";
                case (int)EnumHelper.ActionCode.SellerUpdateAmountAdjusted:
                    return "商家修改应退金额";
                case (int)EnumHelper.ActionCode.SellerAudit:
                    return "商家审核";
                case (int)EnumHelper.ActionCode.SellerUpdatePick:
                    return "商家修改取货信息";
                case (int)EnumHelper.ActionCode.SellerPicked:
                    return "商家确认取货";
                case (int)EnumHelper.ActionCode.SellerUpdateAmountActual:
                    return "商家修改实退金额";
                case (int)EnumHelper.ActionCode.SellerReturnCoupon:
                    return "商家返还优惠劵";
                case (int)EnumHelper.ActionCode.SellerComplete:
                    return "商家完成(确认退款)";      
            }
            return "";
        }
         /// <summary>
        /// 根据退单id获得退单项数据列表
        /// </summary>
        /// <param name="ReturnOrderId"></param>
        /// <returns></returns>
        public List<YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderAction> GetModelList(long ReturnOrderId)
        {
            DataSet ds = dal.GetList(" ReturnOrderId=" + ReturnOrderId);
            return DataTableToList(ds.Tables[0]);
        }
		#endregion  ExtensionMethod
	}
}

