﻿using System;
using System.Data;
namespace YSWL.MALL.IDAL.Members
{
    /// <summary>
    /// 接口层Guestbook
    /// </summary>
    public interface IGuestbook
    {
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        bool Exists(int ID);
        /// <summary>
        /// 增加一条数据
        /// </summary>
        int Add(YSWL.MALL.Model.Members.Guestbook model);
        /// <summary>
        /// 更新一条数据
        /// </summary>
        bool Update(YSWL.MALL.Model.Members.Guestbook model);
        /// <summary>
        /// 删除一条数据
        /// </summary>
        bool Delete(int ID);
        bool DeleteList(string IDlist);
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        YSWL.MALL.Model.Members.Guestbook GetModel(int ID);
        YSWL.MALL.Model.Members.Guestbook DataRowToModel(DataRow row);
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
    }
}
