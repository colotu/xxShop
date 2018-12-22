/**
* Favorite.cs
*
* 功 能： N/A
* 类 名： Favorite
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/6/23 14:47:11   N/A    初版
*
* Copyright (c) 2012-2013 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.MALL.IDAL.Shop;
using YSWL.DBUtility;//Please add references
namespace YSWL.MALL.SQLServerDAL.Shop
{
    /// <summary>
    /// 数据访问类:Favorite
    /// </summary>
    public partial class Favorite : IFavorite
    {
        public Favorite()
        { }
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DBHelper.DefaultDBHelper.GetMaxID("FavoriteId", "Shop_Favorite");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int FavoriteId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Shop_Favorite");
            strSql.Append(" where FavoriteId=@FavoriteId");
            SqlParameter[] parameters = {
					new SqlParameter("@FavoriteId", SqlDbType.Int,4)
			};
            parameters[0].Value = FavoriteId;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.MALL.Model.Shop.Favorite model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Shop_Favorite(");
            strSql.Append("Type,TargetId,UserId,Tags,Remark,CreatedDate)");
            strSql.Append(" values (");
            strSql.Append("@Type,@TargetId,@UserId,@Tags,@Remark,@CreatedDate)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Type", SqlDbType.SmallInt,2),
					new SqlParameter("@TargetId", SqlDbType.BigInt,8),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@Tags", SqlDbType.NVarChar,100),
					new SqlParameter("@Remark", SqlDbType.NVarChar,500),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime)};
            parameters[0].Value = model.Type;
            parameters[1].Value = model.TargetId;
            parameters[2].Value = model.UserId;
            parameters[3].Value = model.Tags;
            parameters[4].Value = model.Remark;
            parameters[5].Value = model.CreatedDate;

            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.Shop.Favorite model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Shop_Favorite set ");
            strSql.Append("Type=@Type,");
            strSql.Append("TargetId=@TargetId,");
            strSql.Append("UserId=@UserId,");
            strSql.Append("Tags=@Tags,");
            strSql.Append("Remark=@Remark,");
            strSql.Append("CreatedDate=@CreatedDate");
            strSql.Append(" where FavoriteId=@FavoriteId");
            SqlParameter[] parameters = {
					new SqlParameter("@Type", SqlDbType.SmallInt,2),
					new SqlParameter("@TargetId", SqlDbType.BigInt,8),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@Tags", SqlDbType.NVarChar,100),
					new SqlParameter("@Remark", SqlDbType.NVarChar,500),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime),
					new SqlParameter("@FavoriteId", SqlDbType.Int,4)};
            parameters[0].Value = model.Type;
            parameters[1].Value = model.TargetId;
            parameters[2].Value = model.UserId;
            parameters[3].Value = model.Tags;
            parameters[4].Value = model.Remark;
            parameters[5].Value = model.CreatedDate;
            parameters[6].Value = model.FavoriteId;

            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int FavoriteId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Shop_Favorite ");
            strSql.Append(" where FavoriteId=@FavoriteId");
            SqlParameter[] parameters = {
					new SqlParameter("@FavoriteId", SqlDbType.Int,4)
			};
            parameters[0].Value = FavoriteId;

            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string FavoriteIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Shop_Favorite ");
            strSql.Append(" where FavoriteId in (" + FavoriteIdlist + ")  ");
            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.Favorite GetModel(int FavoriteId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 FavoriteId,Type,TargetId,UserId,Tags,Remark,CreatedDate from Shop_Favorite ");
            strSql.Append(" where FavoriteId=@FavoriteId");
            SqlParameter[] parameters = {
					new SqlParameter("@FavoriteId", SqlDbType.Int,4)
			};
            parameters[0].Value = FavoriteId;

            YSWL.MALL.Model.Shop.Favorite model = new YSWL.MALL.Model.Shop.Favorite();
            DataSet ds = DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.Favorite DataRowToModel(DataRow row)
        {
            YSWL.MALL.Model.Shop.Favorite model = new YSWL.MALL.Model.Shop.Favorite();
            if (row != null)
            {
                if (row["FavoriteId"] != null && row["FavoriteId"].ToString() != "")
                {
                    model.FavoriteId = int.Parse(row["FavoriteId"].ToString());
                }
                if (row["Type"] != null && row["Type"].ToString() != "")
                {
                    model.Type = int.Parse(row["Type"].ToString());
                }
                if (row["TargetId"] != null && row["TargetId"].ToString() != "")
                {
                    model.TargetId = long.Parse(row["TargetId"].ToString());
                }
                if (row["UserId"] != null && row["UserId"].ToString() != "")
                {
                    model.UserId = int.Parse(row["UserId"].ToString());
                }
                if (row["Tags"] != null)
                {
                    model.Tags = row["Tags"].ToString();
                }
                if (row["Remark"] != null)
                {
                    model.Remark = row["Remark"].ToString();
                }
                if (row["CreatedDate"] != null && row["CreatedDate"].ToString() != "")
                {
                    model.CreatedDate = DateTime.Parse(row["CreatedDate"].ToString());
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select FavoriteId,Type,TargetId,UserId,Tags,Remark,CreatedDate ");
            strSql.Append(" FROM Shop_Favorite ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" FavoriteId,Type,TargetId,UserId,Tags,Remark,CreatedDate ");
            strSql.Append(" FROM Shop_Favorite ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM Shop_Favorite ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.FavoriteId desc");
            }
            strSql.Append(")AS Row, T.*  from Shop_Favorite T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /*
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@tblName", SqlDbType.NVarChar, 255),
                    new SqlParameter("@fldName", SqlDbType.NVarChar, 255),
                    new SqlParameter("@PageSize", SqlDbType.Int),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@IsReCount", SqlDbType.Bit),
                    new SqlParameter("@OrderType", SqlDbType.Bit),
                    new SqlParameter("@strWhere", SqlDbType.NVarChar,1000),
                    };
            parameters[0].Value = "Shop_Favorite";
            parameters[1].Value = "FavoriteId";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DBHelper.DefaultDBHelper.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  BasicMethod
        #region  ExtensionMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long targetId, int UserId,int Type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Shop_Favorite");
            strSql.Append(" where TargetId=@TargetId and UserId=@UserId and Type=@Type ");
            SqlParameter[] parameters = {
					new SqlParameter("@TargetId", SqlDbType.BigInt,8),
					new SqlParameter("@UserId", SqlDbType.Int,4),	
                                        	new SqlParameter("@Type", SqlDbType.Int,4)
                                        };
            parameters[0].Value = targetId;
            parameters[1].Value = UserId;
            parameters[2].Value = Type;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 分页获取收藏商品列表 
        /// </summary>
        public DataSet GetProductListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
//            SELECT * FROM (  SELECT ROW_NUMBER() OVER (
            //    ORDER BY favo.CreatedDate DESC) AS ROW ,favo.FavoriteId AS FavoriteId, favo.CreatedDate AS CreatedDate ,prod.ProductId , prod.ProductName AS ProductName,prod.SaleStatus AS SaleStatus , prod.ThumbnailUrl1 AS ThumbnailUrl1
//        FROM  Shop_Favorite AS favo LEFT JOIN  PMS_Products AS prod ON  favo.TargetId=prod.ProductId	
//             WHERE favo.UserId=189 AND favo.Typeid=1 
//) AS tab WHERE tab.ROW BETWEEN 1 AND 3

            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by favo." + orderby);
            }
            else
            {
                strSql.Append("order by favo.FavoriteId desc");
            }
            strSql.Append(")AS Row, ");
            strSql.Append(" favo.FavoriteId AS FavoriteId, favo.CreatedDate AS CreatedDate ,prod.ProductId as ProductId , prod.ProductName  AS ProductName,prod.SaleStatus AS SaleStatus , prod.ThumbnailUrl1 AS ThumbnailUrl1,prod.LowestSalePrice as LowestSalePrice ,prod.MarketPrice as MarketPrice,prod.SupplierId AS SupplierId,prod.HasSKU as HasSKU");
            strSql.Append(" from Shop_Favorite AS favo LEFT JOIN  PMS_Products AS prod ON  favo.TargetId=prod.ProductId ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) tab");
            strSql.AppendFormat(" WHERE tab.Row between {0} and {1}", startIndex, endIndex);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }




        /// <summary>
        /// 分页获取收藏商品列表(可直接购买) 
        /// </summary>
        public DataSet GetBuyListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by favo." + orderby);
            }
            else
            {
                strSql.Append("order by favo.FavoriteId desc");
            }
            strSql.Append(")AS Row, ");
            strSql.Append(" favo.FavoriteId AS FavoriteId, favo.CreatedDate AS CreatedDate ,prod.ProductId as ProductId , prod.ProductName AS ProductName,prod.SaleStatus AS SaleStatus , prod.ThumbnailUrl1 AS ThumbnailUrl1, prod.ProductCode AS ProductCode ");
            strSql.Append(" from Shop_Favorite AS favo LEFT JOIN  PMS_Products AS prod ON  favo.TargetId=prod.ProductId ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) tab");
            strSql.AppendFormat(" WHERE tab.Row between {0} and {1}", startIndex, endIndex);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }


        /// <summary>
        /// 获取收藏id
        /// </summary>
        /// <param name="targetId"></param>
        /// <param name="UserId"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public int GetFavoriteId(long targetId, int UserId, int Type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select top 1 FavoriteId from Shop_Favorite  where TargetId=@TargetId and UserId=@UserId and Type=@Type ");
            SqlParameter[] parameters = {
                    new SqlParameter("@TargetId", SqlDbType.BigInt,8),
                    new SqlParameter("@UserId", SqlDbType.Int,4),
                                            new SqlParameter("@Type", SqlDbType.Int,4)
                                        };
            parameters[0].Value = targetId;
            parameters[1].Value = UserId;
            parameters[2].Value = Type;
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Common.Globals.SafeInt(obj,0);
            }
        }
        /// <summary>
        /// 先删后增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool BatchFavor(YSWL.MALL.Model.Shop.Favorite model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" delete Shop_Favorite where TargetId=@TargetId and UserId=@UserId ");
            strSql.Append("insert into Shop_Favorite(");
            strSql.Append("Type,TargetId,UserId,Tags,Remark,CreatedDate)");
            strSql.Append(" values (");
            strSql.Append("@Type,@TargetId,@UserId,@Tags,@Remark,@CreatedDate)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@Type", SqlDbType.SmallInt,2),
                    new SqlParameter("@TargetId", SqlDbType.BigInt,8),
                    new SqlParameter("@UserId", SqlDbType.Int,4),
                    new SqlParameter("@Tags", SqlDbType.NVarChar,100),
                    new SqlParameter("@Remark", SqlDbType.NVarChar,500),
                    new SqlParameter("@CreatedDate", SqlDbType.DateTime)};
            parameters[0].Value = model.Type;
            parameters[1].Value = model.TargetId;
            parameters[2].Value = model.UserId;
            parameters[3].Value = model.Tags;
            parameters[4].Value = model.Remark;
            parameters[5].Value = model.CreatedDate;

            int r = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
            return r > 0;
        }

        /// <summary>
        ///  获取记录总数
        /// </summary>
        /// <param name="type">1:商品  2:店铺</param>
        /// <param name="targetId"></param>
        /// <returns></returns>
        public int GetRecordCount(int type,int targetId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM Shop_Favorite ");
            strSql.AppendFormat(" where type={0}  and  TargetId={1} ", type, targetId);
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool AddEx(YSWL.MALL.Model.Shop.Favorite model)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Shop_Favorite(");
            strSql.Append("Type,TargetId,UserId,Tags,Remark,CreatedDate)");
            strSql.Append(" values (");
            strSql.Append("@Type,@TargetId,@UserId,@Tags,@Remark,@CreatedDate)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@Type", SqlDbType.SmallInt,2),
                    new SqlParameter("@TargetId", SqlDbType.BigInt,8),
                    new SqlParameter("@UserId", SqlDbType.Int,4),
                    new SqlParameter("@Tags", SqlDbType.NVarChar,100),
                    new SqlParameter("@Remark", SqlDbType.NVarChar,500),
                    new SqlParameter("@CreatedDate", SqlDbType.DateTime)};
            parameters[0].Value = model.Type;
            parameters[1].Value = model.TargetId;
            parameters[2].Value = model.UserId;
            parameters[3].Value = model.Tags;
            parameters[4].Value = model.Remark;
            parameters[5].Value = model.CreatedDate;
            CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
            sqllist.Add(cmd);

            if (model.Type == (int)Model.Shop.FavoriteEnums.Store)
            {
                    StringBuilder strSql2 = new StringBuilder();
                    strSql2.AppendFormat(" update Shop_Suppliers set FavoritesCount=FavoritesCount+1 where SupplierId = {0} ", model.TargetId);
                    cmd = new CommandInfo(strSql2.ToString(), null);
                    sqllist.Add(cmd);
            }
            int rowsAffected = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
            if (rowsAffected > 0)
            {
                return true;
            }else{
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteEx(YSWL.MALL.Model.Shop.Favorite model)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Shop_Favorite ");
            strSql.Append(" where FavoriteId=@FavoriteId");
            SqlParameter[] parameters = {
                    new SqlParameter("@FavoriteId", SqlDbType.Int,4)
            };
            parameters[0].Value = model.FavoriteId;
            CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
            sqllist.Add(cmd);

            if (model.Type == (int)Model.Shop.FavoriteEnums.Store)
            {
                StringBuilder strSql2 = new StringBuilder();
                strSql2.AppendFormat(" update Shop_Suppliers set FavoritesCount=FavoritesCount-1 where SupplierId = {0} ", model.TargetId);
                cmd = new CommandInfo(strSql2.ToString(), null);
                sqllist.Add(cmd);
            }
            int rowsAffected = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }  
        }
        /// <summary>
        /// 分页获取收藏店铺列表 
        /// </summary>
        public DataSet GetStoreListByPage(int  userId, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by favo." + orderby);
            }
            else
            {
                strSql.Append("order by favo.FavoriteId desc");
            }
            strSql.Append(")AS Row, ");
            strSql.Append(" favo.FavoriteId AS FavoriteId, favo.CreatedDate AS CreatedDate,supp.SupplierId AS SupplierId,supp.ShopName as ShopName,supp.SalesCount as SalesCount,supp.Status as Status,supp.StoreStatus as StoreStatus");
            strSql.Append(" from Shop_Favorite AS favo Left JOIN  Shop_Suppliers AS supp ON  favo.TargetId=supp.SupplierId ");
            strSql.AppendFormat(" WHERE favo.UserId ={0} and favo.Type= {1} ", userId, (int)YSWL.MALL.Model.Shop.FavoriteEnums.Store);
            strSql.Append(" ) tab");
            strSql.AppendFormat(" WHERE tab.Row between {0} and {1}", startIndex, endIndex);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 根据targetId ,userId,类型 删除一条数据
        /// </summary>
        public bool Delete(int targetId, int userId,int type)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Shop_Favorite ");
            strSql.AppendFormat(" where TargetId={0} and UserId={1} and type={2} ", targetId, userId, type);
            CommandInfo cmd = new CommandInfo(strSql.ToString(), null);
            sqllist.Add(cmd);
            if ((int)YSWL.MALL.Model.Shop.FavoriteEnums.Store == type)
            {
                //修改店铺的 已关注数量
                StringBuilder strSql2 = new StringBuilder();
                strSql2.AppendFormat(" update Shop_Suppliers set FavoritesCount=FavoritesCount-1 where SupplierId = {0} ", targetId);
                cmd = new CommandInfo(strSql2.ToString(), null);
                sqllist.Add(cmd);
            }

            int rowsAffected = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion  ExtensionMethod

        /// <summary>
        /// 获取被收藏商品数(已去重)
        /// </summary>
        /// <returns></returns>
        public int FavProductCount()
        {
            string strSql = " SELECT COUNT(DISTINCT favoriteid) FROM Shop_Favorite WHERE TYPE=1 ";
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 获取收藏店铺数 
        /// </summary>
        public int GetStoreRecordCount(int userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM Shop_Favorite ");
            strSql.AppendFormat(" WHERE UserId ={0} and  Type= {1} ", userId, (int)YSWL.MALL.Model.Shop.FavoriteEnums.Store);
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
    }
}



