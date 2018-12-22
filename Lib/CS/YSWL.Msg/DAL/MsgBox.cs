using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.DBUtility;
using YSWL.Msg.Model;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

//Please add references
namespace YSWL.Msg.DAL
{
    /// <summary>
    /// 数据访问类:MsgBox
    /// </summary>
    public partial class MsgBox
    { 
        private Database database = DatabaseFactory.CreateDatabase();
        public MsgBox()
        { }
        #region  Method

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperSQL.GetMaxID("ID", "MsgBox");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from MsgBox");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)
};
            parameters[0].Value = ID;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.Msg.Model.MsgBox model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into MsgBox(");
            strSql.Append("SenderID,ReceiverID,Title,Content,MsgType,SendTime,ReadTime,ReMark,Other,ReceiverIsRead,SenderIsDel,ReaderIsDel)");
            strSql.Append(" values (");
            strSql.Append("@SenderID,@ReceiverID,@Title,@Content,@MsgType,@SendTime,@ReadTime,@ReMark,@Other,@ReceiverIsRead,@SenderIsDel,@ReaderIsDel)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@SenderID", SqlDbType.NVarChar,100),
                    new SqlParameter("@ReceiverID", SqlDbType.NVarChar,100),
                    new SqlParameter("@Title", SqlDbType.NVarChar,100),
                    new SqlParameter("@Content", SqlDbType.NVarChar,4000),
                    new SqlParameter("@MsgType", SqlDbType.NVarChar,50),
                    new SqlParameter("@SendTime", SqlDbType.DateTime),
                    new SqlParameter("@ReadTime", SqlDbType.DateTime),
                    new SqlParameter("@ReMark", SqlDbType.NVarChar,100),
                    new SqlParameter("@Other", SqlDbType.NVarChar,100),
                    new SqlParameter("@ReceiverIsRead", SqlDbType.Bit,1),
                    new SqlParameter("@SenderIsDel", SqlDbType.Bit,1),
                    new SqlParameter("@ReaderIsDel", SqlDbType.Bit,1)};
            parameters[0].Value = model.SenderID;
            parameters[1].Value = model.ReceiverID;
            parameters[2].Value = model.Title;
            parameters[3].Value = model.Content;
            parameters[4].Value = model.MsgType;
            parameters[5].Value = model.SendTime;
            parameters[6].Value = model.ReadTime;
            parameters[7].Value = model.ReMark;
            parameters[8].Value = model.Other;
            parameters[9].Value = model.ReceiverIsRead;
            parameters[10].Value = model.SenderIsDel;
            parameters[11].Value = model.ReaderIsDel;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
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
        public bool Update(YSWL.Msg.Model.MsgBox model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update MsgBox set ");
            strSql.Append("SenderID=@SenderID,");
            strSql.Append("ReceiverID=@ReceiverID,");
            strSql.Append("Title=@Title,");
            strSql.Append("Content=@Content,");
            strSql.Append("MsgType=@MsgType,");
            strSql.Append("SendTime=@SendTime,");
            strSql.Append("ReadTime=@ReadTime,");
            strSql.Append("ReMark=@ReMark,");
            strSql.Append("Other=@Other,");
            strSql.Append("ReceiverIsRead=@ReceiverIsRead,");
            strSql.Append("SenderIsDel=@SenderIsDel,");
            strSql.Append("ReaderIsDel=@ReaderIsDel");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@SenderID", SqlDbType.NVarChar,100),
                    new SqlParameter("@ReceiverID", SqlDbType.NVarChar,100),
                    new SqlParameter("@Title", SqlDbType.NVarChar,100),
                    new SqlParameter("@Content", SqlDbType.NVarChar,4000),
                    new SqlParameter("@MsgType", SqlDbType.NVarChar,50),
                    new SqlParameter("@SendTime", SqlDbType.DateTime),
                    new SqlParameter("@ReadTime", SqlDbType.DateTime),
                    new SqlParameter("@ReMark", SqlDbType.NVarChar,100),
                    new SqlParameter("@Other", SqlDbType.NVarChar,100),
                    new SqlParameter("@ReceiverIsRead", SqlDbType.Bit,1),
                    new SqlParameter("@SenderIsDel", SqlDbType.Bit,1),
                    new SqlParameter("@ReaderIsDel", SqlDbType.Bit,1),
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = model.SenderID;
            parameters[1].Value = model.ReceiverID;
            parameters[2].Value = model.Title;
            parameters[3].Value = model.Content;
            parameters[4].Value = model.MsgType;
            parameters[5].Value = model.SendTime;
            parameters[6].Value = model.ReadTime;
            parameters[7].Value = model.ReMark;
            parameters[8].Value = model.Other;
            parameters[9].Value = model.ReceiverIsRead;
            parameters[10].Value = model.SenderIsDel;
            parameters[11].Value = model.ReaderIsDel;
            parameters[12].Value = model.ID;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
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

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from MsgBox ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)
};
            parameters[0].Value = ID;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
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
        public bool DeleteList(string IDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from MsgBox ");
            strSql.Append(" where ID in (" + IDlist + ")  ");
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
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
        public YSWL.Msg.Model.MsgBox GetModel(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,SenderID,ReceiverID,Title,Content,MsgType,SendTime,ReadTime,ReMark,Other,ReceiverIsRead,SenderIsDel,ReaderIsDel from MsgBox ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)
};
            parameters[0].Value = ID;

            YSWL.Msg.Model.MsgBox model = new YSWL.Msg.Model.MsgBox();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ID"] != null && ds.Tables[0].Rows[0]["ID"].ToString() != "")
                {
                    model.ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["SenderID"] != null && ds.Tables[0].Rows[0]["SenderID"].ToString() != "")
                {
                    model.SenderID = ds.Tables[0].Rows[0]["SenderID"].ToString();
                }
                if (ds.Tables[0].Rows[0]["ReceiverID"] != null && ds.Tables[0].Rows[0]["ReceiverID"].ToString() != "")
                {
                    model.ReceiverID = ds.Tables[0].Rows[0]["ReceiverID"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Title"] != null && ds.Tables[0].Rows[0]["Title"].ToString() != "")
                {
                    model.Title = ds.Tables[0].Rows[0]["Title"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Content"] != null && ds.Tables[0].Rows[0]["Content"].ToString() != "")
                {
                    model.Content = ds.Tables[0].Rows[0]["Content"].ToString();
                }
                if (ds.Tables[0].Rows[0]["MsgType"] != null && ds.Tables[0].Rows[0]["MsgType"].ToString() != "")
                {
                    model.MsgType = ds.Tables[0].Rows[0]["MsgType"].ToString();
                }
                if (ds.Tables[0].Rows[0]["SendTime"] != null && ds.Tables[0].Rows[0]["SendTime"].ToString() != "")
                {
                    model.SendTime = DateTime.Parse(ds.Tables[0].Rows[0]["SendTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ReadTime"] != null && ds.Tables[0].Rows[0]["ReadTime"].ToString() != "")
                {
                    model.ReadTime = DateTime.Parse(ds.Tables[0].Rows[0]["ReadTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ReMark"] != null && ds.Tables[0].Rows[0]["ReMark"].ToString() != "")
                {
                    model.ReMark = ds.Tables[0].Rows[0]["ReMark"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Other"] != null && ds.Tables[0].Rows[0]["Other"].ToString() != "")
                {
                    model.Other = ds.Tables[0].Rows[0]["Other"].ToString();
                }
                if (ds.Tables[0].Rows[0]["ReceiverIsRead"] != null && ds.Tables[0].Rows[0]["ReceiverIsRead"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["ReceiverIsRead"].ToString() == "1") || (ds.Tables[0].Rows[0]["ReceiverIsRead"].ToString().ToLower() == "true"))
                    {
                        model.ReceiverIsRead = true;
                    }
                    else
                    {
                        model.ReceiverIsRead = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["SenderIsDel"] != null && ds.Tables[0].Rows[0]["SenderIsDel"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["SenderIsDel"].ToString() == "1") || (ds.Tables[0].Rows[0]["SenderIsDel"].ToString().ToLower() == "true"))
                    {
                        model.SenderIsDel = true;
                    }
                    else
                    {
                        model.SenderIsDel = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["ReaderIsDel"] != null && ds.Tables[0].Rows[0]["ReaderIsDel"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["ReaderIsDel"].ToString() == "1") || (ds.Tables[0].Rows[0]["ReaderIsDel"].ToString().ToLower() == "true"))
                    {
                        model.ReaderIsDel = true;
                    }
                    else
                    {
                        model.ReaderIsDel = false;
                    }
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
            strSql.Append("select ID,SenderID,ReceiverID,Title,Content,MsgType,SendTime,ReadTime,ReMark,Other,ReceiverIsRead,SenderIsDel,ReaderIsDel ");
            strSql.Append(" FROM MsgBox ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
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
            strSql.Append(" ID,SenderID,ReceiverID,Title,Content,MsgType,SendTime,ReadTime,ReMark,Other,ReceiverIsRead,SenderIsDel,ReaderIsDel ");
            strSql.Append(" FROM MsgBox ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }


        /// <summary>
        /// 根据存储过程和参数填入相应的值，如果存储过程中不存在某个参数，如果是string类型则赋null 如果是int 则赋0
        /// </summary>
        /// <param name="procedureName">存储过程的名称</param>
        /// <param name="AdminId">管理员的id</param>
        /// <param name="UserType">用户的类型</param>
        /// <param name="ReceiverID">接受者的id</param>
        /// <param name="SenderID">发送者的id</param>
        /// <param name="ID">id（删除和修改的时候用到 主键）</param>
        /// <returns>返回值</returns>
        public int ReturnDataCountByProcedure(string procedureName, string AdminId, string UserType, string ReceiverID, string SenderID,int ID)
        {
            if (!string.IsNullOrEmpty(procedureName))
            {
                DbCommand storedProcCommand = this.database.GetStoredProcCommand(procedureName);
                if (!string.IsNullOrEmpty(AdminId))
                {
                    this.database.AddInParameter(storedProcCommand, "AdminId", DbType.String, AdminId);
                }
                if (!string.IsNullOrEmpty(UserType))
                {
                    this.database.AddInParameter(storedProcCommand, "UserType", DbType.String, UserType);
                }
                if (!string.IsNullOrEmpty(ReceiverID))
                {
                    this.database.AddInParameter(storedProcCommand, "ReceiverID", DbType.String, ReceiverID);

                }
                if (!string.IsNullOrEmpty(SenderID))
                {
                    this.database.AddInParameter(storedProcCommand, "SenderID", DbType.String, SenderID);
                }
                if (ID!=0)
                {
                    this.database.AddInParameter(storedProcCommand, "ID", DbType.Int32, ID);
                }

                this.database.AddParameter(storedProcCommand, "RETURN_VALUE", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
                this.database.ExecuteNonQuery(storedProcCommand);
              
                return (int)this.database.GetParameterValue(storedProcCommand, "RETURN_VALUE");
            }
            return 0;
        
        }





        /// <summary>
        /// 根据存储过程和参数填入相应的值，如果存储过程中不存在某个参数，如果是string类型则赋null 如果是int 则赋0
        /// </summary>
        /// <param name="procedureName">存储过程的名称</param>
        /// <param name="AdminId">管理员的id</param>
        /// <param name="UserType">用户的类型</param>
        /// <param name="ReceiverID">接受者的id</param>
        /// <param name="SenderID">发送者的id</param>
        /// <returns>返回值</returns>
        public int ReturnDataCountByProcedure(string procedureName, string AdminId, string UserType, string ReceiverID, string SenderID)
        {

           return this.ReturnDataCountByProcedure(procedureName,AdminId,UserType,ReceiverID,SenderID,0);


        }

        /// <summary>
        /// 根据存储过程和参数填入相应的值，如果存储过程中不存在某个参数，如果是string类型则赋null 如果是int 则赋0
        /// </summary>
        /// <param name="procedureName">存储过程的名称</param>
        /// <param name="AdminId">管理员的id</param>
        /// <param name="UserType">用户的类型</param>
        /// <param name="ReceiverID">接受者的id</param>
        /// <param name="SenderID">发送者的id</param>
        /// <param name="ID">id（删除和修改的时候用到 主键）</param>
        /// <param name="StartIndex">数据集起始的index。用于分页</param>
        /// <param name="EndIndex">i数据集结束的index，用于分页</param>
        /// <returns>返回值</returns>
        public DataSet ReturnDataListByProcedure(string procedureName, string AdminId, string UserType, string ReceiverID, string SenderID,int StartIndex,int EndIndex,int ID)
        {
            if (!string.IsNullOrEmpty(procedureName))
            {
                DbCommand storedProcCommand = this.database.GetStoredProcCommand(procedureName);
                if (!string.IsNullOrEmpty(AdminId))
                {
                    this.database.AddInParameter(storedProcCommand, "AdminId", DbType.String, AdminId);
                }
                if (!string.IsNullOrEmpty(UserType))
                {
                    this.database.AddInParameter(storedProcCommand, "UserType", DbType.String, UserType);
                }
                if (!string.IsNullOrEmpty(ReceiverID))
                {
                    this.database.AddInParameter(storedProcCommand, "ReceiverID", DbType.String, ReceiverID);

                }
                if (!string.IsNullOrEmpty(SenderID))
                {
                    this.database.AddInParameter(storedProcCommand, "SenderID", DbType.String, SenderID);
                }
                if (ID != -1)
                {
                    this.database.AddInParameter(storedProcCommand, "ID", DbType.Int32, ID);
                }
                if (StartIndex != -1)
                {
                    this.database.AddInParameter(storedProcCommand, "StartIndex", DbType.Int32, StartIndex);
                }
              if (EndIndex != -1)
                {
                    this.database.AddInParameter(storedProcCommand, "EndIndex", DbType.Int32, EndIndex);
                }


                return database.ExecuteDataSet(storedProcCommand);
            }
            return null;

        }
        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM MsgBox ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.ID desc");
            }
            strSql.Append(")AS Row, T.*  from MsgBox T ");
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

