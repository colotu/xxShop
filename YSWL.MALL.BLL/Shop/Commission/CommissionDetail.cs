/**  版本信息模板在安装目录下，可自行修改。
* CommissionDetail.cs
*
* 功 能： N/A
* 类 名： CommissionDetail
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/2/26 16:51:35   N/A    初版
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
using YSWL.Common;
using YSWL.MALL.Model.Shop.Commission;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Shop.Commission;
using System.Text;
using YSWL.MALL.Model.Shop.Order;

namespace YSWL.MALL.BLL.Shop.Commission
{
	/// <summary>
	/// CommissionDetail
	/// </summary>
	public partial class CommissionDetail
	{
        private readonly ICommissionDetail dal = DAShopCommission.CreateCommissionDetail();
		public CommissionDetail()
		{}
		#region  BasicMethod
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(long DetailId)
		{
			return dal.Exists(DetailId);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public long Add(YSWL.MALL.Model.Shop.Commission.CommissionDetail model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(YSWL.MALL.Model.Shop.Commission.CommissionDetail model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(long DetailId)
		{
			
			return dal.Delete(DetailId);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string DetailIdlist )
		{
			return dal.DeleteList(DetailIdlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public YSWL.MALL.Model.Shop.Commission.CommissionDetail GetModel(long DetailId)
		{
			
			return dal.GetModel(DetailId);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public YSWL.MALL.Model.Shop.Commission.CommissionDetail GetModelByCache(long DetailId)
		{
			
			string CacheKey = "CommissionDetailModel-" + DetailId;
			object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(DetailId);
					if (objModel != null)
					{
						int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("CacheTime");
						YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (YSWL.MALL.Model.Shop.Commission.CommissionDetail)objModel;
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
		public List<YSWL.MALL.Model.Shop.Commission.CommissionDetail> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<YSWL.MALL.Model.Shop.Commission.CommissionDetail> DataTableToList(DataTable dt)
		{
			List<YSWL.MALL.Model.Shop.Commission.CommissionDetail> modelList = new List<YSWL.MALL.Model.Shop.Commission.CommissionDetail>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				YSWL.MALL.Model.Shop.Commission.CommissionDetail model;
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
        /// <summary>
        /// 添加佣金明细 （应该用事务，还要添加余额明细）
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <param name="detailModel"></param>
        /// <param name="userModel"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public bool AddDetail(YSWL.MALL.Model.Shop.Order.OrderInfo orderInfo, YSWL.MALL.Model.Shop.Commission.CommissionDetail detailModel,
                             YSWL.MALL.Model.Members.Users userModel,int level=1)
	    {
            if (userModel == null || orderInfo == null || detailModel == null)
	            return false;
            detailModel.BuyerID = orderInfo.BuyerID;
            detailModel.BuyerName = orderInfo.BuyerName;
            detailModel.OrderAmount = orderInfo.Amount;
            detailModel.OrderCode = orderInfo.OrderCode;
            detailModel.OrderId = orderInfo.OrderId;
	        detailModel.TradeDate = DateTime.Now;
	        detailModel.TradeType = 1;
	        detailModel.UserId = userModel.UserID;
            switch (level)
	        {
                case 1:
                    detailModel.Fee = detailModel.FirstFee;
                    break;
                case 2:
                    detailModel.Fee = detailModel.SecondFee;
                    break;
                default:
                    detailModel.Fee = 0;
                    break;
	        }
            detailModel.RuleLevel = level; 
	        detailModel.UserName = userModel.UserName;
	     
	        return dal.AddDetail(detailModel);
	    }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Commission.CommissionDetail> GetListByPageEx(string strWhere, string orderby, int startIndex, int endIndex)
        {
            DataSet ds = GetListByPage(strWhere, orderby, startIndex, endIndex);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
	    public decimal GetUserFees(int userId)
	    {
            return dal.GetUserFees(userId);
	    }
        /// <summary>
        /// 统计用户的佣金
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
	    public DataSet StatUserFee(DateTime startDate, DateTime endDate)
	    {
	        return dal.StatUserFee(startDate, endDate);
	    }

        public DataSet StatProFee(DateTime startDate, DateTime endDate)
        {
            return dal.StatProFee(startDate, endDate);
        }

        public DataSet StatCommission(DateTime startDate, DateTime endDate, StatisticMode mode = StatisticMode.Day)
        {
            return dal.StatCommission(startDate, endDate, mode);
        }

        public DataSet GetList(DateTime startDate,DateTime endDate, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("  TradeDate>'{0}' and TradeDate<'{1}' ", startDate, endDate);
            return dal.GetList(-1, strSql.ToString(), filedOrder);
        }
        /// <summary>
        /// 统计规则的佣金
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataSet StatRuleFee(DateTime startDate, DateTime endDate)
        {
            return dal.StatRuleFee(startDate, endDate);
        }
        /// <summary>
        /// 按佣金规则统计佣金商品数量
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataSet StatRulePro(DateTime startDate, DateTime endDate)
        {
            return dal.StatRulePro(startDate, endDate);
        }
        /// <summary>
        /// 按商品统计佣金
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="RuleLevel">级别</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<ViewModel.Shop.CommissionProStat> StatPro(int userId, int RuleLevel, DateTime startDate, DateTime endDate)
        {
            DataSet ds= dal.StatPro(userId, RuleLevel, startDate, endDate);
            if (DataSetTools.DataSetIsNull(ds))
            {
                return null;
            }
            List<ViewModel.Shop.CommissionProStat> list = new List<ViewModel.Shop.CommissionProStat>();
            ViewModel.Shop.CommissionProStat model = null;
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                model = new ViewModel.Shop.CommissionProStat();
                model.ProductId = Globals.SafeInt(dr["ProductId"], 0);
                model.ProName = dr.Field<string>("Name");
                model.TotalProduct =Globals.SafeInt(dr["TotalProduct"], 0);
                model.TotalFee = Globals.SafeDecimal(dr["TotalFee"], 0);
                list.Add(model);
            }
            return list;
        }
        /// <summary>
        /// 按商品统计佣金 (分页)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="RuleLevel">级别</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<ViewModel.Shop.CommissionProStat> StatPro(int userId, int RuleLevel, DateTime startDate, DateTime endDate, int startIndex, int endIndex,out int totalCount,out  decimal totalFee)
        {
            totalCount = dal.StatProRecordCount(userId, RuleLevel, startDate, endDate);       
            if (totalCount == 0) {
                totalFee = 0;
                return null;
            }
            totalFee =dal.GetTotalFee(userId,RuleLevel,startDate,endDate);//获取总金额
            DataSet ds = dal.StatPro(userId, RuleLevel, startDate, endDate, startIndex,endIndex);
            if (DataSetTools.DataSetIsNull(ds))
            {
                return null;
            }
            List<ViewModel.Shop.CommissionProStat> list = new List<ViewModel.Shop.CommissionProStat>();
            ViewModel.Shop.CommissionProStat model = null;
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                model = new ViewModel.Shop.CommissionProStat();
                model.ProductId = Globals.SafeInt(dr["ProductId"], 0);
                model.ProName = dr.Field<string>("Name");
                model.TotalProduct = Globals.SafeInt(dr["TotalProduct"], 0);
                model.TotalFee = Globals.SafeDecimal(dr["TotalFee"], 0);
                list.Add(model);
            }
            return list;
        }

        #region 盟友排行
        /// <summary>
        /// 盟友排行  佣金维度（分页）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public List<ViewModel.Shop.CommissionStat> AllyRanking(int userId, DateTime startDate, DateTime endDate, int startIndex, int endIndex, out int totalCount) {
            totalCount = dal.AllyRankingRecordCount(userId, startDate, endDate);
            if (totalCount == 0)
            {
                return null;
            }
            DataSet ds = dal.AllyRanking(userId, startDate, endDate, startIndex, endIndex);
            if (DataSetTools.DataSetIsNull(ds))
            {
                return null;
            }
            List<ViewModel.Shop.CommissionStat> list = new List<ViewModel.Shop.CommissionStat>();
            ViewModel.Shop.CommissionStat model = null;
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                model = new ViewModel.Shop.CommissionStat();
                model.UserId = Globals.SafeInt(dr["UserId"], 0);
                model.NickName = dr.Field<string>("NickName");
                model.TotalFee = Globals.SafeDecimal(dr["TotalFee"], 0);
                list.Add(model);
            }
            return list;
        }
        /// <summary>
        /// 盟友排行总数   佣金维度
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int AllyRankingRecordCount(int userId, DateTime startDate, DateTime endDate) {
            return dal.AllyRankingRecordCount(userId, startDate, endDate);
        }
        /// <summary>
        /// 获取订单数和商品数  
        /// </summary> 
        /// <param name="userId"></param>
        /// <returns></returns>
        public int  GetOrderCount(int userId,out int productCount) {
            productCount = 0;
            DataSet  ds=  dal.GetOrderCount(userId);
            if (DataSetTools.DataSetIsNull(ds)) {
                return 0;
            }
            DataTable dt = ds.Tables[0];
            DataRow dr = dt.Rows[0];
            productCount = Globals.SafeInt(dr["productCount"],0);
            return Globals.SafeInt(dr["OrderCount"], 0);
        }
        #endregion
        #endregion  ExtensionMethod
    }
}

