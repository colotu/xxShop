/**
* ShippingAddress.cs
*
* 功 能： N/A
* 类 名： ShippingAddress
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/4/27 10:24:44   N/A    初版
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
using YSWL.MALL.IDAL.Shop.Shipping;
using YSWL.DBUtility;//Please add references
using System.Collections.Generic;

namespace YSWL.MALL.SQLServerDAL.Shop.Shipping
{
	/// <summary>
	/// 数据访问类:ShippingAddress
	/// </summary>
	public partial class ShippingAddress:IShippingAddress
	{
		public ShippingAddress()
		{}
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DBHelper.DefaultDBHelper.GetMaxID("ShippingId", "Shop_ShippingAddress");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ShippingId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Shop_ShippingAddress");
            strSql.Append(" where ShippingId=@ShippingId");
            SqlParameter[] parameters = {
					new SqlParameter("@ShippingId", SqlDbType.Int,4)
			};
            parameters[0].Value = ShippingId;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.MALL.Model.Shop.Shipping.ShippingAddress model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Shop_ShippingAddress(");
            strSql.Append("RegionId,UserId,ShipName,Address,Zipcode,EmailAddress,TelPhone,CelPhone,IsDefault,Aliases,Latitude,Longitude,LineId,CircleId,DepotId,Remark)");
            strSql.Append(" values (");
            strSql.Append("@RegionId,@UserId,@ShipName,@Address,@Zipcode,@EmailAddress,@TelPhone,@CelPhone,@IsDefault,@Aliases,@Latitude,@Longitude,@LineId,@CircleId,@DepotId,@Remark)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@RegionId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@ShipName", SqlDbType.NVarChar,50),
					new SqlParameter("@Address", SqlDbType.NVarChar,300),
					new SqlParameter("@Zipcode", SqlDbType.NVarChar,20),
					new SqlParameter("@EmailAddress", SqlDbType.NVarChar,100),
					new SqlParameter("@TelPhone", SqlDbType.NVarChar,50),
					new SqlParameter("@CelPhone", SqlDbType.NVarChar,50),
					new SqlParameter("@IsDefault", SqlDbType.Bit,1),
					new SqlParameter("@Aliases", SqlDbType.NVarChar,100),
					new SqlParameter("@Latitude", SqlDbType.Decimal,9),
					new SqlParameter("@Longitude", SqlDbType.Decimal,9),
					new SqlParameter("@LineId", SqlDbType.Int,4),
					new SqlParameter("@CircleId", SqlDbType.Int,4),
					new SqlParameter("@DepotId", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.NChar,1000)};
            parameters[0].Value = model.RegionId;
            parameters[1].Value = model.UserId;
            parameters[2].Value = model.ShipName;
            parameters[3].Value = model.Address;
            parameters[4].Value = model.Zipcode;
            parameters[5].Value = model.EmailAddress;
            parameters[6].Value = model.TelPhone;
            parameters[7].Value = model.CelPhone;
            parameters[8].Value = model.IsDefault;
            parameters[9].Value = model.Aliases;
            parameters[10].Value = model.Latitude;
            parameters[11].Value = model.Longitude;
            parameters[12].Value = model.LineId;
            parameters[13].Value = model.CircleId;
            parameters[14].Value = model.DepotId;
            parameters[15].Value = model.Remark;

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
        public bool Update(YSWL.MALL.Model.Shop.Shipping.ShippingAddress model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Shop_ShippingAddress set ");
            strSql.Append("RegionId=@RegionId,");
            strSql.Append("UserId=@UserId,");
            strSql.Append("ShipName=@ShipName,");
            strSql.Append("Address=@Address,");
            strSql.Append("Zipcode=@Zipcode,");
            strSql.Append("EmailAddress=@EmailAddress,");
            strSql.Append("TelPhone=@TelPhone,");
            strSql.Append("CelPhone=@CelPhone,");
            strSql.Append("IsDefault=@IsDefault,");
            strSql.Append("Aliases=@Aliases,");
            strSql.Append("Latitude=@Latitude,");
            strSql.Append("Longitude=@Longitude,");
            strSql.Append("LineId=@LineId,");
            strSql.Append("CircleId=@CircleId,");
            strSql.Append("DepotId=@DepotId,");
            strSql.Append("Remark=@Remark");
            strSql.Append(" where ShippingId=@ShippingId");
            SqlParameter[] parameters = {
					new SqlParameter("@RegionId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@ShipName", SqlDbType.NVarChar,50),
					new SqlParameter("@Address", SqlDbType.NVarChar,300),
					new SqlParameter("@Zipcode", SqlDbType.NVarChar,20),
					new SqlParameter("@EmailAddress", SqlDbType.NVarChar,100),
					new SqlParameter("@TelPhone", SqlDbType.NVarChar,50),
					new SqlParameter("@CelPhone", SqlDbType.NVarChar,50),
					new SqlParameter("@IsDefault", SqlDbType.Bit,1),
					new SqlParameter("@Aliases", SqlDbType.NVarChar,100),
					new SqlParameter("@Latitude", SqlDbType.Decimal,9),
					new SqlParameter("@Longitude", SqlDbType.Decimal,9),
					new SqlParameter("@LineId", SqlDbType.Int,4),
					new SqlParameter("@CircleId", SqlDbType.Int,4),
					new SqlParameter("@DepotId", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.NChar,1000),
					new SqlParameter("@ShippingId", SqlDbType.Int,4)};
            parameters[0].Value = model.RegionId;
            parameters[1].Value = model.UserId;
            parameters[2].Value = model.ShipName;
            parameters[3].Value = model.Address;
            parameters[4].Value = model.Zipcode;
            parameters[5].Value = model.EmailAddress;
            parameters[6].Value = model.TelPhone;
            parameters[7].Value = model.CelPhone;
            parameters[8].Value = model.IsDefault;
            parameters[9].Value = model.Aliases;
            parameters[10].Value = model.Latitude;
            parameters[11].Value = model.Longitude;
            parameters[12].Value = model.LineId;
            parameters[13].Value = model.CircleId;
            parameters[14].Value = model.DepotId;
            parameters[15].Value = model.Remark;
            parameters[16].Value = model.ShippingId;

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
        public bool Delete(int ShippingId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Shop_ShippingAddress ");
            strSql.Append(" where ShippingId=@ShippingId");
            SqlParameter[] parameters = {
					new SqlParameter("@ShippingId", SqlDbType.Int,4)
			};
            parameters[0].Value = ShippingId;

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
        public bool DeleteList(string ShippingIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Shop_ShippingAddress ");
            strSql.Append(" where ShippingId in (" + ShippingIdlist + ")  ");
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
        public YSWL.MALL.Model.Shop.Shipping.ShippingAddress GetModel(int ShippingId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ShippingId,RegionId,UserId,ShipName,Address,Zipcode,EmailAddress,TelPhone,CelPhone,IsDefault,Aliases,Latitude,Longitude,LineId,CircleId,DepotId,Remark from Shop_ShippingAddress ");
            strSql.Append(" where ShippingId=@ShippingId");
            SqlParameter[] parameters = {
					new SqlParameter("@ShippingId", SqlDbType.Int,4)
			};
            parameters[0].Value = ShippingId;

            YSWL.MALL.Model.Shop.Shipping.ShippingAddress model = new YSWL.MALL.Model.Shop.Shipping.ShippingAddress();
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
        public YSWL.MALL.Model.Shop.Shipping.ShippingAddress DataRowToModel(DataRow row)
        {
            YSWL.MALL.Model.Shop.Shipping.ShippingAddress model = new YSWL.MALL.Model.Shop.Shipping.ShippingAddress();
            if (row != null)
            {
                if (row["ShippingId"] != null && row["ShippingId"].ToString() != "")
                {
                    model.ShippingId = int.Parse(row["ShippingId"].ToString());
                }
                if (row["RegionId"] != null && row["RegionId"].ToString() != "")
                {
                    model.RegionId = int.Parse(row["RegionId"].ToString());
                }
                if (row["UserId"] != null && row["UserId"].ToString() != "")
                {
                    model.UserId = int.Parse(row["UserId"].ToString());
                }
                if (row["ShipName"] != null)
                {
                    model.ShipName = row["ShipName"].ToString();
                }
                if (row["Address"] != null)
                {
                    model.Address = row["Address"].ToString();
                }
                if (row["Zipcode"] != null)
                {
                    model.Zipcode = row["Zipcode"].ToString();
                }
                if (row["EmailAddress"] != null)
                {
                    model.EmailAddress = row["EmailAddress"].ToString();
                }
                if (row["TelPhone"] != null)
                {
                    model.TelPhone = row["TelPhone"].ToString();
                }
                if (row["CelPhone"] != null)
                {
                    model.CelPhone = row["CelPhone"].ToString();
                }
                if (row["IsDefault"] != null && row["IsDefault"].ToString() != "")
                {
                    if ((row["IsDefault"].ToString() == "1") || (row["IsDefault"].ToString().ToLower() == "true"))
                    {
                        model.IsDefault = true;
                    }
                    else
                    {
                        model.IsDefault = false;
                    }
                }
                if (row["Aliases"] != null)
                {
                    model.Aliases = row["Aliases"].ToString();
                }
                if (row["Latitude"] != null && row["Latitude"].ToString() != "")
                {
                    model.Latitude = decimal.Parse(row["Latitude"].ToString());
                }
                if (row["Longitude"] != null && row["Longitude"].ToString() != "")
                {
                    model.Longitude = decimal.Parse(row["Longitude"].ToString());
                }
                if (row["LineId"] != null && row["LineId"].ToString() != "")
                {
                    model.LineId = int.Parse(row["LineId"].ToString());
                }
                if (row["CircleId"] != null && row["CircleId"].ToString() != "")
                {
                    model.CircleId = int.Parse(row["CircleId"].ToString());
                }
                if (row["DepotId"] != null && row["DepotId"].ToString() != "")
                {
                    model.DepotId = int.Parse(row["DepotId"].ToString());
                }
                if (row["Remark"] != null)
                {
                    model.Remark = row["Remark"].ToString();
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
            strSql.Append("select ShippingId,RegionId,UserId,ShipName,Address,Zipcode,EmailAddress,TelPhone,CelPhone,IsDefault,Aliases,Latitude,Longitude,LineId,CircleId,DepotId,Remark ");
            strSql.Append(" FROM Shop_ShippingAddress ");
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
            strSql.Append(" ShippingId,RegionId,UserId,ShipName,Address,Zipcode,EmailAddress,TelPhone,CelPhone,IsDefault,Aliases,Latitude,Longitude,LineId,CircleId,DepotId,Remark ");
            strSql.Append(" FROM Shop_ShippingAddress ");
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
            strSql.Append("select count(1) FROM Shop_ShippingAddress ");
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
                strSql.Append("order by T.ShippingId desc");
            }
            strSql.Append(")AS Row, T.*  from Shop_ShippingAddress T ");
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
            parameters[0].Value = "Shop_ShippingAddress";
            parameters[1].Value = "ShippingId";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DBHelper.DefaultDBHelper.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  BasicMethod
		#region  ExtensionMethod
        public DataSet GetAddressBySupplier(int supplierId)
        {
            string strWhere =
                "select * from Shop_ShippingAddress s where exists( select * from ERP_Lines e where s.LineId=e.LineId and e.supplierId=@supplierId )";
            SqlParameter parameters = new SqlParameter("@supplierId", SqlDbType.Int, 4);
            parameters.Value = supplierId;
            return DBHelper.DefaultDBHelper.Query(strWhere, parameters);
        }

        /// <summary>
        /// 设置默认收货地址
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ShippingId"></param>
        /// <returns></returns>
        public bool SetDefaultShipAddress(int UserId,int ShippingId)
        {     
            List<CommandInfo> sqllist = new List<CommandInfo>();
            //取消之前的默认收获地址
            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append("update Shop_ShippingAddress ");
            strSql1.AppendFormat(" set IsDefault = 0 where UserId = {0} and IsDefault = 1", UserId);
            CommandInfo cmd = new CommandInfo(strSql1.ToString(), null);
            sqllist.Add(cmd);

            //设置默认收货地址
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append(" update Shop_ShippingAddress  ");
            strSql2.AppendFormat(" set IsDefault = 1 where UserId = {0} and ShippingId = {1}",UserId,ShippingId);
            cmd = new CommandInfo(strSql2.ToString(), null);
            sqllist.Add(cmd);

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
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.Shipping.ShippingAddress GetModelByUserId(int userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ShippingId,RegionId,UserId,ShipName,Address,Zipcode,EmailAddress,TelPhone,CelPhone,IsDefault,Aliases,Latitude,Longitude,LineId,CircleId,DepotId,Remark from Shop_ShippingAddress ");
            strSql.Append(" where UserId=@UserId");
            strSql.Append("  order by   IsDefault desc   ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserId", SqlDbType.Int,4)
            };
            parameters[0].Value = userId;

            YSWL.MALL.Model.Shop.Shipping.ShippingAddress model = new YSWL.MALL.Model.Shop.Shipping.ShippingAddress();
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

        public bool UpdateMapInfo(int userId, decimal latitude, decimal longitude, string address)
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Shop_ShippingAddress set ");
            strSql.Append("Latitude=@Latitude,");
            strSql.Append("Longitude=@Longitude,");
            strSql.Append("Address=@Address");
            strSql.Append(" where UserId=@UserId");
            SqlParameter[] parameters = {
                    new SqlParameter("@Latitude", SqlDbType.Decimal,9),
                    new SqlParameter("@Longitude", SqlDbType.Decimal,9),
                    new SqlParameter("@Address", SqlDbType.NVarChar,300),
                    new SqlParameter("@UserId", SqlDbType.Int,4)};
            parameters[0].Value = latitude;
            parameters[1].Value = longitude;
            parameters[2].Value = address;
            parameters[3].Value = userId;

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
        /// 是否存在默认收货地址
        /// </summary>
        public bool ExistsDefaultAddress(int userId) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Shop_ShippingAddress");
            strSql.Append(" where UserId=@UserId and IsDefault=1 ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserId", SqlDbType.Int,4)
            };
            parameters[0].Value = userId;
            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }

	    #endregion  ExtensionMethod
    }
}