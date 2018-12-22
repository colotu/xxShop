/**  版本信息模板在安装目录下，可自行修改。
* DepotRegion.cs
*
* 功 能： N/A
* 类 名： DepotRegion
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/7/27 17:36:56   N/A    初版
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
	/// DepotRegion
	/// </summary>
	public partial class DepotRegion
	{
        private readonly IDepotRegion dal = DAShopDisDepot.CreateDepotRegion();
		public DepotRegion()
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
		public bool Exists(int DepotId,int RegionId)
		{
			return dal.Exists(DepotId,RegionId);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(YSWL.MALL.Model.Shop.DisDepot.DepotRegion model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(YSWL.MALL.Model.Shop.DisDepot.DepotRegion model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int DepotId,int RegionId)
		{
			
			return dal.Delete(DepotId,RegionId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public YSWL.MALL.Model.Shop.DisDepot.DepotRegion GetModel(int DepotId,int RegionId)
		{
			
			return dal.GetModel(DepotId,RegionId);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public YSWL.MALL.Model.Shop.DisDepot.DepotRegion GetModelByCache(int DepotId,int RegionId)
		{
			
			string CacheKey = "DepotRegionModel-" + DepotId+RegionId;
			object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(DepotId,RegionId);
					if (objModel != null)
					{
						int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("CacheTime");
						YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (YSWL.MALL.Model.Shop.DisDepot.DepotRegion)objModel;
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
		public List<YSWL.MALL.Model.Shop.DisDepot.DepotRegion> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<YSWL.MALL.Model.Shop.DisDepot.DepotRegion> DataTableToList(DataTable dt)
		{
			List<YSWL.MALL.Model.Shop.DisDepot.DepotRegion> modelList = new List<YSWL.MALL.Model.Shop.DisDepot.DepotRegion>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				YSWL.MALL.Model.Shop.DisDepot.DepotRegion model;
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

        public static List<YSWL.MALL.Model.Shop.DisDepot.DepotRegion> GetAllDepotRegion()
        {
            string CacheKey = "GetAllDepotRegion-DepotRegion";
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    YSWL.MALL.BLL.Shop.DisDepot.DepotRegion bll = new DepotRegion();
                    objModel = bll.GetModelList("");
                    if (objModel != null)
                    {
                        int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("CacheTime");
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (List<YSWL.MALL.Model.Shop.DisDepot.DepotRegion>)objModel;
        }

        /// <summary>
        ///  根据地区Id 获取仓库信息
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public static int GetDepotByRegion(int regionId)
        {
            //加上缓存
            string CacheKey = "GetDepotByRegion-" + regionId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    //获取所有的地区仓库
                    List<YSWL.MALL.Model.Shop.DisDepot.DepotRegion> AllDepotRegion = GetAllDepotRegion();
                    YSWL.MALL.BLL.Ms.Regions regionBll = new Ms.Regions();
                    YSWL.MALL.Model.Ms.Regions regionModel = regionBll.GetModel(regionId);
                    if (regionModel == null) {
                        return  Common.Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValue("OMS_DefaultDepot"), 0); //加载默认仓库
                    }
                    var regionList = regionModel.Path.Split(',').ToList();
                    regionList.Insert(0, regionId.ToString());
                    YSWL.MALL.Model.Shop.DisDepot.DepotRegion distRegion =
                        AllDepotRegion.Find(c => regionList.Contains(c.RegionId.ToString()));

                    int defaultDepotId = Common.Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValue("OMS_DefaultDepot"), 0); //加载默认仓库

                    objModel = distRegion == null ? defaultDepotId : distRegion.DepotId;
                 
                    if (objModel != null)
                    {
                        int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("CacheTime");
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (int)objModel;
        }
        /// <summary>
        /// 存在地区仓库
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
	    public static int ExistsDepotByRegion(int regionId)
	    {
            //获取所有的地区仓库
            List<YSWL.MALL.Model.Shop.DisDepot.DepotRegion> AllDepotRegion = GetAllDepotRegion();
            YSWL.MALL.BLL.Ms.Regions regionBll = new Ms.Regions();
            YSWL.MALL.Model.Ms.Regions regionModel = regionBll.GetModel(regionId);
            if (regionModel == null)
            {
                return 0;
            }
            var regionList = regionModel.Path.Split(',').ToList();
            regionList.Insert(0, regionId.ToString());
            YSWL.MALL.Model.Shop.DisDepot.DepotRegion distRegion =
                AllDepotRegion.Find(c => regionList.Contains(c.RegionId.ToString()));

           // int defaultDepotId = Common.Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValue("OMS_DefaultDepot"), 0); //加载默认仓库

            return distRegion == null ? 0 : distRegion.DepotId;
	    }

	    /// <summary>
        /// 清空缓存
        /// </summary>
        /// <param name="depotId"></param>
        /// <param name="regionId"></param>
        public  void ClearCache(int depotId, int regionId)
        {
            Common.DataCache.DeleteCache("GetDepotByRegion-" + regionId);
            Common.DataCache.DeleteCache("GetAllDepotRegion-DepotRegion");
            Common.DataCache.DeleteCache("DepotRegionModel-" + depotId + regionId);
            Common.DataCache.DeleteCache("StockHelper_StockCache_SKU_");
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public int GetRecordCount(int depotId)
        {
            return dal.GetRecordCount(string.Format(" DepotId={0}  ",depotId));
        }
        #endregion  ExtensionMethod
	}
}

