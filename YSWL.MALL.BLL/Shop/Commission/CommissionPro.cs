/**  版本信息模板在安装目录下，可自行修改。
* CommissionPro.cs
*
* 功 能： N/A
* 类 名： CommissionPro
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/2/13 13:59:34   N/A    初版
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
using YSWL.MALL.BLL.Members;
using YSWL.Common;
using YSWL.MALL.Model.Shop.Commission;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Shop.Commission;
using YSWL.MALL.ViewModel.Shop;
using System.Linq;

namespace YSWL.MALL.BLL.Shop.Commission
{
	/// <summary>
	/// CommissionPro
	/// </summary>
	public partial class CommissionPro
    {
        BLL.Members.Users userbll = new BLL.Members.Users();

        private readonly ICommissionPro dal = DAShopCommission.CreateCommissionPro();
		public CommissionPro()
		{}
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
		public bool Exists(int RuleId,long ProductId)
		{
			return dal.Exists(RuleId,ProductId);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(YSWL.MALL.Model.Shop.Commission.CommissionPro model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(YSWL.MALL.Model.Shop.Commission.CommissionPro model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int RuleId,long ProductId)
		{
			
			return dal.Delete(RuleId,ProductId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public YSWL.MALL.Model.Shop.Commission.CommissionPro GetModel(int RuleId,long ProductId)
		{
			
			return dal.GetModel(RuleId,ProductId);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public YSWL.MALL.Model.Shop.Commission.CommissionPro GetModelByCache(int RuleId,long ProductId)
		{
			
			string CacheKey = "CommissionProModel-" + RuleId+ProductId;
			object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(RuleId,ProductId);
					if (objModel != null)
					{
						int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("CacheTime");
						YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (YSWL.MALL.Model.Shop.Commission.CommissionPro)objModel;
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
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<YSWL.MALL.Model.Shop.Commission.CommissionPro> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<YSWL.MALL.Model.Shop.Commission.CommissionPro> DataTableToList(DataTable dt)
		{
			List<YSWL.MALL.Model.Shop.Commission.CommissionPro> modelList = new List<YSWL.MALL.Model.Shop.Commission.CommissionPro>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				YSWL.MALL.Model.Shop.Commission.CommissionPro model;
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
			return dal.GetListByPage( strWhere,  orderby,  startIndex,  endIndex);
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
        public bool DeleteByRule(int RuleId)
        {
            return dal.DeleteByRule(RuleId);
        }

        #region 一键导入
        public int AddList(int ruleId, string name, int categoryId, int status = 1)
        {
            return dal.AddList(ruleId, name, categoryId, status);
        }
        #endregion 
        /// <summary>
        /// 获取佣金规则商品
        /// </summary>
        /// <param name="ruleId"></param>
        /// <param name="categoryId"></param>
        /// <param name="pName"></param>
        /// <returns></returns>
        public DataSet GetRuleProducts(int ruleId, int categoryId, string pName)
        {

            return dal.GetRuleProducts(ruleId, categoryId, pName);
        }

        /// <summary>
        /// 获得佣金
        /// </summary>
        /// <param name="orderInfo"></param>
        public void AddCommission(YSWL.MALL.Model.Shop.Order.OrderInfo orderInfo )
	    {
            //首先获得两级推广人信息
            if(orderInfo==null||orderInfo.OrderItems==null)
                return;
            List<int> referIdList = orderInfo.OrderItems.Select(c => c.ReferId).ToList();
            referIdList = referIdList.Distinct().ToList();
            YSWL.MALL.ViewModel.Shop.CommissionUser comUser = null;
            YSWL.MALL.BLL.Shop.Commission.CommissionDetail detailBll = new CommissionDetail();

            foreach (var reUserId in referIdList)
            {
                comUser = GetComUsers(reUserId);
                if (comUser != null)
                {
                    //获得参与佣金活动的商品
                    List<YSWL.MALL.Model.Shop.Commission.CommissionDetail> matchDetailList = GetComDetails(orderInfo, reUserId);
                    if (matchDetailList == null || matchDetailList.Count == 0)
                       continue;
                    //添加佣金
                    foreach (var detail in matchDetailList)
                    {
                        //添加第一级佣金
                        detailBll.AddDetail(orderInfo, detail, comUser.FirstUser, 1);
                        //添加第二级佣金
                        detailBll.AddDetail(orderInfo, detail, comUser.SecondUser, 2);
                    }
                }
            }
	    }

        //  /// <summary>
        //  /// 暂时获取两级佣金用户
        //  /// </summary>
        //  /// <param name="referId"></param>
        //  /// <returns></returns>
        //public YSWL.MALL.ViewModel.Shop.CommissionUser GetComUsers(int referId)
        //  {
        //      YSWL.MALL.ViewModel.Shop.CommissionUser comUser = null;
        //       YSWL.MALL.BLL.Members.Users userBll=new Users();
        //       //获取第一级用户
        //      YSWL.MALL.Model.Members.Users firstUser = userBll.GetInviteUser(referId);
        //      if (firstUser != null && firstUser.Activity.HasValue && firstUser.Activity.Value)//激活用户
        //      {
        //          comUser=new CommissionUser();
        //          comUser.FirstUser = firstUser;
        //          //获取第二级用户
        //          YSWL.MALL.Model.Members.Users secondUser = userBll.GetInviteUser(firstUser.UserID);
        //          if (secondUser != null && secondUser.Activity.HasValue && secondUser.Activity.Value) //激活用户
        //          {
        //              comUser.SecondUser = secondUser;
        //          }
        //      }
        //      return comUser;
        //}



        /// <summary>
        /// 获得佣金
        /// </summary>
        /// <param name="orderInfo"></param>
        public void AddCommissionT(YSWL.MALL.Model.Shop.Order.OrderInfo orderInfo)
        {
            YSWL.MALL.ViewModel.Shop.CommissionUser comUser = null;
            YSWL.MALL.BLL.Shop.Commission.CommissionDetail detailBll = new CommissionDetail();

            Users userbll = new Users();

            //获得参与佣金活动的商品
            List<YSWL.MALL.Model.Shop.Commission.CommissionDetail> matchDetailList = GetComDetailsT(orderInfo);

            comUser = GetComUsers(orderInfo.BuyerID);
            if (comUser != null)
            {
                //添加佣金
                foreach (var detail in matchDetailList)
                {                    
                    //添加第一级佣金
                    if (comUser.FirstUser != null && userbll.ExistsUserVIP(comUser.FirstUser.UserID.ToString()).ToUpper() == "VIP")
                    {
                        detailBll.AddDetail(orderInfo, detail, comUser.FirstUser, 1);
                    }
                    //添加第二级佣金 
                    if (comUser.SecondUser!=null && userbll.ExistsUserVIP(comUser.SecondUser.UserID.ToString()).ToUpper() == "VIP")//推荐人是否达到要求
                    {
                        detailBll.AddDetail(orderInfo, detail, comUser.SecondUser, 2);
                    }
                }
                

            }
        }

        /// <summary>
        /// 暂时获取san级佣金用户
        /// </summary>
        /// <param name="referId"></param>
        /// <returns></returns>
        public YSWL.MALL.ViewModel.Shop.CommissionUser GetComUsers(int referId)
        {
            YSWL.MALL.ViewModel.Shop.CommissionUser comUser = null;
            YSWL.MALL.BLL.Members.Users userBll = new Users();
            //获取第一级用户
            YSWL.MALL.Model.Members.Users firstUser = userBll.GetInviteUser(referId);
            if (firstUser != null && firstUser.Activity.HasValue && firstUser.Activity.Value)//激活用户
            {
                comUser = new CommissionUser();
                comUser.FirstUser = firstUser;
                //获取第二级用户
                YSWL.MALL.Model.Members.Users secondUser = userBll.GetInviteUser(firstUser.UserID);
                if (secondUser != null && secondUser.Activity.HasValue && secondUser.Activity.Value) //激活用户
                {
                    comUser.SecondUser = secondUser;

                    //获取第三级用户
                    YSWL.MALL.Model.Members.Users ThirdUser = userBll.GetInviteUser(secondUser.UserID);
                    if (ThirdUser != null && ThirdUser.Activity.HasValue && ThirdUser.Activity.Value) //激活用户
                    {
                        comUser.ThirdUser = ThirdUser;                        
                    }

                }
            }
            return comUser;
        }

        

        /// <summary>
        /// 获取商品的级别佣金规则
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <returns></returns>
        public List<YSWL.MALL.Model.Shop.Commission.CommissionDetail> GetComDetails(
	        YSWL.MALL.Model.Shop.Order.OrderInfo orderInfo,int referId)
	    {
            List<YSWL.MALL.Model.Shop.Commission.CommissionDetail> matchDetailList = new List<Model.Shop.Commission.CommissionDetail>();
            //获取所有有效的佣金规则
            YSWL.MALL.BLL.Shop.Commission.CommissionRule ruleBll=new CommissionRule();
	        List<YSWL.MALL.Model.Shop.Commission.CommissionRule> allActRule= ruleBll.GetAllActRule();
	        if (allActRule == null)
	            return null;
            //是否有包含全部商品的规则
	       YSWL.MALL.Model.Shop.Commission.CommissionRule allProRule= allActRule.Find(c => c.IsAll);

            YSWL.MALL.Model.Shop.Commission.CommissionDetail detailModel = null;
	        if (allProRule != null)
	        {
	           
                //排除赠品和不是此推广的订单项
                List<YSWL.MALL.Model.Shop.Order.OrderItems> itemList = orderInfo.OrderItems.Where(c => c.ProductType == 1 && c.ReferId == referId).ToList();
	            foreach (var item in itemList)
	            {
	                detailModel = new Model.Shop.Commission.CommissionDetail();
	                detailModel.BrandId = item.BrandId.HasValue?item.BrandId.Value:0;
	                detailModel.BrandName = item.BrandName;
	                detailModel.Name = item.Name;
	                detailModel.ProductId = item.ProductId;
	                detailModel.Quantity = item.ShipmentQuantity;
	                detailModel.ReferID = item.ReferId;
	                detailModel.ReferType = item.ReferType;
	                detailModel.SupplierId = item.SupplierId.HasValue?item.SupplierId.Value:0;
	                detailModel.SupplierName = item.SupplierName;
	                detailModel.RuleId = allProRule.RuleId;
	                detailModel.RuleName = allProRule.RuleName;
                    switch (allProRule.RuleMode)
                    {
                        //固定价 佣金费用=商品件数*级别佣金值
                        case 0:
                            detailModel.FirstFee = item.ShipmentQuantity * allProRule.FirstValue;
                            detailModel.SecondFee = item.ShipmentQuantity * allProRule.SecondValue;
                            break;
                        //交易值比例 佣金费用=商品交易值*级别佣金值比例
                        case 1:
                            detailModel.FirstFee = item.ShipmentQuantity * item.AdjustedPrice * allProRule.FirstValue / (decimal)100.00;
                            detailModel.SecondFee = item.ShipmentQuantity * item.AdjustedPrice * allProRule.SecondValue / (decimal)100.00;
                            break;
                        default:
                           detailModel.FirstFee = item.ShipmentQuantity * item.AdjustedPrice * allProRule.FirstValue / (decimal)100.00;
                            detailModel.SecondFee = item.ShipmentQuantity * item.AdjustedPrice * allProRule.SecondValue / (decimal)100.00;
                            break;
                    }
	            }
                matchDetailList.Add(detailModel);
	        }
	        else
	        {
                //获取佣金规则
                foreach (var item in orderInfo.OrderItems)
                {
                    //排除赠品和非此推广来源的的情况
                    if (item.ProductType == 1&&item.ReferId==referId)
                    {
                        GetComItem(item, allActRule, matchDetailList);
                    }
                }
	        }
            return matchDetailList;
	    }



        /// <summary>
        /// 获取商品的级别佣金规则
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <returns></returns>
        public List<YSWL.MALL.Model.Shop.Commission.CommissionDetail> GetComDetailsT(YSWL.MALL.Model.Shop.Order.OrderInfo orderInfo)
        {
            List<YSWL.MALL.Model.Shop.Commission.CommissionDetail> matchDetailList = new List<Model.Shop.Commission.CommissionDetail>();
            //获取所有有效的佣金规则
            YSWL.MALL.BLL.Shop.Commission.CommissionRule ruleBll = new CommissionRule();
            List<YSWL.MALL.Model.Shop.Commission.CommissionRule> allActRule = ruleBll.GetAllActRule();
            if (allActRule == null)
                return null;
            //是否有包含全部商品的规则
            // YSWL.MALL.Model.Shop.Commission.CommissionRule allProRule = allActRule.Find(c => c.IsAll);

            YSWL.MALL.Model.Shop.Commission.CommissionRule allProRule = allActRule.Find(c => c.RuleId==1);

            YSWL.MALL.Model.Shop.Commission.CommissionDetail detailModel = null;

            YSWL.MALL.BLL.Shop.Order.OrderItems orderlstiBll = new Order.OrderItems();

            YSWL.MALL.BLL.Shop.Commission.CommissionPro comprobll = new CommissionPro();
            if (allProRule != null)
            {
                //排除赠品和不是此推广的订单项
              //  List<YSWL.MALL.Model.Shop.Order.OrderItems> itemList = orderInfo.OrderItems.Where(c => c.ProductType == 1).ToList();

                List<YSWL.MALL.Model.Shop.Order.OrderItems> itemList = orderlstiBll.GetModelList(" OrderCode='"+orderInfo.OrderCode+"'");

                int vipCount = 0;

                foreach (var item in itemList)
                {
                    if (comprobll.Exists(allProRule.RuleId, item.ProductId))//判断是否符合VIP商品，是VIP商品才有分佣
                    {
                        vipCount += item.ShipmentQuantity;

                        detailModel = new Model.Shop.Commission.CommissionDetail();
                        detailModel.BrandId = item.BrandId.HasValue ? item.BrandId.Value : 0;
                        detailModel.BrandName = item.BrandName;
                        detailModel.Name = item.Name;
                        detailModel.ProductId = item.ProductId;
                        detailModel.Quantity = item.ShipmentQuantity;
                        detailModel.ReferID = item.ReferId;
                        detailModel.ReferType = item.ReferType;
                        detailModel.SupplierId = item.SupplierId.HasValue ? item.SupplierId.Value : 0;
                        detailModel.SupplierName = item.SupplierName;
                        detailModel.RuleId = allProRule.RuleId;
                        detailModel.RuleName = allProRule.RuleName;
                        switch (allProRule.RuleMode)
                        {
                            //固定价 佣金费用=商品件数*级别佣金值
                            case 0:
                                detailModel.FirstFee = item.ShipmentQuantity * allProRule.FirstValue;
                                detailModel.SecondFee = item.ShipmentQuantity * allProRule.SecondValue;
                                break;
                            //交易值比例 佣金费用=商品交易值*级别佣金值比例
                            case 1:
                                detailModel.FirstFee = item.ShipmentQuantity * item.AdjustedPrice * allProRule.FirstValue / (decimal)100.00;
                                detailModel.SecondFee = item.ShipmentQuantity * item.AdjustedPrice * allProRule.SecondValue / (decimal)100.00;
                                break;
                            default:
                                detailModel.FirstFee = item.ShipmentQuantity * item.AdjustedPrice * allProRule.FirstValue / (decimal)100.00;
                                detailModel.SecondFee = item.ShipmentQuantity * item.AdjustedPrice * allProRule.SecondValue / (decimal)100.00;
                                break;
                        }
                    }
                }
                matchDetailList.Add(detailModel);



                #region   成为VIP会员

                if (vipCount > 0)
                {
                    userbll.UpvipUserName(orderInfo.BuyerID.ToString());

                    //注册VIP，赠送积分
                    BLL.Members.PointsDetail pointsManage = new BLL.Members.PointsDetail();

                    int pointI = 0;
                    if (itemList[0].Name.IndexOf("产品1") > -1)
                    {
                        pointI = int.Parse(BLL.SysManage.ConfigSystem.GetValueByCache("vipone"));
                        pointsManage.AddPointsByVip(orderInfo.BuyerID, pointI, "购买VIP会员，赠送积分" + pointI + "", "", 0);
                    }
                    else if (itemList[0].Name.IndexOf("产品2") > -1)
                    {
                        pointI = int.Parse(BLL.SysManage.ConfigSystem.GetValueByCache("vipthree"));
                        pointsManage.AddPointsByVip(orderInfo.BuyerID, pointI, "购买VIP会员，赠送积分" + pointI + "", "", 0);
                    }
                }

                #endregion

                #region lstfor
                //for (int proi = 0; proi < orderInfo.OrderItems.Count; proi++)
                //{
                //    detailModel = new Model.Shop.Commission.CommissionDetail();
                //    detailModel.BrandId = orderInfo.OrderItems[proi].BrandId.HasValue ? orderInfo.OrderItems[proi].BrandId.Value : 0;
                //    detailModel.BrandName = orderInfo.OrderItems[proi].BrandName;
                //    detailModel.Name = orderInfo.OrderItems[proi].Name;
                //    detailModel.ProductId = orderInfo.OrderItems[proi].ProductId;
                //    detailModel.Quantity = orderInfo.OrderItems[proi].ShipmentQuantity;
                //    detailModel.ReferID = orderInfo.OrderItems[proi].ReferId;
                //    detailModel.ReferType = orderInfo.OrderItems[proi].ReferType;
                //    detailModel.SupplierId = orderInfo.OrderItems[proi].SupplierId.HasValue ? orderInfo.OrderItems[proi].SupplierId.Value : 0;
                //    detailModel.SupplierName = orderInfo.OrderItems[proi].SupplierName;
                //    detailModel.RuleId = allProRule.RuleId;
                //    detailModel.RuleName = allProRule.RuleName;
                //    switch (allProRule.RuleMode)
                //    {
                //        //固定价 佣金费用=商品件数*级别佣金值
                //        case 0:
                //            detailModel.FirstFee = orderInfo.OrderItems[proi].ShipmentQuantity * allProRule.FirstValue;
                //            detailModel.SecondFee = orderInfo.OrderItems[proi].ShipmentQuantity * allProRule.SecondValue;
                //            break;
                //        //交易值比例 佣金费用=商品交易值*级别佣金值比例
                //        case 1:
                //            detailModel.FirstFee = orderInfo.OrderItems[proi].ShipmentQuantity * orderInfo.OrderItems[proi].AdjustedPrice * allProRule.FirstValue / (decimal)100.00;
                //            detailModel.SecondFee = orderInfo.OrderItems[proi].ShipmentQuantity * orderInfo.OrderItems[proi].AdjustedPrice * allProRule.SecondValue / (decimal)100.00;
                //            break;
                //        default:
                //            detailModel.FirstFee = orderInfo.OrderItems[proi].ShipmentQuantity * orderInfo.OrderItems[proi].AdjustedPrice * allProRule.FirstValue / (decimal)100.00;
                //            detailModel.SecondFee = orderInfo.OrderItems[proi].ShipmentQuantity * orderInfo.OrderItems[proi].AdjustedPrice * allProRule.SecondValue / (decimal)100.00;
                //            break;
                //    }

                //    //detailModel = new Model.Shop.Commission.CommissionDetail();
                //    //detailModel.BrandId = 0;
                //    //detailModel.BrandName = "";
                //    //detailModel.Name = orderInfo.OrderId.ToString();
                //    //detailModel.ProductId = orderInfo.OrderId;
                //    //detailModel.Quantity = 1;
                //    //detailModel.ReferID = orderInfo.BuyerID;
                //    //detailModel.ReferType = orderInfo.ReferType;
                //    //detailModel.SupplierId = orderInfo.SupplierId.HasValue ? orderInfo.SupplierId.Value : 0;
                //    //detailModel.SupplierName = orderInfo.SupplierName;
                //    //detailModel.RuleId = allProRule.RuleId;
                //    //detailModel.RuleName = allProRule.RuleName;

                //    //switch (allProRule.RuleMode)
                //    //{

                //    //    //固定价 佣金费用=商品件数*级别佣金值
                //    //    case 0:
                //    //        detailModel.FirstFee = orderInfo.Amount * allProRule.FirstValue;
                //    //        detailModel.SecondFee = orderInfo.Amount * allProRule.SecondValue;
                //    //        break;
                //    //    //交易值比例 佣金费用=商品交易值*级别佣金值比例
                //    //    case 1:
                //    //        detailModel.FirstFee = orderInfo.Amount * allProRule.FirstValue / (decimal)100.00;
                //    //        detailModel.SecondFee = orderInfo.Amount * allProRule.SecondValue / (decimal)100.00;
                //    //        break;
                //    //    default:
                //    //        detailModel.FirstFee = orderInfo.Amount * allProRule.FirstValue / (decimal)100.00;
                //    //        detailModel.SecondFee = orderInfo.Amount * allProRule.SecondValue / (decimal)100.00;
                //    //        break;
                //    //}
                //    matchDetailList.Add(detailModel);
                //}
                #endregion
            }
            else
            {
                //获取佣金规则
                foreach (var item in orderInfo.OrderItems)
                {
                    //排除赠品和非此推广来源的的情况
                    if (item.ProductType == 1)
                    {
                        GetComItem(item, allActRule, matchDetailList);
                    }
                }
            }
            return matchDetailList;
        }

        /// <summary>
        ///  获取商品的参加的佣金规则
        /// </summary>
        /// <param name="item"></param>
        /// <param name="allActRule"></param>
        /// <param name="matchRuleList"></param>
        private void GetComItem(YSWL.MALL.Model.Shop.Order.OrderItems item,
             List<YSWL.MALL.Model.Shop.Commission.CommissionRule> allActRule,
                                List<YSWL.MALL.Model.Shop.Commission.CommissionDetail> matchDetailList)
        {
            YSWL.MALL.Model.Shop.Commission.CommissionPro itemPro = GetRuleProduct(item.ProductId);
            if (itemPro == null)
            {
                return;
            }
            YSWL.MALL.Model.Shop.Commission.CommissionRule ruleModel = allActRule.Find(c => c.RuleId == itemPro.RuleId);

            if (ruleModel == null)
            {
                  return;
            }

            Model.Shop.Commission.CommissionDetail detailModel = new Model.Shop.Commission.CommissionDetail();
            detailModel.BrandId = item.BrandId.HasValue ? item.BrandId.Value : 0;
            detailModel.BrandName = item.BrandName;
            detailModel.Name = item.Name;
            detailModel.ProductId = item.ProductId;
            detailModel.Quantity = item.ShipmentQuantity;
            detailModel.ReferID = item.ReferId;
            detailModel.ReferType = item.ReferType;
            detailModel.SupplierId = item.SupplierId.HasValue ? item.SupplierId.Value : 0;
            detailModel.SupplierName = item.SupplierName;
            detailModel.RuleId = ruleModel.RuleId;
            detailModel.RuleName = ruleModel.RuleName;
         
            //计算佣金
            switch (ruleModel.RuleMode)
            {
                //固定价 佣金费用=商品件数*级别佣金值
                case 0:
                    detailModel.FirstFee = item.ShipmentQuantity * ruleModel.FirstValue;
                    detailModel.SecondFee = item.ShipmentQuantity * ruleModel.SecondValue;
                    break;
                //交易值比例 佣金费用=商品交易值*级别佣金值比例
                case 1:
                    detailModel.FirstFee = item.ShipmentQuantity * item.AdjustedPrice * ruleModel.FirstValue / (decimal)100.00;
                    detailModel.SecondFee = item.ShipmentQuantity * item.AdjustedPrice * ruleModel.SecondValue / (decimal)100.00;
                    break;
                default:
                    detailModel.FirstFee = item.ShipmentQuantity * item.AdjustedPrice * ruleModel.FirstValue / (decimal)100.00;
                    detailModel.SecondFee = item.ShipmentQuantity * item.AdjustedPrice * ruleModel.SecondValue / (decimal)100.00;
                    break;
            }
            matchDetailList.Add(detailModel);
        }

        /// <summary>
        /// 根据商品获取对应的佣金规则
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public YSWL.MALL.Model.Shop.Commission.CommissionPro GetRuleProduct(long productId)
        {
            string CacheKey = "CommissionPro-GetRuleProduct-" + productId ;

            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {

                    objModel = dal.GetRuleProduct(productId);
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.Shop.Commission.CommissionPro)objModel;

        }

        /// <summary>
        /// 获取推广商品 分页
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
	    public List<YSWL.MALL.ViewModel.Shop.ProComModel> GetComProByPage(int cid,string name,int startIndex, int endIndex)
        {
            YSWL.MALL.BLL.Shop.Commission.CommissionRule ruleBll=new CommissionRule();
            YSWL.MALL.Model.Shop.Commission.CommissionRule  allProRule = ruleBll.GetExistAllPro();
            int ruleId = 0;
            if (allProRule != null)
            {
                ruleId = allProRule.RuleId;
            }

            DataSet ds = dal.GetComProByPage(cid, name,startIndex, endIndex, ruleId);
            if (DataSetTools.DataSetIsNull(ds)) return null;
            List<YSWL.MALL.ViewModel.Shop.ProComModel> modelList = new List<YSWL.MALL.ViewModel.Shop.ProComModel>();
            DataTable dt = ds.Tables[0];
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.ViewModel.Shop.ProComModel prodmodel;
                for (int n = 0; n < rowsCount; n++)
                {
                    prodmodel = new YSWL.MALL.ViewModel.Shop.ProComModel();
                    if (dt.Rows[n]["ProductId"] != null && dt.Rows[n]["ProductId"].ToString() != "")
                    {
                        prodmodel.ProductId = long.Parse(dt.Rows[n]["ProductId"].ToString());
                    }
                    if (dt.Rows[n]["ProductName"] != null && dt.Rows[n]["ProductName"].ToString() != "")
                    {
                        prodmodel.ProductName = dt.Rows[n]["ProductName"].ToString();
                    }
                    if (dt.Rows[n]["ThumbnailUrl1"] != null && dt.Rows[n]["ThumbnailUrl1"].ToString() != "")
                    {
                        prodmodel.ThumbnailUrl = dt.Rows[n]["ThumbnailUrl1"].ToString();
                    }
                    if (dt.Rows[n]["LowestSalePrice"] != null && dt.Rows[n]["LowestSalePrice"].ToString() != "")
                    {
                        prodmodel.ProductPrice = Common.Globals.SafeDecimal(dt.Rows[n]["LowestSalePrice"].ToString(),0);
                    }
                    if (dt.Rows[n]["RuleId"] != null && dt.Rows[n]["RuleId"].ToString() != "")
                    {
                        prodmodel.RuleId = Common.Globals.SafeInt(dt.Rows[n]["RuleId"].ToString(),0);
                    }
                    if (dt.Rows[n]["RuleName"] != null)
                    {
                        prodmodel.RuleName = dt.Rows[n]["RuleName"].ToString();
                    }
                    if (dt.Rows[n]["RuleMode"] != null && dt.Rows[n]["RuleMode"].ToString() != "")
                    {
                        prodmodel.RuleMode = int.Parse(dt.Rows[n]["RuleMode"].ToString());
                    }
                    if (dt.Rows[n]["FirstValue"] != null && dt.Rows[n]["FirstValue"].ToString() != "")
                    {
                        prodmodel.FirstValue = Common.Globals.SafeDecimal(dt.Rows[n]["FirstValue"].ToString(),0);
                    }
                    //固定金额
                    if (prodmodel.RuleMode == 0)
                    {
                        prodmodel.FirstFee = prodmodel.FirstValue;
                        prodmodel.FeeRate =prodmodel.ProductPrice>0? prodmodel.FirstFee/prodmodel.ProductPrice*(decimal)100.00:100; //有些商品价格为零
                    }
                    else
                    {
                        prodmodel.FeeRate = prodmodel.FirstValue;
                        prodmodel.FirstFee = prodmodel.ProductPrice * (prodmodel.FirstValue / (decimal)100.00);
                    }
                    modelList.Add(prodmodel);
                }
            }
            return modelList;
        }
        /// <summary>
        /// 推广商品行数
        /// </summary>
        /// <returns></returns>
	    public int GetComProCount(int cid,string name)
	    {
            YSWL.MALL.BLL.Shop.Commission.CommissionRule ruleBll = new CommissionRule();
            YSWL.MALL.Model.Shop.Commission.CommissionRule allProRule = ruleBll.GetExistAllPro();
            int ruleId = 0;
            if (allProRule != null)
            {
                ruleId = allProRule.RuleId;
            }
            return dal.GetComProCount(cid,name,ruleId);
	    }

	    #endregion  ExtensionMethod
	}
}

