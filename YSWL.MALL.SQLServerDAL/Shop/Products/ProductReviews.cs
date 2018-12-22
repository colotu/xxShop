/*----------------------------------------------------------------
// Copyright (C) 2012 云商未来 版权所有。
//
// 文件名：ProductReviews.cs
// 文件功能描述：
// 
// 创建标识： [Name]  2012/08/27 14:50:43
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.MALL.IDAL.Shop.Products;
using YSWL.DBUtility;
namespace YSWL.MALL.SQLServerDAL.Shop.Products
{
	/// <summary>
	/// 数据访问类:ProductReviews
	/// </summary>
	public partial class ProductReviews:IProductReviews
	{
		public ProductReviews()
		{}
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DBHelper.DefaultDBHelper.GetMaxID("ReviewId", "Shop_ProductReviews");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ReviewId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Shop_ProductReviews");
            strSql.Append(" where ReviewId=@ReviewId");
            SqlParameter[] parameters = {
					new SqlParameter("@ReviewId", SqlDbType.Int,4)
			};
            parameters[0].Value = ReviewId;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.MALL.Model.Shop.Products.ProductReviews model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Shop_ProductReviews(");
            strSql.Append("ProductId,UserId,ReviewText,UserName,UserEmail,CreatedDate,ParentId,Status,OrderId,SKU,Attribute,ImagesPath,ImagesNames)");
            strSql.Append(" values (");
            strSql.Append("@ProductId,@UserId,@ReviewText,@UserName,@UserEmail,@CreatedDate,@ParentId,@Status,@OrderId,@SKU,@Attribute,@ImagesPath,@ImagesNames)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@ProductId", SqlDbType.BigInt,8),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@ReviewText", SqlDbType.NVarChar,-1),
					new SqlParameter("@UserName", SqlDbType.NVarChar,200),
					new SqlParameter("@UserEmail", SqlDbType.NVarChar,50),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime),
					new SqlParameter("@ParentId", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@OrderId", SqlDbType.BigInt,8),
					new SqlParameter("@SKU", SqlDbType.NVarChar,200),
					new SqlParameter("@Attribute", SqlDbType.Text),
					new SqlParameter("@ImagesPath", SqlDbType.NVarChar,300),
					new SqlParameter("@ImagesNames", SqlDbType.NVarChar,500)};
            parameters[0].Value = model.ProductId;
            parameters[1].Value = model.UserId;
            parameters[2].Value = model.ReviewText;
            parameters[3].Value = model.UserName;
            parameters[4].Value = model.UserEmail;
            parameters[5].Value = model.CreatedDate;
            parameters[6].Value = model.ParentId;
            parameters[7].Value = model.Status;
            parameters[8].Value = model.OrderId;
            parameters[9].Value = model.SKU;
            parameters[10].Value = model.Attribute;
            parameters[11].Value = model.ImagesPath;
            parameters[12].Value = model.ImagesNames;

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
        public bool Update(YSWL.MALL.Model.Shop.Products.ProductReviews model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Shop_ProductReviews set ");
            strSql.Append("ProductId=@ProductId,");
            strSql.Append("UserId=@UserId,");
            strSql.Append("ReviewText=@ReviewText,");
            strSql.Append("UserName=@UserName,");
            strSql.Append("UserEmail=@UserEmail,");
            strSql.Append("CreatedDate=@CreatedDate,");
            strSql.Append("ParentId=@ParentId,");
            strSql.Append("Status=@Status,");
            strSql.Append("OrderId=@OrderId,");
            strSql.Append("SKU=@SKU,");
            strSql.Append("Attribute=@Attribute,");
            strSql.Append("ImagesPath=@ImagesPath,");
            strSql.Append("ImagesNames=@ImagesNames");
            strSql.Append(" where ReviewId=@ReviewId");
            SqlParameter[] parameters = {
					new SqlParameter("@ProductId", SqlDbType.BigInt,8),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@ReviewText", SqlDbType.NVarChar,-1),
					new SqlParameter("@UserName", SqlDbType.NVarChar,200),
					new SqlParameter("@UserEmail", SqlDbType.NVarChar,50),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime),
					new SqlParameter("@ParentId", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@OrderId", SqlDbType.BigInt,8),
					new SqlParameter("@SKU", SqlDbType.NVarChar,200),
					new SqlParameter("@Attribute", SqlDbType.Text),
					new SqlParameter("@ImagesPath", SqlDbType.NVarChar,300),
					new SqlParameter("@ImagesNames", SqlDbType.NVarChar,500),
					new SqlParameter("@ReviewId", SqlDbType.Int,4)};
            parameters[0].Value = model.ProductId;
            parameters[1].Value = model.UserId;
            parameters[2].Value = model.ReviewText;
            parameters[3].Value = model.UserName;
            parameters[4].Value = model.UserEmail;
            parameters[5].Value = model.CreatedDate;
            parameters[6].Value = model.ParentId;
            parameters[7].Value = model.Status;
            parameters[8].Value = model.OrderId;
            parameters[9].Value = model.SKU;
            parameters[10].Value = model.Attribute;
            parameters[11].Value = model.ImagesPath;
            parameters[12].Value = model.ImagesNames;
            parameters[13].Value = model.ReviewId;

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
        public bool Delete(int ReviewId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Shop_ProductReviews ");
            strSql.Append(" where ReviewId=@ReviewId");
            SqlParameter[] parameters = {
					new SqlParameter("@ReviewId", SqlDbType.Int,4)
			};
            parameters[0].Value = ReviewId;

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
        public bool DeleteList(string ReviewIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Shop_ProductReviews ");
            strSql.Append(" where ReviewId in (" + ReviewIdlist + ")  ");
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
        public YSWL.MALL.Model.Shop.Products.ProductReviews GetModel(int ReviewId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ReviewId,ProductId,UserId,ReviewText,UserName,UserEmail,CreatedDate,ParentId,Status,OrderId,SKU,Attribute,ImagesPath,ImagesNames from Shop_ProductReviews ");
            strSql.Append(" where ReviewId=@ReviewId");
            SqlParameter[] parameters = {
					new SqlParameter("@ReviewId", SqlDbType.Int,4)
			};
            parameters[0].Value = ReviewId;

            YSWL.MALL.Model.Shop.Products.ProductReviews model = new YSWL.MALL.Model.Shop.Products.ProductReviews();
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
        public YSWL.MALL.Model.Shop.Products.ProductReviews DataRowToModel(DataRow row)
        {
            YSWL.MALL.Model.Shop.Products.ProductReviews model = new YSWL.MALL.Model.Shop.Products.ProductReviews();
            if (row != null)
            {
                if (row["ReviewId"] != null && row["ReviewId"].ToString() != "")
                {
                    model.ReviewId = int.Parse(row["ReviewId"].ToString());
                }
                if (row["ProductId"] != null && row["ProductId"].ToString() != "")
                {
                    model.ProductId = long.Parse(row["ProductId"].ToString());
                }
                if (row["UserId"] != null && row["UserId"].ToString() != "")
                {
                    model.UserId = int.Parse(row["UserId"].ToString());
                }
                if (row["ReviewText"] != null)
                {
                    model.ReviewText = row["ReviewText"].ToString();
                }
                if (row["UserName"] != null)
                {
                    model.UserName = row["UserName"].ToString();
                }
                if (row["UserEmail"] != null)
                {
                    model.UserEmail = row["UserEmail"].ToString();
                }
                if (row["CreatedDate"] != null && row["CreatedDate"].ToString() != "")
                {
                    model.CreatedDate = DateTime.Parse(row["CreatedDate"].ToString());
                }
                if (row["ParentId"] != null && row["ParentId"].ToString() != "")
                {
                    model.ParentId = int.Parse(row["ParentId"].ToString());
                }
                if (row["Status"] != null && row["Status"].ToString() != "")
                {
                    model.Status = int.Parse(row["Status"].ToString());
                }
                if (row["OrderId"] != null && row["OrderId"].ToString() != "")
                {
                    model.OrderId = long.Parse(row["OrderId"].ToString());
                }
                if (row["SKU"] != null)
                {
                    model.SKU = row["SKU"].ToString();
                }
                if (row["Attribute"] != null)
                {
                    model.Attribute = row["Attribute"].ToString();
                }
                if (row["ImagesPath"] != null)
                {
                    model.ImagesPath = row["ImagesPath"].ToString();
                }
                if (row["ImagesNames"] != null)
                {
                    model.ImagesNames = row["ImagesNames"].ToString();
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
            strSql.Append("select ReviewId,ProductId,UserId,ReviewText,UserName,UserEmail,CreatedDate,ParentId,Status,OrderId,SKU,Attribute,ImagesPath,ImagesNames ");
            strSql.Append(" FROM Shop_ProductReviews ");
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
            strSql.Append(" ReviewId,ProductId,UserId,ReviewText,UserName,UserEmail,CreatedDate,ParentId,Status,OrderId,SKU,Attribute,ImagesPath,ImagesNames ");
            strSql.Append(" FROM Shop_ProductReviews ");
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
            strSql.Append("select count(1) FROM Shop_ProductReviews ");
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
                strSql.Append("order by T.ReviewId desc");
            }
            strSql.Append(")AS Row, T.*  from Shop_ProductReviews T ");
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
            parameters[0].Value = "Shop_ProductReviews";
            parameters[1].Value = "ReviewId";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DBHelper.DefaultDBHelper.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  BasicMethod

        public DataSet GetList(int? Status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT A.*,b.Score FROM Shop_ProductReviews A ");
            strSql.Append("LEFT JOIN Shop_ScoreDetails B ON A.ReviewId = B.ReviewId ");
            if (Status.HasValue)
            {
                strSql.AppendFormat("WHERE Status ={0} ", Status.Value);
            }
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());

        }
         /// <summary>
         /// 获得商品评论列表
         /// </summary>
         /// <param name="Status"></param>
         /// <returns></returns>
        public DataSet GetListsProdRev(int? Status)
        {
            //SELECT prorev.*,orderitems.ThumbnailsUrl , orderitems.Name  FROM  Shop_ProductReviews  AS  prorev   LEFT JOIN    OMS_OrderItems  AS  orderitems   ON prorev.OrderId = orderitems.OrderId  
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT prorev.*,orderitems.ThumbnailsUrl as ThumbnailsUrl, orderitems.Name as ProductName  FROM  Shop_ProductReviews  AS  prorev    ");
            strSql.Append(" inner JOIN  OMS_OrderItems  AS  orderitems   ON prorev.SKU = orderitems.SKU  and  prorev.OrderId = orderitems.OrderId ");
            if (Status.HasValue)
            {
                strSql.AppendFormat("WHERE Status ={0} ", Status.Value);
            }
            strSql.AppendFormat(" ORDER BY prorev.ReviewId DESC  ");
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }
        
        public  bool AuditComment(string ids,int status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Shop_ProductReviews ");
            strSql.AppendFormat("SET Status ={0}  ", status);
            strSql.AppendFormat("WHERE ReviewId IN({0}) ", ids);
            return DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString()) > 0;
        }

	    /// <summary>
	    /// 增加多条数据
	    /// </summary>
        public bool AddEx(List<Model.Shop.Products.ProductReviews> modelList, long OrderId)
	    {
            List<CommandInfo> sqllist = new List<CommandInfo>();
            Model.Shop.Products.ProductReviews model;
            for (int i = 0; i < modelList.Count; i++)
            {
                model = modelList[i];
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Shop_ProductReviews(");
                strSql.Append("ProductId,UserId,ReviewText,UserName,UserEmail,CreatedDate,ParentId,Status,OrderId,SKU,Attribute,ImagesPath,ImagesNames)");
                strSql.Append(" values (");
                strSql.Append("@ProductId,@UserId,@ReviewText,@UserName,@UserEmail,@CreatedDate,@ParentId,@Status,@OrderId,@SKU,@Attribute,@ImagesPath,@ImagesNames)");
                strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = {
					new SqlParameter("@ProductId", SqlDbType.BigInt,8),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@ReviewText", SqlDbType.NVarChar,-1),
					new SqlParameter("@UserName", SqlDbType.NVarChar,200),
					new SqlParameter("@UserEmail", SqlDbType.NVarChar,50),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime),
					new SqlParameter("@ParentId", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@OrderId", SqlDbType.BigInt,8),
					new SqlParameter("@SKU", SqlDbType.NVarChar,200),
					new SqlParameter("@Attribute", SqlDbType.Text),
					new SqlParameter("@ImagesPath", SqlDbType.NVarChar,300),
					new SqlParameter("@ImagesNames", SqlDbType.NVarChar,500)};
                parameters[0].Value = model.ProductId;
                parameters[1].Value = model.UserId;
                parameters[2].Value = model.ReviewText;
                parameters[3].Value = model.UserName;
                parameters[4].Value = model.UserEmail;
                parameters[5].Value = model.CreatedDate;
                parameters[6].Value = model.ParentId;
                parameters[7].Value = model.Status;
                parameters[8].Value = model.OrderId;
                parameters[9].Value = model.SKU;
                parameters[10].Value = model.Attribute;
                parameters[11].Value = model.ImagesPath;
                parameters[12].Value = model.ImagesNames;
                sqllist.Add(new CommandInfo(strSql.ToString(), parameters));
            }
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("update OMS_Orders set ");
            strSql2.Append("IsReviews=@IsReviews");
            strSql2.Append(" where OrderId=@OrderId");
            SqlParameter[] parameters2 = {
					new SqlParameter("@IsReviews", SqlDbType.Bit,1),
					new SqlParameter("@OrderId", SqlDbType.BigInt,8)};
     
            parameters2[0].Value = true;
            parameters2[1].Value = OrderId;
            sqllist.Add(new CommandInfo(strSql2.ToString(), parameters2));

            int rows = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
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
        /// 获取包含商品图片和用户头像的评论列表(MC01用到)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public DataSet GetListExByPage(int userId, int startIndex, int endIndex)
	    {
	        StringBuilder strSql = new StringBuilder();

	        //strSql.Append(" select A.ProductId, A.UserId,A.ReviewText,A.CreatedDate,A.Attribute,A.ImagesPath,A.ImagesNames, ");
	        //strSql.Append(" b.ImageUrl, ");
	        //strSql.Append(" c.Gravatar ");
	        //strSql.Append(" from Shop_ProductReviews as A  LEFT JOIN PMS_Products AS B  ON A.PRODUCTID=B.ProductId ");
	        //strSql.Append(" left join Accounts_UsersExp as c on a.UserId = c.userId ");
	        //strSql.Append(" where a.UserId = @userId ");

            strSql.Append(" SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER ( ");
            strSql.Append(" order by a.ReviewId desc  ");
            strSql.Append(" )AS Row,");
            strSql.Append(
                " A.ProductId,A.UserId,A.ReviewId,A.ReviewText, A.CreatedDate, A.Attribute, A.ImagesPath, A.ImagesNames, ");
            strSql.Append(" b.ImageUrl,b.ProductName,c.Gravatar ");
            strSql.Append(" from Shop_ProductReviews as A LEFT JOIN PMS_Products AS B ON A.PRODUCTID=B.ProductId ");
            strSql.Append(" left join Accounts_UsersExp as c on A.UserId =c.userId ");
            strSql.Append("  where a.UserId=@userId ");
            strSql.Append(" ) TT ");
            strSql.AppendFormat("WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            SqlParameter[] parameters = {
                    new SqlParameter("@userId", SqlDbType.Int,4)
            };
            parameters[0].Value = userId;

            return DBHelper.DefaultDBHelper.Query(strSql.ToString(),parameters);

        }
        /// <summary>
        /// 获取具体用户评论总数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
	    public int GetRecord(int userId)
	    {
	        string strSql = "select COUNT(1) from Shop_ProductReviews where UserId = " + userId;
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
        /// 转换成包含商品图片和用户头像的model
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public YSWL.MALL.Model.Shop.Products.Reviews DataRowToModelEx(DataRow row)
        {
            YSWL.MALL.Model.Shop.Products.Reviews model = new YSWL.MALL.Model.Shop.Products.Reviews();
            if (row != null)
            {
                if (row["ReviewId"] != null && row["ReviewId"].ToString() != "")
                {
                    model.ReviewId = int.Parse(row["ReviewId"].ToString());
                }
                if (row["ProductId"] != null && row["ProductId"].ToString() != "")
                {
                    model.ProductId = long.Parse(row["ProductId"].ToString());
                }
                if (row["UserId"] != null && row["UserId"].ToString() != "")
                {
                    model.UserId = int.Parse(row["UserId"].ToString());
                }
                if (row["ReviewText"] != null)
                {
                    model.ReviewText = row["ReviewText"].ToString();
                }
                if (row["CreatedDate"] != null && row["CreatedDate"].ToString() != "")
                {
                    model.CreatedDate = DateTime.Parse(row["CreatedDate"].ToString());
                }
                if (row["Attribute"] != null)
                {
                    model.Attribute = row["Attribute"].ToString();
                }
                if (row["ImagesPath"] != null)
                {
                    model.ImagesPath = row["ImagesPath"].ToString();
                }
                if (row["ImagesNames"] != null)
                {
                    model.ImagesNames = row["ImagesNames"].ToString();
                }
                if (row["ImageUrl"] != null)
                {
                    model.ImageUrl = row["ImageUrl"].ToString();
                }
                if (row["Gravatar"] != null)
                {
                    model.Gravatar = row["Gravatar"].ToString();
                }
                if (row["ProductName"] != null)
                {
                    model.ProductName = row["ProductName"].ToString();
                }
            }
            return model;
        }
    }
}

