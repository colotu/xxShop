using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using YSWL.MALL.BLL.Shop.Sales;
using YSWL.Common;
using YSWL.Components.Setting;
using YSWL.Json;
using YSWL.MALL.Model.Shop.Products;
using YSWL.MALL.ViewModel.Shop;
using YSWL.MALL.Web.Components.Setting.Shop;
using Webdiyer.WebControls.Mvc;

namespace YSWL.MALL.Web.Areas.MShop.Controllers
{
    public partial class ProductController : YSWL.MALL.Web.Areas.Shop.Controllers.ProductController
    {
        //
        // GET: /Mobile/Product/

        #region  商品列表
        public override ActionResult Index(int cid = 0, int brandid = 0, string attrvalues = "0", string mod = "hot", string price = "",
                                  int? pageIndex = 1, int pageSize = 10,
                                  string viewName = "Index", string ajaxViewName = "_ProductList")
        {
            ViewBag.IsOpenSku = BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_AddProduct_OpenSku");
            ProductListModel model = new ProductListModel();
            //model.CategoryList = categoryManage.MainCategoryList(null);
            YSWL.MALL.Model.Shop.Products.CategoryInfo categoryInfo = null;
            ViewBag.ParentId = 0;
            if (cid > 0)
            {
                List<YSWL.MALL.Model.Shop.Products.CategoryInfo> cateList = YSWL.MALL.BLL.Shop.Products.CategoryInfo.GetAvailableCateList();
                categoryInfo = cateList.FirstOrDefault(c => c.CategoryId == cid);
                if (categoryInfo != null)
                {
                    var path_arr = categoryInfo.Path.Split('|');
                    List<YSWL.MALL.Model.Shop.Products.CategoryInfo> categorysPath =
                        cateList.Where(c => path_arr.Contains(c.CategoryId.ToString())).OrderBy(c => c.Depth)
                                .ToList();
                    model.CategoryPathList = categorysPath;
                    model.CurrentCateName = categoryInfo.Name;
                    if (categoryInfo.ParentCategoryId != 0)
                    {
                        ViewBag.parentCateName = categoryManage.GetFullNameByCache(categoryInfo.ParentCategoryId);
                    }
                    ViewBag.ParentId = categoryInfo.ParentCategoryId;
                }


            }
            if (brandid > 0)//有品牌过来
            {
                Model.Shop.Products.BrandInfo brandInfo = brandBll.GetModelByCache(brandid);
                ViewBag.BrandName = brandInfo.BrandName;
            }
            model.CurrentCid = cid;
            model.CurrentMod = mod;
            // model.CurrentCateName = cname == "all" ? "全部" : cname;

            #region RouteDataParam
            string dataParam = "{";
            foreach (KeyValuePair<string, object> item in Request.RequestContext.RouteData.Values)
            {
                dataParam += item.Key + ":'" + item.Value + "',";
            }
            dataParam = dataParam.TrimEnd(',') + "}";
            ViewBag.DataParam = dataParam;
            #endregion

            // int pageSize =15;
            //重置页面索引
            pageIndex = pageIndex.HasValue && pageIndex.Value > 1 ? pageIndex.Value : 1;
            //计算分页起始索引
            int startIndex = pageIndex.Value > 1 ? (pageIndex.Value - 1) * pageSize + 1 : 1;
            //计算分页结束索引
            int endIndex = pageIndex.Value > 1 ? startIndex + pageSize - 1 : pageSize;
            int toalCount = productManage.GetProductsCountEx(cid, brandid, attrvalues, price);
            ViewBag.PageSize = pageSize;
            ViewBag.totalCount = toalCount.ToString();
            //瀑布流Index
            ViewBag.CurrentPageAjaxStartIndex = endIndex;
            ViewBag.CurrentPageAjaxEndIndex = pageIndex * pageSize;

            List<Model.Shop.Products.ProductInfo> list;
            list = productManage.GetProductsListEx(cid, brandid, attrvalues, price, mod, startIndex, endIndex);
        
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetCategorySetting(categoryInfo);
            //pageSetting.Replace(
            //    new[] { PageSetting.RKEY_CNAME, model.CurrentCateName }, 
            //    new[] { PageSetting.RKEY_CID, model.CurrentCid.ToString() }); //分类名称
            ViewBag.Title = pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion 

            //获取总条数
            if (toalCount < 1 || list == null || list.Count == 0)
            { 
                //检测Ajax请求, 进行无刷新分页
                if (Request.IsAjaxRequest())
                    return PartialView(ajaxViewName, model);
                return View(viewName, model); //NO DATA
            }

            YSWL.MALL.BLL.Shop.Activity.ActivityInfo infoBll = new BLL.Shop.Activity.ActivityInfo();
            #region 处理促销信息
            foreach (var item in list)
            {
                item.RuleIds = infoBll.GetRuleIds(item.ProductId, item.SupplierId);
            }
            #endregion 

            //分页获取数据
            model.ProductPagedList = list.ToPagedList(
                pageIndex ?? 1,
                pageSize
                );

            //检测Ajax请求, 进行无刷新分页
            if (Request.IsAjaxRequest())
                return PartialView(ajaxViewName, model);

            return View(viewName, model);
        }
        #endregion

