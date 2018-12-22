/*----------------------------------------------------------------

// Copyright (C) 2012 云商未来 版权所有。
//
// 文件名：Products.cs
// 文件功能描述：
//
// 创建标识： [Ben]  2012/06/11 20:36:27
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using YSWL.Common;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Shop.Products;
using YSWL.MALL.Model.Shop.Products;
using YSWL.TaoBao;
using YSWL.TaoBao.Request;
using YSWL.TaoBao.Response;
using System.Linq;

namespace YSWL.MALL.BLL.Shop.Products
{
    /// <summary>
    /// ProductInfo
    /// </summary>
    public partial class ProductInfo
    {
        private readonly IProductInfo dal = DAShopProducts.CreateProductInfo();

        public ProductInfo()
        { }

        #region Method

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long ProductId)
        {
            return dal.Exists(ProductId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public long Add(YSWL.MALL.Model.Shop.Products.ProductInfo model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.Shop.Products.ProductInfo model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(long ProductId)
        {
            return dal.Delete(ProductId);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string ProductIdlist)
        {
            return dal.DeleteList(Common.Globals.SafeLongFilter(ProductIdlist,0) );
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.Products.ProductInfo GetModel(long ProductId)
        {
            return dal.GetModel(ProductId);
        }
       
        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public YSWL.MALL.Model.Shop.Products.ProductInfo GetModelByCache(long ProductId)
        {
            string CacheKey = "ProductsModel-" + ProductId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(ProductId);
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.Shop.Products.ProductInfo)objModel;
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
        public List<YSWL.MALL.Model.Shop.Products.ProductInfo> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return dal.DataTableToList(ds.Tables[0]);
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

        #endregion Method

        #region NewMethod

        /// <summary>
        /// 批量处理状态
        /// </summary>
        /// <param name="IDlist"></param>
        /// <param name="strSetValue"></param>
        /// <returns></returns>
        public bool UpdateList(string IDlist, YSWL.MALL.Model.Shop.Products.ProductSaleStatus saleStatus)
        {
            if (string.IsNullOrEmpty(IDlist))
            {
                return false;
            }
            string strWhere = $" SaleStatus ={(int) saleStatus}";
            bool result = dal.UpdateList(IDlist, strWhere);
            if (result)
            {
                #region  同步所有分仓商品的上下架状态
                result=SubUpdateList(IDlist, strWhere);
                #endregion 
                //清除缓存
                ClearCache(IDlist);
            }
            return result;
        }

        public bool SubUpdateList(string IDlist, string strWhere)
        {
            List<YSWL.MALL.Model.Shop.DisDepot.Depot> depotList = DisDepot.Depot.GetAvaDepots();
            if (depotList == null || depotList.Count <= 0) {
                return true;
            }
            string depotIds = string.Join(",", depotList.Select(t => t.DepotId));
            return dal.SubUpdateList(depotIds, IDlist, strWhere);
        }

        public bool UpdateProductName(long productId, string productName)
        {
            bool result = dal.UpdateProductName(productId, productName);
            if (result)
            {
                //清除缓存
                ClearCache(productId);
            }
            return result;
        }

        public DataSet GetListByCategoryIdSaleStatus(Model.Shop.Products.ProductInfo model,int RegionId=0)
        {
            if (model == null) return null;
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" WHERE CategoryId={0}", model.CategoryId);
            strWhere.AppendFormat(" AND SaleStatus <>{0}", model.SaleStatus);
            if (!string.IsNullOrWhiteSpace(model.ProductName))
            {
                strWhere.AppendFormat(" AND ProductName LIKE '%{0}%'", Common.InjectionFilter.SqlFilter(model.ProductName));
            }
            int SupplierId = 0;
            if (RegionId > 0)
            {
               SupplierId= YSWL.MALL.BLL.Shop.Distribution.SuppDistRegion.GetDistSuppId(RegionId);
            }

            return dal.GetListByCategoryIdSaleStatus(strWhere.ToString(), SupplierId);
        }

        /// <summary>
        /// 商品导出数据列表
        /// </summary>
        public DataSet GetListByExport(int SaleStatus, string ProductName, int CategoryId, string SKU, int BrandId)
        {
            return dal.GetListByExport(SaleStatus, ProductName, CategoryId, SKU, BrandId);
        }

        public List<YSWL.MALL.Model.Shop.Products.ProductInfo> SearchProducts(int cateId, Model.Shop.Products.ProductSearch model)
        {
            DataSet ds = dal.SearchProducts(cateId, model);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ProductDataTableToList(ds.Tables[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Products.ProductInfo> ProductDataTableToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> modelList = new List<YSWL.MALL.Model.Shop.Products.ProductInfo>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Shop.Products.ProductInfo model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new YSWL.MALL.Model.Shop.Products.ProductInfo();
                    if (dt.Rows[n]["CategoryId"] != null && dt.Rows[n]["CategoryId"].ToString() != "")
                    {
                        model.CategoryId = int.Parse(dt.Rows[n]["CategoryId"].ToString());
                    }
                    if (dt.Rows[n]["TypeId"] != null && dt.Rows[n]["TypeId"].ToString() != "")
                    {
                        model.TypeId = int.Parse(dt.Rows[n]["TypeId"].ToString());
                    }
                    if (dt.Rows[n]["ProductId"] != null && dt.Rows[n]["ProductId"].ToString() != "")
                    {
                        model.ProductId = long.Parse(dt.Rows[n]["ProductId"].ToString());
                    }
                    if (dt.Rows[n]["BrandId"] != null && dt.Rows[n]["BrandId"].ToString() != "")
                    {
                        model.BrandId = int.Parse(dt.Rows[n]["BrandId"].ToString());
                    }
                    if (dt.Rows[n]["ProductName"] != null && dt.Rows[n]["ProductName"].ToString() != "")
                    {
                        model.ProductName = dt.Rows[n]["ProductName"].ToString();
                    }
                    if (dt.Rows[n]["ProductCode"] != null && dt.Rows[n]["ProductCode"].ToString() != "")
                    {
                        model.ProductCode = dt.Rows[n]["ProductCode"].ToString();
                    }
                    if (dt.Rows[n]["SupplierId"] != null && dt.Rows[n]["SupplierId"].ToString() != "")
                    {
                        model.SupplierId = int.Parse(dt.Rows[n]["SupplierId"].ToString());
                    }
                    if (dt.Rows[n]["RegionId"] != null && dt.Rows[n]["RegionId"].ToString() != "")
                    {
                        model.RegionId = int.Parse(dt.Rows[n]["RegionId"].ToString());
                    }
                    if (dt.Rows[n]["ShortDescription"] != null && dt.Rows[n]["ShortDescription"].ToString() != "")
                    {
                        model.ShortDescription = dt.Rows[n]["ShortDescription"].ToString();
                    }
                    if (dt.Rows[n]["Meta_Title"] != null && dt.Rows[n]["Meta_Title"].ToString() != "")
                    {
                        model.Meta_Title = dt.Rows[n]["Meta_Title"].ToString();
                    }
                    if (dt.Rows[n]["Meta_Description"] != null && dt.Rows[n]["Meta_Description"].ToString() != "")
                    {
                        model.Meta_Description = dt.Rows[n]["Meta_Description"].ToString();
                    }
                    if (dt.Rows[n]["Meta_Keywords"] != null && dt.Rows[n]["Meta_Keywords"].ToString() != "")
                    {
                        model.Meta_Keywords = dt.Rows[n]["Meta_Keywords"].ToString();
                    }
                    if (dt.Rows[n]["SaleStatus"] != null && dt.Rows[n]["SaleStatus"].ToString() != "")
                    {
                        model.SaleStatus = int.Parse(dt.Rows[n]["SaleStatus"].ToString());
                    }
                    if (dt.Rows[n]["VistiCounts"] != null && dt.Rows[n]["VistiCounts"].ToString() != "")
                    {
                        model.VistiCounts = int.Parse(dt.Rows[n]["VistiCounts"].ToString());
                    }
                    if (dt.Rows[n]["SaleCounts"] != null && dt.Rows[n]["SaleCounts"].ToString() != "")
                    {
                        model.SaleCounts = int.Parse(dt.Rows[n]["SaleCounts"].ToString());
                    }
                    if (dt.Rows[n]["DisplaySequence"] != null && dt.Rows[n]["DisplaySequence"].ToString() != "")
                    {
                        model.DisplaySequence = int.Parse(dt.Rows[n]["DisplaySequence"].ToString());
                    }
                    if (dt.Rows[n]["MarketPrice"] != null && dt.Rows[n]["MarketPrice"].ToString() != "")
                    {
                        model.MarketPrice = decimal.Parse(dt.Rows[n]["MarketPrice"].ToString());
                    }
                    if (dt.Rows[n]["LowestSalePrice"] != null && dt.Rows[n]["LowestSalePrice"].ToString() != "")
                    {
                        model.LowestSalePrice = decimal.Parse(dt.Rows[n]["LowestSalePrice"].ToString());
                    }
                    if (dt.Rows[n]["PenetrationStatus"] != null && dt.Rows[n]["PenetrationStatus"].ToString() != "")
                    {
                        model.PenetrationStatus = int.Parse(dt.Rows[n]["PenetrationStatus"].ToString());
                    }
                    if (dt.Rows[n]["MainCategoryPath"] != null && dt.Rows[n]["MainCategoryPath"].ToString() != "")
                    {
                        model.MainCategoryPath = dt.Rows[n]["MainCategoryPath"].ToString();
                    }
                    if (dt.Rows[n]["ExtendCategoryPath"] != null && dt.Rows[n]["ExtendCategoryPath"].ToString() != "")
                    {
                        model.ExtendCategoryPath = dt.Rows[n]["ExtendCategoryPath"].ToString();
                    }
                    if (dt.Rows[n]["Points"] != null && dt.Rows[n]["Points"].ToString() != "")
                    {
                        model.Points = decimal.Parse(dt.Rows[n]["Points"].ToString());
                    }
                    if (dt.Rows[n]["ThumbnailUrl1"] != null && dt.Rows[n]["ThumbnailUrl1"].ToString() != "")
                    {
                        model.ThumbnailUrl1 = dt.Rows[n]["ThumbnailUrl1"].ToString();
                    }
                    if (dt.Rows[n]["MaxQuantity"] != null && dt.Rows[n]["MaxQuantity"].ToString() != "")
                    {
                        model.MaxQuantity = int.Parse(dt.Rows[n]["MaxQuantity"].ToString());
                    }
                    if (dt.Rows[n]["MinQuantity"] != null && dt.Rows[n]["MinQuantity"].ToString() != "")
                    {
                        model.MinQuantity = int.Parse(dt.Rows[n]["MinQuantity"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        #endregion NewMethod

        public List<Model.Shop.Products.ProductInfo> GetProductsList(string selectedPids, string pName, string categoryId, int startIdex, int endIndex, out int dataCount, long productId)
        {
            StringBuilder strWhere = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(pName))
            {
                strWhere.AppendFormat(" AND ProductName LIKE '%{0}%'", Common.InjectionFilter.SqlFilter(pName));
            }
            if (!string.IsNullOrWhiteSpace(categoryId))
            {
                strWhere.AppendFormat(" AND ProductId IN( SELECT DISTINCT ProductId FROM PMS_ProductCategories WHERE (CategoryPath LIKE (SELECT Path FROM PMS_Categories WHERE CategoryId={0})+'|%' or CategoryPath='{0}') ) ", categoryId);
            }

            //if (selectedPids != null && selectedPids.Length > 0)
            //{
            //    strWhere.AppendFormat(" AND ProductId NOT IN ({0})", selectedPids);
            //}

            if (!string.IsNullOrWhiteSpace(selectedPids))
            {
                strWhere.AppendFormat(" AND ProductId NOT IN ({0})", selectedPids);
            }

            DataSet ds = dal.GetProductListInfo(strWhere.ToString(), " ORDER BY SaleCounts DESC ", startIdex, endIndex, out dataCount, productId);

            if (Common.DataSetTools.DataSetIsNull(ds)) return null;
            return dal.DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 商品推荐信息列表
        /// </summary>
        public List<Model.Shop.Products.ProductInfo> GetProductNoRecList(int categoryId,int supplierId, string pName, int modeType, int startIndex, int endIndex)
        {
            DataSet ds = dal.GetProductNoRecList(categoryId, supplierId,pName, modeType, startIndex, endIndex);
            if (Common.DataSetTools.DataSetIsNull(ds)) return null;
            return dal.DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 商品推荐信息Count
        /// </summary>
        public int GetProductNoRecCount(int categoryId, string pName, int modeType,int supplierId=0)
        {
            return dal.GetProductNoRecCount(categoryId, pName, modeType, supplierId);
        }

        public List<Model.Shop.Products.ProductInfo> GetCommendProductsList(string[] selectedPids, string pName, string categoryId, int startIdex, int endIndex, out int dataCount, long productId, int? commendType)
        {
            StringBuilder strWhere = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(pName))
            {
                strWhere.AppendFormat(" AND ProductName LIKE '%{0}%'", Common.InjectionFilter.SqlFilter(pName));
            }
            if (!string.IsNullOrWhiteSpace(categoryId))
            {
                strWhere.AppendFormat(" AND CategoryId ={0}", categoryId);
            }

            if (selectedPids != null && selectedPids.Length > 0)
            {
                strWhere.AppendFormat(" AND ProductId NOT IN ({0})", selectedPids);
            }
            if (commendType.HasValue)
            {
                if (commendType.Value == 0)
                {
                    strWhere.AppendFormat(" AND ProductId NOT IN ({0})", selectedPids);
                }
            }

            DataSet ds = dal.GetProductListInfo(strWhere.ToString(), " ORDER BY SaleCounts DESC ", startIdex, endIndex, out dataCount, productId);

            if (Common.DataSetTools.DataSetIsNull(ds)) return null;
            return dal.DataTableToList(ds.Tables[0]);
        }

        public List<Model.Shop.Products.ProductInfo> GetProductsList(int? categoryId, string mod, int startIndex, int endIndex, out int dataCount,int RegionId=0)
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append(" AND P.SaleStatus=1");
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                strWhere.AppendFormat(" AND ( PC.CategoryId='{0}'  or PC.CategoryPath like '{0}|%') ", categoryId);
            }
            switch (mod)
            {
                case "rec":
                    mod = " P.DisplaySequence DESC ";
                    break;
                case "hot":
                    mod = " P.SaleCounts DESC ";
                    break;
                case "new":
                default:
                    mod = null;
                    break;
            }
            int SupplierId = 0;
            if (RegionId > 0)
            {
                SupplierId = YSWL.MALL.BLL.Shop.Distribution.SuppDistRegion.GetDistSuppId(RegionId);
            }
            DataSet ds = dal.GetProductListByCategoryId(categoryId, strWhere.ToString(), mod, startIndex, endIndex, SupplierId,out dataCount);
            if (Common.DataSetTools.DataSetIsNull(ds)) return null;
            return dal.DataTableToList(ds.Tables[0]);
        }

        public List<Model.Shop.Products.ProductInfo> GetProductsListEx(int? parentCategoryId, int? subCategoryId,
            string mod, int startIndex, int endIndex, out int dataCount, int RegionId=0)
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append(" AND P.SaleStatus=1");
            if (parentCategoryId.HasValue && parentCategoryId.Value > 0)
            {
                strWhere.AppendFormat(" AND PC.CategoryPath LIKE '{0}|%' ", parentCategoryId);
            }
            else if (subCategoryId.HasValue && subCategoryId.Value > 0)
            {
                strWhere.AppendFormat(" AND PC.CategoryId = {0} ", subCategoryId);
            }

            switch (mod)
            {
                case "rec":
                    mod = " P.DisplaySequence DESC ";
                    break;
                case "hot":
                    mod = " P.SaleCounts DESC ";
                    break;
                case "new":
                default:
                    mod = null;
                    break;
            }
            int SupplierId = 0;
            if (RegionId > 0)
            {
                SupplierId = YSWL.MALL.BLL.Shop.Distribution.SuppDistRegion.GetDistSuppId(RegionId);
            }
            DataSet ds = dal.GetProductListByCategoryIdEx(null, strWhere.ToString(), mod, startIndex, endIndex,SupplierId, out dataCount);
            if (Common.DataSetTools.DataSetIsNull(ds)) return null;
            return ProductAndSKUToList(ds.Tables[0]);
        }


        public List<Model.Shop.Products.ProductInfo> GetProductsList(string[] selectedSkus, int startIndex, int endIndex, out int dataCount, long productId)
        {
            if (selectedSkus == null || selectedSkus.Length < 1)
            {
                dataCount = 0;
                return null;
            }
            StringBuilder strWhere = new StringBuilder();
            if (selectedSkus.Length > 0)
            {
                strWhere.Append("  AND  ProductId IN (");
                strWhere.Append(string.Join(",", selectedSkus));
                strWhere.Append(") ");
            }

            DataSet ds = dal.GetProductListInfo(strWhere.ToString(), " ORDER BY SaleCounts DESC ", startIndex, endIndex, out dataCount, productId);
            if (Common.DataSetTools.DataSetIsNull(ds)) return null;
            return dal.DataTableToList(ds.Tables[0]);
        }

        public List<Model.Shop.Products.ProductInfo> GetProductRecListByPage(string[] selectedSkus, 
                                                                       int startIndex, int endIndex)
        {
            if (selectedSkus == null || selectedSkus.Length < 1)
            {
                return null;
            }
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append(" SaleStatus=1");
            if (selectedSkus.Length > 0)
            {
                strWhere.Append("  AND  ProductId IN (");
                strWhere.Append(string.Join(",", selectedSkus));
                strWhere.Append(") ");
            }
            DataSet ds = GetListByPage(strWhere.ToString(), " SaleCounts DESC", startIndex, endIndex);
            return dal.DataTableToList(ds.Tables[0]);
        }

        public int GetProductRecListCount(string[] selectedSkus)
        {
            if (selectedSkus == null || selectedSkus.Length < 1)
            {
                return 0;
            }
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append(" SaleStatus=1");
            if (selectedSkus.Length > 0)
            {
                strWhere.Append("  AND  ProductId IN (");
                strWhere.Append(string.Join(",", selectedSkus));
                strWhere.Append(") ");
            }
            return GetRecordCount(strWhere.ToString());
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Products.ProductInfo> GetModelList(long productId)
        {
            DataSet ds = dal.GetList(string.Format(" ProductId={0}", productId));
            if (ds != null && ds.Tables.Count > 0)
            {
                return dal.DataTableToList(ds.Tables[0]);
            }
            return null;
        }

        /// <summary>
        /// 获取商品名 by tzh
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public string GetProductName(long productId)
        {
            return dal.GetProductName(productId);
        }

        public bool ExistsBrands(int BrandId)
        {
            return dal.ExistsBrands(BrandId);
        }

        public DataSet GetTableSchema()
        {
            return dal.GetTableSchema();
        }
        public DataSet GetTableSchemaEx()
        {

            return dal.GetTableSchemaEx();
        }
        #region 商品的id集合和商品的名称获取数据的相关操作

        /// <summary>
        /// 根据需要的字段获得相应的数据
        /// </summary>
        public DataSet GetList(string strWhere, string DataField,int RegionId=0)
        {
            int SupplierId = 0;
            if (RegionId > 0)
            {
                SupplierId = YSWL.MALL.BLL.Shop.Distribution.SuppDistRegion.GetDistSuppId(RegionId);
            }
            return dal.GetList(strWhere, DataField,SupplierId);
        }

        /// <summary>
        /// 根据商品的id集合和商品的名称获取数据
        /// </summary>
        public DataSet GetListEx(string InIds, string productName, string OutIds, int CategoryId = 0)
        {
            return dal.GetList(GetListExSql(InIds, productName, OutIds, CategoryId));
        }

        /// <summary>
        /// 根据商品的id集合和商品的名称获取数据的条数
        /// </summary>
        public int GetRecordCountEx(string InIds, string productName, string OutIds, int CategoryId = 0)
        {
            return dal.GetRecordCount(GetListExSql(InIds, productName, OutIds, CategoryId));
        }

        public string GetListExSql(string InIds, string productName, string OutIds, int CategoryId = 0)
        {
            StringBuilder sbSql = new StringBuilder();
            if (!string.IsNullOrEmpty(InIds))
            {
                sbSql.Append(" ProductId in (" + InIds.TrimStart(',').TrimEnd(',') + ")");
            }
            if (!string.IsNullOrEmpty(OutIds))
            {
                if (sbSql.Length > 0)
                {
                    sbSql.Append(" and ");
                }
                sbSql.Append(" ProductId not in (" + OutIds.TrimStart(',').TrimEnd(',') + ")");
            }
            if (!string.IsNullOrEmpty(productName))
            {
                if (sbSql.Length > 0)
                {
                    sbSql.Append(" and ");
                }
                sbSql.Append(" ProductName like '%" + Common.InjectionFilter.SqlFilter(productName) + "%'");
            }
            if (CategoryId > 0)
            {
                if (sbSql.Length > 0)
                {
                    sbSql.Append(" and ");
                }
                sbSql.Append(" CategoryId =" + CategoryId + "");
            }
            return sbSql.ToString();
        }

        #endregion 商品的id集合和商品的名称获取数据的相关操作

        public DataSet GetProductInfo(Model.Shop.Products.ProductInfo model, bool showAlert = false, bool IsRest=false)
        {
            StringBuilder strWhere = new StringBuilder();
            if (model == null) return dal.GetProductInfo(null);

            if (!string.IsNullOrWhiteSpace(model.ProductName))
            {
                strWhere.AppendFormat(" AND ProductName LIKE '%{0}%' ", Common.InjectionFilter.SqlFilter(model.ProductName));
            }
            if (model.CategoryId > 0)
            {
                strWhere.AppendFormat("AND ( PC.CategoryId='{0}'  or PC.CategoryPath like '{0}|%')", model.CategoryId);
            }
            if (!string.IsNullOrWhiteSpace(model.SearchProductCategories))
            {
                strWhere.AppendFormat(" AND PC.CategoryId IN ( {0} ) ", model.CategoryId);
            }
            if (!string.IsNullOrWhiteSpace(model.ProductCode))
            {
                strWhere.AppendFormat("AND ProductCode = '{0}' ", model.ProductCode);
            }
            if (model.SupplierId != 0)
            {
                strWhere.AppendFormat("AND P.SupplierId = {0} ", model.SupplierId);
            }
            if (model.SalesType > 0) {
                strWhere.AppendFormat("AND P.SalesType = {0} ", model.SalesType);
            }
            if (model.SuppCategoryId > 0)
            {
                strWhere.AppendFormat(" AND EXISTS ( SELECT *  FROM   Shop_SuppProductCategories WHERE  ProductId =P.ProductId  ");
                strWhere.AppendFormat("   AND ( CategoryPath LIKE ( SELECT Path FROM Shop_SupplierCategories WHERE CategoryId = {0}  ) + '|%' ", model.SuppCategoryId);
                strWhere.AppendFormat(" OR Shop_SuppProductCategories.CategoryId = {0}))", model.SuppCategoryId);
            }
            if(showAlert)
            {
                strWhere.AppendFormat("  AND  SKU.Stock<=SKU.AlertStock  ");
            }
            if (IsRest)
            {
                strWhere.AppendFormat("  AND  P.RestrictionCount>0 ");
            }
            strWhere.AppendFormat(" AND P.SaleStatus= {0}  ", model.SaleStatus);
            return dal.GetProductInfo(strWhere.ToString());
        }

        public DataSet DeleteProducts(string Ids, out int Result)
        {
            DataSet ds= dal.DeleteProducts(Ids, out Result);

            if (Result > 0) //同步删除
            {
                //清除缓存
                ClearCache(Ids);
            }
            return ds;
        }

        public bool ChangeProductsCategory(string productIds, int categoryId)
        {
            return dal.ChangeProductsCategory(productIds, categoryId);
        }
        /// <summary>
        /// 获取回收站数据
        /// </summary>
        /// <returns></returns>
        public DataSet GetRecycleList(string strWhere,int RegionId=0)
        {
            int SupplierId = 0;
            if (RegionId > 0)
            {
                SupplierId = YSWL.MALL.BLL.Shop.Distribution.SuppDistRegion.GetDistSuppId(RegionId);
            }
            return dal.GetRecycleList(strWhere,SupplierId);
        }

        /// <summary>
        /// 还原所有商品
        /// </summary>
        /// <param name="IDlist"></param>
        /// <param name="strSetValue"></param>
        /// <returns></returns>
        public bool RevertAll(int RegionId=0)
        {
            int SupplierId = 0;
            if (RegionId > 0)
            {
                SupplierId = YSWL.MALL.BLL.Shop.Distribution.SuppDistRegion.GetDistSuppId(RegionId);
            }
            return dal.RevertAll(SupplierId);
        }

        /// <summary>
        /// 更新商品状态
        /// </summary>
        /// <param name="IDlist"></param>
        /// <param name="strSetValue"></param>
        /// <returns></returns>
        public bool UpdateStatus(long productId, int SaleStatus,int RegionId=0)
        {
            int SupplierId = 0;
            if (RegionId > 0)
            {
                SupplierId = YSWL.MALL.BLL.Shop.Distribution.SuppDistRegion.GetDistSuppId(RegionId);
            }
            bool result = dal.UpdateStatus(productId, SaleStatus, SupplierId);
            if (result)
            {
                //清除缓存
                ClearCache(productId);
            }
            return result;
        }

        public long StockNum(long productId)
        {
            return dal.StockNum(productId);
        }

        //public bool UpdateStockNum(long productId, int )
        //{
        //    return dal.UpdateStockNum(productId, productName);
        //}

        public bool UpdateMarketPrice(long productId, decimal price,int RegionId=0)
        {
            int SupplierId = 0;
            if (RegionId > 0)
            {
                SupplierId = YSWL.MALL.BLL.Shop.Distribution.SuppDistRegion.GetDistSuppId(RegionId);
            }
            bool result =dal.UpdateMarketPrice(productId, price,SupplierId);
            if (result)
            {
                //清除缓存
                ClearCache(productId);
            }
            return result;
        }
        public bool UpdateLowestSalePrice(long productId, decimal price,int RegionId=0)
        {
            int SupplierId = 0;
            if (RegionId > 0)
            {
                SupplierId = YSWL.MALL.BLL.Shop.Distribution.SuppDistRegion.GetDistSuppId(RegionId);
            }
            bool result =dal.UpdateLowestSalePrice(productId, price,SupplierId);
            if (result)
            {
                //清除缓存
                ClearCache(productId);
            }
            return result;
        }

        /// <summary>
        /// 获取商品推荐信息 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public List<Model.Shop.Products.ProductInfo> GetProductRecList(ProductRecType type, int categoryId = 0, int top = -1,int RegionId=0)
        {
            int SupplierId = 0;
            if (RegionId > 0)
            {
                SupplierId = YSWL.MALL.BLL.Shop.Distribution.SuppDistRegion.GetDistSuppId(RegionId);
            }
            DataSet ds = dal.GetProductRecList(type, categoryId, top,SupplierId);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {

                List<Model.Shop.Products.ProductInfo> listProduct = ProductRecTableToList(ds.Tables[0]);
                YSWL.MALL.BLL.Shop.Products.CategoryInfo cateBll = new CategoryInfo();
                foreach (var productInfo in listProduct)
                {
                    //获取分类名称
                    productInfo.CategoryName = cateBll.GetNameByPid(productInfo.ProductId);
                }
                return listProduct;
            }
            else
            {
                return null;
            }
        }

      public  int GetProductRecCount(ProductRecType type, int categoryId,int RegionId=0)
      {
          int SupplierId = 0;
          if (RegionId > 0)
          {
              SupplierId = YSWL.MALL.BLL.Shop.Distribution.SuppDistRegion.GetDistSuppId(RegionId);
          }
          return dal.GetProductRecCount(type, categoryId,SupplierId);
      }
        public List<Model.Shop.Products.ProductInfo> GetProductRanListByRec(ProductRecType type, int categoryId = 0, int top = -1,int RegionId=0)
        {
            int SupplierId = 0;
            if (RegionId > 0)
            {
                SupplierId = YSWL.MALL.BLL.Shop.Distribution.SuppDistRegion.GetDistSuppId(RegionId);
            }
            DataSet ds = dal.GetProductRanListByRec(type, categoryId, top,SupplierId);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {

                List<Model.Shop.Products.ProductInfo> listProduct = ProductRecTableToList(ds.Tables[0]);
                YSWL.MALL.BLL.Shop.Products.CategoryInfo cateBll = new CategoryInfo();
                foreach (var productInfo in listProduct)
                {
                    //获取分类名称
                    productInfo.CategoryName = cateBll.GetNameByPid(productInfo.ProductId);
                }
                return listProduct;
            }
            else
            {
                return null;
            }
        }

        public List<Model.Shop.Products.ProductInfo> GetProductRanList(int top = -1,int RegionId=0)
        {
            int SupplierId = 0;
            if (RegionId > 0)
            {
                SupplierId = YSWL.MALL.BLL.Shop.Distribution.SuppDistRegion.GetDistSuppId(RegionId);
            }
            DataSet ds = dal.GetProductRanList(top,SupplierId);
            return ProductAndSKUToList(ds.Tables[0]);

        }


        /// <summary>
        /// 获取产品所关联的产品信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<Model.Shop.Products.ProductInfo> RelatedProductsList(long productId, int top = -1)
        {
            DataSet ds = dal.RelatedProductSource(productId, top);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                return dal.DataTableToList(ds.Tables[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 商品推荐
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Products.ProductInfo> ProductRecTableToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> modelList = new List<YSWL.MALL.Model.Shop.Products.ProductInfo>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                bool HasMarketPrice= dt.Columns.Contains("MarketPrice");
                bool HasPoints = dt.Columns.Contains("Points");
                bool HasGwjf = dt.Columns.Contains("Gwjf");
                YSWL.MALL.Model.Shop.Products.ProductInfo model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new YSWL.MALL.Model.Shop.Products.ProductInfo();
                    if (dt.Rows[n]["ProductId"] != null && dt.Rows[n]["ProductId"].ToString() != "")
                    {
                        model.ProductId = long.Parse(dt.Rows[n]["ProductId"].ToString());
                    }
                    if (dt.Rows[n]["ProductName"] != null && dt.Rows[n]["ProductName"].ToString() != "")
                    {
                        model.ProductName = dt.Rows[n]["ProductName"].ToString();
                    }
                    if (dt.Rows[n]["ProductCode"] != null && dt.Rows[n]["ProductCode"].ToString() != "")
                    {
                        model.ProductCode = dt.Rows[n]["ProductCode"].ToString();
                    }
                    if (dt.Rows[n]["ThumbnailUrl1"] != null && dt.Rows[n]["ThumbnailUrl1"].ToString() != "")
                    {
                        model.ThumbnailUrl1 = dt.Rows[n]["ThumbnailUrl1"].ToString();
                    }
                    if (dt.Rows[n]["ThumbnailUrl2"] != null && dt.Rows[n]["ThumbnailUrl2"].ToString() != "")
                    {
                        model.ThumbnailUrl2 = dt.Rows[n]["ThumbnailUrl2"].ToString();
                    }
                    if (dt.Rows[n]["ShortDescription"] != null && dt.Rows[n]["ShortDescription"].ToString() != "")
                    {
                        model.ShortDescription = dt.Rows[n]["ShortDescription"].ToString();
                    }

                    if (dt.Rows[n]["LowestSalePrice"] != null && dt.Rows[n]["LowestSalePrice"].ToString() != "")
                    {
                        model.LowestSalePrice = Common.Globals.SafeDecimal(dt.Rows[n]["LowestSalePrice"].ToString(), 0);
                    }
                    if (HasMarketPrice)
                    {
                        if (dt.Rows[n]["MarketPrice"] != null && dt.Rows[n]["MarketPrice"].ToString() != "")
                        {
                            model.MarketPrice = Common.Globals.SafeDecimal(dt.Rows[n]["MarketPrice"].ToString(), 0);
                        }
                    }
                    if (HasPoints)
                    {
                        if (dt.Rows[n]["Points"] != null && dt.Rows[n]["Points"].ToString() != "")
                        {
                            model.Points = Common.Globals.SafeDecimal(dt.Rows[n]["Points"].ToString(), 0);
                        }
                    }

                    if (HasGwjf)
                    {
                        if (dt.Rows[n]["Gwjf"] != null && dt.Rows[n]["Gwjf"].ToString() != "")
                        {
                            model.Gwjf = Common.Globals.SafeDecimal(dt.Rows[n]["Gwjf"].ToString(), 0);
                        }
                    }

                    //if (dt.Rows[n]["Weight"] != null && dt.Rows[n]["Weight"].ToString() != "")
                    //{
                    //    model.Weight = Common.Globals.SafeDecimal(dt.Rows[n]["Weight"].ToString(), 0);
                    //}
                    //if (dt.Rows[n]["SalePrice"] != null && dt.Rows[n]["SalePrice"].ToString() != "")
                    //{
                    //    model.SalePrice = Common.Globals.SafeDecimal(dt.Rows[n]["SalePrice"].ToString(), 0);
                    //}
                    modelList.Add(model);
                }
            }
            return modelList;
        }


        public int MaxSequence()
        {
            return dal.MaxSequence();
        }

        /// <summary>
        /// 产品信息和SKU信息
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<YSWL.MALL.Model.Shop.Products.ProductInfo> ProductAndSKUToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> modelList = new List<YSWL.MALL.Model.Shop.Products.ProductInfo>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Shop.Products.ProductInfo model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new YSWL.MALL.Model.Shop.Products.ProductInfo();
                    if (dt.Rows[n]["CategoryId"] != null && dt.Rows[n]["CategoryId"].ToString() != "")
                    {
                        model.CategoryId = int.Parse(dt.Rows[n]["CategoryId"].ToString());
                    }
                    if (dt.Rows[n]["TypeId"] != null && dt.Rows[n]["TypeId"].ToString() != "")
                    {
                        model.TypeId = int.Parse(dt.Rows[n]["TypeId"].ToString());
                    }
                    if (dt.Rows[n]["ProductId"] != null && dt.Rows[n]["ProductId"].ToString() != "")
                    {
                        model.ProductId = long.Parse(dt.Rows[n]["ProductId"].ToString());
                    }
                    if (dt.Rows[n]["BrandId"] != null && dt.Rows[n]["BrandId"].ToString() != "")
                    {
                        model.BrandId = int.Parse(dt.Rows[n]["BrandId"].ToString());
                    }
                    if (dt.Rows[n]["ProductName"] != null && dt.Rows[n]["ProductName"].ToString() != "")
                    {
                        model.ProductName = dt.Rows[n]["ProductName"].ToString();
                    }
                    if (dt.Rows[n]["ProductCode"] != null && dt.Rows[n]["ProductCode"].ToString() != "")
                    {
                        model.ProductCode = dt.Rows[n]["ProductCode"].ToString();
                    }
                    if (dt.Rows[n]["SupplierId"] != null && dt.Rows[n]["SupplierId"].ToString() != "")
                    {
                        model.SupplierId = int.Parse(dt.Rows[n]["SupplierId"].ToString());
                    }
                    if (dt.Rows[n]["RegionId"] != null && dt.Rows[n]["RegionId"].ToString() != "")
                    {
                        model.RegionId = int.Parse(dt.Rows[n]["RegionId"].ToString());
                    }
                    if (dt.Rows[n]["ShortDescription"] != null && dt.Rows[n]["ShortDescription"].ToString() != "")
                    {
                        model.ShortDescription = dt.Rows[n]["ShortDescription"].ToString();
                    }
                    if (dt.Rows[n]["Unit"] != null && dt.Rows[n]["Unit"].ToString() != "")
                    {
                        model.Unit = dt.Rows[n]["Unit"].ToString();
                    }
                    if (dt.Rows[n]["Description"] != null && dt.Rows[n]["Description"].ToString() != "")
                    {
                        model.Description = dt.Rows[n]["Description"].ToString();
                    }
                    if (dt.Rows[n]["Meta_Title"] != null && dt.Rows[n]["Meta_Title"].ToString() != "")
                    {
                        model.Meta_Title = dt.Rows[n]["Meta_Title"].ToString();
                    }
                    if (dt.Rows[n]["Meta_Description"] != null && dt.Rows[n]["Meta_Description"].ToString() != "")
                    {
                        model.Meta_Description = dt.Rows[n]["Meta_Description"].ToString();
                    }
                    if (dt.Rows[n]["Meta_Keywords"] != null && dt.Rows[n]["Meta_Keywords"].ToString() != "")
                    {
                        model.Meta_Keywords = dt.Rows[n]["Meta_Keywords"].ToString();
                    }
                    if (dt.Rows[n]["SaleStatus"] != null && dt.Rows[n]["SaleStatus"].ToString() != "")
                    {
                        model.SaleStatus = int.Parse(dt.Rows[n]["SaleStatus"].ToString());
                    }
                    if (dt.Rows[n]["AddedDate"] != null && dt.Rows[n]["AddedDate"].ToString() != "")
                    {
                        model.AddedDate = DateTime.Parse(dt.Rows[n]["AddedDate"].ToString());
                    }
                    if (dt.Rows[n]["VistiCounts"] != null && dt.Rows[n]["VistiCounts"].ToString() != "")
                    {
                        model.VistiCounts = int.Parse(dt.Rows[n]["VistiCounts"].ToString());
                    }
                    if (dt.Rows[n]["SaleCounts"] != null && dt.Rows[n]["SaleCounts"].ToString() != "")
                    {
                        model.SaleCounts = int.Parse(dt.Rows[n]["SaleCounts"].ToString());
                    }
                    if (dt.Rows[n]["DisplaySequence"] != null && dt.Rows[n]["DisplaySequence"].ToString() != "")
                    {
                        model.DisplaySequence = int.Parse(dt.Rows[n]["DisplaySequence"].ToString());
                    }
                    if (dt.Rows[n]["LineId"] != null && dt.Rows[n]["LineId"].ToString() != "")
                    {
                        model.LineId = int.Parse(dt.Rows[n]["LineId"].ToString());
                    }
                    if (dt.Rows[n]["MarketPrice"] != null && dt.Rows[n]["MarketPrice"].ToString() != "")
                    {
                        model.MarketPrice = decimal.Parse(dt.Rows[n]["MarketPrice"].ToString());
                    }
                    if (dt.Rows[n]["LowestSalePrice"] != null && dt.Rows[n]["LowestSalePrice"].ToString() != "")
                    {
                        model.LowestSalePrice = decimal.Parse(dt.Rows[n]["LowestSalePrice"].ToString());
                    }
                    if (dt.Rows[n]["PenetrationStatus"] != null && dt.Rows[n]["PenetrationStatus"].ToString() != "")
                    {
                        model.PenetrationStatus = int.Parse(dt.Rows[n]["PenetrationStatus"].ToString());
                    }
                    if (dt.Rows[n]["MainCategoryPath"] != null && dt.Rows[n]["MainCategoryPath"].ToString() != "")
                    {
                        model.MainCategoryPath = dt.Rows[n]["MainCategoryPath"].ToString();
                    }
                    if (dt.Rows[n]["ExtendCategoryPath"] != null && dt.Rows[n]["ExtendCategoryPath"].ToString() != "")
                    {
                        model.ExtendCategoryPath = dt.Rows[n]["ExtendCategoryPath"].ToString();
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
                    if (dt.Rows[n]["Points"] != null && dt.Rows[n]["Points"].ToString() != "")
                    {
                        model.Points = decimal.Parse(dt.Rows[n]["Points"].ToString());
                    }
                    if (dt.Rows[n]["ImageUrl"] != null && dt.Rows[n]["ImageUrl"].ToString() != "")
                    {
                        model.ImageUrl = dt.Rows[n]["ImageUrl"].ToString();
                    }
                    if (dt.Rows[n]["ThumbnailUrl1"] != null && dt.Rows[n]["ThumbnailUrl1"].ToString() != "")
                    {
                        model.ThumbnailUrl1 = dt.Rows[n]["ThumbnailUrl1"].ToString();
                    }
                    if (dt.Rows[n]["ThumbnailUrl2"] != null && dt.Rows[n]["ThumbnailUrl2"].ToString() != "")
                    {
                        model.ThumbnailUrl2 = dt.Rows[n]["ThumbnailUrl2"].ToString();
                    }
                    if (dt.Rows[n]["ThumbnailUrl3"] != null && dt.Rows[n]["ThumbnailUrl3"].ToString() != "")
                    {
                        model.ThumbnailUrl3 = dt.Rows[n]["ThumbnailUrl3"].ToString();
                    }
                    if (dt.Rows[n]["ThumbnailUrl4"] != null && dt.Rows[n]["ThumbnailUrl4"].ToString() != "")
                    {
                        model.ThumbnailUrl4 = dt.Rows[n]["ThumbnailUrl4"].ToString();
                    }
                    if (dt.Rows[n]["ThumbnailUrl5"] != null && dt.Rows[n]["ThumbnailUrl5"].ToString() != "")
                    {
                        model.ThumbnailUrl5 = dt.Rows[n]["ThumbnailUrl5"].ToString();
                    }
                    if (dt.Rows[n]["ThumbnailUrl6"] != null && dt.Rows[n]["ThumbnailUrl6"].ToString() != "")
                    {
                        model.ThumbnailUrl6 = dt.Rows[n]["ThumbnailUrl6"].ToString();
                    }
                    if (dt.Rows[n]["ThumbnailUrl7"] != null && dt.Rows[n]["ThumbnailUrl7"].ToString() != "")
                    {
                        model.ThumbnailUrl7 = dt.Rows[n]["ThumbnailUrl7"].ToString();
                    }
                    if (dt.Rows[n]["ThumbnailUrl8"] != null && dt.Rows[n]["ThumbnailUrl8"].ToString() != "")
                    {
                        model.ThumbnailUrl8 = dt.Rows[n]["ThumbnailUrl8"].ToString();
                    }
                    if (dt.Rows[n]["MaxQuantity"] != null && dt.Rows[n]["MaxQuantity"].ToString() != "")
                    {
                        model.MaxQuantity = int.Parse(dt.Rows[n]["MaxQuantity"].ToString());
                    }
                    if (dt.Rows[n]["MinQuantity"] != null && dt.Rows[n]["MinQuantity"].ToString() != "")
                    {
                        model.MinQuantity = int.Parse(dt.Rows[n]["MinQuantity"].ToString());
                    }
                    if (dt.Rows[n]["Tags"] != null && dt.Rows[n]["Tags"].ToString() != "")
                    {
                        model.Tags = dt.Rows[n]["Tags"].ToString();
                    }
                    if (dt.Rows[n]["SeoUrl"] != null && dt.Rows[n]["SeoUrl"].ToString() != "")
                    {
                        model.SeoUrl = dt.Rows[n]["SeoUrl"].ToString();
                    }
                    if (dt.Rows[n]["SeoImageAlt"] != null && dt.Rows[n]["SeoImageAlt"].ToString() != "")
                    {
                        model.SeoImageAlt = dt.Rows[n]["SeoImageAlt"].ToString();
                    }
                    if (dt.Rows[n]["SeoImageTitle"] != null && dt.Rows[n]["SeoImageTitle"].ToString() != "")
                    {
                        model.SeoImageTitle = dt.Rows[n]["SeoImageTitle"].ToString();
                    }
                    if (dt.Rows[n]["SalePrice"] != null && dt.Rows[n]["SalePrice"].ToString() != "")
                    {
                        model.SalePrice = decimal.Parse(dt.Rows[n]["SalePrice"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        /// <summary>
        /// 获取对应批发(佣金)规则的商品
        /// </summary>
        /// <param name="selectedSkus"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public List<YSWL.MALL.Model.Shop.Products.ProductInfo> GetRuleProductList(string[] selectedSkus, int startIndex, int endIndex)
        {
            if (selectedSkus == null || selectedSkus.Length < 1)
            {
                return null;
            }
            StringBuilder strWhere = new StringBuilder();
            if (selectedSkus.Length > 0)
            {
                strWhere.Append("   ProductId IN (");
                strWhere.Append(string.Join(",", selectedSkus));
                strWhere.Append(") ");
            }
            DataSet ds = GetListByPage(strWhere.ToString(), " SaleCounts DESC", startIndex, endIndex);
            if (Common.DataSetTools.DataSetIsNull(ds)) return null;
            return dal.DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 获取包含批发规则的商品数
        /// </summary>
        /// <param name="selectedSkus"></param>
        /// <returns></returns>
        public int GetRuleProductCount(string[] selectedSkus)
        {
            if (selectedSkus == null || selectedSkus.Length < 1)
            {
                return 0;
            }
            StringBuilder strWhere = new StringBuilder();
            if (selectedSkus.Length > 0)
            {
                strWhere.Append("   ProductId IN (");
                strWhere.Append(string.Join(",", selectedSkus));
                strWhere.Append(") ");
            }
            return GetRecordCount(strWhere.ToString());
        }

        /// <summary>
        /// 获取不包含批发规则的商品
        /// </summary>
        /// <param name="selectedPids"></param>
        /// <param name="pName"></param>
        /// <param name="categoryId"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public List<Model.Shop.Products.ProductInfo> GetNoRuleProductList(string pName, string categoryId, int status, int ruleId, int startIndex, int endIndex)
        {
            DataSet ds = dal.GetNoRuleProductList(pName, categoryId, status, ruleId, startIndex, endIndex);
            if (Common.DataSetTools.DataSetIsNull(ds)) return null;
            return dal.DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        ///  获取不包含此批发规则的商品数
        /// </summary>
        /// <param name="selectedPids"></param>
        /// <param name="pName"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public int GetNoRuleProductCount(string pName, string categoryId, int ruleId, int status = 1)
        {
            return dal.GetNoRuleProductCount(pName, categoryId, ruleId, status);
        }

        public int GetProductsCountEx(int Cid, int BrandId,string attrValues, string priceRange,int RegionId=0)
        {
            int SupplierId = 0;
            if (RegionId > 0)
            {
                SupplierId = YSWL.MALL.BLL.Shop.Distribution.SuppDistRegion.GetDistSuppId(RegionId);
            }
            return dal.GetProductsCountEx(Cid, BrandId, attrValues, priceRange,SupplierId);
        }
        /// <summary>
        /// 根据条件获取分页数据
        /// </summary>
        /// <param name="Cid">商品分类</param>
        /// <param name="BrandId">品牌</param>
        /// <param name="attrValues">属性值</param>
        /// <param name="priceRange">价格区间</param>
        /// <param name="mod">排序方式</param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public List<Model.Shop.Products.ProductInfo> GetProductsListEx(int Cid, int BrandId,string attrValues, string priceRange,
    string mod, int startIndex, int endIndex,int RegionId=0)
        {
           
            switch (mod)
            {
                case "default":
                    mod = " DisplaySequence DESC ";
                    break;
                case "hot":
                    mod = " SaleCounts DESC ";
                    break;
                case "new":
                    mod = "AddedDate desc ";
                    break;
                case "price":
                    mod = "LowestSalePrice ";
                    break;
                case "pricedesc":
                    mod = "LowestSalePrice  desc";
                    break;
                case "sales":
                    mod = "SaleCounts   desc";
                    break;
                default:
                    mod = " DisplaySequence DESC ";
                    break;
            }
            int SupplierId = 0;
            if (RegionId > 0)
            {
                SupplierId = YSWL.MALL.BLL.Shop.Distribution.SuppDistRegion.GetDistSuppId(RegionId);
            }
            DataSet ds = dal.GetProductsListEx(Cid, BrandId, attrValues, priceRange, mod, startIndex, endIndex,SupplierId);
            if (Common.DataSetTools.DataSetIsNull(ds)) return null;
            return dal.DataTableToList(ds.Tables[0]);
        }

        #region  商品搜索数据查询
        public int GetSearchCountEx(int Cid, int BrandId, string keyWord, string priceRange,int RegionId=0)
        {
            int SupplierId = 0;
            if (RegionId > 0)
            {
                SupplierId = YSWL.MALL.BLL.Shop.Distribution.SuppDistRegion.GetDistSuppId(RegionId);
            }
            var keys = keyWord.Split(' ');
            List<string> keysList = new List<string>();
            foreach (var item in keys)
            {
                var key = item.Replace("，",",").Split(',');
                keysList.AddRange(key);
            }
            return dal.GetSearchCountEx(Cid, BrandId, keysList, priceRange, SupplierId);
        }
        /// <summary>
        /// 根据条件获取分页数据
        /// </summary>
        /// <param name="Cid"></param>
        /// <param name="BrandId"></param>
        /// <param name="attrValues"></param>
        /// <param name="priceRange"></param>
        /// <param name="mod"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public List<Model.Shop.Products.ProductInfo> GetSearchListEx(int Cid, int BrandId, string keyWord, string priceRange,
    string mod, int startIndex, int endIndex, int RegionId = 0)
        {

            switch (mod)
            {

                case "default":
                    mod = " DisplaySequence DESC ";
                    break;
                case "hot":
                    mod = " SaleCounts DESC ";
                    break;
                case "new":
                    mod = "AddedDate desc ";
                    break;
                case "price":
                    mod = "LowestSalePrice ";
                    break;
                case "pricedesc":
                    mod = "LowestSalePrice  desc";
                    break;
                case "sales":
                    mod = "SaleCounts   desc";
                    break;
                default:
                    mod = " DisplaySequence DESC ";
                    break;
            }
            int SupplierId = 0;
            if (RegionId > 0)
            {
                SupplierId = YSWL.MALL.BLL.Shop.Distribution.SuppDistRegion.GetDistSuppId(RegionId);
            }
            var keys = keyWord.Split(' ');
            List<string> keysList = new List<string>();
            foreach (var item in keys)
            {
                var key = item.Replace("，", ",").Split(',');
                keysList.AddRange(key);
            }
            DataSet ds = dal.GetSearchListEx(Cid, BrandId, keysList, priceRange, mod, startIndex, endIndex, SupplierId);
            if (Common.DataSetTools.DataSetIsNull(ds)) return null;
            return dal.DataTableToList(ds.Tables[0]);
        }
        #endregion
        /// <summary>
        /// 根据类别地址 得到该类别下最大顺序值
        /// </summary>
        /// <param name="CategoryPath"></param>
        /// <returns></returns>
        public int MaxSequence(string CategoryPath)
        {
            return dal.MaxSequence(CategoryPath);
        }

        public List<YSWL.MALL.Model.Shop.Products.ProductInfo> GetKeyWordList(int top, string keyWord)
        {
            string CacheKey = "GetKeyWordList-" + top + keyWord;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("  SaleStatus = 1  ");
                    strSql.AppendFormat("  and  ProductName like '%{0}%' or ShortDescription like '%{0}%'  ", Common.InjectionFilter.SqlFilter(keyWord));
                    DataSet ds = dal.GetList(top, strSql.ToString(), "  NewID()");
                    objModel = dal.DataTableToList(ds.Tables[0]);
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (List<YSWL.MALL.Model.Shop.Products.ProductInfo>)objModel;
        }


        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string  productCode)
        {
            return dal.Exists(productCode);
        }

        /// <summary>
        /// 获取授权用户的店铺商品列表
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="keyword"></param>
        /// <param name="page_no"></param>
        /// <param name="page_size"></param>
        /// <param name="vertical_market"></param>
        /// <param name="market_id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<TaoBao.Domain.Item> GetTaoListByUser(string sessionKey, int cid, string keyword, int page_no = 1, int page_size = 40, string hasDiscount = "", string hasShowcase = "")
        {
            List<TaoBao.Domain.Item> TaoDataList = new List<TaoBao.Domain.Item>();
            ITopClient client = BLL.Shop.TaoBaoConfig.GetTopClient();
            ItemsOnsaleGetRequest req = new ItemsOnsaleGetRequest();
            if (cid > 0)
            {
                req.Cid = cid;
            }

            req.PageSize = page_size;
            if (!String.IsNullOrWhiteSpace(keyword))
            {
                req.Q = keyword;
            }
            if (!String.IsNullOrWhiteSpace(hasDiscount))
            {
                req.HasDiscount = Common.Globals.SafeBool(hasDiscount, false);
            }

            if (!String.IsNullOrWhiteSpace(hasShowcase))
            {
                req.HasShowcase = Common.Globals.SafeBool(hasShowcase, false);
            }
            req.Fields = "num_iid,title,nick,type,cid,pic_url,num,props,valid_thru,list_time,price,has_invoice,has_showcase,modified,delist_time,postage_id,seller_cids,outer_id";
            for (int i = 1; i <= page_no; i++)
            {
                req.PageNo = i;
                ItemsOnsaleGetResponse response = client.Execute(req, sessionKey);
                if (response.Items.Count > 0)
                {
                    //获取商品评论  这个可以采用批量方式获取
                    string ids = String.Join(",", response.Items.Select(c => c.NumIid));
                    List<TaoBao.Domain.Item> itemList = GetTaoListByIds(sessionKey, ids);
                    TaoDataList.AddRange(itemList);
                }
            }
            return TaoDataList;
        }

        public List<TaoBao.Domain.Item> GetTaoListByIds(string sessionKey, string ids)
        {
            List<TaoBao.Domain.Item> TaoDataList = new List<TaoBao.Domain.Item>();
            ITopClient client = BLL.Shop.TaoBaoConfig.GetTopClient();
            ItemsListGetRequest req = new ItemsListGetRequest();
            req.Fields = "num_iid,title,nick,type,cid,pic_url,num,props,valid_thru,list_time,price,has_invoice,has_showcase,modified,delist_time,postage_id,seller_cids,desc";
            req.NumIids = ids;
            ItemsListGetResponse response = client.Execute(req, sessionKey);
            if (response.Items.Count > 0)
            {
                TaoDataList.AddRange(response.Items);
            }
            return TaoDataList;
        }

        /// <summary>
        /// 根据分类ID获取商品列表
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public List<YSWL.MALL.Model.Shop.Products.ProductInfo> GetProductsByCid(int cid)
        {
            DataSet ds = dal.GetProductsByCid(cid);
            return dal.DataTableToList(ds.Tables[0]);
        }

        #region 商品促销-限时抢购
        /// <summary>
        /// 获取秒杀Count
        /// </summary>
        /// <returns></returns>
        public int GetProSalesCount()
        {
            return dal.GetProSalesCount();
        }
        /// <summary>
        /// 获取秒杀Count
        /// </summary>
        /// <returns></returns>
        public List<YSWL.MALL.Model.Shop.Products.ProductInfo> GetProSalesList(int startIndex, int endIndex)
        {
            DataSet ds = dal.GetProSalesList(startIndex, endIndex);
            return LimitProSalesToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获取促销商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public YSWL.MALL.Model.Shop.Products.ProductInfo GetProSaleModel( int id )
        {
            DataSet ds = dal.GetProSaleModel(id);
            return ProSalesToList(ds.Tables[0]).Count>0? ProSalesToList(ds.Tables[0])[0]:null;
        }

        /// <summary>
        /// 促销商品Model化
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<YSWL.MALL.Model.Shop.Products.ProductInfo> ProSalesToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> modelList = new List<YSWL.MALL.Model.Shop.Products.ProductInfo>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Shop.Products.ProductInfo model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new YSWL.MALL.Model.Shop.Products.ProductInfo();
              
                    if (dt.Rows[n]["ProductId"] != null && dt.Rows[n]["ProductId"].ToString() != "")
                    {
                        model.ProductId = long.Parse(dt.Rows[n]["ProductId"].ToString());
                    }
                    if (dt.Rows[n]["BrandId"] != null && dt.Rows[n]["BrandId"].ToString() != "")
                    {
                        model.BrandId = int.Parse(dt.Rows[n]["BrandId"].ToString());
                    }
                    if (dt.Rows[n]["ProductName"] != null && dt.Rows[n]["ProductName"].ToString() != "")
                    {
                        model.ProductName = dt.Rows[n]["ProductName"].ToString();
                    }
                    if (dt.Rows[n]["ProductCode"] != null && dt.Rows[n]["ProductCode"].ToString() != "")
                    {
                        model.ProductCode = dt.Rows[n]["ProductCode"].ToString();
                    }
                 
                    if (dt.Rows[n]["RegionId"] != null && dt.Rows[n]["RegionId"].ToString() != "")
                    {
                        model.RegionId = int.Parse(dt.Rows[n]["RegionId"].ToString());
                    }
                    if (dt.Rows[n]["SaleStatus"] != null && dt.Rows[n]["SaleStatus"].ToString() != "")
                    {
                        model.SaleStatus = int.Parse(dt.Rows[n]["SaleStatus"].ToString());
                    }
                    if (dt.Rows[n]["AddedDate"] != null && dt.Rows[n]["AddedDate"].ToString() != "")
                    {
                        model.AddedDate = DateTime.Parse(dt.Rows[n]["AddedDate"].ToString());
                    }
                
                    if (dt.Rows[n]["SaleCounts"] != null && dt.Rows[n]["SaleCounts"].ToString() != "")
                    {
                        model.SaleCounts = int.Parse(dt.Rows[n]["SaleCounts"].ToString());
                    }
                    if (dt.Rows[n]["MarketPrice"] != null && dt.Rows[n]["MarketPrice"].ToString() != "")
                    {
                        model.MarketPrice = decimal.Parse(dt.Rows[n]["MarketPrice"].ToString());
                    }
                    if (dt.Rows[n]["LowestSalePrice"] != null && dt.Rows[n]["LowestSalePrice"].ToString() != "")
                    {
                        model.LowestSalePrice = decimal.Parse(dt.Rows[n]["LowestSalePrice"].ToString());
                    }
                    if (dt.Rows[n]["Points"] != null && dt.Rows[n]["Points"].ToString() != "")
                    {
                        model.Points = decimal.Parse(dt.Rows[n]["Points"].ToString());
                    }
                    if (dt.Rows[n]["Description"] != null)
                    {
                        model.Description = dt.Rows[n]["Description"].ToString();
                    }
                    if (dt.Rows[n]["ImageUrl"] != null && dt.Rows[n]["ImageUrl"].ToString() != "")
                    {
                        model.ImageUrl = dt.Rows[n]["ImageUrl"].ToString();
                    }
                    if (dt.Rows[n]["ThumbnailUrl1"] != null && dt.Rows[n]["ThumbnailUrl1"].ToString() != "")
                    {
                        model.ThumbnailUrl1 = dt.Rows[n]["ThumbnailUrl1"].ToString();
                    }
                    if (dt.Rows[n]["Tags"] != null && dt.Rows[n]["Tags"].ToString() != "")
                    {
                        model.Tags = dt.Rows[n]["Tags"].ToString();
                    }
                    if (dt.Rows[n]["SeoUrl"] != null && dt.Rows[n]["SeoUrl"].ToString() != "")
                    {
                        model.SeoUrl = dt.Rows[n]["SeoUrl"].ToString();
                    }
                    //促销价格
                    if (dt.Rows[n]["ProSalesPrice"] != null && dt.Rows[n]["ProSalesPrice"].ToString() != "")
                    {
                        model.ProSalesPrice = decimal.Parse(dt.Rows[n]["ProSalesPrice"].ToString());
                    }
                    //结束时间
                    if (dt.Rows[n]["ProSalesEndDate"] != null && dt.Rows[n]["ProSalesEndDate"].ToString() != "")
                    {
                        model.ProSalesEndDate = DateTime.Parse(dt.Rows[n]["ProSalesEndDate"].ToString());
                    }
                    if (dt.Rows[n]["CountDownId"] != null && dt.Rows[n]["CountDownId"].ToString() != "")
                    {
                        model.CountDownId = int.Parse(dt.Rows[n]["CountDownId"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }
        #endregion

        #region 团购商品方法
        /// <summary>
        /// 获取团购数据
        /// </summary>
        /// <returns></returns>
        public int GetGroupBuyCount()
        {
            return dal.GetGroupBuyCount();
        }
        /// <summary>
        /// 获取团购数据
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public List<YSWL.MALL.Model.Shop.Products.ProductInfo> GetGroupBuyList(int startIndex, int endIndex)
        {
            DataSet ds = dal.GetGroupBuyList(startIndex, endIndex);
            return GroupBuyToList(ds.Tables[0]);
        }
         public List<YSWL.MALL.Model.Shop.Products.ProductInfo> GetGroupBuyList(int cid, int regionId, int startIndex,
                                                                                int endIndex, string orderby)
         {
             switch (orderby)
             {
                 case "default":
                     orderby = " DisplaySequence DESC ";
                     break;
                 case "hot":
                     orderby = " SaleCounts DESC ";
                     break;
                 case "new":
                     orderby = "AddedDate desc ";
                     break;
                 case "price":
                     orderby = "LowestSalePrice ";
                     break;
                 default:
                     orderby = "ProductId desc";
                     break;
             }
             DataSet ds = dal.GetProSalesList(cid,regionId,startIndex,endIndex,orderby);
             return GroupBuyToList(ds.Tables[0]);
         }
        /// <summary>
        /// 获取团购Model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public YSWL.MALL.Model.Shop.Products.ProductInfo GetGroupBuyModel(int id)
        {
            DataSet ds = dal.GetGroupBuyModel(id);
            return GroupBuyToList(ds.Tables[0]).Count > 0 ? GroupBuyToList(ds.Tables[0])[0] : null;
        }
        /// <summary>
        /// 团购商品Model化
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<YSWL.MALL.Model.Shop.Products.ProductInfo> GroupBuyToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> modelList = new List<YSWL.MALL.Model.Shop.Products.ProductInfo>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                bool hasgRegionId = dt.Columns.Contains("gRegionId"); 
                YSWL.MALL.Model.Shop.Products.ProductInfo model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new YSWL.MALL.Model.Shop.Products.ProductInfo();

                    if (dt.Rows[n]["ProductId"] != null && dt.Rows[n]["ProductId"].ToString() != "")
                    {
                        model.ProductId = long.Parse(dt.Rows[n]["ProductId"].ToString());
                    }
                    if (dt.Rows[n]["BrandId"] != null && dt.Rows[n]["BrandId"].ToString() != "")
                    {
                        model.BrandId = int.Parse(dt.Rows[n]["BrandId"].ToString());
                    }
                    if (dt.Rows[n]["ProductName"] != null && dt.Rows[n]["ProductName"].ToString() != "")
                    {
                        model.ProductName = dt.Rows[n]["ProductName"].ToString();
                    }
                    if (dt.Rows[n]["ProductCode"] != null && dt.Rows[n]["ProductCode"].ToString() != "")
                    {
                        model.ProductCode = dt.Rows[n]["ProductCode"].ToString();
                    }

                    if (dt.Rows[n]["RegionId"] != null && dt.Rows[n]["RegionId"].ToString() != "")
                    {
                        model.RegionId = int.Parse(dt.Rows[n]["RegionId"].ToString());
                    }
                    if (dt.Rows[n]["SaleStatus"] != null && dt.Rows[n]["SaleStatus"].ToString() != "")
                    {
                        model.SaleStatus = int.Parse(dt.Rows[n]["SaleStatus"].ToString());
                    }
                    if (dt.Rows[n]["AddedDate"] != null && dt.Rows[n]["AddedDate"].ToString() != "")
                    {
                        model.AddedDate = DateTime.Parse(dt.Rows[n]["AddedDate"].ToString());
                    }

                    if (dt.Rows[n]["SaleCounts"] != null && dt.Rows[n]["SaleCounts"].ToString() != "")
                    {
                        model.SaleCounts = int.Parse(dt.Rows[n]["SaleCounts"].ToString());
                    }
                    if (dt.Rows[n]["MarketPrice"] != null && dt.Rows[n]["MarketPrice"].ToString() != "")
                    {
                        model.MarketPrice = decimal.Parse(dt.Rows[n]["MarketPrice"].ToString());
                    }
                    if (dt.Rows[n]["LowestSalePrice"] != null && dt.Rows[n]["LowestSalePrice"].ToString() != "")
                    {
                        model.LowestSalePrice = decimal.Parse(dt.Rows[n]["LowestSalePrice"].ToString());
                    }
                    if (dt.Rows[n]["Points"] != null && dt.Rows[n]["Points"].ToString() != "")
                    {
                        model.Points = decimal.Parse(dt.Rows[n]["Points"].ToString());
                    }
                    if (dt.Rows[n]["ImageUrl"] != null && dt.Rows[n]["ImageUrl"].ToString() != "")
                    {
                        model.ImageUrl = dt.Rows[n]["ImageUrl"].ToString();
                    }
                    if (dt.Rows[n]["ThumbnailUrl1"] != null && dt.Rows[n]["ThumbnailUrl1"].ToString() != "")
                    {
                        model.ThumbnailUrl1 = dt.Rows[n]["ThumbnailUrl1"].ToString();
                    }
                    if (dt.Rows[n]["Description"] != null)
                    {
                        model.Description = dt.Rows[n]["Description"].ToString();
                    }
                    if (dt.Rows[n]["Tags"] != null && dt.Rows[n]["Tags"].ToString() != "")
                    { 
                        model.Tags = dt.Rows[n]["Tags"].ToString();
                    }
                    if (dt.Rows[n]["SeoUrl"] != null && dt.Rows[n]["SeoUrl"].ToString() != "")
                    {
                        model.SeoUrl = dt.Rows[n]["SeoUrl"].ToString();
                    }

                    #region 团购部分Model

                    if (dt.Rows[n]["GroupBuyId"] != null && dt.Rows[n]["GroupBuyId"].ToString() != "")
                    {
                        model.GroupBuy.GroupBuyId = int.Parse(dt.Rows[n]["GroupBuyId"].ToString());
                    }
                    if (dt.Rows[n]["Sequence"] != null && dt.Rows[n]["Sequence"].ToString() != "")
                    {
                        model.GroupBuy.Sequence = int.Parse(dt.Rows[n]["Sequence"].ToString());
                    }
                    if (dt.Rows[n]["FinePrice"] != null && dt.Rows[n]["FinePrice"].ToString() != "")
                    {
                        model.GroupBuy.FinePrice = decimal.Parse(dt.Rows[n]["FinePrice"].ToString());
                    }
                    if (dt.Rows[n]["StartDate"] != null && dt.Rows[n]["StartDate"].ToString() != "")
                    {
                        model.GroupBuy.StartDate = DateTime.Parse(dt.Rows[n]["StartDate"].ToString());
                    }
                    if (dt.Rows[n]["EndDate"] != null && dt.Rows[n]["EndDate"].ToString() != "")
                    {
                        model.GroupBuy.EndDate = DateTime.Parse(dt.Rows[n]["EndDate"].ToString());
                    }
                    if (dt.Rows[n]["MaxCount"] != null && dt.Rows[n]["MaxCount"].ToString() != "")
                    {
                        model.GroupBuy.MaxCount = int.Parse(dt.Rows[n]["MaxCount"].ToString());
                    }
                    if (dt.Rows[n]["GroupCount"] != null && dt.Rows[n]["GroupCount"].ToString() != "")
                    {
                        model.GroupBuy.GroupCount = int.Parse(dt.Rows[n]["GroupCount"].ToString());
                    }
                    if (dt.Rows[n]["BuyCount"] != null && dt.Rows[n]["BuyCount"].ToString() != "")
                    {
                        model.GroupBuy.BuyCount = int.Parse(dt.Rows[n]["BuyCount"].ToString());
                    }
                    if (dt.Rows[n]["Price"] != null && dt.Rows[n]["Price"].ToString() != "")
                    {
                        model.GroupBuy.Price = decimal.Parse(dt.Rows[n]["Price"].ToString());
                    }
                    if (dt.Rows[n]["Status"] != null && dt.Rows[n]["Status"].ToString() != "")
                    {
                        model.GroupBuy.Status = int.Parse(dt.Rows[n]["Status"].ToString());
                    }
                    if (dt.Rows[n]["BuyDesc"] != null)
                    {
                        model.GroupBuy.Description = dt.Rows[n]["BuyDesc"].ToString();
                    }

                    
                    if (hasgRegionId)
                    {
                        if (dt.Rows[n]["gRegionId"] != null && dt.Rows[n]["gRegionId"].ToString() != "")
                        {
                            model.GroupBuy.RegionId = int.Parse(dt.Rows[n]["gRegionId"].ToString());
                        }
                   }
                 
                    #endregion 
                    modelList.Add(model);
                }
            }
            return modelList;
        }


        public int GetProductStatus(long productId)
        {
            return dal.GetProductStatus(productId);
        }
        #endregion

        /// <summary>
        /// 获取供应商商品数量
        /// </summary>
        /// <param name="Cid">分类</param>
        /// <param name="supplierId">供应商ID</param>
        /// <param name="keyword">关键词</param>
        /// <param name="priceRange">价格区间</param>
        /// <returns></returns>
        public int GetSuppProductsCount(int Cid, int supplierId, string keyword, string priceRange)
        {
            return dal.GetSuppProductsCount(Cid, supplierId,keyword,priceRange);
        }

        /// <summary>
        /// 根据条件获取供应商商品
        /// </summary>
        /// <param name="Cid">类别ID</param>
        /// <param name="supplierId">供应商id</param>
        /// <param name="keyword">关键词</param>
        /// <param name="priceRange">价格区间</param>
        /// <param name="orderby">排序</param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public DataSet GetSuppProductsList(int Cid, int supplierId, string keyword, string priceRange, string orderby, int startIndex, int endIndex)
        {
            return dal.GetSuppProductsList(Cid, supplierId,keyword,priceRange, orderby,startIndex,endIndex);
        }
       /// <summary>
        /// 根据条件获取分页数据
       /// </summary>
        /// <param name="Cid">类别ID</param>
        /// <param name="supplierId">供应商id</param>
        /// <param name="keyword">关键词</param>
        /// <param name="priceRange">价格区间</param>
        /// <param name="mod">排序</param>
       /// <param name="startIndex"></param>
       /// <param name="endIndex"></param>
       /// <returns></returns>
        public List<Model.Shop.Products.ProductInfo> GetSuppProductsListEx(int Cid, int supplierId, string keyword, string priceRange, string mod, int startIndex, int endIndex)
        {
            switch (mod)
            {
                case "hot":
                    mod = " SaleCounts DESC ";
                    break;
                case "new":
                    mod = "AddedDate desc ";
                    break;
                case "price":
                    mod = "LowestSalePrice ";
                    break;
                case "pricedesc":
                    mod = "LowestSalePrice  desc";
                    break;
                default:
                    mod = null;
                    break;
            }
            DataSet ds = dal.GetSuppProductsList(Cid,supplierId,keyword,priceRange, mod, startIndex, endIndex);
            if (Common.DataSetTools.DataSetIsNull(ds)) return null;
            return dal.DataTableToList(ds.Tables[0]);
        }

      /// <summary>
      /// 根据条件获取数据
      /// </summary>
        /// <param name="top"></param>
      /// <param name="Cid"></param>
      /// <param name="supplierId"></param>
      /// <param name="mod"></param>
      /// <param name="keyword"></param>
      /// <param name="priceRange"></param>
      /// <returns></returns>
        public List<Model.Shop.Products.ProductInfo> GetSuppProductsList(int top,int Cid, int supplierId, string mod, string keyword, string priceRange)
        {
            switch (mod)
            {
                case "hot":
                    mod = " SaleCounts DESC ";
                    break;
                case "new":
                    mod = "AddedDate desc ";
                    break;
                case "pricedesc":
                    mod = "LowestSalePrice  desc";
                    break;
                default:
                    mod = null;
                    break;
            }
            DataSet ds = dal.GetSuppProductsList(top ,Cid, supplierId, mod,keyword, priceRange );
            if (DataSetTools.DataSetIsNull(ds)) return null;
            return dal.DataTableToList(ds.Tables[0]);
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool UpdateThumbnail(YSWL.MALL.Model.Shop.Products.ProductInfo model)
        {
            return dal.UpdateThumbnail(model);
        }


        /// <summary>
        /// 获取需要静态化的商品数据(或者图片重新生成)
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<int> GetListToReGen(string strWhere)
        {
            DataSet ds = dal.GetListToReGen(strWhere);
            List<int> PhotoIdList = new List<int>();
            if (ds != null && ds.Tables.Count > 0)
            {
                for (int n = 0; n < ds.Tables[0].Rows.Count; n++)
                {
                    if (ds.Tables[0].Rows[n]["ProductID"] != null && ds.Tables[0].Rows[n]["ProductId"].ToString() != "")
                    {
                        PhotoIdList.Add(int.Parse(ds.Tables[0].Rows[n]["ProductId"].ToString()));
                    }
                }
            }
            return PhotoIdList;
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public List<Model.Shop.Products.ProductInfo> GetListByPage(Model.Shop.Products.ProductInfo model,string orderby, int startIndex, int endIndex)
        {
            StringBuilder strWhere = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(model.ProductName))
            {
                strWhere.AppendFormat(" AND ProductName LIKE '%{0}%' ", Common.InjectionFilter.SqlFilter(model.ProductName));
            }
            if (model.CategoryId > 0)
            {
                strWhere.AppendFormat("AND ( PC.CategoryId='{0}'  or PC.CategoryPath like '{0}|%') ", model.CategoryId);
            }
            if (!string.IsNullOrWhiteSpace(model.SearchProductCategories))
            {
                strWhere.AppendFormat(" AND PC.CategoryId IN ( {0} ) ", model.CategoryId);
            }
            if (!string.IsNullOrWhiteSpace(model.ProductCode))
            {
                strWhere.AppendFormat("AND SKU.SKU LIKE '%{0}%' ", Common.InjectionFilter.SqlFilter(model.ProductCode));
            }
            if (model.SupplierId > 0)
            {
                strWhere.AppendFormat("AND P.SupplierId = {0} ", model.SupplierId);
            }
            if (model.SuppCategoryId > 0)
            {
                strWhere.AppendFormat(" AND EXISTS ( SELECT *  FROM   Shop_SuppProductCategories WHERE  ProductId =P.ProductId  ");
                strWhere.AppendFormat("   AND ( CategoryPath LIKE ( SELECT Path FROM Shop_SupplierCategories WHERE CategoryId = {0}  ) + '|%' ", model.SuppCategoryId);
                strWhere.AppendFormat(" OR Shop_SuppProductCategories.CategoryId = {0}))", model.SuppCategoryId);
            }
            strWhere.AppendFormat(" AND P.SaleStatus= {0}  ", model.SaleStatus);
            return null; //dal.GetListByPage(strWhere.ToString(), orderby, startIndex, endIndex);
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetProdRecordCount(string strWhere,int RegionId=0)
        {
            int SupplierId = 0;
            if (RegionId > 0)
            {
                SupplierId = YSWL.MALL.BLL.Shop.Distribution.SuppDistRegion.GetDistSuppId(RegionId);
            }
            return dal.GetProdRecordCount(strWhere,SupplierId);
        }

        /// <summary>
        /// 商品数据分页列表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public List<Model.Shop.Products.ProductInfo> GetProdListByPage(Model.Shop.Products.ProductInfo model, int startIndex, int endIndex, out int toalCount,int RegionId=0)
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" P.SaleStatus= {0}  ", model.SaleStatus);
            if (!string.IsNullOrWhiteSpace(model.ProductName))
            {
                strWhere.AppendFormat(" AND ProductName LIKE '%{0}%' ", InjectionFilter.SqlFilter(model.ProductName));
            }
            if (model.CategoryId > 0)
            {
                strWhere.AppendFormat(" AND EXISTS ( SELECT *  FROM   PMS_ProductCategories WHERE  ProductId =P.ProductId  ");
                strWhere.AppendFormat("   AND ( CategoryPath LIKE ( SELECT Path FROM PMS_Categories WHERE CategoryId = {0}  ) + '|%' ", model.CategoryId);
                strWhere.AppendFormat(" OR PMS_ProductCategories.CategoryId = {0}))", model.CategoryId);
            }
            if (!string.IsNullOrWhiteSpace(model.ProductCode))
            {        
                strWhere.AppendFormat(" AND EXISTS(SELECT * FROM  PMS_SKUs WHERE ProductId=p.ProductId and SKU  LIKE '%{0}%' )", InjectionFilter.SqlFilter(model.ProductCode)); 
            }
            if (model.SupplierId > 0)
            {
                strWhere.AppendFormat("AND P.SupplierId = {0} ", model.SupplierId);
            }
            if (model.SuppCategoryId > 0)
            {
                strWhere.AppendFormat(" AND EXISTS ( SELECT *  FROM   Shop_SuppProductCategories WHERE  ProductId =P.ProductId  ");
                strWhere.AppendFormat("   AND ( CategoryPath LIKE ( SELECT Path FROM Shop_SupplierCategories WHERE CategoryId = {0}  ) + '|%' ", model.SuppCategoryId);
                strWhere.AppendFormat(" OR Shop_SuppProductCategories.CategoryId = {0}))", model.SuppCategoryId);
            }
            int SupplierId = 0;
            if (RegionId > 0)
            {
                SupplierId = YSWL.MALL.BLL.Shop.Distribution.SuppDistRegion.GetDistSuppId(RegionId);
            }
            toalCount = dal.GetProdRecordCount(strWhere.ToString(),SupplierId);
            DataSet ds= dal.GetProdListByPage(strWhere.ToString(),"",startIndex,endIndex,SupplierId);
            if (DataSetTools.DataSetIsNull(ds)) return null;
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> modelList = new List<YSWL.MALL.Model.Shop.Products.ProductInfo>();
            DataTable dt = ds.Tables[0];
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                Supplier.SupplierCategories suppcateBll = new Supplier.SupplierCategories();
                YSWL.MALL.Model.Shop.Products.ProductInfo prodmodel;
                for (int n = 0; n < rowsCount; n++)
                {
                    prodmodel = new Model.Shop.Products.ProductInfo();
                    if (dt.Rows[n]["ProductId"] != null && dt.Rows[n]["ProductId"].ToString() != "")
                    {
                        prodmodel.ProductId = long.Parse(dt.Rows[n]["ProductId"].ToString());
                    }
                    if (dt.Rows[n]["ProductName"] != null && dt.Rows[n]["ProductName"].ToString() != "")
                    {
                        prodmodel.ProductName = dt.Rows[n]["ProductName"].ToString();
                    }
                    if (dt.Rows[n]["ProductCode"] != null && dt.Rows[n]["ProductCode"].ToString() != "")
                    {
                        prodmodel.ProductCode = dt.Rows[n]["ProductCode"].ToString();
                    }
                    if (dt.Rows[n]["AddedDate"] != null && dt.Rows[n]["AddedDate"].ToString() != "")
                    {
                        prodmodel.AddedDate = DateTime.Parse(dt.Rows[n]["AddedDate"].ToString());
                    }
                    if (dt.Rows[n]["VistiCounts"] != null && dt.Rows[n]["VistiCounts"].ToString() != "")
                    {
                        prodmodel.VistiCounts = int.Parse(dt.Rows[n]["VistiCounts"].ToString());
                    }
                    if (dt.Rows[n]["SaleCounts"] != null && dt.Rows[n]["SaleCounts"].ToString() != "")
                    {
                        prodmodel.SaleCounts = int.Parse(dt.Rows[n]["SaleCounts"].ToString());
                    }
                    if (dt.Rows[n]["MarketPrice"] != null && dt.Rows[n]["MarketPrice"].ToString() != "")
                    {
                        prodmodel.MarketPrice = decimal.Parse(dt.Rows[n]["MarketPrice"].ToString());
                    }
                    if (dt.Rows[n]["LowestSalePrice"] != null && dt.Rows[n]["LowestSalePrice"].ToString() != "")
                    {
                        prodmodel.LowestSalePrice = decimal.Parse(dt.Rows[n]["LowestSalePrice"].ToString());
                    }
                    if (dt.Rows[n]["PenetrationStatus"] != null && dt.Rows[n]["PenetrationStatus"].ToString() != "")
                    {
                        prodmodel.PenetrationStatus = int.Parse(dt.Rows[n]["PenetrationStatus"].ToString());
                    }
                    if (dt.Rows[n]["MainCategoryPath"] != null && dt.Rows[n]["MainCategoryPath"].ToString() != "")
                    {
                        prodmodel.MainCategoryPath = dt.Rows[n]["MainCategoryPath"].ToString();
                    }
                    if (dt.Rows[n]["ExtendCategoryPath"] != null && dt.Rows[n]["ExtendCategoryPath"].ToString() != "")
                    {
                        prodmodel.ExtendCategoryPath = dt.Rows[n]["ExtendCategoryPath"].ToString();
                    }
                    if (dt.Rows[n]["ImageUrl"] != null && dt.Rows[n]["ImageUrl"].ToString() != "")
                    {
                        prodmodel.ImageUrl = dt.Rows[n]["ImageUrl"].ToString();
                    }
                    if (dt.Rows[n]["ThumbnailUrl1"] != null && dt.Rows[n]["ThumbnailUrl1"].ToString() != "")
                    {
                        prodmodel.ThumbnailUrl1 = dt.Rows[n]["ThumbnailUrl1"].ToString();
                    }
                    prodmodel.CategoryName = ProductCategories(prodmodel.ProductId);
                    prodmodel.SuppCategoryName = suppcateBll.ProductSuppCategories(prodmodel.ProductId, model.SupplierId);
                    prodmodel.StockNum = StockNum(prodmodel.ProductId);
                    modelList.Add(prodmodel);
                }
            }
            return modelList;
        }

        CategoryInfo manage = new CategoryInfo();
        /// <summary>
        /// 获取商品所在商城分类信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        private string ProductCategories(long productId)
        { 
           List<Model.Shop.Products.ProductCategories> list =new  ProductCategories().GetModelList(productId);
           StringBuilder strName = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                foreach (Model.Shop.Products.ProductCategories productCategoriese in list)
                {
                    strName.Append(manage.GetFullNameByCache(productCategoriese.CategoryId));
                    strName.Append("</br>");
                }
            }
            return strName.ToString();
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.Shop.Products.ProductInfo> GetModelListByIdList(string idlist)
        {
            if (!String.IsNullOrWhiteSpace(idlist))
            {
                DataSet ds = dal.GetList(0, string.Format(" ProductId in ( {0} ) ", idlist), " ProductId desc ");
                return dal. DataTableToList(ds.Tables[0]);
            }
            return null;
        }

        /// <summary>
        /// 获取商家推荐的商品列表
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="type"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public DataSet GetSuppRecList(int supplierId, int type, string orderby)
        {
            return dal.GetSuppRecList(supplierId, type, orderby);
        }
        public List<Model.Shop.Products.ProductInfo> GetSuppRecList(int supplierId, int  type)
        {
            DataSet ds = dal.GetSuppRecList(supplierId, type, "   s.StationId  DESC ");
            return dal.DataTableToList(ds.Tables[0]);
        }

        //public List<YSWL.MALL.Model.Shop.Products.ProductInfo> GetModelList(string strWhere)
        //{
        //    DataSet ds = dal.GetList(strWhere);
        //    return DataTableToList(ds.Tables[0]);
        //}
        public DataSet GetProList(string strWhere)
        {
            return dal.GetProList(strWhere);
        }
        public List<YSWL.MALL.Model.Shop.Products.ProductInfo> GetProModelList(string strWhere)
        {
            DataSet ds = dal.GetProList(strWhere);
            return dal.DataTableToList(ds.Tables[0]);
        }
       
        public string GetProductUrl(YSWL.MALL.Model.Shop.Products.ProductInfo model)
        {
            if(model==null)
            {
                return "";
            }
            int rule = YSWL.MALL.BLL.SysManage.ConfigSystem.GetIntValueByCache("Shop_Static_NameRule");
            if (rule == 0)
            {
                return model.ProductId.ToString();
            }
            if (rule == 1)
            {
                return Common.PinyinHelper.GetPinyin(model.ProductName) + "_" + model.ProductId;
            }
            if (rule == 2)
            {
                return String.IsNullOrWhiteSpace(model.SeoUrl) ? model.ProductId.ToString() : model.SeoUrl + "_" + model.ProductId;
            }
            return model.ProductId.ToString();
        }
        public int GetGroupBuyCount(int cid, int regionid)
        {
          return   dal.GetCount(cid, regionid);
        }

        /// <summary>
        /// 商品对比
        /// </summary>
        /// <param name="pids"></param>
        /// <returns></returns>
        public Dictionary<string, Model.Shop.Products.AttributeInfo> GetProdValueList(long[] pids)
        { 
            AttributeInfo attributeManage = new YSWL.MALL.BLL.Shop.Products.AttributeInfo();
            Dictionary<string, Model.Shop.Products.AttributeInfo> dic = new Dictionary<string, Model.Shop.Products.AttributeInfo>();
            Model.Shop.Products.ProductInfo pInfo = new Model.Shop.Products.ProductInfo();
            List<Model.Shop.Products.AttributeInfo> pAttrList = new List<Model.Shop.Products.AttributeInfo>();//商品属性
            //Dictionary<long, Model.Shop.Products.AttributeInfo> data = new Dictionary<long, Model.Shop.Products.AttributeInfo>();  
            //for (int i = 0; i < pids.Length; i++)
            //{
            //    data.Add(pids[i],null); 
            //}
            int index = 0;
            //foreach (KeyValuePair<long, Model.Shop.Products.AttributeInfo> kvp in  data)
            //{
            foreach (long  kvp in pids)
            {
                pInfo = GetModelByCache(kvp);//根据商品id 读取单个商品基本信息// 
                pAttrList = attributeManage.GetAttributeInfoListByProductId(pids[index]);//读取单个商品的所有属性
                if (dic.ContainsKey("商品图片"))
                {
                    dic["商品图片"].AttributeValues.Add(new Model.Shop.Products.AttributeValue
                    {
                        ValueStr = pInfo.ProductName,
                        ImageUrl = pInfo.ThumbnailUrl1,
                        ValueId = pInfo.ProductId
                    });
                }
                else
                {
                    dic.Add("商品图片", new Model.Shop.Products.AttributeInfo
                    {
                        AttributeName = "商品图片",
                        AttributeValues = new List<Model.Shop.Products.AttributeValue>()
                                {
                                    new Model.Shop.Products.AttributeValue
                                        {
                                            ValueStr = pInfo.ProductName,
                                            ImageUrl = pInfo.ThumbnailUrl1,
                                            ValueId=pInfo.ProductId
                                        }
                                }
                    });
                }
                if (dic.ContainsKey("价格"))
                {
                    dic["价格"].AttributeValues.Add(new Model.Shop.Products.AttributeValue
                    {
                        ValueStr = pInfo.LowestSalePrice.ToString("F"),
                    });
                }
                else
                {
                    dic.Add("价格", new Model.Shop.Products.AttributeInfo
                    {
                        AttributeName = "价格",
                        AttributeValues = new List<Model.Shop.Products.AttributeValue>()
                                {
                                    new Model.Shop.Products.AttributeValue
                                        {
                                            ValueStr = pInfo.LowestSalePrice.ToString("F"),
                                        }
                                }
                    });
                }
                foreach (Model.Shop.Products.AttributeInfo attr in pAttrList)
                {
                    string value = string.Empty;
                    List<string> valueList = new List<string>();
                    for (int k = 0; k < pids.Length; k++)
                    {
                        valueList.Add(string.Empty);
                    }
                    attr.AttributeValues.ForEach(val => value += val.ValueStr + ",");
                    if (!dic.ContainsKey(attr.AttributeName))
                    {
                        dic.Add(attr.AttributeName, new Model.Shop.Products.AttributeInfo
                        {
                            AttributeName = attr.AttributeName,
                            ValueStr = valueList
                        });
                    }
                    if (dic[attr.AttributeName].ValueStr.Count > 0)
                    {
                        dic[attr.AttributeName].ValueStr[index] = value.TrimEnd(',');
                    }
                    //dic[attr.AttributeName].ValueStr[index] = value.TrimEnd(',');
                }
                index++;
            }
            return dic;
        }
        public DataSet GetTableHead()
        {
            return dal.GetTableHead();
        }

        /// <summary>
        /// 根据商家id获得是否存在该记录
        /// </summary>
        public bool Exists(int supplierId)
        {
            return dal.Exists(supplierId);
        }



        /// <summary>
        /// 根据条件获取分页数据
        /// </summary>
        /// <param name="Cid"></param>
        /// <param name="BrandId"></param>
        /// <param name="attrValues"></param>
        /// <param name="priceRange"></param>
        /// <param name="mod"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public List<Model.Shop.Products.ProductInfo> GetProListEx(int Cid, int BrandId, string keyWord, string priceRange,
    string mod, int startIndex, int endIndex, int RegionId = 0,int type=-1,string strWhere="")
        {
            switch (mod)
            {

                case "hot":
                    type=1;
                    mod = " DisplaySequence DESC";
                    break;
                case "sales":
                    mod = " SaleCounts DESC ";
                    break;
                case "new":
                    mod = "AddedDate desc ";
                    break;
                case "price":
                    mod = "LowestSalePrice ";
                    break;
                default:
                    mod = "DisplaySequence DESC ";
                    break;
            }
            int SupplierId = 0;
            if (RegionId > 0)
            {
                SupplierId = YSWL.MALL.BLL.Shop.Distribution.SuppDistRegion.GetDistSuppId(RegionId);
            }
            DataSet ds = dal.GetSearchListEx(Cid, BrandId, keyWord, priceRange, mod, startIndex, endIndex, SupplierId,type,strWhere);
            if (Common.DataSetTools.DataSetIsNull(ds)) return null;
            return dal.DataTableToList(ds.Tables[0]);
        }



        /// <summary>
        /// 根据条件获取分页数据
        /// </summary>
        /// <param name="Cid">商品分类</param>
        /// <param name="BrandId">品牌</param>
        /// <param name="attrValues">属性值</param>
        /// <param name="priceRange">价格区间</param>
        /// <param name="mod">排序方式</param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public List<Model.Shop.Products.ProductInfo> GetProdsListEx(int Cid, int BrandId, string attrValues, string priceRange,
    string orderby, int startIndex, int endIndex, int RegionId = 0,int type=-1)
        {
            switch (orderby)
            {
                case "default":
                    orderby = " DisplaySequence DESC ";
                    break;
                case "hot":
                    orderby = " DisplaySequence DESC ";
                    type = 1;
                    break;
                case "new":
                    orderby = "AddedDate desc ";
                    break;
                case "price":
                    orderby = "LowestSalePrice ";
                    break;
                case "sales":
                    orderby = "SaleCounts   desc";
                    break;
                default:
                    orderby = null;
                    break;
            }
            int SupplierId = 0;
            if (RegionId > 0)
            {
                SupplierId = YSWL.MALL.BLL.Shop.Distribution.SuppDistRegion.GetDistSuppId(RegionId);
            }
            DataSet ds = dal.GetProductsListEx(Cid, BrandId, attrValues, priceRange, orderby, startIndex, endIndex, SupplierId,type);
            if (Common.DataSetTools.DataSetIsNull(ds)) return null;
            return dal.DataTableToList(ds.Tables[0]);
        }
        public List<YSWL.MALL.Model.Shop.Products.ProductInfo> GetListEx(int top, string strWhere, string orderby)
        {
            DataSet ds = dal.GetList(top, strWhere, orderby);
            return dal.DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 按品类统计商品数量
        /// </summary>
        /// <returns></returns>
        public DataSet GetCategoriesCount()
        {
            return dal.GetCategoriesCount();
        }
        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public string  GetNameByCache(long ProductId)
        {
            string CacheKey = "ProductsModel-" + ProductId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(ProductId);
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            YSWL.MALL.Model.Shop.Products.ProductInfo model= (YSWL.MALL.Model.Shop.Products.ProductInfo)objModel;
            return model != null ? model.ProductName : "";
        }

           /// <summary>
        /// 获取限购数量
        /// </summary>
        public int GetRestrictionCount(long productId)
        {
            return dal.GetRestrictionCount(productId);
        }
        public List<YSWL.MALL.Model.Shop.Products.ProductInfo> DataTableToList(DataTable dt) {
            return dal.DataTableToList(dt);
        }

        /// <summary>
        /// 促销商品Model化
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<YSWL.MALL.Model.Shop.Products.ProductInfo> LimitProSalesToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> modelList = new List<YSWL.MALL.Model.Shop.Products.ProductInfo>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Shop.Products.ProductInfo model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new YSWL.MALL.Model.Shop.Products.ProductInfo();

                    if (dt.Rows[n]["ProductId"] != null && dt.Rows[n]["ProductId"].ToString() != "")
                    {
                        model.ProductId = long.Parse(dt.Rows[n]["ProductId"].ToString());
                    }
                    if (dt.Rows[n]["BrandId"] != null && dt.Rows[n]["BrandId"].ToString() != "")
                    {
                        model.BrandId = int.Parse(dt.Rows[n]["BrandId"].ToString());
                    }
                    if (dt.Rows[n]["ProductName"] != null && dt.Rows[n]["ProductName"].ToString() != "")
                    {
                        model.ProductName = dt.Rows[n]["ProductName"].ToString();
                    }
                    if (dt.Rows[n]["ProductCode"] != null && dt.Rows[n]["ProductCode"].ToString() != "")
                    {
                        model.ProductCode = dt.Rows[n]["ProductCode"].ToString();
                    }

                    if (dt.Rows[n]["RegionId"] != null && dt.Rows[n]["RegionId"].ToString() != "")
                    {
                        model.RegionId = int.Parse(dt.Rows[n]["RegionId"].ToString());
                    }
                    if (dt.Rows[n]["SaleStatus"] != null && dt.Rows[n]["SaleStatus"].ToString() != "")
                    {
                        model.SaleStatus = int.Parse(dt.Rows[n]["SaleStatus"].ToString());
                    }
                    if (dt.Rows[n]["AddedDate"] != null && dt.Rows[n]["AddedDate"].ToString() != "")
                    {
                        model.AddedDate = DateTime.Parse(dt.Rows[n]["AddedDate"].ToString());
                    }

                    if (dt.Rows[n]["SaleCounts"] != null && dt.Rows[n]["SaleCounts"].ToString() != "")
                    {
                        model.SaleCounts = int.Parse(dt.Rows[n]["SaleCounts"].ToString());
                    }
                    if (dt.Rows[n]["MarketPrice"] != null && dt.Rows[n]["MarketPrice"].ToString() != "")
                    {
                        model.MarketPrice = decimal.Parse(dt.Rows[n]["MarketPrice"].ToString());
                    }
                    if (dt.Rows[n]["LowestSalePrice"] != null && dt.Rows[n]["LowestSalePrice"].ToString() != "")
                    {
                        model.LowestSalePrice = decimal.Parse(dt.Rows[n]["LowestSalePrice"].ToString());
                    }
                    if (dt.Rows[n]["Points"] != null && dt.Rows[n]["Points"].ToString() != "")
                    {
                        model.Points = decimal.Parse(dt.Rows[n]["Points"].ToString());
                    }
                    if (dt.Rows[n]["Description"] != null)
                    {
                        model.Description = dt.Rows[n]["Description"].ToString();
                    }
                    if (dt.Rows[n]["ImageUrl"] != null && dt.Rows[n]["ImageUrl"].ToString() != "")
                    {
                        model.ImageUrl = dt.Rows[n]["ImageUrl"].ToString();
                    }
                    if (dt.Rows[n]["ThumbnailUrl1"] != null && dt.Rows[n]["ThumbnailUrl1"].ToString() != "")
                    {
                        model.ThumbnailUrl1 = dt.Rows[n]["ThumbnailUrl1"].ToString();
                    }
                    if (dt.Rows[n]["Tags"] != null && dt.Rows[n]["Tags"].ToString() != "")
                    {
                        model.Tags = dt.Rows[n]["Tags"].ToString();
                    }
                    if (dt.Rows[n]["SeoUrl"] != null && dt.Rows[n]["SeoUrl"].ToString() != "")
                    {
                        model.SeoUrl = dt.Rows[n]["SeoUrl"].ToString();
                    }
                    //促销价格
                    if (dt.Rows[n]["ProSalesPrice"] != null && dt.Rows[n]["ProSalesPrice"].ToString() != "")
                    {
                        model.ProSalesPrice = decimal.Parse(dt.Rows[n]["ProSalesPrice"].ToString());
                    }
                    //结束时间
                    if (dt.Rows[n]["ProSalesEndDate"] != null && dt.Rows[n]["ProSalesEndDate"].ToString() != "")
                    {
                        model.ProSalesEndDate = DateTime.Parse(dt.Rows[n]["ProSalesEndDate"].ToString());
                    }
                    if (dt.Rows[n]["CountDownId"] != null && dt.Rows[n]["CountDownId"].ToString() != "")
                    {
                        model.CountDownId = int.Parse(dt.Rows[n]["CountDownId"].ToString());
                    }
                    if (dt.Rows[n]["LimitQty"] != null && dt.Rows[n]["LimitQty"].ToString() != "")//限购数量
                    {
                        model.LimitCount = int.Parse(dt.Rows[n]["LimitQty"].ToString());
                    }
                    if (dt.Rows[n]["ProSalesDescription"] != null)
                    {
                        model.ProSalesDescription = dt.Rows[n]["ProSalesDescription"].ToString();
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        /// <summary>
        /// 根据分类ID获取商品列表
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public List<YSWL.MALL.Model.Shop.Products.ProductInfo> GetGiftsByCid(int cid)
        {
            DataSet ds = dal.GetGiftsByCid(cid);
            return dal.DataTableToList(ds.Tables[0]);
        }  
        /// <summary>
        /// 获取限时抢购 商品
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<YSWL.MALL.Model.Shop.Products.ProductInfo> GetProSalesList(int top)
        {
            DataSet ds = dal.GetProSalesList(top);
            return LimitProSalesToList(ds.Tables[0]);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public long UpdatePV(long pId)
        {
            long result = dal.UpdatePV(pId);
            ClearCache(pId);  //清除缓存
            return result;
        }

        /// <summary>
        /// 根据分类ID获取商品列表
        /// </summary>
        /// <param name="Cid"></param>
        /// <param name="storeIsInActivity"></param>
        /// <returns></returns>
        public List<YSWL.MALL.Model.Shop.Products.ProductInfo> GetProductsByCid(int Cid, bool storeIsInActivity)
        {
            DataSet ds = dal.GetProductsByCid(Cid, storeIsInActivity);
            return dal.DataTableToList(ds.Tables[0]);
        }


        /// <summary>
        /// 获取不包含批发规则的商品
        /// </summary>
        /// <param name="selectedPids"></param>
        /// <param name="pName"></param>
        /// <param name="categoryId"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public List<Model.Shop.Products.ProductInfo> GetNoComProList(string pName, string categoryId, int status, int ruleId, int startIndex, int endIndex)
        {
            StringBuilder strWhere = new StringBuilder();

            strWhere.AppendFormat("   NOT EXISTS(SELECT *  FROM Shop_CommissionPro WHERE ProductId=T.ProductId  and RuleId={0} )",ruleId);
            if (status > -1)
            {
                strWhere.AppendFormat(" and  SaleStatus={0}", status);
            }
            if (!string.IsNullOrWhiteSpace(pName))
            {

                strWhere.AppendFormat("  and  ProductName LIKE '%{0}%'", Common.InjectionFilter.SqlFilter(pName));
            }
            if (!string.IsNullOrWhiteSpace(categoryId))
            {

                strWhere.AppendFormat(" and   ProductId IN( SELECT DISTINCT ProductId FROM PMS_ProductCategories WHERE (CategoryPath LIKE (SELECT Path FROM PMS_Categories WHERE CategoryId={0})+'|%' or CategoryId={0}) ) ", categoryId);
            }


            DataSet ds = GetListByPage(strWhere.ToString(), "  SaleCounts DESC ", startIndex, endIndex);
            if (Common.DataSetTools.DataSetIsNull(ds)) return null;
            return dal.DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        ///  获取不包含此佣金规则的商品数
        /// </summary>
        /// <param name="selectedPids"></param>
        /// <param name="pName"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public int GetNoComProCount(string pName, string categoryId, int ruleId, int status = 1)
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat("   NOT EXISTS(SELECT *  FROM Shop_CommissionPro WHERE ProductId=PMS_Products.ProductId  and RuleId={0} )  ", ruleId);
            if (status > -1)
            {
                strWhere.AppendFormat(" and SaleStatus={0}", status);
            }

            if (!string.IsNullOrWhiteSpace(pName))
            {
                strWhere.AppendFormat(" and ProductName LIKE '%{0}%'", Common.InjectionFilter.SqlFilter(pName));
            }
            if (!string.IsNullOrWhiteSpace(categoryId))
            {

                strWhere.AppendFormat(" and  ProductId IN( SELECT DISTINCT ProductId FROM PMS_ProductCategories WHERE (CategoryPath LIKE (SELECT Path FROM PMS_Categories WHERE CategoryId={0})+'|%' or CategoryId={0}) ) ", categoryId);
            }


            return GetRecordCount(strWhere.ToString());
        }

        #region 预订商品操作
        /// <summary>
        /// 加载所有预订的商品
        /// </summary>
        /// <returns></returns>
        public DataSet GetPreProductList()
        {
            return dal.GetPreProductList();
        }

        #endregion


        public void ClearCache(long productId)
        {
            Common.DataCache.DeleteCache("ProductsModel-" + productId);
        }
        public void ClearCache(string idlist)
        {
            if (String.IsNullOrWhiteSpace(idlist)) {
                return;
            }
            string[] pidArr=  idlist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (pidArr == null || pidArr.Length <= 0) {
                return;
            }
            foreach (string item in pidArr) {
                Common.DataCache.DeleteCache("ProductsModel-" + item);
            }
        }

        #region 获取参与批发规则商品
        /// <summary>
        /// 获取参与批发规则的商品
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="orderBy"></param>
        /// <param name="rankId">用户等级Id</param>
        /// <returns></returns>
        public List<Model.Shop.Products.ProductInfo> GetSalesRuleProductsList(int startIndex, int endIndex,int userId)
        {
            //是否开启会员等级
            bool isEnable = SysManage.ConfigSystem.GetBoolValueByCache("RankScoreEnable");
            DataSet ds;
            if (isEnable)  
            {
                BLL.Members.UserRank userRankBll = new Members.UserRank();
                YSWL.MALL.Model.Members.UserRank userRank = userRankBll.GetUserRank(userId);
                if (userRank == null)//未找到对应等级返回null
                {
                    return null;
                }
                else
                {
                    ds = dal.GetSalesRuleProductsList(startIndex, endIndex, "",userRank.RankId);
                }
            }
            else
            {  //未开启会员等级
                ds = dal.GetSalesRuleProductsList(startIndex, endIndex, "",0);
            }
            if (Common.DataSetTools.DataSetIsNull(ds)) return null;
            return dal.DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获取参与批发规则的商品总数
        /// </summary>
        public int GetSalesRuleProdCount(int userId) {
            //是否开启会员等级
            bool isEnable = SysManage.ConfigSystem.GetBoolValueByCache("RankScoreEnable");
            if (isEnable)
            {
                BLL.Members.UserRank userRankBll = new Members.UserRank();
                YSWL.MALL.Model.Members.UserRank userRank = userRankBll.GetUserRank(userId);
                if (userRank == null)//未找到对应等级返回null
                {
                    return 0;
                }
                else
                {
                    return dal.GetSalesRuleProdCount(userRank.RankId);
                }
            }
            else
            {  //未开启会员等级
                return dal.GetSalesRuleProdCount(0);
            }
        }
        #endregion


        #region 获取最近订购的商品
        /// <summary>
        /// 获取用户最近订购的商品
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public List<Model.Shop.Products.ProductInfo> GetOrderedProdList(int userId, DateTime startDate, DateTime endDate, int startIndex, int endIndex) {
            DataSet ds = dal.GetOrderedProdList(userId, startDate, endDate, startIndex, endIndex);
            if (Common.DataSetTools.DataSetIsNull(ds)) return null;
            return dal.DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获取用户最近订购的商品总数
        /// </summary>
        public int GetOrderedProdCount(int userId, DateTime startDate, DateTime endDate) {
            return dal.GetOrderedProdCount(userId, startDate, endDate);
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        /// <param name="SaleStatus">状态 0:下架(仓库中)  1:上架 2:已删除</param>
        /// <returns></returns>
        public int GetRecordCount(int SaleStatus)
        {
            return dal.GetRecordCount(string.Format(" SaleStatus ={0} ",SaleStatus));
        }

        /// <summary>
        /// 根据需要的字段获得相应的数据
        /// </summary>
        public DataSet GetListExport(string strWhere, string DataField, int RegionId = 0)
        {
            int SupplierId = 0;
            if (RegionId > 0)
            {
                SupplierId = YSWL.MALL.BLL.Shop.Distribution.SuppDistRegion.GetDistSuppId(RegionId);
            }
            return dal.GetListExport(strWhere, DataField, SupplierId);
        }
        /// <summary>
        /// 根据分页获取推荐产品信息
        /// </summary>
        /// <param name="type">推荐类型</param>
        /// <param name="categoryId">分类id</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="endIndex">结束索引</param>
        /// <returns></returns>
        public List<Model.Shop.Products.ProductInfo> GetProductRecListByPage(ProductRecType type, int categoryId, string orderby, int startIndex, int endIndex)
        {
            DataSet ds= dal.GetProductRecListByPage(type,categoryId, orderby, startIndex, endIndex);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                return ProductRecTableToList(ds.Tables[0]); 
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取推荐产品信息记录总数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public int GetProductRecCount(ProductRecType type, int categoryId) {
            return dal.GetProductRecCount(type, categoryId);
        }
        #endregion
        /// <summary>
        /// 根据sku获取商品详情
        /// </summary>
        public Model.Shop.Products.ProductInfo GetModelBySku(string sku)
        {
            return dal.GetModelBySku(sku);
        }


        #region saas app接口

        public int GetSearchCountExApp(int Cid, int BrandId, string keyWord, string priceRange, int RegionId = 0,int? SaleStatus=null)
        {
            int SupplierId = 0;
            if (RegionId > 0)
            {
                SupplierId = YSWL.MALL.BLL.Shop.Distribution.SuppDistRegion.GetDistSuppId(RegionId);
            }
            List<string> keysList = new List<string>();
            if (!String.IsNullOrWhiteSpace(keyWord))
            {
                var keys = keyWord.Split(' ');
                foreach (var item in keys)
                {
                    var key = item.Replace("，", ",").Split(',');
                    keysList.AddRange(key);
                }
            }
            return dal.GetSearchCountExApp(Cid, BrandId, keysList, priceRange, SupplierId,SaleStatus);
        }
        /// <summary>
        /// 根据条件获取分页数据
        /// </summary>
        /// <param name="Cid"></param>
        /// <param name="BrandId"></param>
        /// <param name="attrValues"></param>
        /// <param name="priceRange"></param>
        /// <param name="mod"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public List<Model.Shop.Products.ProductInfo> GetSearchListExApp(int Cid, int BrandId, string keyWord, string priceRange,
    string mod, int startIndex, int endIndex, int RegionId = 0, int? SaleStatus = null)
        {

            switch (mod)
            {

                case "default":
                    mod = " DisplaySequence DESC ";
                    break;
                case "hot":
                    mod = " SaleCounts DESC ";
                    break;
                case "new":
                    mod = " AddedDate desc ";
                    break;
                case "price":
                    mod = " LowestSalePrice ";
                    break;
                case "pricedesc":
                    mod = " LowestSalePrice  desc";
                    break;
                case "sales":
                    mod = " SaleCounts desc";
                    break;
                default:
                    mod = " DisplaySequence DESC ";
                    break;
            }
            int SupplierId = 0;
            if (RegionId > 0)
            {
                SupplierId = YSWL.MALL.BLL.Shop.Distribution.SuppDistRegion.GetDistSuppId(RegionId);
            }
            var keys = keyWord.Split(' ');
            List<string> keysList = new List<string>();
            foreach (var item in keys)
            {
                var key = item.Replace("，", ",").Split(',');
                keysList.AddRange(key);
            }
            DataSet ds = dal.GetSearchListExApp(Cid, BrandId, keysList, priceRange, mod, startIndex, endIndex, SupplierId,SaleStatus);
            if (Common.DataSetTools.DataSetIsNull(ds)) return null;
            return DataTableToListApp(ds.Tables[0]);
        }

        public List<YSWL.MALL.Model.Shop.Products.ProductInfo> DataTableToListApp(DataTable dt)
        {
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> modelList = new List<YSWL.MALL.Model.Shop.Products.ProductInfo>();
            int rowsCount = dt.Rows.Count;
            bool IsHasSalesType = dt.Columns.Contains("SalesType");
            bool IsHasRestCount = dt.Columns.Contains("RestrictionCount");
            bool IsHasDeliveryTip = dt.Columns.Contains("DeliveryTip");
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Shop.Products.ProductInfo model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new YSWL.MALL.Model.Shop.Products.ProductInfo();
                    if (dt.Rows[n]["CategoryId"] != null && dt.Rows[n]["CategoryId"].ToString() != "")
                    {
                        model.CategoryId = int.Parse(dt.Rows[n]["CategoryId"].ToString());
                    }
                    if (dt.Rows[n]["TypeId"] != null && dt.Rows[n]["TypeId"].ToString() != "")
                    {
                        model.TypeId = int.Parse(dt.Rows[n]["TypeId"].ToString());
                    }
                    if (dt.Rows[n]["ProductId"] != null && dt.Rows[n]["ProductId"].ToString() != "")
                    {
                        model.ProductId = long.Parse(dt.Rows[n]["ProductId"].ToString());
                    }
                    if (dt.Rows[n]["BrandId"] != null && dt.Rows[n]["BrandId"].ToString() != "")
                    {
                        model.BrandId = int.Parse(dt.Rows[n]["BrandId"].ToString());
                    }
                    if (dt.Rows[n]["ProductName"] != null && dt.Rows[n]["ProductName"].ToString() != "")
                    {
                        model.ProductName = dt.Rows[n]["ProductName"].ToString();
                    }
                    if (dt.Rows[n]["ProductCode"] != null && dt.Rows[n]["ProductCode"].ToString() != "")
                    {
                        model.ProductCode = dt.Rows[n]["ProductCode"].ToString();
                    }
                    if (dt.Rows[n]["SupplierId"] != null && dt.Rows[n]["SupplierId"].ToString() != "")
                    {
                        model.SupplierId = int.Parse(dt.Rows[n]["SupplierId"].ToString());
                    }
                    if (dt.Rows[n]["RegionId"] != null && dt.Rows[n]["RegionId"].ToString() != "")
                    {
                        model.RegionId = int.Parse(dt.Rows[n]["RegionId"].ToString());
                    }
                    if (dt.Rows[n]["ShortDescription"] != null && dt.Rows[n]["ShortDescription"].ToString() != "")
                    {
                        model.ShortDescription = dt.Rows[n]["ShortDescription"].ToString();
                    }
                    if (dt.Rows[n]["Unit"] != null && dt.Rows[n]["Unit"].ToString() != "")
                    {
                        model.Unit = dt.Rows[n]["Unit"].ToString();
                    }
                    if (dt.Rows[n]["Description"] != null && dt.Rows[n]["Description"].ToString() != "")
                    {
                        model.Description = dt.Rows[n]["Description"].ToString();
                    }
                    if (dt.Rows[n]["Meta_Title"] != null && dt.Rows[n]["Meta_Title"].ToString() != "")
                    {
                        model.Meta_Title = dt.Rows[n]["Meta_Title"].ToString();
                    }
                    if (dt.Rows[n]["Meta_Description"] != null && dt.Rows[n]["Meta_Description"].ToString() != "")
                    {
                        model.Meta_Description = dt.Rows[n]["Meta_Description"].ToString();
                    }
                    if (dt.Rows[n]["Meta_Keywords"] != null && dt.Rows[n]["Meta_Keywords"].ToString() != "")
                    {
                        model.Meta_Keywords = dt.Rows[n]["Meta_Keywords"].ToString();
                    }
                    if (dt.Rows[n]["SaleStatus"] != null && dt.Rows[n]["SaleStatus"].ToString() != "")
                    {
                        model.SaleStatus = int.Parse(dt.Rows[n]["SaleStatus"].ToString());
                    }
                    if (dt.Rows[n]["AddedDate"] != null && dt.Rows[n]["AddedDate"].ToString() != "")
                    {
                        model.AddedDate = DateTime.Parse(dt.Rows[n]["AddedDate"].ToString());
                    }
                    if (dt.Rows[n]["VistiCounts"] != null && dt.Rows[n]["VistiCounts"].ToString() != "")
                    {
                        model.VistiCounts = int.Parse(dt.Rows[n]["VistiCounts"].ToString());
                    }
                    if (dt.Rows[n]["SaleCounts"] != null && dt.Rows[n]["SaleCounts"].ToString() != "")
                    {
                        model.SaleCounts = int.Parse(dt.Rows[n]["SaleCounts"].ToString());
                    }
                    if (dt.Rows[n]["DisplaySequence"] != null && dt.Rows[n]["DisplaySequence"].ToString() != "")
                    {
                        model.DisplaySequence = int.Parse(dt.Rows[n]["DisplaySequence"].ToString());
                    }
                    if (dt.Rows[n]["LineId"] != null && dt.Rows[n]["LineId"].ToString() != "")
                    {
                        model.LineId = int.Parse(dt.Rows[n]["LineId"].ToString());
                    }
                    if (dt.Rows[n]["MarketPrice"] != null && dt.Rows[n]["MarketPrice"].ToString() != "")
                    {
                        model.MarketPrice = decimal.Parse(dt.Rows[n]["MarketPrice"].ToString());
                    }
                    if (dt.Rows[n]["LowestSalePrice"] != null && dt.Rows[n]["LowestSalePrice"].ToString() != "")
                    {
                        model.LowestSalePrice = decimal.Parse(dt.Rows[n]["LowestSalePrice"].ToString());
                    }
                    if (dt.Rows[n]["PenetrationStatus"] != null && dt.Rows[n]["PenetrationStatus"].ToString() != "")
                    {
                        model.PenetrationStatus = int.Parse(dt.Rows[n]["PenetrationStatus"].ToString());
                    }
                    if (dt.Rows[n]["MainCategoryPath"] != null && dt.Rows[n]["MainCategoryPath"].ToString() != "")
                    {
                        model.MainCategoryPath = dt.Rows[n]["MainCategoryPath"].ToString();
                    }
                    if (dt.Rows[n]["ExtendCategoryPath"] != null && dt.Rows[n]["ExtendCategoryPath"].ToString() != "")
                    {
                        model.ExtendCategoryPath = dt.Rows[n]["ExtendCategoryPath"].ToString();
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
                    if (dt.Rows[n]["Points"] != null && dt.Rows[n]["Points"].ToString() != "")
                    {
                        model.Points = decimal.Parse(dt.Rows[n]["Points"].ToString());
                    }
                    if (dt.Rows[n]["ImageUrl"] != null && dt.Rows[n]["ImageUrl"].ToString() != "")
                    {
                        model.ImageUrl = dt.Rows[n]["ImageUrl"].ToString();
                    }

                    if (dt.Rows[n]["BrandName"] != null && dt.Rows[n]["BrandName"].ToString() != "")
                    {
                        model.BrandName = dt.Rows[n]["BrandName"].ToString();
                    }

                    if (dt.Rows[n]["ThumbnailUrl1"] != null && dt.Rows[n]["ThumbnailUrl1"].ToString() != "")
                    {
                        model.ThumbnailUrl1 = dt.Rows[n]["ThumbnailUrl1"].ToString();
                    }
                    if (dt.Rows[n]["ThumbnailUrl2"] != null && dt.Rows[n]["ThumbnailUrl2"].ToString() != "")
                    {
                        model.ThumbnailUrl2 = dt.Rows[n]["ThumbnailUrl2"].ToString();
                    }
                    if (dt.Rows[n]["ThumbnailUrl3"] != null && dt.Rows[n]["ThumbnailUrl3"].ToString() != "")
                    {
                        model.ThumbnailUrl3 = dt.Rows[n]["ThumbnailUrl3"].ToString();
                    }
                    if (dt.Rows[n]["ThumbnailUrl4"] != null && dt.Rows[n]["ThumbnailUrl4"].ToString() != "")
                    {
                        model.ThumbnailUrl4 = dt.Rows[n]["ThumbnailUrl4"].ToString();
                    }
                    if (dt.Rows[n]["ThumbnailUrl5"] != null && dt.Rows[n]["ThumbnailUrl5"].ToString() != "")
                    {
                        model.ThumbnailUrl5 = dt.Rows[n]["ThumbnailUrl5"].ToString();
                    }
                    if (dt.Rows[n]["ThumbnailUrl6"] != null && dt.Rows[n]["ThumbnailUrl6"].ToString() != "")
                    {
                        model.ThumbnailUrl6 = dt.Rows[n]["ThumbnailUrl6"].ToString();
                    }
                    if (dt.Rows[n]["ThumbnailUrl7"] != null && dt.Rows[n]["ThumbnailUrl7"].ToString() != "")
                    {
                        model.ThumbnailUrl7 = dt.Rows[n]["ThumbnailUrl7"].ToString();
                    }
                    if (dt.Rows[n]["ThumbnailUrl8"] != null && dt.Rows[n]["ThumbnailUrl8"].ToString() != "")
                    {
                        model.ThumbnailUrl8 = dt.Rows[n]["ThumbnailUrl8"].ToString();
                    }
                    if (dt.Rows[n]["MaxQuantity"] != null && dt.Rows[n]["MaxQuantity"].ToString() != "")
                    {
                        model.MaxQuantity = int.Parse(dt.Rows[n]["MaxQuantity"].ToString());
                    }
                    if (dt.Rows[n]["MinQuantity"] != null && dt.Rows[n]["MinQuantity"].ToString() != "")
                    {
                        model.MinQuantity = int.Parse(dt.Rows[n]["MinQuantity"].ToString());
                    }
                    if (dt.Rows[n]["Tags"] != null && dt.Rows[n]["Tags"].ToString() != "")
                    {
                        model.Tags = dt.Rows[n]["Tags"].ToString();
                    }
                    if (dt.Rows[n]["SeoUrl"] != null && dt.Rows[n]["SeoUrl"].ToString() != "")
                    {
                        model.SeoUrl = dt.Rows[n]["SeoUrl"].ToString();
                    }
                    if (dt.Rows[n]["SeoImageAlt"] != null && dt.Rows[n]["SeoImageAlt"].ToString() != "")
                    {
                        model.SeoImageAlt = dt.Rows[n]["SeoImageAlt"].ToString();
                    }
                    if (dt.Rows[n]["SeoImageTitle"] != null && dt.Rows[n]["SeoImageTitle"].ToString() != "")
                    {
                        model.SeoImageTitle = dt.Rows[n]["SeoImageTitle"].ToString();
                    }
                    if (IsHasSalesType)
                    {
                        if (dt.Rows[n]["SalesType"] != null && dt.Rows[n]["SalesType"].ToString() != "")
                        {
                            model.SalesType = int.Parse(dt.Rows[n]["SalesType"].ToString());
                        }
                    }
                    if (IsHasRestCount)
                    {
                        if (dt.Rows[n]["RestrictionCount"] != null && dt.Rows[n]["RestrictionCount"].ToString() != "")
                        {
                            model.RestrictionCount = int.Parse(dt.Rows[n]["RestrictionCount"].ToString());
                        }
                    }
                    if (IsHasDeliveryTip)
                    {
                        if (dt.Rows[n]["DeliveryTip"] != null && dt.Rows[n]["DeliveryTip"].ToString() != "")
                        {
                            model.DeliveryTip = dt.Rows[n]["DeliveryTip"].ToString();
                        }
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

		
		    /// <summary>
        /// 获取商品价格
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public decimal GetLowestSalePrice(long productId) {
            return dal.GetLowestSalePrice(productId);
        }


        /// <summary>
        /// 获取商家推荐的商品数据分页列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="supplierId"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public List<Model.Shop.Products.ProductInfo> GetSuppRecListByPage(int type, int supplierId, int startIndex, int endIndex)
        {
            DataSet ds = dal.GetSuppRecListByPage(type, supplierId, startIndex, endIndex, "StationId  DESC ");
            return dal.DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 获取商品记录总数
        /// </summary>
        /// <param name="status">(-1全部/0下架/1上架)</param>
        /// <returns></returns>
        public int GetProductCount(int status)
        {
            return dal.GetProductCount(status);
        }
        #endregion
		
		  #region saas app
        /// <summary>
        /// 获取对应批发(佣金)规则的商品
        /// </summary>
        /// <param name="selectedSkus"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public List<YSWL.MALL.Model.Shop.Products.ProductInfo> GetRuleProductListApp(string[] selectedSkus, int startIndex, int endIndex,string brandId)
        {
            if (selectedSkus == null || selectedSkus.Length < 1)
            {
                return null;
            }
            StringBuilder strWhere = new StringBuilder();
            if (selectedSkus.Length > 0)
            {
                strWhere.Append("   ProductId IN (");
                strWhere.Append(string.Join(",", selectedSkus));
                strWhere.Append(") ");
            }
            if (!string.IsNullOrEmpty(brandId))
            {
                strWhere.Append(" and BrandId="+brandId);
            }
            DataSet ds = GetListByPage(strWhere.ToString(), " SaleCounts DESC", startIndex, endIndex);
            if (Common.DataSetTools.DataSetIsNull(ds)) return null;
            return dal.DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获取不包含批发规则的商品
        /// </summary>
        /// <param name="selectedPids"></param>
        /// <param name="pName"></param>
        /// <param name="categoryId"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public List<Model.Shop.Products.ProductInfo> GetNoRuleProductListApp(string pName, string categoryId, string brandId,int status, int ruleId, int startIndex, int endIndex)
        {
            DataSet ds = dal.GetNoRuleProductListApp(pName, categoryId, brandId,status, ruleId, startIndex, endIndex);
            if (Common.DataSetTools.DataSetIsNull(ds)) return null;
            return dal.DataTableToList(ds.Tables[0]);
        }
        #endregion
		
    }
}