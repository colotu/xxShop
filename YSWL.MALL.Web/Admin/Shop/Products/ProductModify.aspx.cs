﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.Common;
using YSWL.MALL.Model.Settings;
using YSWL.MALL.Model.Shop.Products;
using YSWL.Web;
using AttributeValue = YSWL.MALL.Model.Shop.Products.AttributeValue;
using ProductAccessorie = YSWL.MALL.Model.Shop.Products.ProductAccessorie;
using ProductImage = YSWL.MALL.Model.Shop.Products.ProductImage;

namespace YSWL.MALL.Web.Admin.Shop.Products
{
    public partial class ProductModify : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 486; } } //Shop_商品管理_编辑页

        private BLL.Shop.Products.CategoryInfo categoryManage = new BLL.Shop.Products.CategoryInfo();
        BLL.Shop.Products.ProductInfo prodBll = new BLL.Shop.Products.ProductInfo();
        //待上传的SKU图片名称
        private string tempFile = string.Format("/Upload/Temp/{0}/", DateTime.Now.ToString("yyyyMMdd"));
        private string skuImageFile = string.Format("/Upload/Shop/Images/ProductsSkuImages/{0}/", DateTime.Now.ToString("yyyyMMdd"));
        private ArrayList skuImageList = new ArrayList();
        private ArrayList salePriceList = new ArrayList();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindProductBaseInfo();

                //ProductImagesThumbSize
                Size thumbSize = YSWL.Common.StringPlus.SplitToSize(
                    BLL.SysManage.ConfigSystem.GetValueByCache(SettingConstant.PRODUCT_NORMAL_SIZE_KEY),
                    '|', SettingConstant.ProductThumbSize.Width, SettingConstant.ProductThumbSize.Height);
                hfProductImagesThumbSize.Value = thumbSize.Width + "," + thumbSize.Height;

                //获取是否开启相关选项卡
                this.hfIsOpenSku.Value = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_AddProduct_OpenSku");
                this.hfIsOpenRelated.Value = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_AddProduct_OpenRelated");
                this.hfIsOpenSEO.Value = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_AddProduct_OpenSEO");
            }
        }

        public long ProductId
        {
            get
            {
                long pid = 0;
                if (!string.IsNullOrWhiteSpace(Request.Params["pid"]))
                {
                    pid = Globals.SafeLong(Request.Params["pid"], 0);
                }
                return pid;
            }
        }

        private void BindProductBaseInfo()
        {
            #region 商家
            YSWL.MALL.BLL.Shop.Supplier.SupplierInfo supplierManage = new BLL.Shop.Supplier.SupplierInfo();
            drpSupplier.DataSource = supplierManage.GetModelList("");
            DataSet dsSupplier = supplierManage.GetAllList();
            if (!DataSetTools.DataSetIsNull(dsSupplier))
            {
                this.drpSupplier.DataSource = dsSupplier;
                this.drpSupplier.DataTextField = "Name";
                this.drpSupplier.DataValueField = "SupplierId";
                this.drpSupplier.DataBind();
            }
            this.drpSupplier.Items.Insert(0, new ListItem("无", "0"));
            this.drpSupplier.SelectedIndex = 0;
            #endregion

            BLL.Shop.Products.ProductInfo manage = new BLL.Shop.Products.ProductInfo();
            Model.Shop.Products.ProductInfo info = manage.GetModel(ProductId);
            if (info == null) return;

            BLL.Shop.Products.ProductCategories productCate = new BLL.Shop.Products.ProductCategories();

            this.hfCategoryId.Value = info.CategoryId.ToString();

            DataSet ds = productCate.GetListByProductId(info.ProductId);
            StringBuilder str = new StringBuilder();
            StringBuilder strProductName = new StringBuilder();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                str.Append(ds.Tables[0].Rows[i]["CategoryId"] + "_" + ds.Tables[0].Rows[i]["CategoryPath"]);
                str.Append(",");
                Model.Shop.Products.CategoryInfo cateModel =
                    categoryManage.GetModel(Globals.SafeInt(ds.Tables[0].Rows[i]["CategoryId"].ToString(), 0));
                if (cateModel != null)
                {
                    strProductName.Append(categoryManage.GetFullNameByCache(cateModel.CategoryId));
                    strProductName.Append("  ");
                }
            }
            this.Hidden_SelectValue.Value = str.ToString().TrimEnd(',');
            this.Hidden_SelectName.Value = this.LitPName.Text = strProductName.ToString();
            this.rptSelectCategory.DataSource = productCate.GetListByProductId(info.ProductId);
            this.rptSelectCategory.DataBind();
            if (info.RegionId.HasValue)
            {
                this.ajaxRegion.Area_iID = info.RegionId.Value;
                this.ajaxRegion.SelectedValue = info.RegionId.Value.ToString();
            }
            this.txtSeoImageAlt.Text = info.SeoImageAlt;
            this.txtSeoImageTitle.Text = info.SeoImageTitle;
            this.txtMeta_Title.Text = info.Meta_Title;
            this.txtMeta_Keywords.Text = info.Meta_Keywords;
            this.txtMeta_Description.Text = info.Meta_Description;
            this.txtUrlRule.Text = info.SeoUrl;
            this.txtSalePrice.Text = info.LowestSalePrice.ToString("F");
            this.txtPoints.Text = info.Points.HasValue ? info.Points.Value.ToString("F0") : "0";
            rblUpselling.SelectedValue = info.SaleStatus.ToString();
            rblSalesType.SelectedValue= info.SalesType.ToString();

            //--zhou 20170916新增修改扣商城积分

            txtGwjf.Text = info.Gwjf.HasValue ? info.Gwjf.Value.ToString("F0") : "0";

            //--zhou 20170916新增修改扣商城积分

            if (info.SalesType==2)
            {
                deliveryTip.Visible = true;
            }
            if (info.SupplierId > 0 && drpSupplier.Items.FindByValue(info.SupplierId.ToString()) != null)
            {
                this.drpSupplier.SelectedValue = info.SupplierId.ToString();
            }
            txtRestrictionCount.Text = info.RestrictionCount.ToString();
            txtDeliveryTip.Text = info.DeliveryTip;

        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            SaveProductInfo();
        }

        private void SaveProductInfo()
        {
            #region Get PageData

            //分类
            if (string.IsNullOrWhiteSpace(this.Hidden_SelectValue.Value))
            {
                MessageBox.ShowFailTip(this, "请选择产品分类！");
                return;
            }

            //基本信息
            string productName = txtProductName.Text;
            int selectedProductType = Globals.SafeInt(hfCurrentProductType.Value, -1);
            int selectedProductBrand = Globals.SafeInt(hfCurrentProductBrand.Value, -1);
            int supplierId = Globals.SafeInt(drpSupplier.SelectedValue, -1);
            int? selectedRegionId = Globals.SafeInt(ajaxRegion.SelectedValue, -1);
            string unit = txtUnit.Text;
            decimal? marketPrice = Globals.SafeDecimal(txtMarketPrice.Text, -1);
            int displaySequence = Globals.SafeInt(txtDisplaySequence.Text, -1);
            int restrictionCount = Globals.SafeInt(txtRestrictionCount.Text, 0);
            int salestype = Common.Globals.SafeInt(rblSalesType.SelectedValue, 1);
            if (!BLL.SysManage.ConfigSystem.GetBoolValueByCache("StoreIsInActivity"))
            {
                if (salestype == 3 && supplierId > 0)
                {
                    MessageBox.Show(this, "商家商品不支持赠送，请重新设置！");
                    return;
                }
            }
            string deliveryTip = txtDeliveryTip.Text;
            //描述
            string shortDescription = txtShortDescription.Text;
            string description = txtDescription.Text;

            //SEO
            string urlRule = txtUrlRule.Text;
            string metaTitle = txtMeta_Title.Text;
            string metaDescription = txtMeta_Description.Text;
            string metaKeywords = txtMeta_Keywords.Text;
            string seoImageAlt = txtSeoImageAlt.Text;
            string seoImageTitle = txtSeoImageTitle.Text;

            //图片
            string splitProductImages = hfProductImages.Value;

            //属性
            string attributeInfoJson = hfCurrentAttributes.Value;

            //SKU
            string skuBaseJson = hfCurrentBaseProductSKUs.Value;
            string skuJson = hfCurrentProductSKUs.Value;
            bool hasSKU = false;

            //配件
           // string productAccessorieJson = hfSelectedAccessories.Value;

            //相关商品
            //string relatedProductJson = this.hfRelatedProducts.Value;
            string relatedProductJson = this.HiddenField_RelatedProductInfo.Value;
            #endregion Get PageData

            #region Data Proc

            //CategoryId

            string[] productImages = new string[0];

            //简介信息去除换行符号处理
            if (!string.IsNullOrWhiteSpace(shortDescription))
            {
                shortDescription = Globals.HtmlEncodeForSpaceWrap(shortDescription);
            }
            if (!string.IsNullOrWhiteSpace(splitProductImages))
            {
                productImages = splitProductImages.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            }

            #region Attribute

            if (string.IsNullOrWhiteSpace(attributeInfoJson))
            {
                MessageBox.Show(this, "属性信息不存在,请检查已填数据是否正确!");
                return;
            }
            List<Model.Shop.Products.AttributeInfo> attributeList = GetAttributeInfo4Json(LitJson.JsonMapper.ToObject(attributeInfoJson));

            #endregion Attribute

            #region SKU

            //SKU
            if (string.IsNullOrWhiteSpace(skuBaseJson))
            {
                MessageBox.Show(this, "基础SKU信息不存在,请检查已填数据是否正确!");
                return;
            }
            List<Model.Shop.Products.SKUInfo> skuList = null;

            decimal? lowestSalePrice = 0M;

            //是否启用SKU
            hasSKU = !string.IsNullOrWhiteSpace(skuJson);
            if (hasSKU)
            {
                //已启用SKU
                skuList = GetSKUInfo4Json(LitJson.JsonMapper.ToObject(skuJson));
                if (salePriceList.Count > 0)
                {
                    salePriceList.Sort();
                    lowestSalePrice = Convert.ToDecimal(salePriceList[0]);
                }
            }
            else
            {
                //未启用SKU
                skuList = GetSKUInfo4Json(LitJson.JsonMapper.ToObject(skuBaseJson));
                lowestSalePrice = Globals.SafeDecimal(txtSalePrice.Text, -1);
            }

            #endregion SKU

            #endregion Data Proc

            #region Set ProductInfo

            BLL.Shop.Products.ProductInfo manage = new BLL.Shop.Products.ProductInfo();
            Model.Shop.Products.ProductInfo productInfo = manage.GetModel(ProductId);// new Model.Shop.Products.ProductInfo();
            int oldSaleStatus = productInfo.SaleStatus;
            //产品分类
            string[] productCategoriesArray = Hidden_SelectValue.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            productInfo.Product_Categories = productCategoriesArray;

            //基本信息
            productInfo.ProductName = productName;
            productInfo.CategoryId = 0;
            productInfo.TypeId = selectedProductType;
            productInfo.BrandId = selectedProductBrand;
            //商家
            if (supplierId > 0)
            {
                productInfo.SupplierId = supplierId;
            }
            else
            {
                productInfo.SupplierId = -1;
            }
            //修改了原来为赠品的商品
            if ( productInfo.SalesType==3 &&  salestype!=3 ) {
                YSWL.MALL.BLL.Shop.Activity.ActivityInfo bll = new YSWL.MALL.BLL.Shop.Activity.ActivityInfo();
                if (bll.Exists(ProductId)) {
                    MessageBox.Show(this, "该赠送已新增到【促销活动】,不能随意修改【销售类型】！");
                    return;
                }
            }
            productInfo.RegionId = selectedRegionId == -1 ? null : selectedRegionId;
            productInfo.Unit = unit;
            productInfo.MarketPrice = marketPrice == -1 ? null : marketPrice;
            productInfo.LowestSalePrice = lowestSalePrice.Value;
            productInfo.DisplaySequence = displaySequence;
            productInfo.RestrictionCount = restrictionCount;
            productInfo.DeliveryTip = deliveryTip;
            productInfo.ProductCode = this.txtProductSKU.Text;

            //productInfo.MainCategoryPath = categoryInfo.Path;
            productInfo.AddedDate = DateTime.Now;
            productInfo.SaleStatus = Globals.SafeInt(rblUpselling.SelectedValue, 0);

            //描述
            productInfo.ShortDescription = shortDescription;
            productInfo.Description = description;

            //SEO
            productInfo.SeoUrl = urlRule;
            productInfo.Meta_Title = metaTitle;
            productInfo.Meta_Description = metaDescription;
            productInfo.Meta_Keywords = metaKeywords;
            productInfo.SeoImageAlt = seoImageAlt;
            productInfo.SeoImageTitle = seoImageTitle;

            productInfo.Points = Globals.SafeDecimal(this.txtPoints.Text, 0);

            productInfo.Gwjf = Globals.SafeDecimal(this.txtGwjf.Text, 0);
            //Tags
            productInfo.Tags = "";
            
            if (productImages.Length == 0)
            {
                productInfo.ImageUrl =
                    productInfo.ThumbnailUrl1 = productInfo.ThumbnailUrl2 = "/Content/themes/base/Shop/images/none.png";
            }

            //待上传的图片名称
            //string ImageFile = string.Format("/Upload/Shop/Images/Product/{0}", DateTime.Now.ToString("yyyyMMdd"));

            string savePath = string.Format("/Upload/Shop/Images/Product/{0}/", DateTime.Now.ToString("yyyyMMdd"));
            string saveThumbsPath = "/Upload/Shop/Images/ProductThumbs/" + DateTime.Now.ToString("yyyyMMdd") + "/";
            ArrayList imageList = new ArrayList();
            for (int i = 0; i < productImages.Length; i++)
            {
                if (i == 0)
                {
                    //主图片
                    if (productImages[i].Contains(tempFile))
                    {
                        string imageUrl = string.Format(productImages[i], "");
                        string MainThumbnailUrl1 = productImages[i];
                        // string MainThumbnailUrl2 = string.Format(productImages[i], "N_");
                        productInfo.ImageUrl = imageUrl.Replace(tempFile, savePath);
                        productInfo.ThumbnailUrl1 = MainThumbnailUrl1.Replace(tempFile, saveThumbsPath);
                        //  productInfo.ThumbnailUrl2 = MainThumbnailUrl2.Replace(tempFile, ImageFile);
                        imageList.Add(imageUrl.Replace(tempFile, ""));
                        imageList.Add(MainThumbnailUrl1.Replace(tempFile, ""));
                        //  imageList.Add(MainThumbnailUrl2.Replace(tempFile, ""));
                        YSWL.MALL.BLL.Shop.Products.ProductImage.MoveImage(productImages[i], savePath, saveThumbsPath);
                    }
                }
                else
                {
                    if (productImages[i].Contains(tempFile))//新文件
                    {
                        string AttachImageUrl = string.Format(productImages[i], "");
                        string AttachThumbnailUrl1 = productImages[i];
                        // string AttachThumbnailUrl2 = string.Format(productImages[i], "N_");

                        productInfo.ProductImages.Add(
                            new ProductImage
                                {
                                    ImageUrl = AttachImageUrl.Replace(tempFile, savePath),
                                    ThumbnailUrl1 = AttachThumbnailUrl1.Replace(tempFile, saveThumbsPath),
                                    //ThumbnailUrl2 =AttachThumbnailUrl2. Replace(tempFile, ImageFile)
                                }
                            );

                        imageList.Add(AttachImageUrl.Replace(tempFile, savePath));
                        imageList.Add(AttachThumbnailUrl1.Replace(tempFile, ""));
                        //imageList.Add(AttachThumbnailUrl2.Replace(tempFile, ""));
                        //}
                        YSWL.MALL.BLL.Shop.Products.ProductImage.MoveImage(productImages[i], savePath, saveThumbsPath);
                    }
                    else//旧文件
                    {
                        string tempFileSwap="/Upload/Shop/Images/ProductThumbs/";
                        string savePathSwap= "/Upload/Shop/Images/Product/";
                        string AttachImageUrl = string.Format(productImages[i], "");
                        string AttachThumbnailUrl1 = productImages[i];
                        productInfo.ProductImages.Add(
                          new ProductImage
                          {
                              ImageUrl = AttachImageUrl.Replace(tempFileSwap, savePathSwap),
                              ThumbnailUrl1 = AttachThumbnailUrl1.Replace(tempFile, saveThumbsPath),
                              //ThumbnailUrl2 =AttachThumbnailUrl2. Replace(tempFile, ImageFile)
                          }
                          );
                    }
                }
            }

            //属性
            productInfo.AttributeInfos = attributeList;
            
            //SKU
            productInfo.HasSKU = hasSKU;
            productInfo.SkuInfos = skuList;

            //配件
            //string[] strSkuIds = productAccessorieJson.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //List<ProductAccessorie> acList = new List<ProductAccessorie>();
            //foreach (string item in strSkuIds)
            //{
            //    ProductAccessorie Acmodel = new ProductAccessorie();
            //    Acmodel.Name = this.txtAccessorieName.Text;
            //    Acmodel.DiscountAmount = Globals.SafeDecimal(this.txtDiscountAmount.Text, 0);
            //    Acmodel.DiscountType = 1; //this.rdoDiscountType4D.Checked ? 0 : 1;
            //    Acmodel.MaxQuantity = Globals.SafeInt(this.txtMaxQuantity.Text, 1);
            //    Acmodel.MinQuantity = Globals.SafeInt(this.txtMinQuantity.Text, 1);
            //    Acmodel.SkuId = item;
            //    acList.Add(Acmodel);
            //}
            //productInfo.ProductAccessories = acList;// productAccessorieJson;

            //相关商品
            string[] strRelatedProductIds = relatedProductJson.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            productInfo.RelatedProductId = strRelatedProductIds;

            productInfo.ProductId = ProductId;
            productInfo.SalesType = salestype;
            #endregion Set ProductInfo
            if (ProductManage.ModifyProduct(productInfo, oldSaleStatus))
            {
                if (productInfo.SupplierId > 0)
                {
                    DataCache.DeleteCache("SuppliersModel-" + productInfo.SupplierId);
                }
                prodBll.ClearCache(ProductId); 
                //将图片从临时文件夹移动到正式的文件夹下
                try
                {
                    if (imageList.Count > 0)
                    {
                        //  FileManage.MoveFile(Server.MapPath(tempFile), Server.MapPath(ImageFile), imageList);
                    }
                    if (skuImageList.Count > 0)
                    {
                        FileManage.MoveFile(Server.MapPath(tempFile), Server.MapPath(skuImageFile), skuImageList);
                    }


                    #region 清理seo关联链接缓存
                    string CacheKey = string.Format("SEORelationList-{0}-{1}", "ShopDescription", ProductId);
                    DataCache.DeleteCache(CacheKey);
                    #endregion

                    #region 生成二维码

                    string area = BLL.SysManage.ConfigSystem.GetValueByCache("MainArea");
                    string basepath = "/";
                    if (area.ToLower() != AreaRoute.MShop.ToString().ToLower())
                    {
                        basepath = "/MShop/";
                    }
                    string _uploadFolder = string.Format("/{0}/Shop/QR/Product/", MvcApplication.UploadFolder);
                    string filename = string.Format("{0}.png", ProductId);
                    string mapPath = Request.MapPath(_uploadFolder);
                    string mapPathQRImgUrl = mapPath + filename;

                    string baseURL = string.Format("/tools/qr/gen.aspx?margin={0}&size={1}&level={2}&format={3}&content={4}", 0, 180, "30%", "png", "{0}");
                    string websiteUrl = "http://" + Globals.DomainFullName + basepath + "p/d/" + ProductId;
                    websiteUrl = "http://" + Globals.DomainFullName + string.Format(baseURL, Common.Globals.UrlEncode(websiteUrl));
                    if (!Directory.Exists(mapPath))
                    {
                        Directory.CreateDirectory(mapPath);
                    }
                    using (var webClient = new System.Net.WebClient())
                    {
                        try
                        {
                            webClient.DownloadFile(websiteUrl, mapPathQRImgUrl);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                    #endregion
              
                }
                catch (Exception)
                {
                }
                Response.Redirect(string.Format("ProductsInStock.aspx?SaleStatus={0}", productInfo.SaleStatus));
            }
            else
            {
                MessageBox.Show(this, "保存失败! 请重试.");
                return;
            }
        }

        #region Json处理

        #region Attribute

        private List<Model.Shop.Products.AttributeInfo> GetAttributeInfo4Json(LitJson.JsonData jsonData)
        {
            List<Model.Shop.Products.AttributeInfo> list = new List<Model.Shop.Products.AttributeInfo>();
            if (!jsonData.IsArray || jsonData.Count < 1) return list;

            foreach (LitJson.JsonData item in jsonData)
            {
                list.Add(GetAttributeInfo4Obj(item));
            }
            return list;
        }

        private Model.Shop.Products.AttributeInfo GetAttributeInfo4Obj(LitJson.JsonData jsonData)
        {
            Model.Shop.Products.AttributeInfo attributeInfo = null;
            if (!jsonData.IsObject) return null;

            attributeInfo = new Model.Shop.Products.AttributeInfo();

            //Base Info
            attributeInfo.AttributeId = Globals.SafeInt(jsonData["AttributeId"].ToString(), -1);
            string attributeModeStr = jsonData["AttributeMode"].ToString();
            ProductAttributeModel attributeModel =
                (ProductAttributeModel)Enum.Parse(typeof(ProductAttributeModel), attributeModeStr);
            attributeInfo.UsageMode = (int)attributeModel;
            switch (attributeModel)
            {
                //单选
                case ProductAttributeModel.One:
                    attributeInfo.AttributeValues.Add(new AttributeValue
                                                          {
                                                              AttributeId = attributeInfo.AttributeId,
                                                              ValueId =
                                                                  Globals.SafeInt(jsonData["ValueItem"].ToString(), -1)
                                                          });
                    break;

                //多选
                case ProductAttributeModel.Any:
                    foreach (LitJson.JsonData item in jsonData["ValueItem"])
                    {
                        attributeInfo.AttributeValues.Add(new AttributeValue
                        {
                            AttributeId = attributeInfo.AttributeId,
                            ValueId = Globals.SafeInt(item.ToString(), -1)
                        });
                    }
                    break;

                //自定义
                case ProductAttributeModel.Input:
                    attributeInfo.AttributeValues.Add(new AttributeValue
                                                          {
                                                              AttributeId = attributeInfo.AttributeId,
                                                              ValueStr = jsonData["ValueItem"].ToString()
                                                          });
                    break;
                default:
                    break;
            }
            return attributeInfo;
        }

        #endregion Attribute

        #region SKU

        private List<Model.Shop.Products.SKUInfo> GetSKUInfo4Json(LitJson.JsonData jsonData)
        {
            List<Model.Shop.Products.SKUInfo> list = new List<Model.Shop.Products.SKUInfo>();
            if (jsonData.IsArray && jsonData.Count > 0)
            {
                //开启SKU时
                foreach (LitJson.JsonData item in jsonData)
                {
                    list.Add(GetSKUInfo4Obj(item));
                }
            }
            else if (jsonData.IsObject)
            {
                //关闭SKU时
                list.Add(GetSKUInfo4Obj(jsonData));
            }
            return list;
        }

        private Model.Shop.Products.SKUInfo GetSKUInfo4Obj(LitJson.JsonData jsonData)
        {
            Model.Shop.Products.SKUInfo skuInfo = null;
            if (!jsonData.IsObject) return null;

            skuInfo = new Model.Shop.Products.SKUInfo();

            //Base Info
            skuInfo.SKU = jsonData["SKU"].ToString();

            //CostPrice 允许为空
            string tmpCostPrice = jsonData["CostPrice"].ToString();
            if (!string.IsNullOrWhiteSpace(tmpCostPrice))
            {
                skuInfo.CostPrice = Globals.SafeDecimal(tmpCostPrice, decimal.MinusOne);
            }
            skuInfo.SalePrice = Globals.SafeDecimal(jsonData["SalePrice"].ToString(), decimal.MinusOne);
            salePriceList.Add(skuInfo.SalePrice);
            skuInfo.Stock = Globals.SafeInt(jsonData["Stock"].ToString(), -1);
            skuInfo.AlertStock = Globals.SafeInt(jsonData["AlertStock"].ToString(), -1);
            skuInfo.Weight = Globals.SafeInt(jsonData["Weight"].ToString(), 0);
            skuInfo.Upselling = Globals.SafeBool(jsonData["Upselling"].ToString(), false);

            //SKU Info
            if (jsonData["SKUItems"].IsArray && jsonData["SKUItems"].Count > 0)
            {
                foreach (LitJson.JsonData item in jsonData["SKUItems"])
                {
                    string skuImagepath = string.Empty;
                    if (!string.IsNullOrWhiteSpace(item["ImageUrl"].ToString()))
                    {

                        skuImagepath = item["ImageUrl"].ToString().Replace(tempFile, skuImageFile);
                        if (item["ImageUrl"].ToString().Contains(tempFile))
                        {
                            string BaseImage = item["ImageUrl"].ToString().Replace(tempFile, "");
                            if (!skuImageList.Contains(String.Format(BaseImage, "T32X32_")))
                            {
                                skuImageList.Add(String.Format(BaseImage, "T32X32_"));
                                skuImageList.Add(String.Format(BaseImage, "T130X130_"));
                                skuImageList.Add(String.Format(BaseImage, "T300X390_"));
                                skuImageList.Add(String.Format(BaseImage, "T350X350_"));
                                skuImageList.Add(String.Format(BaseImage, "T240X300_"));
                            }
                        }
                    }
                    skuInfo.SkuItems.Add(
                            new Model.Shop.Products.SKUItem
                            {
                                AttributeId = Globals.SafeLong(item["AttributeId"].ToString(), -1),
                                ValueId = Globals.SafeLong(item["ValueId"].ToString(), -1),
                                ImageUrl = skuImagepath,
                                ValueStr = item["ValueStr"].ToString()
                            }
                        );
                }
            }
            return skuInfo;
        }

        #endregion SKU

        #endregion Json处理

        private int _categoryId;

        public int CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
        }

        protected void rptSelectCategory_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal litCateName_1 = (Literal)e.Item.FindControl("litCateName_1");
                Literal litCateName_2 = (Literal)e.Item.FindControl("litCateName_2");
                object objId = DataBinder.Eval(e.Item.DataItem, "CategoryId");
                if (objId != null)
                {
                    if (litCateName_1 != null)
                    {
                        litCateName_1.Text = categoryManage.GetFullNameByCache(Globals.SafeInt(objId.ToString(), 0));
                    }
                    if (litCateName_2 != null)
                    {
                        litCateName_2.Text = categoryManage.GetFullNameByCache(Globals.SafeInt(objId.ToString(), 0));
                    }
                }
            }
        }


        protected void rblSalesType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblSalesType.SelectedValue == "2")
            {
                deliveryTip.Visible = true;
            }
            else
            {
                deliveryTip.Visible = false;
            }
        }
    }
}