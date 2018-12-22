﻿using System;
using System.Data;
using System.Collections.Generic;
using YSWL.Common;
using YSWL.MALL.Model.Shop.Order;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Shop.Order;
namespace YSWL.MALL.BLL.Shop.Order
{
	/// <summary>
	/// OrderOptions
	/// </summary>
	public partial class OrderOptions
	{
        private readonly IOrderOptions dal = DAShopOrder.CreateOrderOptions();
		public OrderOptions()
		{}
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return dal.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int LookupListId, int LookupItemId, long OrderId)
        {
            return dal.Exists(LookupListId, LookupItemId, OrderId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(YSWL.MALL.Model.Shop.Order.OrderOptions model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.Shop.Order.OrderOptions model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int LookupListId, int LookupItemId, long OrderId)
        {

            return dal.Delete(LookupListId, LookupItemId, OrderId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.Order.OrderOptions GetModel(int LookupListId, int LookupItemId, long OrderId)
        {

            return dal.GetModel(LookupListId, LookupItemId, OrderId);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public YSWL.MALL.Model.Shop.Order.OrderOptions GetModelByCache(int LookupListId, int LookupItemId, long OrderId)
        {

            string CacheKey = "OrderOptionsModel-" + LookupListId + LookupItemId + OrderId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(LookupListId, LookupItemId, OrderId);
                    if (objModel != null)
                    {
                    int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.Shop.Order.OrderOptions)objModel;
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
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Order.OrderOptions> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Order.OrderOptions> DataTableToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Shop.Order.OrderOptions> modelList = new List<YSWL.MALL.Model.Shop.Order.OrderOptions>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Shop.Order.OrderOptions model;
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
            return dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
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
	    public List<YSWL.MALL.Model.Shop.Order.OrderOptions> Get2ListByOrderId(long orderId)
	    {
	        return DataTableToList(dal.Get2ListByOrderId(orderId).Tables[0]);

	    }
		#endregion  ExtensionMethod
	}
}

