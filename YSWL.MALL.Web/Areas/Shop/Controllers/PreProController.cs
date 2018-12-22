using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YSWL.Components.Setting;
using Webdiyer.WebControls.Mvc;
using YSWL.MALL.BLL.Shop.PrePro;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.MALL.ViewModel.Shop;
using YSWL.MALL.Web.Components.Setting.Shop;

namespace YSWL.MALL.Web.Areas.Shop.Controllers
{
    public class PreProController : ShopControllerBase
    {
     
        private YSWL.MALL.BLL.Shop.PrePro.PreProduct preProBll = new PreProduct();
        private YSWL.MALL.BLL.Shop.Products.ProductInfo productManage = new YSWL.MALL.BLL.Shop.Products.ProductInfo();
        private YSWL.MALL.BLL.Shop.Products.SKUInfo skuBll=new SKUInfo();
        protected BLL.Shop.Products.ProductReviews reviewsBll = new YSWL.MALL.BLL.Shop.Products.ProductReviews();
        protected BLL.Shop.Products.ProductConsults conBll = new YSWL.MALL.BLL.Shop.Products.ProductConsults();
        #region 预订商品
        public  ActionResult Index(string mod = "default", int? pageIndex = 1, int pageSize = 16,
                               string viewName = "Index", string ajaxViewName = "_PreProductList")
        {
            //重置页面索引
            pageIndex = pageIndex.HasValue && pageIndex.Value > 1 ? pageIndex.Value : 1;
            //计算分页起始索引
            int startIndex = pageIndex.Value > 1 ? (pageIndex.Value - 1) * pageSize + 1 : 1;
            //计算分页结束索引
            int endIndex = pageIndex.Value > 1 ? startIndex + pageSize - 1 : pageSize;
            int totalCount = preProBll.GetTotalCount();
            //获取总条数
              if (totalCount ==0)
                  return View(viewName); //NO DATA
            List<YSWL.MALL.Model.Shop.PrePro.PreProduct> preProductList = preProBll.GetListByPage(0, mod, startIndex, endIndex);
            PagedList<YSWL.MALL.Model.Shop.PrePro.PreProduct> pagedList = new PagedList<YSWL.MALL.Model.Shop.PrePro.PreProduct>(preProductList, pageIndex.Value, pageSize, totalCount);

            #region RouteDataParam
            string dataParam = "{";
            foreach (KeyValuePair<string, object> item in Request.RequestContext.RouteData.Values)
            {
                dataParam += item.Key + ":'" + item.Value + "',";
            }
         
            dataParam = dataParam.TrimEnd(',') + "}";
            ViewBag.DataParam = dataParam;
            #endregion
         
            //检测Ajax请求, 进行无刷新分页
            if (Request.IsAjaxRequest())
            {
                return PartialView(ajaxViewName, pagedList);
            }
            
            return View(viewName, pagedList);
        }

        #endregion

        #region  商品详情
        public virtual ActionResult Detail(long  id, string viewName = "Detail")
        {
            YSWL.MALL.ViewModel.Shop.ProductModel model = new ProductModel();

            model.PreProduct = preProBll.GetModelInfo(id);  //id 为商品id
            if (model.PreProduct == null || model.PreProduct.Status!=1)
            {
                return RedirectToAction("Index", "Home");
            }

            model.ProductInfo = productManage.GetModel(model.PreProduct.ProductId);

            if (model.ProductInfo == null || model.ProductInfo.SaleStatus != 1)
            {
                return RedirectToAction("Index", "Home");
            }

            if (model.ProductInfo.SalesType == 1)//正常商品，跳转到正常商品详情页
            {
                return RedirectToAction("Index", "Home");
            }
            BLL.Shop.Products.ProductImage imageManage = new BLL.Shop.Products.ProductImage();
            model.ProductImages = imageManage.ProductImagesList(model.ProductInfo.ProductId);
            model.ProductSkus = skuBll.GetProductSkuInfo(model.ProductInfo.ProductId);
            BLL.Shop.Products.BrandInfo brandManage = new BLL.Shop.Products.BrandInfo();

            Model.Shop.Products.BrandInfo brandModel = brandManage.GetModelByCache(model.ProductInfo.BrandId);
            if (brandModel != null)
            {
                ViewBag.BrandName = brandModel.BrandName;
            }

            #region SEO设置
            PageSetting pageSetting = PageSetting.GetProductSetting(model.ProductInfo);
            ViewBag.Title = pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;  
            ViewBag.Description = pageSetting.Description;
            #endregion SEO设置

            ViewBag.ConsultCount = conBll.GetRecordCount("Status=1 and ProductId=" + model.ProductInfo.ProductId);
            ViewBag.CommentCount = reviewsBll.GetRecordCount("Status=1 and ProductId=" + model.ProductInfo.ProductId);
            ViewBag.IsMultiDepot = IsMultiDepot;

            return View(viewName, model);
        }
        #endregion 

    }
}
