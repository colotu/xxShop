using System.Collections.Generic;
using System.Web.Mvc;

namespace YSWL.MALL.Web.Areas.MShop.Controllers
{
    public partial class PartialController : MShopControllerBase
    {
        protected BLL.Ms.Regions regsBll = new BLL.Ms.Regions();
        protected YSWL.MALL.BLL.Shop.Sales.SalesRuleProduct ruleProductBll = new BLL.Shop.Sales.SalesRuleProduct();
        protected BLL.Shop.Products.ProductInfo productManage = new BLL.Shop.Products.ProductInfo();
        protected BLL.Shop.Products.SKUInfo skuBll = new BLL.Shop.Products.SKUInfo();
        private BLL.Members.SiteMessage bllSM = new BLL.Members.SiteMessage();
        #region 热门关键字
        public PartialViewResult HotKeyword(int Cid = 0, int Top = 6, string ViewName = "_HotKeyword")
        {
            YSWL.MALL.BLL.Shop.Products.HotKeyword keywordBll = new YSWL.MALL.BLL.Shop.Products.HotKeyword();
            List<YSWL.MALL.Model.Shop.Products.HotKeyword> keywords = keywordBll.GetKeywordsList(Cid, Top);
            ViewBag.Cid = Cid;
            return PartialView(ViewName, keywords);
        }
        #endregion

        #region 选择地区
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public PartialViewResult SelectRegion(string viewName = "_SelectRegion")
        {
            int regionId;
            if (IsMultiDepot)
            {
                //开启了分仓
                regionId = GetRegionId;
            }
            else
            {
                regionId = Common.Globals.SafeInt(Common.Cookies.getKeyCookie("groupbuy_regionId"), 0);
            }
            if (regionId <= 0)
            {
                //先清空值  避免出现  id与名称不对应的情况
                Common.Cookies.setKeyCookie("deliveryareas_lastRegionName", "", 1440);
                regionId = Common.Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("Shop_GroupBuy_DefaultRegion"), 651);//默认北京
                Common.Cookies.setKeyCookie("deliveryareas_regionname", regsBll.GetRegionFullName(regionId), 1440);
                YSWL.MALL.Model.Ms.Regions regionsModel = regsBll.GetModelByCache(regionId);
                if (regionsModel != null)
                {
                    Common.Cookies.setKeyCookie("deliveryareas_lastRegionName", regionsModel.RegionName, 1440);
                }
            }
            ViewBag.RegionId = regionId;
            return PartialView(viewName);
        }
        #endregion

        #region 购物车层
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public PartialViewResult AddCart(int productId = -1, string viewName = "_AddCart")
        {
            //是否开启分仓
            ViewBag.IsMultiDepot = IsMultiDepot;
            YSWL.MALL.ViewModel.Shop.ProductModel model = new YSWL.MALL.ViewModel.Shop.ProductModel();
            model.ProductInfo = productManage.GetModel(productId);
            if (model.ProductInfo == null)
            {
                return PartialView(viewName, model);
            }
            model.ProductSkus = skuBll.GetProductSkuInfo(productId);
            //一键显示会员价格
            if (currentUser != null && currentUser.UserType != "AA" && model.ProductInfo.SupplierId <= 0)
            {
                model.ProductSkus[0].RankPrice = ruleProductBll.GetUserPrice(productId, model.ProductSkus[0].SalePrice, currentUser.UserID);
            }
            return PartialView(viewName, model);
        }
        #endregion

        #region 获取未读消息数
        /// <summary>
        /// ajax获取异步消息数
        /// </summary>
        /// <returns></returns>
        public ActionResult GetNotRead()
        {
            if (CurrentUser == null) {
                return Content("0");
            }
            return Content(bllSM.GetNoReadCount(CurrentUser.UserID).ToString());
        }
        #endregion

    }
}
