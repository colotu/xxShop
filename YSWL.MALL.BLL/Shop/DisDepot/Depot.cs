/**  版本信息模板在安装目录下，可自行修改。
* Depot.cs
*
* 功 能： N/A
* 类 名： Depot
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/7/27 17:36:54   N/A    初版
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
using System.Linq;
using YSWL.Common;
using YSWL.MALL.Model.Shop.DisDepot;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Shop.DisDepot;
namespace YSWL.MALL.BLL.Shop.DisDepot
{
	/// <summary>
	/// Depot
	/// </summary>
	public partial class Depot
	{
        private readonly IDepot dal = DAShopDisDepot.CreateDepot();
		public Depot()
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
		public bool Exists(int DepotId)
		{
			return dal.Exists(DepotId);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(YSWL.MALL.Model.Shop.DisDepot.Depot model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(YSWL.MALL.Model.Shop.DisDepot.Depot model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int DepotId)
		{
			return dal.Delete(DepotId);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string DepotIdlist )
		{
			return dal.DeleteList(DepotIdlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public YSWL.MALL.Model.Shop.DisDepot.Depot GetModel(int DepotId)
		{
			
			return dal.GetModel(DepotId);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public YSWL.MALL.Model.Shop.DisDepot.Depot GetModelByCache(int DepotId)
		{
			
			string CacheKey = "DepotModel-" + DepotId;
			object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(DepotId);
					if (objModel != null)
					{
						int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("CacheTime");
						YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (YSWL.MALL.Model.Shop.DisDepot.Depot)objModel;
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
		public List<YSWL.MALL.Model.Shop.DisDepot.Depot> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<YSWL.MALL.Model.Shop.DisDepot.Depot> DataTableToList(DataTable dt)
		{
			List<YSWL.MALL.Model.Shop.DisDepot.Depot> modelList = new List<YSWL.MALL.Model.Shop.DisDepot.Depot>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				YSWL.MALL.Model.Shop.DisDepot.Depot model;
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
        /// 添加仓库，事务创建仓库商品表，仓库商品库存表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
	    public int AddEx(YSWL.MALL.Model.Shop.DisDepot.Depot model)
	    {
	        return dal.AddEx(model);
	    }


        /// <summary>
        /// 是否存在该编码
        /// </summary>
        /// <param name="code"></param>
        /// <param name="depotId"></param>
        /// <returns></returns>
        public bool IsExistCode(string code, int depotId = 0)
        {
            return dal.IsExistCode(code, depotId);
        }

        /// <summary>
        /// 删除仓库信息
        /// </summary>
        public bool DeleteEx(int depotId) { 
            return dal.DeleteEx(depotId);
        }

        /// <summary>
        /// 获取所有的仓库
        /// </summary>
        /// <returns></returns>
        public static List<YSWL.MALL.Model.Shop.DisDepot.Depot> GetAllDepots()
        {
            string CacheKey = "GetAllDepots-Depot";
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    YSWL.MALL.BLL.Shop.DisDepot.Depot bll = new Depot();
                    objModel = bll.GetModelList("");
                    if (objModel != null)
                    {
                        int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("ModelCache");
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (List<YSWL.MALL.Model.Shop.DisDepot.Depot>)objModel;
        }
        /// <summary>
        /// 获取可用的仓库列表
        /// </summary>
        /// <returns></returns>
	    public static List<YSWL.MALL.Model.Shop.DisDepot.Depot> GetAvaDepots()
        {
            List<YSWL.MALL.Model.Shop.DisDepot.Depot> allList = GetAllDepots();
            if (allList == null || allList.Count == 0)
            {
                return null;
            }
            return allList.Where(c => c.Status == 1).ToList();
        }

        /// <summary>
        /// 获取默认仓库
        /// </summary>
        /// <returns></returns>
	    public static int GetDefaultDepot()
	    {
            string CacheKey = "OMS_DefaultDepot";
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
	        if (objModel==null)
	        {
	            try
	            {
                    int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("ModelCache");
                    objModel = Common.Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValue("OMS_DefaultDepot"), 0);
                    YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                }
	            catch
	            {
	                objModel = -1;
	            }
	        }
	        return YSWL.Common.Globals.SafeInt(objModel, -1);
	    }
        #endregion  ExtensionMethod
    }
}

