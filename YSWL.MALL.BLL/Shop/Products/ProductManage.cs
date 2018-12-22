/**
* ProductManage.cs
*
* 功 能： Shop模块-产品相关 多表事务业务类
* 类 名： ProductManage
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/6/22 11:14:41  Ben    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System.Collections.Generic;
using System.Data;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Shop.Products;
using System.Linq;

namespace YSWL.MALL.BLL.Shop.Products
{
    public class ProductManage
    {
        private static readonly IProductService service = DAShopProducts.CreateProductService();
        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="productInfo"></param>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        public static bool AddProduct(Model.Shop.Products.ProductInfo productInfo, out long ProductId)
        {
            bool IsSuccess= service.AddProduct(productInfo,out ProductId);
         
            return IsSuccess; 
        }

        public static bool AddProductOne(Model.Shop.Products.ProductInfo productInfo)
        {
            return service.AddProductOne(productInfo);
        }
        public static bool SubUpdateList(Model.Shop.Products.ProductInfo productInfo)
        {
            List<YSWL.MALL.Model.Shop.DisDepot.Depot> depotList = DisDepot.Depot.GetAvaDepots();
            string depotIds = depotList!=null&&depotList.Any()?string.Join(",", depotList.Select(t => t.DepotId)):"";
            if (!string.IsNullOrEmpty(depotIds))
            {
                return service.SubUpdateList(depotIds, productInfo);
            }
            return true;
        }
        /// <summary>
        /// 编辑商品
        /// </summary>
        /// <param name="productInfo"></param>
        /// <returns></returns>
        public static bool ModifyProduct(Model.Shop.Products.ProductInfo productInfo, int oldSaleStatus)
        {

            bool IsSuccess= service.ModifyProduct(productInfo,oldSaleStatus);
       
            return IsSuccess;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productInfo"></param>
        /// <param name="ProductId"></param>
        /// <returns></returns>
       public static bool AddSuppProduct(Model.Shop.Products.ProductInfo productInfo, out long ProductId)
       {

           return service.AddSuppProduct(productInfo, out ProductId);
       }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productInfo"></param>
        /// <returns></returns>
        public static bool ModifySuppProduct(Model.Shop.Products.ProductInfo productInfo,int oldSaleStatus)
        {

            return service.ModifySuppProduct(productInfo, oldSaleStatus);
        }

        #region 对比信息获取
        /// <summary>
        /// 获取对比结果
        /// </summary>
        public static List<Model.Shop.Products.ProductCompareServer> GetCompareProudctInfo(string ids)
        {
            DataSet ds = service.GetCompareProudctInfo(ids);
            if (ds != null && ds.Tables.Count > 0)
            {
                return DataTableToList(ds.Tables[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取对比结果基本信息 图片价格
        /// </summary>
        public static List<Model.Shop.Products.ProductCompareServer> GetCompareProudctBasicInfo(string ids)
        {
            DataSet ds = service.GetCompareProudctBasicInfo(ids);
            if (ds != null && ds.Tables.Count > 0)
            {
                return DataTableToList(ds.Tables[0]);
            }
            else
            {
                return null;
            }
        }

        private static List<Model.Shop.Products.ProductCompareServer> DataTableToList(DataTable dt)
        {
            List<Model.Shop.Products.ProductCompareServer> list = null;
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                list = new List<Model.Shop.Products.ProductCompareServer>();

                Model.Shop.Products.ProductCompareServer model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new Model.Shop.Products.ProductCompareServer();
                    if (dt.Rows[n]["AttName"] != null && dt.Rows[n]["AttName"].ToString() != "")
                    {
                        model.AttrName = dt.Rows[n]["AttName"].ToString();
                    }
                    if (dt.Rows[n]["Product1"] != null && dt.Rows[n]["Product1"].ToString() != "")
                    {
                        model.Product1 = dt.Rows[n]["Product1"].ToString();
                    }
                    if (dt.Rows[n]["Product2"] != null && dt.Rows[n]["Product2"].ToString() != "")
                    {
                        model.Product2 = dt.Rows[n]["Product2"].ToString();
                    }
                    if (dt.Rows[n]["Product3"] != null && dt.Rows[n]["Product3"].ToString() != "")
                    {
                        model.Product3 = dt.Rows[n]["Product3"].ToString();
                    }
                    if (dt.Rows[n]["Product4"] != null && dt.Rows[n]["Product4"].ToString() != "")
                    {
                        model.Product4 = dt.Rows[n]["Product4"].ToString();
                    }
                    list.Add(model);
                }
            }
            return list;
        } 
        #endregion



        #region 导入商品

        /// <summary>
        /// 导入商品
        /// </summary>
        public static void ImportProduct(DataTable dt, ref int successCount, ref string errorfileurl, ref int errorCount)
        {
            if (dt == null) {
                return;
            }
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> list =ImportProdDataTableToList(dt);
            if (list == null || list.Count<=0) {
                return ;
            }
            long productId;
            System.Text.StringBuilder strtext = new System.Text.StringBuilder();
            strtext.Append("导入失败商品信息如下：");
            strtext.AppendLine();
            BLL.Shop.Products.SKUInfo skuBll = new SKUInfo();
            bool status;
            foreach (YSWL.MALL.Model.Shop.Products.ProductInfo item in list)
            {
                if (item.SkuInfos == null || item.SkuInfos.Count == 0) {
                    errorCount++;
                    //错误日志记录
                    strtext.AppendFormat("名称：{0}，编码：{1}", item.ProductName, item.ProductCode);
                    strtext.AppendLine();
                    continue;
                }
                YSWL.MALL.Model.Shop.Products.SKUInfo  skuInfo=skuBll.GetModelBySKU(item.SkuInfos[0].SKU);
                if (skuInfo != null)
                {
                    productId= item.ProductId = skuInfo.ProductId;
                    //sku存在 则更新
                    status = service.ImportModifyProduct(item);  
                    if (status && productId > 0)
                    {
                        successCount++;
                    }
                }
                else {
                    //sku不存在 则添加
                    status = service.AddProduct(item, out productId);
                        if (status && productId > 0)
                        {
                            successCount++;
                        }
                }
                if (!status || productId<=0) {
                    errorCount++;
                    //错误日志记录
                    strtext.AppendFormat("名称：{0}，编码：{1}", item.ProductName, item.ProductCode);
                    strtext.AppendLine();
                }
            }
            strtext.AppendLine();
            if (errorCount > 0) {
                 System.Text.StringBuilder strhead= new System.Text.StringBuilder();
                 strhead.Append(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                 strhead.AppendLine();
                 strhead.AppendFormat("{0}条成功,{1}条失败", successCount, errorCount);
                 strhead.Append(strtext.ToString());
                 errorfileurl = string.Format("/log/ImportProductLog_{0}.txt",System.DateTime.Now.ToString("yyyyMMdd"));
                 Common.FileManage.WriteText(strhead, "ImportProductLog");  
            }
        }
        private static List<YSWL.MALL.Model.Shop.Products.ProductInfo> ImportProdDataTableToList(DataTable dt)
        {
            BLL.Shop.Products.ProductInfo bllprod = new ProductInfo();
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> modelList = new List<YSWL.MALL.Model.Shop.Products.ProductInfo>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Shop.Products.ProductInfo model;
                Model.Shop.Products.SKUInfo skuMode;
                int displaySequence = bllprod.MaxSequence();
                string productCategories;
                string[] cateArray;
                List<YSWL.MALL.Model.Shop.Products.CategoryInfo> cateList = BLL.Shop.Products.CategoryInfo.GetAllCateList();
                List<YSWL.MALL.Model.Shop.Products.CategoryInfo> cateModel;
                int cateId;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new YSWL.MALL.Model.Shop.Products.ProductInfo();
                    productCategories = "";
                    model.HasSKU = false;
                    model.DisplaySequence = ++displaySequence;

                    if (dt.Rows[n]["TypeId"] != null && dt.Rows[n]["TypeId"].ToString() != "")
                    {
                        model.TypeId = Common.Globals.SafeInt(dt.Rows[n]["TypeId"].ToString(),0);
                    }
                    if (dt.Rows[n]["BrandId"] != null && dt.Rows[n]["BrandId"].ToString() != "")
                    {
                        model.BrandId = Common.Globals.SafeInt(dt.Rows[n]["BrandId"].ToString(),0);
                    }
                    if (dt.Rows[n]["ProductName"] != null && dt.Rows[n]["ProductName"].ToString() != "")
                    {
                        model.ProductName = dt.Rows[n]["ProductName"].ToString();
                    }
                    if (dt.Rows[n]["ProductCode"] != null && dt.Rows[n]["ProductCode"].ToString() != "")
                    {
                        model.ProductCode = dt.Rows[n]["ProductCode"].ToString();
                    }
                    //if (dt.Rows[n]["Unit"] != null && dt.Rows[n]["Unit"].ToString() != "")
                    //{
                    //    model.Unit = dt.Rows[n]["Unit"].ToString();
                    //}
                    if (dt.Rows[n]["Description"] != null && dt.Rows[n]["Description"].ToString() != "")
                    {
                        model.Description = dt.Rows[n]["Description"].ToString();
                    }
                    if (dt.Rows[n]["SaleStatus"] != null && dt.Rows[n]["SaleStatus"].ToString() != "")
                    {
                        model.SaleStatus = Common.Globals.SafeInt(dt.Rows[n]["SaleStatus"].ToString(), 0) ;
                    }
                    if (dt.Rows[n]["AddedDate"] != null && dt.Rows[n]["AddedDate"].ToString() != "")
                    {
                        model.AddedDate =Common.Globals.SafeDateTime(dt.Rows[n]["AddedDate"].ToString(),System.DateTime.Now);
                    }
                    if (dt.Rows[n]["MarketPrice"] != null && dt.Rows[n]["MarketPrice"].ToString() != "")
                    {
                        model.MarketPrice = Common.Globals.SafeDecimal(dt.Rows[n]["MarketPrice"].ToString(),null);
                    }
                    if (dt.Rows[n]["LowestSalePrice"] != null && dt.Rows[n]["LowestSalePrice"].ToString() != "")
                    {
                        model.LowestSalePrice =Common.Globals.SafeDecimal(dt.Rows[n]["LowestSalePrice"].ToString(),0);
                    }
                    if (dt.Rows[n]["Points"] != null && dt.Rows[n]["Points"].ToString() != "")
                    {
                        model.Points =Common.Globals.SafeDecimal(dt.Rows[n]["Points"].ToString(),0);
                    }
                    if (dt.Rows[n]["ImageUrl"] != null && dt.Rows[n]["ImageUrl"].ToString() != "")
                    {
                        model.ImageUrl ="/Upload/Shop/Images/Product/"+ dt.Rows[n]["ImageUrl"].ToString();
                        if (dt.Rows[n]["ThumbnailUrl1"] != null && dt.Rows[n]["ThumbnailUrl1"].ToString() != "")
                        {
                            model.ThumbnailUrl1 = "/Upload/Shop/Images/ProductThumbs/" + dt.Rows[n]["ThumbnailUrl1"].ToString();
                        }
                        else
                        {
                            model.ThumbnailUrl1 = "/Content/themes/base/Shop/images/none.png";
                        }
                    }
                    else
                    {
                        model.ImageUrl = "/Content/themes/base/Shop/images/none.png";
                        model.ThumbnailUrl1 = "/Content/themes/base/Shop/images/none.png";
                    }

                


                    #region  产品分类
                    if (dt.Rows[n]["CategoryIds"] != null && dt.Rows[n]["CategoryIds"].ToString() != "")
                    {
                        cateArray = dt.Rows[n]["CategoryIds"].ToString().Split(new char[] { ',','，' }, System.StringSplitOptions.RemoveEmptyEntries);

                        if (cateArray != null && cateArray.Length > 0)
                        {
                            foreach (string itemcateId in cateArray)
                            {
                                cateModel = new List<Model.Shop.Products.CategoryInfo>();
                                cateId = Common.Globals.SafeInt(itemcateId, 0);
                                if (cateId > 0)
                                {
                                    cateModel = cateList.Where(c => c.CategoryId == cateId).ToList();
                                    if (cateModel == null || cateModel.Count <= 0)
                                    {
                                        continue;
                                    }
                                    productCategories += string.Format("{0}_{1},", cateModel[0].CategoryId, cateModel[0].Path);  
                                }
                            }
                            model.Product_Categories = productCategories.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                        }
                    }
                    #endregion

                    #region  sku数据
                    skuMode = new Model.Shop.Products.SKUInfo();
                    skuMode.AlertStock = 0;
                    skuMode.Upselling = true;
                    skuMode.SKU = model.ProductCode;
                    skuMode.SalePrice = model.LowestSalePrice;
                    if (dt.Rows[n]["CostPrice"] != null && dt.Rows[n]["CostPrice"].ToString() != "")
                    {
                        skuMode.CostPrice =Common.Globals.SafeDecimal(dt.Rows[n]["CostPrice"].ToString(),null);
                    }
                    if (dt.Rows[n]["Stock"] != null && dt.Rows[n]["Stock"].ToString() != "")
                    {
                        skuMode.Stock = Common.Globals.SafeInt(dt.Rows[n]["Stock"].ToString(), 0);
                    }
                    if (dt.Rows[n]["Weight"] != null && dt.Rows[n]["Weight"].ToString() != "")
                    {
                        skuMode.Weight = Common.Globals.SafeInt(dt.Rows[n]["Weight"].ToString(), 0);
                    }
                    else {
                        skuMode.Weight = 0;
                    }
                    model.SkuInfos.Add(skuMode);
                    #endregion

                    modelList.Add(model);
                }
            }
            return modelList;
        }

        #endregion

        public static bool ModifyPMSProduct(Model.Shop.Products.ProductInfo productInfo)
        {
            //保留现有SKU库存
            YSWL.MALL.BLL.Shop.Products.SKUInfo skuBll=new SKUInfo();
            List <YSWL.MALL.Model.Shop.Products.SKUInfo> infoList= skuBll.GetProductSkuInfo(productInfo.ProductId);
            if (infoList != null&&productInfo.SkuInfos!=null)
            {
                YSWL.MALL.Model.Shop.Products.SKUInfo skuInfo = null;
                foreach (var item in productInfo.SkuInfos)
                {
                    skuInfo = infoList.Find(c => c.SKU == item.SKU);
                    if (skuInfo != null)
                    {
                        item.Stock = skuInfo.Stock;
                    }
                }
            }
            return service.ModifyPMSProduct(productInfo);
        }


        public static bool AddPMSProduct(Model.Shop.Products.ProductInfo productInfo)
        {
            return service.AddPMSProduct(productInfo);
        }

        #region 修改SKU库存信息
        public static bool ModifyStock(List<Model.Shop.Products.SKUInfo> skuInfo)
        {
            bool isSuccess = true;
            foreach (var item in skuInfo)
            {
                if (!service.ModifyStock(item.SKU, item.Stock))
                {
                    isSuccess = false;
                }
            }
            return isSuccess;
        }

        public static bool ModifyStock(string sku,int stock)
        {
            return service.ModifyStock(sku, stock);
        }
        #endregion

    }
}