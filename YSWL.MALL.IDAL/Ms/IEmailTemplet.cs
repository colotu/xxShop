using System;
using System.Data;
namespace YSWL.MALL.IDAL.Ms
{
	/// <summary>
	/// 接口层EmailTemplet
	/// </summary>
	public interface IEmailTemplet
	{
        #region  成员方法
        /// <summary>
        /// 得到最大ID
        /// </summary>
        int GetMaxId();
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        bool Exists(int TempletId);
        /// <summary>
        /// 增加一条数据
        /// </summary>
        int Add(YSWL.MALL.Model.Ms.EmailTemplet model);
        /// <summary>
        /// 更新一条数据
        /// </summary>
        bool Update(YSWL.MALL.Model.Ms.EmailTemplet model);
        /// <summary>
        /// 删除一条数据
        /// </summary>
        bool Delete(int TempletId);
        bool DeleteList(string TempletIdlist);
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        YSWL.MALL.Model.Ms.EmailTemplet GetModel(int TempletId);
        YSWL.MALL.Model.Ms.EmailTemplet DataRowToModel(DataRow row);
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
		#region  MethodEx

		#endregion  MethodEx
	} 
}
