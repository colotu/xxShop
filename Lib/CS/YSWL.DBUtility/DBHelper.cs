using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace YSWL.DBUtility
{
    /// <summary>
    /// 数据库辅助类（所有操作库的地方全部通过DBHelper）
    /// </summary>
    public class DBHelper
    {
        /// <summary>
        /// 具体目标数据操作层程序集配置信息
        /// </summary>
        public static string dbObjectKey = "TargetDB_Object";

        /// <summary>
        /// 全局的DBHelper辅助类
        /// </summary>
        public static DBHelper DefaultDBHelper
        {
            get { return new DBHelper(); }
        }

        /// <summary>
        /// 获取具体的数据库操作对象
        /// </summary>
        /// <returns></returns>
        public DBBase GetDBObject()
        {
            return DBFactory.CreateDBObject(dbObjectKey);
        }

        #region 公用方法

        /// <summary>
        /// 判断是否存在某表的某个字段
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="columnName">列名称</param>
        /// <returns>是否存在</returns>
        public bool ColumnExists(string tableName, string columnName)
        {
            return GetDBObject().ColumnExists(tableName, columnName);
        }
        public int GetMaxID(string FieldName, string TableName)
        {
            return GetDBObject().GetMaxID(FieldName, TableName);
        }
        public bool Exists(string strSql)
        {
            return GetDBObject().Exists(strSql);
        }
        /// <summary>
        /// 表是否存在
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public bool TabExists(string TableName)
        {
            return GetDBObject().TabExists(TableName);
        }
        public bool Exists(string strSql, params SqlParameter[] cmdParms)
        {
            return GetDBObject().Exists(strSql, cmdParms);
        }
        #endregion

        #region  执行简单SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public int ExecuteSql(string SQLString)
        {
            return GetDBObject().ExecuteSql(SQLString);
        }

        public int ExecuteSqlByTime(string SQLString, int Times)
        {
            return GetDBObject().ExecuteSqlByTime(SQLString, Times);
        }

        /// <summary>
        /// 执行Sql和Oracle滴混合事务
        /// </summary>
        /// <param name="list">SQL命令行列表</param>
        /// <param name="oracleCmdSqlList">Oracle命令行列表</param>
        /// <returns>执行结果 0-由于SQL造成事务失败 -1 由于Oracle造成事务失败 1-整体事务执行成功</returns>
        public int ExecuteSqlTran(List<CommandInfo> list, List<CommandInfo> oracleCmdSqlList)
        {
            return 0;
        }
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public int ExecuteSqlTran(List<String> SQLStringList)
        {
            return GetDBObject().ExecuteSqlTran(SQLStringList);
        }
        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public int ExecuteSql(string SQLString, string content)
        {
            return GetDBObject().ExecuteSql(SQLString, content);
        }
        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public object ExecuteSqlGet(string SQLString, string content)
        {
            return GetDBObject().ExecuteSqlGet(SQLString, content);
        }
        /// <summary>
        /// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
        /// <returns>影响的记录数</returns>
        public int ExecuteSqlInsertImg(string strSQL, byte[] fs)
        {
            return GetDBObject().ExecuteSqlInsertImg(strSQL, fs);
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public object GetSingle(string SQLString)
        {
            return GetDBObject().GetSingle(SQLString);
        }
        public object GetSingle(string SQLString, int Times)
        {
            return GetDBObject().GetSingle(SQLString, Times);
        }
        /// <summary>
        /// 执行查询语句，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SqlDataReader</returns>
        public SqlDataReader ExecuteReader(string strSQL)
        {
            return GetDBObject().ExecuteReader(strSQL);

        }
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public DataSet Query(string SQLString)
        {
            return GetDBObject().Query(SQLString);
        }
        public DataSet Query(string SQLString, int Times)
        {
            return GetDBObject().Query(SQLString, Times);
        }



        #endregion

        #region 执行带参数的SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public int ExecuteSql(string SQLString, params SqlParameter[] cmdParms)
        {
            return GetDBObject().ExecuteSql(SQLString, cmdParms);
        }


        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public void ExecuteSqlTran(Hashtable SQLStringList)
        {
            GetDBObject().ExecuteSqlTran(SQLStringList);
        }
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public int ExecuteSqlTran(System.Collections.Generic.List<CommandInfo> cmdList)
        {
            return GetDBObject().ExecuteSqlTran(cmdList);
        }
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="cmdList">SQL语句的CommandInfo</param>
        /// <remarks>只使用了CommandInfo-EffentNextType.ExcuteEffectRows, 其它项暂不支持</remarks>
        public int ExecuteSqlTran4Indentity(System.Collections.Generic.List<CommandInfo> cmdList)
        {
            return GetDBObject().ExecuteSqlTran4Indentity(cmdList);
        }
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="cmdList">SQL语句的CommandInfo</param>
        /// <param name="trans">外部事务对象</param>
        /// <remarks>警告:内部不触发事务的提交和回滚</remarks>
        /// <remarks>只使用了CommandInfo-EffentNextType.ExcuteEffectRows, 其它项暂不支持</remarks>
        public int ExecuteSqlTran4Indentity(System.Collections.Generic.List<CommandInfo> cmdList, SqlTransaction trans)
        {
            return GetDBObject().ExecuteSqlTran4Indentity(cmdList, trans);
        }

        public int ExecuteSqlTran4IndentityEx(System.Collections.Generic.List<CommandInfo> cmdList, SqlTransaction trans)
        {
            return GetDBObject().ExecuteSqlTran4IndentityEx(cmdList, trans);
        }
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。[已过时, 请使用ExecuteSqlTran4Indentity]
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        [Obsolete]
        public void ExecuteSqlTranWithIndentity(System.Collections.Generic.List<CommandInfo> SQLStringList)
        {
            GetDBObject().ExecuteSqlTranWithIndentity(SQLStringList);
        }
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public void ExecuteSqlTranWithIndentity(Hashtable SQLStringList)
        {
            GetDBObject().ExecuteSqlTranWithIndentity(SQLStringList);
        }
        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public object GetSingle(string SQLString, params SqlParameter[] cmdParms)
        {
            return GetDBObject().GetSingle(SQLString, cmdParms);
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="commandInfo">SQL语句的CommandInfo</param>
        /// <param name="trans">外部事务对象</param>
        /// <remarks>警告: 内部无异常处理和using语句, 请务必在外部实现</remarks>
        /// <returns>查询结果（object）</returns>
        public object GetSingle4Trans(CommandInfo commandInfo, SqlTransaction trans)
        {
            return GetDBObject().GetSingle4Trans(commandInfo, trans);
        }

        /// <summary>
        /// 执行查询语句，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SqlDataReader</returns>
        public SqlDataReader ExecuteReader(string SQLString, params SqlParameter[] cmdParms)
        {
            return GetDBObject().ExecuteReader(SQLString, cmdParms);

        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public DataSet Query(string SQLString, params SqlParameter[] cmdParms)
        {
            return GetDBObject().Query(SQLString, cmdParms);
        }        

        #endregion

        #region 存储过程操作

        /// <summary>
        /// 执行存储过程，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlDataReader</returns>
        public SqlDataReader RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            return GetDBObject().RunProcedure(storedProcName, parameters);

        }

        //public static SqlConnection GetConnection
        //{
        //    get { return new SqlConnection(connectionString); }
        //}
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <returns>DataSet</returns>
        public DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
        {
            return GetDBObject().RunProcedure(storedProcName, parameters, tableName);
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <returns>DataSet</returns>
        public DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName, out int returnValue)
        {
            return GetDBObject().RunProcedure(storedProcName, parameters, tableName, out returnValue);
        }
        public DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName, int Times)
        {
            return GetDBObject().RunProcedure(storedProcName, parameters, tableName, Times);
        }



        /// <summary>
        /// 执行存储过程，将结果集添加到现有的DataSet中。
        /// Takes an -existing- dataset and fills the given table name with the results
        /// of the stored procedure.
        /// </summary>
        /// <param name="storedProcName"></param>
        /// <param name="parameters"></param>
        /// <param name="dataSet"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public  void RunProcedure(string storedProcName, IDataParameter[] parameters, DataSet dataSet, string tableName)
        {
             GetDBObject().RunProcedure(storedProcName, parameters, dataSet, tableName);
        }

        /// <summary>
        /// 执行存储过程，返回影响的行数		
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="rowsAffected">影响的行数</param>
        /// <returns></returns>
        public int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)
        {
            return GetDBObject().RunProcedure(storedProcName, parameters,out rowsAffected);
        }
        
        #endregion

        #region 添加一个传连接字符串的简单执行sql语句 2009-2-4
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="strConnectionString">连接字符串</param>
        /// <returns>影响的记录数</returns>
        public int ExecuteSql(string SQLString, string strConnectionString, params SqlParameter[] cmdParms)
        {
            return GetDBObject().ExecuteSql(SQLString, strConnectionString, cmdParms);
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <param name="strConnectionString">连接字符串</param>
        /// <returns>DataSet</returns>
        public DataSet Query(string SQLString, string strConnectionString)
        {
            return GetDBObject().Query(SQLString, strConnectionString);
        }
        #endregion

        #region 创建参数 SqlParameter       

        /// <summary>
        /// 创建输入类型参数
        /// </summary>
        /// <param name="ParamName">参数名称</param>
        /// <param name="DbType">参数类型</param></param>
        /// <param name="Size">参数大小</param>
        /// <param name="Value">参数值</param>
        /// <returns>新的parameter 对象</returns>
        public SqlParameter CreateInParam(string ParamName,
            SqlDbType DbType, int Size, object Value)
        {
            return GetDBObject().CreateInParam(ParamName, DbType, Size, Value);
        }

        /// <summary>
        /// 创建输出类型参数
        /// </summary>
        /// <param name="ParamName">参数名称</param>
        /// <param name="DbType">参数类型</param>
        /// <param name="Size">参数大小</param>
        /// <returns>新的 parameter 对象</returns>
        public SqlParameter CreateOutParam(string ParamName,
            SqlDbType DbType, int Size)
        {
            return GetDBObject().CreateOutParam(ParamName, DbType, Size);
        }
        /// <summary>
        /// 创建输出类型参数
        /// </summary>
        /// <param name="ParamName">参数名称</param>
        /// <param name="DbType">参数类型</param>
        /// <param name="Size">参数大小</param>
        /// <returns>新的 parameter 对象</returns>
        public SqlParameter CreateInputOutParam(string ParamName,
            SqlDbType DbType, int Size, object Value)
        {
            return GetDBObject().CreateInputOutParam(ParamName, DbType, Size, Value);
        }
        /// <summary>
        /// 创建返回类型参数
        /// </summary>
        /// <param name="ParamName">参数名称</param>
        /// <param name="DbType">参数类型</param>
        /// <param name="Size">参数大小</param>
        /// <returns>新的 parameter 对象</returns>
        public SqlParameter CreateReturnParam(string ParamName,
            SqlDbType DbType, int Size)
        {
            return GetDBObject().CreateReturnParam(ParamName, DbType, Size);
        }

        #endregion
    }
}