        #region  商品二级页面显示列表
        public ActionResult ProCatTwo(int cid = 0, int brandid = 0, string attrvalues = "0", string mod = "hot", string price = "", int? pageIndex = 1, int pageSize = 10, string viewName = "ProCatTwo", string ajaxViewName = "_ProductList")
        {
            ViewBag.IsOpenSku = BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_AddProduct_OpenSku");
            ProductListModel model = new ProductListModel();
            //model.CategoryList = categoryManage.MainCategoryList(null);
            YSWL.MALL.Model.Shop.Products.CategoryInfo categoryInfo = null;
            ViewBag.ParentId = 0;
            if (cid > 0)
            {
                List<YSWL.MALL.Model.Shop.Products.CategoryInfo> cateList = YSWL.MALL.BLL.Shop.Products.CategoryInfo.GetAvailableCateList();
                categoryInfo = cateList.FirstOrDefault(c => c.CategoryId == cid);
                if (categoryInfo != null)
                {
                    var path_arr = categoryInfo.Path.Split('|');
                    List<YSWL.MALL.Model.Shop.Products.CategoryInfo> categorysPath =
                        cateList.Where(c => path_arr.Contains(c.CategoryId.ToString())).OrderBy(c => c.Depth)
                                .ToList();
                    model.CategoryPathList = categorysPath;
                    model.CurrentCateName = categoryInfo.Name;
                    if (categoryInfo.ParentCategoryId != 0)
                    {
                        ViewBag.parentCateName = categoryManage.GetFullNameByCache(categoryInfo.ParentCategoryId);
                    }
                    ViewBag.ParentId = categoryInfo.ParentCategoryId;
                }


            }
          
            model.CurrentCid = cid;
            // model.CurrentCateName = cname == "all" ? "全部" : cname;

            #region RouteDataParam
            string dataParam = "{";
            foreach (KeyValuePair<string, object> item in Request.RequestContext.RouteData.Values)
            {
                dataParam += item.Key + ":'" + item.Value + "',";
            }
            dataParam = dataParam.TrimEnd(',') + "}";
            ViewBag.DataParam = dataParam;
            #endregion

            // int pageSize =15;
            //重置页面索引
            pageIndex = pageIndex.HasValue && pageIndex.Value > 1 ? pageIndex.Value : 1;
            //计算分页起始索引
            int startIndex = pageIndex.Value > 1 ? (pageIndex.Value - 1) * pageSize + 1 : 1;
            //计算分页结束索引
            int endIndex = pageIndex.Value > 1 ? startIndex + pageSize - 1 : pageSize;
            int toalCount = productManage.GetProductsCountEx(cid, brandid, attrvalues, price);
            ViewBag.PageSize = pageSize;
            ViewBag.totalCount = toalCount.ToString();
            //瀑布流Index
            ViewBag.CurrentPageAjaxStartIndex = endIndex;
            ViewBag.CurrentPageAjaxEndIndex = pageIndex * pageSize;

            List<Model.Shop.Products.ProductInfo> list;
            list = productManage.GetProductsListEx(cid, brandid, attrvalues, price, mod, startIndex, endIndex);

            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetCategorySetting(categoryInfo);
            //pageSetting.Replace(
            //    new[] { PageSetting.RKEY_CNAME, model.CurrentCateName }, 
            //    new[] { PageSetting.RKEY_CID, model.CurrentCid.ToString() }); //分类名称
            ViewBag.Title = pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion 

            //获取总条数
            if (toalCount < 1 || list == null || list.Count == 0)
            {
                //检测Ajax请求, 进行无刷新分页
                if (Request.IsAjaxRequest())
                    return PartialView(ajaxViewName, model);
                return View(viewName, model); //NO DATA
            }

            YSWL.MALL.BLL.Shop.Activity.ActivityInfo infoBll = new BLL.Shop.Activity.ActivityInfo();
            #region 处理促销信息
            foreach (var item in list)
            {
                item.RuleIds = infoBll.GetRuleIds(item.ProductId, item.SupplierId);
            }
            #endregion 

            //分页获取数据
            model.ProductPagedList = list.ToPagedList(
                pageIndex ?? 1,
                pageSize
                );

            //检测Ajax请求, 进行无刷新分页
            if (Request.IsAjaxRequest())
                return PartialView(ajaxViewName, model);

            return View(viewName, model);
        }
        #endregion

