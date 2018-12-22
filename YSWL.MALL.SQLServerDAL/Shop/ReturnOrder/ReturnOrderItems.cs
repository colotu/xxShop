/**  版本信息模板在安装目录下，可自行修改。
* ReturnOrderItems.cs
*
* 功 能： N/A
* 类 名： ReturnOrderItems
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/7/2 11:50:36   N/A    初版  
* 负责人   [hhy]
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
using YSWL.MALL.IDAL.Shop.ReturnOrder;
using YSWL.DBUtility;//Please add references
namespace YSWL.MALL.SQLServerDAL.Shop.ReturnOrder
{
	/// <summary>
	/// 数据访问类:ReturnOrderItems
	/// </summary>
	public partial class ReturnOrderItems:IReturnOrderItems
	{
		public ReturnOrderItems()
		{}
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long ItemId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Shop_ReturnOrderItems");
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
        public long Add(YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderItems model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Shop_ReturnOrderItems(");
            strSql.Append("OrderItemId,ReturnOrderId,ReturnOrderCode,OrderId,OrderCode,ProductId,ProductCode,SKU,Name,ThumbnailsUrl,Description,Quantity,ShipmentQuantity,CostPrice,SellPrice,ReturnPrice,Attribute,Remark,Weight,Deduct,Points,ProductLineId,SupplierId,SupplierName,BrandId,BrandName,ProductType)");
            strSql.Append(" values (");
            strSql.Append("@OrderItemId,@ReturnOrderId,@ReturnOrderCode,@OrderId,@OrderCode,@ProductId,@ProductCode,@SKU,@Name,@ThumbnailsUrl,@Description,@Quantity,@ShipmentQuantity,@CostPrice,@SellPrice,@ReturnPrice,@Attribute,@Remark,@Weight,@Deduct,@Points,@ProductLineId,@SupplierId,@SupplierName,@BrandId,@BrandName,@ProductType)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@OrderItemId", SqlDbType.BigInt,8),
					new SqlParameter("@ReturnOrderId", SqlDbType.BigInt,8),
					new SqlParameter("@ReturnOrderCode", SqlDbType.NVarChar,50),
					new SqlParameter("@OrderId", SqlDbType.BigInt,8),
					new SqlParameter("@OrderCode", SqlDbType.NVarChar,50),
					new SqlParameter("@ProductId", SqlDbType.BigInt,8),
					new SqlParameter("@ProductCode", SqlDbType.NVarChar,50),
					new SqlParameter("@SKU", SqlDbType.NVarChar,200),
					new SqlParameter("@Name", SqlDbType.NVarChar,200),
					new SqlParameter("@ThumbnailsUrl", SqlDbType.NVarChar,300),
					new SqlParameter("@Description", SqlDbType.NVarChar,500),
					new SqlParameter("@Quantity", SqlDbType.Int,4),
					new SqlParameter("@ShipmentQuantity", SqlDbType.Decimal,9),
					new SqlParameter("@CostPrice", SqlDbType.Money,8),
					new SqlParameter("@SellPrice", SqlDbType.Money,8),
					new SqlParameter("@ReturnPrice", SqlDbType.Money,8),
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
					new SqlParameter("@ProductType", SqlDbType.SmallInt,2)};
            parameters[0].Value = model.OrderItemId;
            parameters[1].Value = model.ReturnOrderId;
            parameters[2].Value = model.ReturnOrderCode;
            parameters[3].Value = model.OrderId;
            parameters[4].Value = model.OrderCode;
            parameters[5].Value = model.ProductId;
            parameters[6].Value = model.ProductCode;
            parameters[7].Value = model.SKU;
            parameters[8].Value = model.Name;
            parameters[9].Value = model.ThumbnailsUrl;
            parameters[10].Value = model.Description;
            parameters[11].Value = model.Quantity;
            parameters[12].Value = model.ShipmentQuantity;
            parameters[13].Value = model.CostPrice;
            parameters[14].Value = model.SellPrice;
            parameters[15].Value = model.ReturnPrice;
            parameters[16].Value = model.Attribute;
            parameters[17].Value = model.Remark;
            parameters[18].Value = model.Weight;
            parameters[19].Value = model.Deduct;
            parameters[20].Value = model.Points;
            parameters[21].Value = model.ProductLineId;
            parameters[22].Value = model.SupplierId;
            parameters[23].Value = model.SupplierName;
            parameters[24].Value = model.BrandId;
            parameters[25].Value = model.BrandName;
            parameters[26].Value = model.ProductType;

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
        public bool Update(YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderItems model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Shop_ReturnOrderItems set ");
            strSql.Append("OrderItemId=@OrderItemId,");
            strSql.Append("ReturnOrderId=@ReturnOrderId,");
            strSql.Append("ReturnOrderCode=@ReturnOrderCode,");
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
            strSql.Append("ReturnPrice=@ReturnPrice,");
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
            strSql.Append("ProductType=@ProductType");
            strSql.Append(" where ItemId=@ItemId");
            SqlParameter[] parameters = {
					new SqlParameter("@OrderItemId", SqlDbType.BigInt,8),
					new SqlParameter("@ReturnOrderId", SqlDbType.BigInt,8),
					new SqlParameter("@ReturnOrderCode", SqlDbType.NVarChar,50),
					new SqlParameter("@OrderId", SqlDbType.BigInt,8),
					new SqlParameter("@OrderCode", SqlDbType.NVarChar,50),
					new SqlParameter("@ProductId", SqlDbType.BigInt,8),
					new SqlParameter("@ProductCode", SqlDbType.NVarChar,50),
					new SqlParameter("@SKU", SqlDbType.NVarChar,200),
					new SqlParameter("@Name", SqlDbType.NVarChar,200),
					new SqlParameter("@ThumbnailsUrl", SqlDbType.NVarChar,300),
					new SqlParameter("@Description", SqlDbType.NVarChar,500),
					new SqlParameter("@Quantity", SqlDbType.Int,4),
					new SqlParameter("@ShipmentQuantity", SqlDbType.Decimal,9),
					new SqlParameter("@CostPrice", SqlDbType.Money,8),
					new SqlParameter("@SellPrice", SqlDbType.Money,8),
					new SqlParameter("@ReturnPrice", SqlDbType.Money,8),
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
					new SqlParameter("@ItemId", SqlDbType.BigInt,8)};
            parameters[0].Value = model.OrderItemId;
            parameters[1].Value = model.ReturnOrderId;
            parameters[2].Value = model.ReturnOrderCode;
            parameters[3].Value = model.OrderId;
            parameters[4].Value = model.OrderCode;
            parameters[5].Value = model.ProductId;
            parameters[6].Value = model.ProductCode;
            parameters[7].Value = model.SKU;
            parameters[8].Value = model.Name;
            parameters[9].Value = model.ThumbnailsUrl;
            parameters[10].Value = model.Description;
            parameters[11].Value = model.Quantity;
            parameters[12].Value = model.ShipmentQuantity;
            parameters[13].Value = model.CostPrice;
            parameters[14].Value = model.SellPrice;
            parameters[15].Value = model.ReturnPrice;
            parameters[16].Value = model.Attribute;
            parameters[17].Value = model.Remark;
            parameters[18].Value = model.Weight;
            parameters[19].Value = model.Deduct;
            parameters[20].Value = model.Points;
            parameters[21].Value = model.ProductLineId;
            parameters[22].Value = model.SupplierId;
            parameters[23].Value = model.SupplierName;
            parameters[24].Value = model.BrandId;
            parameters[25].Value = model.BrandName;
            parameters[26].Value = model.ProductType;
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
            strSql.Append("delete from Shop_ReturnOrderItems ");
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
            strSql.Append("delete from Shop_ReturnOrderItems ");
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
        public YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderItems GetModel(long ItemId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ItemId,OrderItemId,ReturnOrderId,ReturnOrderCode,OrderId,OrderCode,ProductId,ProductCode,SKU,Name,ThumbnailsUrl,Description,Quantity,ShipmentQuantity,CostPrice,SellPrice,ReturnPrice,Attribute,Remark,Weight,Deduct,Points,ProductLineId,SupplierId,SupplierName,BrandId,BrandName,ProductType from Shop_ReturnOrderItems ");
            strSql.Append(" where ItemId=@ItemId");
            SqlParameter[] parameters = {
					new SqlParameter("@ItemId", SqlDbType.BigInt)
			};
            parameters[0].Value = ItemId;

            YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderItems model = new YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderItems();
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
        public YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderItems DataRowToModel(DataRow row)
        {
            YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderItems model = new YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderItems();
            if (row != null)
            {
                if (row["ItemId"] != null && row["ItemId"].ToString() != "")
                {
                    model.ItemId = long.Parse(row["ItemId"].ToString());
                }
                if (row["OrderItemId"] != null && row["OrderItemId"].ToString() != "")
                {
                    model.OrderItemId = long.Parse(row["OrderItemId"].ToString());
                }
                if (row["ReturnOrderId"] != null && row["ReturnOrderId"].ToString() != "")
                {
                    model.ReturnOrderId = long.Parse(row["ReturnOrderId"].ToString());
                }
                if (row["ReturnOrderCode"] != null)
                {
                    model.ReturnOrderCode = row["ReturnOrderCode"].ToString();
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
                    model.ShipmentQuantity = decimal.Parse(row["ShipmentQuantity"].ToString());
                }
                if (row["CostPrice"] != null && row["CostPrice"].ToString() != "")
                {
                    model.CostPrice = decimal.Parse(row["CostPrice"].ToString());
                }
                if (row["SellPrice"] != null && row["SellPrice"].ToString() != "")
                {
                    model.SellPrice = decimal.Parse(row["SellPrice"].ToString());
                }
                if (row["ReturnPrice"] != null && row["ReturnPrice"].ToString() != "")
                {
                    model.ReturnPrice = decimal.Parse(row["ReturnPrice"].ToString());
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
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ItemId,OrderItemId,ReturnOrderId,ReturnOrderCode,OrderId,OrderCode,ProductId,ProductCode,SKU,Name,ThumbnailsUrl,Description,Quantity,ShipmentQuantity,CostPrice,SellPrice,ReturnPrice,Attribute,Remark,Weight,Deduct,Points,ProductLineId,SupplierId,SupplierName,BrandId,BrandName,ProductType ");
            strSql.Append(" FROM Shop_ReturnOrderItems ");
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
            strSql.Append(" ItemId,OrderItemId,ReturnOrderId,ReturnOrderCode,OrderId,OrderCode,ProductId,ProductCode,SKU,Name,ThumbnailsUrl,Description,Quantity,ShipmentQuantity,CostPrice,SellPrice,ReturnPrice,Attribute,Remark,Weight,Deduct,Points,ProductLineId,SupplierId,SupplierName,BrandId,BrandName,ProductType ");
            strSql.Append(" FROM Shop_ReturnOrderItems ");
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
            strSql.Append("select count(1) FROM Shop_ReturnOrderItems ");
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
            strSql.Append(")AS Row, T.*  from Shop_ReturnOrderItems T ");
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
            parameters[0].Value = "Shop_ReturnOrderItems";
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

		#endregion  ExtensionMethod
	}
}

