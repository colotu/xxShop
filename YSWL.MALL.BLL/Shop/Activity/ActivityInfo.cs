/**  版本信息模板在安装目录下，可自行修改。
* ActivityInfo.cs
*
* 功 能： N/A
* 类 名： ActivityInfo
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/6/10 22:26:32   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Collections.Generic;
using YSWL.MALL.BLL.Shop.Coupon;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.Common;
using YSWL.MALL.Model.Shop.Activity;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Shop.Activity;
using System.Linq;
using YSWL.MALL.Model.Shop.Products;
using YSWL.MALL.ViewModel.Shop;
using ProductInfo = YSWL.MALL.Model.Shop.Products.ProductInfo;
using SKUItem = YSWL.MALL.Model.Shop.Products.SKUItem;
using System.Text;

namespace YSWL.MALL.BLL.Shop.Activity
{
    /// <summary>
    /// ActivityInfo
    /// </summary>
    public partial class ActivityInfo
    {
        private readonly IActivityInfo dal = DAShopActivity.CreateActivityInfo();
        private YSWL.MALL.BLL.Shop.Activity.ActivityRule ruleBll = new Activity.ActivityRule();
        private BLL.Shop.Order.Orders orderBll = new Order.Orders();
        private YSWL.MALL.BLL.Shop.Activity.ActivityDetail bllActiDetail = new BLL.Shop.Activity.ActivityDetail();
        private YSWL.MALL.BLL.Shop.Coupon.CouponInfo infoBll = new CouponInfo();
        public ActivityInfo()
        { }
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return dal.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ActivityId)
        {
            return dal.Exists(ActivityId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.MALL.Model.Shop.Activity.ActivityInfo model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.Shop.Activity.ActivityInfo model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ActivityId)
        {

            return dal.Delete(ActivityId);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string ActivityIdlist)
        {
            return dal.DeleteList(ActivityIdlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.Activity.ActivityInfo GetModel(int ActivityId)
        {

            return dal.GetModel(ActivityId);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public YSWL.MALL.Model.Shop.Activity.ActivityInfo GetModelByCache(int ActivityId)
        {

            string CacheKey = "ActivityInfoModel-" + ActivityId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(ActivityId);
                    if (objModel != null)
                    {
                        int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("CacheTime");
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.Shop.Activity.ActivityInfo)objModel;
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
        public List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> DataTableToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> modelList = new List<YSWL.MALL.Model.Shop.Activity.ActivityInfo>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Shop.Activity.ActivityInfo model;
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
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> GetModelList(int Top, string strWhere, string filedOrder)
        {
            DataSet ds = dal.GetList(Top, strWhere, filedOrder);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 根据购物车获取符合活动赠送的商品
        /// </summary>
        /// <param name="cartInfo"></param>
        /// <returns></returns>
        public YSWL.MALL.Model.Shop.Activity.ActivityInfo GetActGift(YSWL.MALL.Model.Shop.Products.ShoppingCartInfo cartInfo, int BuyerId)
        {
            if (cartInfo == null)
            {
                return null;
            }
            //规则列表 按照优先级正序
            List<YSWL.MALL.Model.Shop.Activity.ActivityRule> ruleList = ruleBll.GetAvailableRuleList();
            if (ruleList == null || ruleList.Count <= 0)
            {
                return null;
            }
            YSWL.MALL.Model.Shop.Activity.ActivityRule firstRule = ruleList.FirstOrDefault(c => c.RuleId == 1);
            YSWL.MALL.Model.Shop.Activity.ActivityRule fullRule = ruleList.FirstOrDefault(c => c.RuleId == 2);
            firstRule = firstRule == null ? new Model.Shop.Activity.ActivityRule() : firstRule;
            fullRule = fullRule == null ? new Model.Shop.Activity.ActivityRule() : fullRule;

            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> AllInfoList = GetActInfos();
            if (AllInfoList == null)
            {
                return null;
            }
            //活动列表 按照主键倒叙
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> actInfoList = AllInfoList.Where(o => o.StartDate <= DateTime.Now).Where(o => o.EndDate.AddDays(1) > DateTime.Now).ToList();

            if (actInfoList == null || actInfoList.Count <= 0)
            {
                return null;
            }
            //首单的活动列表
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> firstActList =
                actInfoList.Where(c => c.RuleId == 1).ToList();

            //满额赠送活动列表
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> fullActList =
             actInfoList.Where(c => c.RuleId == 2).ToList();

            YSWL.MALL.Model.Shop.Activity.ActivityInfo matchFistAct = null;
            YSWL.MALL.Model.Shop.Activity.ActivityInfo matchFullAct = null;
            if (firstActList != null && firstActList.Count > 0)
            {
                matchFistAct = IsMatchFistOrder_O(cartInfo, BuyerId, firstActList);
            }
            if (fullActList != null && fullActList.Count > 0)
            {
                matchFullAct = IsMatchFull_O(cartInfo, fullActList);
            }

            if (matchFistAct == null)
            {
                return matchFullAct;
            }
            if (matchFullAct == null)
            {
                return matchFistAct;
            }
            //判断优先级
            return firstRule.Priority <= fullRule.Priority ? matchFistAct : matchFullAct;
        }

       /// <summary>
        /// 根据购物车获取符合条件的活动 （不含运费）
       /// </summary>
       /// <param name="cartInfo"></param>
       /// <param name="buyerId"></param>
       /// <param name="couponAmount">优惠劵金额</param>
       /// <returns></returns>
        public List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> GetActGiftList(KeyValuePair<int, List<YSWL.MALL.Model.Shop.Products.ShoppingCartItem>> suppCartItems, int buyerId, decimal couponAmount = 0)
        {
            if (suppCartItems.Value == null)
            {
                return null;
            }
            decimal amount =0;//订单商品调整后总额-优惠劵面值 （不包含运费）
            decimal totalAdjustedPrice = 0;
            suppCartItems.Value.ForEach(info => totalAdjustedPrice += info.AdjustedPrice * info.Quantity);
            if (couponAmount > 0)
            {
                amount = totalAdjustedPrice - couponAmount;
                amount = amount > 0 ? amount : 0;//未防止优惠劵金额按大于商品总额
            }
            else {
                amount = totalAdjustedPrice;
            }
            //规则列表 按照优先级正序
            //List<YSWL.MALL.Model.Shop.Activity.ActivityRule> ruleList = ruleBll.GetAvailableRuleList();
            //if (ruleList == null || ruleList.Count <= 0)
            //{
            //    return null;
            //}
            //YSWL.MALL.Model.Shop.Activity.ActivityRule firstRule = ruleList.FirstOrDefault(c => c.RuleId == 1);  //首单赠送
            //YSWL.MALL.Model.Shop.Activity.ActivityRule fullRule= ruleList.FirstOrDefault(c => c.RuleId == 2);   //满额赠送
            //YSWL.MALL.Model.Shop.Activity.ActivityRule couponRule = ruleList.FirstOrDefault(c => c.RuleId == 3); //优惠券赠送

            //firstRule = firstRule == null ? new Model.Shop.Activity.ActivityRule() : firstRule;
            //fullRule = fullRule == null ? new Model.Shop.Activity.ActivityRule() : fullRule;
            //couponRule = couponRule == null ? new Model.Shop.Activity.ActivityRule() : couponRule;
            bool storeIsInActivity = SysManage.ConfigSystem.GetBoolValueByCache("StoreIsInActivity");
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> AllInfoList = null;
            if (storeIsInActivity) //如果商家参加活动
            {
                AllInfoList = GetAllActs();
            }
            else
            {
                AllInfoList = GetAllActs(suppCartItems.Key);
            }
            if (AllInfoList == null)
            {
                return null;    
            }
            //活动列表 按照主键倒叙
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> actInfoList = AllInfoList.Where(c => c.Status == 1 && c.StartDate <= DateTime.Now && c.EndDate.AddDays(1) > DateTime.Now).ToList();

            if (actInfoList == null || actInfoList.Count <= 0)
            {
                return null;
            }
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> matchList =
                new List<YSWL.MALL.Model.Shop.Activity.ActivityInfo>();
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> matchFistAct = IsMatchFistOrder(suppCartItems, buyerId, actInfoList, amount);//首单
            if (matchFistAct != null)
            {
                matchList.AddRange(matchFistAct);
            }
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> matchFullAct = IsMatchFull(suppCartItems, actInfoList, amount);//满额赠送
            if (matchFullAct != null)
            {
                matchList.AddRange(matchFullAct);
            }
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> matchCouponAct = IsMatchCoupon(suppCartItems, actInfoList, amount);//优惠券赠送
            if (matchCouponAct != null)
            {
                matchList.AddRange(matchCouponAct);
            }

            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> matchFreeShippingAct = IsFreeShippingActiv(suppCartItems, actInfoList, amount);//包邮   IsFreeShippingActiv
            if (matchFreeShippingAct != null)
            {
                matchList.AddRange(matchFreeShippingAct);
            }
            return matchList;
        }

        /// <summary>
        /// 根据订单获取符合条件的活动 （不含运费）
        /// </summary>
        /// <param name="cartInfo"></param>
        /// <param name="buyerId"></param>
        /// <param name="couponAmount">优惠劵金额</param>
        public List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> GetActGiftList(YSWL.MALL.Model.Shop.Order.OrderInfo orderInfo, int buyerId, decimal couponAmount = 0)
        {
            if (orderInfo == null)
            {
                return null;
            }
            decimal amount = 0;//订单商品调整后总额-优惠劵面值 （不包含运费）
            decimal totalAdjustedPrice = 0;
            orderInfo.OrderItems.ForEach(info => totalAdjustedPrice += info.AdjustedPrice * info.Quantity);
            if (couponAmount > 0)
            {
                amount = totalAdjustedPrice - couponAmount;
                amount = amount > 0 ? amount : 0;//未防止优惠劵金额按大于商品总额
            }
            else
            {
                amount = totalAdjustedPrice;
            }
            //规则列表 按照优先级正序
            //List<YSWL.MALL.Model.Shop.Activity.ActivityRule> ruleList = ruleBll.GetAvailableRuleList();
            //if (ruleList == null || ruleList.Count <= 0)
            //{
            //    return null;
            //}
            //YSWL.MALL.Model.Shop.Activity.ActivityRule firstRule = ruleList.FirstOrDefault(c => c.RuleId == 1);  //首单赠送
            //YSWL.MALL.Model.Shop.Activity.ActivityRule fullRule= ruleList.FirstOrDefault(c => c.RuleId == 2);   //满额赠送
            //YSWL.MALL.Model.Shop.Activity.ActivityRule couponRule = ruleList.FirstOrDefault(c => c.RuleId == 3); //优惠券赠送

            //firstRule = firstRule == null ? new Model.Shop.Activity.ActivityRule() : firstRule;
            //fullRule = fullRule == null ? new Model.Shop.Activity.ActivityRule() : fullRule;
            //couponRule = couponRule == null ? new Model.Shop.Activity.ActivityRule() : couponRule;

            bool storeIsInActivity = SysManage.ConfigSystem.GetBoolValueByCache("StoreIsInActivity");
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> AllInfoList = null;
            if (storeIsInActivity)
            {
                AllInfoList = GetAllActs(-1);
                
            }
            else
            {
                AllInfoList = GetAllActs(orderInfo.SupplierId ?? -1);
            }
            
            if (AllInfoList == null)
            {
                return null;
            }
            //活动列表 按照主键倒叙
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> actInfoList = AllInfoList.Where(c => c.Status == 1 && c.StartDate <= DateTime.Now && c.EndDate.AddDays(1) > DateTime.Now).ToList();

            if (actInfoList == null || actInfoList.Count <= 0)
            {
                return null;
            }
            #region 暂时未实现
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> matchList =
                new List<YSWL.MALL.Model.Shop.Activity.ActivityInfo>();
            //List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> matchFistAct = IsMatchFistOrder(suppCartItems, buyerId, actInfoList, amount);//首单
            //if (matchFistAct != null)
            //{
            //    matchList.AddRange(matchFistAct);
            //}
            //List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> matchFullAct = IsMatchFull(suppCartItems, actInfoList, amount);//满额赠送
            //if (matchFullAct != null)
            //{
            //    matchList.AddRange(matchFullAct);
            //}
            //List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> matchCouponAct = IsMatchCoupon(suppCartItems, actInfoList, amount);//优惠券赠送
            //if (matchCouponAct != null)
            //{
            //    matchList.AddRange(matchCouponAct);
            //}
            #endregion

            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> matchFreeShippingAct = IsFreeShippingActiv(orderInfo, actInfoList, amount);//包邮   IsFreeShippingActiv
            if (matchFreeShippingAct != null)
            {
                matchList.AddRange(matchFreeShippingAct);
            }
            return matchList;
        }

        #region 过时的逻辑
        /// <summary>
        /// 根据购物车获取符合的活动 （不含运费）
        /// </summary>
        /// <param name="cartInfo"></param>
        /// <param name="buyerId"></param>
        /// <param name="couponAmount">优惠劵金额</param>
        /// <returns></returns>
        public List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> GetActGiftList(YSWL.MALL.Model.Shop.Products.ShoppingCartInfo cartInfo, int buyerId, decimal couponAmount = 0)
        {
            if (cartInfo == null)
            {
                return null;
            }
            decimal amount = -1;//订单商品调整后总额-优惠劵面值 （不包含运费）
            if (couponAmount > 0)
            {
                amount = cartInfo.TotalAdjustedPrice - couponAmount;
                amount = amount > 0 ? amount : 0;//未防止优惠劵金额按大于商品总额
            }

            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> AllInfoList = GetAllActs();
            if (AllInfoList == null)
            {
                return null;
            }
            //活动列表 按照主键倒叙
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> actInfoList = AllInfoList.Where(c => c.Status == 1 && c.StartDate <= DateTime.Now && c.EndDate.AddDays(1) > DateTime.Now).ToList();

            if (actInfoList == null || actInfoList.Count <= 0)
            {
                return null;
            }
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> matchList =
                new List<YSWL.MALL.Model.Shop.Activity.ActivityInfo>();
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> matchFistAct = IsMatchFistOrder_O(cartInfo, buyerId, actInfoList, amount);//首单
            if (matchFistAct != null)
            {
                matchList.AddRange(matchFistAct);
            }
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> matchFullAct = IsMatchFull_O(cartInfo, actInfoList, amount);//满额赠送
            if (matchFullAct != null)
            {
                matchList.AddRange(matchFullAct);
            }
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> matchCouponAct = IsMatchCoupon_O(cartInfo, actInfoList, amount);//优惠券赠送
            if (matchCouponAct != null)
            {
                matchList.AddRange(matchCouponAct);
            }

            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> matchFreeShippingAct = IsFreeShippingActiv_O(cartInfo, actInfoList, amount);//包邮   IsFreeShippingActiv
            if (matchFreeShippingAct != null)
            {
                matchList.AddRange(matchFreeShippingAct);
            }
            return matchList;
        }

        #region 过时的逻辑
        /// <summary>
        /// 是否满足收单的条件
        /// </summary>
        /// <param name="cartInfo"></param>
        /// <param name="BuyerId"></param>
        /// <returns></returns>
        private YSWL.MALL.Model.Shop.Activity.ActivityInfo IsMatchFistOrder_O(YSWL.MALL.Model.Shop.Products.ShoppingCartInfo cartInfo, int BuyerId, List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> firstActList)
        {
            if (firstActList == null || firstActList.Count == 0)
            {
                return null;
            }
            if (!orderBll.IsFirstOrder(BuyerId)) //是否是首单
            {
                return null;
            }
            //首单过滤满足金额  （暂时不支持指定商品）
            var matchActList = firstActList.Where(c => c.LimitPrice <= cartInfo.TotalAdjustedPrice).OrderByDescending(c => c.LimitPrice).ToList();
            return matchActList == null || matchActList.Count == 0 ? null : matchActList[0];
        }

        /// <summary>
        /// 是否满足满额赠送
        /// </summary>
        /// <param name="cartInfo"></param>
        /// <param name="BuyerId"></param>
        /// <returns></returns>
        private YSWL.MALL.Model.Shop.Activity.ActivityInfo IsMatchFull_O(YSWL.MALL.Model.Shop.Products.ShoppingCartInfo cartInfo, List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> fullActList)
        {
            if (fullActList == null || fullActList.Count == 0)
            {
                return null;
            }
            //满额赠送过滤满足金额  （暂时不支持指定商品）
            var matchActList = fullActList.Where(c => c.LimitPrice <= cartInfo.TotalAdjustedPrice).ToList();
            if (matchActList == null || matchActList.Count == 0)
            {
                return null;
            }
            matchActList = matchActList.OrderByDescending(c => c.LimitPrice).ToList();
            return matchActList == null || matchActList.Count == 0 ? null : matchActList[0];
        }


        /// <summary>
        /// 是否满足首单的条件
        /// </summary>
        /// <param name="cartInfo"></param>
        /// <param name="buyerId"></param>
        /// <returns></returns>
        private List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> IsMatchFistOrder_O(YSWL.MALL.Model.Shop.Products.ShoppingCartInfo cartInfo, int buyerId, List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> actInfoList, decimal amount)
        {

            //首单的活动列表
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> firstActList =
                actInfoList.Where(c => c.RuleId == 1).ToList();
            if (firstActList == null || firstActList.Count == 0)
            {
                return null;
            }
            if (!orderBll.IsFirstOrder(buyerId)) //是否是首单
            {
                return null;
            }
            return MatchActiv_O(cartInfo, firstActList, amount);
        }

        /// <summary>
        /// 是否满足满额赠送
        /// </summary>
        /// <param name="cartInfo"></param>
        /// <param name="BuyerId"></param>
        /// <returns></returns>
        private List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> IsMatchFull_O(YSWL.MALL.Model.Shop.Products.ShoppingCartInfo cartInfo, List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> actInfoList, decimal amount)
        {
            //满额赠送活动列表
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> fullActList =
             actInfoList.Where(c => c.RuleId == 2).ToList();

            if (fullActList == null || fullActList.Count == 0)
            {
                return null;
            }

            return MatchActiv_O(cartInfo, fullActList, amount);
        }

        /// <summary>
        /// 是否满足优惠券赠送
        /// </summary>
        /// <param name="cartInfo"></param>
        /// <param name="BuyerId"></param>
        /// <returns></returns>
        private List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> IsMatchCoupon_O(YSWL.MALL.Model.Shop.Products.ShoppingCartInfo cartInfo, List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> actInfoList, decimal amount)
        {
            //优惠券赠送活动列表
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> couponActList =
             actInfoList.Where(c => c.RuleId == 3).ToList();

            if (couponActList == null || couponActList.Count == 0)
            {
                return null;
            }
            return MatchActiv_O(cartInfo, couponActList, amount);
        }

        private List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> MatchActiv_O(
            YSWL.MALL.Model.Shop.Products.ShoppingCartInfo cartInfo,
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> matchActList, decimal amount)
        {
            if (amount < 0)
            {
                amount = cartInfo.TotalAdjustedPrice;
            }
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> matchAct = new List<YSWL.MALL.Model.Shop.Activity.ActivityInfo>();
            matchActList = matchActList.Where(c => c.LimitPrice <= amount).OrderByDescending(c => c.LimitPrice).ToList();//过滤限制价格高于购物车价格规则
            if (matchActList == null || matchActList.Count == 0)
            {
                return null;
            }
            var selfItems = cartInfo.Items;
            bool storeIsInActivity = SysManage.ConfigSystem.GetBoolValueByCache("StoreIsInActivity");
            if (!storeIsInActivity)
            {
                selfItems = cartInfo.Items.Where(c => c.SupplierId == null || c.SupplierId.Value <= 0).ToList();//排除第三方商品
            }


            if (selfItems != null && selfItems.Count == 0)
            {
                return null;
            }
            foreach (var item in matchActList)
            {
                //指定了商品SKU
                if (!String.IsNullOrWhiteSpace(item.BuySKU))
                {
                    List<ShoppingCartItem> matchCartItem = selfItems.Where(c => c.SKU == item.BuySKU).ToList();
                    if (matchCartItem.Count == 0)
                    {
                        continue;
                    }
                    //判断购买数量限制
                    int totalCount = 0;
                    matchCartItem.ForEach(info => totalCount += info.Quantity);
                    if (item.BuyCount > totalCount)
                    {
                        continue;
                    }
                    //判断价格区间
                    decimal totalPrice = 0;
                    matchCartItem.ForEach(info => totalPrice += info.AdjustedPrice * info.Quantity);
                    //指定条件的商品价格与最终交易的价格比较
                    totalPrice = totalPrice > amount ? amount : totalPrice;
                    if (item.LimitPrice > totalPrice || (item.LimitMaxPrice.HasValue && item.LimitMaxPrice < totalPrice))
                    {
                        continue;
                    }
                    matchAct.Add(item);
                    continue;
                }
                //指定了商品
                if (item.BuyProductId > 0 && String.IsNullOrWhiteSpace(item.BuySKU))
                {
                    List<ShoppingCartItem> matchCartItem = selfItems.Where(c => c.ProductId == item.BuyProductId).ToList();
                    if (matchCartItem == null || matchCartItem.Count == 0)
                    {
                        continue;
                    }

                    //判断购买数量限制
                    int totalCount = 0;
                    matchCartItem.ForEach(info => totalCount += info.Quantity);
                    if (item.BuyCount > totalCount)
                    {
                        continue;
                    }
                    //判断价格区间
                    decimal totalPrice = 0;
                    matchCartItem.ForEach(info => totalPrice += info.AdjustedPrice * info.Quantity);
                    //指定条件的商品价格与最终交易的价格比较
                    totalPrice = totalPrice > amount ? amount : totalPrice;
                    if (item.LimitPrice > totalPrice || (item.LimitMaxPrice.HasValue && item.LimitMaxPrice < totalPrice))
                    {
                        continue;
                    }
                    matchAct.Add(item);
                    continue;
                }
                //如果指定了分类
                if (item.BuyCategoryId > 0)
                {
                    //获取该分类下的商品及其子分类下的所有商品集合
                    YSWL.MALL.BLL.Shop.Products.ProductInfo productBll = new YSWL.MALL.BLL.Shop.Products.ProductInfo();
                    List<YSWL.MALL.Model.Shop.Products.ProductInfo> productInfos =
                     productBll.GetProductsByCid(item.BuyCategoryId);
                    List<ShoppingCartItem> matchCartItem = selfItems.Where(c => productInfos.Exists(f => f.ProductId == c.ProductId)).ToList();
                    if (matchCartItem == null || matchCartItem.Count == 0)
                    {
                        continue;
                    }

                    //判断购买数量限制
                    int totalCount = 0;
                    matchCartItem.ForEach(info => totalCount += info.Quantity);
                    if (item.BuyCount > totalCount)
                    {
                        continue;
                    }
                    //判断价格区间
                    decimal totalPrice = 0;
                    matchCartItem.ForEach(info => totalPrice += info.AdjustedPrice * info.Quantity);

                    //指定条件的商品价格与最终交易的价格比较
                    totalPrice = totalPrice > amount ? amount : totalPrice;
                    if (item.LimitPrice > totalPrice || (item.LimitMaxPrice.HasValue && item.LimitMaxPrice < totalPrice))
                    {
                        continue;
                    }
                    matchAct.Add(item);
                    continue;
                }
                //什么都没有指定，就直接判断数量以及价格区间

                //判断购买数量限制
                int totalCount2 = 0;
                selfItems.ForEach(info => totalCount2 += info.Quantity);
                if (item.BuyCount > totalCount2)
                {
                    continue;
                }
                //判断价格区间
                //decimal totalPrice2 = 0;
                //selfItems.ForEach(info => totalPrice2 += info.AdjustedPrice * info.Quantity);
                if (item.LimitPrice > amount || (item.LimitMaxPrice.HasValue && item.LimitMaxPrice < amount))
                {
                    continue;
                }
                matchAct.Add(item);
            }
            return matchAct;
        }
        /// <summary>
        /// 根据购物车获取符合包邮的活动
        /// </summary>
        /// <param name="cartInfo"></param>
        /// <param name="actInfoList"></param>
        /// <returns></returns>
        private List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> IsFreeShippingActiv_O(YSWL.MALL.Model.Shop.Products.ShoppingCartInfo cartInfo, List<Model.Shop.Activity.ActivityInfo> actInfoList, decimal amount)
        {
            //包邮活动列表
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> freeShippingActList = actInfoList.Where(c => c.RuleId == 4).ToList();

            if (freeShippingActList == null || freeShippingActList.Count == 0)
            {
                return null;
            }
            return MatchActiv_O(cartInfo, freeShippingActList, amount);
        }
        #endregion

        #region 新的逻辑
        /// <summary>
        /// 是否满足首单的条件
        /// </summary>
        /// <param name="cartInfo"></param>
        /// <param name="buyerId"></param>
        /// <returns></returns>
        private List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> IsMatchFistOrder(KeyValuePair<int, List<YSWL.MALL.Model.Shop.Products.ShoppingCartItem>> suppCartItems, int buyerId, List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> actInfoList, decimal amount)
        {

            //首单的活动列表
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> firstActList =
                actInfoList.Where(c => c.RuleId == 1).ToList();
            if (firstActList == null || firstActList.Count == 0)
            {
                return null;
            }
            if (!orderBll.IsFirstOrder(buyerId)) //是否是首单
            {
                return null;
            }
            return MatchActiv(suppCartItems, firstActList, amount);
        }

        /// <summary>
        /// 是否满足满额赠送
        /// </summary>
        /// <param name="cartInfo"></param>
        /// <param name="BuyerId"></param>
        /// <returns></returns>
        private List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> IsMatchFull(KeyValuePair<int, List<YSWL.MALL.Model.Shop.Products.ShoppingCartItem>> suppCartItems, List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> actInfoList, decimal amount)
        {
            //满额赠送活动列表
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> fullActList =
             actInfoList.Where(c => c.RuleId == 2).ToList();

            if (fullActList == null || fullActList.Count == 0)
            {
                return null;
            }

            return MatchActiv(suppCartItems, fullActList,amount);
        }

        /// <summary>
        /// 是否满足优惠券赠送
        /// </summary>
        /// <param name="cartInfo"></param>
        /// <param name="BuyerId"></param>
        /// <returns></returns>
        private List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> IsMatchCoupon(KeyValuePair<int, List<YSWL.MALL.Model.Shop.Products.ShoppingCartItem>> suppCartItems, List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> actInfoList, decimal amount)
        {
            //优惠券赠送活动列表
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> couponActList =
             actInfoList.Where(c => c.RuleId == 3).ToList();

            if (couponActList == null || couponActList.Count == 0)
            {
                return null;
            }
            return MatchActiv(suppCartItems, couponActList,amount);
        }

        private List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> MatchActiv(
            KeyValuePair<int, List<YSWL.MALL.Model.Shop.Products.ShoppingCartItem>> suppCartItems,
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> matchActList, decimal amount)
        {
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> matchAct = new List<YSWL.MALL.Model.Shop.Activity.ActivityInfo>();
            matchActList = matchActList.Where(c => c.LimitPrice <= amount).OrderByDescending(c => c.LimitPrice).ToList();//过滤限制价格高于购物车价格规则
            if (matchActList == null || matchActList.Count == 0)
            {
                return null;
            }
            var selfItems = suppCartItems.Value;
            //bool storeIsInActivity = SysManage.ConfigSystem.GetBoolValueByCache("StoreIsInActivity");
            //if (!storeIsInActivity)
            //{
            //    selfItems = suppCartItems.Value.Where(c => c.SupplierId == null || c.SupplierId.Value <= 0).ToList();//排除第三方商品
            //}

            if (selfItems != null && selfItems.Count == 0)
            {
                return null;
            }
            foreach (var item in matchActList)
            {
                //指定了商品SKU
                if (!String.IsNullOrWhiteSpace(item.BuySKU))
                {
                    List<ShoppingCartItem> matchCartItem = selfItems.Where(c => c.SKU == item.BuySKU).ToList();
                    if (matchCartItem.Count == 0)
                    {
                        continue;
                    }
                    //判断购买数量限制
                    int totalCount = 0;
                    matchCartItem.ForEach(info => totalCount += info.Quantity);
                    if (item.BuyCount > totalCount)
                    {
                        continue;
                    }
                    //判断价格区间
                    decimal totalPrice = 0;
                    matchCartItem.ForEach(info => totalPrice += info.AdjustedPrice * info.Quantity);
                    //指定条件的商品价格与最终交易的价格比较
                    totalPrice = totalPrice > amount ? amount : totalPrice;
                    if (item.LimitPrice > totalPrice || (item.LimitMaxPrice.HasValue && item.LimitMaxPrice < totalPrice))
                    {
                        continue;
                    }
                    matchAct.Add(item);
                    continue;
                }
                //指定了商品
                if (item.BuyProductId > 0 && String.IsNullOrWhiteSpace(item.BuySKU))
                {
                    List<ShoppingCartItem> matchCartItem = selfItems.Where(c => c.ProductId == item.BuyProductId).ToList();
                    if (matchCartItem == null || matchCartItem.Count == 0)
                    {
                        continue;
                    }

                    //判断购买数量限制
                    int totalCount = 0;
                    matchCartItem.ForEach(info => totalCount += info.Quantity);
                    if (item.BuyCount > totalCount)
                    {
                        continue;
                    }
                    //判断价格区间
                    decimal totalPrice = 0;
                    matchCartItem.ForEach(info => totalPrice += info.AdjustedPrice * info.Quantity);
                    //指定条件的商品价格与最终交易的价格比较
                    totalPrice = totalPrice > amount ? amount : totalPrice;
                    if (item.LimitPrice > totalPrice || (item.LimitMaxPrice.HasValue && item.LimitMaxPrice < totalPrice))
                    {
                        continue;
                    }
                    matchAct.Add(item);
                    continue;
                }
                //如果指定了分类
                if (item.BuyCategoryId > 0)
                {
                    //获取该分类下的商品及其子分类下的所有商品集合
                    YSWL.MALL.BLL.Shop.Products.ProductInfo productBll = new YSWL.MALL.BLL.Shop.Products.ProductInfo();
                    List<YSWL.MALL.Model.Shop.Products.ProductInfo> productInfos =
                     productBll.GetProductsByCid(item.BuyCategoryId);
                    List<ShoppingCartItem> matchCartItem = selfItems.Where(c => productInfos.Exists(f => f.ProductId == c.ProductId)).ToList();
                    if (matchCartItem == null || matchCartItem.Count == 0)
                    {
                        continue;
                    }

                    //判断购买数量限制
                    int totalCount = 0;
                    matchCartItem.ForEach(info => totalCount += info.Quantity);
                    if (item.BuyCount > totalCount)
                    {
                        continue;
                    }
                    //判断价格区间
                    decimal totalPrice = 0;
                    matchCartItem.ForEach(info => totalPrice += info.AdjustedPrice * info.Quantity);

                    //指定条件的商品价格与最终交易的价格比较
                    totalPrice = totalPrice > amount ? amount : totalPrice;
                    if (item.LimitPrice > totalPrice || (item.LimitMaxPrice.HasValue && item.LimitMaxPrice < totalPrice))
                    {
                        continue;
                    }
                    matchAct.Add(item);
                    continue;
                }
                //什么都没有指定，就直接判断数量以及价格区间

                //判断购买数量限制
                int totalCount2 = 0;
                selfItems.ForEach(info => totalCount2 += info.Quantity);
                if (item.BuyCount > totalCount2)
                {
                    continue;
                }
                //判断价格区间
                //decimal totalPrice2 = 0;
                //selfItems.ForEach(info => totalPrice2 += info.AdjustedPrice * info.Quantity);
                if (item.LimitPrice > amount || (item.LimitMaxPrice.HasValue && item.LimitMaxPrice < amount))
                {
                    continue;
                }
                matchAct.Add(item);
            }
            return matchAct;
        }

        /// <summary>
        /// 根据订单匹配活动
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <param name="matchActList"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        private List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> MatchActiv(
            YSWL.MALL.Model.Shop.Order.OrderInfo orderInfo,
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> matchActList, decimal amount)
        {
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> matchAct = new List<YSWL.MALL.Model.Shop.Activity.ActivityInfo>();
            matchActList = matchActList.Where(c => c.LimitPrice <= amount).OrderByDescending(c => c.LimitPrice).ToList();//过滤限制价格高于购物车价格规则
            if (matchActList == null || matchActList.Count == 0)
            {
                return null;
            }
            var selfItems = orderInfo.OrderItems;
            //bool storeIsInActivity = SysManage.ConfigSystem.GetBoolValueByCache("StoreIsInActivity");
            //if (!storeIsInActivity)
            //{
            //    selfItems = suppCartItems.Value.Where(c => c.SupplierId == null || c.SupplierId.Value <= 0).ToList();//排除第三方商品
            //}

            if (selfItems != null && selfItems.Count == 0)
            {
                return null;
            }
            foreach (var item in matchActList)
            {
                //指定了商品SKU
                if (!String.IsNullOrWhiteSpace(item.BuySKU))
                {
                    var matchCartItem = selfItems.Where(c => c.SKU == item.BuySKU).ToList();
                    if (matchCartItem.Count == 0)
                    {
                        continue;
                    }
                    //判断购买数量限制
                    int totalCount = 0;
                    matchCartItem.ForEach(info => totalCount += info.Quantity);
                    if (item.BuyCount > totalCount)
                    {
                        continue;
                    }
                    //判断价格区间
                    decimal totalPrice = 0;
                    matchCartItem.ForEach(info => totalPrice += info.AdjustedPrice * info.Quantity);
                    //指定条件的商品价格与最终交易的价格比较
                    totalPrice = totalPrice > amount ? amount : totalPrice;
                    if (item.LimitPrice > totalPrice || (item.LimitMaxPrice.HasValue && item.LimitMaxPrice < totalPrice))
                    {
                        continue;
                    }
                    matchAct.Add(item);
                    continue;
                }
                //指定了商品
                if (item.BuyProductId > 0 && String.IsNullOrWhiteSpace(item.BuySKU))
                {
                    var matchCartItem = selfItems.Where(c => c.ProductId == item.BuyProductId).ToList();
                    if (matchCartItem == null || matchCartItem.Count == 0)
                    {
                        continue;
                    }

                    //判断购买数量限制
                    int totalCount = 0;
                    matchCartItem.ForEach(info => totalCount += info.Quantity);
                    if (item.BuyCount > totalCount)
                    {
                        continue;
                    }
                    //判断价格区间
                    decimal totalPrice = 0;
                    matchCartItem.ForEach(info => totalPrice += info.AdjustedPrice * info.Quantity);
                    //指定条件的商品价格与最终交易的价格比较
                    totalPrice = totalPrice > amount ? amount : totalPrice;
                    if (item.LimitPrice > totalPrice || (item.LimitMaxPrice.HasValue && item.LimitMaxPrice < totalPrice))
                    {
                        continue;
                    }
                    matchAct.Add(item);
                    continue;
                }
                //如果指定了分类
                if (item.BuyCategoryId > 0)
                {
                    //获取该分类下的商品及其子分类下的所有商品集合
                    YSWL.MALL.BLL.Shop.Products.ProductInfo productBll = new YSWL.MALL.BLL.Shop.Products.ProductInfo();
                    List<YSWL.MALL.Model.Shop.Products.ProductInfo> productInfos =
                     productBll.GetProductsByCid(item.BuyCategoryId);
                    var matchCartItem = selfItems.Where(c => productInfos.Exists(f => f.ProductId == c.ProductId)).ToList();
                    if (matchCartItem == null || matchCartItem.Count == 0)
                    {
                        continue;
                    }

                    //判断购买数量限制
                    int totalCount = 0;
                    matchCartItem.ForEach(info => totalCount += info.Quantity);
                    if (item.BuyCount > totalCount)
                    {
                        continue;
                    }
                    //判断价格区间
                    decimal totalPrice = 0;
                    matchCartItem.ForEach(info => totalPrice += info.AdjustedPrice * info.Quantity);

                    //指定条件的商品价格与最终交易的价格比较
                    totalPrice = totalPrice > amount ? amount : totalPrice;
                    if (item.LimitPrice > totalPrice || (item.LimitMaxPrice.HasValue && item.LimitMaxPrice < totalPrice))
                    {
                        continue;
                    }
                    matchAct.Add(item);
                    continue;
                }
                //什么都没有指定，就直接判断数量以及价格区间

                //判断购买数量限制
                int totalCount2 = 0;
                selfItems.ForEach(info => totalCount2 += info.Quantity);
                if (item.BuyCount > totalCount2)
                {
                    continue;
                }
                //判断价格区间
                //decimal totalPrice2 = 0;
                //selfItems.ForEach(info => totalPrice2 += info.AdjustedPrice * info.Quantity);
                if (item.LimitPrice > amount || (item.LimitMaxPrice.HasValue && item.LimitMaxPrice < amount))
                {
                    continue;
                }
                matchAct.Add(item);
            }
            return matchAct;
        }

        #endregion
        /// <summary>
        /// 获得可用活动数据列表从缓存
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> GetActInfos()
        {
            string CacheKey = "GetAvailable_ActivityInfo";
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    DataSet ds = dal.GetList(-1, "  Status=1  ", " ActivityId desc  ");
                    objModel = DataTableToList(ds.Tables[0]);
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (List<YSWL.MALL.Model.Shop.Activity.ActivityInfo>)objModel;
        }

        public List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> GetAllActs()
        {
            DataSet ds = dal.GetList(-1, "  Status=1  ", " ActivityId desc  ");
            return DataTableToList(ds.Tables[0]);
        }

       
        /// <summary>
        /// 获得包邮活动数据列表
        /// </summary>
        /// <returns></returns>
        public List<Model.Shop.Activity.ActivityInfo> GetFreeShippingActs()
        {
            DataSet ds = dal.GetList(-1, "  Status=1  and  RuleId=4  ", " ActivityId desc  ");
            return DataTableToList(ds.Tables[0]);
        }

        #region  获取赠品商品

        public static List<YSWL.MALL.Model.Shop.Products.ProductInfo> GetActProList(List<Model.Shop.Activity.ActivityInfo> actInfoList,int regionId)
        {
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> actProductList =
                new List<YSWL.MALL.Model.Shop.Products.ProductInfo>();
            BLL.Shop.Products.ProductInfo productManage = new BLL.Shop.Products.ProductInfo();
            BLL.Shop.Products.SKUInfo skuManage = new BLL.Shop.Products.SKUInfo();
            //首先合并同一个商品 (暂时不考虑SKU)
            List<Model.Shop.Activity.ActivityInfo> activityInfos = new List<Model.Shop.Activity.ActivityInfo>();//合并后的赠品对象
            foreach (var item in actInfoList)
            {
                if (item.RuleId != 1  &&  item.RuleId != 2)
                {
                    continue;
                }
                //如果已经存在该赠品
                if (activityInfos.Exists(c => c.ProductId == item.ProductId))
                {
                    Model.Shop.Activity.ActivityInfo extModel =
                        activityInfos.FirstOrDefault(c => c.ProductId == item.ProductId);
                    if (extModel != null)
                    {
                        extModel.Count += item.Count;
                    }
                }
                else
                {
                    activityInfos.Add(item);
                }
            }

            int stock = 0;
            //是否开启多分仓库存
            bool IsMultiDepot = YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot();
            foreach (var actInfoModel in activityInfos)
            {

                ProductInfo productInfo = productManage.GetModel(actInfoModel.ProductId);
                if (productInfo != null && productInfo.SaleStatus==1)
                {
                    productInfo.Count = actInfoModel.Count;
                    productInfo.SalePrice = actInfoModel.SalePrice;
                    productInfo.SkuInfos = skuManage.GetProductSkuInfo(actInfoModel.ProductId);
                    if (productInfo.SkuInfos != null && productInfo.SkuInfos.Count > 0)
                    {
                            List<SKUItem> listSkuItems = skuManage.GetSKUItemsBySkuId(productInfo.SkuInfos[0].SkuId);
                            if (listSkuItems != null && listSkuItems.Count > 0)
                            {
                                productInfo.SkuValues = new string[listSkuItems.Count];
                                int index = 0;
                                listSkuItems.ForEach(xx =>
                                {
                                    productInfo.SkuValues[index++] = xx.ValueStr;
                                });
                            }
                            stock = productInfo.SkuInfos[0].Stock;

                            if (productInfo.SupplierId <= 0 && IsMultiDepot)
                            {
                                //开启了多仓对接，就需要对接仓库库存,且不是商家商品
                                stock = YSWL.MALL.BLL.Shop.Products.StockHelper.GetSkuStockByCache(productInfo.SkuInfos[0].SKU, regionId, productInfo.SupplierId);
                            }

                            if (stock >= actInfoModel.Count)
                            {
                                actProductList.Add(productInfo);
                            }  
                    }
                }
            }
            return actProductList;
        }
        public static List<YSWL.MALL.Model.Shop.Products.ProductInfo> GetActProList(List<Model.Shop.Activity.ActivityInfo> actInfoList,int regionId, out List<YSWL.MALL.Model.Shop.Products.ProductInfo> notStockProd)
        {
            notStockProd = new List<YSWL.MALL.Model.Shop.Products.ProductInfo>();
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> actProductList =
                new List<YSWL.MALL.Model.Shop.Products.ProductInfo>();
            BLL.Shop.Products.ProductInfo productManage = new BLL.Shop.Products.ProductInfo();
            BLL.Shop.Products.SKUInfo skuManage = new BLL.Shop.Products.SKUInfo();
            //首先合并同一个商品 (暂时不考虑SKU)
            List<Model.Shop.Activity.ActivityInfo> activityInfos = new List<Model.Shop.Activity.ActivityInfo>();//合并后的赠品对象
            foreach (var item in actInfoList)
            {
                if (item.RuleId != 1 && item.RuleId != 2)
                {
                    continue;
                }
                //如果已经存在该赠品
                if (activityInfos.Exists(c => c.ProductId == item.ProductId))
                {
                    Model.Shop.Activity.ActivityInfo extModel =
                        activityInfos.FirstOrDefault(c => c.ProductId == item.ProductId);
                    if (extModel != null)
                    {
                        extModel.Count += item.Count;
                    }
                }
                else
                {
                    activityInfos.Add(item);
                }
            }
            int stock = 0;
            //是否开启多分仓库存
            bool IsMultiDepot = YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot();
            foreach (var actInfoModel in activityInfos)
            {
                ProductInfo productInfo = productManage.GetModel(actInfoModel.ProductId);
                if (productInfo != null && productInfo.SaleStatus==1)
                {
                    productInfo.Count = actInfoModel.Count;
                    productInfo.SalePrice = actInfoModel.SalePrice;
                    productInfo.SkuInfos = skuManage.GetProductSkuInfo(actInfoModel.ProductId);
                    if (productInfo.SkuInfos != null && productInfo.SkuInfos.Count > 0)
                    {
                        List<SKUItem> listSkuItems =
                            skuManage.GetSKUItemsBySkuId(productInfo.SkuInfos[0].SkuId);
                        if (listSkuItems != null && listSkuItems.Count > 0)
                        {
                            productInfo.SkuValues = new string[listSkuItems.Count];
                            int index = 0;
                            listSkuItems.ForEach(xx =>
                            {
                                productInfo.SkuValues[index++] = xx.ValueStr;
                            });
                        }
                        stock = productInfo.SkuInfos[0].Stock;
                       
                        if (productInfo.SupplierId<=0 &&  IsMultiDepot)
                        { 
                            //开启了多仓对接，就需要对接仓库库存,且不是商家商品
                            stock=YSWL.MALL.BLL.Shop.Products.StockHelper.GetSkuStockByCache(productInfo.SkuInfos[0].SKU, regionId, productInfo.SupplierId);
                        }
                        if ( stock>= actInfoModel.Count)
                        {
                            actProductList.Add(productInfo);
                        }
                        else
                        {
                            //无库存
                            notStockProd.Add(productInfo);
                        }
                    }
                }
            }
            return actProductList;
        }
        #endregion

        #region 获取赠品优惠券
        public static List<YSWL.MALL.Model.Shop.Coupon.CouponRule> GetActCoupon(List<Model.Shop.Activity.ActivityInfo> actInfoList)
        {
            actInfoList = actInfoList.Where(c => c.RuleId == 3).ToList();
            YSWL.MALL.BLL.Shop.Coupon.CouponRule ruleBll = new CouponRule();
            List<YSWL.MALL.Model.Shop.Coupon.CouponRule> couponList = new List<YSWL.MALL.Model.Shop.Coupon.CouponRule>();
            foreach (var actInfo in actInfoList)
            {
                if (actInfo != null && actInfo.CpRuleId > 0)
                {
                    YSWL.MALL.Model.Shop.Coupon.CouponRule ruleModel = ruleBll.GetModel(actInfo.CpRuleId);
                    if (ruleModel != null)
                    {
                        ruleModel.SendCount = actInfo.Count;
                        couponList.Add(ruleModel);
                    }
                }
            }
            return couponList;
        }
        #endregion

        #region 判断该商品满足哪些赠送活动

        public List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> GetActInfos(long productId, int suppId)
        {  
            YSWL.MALL.BLL.Shop.Products.ProductCategories proCateBll = new YSWL.MALL.BLL.Shop.Products.ProductCategories();
            //获取所有可用的活动列表
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> AllInfos = GetActInfos(suppId);
            if (AllInfos == null)
            {
                return null;
            }
            //满足条件的活动列表
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> matchInfos =
                new List<YSWL.MALL.Model.Shop.Activity.ActivityInfo>();
            //首先加上没有指定任何分类或者商品的活动条件
            matchInfos.AddRange(AllInfos.Where(c => c.BuyCategoryId == 0).ToList());

            //获取该商品的分类 (考虑一个商品可以属于多个分类)
            List<YSWL.MALL.Model.Shop.Products.ProductCategories> proCategory = proCateBll.GetModelList(productId);
            foreach (var item in proCategory)
            {
                //获取指定分类的商品
                var cates = item.CategoryPath.Split('|');
                var cateActivs = AllInfos.Where(
                     c =>
                     (c.BuyProductId == null || c.BuyProductId.Value == 0) &&
                     (c.BuyCategoryId == item.CategoryId || cates.Contains(c.BuyCategoryId.ToString())));
                matchInfos.AddRange(cateActivs);
            }
            //添加指定该商品的活动
            var proActivs = AllInfos.Where(c => c.BuyProductId.HasValue && c.BuyProductId == productId);
            
            matchInfos.AddRange(proActivs);

            if (matchInfos == null || matchInfos.Count == 0)
            {
                return null;
            }
            return matchInfos.Where(o => o.StartDate <= System.DateTime.Now).Where(o => o.EndDate.AddDays(1) > System.DateTime.Now).ToList();
        }
        #endregion
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long ProductId)
        {
            return dal.Exists(ProductId);
        }
        /// <summary>
        /// 生成促销活动相关数据
        /// </summary>
        /// <param name="mainOrder"></param>
        /// <param name="actInfoList"></param>
        public void GenerateData(Model.Shop.Order.OrderInfo mainOrder, List<Model.Shop.Activity.ActivityInfo> actInfoList)
        {
            if (actInfoList == null || actInfoList.Count == 0)
            {
                return;
            }
            long orderId = mainOrder.OrderId;
            string orderCode = mainOrder.OrderCode;
            #region 生成赠品优惠券
            List<YSWL.MALL.Model.Shop.Coupon.CouponRule> activCoupon = YSWL.MALL.BLL.Shop.Activity.ActivityInfo.GetActCoupon(actInfoList);
            if (activCoupon != null && activCoupon.Count>0)
            {
                foreach (var item in activCoupon)
                {
                    //对应的时间
                    item.StartDate = DateTime.Now.AddDays(item.DeferDay);
                    if (item.AvaType == 1)//次月可用
                    {
                        if (item.StartDate.Month == DateTime.Now.Month)
                        {
                            item.StartDate =
                                Common.Globals.SafeDateTime(DateTime.Now.AddMonths(1).ToString("yyyy-MM") + "-1",
                                                            DateTime.Now);
                        }
                    }
                    //是否合并
                    bool IsMerge = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_Activity_IsMerge");
                    if (IsMerge)
                    {
                        item.LimitPrice = item.LimitPrice * item.SendCount;
                        item.CouponPrice = item.CouponPrice * item.SendCount;
                        infoBll.GenActCoupon(item, mainOrder.BuyerID, orderCode,orderId);
                    }
                    else
                    {
                        for (int i = 0; i < item.SendCount; i++)
                        {
                            infoBll.GenActCoupon(item, mainOrder.BuyerID, orderCode, orderId);
                        }
                    }
                }
            }
            #endregion

            #region  生成参与活动详情
            YSWL.MALL.Model.Shop.Activity.ActivityDetail actiDetailModel = null;
            foreach (Model.Shop.Activity.ActivityInfo item in actInfoList)
            {
                actiDetailModel = new Model.Shop.Activity.ActivityDetail();
                actiDetailModel.ActivityId = item.ActivityId;
                actiDetailModel.RuleId = item.RuleId;
                actiDetailModel.SupplierId = item.SupplierId;
                actiDetailModel.OrderId = orderId;
                actiDetailModel.OrderCode = orderCode;
                actiDetailModel.CreateDate = DateTime.Now;
                actiDetailModel.UserId = mainOrder.BuyerID;
                actiDetailModel.UserName = mainOrder.BuyerName;
                bllActiDetail.Add(actiDetailModel);
            }
            #endregion
        }

        /// <summary>
        /// 根据购物车获取符合包邮的活动
        /// </summary>
        /// <param name="cartInfo"></param>
        /// <param name="actInfoList"></param>
        /// <returns></returns>
        private List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> IsFreeShippingActiv(KeyValuePair<int, List<YSWL.MALL.Model.Shop.Products.ShoppingCartItem>> suppCartItems, List<Model.Shop.Activity.ActivityInfo> actInfoList, decimal amount)
        {
            //包邮活动列表
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> freeShippingActList = actInfoList.Where(c => c.RuleId == 4).ToList();

            if (freeShippingActList == null || freeShippingActList.Count == 0)
            {
                return null;
            }
            return MatchActiv(suppCartItems, freeShippingActList,amount);
        }

        /// <summary>
        /// 根据订单获取包邮的活动
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <param name="actInfoList"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        private List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> IsFreeShippingActiv(YSWL.MALL.Model.Shop.Order.OrderInfo orderInfo , List<Model.Shop.Activity.ActivityInfo> actInfoList, decimal amount)
        {
            //包邮活动列表
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> freeShippingActList = actInfoList.Where(c => c.RuleId == 4).ToList();

            if (freeShippingActList == null || freeShippingActList.Count == 0)
            {
                return null;
            }
            return MatchActiv(orderInfo, freeShippingActList, amount);
        }


        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="suppId"></param>
        /// <returns></returns>
        public ViewModel.Shop.ActivityModel GetActivityList(long pId = 0, int suppId = 0)
        {
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> actInfoList = GetActInfos(pId, suppId);
            if (actInfoList == null || actInfoList.Count == 0)
            {
                return null;
            }
            ViewModel.Shop.ActivityModel activityModel = new ViewModel.Shop.ActivityModel();
            //满额送
            activityModel.Full = actInfoList.Where(o => o.RuleId == 2).ToList();

            //首单[满额]送             
            activityModel.First = actInfoList.Where(o => o.RuleId == 1).ToList();

            //送优惠劵
            activityModel.Coupon = actInfoList.Where(o => o.RuleId == 3).ToList();

            //包邮
            activityModel.FreeShipping = actInfoList.Where(o => o.RuleId == 4).ToList();

            return activityModel;
        }

        /// <summary>
        /// 获取活动赠送列表
        /// </summary>
        /// <param name="cartInfo"></param>
        /// <param name="buyerID"></param>
        /// <param name="coupPrice"></param>
        /// <param name="isFreeShippingActiv"></param>
        /// <returns></returns>
        public ViewModel.Shop.ActicityGiveList GetActivityGiveList(ShoppingCartInfo cartInfo, int buyerID, decimal coupPrice, int regionId, out bool isFreeShippingActiv)
        {
            isFreeShippingActiv = false;
            List<Model.Shop.Activity.ActivityInfo> actInfoList = GetActGiftList(cartInfo, buyerID, coupPrice);
            if (actInfoList == null || actInfoList.Count == 0)
            {
                return null;
            }
            //获取包邮活动
            List<Model.Shop.Activity.ActivityInfo> freeShippingList = actInfoList.Where(p => p.RuleId == 4).ToList();
            if (freeShippingList != null && freeShippingList.Count > 0)
            {
                //是否包邮
                isFreeShippingActiv = true;
            }
            YSWL.MALL.ViewModel.Shop.ActicityGiveList model = new ActicityGiveList();
            List<ProductInfo> notStockActivProd;
            model.ActProdList = GetActProList(actInfoList, regionId, out notStockActivProd);
            model.NotStockActProdList = notStockActivProd;
            model.ActCouponList = GetActCoupon(actInfoList);
            return model;
        }
        /// <summary>
        /// 获取活动赠送列表
       /// </summary>
        /// <param name="list"></param>
       /// <param name="buyerID"></param>
       /// <param name="coupPrice"></param>
        /// <param name="isFreeShippingActiv"></param>
       /// <returns></returns>
        public ViewModel.Shop.ActicityGiveList GetActivityGiveList(KeyValuePair<int, List<YSWL.MALL.Model.Shop.Products.ShoppingCartItem>> suppCartItems, int buyerID, decimal coupPrice, int regionId, out bool isFreeShippingActiv)
       {
           isFreeShippingActiv = false;
           List<Model.Shop.Activity.ActivityInfo> actInfoList = GetActGiftList(suppCartItems, buyerID, coupPrice);
            if (actInfoList == null || actInfoList.Count == 0)
            {
                return null;
            }
            //获取包邮活动
            List<Model.Shop.Activity.ActivityInfo> freeShippingList = actInfoList.Where(p => p.RuleId == 4).ToList();
            if (freeShippingList != null && freeShippingList.Count > 0)
            {
                //是否包邮
                isFreeShippingActiv = true;
            }
            YSWL.MALL.ViewModel.Shop.ActicityGiveList model = new ActicityGiveList();
            List<ProductInfo> notStockActivProd;
            model.ActProdList =GetActProList(actInfoList, regionId,out notStockActivProd);
            model.NotStockActProdList = notStockActivProd;
            model.ActCouponList =GetActCoupon(actInfoList);
            model.SupplierId = suppCartItems.Key;
            return model;
        }


        /// <summary>
        /// 获取活动赠送列表
        /// </summary>
        /// <param name="list"></param>
        /// <param name="buyerID"></param>
        /// <param name="coupPrice"></param>
        /// <param name="isFreeShippingActiv"></param>
        /// <returns></returns>
        public ViewModel.Shop.ActicityGiveList GetActivityGiveList(YSWL.MALL.Model.Shop.Order.OrderInfo orderInfo, int buyerID, decimal coupPrice, int regionId, out bool isFreeShippingActiv)
        {
            isFreeShippingActiv = false;
            List<Model.Shop.Activity.ActivityInfo> actInfoList = GetActGiftList(orderInfo, buyerID, coupPrice);
            if (actInfoList == null || actInfoList.Count == 0)
            {
                return null;
            }
            //获取包邮活动
            List<Model.Shop.Activity.ActivityInfo> freeShippingList = actInfoList.Where(p => p.RuleId == 4).ToList();
            if (freeShippingList != null && freeShippingList.Count > 0)
            {
                //是否包邮
                isFreeShippingActiv = true;
            }
            YSWL.MALL.ViewModel.Shop.ActicityGiveList model = new ActicityGiveList();
            List<ProductInfo> notStockActivProd;
            model.ActProdList = GetActProList(actInfoList, regionId, out notStockActivProd);
            model.NotStockActProdList = notStockActivProd;
            model.ActCouponList = GetActCoupon(actInfoList);
            model.SupplierId = orderInfo.SupplierId ?? -1;
            return model;
        }

        /// <summary>
        /// 根据订单获取是否包邮
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <param name="buyerID"></param>
        /// <param name="coupPrice"></param>
        /// <returns></returns>
        public bool IsFreeShippingActiv(YSWL.MALL.Model.Shop.Order.OrderInfo orderInfo, int buyerID, decimal coupPrice)
        {
            bool isFreeShippingActiv = false;
            List<Model.Shop.Activity.ActivityInfo> actInfoList = GetActGiftList(orderInfo, buyerID, coupPrice);
            if (actInfoList == null || actInfoList.Count == 0)
            {
                return false;
            }
            //获取包邮活动
            List<Model.Shop.Activity.ActivityInfo> freeShippingList = actInfoList.Where(p => p.RuleId == 4).ToList();
            if (freeShippingList != null && freeShippingList.Count > 0)
            {
                //是否包邮
                isFreeShippingActiv = true;
            }
            return isFreeShippingActiv;
        }

        /// <summary>
        /// 根据购物车项获取是否包邮
        /// </summary>
        /// <param name="suppCartItems"></param>
        /// <param name="buyerId"></param>
        /// <param name="coupPrice"></param>
        /// <returns></returns>
        public bool IsFreeShippByCareItem(KeyValuePair<int, List<YSWL.MALL.Model.Shop.Products.ShoppingCartItem>> suppCartItems, int buyerId)
        {
            List<Model.Shop.Activity.ActivityInfo> actInfoList = GetActGiftList(suppCartItems, buyerId);
            if (actInfoList == null || actInfoList.Count == 0)
            {
                return false;
            }
            //获取包邮活动
            List<Model.Shop.Activity.ActivityInfo> freeShippingList = actInfoList.Where(p => p.RuleId == 4).ToList();
            if (freeShippingList != null && freeShippingList.Count > 0)
            {
                //是否包邮
                return true;
            }
            return false;
        }


        /// <summary>
        /// 获得商家可用活动数据列表从缓存
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> GetActInfos(int suppId)
        {
            if (suppId <= 0) {
                suppId = 0;
            }
            string CacheKey = "GetAvailable_ActivityInfo_Supp_" + suppId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    List <YSWL.MALL.Model.Shop.Activity.ActivityInfo > list= GetActInfos();
                    if (list != null) {
                        list = list.Where(o => o.SupplierId == suppId).ToList();
                    }
                    objModel = list;
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (List<YSWL.MALL.Model.Shop.Activity.ActivityInfo>)objModel;
        }
        /// <summary>
        /// 清理缓存
        /// </summary>
        /// <param name="suppId">商家id</param>
        public void ClearCache(int suppId)
        {
            if (suppId <= 0)
            {
                suppId = 0;
            }
            Common.DataCache.DeleteCache("GetAvailable_ActivityInfo_Supp_"+suppId);
            Common.DataCache.DeleteCache("GetAvailable_ActivityInfo");
        }
        public List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> GetAllActs(int suppId)
        {
           StringBuilder  where =new StringBuilder ();
            where.Append("  Status=1  ");
            if (suppId > 0)
            {
                where.AppendFormat(" and SupplierId={0} ", suppId);
            }
            else {
                where.Append(" and  SupplierId<=0 ");
            }
            DataSet ds = dal.GetList(-1, where.ToString(), " ActivityId desc  ");
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获取活动赠送列表
        /// </summary>
        /// <param name="cartInfo"></param>
        /// <param name="buyerID"></param>
        /// <param name="coupPrice"></param>
        /// <returns></returns>
        public ViewModel.Shop.ActicityGiveList GetActivityGiveList(ShoppingCartInfo cartInfo, int buyerID, decimal coupPrice, int regionId)
        {
            List<Model.Shop.Activity.ActivityInfo> actInfoList = GetActGiftList(cartInfo, buyerID, coupPrice);
            if (actInfoList == null || actInfoList.Count == 0)
            {
                return null;
            }
            YSWL.MALL.ViewModel.Shop.ActicityGiveList model = new ActicityGiveList();
            List<ProductInfo> notStockActivProd;
            model.ActProdList = GetActProList(actInfoList, regionId, out notStockActivProd);
            model.NotStockActProdList = notStockActivProd;
            model.ActCouponList = GetActCoupon(actInfoList);
            return model;
        }


        public List<int> GetRuleIds(long pId = 0, int suppId = 0)
        {
            List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> actInfoList = GetActInfos(pId, suppId);
            if (actInfoList == null || actInfoList.Count == 0)
            {
                return null;
            }
            ViewModel.Shop.ActivityModel activityModel = new ViewModel.Shop.ActivityModel();

            List<int> ruleIds = actInfoList.Select(c => c.RuleId).Distinct().ToList();

            return ruleIds;
        }
        #endregion
        #endregion

    }
}

