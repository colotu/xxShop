using System;
using System.Collections.Generic;
using System.Web.Mvc;
using YSWL.MALL.Web.Components.Setting.Shop;
using YSWL.MALL.Model.Shop.Products;

namespace YSWL.MALL.Web.Areas.MShop.Controllers
{
    public  class RecommendController : MShopControllerBase
    {
        //
        // GET: /Mobile/Recommend/
        YSWL.MALL.BLL.Shop.Products.ProductInfo productBll = new YSWL.MALL.BLL.Shop.Products.ProductInfo();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">rec:推荐, hot:热卖,cheap:特价,new:最新,irec:首页推荐</param>
        /// <param name="Cid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="vName">viewName</param>
        /// <param name="ajaxVName">ajaxViewName</param>
        /// <returns></returns>
        public ActionResult Index(string type="rec", int cid = 0, int? pageIndex = 1, int pageSize = 30,string vName = "Index", string ajaxVName = "")
        {
            ProductRecType Type;
            switch (type) {
                case "rec":
                    Type= ProductRecType.Recommend;
                    break;
                case "hot":
                    Type = ProductRecType.Hot;
                    break;
                case "cheap":
                    Type = ProductRecType.Cheap;
                    break;
                case "new":
                    Type = ProductRecType.Latest;
                    break;
                case "irec":
                    Type = ProductRecType.IndexRec;
                    break;
                default:
                    Type = ProductRecType.Recommend;
                    break;
            }
            pageIndex = pageIndex.HasValue && pageIndex.Value > 1 ? pageIndex.Value : 1;
            //计算分页起始索引
            int startIndex = pageIndex.Value > 1 ? (pageIndex.Value - 1) * pageSize + 1 : 1;
            //计算分页结束索引
            int endIndex = pageIndex.Value > 1 ? startIndex + pageSize - 1 : pageSize;
            ViewBag.PageSize = pageSize;
            #region DataParam
            ViewBag.DataParam = String.Format("{0}type:'{1}',cid:'{2}'{3}", "{", type, cid, "}");
            #endregion

            List<YSWL.MALL.Model.Shop.Products.ProductInfo> productList = productBll.GetProductRecListByPage(Type,cid, "", startIndex, endIndex);
            //检测Ajax请求, 进行无刷新分页
            if (Request.IsAjaxRequest())
                return PartialView(ajaxVName, productList);

            return View(vName, productList);
        }
    }
}

