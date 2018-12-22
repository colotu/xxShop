/**  版本信息模板在安装目录下，可自行修改。
* SuppDistProduct.cs
*
* 功 能： N/A
* 类 名： SuppDistProduct
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/1/27 17:36:25   N/A    初版
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
using YSWL.MALL.Model.Shop.Distribution;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Shop.Distribution;
namespace YSWL.MALL.BLL.Shop.Distribution
{
	/// <summary>
	/// SuppDistProduct
	/// </summary>
	public partial class SuppDistProduct
	{
		private readonly ISuppDistProduct dal=DAShopDist.CreateSuppDistProduct();
		public SuppDistProduct()
		{}
		#region  BasicMethod
		/// <summary>
		/// 是否存在该记录
		/// </summary>
        public bool Exists(int supplierId, long ProductId)
		{
			return dal.Exists(supplierId,ProductId);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
        public bool Add(int supplierId, YSWL.MALL.Model.Shop.Distribution.SuppDistProduct model)
		{
			return dal.Add(supplierId,model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
        public bool Update(int supplierId, YSWL.MALL.Model.Shop.Distribution.SuppDistProduct model)
		{
			return dal.Update(supplierId,model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
        public bool Delete(int supplierId, long ProductId)
		{
			
			return dal.Delete(supplierId,ProductId);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
        public bool DeleteList(int supplierId, string ProductIdlist)
		{
            return dal.DeleteList(supplierId, Common.Globals.SafeLongFilter(ProductIdlist, 0));
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
        public YSWL.MALL.Model.Shop.Distribution.SuppDistProduct GetModel(int supplierId, long ProductId)
		{
			
			return dal.GetModel(supplierId,ProductId);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
        public YSWL.MALL.Model.Shop.Distribution.SuppDistProduct GetModelByCache(int supplierId, long ProductId)
		{
			
			string CacheKey = "SuppDistProductModel-" + ProductId+supplierId;
			object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(supplierId,ProductId);
					if (objModel != null)
					{
						int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("CacheTime");
						YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (YSWL.MALL.Model.Shop.Distribution.SuppDistProduct)objModel;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
        public DataSet GetList(int supplierId, string strWhere)
		{
			return dal.GetList(supplierId,strWhere);
		}
		/// <summary>
		/// 获得前几行数据
		/// </summary>
        public DataSet GetList(int supplierId, int Top, string strWhere, string filedOrder)
		{
			return dal.GetList(supplierId,Top,strWhere,filedOrder);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
        public List<YSWL.MALL.Model.Shop.Distribution.SuppDistProduct> GetModelList(int supplierId, string strWhere)
		{
			DataSet ds = dal.GetList(supplierId,strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<YSWL.MALL.Model.Shop.Distribution.SuppDistProduct> DataTableToList(DataTable dt)
		{
			List<YSWL.MALL.Model.Shop.Distribution.SuppDistProduct> modelList = new List<YSWL.MALL.Model.Shop.Distribution.SuppDistProduct>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				YSWL.MALL.Model.Shop.Distribution.SuppDistProduct model;
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
		public DataSet GetAllList(int supplierId)
		{
			return GetList(supplierId,"");
		}

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public int GetRecordCount(int supplierId,string strWhere)
		{
			return dal.GetRecordCount(supplierId,strWhere);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(int supplierId,string strWhere, string orderby, int startIndex, int endIndex)
		{
			return dal.GetListByPage(supplierId,strWhere,  orderby,  startIndex,  endIndex);
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

		#endregion  ExtensionMethod
	}
}

