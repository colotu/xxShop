using System.Collections.Generic;
using System.Web.Mvc;
using YSWL.Components.Setting;
using YSWL.MALL.Model.Shop.Products;
using YSWL.MALL.Web.Components.Setting.Shop;
using System;
using YSWL.Web;

namespace YSWL.MALL.Web.Areas.Shop.Controllers
{

    public class HomeController : ShopControllerBase
    {
        private YSWL.MALL.BLL.Shop.Products.ProductInfo productBll = new YSWL.MALL.BLL.Shop.Products.ProductInfo();
        private YSWL.MALL.BLL.Shop.Products.CategoryInfo categoryBll = new YSWL.MALL.BLL.Shop.Products.CategoryInfo();
        private YSWL.MALL.BLL.Shop.Products.BrandInfo brandInfoBll = new YSWL.MALL.BLL.Shop.Products.BrandInfo();
        private BLL.Shop.PromoteSales.GroupBuy groupBuy = new BLL.Shop.PromoteSales.GroupBuy();
        private BLL.Ms.Regions regionsBll = new BLL.Ms.Regions();
        public ActionResult Index()
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            return View();
        }

        #region 商品分类

        public PartialViewResult CategoryList(string viewName = "_CategoryList")
        {
            return PartialView(viewName);
        }

        #endregion

        #region 热销品牌
        public PartialViewResult HotBrands(int top = 10, int productTypeId = 2, string viewName = "_HotBrands")
        {
            List<YSWL.MALL.Model.Shop.Products.BrandInfo> brandInfos = null;
            if (productTypeId > 0)
            {
                brandInfos = brandInfoBll.GetModelListByProductTypeId(productTypeId, top);
            }
            else
            {
                brandInfos = brandInfoBll.GetBrandList("", top);
            }
            return PartialView(viewName, brandInfos);
        }
        #endregion

        #region 品牌库
        public PartialViewResult Brands(int top = 18, int productTypeId = 2, string viewName = "_HotBrands")
        {
            List<YSWL.MALL.Model.Shop.Products.BrandInfo> brandInfos = null;
            if (productTypeId > 0)
            {
                brandInfos = brandInfoBll.GetModelListByProductTypeId(productTypeId, top);
                YSWL.MALL.BLL.Shop.Products.ProductType productTypeBll = new YSWL.MALL.BLL.Shop.Products.ProductType();
                YSWL.MALL.Model.Shop.Products.ProductType productTypeModel = productTypeBll.GetModel(productTypeId);
                string typeName = null;
                if (null != productTypeModel)
                {
                    typeName = productTypeModel.TypeName;
                }
                else
                {
                    typeName = "暂无此品牌";
                }
                YSWL.MALL.Model.Shop.Products.BrandInfo brandType = new YSWL.MALL.Model.Shop.Products.BrandInfo();
                brandType.BrandName = typeName;
                //brandInfos.Add(brandType);
                brandInfos.Add(brandType);
            }
            else
            {
                brandInfos = brandInfoBll.GetBrandList("", top);
            }
            return PartialView(viewName, brandInfos);
        } 
        #endregion

        #region 品牌列表
        public ViewResult BrandsList()
        {
            return View();
        }
        #endregion

