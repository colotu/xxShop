using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using YSWL.Accounts.Bus;
using YSWL.MALL.BLL.Ms;
using YSWL.MALL.BLL.Settings;
using YSWL.MALL.BLL.Shop.Order;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.MALL.BLL.Shop.Sales;
using YSWL.MALL.BLL.Shop.Shipping;
using YSWL.MALL.BLL.SysManage;
using YSWL.Common;
using YSWL.Components.Handlers.API;
using YSWL.Json;
using YSWL.Json.RPC;
using YSWL.MALL.Model.Members;
using YSWL.MALL.Model.Shop;
using YSWL.MALL.Model.Shop.Order;
using YSWL.MALL.Model.Shop.Products;
using YSWL.MALL.Model.SysManage;
using YSWL.MALL.ViewModel.Shop;
using Webdiyer.WebControls.Mvc;
using BrandInfo = YSWL.MALL.BLL.Shop.Products.BrandInfo;
using CategoryInfo = YSWL.MALL.BLL.Shop.Products.CategoryInfo;
using EnumHelper = YSWL.MALL.Model.Shop.Order.EnumHelper;
using OrderAction = YSWL.MALL.BLL.Shop.Order.OrderAction;
using OrderItems = YSWL.MALL.Model.Shop.Order.OrderItems;

namespace YSWL.MALL.API.Shop.v1
{
    public partial class ShopHandler
    {
        private YSWL.MALL.BLL.Members.UsersExp bllUserExp = new YSWL.MALL.BLL.Members.UsersExp();

        #region 商品收藏
        [JsonRpcMethod("ProductAddFav", Idempotent = false)]
        [JsonRpcHelp("商品收藏")]
        public JsonObject ProductAddFav(int productId = -1, int userId = -1)
        {
            JsonObject jsonObject = new JsonObject();
            if (productId < 1 || userId < 1)
            {
                return new Result(ResultStatus.Failed, "dataError");
            }
            YSWL.MALL.BLL.Shop.Favorite favBLL = new YSWL.MALL.BLL.Shop.Favorite();
            //是否已经收藏
            if (favBLL.Exists(productId, userId, 1))
            {
                return new Result(ResultStatus.Success, "hasFav");
            }
            YSWL.MALL.Model.Shop.Favorite favMode = new YSWL.MALL.Model.Shop.Favorite();
            favMode.CreatedDate = DateTime.Now;
            favMode.TargetId = productId;
            favMode.Type = 1;
            favMode.UserId = userId;
            if (favBLL.Add(favMode) > 0)
            {
                return new Result(ResultStatus.Success, "success");
            }
            return new Result(ResultStatus.Failed, "error");

        } 
        #endregion

        #region 取消商品收藏
        [JsonRpcMethod("CancelFav", Idempotent = false)]
        [JsonRpcHelp("取消商品收藏")]
        public JsonObject CancelFav(int userId = -1, int favId = -1)
        {
            if (favId < 1)
            {
                return new Result(ResultStatus.Failed, "noItemId");
            }
            if (userId < 1)
            {
                return new Result(ResultStatus.Failed, "noUser");
            }
            int favoriteId = YSWL.Common.Globals.SafeInt(favId, 0);
            if (favoBLL.Delete(favoriteId))
            {
                return new Result(ResultStatus.Success, "success");
            }

            return new Result(ResultStatus.Error, "error");
        } 
        #endregion

