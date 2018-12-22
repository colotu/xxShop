using System;
using System.Data;
namespace YSWL.MALL.IDAL.Poll
{
	/// <summary>
	/// 接口层Users
	/// </summary>
    public interface IPollUsers
	{
        #region  成员方法
        /// <summary>
        /// 得到最大ID
        /// </summary>
        int GetMaxId();
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        bool Exists(int UserID);
        /// <summary>
        /// 增加一条数据
        /// </summary>
        int Add(YSWL.MALL.Model.Poll.PollUsers model);
        /// <summary>
        /// 更新一条数据
        /// </summary>
        bool Update(YSWL.MALL.Model.Poll.PollUsers model);
        /// <summary>
        /// 删除一条数据
        /// </summary>
        bool Delete(int UserID);
        bool DeleteList(string UserIDlist);
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        YSWL.MALL.Model.Poll.PollUsers GetModel(int UserID);
        YSWL.MALL.Model.Poll.PollUsers DataRowToModel(DataRow row);
        /// <summary>
        /// 获得数据列表
        /// </summary>
        DataSet GetList(string strWhere);
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        DataSet GetList(int Top, string strWhere, string filedOrder);
        int GetRecordCount(string strWhere);
        DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex);

	    /// <summary>
	    /// 根据分页获得数据列表
	    /// </summary>
	    //DataSet GetList(int PageSize,int PageIndex,string strWhere);

	    #endregion  成员方法


	    /// <summary>
	    /// 是否存在该记录（系统中的用户）
	    /// </summary>
	    /// <param name="UserId">系统用户UserID</param>
	    /// <returns></returns>
	    bool ExistsSysUser(int UserId);

	} 
}
