using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.IDAL.Tao;
using YSWL.DBUtility;//Please add references
namespace YSWL.SQLServerDAL.Tao
{
	/// <summary>
	/// 数据访问类:Shop
	/// </summary>
	public partial class Shop:IShop
	{
		public Shop()
		{}
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long ShopId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Tao_Shop");
            strSql.Append(" where ShopId=@ShopId");
            SqlParameter[] parameters = {
					new SqlParameter("@ShopId", SqlDbType.BigInt)
			};
            parameters[0].Value = ShopId;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public long Add(YSWL.Model.Tao.Shop model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Tao_Shop(");
            strSql.Append("CategoryId,ShopName,ShopLogo,SellerNick,ClickUrl,CommissionRate,SellerCredit,TotalAuction,AuctionCount,Recomend,Status,ImageUrl)");
            strSql.Append(" values (");
            strSql.Append("@CategoryId,@ShopName,@ShopLogo,@SellerNick,@ClickUrl,@CommissionRate,@SellerCredit,@TotalAuction,@AuctionCount,@Recomend,@Status,@ImageUrl)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@CategoryId", SqlDbType.Int,4),
					new SqlParameter("@ShopName", SqlDbType.NVarChar,300),
					new SqlParameter("@ShopLogo", SqlDbType.NVarChar,300),
					new SqlParameter("@SellerNick", SqlDbType.NVarChar,300),
					new SqlParameter("@ClickUrl", SqlDbType.NVarChar),
					new SqlParameter("@CommissionRate", SqlDbType.NVarChar,200),
					new SqlParameter("@SellerCredit", SqlDbType.NVarChar,50),
					new SqlParameter("@TotalAuction", SqlDbType.NVarChar,200),
					new SqlParameter("@AuctionCount", SqlDbType.BigInt,8),
					new SqlParameter("@Recomend", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@ImageUrl", SqlDbType.NVarChar)};
            parameters[0].Value = model.CategoryId;
            parameters[1].Value = model.ShopName;
            parameters[2].Value = model.ShopLogo;
            parameters[3].Value = model.SellerNick;
            parameters[4].Value = model.ClickUrl;
            parameters[5].Value = model.CommissionRate;
            parameters[6].Value = model.SellerCredit;
            parameters[7].Value = model.TotalAuction;
            parameters[8].Value = model.AuctionCount;
            parameters[9].Value = model.Recomend;
            parameters[10].Value = model.Status;
            parameters[11].Value = model.ImageUrl;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
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
        public bool Update(YSWL.Model.Tao.Shop model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Tao_Shop set ");
            strSql.Append("CategoryId=@CategoryId,");
            strSql.Append("ShopName=@ShopName,");
            strSql.Append("ShopLogo=@ShopLogo,");
            strSql.Append("SellerNick=@SellerNick,");
            strSql.Append("ClickUrl=@ClickUrl,");
            strSql.Append("CommissionRate=@CommissionRate,");
            strSql.Append("SellerCredit=@SellerCredit,");
            strSql.Append("TotalAuction=@TotalAuction,");
            strSql.Append("AuctionCount=@AuctionCount,");
            strSql.Append("Recomend=@Recomend,");
            strSql.Append("Status=@Status,");
            strSql.Append("ImageUrl=@ImageUrl");
            strSql.Append(" where ShopId=@ShopId");
            SqlParameter[] parameters = {
					new SqlParameter("@CategoryId", SqlDbType.Int,4),
					new SqlParameter("@ShopName", SqlDbType.NVarChar,300),
					new SqlParameter("@ShopLogo", SqlDbType.NVarChar,300),
					new SqlParameter("@SellerNick", SqlDbType.NVarChar,300),
					new SqlParameter("@ClickUrl", SqlDbType.NVarChar),
					new SqlParameter("@CommissionRate", SqlDbType.NVarChar,200),
					new SqlParameter("@SellerCredit", SqlDbType.NVarChar,50),
					new SqlParameter("@TotalAuction", SqlDbType.NVarChar,200),
					new SqlParameter("@AuctionCount", SqlDbType.BigInt,8),
					new SqlParameter("@Recomend", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@ImageUrl", SqlDbType.NVarChar),
					new SqlParameter("@ShopId", SqlDbType.BigInt,8)};
            parameters[0].Value = model.CategoryId;
            parameters[1].Value = model.ShopName;
            parameters[2].Value = model.ShopLogo;
            parameters[3].Value = model.SellerNick;
            parameters[4].Value = model.ClickUrl;
            parameters[5].Value = model.CommissionRate;
            parameters[6].Value = model.SellerCredit;
            parameters[7].Value = model.TotalAuction;
            parameters[8].Value = model.AuctionCount;
            parameters[9].Value = model.Recomend;
            parameters[10].Value = model.Status;
            parameters[11].Value = model.ImageUrl;
            parameters[12].Value = model.ShopId;

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
        public bool Delete(long ShopId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Tao_Shop ");
            strSql.Append(" where ShopId=@ShopId");
            SqlParameter[] parameters = {
					new SqlParameter("@ShopId", SqlDbType.BigInt)
			};
            parameters[0].Value = ShopId;

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
        public bool DeleteList(string ShopIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Tao_Shop ");
            strSql.Append(" where ShopId in (" + ShopIdlist + ")  ");
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
        public YSWL.Model.Tao.Shop GetModel(long ShopId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ShopId,CategoryId,ShopName,ShopLogo,SellerNick,ClickUrl,CommissionRate,SellerCredit,TotalAuction,AuctionCount,Recomend,Status,ImageUrl from Tao_Shop ");
            strSql.Append(" where ShopId=@ShopId");
            SqlParameter[] parameters = {
					new SqlParameter("@ShopId", SqlDbType.BigInt)
			};
            parameters[0].Value = ShopId;

            YSWL.Model.Tao.Shop model = new YSWL.Model.Tao.Shop();
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
        public YSWL.Model.Tao.Shop DataRowToModel(DataRow row)
        {
            YSWL.Model.Tao.Shop model = new YSWL.Model.Tao.Shop();
            if (row != null)
            {
                if (row["ShopId"] != null && row["ShopId"].ToString() != "")
                {
                    model.ShopId = long.Parse(row["ShopId"].ToString());
                }
                if (row["CategoryId"] != null && row["CategoryId"].ToString() != "")
                {
                    model.CategoryId = int.Parse(row["CategoryId"].ToString());
                }
                if (row["ShopName"] != null)
                {
                    model.ShopName = row["ShopName"].ToString();
                }
                if (row["ShopLogo"] != null)
                {
                    model.ShopLogo = row["ShopLogo"].ToString();
                }
                if (row["SellerNick"] != null)
                {
                    model.SellerNick = row["SellerNick"].ToString();
                }
                if (row["ClickUrl"] != null)
                {
                    model.ClickUrl = row["ClickUrl"].ToString();
                }
                if (row["CommissionRate"] != null)
                {
                    model.CommissionRate = row["CommissionRate"].ToString();
                }
                if (row["SellerCredit"] != null)
                {
                    model.SellerCredit = row["SellerCredit"].ToString();
                }
                if (row["TotalAuction"] != null)
                {
                    model.TotalAuction = row["TotalAuction"].ToString();
                }
                if (row["AuctionCount"] != null && row["AuctionCount"].ToString() != "")
                {
                    model.AuctionCount = long.Parse(row["AuctionCount"].ToString());
                }
                if (row["Recomend"] != null && row["Recomend"].ToString() != "")
                {
                    model.Recomend = int.Parse(row["Recomend"].ToString());
                }
                if (row["Status"] != null && row["Status"].ToString() != "")
                {
                    model.Status = int.Parse(row["Status"].ToString());
                }
                if (row["ImageUrl"] != null)
                {
                    model.ImageUrl = row["ImageUrl"].ToString();
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
            strSql.Append("select ShopId,CategoryId,ShopName,ShopLogo,SellerNick,ClickUrl,CommissionRate,SellerCredit,TotalAuction,AuctionCount,Recomend,Status,ImageUrl ");
            strSql.Append(" FROM Tao_Shop ");
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
            strSql.Append(" ShopId,CategoryId,ShopName,ShopLogo,SellerNick,ClickUrl,CommissionRate,SellerCredit,TotalAuction,AuctionCount,Recomend,Status,ImageUrl ");
            strSql.Append(" FROM Tao_Shop ");
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
            strSql.Append("select count(1) FROM Tao_Shop ");
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
                strSql.Append("order by T.ShopId desc");
            }
            strSql.Append(")AS Row, T.*  from Tao_Shop T ");
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
            parameters[0].Value = "Tao_Shop";
            parameters[1].Value = "ShopId";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  BasicMethod
		#region  ExtensionMethod
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Tao_Shop");
            strSql.Append(" where ShopName=@ShopName");
            SqlParameter[] parameters = {
						new SqlParameter("@ShopName", SqlDbType.NVarChar,300)
			};
            parameters[0].Value = name;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        public bool UpdateCateList(string ids, int CateId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Tao_Shop set ");
            strSql.Append("CategoryID=@CategoryID");
            strSql.Append(" where ShopId in (" + ids + ")");
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

        public bool UpdateStateList(string ids, int state)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Tao_Shop  set Status=@Status");
            strSql.Append(" where ShopId in (" + ids + ")  ");
            SqlParameter[] parameters = {
					new SqlParameter("@Status", SqlDbType.Int,4)			};
            parameters[0].Value = state;

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

        public bool UpdateRecomendList(string ids, int Recomend)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Tao_Shop  set Recomend=@Recomend");
            strSql.Append(" where ShopId in (" + ids + ")  ");
            SqlParameter[] parameters = {
					new SqlParameter("@Recomend", SqlDbType.Int,4)			};
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
		#endregion  ExtensionMethod
	}
}