        #region 我的收藏
        [JsonRpcMethod("MyFavorList", Idempotent = false)]
        [JsonRpcHelp("我的收藏")]
        public JsonObject MyFav(int userId = -1, int pageIndex = 1, int pageSize = 10)
        {
            if (userId < 1)
            {
                return new Result(ResultStatus.Failed, "error");
            }
            bool openAlertStock = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_OpenAlertStock");
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendFormat(" favo.UserId ={0} and favo.Type= {1} ", userId, (int)FavoriteEnums.Product);
            pageSize = pageSize > 0 ? pageSize : 10;
            pageIndex = pageIndex > 0 ? pageIndex : 1;
            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * pageSize + 1 : 0;

            //计算分页结束索引
            int endIndex = pageIndex * pageSize;
            int toalCount = favoBLL.GetRecordCount(" UserId =" + userId + " and Type=" + (int)FavoriteEnums.Product);//获取总条数 
            if (toalCount < 1)
            {
                return new Result(ResultStatus.Success, "[]");
            }
            List<YSWL.MALL.ViewModel.Shop.FavoBuyProdModel> favoList = favoBLL.GetBuyListByPage(strBuilder.ToString(),"", startIndex, endIndex);
            PagedList<YSWL.MALL.ViewModel.Shop.FavoBuyProdModel> lists = new PagedList<YSWL.MALL.ViewModel.Shop.FavoBuyProdModel>(favoList, pageIndex, pageSize, toalCount);
            List<JsonObject> jList = new List<JsonObject>();
            JsonObject itemJson;
            YSWL.MALL.Model.Shop.Products.ProductInfo productInfo;
            YSWL.MALL.Model.Shop.Products.SKUInfo skuInfoModel;
            YSWL.MALL.BLL.Members.UserRank rankBll = new BLL.Members.UserRank();
            YSWL.MALL.Model.Members.UserRank userRank = rankBll.GetUserRank(userId);
            foreach (var item in lists)
            {
                itemJson = new JsonObject();
                skuInfoModel = skuBLL.GetModelBySKU(item.ProductCode) ?? new YSWL.MALL.Model.Shop.Products.SKUInfo();
                itemJson.Put("id", item.ProductId);
                itemJson.Put("favId", item.FavoriteId);
                itemJson.Put("name", item.ProductName);
                itemJson.Put("createddate", item.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"));
                itemJson.Put("pic", item.ThumbnailUrl1);
                itemJson.Put("saleStatus", item.SaleStatus);
                itemJson.Put("sku", item.ProductCode);
                itemJson.Put("discount", GetProductSale(item.ProductId, userRank));
                itemJson.Put("discountRule", GetAllSaleRule(item.ProductId, userRank));
                productInfo = productManage.GetModelByCache(item.ProductId);
                if (null != productInfo)
                {
                    itemJson.Put("marketprice",
                                 productInfo.MarketPrice.HasValue ? productInfo.MarketPrice.Value.ToString("F") : "0.00");
                    itemJson.Put("saleprice", productInfo.LowestSalePrice.ToString("F"));
                }
                else
                {
                    itemJson.Put("marketprice", "0.00");
                    itemJson.Put("saleprice", "0.00");
                }
                if (openAlertStock) //开启警戒库存
                {
                    if (skuInfoModel.Stock > skuInfoModel.AlertStock)
                    {
                        itemJson.Put("hasStock", true);
                        itemJson.Put("stockcount", skuInfoModel.Stock);

                    }
                    else
                    {
                        itemJson.Put("hasStock", false);
                    }
                }
                else
                {
                    if (skuInfoModel.Stock > 0)
                    {
                        itemJson.Put("hasStock", true);
                        itemJson.Put("stockcount", skuInfoModel.Stock);
                    }
                    else
                    {
                        itemJson.Put("hasStock", false);
                    }
                }

                jList.Add(itemJson);
            }
            return new Result(ResultStatus.Success, jList);
        } 
        #endregion

        #region 保存地址
        [JsonRpcMethod("SaveAddress", Idempotent = false)]
        [JsonRpcHelp("保存地址")]
        public JsonObject SaveAddress(int id, int UserId, string name, string phonenumber, int areaid,
                                         string areadetail, string zipcode, string celphone)
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
                    Zipcode = zipcode
                };
                if (_shippingAddressManage.Add(modelShip) > 0) //新增地址成功
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
                YSWL.MALL.Model.Shop.Shipping.ShippingAddress shipModel = _shippingAddressManage.GetModelByCache(id);
                if (null != shipModel)
                {
                    shipModel.ShipName = name;
                    shipModel.TelPhone = phonenumber;
                    shipModel.RegionId = YSWL.Common.Globals.SafeInt(areaid, -1);
                    shipModel.Address = areadetail;
                    shipModel.CelPhone = celphone;
                    shipModel.Zipcode = zipcode;
                    if (_shippingAddressManage.Update(shipModel))
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
                    _shippingAddressManage.GetModelList(" UserId=" + UserId);
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
                    addItem.Put("isDefault",item.IsDefault);
                    addList.Add(addItem);
                }
               // jsonResult.Put("addresslist", addList);
            }
            #endregion

