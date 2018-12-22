/**  版本信息模板在安装目录下，可自行修改。
* PreProduct.cs
*
* 功 能： N/A
* 类 名： PreProduct
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/8/24 16:08:40   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Xml.Serialization;
using YSWL.MALL.IDAL.Shop.PrePro;
using YSWL.DBUtility;//Please add references
namespace YSWL.MALL.SQLServerDAL.Shop.PrePro
{
    /// <summary>
    /// 数据访问类:PreProduct
    /// </summary>
    public partial class PreProduct : IPreProduct
    {
        public PreProduct()
        { }
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DBHelper.DefaultDBHelper.GetMaxID("PreProId", "Shop_PreProduct");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int PreProId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Shop_PreProduct");
            strSql.Append(" where PreProId=@PreProId");
            SqlParameter[] parameters = {
					new SqlParameter("@PreProId", SqlDbType.Int,4)
			};
            parameters[0].Value = PreProId;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.MALL.Model.Shop.PrePro.PreProduct model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Shop_PreProduct(");
            strSql.Append("ProductId,PreAmount,PreStartDate,PreEndDate,BuyStartDate,BuyEndDate,BuyCount,LimitQty,RegionId,Status,Description)");
            strSql.Append(" values (");
            strSql.Append("@ProductId,@PreAmount,@PreStartDate,@PreEndDate,@BuyStartDate,@BuyEndDate,@BuyCount,@LimitQty,@RegionId,@Status,@Description)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@ProductId", SqlDbType.BigInt,8),
					new SqlParameter("@PreAmount", SqlDbType.Money,8),
					new SqlParameter("@PreStartDate", SqlDbType.DateTime),
					new SqlParameter("@PreEndDate", SqlDbType.DateTime),
					new SqlParameter("@BuyStartDate", SqlDbType.DateTime),
					new SqlParameter("@BuyEndDate", SqlDbType.DateTime),
					new SqlParameter("@BuyCount", SqlDbType.Int,4),
					new SqlParameter("@LimitQty", SqlDbType.Int,4),
					new SqlParameter("@RegionId", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@Description", SqlDbType.Text)};
            parameters[0].Value = model.ProductId;
            parameters[1].Value = model.PreAmount;
            parameters[2].Value = model.PreStartDate;
            parameters[3].Value = model.PreEndDate;
            parameters[4].Value = model.BuyStartDate;
            parameters[5].Value = model.BuyEndDate;
            parameters[6].Value = model.BuyCount;
            parameters[7].Value = model.LimitQty;
            parameters[8].Value = model.RegionId;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.Description;

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
        public bool Update(YSWL.MALL.Model.Shop.PrePro.PreProduct model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Shop_PreProduct set ");
            strSql.Append("ProductId=@ProductId,");
            strSql.Append("PreAmount=@PreAmount,");
            strSql.Append("PreStartDate=@PreStartDate,");
            strSql.Append("PreEndDate=@PreEndDate,");
            strSql.Append("BuyStartDate=@BuyStartDate,");
            strSql.Append("BuyEndDate=@BuyEndDate,");
            strSql.Append("BuyCount=@BuyCount,");
            strSql.Append("LimitQty=@LimitQty,");
            strSql.Append("RegionId=@RegionId,");
            strSql.Append("Status=@Status,");
            strSql.Append("Description=@Description");
            strSql.Append(" where PreProId=@PreProId");
            SqlParameter[] parameters = {
					new SqlParameter("@ProductId", SqlDbType.BigInt,8),
					new SqlParameter("@PreAmount", SqlDbType.Money,8),
					new SqlParameter("@PreStartDate", SqlDbType.DateTime),
					new SqlParameter("@PreEndDate", SqlDbType.DateTime),
					new SqlParameter("@BuyStartDate", SqlDbType.DateTime),
					new SqlParameter("@BuyEndDate", SqlDbType.DateTime),
					new SqlParameter("@BuyCount", SqlDbType.Int,4),
					new SqlParameter("@LimitQty", SqlDbType.Int,4),
					new SqlParameter("@RegionId", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@Description", SqlDbType.Text),
					new SqlParameter("@PreProId", SqlDbType.Int,4)};
            parameters[0].Value = model.ProductId;
            parameters[1].Value = model.PreAmount;
            parameters[2].Value = model.PreStartDate;
            parameters[3].Value = model.PreEndDate;
            parameters[4].Value = model.BuyStartDate;
            parameters[5].Value = model.BuyEndDate;
            parameters[6].Value = model.BuyCount;
            parameters[7].Value = model.LimitQty;
            parameters[8].Value = model.RegionId;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.Description;
            parameters[11].Value = model.PreProId;

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
        public bool Delete(int PreProId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Shop_PreProduct ");
            strSql.Append(" where PreProId=@PreProId");
            SqlParameter[] parameters = {
					new SqlParameter("@PreProId", SqlDbType.Int,4)
			};
            parameters[0].Value = PreProId;

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
        public bool DeleteList(string PreProIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Shop_PreProduct ");
            strSql.Append(" where PreProId in (" + PreProIdlist + ")  ");
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
        public YSWL.MALL.Model.Shop.PrePro.PreProduct GetModel(int PreProId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 PreProId,ProductId,PreAmount,PreStartDate,PreEndDate,BuyStartDate,BuyEndDate,BuyCount,LimitQty,RegionId,Status,Description from Shop_PreProduct ");
            strSql.Append(" where PreProId=@PreProId");
            SqlParameter[] parameters = {
					new SqlParameter("@PreProId", SqlDbType.Int,4)
			};
            parameters[0].Value = PreProId;

            YSWL.MALL.Model.Shop.PrePro.PreProduct model = new YSWL.MALL.Model.Shop.PrePro.PreProduct();
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
        public YSWL.MALL.Model.Shop.PrePro.PreProduct DataRowToModel(DataRow row)
        {
            YSWL.MALL.Model.Shop.PrePro.PreProduct model = new YSWL.MALL.Model.Shop.PrePro.PreProduct();
            if (row != null)
            {
                if (row["PreProId"] != null && row["PreProId"].ToString() != "")
                {
                    model.PreProId = int.Parse(row["PreProId"].ToString());
                }
                if (row["ProductId"] != null && row["ProductId"].ToString() != "")
                {
                    model.ProductId = long.Parse(row["ProductId"].ToString());
                }
                if (row["PreAmount"] != null && row["PreAmount"].ToString() != "")
                {
                    model.PreAmount = decimal.Parse(row["PreAmount"].ToString());
                }
                if (row["PreStartDate"] != null && row["PreStartDate"].ToString() != "")
                {
                    model.PreStartDate = DateTime.Parse(row["PreStartDate"].ToString());
                }
                if (row["PreEndDate"] != null && row["PreEndDate"].ToString() != "")
                {
                    model.PreEndDate = DateTime.Parse(row["PreEndDate"].ToString());
                }
                if (row["BuyStartDate"] != null && row["BuyStartDate"].ToString() != "")
                {
                    model.BuyStartDate = DateTime.Parse(row["BuyStartDate"].ToString());
                }
                if (row["BuyEndDate"] != null && row["BuyEndDate"].ToString() != "")
                {
                    model.BuyEndDate = DateTime.Parse(row["BuyEndDate"].ToString());
                }
                if (row["BuyCount"] != null && row["BuyCount"].ToString() != "")
                {
                    model.BuyCount = int.Parse(row["BuyCount"].ToString());
                }
                if (row["LimitQty"] != null && row["LimitQty"].ToString() != "")
                {
                    model.LimitQty = int.Parse(row["LimitQty"].ToString());
                }
                if (row["RegionId"] != null && row["RegionId"].ToString() != "")
                {
                    model.RegionId = int.Parse(row["RegionId"].ToString());
                }
                if (row["Status"] != null && row["Status"].ToString() != "")
                {
                    model.Status = int.Parse(row["Status"].ToString());
                }
                if (row["Description"] != null)
                {
                    model.Description = row["Description"].ToString();
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
            strSql.Append("select PreProId,ProductId,PreAmount,PreStartDate,PreEndDate,BuyStartDate,BuyEndDate,BuyCount,LimitQty,RegionId,Status,Description ");
            strSql.Append(" FROM Shop_PreProduct ");
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
            strSql.Append(" PreProId,ProductId,PreAmount,PreStartDate,PreEndDate,BuyStartDate,BuyEndDate,BuyCount,LimitQty,RegionId,Status,Description ");
            strSql.Append(" FROM Shop_PreProduct ");
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
            strSql.Append("select count(1) FROM Shop_PreProduct ");
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
                strSql.Append("order by T.PreProId desc");
            }
            strSql.Append(")AS Row, T.*  from Shop_PreProduct T ");
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
                    new SqlParameter("@tblName", SqlDbType.VarChar, 255),
                    new SqlParameter("@fldName", SqlDbType.VarChar, 255),
                    new SqlParameter("@PageSize", SqlDbType.Int),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@IsReCount", SqlDbType.Bit),
                    new SqlParameter("@OrderType", SqlDbType.Bit),
                    new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
                    };
            parameters[0].Value = "Shop_PreProduct";
            parameters[1].Value = "PreProId";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DBHelper.DefaultDBHelper.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  BasicMethod
        #region  ExtensionMethod

        public bool UpdateStatus(string ids, int status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("update Shop_PreProduct  Set  status ={0}", status);
            strSql.Append(" where PreProId in (" + ids + ")  ");
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


        public bool IsExists(long productId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Shop_PreProduct");
            strSql.Append(" where ProductId=@ProductId");
            SqlParameter[] parameters = {
					new SqlParameter("@ProductId", SqlDbType.BigInt,8)
			};
            parameters[0].Value = productId;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 获取所有的预订商品
        /// </summary>
        /// <returns></returns>
        public int GetTotalCount()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *  FROM  Shop_PreProduct WHERE Status=1 ");
            strSql.Append(" AND EXISTS(SELECT *  FROM PMS_Products WHERE SaleStatus=1 AND SalesType=2 AND ProductId=Shop_PreProduct.ProductId) ");
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
        /// 
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="orderby"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public DataSet GetListByPage(int cid, string orderby, int startIndex, int endIndex)
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
                strSql.Append("order by T.PreProId desc");
            }
            strSql.Append(")AS Row,   T.*,P.ProductName,P.LowestSalePrice,P.MarketPrice,P.ImageUrl,P.ThumbnailUrl1  from Shop_PreProduct T JOIN PMS_Products P ON T.ProductId=P.ProductId  ");
            strSql.Append(" WHERE T.Status=1 AND P.SalesType=2 AND P.SaleStatus=1  ");
            if (cid > 0)//有cid不是默认过来的
            {
                strSql.AppendFormat("   AND EXISTS (SELECT DISTINCT ProductId FROM PMS_ProductCategories  ");
                strSql.AppendFormat(" WHERE  ( CategoryPath LIKE (SELECT Path FROM PMS_Categories WHERE CategoryId={0})+'|%') or  PMS_ProductCategories.CategoryId={0}  AND P.ProductId=ProductId) ", cid);
            }

            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public YSWL.MALL.Model.Shop.PrePro.PreProduct DataRowToModelEx(DataRow row)
        {
            YSWL.MALL.Model.Shop.PrePro.PreProduct model = new YSWL.MALL.Model.Shop.PrePro.PreProduct();
            if (row != null)
            {
                if (row["PreProId"] != null && row["PreProId"].ToString() != "")
                {
                    model.PreProId = int.Parse(row["PreProId"].ToString());
                }
                if (row["ProductId"] != null && row["ProductId"].ToString() != "")
                {
                    model.ProductId = long.Parse(row["ProductId"].ToString());
                }
                if (row["PreAmount"] != null && row["PreAmount"].ToString() != "")
                {
                    model.PreAmount = decimal.Parse(row["PreAmount"].ToString());
                }
                if (row["PreStartDate"] != null && row["PreStartDate"].ToString() != "")
                {
                    model.PreStartDate = DateTime.Parse(row["PreStartDate"].ToString());
                }
                if (row["PreEndDate"] != null && row["PreEndDate"].ToString() != "")
                {
                    model.PreEndDate = DateTime.Parse(row["PreEndDate"].ToString());
                }
                if (row["BuyStartDate"] != null && row["BuyStartDate"].ToString() != "")
                {
                    model.BuyStartDate = DateTime.Parse(row["BuyStartDate"].ToString());
                }
                if (row["BuyEndDate"] != null && row["BuyEndDate"].ToString() != "")
                {
                    model.BuyEndDate = DateTime.Parse(row["BuyEndDate"].ToString());
                }
                if (row["BuyCount"] != null && row["BuyCount"].ToString() != "")
                {
                    model.BuyCount = int.Parse(row["BuyCount"].ToString());
                }
                if (row["LimitQty"] != null && row["LimitQty"].ToString() != "")
                {
                    model.LimitQty = int.Parse(row["LimitQty"].ToString());
                }
                if (row["RegionId"] != null && row["RegionId"].ToString() != "")
                {
                    model.RegionId = int.Parse(row["RegionId"].ToString());
                }
                if (row["Status"] != null && row["Status"].ToString() != "")
                {
                    model.Status = int.Parse(row["Status"].ToString());
                }
                if (row["Description"] != null)
                {
                    model.Description = row["Description"].ToString();
                }
                if (row["ProductName"] != null)
                {
                    model.ProductName = row["ProductName"].ToString();
                }
                if (row["LowestSalePrice"] != null)
                {
                    model.SalePrice = Common.Globals.SafeDecimal(row["LowestSalePrice"].ToString(),0);
                }
                if (row["MarketPrice"] != null)
                {
                    model.MarketPrice = Common.Globals.SafeDecimal(row["MarketPrice"].ToString(),0);
                }
                if (row["ImageUrl"] != null)
                {
                    model.ImageUrl = row["ImageUrl"].ToString();
                }

                if (row["ThumbnailUrl1"] != null)
                {
                    model.ThumbnailUrl = row["ThumbnailUrl1"].ToString();
                }
            }
            return model;
        }


        public YSWL.MALL.Model.Shop.PrePro.PreProduct GetModelInfo(long productId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from Shop_PreProduct ");
            strSql.Append(" where ProductId=@ProductId");
            SqlParameter[] parameters = {
					new SqlParameter("@ProductId", SqlDbType.BigInt,8)
			};
            parameters[0].Value = productId;

            YSWL.MALL.Model.Shop.PrePro.PreProduct model = new YSWL.MALL.Model.Shop.PrePro.PreProduct();
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

        #endregion  ExtensionMethod
    }
}