        #region  商品分类
        public ActionResult CategoryList(int parentId = 0, string viewName = "CategoryList")
        {
            List<YSWL.MALL.Model.Shop.Products.CategoryInfo> cateList = YSWL.MALL.BLL.Shop.Products.CategoryInfo.GetAvailableCateList();
            List<YSWL.MALL.Model.Shop.Products.CategoryInfo> categoryInfos = cateList.Where(c => c.ParentCategoryId == parentId).OrderBy(c => c.DisplaySequence).ToList();
            YSWL.MALL.Model.Shop.Products.CategoryInfo parentInfo =
                cateList.FirstOrDefault(c => c.CategoryId == parentId);
            ViewBag.ParentId = -1;
            ViewBag.CurrentCategoryId = 0;
            if (parentInfo != null)
            {
                ViewBag.CurrentName = parentInfo.Name;
                ViewBag.CurrentCategoryId= parentInfo.CategoryId;
                ViewBag.ParentId = parentInfo.ParentCategoryId;
            }
            return View(viewName, categoryInfos);
        }
        #endregion

        #region Detail
        public override ActionResult Detail(int ProductId = -1, string viewName = "Detail")
        {
            //是否开启分仓
            ViewBag.IsMultiDepot = IsMultiDepot;

            YSWL.MALL.ViewModel.Shop.ProductModel model = new ProductModel();
            model.ProductInfo = productManage.GetModel(ProductId);
            BLL.Shop.Products.ProductImage imageManage = new BLL.Shop.Products.ProductImage();
            model.ProductImages = imageManage.ProductImagesList(ProductId);
            model.ProductSkus = skuBll.GetProductSkuInfo(ProductId);

            BLL.Shop.Products.BrandInfo brandManage = new BLL.Shop.Products.BrandInfo();
            if (model.ProductInfo == null)
            {
                return RedirectToAction("Index", "Home");
            }

            #region 商品是否参与了促销活动，如团购和限时抢购
            //判断是否参与了促销活动
            YSWL.MALL.Model.Shop.PromoteSales.CountDown countDownModel = countBll.GetActModel(ProductId);
            if (countDownModel != null)
            {
                return RedirectToAction("ProSaleDetail", "Product", new { id = countDownModel.CountDownId });
            }
            //是否参与了团购 （此部分比较麻烦，由于与地区相关，且无法定位用户的地区， 暂不跳转）
           
            #endregion
            //一键显示会员价格
            if (currentUser != null && currentUser.UserType != "AA" && model.ProductInfo.SupplierId <= 0)
            {
                model.ProductSkus[0].RankPrice= ruleProductBll.GetUserPrice(ProductId, model.ProductSkus[0].SalePrice, currentUser.UserID);
            }

            Model.Shop.Products.BrandInfo brandModel = brandManage.GetModelByCache(model.ProductInfo.BrandId);
            if (brandModel != null)
            {
                ViewBag.BrandName = brandModel.BrandName;
            }

            #region 分类导航

            BLL.Shop.Products.ProductCategories productCategoriesManage = new BLL.Shop.Products.ProductCategories();
            Model.Shop.Products.ProductCategories categoryModel = productCategoriesManage.GetModel(ProductId);
            if (categoryModel != null)
            {
                List<YSWL.MALL.Model.Shop.Products.CategoryInfo> AllCateList = YSWL.MALL.BLL.Shop.Products.CategoryInfo.GetAvailableCateList();
                var currentModel = AllCateList.FirstOrDefault(c => c.CategoryId == categoryModel.CategoryId);
                if (currentModel != null)
                {
                    var cateIds = currentModel.Path.Split('|');
                    List<Model.Shop.Products.CategoryInfo> list =
                        AllCateList.Where(c => cateIds.Contains(c.CategoryId.ToString())).OrderBy(c => c.Depth).ToList();
                    System.Text.StringBuilder sbPath = new System.Text.StringBuilder();
                    System.Text.StringBuilder CategoryStr = new System.Text.StringBuilder();
                    if (list != null && list.Count > 0)
                    {
                        foreach (var categoryInfo in list)
                        {
                            CategoryStr.AppendFormat("<a href='/Product/" + categoryInfo.CategoryId + "'>{0}</a> > ", categoryInfo.Name);
                        }
                    }
                    ViewBag.Cid = categoryModel.CategoryId;
                    ViewBag.PathInfo = sbPath.ToString();
                    ViewBag.CategoryStr = CategoryStr.ToString();
                }
            }



            #endregion 分类导航

            #region SEO设置
            PageSetting pageSetting = PageSetting.GetProductSetting(model.ProductInfo);
            ViewBag.Title = pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;

            #endregion SEO设置

            #region   推广信息

            ViewBag.Spread = false;
            string r = Request.Params["r"];
            if (!String.IsNullOrWhiteSpace(r))
            {
                int userId = Common.Globals.SafeInt(YSWL.Common.UrlOper.Base64Decrypt(r), 0);
                if (currentUser!=null&&userId == currentUser.UserID)
                {
                    ViewBag.Spread = true;
                }
            }
         
            #endregion 

            ViewBag.ConsultCount = conBll.GetRecordCount("Status=1 and ProductId=" + ProductId);
            ViewBag.CommentCount = reviewsBll.GetRecordCount("Status=1 and ProductId=" + ProductId);
            ViewBag.IsMultiDepot = IsMultiDepot;

            return View(viewName, model);
        }
        /// <summary>
        /// 商品详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ProductDesc(int id)
        {
            YSWL.MALL.Model.Shop.Products.ProductInfo productInfo = productManage.GetModel(id);
            if (productInfo == null)
            {
                return RedirectToAction("Index", "Home");
            }

            #region SEO设置
            PageSetting pageSetting = PageSetting.GetProductSetting(productInfo);
            ViewBag.Title = pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion SEO设置
            return View(productInfo);
        }

