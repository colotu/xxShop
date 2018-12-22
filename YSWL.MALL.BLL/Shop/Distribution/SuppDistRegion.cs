/**  版本信息模板在安装目录下，可自行修改。
* SuppDistRegion.cs
*
* 功 能： N/A
* 类 名： SuppDistRegion
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/1/26 18:31:53   N/A    初版
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
using System.Linq;
namespace YSWL.MALL.BLL.Shop.Distribution
{
    /// <summary>
    /// SuppDistRegion
    /// </summary>
    public partial class SuppDistRegion
    {
        private readonly ISuppDistRegion dal = DAShopDist.CreateSuppDistRegion();
        public SuppDistRegion()
        { }
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
        public bool Exists(int SupplierId, int RegionId)
        {
            return dal.Exists(SupplierId, RegionId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(YSWL.MALL.Model.Shop.Distribution.SuppDistRegion model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.Shop.Distribution.SuppDistRegion model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int SupplierId, int RegionId)
        {

            return dal.Delete(SupplierId, RegionId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.Distribution.SuppDistRegion GetModel(int SupplierId, int RegionId)
        {

            return dal.GetModel(SupplierId, RegionId);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public YSWL.MALL.Model.Shop.Distribution.SuppDistRegion GetModelByCache(int SupplierId, int RegionId)
        {

            string CacheKey = "SuppDistRegionModel-" + SupplierId + RegionId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(SupplierId, RegionId);
                    if (objModel != null)
                    {
                        int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("CacheTime");
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.Shop.Distribution.SuppDistRegion)objModel;
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
        public List<YSWL.MALL.Model.Shop.Distribution.SuppDistRegion> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Distribution.SuppDistRegion> DataTableToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Shop.Distribution.SuppDistRegion> modelList = new List<YSWL.MALL.Model.Shop.Distribution.SuppDistRegion>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Shop.Distribution.SuppDistRegion model;
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
        public static List<YSWL.MALL.Model.Shop.Distribution.SuppDistRegion> GetAllDistRegion()
        {
            string CacheKey = "GetAllDistRegion-";
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    YSWL.MALL.BLL.Shop.Distribution.SuppDistRegion bll = new SuppDistRegion();
                    objModel = bll.GetModelList("");
                    if (objModel != null)
                    {
                        int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("CacheTime");
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (List<YSWL.MALL.Model.Shop.Distribution.SuppDistRegion>)objModel;
        }
        /// <summary>
        /// 根据区域获取分销商Id
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public static int GetDistSuppId(int regionId)
        {
            //分销商的地区数据不会很多，最多每个市一个分销
            List<YSWL.MALL.Model.Shop.Distribution.SuppDistRegion> AllDistRegion = GetAllDistRegion();
            YSWL.MALL.BLL.Ms.Regions regionBll = new Ms.Regions();
            YSWL.MALL.Model.Ms.Regions regionModel = regionBll.GetModel(regionId);
            var regionList = regionModel.Path.Split(',').ToList();
            regionList.Insert(0, regionId.ToString());
            YSWL.MALL.Model.Shop.Distribution.SuppDistRegion distRegion = AllDistRegion.Where(c => regionList.Contains(c.RegionId.ToString())).FirstOrDefault();
            return distRegion == null ? 0 : distRegion.SupplierId;
        }
        #endregion  ExtensionMethod
    }
}

