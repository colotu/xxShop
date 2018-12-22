/**
* GroupBuy.cs
*
* 功 能： N/A
* 类 名： GroupBuy
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/10/14 15:51:55   N/A    初版
*
* Copyright (c) 2012-2013 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
namespace YSWL.MALL.IDAL.Shop.PromoteSales
{
	/// <summary>
	/// 接口层GroupBuy
	/// </summary>
	public interface IGroupBuy
	{
        #region  成员方法
        /// <summary>
        /// 得到最大ID
        /// </summary>
        int GetMaxId();
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        bool Exists(int GroupBuyId);
        /// <summary>
        /// 增加一条数据
        /// </summary>
        int Add(YSWL.MALL.Model.Shop.PromoteSales.GroupBuy model);
        /// <summary>
        /// 更新一条数据
        /// </summary>
        bool Update(YSWL.MALL.Model.Shop.PromoteSales.GroupBuy model);
        /// <summary>
        /// 删除一条数据
        /// </summary>
        bool Delete(int GroupBuyId);
        bool DeleteList(string GroupBuyIdlist);
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        YSWL.MALL.Model.Shop.PromoteSales.GroupBuy GetModel(int GroupBuyId);
        YSWL.MALL.Model.Shop.PromoteSales.GroupBuy DataRowToModel(DataRow row);
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
        int MaxSequence();

        bool IsExists(long ProductId);
        bool UpdateStatus(string ids, int status);
	    bool UpdateBuyCount(int buyId, int count);
	    DataSet GetListByPage(string strWhere, int cid, int regionId, string orderby, int startIndex, int endIndex);//分页获取数据
	    int GetCount(string strWhere, int regionId);//得到总数量
	    DataSet GetCategory(string strWhere);//为了取分类数据
        DataSet GetList(int Top, int cid, int regionId, string filedOrder);
        
            #endregion  MethodEx
        } 
}
