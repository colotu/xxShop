using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace YSWL.DBUtility
{
    /// <summary>
    /// 数据库基类（所有具体实现数据库必须继承该类）
    /// </summary>
    public abstract class DBBase
    {    

        #region 公用方法

        /// <summary>
        /// 获取Connection链接
        /// </summary>
        public SqlConnection GetConnection
        {
            get { return new SqlConnection(GetConnectionStr()); }
        }

        /// <summary>
        /// 获取链接字符串
        /// </summary>
        /// <returns></returns>
        public string GetConnectionStr()
        {
            return ConnectionStrManage.GetConnectionStr();
        }

        /// <summary>
        /// 判断是否存在某表的某个字段
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="columnName">列名称</param>
        /// <returns>是否存在</returns>
        public abstract bool ColumnExists(string tableName, string columnName);

        public abstract int GetMaxID(string FieldName, string TableName);

        public abstract bool Exists(string strSql);

        /// <summary>
        /// 表是否存在
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public abstract bool TabExists(string TableName);

        public abstract bool Exists(string strSql, params SqlParameter[] cmdParms);

        #endregion

        #region  执行简单SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public abstract int ExecuteSql(string SQLString);

        public abstract int ExecuteSqlByTime(string SQLString, int Times);

        /// <summary>
        /// 执行Sql和Oracle滴混合事务
        /// </summary>
        /// <param name="list">SQL命令行列表</param>
        /// <param name="oracleCmdSqlList">Oracle命令行列表</param>
        /// <returns>执行结果 0-由于SQL造成事务失败 -1 由于Oracle造成事务失败 1-整体事务执行成功</returns>
        public abstract int ExecuteSqlTran(List<CommandInfo> list, List<CommandInfo> oracleCmdSqlList);

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public abstract int ExecuteSqlTran(List<String> SQLStringList);

        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public abstract int ExecuteSql(string SQLString, string content);

        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public abstract object ExecuteSqlGet(string SQLString, string content);

        /// <summary>
        /// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
        /// <returns>影响的记录数</returns>
        public abstract int ExecuteSqlInsertImg(string strSQL, byte[] fs);

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public abstract object GetSingle(string SQLString);

        public abstract object GetSingle(string SQLString, int Times);

        /// <summary>
        /// 执行查询语句，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SqlDataReader</returns>
        public abstract SqlDataReader ExecuteReader(string strSQL);

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public abstract DataSet Query(string SQLString);


        public abstract DataSet Query(string SQLString, int Times);



        #endregion

        #region 执行带参数的SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public abstract int ExecuteSql(string SQLString, params SqlParameter[] cmdParms);


        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public abstract void ExecuteSqlTran(Hashtable SQLStringList);

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public abstract int ExecuteSqlTran(System.Collections.Generic.List<CommandInfo> cmdList);

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="cmdList">SQL语句的CommandInfo</param>
        /// <remarks>只使用了CommandInfo-EffentNextType.ExcuteEffectRows, 其它项暂不支持</remarks>
        public abstract int ExecuteSqlTran4Indentity(System.Collections.Generic.List<CommandInfo> cmdList);

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="cmdList">SQL语句的CommandInfo</param>
        /// <param name="trans">外部事务对象</param>
        /// <remarks>警告:内部不触发事务的提交和回滚</remarks>
        /// <remarks>只使用了CommandInfo-EffentNextType.ExcuteEffectRows, 其它项暂不支持</remarks>
        public abstract int ExecuteSqlTran4Indentity(System.Collections.Generic.List<CommandInfo> cmdList, SqlTransaction trans);

        public abstract int ExecuteSqlTran4IndentityEx(System.Collections.Generic.List<CommandInfo> cmdList,
            SqlTransaction trans);

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。[已过时, 请使用ExecuteSqlTran4Indentity]
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        [Obsolete]
        public abstract void ExecuteSqlTranWithIndentity(System.Collections.Generic.List<CommandInfo> SQLStringList);

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public abstract void ExecuteSqlTranWithIndentity(Hashtable SQLStringList);

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public abstract object GetSingle(string SQLString, params SqlParameter[] cmdParms);

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="commandInfo">SQL语句的CommandInfo</param>
        /// <param name="trans">外部事务对象</param>
        /// <remarks>警告: 内部无异常处理和using语句, 请务必在外部实现</remarks>
        /// <returns>查询结果（object）</returns>
        public abstract object GetSingle4Trans(CommandInfo commandInfo, SqlTransaction trans);

        /// <summary>
        /// 执行查询语句，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SqlDataReader</returns>
        public abstract SqlDataReader ExecuteReader(string SQLString, params SqlParameter[] cmdParms);

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public abstract DataSet Query(string SQLString, params SqlParameter[] cmdParms);

        #endregion

        #region 存储过程操作

        /// <summary>
        /// 执行存储过程，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlDataReader</returns>
        public abstract SqlDataReader RunProcedure(string storedProcName, IDataParameter[] parameters);


        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <returns>DataSet</returns>
        public abstract DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName);

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <returns>DataSet</returns>
        public abstract DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName, out int returnValue);

        public abstract DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName, int Times);

        /// <summary>
        /// 执行存储过程，返回影响的行数		
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="rowsAffected">影响的行数</param>
        /// <returns></returns>
        public abstract int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected);

        public abstract void RunProcedure(string storedProcName, IDataParameter[] parameters, DataSet dataSet,
            string tableName);

        #endregion

        #region 添加一个传连接字符串的简单执行sql语句 2009-2-4
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="strConnectionString">连接字符串</param>
        /// <returns>影响的记录数</returns>
        public abstract int ExecuteSql(string SQLString, string strConnectionString, params SqlParameter[] cmdParms);

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <param name="strConnectionString">连接字符串</param>
        /// <returns>DataSet</returns>
        public abstract DataSet Query(string SQLString, string strConnectionString);

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
        public abstract SqlParameter CreateInParam(string ParamName,
            SqlDbType DbType, int Size, object Value);

        /// <summary>
        /// 创建输出类型参数
        /// </summary>
        /// <param name="ParamName">参数名称</param>
        /// <param name="DbType">参数类型</param>
        /// <param name="Size">参数大小</param>
        /// <returns>新的 parameter 对象</returns>
        public abstract SqlParameter CreateOutParam(string ParamName,
            SqlDbType DbType, int Size);

        /// <summary>
        /// 创建输出类型参数
        /// </summary>
        /// <param name="ParamName">参数名称</param>
        /// <param name="DbType">参数类型</param>
        /// <param name="Size">参数大小</param>
        /// <returns>新的 parameter 对象</returns>
        public abstract SqlParameter CreateInputOutParam(string ParamName,
            SqlDbType DbType, int Size, object Value);

        /// <summary>
        /// 创建返回类型参数
        /// </summary>
        /// <param name="ParamName">参数名称</param>
        /// <param name="DbType">参数类型</param>
        /// <param name="Size">参数大小</param>
        /// <returns>新的 parameter 对象</returns>
        public abstract SqlParameter CreateReturnParam(string ParamName,
            SqlDbType DbType, int Size);


        #endregion
    }
}
