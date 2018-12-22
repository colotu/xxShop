/**
* ProductCategories.cs
*
* 功 能： N/A
* 类 名： ProductCategories
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012年12月14日 11:46:01  Rock    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Data;
using YSWL.Common;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Shop.Products;
using System.Text;

namespace YSWL.MALL.BLL.Shop.Products
{
    /// <summary>
    /// 产品类别关联
    /// </summary>
    public partial class ProductCategories
    {
        private readonly IProductCategories dal = DAShopProducts.CreateProductCategories();

        public ProductCategories()
        { }

        #region Method

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(YSWL.MALL.Model.Shop.Products.ProductCategories model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(long produtId)
        {
            return dal.Delete(produtId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.Products.ProductCategories GetModel(long produtId)
        {
            return dal.GetModel(produtId);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public YSWL.MALL.Model.Shop.Products.ProductCategories GetModelByCache(long produtId)
        {
            string CacheKey = "ProductCategoriesModel-";
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(produtId);
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.Shop.Products.ProductCategories)objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }

        public DataSet GetListByProductId(long productId)
        {
            return dal.GetList(string.Format(" ProductId ={0}",productId));
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
        public List<YSWL.MALL.Model.Shop.Products.ProductCategories> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }


        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Products.ProductCategories> GetModelList(long productId)
        {
            DataSet ds = dal.GetList(string.Format(" ProductId ={0}", productId));
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Products.ProductCategories> DataTableToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Shop.Products.ProductCategories> modelList = new List<YSWL.MALL.Model.Shop.Products.ProductCategories>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Shop.Products.ProductCategories model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new YSWL.MALL.Model.Shop.Products.ProductCategories();
                    if (dt.Rows[n]["CategoryId"] != null && dt.Rows[n]["CategoryId"].ToString() != "")
                    {
                        model.CategoryId = int.Parse(dt.Rows[n]["CategoryId"].ToString());
                    }
                    if (dt.Rows[n]["ProductId"] != null && dt.Rows[n]["ProductId"].ToString() != "")
                    {
                        model.ProductId = long.Parse(dt.Rows[n]["ProductId"].ToString());
                    }
                    if (dt.Rows[n]["CategoryPath"] != null && dt.Rows[n]["CategoryPath"].ToString() != "")
                    {
                        model.CategoryPath = dt.Rows[n]["CategoryPath"].ToString();
                    }
                    modelList.Add(model);
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

       
        #endregion Method

        public int GetCount(int Cid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("  CategoryPath LIKE (SELECT Path FROM PMS_Categories ");
            strSql.AppendFormat("WHERE CategoryId={0})+'|%' or CategoryId={0}  ", Cid);
            return dal.GetRecordCount(strSql.ToString());
        }

    }
}