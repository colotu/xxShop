using System;
using System.Data;
using System.Collections.Generic;
using YSWL.Common;
using YSWL.MALL.Model.Members;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Members;
using System.Text;

namespace YSWL.MALL.BLL.Members
{
    /// <summary>
    /// 积分记录
    /// </summary>
    public partial class PointsDetail
    {
        private readonly IPointsDetail dal = DAMembers.CreatePointsDetail();
        Users userbll = new Users();
        public PointsDetail()
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
        public bool Exists(int DetailID)
        {
            return dal.Exists(DetailID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.MALL.Model.Members.PointsDetail model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.Members.PointsDetail model)
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
        public bool DeleteList(string DetailIDlist)
        {
            return dal.DeleteList(Common.Globals.SafeLongFilter(DetailIDlist,0) );
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Members.PointsDetail GetModel(int DetailID)
        {

            return dal.GetModel(DetailID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public YSWL.MALL.Model.Members.PointsDetail GetModelByCache(int DetailID)
        {

            string CacheKey = "PointsDetailModel-" + DetailID;
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
                catch { }
            }
            return (YSWL.MALL.Model.Members.PointsDetail)objModel;
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
        public List<YSWL.MALL.Model.Members.PointsDetail> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Members.PointsDetail> DataTableToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Members.PointsDetail> modelList = new List<YSWL.MALL.Model.Members.PointsDetail>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Members.PointsDetail model;
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

        public List<YSWL.MALL.Model.Members.PointsDetail> GetPointListByPageByEmpid(string Empid, string userid, string dr, int startIndex, int endIndex, out int toalCount)
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" 1=1");
            if (!String.IsNullOrWhiteSpace(Empid))
            {
                strWhere.AppendFormat(" and  Empid = '{0}'  ", InjectionFilter.SqlFilter(Empid));
            }

            if (!string.IsNullOrWhiteSpace(userid))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" UserID = '{0}'", InjectionFilter.SqlFilter(userid));
            }

            if (!string.IsNullOrEmpty(dr))
            {
                string[] date = dr.Split('_');
                if (date.Length > 0)
                {
                    DateTime? dateStart = Globals.SafeDateTime(date[0], null);
                    if (dateStart.HasValue)
                    {
                        if (strWhere.Length > 1)
                        {
                            strWhere.Append(" and ");
                        }
                        strWhere.AppendFormat(" CreatedDate >= CONVERT(DATETIME, '{0}') ", dateStart.Value);
                    }
                }
                if (date.Length > 1)
                {
                    DateTime? dateEnd = Globals.SafeDateTime(date[1], null);
                    if (dateEnd.HasValue)
                    {
                        if (strWhere.Length > 1)
                        {
                            strWhere.Append(" and ");
                        }
                        strWhere.AppendFormat(" CreatedDate < CONVERT(DATETIME, '{0}')", dateEnd.Value.AddDays(1));
                    }
                }
            }


            toalCount = GetRecordCount(strWhere.ToString());
            if (toalCount == 0)
                return null;

            DataSet ds = dal.GetListByPage(strWhere.ToString(), " CreatedDate desc ", startIndex, endIndex);

            return DataTableToList(ds.Tables[0]);
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
        /// 日常操作获取积分
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userid"></param>
        /// <param name="desc"></param>
        /// <param name="extdata"></param>
        /// <returns></returns>
        public int AddPoints(int actionId, int userid, string desc, string extdata = "")
        {
            PointsRule ruleBll = new PointsRule();

            Model.Members.PointsRule ruleModel = ruleBll.GetModel(actionId, userid);
            Model.Members.PointsDetail pointModel = new Model.Members.PointsDetail();
            bool isEnable = Common.Globals.SafeBool(YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("PointEnable"), true);
            if (!isLimit(ruleModel, userid) && isEnable)
            {
                //添加积分详细信息
                pointModel.CreatedDate = DateTime.Now;
                pointModel.Description = desc;
                pointModel.ExtData = extdata;
                pointModel.Score = ruleModel.Score;
                pointModel.RuleId = ruleModel.RuleId;
                pointModel.UserID = userid;
                pointModel.Type = 0;
                if (dal.AddDetail(pointModel))
                {
                    return ruleModel.Score;
                }
            }
            return 0;
        }
        /// <summary>
        /// 是否限制
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool isLimit(YSWL.MALL.Model.Members.PointsRule Rule, int userid)
        {
            //根据规则获取限制条件
            YSWL.MALL.BLL.Members.PointsLimit limitBll = new PointsLimit();
            if (Rule != null)
            {
                if (Rule.LimitID < 0)
                {
                    return false;
                }
                YSWL.MALL.Model.Members.PointsLimit limitModel = limitBll.GetModel(Rule.LimitID);
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
            // return dal.GetRecordCount(" userid=" + userid + " and RuleId=" + RuleId + " and datediff( " + unit + ", CreatedDate, GETDATE())<" + cycle);
        }
        /// <summary>
        /// 添加积分消费明细(例如：积分礼品兑换)
        /// </summary>
        /// <returns></returns>
        public bool UsePoints(int userid, int score, string desc, string extdata = "")
        {
            YSWL.MALL.Model.Members.PointsDetail pointModel = new Model.Members.PointsDetail();
            pointModel.CreatedDate = DateTime.Now;
            pointModel.Description = desc;
            pointModel.ExtData = extdata;
            pointModel.Score = score;
            pointModel.RuleId = -1;
            pointModel.Type = 1;
            pointModel.UserID = userid;
            return dal.AddDetail(pointModel);
        }
        /// <summary>
        /// 通过消费获取积分（例如：购买某件商品获得与商品价格一定比率的积分，比率可以通过配置表配置）
        /// </summary>
        public bool AddPointsByBuy(int userid, int score, string desc, string extdata = "")
        {
            YSWL.MALL.Model.Members.PointsDetail pointModel = new Model.Members.PointsDetail();
            pointModel.CreatedDate = DateTime.Now;
            pointModel.Description = desc;
            pointModel.ExtData = extdata;
            pointModel.Score = score;
            pointModel.RuleId = 0;
            pointModel.Type = 0;
            pointModel.UserID = userid;
            return dal.AddDetail(pointModel);
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Members.PointsDetail> GetListByPageEX(string strWhere, string orderby, int startIndex, int endIndex)
        {
            DataSet ds = GetListByPage(strWhere, orderby, startIndex, endIndex);
            return DataTableToList(ds.Tables[0]);
        }

        public int GetSignCount(int userId)
        {
            return dal.GetSignCount(userId);
        }
        public List<YSWL.MALL.Model.Members.PointsDetail> GetSignListByPage(int userId, string orderby, int startIndex, int endIndex)
        {
            DataSet ds = dal.GetSignListByPage(userId, orderby, startIndex, endIndex);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 线下充值积分
        /// </summary>
        public bool AddPointsByOffline(int userid, int score, string extdata = "", int type = 0)
        {
            YSWL.MALL.Model.Members.PointsDetail pointModel = new Model.Members.PointsDetail();
            pointModel.CreatedDate = DateTime.Now;
            pointModel.Description = type == 0 ? "线下充值" : "线下消费";
            pointModel.ExtData = extdata;
            pointModel.Score = score;
            pointModel.RuleId = 0;
            pointModel.Type = type;
            pointModel.UserID = userid;
            return dal.AddDetail(pointModel);
        }

        /// <summary>
        /// 购买VIP赠送积分
        /// </summary>
        public bool AddPointsByVip(int userid, int score, string desInfo,string extdata = "", int type = 0)
        {
            YSWL.MALL.Model.Members.PointsDetail pointModel = new Model.Members.PointsDetail();
            pointModel.CreatedDate = DateTime.Now;
            pointModel.Description = desInfo;
            pointModel.ExtData = extdata;
            pointModel.Score = score;
            pointModel.RuleId = 0;
            pointModel.Type = type;
            pointModel.UserID = userid;
            return dal.AddDetail(pointModel);
        }

        #endregion

        /// <summary>
        /// 获取会员当前积分
        /// </summary> 
        /// <returns></returns>
        public int GetPointByUserid(int userid)
        {
            return dal.GetPointByUserid(userid);//获取当前积分
        }


        /// <summary>
        /// 获取会员当前积分
        /// </summary> 
        /// <returns></returns>
        public int GetPointByUsername(string Username)
        {
            int userid = userbll.GetUserIdByUserName(Username);
            return dal.GetPointByUserid(userid);//获取当前积分
        }


        /// <summary>
        /// 会员积分互转
        /// </summary>
        /// <param name="Ruleid">98表示转出，99表示转入</param>
        /// <param name="userid">会员主ID</param>
        /// <param name="desc">积分详情</param>
        /// <param name="Score">操作的积分数量</param>
        /// <param name="extdata">操作的对方会员ID</param>
        /// <param name="type">0表示增加积分，1表示减少积分</param>
        /// <returns></returns>
        public int PointsHuzhuan(int Ruleid, int userid, string desc, int Score, string extdata, int type)
        {
            PointsRule ruleBll = new PointsRule();

            Model.Members.PointsDetail pointModel = new Model.Members.PointsDetail();

            //添加积分详细信息
            pointModel.CreatedDate = DateTime.Now;
            pointModel.Description = desc;
            pointModel.ExtData = extdata;
            pointModel.Score = Score;

            pointModel.RuleId = Ruleid;
            pointModel.UserID = userid;
            pointModel.Type = type;

            pointModel.CurrentPoints = dal.GetPointByUserid(userid);//获取当前积分
            if (type == 0)//0表示增加积分
            {
                pointModel.CurrentPoints = pointModel.CurrentPoints + Score;//当前积分
            }
            else
            {
                pointModel.CurrentPoints = pointModel.CurrentPoints - Score;//当前积分
            }
            if (dal.AddDetail(pointModel))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }


    }
}

