/**
* Favorite.cs
*
* 功 能： N/A
* 类 名： Favorite
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/6/22 15:32:12   N/A    初版
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
using System.Text;
using YSWL.Common;
using YSWL.MALL.Model.Shop;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Shop;
namespace YSWL.MALL.BLL.Shop
{
    /// <summary>
    /// Favorite
    /// </summary>
    public partial class Favorite
    {
        private readonly IFavorite dal = DAShop.CreateFavorite();
        public Favorite()
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
        public bool Exists(int FavoriteId)
        {
            return dal.Exists(FavoriteId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.MALL.Model.Shop.Favorite model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.Shop.Favorite model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int FavoriteId)
        {

            return dal.Delete(FavoriteId);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string FavoriteIdlist)
        {
            return dal.DeleteList(Common.Globals.SafeLongFilter(FavoriteIdlist,0) );
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.Favorite GetModel(int FavoriteId)
        {

            return dal.GetModel(FavoriteId);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public YSWL.MALL.Model.Shop.Favorite GetModelByCache(int FavoriteId)
        {

            string CacheKey = "FavoriteModel-" + FavoriteId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(FavoriteId);
                    if (objModel != null)
                    {
                    int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.Shop.Favorite)objModel;
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
        public List<YSWL.MALL.Model.Shop.Favorite> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Favorite> DataTableToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Shop.Favorite> modelList = new List<YSWL.MALL.Model.Shop.Favorite>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Shop.Favorite model;
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
        #region 扩展方法
        public DataSet GetListEX(int userid, string keyword)
        {
            StringBuilder strWhere = new StringBuilder();
            if (userid >0)
            {
                strWhere.AppendFormat("userid={0}", userid);
                if (!String.IsNullOrWhiteSpace(keyword))
                {
                    strWhere.AppendFormat("and Tags like '%{0}%' or Remark like '%{0}%'", Common.InjectionFilter.SqlFilter(keyword));
                }
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(keyword))
                {
                    strWhere.AppendFormat("Tags like '%{0}%' or Remark like '%{0}%'", Common.InjectionFilter.SqlFilter(keyword));
                }
                else
                {
                    return GetAllList();
                }
            }

            return dal.GetList(strWhere.ToString());
        }

        /// <summary>
        /// 分页获取收藏商品列表 
        /// </summary>
        public DataSet GetProductListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            return dal.GetProductListByPage(strWhere, orderby, startIndex, endIndex);
        }
        /// <summary>
        /// 分页获取收藏商品列表 
        /// </summary>
        public List<YSWL.MALL.ViewModel.Shop.FavoProdModel> GetFavoriteProductListByPage(string strWhere, int startIndex, int endIndex)
        {   //FavoriteId,  CreatedDate ,  ProductId ,  ProductName,  SaleStatus     MarketPrice
            DataSet ds= dal.GetProductListByPage(strWhere, "CreatedDate desc ", startIndex, endIndex);
            DataTable dt = ds.Tables[0];
            List<YSWL.MALL.ViewModel.Shop.FavoProdModel> modelList = new List<YSWL.MALL.ViewModel.Shop.FavoProdModel>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.ViewModel.Shop.FavoProdModel model;
                for (int n = 0; n < rowsCount; n++)
                {
                    DataRow row = dt.Rows[n];
                    model = new YSWL.MALL.ViewModel.Shop.FavoProdModel();
                    if (row != null)
                    {
                        if (row["FavoriteId"] != null && row["FavoriteId"].ToString() != "")
                        {
                            model.FavoriteId = int.Parse(row["FavoriteId"].ToString());
                        }
                        if (row["ProductId"] != null && row["ProductId"].ToString() != "")
                        {
                            model.ProductId = long.Parse(row["ProductId"].ToString());
                        }
                        if (row["ProductName"] != null )
                        {
                            model.ProductName =  row["ProductName"].ToString() ;
                        }
                        if (row["SaleStatus"] != null && row["SaleStatus"].ToString() != "")
                        {
                            model.SaleStatus =int.Parse(row["SaleStatus"].ToString());
                        }
                        if (row["CreatedDate"] != null && row["CreatedDate"].ToString() != "")
                        {
                            model.CreatedDate = DateTime.Parse(row["CreatedDate"].ToString());
                        }
                        if (row["ThumbnailUrl1"] != null )
                        {
                            model.ThumbnailUrl1 = row["ThumbnailUrl1"].ToString();
                        }
                        if (row["LowestSalePrice"] != null && row["LowestSalePrice"].ToString() != "")
                        {
                            model.LowestSalePrice = decimal.Parse(row["LowestSalePrice"].ToString());
                        }
                        if (dt.Rows[n]["MarketPrice"] != null && dt.Rows[n]["MarketPrice"].ToString() != "")
                        {
                            model.MarketPrice = decimal.Parse(dt.Rows[n]["MarketPrice"].ToString());
                        }
                        if (dt.Rows[n]["SupplierId"] != null && dt.Rows[n]["SupplierId"].ToString() != "")
                        {
                            model.SupplierId = int.Parse(dt.Rows[n]["SupplierId"].ToString());
                        }
                        if (dt.Rows[n]["HasSKU"] != null && dt.Rows[n]["HasSKU"].ToString() != "")
                        {
                            if ((dt.Rows[n]["HasSKU"].ToString() == "1") || (dt.Rows[n]["HasSKU"].ToString().ToLower() == "true"))
                            {
                                model.HasSKU = true;
                            }
                            else
                            {
                                model.HasSKU = false;
                            }
                        }
                        modelList.Add(model);
                    } 
                }
            }
            return modelList;
        }

        public bool Exists(long targetId,int userId,int type)
        {
            return dal.Exists(targetId, userId,type);
        }

        public int GetCount(int targetId, int type)
        {
            return GetRecordCount(" Type=1 And TargetId=" + targetId);
        }

        public List<YSWL.MALL.ViewModel.Shop.FavoBuyProdModel> GetBuyListByPage(string strWhere, string orderby,
                                                                             int startIndex, int endIndex)
        {
            DataSet ds = dal.GetBuyListByPage(strWhere, "CreatedDate desc ", startIndex, endIndex);
            DataTable dt = ds.Tables[0];
            List<YSWL.MALL.ViewModel.Shop.FavoBuyProdModel> modelList = new List<YSWL.MALL.ViewModel.Shop.FavoBuyProdModel>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.ViewModel.Shop.FavoBuyProdModel model;
                for (int n = 0; n < rowsCount; n++)
                {
                    DataRow row = dt.Rows[n];
                    model = new YSWL.MALL.ViewModel.Shop.FavoBuyProdModel();
                    if (row != null)
                    {
                        if (row["FavoriteId"] != null && row["FavoriteId"].ToString() != "")
                        {
                            model.FavoriteId = int.Parse(row["FavoriteId"].ToString());
                        }
                        if (row["ProductId"] != null && row["ProductId"].ToString() != "")
                        {
                            model.ProductId = long.Parse(row["ProductId"].ToString());
                        }
                        if (row["ProductName"] != null)
                        {
                            model.ProductName = row["ProductName"].ToString();
                        }
                        if (row["SaleStatus"] != null && row["SaleStatus"].ToString() != "")
                        {
                            model.SaleStatus = int.Parse(row["SaleStatus"].ToString());
                        }
                        if (row["CreatedDate"] != null && row["CreatedDate"].ToString() != "")
                        {
                            model.CreatedDate = DateTime.Parse(row["CreatedDate"].ToString());
                        }
                        if (row["ThumbnailUrl1"] != null)
                        {
                            model.ThumbnailUrl1 = row["ThumbnailUrl1"].ToString();
                        }
                        if (row["ProductCode"] != null)
                        {
                            model.ProductCode = row["ProductCode"].ToString();
                        }
                        modelList.Add(model);
                    }
                }
            }
            return modelList;
        }
        /// <summary>
        /// 获取收藏id
        /// </summary>
        /// <param name="targetId"></param>
        /// <param name="UserId"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public int GetFavoriteId(long targetId, int UserId, int Type)
        {
            return dal.GetFavoriteId(targetId, UserId, Type);
        }

        /// <summary>
        /// 批量收藏
        /// </summary>
        /// <param name="idList"></param>
        /// <param name="currentUser"></param>
        public void BatchFavor(string idList,YSWL.Accounts.Bus.User currentUser)
        {
            idList = YSWL.Common.InjectionFilter.SqlFilter(idList);

            YSWL.MALL.BLL.Shop.Favorite favBll = new BLL.Shop.Favorite();
            foreach (string productId in idList.Split(','))
            {
                YSWL.MALL.Model.Shop.Favorite model = new Model.Shop.Favorite();
                model.Type = 1;
                model.TargetId = YSWL.Common.Globals.SafeInt(productId,0) ;
                model.UserId=currentUser.UserID;
                model.CreatedDate = System.DateTime.Now;
                dal.BatchFavor(model);
            }
        }

        /// <summary>
        ///  获取记录总数
        /// </summary>
        /// <param name="type">1:商品  2:店铺</param>
        /// <param name="targetId"></param>
        /// <returns></returns>
        public int GetRecordCount(int type, int targetId)
        {
            return dal.GetRecordCount(type, targetId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool AddEx(Model.Shop.Favorite model)
        {
            bool result = dal.AddEx(model);
            if (result && model.Type== (int)FavoriteEnums.Store)
            {
               DataCache.DeleteCache("SuppliersModel-" + model.TargetId);
            }
            return result;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteEx(YSWL.MALL.Model.Shop.Favorite model)
        {
            bool result = dal.DeleteEx(model);
            if (result && model.Type == (int)FavoriteEnums.Store)
            {
                DataCache.DeleteCache("SuppliersModel-" + model.TargetId);
            }
            return result;
        }
        /// <summary>
        /// 分页获取收藏店铺列表 
        /// </summary>
        public List<YSWL.MALL.ViewModel.Shop.FavoStoreModel> GetStoreListByPage(int userId, int startIndex, int endIndex)
        {
            DataSet ds = dal.GetStoreListByPage(userId, "CreatedDate desc ", startIndex, endIndex);
            DataTable dt = ds.Tables[0];
            List<YSWL.MALL.ViewModel.Shop.FavoStoreModel> modelList = new List<YSWL.MALL.ViewModel.Shop.FavoStoreModel>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.ViewModel.Shop.FavoStoreModel model;
                for (int n = 0; n < rowsCount; n++)
                {
                    DataRow row = dt.Rows[n];
                    model = new YSWL.MALL.ViewModel.Shop.FavoStoreModel();
                    if (row != null)
                    {
                        if (row["FavoriteId"] != null && row["FavoriteId"].ToString() != "")
                        {
                            model.FavoriteId = int.Parse(row["FavoriteId"].ToString());
                        }
                        if (row["SupplierId"] != null && row["SupplierId"].ToString() != "")
                        {
                            model.SupplierId = int.Parse(row["SupplierId"].ToString());
                        }
                        if (row["ShopName"] != null)
                        {
                            model.ShopName = row["ShopName"].ToString();
                        }
                        if (row["SalesCount"] != null && row["SalesCount"].ToString() != "")
                        {
                            model.SalesCount = int.Parse(row["SalesCount"].ToString());
                        }
                        if (row["CreatedDate"] != null && row["CreatedDate"].ToString() != "")
                        {
                            model.CreatedDate = DateTime.Parse(row["CreatedDate"].ToString());
                        }
                        if (row["StoreStatus"] != null && row["StoreStatus"].ToString() != "")
                        {
                            model.StoreStatus = int.Parse(row["StoreStatus"].ToString());
                        }
                        if (row["Status"] != null && row["Status"].ToString() != "")
                        {
                            model.Status = int.Parse(row["Status"].ToString());
                        }
                        modelList.Add(model);
                    }
                }
            }
            return modelList;
        }
        /// <summary>
        /// 根据targetId ,userId,类型 删除一条数据
        /// </summary>
        public bool Delete(int targetId, int userId, int type) {
            bool result = dal.Delete(targetId, userId, type);
            if (result && type == (int)FavoriteEnums.Store)
            {
                DataCache.DeleteCache("SuppliersModel-" + targetId);
            }
            return result;
        }

        /// <summary>
        /// 获取被收藏商品总数 已去重
        /// </summary>
        /// <returns></returns>
        public int FavProductCount()
        {
            return dal.FavProductCount();
        }
        /// <summary>
        /// 获取收藏店铺数
        /// </summary>
        public int GetStoreRecordCount(int userId) {
            return dal.GetStoreRecordCount(userId);
        }
        #endregion
    }
}

