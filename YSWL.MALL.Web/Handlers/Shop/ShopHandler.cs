/**
* ShopHandler.cs
*
* 功 能： [N/A]
* 类 名： ShopHandler
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/5/27 13:12:07  Rock    初版
* V0.02  2012/6/14 20:09:05  Ben     1. 新增Json模式
* 　　　　　　　　　　　　　　　　　 2. 产品类型相关操作
* 　　　　　　　　　　　　　　　　　 3. 品牌json版相关操作
* 　　　　　　　　　　　　　　　　　 4. 属性/规格相关操作
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：小鸟科技（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using YSWL.MALL.BLL.Shop.Commission;
using YSWL.MALL.BLL.Shop.Sales;
using YSWL.Json;
using YSWL.MALL.BLL.Shop.Package;
using YSWL.Common;
using YSWL.MALL.Model.Shop.Products;

namespace YSWL.MALL.Web.Handlers.Shop
{
    public class ShopHandler : IHttpHandler
    {
        public const string SHOP_KEY_STATUS = "STATUS";
        public const string SHOP_KEY_DATA = "DATA";

        public const string SHOP_STATUS_SUCCESS = "SUCCESS";
        public const string SHOP_STATUS_FAILED = "FAILED";
        public const string SHOP_STATUS_ERROR = "ERROR";

        #region IHttpHandler 成员

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            //安全起见, 所有产品相关Ajax请求为POST模式
            string action = context.Request.Form["Action"];

            context.Response.Clear();
            context.Response.ContentType = "application/json";
            string msg = "";
            
            try
            {
                switch (action)
                {
                    #region 属性/规格

                    case "GetAttributesList":
                        GetAttributesList(context);
                        break;

                    case "EditValue":
                        EditValue(context);
                        break;

                    #endregion 属性/规格

                    #region 产品类型

                    case "GetProductTypesKVList":
                        GetProductTypesKVList(context);
                        break;

                    #endregion 产品类型

                    #region 品牌

                    case "GetBrandsKVList":
                        GetBrandsKVList(context);
                        break;

                    case "GetBrandsList":
                        GetBrandsInfo(context);
                        break;

                    case "DeleteBrands":
                        DeleteBrands(context);
                        break;

                    case "DeleteImage":
                        DeleteImage(context);
                        break;

                    #endregion 品牌

                    #region 商品分类

                    case "GetChildNode":
                        GetChildNode(context);
                        break;

                    case "GetDepthNode":
                        GetDepthNode(context);
                        break;

                    case "GetParentNode":
                        GetParentNode(context);
                        break;

                    case "IsExistedProduct":
                        IsExistedProduct(context);
                        break;

                    #endregion 商品分类

                    #region 礼品分类

                    case "GetGiftChildNode":
                        GetGiftChildNode(context);
                        break;

                    case "GetGiftDepthNode":
                        GetGiftDepthNode(context);
                        break;

                    case "GetGiftParentNode":
                        GetGiftParentNode(context);
                        break;

                    case "IsExistedGift":
                        IsExistedGift(context);
                        break;

                    #endregion 礼品分类

                  

                    case "ProductInfo":
                        ProductInfo(context);
                        break;

                    case "LoadExistAttributes":
                        LoadExistAttributes(context);
                        break;

                    case "LoadAttributesvalues":
                        LoadAttributesvalues(context);
                        break;

                    case "ProductSkuInfo":
                        ProductSkuInfo(context);
                        break;

                    case "ProductAccessoriesManage":
                        ProductAccessoriesManage(context);
                        break;

                    case "ProductAccessoriesValues":
                        ProductAccessoriesValues(context);
                        break;

                    case "RelatedProductFactory":
                        RelatedProductFactory(context);
                        break;

                    case "ProductIamges":
                        ProductIamges(context);
                        break;

                    case "GetPackage":
                        GetPackageNode(context);
                        break;

                    case "IsExistSkuCode":
                        IsExistSkuCode(context);
                        break;

                    #region 添加、删除商品推荐

                    case "InsertProductStationMode":
                        InsertProductStationMode(context);
                        break;

                    case "RemoveProductStationMode":
                        RemoveProductStationMode(context);
                        break;

                    case "SEORelation":
                        SEORelation(context);
                        break;

                    #endregion 添加、删除商品推荐

                    #region  添加，删除批发规则商品
                    case "AddRuleProduct":
                        msg = AddRuleProduct(context);
                        break;

                    case "DeleteRuleProduct":
                        msg = DeleteRuleProduct(context);
                        break;
                    #endregion

                    #region  添加，删除佣金规则商品
                    case "AddCommissionPro":
                        msg = AddCommissionPro(context);
                        break;

                    case "DeleteCommissionPro":
                        msg = DeleteCommissionPro(context);
                        break;
                        
                    #endregion

                    #region 店铺商品分类
                    case "GetSuppCateNode":
                        GetSuppCateNode(context);
                        return;
                    #endregion


                    #region 添加、删除分仓商品
                    case "AddDepotProduct":
                        AddDepotProduct(context);
                        break;
                    case "DeleteDepotProduct":
                        DeleteDepotProduct(context);
                        break;
                    #endregion

 

                    default:
                        break;
                }
                context.Response.Write(msg);
            }
            catch (Exception ex)
            {
                JsonObject json = new JsonObject();
                json.Put(SHOP_KEY_STATUS, SHOP_STATUS_ERROR);
                json.Put(SHOP_KEY_DATA, ex.Message);
                context.Response.Write(json.ToString());
            }
        }

        #endregion IHttpHandler 成员

        #region 2012年10月15日 19:36:39 Rock ADD


        #region SEO关联链接

        private void SEORelation(HttpContext context)
        {
            JsonObject json = new JsonObject();
            string IsCMS = context.Request.Params["IsCMS"];
            string IsShop = context.Request.Params["IsShop"];
            string IsSNS = context.Request.Params["IsSNS"];
            string IsComment = context.Request.Params["IsComment"];
            System.Text.StringBuilder strWhere = new System.Text.StringBuilder();
            strWhere.Append(" IsActive=1 ");
            if (!string.IsNullOrWhiteSpace(IsCMS) && bool.Parse(IsCMS))
            {
                strWhere.AppendFormat(" AND IsCMS=1 ");
            }
            if (!string.IsNullOrWhiteSpace(IsShop) && bool.Parse(IsShop))
            {
                strWhere.AppendFormat(" AND IsShop=1 ");
            }
            if (!string.IsNullOrWhiteSpace(IsSNS) && bool.Parse(IsSNS))
            {
                strWhere.AppendFormat(" AND IsSNS=1 ");
            }
            if (!string.IsNullOrWhiteSpace(IsComment) && bool.Parse(IsComment))
            {
                strWhere.AppendFormat(" AND IsComment=1 ");
            }
            if (!string.IsNullOrWhiteSpace(strWhere.ToString()))
            {
                BLL.Settings.SEORelation manage = new BLL.Settings.SEORelation();
                List<Model.Settings.SEORelation> list = manage.GetModelList(strWhere.ToString());
                if (list != null && list.Count > 0)
                {
                    JsonArray data = new JsonArray();
                    list.ForEach(info => data.Add(new JsonObject(new string[] { "KeyName", "LinkURL" }, new object[] { info.KeyName, info.LinkURL })));

                    json.Put(SHOP_KEY_STATUS, SHOP_STATUS_SUCCESS);
                    json.Put(SHOP_KEY_DATA, data);
                }
                else
                {
                    json.Put(SHOP_KEY_STATUS, SHOP_STATUS_FAILED);
                }
            }
            else
            {
                json.Put(SHOP_KEY_STATUS, SHOP_STATUS_FAILED);
            }
            context.Response.Write(json.ToString());
        }

        #endregion SEO关联链接

        #endregion 2012年10月15日 19:36:39 Rock ADD

        #region 检查商品编码是否存在

        private void IsExistSkuCode(HttpContext context)
        {
            string SKUCode = context.Request.Form["SKUCode"];
            long pid = Globals.SafeLong(context.Request.Form["pid"], -1);
            JsonObject json = new JsonObject();
            if (!string.IsNullOrWhiteSpace(SKUCode))
            {
                BLL.Shop.Products.SKUInfo manage = new BLL.Shop.Products.SKUInfo();
                if (manage.Exists(SKUCode, pid))
                {
                    json.Put(SHOP_KEY_STATUS, SHOP_STATUS_FAILED);
                }
                else
                {
                    json.Put(SHOP_KEY_STATUS, SHOP_STATUS_SUCCESS);
                }
            }
            else
            {
                json.Put(SHOP_KEY_STATUS, SHOP_STATUS_ERROR);
            }
            context.Response.Write(json.ToString());
        }

        #endregion 检查商品编码是否存在

        #region 添加、删除商品推荐

        private BLL.Shop.Products.ProductStationMode productStationModeBLL = new BLL.Shop.Products.ProductStationMode();

        private string InsertProductStationMode(HttpContext context)
        {
            JsonObject json = new JsonObject();
            int productId = Convert.ToInt32(context.Request.Form["ProductId"]);
            int type = Convert.ToInt32(context.Request.Form["Type"]);

            if (productStationModeBLL.Exists(productId, type))
            {
                json.Put(SHOP_KEY_STATUS, "Presence");
                return json.ToString();
            }
            ProductStationMode productStationMode = new ProductStationMode();
            productStationMode.ProductId = productId;
            productStationMode.DisplaySequence = productStationModeBLL.GetRecordCount(string.Empty) == 0 ? 1 : productStationModeBLL.GetRecordCount(string.Empty) + 1;
            productStationMode.Type = type;

            if (productStationModeBLL.Add(productStationMode) > 0)
            {
                json.Put(SHOP_KEY_STATUS, SHOP_STATUS_SUCCESS);
                json.Put(SHOP_KEY_DATA, "Approve");
            }
            else
            {
                json.Put(SHOP_KEY_STATUS, "NODATA");
                return json.ToString();
            }
            return json.ToString();
        }

        //删除
        private string RemoveProductStationMode(HttpContext context)
        {
            int productId = Convert.ToInt32(context.Request.Form["ProductId"]);
            int type = Convert.ToInt32(context.Request.Form["Type"]);
            JsonObject json = new JsonObject();
            if (productStationModeBLL.Delete(productId, type))
            {
                json.Put(SHOP_KEY_STATUS, SHOP_STATUS_SUCCESS);
            }
            else
            {
                json.Put(SHOP_KEY_STATUS, "NODATA");
                return json.ToString();
            }
            return json.ToString();
        }

        #endregion 添加、删除商品推荐

        #region 添加、删除批发规则商品

        private BLL.Shop.Sales.SalesRuleProduct ruleProductBll = new SalesRuleProduct();

        private string AddRuleProduct(HttpContext context)
        {
            JsonObject json = new JsonObject();
            int productId = Convert.ToInt32(context.Request.Form["ProductId"]);
            int ruleId = Convert.ToInt32(context.Request.Form["RuleId"]);
            string productName = context.Request.Form["ProductName"];
          
            if (ruleProductBll.Exists(ruleId, productId))
            {
                json.Put(SHOP_KEY_STATUS, "Presence");
                return json.ToString();
            }
            YSWL.MALL.Model.Shop.Sales.SalesRuleProduct ruleProductModel = new Model.Shop.Sales.SalesRuleProduct();
            ruleProductModel.ProductId = productId;
            ruleProductModel.RuleId = ruleId;
            ruleProductModel.ProductName = productName;

            if (ruleProductBll.Add(ruleProductModel))
            {
                json.Put(SHOP_KEY_STATUS, SHOP_STATUS_SUCCESS);
                json.Put(SHOP_KEY_DATA, "Approve");
            }
            else
            {
                json.Put(SHOP_KEY_STATUS, "NODATA");
                return json.ToString();
            }
            return json.ToString();
        }

        //删除
        private string DeleteRuleProduct(HttpContext context)
        {
            int productId = Convert.ToInt32(context.Request.Form["ProductId"]);
            int ruleId = Convert.ToInt32(context.Request.Form["RuleId"]);
            JsonObject json = new JsonObject();
            if (ruleProductBll.Delete(ruleId, productId))
            {
                json.Put(SHOP_KEY_STATUS, SHOP_STATUS_SUCCESS);
            }
            else
            {
                json.Put(SHOP_KEY_STATUS, "NODATA");
                return json.ToString();
            }
            return json.ToString();
        }

        #endregion 添加、删除商品推荐

        #region 添加、删除佣金规则商品

        private BLL.Shop.Commission.CommissionPro comProBll = new CommissionPro();

        private string AddCommissionPro(HttpContext context)
        {
            JsonObject json = new JsonObject();
            int productId = Convert.ToInt32(context.Request.Form["ProductId"]);
            int ruleId = Convert.ToInt32(context.Request.Form["RuleId"]);
            string productName = context.Request.Form["ProductName"];
            if (comProBll.Exists(ruleId, productId))
            {
                json.Put(SHOP_KEY_STATUS, "Presence");
                return json.ToString();
            }
            YSWL.MALL.Model.Shop.Commission.CommissionPro ruleProductModel = new Model.Shop.Commission.CommissionPro();
            ruleProductModel.ProductId = productId;
            ruleProductModel.RuleId = ruleId;

            if (comProBll.Add(ruleProductModel))
            {
                json.Put(SHOP_KEY_STATUS, SHOP_STATUS_SUCCESS);
                json.Put(SHOP_KEY_DATA, "Approve");
            }
            else
            {
                json.Put(SHOP_KEY_STATUS, "NODATA");
                return json.ToString();
            }
            return json.ToString();
        }

        //删除
        private string DeleteCommissionPro(HttpContext context)
        {
            int productId = Convert.ToInt32(context.Request.Form["ProductId"]);
            int ruleId = Convert.ToInt32(context.Request.Form["RuleId"]);
            JsonObject json = new JsonObject();
            if (comProBll.Delete(ruleId, productId))
            {
                json.Put(SHOP_KEY_STATUS, SHOP_STATUS_SUCCESS);
            }
            else
            {
                json.Put(SHOP_KEY_STATUS, "NODATA");
                return json.ToString();
            }
            return json.ToString();
        }

        #endregion

        #region 根据商品ID获取商品信息

        #region 获取商品相关配件信息

        private void ProductAccessoriesValues(HttpContext context)
        {
            JsonObject json = new JsonObject();
            string strPid = context.Request.Params["pid"];
            if (!string.IsNullOrWhiteSpace(strPid))
            {
                long pid = Globals.SafeLong(strPid, -1);
                BLL.Shop.Products.AccessoriesValue manage = new BLL.Shop.Products.AccessoriesValue();
                List<Model.Shop.Products.AccessoriesValue> list = null;//TODO  //manage.AccessoriesByProductId(pid);
                if (list != null && list.Count > 0)
                {
                    System.Text.StringBuilder strAccValues = new System.Text.StringBuilder();
                    list.ForEach(info =>
                    {
                        strAccValues.Append(info.SKU);
                        strAccValues.Append(",");
                    });
                    json.Put(SHOP_KEY_STATUS, SHOP_STATUS_SUCCESS);
                    json.Put(SHOP_KEY_DATA, strAccValues.ToString());
                }
                else
                {
                    json.Put(SHOP_KEY_STATUS, SHOP_STATUS_FAILED);
                }
            }
            else
            {
                json.Put(SHOP_KEY_STATUS, SHOP_STATUS_FAILED);
            }
            context.Response.Write(json.ToString());
        }

        #endregion 获取商品相关配件信息

        #region 获取单个商品的相关商品信息

        private void RelatedProductFactory(HttpContext context)
        {
            JsonObject json = new JsonObject();
            string strPid = context.Request.Params["pid"];
            if (!string.IsNullOrWhiteSpace(strPid))
            {
                long pid = Globals.SafeLong(strPid, -1);
                BLL.Shop.Products.RelatedProduct manage = new BLL.Shop.Products.RelatedProduct();
                List<Model.Shop.Products.RelatedProduct> list = manage.GetModelList(pid);
                if (list != null && list.Count > 0)
                {
                    System.Text.StringBuilder strReleatedInfo = new System.Text.StringBuilder();
                    list.ForEach(info =>
                    {
                        strReleatedInfo.Append(info.RelatedId);
                        strReleatedInfo.Append(",");
                    });
                    json.Put(SHOP_KEY_STATUS, SHOP_STATUS_SUCCESS);
                    json.Put(SHOP_KEY_DATA, strReleatedInfo.ToString());
                }
                else
                {
                    json.Put(SHOP_KEY_STATUS, SHOP_STATUS_FAILED);
                }
            }
            else
            {
                json.Put(SHOP_KEY_STATUS, SHOP_STATUS_FAILED);
            }
            context.Response.Write(json.ToString());
        }

        #endregion 获取单个商品的相关商品信息

        #region 商品配件基础信息

        private void ProductAccessoriesManage(HttpContext context)
        {
            JsonObject json = new JsonObject();
            string strPid = context.Request.Params["pid"];
            string strtype = context.Request.Params["actype"];
            if (!string.IsNullOrWhiteSpace(strPid))
            {
                long pid = Globals.SafeLong(strPid, -1);
                int type = Globals.SafeInt(strtype, -1);
                BLL.Shop.Products.ProductAccessorie manage = new BLL.Shop.Products.ProductAccessorie();
                List<Model.Shop.Products.ProductAccessorie> list = manage.GetModelList(pid, type);
                if (list != null && list.Count > 0)
                {
                    JsonArray data = new JsonArray();
                    list.ForEach(info => data.Add(new JsonObject(new string[] { "ProductId", "AccessoriesId", "Name", "MaxQuantity", "MinQuantity", "DiscountType", "DiscountAmount", "Stock" },
                        new object[] { info.ProductId, info.AccessoriesId, info.Name, info.MaxQuantity, info.MinQuantity, info.DiscountType, info.DiscountAmount, info.Stock })));
                    json.Put(SHOP_KEY_STATUS, SHOP_STATUS_SUCCESS);
                    json.Put(SHOP_KEY_DATA, data);
                }
                else
                {
                    json.Put(SHOP_KEY_STATUS, SHOP_STATUS_FAILED);
                }
            }
            else
            {
                json.Put(SHOP_KEY_STATUS, SHOP_STATUS_FAILED);
            }
            context.Response.Write(json.ToString());
        }

        #endregion 商品配件基础信息

        #region 商品规格值信息

        private void ProductSkuInfo(HttpContext context)
        {
            JsonObject json = new JsonObject();
            string strPid = context.Request.Params["pid"];
            if (!string.IsNullOrWhiteSpace(strPid))
            {
                long pid = Globals.SafeLong(strPid, -1);
                BLL.Shop.Products.SKUInfo manage = new BLL.Shop.Products.SKUInfo();
                List<Model.Shop.Products.SKUInfo> list = manage.GetProductSkuInfo(pid);
                if (list != null && list.Count > 0)
                {
                    JsonArray data = new JsonArray();
                    list.ForEach(info => data.Add(new JsonObject(
                        new string[] { "SkuId", "ProductId", "SKU", "Weight", "Stock", "AlertStock", "CostPrice", "SalePrice", "Upselling" },
                        new object[] { info.SkuId, info.ProductId, info.SKU, info.Weight, info.Stock, info.AlertStock, info.CostPrice, info.SalePrice, info.Upselling })));
                    json.Put(SHOP_KEY_STATUS, SHOP_STATUS_SUCCESS);
                    json.Put(SHOP_KEY_DATA, data);
                }
                else
                {
                    json.Put(SHOP_KEY_STATUS, SHOP_STATUS_FAILED);
                }
            }
            else
            {
                json.Put(SHOP_KEY_STATUS, SHOP_STATUS_FAILED);
            }
            context.Response.Write(json.ToString());
        }

        #endregion 商品规格值信息

        private void ProductIamges(HttpContext context)
        {
            JsonObject json = new JsonObject();
            string strPid = context.Request.Params["pid"];
            if (!string.IsNullOrWhiteSpace(strPid))
            {
                long pid = Globals.SafeLong(strPid, -1);
                BLL.Shop.Products.ProductImage manage = new BLL.Shop.Products.ProductImage();
                List<Model.Shop.Products.ProductImage> list = manage.GetModelList(pid);
                if (list != null && list.Count > 0)
                {
                    JsonArray data = new JsonArray();
                    list.ForEach(info => data.Add(new JsonObject(
                        new string[] { "ProductImageId", "ProductId", "ImageUrl", "ThumbnailUrl1", "ThumbnailUrl2" },
                        new object[] { info.ProductImageId, info.ProductId, info.ImageUrl, info.ThumbnailUrl1, info.ThumbnailUrl2 }
                        )));
                    json.Put(SHOP_KEY_STATUS, SHOP_STATUS_SUCCESS);
                    json.Put(SHOP_KEY_DATA, data);
                }
                else
                {
                    json.Put(SHOP_KEY_STATUS, SHOP_STATUS_FAILED);
                }
            }
            else
            {
                json.Put(SHOP_KEY_STATUS, SHOP_STATUS_FAILED);
            }
            context.Response.Write(json.ToString());
        }

        private void LoadAttributesvalues(HttpContext context)
        {
            JsonObject json = new JsonObject();
            string strPid = context.Request.Params["pid"];
            if (!string.IsNullOrWhiteSpace(strPid))
            {
                long pid = Globals.SafeLong(strPid, -1);
                BLL.Shop.Products.SKUItem manage = new BLL.Shop.Products.SKUItem();
                List<Model.Shop.Products.SKUItem> list = manage.AttributeValueInfo(pid);
                if (list != null && list.Count > 0)
                {
                    JsonArray data = new JsonArray();
                    list.ForEach(info => data.Add(new JsonObject(new string[] { "SpecId", "AttributeId", "ValueId", "ImageUrl", "ValueStr", "UserDefinedPic" }, new object[] { info.SpecId, info.AttributeId, info.ValueId, info.ImageUrl, info.ValueStr, info.UserDefinedPic })));

                    json.Put(SHOP_KEY_STATUS, SHOP_STATUS_SUCCESS);
                    json.Put(SHOP_KEY_DATA, data);
                }
                else
                {
                    json.Put(SHOP_KEY_STATUS, SHOP_STATUS_FAILED);
                }
            }
            else
            {
                json.Put(SHOP_KEY_STATUS, SHOP_STATUS_FAILED);
            }
            context.Response.Write(json.ToString());
        }

        private void ProductInfo(HttpContext context)
        {
            JsonObject json = new JsonObject();
            string strPid = context.Request.Params["pid"];
            if (!string.IsNullOrWhiteSpace(strPid))
            {
                long pid = Globals.SafeLong(strPid, -1);
                BLL.Shop.Products.ProductInfo manage = new BLL.Shop.Products.ProductInfo();
                List<Model.Shop.Products.ProductInfo> list = manage.GetModelList(pid);
                if (list != null && list.Count > 0)
                {
                    JsonArray data = new JsonArray();
                    list.ForEach(info => data.Add(new JsonObject(
                        new string[] { "CategoryId", "TypeId", "ProductId", "BrandId", "ProductName", "ProductCode", "EnterpriseId", "RegionId", "ShortDescription", "Unit", "Description", "Title", "Meta_Description", "Meta_Keywords", "DisplaySequence", "MarketPrice", "HasSKU", "ImageUrl", "ThumbnailUrl1", "MaxQuantity", "MinQuantity", "SaleStatus" },
                        new object[] { info.CategoryId, info.TypeId, info.ProductId, info.BrandId, info.ProductName, info.ProductCode, info.SupplierId, info.RegionId, info.ShortDescription, info.Unit, info.Description, info.Meta_Title, info.Meta_Description, info.Meta_Keywords, info.DisplaySequence, info.MarketPrice, info.HasSKU, info.ImageUrl, info.ThumbnailUrl1, info.MaxQuantity, info.MinQuantity, info.SaleStatus }
                        )));

                    json.Put(SHOP_KEY_STATUS, SHOP_STATUS_SUCCESS);
                    json.Put(SHOP_KEY_DATA, data);
                }
                else
                {
                    json.Put(SHOP_KEY_STATUS, SHOP_STATUS_FAILED);
                }
            }
            else
            {
                json.Put(SHOP_KEY_STATUS, SHOP_STATUS_FAILED);
            }
            context.Response.Write(json.ToString());
        }

        private void LoadExistAttributes(HttpContext context)
        {
            JsonObject json = new JsonObject();
            string strPid = context.Request.Params["pid"];
            if (!string.IsNullOrWhiteSpace(strPid))
            {
                long pid = Globals.SafeLong(strPid, -1);
                BLL.Shop.Products.AttributeInfo manage = new BLL.Shop.Products.AttributeInfo();
                List<Model.Shop.Products.AttributeHelper> list = manage.ProductAttributeInfo(pid);
                if (list != null && list.Count > 0)
                {
                    JsonArray data = new JsonArray();
                    list.ForEach(
                        info =>
                            data.Add(new JsonObject(new string[] { "AttributeId", "ValueId", "UsageMode", "ValueStr" },
                                new object[] {info.AttributeId, info.ValueId, info.UsageMode, info.ValueStr})));
                    json.Put(SHOP_KEY_STATUS, SHOP_STATUS_SUCCESS);
                    json.Put(SHOP_KEY_DATA, data);
                }
                else
                {
                    json.Put(SHOP_KEY_STATUS, SHOP_STATUS_FAILED);
                }
            }
            else
            {
                json.Put(SHOP_KEY_STATUS, SHOP_STATUS_FAILED);
            }
            context.Response.Write(json.ToString());
        }

        #endregion 根据商品ID获取商品信息

        #region 商品分类

        private BLL.Shop.Products.CategoryInfo cateBll = new BLL.Shop.Products.CategoryInfo();

        private void IsExistedProduct(HttpContext context)
        {
            string CategoryIdStr = context.Request.Params["CategoryId"];
            int cateId = Globals.SafeInt(CategoryIdStr, -2);
            JsonObject json = new JsonObject();
            if (cateBll.IsExistedProduce(cateId))
            {
                json.Put(SHOP_KEY_STATUS, SHOP_STATUS_SUCCESS);
            }
            else
            {
                json.Put(SHOP_KEY_STATUS, SHOP_STATUS_FAILED);
            }
            context.Response.Write(json.ToString());
        }

        private void GetChildNode(HttpContext context)
        {
            string parentIdStr = Common.InjectionFilter.SqlFilter(context.Request.Params["ParentId"]);
            JsonObject json = new JsonObject();
            int parentId = Globals.SafeInt(parentIdStr, 0);
            DataSet ds = cateBll.GetCategorysByParentIdDs(parentId);
            if (ds.Tables[0].Rows.Count < 1)
            {
                json.Accumulate("STATUS", "NODATA");
                context.Response.Write(json.ToString());
                return;
            }
            json.Accumulate("STATUS", "OK");
            json.Accumulate(SHOP_KEY_DATA, ds.Tables[0]);
            context.Response.Write(json.ToString());
        }

        private void GetPackageNode(HttpContext context)
        {
            YSWL.MALL.BLL.Shop.Package.Package bll = new Package();
            int CategoryId = Globals.SafeInt(context.Request.Params["id"], 0);
            string keyword = context.Request.Params["q"];
            JsonObject json = new JsonObject();
            StringBuilder sb = new StringBuilder();
            if (CategoryId > 0)
            {
                sb.Append(" CategoryId =" + CategoryId + "");
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                if (sb.Length > 0)
                {
                    sb.Append(" and ");
                }
                sb.Append(" Name like '%" + Common.InjectionFilter.SqlFilter(keyword) + "%'");
            }
            DataSet ds = null;
            if (sb.Length > 0)
            {
                ds = bll.GetList(sb.ToString());
            }
            if (ds == null || ds.Tables[0].Rows.Count < 1)
            {
                json.Accumulate("STATUS", "NODATA");
                context.Response.Write(json.ToString());
                return;
            }
            json.Accumulate("STATUS", "OK");
            json.Accumulate("DATA", ds.Tables[0]);
            context.Response.Write(json.ToString());
        }

        private void GetDepthNode(HttpContext context)
        {
            int nodeId = Globals.SafeInt(context.Request.Params["NodeId"], 0);
            JsonObject json = new JsonObject();
            List<YSWL.MALL.Model.Shop.Products.CategoryInfo> list;
            if (nodeId > 0)
            {
                Model.Shop.Products.CategoryInfo model = cateBll.GetModel(nodeId);
                list = cateBll.GetCategorysByDepth(model.Depth);
            }
            else
            {
                list = cateBll.GetCategorysByDepth(1);
            }
            if (list.Count < 1)
            {
                json.Accumulate("STATUS", "NODATA");
                context.Response.Write(json.ToString());
                return;
            }
            json.Accumulate("STATUS", "OK");
            JsonArray data = new JsonArray();
            list.ForEach(info => data.Add(
                new JsonObject(
                    new string[] { "ClassID", "ClassName" },
                    new object[] { info.CategoryId, info.Name }
                    )));
            json.Accumulate("DATA", data);
            context.Response.Write(json.ToString());
        }

        private void GetParentNode(HttpContext context)
        {
            int ParentId = Globals.SafeInt(context.Request.Params["NodeId"], 0);
            JsonObject json = new JsonObject();
            DataSet ds = cateBll.GetList("   Status=1  ");
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                Model.Shop.Products.CategoryInfo model = cateBll.GetModel(ParentId);
                if (model != null)
                {
                    string[] strList = model.Path.TrimEnd('|').Split('|');
                    string strClassID = string.Empty;
                    if (strList.Length > 0)
                    {
                        List<DataRow[]> list = new List<DataRow[]>();
                        for (int i = 0; i <= strList.Length; i++)
                        {
                            DataRow[] dsParent = null;
                            if (i == 0)
                            {
                                dsParent = dt.Select("ParentCategoryId=0");
                            }
                            else
                            {
                                dsParent = dt.Select("ParentCategoryId=" + strList[i - 1]);
                            }
                            if (dsParent.Length > 0)
                            {
                                list.Add(dsParent);
                            }
                        }
                        json.Accumulate("STATUS", "OK");
                        json.Accumulate("DATA", list);
                        json.Accumulate("PARENT", strList);
                    }
                    else
                    {
                        json.Accumulate("STATUS", "NODATA");
                        context.Response.Write(json.ToString());
                        return;
                    }
                }
            }

            context.Response.Write(json.ToString());
        }

        #endregion 商品分类

        #region 礼品分类

        private BLL.Shop.Gift.GiftsCategory giftCateBll = new BLL.Shop.Gift.GiftsCategory();

        private void IsExistedGift(HttpContext context)
        {
            string CategoryIdStr = context.Request.Params["CategoryId"];
            int cateId = Globals.SafeInt(CategoryIdStr, -2);
            JsonObject json = new JsonObject();
            if (giftCateBll.IsExistedGift(cateId))
            {
                json.Put(SHOP_KEY_STATUS, SHOP_STATUS_SUCCESS);
            }
            else
            {
                json.Put(SHOP_KEY_STATUS, SHOP_STATUS_FAILED);
            }
            context.Response.Write(json.ToString());
        }

        private void GetGiftChildNode(HttpContext context)
        {
            string parentIdStr = context.Request.Params["ParentId"];
            JsonObject json = new JsonObject();
            int parentId = Globals.SafeInt(parentIdStr, 0);
            DataSet ds = giftCateBll.GetCategorysByParentId(parentId);
            if (ds.Tables[0].Rows.Count < 1)
            {
                json.Accumulate("STATUS", "NODATA");
                context.Response.Write(json.ToString());
                return;
            }
            json.Accumulate("STATUS", "OK");
            json.Accumulate(SHOP_KEY_DATA, ds.Tables[0]);
            context.Response.Write(json.ToString());
        }

        private void GetGiftDepthNode(HttpContext context)
        {
            int nodeId = Globals.SafeInt(context.Request.Params["NodeId"], 0);
            JsonObject json = new JsonObject();
            List<YSWL.MALL.Model.Shop.Gift.GiftsCategory> list;
            if (nodeId > 0)
            {
                Model.Shop.Gift.GiftsCategory model = giftCateBll.GetModel(nodeId);
                list = giftCateBll.GetCategorysByDepth(model.Depth);
            }
            else
            {
                list = giftCateBll.GetCategorysByDepth(1);
            }
            if (list.Count < 1)
            {
                json.Accumulate("STATUS", "NODATA");
                context.Response.Write(json.ToString());
                return;
            }
            json.Accumulate("STATUS", "OK");
            JsonArray data = new JsonArray();
            list.ForEach(info => data.Add(
                new JsonObject(
                    new string[] { "ClassID", "ClassName" },
                    new object[] { info.CategoryID, info.Name }
                    )));
            json.Accumulate("DATA", data);
            context.Response.Write(json.ToString());
        }

        private void GetGiftParentNode(HttpContext context)
        {
            int ParentId = Globals.SafeInt(context.Request.Params["NodeId"], 0);
            JsonObject json = new JsonObject();
            DataSet ds = giftCateBll.GetList("");
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                Model.Shop.Gift.GiftsCategory model = giftCateBll.GetModel(ParentId);
                if (model != null)
                {
                    string[] strList = model.Path.TrimEnd('|').Split('|');
                    string strClassID = string.Empty;
                    if (strList.Length > 0)
                    {
                        List<DataRow[]> list = new List<DataRow[]>();
                        for (int i = 0; i <= strList.Length; i++)
                        {
                            DataRow[] dsParent = null;
                            if (i == 0)
                            {
                                dsParent = dt.Select("ParentCategoryId=0");
                            }
                            else
                            {
                                dsParent = dt.Select("ParentCategoryId=" + strList[i - 1]);
                            }
                            if (dsParent.Length > 0)
                            {
                                list.Add(dsParent);
                            }
                        }
                        json.Accumulate("STATUS", "OK");
                        json.Accumulate("DATA", list);
                        json.Accumulate("PARENT", strList);
                    }
                    else
                    {
                        json.Accumulate("STATUS", "NODATA");
                        context.Response.Write(json.ToString());
                        return;
                    }
                }
            }

            context.Response.Write(json.ToString());
        }

        #endregion 礼品分类

  

        #region 属性/规格

        private void EditValue(HttpContext context)
        {
            JsonObject json = new JsonObject();
            string strValue = context.Request.Form["ValueId"];
            if (!string.IsNullOrWhiteSpace(strValue))
            {
                long ValueId = Convert.ToInt64(strValue);
                BLL.Shop.Products.SKUItem skuBll = new BLL.Shop.Products.SKUItem();
                bool skuResult = skuBll.Exists(null, null, ValueId);
                BLL.Shop.Products.ProductAttribute productAttBll = new BLL.Shop.Products.ProductAttribute();
                bool productResult = productAttBll.Exists(null, null, ValueId);
                if (skuResult || productResult)
                {
                    json.Put(SHOP_KEY_STATUS, SHOP_STATUS_FAILED);
                }
                else
                {
                    BLL.Shop.Products.ProductType ptBll = new BLL.Shop.Products.ProductType();
                    if (ptBll.DeleteManage(null, null, ValueId))
                    {
                        json.Put(SHOP_KEY_STATUS, SHOP_STATUS_SUCCESS);
                    }
                    else
                    {
                        json.Put(SHOP_KEY_STATUS, SHOP_STATUS_FAILED);
                    }
                }
            }
            else
            {
                json.Put(SHOP_KEY_STATUS, SHOP_STATUS_FAILED);
            }
            context.Response.Write(json.ToString());
        }

        private void GetAttributesList(HttpContext context)
        {
            JsonObject json = new JsonObject();
            BLL.Shop.Products.AttributeInfo manage = new BLL.Shop.Products.AttributeInfo();
            string dataMode = context.Request.Form["DataMode"];
            int productTypeId = Globals.SafeInt(context.Request.Form["ProductTypeId"], -1);
            if (string.IsNullOrWhiteSpace(dataMode) || productTypeId < 1)
            {
                json.Put(SHOP_KEY_STATUS, SHOP_STATUS_ERROR);
                context.Response.Write(json.ToString());
                return;
            }
            SearchType searchType;
            if (dataMode == "0")
            {
                searchType = SearchType.ExtAttribute;
            }
            else
            {
                searchType = SearchType.Specification;
            }
            List<Model.Shop.Products.AttributeInfo> list = manage.GetAttributeInfoList(productTypeId, searchType);
            JsonArray data = new JsonArray();
            list.ForEach(info => data.Add(
                new JsonObject(
                    new string[] { "AttributeId", "AttributeName", "AttributeUsageMode", "AttributeValues", "UserDefinedPic" },
                    new object[]
                        {
                            info.AttributeId, info.AttributeName, info.UsageMode,
                            info.AttributeValues,info.UserDefinedPic

                            //info.UsageMode == 2 ? new List<AttributeValue>() : info.AttributeValues
                        }
                    )));
            json.Put(SHOP_KEY_STATUS, SHOP_STATUS_SUCCESS);
            json.Put(SHOP_KEY_DATA, data);
            context.Response.Write(json.ToString());
        }

        #endregion 属性/规格

        #region 产品类型

        private void GetProductTypesKVList(HttpContext context)
        {
            JsonObject json = new JsonObject();
            BLL.Shop.Products.ProductType manage = new BLL.Shop.Products.ProductType();
            List<Model.Shop.Products.ProductType> list = manage.GetProductTypes();
            JsonArray data = new JsonArray();
            list.ForEach(info => data.Add(
                new JsonObject(
                    new string[] { "TypeId", "TypeName" },
                    new object[] { info.TypeId, info.TypeName }
                    )));
            json.Put(SHOP_KEY_STATUS, SHOP_STATUS_SUCCESS);
            json.Put(SHOP_KEY_DATA, data);
            context.Response.Write(json.ToString());
        }

        #endregion 产品类型

        #region 品牌

        private void GetBrandsKVList(HttpContext context)
        {
            JsonObject json = new JsonObject();
            BLL.Shop.Products.BrandInfo manage = new BLL.Shop.Products.BrandInfo();
            List<Model.Shop.Products.BrandInfo> list;
            int productTypeId = Globals.SafeInt(context.Request.Form["ProductTypeId"], -1);
            if (productTypeId < 1)
                list = manage.GetBrands();
            else
                list = manage.GetModelListByProductTypeId(productTypeId);
            JsonArray data = new JsonArray();
            list.ForEach(info => data.Add(
                new JsonObject(
                    new string[] { "BrandId", "BrandName" },
                    new object[] { info.BrandId, info.BrandName }
                    )));
            json.Put(SHOP_KEY_STATUS, SHOP_STATUS_SUCCESS);
            json.Put(SHOP_KEY_DATA, data);
            context.Response.Write(json.ToString());
        }

        /// <summary>
        /// 删除选中的品牌
        /// </summary>
        private void DeleteBrands(HttpContext context)
        {
            JsonObject json = new JsonObject();
            string strID = context.Request.Params["idList"];
            if (!string.IsNullOrWhiteSpace(strID))
            {
                BLL.Shop.Products.BrandInfo bll = new BLL.Shop.Products.BrandInfo();
                BLL.Shop.Products.ProductInfo productManage = new BLL.Shop.Products.ProductInfo();
                BLL.Shop.Products.ProductTypeBrand ProductTypeBandManage = new BLL.Shop.Products.ProductTypeBrand();
                int bid = Common.Globals.SafeInt(strID, 0);
                if (productManage.ExistsBrands(bid))
                {
                    json.Put(SHOP_KEY_STATUS, SHOP_STATUS_FAILED);
                    json.Put(SHOP_KEY_DATA, "该品牌正在使用中！");
                }

                if (bll.DeleteList(strID))
                {
                    ProductTypeBandManage.Delete(null, bid);

                    #region 同步删除PMS基础数据
                    //YSWL.MALL.BLL.Shop.Service.PMSServiceHelper.DeleteEx(bid);
                    #endregion

                    json.Put(SHOP_KEY_STATUS, SHOP_STATUS_SUCCESS);
                }
                else
                {
                    json.Put(SHOP_KEY_STATUS, SHOP_STATUS_FAILED);
                    json.Put(SHOP_KEY_DATA, "系统忙，请稍后再试！");
                }
            }
            else
            {
                json.Put(SHOP_KEY_STATUS, SHOP_STATUS_FAILED);
                json.Put(SHOP_KEY_DATA, "系统忙，请稍后再试！");
            }
            context.Response.Write(json.ToString());
        }

        private void DeleteImage(HttpContext context)
        {
            string vId = context.Request.Form["ValueId"];
            int valueId = Globals.SafeInt(vId, 0);
            BLL.Shop.Products.AttributeValue bll = new BLL.Shop.Products.AttributeValue();
            if (bll.DeleteImage(valueId))
            {
                context.Response.Write(SHOP_STATUS_SUCCESS);
            }
            else
            {
                context.Response.Write(SHOP_STATUS_FAILED);
            }
        }

        /// <summary>
        /// 获取品牌列表
        /// </summary>
        private void GetBrandsInfo(HttpContext context)
        {
            string ProductTypeId = context.Request.Params["ProductTypeId"];
            string strPi = context.Request.Params["pageIndex"];
            string tabNum = context.Request.Params["TabNum"];
            int num = Globals.SafeInt(tabNum, 0);
            int intPi = 0;
            if (!int.TryParse(strPi, out intPi))//将字符串页码 转成 整型页码，如果失败，设置页码为1
            {
                intPi = 1;
            }
            int intPz = Globals.SafeInt(context.Request.Params["pageSize"], 1);
            int rowCount = 0;
            int pageCount = 0;
            if (!string.IsNullOrWhiteSpace(ProductTypeId))
            {
                YSWL.MALL.BLL.Shop.Products.BrandInfo bll = new YSWL.MALL.BLL.Shop.Products.BrandInfo();
                System.Collections.Generic.List<Model.Shop.Products.BrandInfo> list = null;
                if (num == 0)
                {
                    list = bll.GetListByProductTypeId(out rowCount, out pageCount, int.Parse(ProductTypeId), intPi, intPz, 1);
                }
                else
                {
                    list = bll.GetListByProductTypeId(out rowCount, out pageCount, int.Parse(ProductTypeId), intPi, intPz, 1);
                }

                JsonObject json = new JsonObject();
                JsonArray data = new JsonArray();
                list.ForEach(info => data.Add(
                    new JsonObject(
                        new string[] {"BrandId", "BrandName", "DisplaySequence", "Logo", "Description"},
                        new object[]
                        {
                            info.BrandId, info.BrandName, info.DisplaySequence, info.Logo,
                            InjectionFilter.HtmlFilter(info.Description)
                        }
                        )));
                json.Put(SHOP_KEY_STATUS, SHOP_STATUS_SUCCESS);
                json.Put(SHOP_KEY_DATA, data);
                json.Put("rowCount", rowCount);
                json.Put("pageCount", pageCount);
                context.Response.Write(json.ToString());
            }
        }

        #endregion 品牌
        #region 店铺商品分类

        private BLL.Shop.Supplier.SupplierCategories suppcateBll = new BLL.Shop.Supplier.SupplierCategories();
        private void GetSuppCateNode(HttpContext context)
        {
            string parentIdStr = context.Request.Params["ParentId"];
            int suppId = Globals.SafeInt(context.Request.Params["SuppId"], 0);
            JsonObject json = new JsonObject();
            int parentId = Globals.SafeInt(parentIdStr, 0);
            DataSet ds = suppcateBll.GetList(string.Format(" ParentCategoryId = {0} and SupplierId={1}", parentId, suppId));
            if (ds.Tables[0].Rows.Count < 1)
            {
                json.Accumulate("STATUS", "NODATA");
                context.Response.Write(json.ToString());
                return;
            }
            json.Accumulate("STATUS", "OK");
            json.Accumulate(SHOP_KEY_DATA, ds.Tables[0]);
            context.Response.Write(json.ToString());
        }
        #endregion 店铺分类

        #region 分仓商品
        private void AddDepotProduct(HttpContext context)
        {
            string sku = context.Request.Form["Sku"];
            JsonObject json = new JsonObject();
            int depotId = Globals.SafeInt(context.Request.Form["Depot"], 0);
            int stock = Globals.SafeInt(context.Request.Form["Stock"], 0);
            BLL.Shop.DisDepot.DepotProSKUs depotSkuBll = new BLL.Shop.DisDepot.DepotProSKUs();
            if (depotSkuBll.AddProduct(depotId, sku, stock))
            {
                json.Accumulate("STATUS", "OK");
                context.Response.Write(json.ToString());
                return;
            }
            json.Accumulate("STATUS", "NO");
            context.Response.Write(json.ToString());
            return;
        }

        private void DeleteDepotProduct(HttpContext context)
        {
            string sku = Common.InjectionFilter.SqlFilter(context.Request.Form["sku"]);
            JsonObject json = new JsonObject();
            int depotId = Globals.SafeInt(context.Request.Form["depotId"], 0);
            BLL.Shop.DisDepot.DepotProSKUs depotSkuBll = new BLL.Shop.DisDepot.DepotProSKUs();
            if (depotSkuBll.DeleteProduct(depotId, sku))
            {
                json.Accumulate("STATUS", "OK");
                context.Response.Write(json.ToString());
                return;
            }
            json.Accumulate("STATUS", "NO");
            context.Response.Write(json.ToString());
            return;
        }

        #endregion

    }
}