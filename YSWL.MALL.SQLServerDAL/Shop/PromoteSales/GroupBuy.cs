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
using System.Text;
using System.Data.SqlClient;
using YSWL.MALL.IDAL.Shop.PromoteSales;
using YSWL.DBUtility;//Please add references
namespace YSWL.MALL.SQLServerDAL.Shop.PromoteSales
{
    /// <summary>
    /// 数据访问类:GroupBuy
    /// </summary>
    public partial class GroupBuy : IGroupBuy
    {
        public GroupBuy()
        { }
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DBHelper.DefaultDBHelper.GetMaxID("GroupBuyId", "Shop_GroupBuy");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int GroupBuyId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Shop_GroupBuy");
            strSql.Append(" where GroupBuyId=@GroupBuyId");
            SqlParameter[] parameters = {
                    new SqlParameter("@GroupBuyId", SqlDbType.Int,4)
            };
            parameters[0].Value = GroupBuyId;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.MALL.Model.Shop.PromoteSales.GroupBuy model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Shop_GroupBuy(");
            strSql.Append("ProductId,Sequence,FinePrice,StartDate,EndDate,MaxCount,GroupCount,BuyCount,Price,Status,Description,RegionId,ProductName,ProductCategory,GroupBuyImage,CategoryId,CategoryPath,LimitQty)");
            strSql.Append(" values (");
            strSql.Append("@ProductId,@Sequence,@FinePrice,@StartDate,@EndDate,@MaxCount,@GroupCount,@BuyCount,@Price,@Status,@Description,@RegionId,@ProductName,@ProductCategory,@GroupBuyImage,@CategoryId,@CategoryPath,@LimitQty)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@ProductId", SqlDbType.BigInt,8),
                    new SqlParameter("@Sequence", SqlDbType.Int,4),
                    new SqlParameter("@FinePrice", SqlDbType.Money,8),
                    new SqlParameter("@StartDate", SqlDbType.DateTime),
                    new SqlParameter("@EndDate", SqlDbType.DateTime),
                    new SqlParameter("@MaxCount", SqlDbType.Int,4),
                    new SqlParameter("@GroupCount", SqlDbType.Int,4),
                    new SqlParameter("@BuyCount", SqlDbType.Int,4),
                    new SqlParameter("@Price", SqlDbType.Money,8),
                    new SqlParameter("@Status", SqlDbType.Int,4),
                    new SqlParameter("@Description", SqlDbType.Text),
                    new SqlParameter("@RegionId",SqlDbType.Int) ,
                      new SqlParameter("@ProductName",SqlDbType.NVarChar) ,
                        new SqlParameter("@ProductCategory",SqlDbType.NVarChar) ,
                          new SqlParameter("@GroupBuyImage",SqlDbType.NVarChar) ,
                                        new SqlParameter("@CategoryId",SqlDbType.Int) ,
                      new SqlParameter("@CategoryPath",SqlDbType.NVarChar),
                       new SqlParameter("@LimitQty",SqlDbType.Int)                  
                                        };
            parameters[0].Value = model.ProductId;
            parameters[1].Value = model.Sequence;
            parameters[2].Value = model.FinePrice;
            parameters[3].Value = model.StartDate;
            parameters[4].Value = model.EndDate;
            parameters[5].Value = model.MaxCount;
            parameters[6].Value = model.GroupCount;
            parameters[7].Value = model.BuyCount;
            parameters[8].Value = model.Price;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.Description;
            parameters[11].Value = model.RegionId;
            parameters[12].Value = model.ProductName;
            parameters[13].Value = model.ProductCategory;
            parameters[14].Value = model.GroupBuyImage;
            parameters[15].Value = model.CategoryId;
            parameters[16].Value = model.CategoryPath;
            parameters[17].Value = model.LimitQty;
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
        public bool Update(YSWL.MALL.Model.Shop.PromoteSales.GroupBuy model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Shop_GroupBuy set ");
            strSql.Append("ProductId=@ProductId,");
            strSql.Append("Sequence=@Sequence,");
            strSql.Append("FinePrice=@FinePrice,");
            strSql.Append("StartDate=@StartDate,");
            strSql.Append("EndDate=@EndDate,");
            strSql.Append("MaxCount=@MaxCount,");
            strSql.Append("GroupCount=@GroupCount,");
            strSql.Append("BuyCount=@BuyCount,");
            strSql.Append("Price=@Price,");
            strSql.Append("Status=@Status,");
            strSql.Append("Description=@Description,");
            strSql.Append("RegionId=@RegionId,");

            strSql.Append("ProductName=@ProductName,");
            strSql.Append("ProductCategory=@ProductCategory,");
            strSql.Append("GroupBuyImage=@GroupBuyImage,");

            strSql.Append("CategoryId=@CategoryId,");
            strSql.Append("CategoryPath=@CategoryPath,");
            strSql.Append("LimitQty=@LimitQty");
            strSql.Append(" where GroupBuyId=@GroupBuyId");
            SqlParameter[] parameters = {
                    new SqlParameter("@ProductId", SqlDbType.BigInt,8),
                    new SqlParameter("@Sequence", SqlDbType.Int,4),
                    new SqlParameter("@FinePrice", SqlDbType.Money,8),
                    new SqlParameter("@StartDate", SqlDbType.DateTime),
                    new SqlParameter("@EndDate", SqlDbType.DateTime),
                    new SqlParameter("@MaxCount", SqlDbType.Int,4),
                    new SqlParameter("@GroupCount", SqlDbType.Int,4),
                    new SqlParameter("@BuyCount", SqlDbType.Int,4),
                    new SqlParameter("@Price", SqlDbType.Money,8),
                    new SqlParameter("@Status", SqlDbType.Int,4),
                    new SqlParameter("@Description", SqlDbType.Text),
                    new SqlParameter("@RegionId",SqlDbType.Int), 
                    new SqlParameter("@ProductName",SqlDbType.NVarChar) ,
                        new SqlParameter("@ProductCategory",SqlDbType.NVarChar) ,
                          new SqlParameter("@GroupBuyImage",SqlDbType.NVarChar),
                             new SqlParameter("@CategoryId",SqlDbType.Int) ,
                      new SqlParameter("@CategoryPath",SqlDbType.NVarChar) ,
                    new SqlParameter("@GroupBuyId", SqlDbType.Int,4),
                                        new SqlParameter("@LimitQty",SqlDbType.Int)
                                        };
            parameters[0].Value = model.ProductId;
            parameters[1].Value = model.Sequence;
            parameters[2].Value = model.FinePrice;
            parameters[3].Value = model.StartDate;
            parameters[4].Value = model.EndDate;
            parameters[5].Value = model.MaxCount;
            parameters[6].Value = model.GroupCount;
            parameters[7].Value = model.BuyCount;
            parameters[8].Value = model.Price;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.Description;
            parameters[11].Value = model.RegionId;
            parameters[12].Value = model.ProductName;
            parameters[13].Value = model.ProductCategory;
            parameters[14].Value = model.GroupBuyImage;
            parameters[15].Value = model.CategoryId;
            parameters[16].Value = model.CategoryPath;
            parameters[17].Value = model.GroupBuyId;
            parameters[18].Value = model.LimitQty;
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
        public bool Delete(int GroupBuyId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Shop_GroupBuy ");
            strSql.Append(" where GroupBuyId=@GroupBuyId");
            SqlParameter[] parameters = {
                    new SqlParameter("@GroupBuyId", SqlDbType.Int,4)
            };
            parameters[0].Value = GroupBuyId;

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
        public bool DeleteList(string GroupBuyIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Shop_GroupBuy ");
            strSql.Append(" where GroupBuyId in (" + GroupBuyIdlist + ")  ");
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
        public YSWL.MALL.Model.Shop.PromoteSales.GroupBuy GetModel(int GroupBuyId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 GroupBuyId,ProductId,Sequence,FinePrice,StartDate,EndDate,MaxCount,GroupCount,BuyCount,Price,Status,Description,RegionId,ProductName,ProductCategory,GroupBuyImage,CategoryId,CategoryPath,LimitQty from Shop_GroupBuy ");
            strSql.Append(" where GroupBuyId=@GroupBuyId");
            SqlParameter[] parameters = {
                    new SqlParameter("@GroupBuyId", SqlDbType.Int,4)
            };
            parameters[0].Value = GroupBuyId;

            YSWL.MALL.Model.Shop.PromoteSales.GroupBuy model = new YSWL.MALL.Model.Shop.PromoteSales.GroupBuy();
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
        public YSWL.MALL.Model.Shop.PromoteSales.GroupBuy DataRowToModel(DataRow row)
        {
            YSWL.MALL.Model.Shop.PromoteSales.GroupBuy model = new YSWL.MALL.Model.Shop.PromoteSales.GroupBuy();
            if (row != null)
            {
                if (row["GroupBuyId"] != null && row["GroupBuyId"].ToString() != "")
                {
                    model.GroupBuyId = int.Parse(row["GroupBuyId"].ToString());
                }
                if (row["ProductId"] != null && row["ProductId"].ToString() != "")
                {
                    model.ProductId = long.Parse(row["ProductId"].ToString());
                }
                if (row["Sequence"] != null && row["Sequence"].ToString() != "")
                {
                    model.Sequence = int.Parse(row["Sequence"].ToString());
                }
                if (row["FinePrice"] != null && row["FinePrice"].ToString() != "")
                {
                    model.FinePrice = decimal.Parse(row["FinePrice"].ToString());
                }
                if (row["StartDate"] != null && row["StartDate"].ToString() != "")
                {
                    model.StartDate = DateTime.Parse(row["StartDate"].ToString());
                }
                if (row["EndDate"] != null && row["EndDate"].ToString() != "")
                {
                    model.EndDate = DateTime.Parse(row["EndDate"].ToString());
                }
                if (row["MaxCount"] != null && row["MaxCount"].ToString() != "")
                {
                    model.MaxCount = int.Parse(row["MaxCount"].ToString());
                }
                if (row["GroupCount"] != null && row["GroupCount"].ToString() != "")
                {
                    model.GroupCount = int.Parse(row["GroupCount"].ToString());
                }
                if (row["BuyCount"] != null && row["BuyCount"].ToString() != "")
                {
                    model.BuyCount = int.Parse(row["BuyCount"].ToString());
                }
                if (row["Price"] != null && row["Price"].ToString() != "")
                {
                    model.Price = decimal.Parse(row["Price"].ToString());
                }
                if (row["Status"] != null && row["Status"].ToString() != "")
                {
                    model.Status = int.Parse(row["Status"].ToString());
                }
                if (row["Description"] != null)
                {
                    model.Description = row["Description"].ToString();
                }
                if (row["RegionId"] != null)
                {
                    model.RegionId = Common.Globals.SafeInt(row["RegionId"], -1);
                }
                //,ProductName,ProductCategory,GroupBuyImage
                if (row["ProductName"] != null)
                {
                    model.ProductName = row["ProductName"].ToString();
                }
                if (row["ProductCategory"] != null)
                {
                    model.ProductCategory = row["ProductCategory"].ToString();
                }
                if (row["GroupBuyImage"] != null)
                {
                    model.GroupBuyImage = row["GroupBuyImage"].ToString();
                }
                if (row["CategoryId"] != null)
                {
                    model.CategoryId = Common.Globals.SafeInt(row["CategoryId"], 0);
                }
                if (row["CategoryPath"] != null)
                {
                    model.CategoryPath = row["CategoryPath"].ToString();
                }
                if (row["LimitQty"] != null)
                {
                    model.LimitQty = Common.Globals.SafeInt(row["LimitQty"], 0);
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
            strSql.Append("select GroupBuyId,ProductId,Sequence,FinePrice,StartDate,EndDate,MaxCount,GroupCount,BuyCount,Price,Status,Description,RegionId,ProductName,ProductCategory,GroupBuyImage,CategoryId,CategoryPath,LimitQty ");
            strSql.Append(" FROM Shop_GroupBuy ");
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
            strSql.Append(" GroupBuyId,ProductId,Sequence,FinePrice,StartDate,EndDate,MaxCount,GroupCount,BuyCount,Price,Status,Description,RegionId,ProductName,ProductCategory,GroupBuyImage,CategoryId,CategoryPath,LimitQty ");
            strSql.Append(" FROM Shop_GroupBuy ");
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
            strSql.Append("select count(1) FROM Shop_GroupBuy ");
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
                strSql.Append("order by T.GroupBuyId desc");
            }
            strSql.Append(")AS Row, T.*  from Shop_GroupBuy T ");
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
            parameters[0].Value = "Shop_GroupBuy";
            parameters[1].Value = "GroupBuyId";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DBHelper.DefaultDBHelper.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  BasicMethod
        #region  ExtensionMethod


        public int MaxSequence()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT MAX(Sequence) AS Sequence FROM Shop_GroupBuy");
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

        public bool IsExists(long ProductId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Shop_GroupBuy");
            strSql.Append(" where ProductId=@ProductId");
            SqlParameter[] parameters = {
                    new SqlParameter("@ProductId", SqlDbType.BigInt)
            };
            parameters[0].Value = ProductId;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }

        public bool UpdateStatus(string ids, int status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Shop_GroupBuy set ");
            strSql.Append("Status=@Status");
            strSql.Append(" where GroupBuyId in (" + ids + ")  ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Status", SqlDbType.Int,4)
                                        };
            parameters[0].Value = status;

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
        /// 更新购买数量
        /// </summary>
        /// <param name="buyId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool UpdateBuyCount(int buyId, int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Shop_GroupBuy set ");
            strSql.Append("BuyCount=BuyCount+@BuyCount");
            strSql.Append(" where GroupBuyId =@GroupBuyId ");
            SqlParameter[] parameters = {
                    new SqlParameter("@BuyCount", SqlDbType.Int,4),
                        new SqlParameter("@GroupBuyId", SqlDbType.Int,4)
                                        };
            parameters[0].Value = count;
            parameters[1].Value = buyId;

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

        public DataSet GetListByPage(string strWhere, int cid, int regionId, string orderby, int startIndex, int endIndex)
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
                strSql.Append("order by T.GroupBuyId desc");
            }

            if (regionId > 0)//有选择地区
            {
                strSql.Append(")AS Row, T.*  from Shop_GroupBuy T,Ms_Regions R  ");
                strSql.AppendFormat(" where  (R.ParentId={0} or R.RegionId={0}) And R.RegionId=T.RegionId", regionId);
                strSql.Append(" And  T.Status = 1 AND T.EndDate>=GETDATE()  AND T.StartDate<=GETDATE() ");
            }
            else
            {
                strSql.Append(")AS Row, T.*  from Shop_GroupBuy T  ");
                strSql.Append(" where   T.Status = 1 AND T.EndDate>=GETDATE()  AND T.StartDate<=GETDATE() ");
            }
            strSql.AppendFormat("   AND EXISTS ( SELECT ProductId FROM   PMS_Products P WHERE  SaleStatus = 1 AND T.ProductId = P.ProductId ) ");
            if (cid > 0)//有cid不是默认过来的
            {
                strSql.AppendFormat(" And    (CategoryPath LIKE (SELECT Path FROM PMS_Categories WHERE CategoryId={0})+'|%' ", cid);
                strSql.AppendFormat(" OR T.CategoryId = {0})", cid);
                //strSql.AppendFormat("  And  T.ProductCategory='{0}'", cate);
            }
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql.Append("  And  " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        public int GetCount(string strWhere, int regionId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM  Shop_GroupBuy T,Ms_Regions R  ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
                strSql.Append("And R.RegionId=T.RegionId");
            }
            else
            {
                strSql.Append("where R.RegionId=T.RegionId");
            }
            strSql.Append(" And  T.Status = 1 AND T.EndDate>=GETDATE()  AND T.StartDate<=GETDATE() ");
            strSql.AppendFormat("   AND EXISTS ( SELECT ProductId FROM   PMS_Products P WHERE  SaleStatus = 1 AND T.ProductId = P.ProductId ) ");
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
        public DataSet GetCategory(string strWhere)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("   select * from Shop_GroupBuy T  ");
            sb.Append(" where GroupBuyId=(");
            sb.Append("select min(GroupBuyId) from Shop_GroupBuy");
            sb.Append("  where T.CategoryId=CategoryId)");
            //sb.Append(" And ");
            //sb.Append("   T.Status = 1 AND T.EndDate>=GETDATE()  AND T.StartDate<=GETDATE() ");
            if (!string.IsNullOrWhiteSpace(strWhere))
            {
                sb.AppendFormat(" And  {0}", strWhere);
            }
            return DBHelper.DefaultDBHelper.Query(sb.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, int cid, int regionId, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" GroupBuyId,ProductId,Sequence,FinePrice,StartDate,EndDate,MaxCount,GroupCount,BuyCount,Price,Status,Description,T.RegionId,ProductName,ProductCategory,GroupBuyImage,CategoryId,CategoryPath,LimitQty ");
            strSql.Append(" FROM Shop_GroupBuy T");
            if (regionId > 0)//有选择地区
            {
                strSql.Append(",Ms_Regions R  ");
                strSql.AppendFormat(" where  (R.ParentId={0} or R.RegionId={0}) And R.RegionId=T.RegionId", regionId);
                strSql.Append(" And  T.Status = 1 AND T.EndDate>=GETDATE()  AND T.StartDate<=GETDATE() ");
            }
            else
            {
                strSql.Append(" where   T.Status = 1 AND T.EndDate>=GETDATE()  AND T.StartDate<=GETDATE() ");
            }
            strSql.AppendFormat("   AND EXISTS ( SELECT ProductId FROM   PMS_Products P WHERE  SaleStatus = 1 AND T.ProductId = P.ProductId ) ");
            if (cid > 0)//有cid不是默认过来的
            {
                strSql.AppendFormat(" And    (CategoryPath LIKE (SELECT Path FROM PMS_Categories WHERE CategoryId={0})+'|%' ", cid);
                strSql.AppendFormat(" OR T.CategoryId = {0})", cid);
                //strSql.AppendFormat("  And  T.ProductCategory='{0}'", cate);
            }
            if (String.IsNullOrWhiteSpace(filedOrder))
            {
                strSql.Append(" order by GroupBuyId DESC  ");
            }
            else {
                strSql.Append(" order by " + filedOrder);
            }
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }
        #endregion  ExtensionMethod
    }
}

