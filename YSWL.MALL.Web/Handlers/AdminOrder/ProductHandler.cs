/**
* Product.cs
*
* 功 能： [N/A]
* 类 名： Product
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/6/20 18:04:38  huhy    初版
*
* Copyright (c) 2013 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.SessionState;
using YSWL.Json;

namespace YSWL.MALL.Web.Handlers.AdminOrder
{

    public class ProductHandler : HandlerBase, IRequiresSessionState
    {
        #region HandlerBase
        public override bool IsReusable
        {
            get { return false; }
        }
        public override void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //安全起见, 所有产品相关Ajax请求为POST模式
            string action = context.Request.Form["Action"];
            context.Response.Clear();
            context.Response.ContentType = "application/json";
            if (CurrentUser == null || CurrentUser.UserType != "AA")
            {
                JsonObject result = new JsonObject();
                result.Accumulate(KEY_STATUS, STATUS_NOLOGIN);
                context.Response.Write(result.ToString());
            }
            try
            {
                switch (action)
                {
                    case "GetSKUList":
                        context.Response.Write(GetSKUList(context));
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                JsonObject json = new JsonObject();
                json.Put(KEY_STATUS, STATUS_ERROR);
                json.Put(KEY_DATA, ex);
                context.Response.Write(json.ToString());
            }
        }
        #endregion

   

        #region 查询商品
        private string GetSKUList(HttpContext context)
        {
            JsonObject jsonResult = new JsonObject();
            YSWL.MALL.BLL.Shop.Products.ProductInfo infoBll = new BLL.Shop.Products.ProductInfo();
            List<JsonObject> jsonList = new List<JsonObject>();
            string kw = context.Request.Form["q"];
 
            int page_limit = YSWL.Common.Globals.SafeInt(context.Request.Form["page_limit"], 10);
            int page = YSWL.Common.Globals.SafeInt(context.Request.Form["page"], 1);
            int startIndex = page > 1 ? (page - 1) * page_limit + 1 : 1;
            //计算分页结束索引
            int endIndex = page * page_limit;
            int total;
            DataSet ds;
            if (YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot())//是否开启分仓
            {
                BLL.Shop.DisDepot.DepotProSKUs deposkuBll = new BLL.Shop.DisDepot.DepotProSKUs();
                total = deposkuBll.GetSKURecordCount(DepotId, kw);
                ds = deposkuBll.GetSKUListByPage(DepotId, kw, startIndex, endIndex, "");
            }
            else {
                BLL.Shop.Products.SKUInfo skuBll = new BLL.Shop.Products.SKUInfo();
                total = skuBll.GetSKURecordCount( kw);
                ds = skuBll.GetSKUListByPage( kw, startIndex, endIndex, "");
            }
            DataTable dt = ds.Tables[0];
            JsonObject json = null;
            if (!Common.DataTableTools.DataTableIsNull(dt)) {
                jsonList = new List<JsonObject>();
                foreach (DataRow dr in dt.Rows)
                {
                    json = new JsonObject();
                    json.Put("id", dr["SKU"]);
                    json.Put("text", string.Format(" {0}     (编码：{1}     库存：{2}  ) ", dr["ProductName"], dr["SKU"], dr["Stock"]));
                    jsonList.Add(json);
                }
            } 
            jsonResult.Put("total", total);
            jsonResult.Put("productList", jsonList);
            return jsonResult.ToString();
        }
 
        #endregion
        private int DepotId
        {
            get
            {
                return  Common.Globals.SafeInt(Common.Cookies.getCookie("A_Order_DepotId", "value"), 0);
            }
        }
    }
}