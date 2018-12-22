/**  版本信息模板在安装目录下，可自行修改。
* RankDetail.cs
*
* 功 能： N/A
* 类 名： RankDetail
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/12/17 16:54:41   N/A    初版
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
using YSWL.MALL.Model.Members;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Members;
namespace YSWL.MALL.BLL.Members
{
	/// <summary>
	/// RankDetail
	/// </summary>
	public partial class RankDetail
	{
        private readonly IRankDetail dal = DAMembers.CreateRankDetail();
		public RankDetail()
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
		public bool Exists(int DetailID)
		{
			return dal.Exists(DetailID);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(YSWL.MALL.Model.Members.RankDetail model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(YSWL.MALL.Model.Members.RankDetail model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int DetailID)
		{
			
			return dal.Delete(DetailID);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string DetailIDlist )
		{
			return dal.DeleteList(DetailIDlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public YSWL.MALL.Model.Members.RankDetail GetModel(int DetailID)
		{
			
			return dal.GetModel(DetailID);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public YSWL.MALL.Model.Members.RankDetail GetModelByCache(int DetailID)
		{
			
			string CacheKey = "RankDetailModel-" + DetailID;
			object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(DetailID);
					if (objModel != null)
					{
						int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("CacheTime");
						YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (YSWL.MALL.Model.Members.RankDetail)objModel;
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
		public List<YSWL.MALL.Model.Members.RankDetail> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<YSWL.MALL.Model.Members.RankDetail> DataTableToList(DataTable dt)
		{
			List<YSWL.MALL.Model.Members.RankDetail> modelList = new List<YSWL.MALL.Model.Members.RankDetail>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				YSWL.MALL.Model.Members.RankDetail model;
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
        #region 扩展方法
        /// <summary>
        /// 日常操作获取成长值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userid"></param>
        /// <param name="desc"></param>
        /// <param name="extdata"></param>
        /// <returns></returns>
        public static int AddScore(int actionId, int userid, string desc, string extdata = "")
        {
            YSWL.MALL.BLL.Members.RankDetail detailBll=new RankDetail();
            RankRule ruleBll = new RankRule();
            Model.Members.RankRule ruleModel = ruleBll.GetModel(actionId, userid);
            Model.Members.RankDetail rankModel = new Model.Members.RankDetail();
            bool isEnable = SysManage.ConfigSystem.GetBoolValueByCache("RankScoreEnable");
            if (!detailBll.isLimit(ruleModel, userid) && isEnable)
            {
                //添加成长值详细信息
                rankModel.CreatedDate = DateTime.Now;
                rankModel.Description = desc;
                rankModel.ExtData = extdata;
                rankModel.Score = ruleModel.Score;
                rankModel.RuleId = ruleModel.RuleId;
                rankModel.UserID = userid;
                rankModel.Type = 0;
                if (detailBll.AddDetail(rankModel))
                {
                    return ruleModel.Score;
                }
            }
            return 0;
        }

	    public bool AddDetail(Model.Members.RankDetail rankModel)
	    {
            bool isAuto = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_UserRank_AutoUpdate");
            return dal.AddDetail(rankModel, isAuto);
	    }

	    /// <summary>
        /// 是否限制
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool isLimit(YSWL.MALL.Model.Members.RankRule Rule, int userid)
        {
            //根据规则获取限制条件
            YSWL.MALL.BLL.Members.RankLimit limitBll = new RankLimit();
            if (Rule != null)
            {
                if (Rule.LimitID < 0)
                {
                    return false;
                }
                YSWL.MALL.Model.Members.RankLimit limitModel = limitBll.GetModel(Rule.LimitID);
                int count = GetCount(userid, limitModel.CycleUnit, limitModel.Cycle, Rule.RuleId);
                if (count >= limitModel.MaxTimes)
                {
                    return true;
                }
                return false;
            }
            return true;
        }
        /// <summary>
        /// 根据条件限制获取
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="unit"></param>
        /// <param name="cycle"></param>
        /// <returns></returns>
        public int GetCount(int userid, string unit, int cycle, int RuleId)
        {
            return dal.GetCount(userid, unit, cycle, RuleId);
        }
        /// <summary>
        /// 扣除成长值消费明细(例如：如恶意操作)
        /// </summary>
        /// <returns></returns>
        public bool DeductScore(int userid, int score, string desc, string extdata = "")
        {
            YSWL.MALL.Model.Members.RankDetail scoreModel = new Model.Members.RankDetail();
            scoreModel.CreatedDate = DateTime.Now;
            scoreModel.Description = desc;
            scoreModel.ExtData = extdata;
            scoreModel.Score = score;
            scoreModel.RuleId = -1;
            scoreModel.Type = 1;
            scoreModel.UserID = userid;
            bool isAuto = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_UserRank_AutoUpdate");
            return dal.AddDetail(scoreModel, isAuto);
        }
        /// <summary>
        /// 通过消费获取成长值（例如：购买某件商品获得与商品价格一定比率的成长值，比率可以通过配置表配置）
        /// </summary>
        public bool AddScoreByBuy(int userid, int score, string desc, string extdata = "")
        {
            YSWL.MALL.Model.Members.RankDetail scoreModel = new Model.Members.RankDetail();
            scoreModel.CreatedDate = DateTime.Now;
            scoreModel.Description = desc;
            scoreModel.ExtData = extdata;
            scoreModel.Score = score;
            scoreModel.RuleId = 0;
            scoreModel.Type = 0;
            scoreModel.UserID = userid;
            bool isAuto = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_UserRank_AutoUpdate");
            return dal.AddDetail(scoreModel, isAuto);
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Members.RankDetail> GetListByPageEX(string strWhere, string orderby, int startIndex, int endIndex)
        {
            DataSet ds = GetListByPage(strWhere, orderby, startIndex, endIndex);
            return DataTableToList(ds.Tables[0]);
        }
        //签到
        public int GetSignCount(int userId)
        {
            return dal.GetSignCount(userId);
        }
        public List<YSWL.MALL.Model.Members.RankDetail> GetSignListByPage(int userId, string orderby, int startIndex, int endIndex)
        {
            DataSet ds = dal.GetSignListByPage(userId, orderby, startIndex, endIndex);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 管理员给用户添加成长值
        /// </summary>
        public bool AddScoreByAdmin(int userid, int score, string extdata = "",int type=0)
        {
            YSWL.MALL.Model.Members.RankDetail scoreModel = new Model.Members.RankDetail();
            scoreModel.CreatedDate = DateTime.Now;
            scoreModel.Description = type == 0 ? "线下充值" : "线下消费";
            scoreModel.ExtData = extdata;
            scoreModel.Score = score;
            scoreModel.RuleId = 0;
            scoreModel.Type =type;
            scoreModel.UserID = userid;
            bool isAuto = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_UserRank_AutoUpdate");
            return dal.AddDetail(scoreModel, isAuto);
        }
        #endregion
	}
}

