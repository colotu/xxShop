/**  版本信息模板在安装目录下，可自行修改。
* ReturnOrderItems.cs
*
* 功 能： N/A
* 类 名： ReturnOrderItems
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
using YSWL.Common;
using YSWL.MALL.Model.Shop.ReturnOrder;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Shop.ReturnOrder;
namespace YSWL.MALL.BLL.Shop.ReturnOrder
{
	/// <summary>
	/// ReturnOrderItems
	/// </summary>
	public partial class ReturnOrderItems
	{
        private readonly IReturnOrderItems dal = DAShopReturnOrder.CreateReturnOrderItems();
		public ReturnOrderItems()
		{}
		#region  BasicMethod
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(long ItemId)
		{
			return dal.Exists(ItemId);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public long Add(YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderItems model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderItems model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(long ItemId)
		{
			
			return dal.Delete(ItemId);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string ItemIdlist )
		{
            return dal.DeleteList(YSWL.Common.Globals.SafeLongFilter(ItemIdlist, 0));
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderItems GetModel(long ItemId)
		{
			
			return dal.GetModel(ItemId);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderItems GetModelByCache(long ItemId)
		{
			
			string CacheKey = "ReturnOrderItemsModel-" + ItemId;
			object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(ItemId);
					if (objModel != null)
					{
						int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("CacheTime");
						YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderItems)objModel;
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
		public List<YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderItems> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderItems> DataTableToList(DataTable dt)
		{
			List<YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderItems> modelList = new List<YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderItems>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderItems model;
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
        /// 根据退单id获得退单项数据列表
        /// </summary>
        /// <param name="ReturnOrderId"></param>
        /// <returns></returns>
        public List<YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderItems> GetModelList(long ReturnOrderId)
        {
            DataSet ds = dal.GetList(" ReturnOrderId=" + ReturnOrderId);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetListByCache(string strWhere)
        {
            string CacheKey = "GetReturnItemListByCache-" + strWhere;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetList(strWhere);
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (DataSet)objModel;
        }
        /// <summary>
        ///将订单项装换成退单项
        /// </summary>
        /// <param name="item"></param>
        /// <param name="count"></param>
        /// <param name="returnOrderCode"></param>
        /// <returns></returns>
         public  Model.Shop.ReturnOrder.ReturnOrderItems GetReturnItemInfo(Model.Shop.Order.OrderItems item, int count, string returnOrderCode)
        {
         return   new Model.Shop.ReturnOrder.ReturnOrderItems
            {
                ReturnOrderCode = returnOrderCode,
                OrderId = item.OrderId,
                OrderCode = item.OrderCode,
                ProductId = item.ProductId,
                ProductCode = item.ProductCode,
                SKU = item.SKU,
                Name = item.Name,
                ThumbnailsUrl = item.ThumbnailsUrl,
                Description = item.Description,
                Quantity = count,
                ShipmentQuantity = count,
                CostPrice = item.CostPrice,
                SellPrice = item.SellPrice,
                ReturnPrice = item.AdjustedPrice,
                Attribute = item.Attribute,
                Remark = item.Remark,
                Weight = item.Weight,
                Deduct = item.Deduct,
                Points = item.Points,
                ProductLineId = item.ProductLineId,
                SupplierId = item.SupplierId,
                SupplierName = item.SupplierName,
                BrandId = item.BrandId,
                BrandName = item.BrandName,
                ProductType = item.ProductType
            };
        }
		#endregion  ExtensionMethod
	}
}

