/**  版本信息模板在安装目录下，可自行修改。
* PreProduct.cs
*
* 功 能： N/A
* 类 名： PreProduct
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/8/24 16:08:40   N/A    初版
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
using YSWL.MALL.Model.Shop.PrePro;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Shop.PrePro;
namespace YSWL.MALL.BLL.Shop.PrePro
{
	/// <summary>
	/// 预定商品
	/// </summary>
	public partial class PreProduct
	{
        private readonly IPreProduct dal = DAShopPrePro.CreatePreProduct();
		public PreProduct()
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
		public bool Exists(int PreProId)
		{
			return dal.Exists(PreProId);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(YSWL.MALL.Model.Shop.PrePro.PreProduct model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(YSWL.MALL.Model.Shop.PrePro.PreProduct model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int PreProId)
		{
			
			return dal.Delete(PreProId);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string PreProIdlist )
		{
			return dal.DeleteList(PreProIdlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public YSWL.MALL.Model.Shop.PrePro.PreProduct GetModel(int PreProId)
		{
			
			return dal.GetModel(PreProId);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public YSWL.MALL.Model.Shop.PrePro.PreProduct GetModelByCache(int PreProId)
		{
			
			string CacheKey = "PreProductModel-" + PreProId;
			object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(PreProId);
					if (objModel != null)
					{
						int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("CacheTime");
						YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (YSWL.MALL.Model.Shop.PrePro.PreProduct)objModel;
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
		public List<YSWL.MALL.Model.Shop.PrePro.PreProduct> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<YSWL.MALL.Model.Shop.PrePro.PreProduct> DataTableToList(DataTable dt)
		{
			List<YSWL.MALL.Model.Shop.PrePro.PreProduct> modelList = new List<YSWL.MALL.Model.Shop.PrePro.PreProduct>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				YSWL.MALL.Model.Shop.PrePro.PreProduct model;
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
        /// 批量更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <returns></returns>
	    public bool UpdateStatus(string ids,int status)
	    {
	        return dal.UpdateStatus(ids, status);
	    }
        /// <summary>
        /// 该商品是否存在
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
	    public bool IsExists(long productId)
	    {
	        return dal.IsExists(productId);
	    }
        /// <summary>
        ///获取所有的预订商品
        /// </summary>
        /// <returns></returns>
	    public int GetTotalCount()
	    {
	        return dal.GetTotalCount();
	    }
        /// <summary>
        /// 分类获取数据
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="orderby"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public List<YSWL.MALL.Model.Shop.PrePro.PreProduct> GetListByPage(int cid, string orderby, int startIndex, int endIndex)
        {
            switch (orderby)
            {
                case "default":
                    orderby = " PreProId DESC ";
                    break;
                case "hot":
                    orderby = " BuyCount DESC ";
                    break;
                case "new":
                    orderby = "PreStartDate desc ";
                    break;
                case "price":
                    orderby = "PreAmount   ";
                    break;
                default:
                    orderby = "PreProId desc";
                    break;
            }
            DataSet ds = dal.GetListByPage(cid, orderby, startIndex, endIndex);
            return DataTableToListEx(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.PrePro.PreProduct> DataTableToListEx(DataTable dt)
        {
            List<YSWL.MALL.Model.Shop.PrePro.PreProduct> modelList = new List<YSWL.MALL.Model.Shop.PrePro.PreProduct>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Shop.PrePro.PreProduct model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = dal.DataRowToModelEx(dt.Rows[n]);
                    if (model != null)
                    {
                        modelList.Add(model);
                    }
                }
            }
            return modelList;
        }

        /// <summary>
        /// 根据商品ID获取预订商品数据信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public YSWL.MALL.Model.Shop.PrePro.PreProduct GetModelInfo(long productId)
        {
            return dal.GetModelInfo(productId);
        }
	    #endregion  ExtensionMethod
	}
}

