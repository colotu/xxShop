/**  版本信息模板在安装目录下，可自行修改。
* PreOrder.cs
*
* 功 能： N/A
* 类 名： PreOrder
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/8/24 16:08:39   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
namespace YSWL.MALL.IDAL.Shop.PrePro
{
	/// <summary>
	/// 接口层预定
	/// </summary>
	public interface IPreOrder
	{
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        bool Exists(long PreOrderId);
        /// <summary>
        /// 增加一条数据
        /// </summary>
        long Add(YSWL.MALL.Model.Shop.PrePro.PreOrder model);
        /// <summary>
        /// 更新一条数据
        /// </summary>
        bool Update(YSWL.MALL.Model.Shop.PrePro.PreOrder model);
        /// <summary>
        /// 删除一条数据
        /// </summary>
        bool Delete(long PreOrderId);
        bool DeleteList(string PreOrderIdlist);
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        YSWL.MALL.Model.Shop.PrePro.PreOrder GetModel(long PreOrderId);
        YSWL.MALL.Model.Shop.PrePro.PreOrder DataRowToModel(DataRow row);
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
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        bool Exists(int userId, string sku, int status);
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        YSWL.MALL.Model.Shop.PrePro.PreOrder GetModel(int userId, string sku, int status);
        /// <summary>
        /// 更新一条数据
        /// </summary>
        bool Update(long PreOrderId, int count, string deliveryTip);
        /// <summary>
        ///  更新状态 
        /// </summary>
        /// <param name="PreOrderId">Id</param>
        /// <param name="Status">状态</param>
        /// <param name="HandleUserId">处理人Id</param>
        /// <returns></returns>
        bool UpdateStatus(long PreOrderId, int Status, int HandleUserId);
        /// <summary>
        ///  批量修改状态
        /// </summary>
        /// <param name="IDlist"></param>
        /// <param name="Status"></param>
        /// <param name="HandleUserId"></param>
        /// <returns></returns>
        bool UpdateList(string IDlist, int Status, int HandleUserId);

        #endregion  MethodEx
	} 
}