        #region 商品列表
        public PartialViewResult ProductList(int Cid, YSWL.MALL.Model.Shop.Products.ProductRecType RecType = ProductRecType.IndexRec, int Top = 10, string viewName = "_ProductList")
        {
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> productList = productBll.GetProductRecList(RecType, Cid, Top);
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> prolist = new List<Model.Shop.Products.ProductInfo>();
            if (productList != null)
            {
                #region 是否静态化
                string IsStatic = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("ShopProductStatic");
                string basepath = YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.Shop);
                foreach (var item in productList)
                {
                    //zhou20181212修改
                    item.LowestSalePrice = decimal.Parse((item.LowestSalePrice - item.Gwjf).ToString());

                    if (IsStatic == "1")
                    {
                        item.SeoUrl = PageSetting.GetProStaticUrl(Convert.ToInt32(item.ProductId.ToString())).Replace("//", "/");
                    }
                    else if (IsStatic == "2")
                    {
                        item.SeoUrl = basepath + "Product-" + item.ProductId + ".html";
                    }
                    else
                    {
                        item.SeoUrl = basepath + "Product/Detail/" + item.ProductId;
                    }
                    prolist.Add(item);
                }
                #endregion
            }
            return PartialView(viewName, prolist);
        }
        #endregion

        #region 新品商品
        public PartialViewResult ProductNewList(int Top = 7, string viewName = "_ProductNewList")
        {
            List<YSWL.MALL.Model.Shop.Products.CategoryInfo> categoryInfos = categoryBll.GetCategorysByParentId(0, Top);
            return PartialView(viewName, categoryInfos);
        }

        public PartialViewResult NewListPart(int Cid, int Top = 5, string viewName = "_NewListPart")
        {
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> productList = productBll.GetProductRecList(ProductRecType.Latest, Cid, Top);
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> prolist = new List<Model.Shop.Products.ProductInfo>();
            if (productList != null)
            {
                #region 是否静态化
                string IsStatic = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("ShopProductStatic");
                string basepath = YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.Shop);
                foreach (var item in productList)
                {
                    //zhou20181212修改
                    item.LowestSalePrice = decimal.Parse((item.LowestSalePrice - item.Gwjf).ToString());

                    if (IsStatic == "1")
                    {
                        item.SeoUrl = PageSetting.GetProStaticUrl(Convert.ToInt32(item.ProductId.ToString())).Replace("//", "/");
                    }
                    else if (IsStatic == "2")
                    {
                        item.SeoUrl = basepath + "Product-" + item.ProductId + ".html";
                    }
                    else
                    {
                        item.SeoUrl = basepath + "Product/Detail/" + item.ProductId;
                    }
                    prolist.Add(item);
                }
                #endregion
            }
            return PartialView(viewName, prolist);
        }
        #endregion


        #region 热门评论
        public PartialViewResult HotComments(int Top = 6, string viewName = "_HotComments")
        {
            YSWL.MALL.BLL.Shop.Products.ProductReviews reviewsBll = new YSWL.MALL.BLL.Shop.Products.ProductReviews();
            string str = "Status=1";//获取通过审核的评论
            List<YSWL.MALL.Model.Shop.Products.ProductReviews> list = reviewsBll.GetModelList(str);
            //List<YSWL.MALL.Model.Shop.Products.CategoryInfo> categoryInfos = categoryBll.GetCategorysByParentId(0, Top);
            return PartialView(viewName, list);
        }
        public PartialViewResult CommentPart(int Cid, int Top = 9, string viewName = "_CommentPart")
        {
            //YSWL.MALL.BLL.Shop.Products.ProductReviews reviewsBll = new YSWL.MALL.BLL.Shop.Products.ProductReviews();
            //reviewsBll.GetModelList()
            //List<YSWL.MALL.Model.Shop.Products.CategoryInfo> categoryInfos = categoryBll.GetCategorysByParentId(0, Top);
            return PartialView(viewName);
        }
        #endregion

        #region 手机商城
        public ActionResult App()
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            return View();
        }
        #endregion 


       

        #region 限时抢购
        public PartialViewResult CountDown(int Top = 8, string viewName = "_CountDown")
        {
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> proSalesList = productBll.GetProSalesList(Top);
            return PartialView(viewName,proSalesList);
        }
        #endregion

        #region 团购
        public virtual ActionResult GroupBuy(int regionid=0, int cid=0,int top=6,
               string viewName = "_GroupBuy")
        {
            //优先取传过来的值 //其次取分仓地区的值 //如果没有开启分仓 取前台用户自己设置的值 //再去团购默认地区的值
            if (regionid <= 0)
            {
                if (IsMultiDepot)
                {
                    //开启了分仓
                    regionid = GetRegionId;
                }
                else
                {
                    regionid = Common.Globals.SafeInt(Common.Cookies.getKeyCookie("groupbuy_regionId"), 0);
                }
                if (regionid <= 0)
                {
                    regionid = Common.Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("Shop_GroupBuy_DefaultRegion"), 651);//默认北京
                }
            }
            regionid = regionid <= 0 ? 643 : regionid;//防止从cache中未取到参数报错
            List<YSWL.MALL.Model.Shop.PromoteSales.GroupBuy> groupList = groupBuy.GetList(top, cid, regionid, "");
            if (groupList != null && groupList.Count > 0)
            {
                foreach (Model.Shop.PromoteSales.GroupBuy item in groupList)
                {
                    item.LowestSalePrice = productBll.GetLowestSalePrice(item.ProductId);
                }
            }
            return View(viewName, groupList);
        }
        #endregion

    }
}
