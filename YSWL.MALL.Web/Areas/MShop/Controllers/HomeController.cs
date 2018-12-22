using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YSWL.Accounts.Bus;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.Components.Setting;
using YSWL.Common;
using YSWL.MALL.Model.Shop.Products;
using YSWL.MALL.Web.Components.Setting.Shop;
using YSWL.MALL.Model.SysManage;

namespace YSWL.MALL.Web.Areas.MShop.Controllers
{
    public class HomeController : MShopControllerBase
    {
        //
        // GET: /Mobile/Home/
        private YSWL.MALL.BLL.Shop.Products.ProductInfo productBll = new YSWL.MALL.BLL.Shop.Products.ProductInfo();
        private BLL.Shop.Products.BrandInfo brandInfoBll = new BLL.Shop.Products.BrandInfo();
        private BLL.CMS.ContentClass contentclassBll = new BLL.CMS.ContentClass();
        public ActionResult Index()
        {
            ViewBag.MShopLogo = BLL.SysManage.ConfigSystem.GetValueByCache("WeChat_MShop_Logo");
            ViewBag.Title=ViewBag.MShopName = BLL.SysManage.ConfigSystem.GetValueByCache("WeChat_MShop_Name");
            return View();
        }

        public ActionResult home()
        {
            ViewBag.MShopLogo = BLL.SysManage.ConfigSystem.GetValueByCache("WeChat_MShop_Logo");
            ViewBag.Title = ViewBag.MShopName = BLL.SysManage.ConfigSystem.GetValueByCache("WeChat_MShop_Name");
            return View();
        }

        #region 商品列表
        public PartialViewResult ProductList(int Cid, YSWL.MALL.Model.Shop.Products.ProductRecType RecType = ProductRecType.IndexRec, int Top = 10, string viewName = "_ProductList")
        {
            List<YSWL.MALL.Model.Shop.Products.CategoryInfo> cateList = YSWL.MALL.BLL.Shop.Products.CategoryInfo.GetAvailableCateList();
            YSWL.MALL.Model.Shop.Products.CategoryInfo categoryInfo = cateList.FirstOrDefault(c => c.CategoryId == Cid);
            if (categoryInfo != null)
            {
                ViewBag.CategoryName = categoryInfo.Name;
            }
            ViewBag.RecType = RecType;
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> productList = productBll.GetProductRecList(RecType, Cid, Top);
            #region 前台需要什么类型的商品
            switch (RecType)
            {
                case ProductRecType.Cheap:
                    ViewBag.productType = "特价商品";
                    break;
                case ProductRecType.Hot:
                    ViewBag.productType = "热卖商品";
                    break;
                case ProductRecType.Latest:
                    ViewBag.productType = "最新商品";
                    break;
                case ProductRecType.Recommend:
                    ViewBag.productType = "推荐商品";
                    break;
                default:
                    ViewBag.productType = "推荐商品";
                    break;
            } 
            #endregion
            return PartialView(viewName, productList);
        }
        #endregion

        public PartialViewResult CategoryList(int Cid = 0, int Top = 10, string ViewName = "_CategoryList")
        {
            List<YSWL.MALL.Model.Shop.Products.CategoryInfo> cateList = YSWL.MALL.BLL.Shop.Products.CategoryInfo.GetAvailableCateList();
            List<YSWL.MALL.Model.Shop.Products.CategoryInfo> categoryInfos = cateList.Where(c => c.ParentCategoryId == Cid).Take(Top).ToList();
            return PartialView(ViewName, categoryInfos);
        }

        public PartialViewResult NewsList(string viewName, int ClassID, int Top)
        {
            BLL.CMS.Content conBll = new BLL.CMS.Content();
            List<Model.CMS.Content> list = conBll.GetModelList(ClassID, Top);
            ViewBag.ContentClassName = contentclassBll.GetClassnameById(ClassID);
            return PartialView(viewName, list);
        }

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

        //网站信息
        public PartialViewResult OpertorsInfo(string viewName = "_OpertorsInfo")
        {
            ViewBag.Address = GetValueByCache("Opertors_Address");
            ViewBag.BusinessHoursEnd = GetValueByCache("Opertors_BusinessHoursEnd");
            ViewBag.BusinessHoursStart = GetValueByCache("Opertors_BusinessHoursStart");
            ViewBag.DeliveryArea = GetValueByCache("Opertors_DeliveryArea");
            ViewBag.SentPrices = GetValueByCache("Opertors_SentPrices");
            ViewBag.ServiceRadius = GetValueByCache("Opertors_ServiceRadius");
            ViewBag.Telephone = GetValueByCache("Opertors_Telephone");
            return PartialView(viewName);
        }
        public string GetValueByCache(string keyName)
        {
            return BLL.SysManage.ConfigSystem.GetValueByCache(keyName,ApplicationKeyType.Shop);
        }
        #region 限时抢购
        public PartialViewResult CountDown(int Top = 8, string viewName = "_CountDown")
        {
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> proSalesList = productBll.GetProSalesList(Top);
            return PartialView(viewName, proSalesList);
        }
        #endregion
    }
}
