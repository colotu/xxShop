/**
* PartialHandler.cs
*
* 功 能： Shop 部分类API
* 类 名： PartialHandler
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/12/17 17:04:23  GW    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Web.SessionState;
using YSWL.Components.Handlers.API;
using YSWL.Json.RPC.Web;
using YSWL.Json.RPC;
using YSWL.Json;
using YSWL.MALL.Model.Settings;
using System.Collections.Generic;
using YSWL.MALL.Model.Shop.Products;
using System.Linq;
using System.Text.RegularExpressions;
using YSWL.MALL.Model.Ms;
using System.Text;
using YSWL.MALL.Model.Shop;
using YSWL.Accounts.Bus;
using YSWL.Components;

namespace YSWL.MALL.API.Shop.v2
{
    public partial class ShopHandler
    {
        YSWL.MALL.BLL.Shop.Favorite favoBLL = new YSWL.MALL.BLL.Shop.Favorite();
        private YSWL.MALL.BLL.Members.UsersExp bllUserExp = new YSWL.MALL.BLL.Members.UsersExp();
        #region 我的收藏
        [JsonRpcMethod("MyFavorListV2", Idempotent = false)]
        [JsonRpcHelp("我的收藏")]
        public JsonObject MyFavV2(int userId = -1, int pageIndex = 1, int pageSize = 10)
        {
            if (userId < 1)
            {
                return new Result(ResultStatus.Failed, "error");
            }
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendFormat(" favo.UserId ={0} and favo.Type= {1} ", userId, (int)FavoriteEnums.Product);
            pageSize = pageSize > 0 ? pageSize : 10;
            pageIndex = pageIndex > 0 ? pageIndex : 1;
            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * pageSize + 1 : 0;
            //计算分页结束索引
            int endIndex = pageIndex * pageSize;
            List<YSWL.MALL.ViewModel.Shop.FavoProdModel> favoList = favoBLL.GetFavoriteProductListByPage(strBuilder.ToString(),  startIndex, endIndex);
            List<JsonObject> jList = new List<JsonObject>();
            JsonObject itemJson;
            List<Model.Shop.Products.SKUInfo> skulist;
            int stock;
            bool openAlertStock = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_OpenAlertStock");
            foreach (var item in favoList)
            {
                skulist = skuBLL.GetProductSkuInfo(item.ProductId);//sku信息
                itemJson = new JsonObject();
                itemJson.Put("id", item.ProductId);
                itemJson.Put("favId", item.FavoriteId);
                itemJson.Put("name", item.ProductName);
                itemJson.Put("createddate", item.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"));
                itemJson.Put("pic", item.ThumbnailUrl1);
                itemJson.Put("saleStatus", item.SaleStatus);
                itemJson.Put("ruleType", (int)ruleProductBll.GetRuleType(item.ProductId, userId));  //促销规则形式： -1：没有参与促销 0：打折  1：减价  2：直降
                itemJson.Put("marketprice", item.MarketPrice.HasValue ? item.MarketPrice.Value.ToString("F") : "0.00");
                itemJson.Put("saleprice", item.LowestSalePrice.ToString("F"));
                if (skulist != null && skulist.Count > 0)
                {
                    stock = 0;
                    if (openAlertStock) //开启警戒库存
                    {
                        skulist.ForEach(info => {
                            if (info.Stock > info.AlertStock)
                            {
                                stock += info.Stock;
                            }
                        });
                        itemJson.Put("hasStock", stock > 0 ? true : false);
                    }
                    else
                    {
                        skulist.ForEach(info => {
                            stock += info.Stock;
                        });
                        itemJson.Put("hasStock", stock > 0 ? true : false);
                    }
                }
                else
                {
                    itemJson.Put("hasStock", false);
                }
                jList.Add(itemJson);
            }
            return new Result(ResultStatus.Success, jList);
        }
        #endregion

        #region 获取个人中心首页信息
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        [JsonRpcMethod("GetUserIndexInfoV2", Idempotent = false)]
        [JsonRpcHelp("获取用户信息")]
        public JsonObject GetUserIndexInfoV2(int userId)
        {
            YSWL.MALL.Model.Members.UsersExpModel user = bllUserExp.GetUsersModel(userId);
            if (null == user)
            {
                return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
            }
            if (user.UserType=="AA")
            {
                return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
            }
            YSWL.MALL.BLL.Shop.Order.Orders orderBll = new BLL.Shop.Order.Orders();
                JsonObject json = new JsonObject();
                json.Put("userId", user.UserID);
                json.Put("userName", user.UserName);
                json.Put("trueName", user.TrueName);
                json.Put("level", user.UserType);
                json.Put("unpaid", orderBll.GetUnPaidCounts(userId));//未支付订单数
                json.Put("unShipping", orderBll.GetRecordCount(string.Format("  BuyerID={0} AND  ShippingStatus<2 AND OrderStatus!=-1 and OrderType=1 and ( ( PaymentStatus=2 and PaymentGateway<>'cod' ) or ( PaymentStatus=0 and PaymentGateway='cod' ) ) ", userId)));//待发货订单数（货到付款未发货，在线支付已支付未发货时）
                json.Put("unReceipt", orderBll.GetRecordCount(string.Format("  BuyerID={0} AND ShippingStatus=2 AND OrderStatus=1 and OrderType=1 ", userId)));//待收货 
                json.Put("isEnableRank", BLL.SysManage.ConfigSystem.GetBoolValueByCache("RankScoreEnable")); //是否开启会员等级
                BLL.Members.UserRank userRankBll = new BLL.Members.UserRank();
                //会员等级
                user.UserRank = userRankBll.GetRankInfo(user.Grade.HasValue ? user.Grade.Value : 0);
            if (user.UserRank != null)
                {
                    json.Put("userRank", user.UserRank.Name);
                }
                else {
                    json.Put("userRank", "");
                }
                return new Result(ResultStatus.Success, json);
        }
        #endregion


        #region 更改用户信息 
        [JsonRpcMethod("UpdateUserV2", Idempotent = false)]
        [JsonRpcHelp("更改用户信息")]
        public JsonObject UpdateUserV2(int userId, string trueName, string phone, string email,string qq, string password="",string newPassword="") //userId，userName,trueName,phone,email,qq
        {
            try
            {
                YSWL.MALL.Model.Members.Users userModel = BLLUser.GetModelByCache(userId);
                YSWL.MALL.Model.Members.UsersExpModel userExp = bllUserExp.GetUsersModel(userId);
                YSWL.Accounts.Bus.User user = new User(userId);
                if (null == user)
                {
                    return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
                }
                if (user.UserType == "AA")
                {
                    return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
                }

                //修改密码
                if (!string.IsNullOrWhiteSpace(password) && !string.IsNullOrWhiteSpace(newPassword))
                {
                    //验证旧密码
                    SiteIdentity siteIdentity = new SiteIdentity(userId);
                    if (siteIdentity.TestPassword(password) == 0)
                    {
                        return new Result(ResultStatus.Failed, Result.FormatFailed("101", "原密码不正确, 更新个人信息失败!"));
                    }
                    //非普通用户禁用接口
                    if (user.UserType != "UU")
                    {
                        return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_UNAUTHORIZED, ERROR_MSG_UNAUTHORIZED));
                    }
                    Accounts.Bus.User currentUser=new User(userId);
                    if (currentUser.SetPassword(currentUser.UserName, newPassword))
                    {
                        return new Result(ResultStatus.Success, "success");
                    }
                    else
                    {
                        return new Result(ResultStatus.Failed, "Failed");
                    }
                }

                //NO DATA
                if (string.IsNullOrWhiteSpace(user.UserType))
                {
                    return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_NODATA, ERROR_MSG_NODATA));
                }
                if (!String.IsNullOrWhiteSpace(trueName))
                {
                    userModel.TrueName = trueName;
                }

                if (!String.IsNullOrWhiteSpace(phone))
                {
                    userModel.Phone = phone;
                }
                if (!String.IsNullOrWhiteSpace(email))
                {
                    userModel.Email = email;
                }
                if (!String.IsNullOrWhiteSpace(qq))
                {
                    userExp.QQ = qq;
                }

                if (BLLUser.Update(userModel) && bllUserExp.UpdateEx(userExp))
                {
                    return new Result(ResultStatus.Success, "success");
                }
                else
                {
                    return new Result(ResultStatus.Failed, "Failed");
                }
            }
            catch (Exception ex)
            {
                YSWL.MALL.BLL.SysManage.LogHelp.AddErrorLog(string.Format(ERROR_MSG_LOG, Request.Headers[REQUEST_HEADER_METHOD], ex.Message), ex.StackTrace, Request);
                return new Result(ResultStatus.Error, ex);
            }
        }
        #endregion

        #region 保存地址
        [JsonRpcMethod("SaveAddressV2", Idempotent = false)]
        [JsonRpcHelp("保存地址")]
        public JsonObject SaveAddressV2(int id, int UserId, string name, string phonenumber, int areaid,
                                         string areadetail, string zipcode, string celphone, string email)
        {
            JsonObject jsonResult;


            #region 新增地址
            if (id < 1)//新增地址
            {
                jsonResult = new JsonObject();
                if (areaid < 1)
                {
                    jsonResult.Put("response", "saveFailure");
                }
                YSWL.MALL.Model.Shop.Shipping.ShippingAddress modelShip = new YSWL.MALL.Model.Shop.Shipping.ShippingAddress
                {
                    UserId = YSWL.Common.Globals.SafeInt(UserId, -1),
                    ShipName = name,
                    RegionId = areaid,
                    Address = areadetail,
                    CelPhone = celphone,
                    Zipcode = zipcode,
                    EmailAddress = email
                };
                if (_addressManage.Add(modelShip) > 0) //新增地址成功
                {
                    jsonResult.Put("response", "saveSuccess");
                }
                else//添加失败 
                {
                    jsonResult.Put("response", "saveFailure");
                }

                //  return jsonResult;
            }
            #endregion

            #region 修改地址
            else//
            {
                jsonResult = new JsonObject();
                YSWL.MALL.Model.Shop.Shipping.ShippingAddress shipModel = _addressManage.GetModelByCache(id);
                if (null != shipModel)
                {
                    shipModel.ShipName = name;
                    shipModel.TelPhone = phonenumber;
                    shipModel.RegionId = YSWL.Common.Globals.SafeInt(areaid, -1);
                    shipModel.Address = areadetail;
                    shipModel.CelPhone = celphone;
                    shipModel.Zipcode = zipcode;
                    shipModel.EmailAddress = email;
                    if (_addressManage.Update(shipModel))
                    {
                        jsonResult.Put("response", "updateSuccess");
                    }
                    else
                    {
                        jsonResult.Put("response", "updateFailure");
                    }

                }
                else
                {
                    jsonResult.Put("response", "updateFailure");
                }

            }
            #endregion

            #region 获取地址列表
            List<YSWL.MALL.Model.Shop.Shipping.ShippingAddress> shipList =
                    _addressManage.GetModelList(" UserId=" + UserId);
            List<JsonObject> addList = new List<JsonObject>();
            JsonObject addItem;

            if (null == shipList || shipList.Count < 1)
            {
                jsonResult.Put("addresslist", null);
            }
            else
            {
                jsonResult = new JsonObject();
                foreach (var item in shipList)
                {
                    addItem = new JsonObject();
                    addItem.Put("id", item.ShippingId);
                    addItem.Put("regionId", item.RegionId);
                    addItem.Put("addressArea", item.RegionFullName);
                    addItem.Put("name", item.ShipName);
                    addItem.Put("phone", item.CelPhone);
                    addItem.Put("areaDetail", item.Address);
                    addItem.Put("zipcode", item.Zipcode);
                    addItem.Put("isDefault", item.IsDefault);
                    addItem.Put("email", item.EmailAddress);
                    addList.Add(addItem);
                }
                // jsonResult.Put("addresslist", addList);
            }
            #endregion

            return new Result(ResultStatus.Success, addList);
        }
        #endregion

        #region 地址列表
        [JsonRpcMethod("AddressListV2", Idempotent = false)]
        [JsonRpcHelp("地址列表")]
        public JsonObject AddressListV2(int userId, int pageIndex, int pageNum)
        {
            JsonObject jsonResult = new JsonObject();
            List<JsonObject> jsList;
            if (userId < 1)
            {
                return new Result(ResultStatus.Failed, "dataerror");
            }
            pageNum = pageNum < 1 ? 10 : pageNum;
            int totalCount = _addressManage.GetRecordCount(" UserId=" + userId);
            if (totalCount < 1)
            {
                return new Result(ResultStatus.Success, "[]");
            }
            int starIndex = pageIndex > 1 ? (pageIndex - 1) * pageNum + 1 : 0;
            int endIndex = pageIndex * pageNum;
            List<YSWL.MALL.Model.Shop.Shipping.ShippingAddress> attrList =
                _addressManage.GetListByPageEx(" UserId=" + userId, "", starIndex, endIndex);
            if (null != attrList)
            {
                jsList = new List<JsonObject>();
                JsonObject jsonItem;
                foreach (var item in attrList)
                {
                    jsonItem = new JsonObject();
                    jsonItem.Put("id", item.ShippingId);
                    jsonItem.Put("regionId", item.RegionId);
                    jsonItem.Put("addressArea", item.RegionFullName);
                    jsonItem.Put("name", item.ShipName);
                    jsonItem.Put("phone", item.CelPhone);
                    jsonItem.Put("areaDetail", item.Address);
                    jsonItem.Put("zipCode", item.Zipcode);
                    jsonItem.Put("hasDefault", item.IsDefault);
                    jsonItem.Put("email", item.EmailAddress);
                    jsList.Add(jsonItem);
                }
                jsonResult.Put("status", "success");
                jsonResult.Put("result", jsList);
            }
            return jsonResult;
        }
        #endregion

    }
}