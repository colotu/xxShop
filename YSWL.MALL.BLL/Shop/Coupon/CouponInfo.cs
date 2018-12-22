/**
* CouponInfo.cs
*
* 功 能： N/A
* 类 名： CouponInfo
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/8/26 17:20:59   N/A    初版
*
* Copyright (c) 2012-2013 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Collections.Generic;
using YSWL.Accounts.Bus;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.Common;
using YSWL.MALL.Model.Shop.Coupon;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Shop.Coupon;
using YSWL.MALL.Model.Shop.Products;
using System.Linq;

namespace YSWL.MALL.BLL.Shop.Coupon
{
    /// <summary>
    /// CouponInfo
    /// </summary>
    public partial class CouponInfo
    {
        private readonly ICouponInfo dal = DAShopCoupon.CreateCouponInfo();
        public CouponInfo()
        { }
        #region  BasicMethod
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string CouponCode)
        {
            return dal.Exists(CouponCode);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(YSWL.MALL.Model.Shop.Coupon.CouponInfo model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.Shop.Coupon.CouponInfo model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string CouponCode)
        {

            return dal.Delete(CouponCode);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string CouponCodelist)
        {
            return dal.DeleteList(CouponCodelist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.Coupon.CouponInfo GetModel(string CouponCode)
        {

            return dal.GetModel(CouponCode);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public YSWL.MALL.Model.Shop.Coupon.CouponInfo GetModelByCache(string CouponCode)
        {

            string CacheKey = "CouponInfoModel-" + CouponCode;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(CouponCode);
                    if (objModel != null)
                    {
                        int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("CacheTime");
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.Shop.Coupon.CouponInfo)objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Coupon.CouponInfo> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Coupon.CouponInfo> DataTableToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Shop.Coupon.CouponInfo> modelList = new List<YSWL.MALL.Model.Shop.Coupon.CouponInfo>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Shop.Coupon.CouponInfo model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = dal.DataRowToModel(dt.Rows[n]);
                    if (model != null)
                    {
                        modelList.Add(model);
                    }
                }
            }
            return modelList;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            return dal.GetRecordCount(strWhere);
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            return dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //return dal.GetList(PageSize,PageIndex,strWhere);
        //}

        #endregion  BasicMethod

        #region  ExtensionMethod
        /// <summary>
        /// 转移历史数据
        /// </summary>
        /// <returns></returns>
        public bool MoveHistory(out bool isNotData)
        {
            isNotData = false;
            List<YSWL.MALL.Model.Shop.Coupon.CouponInfo> infoList = GetModelList(" EndDate<'" + DateTime.Now + "' ");
            bool IsSuccess = true;
            if (infoList != null && infoList.Count > 0)
            {
                foreach (var couponInfo in infoList)
                {
                    if (!dal.AddHistory(couponInfo))
                        IsSuccess = false;
                }
            }
            else
            {
                isNotData = true;
            }
            return IsSuccess;
        }

        /// <summary>
        /// 获取优惠券，(IsExpired：是否包括过期优惠券，默认为不包括)
        /// </summary>
        /// <param name="CouponCode"></param>
        /// <returns></returns>
        public YSWL.MALL.Model.Shop.Coupon.CouponInfo GetCouponInfo(string CouponCode, bool IsExpired = false)
        {
            return dal.GetCouponInfo(CouponCode, IsExpired);
        }
        /// <summary>
        /// 获取优惠券,需要密码，(IsExpired：是否包括过期优惠券，默认为包括)
        /// </summary>
        /// <param name="CouponCode"></param>
        /// <param name="pwd"></param>
        /// <param name="IsExpired"></param>
        /// <returns></returns>
        public YSWL.MALL.Model.Shop.Coupon.CouponInfo GetCouponInfo(string CouponCode, string pwd, bool IsExpired = true)
        {
            return dal.GetCouponInfo(CouponCode, pwd, IsExpired);
        }

        /// <summary>
        /// 分配优惠券给用户（与用户挂钩）
        /// </summary>
        /// <param name="CouponCode"></param>
        /// <param name="userId"></param>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        public bool UpdateUser(string couponCode, int userId, string userEmail)
        {
            return dal.UpdateUser(couponCode, userId, userEmail);
        }
        /// <summary>
        /// 分配给用户优惠券(根据优惠券规则)
        /// </summary>
        /// <param name="ruleId"></param>
        /// <param name="userId"></param>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        public bool UpdateUser(int ruleId, int userId, string userEmail)
        {
            return dal.UpdateUser(ruleId, userId, userEmail);
        }
        /// <summary>
        ///  使用优惠券
        /// </summary>
        /// <param name="couponCode"></param>
        /// <param name="userId"></param>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        public bool UseCoupon(string couponCode, int userId, string userEmail)
        {
            return dal.UseCoupon(couponCode, userId, userEmail);
        }
        public bool UseCoupon(string couponCode, int userId)
        {
            return dal.UseCoupon(couponCode, userId);
        }
        public bool UseCoupon(string couponCode)
        {
            return dal.UseCoupon(couponCode);
        }

        public bool DeleteEx(int ruleId)
        {
            return dal.DeleteEx(ruleId);
        }

        ///// <summary>
        ///// 用户获取优惠券
        ///// </summary>
        ///// <param name="CouponCode"></param>
        ///// <param name="pwd"></param>
        ///// <param name="IsExpired"></param>
        ///// <returns></returns>
        //public YSWL.MALL.Model.Shop.Coupon.CouponInfo GetCoupon(int userId, int ruleId)
        //{
        //    //随机获取剩余优惠券
        //    YSWL.MALL.Model.Shop.Coupon.CouponInfo info = GetActCoupon(ruleId, 0);
        //    if (info != null)
        //    {
        //        YSWL.Accounts.Bus.User user = new User(userId);
        //        UpdateUser(info.CouponCode, userId, user.Email);
        //    }
        //    return info;
        //}

        public YSWL.MALL.Model.Shop.Coupon.CouponInfo GetCoupon(string openId, string userName, int ruleId)
        {
            //随机获取剩余优惠券
            YSWL.MALL.Model.Shop.Coupon.CouponInfo info = GetActCoupon(ruleId, 0);
            if (info != null)
            {
                YSWL.WeChat.BLL.Core.User wUserBll = new WeChat.BLL.Core.User();
                YSWL.WeChat.Model.Core.User wUserModel = wUserBll.GetUser(openId, userName);
                int userId = wUserModel == null ? -1 : wUserModel.UserId;
                UpdateUser(info.CouponCode, userId, userName);
            }
            return info;
        }

        public bool ExistsByUser(int userId,int ruleId=0)
        {
            return dal.ExistsByUser(userId, ruleId);
        }

        public bool ExistsByUser(string Email,int ruleId=0)
        {
            return dal.ExistsByUser(Email, ruleId);
        }

        public int  GetRuleId(int userId)
        {
            return dal.GetRuleId(userId);
        }

        public int GetRuleId(string  UserName)
        {
            return dal.GetRuleId(UserName);
        }
        /// <summary>
        /// 随机获取各个状态下的优惠券
        /// </summary>
        /// <returns></returns>
        public YSWL.MALL.Model.Shop.Coupon.CouponInfo GetActCoupon(int ruleId, int status)
        {
            //随机获取剩余优惠券
            return dal.GetActCoupon(ruleId, status);
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool UpdateStatusList(string ids, int status)
        {
            return dal.UpdateStatusList(ids, status);
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Coupon.CouponInfo> GetListByPageEX(string strWhere, string orderby, int startIndex, int endIndex)
        {
            DataSet ds = GetListByPage(strWhere, orderby, startIndex, endIndex);
            return DataTableToList(ds.Tables[0]);
        }

        public List<YSWL.MALL.Model.Shop.Coupon.CouponInfo> GetCouponList(int userId, int status, bool IsExpired = true)
        {
            DataSet ds = dal.GetCouponList(userId, status, IsExpired);
            return DataTableToList(ds.Tables[0]);
        }
        //是否过期
        public bool IsEffect(string coupon)
        {
            return dal.IsEffect(coupon);
        }

        public YSWL.MALL.Model.Shop.Coupon.CouponInfo GetAwardCode(int ActivityId,bool IsExpired=true)
        {
            return dal.GetAwardCode(ActivityId, IsExpired);
        }

        /// <summary>
        /// 绑定优惠券
        /// </summary>
        /// <param name="ActivityId"></param>
        /// <param name="IsExpired"></param>
        /// <returns></returns>
        public bool BindCoupon(string Code, int userId)
        {
            return dal.BindCoupon(Code, userId);
        }
        /// <summary>
        /// 判断优惠券是否能使用  -1:已冻结  0:数据异常 1：为正常  2：已使用 3：最消费金额限制 4：指定分类限制   5:指定商品限制  6:指定商品SKU限制 7:没购买分类  8：没购买商品  9：没购买SKU
        /// </summary>
        /// <param name="shoppingCartInfo"></param>
        /// <param name="infoModel"></param>
        /// <returns></returns>
        public int GetUseStatus(ShoppingCartInfo  cartInfo, YSWL.MALL.Model.Shop.Coupon.CouponInfo infoModel)
        {
            if (cartInfo == null || infoModel == null)
                return 0;
            if (infoModel.Status == -1)
                return -1;
            if (infoModel.Status == 2)
                return 2;
            if (infoModel.LimitPrice > cartInfo.TotalAdjustedPrice)
                return 3;
            //如果指定了分类
            if (infoModel.CategoryId > 0 && infoModel.ProductId <= 0)
            {
                //获取该分类下的商品及其子分类下的所有商品集合
                YSWL.MALL.BLL.Shop.Products.ProductInfo productBll=new YSWL.MALL.BLL.Shop.Products.ProductInfo();
                List<YSWL.MALL.Model.Shop.Products.ProductInfo> productInfos =
                 productBll.GetProductsByCid(infoModel.CategoryId);
                List<ShoppingCartItem> matchCartItem=  cartInfo.Items.Where(c=>productInfos.Exists(f=>f.ProductId==c.ProductId)).ToList() ;
                if (matchCartItem == null || matchCartItem.Count == 0)
                    return 7;
                decimal totalPrice = 0;
                matchCartItem.ForEach(info => totalPrice += info.AdjustedPrice * info.Quantity);
                if (infoModel.LimitPrice > totalPrice)
                    return 4;
            }
            //指定了商品SKU
            if (infoModel.ProductId > 0 && String.IsNullOrWhiteSpace(infoModel.ProductSku))
            {
                List<ShoppingCartItem> matchCartItem = cartInfo.Items.Where(c =>c.ProductId==infoModel.ProductId).ToList();
                decimal totalPrice = 0;
                if (matchCartItem == null || matchCartItem.Count == 0)
                    return 8;
                matchCartItem.ForEach(info => totalPrice += info.AdjustedPrice * info.Quantity);
                if (infoModel.LimitPrice > totalPrice)
                    return 5;
            }
            //指定了商品SKU
            if (!String.IsNullOrWhiteSpace(infoModel.ProductSku))
            {
                List<ShoppingCartItem> matchCartItem = cartInfo.Items.Where(c => c.SKU == infoModel.ProductSku).ToList();
                if (matchCartItem == null || matchCartItem.Count == 0)
                    return 9;
                decimal totalPrice = 0;
                matchCartItem.ForEach(info => totalPrice += info.AdjustedPrice * info.Quantity);
                if (infoModel.LimitPrice > totalPrice)
                    return 6;
            }
            return 1;
        }
        /// <summary>
        /// 优惠券使用限制
        /// </summary>
        /// <param name="infoModel"></param>
        /// <param name="IsAddLink">返回值中是否增加链接</param>
        /// <returns></returns>
        public static string GetLimitStr(YSWL.MALL.Model.Shop.Coupon.CouponInfo infoModel,bool IsAddLink=true) 
        {
            if (infoModel == null)
                return "无限制";
            if (String.IsNullOrWhiteSpace(infoModel.ProductSku))
            {
                YSWL.MALL.BLL.Shop.Products.SKUInfo skuBll = new YSWL.MALL.BLL.Shop.Products.SKUInfo();
            }
            if (infoModel.ProductId > 0)
            {
                YSWL.MALL.BLL.Shop.Products.ProductInfo infoBll = new YSWL.MALL.BLL.Shop.Products.ProductInfo();
                YSWL.MALL.Model.Shop.Products.ProductInfo productInfo = infoBll.GetModelByCache(infoModel.ProductId);
                if (IsAddLink)
                {
                    return "购买指定商品：<a href=\"{0}Product/Detail/" + productInfo.ProductId + "\">" + productInfo.ProductName + "</a>使用";
                }
                else {
                    return "购买指定商品： " + productInfo.ProductName + " 使用";
                }
               
            }
            if (infoModel.CategoryId > 0)
            {
                YSWL.MALL.BLL.Shop.Products.CategoryInfo cateBll = new YSWL.MALL.BLL.Shop.Products.CategoryInfo();
                YSWL.MALL.Model.Shop.Products.CategoryInfo categoryInfo = cateBll.GetModelByCache(infoModel.CategoryId);
                if (IsAddLink)
                {
                  return  "购买指定分类：<a href=\"{0}Product/" + categoryInfo.CategoryId + "\">" + categoryInfo.Name + "</a>商品使用";
                }
                else {
                    return "购买指定分类：" + categoryInfo.Name + " 商品使用";
                }
            }
            return "无限制";
        }

        ///// <summary>
        ///// 优惠券使用限制粗略显示(MC01)
        ///// </summary>
        ///// <param name="infoModel"></param>
        ///// <returns></returns>
        //public static string GetLimitStrEx(YSWL.MALL.Model.Shop.Coupon.CouponInfo infoModel)
        //{
        //    if (infoModel == null)
        //        return "全部商品";//无限制

        //    if (infoModel.ProductId > 0)
        //    {
        //        return "限定商品";
        //    }
        //    if (infoModel.CategoryId > 0)
        //    {
        //        return "限定分类";
        //    }
        //    return "全部商品";
        //}
        public bool GenActCoupon(YSWL.MALL.Model.Shop.Coupon.CouponRule model, int userId, string orderCode = "", long orderId = 0, int status = -1)
        {
            YSWL.MALL.Model.Shop.Coupon.CouponInfo infoModel = new Model.Shop.Coupon.CouponInfo();
            YSWL.MALL.BLL.Shop.Coupon.CouponInfo infoBll = new CouponInfo();
            infoModel.CategoryId = model.CategoryId;
            infoModel.ClassId = model.ClassId;
            infoModel.RuleId = model.RuleId;
            infoModel.ProductId = model.ProductId;
            infoModel.ProductSku = model.ProductSku;

            infoModel.CouponName = model.Name;
            infoModel.CouponPrice = model.CouponPrice;
            Random rnd = new Random();
            infoModel.EndDate = model.EndDate;
            infoModel.StartDate = model.StartDate;
            infoModel.GenerateTime = DateTime.Now;
            infoModel.IsPwd = model.IsPwd;
            infoModel.IsReuse = model.IsReuse;
            infoModel.LimitPrice = model.LimitPrice;
            infoModel.SupplierId = model.SupplierId;
            infoModel.NeedPoint = model.NeedPoint;
            infoModel.UserId = userId;
            infoModel.Status = status;
            infoModel.Type = model.Type;
            infoModel.OrderCode = orderCode;
            infoModel.OrderId = orderId;
            int maxValue = 10;
            for (int j = 1; j < model.CpLength - 4; j++)
            {
                maxValue = maxValue * 10;
            }
            int pwdValue = 10;
            for (int k = 1; k < model.PwdLength; k++)
            {
                pwdValue = pwdValue * 10;
            }
            int rand = rnd.Next(maxValue / 10 + 1, maxValue - 1);
            infoModel.CouponCode = model.PreName + userId + DateTime.Now.ToString("MMdd") +
                                   rand.ToString();
            infoModel.CouponPwd = infoModel.IsPwd == 1 ? rnd.Next(pwdValue / 10, pwdValue - 1).ToString() : "";
            while (infoBll.Exists(infoModel.CouponCode))
            {
                rand = rnd.Next(maxValue / 10 + 1, maxValue - 1);
                infoModel.CouponCode = model.PreName + userId + DateTime.Now.ToString("MMdd") +
                                  rand.ToString();
            }
            return dal.Add(infoModel);
        }
        /// <summary>
        /// 获取状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public string GetStatusName(int status)
        {
            switch (status)
            {
                case -1:
                    return "已冻结";
                case 0:
                    return "未分配";
                case 1:
                    return "已分配";
                case 2:
                    return "已使用";
                default:
                    break;
            }
            return "";
        }
        #endregion  ExtensionMethod
    }
}  

