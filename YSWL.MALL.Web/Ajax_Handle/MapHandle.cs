/**
* MapHandle.cs
*
* 功 能： 地图Handler
* 类 名： MapHandle
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/5/31 14:11:06   Ben    初版
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
using System.Linq;
using System.Web;
using YSWL.MALL.BLL.Shop.Order;
using YSWL.MALL.BLL.Shop.Shipping;
using YSWL.MALL.BLL.Shop.Supplier;
using YSWL.Json;
using YSWL.Common;
using YSWL.Map.Model;

namespace YSWL.MALL.Web.AjaxHandle
{
    public class MapHandle : YSWL.Map.Handler.MapHandlerBase
    {

        protected override bool CheckUser(HttpContext context)
        {
            return (context.User.Identity.IsAuthenticated && context.Session[Globals.SESSIONKEY_ENTERPRISE] != null);
        }

        protected override void ProcessAction(string actionName, HttpContext context)
        {
            switch (actionName)
            {
                case "SetDepartmentMap":
                    SetDepartmentMap(context);
                    return;
                case "GetSpInfo"://获取配送站信息
                    GetSpInfo(context);
                    return;
                case "GetDistributionLoad"://获取订单数量
                    GetDistributionLoad(context);
                    return;
                case "GetUserPosition"://获取商家信息
                    GetUserPositionByRegion(context);
                    return;
                default:
                    return;
            }
        }

        #region 设置企业地图标注点
        /// <summary>
        /// 设置企业地图标注点
        /// </summary>
        protected void SetDepartmentMap(HttpContext context)
        {
            JsonObject json = new JsonObject();

            //当前操作用户
            YSWL.Accounts.Bus.User currentUser = context.Session[Globals.SESSIONKEY_ENTERPRISE] as YSWL.Accounts.Bus.User;
            if (currentUser == null) return;

            //获取Ajax参数
            int departmentId = YSWL.Common.Globals.SafeInt(
                context.Request.Params["DepartmentId"], 0);                         //设置企业ID
            string markersLongitude = context.Request.Params["MarkersLongitude"];   //标注点-经度
            string markersDimension = context.Request.Params["MarkersDimension"];   //标注点-纬度
            string pointerTitle = context.Request.Params["PointerTitle"];           //标注点-标题
            string pointerContent = context.Request.Params["PointerContent"];       //标注点-描述内容
            string pointImg = context.Request.Params["PointImg"];                   //标注点-图片URL
            int mapId = YSWL.Common.Globals.SafeInt(
                context.Request.Params["MapId"], 0);                                //是否更新标注点

            if (departmentId < 1)
            {
                json.Accumulate("ERROR", "NOENTERPRISEID");
                context.Response.Write(json.ToString());
                return;
            }

            //保存数据
            Map.Model.MapInfo mapInfo = new MapInfo();
            mapInfo.UserId = currentUser.UserID;
            mapInfo.DepartmentId = departmentId;
            mapInfo.MarkersLongitude = markersLongitude;
            mapInfo.MarkersDimension = markersDimension;
            mapInfo.PointerTitle =
                YSWL.Common.Globals.HtmlEncode(pointerTitle);                  //标题编码保存
            mapInfo.PointerContent =
                YSWL.Common.Globals.HtmlEncodeForSpaceWrap(pointerContent);    //将换行符编码保存
            if (!string.IsNullOrWhiteSpace(pointImg))
            {
                //图片URL 可选项
                mapInfo.PointImg = pointImg;
            }
            if (mapId < 1)  //更新判断
            {
                mapInfo.MapId = mapInfoManage.Add(mapInfo); //ADD
            }
            else
            {
                mapInfo.MapId = mapId;
                mapInfoManage.Update(mapInfo);  //UPDATE
            }
            json.Accumulate("STATUS", "OK");
            json.Accumulate("DATA", mapInfo);
            context.Response.Write(json.ToString());
            return;
        } 
        #endregion

        #region 获取商家信息
        /// <summary>
        /// 获取商家信息
        /// </summary>
        /// <param name="context"></param>
        protected void GetSpInfo(HttpContext context)
        {
            BLL.Shop.Supplier.SupplierInfo spBll = new BLL.Shop.Supplier.SupplierInfo();
            string spName = context.Request.Params["spName"];
            int regionId = Globals.SafeInt(context.Request.Params["regionId"], -1);
            List<YSWL.MALL.Model.Shop.Supplier.SupplierInfo> supplierInfos = spBll.GetSpByRegion(regionId,
                                                                                                 spName);
            JsonObject jsonResult = new JsonObject();
            if (null != supplierInfos)
            {

                List<JsonObject> jList = new List<JsonObject>();
                JsonObject jsonObject;
                foreach (var supplierInfo in supplierInfos)
                {
                    jsonObject = new JsonObject();
                    jsonObject.Put("name", supplierInfo.Name);
                    jsonObject.Put("shopName", supplierInfo.ShopName);
                    jsonObject.Put("phone", supplierInfo.CellPhone);
                    jsonObject.Put("logo", supplierInfo.LOGO);
                    jsonObject.Put("markersDimension", supplierInfo.Latitude);
                    jsonObject.Put("markersLongitude", supplierInfo.Longitude);

                    jsonObject.Put("pointerTitle", supplierInfo.Name);//以下是标注信息
                    jsonObject.Put("pointImg", supplierInfo.LOGO);
                    jsonObject.Put("pointerContent", "电话：" + supplierInfo.CellPhone);
                    jsonObject.Put("Longitude", supplierInfo.Longitude);
                    jsonObject.Put("Dimension", supplierInfo.Latitude);

                    jList.Add(jsonObject);
                }
                jsonResult.Accumulate("count", supplierInfos.Count);
                jsonResult.Accumulate("spInfos", jList);
                context.Response.Write(jsonResult.ToString());
                return;
            }
        } 
        #endregion

        #region 配送负荷
        /// <summary>
        /// 配送负荷
        /// </summary>
        /// <param name="context"></param>
        public void GetDistributionLoad(HttpContext context)
        {
            BLL.Shop.Supplier.SupplierInfo spBll = new BLL.Shop.Supplier.SupplierInfo();
            BLL.Shop.Order.Orders ordersBll = new BLL.Shop.Order.Orders();
            string StartDate = context.Request.Params["startDate"];
            string EndDate = context.Request.Params["endDate"];
            int regionId = Globals.SafeInt(context.Request.Params["regionId"], -1);
            List<YSWL.MALL.Model.Shop.Supplier.SupplierInfo> supplierInfos = spBll.GetSpByRegion(regionId,
                                                                                                 null);
            JsonObject jsonResult = new JsonObject();
            if (null != supplierInfos)
            {
                List<JsonObject> jList = new List<JsonObject>();
                JsonObject jsonObject;
                foreach (YSWL.MALL.Model.Shop.Supplier.SupplierInfo supplierInfo in supplierInfos)
                {
                    jsonObject = new JsonObject();
                    List<YSWL.MALL.Model.Shop.Order.OrderInfo> orderList = ordersBll.GetModelList(StartDate, EndDate, supplierInfo.SupplierId);
                    int hasDistribute = 0;//已配送数量
                    int notDistribute = 0;//未配送数量
                    if (null != orderList)
                    {

                        foreach (YSWL.MALL.Model.Shop.Order.OrderInfo orderInfo in orderList)
                        {
                            if (orderInfo.ShippingStatus == 0)//未配送数量
                            {
                                notDistribute++;
                            }
                            else//已配送数量
                            {
                                hasDistribute++;
                            }
                        }
                    }

                    jsonObject.Put("spId", supplierInfo.SupplierId);
                    jsonObject.Put("totalCount", orderList != null ? orderList.Count : 0);
                    jsonObject.Put("hasDistributeCount", hasDistribute);
                    jsonObject.Put("notDistribute", notDistribute);
                    jsonObject.Put("longitude", supplierInfo.Longitude);
                    jsonObject.Put("latitude", supplierInfo.Latitude);
                    jList.Add(jsonObject);
                }
                jsonResult.Put("status", "Ok");
                jsonResult.Put("data", jList);
                context.Response.Write(jsonResult.ToString());
                return;
            }
            jsonResult.Put("status", "nodata");
            context.Response.Write(jsonResult.ToString());
            return;
        } 
        #endregion


        #region 根据加盟商获取用户数据
        /// <summary>
        /// 根据加盟商获取用户数据
        /// </summary>
        /// <param name="context"></param>
        public void GetUserPositionBySp(HttpContext context)
        {
            int supplierId = Globals.SafeInt(context.Request.Params["spId"], 0);
            YSWL.MALL.BLL.Shop.Shipping.ShippingAddress shippingBll = new ShippingAddress();
            JsonObject jsonResult = new JsonObject();
            if (supplierId > 0)
            {
                List<YSWL.MALL.Model.Shop.Shipping.ShippingAddress> addressList =
                    shippingBll.GetAddressBySupplier(supplierId);
                if (null != addressList && addressList.Count > 0)
                {
                    JsonObject jsonObject;//=new JsonObject();
                    List<JsonObject> jList = new List<JsonObject>();
                    foreach (var shippingAddress in addressList)
                    {
                        jsonObject = new JsonObject();
                        // jsonObject.Put("");
                    }
                }
            }
        } 
        #endregion

        #region  根据地区获取用户信息
        /// <summary>
        /// 根据地区获取用户信息
        /// </summary>
        /// <param name="context"></param>
        public void GetUserPositionByRegion(HttpContext context)
        {
            List<JsonObject> jsonList = new List<JsonObject>();
            JsonObject jsonResult = new JsonObject();
            JsonObject jsonObject;
            YSWL.MALL.BLL.Members.Users usersBll = new BLL.Members.Users();
            int regionId = Globals.SafeInt(context.Request.Params["regionId"], 0);
            int supplierId = Globals.SafeInt(context.Request.Params["supplierId"], 0);
            List<YSWL.MALL.ViewModel.Member.UserPosition> userPositions = usersBll.GetUserPositionList(regionId, supplierId);
            if (null != userPositions)
            {
                foreach (YSWL.MALL.ViewModel.Member.UserPosition userPosition in userPositions)
                {
                    jsonObject = new JsonObject();
                    jsonObject.Put("pointerTitle", userPosition.UserModel.NickName);//以下是标注信息
                    jsonObject.Put("pointImg", userPosition.ShopPhoto);
                    jsonObject.Put("pointerContent", "" + userPosition.UserModel.Phone);
                    jsonObject.Put("Longitude", userPosition.Longitude);
                    jsonObject.Put("Dimension", userPosition.Latitude);
                    jsonObject.Put("markersDimension", userPosition.Latitude);
                    jsonObject.Put("markersLongitude", userPosition.Longitude);
                    jsonList.Add(jsonObject);
                }
                jsonResult.Accumulate("count", userPositions.Count);
                jsonResult.Accumulate("data", jsonList);
            }
            context.Response.Write(jsonResult);
        } 
        #endregion
    }
}