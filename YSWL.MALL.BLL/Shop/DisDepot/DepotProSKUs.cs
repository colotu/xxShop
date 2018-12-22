/**  版本信息模板在安装目录下，可自行修改。
* DepotProSKUs.cs
*
* 功 能： N/A
* 类 名： DepotProSKUs
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/7/27 17:36:55   N/A    初版
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
using YSWL.MALL.Model.Shop.DisDepot;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Shop.DisDepot;
using System.Text;
using System.Web.UI.WebControls;
using YSWL.MALL.IDAL.Shop.Products;

namespace YSWL.MALL.BLL.Shop.DisDepot
{
	/// <summary>
	/// DepotProSKUs
	/// </summary>
	public partial class DepotProSKUs
	{
        private readonly IDepotProSKUs dal = DAShopDisDepot.CreateDepotProSKUs();
        public DepotProSKUs()
		{}
		#region  BasicMethod
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string SKU)
		{
			return dal.Exists(SKU);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(string SKU)
		{
			
			return dal.Delete(SKU);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string SKUlist )
		{
			return dal.DeleteList(SKUlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs GetModel(string SKU)
		{
			
			return dal.GetModel(SKU);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs GetModelByCache(string SKU)
		{
			
			string CacheKey = "DepotProSKUsModel-" + SKU;
			object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(SKU);
					if (objModel != null)
					{
						int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("CacheTime");
						YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs)objModel;
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
		public List<YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs> DataTableToList(DataTable dt)
		{
			List<YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs> modelList = new List<YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs model;
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
        /// 分仓仓库SKU
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="depotId"></param>
        /// <returns></returns>
	    public  DataSet GetProductSkuInfo(long productId, int depotId )
	    {
	        return dal.GetProductSkuInfo(productId, depotId);
	    }
        /// <summary>
        /// 获取SKU 库存
        /// </summary>
        /// <param name="SKU"></param>
        /// <param name="depotId"></param>
        /// <param name="IsOpenAS"></param>
        /// <returns></returns>
	    public int GetStockBySKU(string SKU, int depotId, bool IsOpenAS,int ownerId=0)
	    {
	        return dal.GetStockBySKU(SKU, depotId, IsOpenAS, ownerId);
	    }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        /// <param name="depotId">仓库id</param>
        /// <param name="strWhere"></param>
        /// <param name="orderby"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public DataSet GetListByPage(int depotId, string strWhere, string orderby, int startIndex, int endIndex)
        {
            return dal.GetListByPage(depotId, strWhere, orderby, startIndex, endIndex);
        }
           /// <summary>
        /// 获取记录总数
       /// </summary>
       /// <param name="depotId">仓库id</param>
       /// <param name="strWhere"></param>
       /// <returns></returns>
        public int GetRecordCount(int depotId, string strWhere)
        {
            return dal.GetRecordCount(depotId, strWhere);
        }
          /// <summary>
        /// 分页获取数据列表
        /// </summary>
        /// <param name="Top"></param>
        /// <param name="depotId">仓库id</param>
        /// <param name="strWhere"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public DataSet GetList(int Top, int depotId, string strWhere, string orderby)
        {
            return dal.GetList(Top, depotId, strWhere, orderby);
        }

        /// <summary>
        /// 获取未添加sku 数据 总条数
        /// </summary>
        /// <param name="depotId"></param>
        /// <param name="categoryId"></param>
        /// <param name="pName"></param>
        /// <returns></returns>
        public int GetNoAddSKURecordCount(int depotId, int categoryId, string pName) {
            return dal.GetNoAddSKURecordCount(depotId, categoryId, pName);
        }
        /// <summary>
        /// 获取未添加sku 数据列表
        /// </summary>
        /// <param name="depotId"></param>
        /// <param name="categoryId"></param>
        /// <param name="pName"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public DataSet GetNoAddSKUList(int depotId, int categoryId, string pName, int startIndex, int endIndex) {
            return dal.GetNoAddSKUList(depotId, categoryId, pName, startIndex, endIndex);
        }

          /// <summary>
        /// 分页获取数据列表
     /// </summary>
     /// <param name="depotId">仓库id</param>
     /// <param name="strWhere"></param>
     /// <param name="orderby"></param>
     /// <param name="startIndex"></param>
     /// <param name="endIndex"></param>
     /// <returns></returns>
        public DataSet GetListByPage(int depotId, int categoryId, string pName, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strWhere = new StringBuilder();
            if (!String.IsNullOrWhiteSpace(pName))
            {
                strWhere.AppendFormat("   ProductName LIKE '%{0}%' ", Common.InjectionFilter.SqlFilter(pName));
            }
            if (categoryId > 0)
            {
                if (strWhere.Length > 0)
                {
                    strWhere.Append(" AND ");
                }
                strWhere.Append(" EXISTS (  SELECT DISTINCT  *  FROM   PMS_ProductCategories  ");
                strWhere.AppendFormat(
                    "  WHERE  ( CategoryPath LIKE  ( SELECT Path FROM PMS_Categories WHERE     CategoryId = {0} ) +  '|%' OR CategoryId = {0}  )  AND ProductId = p.ProductId ) ",
                    categoryId);
            }
        return dal.GetListByPage(depotId, strWhere.ToString(),orderby,startIndex,endIndex);
        }
        /// <summary>
        /// 获取记录总数
        /// </summary>
        /// <param name="depotId">仓库id</param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public int GetRecordCount(int depotId, int categoryId, string pName)
        {
            StringBuilder strWhere = new StringBuilder();
            if (!String.IsNullOrWhiteSpace(pName))
            {
                strWhere.AppendFormat("   ProductName LIKE '%{0}%' ", Common.InjectionFilter.SqlFilter(pName));
            }
            if (categoryId > 0)
            {
                if (strWhere.Length > 0) {
                    strWhere.Append(" AND ");
                }
                strWhere.Append("  EXISTS (  SELECT DISTINCT  *  FROM   PMS_ProductCategories  ");
                strWhere.AppendFormat(
                    "  WHERE  ( CategoryPath LIKE  ( SELECT Path FROM PMS_Categories WHERE     CategoryId = {0} ) +  '|%' OR CategoryId = {0}  )  AND ProductId = p.ProductId ) ",
                    categoryId);
            }
            return dal.GetRecordCount(depotId, strWhere.ToString());
        }

        public bool AddProduct(int depotId, string sku, int stock)
        {
            BLL.Shop.DisDepot.Depot depotBll = new Depot();
            BLL.Shop.Products.SKUInfo skuBll = new Products.SKUInfo();
            BLL.Shop.Products.ProductInfo prodBll = new Products.ProductInfo();
            if (depotBll.GetModel(depotId) == null)
            {//仓库不存在
                return false;
            }
            YSWL.MALL.Model.Shop.Products.SKUInfo skuModel = skuBll.GetModelBySKU(sku);
            if (skuModel == null)//sku数据不存在
            {
                return false;
            }
            if (dal.SkuExists(depotId, skuModel.SKU))
            {//sku 数据已存在
                return false;
            }
            YSWL.MALL.Model.Shop.Products.ProductInfo prodModel = prodBll.GetModelByCache(skuModel.ProductId);
            if (prodModel == null)//商品数据不存在
            {
                return false;
            }
            prodModel.Stock =prodModel.Stock.HasValue? prodModel.Stock:0;
            skuModel.Stock = stock <= 0 ? 0 : stock;
            skuModel.AlertStock = 0;
            bool result = dal.AddProduct(depotId, skuModel, prodModel);
            if (result)
            {
                ClearCache();
            }
            return result;
        }


        public bool DeleteProduct(int depotId,string sku)
        {
            BLL.Shop.DisDepot.Depot depotBll = new Depot();
            if (depotBll.GetModel(depotId) == null)
            {
                //仓库不存在
                return false;
            }
            YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs model = GetModel(depotId, sku);
            if (model == null) {//改sku数据不存在
                return false;
            }
            bool result = dal.DeleteProduct(depotId, model.ProductId, sku);
            if (result)
            {
                ClearCache();
            }
            return result;
        }
           /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs GetModel(int depotId, string sku) {
            return dal.GetModel(depotId,sku);
        }

        public bool UpdateStockNum(int depotId, string sku, int stock)
        {
            bool result=dal.UpdateStockNum(depotId, sku, stock);
            if(result){
                ClearCache();
            }
            return result;
        }
        /// <summary>
        /// 是否上架
        /// </summary>
        /// <param name="IsUp"></param>
        /// <returns></returns>
	    public bool UpdateSkuStatus(int depotId,string sku,bool IsUp,int status=1)
        {
            return dal.UpdateSkuStatus(depotId, sku,IsUp,status);
        }

       /// <summary>
        /// 清理缓存数据
       /// </summary>
        public void ClearCache()
        {
            Common.DataCache.ClearBatch("StockHelper_StockCache_SKU_");
        }
        public bool  SyncProdcut(long  prodcutId)
        {
            BLL.Shop.DisDepot.Depot depotBll = new BLL.Shop.DisDepot.Depot();
            List<YSWL.MALL.Model.Shop.DisDepot.Depot> depotList = depotBll.GetModelList(" Status=1  ");
            BLL.Shop.Products.ProductInfo productBll= new Products.ProductInfo();
            BLL.Shop.Products.SKUInfo skuBll = new Products.SKUInfo();
            YSWL.MALL.Model.Shop.Products.ProductInfo  prodModel=productBll.GetModel(prodcutId);
            List<Model.Shop.Products.SKUInfo> skuList= skuBll.GetListInnerJoinProd(prodcutId);
            if (depotList==null ||   prodModel == null || skuList == null || skuList.Count <= 0)
            {
                return false;
            }
            bool result = dal.SyncProdcut(depotList, prodModel, skuList);
            if (result)
            {
                ClearCache();
            }
            return result;
        }
        /// <summary>
        /// 获取分仓商品sku列表 （分页）
        /// </summary>
        /// <param name="depotId">仓库Id</param>
        /// <param name="keyw">商品名称或编码</param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="orderby">排序方式</param>
        /// <returns></returns>
        public DataSet GetSKUListByPage(int depotId, string keyw, int startIndex, int endIndex, string orderby) {
            return dal.GetSKUListByPage(depotId, keyw, startIndex, endIndex, orderby);
        }
        /// <summary>
        ///  获取记录总数
        /// </summary>
        /// <param name="depotId">仓库id</param>
        /// <param name="keyw">商品名称或编码</param>
        /// <returns></returns>
        public int GetSKURecordCount(int depotId, string keyw) {
            return dal.GetSKURecordCount(depotId, keyw);
        }


        #region ProductSkuDataTableToList
        public List<YSWL.MALL.Model.Shop.Products.SKUInfo> ProductSkuDataTableToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Shop.Products.SKUInfo> modelList = new List<YSWL.MALL.Model.Shop.Products.SKUInfo>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Shop.Products.SKUInfo model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new YSWL.MALL.Model.Shop.Products.SKUInfo();
                    if (dt.Rows[n]["SkuId"] != null && dt.Rows[n]["SkuId"].ToString() != "")
                    {
                        model.SkuId = long.Parse(dt.Rows[n]["SkuId"].ToString());
                    }

                    //if (dt.Rows[n]["SpecId"] != null && dt.Rows[n]["SpecId"].ToString() != "")
                    //{
                    //    model.SpecId = long.Parse(dt.Rows[n]["SpecId"].ToString());
                    //}
                    //if (dt.Rows[n]["AttributeId"] != null && dt.Rows[n]["AttributeId"].ToString() != "")
                    //{
                    //    model.AttributeId = long.Parse(dt.Rows[n]["AttributeId"].ToString());
                    //}
                    //if (dt.Rows[n]["ValueId"] != null && dt.Rows[n]["ValueId"].ToString() != "")
                    //{
                    //    model.AttributeId = long.Parse(dt.Rows[n]["ValueId"].ToString());
                    //}
                    if (dt.Rows[n]["ProductId"] != null && dt.Rows[n]["ProductId"].ToString() != "")
                    {
                        model.ProductId = long.Parse(dt.Rows[n]["ProductId"].ToString());
                    }

                    //if (dt.Rows[n]["ValueStr"] != null && dt.Rows[n]["ValueStr"].ToString() != "")
                    //{
                    //    model.ValuesStr = dt.Rows[n]["ValueStr"].ToString();
                    //}
                    if (dt.Rows[n]["SKU"] != null && dt.Rows[n]["SKU"].ToString() != "")
                    {
                        model.SKU = dt.Rows[n]["SKU"].ToString();
                    }
                    if (dt.Rows[n]["Weight"] != null && dt.Rows[n]["Weight"].ToString() != "")
                    {
                        model.Weight = int.Parse(dt.Rows[n]["Weight"].ToString());
                    }
                    if (dt.Rows[n]["Stock"] != null && dt.Rows[n]["Stock"].ToString() != "")
                    {
                        model.Stock = int.Parse(dt.Rows[n]["Stock"].ToString());
                    }
                    if (dt.Rows[n]["AlertStock"] != null && dt.Rows[n]["AlertStock"].ToString() != "")
                    {
                        model.AlertStock = int.Parse(dt.Rows[n]["AlertStock"].ToString());
                    }
                    if (dt.Rows[n]["CostPrice"] != null && dt.Rows[n]["CostPrice"].ToString() != "")
                    {
                        model.CostPrice = decimal.Parse(dt.Rows[n]["CostPrice"].ToString());
                    }
                    if (dt.Rows[n]["SalePrice"] != null && dt.Rows[n]["SalePrice"].ToString() != "")
                    {
                        model.SalePrice = decimal.Parse(dt.Rows[n]["SalePrice"].ToString());
                    }
                    if (dt.Rows[n]["Upselling"] != null && dt.Rows[n]["Upselling"].ToString() != "")
                    {
                        if ((dt.Rows[n]["Upselling"].ToString() == "1") || (dt.Rows[n]["Upselling"].ToString().ToLower() == "true"))
                        {
                            model.Upselling = true;
                        }
                        else
                        {
                            model.Upselling = false;
                        }
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }
        #endregion
        #endregion  ExtensionMethod
    }
}

