/*----------------------------------------------------------------
// Copyright (C) 2012 云商未来 版权所有。
//
// 文件名：SKUItems.cs
// 文件功能描述：
// 
// 创建标识： [Ben]  2012/06/11 20:36:32
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/
using System;
using System.Data;
using System.Collections.Generic;
using YSWL.Common;
using YSWL.MALL.Model.Shop.Products;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Shop.Products;
namespace YSWL.MALL.BLL.Shop.Products
{
    /// <summary>
    /// SKUItem
    /// </summary>
    public partial class SKUItem
    {
        private readonly ISKUItem dal = DAShopProducts.CreateSKUItem();
        public SKUItem()
        { }
        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long SkuId, long AttributeId, long ValueId)
        {
            return dal.Exists(SkuId, AttributeId, ValueId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(YSWL.MALL.Model.Shop.Products.SKUItem model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.Shop.Products.SKUItem model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(long SkuId, long AttributeId, long ValueId)
        {

            return dal.Delete(SkuId, AttributeId, ValueId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.Products.SKUItem GetModel(long SkuId, long AttributeId, long ValueId)
        {

            return dal.GetModel(SkuId, AttributeId, ValueId);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public YSWL.MALL.Model.Shop.Products.SKUItem GetModelByCache(long SkuId, long AttributeId, long ValueId)
        {

            string CacheKey = "SKUItemsModel-" + SkuId + AttributeId + ValueId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(SkuId, AttributeId, ValueId);
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.Shop.Products.SKUItem)objModel;
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
        public List<YSWL.MALL.Model.Shop.Products.SKUItem> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Products.SKUItem> DataTableToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Shop.Products.SKUItem> modelList = new List<YSWL.MALL.Model.Shop.Products.SKUItem>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Shop.Products.SKUItem model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new YSWL.MALL.Model.Shop.Products.SKUItem();
                    if (dt.Rows[n]["SkuId"] != null && dt.Rows[n]["SkuId"].ToString() != "")
                    {
                        model.SkuId = long.Parse(dt.Rows[n]["SkuId"].ToString());
                    }
                    if (dt.Rows[n]["AttributeId"] != null && dt.Rows[n]["AttributeId"].ToString() != "")
                    {
                        model.AttributeId = long.Parse(dt.Rows[n]["AttributeId"].ToString());
                    }
                    if (dt.Rows[n]["ValueId"] != null && dt.Rows[n]["ValueId"].ToString() != "")
                    {
                        model.ValueId = long.Parse(dt.Rows[n]["ValueId"].ToString());
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

        ///// <summary>
        ///// 分页获取数据列表
        ///// </summary>
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //return dal.GetList(PageSize,PageIndex,strWhere);
        //}

        #endregion  Method

        public List<Model.Shop.Products.SKUItem> GetSKUItemsByProductId(long productId)
        {
            DataSet ds = dal.GetSKUItem4AttrValByProductId(productId);
            return SKUItem4AVDataTableToList(ds.Tables[0]);
        }

        public List<Model.Shop.Products.SKUItem> GetSKUItemsBySkuId(long skuId)
        {
            DataSet ds = dal.GetSKUItem4AttrValBySkuId(skuId);
            return SKUItem4AVDataTableToList(ds.Tables[0]);
        }

        public bool Exists(long? SkuId, long? AttributeId, long? ValueId)
        {
            return dal.Exists(SkuId, AttributeId, ValueId);
        }

        public List<Model.Shop.Products.SKUItem> AttributeValueInfo(long productId)
        {
            DataSet ds = dal.AttributeValuesInfo(productId);
            if (ds != null && ds.Tables.Count > 0)
            {
                return AttributeVakueDataTableToList(ds.Tables[0]);
            }
            else
            {
                return null;
            }
        }

        public List<YSWL.MALL.Model.Shop.Products.SKUItem> AttributeVakueDataTableToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Shop.Products.SKUItem> modelList = new List<YSWL.MALL.Model.Shop.Products.SKUItem>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Shop.Products.SKUItem model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new YSWL.MALL.Model.Shop.Products.SKUItem();
                    if (dt.Rows[n]["ImageUrl"] != null && dt.Rows[n]["ImageUrl"].ToString() != "")
                    {
                        model.ImageUrl = dt.Rows[n]["ImageUrl"].ToString();
                    }
                    if (dt.Rows[n]["AttributeId"] != null && dt.Rows[n]["AttributeId"].ToString() != "")
                    {
                        model.AttributeId = long.Parse(dt.Rows[n]["AttributeId"].ToString());
                    }
                    if (dt.Rows[n]["ValueId"] != null && dt.Rows[n]["ValueId"].ToString() != "")
                    {
                        model.ValueId = long.Parse(dt.Rows[n]["ValueId"].ToString());
                    }
                    if (dt.Rows[n]["ValueStr"] != null && dt.Rows[n]["ValueStr"].ToString() != "")
                    {
                        model.ValueStr = dt.Rows[n]["ValueStr"].ToString();
                    }

                    if (dt.Rows[n]["UserDefinedPic"] != null && dt.Rows[n]["UserDefinedPic"].ToString() != "")
                    {
                        model.UserDefinedPic = bool.Parse(dt.Rows[n]["UserDefinedPic"].ToString());

                    }

                    if (dt.Rows[n]["SpecId"] != null && dt.Rows[n]["SpecId"].ToString() != "")
                    {
                        model.SpecId = long.Parse(dt.Rows[n]["SpecId"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public List<YSWL.MALL.Model.Shop.Products.SKUItem> SKUItem4AVDataTableToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Shop.Products.SKUItem> modelList = new List<YSWL.MALL.Model.Shop.Products.SKUItem>();
            if (dt.Rows.Count < 1) return modelList;

            YSWL.MALL.Model.Shop.Products.SKUItem model;
            try
            {
                foreach (DataRow dataRow in dt.Rows)
                {
                    if (dataRow["SkuId"] == DBNull.Value) continue;

                    model = new Model.Shop.Products.SKUItem();
                    model.SkuId = dataRow.Field<long>("SkuId");
                    model.SpecId = dataRow.Field<long>("SpecId");
                    model.AttributeId = dataRow.Field<long>("AttributeId");
                    model.ValueId = dataRow.Field<long>("ValueId");
                    model.ImageUrl = dataRow.Field<string>("ImageUrl");
                    model.ValueStr = dataRow.Field<string>("ValueStr");
                    model.ProductId = dataRow.Field<long>("ProductId");

                    model.AttributeName = dataRow.Field<string>("AttributeName");
                    model.AB_DisplaySequence = dataRow.Field<int>("AB_DisplaySequence");
                    model.UsageMode = dataRow.Field<int>("UsageMode");
                    model.UseAttributeImage = dataRow.Field<bool>("UseAttributeImage");
                    model.UserDefinedPic = dataRow.Field<bool>("UserDefinedPic");

                    model.AV_DisplaySequence = dataRow.Field<int>("AV_DisplaySequence");
                    model.AV_ValueStr = dataRow.Field<string>("AV_ValueStr");
                    model.AV_ImageUrl = dataRow.Field<string>("AV_ImageUrl");

                    modelList.Add(model);

                    //if (dataRow["SkuId"] != DBNull.Value)
                    //{
                    //    model.SkuId = dataRow.Field<long>("SkuId");
                    //}
                    //if (dataRow["SpecId"] != DBNull.Value)
                    //{
                    //    model.SpecId = dataRow.Field<long>("SpecId");
                    //}
                    //if (dataRow["AttributeId"] != DBNull.Value)
                    //{
                    //    model.AttributeId = dataRow.Field<long>("AttributeId");
                    //}
                    //if (dataRow["ValueId"] != DBNull.Value)
                    //{
                    //    model.ValueId = dataRow.Field<long>("ValueId");
                    //}
                    //if (dataRow["ImageUrl"] != DBNull.Value)
                    //{
                    //    model.ImageUrl = dataRow.Field<string>("ImageUrl");
                    //}
                    //if (dataRow["ValueStr"] != DBNull.Value)
                    //{
                    //    model.ValueStr = dataRow.Field<string>("ValueStr");
                    //}
                    //if (dataRow["ProductId"] != DBNull.Value)
                    //{
                    //    model.ProductId = dataRow.Field<long>("ProductId");
                    //}
                    //if (dataRow["AttributeName"] != DBNull.Value)
                    //{
                    //    model.AttributeName = dataRow.Field<string>("AttributeName");
                    //}
                    //if (dataRow["AB_DisplaySequence"] != DBNull.Value)
                    //{
                    //    model.AB_DisplaySequence = dataRow.Field<int>("AB_DisplaySequence");
                    //}
                    //if (dataRow["UsageMode"] != DBNull.Value)
                    //{
                    //    model.UsageMode = dataRow.Field<int>("UsageMode");
                    //}
                    //if (dataRow["UseAttributeImage"] != DBNull.Value)
                    //{
                    //    model.UseAttributeImage = dataRow.Field<bool>("UseAttributeImage");
                    //}
                    //if (dataRow["UserDefinedPic"] != DBNull.Value)
                    //{
                    //    model.UserDefinedPic = dataRow.Field<bool>("UserDefinedPic");
                    //}
                    //if (dataRow["AV_DisplaySequence"] != DBNull.Value)
                    //{
                    //    model.AV_DisplaySequence = dataRow.Field<int>("AV_DisplaySequence");
                    //}
                    //if (dataRow["AV_ValueStr"] != DBNull.Value)
                    //{
                    //    model.AV_ValueStr = dataRow.Field<string>("AV_ValueStr");
                    //}
                    //if (dataRow["AV_ImageUrl"] != DBNull.Value)
                    //{
                    //    model.AV_ImageUrl = dataRow.Field<string>("AV_ImageUrl");
                    //}
                }

                return modelList;
            }
            catch (Exception ex)
            {
                YSWL.Log.LogHelper.AddTextLog("SKUItem4AVDataTableToList异常",ex.Message+"------"+ex.StackTrace);
                return new List<Model.Shop.Products.SKUItem>();
            }
            
        }

    }
}

