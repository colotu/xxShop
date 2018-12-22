/**  版本信息模板在安装目录下，可自行修改。
* SupplierAD.cs
*
* 功 能： N/A
* 类 名： SupplierAD
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/10/30 10:48:44   N/A    初版
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
using YSWL.MALL.Model.Shop;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Shop;
using YSWL.MALL.IDAL.Shop.Supplier;
namespace YSWL.MALL.BLL.Shop.Supplier
{
	/// <summary>
	/// SupplierAD
	/// </summary>
	public partial class SupplierAD
	{
        private readonly ISupplierAD dal = DAShopSupplier.CreateSupplierAD();
		public SupplierAD()
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
		public bool Exists(int AdvertisementId)
		{
			return dal.Exists(AdvertisementId);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
        public int Add(YSWL.MALL.Model.Shop.Supplier.SupplierAD model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
        public bool Update(YSWL.MALL.Model.Shop.Supplier.SupplierAD model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int AdvertisementId)
		{
			
			return dal.Delete(AdvertisementId);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string AdvertisementIdlist )
		{
		    return dal.DeleteList(Common.Globals.SafeLongFilter(AdvertisementIdlist, 0));
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
        public YSWL.MALL.Model.Shop.Supplier.SupplierAD GetModel(int AdvertisementId)
		{
			
			return dal.GetModel(AdvertisementId);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
        public YSWL.MALL.Model.Shop.Supplier.SupplierAD GetModelByCache(int AdvertisementId)
		{
			
			string CacheKey = "SupplierADModel-" + AdvertisementId;
			object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(AdvertisementId);
					if (objModel != null)
					{
						int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("CacheTime");
						YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
            return (YSWL.MALL.Model.Shop.Supplier.SupplierAD)objModel;
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
		public List<YSWL.MALL.Model.Shop.Supplier.SupplierAD> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
        public List<YSWL.MALL.Model.Shop.Supplier.SupplierAD> DataTableToList(DataTable dt)
		{
            List<YSWL.MALL.Model.Shop.Supplier.SupplierAD> modelList = new List<YSWL.MALL.Model.Shop.Supplier.SupplierAD>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
                YSWL.MALL.Model.Shop.Supplier.SupplierAD model;
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
        /// 获得前几行数据
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Supplier.SupplierAD> GetModelList(int Top, string strWhere, string filedOrder)
        {
            DataSet ds= dal.GetList(Top, strWhere, filedOrder);
            return  DataTableToList(ds.Tables[0]);
        }
       /// <summary>
       /// 获得供应商广告位
       /// </summary>
       /// <param name="top"></param>
        /// <param name="suppId"></param>
       /// <returns></returns>
        public List<YSWL.MALL.Model.Shop.Supplier.SupplierAD> GetModelList(int top,int suppId)
        {
            return GetModelList(top , string.Format(" SupplierId = {0} and Status=1", suppId), " Sequence ");
        }

        public List<YSWL.MALL.Model.Shop.Supplier.SupplierAD> GetListByPageEx(string strWhere, string orderby,
                                                                              int startIndex, int endIndex)
        {
            DataSet ds = dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 根据广告位 获取广告内容
        /// </summary>
        /// <param name="aid"></param>
        /// <param name="suppId"></param>
        /// <returns></returns>
        public List<YSWL.MALL.Model.Shop.Supplier.SupplierAD> GetListByAidCache(int aid, int suppId)
        {
            string CacheKey = "GetListByAidCache-" + aid + "Supp_"+ suppId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    DataSet ds = dal.GetList(0,string.Format(" SupplierId={0} and   Status=1 AND   PositionId={1}", suppId, aid), " Sequence  ");
                    objModel = DataTableToList(ds.Tables[0]);
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (List<YSWL.MALL.Model.Shop.Supplier.SupplierAD>)objModel;
        }

	    /// <summary>
	    /// 根据广告位Id、商家名称得到一个对象实体 
	    /// </summary>
	    /// <param name="AdvPositionId">广告位</param>
	    /// <param name="suppId">商家Id</param>
	    /// <returns></returns>
	    public YSWL.MALL.Model.Shop.Supplier.SupplierAD GetModelByAdvPositionId(int AdvPositionId, int suppId)
	    {
	        return dal.GetModelByAdvPositionId(AdvPositionId, suppId);
	    }

	    #endregion  ExtensionMethod
        }
}

