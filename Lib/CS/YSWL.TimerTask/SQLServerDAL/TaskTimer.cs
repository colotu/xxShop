/**
* TaskTimers.cs
*
* 功 能： N/A
* 类 名： TaskTimers
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/8/21 21:28:54   Ben    初版
*
* Copyright (c) 2012-2013 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using YSWL.DBUtility;

//Please add references
namespace YSWL.TimerTask.SQLServerDAL
{
	/// <summary>
	/// 数据访问类:TaskTimers
	/// </summary>
	public partial class TaskTimer:DAL.ITaskTimer
	{
		public TaskTimer()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("ID", "Ms_TaskTimers"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Ms_TaskTimers");
			strSql.Append(" where ID=@ID");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
			parameters[0].Value = ID;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(Model.TaskTimer model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Ms_TaskTimers(");
			strSql.Append("ExecuteType,IsSingle,Interval,ExecuteTime,ExecuteNumber,Param1,Param2,Param3,Param4,Param5,Param6,Param7,Param8,Param9,Param10)");
			strSql.Append(" values (");
			strSql.Append("@ExecuteType,@IsSingle,@Interval,@ExecuteTime,@ExecuteNumber,@Param1,@Param2,@Param3,@Param4,@Param5,@Param6,@Param7,@Param8,@Param9,@Param10)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@ExecuteType", SqlDbType.NVarChar,2000),
					new SqlParameter("@IsSingle", SqlDbType.Bit,1),
					new SqlParameter("@Interval", SqlDbType.Decimal,9),
					new SqlParameter("@ExecuteTime", SqlDbType.DateTime),
					new SqlParameter("@ExecuteNumber", SqlDbType.Int,4),
					new SqlParameter("@Param1", SqlDbType.NVarChar,4000),
					new SqlParameter("@Param2", SqlDbType.NVarChar,4000),
					new SqlParameter("@Param3", SqlDbType.NVarChar,4000),
					new SqlParameter("@Param4", SqlDbType.NVarChar,4000),
					new SqlParameter("@Param5", SqlDbType.NVarChar,4000),
					new SqlParameter("@Param6", SqlDbType.NVarChar,4000),
					new SqlParameter("@Param7", SqlDbType.NVarChar,4000),
					new SqlParameter("@Param8", SqlDbType.NVarChar,4000),
					new SqlParameter("@Param9", SqlDbType.NVarChar,4000),
					new SqlParameter("@Param10", SqlDbType.NVarChar,4000)};
			parameters[0].Value = model.ExecuteType;
			parameters[1].Value = model.IsSingle;
			parameters[2].Value = model.Interval;
			parameters[3].Value = model.ExecuteTime;
			parameters[4].Value = model.ExecuteNumber;
			parameters[5].Value = model.Param1;
			parameters[6].Value = model.Param2;
			parameters[7].Value = model.Param3;
			parameters[8].Value = model.Param4;
			parameters[9].Value = model.Param5;
			parameters[10].Value = model.Param6;
			parameters[11].Value = model.Param7;
			parameters[12].Value = model.Param8;
			parameters[13].Value = model.Param9;
			parameters[14].Value = model.Param10;

			object obj = DbHelperSQL.GetSingle(strSql.ToString(),parameters);
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
		public bool Update(Model.TaskTimer model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Ms_TaskTimers set ");
			strSql.Append("ExecuteType=@ExecuteType,");
			strSql.Append("IsSingle=@IsSingle,");
			strSql.Append("Interval=@Interval,");
			strSql.Append("ExecuteTime=@ExecuteTime,");
			strSql.Append("ExecuteNumber=@ExecuteNumber,");
			strSql.Append("Param1=@Param1,");
			strSql.Append("Param2=@Param2,");
			strSql.Append("Param3=@Param3,");
			strSql.Append("Param4=@Param4,");
			strSql.Append("Param5=@Param5,");
			strSql.Append("Param6=@Param6,");
			strSql.Append("Param7=@Param7,");
			strSql.Append("Param8=@Param8,");
			strSql.Append("Param9=@Param9,");
			strSql.Append("Param10=@Param10");
			strSql.Append(" where ID=@ID");
			SqlParameter[] parameters = {
					new SqlParameter("@ExecuteType", SqlDbType.NVarChar,2000),
					new SqlParameter("@IsSingle", SqlDbType.Bit,1),
					new SqlParameter("@Interval", SqlDbType.Decimal,9),
					new SqlParameter("@ExecuteTime", SqlDbType.DateTime),
					new SqlParameter("@ExecuteNumber", SqlDbType.Int,4),
					new SqlParameter("@Param1", SqlDbType.NVarChar,4000),
					new SqlParameter("@Param2", SqlDbType.NVarChar,4000),
					new SqlParameter("@Param3", SqlDbType.NVarChar,4000),
					new SqlParameter("@Param4", SqlDbType.NVarChar,4000),
					new SqlParameter("@Param5", SqlDbType.NVarChar,4000),
					new SqlParameter("@Param6", SqlDbType.NVarChar,4000),
					new SqlParameter("@Param7", SqlDbType.NVarChar,4000),
					new SqlParameter("@Param8", SqlDbType.NVarChar,4000),
					new SqlParameter("@Param9", SqlDbType.NVarChar,4000),
					new SqlParameter("@Param10", SqlDbType.NVarChar,4000),
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = model.ExecuteType;
			parameters[1].Value = model.IsSingle;
			parameters[2].Value = model.Interval;
			parameters[3].Value = model.ExecuteTime;
			parameters[4].Value = model.ExecuteNumber;
			parameters[5].Value = model.Param1;
			parameters[6].Value = model.Param2;
			parameters[7].Value = model.Param3;
			parameters[8].Value = model.Param4;
			parameters[9].Value = model.Param5;
			parameters[10].Value = model.Param6;
			parameters[11].Value = model.Param7;
			parameters[12].Value = model.Param8;
			parameters[13].Value = model.Param9;
			parameters[14].Value = model.Param10;
			parameters[15].Value = model.ID;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
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
		public bool Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Ms_TaskTimers ");
			strSql.Append(" where ID=@ID");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
			parameters[0].Value = ID;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
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
		public bool DeleteList(string IDlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Ms_TaskTimers ");
			strSql.Append(" where ID in ("+IDlist + ")  ");
			int rows=DbHelperSQL.ExecuteSql(strSql.ToString());
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
		public Model.TaskTimer GetModel(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,ExecuteType,IsSingle,Interval,ExecuteTime,ExecuteNumber,Param1,Param2,Param3,Param4,Param5,Param6,Param7,Param8,Param9,Param10 from Ms_TaskTimers ");
			strSql.Append(" where ID=@ID");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
			parameters[0].Value = ID;

			Model.TaskTimer model=new Model.TaskTimer();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
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
		public Model.TaskTimer DataRowToModel(DataRow row)
		{
			Model.TaskTimer model=new Model.TaskTimer();
			if (row != null)
			{
				if(row["ID"]!=null && row["ID"].ToString()!="")
				{
					model.ID=int.Parse(row["ID"].ToString());
				}
				if(row["ExecuteType"]!=null)
				{
					model.ExecuteType=row["ExecuteType"].ToString();
				}
				if(row["IsSingle"]!=null && row["IsSingle"].ToString()!="")
				{
					if((row["IsSingle"].ToString()=="1")||(row["IsSingle"].ToString().ToLower()=="true"))
					{
						model.IsSingle=true;
					}
					else
					{
						model.IsSingle=false;
					}
				}
				if(row["Interval"]!=null && row["Interval"].ToString()!="")
				{
					model.Interval=decimal.Parse(row["Interval"].ToString());
				}
				if(row["ExecuteTime"]!=null && row["ExecuteTime"].ToString()!="")
				{
					model.ExecuteTime=DateTime.Parse(row["ExecuteTime"].ToString());
				}
				if(row["ExecuteNumber"]!=null && row["ExecuteNumber"].ToString()!="")
				{
					model.ExecuteNumber=int.Parse(row["ExecuteNumber"].ToString());
				}
				if(row["Param1"]!=null)
				{
					model.Param1=row["Param1"].ToString();
				}
				if(row["Param2"]!=null)
				{
					model.Param2=row["Param2"].ToString();
				}
				if(row["Param3"]!=null)
				{
					model.Param3=row["Param3"].ToString();
				}
				if(row["Param4"]!=null)
				{
					model.Param4=row["Param4"].ToString();
				}
				if(row["Param5"]!=null)
				{
					model.Param5=row["Param5"].ToString();
				}
				if(row["Param6"]!=null)
				{
					model.Param6=row["Param6"].ToString();
				}
				if(row["Param7"]!=null)
				{
					model.Param7=row["Param7"].ToString();
				}
				if(row["Param8"]!=null)
				{
					model.Param8=row["Param8"].ToString();
				}
				if(row["Param9"]!=null)
				{
					model.Param9=row["Param9"].ToString();
				}
				if(row["Param10"]!=null)
				{
					model.Param10=row["Param10"].ToString();
				}
			}
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ID,ExecuteType,IsSingle,Interval,ExecuteTime,ExecuteNumber,Param1,Param2,Param3,Param4,Param5,Param6,Param7,Param8,Param9,Param10 ");
			strSql.Append(" FROM Ms_TaskTimers ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" ID,ExecuteType,IsSingle,Interval,ExecuteTime,ExecuteNumber,Param1,Param2,Param3,Param4,Param5,Param6,Param7,Param8,Param9,Param10 ");
			strSql.Append(" FROM Ms_TaskTimers ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM Ms_TaskTimers ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			object obj = DbHelperSQL.GetSingle(strSql.ToString());
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
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT * FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby );
			}
			else
			{
				strSql.Append("order by T.ID desc");
			}
			strSql.Append(")AS Row, T.*  from Ms_TaskTimers T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			return DbHelperSQL.Query(strSql.ToString());
		}

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

