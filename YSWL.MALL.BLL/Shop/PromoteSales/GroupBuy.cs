﻿/**
* GroupBuy.cs
*
* 功 能： N/A
* 类 名： GroupBuy
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/10/14 15:51:55   N/A    初版
*
* Copyright (c) 2012-2013 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Collections.Generic;
using YSWL.Common;
using YSWL.MALL.Model.Shop.PromoteSales;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Shop.PromoteSales;
using System.Text;
namespace YSWL.MALL.BLL.Shop.PromoteSales
{
    /// <summary>
    /// GroupBuy
    /// </summary>
    public partial class GroupBuy
    {
        private readonly IGroupBuy dal = DAShopProSales.CreateGroupBuy();
        public GroupBuy()
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
        public bool Exists(int GroupBuyId)
        {
            return dal.Exists(GroupBuyId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.MALL.Model.Shop.PromoteSales.GroupBuy model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.Shop.PromoteSales.GroupBuy model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int GroupBuyId)
        {

            return dal.Delete(GroupBuyId);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string GroupBuyIdlist)
        {
            return dal.DeleteList(Common.Globals.SafeLongFilter(GroupBuyIdlist, 0));
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.PromoteSales.GroupBuy GetModel(int GroupBuyId)
        {

            return dal.GetModel(GroupBuyId);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public YSWL.MALL.Model.Shop.PromoteSales.GroupBuy GetModelByCache(int GroupBuyId)
        {

            string CacheKey = "GroupBuyModel-" + GroupBuyId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(GroupBuyId);
                    if (objModel != null)
                    {
                        int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("CacheTime");
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.Shop.PromoteSales.GroupBuy)objModel;
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
        public List<YSWL.MALL.Model.Shop.PromoteSales.GroupBuy> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.PromoteSales.GroupBuy> DataTableToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Shop.PromoteSales.GroupBuy> modelList = new List<YSWL.MALL.Model.Shop.PromoteSales.GroupBuy>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Shop.PromoteSales.GroupBuy model;
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
        public int MaxSequence()
        {
            return dal.MaxSequence();
        }
        /// <summary>
        /// 是否在活动期间内
        /// </summary>
        /// <param name="countId"></param>
        /// <returns></returns>
        public bool IsActivity(int buyId)
        {
            YSWL.MALL.Model.Shop.PromoteSales.GroupBuy model = GetModelByCache(buyId);
            if (model == null)
                return false;
            return model.EndDate >= DateTime.Now && model.StartDate <= DateTime.Now;
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExists(long ProductId)
        {
            return dal.IsExists(ProductId);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        public bool UpdateStatus(string ids, int status)
        {
            return dal.UpdateStatus(ids, status);
        }

        /// <summary>
        /// 更新购买的数量
        /// </summary>
        /// <param name="buyId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool UpdateBuyCount(int buyId, int count)
        {
            return dal.UpdateBuyCount(buyId, count);
        }

        public int GetCount(int cid, int regionId)
        {
            StringBuilder sb = new StringBuilder();
            if (cid > 0)//有选择分类
            {
                //  sb.AppendFormat(" ProductCategory='{0}'", cate);
                sb.AppendFormat(" (CategoryPath LIKE (SELECT Path FROM PMS_Categories WHERE CategoryId={0})+'|%'  ", cid);
                sb.AppendFormat(" OR T.CategoryId = {0})", cid);
            }
            if (regionId > 0)//有选择地区
            {
                if (!String.IsNullOrWhiteSpace(sb.ToString()))
                {
                    sb.Append(" And ");
                }
                sb.AppendFormat(" (T.RegionId={0} or ParentId={0} )", regionId);
            }
            return dal.GetCount(sb.ToString(), regionId);
        }

        public List<Model.Shop.PromoteSales.GroupBuy> GetListByPage(string strWhere, int cid, int regionId, string orderby, int startIndex, int endIndex)
        {
            switch (orderby)
            {
                case "default":
                    orderby = " Sequence DESC ";
                    break;
                case "hot":
                    orderby = " BuyCount DESC ";
                    break;
                case "new":
                    orderby = "StartDate desc ";
                    break;
                case "price":
                    orderby = "Price ";
                    break;
                default:
                    orderby = "ProductId desc";
                    break;
            }
            DataSet ds = dal.GetListByPage(strWhere, cid, regionId, orderby, startIndex, endIndex);
            return DataTableToList(ds.Tables[0]);
        }

        public List<Model.Shop.PromoteSales.GroupBuy> GetCategory(string strWhere)
        {
            DataSet ds = dal.GetCategory(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        //public YSWL.MALL.Model.Shop.PromoteSales.CountDown GetActModel(long ProductId)
        //{
        //    return dal.GetActModel(ProductId);
        //}

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public List<Model.Shop.PromoteSales.GroupBuy> GetList(int Top, int cid, int regionId, string filedOrder) {
            DataSet ds = dal.GetList(Top, cid, regionId, filedOrder);
            return DataTableToList(ds.Tables[0]);
        }
        #endregion  ExtensionMethod
    }
}

