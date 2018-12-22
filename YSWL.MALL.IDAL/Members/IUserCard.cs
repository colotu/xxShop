/**  版本信息模板在安装目录下，可自行修改。
* UserCard.cs
*
* 功 能： N/A
* 类 名： UserCard
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/12/17 19:10:22   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
namespace YSWL.MALL.IDAL.Members
{
	/// <summary>
	/// 接口层UserCard
	/// </summary>
	public interface IUserCard
	{
		#region  成员方法
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		bool Exists(string CardCode);
		/// <summary>
		/// 增加一条数据
		/// </summary>
		bool Add(YSWL.MALL.Model.Members.UserCard model);
		/// <summary>
		/// 更新一条数据
		/// </summary>
		bool Update(YSWL.MALL.Model.Members.UserCard model);
		/// <summary>
		/// 删除一条数据
		/// </summary>
		bool Delete(string CardCode);
		bool DeleteList(string CardCodelist );
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		YSWL.MALL.Model.Members.UserCard GetModel(string CardCode);
		YSWL.MALL.Model.Members.UserCard DataRowToModel(DataRow row);
		/// <summary>
		/// 获得数据列表
		/// </summary>
		DataSet GetList(string strWhere);
		/// <summary>
		/// 获得前几行数据
		/// </summary>
		DataSet GetList(int Top,string strWhere,string filedOrder);
		int GetRecordCount(string strWhere);
		DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex);
		/// <summary>
		/// 根据分页获得数据列表
		/// </summary>
		//DataSet GetList(int PageSize,int PageIndex,string strWhere);
		#endregion  成员方法
		#region  MethodEx
        bool AddCard(YSWL.MALL.Model.Members.UserCard model);

        bool DeleteEx(string CardCode);

		#endregion  MethodEx
	} 
}