        #endregion

        #region 关联商品
        public override PartialViewResult ProductRelation(int id, int top = 12, string viewName = "_ProductRelation")
        {
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> productList = productManage.RelatedProductsList(id, top);
            return PartialView(viewName, productList);
        }
        #endregion

        #region 商品SKU规格选择

        public override ActionResult OptionSKU(long productId,int SuppId, string viewName = "_OptionSKU")
        {
            if (productId < 1) return new EmptyResult();
            ViewModel.Shop.ProductSKUModel productSKUModel = skuBll.GetProductSKUInfoByProductId(productId);
            //NO SKU ERROR
            if (productSKUModel == null) return new EmptyResult();
            //NO SKU ERROR
            if (productSKUModel.ListSKUInfos == null || productSKUModel.ListSKUInfos.Count < 1 ||
                productSKUModel.ListSKUItems == null) return new EmptyResult();

            ViewBag.HasSKU = true;

            //木有开启SKU的情况
            if (productSKUModel.ListSKUItems.Count == 0)
            {
                ViewBag.HasSKU = false;
                return View(viewName, productSKUModel);
            }

            ViewBag.SKUJson = SKUInfoToJson(productSKUModel.ListSKUInfos,SuppId).ToString();

            return View(viewName, productSKUModel);
        }