            return new Result(ResultStatus.Success, addList);
        } 
        #endregion

        #region 设置默认地址
        [JsonRpcMethod("SetAddressDefault", Idempotent = false)]
        [JsonRpcHelp("设置默认地址")]
        public JsonObject SetDefault(int userId, int shipId)
        {
            JsonObject jsonResult = new JsonObject();
            if (shipId < 1)
            {
                jsonResult.Put("status", "failure");
                return jsonResult;
            }
            List<YSWL.MALL.Model.Shop.Shipping.ShippingAddress> list = _shippingAddressManage.GetModelList(" UserId=" + userId);
            if (null != list)
            {
                foreach (var item in list)
                {
                    if (item.IsDefault && item.ShippingId != shipId)
                    {
                        item.IsDefault = false;
                        _shippingAddressManage.Update(item);
                    }
                    if (item.ShippingId == shipId)
                    {
                        item.IsDefault = true;
                        _shippingAddressManage.Update(item);
                    }
                }
                jsonResult.Put("status", "success");
            }
            return  jsonResult;

        } 
        #endregion

        #region 地址列表
        [JsonRpcMethod("addresslist", Idempotent = false)]
        [JsonRpcHelp("地址列表")]
        public JsonObject AddressList(int userId, int pageIndex, int pageNum)
        {
            JsonObject jsonResult = new JsonObject();
            List<JsonObject> jsList;
            if (userId < 1)
            {
                return new Result(ResultStatus.Failed, "dataerror");
            }
            pageNum = pageNum < 1 ? 10 : pageNum;
            int totalCount = _shippingAddressManage.GetRecordCount(" UserId=" + userId);
            if (totalCount < 1)
            {
                return new Result(ResultStatus.Success, "[]");
            }
            int starIndex = pageIndex > 1 ? (pageIndex - 1) * pageNum + 1 : 0;
            int endIndex = pageIndex * pageNum;
            List<YSWL.MALL.Model.Shop.Shipping.ShippingAddress> attrList =
                _shippingAddressManage.GetListByPageEx(" UserId=" + userId, "", starIndex, endIndex);
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
                    jsList.Add(jsonItem);
                }
                jsonResult.Put("status", "success");
                jsonResult.Put("result",jsList);
            }
            return jsonResult;
        } 
        #endregion

        #region 我的积分

        [JsonRpcMethod("myintegral", Idempotent = false)]
        [JsonRpcHelp("我的积分")]
        public JsonObject MyPoints(int userId = -1, int pageIndex = 1, int pageNum = 10)
        {
            JsonObject jsonObject;
            List<JsonObject> jsObjectList;
            if (userId < 1)
            {
                return new Result(ResultStatus.Success, "暂无用户数据");
            }
            //首页用户数据
            YSWL.MALL.Model.Members.UsersExpModel userEXModel = userEXBLL.GetUsersModel(userId);
            if (userEXModel == null)
            {
                return new Result(ResultStatus.Success, "暂无用户数据");
            }
            pageIndex = pageIndex > 0 ? pageIndex : 1;
            pageNum = pageNum > 0 ? pageNum : 10;
            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * pageNum + 1 : 0;

            //计算分页结束索引
            int endIndex = pageIndex * pageNum;
            int toalCount = 0;
            //获取总条数
            toalCount = detailBLL.GetRecordCount(" UserID=" + userId);
            if (toalCount < 1)
            {
                return new Result(ResultStatus.Success, "[]");
            }
            List<YSWL.MALL.Model.Members.PointsDetail> detailList = detailBLL.GetListByPageEX("UserID=" + userId, " ", startIndex, endIndex);
            if(null==detailList) return new Result(ResultStatus.Success,"[]");
            if (detailList.Count > 0)
            {
                foreach (var item in detailList)
                {
                    item.RuleName = GetRuleName(item.RuleId);
                }
            }
            jsObjectList = new List<JsonObject>();
            foreach (var item in detailList)
            {
                jsonObject = new JsonObject();
                jsonObject.Put("time", item.CreatedDate.ToString("yyyy-MM-dd"));
                jsonObject.Put("type", item.Type);
                jsonObject.Put("typeStr", item.RuleName);
                jsonObject.Put("integralvalue", item.Score);
                jsonObject.Put("describe", item.Description);
                jsObjectList.Add(jsonObject);
            }
            return new Result(ResultStatus.Success, jsObjectList);
        }
        public string GetRuleName(int RuleId)
        {
            if (RuleId == -1)
            {
                return "积分消费";
            }
            return ruleBLL.GetRuleName(RuleId);
        } 
        #endregion

        #region 我的优惠券

        /// <summary>
        /// 我的优惠券
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageIndex">当前页面</param>
        /// <param name="pageNum">页显示数量</param>
        /// <param name="type">优惠券类型</param>
        /// <returns></returns>
        [JsonRpcMethod("MyCunpon", Idempotent = false)]
        [JsonRpcHelp("我的优惠券")]
        public JsonObject MyCunpon(int userId = -1, int pageIndex = 1, int pageNum = 10, int type = 1)
        {
            JsonObject jsonObject;
            List<JsonObject> jList;
            if (userId < 1)
            {
                return new Result(ResultStatus.Failed, "userdataError");
            }
            pageIndex = pageIndex > 1 ? pageIndex : 1;
            pageNum = pageNum > 0 ? pageNum : 10;
            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * pageNum + 1 : 0;
            //计算分页结束索引
            int endIndex = pageIndex * pageNum;
            int toalCount = 0;
            //获取总条数
            toalCount =couponBLL.GetRecordCount(String.Format(" UserID={0} and Status={1}", userId, type));
            if (toalCount < 1)
            {
                return new Result(ResultStatus.Success, "[]");//NO DATA
            }
            List<YSWL.MALL.Model.Shop.Coupon.CouponInfo> infoList = couponBLL.GetListByPageEX(String.Format(" UserID={0} and Status={1}", userId, type), " GenerateTime desc", startIndex, endIndex);
            YSWL.MALL.BLL.Shop.Coupon.CouponClass classBLL = new YSWL.MALL.BLL.Shop.Coupon.CouponClass();
            foreach (var Info in infoList)
            {
                YSWL.MALL.Model.Shop.Coupon.CouponClass classModel = classBLL.GetModelByCache(Info.ClassId);
                Info.ClassName = classModel == null ? "" : classModel.Name;
            }
            if (infoList.Count > 0)
            {
                jList = new List<JsonObject>();
                foreach (var item in infoList)
                {
                    jsonObject = new JsonObject();
                    jsonObject.Put("id", item.RuleId);
                    jsonObject.Put("classId", item.ClassId);
                    jsonObject.Put("className", item.ClassName);
                    jsonObject.Put("couponCode", item.CouponCode);
                    jsonObject.Put("couponName", item.CouponName);
                    jsonObject.Put("status", item.Status);
                    jsonObject.Put("endDate", item.StartDate.ToString("yyyy-MM-dd") + "至" + item.EndDate.ToString("yyyy-MM-dd"));
                    jsonObject.Put("couponPrice", item.CouponPrice);
                    jsonObject.Put("limitPrice", item.LimitPrice);
                    jsonObject.Put("limitStr", BLL.Shop.Coupon.CouponInfo.GetLimitStr(item,false));
                    jList.Add(jsonObject);
                }
                return new Result(ResultStatus.Success, jList);
            }
            return new Result(ResultStatus.Failed, "nodata");//NO DATA
        }
        #endregion

        #region 获取用户信息
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        [JsonRpcMethod("GetUserInfo", Idempotent = false)]
        [JsonRpcHelp("获取用户信息")]
        public  JsonObject GetUserInfo(int userId)
        {
            //超级管理员信息保护 过滤UserId=1用户
            if (userId < 2) return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
            YSWL.MALL.Model.Members.UsersExpModel user = bllUserExp.GetUsersModel(userId);
            if (null != user)
            {
                JsonObject json = new JsonObject();
                json.Put("userId", user.UserID);
                json.Put("userName", user.UserName);
                json.Put("trueName", user.TrueName);
                json.Put("phone", user.Phone);
                json.Put("level", user.UserType);
                json.Put("nickName", user.NickName);
                json.Put("headImage", "/Upload/User/Gravatar/" + (user.UserID) + ".jpg?id=" + DateTime.Now);
                json.Put("point", user.Points);
                json.Put("sex", user.Sex);
                json.Put("tel", user.TelPhone);
                json.Put("email", user.Email);
                json.Put("birthday", user.Birthday.HasValue ? user.Birthday.Value.ToString("yyyy-MM-dd") : "");
                json.Put("province", user.RegionId);//地区
                json.Put("regionFullName", RegionBLL.GetRegionFullName(user.RegionId));//地区全名
                return new Result(ResultStatus.Success,json);
            }
            else
            {
                return new Result(ResultStatus.Success, null);
            }
        } 
        #endregion

        #region 更改用户信息
        [JsonRpcMethod("UpdateUser", Idempotent = false)]
        [JsonRpcHelp("更改用户信息")]
        public JsonObject UpdateUserInfo(int userId, string password, string newPassword, string nickName, string tel, string phone, string email, DateTime? birthday, int regionId = -1, int sex = -1)
        {
            if (userId < 2) 
                return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
            try
            {
                YSWL.MALL.Model.Members.Users userModel = BLLUser.GetModelByCache(userId);
                YSWL.MALL.Model.Members.UsersExpModel userExp = bllUserExp.GetUsersModel(userId);
                YSWL.Accounts.Bus.User user = new User(userId);
                //NO DATA
                if (string.IsNullOrWhiteSpace(user.UserType)) return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_NODATA, ERROR_MSG_NODATA));
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
                    user.Password = AccountsPrincipal.EncryptPassword(newPassword);
                    userModel.Password = user.Password;
                }
                if (!string.IsNullOrWhiteSpace(nickName))
                {
                    userModel.NickName = nickName;
                }
                if (!string.IsNullOrWhiteSpace(tel))
                {
                    userExp.TelPhone = tel;
                }
                if (!string.IsNullOrWhiteSpace(phone))
                {
                    userModel.Phone = phone;
                    userExp.Phone = phone;
                }
                if (!string.IsNullOrWhiteSpace(email))
                {
                    userModel.Email = email;
                    userExp.Email = email;
                }
                if (birthday.HasValue)
                {
                    userExp.Birthday = birthday.Value;
                }
                if (sex != -1)
                {
                    userModel.Sex = sex.ToString();
                    userExp.Sex = sex.ToString();
                }
                if (regionId > 0)
                {
                    userExp.RegionId = regionId;
                }
                if (BLLUser.Update(userModel) && bllUserExp.Update(userExp))
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

    }
}