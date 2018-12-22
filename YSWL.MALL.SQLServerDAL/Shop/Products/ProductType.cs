/*----------------------------------------------------------------
// Copyright (C) 2012 云商未来 版权所有。
//
// 文件名：ProductTypes.cs
// 文件功能描述：
// 
// 创建标识： [Ben]  2012/06/11 20:36:30
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.MALL.IDAL.Shop.Products;
using YSWL.DBUtility;
using System.Collections.Generic;
namespace YSWL.MALL.SQLServerDAL.Shop.Products
{
	/// <summary>
	/// 数据访问类:ProductType
	/// </summary>
	public partial class ProductType:IProductType
	{
		public ProductType()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DBHelper.DefaultDBHelper.GetMaxID("TypeId", "PMS_ProductTypes"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int TypeId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT COUNT(1) FROM PMS_ProductTypes");
			strSql.Append(" WHERE TypeId=@TypeId");
			SqlParameter[] parameters = {
					new SqlParameter("@TypeId", SqlDbType.Int,4)
			};
			parameters[0].Value = TypeId;

			return DBHelper.DefaultDBHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(YSWL.MALL.Model.Shop.Products.ProductType model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("INSERT INTO PMS_ProductTypes(");
			strSql.Append("TypeName,Remark)");
			strSql.Append(" VALUES (");
			strSql.Append("@TypeName,@Remark)");
			strSql.Append(";SELECT @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@TypeName", SqlDbType.NVarChar,50),
					new SqlParameter("@Remark", SqlDbType.NVarChar,200)};
			parameters[0].Value = model.TypeName;
			parameters[1].Value = model.Remark;

			object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(),parameters);
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
		public bool Update(YSWL.MALL.Model.Shop.Products.ProductType model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("UPDATE PMS_ProductTypes SET ");
			strSql.Append("TypeName=@TypeName,");
			strSql.Append("Remark=@Remark");
			strSql.Append(" WHERE TypeId=@TypeId");
			SqlParameter[] parameters = {
					new SqlParameter("@TypeName", SqlDbType.NVarChar,50),
					new SqlParameter("@Remark", SqlDbType.NVarChar,200),
					new SqlParameter("@TypeId", SqlDbType.Int,4)};
			parameters[0].Value = model.TypeName;
			parameters[1].Value = model.Remark;
			parameters[2].Value = model.TypeId;

			int rows=DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(),parameters);
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
		public bool Delete(int TypeId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("DELETE FROM PMS_ProductTypes ");
			strSql.Append(" WHERE TypeId=@TypeId");
			SqlParameter[] parameters = {
					new SqlParameter("@TypeId", SqlDbType.Int,4)
			};
			parameters[0].Value = TypeId;

			int rows=DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(),parameters);
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
		public bool DeleteList(string TypeIdlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("DELETE FROM PMS_ProductTypes ");
			strSql.Append(" WHERE TypeId in ("+TypeIdlist + ")  ");
			int rows=DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString());
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
		public YSWL.MALL.Model.Shop.Products.ProductType GetModel(int TypeId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT  TOP 1 TypeId,TypeName,Remark FROM PMS_ProductTypes ");
			strSql.Append(" WHERE TypeId=@TypeId");
			SqlParameter[] parameters = {
					new SqlParameter("@TypeId", SqlDbType.Int,4)
			};
			parameters[0].Value = TypeId;

			YSWL.MALL.Model.Shop.Products.ProductType model=new YSWL.MALL.Model.Shop.Products.ProductType();
			DataSet ds=DBHelper.DefaultDBHelper.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["TypeId"]!=null && ds.Tables[0].Rows[0]["TypeId"].ToString()!="")
				{
					model.TypeId=int.Parse(ds.Tables[0].Rows[0]["TypeId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["TypeName"]!=null && ds.Tables[0].Rows[0]["TypeName"].ToString()!="")
				{
					model.TypeName=ds.Tables[0].Rows[0]["TypeName"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Remark"]!=null && ds.Tables[0].Rows[0]["Remark"].ToString()!="")
				{
					model.Remark=ds.Tables[0].Rows[0]["Remark"].ToString();
				}
				return model;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT TypeId,TypeName,Remark ");
			strSql.Append(" FROM PMS_ProductTypes ");
			if(!string.IsNullOrWhiteSpace(strWhere.Trim()))
			{
				strSql.Append(" WHERE "+strWhere);
			}
			return DBHelper.DefaultDBHelper.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT ");
			if(Top>0)
			{
				strSql.Append(" TOP "+Top.ToString());
			}
			strSql.Append(" TypeId,TypeName,Remark ");
			strSql.Append(" FROM PMS_ProductTypes ");
			if(!string.IsNullOrWhiteSpace(strWhere.Trim()))
			{
				strSql.Append(" WHERE "+strWhere);
			}
			strSql.Append(" ORDER BY " + filedOrder);
			return DBHelper.DefaultDBHelper.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT COUNT(1) FROM PMS_ProductTypes ");
			if(!string.IsNullOrWhiteSpace(strWhere.Trim()))
			{
				strSql.Append(" WHERE "+strWhere);
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
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT * FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrWhiteSpace(orderby.Trim()))
			{
				strSql.Append("ORDER BY T." + orderby );
			}
			else
			{
				strSql.Append("ORDER BY T.TypeId desc");
			}
			strSql.Append(")AS Row, T.*  FROM PMS_ProductTypes T ");
			if (!string.IsNullOrWhiteSpace(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row BETWEEN {0} AND {1}", startIndex, endIndex);
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
			parameters[0].Value = "PMS_ProductTypes";
			parameters[1].Value = "TypeId";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DBHelper.DefaultDBHelper.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  Method

        #region NewMethod
        public List<YSWL.MALL.Model.Shop.Products.ProductType> GetProductTypes()
        {
            List<YSWL.MALL.Model.Shop.Products.ProductType> list = new List<YSWL.MALL.Model.Shop.Products.ProductType>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM PMS_ProductTypes");
            DataSet ds = DBHelper.DefaultDBHelper.Query(strSql.ToString());
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    YSWL.MALL.Model.Shop.Products.ProductType model = new YSWL.MALL.Model.Shop.Products.ProductType();
                    LoadEntityData(ref model, dr);
                    list.Add(model);
                }
            }
            return list;
        }

        #region 将行数据 转成 实体对象
        /// <summary>
        /// 将行数据 转成 实体对象
        /// </summary>
        /// <param name="model">Entity</param>
        /// <param name="dr">DataRow</param>
        private void LoadEntityData(ref YSWL.MALL.Model.Shop.Products.ProductType model, DataRow dr)
        {
            if (dr["TypeId"] != null && dr["TypeId"].ToString() != "")
            {
                model.TypeId = int.Parse(dr["TypeId"].ToString());
            }
            if (dr["TypeName"] != null && dr["TypeName"].ToString() != "")
            {
                model.TypeName = dr["TypeName"].ToString();
            }
            if (dr["Remark"] != null && dr["Remark"].ToString() != "")
            {
                model.Remark = dr["Remark"].ToString();
            }
        }
        #endregion 

        public bool ProductTypeManage(Model.Shop.Products.ProductType model,Model.Shop.Products.DataProviderAction Action,out int Typeid)
        {
            int rows = 0;
            SqlParameter[] param ={
                                 new SqlParameter("@TypeId",SqlDbType.Int),
                                 new SqlParameter("@TypeName",SqlDbType.NVarChar),
                                 new SqlParameter("@Remark",SqlDbType.NVarChar),
                                 new SqlParameter("@Action",SqlDbType.Int),
                                 new SqlParameter("@TypeIdOut",SqlDbType.Int)
                                 };
            param[0].Value = model.TypeId;
            param[1].Value = model.TypeName;
            param[2].Value = model.Remark;
            param[3].Value = (int)Action;
            param[4].Direction = ParameterDirection.Output;
            DBHelper.DefaultDBHelper.RunProcedure("sp_Show_PMS_ProductTypesCreateUpdateDelete", param, out rows);
            int typeId = 0;
            if (Action == Model.Shop.Products.DataProviderAction.Create)
            {
                typeId = Convert.ToInt32(param[4].Value);
            }
            else
            {
                typeId = model.TypeId;
            }
            if (rows > 0 && typeId > 0)
            {
                ProductTypeBrand productTypeBrands = new ProductTypeBrand();
                if (Action == Model.Shop.Products.DataProviderAction.Update)
                {
                //TODO: 级联删除/更新 没考虑
                    productTypeBrands.Delete(typeId, null);
                }
                if (model.BrandsTypes != null)
                {
                    foreach (int bid in model.BrandsTypes)
                    {
                        productTypeBrands.Add(typeId, bid);
                    }
                }
                Typeid = typeId;
                return true;
            }
            else
            {
                Typeid = 0;
                return false;
            }
        }


        public bool DeleteManage(int? TypeId,long? AttributeId,long? ValueId)
        {
            int rowsAffected=0;
            SqlParameter[] parameter = { 
                                       new SqlParameter("@TypeId",SqlDbType.Int),
                                       new SqlParameter("@AttributeId",SqlDbType.BigInt),
                                       new SqlParameter("@ValueId",SqlDbType.BigInt)
                                       };
            parameter[0].Value = TypeId;
            parameter[1].Value = AttributeId;
            parameter[2].Value = ValueId;

            DBHelper.DefaultDBHelper.RunProcedure("sp_Shop_DeleteManage", parameter, out rowsAffected);
            return rowsAffected > 0;
        }

        public bool SwapSeqManage(int? TypeId, long? AttributeId, long? ValueId, Model.Shop.Products.SwapSequenceIndex zIndex, bool UsageMode)
        {
            int rowsAffected = 0;
            SqlParameter[] parameter = { 
                                       new SqlParameter("@TypeId",SqlDbType.Int),
                                       new SqlParameter("@AttributeId",SqlDbType.BigInt),
                                       new SqlParameter("@ValueId",SqlDbType.BigInt),
                                       new SqlParameter("@ZIndex",SqlDbType.Int),
                                       new SqlParameter("@UsageMode",SqlDbType.Bit)
                                       };
            parameter[0].Value = TypeId;
            parameter[1].Value = AttributeId;
            parameter[2].Value = ValueId;
            parameter[3].Value = (int)zIndex;
            parameter[4].Value = UsageMode;

            DBHelper.DefaultDBHelper.RunProcedure("sp_Shop_SwapManage", parameter, out rowsAffected);
            return rowsAffected > 0;
        }


        /// <summary>
        /// 重置表
        /// </summary>
        /// <returns></returns>
        public bool ResetTable()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("TRUNCATE TABLE PMS_ProductTypeBrands ");
            strSql.Append(" TRUNCATE TABLE PMS_ProductTypes  ");

            return DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString()) > 0;
        }

        public bool ProductTypeManage(Model.Shop.Products.ProductType model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("  SET IDENTITY_INSERT [PMS_ProductTypes] ON ");
            strSql.Append("INSERT INTO PMS_ProductTypes(");
            strSql.Append("TypeId,TypeName,Remark)");
            strSql.Append(" VALUES (");
            strSql.Append("@TypeId,@TypeName,@Remark)");
            strSql.Append(" ; SET IDENTITY_INSERT [PMS_ProductTypes] OFF  ");
            SqlParameter[] parameters = {
                    new SqlParameter("@TypeName", SqlDbType.NVarChar,50),
                    new SqlParameter("@Remark", SqlDbType.NVarChar,200),
                    new SqlParameter("@TypeId", SqlDbType.Int,4)
            };
            parameters[0].Value = model.TypeName;
            parameters[1].Value = model.Remark;
            parameters[2].Value = model.TypeId;

            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                ProductTypeBrand productTypeBrands = new ProductTypeBrand();
                foreach (int bid in model.BrandsTypes)
                {
                    productTypeBrands.AddEx(model.TypeId, bid);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

    }
}