        #endregion

        #region 商品评论
        public ActionResult Comments(int id, int pageIndex = 1, int pageSize = 15, string viewName = "Comments")
        {
            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * pageSize + 1 : 1;
            ViewBag.ProductName = productManage.GetProductName(id);
            //计算分页结束索引
            int endIndex = pageIndex * pageSize;
            int totalCount = 0;
            ViewBag.PageSize = pageSize;
            #region DataParam
            ViewBag.DataParam = String.Format("{0}id:'{1}'{2}", "{", id, "}");
            #endregion

            //获取总条数
            totalCount = reviewsBll.GetRecordCount("Status=1 and ProductId=" + id);
            ViewBag.TotalCount = totalCount;
            if (totalCount == 0)
            {
                return View(viewName);//NO DATA
            }
            List<YSWL.MALL.Model.Shop.Products.ProductReviews> productReviewses = reviewsBll.GetReviewsByPage(id, " CreatedDate desc", startIndex, endIndex);

            PagedList<YSWL.MALL.Model.Shop.Products.ProductReviews> lists = new PagedList<YSWL.MALL.Model.Shop.Products.ProductReviews>(productReviewses, pageIndex, pageSize, totalCount);
            //检测Ajax请求, 进行无刷新分页
            if (Request.IsAjaxRequest())
                return View(viewName, lists);
            return View(viewName, lists);
        }
        #endregion

        #region 商品咨询
        public ActionResult Consults(int id, int pageIndex = 1, string viewName = "Consults")
        {

            int _pageSize = 4;
            ViewBag.ProductName = productManage.GetProductName(id);
            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * _pageSize + 1 : 1;

            //计算分页结束索引
            int endIndex = pageIndex * _pageSize;
            int totalCount = 0;

            //获取总条数
            totalCount = conBll.GetRecordCount("Status=1 and ProductId=" + id);
            ViewBag.TotalCount = totalCount;
            if (totalCount == 0)
            {
                return PartialView(viewName);//NO DATA
            }
            List<YSWL.MALL.Model.Shop.Products.ProductConsults> productConsults = conBll.GetConsultationsByPage(id, " CreatedDate desc", startIndex, endIndex);

            PagedList<YSWL.MALL.Model.Shop.Products.ProductConsults> lists = new PagedList<YSWL.MALL.Model.Shop.Products.ProductConsults>(productConsults, pageIndex, _pageSize, totalCount);
            //检测Ajax请求, 进行无刷新分页
            if (Request.IsAjaxRequest())
                return PartialView(viewName, lists);
            return PartialView(viewName, lists);
        }
        #endregion

        #region 条件筛选
        public ActionResult Filter(int id = 0)
        {
            #region SEO 优化设置
            ViewBag.Title = "商品筛选";
            #endregion
            return View();
        }
        #endregion
        
        #region 商品扩展属性
        public override ActionResult OptionAttr(long productId, string viewName = "_OptionAttr")
        {
            if (productId < 1) return new EmptyResult();
            List<YSWL.MALL.Model.Shop.Products.AttributeInfo> model = attributeManage.GetAttributeInfoListByProductId(productId);
            return View(viewName, model);
        }
        #endregion

