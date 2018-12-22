/**
* Favorite.cs
*
* 功 能： N/A
* 类 名： Favorite
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/6/22 15:32:12   N/A    初版
*
* Copyright (c) 2012-2013 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
namespace YSWL.MALL.IDAL.Shop
{
    /// <summary>
    /// 接口层Favorite
    /// </summary>
    public interface IFavorite
    {
        #region  成员方法
        /// <summary>
        /// 得到最大ID
        /// </summary>
        int GetMaxId();
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        bool Exists(int FavoriteId);
        /// <summary>
        /// 增加一条数据
        /// </summary>
        int Add(YSWL.MALL.Model.Shop.Favorite model);
        /// <summary>
        /// 更新一条数据
        /// </summary>
        bool Update(YSWL.MALL.Model.Shop.Favorite model);
        /// <summary>
        /// 删除一条数据
        /// </summary>
        bool Delete(int FavoriteId);
        bool DeleteList(string FavoriteIdlist);
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        YSWL.MALL.Model.Shop.Favorite GetModel(int FavoriteId);
        YSWL.MALL.Model.Shop.Favorite DataRowToModel(DataRow row);
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
        /// 分页获取收藏商品列表 
        /// </summary>
        DataSet GetProductListByPage(string strWhere, string orderby, int startIndex, int endIndex);
        bool Exists(long targetId, int userId,int type);

        DataSet GetBuyListByPage(string strWhere, string orderby, int startIndex, int endIndex);
        /// <summary>
        /// 获取收藏id
        /// </summary>
        /// <param name="targetId"></param>
        /// <param name="UserId"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        int GetFavoriteId(long targetId, int UserId, int Type);
        /// <summary>
        /// 先删后增增加数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool BatchFavor(YSWL.MALL.Model.Shop.Favorite model);

        /// <summary>
        ///  获取记录总数
        /// </summary>
        /// <param name="type">1:商品  2:店铺</param>
        /// <param name="targetId"></param>
        /// <returns></returns>
        int GetRecordCount(int type, int targetId);

        /// <summary>
        /// 增加一条数据
        /// </summary>
        bool AddEx(YSWL.MALL.Model.Shop.Favorite model);

        /// <summary>
        /// 删除一条数据
        /// </summary>
        bool DeleteEx(YSWL.MALL.Model.Shop.Favorite model);

        /// <summary>
        /// 分页获取收藏店铺列表 
        /// </summary>
        DataSet GetStoreListByPage(int userId, string orderby, int startIndex, int endIndex);
        /// <summary>
        /// 根据targetId ,userId,类型 删除一条数据
        /// </summary>
        bool Delete(int targetId, int userId, int type);

        int FavProductCount();
        /// <summary>
        /// 获取收藏店铺数
        /// </summary>
        int GetStoreRecordCount(int userId);
      
        #endregion  MethodEx
    }
}

