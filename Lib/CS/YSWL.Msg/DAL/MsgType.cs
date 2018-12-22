using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.DBUtility;
using YSWL.Msg.Model;

//Please add references
namespace YSWL.Msg.DAL
{
    /// <summary>
    /// 数据访问类:MsgType
    /// </summary>
    public partial class MsgType
    {
        public MsgType()
        {}
        #region  Method

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
        return DbHelperSQL.GetMaxID("ID", "MsgType"); 
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql=new StringBuilder();
            strSql.Append("select count(1) from MsgType");
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
        public int Add(YSWL.Msg.Model.MsgType model)
        {
            StringBuilder strSql=new StringBuilder();
            strSql.Append("insert into MsgType(");
            strSql.Append("Title,UserType,Remark,Other)");
            strSql.Append(" values (");
            strSql.Append("@Title,@UserType,@Remark,@Other)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@Title", SqlDbType.NVarChar,50),
                    new SqlParameter("@UserType", SqlDbType.NVarChar,50),
                    new SqlParameter("@Remark", SqlDbType.NVarChar,100),
                    new SqlParameter("@Other", SqlDbType.NVarChar,100)};
            parameters[0].Value = model.Title;
            parameters[1].Value = model.UserType;
            parameters[2].Value = model.Remark;
            parameters[3].Value = model.Other;

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
        public bool Update(YSWL.Msg.Model.MsgType model)
        {
            StringBuilder strSql=new StringBuilder();
            strSql.Append("update MsgType set ");
            strSql.Append("Title=@Title,");
            strSql.Append("UserType=@UserType,");
            strSql.Append("Remark=@Remark,");
            strSql.Append("Other=@Other");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@Title", SqlDbType.NVarChar,50),
                    new SqlParameter("@UserType", SqlDbType.NVarChar,50),
                    new SqlParameter("@Remark", SqlDbType.NVarChar,100),
                    new SqlParameter("@Other", SqlDbType.NVarChar,100),
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = model.Title;
            parameters[1].Value = model.UserType;
            parameters[2].Value = model.Remark;
            parameters[3].Value = model.Other;
            parameters[4].Value = model.ID;

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
            strSql.Append("delete from MsgType ");
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
            strSql.Append("delete from MsgType ");
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
        public YSWL.Msg.Model.MsgType GetModel(int ID)
        {
            
            StringBuilder strSql=new StringBuilder();
            strSql.Append("select  top 1 ID,Title,UserType,Remark,Other from MsgType ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)
            };
            parameters[0].Value = ID;

            YSWL.Msg.Model.MsgType model=new YSWL.Msg.Model.MsgType();
            DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
            if(ds.Tables[0].Rows.Count>0)
            {
                if(ds.Tables[0].Rows[0]["ID"]!=null && ds.Tables[0].Rows[0]["ID"].ToString()!="")
                {
                    model.ID=int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                }
                if(ds.Tables[0].Rows[0]["Title"]!=null && ds.Tables[0].Rows[0]["Title"].ToString()!="")
                {
                    model.Title=ds.Tables[0].Rows[0]["Title"].ToString();
                }
                if(ds.Tables[0].Rows[0]["UserType"]!=null && ds.Tables[0].Rows[0]["UserType"].ToString()!="")
                {
                    model.UserType=ds.Tables[0].Rows[0]["UserType"].ToString();
                }
                if(ds.Tables[0].Rows[0]["Remark"]!=null && ds.Tables[0].Rows[0]["Remark"].ToString()!="")
                {
                    model.Remark=ds.Tables[0].Rows[0]["Remark"].ToString();
                }
                if(ds.Tables[0].Rows[0]["Other"]!=null && ds.Tables[0].Rows[0]["Other"].ToString()!="")
                {
                    model.Other=ds.Tables[0].Rows[0]["Other"].ToString();
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
            StringBuilder strSql=new StringBuilder();
            strSql.Append("select ID,Title,UserType,Remark,Other ");
            strSql.Append(" FROM MsgType ");
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
            strSql.Append(" ID,Title,UserType,Remark,Other ");
            strSql.Append(" FROM MsgType ");
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
            strSql.Append("select count(1) FROM MsgType ");
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
            strSql.Append(")AS Row, T.*  from MsgType T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.Query(strSql.ToString());
        }

        #endregion  Method
    }
}

