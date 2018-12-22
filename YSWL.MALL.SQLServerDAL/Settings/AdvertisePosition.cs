/*----------------------------------------------------------------
// Copyright (C) 2012 云商未来 版权所有。
//
// 文件名：AdvertisePosition.cs
// 文件功能描述：
// 
// 创建标识： [孙鹏]  2012/05/31 13:22:19
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.MALL.IDAL.Settings;
using YSWL.DBUtility;
using System.Collections.Generic;
using System.Linq;

namespace YSWL.MALL.SQLServerDAL.Settings
{
    /// <summary>
    /// 数据访问类:AdvertisePosition
    /// </summary>
    public partial class AdvertisePosition : IAdvertisePosition
    {
        public AdvertisePosition()
        { }
        #region  Method

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int AdvPositionId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(1) FROM AD_AdvertisePosition");
            strSql.Append(" WHERE AdvPositionId=@AdvPositionId");
            SqlParameter[] parameters = {
                    new SqlParameter("@AdvPositionId", SqlDbType.Int,4)
            };
            parameters[0].Value = AdvPositionId;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.MALL.Model.Settings.AdvertisePosition model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO AD_AdvertisePosition(");
            strSql.Append("AdvPositionName,ShowType,RepeatColumns,Width,Height,AdvHtml,IsOne,TimeInterval,CreatedDate,CreatedUserID)");
            strSql.Append(" VALUES (");
            strSql.Append("@AdvPositionName,@ShowType,@RepeatColumns,@Width,@Height,@AdvHtml,@IsOne,@TimeInterval,@CreatedDate,@CreatedUserID)");
            strSql.Append(";SELECT @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@AdvPositionName", SqlDbType.NVarChar,50),
                    new SqlParameter("@ShowType", SqlDbType.Int,4),
                    new SqlParameter("@RepeatColumns", SqlDbType.Int,4),
                    new SqlParameter("@Width", SqlDbType.Int,4),
                    new SqlParameter("@Height", SqlDbType.Int,4),
                    new SqlParameter("@AdvHtml", SqlDbType.NVarChar,1000),
                    new SqlParameter("@IsOne", SqlDbType.Bit,1),
                    new SqlParameter("@TimeInterval", SqlDbType.Int,4),
                    new SqlParameter("@CreatedDate", SqlDbType.DateTime),
                    new SqlParameter("@CreatedUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.AdvPositionName;
            parameters[1].Value = model.ShowType;
            parameters[2].Value = model.RepeatColumns;
            parameters[3].Value = model.Width;
            parameters[4].Value = model.Height;
            parameters[5].Value = model.AdvHtml;
            parameters[6].Value = model.IsOne;
            parameters[7].Value = model.TimeInterval;
            parameters[8].Value = model.CreatedDate;
            parameters[9].Value = model.CreatedUserID;

            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.Settings.AdvertisePosition model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE AD_AdvertisePosition SET ");
            strSql.Append("AdvPositionName=@AdvPositionName,");
            strSql.Append("ShowType=@ShowType,");
            strSql.Append("RepeatColumns=@RepeatColumns,");
            strSql.Append("Width=@Width,");
            strSql.Append("Height=@Height,");
            strSql.Append("AdvHtml=@AdvHtml,");
            strSql.Append("IsOne=@IsOne,");
            strSql.Append("TimeInterval=@TimeInterval,");
            strSql.Append("CreatedDate=@CreatedDate,");
            strSql.Append("CreatedUserID=@CreatedUserID");
            strSql.Append(" WHERE AdvPositionId=@AdvPositionId");
            SqlParameter[] parameters = {
                    new SqlParameter("@AdvPositionName", SqlDbType.NVarChar,50),
                    new SqlParameter("@ShowType", SqlDbType.Int,4),
                    new SqlParameter("@RepeatColumns", SqlDbType.Int,4),
                    new SqlParameter("@Width", SqlDbType.Int,4),
                    new SqlParameter("@Height", SqlDbType.Int,4),
                    new SqlParameter("@AdvHtml", SqlDbType.NVarChar,1000),
                    new SqlParameter("@IsOne", SqlDbType.Bit,1),
                    new SqlParameter("@TimeInterval", SqlDbType.Int,4),
                    new SqlParameter("@CreatedDate", SqlDbType.DateTime),
                    new SqlParameter("@CreatedUserID", SqlDbType.Int,4),
                    new SqlParameter("@AdvPositionId", SqlDbType.Int,4)};
            parameters[0].Value = model.AdvPositionName;
            parameters[1].Value = model.ShowType;
            parameters[2].Value = model.RepeatColumns;
            parameters[3].Value = model.Width;
            parameters[4].Value = model.Height;
            parameters[5].Value = model.AdvHtml;
            parameters[6].Value = model.IsOne;
            parameters[7].Value = model.TimeInterval;
            parameters[8].Value = model.CreatedDate;
            parameters[9].Value = model.CreatedUserID;
            parameters[10].Value = model.AdvPositionId;

            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int AdvPositionId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE FROM AD_AdvertisePosition ");
            strSql.Append(" WHERE AdvPositionId=@AdvPositionId");
            SqlParameter[] parameters = {
                    new SqlParameter("@AdvPositionId", SqlDbType.Int,4)
            };
            parameters[0].Value = AdvPositionId;

            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string AdvPositionIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE FROM AD_AdvertisePosition ");
            strSql.Append(" WHERE AdvPositionId in (" + AdvPositionIdlist + ")  ");
            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Settings.AdvertisePosition GetModel(int AdvPositionId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  TOP 1 * FROM AD_AdvertisePosition ");
            strSql.Append(" WHERE AdvPositionId=@AdvPositionId");
            SqlParameter[] parameters = {
                    new SqlParameter("@AdvPositionId", SqlDbType.Int,4)
            };
            parameters[0].Value = AdvPositionId;

            YSWL.MALL.Model.Settings.AdvertisePosition model = new YSWL.MALL.Model.Settings.AdvertisePosition();
            DataSet ds = DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["AdvPositionId"] != null && ds.Tables[0].Rows[0]["AdvPositionId"].ToString() != "")
                {
                    model.AdvPositionId = int.Parse(ds.Tables[0].Rows[0]["AdvPositionId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["AdvPositionName"] != null && ds.Tables[0].Rows[0]["AdvPositionName"].ToString() != "")
                {
                    model.AdvPositionName = ds.Tables[0].Rows[0]["AdvPositionName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["ShowType"] != null && ds.Tables[0].Rows[0]["ShowType"].ToString() != "")
                {
                    model.ShowType = int.Parse(ds.Tables[0].Rows[0]["ShowType"].ToString());
                }
                if (ds.Tables[0].Rows[0]["RepeatColumns"] != null && ds.Tables[0].Rows[0]["RepeatColumns"].ToString() != "")
                {
                    model.RepeatColumns = int.Parse(ds.Tables[0].Rows[0]["RepeatColumns"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Width"] != null && ds.Tables[0].Rows[0]["Width"].ToString() != "")
                {
                    model.Width = int.Parse(ds.Tables[0].Rows[0]["Width"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Height"] != null && ds.Tables[0].Rows[0]["Height"].ToString() != "")
                {
                    model.Height = int.Parse(ds.Tables[0].Rows[0]["Height"].ToString());
                }
                if (ds.Tables[0].Rows[0]["AdvHtml"] != null && ds.Tables[0].Rows[0]["AdvHtml"].ToString() != "")
                {
                    model.AdvHtml = ds.Tables[0].Rows[0]["AdvHtml"].ToString();
                }
                if (ds.Tables[0].Rows[0]["IsOne"] != null && ds.Tables[0].Rows[0]["IsOne"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["IsOne"].ToString() == "1") || (ds.Tables[0].Rows[0]["IsOne"].ToString().ToLower() == "true"))
                    {
                        model.IsOne = true;
                    }
                    else
                    {
                        model.IsOne = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["TimeInterval"] != null && ds.Tables[0].Rows[0]["TimeInterval"].ToString() != "")
                {
                    model.TimeInterval = int.Parse(ds.Tables[0].Rows[0]["TimeInterval"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CreatedDate"] != null && ds.Tables[0].Rows[0]["CreatedDate"].ToString() != "")
                {
                    model.CreatedDate = DateTime.Parse(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CreatedUserID"] != null && ds.Tables[0].Rows[0]["CreatedUserID"].ToString() != "")
                {
                    model.CreatedUserID = int.Parse(ds.Tables[0].Rows[0]["CreatedUserID"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT AdvPositionId,AdvPositionName,ShowType,RepeatColumns,Width,Height,AdvHtml,IsOne,TimeInterval,CreatedDate,CreatedUserID ");
            strSql.Append(" FROM AD_AdvertisePosition ");
            if (!string.IsNullOrWhiteSpace(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT ");
            if (Top > 0)
            {
                strSql.Append(" TOP " + Top.ToString());
            }
            strSql.Append(" AdvPositionId,AdvPositionName,ShowType,RepeatColumns,Width,Height,AdvHtml,IsOne,TimeInterval,CreatedDate,CreatedUserID ");
            strSql.Append(" FROM AD_AdvertisePosition ");
            if (!string.IsNullOrWhiteSpace(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ORDER BY " + filedOrder);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(1) FROM AD_AdvertisePosition ");
            if (!string.IsNullOrWhiteSpace(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrWhiteSpace(orderby.Trim()))
            {
                strSql.Append("ORDER BY T." + orderby);
            }
            else
            {
                strSql.Append("ORDER BY T.AdvPositionId desc");
            }
            strSql.Append(")AS Row, T.*  FROM AD_AdvertisePosition T ");
            if (!string.IsNullOrWhiteSpace(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row BETWEEN {0} AND {1}", startIndex, endIndex);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.NVarChar, 255),
					new SqlParameter("@fldName", SqlDbType.NVarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.NVarChar,1000),
					};
			parameters[0].Value = "AD_AdvertisePosition";
			parameters[1].Value = "AdvPositionId";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DBHelper.DefaultDBHelper.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

        #endregion  Method

        #region saas app

        /// <summary>
        /// 编辑广告位
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateAdvPostion(ViewModel.AdvertisePositionVm model)
        {
            using (SqlConnection connection = DBHelper.DefaultDBHelper.GetDBObject().GetConnection)
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        //编辑广告位信息
                        DBHelper.DefaultDBHelper.GetSingle4Trans(UpdateAdvPostion(new Model.Settings.AdvertisePosition
                        {
                            AdvPositionName = model.Name,
                            AdvPositionId = model.Id,
                            IsOne = model.IsOne
                        }), transaction);

                        string advIds = string.Empty;
                        List<Model.Settings.Advertisement> advList = new List<Model.Settings.Advertisement>();
                        if (model.Advertisement != null && model.Advertisement.Count > 0)
                        {
                            advIds = string.Join(",", model.Advertisement.Where(t => t.Id > 0).Select(t => t.Id));
                            advList = model.Advertisement
                                .Select(t => new Model.Settings.Advertisement
                                {
                                    FileUrl = t.Image,
                                    NavigateUrl = t.Url,
                                    AdvertisementId = t.Id,
                                    AdvPositionId = model.Id
                                }).ToList();
                        }

                        DBHelper.DefaultDBHelper.GetSingle4Trans(DeleteAdv(model.Id, advIds), transaction);
                        //编辑广告位广告信息
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(UpdateAdv(advList.Where(t => t.AdvertisementId > 0).ToList()), transaction);
                        //新增广告位广告信息
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(AddAdv(advList.Where(t => t.AdvertisementId < 0).ToList()), transaction);
                        transaction.Commit();
                    }
                    catch (SqlException ex)
                    {
                        Log.LogHelper.AddErrorLog("编辑广告位信息失败", ex.Message + "------" + ex.StackTrace);
                        transaction.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 编辑广告位信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CommandInfo UpdateAdvPostion(Model.Settings.AdvertisePosition model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE AD_AdvertisePosition SET ");
            strSql.Append("AdvPositionName=@AdvPositionName,");
            strSql.Append("IsOne=@IsOne");
            strSql.Append(" WHERE AdvPositionId=@AdvPositionId");
            SqlParameter[] parameters = {
                    new SqlParameter("@AdvPositionName", SqlDbType.NVarChar,50),
                    new SqlParameter("@IsOne", SqlDbType.Bit,1),
                    new SqlParameter("@AdvPositionId", SqlDbType.Int,4)};
            parameters[0].Value = model.AdvPositionName;
            parameters[1].Value = model.IsOne ? 1 : 0;
            parameters[2].Value = model.AdvPositionId;

            return new CommandInfo(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 新增广告相关信息
        /// </summary>
        /// <param name="modelList"></param>
        /// <returns></returns>
        public List<CommandInfo> AddAdv(List<Model.Settings.Advertisement> modelList)
        {
            List<CommandInfo> list = new List<CommandInfo>();
            foreach (Model.Settings.Advertisement model in modelList)
            {
                //StringBuilder strSql = new StringBuilder();
                //strSql.Append("INSERT INTO AD_Advertisement(");
                //strSql.Append("AdvPositionId,FileUrl,NavigateUrl,CreatedDate,ContentType)");
                //strSql.Append(" VALUES (");
                //strSql.Append("@AdvPositionId,@FileUrl,@NavigateUrl,@CreatedDate,@ContentType)");
                //SqlParameter[] parameters = {
                //    new SqlParameter("@AdvPositionId", SqlDbType.Int,4),
                //    new SqlParameter("@FileUrl", SqlDbType.NVarChar,200),
                //    new SqlParameter("@NavigateUrl", SqlDbType.NVarChar,200),
                //    new SqlParameter("@CreatedDate", SqlDbType.DateTime),
                //    new SqlParameter("@ContentType", SqlDbType.Int,4)
                //};
                //parameters[0].Value = model.AdvPositionId;
                //parameters[1].Value = model.FileUrl;
                //parameters[2].Value = model.NavigateUrl;
                //parameters[3].Value = DateTime.Now;
                //parameters[4].Value = 1;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("INSERT INTO AD_Advertisement(");
                strSql.Append("AdvertisementName,AdvPositionId,ContentType,FileUrl,AlternateText,NavigateUrl,AdvHtml,Impressions,CreatedDate,CreatedUserID,State,StartDate,EndDate,DayMaxPV,DayMaxIP,CPMPrice,AutoStop,Sequence,EnterpriseID)");
                strSql.Append(" VALUES (");
                strSql.Append("@AdvertisementName,@AdvPositionId,@ContentType,@FileUrl,@AlternateText,@NavigateUrl,@AdvHtml,@Impressions,@CreatedDate,@CreatedUserID,@State,@StartDate,@EndDate,@DayMaxPV,@DayMaxIP,@CPMPrice,@AutoStop,@Sequence,@EnterpriseID)");
                strSql.Append(";SELECT @@IDENTITY");
                SqlParameter[] parameters = {
                    new SqlParameter("@AdvertisementName", SqlDbType.NVarChar,50),
                    new SqlParameter("@AdvPositionId", SqlDbType.Int,4),
                    new SqlParameter("@ContentType", SqlDbType.Int,4),
                    new SqlParameter("@FileUrl", SqlDbType.NVarChar,200),
                    new SqlParameter("@AlternateText", SqlDbType.NVarChar,200),
                    new SqlParameter("@NavigateUrl", SqlDbType.NVarChar,200),
                    new SqlParameter("@AdvHtml", SqlDbType.NVarChar),
                    new SqlParameter("@Impressions", SqlDbType.Int,4),
                    new SqlParameter("@CreatedDate", SqlDbType.DateTime),
                    new SqlParameter("@CreatedUserID", SqlDbType.Int,4),
                    new SqlParameter("@State", SqlDbType.Int,4),
                    new SqlParameter("@StartDate", SqlDbType.DateTime),
                    new SqlParameter("@EndDate", SqlDbType.DateTime),
                    new SqlParameter("@DayMaxPV", SqlDbType.Int,4),
                    new SqlParameter("@DayMaxIP", SqlDbType.Int,4),
                    new SqlParameter("@CPMPrice", SqlDbType.Money,8),
                    new SqlParameter("@AutoStop", SqlDbType.Int,4),
                    new SqlParameter("@Sequence", SqlDbType.Int,4),
                    new SqlParameter("@EnterpriseID", SqlDbType.Int,4)};
                parameters[0].Value = "";
                parameters[1].Value = model.AdvPositionId;
                parameters[2].Value = 1;
                parameters[3].Value = model.FileUrl;
                parameters[4].Value = "";
                parameters[5].Value = model.NavigateUrl;
                parameters[6].Value = "";
                parameters[7].Value = -1;
                parameters[8].Value = DateTime.Now;
                parameters[9].Value = 0;
                parameters[10].Value = 1;
                parameters[11].Value = model.StartDate;
                parameters[12].Value = model.EndDate;
                parameters[13].Value = 0;
                parameters[14].Value = 0;
                parameters[15].Value = 0;
                parameters[16].Value = -1;
                parameters[17].Value = 0;
                parameters[18].Value = 0;
                list.Add(new CommandInfo(strSql.ToString(), parameters));
            }
            return list;
        }

        /// <summary>
        /// 删除广告相关信息
        /// </summary>
        /// <returns></returns>
        public CommandInfo DeleteAdv(int postionId, string advids)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from AD_Advertisement ");
            strSql.Append(" WHERE AdvPositionId=@AdvPositionId");
            if (!string.IsNullOrEmpty(advids))
            {
                strSql.AppendFormat(" and AdvertisementId not in({0})", advids);
            }
            SqlParameter[] parameters ={
                new SqlParameter("@AdvPositionId", SqlDbType.Int,4)
            };
            parameters[0].Value = postionId;
            return new CommandInfo(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 编辑广告信息
        /// </summary>
        /// <param name="modelList"></param>
        /// <returns></returns>
        public List<CommandInfo> UpdateAdv(List<Model.Settings.Advertisement> modelList)
        {
            List<CommandInfo> list = new List<CommandInfo>();
            foreach (Model.Settings.Advertisement model in modelList)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("UPDATE AD_Advertisement SET ");
                strSql.Append("FileUrl=@FileUrl,");
                strSql.Append("NavigateUrl=@NavigateUrl");
                strSql.Append(" WHERE AdvertisementId=@AdvertisementId");
                SqlParameter[] parameters = {
                    new SqlParameter("@FileUrl", SqlDbType.NVarChar,200),
                    new SqlParameter("@NavigateUrl", SqlDbType.NVarChar,200),
                    new SqlParameter("@AdvertisementId", SqlDbType.Int,4)
                };

                parameters[0].Value = model.FileUrl;
                parameters[1].Value = model.NavigateUrl;
                parameters[2].Value = model.AdvertisementId;

                list.Add(new CommandInfo(strSql.ToString(), parameters));
            }
            return list;
        }
        #endregion
    }
}

