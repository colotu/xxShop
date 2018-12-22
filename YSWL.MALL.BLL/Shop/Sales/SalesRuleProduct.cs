/**
* SalesRuleProduct.cs
*
* 功 能： N/A
* 类 名： SalesRuleProduct
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/6/8 18:54:58   N/A    初版
*
* Copyright (c) 2012-2013 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using YSWL.MALL.BLL.Members;
using YSWL.Common;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Shop.Sales;
using YSWL.MALL.Model.Shop.Products;
using YSWL.MALL.ViewModel.Shop;

namespace YSWL.MALL.BLL.Shop.Sales
{
    /// <summary>
    /// SalesRuleProduct
    /// </summary>
    public partial class SalesRuleProduct
    {
        private readonly ISalesRuleProduct dal = DAShopSales.CreateSalesRuleProduct();
        public SalesRuleProduct()
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
        public bool Exists(int RuleId, long ProductId)
        {
            return dal.Exists(RuleId, ProductId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(YSWL.MALL.Model.Shop.Sales.SalesRuleProduct model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.Shop.Sales.SalesRuleProduct model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int RuleId, long ProductId)
        {
            return dal.Delete(RuleId, ProductId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.Sales.SalesRuleProduct GetModel(int RuleId, long ProductId)
        {

            return dal.GetModel(RuleId, ProductId);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public YSWL.MALL.Model.Shop.Sales.SalesRuleProduct GetModelByCache(int RuleId, long ProductId)
        {

            string CacheKey = "SalesRuleProductModel-R" + RuleId+"P" + ProductId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(RuleId, ProductId);
                    if (objModel != null)
                    {
                    int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.Shop.Sales.SalesRuleProduct)objModel;
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
        public List<YSWL.MALL.Model.Shop.Sales.SalesRuleProduct> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Sales.SalesRuleProduct> DataTableToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Shop.Sales.SalesRuleProduct> modelList = new List<YSWL.MALL.Model.Shop.Sales.SalesRuleProduct>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Shop.Sales.SalesRuleProduct model;
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
        /// 删除 数据
        /// </summary>
        public bool DeleteByRule(int RuleId)
        {
            return dal.DeleteByRule(RuleId);
        }

        public List<YSWL.MALL.Model.Shop.Sales.SalesRuleProduct> GetRuleProList(long productId)
        {
            DataSet ds = dal.GetRuleProList(productId);
            return DataTableToList(ds.Tables[0]);
        }

        public DataSet GetRuleProducts(int ruleId, string categoryId, string pName)
        {
            StringBuilder strWhere = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(pName))
            {
                strWhere.AppendFormat(" AND ProductName LIKE '%{0}%'", Common.InjectionFilter.SqlFilter(pName));
            }
            if (!string.IsNullOrWhiteSpace(categoryId))
            {
                strWhere.AppendFormat("AND ProductId IN (SELECT DISTINCT ProductId FROM  PMS_ProductCategories PC WHERE (CategoryPath LIKE (SELECT Path FROM PMS_Categories WHERE CategoryId={0})+'|%'  or CategoryId={0}))", categoryId);
            }
            return dal.GetRuleProducts(ruleId, strWhere.ToString());
        }

        /// <summary>
        /// 批量删除数据 （这个是联合主键，需要采用特殊的方式处理）
        /// </summary>
        /// <param name="idlist"></param>
        /// <returns></returns>
        public bool DeleteList(string idlist)
        {
            var ids_arr = idlist.Split(',');
            bool IsSuccess = true;
            foreach (var idStr in ids_arr)
            {
                if (!String.IsNullOrWhiteSpace(idStr))
                {
                    int ruleId = Common.Globals.SafeInt(idStr.Split('|')[0], 0);
                    long productId = Common.Globals.SafeLong(idStr.Split('|')[1], 0);
                    if (!Delete(ruleId, productId))
                    {
                        IsSuccess = false;
                    }
                }
            }
            return IsSuccess;
        }


        public YSWL.MALL.Model.Shop.Sales.SalesRuleProduct GetRuleProduct(long productId,YSWL.MALL.Model.Members.UserRank userRank)
        {
            string CacheKey = "GetRuleProduct-P" + productId+"U" +(userRank==null?"-1": userRank.RankId.ToString());

            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    List<YSWL.MALL.Model.Shop.Sales.SalesRuleProduct> ruleProducts = GetRuleProList(productId);
                    if (ruleProducts == null)
                        return null;
                    //判断是否满足会员等级条件
                    YSWL.MALL.Model.Shop.Sales.SalesRuleProduct resultModel = null;
                    foreach (var item in ruleProducts)
                    {
                        if (!RankIsLimit(item.RuleId, userRank))
                        {
                            resultModel = item;
                            break;
                        }
                    }
                    objModel = resultModel;
                    if (objModel != null)
                    {
                    int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.Shop.Sales.SalesRuleProduct)objModel;

        }

        /// <summary>
        /// 获取批发优惠
        /// </summary>
        /// <param name="cartInfo"></param>
        /// <returns></returns>
        public YSWL.MALL.Model.Shop.Products.ShoppingCartInfo GetWholeSale(
            YSWL.MALL.Model.Shop.Products.ShoppingCartInfo cartInfo)
        {
            Dictionary<int, List<YSWL.MALL.Model.Shop.Products.ShoppingCartItem>> dictionary = new Dictionary<int, List<ShoppingCartItem>>();
            //获取所有的批发规则
            YSWL.MALL.BLL.Shop.Sales.SalesRule ruleBll=new SalesRule();
            List<YSWL.MALL.Model.Shop.Sales.SalesRule> allRules = ruleBll.GetModelList("Status=1");
            //批发规则处理
            foreach (var cartItem in cartInfo.Items)
            {
                GetCartItem(cartItem, dictionary);
            }

            //商品总计的情况处理
            if (dictionary != null && dictionary.Count > 0)
            {
                foreach (var dic in dictionary)
                {
                    YSWL.MALL.Model.Shop.Sales.SalesRule ruleModel = allRules.FirstOrDefault(c=>c.RuleId==dic.Key);
                    if (ruleModel != null)
                    {
                        cartInfo = GetRateValueList(ruleModel.RuleId, ruleModel.RuleUnit, dic.Value, cartInfo);
                        cartInfo.Items.RemoveAll(c => dic.Value.Contains(c));
                         foreach (var item in dic.Value)
                        {
                            item.SaleDes = ruleModel.RuleName;
                            cartInfo.Items.Add(item);
                        }
                    }
                }
            }
            return cartInfo;
        }
        //获取订单优惠值
        public void GetCartItem(YSWL.MALL.Model.Shop.Products.ShoppingCartItem cartItem,
            Dictionary<int, List<YSWL.MALL.Model.Shop.Products.ShoppingCartItem>> dictionary)
        {
            if (cartItem.SupplierId > 0)
            {
                return;
            }

              YSWL.MALL.BLL.Members.UserRank rankBll = new BLL.Members.UserRank();
            YSWL.MALL.Model.Members.UserRank userRank = rankBll.GetUserRank(cartItem.UserId);

            YSWL.MALL.Model.Shop.Sales.SalesRuleProduct RuleProductModel = GetRuleProduct(cartItem.ProductId, userRank);
            //如果没有对应规则，直接返回订单项
            if (RuleProductModel == null)
            {
                return;
            }
            YSWL.MALL.BLL.Shop.Sales.SalesRule ruleBll = new SalesRule();
            YSWL.MALL.Model.Shop.Sales.SalesRule RuleModel = ruleBll.GetModelByCache(RuleProductModel.RuleId);
            //不存在该批发规则，或者该规则不启用
            if (RuleModel == null || RuleModel.Status == 0)
            {
                return;
            }
           

            //优惠名称 暂时使用规则名称
            cartItem.SaleDes = RuleModel.RuleName;

            //单个商品模式处理
            if (RuleModel.RuleMode == 0)
            {
                //计算商品优惠值
                GetRateValue(RuleModel.RuleId, RuleModel.RuleUnit, cartItem);
            }
            //商品总计的情况处理
            else
            {
                if (dictionary.ContainsKey(RuleModel.RuleId))
                {
                    dictionary[RuleModel.RuleId].Add(cartItem);
                }
                else
                {
                    dictionary.Add(RuleModel.RuleId, new List<ShoppingCartItem> { cartItem });
                }
            }
        }

        /// <summary>
        /// 商品总计的情况处理
        /// </summary>
        /// <param name="ruleId"></param>
        /// <param name="ruleUnit"></param>
        /// <param name="cartItems"></param>
        /// <returns></returns>
        public YSWL.MALL.Model.Shop.Products.ShoppingCartInfo GetRateValueList(int ruleId, int ruleUnit,
                                                                                     List<YSWL.MALL.Model.Shop.Products.ShoppingCartItem> cartItems, YSWL.MALL.Model.Shop.Products.ShoppingCartInfo cartInfo)
        {
            YSWL.MALL.BLL.Shop.Sales.SalesItem itemBll = new SalesItem();
            List<YSWL.MALL.Model.Shop.Sales.SalesItem> itemList = itemBll.GetModelList(" RuleId=" + ruleId);
            //不存在该优惠项，直接返回
            if (itemList == null || itemList.Count == 0)
            {
                return cartInfo;
            }
            YSWL.MALL.Model.Shop.Sales.SalesItem salesItem = null;
            int quantity = cartItems.Select(c => c.Quantity).Sum();
            decimal totalSellPrice = cartItems.Select(c => c.SubTotal).Sum();
            //规则单位 为 “个”的情况
            if (ruleUnit == 0)
            {
                //计算数量之和
                salesItem = GetItemByQuantity(itemList, quantity);
            }
            //规则单位 为 “元”的情况
            if (ruleUnit == 1)
            {
                salesItem = GetItemByTotalPrice(itemList, totalSellPrice);
            }
            if (salesItem == null)
            {
                return cartInfo;
            }

             //优惠类型
                switch (itemList[0].ItemType)
                {
                    case 0: //打折
                        cartInfo.TotalRate += totalSellPrice * (100 - salesItem.RateValue) / 100;
                        break;
                    case 1: //减价
                        cartInfo.TotalRate += totalSellPrice > salesItem.RateValue ? salesItem.RateValue : totalSellPrice;
                        //cartItem.AdjustedPrice = (cartItem.SellPrice * cartItem.Quantity - salesItem.RateValue) /
                        //                         cartItem .Quantity;
                        break;
                    case 2: //固定价
                        cartInfo.TotalRate += (totalSellPrice > salesItem.RateValue * quantity) ? salesItem.RateValue * quantity : totalSellPrice;
                        break;
                    default:
                        cartInfo.TotalRate += totalSellPrice * (100 - salesItem.RateValue) / 100;
                        break;
                }

                return cartInfo;
        }

        //会员等级是否限制
        public bool RankIsLimit(int ruleId, int userid)
        {
            bool isEnable = SysManage.ConfigSystem.GetBoolValueByCache("RankScoreEnable"); //是否开启会员等级
            if (!isEnable)
                return false;
            YSWL.MALL.BLL.Shop.Sales.SalesUserRank userRankBll = new SalesUserRank();
            List<YSWL.MALL.Model.Shop.Sales.SalesUserRank> AllSalesRankList = userRankBll.GetAllSalesRank();
            List<YSWL.MALL.Model.Shop.Sales.SalesUserRank> userRankList = AllSalesRankList.Where(c => c.RuleId == ruleId).ToList();
            //如果没有等级条件，就全部限制
            if (userRankList == null || userRankList.Count == 0)
            {
                return true;
            }
            //获取用户等级
            YSWL.MALL.BLL.Members.UserRank rankBll = new BLL.Members.UserRank();
            YSWL.MALL.Model.Members.UserRank userRank = rankBll.GetUserRank(userid);
            if (userRank == null)
                return true;
            return !userRankList.Select(c => c.RankId).Contains(userRank.RankId);
        }

        /// <summary>
        /// 会员等级是否限制
        /// </summary>
        /// <param name="ruleId"></param>
        /// <param name="userRank"></param>
        /// <returns></returns>
        public bool RankIsLimit(int ruleId, YSWL.MALL.Model.Members.UserRank userRank)
        {
            bool isEnable = SysManage.ConfigSystem.GetBoolValueByCache("RankScoreEnable"); //是否开启会员等级
            if (!isEnable)
                return false;
            YSWL.MALL.BLL.Shop.Sales.SalesUserRank userRankBll = new SalesUserRank();
            List<YSWL.MALL.Model.Shop.Sales.SalesUserRank> AllSalesRankList = userRankBll.GetAllSalesRank();
            List<YSWL.MALL.Model.Shop.Sales.SalesUserRank> userRankList = AllSalesRankList.Where(c => c.RuleId == ruleId).ToList();
            //如果没有等级条件，就全部限制
            if (userRankList == null || userRankList.Count == 0)
            {
                return true;
            }
            //获取用户等级
            if (userRank == null)
                return true;
            return !userRankList.Select(c => c.RankId).Contains(userRank.RankId);
        }

        /// <summary>
        /// 计算购物车项的优惠值
        /// </summary>
        /// <param name="ruleId"></param>
        /// <param name="cartItem"></param>
        /// <returns></returns>
        public void GetRateValue(int ruleId, int ruleUnit,
            YSWL.MALL.Model.Shop.Products.
                ShoppingCartItem cartItem)
        {
            YSWL.MALL.BLL.Shop.Sales.SalesItem itemBll = new SalesItem();
            List<YSWL.MALL.Model.Shop.Sales.SalesItem> itemList = itemBll.GetModelList(" RuleId=" + ruleId);
            //不存在该优惠项，直接返回
            if (itemList == null || itemList.Count == 0)
            {
                return;
            }
            YSWL.MALL.Model.Shop.Sales.SalesItem salesItem = null;
            //规则单位 为 “个”的情况
            if (ruleUnit == 0)
            {
                salesItem = GetItemByQuantity(itemList, cartItem.Quantity);
            }
            //规则单位 为 “元”的情况
            if (ruleUnit == 1)
            {
                decimal totalSellPrice = cartItem.SellPrice * cartItem.Quantity;
                salesItem = GetItemByTotalPrice(itemList, totalSellPrice);
            }
            if (salesItem == null)
            {
                return;
            }
            //优惠类型
            switch (itemList[0].ItemType)
            {
                case 0: //打折
                    cartItem.AdjustedPrice = cartItem.SellPrice * salesItem.RateValue / 100;
                    break;
                case 1: //减价
                    decimal rateMoney = cartItem.SellPrice*cartItem.Quantity - salesItem.RateValue;
                    cartItem.AdjustedPrice = (rateMoney>0?rateMoney:0) /
                                             cartItem.Quantity;
                    break;
                case 2: //固定价
                    cartItem.AdjustedPrice =cartItem.SellPrice - salesItem.RateValue>0? cartItem.SellPrice - salesItem.RateValue:0;
                    break;
                default:
                    cartItem.AdjustedPrice = cartItem.SellPrice * salesItem.RateValue / 100;
                    break;
            }
        }

        /// <summary>
        /// 根据数量条件获取最优优惠项
        /// </summary>
        /// <param name="itemList"></param>
        /// <param name="Quantity"></param>
        /// <returns></returns>
        public YSWL.MALL.Model.Shop.Sales.SalesItem GetItemByQuantity(List<YSWL.MALL.Model.Shop.Sales.SalesItem> itemList,
                                                                  int Quantity)
        {
            //先对优惠项进行排序
            itemList = itemList.OrderByDescending(c => c.UnitValue).ToList();
            YSWL.MALL.Model.Shop.Sales.SalesItem itemModel = null;
            foreach (var salesItem in itemList)
            {
                if (Quantity >= salesItem.UnitValue)
                {
                    itemModel = salesItem;
                    break;
                }
            }
            return itemModel;
        }
        /// <summary>
        ///  根据总价条件获取最优优惠项
        /// </summary>
        /// <param name="itemList"></param>
        /// <param name="TotalPrice"></param>
        /// <returns></returns>
        public YSWL.MALL.Model.Shop.Sales.SalesItem GetItemByTotalPrice(
            List<YSWL.MALL.Model.Shop.Sales.SalesItem> itemList,
           decimal TotalPrice)
        {
            //先对优惠项进行排序
            itemList = itemList.OrderByDescending(c => c.UnitValue).ToList();
            YSWL.MALL.Model.Shop.Sales.SalesItem itemModel = null;
            foreach (var salesItem in itemList)
            {
                if (TotalPrice >= salesItem.UnitValue)
                {
                    itemModel = salesItem;
                    break;
                }
            }
            return itemModel;
        }
        /// <summary>
        /// 根据商品ID 获取批发优惠规则以及规则项
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public YSWL.MALL.ViewModel.Shop.SalesModel GetSalesRule(long productId,int userId)
        {
            YSWL.MALL.ViewModel.Shop.SalesModel salesModel=new SalesModel();
            BLL.Members.UserRank userRankBll = new UserRank();
            YSWL.MALL.Model.Members.UserRank userRank = userRankBll.GetUserRank(userId);
            YSWL.MALL.Model.Shop.Sales.SalesRuleProduct RuleProductModel = GetRuleProduct(productId, userRank);
            //如果没有对应规则，直接返回订单项
            if (RuleProductModel == null)
            {
                return salesModel;
            }
            YSWL.MALL.BLL.Shop.Sales.SalesRule ruleBll = new SalesRule();
            YSWL.MALL.Model.Shop.Sales.SalesRule RuleModel = ruleBll.GetModelByCache(RuleProductModel.RuleId);
            //不存在该批发规则，或者该规则不启用
            if (RuleModel == null || RuleModel.Status == 0)
            {
                return salesModel;
            }
            YSWL.MALL.BLL.Shop.Sales.SalesItem itembll=new BLL.Shop.Sales.SalesItem();
            salesModel.SalesRule = RuleModel;
            salesModel.SalesItems = itembll.GetModelList(" RuleId=" + RuleModel.RuleId);
            
            //是否开启会员等级
            bool isEnable = SysManage.ConfigSystem.GetBoolValueByCache("RankScoreEnable");
            if (isEnable && salesModel.SalesItems != null && salesModel.SalesItems.Count > 0)
            {
                foreach (Model.Shop.Sales.SalesItem item in salesModel.SalesItems)
                {
                    item.UserRankList = userRankBll.GetList(item.RuleId);
                }
            }     
            return salesModel;
        }

        public YSWL.MALL.ViewModel.Shop.SalesModel GetSalesRuleByCache(long productId,int userid)
        {
            string CacheKey = string.Format("GetSalesRuleByCache-pro{0}_user{1}",productId,userid);
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = GetSalesRule(productId, userid);
                    if (objModel != null)
                    {
                    int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.ViewModel.Shop.SalesModel)objModel;
        }

        #region 一键加入规则
        public int AddList(int ruleId, string name, int categoryId, int status = 1)
        {
            return dal.AddList(ruleId, name, categoryId, status);
        }
        #endregion 

        #region 判断商品是否存在
        public bool ExistsProduct (int ruleId,  long productId )
        {
            return dal.ExistsProduct(ruleId, productId);
        }
        #endregion

        #region 计算会员价格

        public List<Model.Shop.Products.SKUInfo> GetRankSales(List<Model.Shop.Products.SKUInfo> list,int userid)
        {
            if (userid == 0)//用户没有登录
            {
                return list;
            }
            YSWL.MALL.ViewModel.Shop.SalesModel salesModel = GetSalesRuleByCache(list[0].ProductId, userid);
            if (salesModel == null || salesModel.SalesItems == null || salesModel.SalesRule == null || salesModel.SalesRule.Type == 0)//没有对应的一键会员规则
            {
                return list;
            }
            foreach (var item in list)
            {
                switch (salesModel.SalesItems[0].ItemType)
                {
                    case 0:
                        item.RankPrice = item.SalePrice * salesModel.SalesItems[0].RateValue / 100;
                        break;
                    case 2:
                        item.RankPrice = item.SalePrice > salesModel.SalesItems[0].RateValue ? item.SalePrice - salesModel.SalesItems[0].RateValue : 0;
                        break;
                    default:
                        item.RankPrice = item.SalePrice * salesModel.SalesItems[0].RateValue / 100;
                        break;
                }
            }
            return list;
        }

        public decimal GetUserPrice(long productId, decimal salesPrice,int userid)
        {
            decimal rankPrice = 0;
            if (userid == 0)//用户没有登录
            {
                return rankPrice;
            }
            YSWL.MALL.ViewModel.Shop.SalesModel salesModel = GetSalesRuleByCache(productId, userid);
            if (salesModel == null || salesModel.SalesItems == null || salesModel.SalesRule == null || salesModel.SalesRule.Type == 0)//没有对应的一键会员规则
            {
                return rankPrice;
            }
            switch (salesModel.SalesItems[0].ItemType)
            {
                case 0:
                    rankPrice = salesPrice * salesModel.SalesItems[0].RateValue / 100;
                    break;
                case 2:
                    rankPrice = salesPrice > salesModel.SalesItems[0].RateValue ? salesPrice - salesModel.SalesItems[0].RateValue : 0;
                    break;
                default:
                    rankPrice = salesPrice * salesModel.SalesItems[0].RateValue / 100;
                    break;
            }

            return rankPrice;
        }

        #endregion

        #region 获取商品的批发规则信息
        public  YSWL.MALL.Model.Shop.Sales.EnumHelper.RuleType GetRuleType(long productId, int userid)
        {
            YSWL.MALL.ViewModel.Shop.SalesModel objModel = GetSalesRuleByCache(productId, userid);
            if (objModel == null|| objModel.SalesItems==null)
            {
                return YSWL.MALL.Model.Shop.Sales.EnumHelper.RuleType.None;
            }
            switch (objModel.SalesItems[0].ItemType)
            {
                case 0://打折
                    return YSWL.MALL.Model.Shop.Sales.EnumHelper.RuleType.Discount;
                case 1://减价
                    return YSWL.MALL.Model.Shop.Sales.EnumHelper.RuleType.Cut;
                case 2://固定价
                    return YSWL.MALL.Model.Shop.Sales.EnumHelper.RuleType.Reduce;
                default:
                    return YSWL.MALL.Model.Shop.Sales.EnumHelper.RuleType.None; 
            }

        }
        #endregion

        #region saas app

        /// <summary>
        /// 批量添加规则商品
        /// </summary>
        /// <param name="productItem"></param>
        /// <returns></returns>
        public bool AddSaleRuleBatch(List<Model.Shop.Sales.SalesRuleProduct> productItem)
        {
            return dal.AddSaleRuleBatch(productItem);
        }

        /// <summary>
        /// 批量删除规则商品
        /// </summary>
        /// <param name="productItem"></param>
        /// <returns></returns>
        public bool DeleteSaleRuleBatch(List<Model.Shop.Sales.SalesRuleProduct> productItem)
        {
            return dal.DeleteSaleRuleBatch(productItem);
        }

        public DataSet GetRuleProductsApp(int ruleId, string categoryId, string pName,string brandId)
        {
            StringBuilder strWhere = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(pName))
            {
                strWhere.AppendFormat(" AND ProductName LIKE '%{0}%'", Common.InjectionFilter.SqlFilter(pName));
            }
            if (!string.IsNullOrWhiteSpace(categoryId))
            {
                strWhere.AppendFormat("AND ProductId IN (SELECT DISTINCT ProductId FROM  PMS_ProductCategories PC WHERE (CategoryPath LIKE (SELECT Path FROM PMS_Categories WHERE CategoryId={0})+'|%'  or CategoryId={0}))", categoryId);
            }
            if (!string.IsNullOrEmpty(brandId))
            {
                strWhere.AppendFormat(" and  BrandId={0}", brandId);
            }
            return dal.GetRuleProducts(ruleId, strWhere.ToString());
        }

        #endregion
        #endregion  ExtensionMethod
    }
}

