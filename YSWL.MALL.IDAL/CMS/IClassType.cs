using System;
using System.Data;

using System.Collections.Generic;

namespace YSWL.MALL.IDAL.CMS
{
	/// <summary>
	/// 接口层ClassType
	/// </summary>
	public interface IClassType
	{
		#region  成员方法
		/// <summary>
		/// 得到最大ID
		/// </summary>
		int GetMaxId();
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		bool Exists(int ClassTypeID);
		/// <summary>
		/// 增加一条数据
		/// </summary>
        bool Add(YSWL.MALL.Model.CMS.ClassType model);
		/// <summary>
		/// 更新一条数据
		/// </summary>
		bool Update(YSWL.MALL.Model.CMS.ClassType model);
		/// <summary>
		/// 删除一条数据
		/// </summary>
		bool Delete(int ClassTypeID);
        /// <summary>
        /// 批量删除数据
        /// </summary>
		bool DeleteList(string ClassTypeIDlist );
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		YSWL.MALL.Model.CMS.ClassType GetModel(int ClassTypeID);
		/// <summary>
		/// 获得数据列表
		/// </summary>
		DataSet GetList(string strWhere);
          /// <summary>
        /// 获得数据列表
        /// </summary>
        List<YSWL.MALL.Model.CMS.ClassType> DataTableToList(DataTable dt);
		/// <summary>
		/// 获得前几行数据
		/// </summary>
		DataSet GetList(int Top,string strWhere,string filedOrder);
		/// <summary>
		/// 根据分页获得数据列表
		/// </summary>
		//DataSet GetList(int PageSize,int PageIndex,string strWhere);
		#endregion  成员方法
	} 
}
