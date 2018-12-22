using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.IDAL.Tao;
using YSWL.DBUtility;
using System.Collections.Generic;//Please add references
namespace YSWL.SQLServerDAL.Tao
{
	/// <summary>
	/// 数据访问类:Product
	/// </summary>
	public partial class Product:IProduct
	{
		public Product()
		{}
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long ProductID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Tao_Product");
            strSql.Append(" where ProductID=@ProductID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ProductID", SqlDbType.BigInt,8)			};
            parameters[0].Value = ProductID;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(YSWL.Model.Tao.Product model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Tao_Product(");
            strSql.Append("ProductID,ProductName,ProductDescription,Price,CategoryID,ImageUrl,SellerNick,SellerScore,ClickUrl,ShopUrl,CouponRate,CouponPrice,CouponStartTime,CouponEndTime,CommissionRate,Commission,Rebate,CommissionNum,CommissionVolume,Volume,ShopType,Recomend,Status,Sequence,SkipCount,Tags,CreatedDate)");
            strSql.Append(" values (");
            strSql.Append("@ProductID,@ProductName,@ProductDescription,@Price,@CategoryID,@ImageUrl,@SellerNick,@SellerScore,@ClickUrl,@ShopUrl,@CouponRate,@CouponPrice,@CouponStartTime,@CouponEndTime,@CommissionRate,@Commission,@Rebate,@CommissionNum,@CommissionVolume,@Volume,@ShopType,@Recomend,@Status,@Sequence,@SkipCount,@Tags,@CreatedDate)");
            SqlParameter[] parameters = {
					new SqlParameter("@ProductID", SqlDbType.BigInt,8),
					new SqlParameter("@ProductName", SqlDbType.NVarChar,200),
					new SqlParameter("@ProductDescription", SqlDbType.NVarChar,500),
					new SqlParameter("@Price", SqlDbType.Money,8),
					new SqlParameter("@CategoryID", SqlDbType.Int,4),
					new SqlParameter("@ImageUrl", SqlDbType.NVarChar),
					new SqlParameter("@SellerNick", SqlDbType.NVarChar,200),
					new SqlParameter("@SellerScore", SqlDbType.BigInt,8),
					new SqlParameter("@ClickUrl", SqlDbType.NVarChar),
					new SqlParameter("@ShopUrl", SqlDbType.NVarChar),
					new SqlParameter("@CouponRate", SqlDbType.Decimal,9),
					new SqlParameter("@CouponPrice", SqlDbType.Money,8),
					new SqlParameter("@CouponStartTime", SqlDbType.DateTime),
					new SqlParameter("@CouponEndTime", SqlDbType.DateTime),
					new SqlParameter("@CommissionRate", SqlDbType.Decimal,9),
					new SqlParameter("@Commission", SqlDbType.Money,8),
					new SqlParameter("@Rebate", SqlDbType.Money,8),
					new SqlParameter("@CommissionNum", SqlDbType.Int,4),
					new SqlParameter("@CommissionVolume", SqlDbType.Decimal,9),
					new SqlParameter("@Volume", SqlDbType.BigInt,8),
					new SqlParameter("@ShopType", SqlDbType.NVarChar,50),
					new SqlParameter("@Recomend", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@Sequence", SqlDbType.Int,4),
					new SqlParameter("@SkipCount", SqlDbType.Int,4),
					new SqlParameter("@Tags", SqlDbType.NVarChar,400),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime)};
            parameters[0].Value = model.ProductID;
            parameters[1].Value = model.ProductName;
            parameters[2].Value = model.ProductDescription;
            parameters[3].Value = model.Price;
            parameters[4].Value = model.CategoryID;
            parameters[5].Value = model.ImageUrl;
            parameters[6].Value = model.SellerNick;
            parameters[7].Value = model.SellerScore;
            parameters[8].Value = model.ClickUrl;
            parameters[9].Value = model.ShopUrl;
            parameters[10].Value = model.CouponRate;
            parameters[11].Value = model.CouponPrice;
            parameters[12].Value = model.CouponStartTime;
            parameters[13].Value = model.CouponEndTime;
            parameters[14].Value = model.CommissionRate;
            parameters[15].Value = model.Commission;
            parameters[16].Value = model.Rebate;
            parameters[17].Value = model.CommissionNum;
            parameters[18].Value = model.CommissionVolume;
            parameters[19].Value = model.Volume;
            parameters[20].Value = model.ShopType;
            parameters[21].Value = model.Recomend;
            parameters[22].Value = model.Status;
            parameters[23].Value = model.Sequence;
            parameters[24].Value = model.SkipCount;
            parameters[25].Value = model.Tags;
            parameters[26].Value = model.CreatedDate;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
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
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.Model.Tao.Product model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Tao_Product set ");
            strSql.Append("ProductName=@ProductName,");
            strSql.Append("ProductDescription=@ProductDescription,");
            strSql.Append("Price=@Price,");
            strSql.Append("CategoryID=@CategoryID,");
            strSql.Append("ImageUrl=@ImageUrl,");
            strSql.Append("SellerNick=@SellerNick,");
            strSql.Append("SellerScore=@SellerScore,");
            strSql.Append("ClickUrl=@ClickUrl,");
            strSql.Append("ShopUrl=@ShopUrl,");
            strSql.Append("CouponRate=@CouponRate,");
            strSql.Append("CouponPrice=@CouponPrice,");
            strSql.Append("CouponStartTime=@CouponStartTime,");
            strSql.Append("CouponEndTime=@CouponEndTime,");
            strSql.Append("CommissionRate=@CommissionRate,");
            strSql.Append("Commission=@Commission,");
            strSql.Append("Rebate=@Rebate,");
            strSql.Append("CommissionNum=@CommissionNum,");
            strSql.Append("CommissionVolume=@CommissionVolume,");
            strSql.Append("Volume=@Volume,");
            strSql.Append("ShopType=@ShopType,");
            strSql.Append("Recomend=@Recomend,");
            strSql.Append("Status=@Status,");
            strSql.Append("Sequence=@Sequence,");
            strSql.Append("SkipCount=@SkipCount,");
            strSql.Append("Tags=@Tags,");
            strSql.Append("CreatedDate=@CreatedDate");
            strSql.Append(" where ProductID=@ProductID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ProductName", SqlDbType.NVarChar,200),
					new SqlParameter("@ProductDescription", SqlDbType.NVarChar,500),
					new SqlParameter("@Price", SqlDbType.Money,8),
					new SqlParameter("@CategoryID", SqlDbType.Int,4),
					new SqlParameter("@ImageUrl", SqlDbType.NVarChar),
					new SqlParameter("@SellerNick", SqlDbType.NVarChar,200),
					new SqlParameter("@SellerScore", SqlDbType.BigInt,8),
					new SqlParameter("@ClickUrl", SqlDbType.NVarChar),
					new SqlParameter("@ShopUrl", SqlDbType.NVarChar),
					new SqlParameter("@CouponRate", SqlDbType.Decimal,9),
					new SqlParameter("@CouponPrice", SqlDbType.Money,8),
					new SqlParameter("@CouponStartTime", SqlDbType.DateTime),
					new SqlParameter("@CouponEndTime", SqlDbType.DateTime),
					new SqlParameter("@CommissionRate", SqlDbType.Decimal,9),
					new SqlParameter("@Commission", SqlDbType.Money,8),
					new SqlParameter("@Rebate", SqlDbType.Money,8),
					new SqlParameter("@CommissionNum", SqlDbType.Int,4),
					new SqlParameter("@CommissionVolume", SqlDbType.Decimal,9),
					new SqlParameter("@Volume", SqlDbType.BigInt,8),
					new SqlParameter("@ShopType", SqlDbType.NVarChar,50),
					new SqlParameter("@Recomend", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@Sequence", SqlDbType.Int,4),
					new SqlParameter("@SkipCount", SqlDbType.Int,4),
					new SqlParameter("@Tags", SqlDbType.NVarChar,400),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime),
					new SqlParameter("@ProductID", SqlDbType.BigInt,8)};
            parameters[0].Value = model.ProductName;
            parameters[1].Value = model.ProductDescription;
            parameters[2].Value = model.Price;
            parameters[3].Value = model.CategoryID;
            parameters[4].Value = model.ImageUrl;
            parameters[5].Value = model.SellerNick;
            parameters[6].Value = model.SellerScore;
            parameters[7].Value = model.ClickUrl;
            parameters[8].Value = model.ShopUrl;
            parameters[9].Value = model.CouponRate;
            parameters[10].Value = model.CouponPrice;
            parameters[11].Value = model.CouponStartTime;
            parameters[12].Value = model.CouponEndTime;
            parameters[13].Value = model.CommissionRate;
            parameters[14].Value = model.Commission;
            parameters[15].Value = model.Rebate;
            parameters[16].Value = model.CommissionNum;
            parameters[17].Value = model.CommissionVolume;
            parameters[18].Value = model.Volume;
            parameters[19].Value = model.ShopType;
            parameters[20].Value = model.Recomend;
            parameters[21].Value = model.Status;
            parameters[22].Value = model.Sequence;
            parameters[23].Value = model.SkipCount;
            parameters[24].Value = model.Tags;
            parameters[25].Value = model.CreatedDate;
            parameters[26].Value = model.ProductID;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
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
        public bool Delete(long ProductID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Tao_Product ");
            strSql.Append(" where ProductID=@ProductID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ProductID", SqlDbType.BigInt,8)			};
            parameters[0].Value = ProductID;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
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
        public bool DeleteList(string ProductIDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Tao_Product ");
            strSql.Append(" where ProductID in (" + ProductIDlist + ")  ");
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
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
        public YSWL.Model.Tao.Product GetModel(long ProductID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ProductID,ProductName,ProductDescription,Price,CategoryID,ImageUrl,SellerNick,SellerScore,ClickUrl,ShopUrl,CouponRate,CouponPrice,CouponStartTime,CouponEndTime,CommissionRate,Commission,Rebate,CommissionNum,CommissionVolume,Volume,ShopType,Recomend,Status,Sequence,SkipCount,Tags,CreatedDate from Tao_Product ");
            strSql.Append(" where ProductID=@ProductID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ProductID", SqlDbType.BigInt,8)			};
            parameters[0].Value = ProductID;

            YSWL.Model.Tao.Product model = new YSWL.Model.Tao.Product();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
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
        public YSWL.Model.Tao.Product DataRowToModel(DataRow row)
        {
            YSWL.Model.Tao.Product model = new YSWL.Model.Tao.Product();
            if (row != null)
            {
                if (row["ProductID"] != null && row["ProductID"].ToString() != "")
                {
                    model.ProductID = long.Parse(row["ProductID"].ToString());
                }
                if (row["ProductName"] != null)
                {
                    model.ProductName = row["ProductName"].ToString();
                }
                if (row["ProductDescription"] != null)
                {
                    model.ProductDescription = row["ProductDescription"].ToString();
                }
                if (row["Price"] != null && row["Price"].ToString() != "")
                {
                    model.Price = decimal.Parse(row["Price"].ToString());
                }
                if (row["CategoryID"] != null && row["CategoryID"].ToString() != "")
                {
                    model.CategoryID = int.Parse(row["CategoryID"].ToString());
                }
                if (row["ImageUrl"] != null)
                {
                    model.ImageUrl = row["ImageUrl"].ToString();
                }
                if (row["SellerNick"] != null)
                {
                    model.SellerNick = row["SellerNick"].ToString();
                }
                if (row["SellerScore"] != null && row["SellerScore"].ToString() != "")
                {
                    model.SellerScore = long.Parse(row["SellerScore"].ToString());
                }
                if (row["ClickUrl"] != null)
                {
                    model.ClickUrl = row["ClickUrl"].ToString();
                }
                if (row["ShopUrl"] != null)
                {
                    model.ShopUrl = row["ShopUrl"].ToString();
                }
                if (row["CouponRate"] != null && row["CouponRate"].ToString() != "")
                {
                    model.CouponRate = decimal.Parse(row["CouponRate"].ToString());
                }
                if (row["CouponPrice"] != null && row["CouponPrice"].ToString() != "")
                {
                    model.CouponPrice = decimal.Parse(row["CouponPrice"].ToString());
                }
                if (row["CouponStartTime"] != null && row["CouponStartTime"].ToString() != "")
                {
                    model.CouponStartTime = DateTime.Parse(row["CouponStartTime"].ToString());
                }
                if (row["CouponEndTime"] != null && row["CouponEndTime"].ToString() != "")
                {
                    model.CouponEndTime = DateTime.Parse(row["CouponEndTime"].ToString());
                }
                if (row["CommissionRate"] != null && row["CommissionRate"].ToString() != "")
                {
                    model.CommissionRate = decimal.Parse(row["CommissionRate"].ToString());
                }
                if (row["Commission"] != null && row["Commission"].ToString() != "")
                {
                    model.Commission = decimal.Parse(row["Commission"].ToString());
                }
                if (row["Rebate"] != null && row["Rebate"].ToString() != "")
                {
                    model.Rebate = decimal.Parse(row["Rebate"].ToString());
                }
                if (row["CommissionNum"] != null && row["CommissionNum"].ToString() != "")
                {
                    model.CommissionNum = int.Parse(row["CommissionNum"].ToString());
                }
                if (row["CommissionVolume"] != null && row["CommissionVolume"].ToString() != "")
                {
                    model.CommissionVolume = decimal.Parse(row["CommissionVolume"].ToString());
                }
                if (row["Volume"] != null && row["Volume"].ToString() != "")
                {
                    model.Volume = long.Parse(row["Volume"].ToString());
                }
                if (row["ShopType"] != null)
                {
                    model.ShopType = row["ShopType"].ToString();
                }
                if (row["Recomend"] != null && row["Recomend"].ToString() != "")
                {
                    model.Recomend = int.Parse(row["Recomend"].ToString());
                }
                if (row["Status"] != null && row["Status"].ToString() != "")
                {
                    model.Status = int.Parse(row["Status"].ToString());
                }
                if (row["Sequence"] != null && row["Sequence"].ToString() != "")
                {
                    model.Sequence = int.Parse(row["Sequence"].ToString());
                }
                if (row["SkipCount"] != null && row["SkipCount"].ToString() != "")
                {
                    model.SkipCount = int.Parse(row["SkipCount"].ToString());
                }
                if (row["Tags"] != null)
                {
                    model.Tags = row["Tags"].ToString();
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
            strSql.Append("select ProductID,ProductName,ProductDescription,Price,CategoryID,ImageUrl,SellerNick,SellerScore,ClickUrl,ShopUrl,CouponRate,CouponPrice,CouponStartTime,CouponEndTime,CommissionRate,Commission,Rebate,CommissionNum,CommissionVolume,Volume,ShopType,Recomend,Status,Sequence,SkipCount,Tags,CreatedDate ");
            strSql.Append(" FROM Tao_Product ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
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
            strSql.Append(" ProductID,ProductName,ProductDescription,Price,CategoryID,ImageUrl,SellerNick,SellerScore,ClickUrl,ShopUrl,CouponRate,CouponPrice,CouponStartTime,CouponEndTime,CommissionRate,Commission,Rebate,CommissionNum,CommissionVolume,Volume,ShopType,Recomend,Status,Sequence,SkipCount,Tags,CreatedDate ");
            strSql.Append(" FROM Tao_Product ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM Tao_Product ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperSQL.GetSingle(strSql.ToString());
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
                strSql.Append("order by T.ProductID desc");
            }
            strSql.Append(")AS Row, T.*  from Tao_Product T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.Query(strSql.ToString());
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
            parameters[0].Value = "Tao_Product";
            parameters[1].Value = "ProductID";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  BasicMethod
		#region  ExtensionMethod
        public int GetRecordCountEx(string strWhere, int CateId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM Tao_Product ");
            if (CateId > 0 || strWhere.Length > 1)
            {
                strSql.Append(" where ");
            }
            if (CateId > 0)
            {
                strSql.Append("  CategoryID in ( select CategoryID from Tao_Category where  (CategoryId=" + CateId + " or Path like '" + CateId + "|%'))");
            }
            if (strWhere.Trim() != "")
            {
                if (CateId > 0)
                {
                    strSql.Append(" and ");
                }
                strSql.Append(strWhere);
            }
            object obj = DbHelperSQL.GetSingle(strSql.ToString());
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
        public DataSet GetListByPageEx(string strWhere, int CateId, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.AppendFormat("order by T.{0} DESC", orderby);
            }
            else
            {
                strSql.Append("order by T.ProductID desc");
            }
            strSql.Append(")AS Row, T.*  from Tao_Product T  ");
            if (CateId > 0 || strWhere.Length > 1)
            {
                strSql.Append(" where ");
            }
            if (CateId > 0)
            {
                strSql.Append("  CategoryID in ( select CategoryID from Tao_Category where (CategoryID=" + CateId + " or Path like '" + CateId + "|%'))");
            }
            if (strWhere.Length > 1)
            {
                if (CateId > 0)
                {
                    strSql.Append(" and ");
                }
                strSql.Append(strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.Query(strSql.ToString());
        }

        public DataSet GetListEx(string strWhere, int CateId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM Tao_Product where");
            if (CateId > 0)
            {
                strSql.Append("  CategoryID in ( select CategoryID from Tao_Category where (CategoryID=" + CateId + " or Path like '" + CateId + "|%'))");
            }
            if (strWhere.Trim() != "")
            {
                if (CateId > 0)
                {
                    strSql.Append(" and ");
                }
                strSql.Append(strWhere);
            }
            strSql.Append(" ORDER BY CreatedDate DESC");

            return DbHelperSQL.Query(strSql.ToString());
        }


        /// <summary>
        /// 事务删除一条数据
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public bool DeleteEX(int ProductID)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();
            //删除商品数据
            StringBuilder strSql4 = new StringBuilder();
            strSql4.Append("delete Tao_Product ");
            strSql4.Append(" where ProductID=@ProductID");
            SqlParameter[] parameters4 = {
					new SqlParameter("@ProductID", SqlDbType.Int,4)
			};
            parameters4[0].Value = ProductID;
           CommandInfo cmd = new CommandInfo(strSql4.ToString(), parameters4);
            sqllist.Add(cmd);

            int rowsAffected = DbHelperSQL.ExecuteSqlTran(sqllist);
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
        /// 批量删除数据（事务删除）
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public bool DeleteListEX(string ProductIds)
        {
            int count = ProductIds.Split(',').Length;
            List<CommandInfo> sqllist = new List<CommandInfo>();
            //删除商品数据
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete Tao_Product ");
            strSql.Append(" where ProductID in (" + ProductIds + ")");
             SqlParameter[] parameters = {
					
			};
           CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
            sqllist.Add(cmd);


            int rowsAffected = DbHelperSQL.ExecuteSqlTran(sqllist);
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateCateList(string ProductIds, int CateId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Tao_Product set ");
            strSql.Append("CategoryID=@CategoryID");
            strSql.Append(" where ProductID in (" + ProductIds + ")");
            SqlParameter[] parameters = {
					new SqlParameter("@CategoryID", SqlDbType.Int,4)
					};
            parameters[0].Value = CateId;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateEX(int ProductId, int CateId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Tao_Product set ");
            strSql.Append("CategoryID=@CategoryID");
            strSql.Append(" where ProductID=@ProductID");
            SqlParameter[] parameters = {
					new SqlParameter("@CategoryID", SqlDbType.Int,4),
                    	new SqlParameter("@ProductID", SqlDbType.Int,4)
					};
            parameters[0].Value = CateId;
            parameters[1].Value = ProductId;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //批量推荐到首页
        public bool UpdateRecomendList(string ProductIds, int Recomend)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Tao_Product set ");
            strSql.Append("Recomend=@Recomend");
            strSql.Append(" where ProductID in (" + ProductIds + ")");
            SqlParameter[] parameters = {
					new SqlParameter("@Recomend", SqlDbType.Int,4)
					};
            parameters[0].Value = Recomend;
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateRecomend(int ProductId, int Recomend)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Tao_Product set ");
            strSql.Append("Recomend=@Recomend");
            strSql.Append(" where ProductID =@ProductId");
            SqlParameter[] parameters = {
					new SqlParameter("@Recomend", SqlDbType.Int,4),
                    	new SqlParameter("@ProductId", SqlDbType.Int,4)
					};
            parameters[0].Value = Recomend;
            parameters[1].Value = ProductId;
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateStatus(int ProductId, int Status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Tao_Product set ");
            strSql.Append("Status=@Status");
            strSql.Append(" where ProductID =@ProductId");
            SqlParameter[] parameters = {
					new SqlParameter("@Status", SqlDbType.Int,4),
                    	new SqlParameter("@ProductId", SqlDbType.Int,4)
					};
            parameters[0].Value = Status;
            parameters[1].Value = ProductId;
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

		#endregion  ExtensionMethod
	}
}

