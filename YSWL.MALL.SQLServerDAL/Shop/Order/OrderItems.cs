using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.MALL.IDAL.Shop.Order;
using YSWL.DBUtility;//Please add references
namespace YSWL.MALL.SQLServerDAL.Shop.Order
{
	/// <summary>
	/// 数据访问类:OrderItem
	/// </summary>
	public partial class OrderItems:IOrderItems
	{
		public OrderItems()
		{}
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long ItemId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from OMS_OrderItems");
            strSql.Append(" where ItemId=@ItemId");
            SqlParameter[] parameters = {
					new SqlParameter("@ItemId", SqlDbType.BigInt)
			};
            parameters[0].Value = ItemId;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public long Add(YSWL.MALL.Model.Shop.Order.OrderItems model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into OMS_OrderItems(");
            strSql.Append("OrderId,OrderCode,ProductId,ProductCode,SKU,Name,ThumbnailsUrl,Description,Quantity,ShipmentQuantity,CostPrice,SellPrice,AdjustedPrice,Attribute,Remark,Weight,Deduct,Points,ProductLineId,SupplierId,SupplierName,BrandId,BrandName,ProductType,ReferId,ReferType,Gwjf)");
            strSql.Append(" values (");
            strSql.Append("@OrderId,@OrderCode,@ProductId,@ProductCode,@SKU,@Name,@ThumbnailsUrl,@Description,@Quantity,@ShipmentQuantity,@CostPrice,@SellPrice,@AdjustedPrice,@Attribute,@Remark,@Weight,@Deduct,@Points,@ProductLineId,@SupplierId,@SupplierName,@BrandId,@BrandName,@ProductType,@ReferId,@ReferType,@Gwjf)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@OrderId", SqlDbType.BigInt,8),
					new SqlParameter("@OrderCode", SqlDbType.NVarChar,50),
					new SqlParameter("@ProductId", SqlDbType.BigInt,8),
					new SqlParameter("@ProductCode", SqlDbType.NVarChar,50),
					new SqlParameter("@SKU", SqlDbType.NVarChar,200),
					new SqlParameter("@Name", SqlDbType.NVarChar,200),
					new SqlParameter("@ThumbnailsUrl", SqlDbType.NVarChar,300),
					new SqlParameter("@Description", SqlDbType.NVarChar,500),
					new SqlParameter("@Quantity", SqlDbType.Int,4),
					new SqlParameter("@ShipmentQuantity", SqlDbType.Int,4),
					new SqlParameter("@CostPrice", SqlDbType.Money,8),
					new SqlParameter("@SellPrice", SqlDbType.Money,8),
					new SqlParameter("@AdjustedPrice", SqlDbType.Money,8),
					new SqlParameter("@Attribute", SqlDbType.Text),
					new SqlParameter("@Remark", SqlDbType.Text),
					new SqlParameter("@Weight", SqlDbType.Int,4),
					new SqlParameter("@Deduct", SqlDbType.Money,8),
					new SqlParameter("@Points", SqlDbType.Int,4),
					new SqlParameter("@ProductLineId", SqlDbType.Int,4),
					new SqlParameter("@SupplierId", SqlDbType.Int,4),
					new SqlParameter("@SupplierName", SqlDbType.NVarChar,100),
					new SqlParameter("@BrandId", SqlDbType.Int,4),
					new SqlParameter("@BrandName", SqlDbType.NVarChar,100),
					new SqlParameter("@ProductType", SqlDbType.SmallInt,2),
					new SqlParameter("@ReferId", SqlDbType.Int,4),
					new SqlParameter("@ReferType", SqlDbType.Int,4),
                    new SqlParameter("@Gwjf", SqlDbType.Decimal,9)};
            parameters[0].Value = model.OrderId;
            parameters[1].Value = model.OrderCode;
            parameters[2].Value = model.ProductId;
            parameters[3].Value = model.ProductCode;
            parameters[4].Value = model.SKU;
            parameters[5].Value = model.Name;
            parameters[6].Value = model.ThumbnailsUrl;
            parameters[7].Value = model.Description;
            parameters[8].Value = model.Quantity;
            parameters[9].Value = model.ShipmentQuantity;
            parameters[10].Value = model.CostPrice;
            parameters[11].Value = model.SellPrice;
            parameters[12].Value = model.AdjustedPrice;
            parameters[13].Value = model.Attribute;
            parameters[14].Value = model.Remark;
            parameters[15].Value = model.Weight;
            parameters[16].Value = model.Deduct;
            parameters[17].Value = model.Points;
            parameters[18].Value = model.ProductLineId;
            parameters[19].Value = model.SupplierId;
            parameters[20].Value = model.SupplierName;
            parameters[21].Value = model.BrandId;
            parameters[22].Value = model.BrandName;
            parameters[23].Value = model.ProductType;
            parameters[24].Value = model.ReferId;
            parameters[25].Value = model.ReferType;
            parameters[26].Value = model.Gwjf;

            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt64(obj);
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.Shop.Order.OrderItems model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update OMS_OrderItems set ");
            strSql.Append("OrderId=@OrderId,");
            strSql.Append("OrderCode=@OrderCode,");
            strSql.Append("ProductId=@ProductId,");
            strSql.Append("ProductCode=@ProductCode,");
            strSql.Append("SKU=@SKU,");
            strSql.Append("Name=@Name,");
            strSql.Append("ThumbnailsUrl=@ThumbnailsUrl,");
            strSql.Append("Description=@Description,");
            strSql.Append("Quantity=@Quantity,");
            strSql.Append("ShipmentQuantity=@ShipmentQuantity,");
            strSql.Append("CostPrice=@CostPrice,");
            strSql.Append("SellPrice=@SellPrice,");
            strSql.Append("AdjustedPrice=@AdjustedPrice,");
            strSql.Append("Attribute=@Attribute,");
            strSql.Append("Remark=@Remark,");
            strSql.Append("Weight=@Weight,");
            strSql.Append("Deduct=@Deduct,");
            strSql.Append("Points=@Points,");
            strSql.Append("ProductLineId=@ProductLineId,");
            strSql.Append("SupplierId=@SupplierId,");
            strSql.Append("SupplierName=@SupplierName,");
            strSql.Append("BrandId=@BrandId,");
            strSql.Append("BrandName=@BrandName,");
            strSql.Append("ProductType=@ProductType,");
            strSql.Append("ReferId=@ReferId,");
            strSql.Append("ReferType=@ReferType,");
            strSql.Append("Gwjf=@Gwjf");
            strSql.Append(" where ItemId=@ItemId");
            SqlParameter[] parameters = {
					new SqlParameter("@OrderId", SqlDbType.BigInt,8),
					new SqlParameter("@OrderCode", SqlDbType.NVarChar,50),
					new SqlParameter("@ProductId", SqlDbType.BigInt,8),
					new SqlParameter("@ProductCode", SqlDbType.NVarChar,50),
					new SqlParameter("@SKU", SqlDbType.NVarChar,200),
					new SqlParameter("@Name", SqlDbType.NVarChar,200),
					new SqlParameter("@ThumbnailsUrl", SqlDbType.NVarChar,300),
					new SqlParameter("@Description", SqlDbType.NVarChar,500),
					new SqlParameter("@Quantity", SqlDbType.Int,4),
					new SqlParameter("@ShipmentQuantity", SqlDbType.Int,4),
					new SqlParameter("@CostPrice", SqlDbType.Money,8),
					new SqlParameter("@SellPrice", SqlDbType.Money,8),
					new SqlParameter("@AdjustedPrice", SqlDbType.Money,8),
					new SqlParameter("@Attribute", SqlDbType.Text),
					new SqlParameter("@Remark", SqlDbType.Text),
					new SqlParameter("@Weight", SqlDbType.Int,4),
					new SqlParameter("@Deduct", SqlDbType.Money,8),
					new SqlParameter("@Points", SqlDbType.Int,4),
					new SqlParameter("@ProductLineId", SqlDbType.Int,4),
					new SqlParameter("@SupplierId", SqlDbType.Int,4),
					new SqlParameter("@SupplierName", SqlDbType.NVarChar,100),
					new SqlParameter("@BrandId", SqlDbType.Int,4),
					new SqlParameter("@BrandName", SqlDbType.NVarChar,100),
					new SqlParameter("@ProductType", SqlDbType.SmallInt,2),
					new SqlParameter("@ReferId", SqlDbType.Int,4),
					new SqlParameter("@ReferType", SqlDbType.Int,4),
                    new SqlParameter("@Gwjf", SqlDbType.Decimal,9),
                    new SqlParameter("@ItemId", SqlDbType.BigInt,8)};
            parameters[0].Value = model.OrderId;
            parameters[1].Value = model.OrderCode;
            parameters[2].Value = model.ProductId;
            parameters[3].Value = model.ProductCode;
            parameters[4].Value = model.SKU;
            parameters[5].Value = model.Name;
            parameters[6].Value = model.ThumbnailsUrl;
            parameters[7].Value = model.Description;
            parameters[8].Value = model.Quantity;
            parameters[9].Value = model.ShipmentQuantity;
            parameters[10].Value = model.CostPrice;
            parameters[11].Value = model.SellPrice;
            parameters[12].Value = model.AdjustedPrice;
            parameters[13].Value = model.Attribute;
            parameters[14].Value = model.Remark;
            parameters[15].Value = model.Weight;
            parameters[16].Value = model.Deduct;
            parameters[17].Value = model.Points;
            parameters[18].Value = model.ProductLineId;
            parameters[19].Value = model.SupplierId;
            parameters[20].Value = model.SupplierName;
            parameters[21].Value = model.BrandId;
            parameters[22].Value = model.BrandName;
            parameters[23].Value = model.ProductType;
            parameters[24].Value = model.ReferId;
            parameters[25].Value = model.ReferType;
            parameters[26].Value = model.Gwjf;
            parameters[27].Value = model.ItemId;

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
        public bool Delete(long ItemId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from OMS_OrderItems ");
            strSql.Append(" where ItemId=@ItemId");
            SqlParameter[] parameters = {
					new SqlParameter("@ItemId", SqlDbType.BigInt)
			};
            parameters[0].Value = ItemId;

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
        public bool DeleteList(string ItemIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from OMS_OrderItems ");
            strSql.Append(" where ItemId in (" + ItemIdlist + ")  ");
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
        public YSWL.MALL.Model.Shop.Order.OrderItems GetModel(long ItemId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ItemId,OrderId,OrderCode,ProductId,ProductCode,SKU,Name,ThumbnailsUrl,Description,Quantity,ShipmentQuantity,CostPrice,SellPrice,AdjustedPrice,Attribute,Remark,Weight,Deduct,Points,ProductLineId,SupplierId,SupplierName,BrandId,BrandName,ProductType,ReferId,ReferType,Gwjf from OMS_OrderItems ");
            strSql.Append(" where ItemId=@ItemId");
            SqlParameter[] parameters = {
					new SqlParameter("@ItemId", SqlDbType.BigInt)
			};
            parameters[0].Value = ItemId;

            YSWL.MALL.Model.Shop.Order.OrderItems model = new YSWL.MALL.Model.Shop.Order.OrderItems();
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
        public YSWL.MALL.Model.Shop.Order.OrderItems DataRowToModel(DataRow row)
        {
            YSWL.MALL.Model.Shop.Order.OrderItems model = new YSWL.MALL.Model.Shop.Order.OrderItems();
            if (row != null)
            {
                if (row["ItemId"] != null && row["ItemId"].ToString() != "")
                {
                    model.ItemId = long.Parse(row["ItemId"].ToString());
                }
                if (row["OrderId"] != null && row["OrderId"].ToString() != "")
                {
                    model.OrderId = long.Parse(row["OrderId"].ToString());
                }
                if (row["OrderCode"] != null)
                {
                    model.OrderCode = row["OrderCode"].ToString();
                }
                if (row["ProductId"] != null && row["ProductId"].ToString() != "")
                {
                    model.ProductId = long.Parse(row["ProductId"].ToString());
                }
                if (row["ProductCode"] != null)
                {
                    model.ProductCode = row["ProductCode"].ToString();
                }
                if (row["SKU"] != null)
                {
                    model.SKU = row["SKU"].ToString();
                }
                if (row["Name"] != null)
                {
                    model.Name = row["Name"].ToString();
                }
                if (row["ThumbnailsUrl"] != null)
                {
                    model.ThumbnailsUrl = row["ThumbnailsUrl"].ToString();
                }
                if (row["Description"] != null)
                {
                    model.Description = row["Description"].ToString();
                }
                if (row["Quantity"] != null && row["Quantity"].ToString() != "")
                {
                    model.Quantity = int.Parse(row["Quantity"].ToString());
                }
                if (row["ShipmentQuantity"] != null && row["ShipmentQuantity"].ToString() != "")
                {
                    model.ShipmentQuantity = int.Parse(row["ShipmentQuantity"].ToString());
                }
                if (row["CostPrice"] != null && row["CostPrice"].ToString() != "")
                {
                    model.CostPrice = decimal.Parse(row["CostPrice"].ToString());
                }
                if (row["SellPrice"] != null && row["SellPrice"].ToString() != "")
                {
                    model.SellPrice = decimal.Parse(row["SellPrice"].ToString());
                }
                if (row["AdjustedPrice"] != null && row["AdjustedPrice"].ToString() != "")
                {
                    model.AdjustedPrice = decimal.Parse(row["AdjustedPrice"].ToString());
                }
                if (row["Attribute"] != null)
                {
                    model.Attribute = row["Attribute"].ToString();
                }
                if (row["Remark"] != null)
                {
                    model.Remark = row["Remark"].ToString();
                }
                if (row["Weight"] != null && row["Weight"].ToString() != "")
                {
                    model.Weight = int.Parse(row["Weight"].ToString());
                }
                if (row["Deduct"] != null && row["Deduct"].ToString() != "")
                {
                    model.Deduct = decimal.Parse(row["Deduct"].ToString());
                }
                if (row["Points"] != null && row["Points"].ToString() != "")
                {
                    model.Points = int.Parse(row["Points"].ToString());
                }
                if (row["ProductLineId"] != null && row["ProductLineId"].ToString() != "")
                {
                    model.ProductLineId = int.Parse(row["ProductLineId"].ToString());
                }
                if (row["SupplierId"] != null && row["SupplierId"].ToString() != "")
                {
                    model.SupplierId = int.Parse(row["SupplierId"].ToString());
                }
                if (row["SupplierName"] != null)
                {
                    model.SupplierName = row["SupplierName"].ToString();
                }
                if (row["BrandId"] != null && row["BrandId"].ToString() != "")
                {
                    model.BrandId = int.Parse(row["BrandId"].ToString());
                }
                if (row["BrandName"] != null)
                {
                    model.BrandName = row["BrandName"].ToString();
                }
                if (row["ProductType"] != null && row["ProductType"].ToString() != "")
                {
                    model.ProductType = int.Parse(row["ProductType"].ToString());
                }
                if (row["ReferId"] != null && row["ReferId"].ToString() != "")
                {
                    model.ReferId = int.Parse(row["ReferId"].ToString());
                }
                if (row["ReferType"] != null && row["ReferType"].ToString() != "")
                {
                    model.ReferType = int.Parse(row["ReferType"].ToString());
                }
                if (row["Gwjf"] != null && row["Gwjf"].ToString() != "")
                {
                    model.Gwjf = int.Parse(row["Gwjf"].ToString());
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
            strSql.Append("select ItemId,OrderId,OrderCode,ProductId,ProductCode,SKU,Name,ThumbnailsUrl,Description,Quantity,ShipmentQuantity,CostPrice,SellPrice,AdjustedPrice,Attribute,Remark,Weight,Deduct,Points,ProductLineId,SupplierId,SupplierName,BrandId,BrandName,ProductType,ReferId,ReferType,Gwjf ");
            strSql.Append(" FROM OMS_OrderItems ");
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
            strSql.Append(" ItemId,OrderId,OrderCode,ProductId,ProductCode,SKU,Name,ThumbnailsUrl,Description,Quantity,ShipmentQuantity,CostPrice,SellPrice,AdjustedPrice,Attribute,Remark,Weight,Deduct,Points,ProductLineId,SupplierId,SupplierName,BrandId,BrandName,ProductType,ReferId,ReferType,Gwjf ");
            strSql.Append(" FROM OMS_OrderItems ");
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
            strSql.Append("select count(1) FROM OMS_OrderItems ");
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
        /// 获取记录总数
        /// </summary>
        public int GetRecordSum(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(Quantity) FROM OMS_OrderItems ");
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
                strSql.Append("order by T.ItemId desc");
            }
            strSql.Append(")AS Row, T.*  from OMS_OrderItems T ");
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
            parameters[0].Value = "OMS_OrderItems";
            parameters[1].Value = "ItemId";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DBHelper.DefaultDBHelper.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  BasicMethod
		#region  ExtensionMethod

	    public  DataSet GetSaleRecordByPage(long  productId ,string  orderby , int startIndex ,int endIndex )
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
                strSql.Append("order by T.ItemId desc");
            }
            strSql.Append(")AS Row,  T.SellPrice , T.ShipmentQuantity , p.BuyerName ,p.CreatedDate  from OMS_OrderItems T ");
            strSql.AppendFormat(" JOIN OMS_Orders p ON T.ProductId = {0} AND p.OrderStatus = 2  and  T.OrderId=p.OrderId", productId);
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
	    }


	    public int GetSaleRecordCount(long productId)
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT COUNT(1) FROM OMS_OrderItems tt JOIN    OMS_Orders p");
	        strSql.AppendFormat(" ON tt.ProductId={0} AND p.OrderStatus=2 AND tt.OrderId=p.OrderId", productId);
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


        public DataSet GetCommission(decimal DErate, decimal CPrate)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(
                @"
--设计师 总量/额
SELECT  1 AS [Type], SUM(I.Quantity) ToalQuantity
      , SUM(I.SellPrice * I.Quantity) * {0} ToalPrice
FROM    OMS_OrderItems I, OMS_Orders O,Accounts_Users U,PMS_Products P
WHERE I.OrderId = O.OrderId
  AND P.ProductId=I.ProductId AND P.CreateUserID=U.UserID AND U.UserType = 'DE'
AND O.OrderStatus = 2 AND O.OrderType = 1
UNION ALL      
--CP 总量/额         
SELECT  2 AS [Type], SUM(I.Quantity) ToalQuantity
      , SUM(I.SellPrice * I.Quantity) * {1} ToalPrice
FROM    OMS_OrderItems I, OMS_Orders O,Accounts_Users U,PMS_Products P
WHERE I.OrderId = O.OrderId
 AND P.ProductId=I.ProductId AND P.CreateUserID=U.UserID AND U.UserType = 'CP'
AND O.OrderStatus = 2 AND O.OrderType = 1

", DErate, CPrate);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 根据商家Id获取售出商品总数
        /// </summary>
        public int GetRecordSum(int supplierId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(Quantity) FROM OMS_OrderItems ");
            strSql.AppendFormat(" where  supplierid ={0} " , supplierId);
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
    #endregion  ExtensionMethod

    #region  本月商品销售数量

    /// <summary>
    /// 本月商品销售数量
    /// </summary>
    /// <returns></returns>
    public DataSet GetSaleResult()
    {
      StringBuilder strSql = new StringBuilder();
      strSql.Append(@"SELECT  TOP 5 SUM(L.Quantity) AS Count,MAX(L.Name) AS Name
                            FROM OMS_Orders AS M INNER JOIN OMS_OrderItems AS L ON M.OrderId = L.OrderId
                            WHERE OrderStatus<>-1 AND YEAR(M.CreatedDate)=YEAR(GETDATE()) AND MONTH(M.CreatedDate)=MONTH(GETDATE())
                            GROUP BY L.ProductId
                            ORDER BY SUM(L.Quantity) DESC");
      return DBHelper.DefaultDBHelper.Query(strSql.ToString());
    }

    #endregion

    #region  本月商品销售额排行

    /// <summary>
    /// 本月商品销售额排行
    /// </summary>
    /// <returns></returns>
    public DataSet GetSaleAmountResult()
    {
      StringBuilder strSql = new StringBuilder();
      strSql.Append(@"SELECT TOP 5 SUM(L.AdjustedPrice*L.Quantity) AS Amount,MAX(L.Name) AS Name
                            FROM OMS_Orders AS M INNER JOIN OMS_OrderItems AS L ON M.OrderId = L.OrderId
                            WHERE OrderStatus<>-1 AND YEAR(M.CreatedDate)=YEAR(GETDATE()) AND MONTH(M.CreatedDate)=MONTH(GETDATE())
                            GROUP BY L.ProductId
                            ORDER BY SUM(L.AdjustedPrice*L.Quantity) DESC");
      return DBHelper.DefaultDBHelper.Query(strSql.ToString());
    }

    #endregion
  }
}

