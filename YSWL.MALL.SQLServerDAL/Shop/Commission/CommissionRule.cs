/**  版本信息模板在安装目录下，可自行修改。
* CommissionRule.cs
*
* 功 能： N/A
* 类 名： CommissionRule
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/2/13 13:59:35   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.MALL.IDAL.Shop.Commission;
using YSWL.DBUtility;//Please add references
namespace YSWL.MALL.SQLServerDAL.Shop.Commission
{
	/// <summary>
	/// 数据访问类:CommissionRule
	/// </summary>
	public partial class CommissionRule:ICommissionRule
	{
		public CommissionRule()
		{}
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DBHelper.DefaultDBHelper.GetMaxID("RuleId", "Shop_CommissionRule");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int RuleId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Shop_CommissionRule");
            strSql.Append(" where RuleId=@RuleId");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4)
			};
            parameters[0].Value = RuleId;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.MALL.Model.Shop.Commission.CommissionRule model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Shop_CommissionRule(");
            strSql.Append("RuleName,RuleMode,FirstValue,SecondValue,ThirdValue,FourthValue,FifthValue,IsAll,Status,CreatedDate,CreatedUserID)");
            strSql.Append(" values (");
            strSql.Append("@RuleName,@RuleMode,@FirstValue,@SecondValue,@ThirdValue,@FourthValue,@FifthValue,@IsAll,@Status,@CreatedDate,@CreatedUserID)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleName", SqlDbType.NVarChar,200),
					new SqlParameter("@RuleMode", SqlDbType.Int,4),
					new SqlParameter("@FirstValue", SqlDbType.Decimal,9),
					new SqlParameter("@SecondValue", SqlDbType.Decimal,9),
					new SqlParameter("@ThirdValue", SqlDbType.Decimal,9),
					new SqlParameter("@FourthValue", SqlDbType.Decimal,9),
					new SqlParameter("@FifthValue", SqlDbType.Decimal,9),
					new SqlParameter("@IsAll", SqlDbType.Bit,1),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime),
					new SqlParameter("@CreatedUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RuleName;
            parameters[1].Value = model.RuleMode;
            parameters[2].Value = model.FirstValue;
            parameters[3].Value = model.SecondValue;
            parameters[4].Value = model.ThirdValue;
            parameters[5].Value = model.FourthValue;
            parameters[6].Value = model.FifthValue;
            parameters[7].Value = model.IsAll;
            parameters[8].Value = model.Status;
            parameters[9].Value = model.CreatedDate;
            parameters[10].Value = model.CreatedUserID;

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
        public bool Update(YSWL.MALL.Model.Shop.Commission.CommissionRule model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Shop_CommissionRule set ");
            strSql.Append("RuleName=@RuleName,");
            strSql.Append("RuleMode=@RuleMode,");
            strSql.Append("FirstValue=@FirstValue,");
            strSql.Append("SecondValue=@SecondValue,");
            strSql.Append("ThirdValue=@ThirdValue,");
            strSql.Append("FourthValue=@FourthValue,");
            strSql.Append("FifthValue=@FifthValue,");
            strSql.Append("IsAll=@IsAll,");
            strSql.Append("Status=@Status,");
            strSql.Append("CreatedDate=@CreatedDate,");
            strSql.Append("CreatedUserID=@CreatedUserID");
            strSql.Append(" where RuleId=@RuleId");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleName", SqlDbType.NVarChar,200),
					new SqlParameter("@RuleMode", SqlDbType.Int,4),
					new SqlParameter("@FirstValue", SqlDbType.Decimal,9),
					new SqlParameter("@SecondValue", SqlDbType.Decimal,9),
					new SqlParameter("@ThirdValue", SqlDbType.Decimal,9),
					new SqlParameter("@FourthValue", SqlDbType.Decimal,9),
					new SqlParameter("@FifthValue", SqlDbType.Decimal,9),
					new SqlParameter("@IsAll", SqlDbType.Bit,1),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime),
					new SqlParameter("@CreatedUserID", SqlDbType.Int,4),
					new SqlParameter("@RuleId", SqlDbType.Int,4)};
            parameters[0].Value = model.RuleName;
            parameters[1].Value = model.RuleMode;
            parameters[2].Value = model.FirstValue;
            parameters[3].Value = model.SecondValue;
            parameters[4].Value = model.ThirdValue;
            parameters[5].Value = model.FourthValue;
            parameters[6].Value = model.FifthValue;
            parameters[7].Value = model.IsAll;
            parameters[8].Value = model.Status;
            parameters[9].Value = model.CreatedDate;
            parameters[10].Value = model.CreatedUserID;
            parameters[11].Value = model.RuleId;

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
        public bool Delete(int RuleId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Shop_CommissionRule ");
            strSql.Append(" where RuleId=@RuleId");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4)
			};
            parameters[0].Value = RuleId;

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
        public bool DeleteList(string RuleIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Shop_CommissionRule ");
            strSql.Append(" where RuleId in (" + RuleIdlist + ")  ");
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
        public YSWL.MALL.Model.Shop.Commission.CommissionRule GetModel(int RuleId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 RuleId,RuleName,RuleMode,FirstValue,SecondValue,ThirdValue,FourthValue,FifthValue,IsAll,Status,CreatedDate,CreatedUserID from Shop_CommissionRule ");
            strSql.Append(" where RuleId=@RuleId");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4)
			};
            parameters[0].Value = RuleId;

            YSWL.MALL.Model.Shop.Commission.CommissionRule model = new YSWL.MALL.Model.Shop.Commission.CommissionRule();
            DataSet ds = DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.Commission.CommissionRule DataRowToModel(DataRow row)
        {
            YSWL.MALL.Model.Shop.Commission.CommissionRule model = new YSWL.MALL.Model.Shop.Commission.CommissionRule();
            if (row != null)
            {
                if (row["RuleId"] != null && row["RuleId"].ToString() != "")
                {
                    model.RuleId = int.Parse(row["RuleId"].ToString());
                }
                if (row["RuleName"] != null)
                {
                    model.RuleName = row["RuleName"].ToString();
                }
                if (row["RuleMode"] != null && row["RuleMode"].ToString() != "")
                {
                    model.RuleMode = int.Parse(row["RuleMode"].ToString());
                }
                if (row["FirstValue"] != null && row["FirstValue"].ToString() != "")
                {
                    model.FirstValue = decimal.Parse(row["FirstValue"].ToString());
                }
                if (row["SecondValue"] != null && row["SecondValue"].ToString() != "")
                {
                    model.SecondValue = decimal.Parse(row["SecondValue"].ToString());
                }
                if (row["ThirdValue"] != null && row["ThirdValue"].ToString() != "")
                {
                    model.ThirdValue = decimal.Parse(row["ThirdValue"].ToString());
                }
                if (row["FourthValue"] != null && row["FourthValue"].ToString() != "")
                {
                    model.FourthValue = decimal.Parse(row["FourthValue"].ToString());
                }
                if (row["FifthValue"] != null && row["FifthValue"].ToString() != "")
                {
                    model.FifthValue = decimal.Parse(row["FifthValue"].ToString());
                }
                if (row["IsAll"] != null && row["IsAll"].ToString() != "")
                {
                    if ((row["IsAll"].ToString() == "1") || (row["IsAll"].ToString().ToLower() == "true"))
                    {
                        model.IsAll = true;
                    }
                    else
                    {
                        model.IsAll = false;
                    }
                }
                if (row["Status"] != null && row["Status"].ToString() != "")
                {
                    model.Status = int.Parse(row["Status"].ToString());
                }
                if (row["CreatedDate"] != null && row["CreatedDate"].ToString() != "")
                {
                    model.CreatedDate = DateTime.Parse(row["CreatedDate"].ToString());
                }
                if (row["CreatedUserID"] != null && row["CreatedUserID"].ToString() != "")
                {
                    model.CreatedUserID = int.Parse(row["CreatedUserID"].ToString());
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select RuleId,RuleName,RuleMode,FirstValue,SecondValue,ThirdValue,FourthValue,FifthValue,IsAll,Status,CreatedDate,CreatedUserID ");
            strSql.Append(" FROM Shop_CommissionRule ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" RuleId,RuleName,RuleMode,FirstValue,SecondValue,ThirdValue,FourthValue,FifthValue,IsAll,Status,CreatedDate,CreatedUserID ");
            strSql.Append(" FROM Shop_CommissionRule ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM Shop_CommissionRule ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
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
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.RuleId desc");
            }
            strSql.Append(")AS Row, T.*  from Shop_CommissionRule T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /*
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@tblName", SqlDbType.VarChar, 255),
                    new SqlParameter("@fldName", SqlDbType.VarChar, 255),
                    new SqlParameter("@PageSize", SqlDbType.Int),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@IsReCount", SqlDbType.Bit),
                    new SqlParameter("@OrderType", SqlDbType.Bit),
                    new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
                    };
            parameters[0].Value = "Shop_CommissionRule";
            parameters[1].Value = "RuleId";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DBHelper.DefaultDBHelper.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  BasicMethod
		#region  ExtensionMethod
        public bool DeleteListEx(string RuleIdlist)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();

            //删除规则表数据
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Shop_CommissionRule ");
            strSql.Append(" where RuleId in (" + RuleIdlist + ") ");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleIdt", SqlDbType.Int,4)};
            parameters[0].Value = 1;
            CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
            sqllist.Add(cmd);

            //删除商品规则中间表数据
            StringBuilder strSql4 = new StringBuilder();
            strSql4.Append("delete Shop_CommissionPro ");
            strSql4.Append(" where RuleId in (" + RuleIdlist + ") ");
            cmd = new CommandInfo(strSql4.ToString(), parameters);
            sqllist.Add(cmd);

            int rowsAffected = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool UpdateStatus(int RuleId, int Status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Shop_CommissionRule set ");
            strSql.Append("Status=@Status");
            strSql.Append(" where RuleId=@RuleId");
            SqlParameter[] parameters = {
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@RuleId", SqlDbType.Int,4)};
            parameters[0].Value = Status;
            parameters[1].Value = RuleId;

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

        public bool DeleteEx(int RuleId)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();

            //删除规则表数据
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete Shop_CommissionRule ");
            strSql.Append(" where RuleId=@RuleId ");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4)};
            parameters[0].Value = RuleId;
            CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
            sqllist.Add(cmd);

        

            //删除商品规则中间表数据
            StringBuilder strSql4 = new StringBuilder();
            strSql4.Append("delete Shop_CommissionPro ");
            strSql4.Append(" where  RuleId=@RuleId ");
            SqlParameter[] parameters4 = {
					new SqlParameter("@RuleId", SqlDbType.Int,4)};
            parameters4[0].Value = RuleId;
            cmd = new CommandInfo(strSql4.ToString(), parameters4);
            sqllist.Add(cmd);

            int rowsAffected = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public YSWL.MALL.Model.Shop.Commission.CommissionRule GetExistAllPro()
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Top 1 *  FROM Shop_CommissionRule WHERE IsAll=1 AND Status=1  ");
            YSWL.MALL.Model.Shop.Commission.CommissionRule model = new YSWL.MALL.Model.Shop.Commission.CommissionRule();
            DataSet ds = DBHelper.DefaultDBHelper.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
          
	    }

	    #endregion  ExtensionMethod
	}
}