        #region 得到推荐商品数据
        public ActionResult ProductList(int Cid = 0, YSWL.MALL.Model.Shop.Products.ProductRecType RecType = ProductRecType.IndexRec, int pageIndex = 1, int pageSize = 15, string viewName = "Index", string ajaxViewName = "_ProductList")
        {
            List<YSWL.MALL.Model.Shop.Products.CategoryInfo> cateList = YSWL.MALL.BLL.Shop.Products.CategoryInfo.GetAvailableCateList();
            ProductListModel model = new ProductListModel();
            YSWL.MALL.Model.Shop.Products.CategoryInfo categoryInfo = cateList.FirstOrDefault(c => c.CategoryId == Cid);
            if (categoryInfo != null)
            {
                ViewBag.CategoryName = categoryInfo.Name;
            }
            ViewBag.RecType = RecType;
            int toalCount = productManage.GetProductRecCount(RecType, Cid);
            ViewBag.totalCount = toalCount.ToString();

            int startIndex = pageIndex > 1 ? (pageIndex - 1) * pageSize : 1;
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> productList = productManage.GetProductRecList(RecType, Cid, -1);
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> list = productList.Skip(startIndex).Take(pageSize).ToList();
            //分页获取数据
            model.ProductPagedList = list.ToPagedList(
                pageIndex,
                pageSize);
            if (Request.IsAjaxRequest())
                return PartialView(ajaxViewName, model);

            return View(viewName, model);
        }
        #endregion

        #region  批发规则
        public override PartialViewResult WholeSale(int pId, int suppId, string viewName = "_WholeSale")
        { 
            //批发规则  只有自营商品使用 
            if (suppId > 0)
            {
                return null;
            }
            YSWL.MALL.ViewModel.Shop.SalesModel salesModel = new YSWL.MALL.ViewModel.Shop.SalesModel();
            if (currentUser != null)
            {
                YSWL.MALL.BLL.Shop.Sales.SalesRuleProduct ruleBll = new YSWL.MALL.BLL.Shop.Sales.SalesRuleProduct();
                salesModel = ruleBll.GetSalesRuleByCache(pId, currentUser.UserID);
            }      
            return PartialView(viewName, salesModel);
        }
        #endregion


        #region Ajax 获取库存
        /// <summary>
        /// 根据商品ID获取分仓仓库库存
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="regionId"></param>
        /// <param name="suppId"></param>
        /// <returns></returns>
        [HttpPost]
        public override ActionResult GetSKUInfos(long productId, int suppId = 0)
        {
            if (productId < 1) return new EmptyResult();
            ViewModel.Shop.ProductSKUModel productSKUModel =
                YSWL.MALL.BLL.Shop.Products.StockHelper.GetProductSKUInfo(productId, GetRegionId, suppId);
            //NO SKU ERROR
            if (productSKUModel == null) return new EmptyResult();
            //NO SKU ERROR
            if (productSKUModel.ListSKUInfos == null || productSKUModel.ListSKUInfos.Count < 1 ||
                productSKUModel.ListSKUItems == null) return new EmptyResult();

            ViewBag.HasSKU = true;

            //木有开启SKU的情况  Ajax 返回空js停止处理
            if (productSKUModel.ListSKUItems.Count == 0) return new EmptyResult();

            //DONE: 生成JsonSKU数据结构
            return Content(SKUInfoToJson(productSKUModel.ListSKUInfos, suppId).ToString());
        }
        /// <summary>
        /// 获取SKU库存
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="regionId"></param>
        /// <param name="suppId"></param>
        /// <returns></returns>
        [HttpPost]
        public override ActionResult GetSKUStock(string sku, int suppId = 0)
        {
            int stock = YSWL.MALL.BLL.Shop.Products.StockHelper.GetSkuStockByCache(sku, GetRegionId, suppId);
            return Content(stock.ToString());
        }
        #endregion 
    }
}
