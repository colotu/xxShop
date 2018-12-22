﻿/*----------------------------------------------------------------
// Copyright (C) 2012 云商未来 版权所有。
//
// 文件名：Attributes.cs
// 文件功能描述：
// 
// 创建标识： [Ben]  2012/06/11 20:36:22
// 修改标识： [Rock]  2012年6月14日 17:08:19
// 修改描述： 新增  AttributeManage 放法
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Data.SqlClient;
using YSWL.MALL.IDAL.Shop.Products;
using YSWL.DBUtility;
using YSWL.MALL.Model.Shop.Products;


namespace YSWL.MALL.SQLServerDAL.Shop.Products
{
    /// <summary>
    /// 数据访问类:AttributeInfo
    /// </summary>
    public partial class AttributeInfo : IAttributeInfo
    {
        public AttributeInfo()
        { }
        #region  Method

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long AttributeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(1) FROM PMS_Attributes");
            strSql.Append(" WHERE AttributeId=@AttributeId");
            SqlParameter[] parameters = {
                    new SqlParameter("@AttributeId", SqlDbType.BigInt)
            };
            parameters[0].Value = AttributeId;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public long Add(YSWL.MALL.Model.Shop.Products.AttributeInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO PMS_Attributes(");
            strSql.Append("AttributeName,DisplaySequence,TypeId,UsageMode,UseAttributeImage)");
            strSql.Append(" VALUES (");
            strSql.Append("@AttributeName,@DisplaySequence,@TypeId,@UsageMode,@UseAttributeImage)");
            strSql.Append(";SELECT @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@AttributeName", SqlDbType.NVarChar,50),
                    new SqlParameter("@DisplaySequence", SqlDbType.Int,4),
                    new SqlParameter("@TypeId", SqlDbType.Int,4),
                    new SqlParameter("@UsageMode", SqlDbType.Int,4),
                    new SqlParameter("@UseAttributeImage", SqlDbType.Bit,1)};
            parameters[0].Value = model.AttributeName;
            parameters[1].Value = model.DisplaySequence;
            parameters[2].Value = model.TypeId;
            parameters[3].Value = model.UsageMode;
            parameters[4].Value = model.UseAttributeImage;

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
        public bool Update(YSWL.MALL.Model.Shop.Products.AttributeInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE PMS_Attributes SET ");
            strSql.Append("AttributeName=@AttributeName,");
            strSql.Append("DisplaySequence=@DisplaySequence,");
            strSql.Append("TypeId=@TypeId,");
            strSql.Append("UsageMode=@UsageMode,");
            strSql.Append("UseAttributeImage=@UseAttributeImage");
            strSql.Append(" WHERE AttributeId=@AttributeId");
            SqlParameter[] parameters = {
                    new SqlParameter("@AttributeName", SqlDbType.NVarChar,50),
                    new SqlParameter("@DisplaySequence", SqlDbType.Int,4),
                    new SqlParameter("@TypeId", SqlDbType.Int,4),
                    new SqlParameter("@UsageMode", SqlDbType.Int,4),
                    new SqlParameter("@UseAttributeImage", SqlDbType.Bit,1),
                    new SqlParameter("@AttributeId", SqlDbType.BigInt,8)};
            parameters[0].Value = model.AttributeName;
            parameters[1].Value = model.DisplaySequence;
            parameters[2].Value = model.TypeId;
            parameters[3].Value = model.UsageMode;
            parameters[4].Value = model.UseAttributeImage;
            parameters[5].Value = model.AttributeId;

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
        public bool Delete(long AttributeId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE FROM PMS_Attributes ");
            strSql.Append(" WHERE AttributeId=@AttributeId");
            SqlParameter[] parameters = {
                    new SqlParameter("@AttributeId", SqlDbType.BigInt)
            };
            parameters[0].Value = AttributeId;

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
        public bool DeleteList(string AttributeIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE FROM PMS_Attributes ");
            strSql.Append(" WHERE AttributeId in (" + AttributeIdlist + ")  ");
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
        public YSWL.MALL.Model.Shop.Products.AttributeInfo GetModel(long AttributeId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  TOP 1 AttributeId,AttributeName,DisplaySequence,TypeId,UsageMode,UseAttributeImage FROM PMS_Attributes ");
            strSql.Append(" WHERE AttributeId=@AttributeId");
            SqlParameter[] parameters = {
                    new SqlParameter("@AttributeId", SqlDbType.BigInt)
            };
            parameters[0].Value = AttributeId;

            YSWL.MALL.Model.Shop.Products.AttributeInfo model = new YSWL.MALL.Model.Shop.Products.AttributeInfo();
            DataSet ds = DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["AttributeId"] != null && ds.Tables[0].Rows[0]["AttributeId"].ToString() != "")
                {
                    model.AttributeId = long.Parse(ds.Tables[0].Rows[0]["AttributeId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["AttributeName"] != null && ds.Tables[0].Rows[0]["AttributeName"].ToString() != "")
                {
                    model.AttributeName = ds.Tables[0].Rows[0]["AttributeName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["DisplaySequence"] != null && ds.Tables[0].Rows[0]["DisplaySequence"].ToString() != "")
                {
                    model.DisplaySequence = int.Parse(ds.Tables[0].Rows[0]["DisplaySequence"].ToString());
                }
                if (ds.Tables[0].Rows[0]["TypeId"] != null && ds.Tables[0].Rows[0]["TypeId"].ToString() != "")
                {
                    model.TypeId = int.Parse(ds.Tables[0].Rows[0]["TypeId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["UsageMode"] != null && ds.Tables[0].Rows[0]["UsageMode"].ToString() != "")
                {
                    model.UsageMode = int.Parse(ds.Tables[0].Rows[0]["UsageMode"].ToString());
                }
                if (ds.Tables[0].Rows[0]["UseAttributeImage"] != null && ds.Tables[0].Rows[0]["UseAttributeImage"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["UseAttributeImage"].ToString() == "1") || (ds.Tables[0].Rows[0]["UseAttributeImage"].ToString().ToLower() == "true"))
                    {
                        model.UseAttributeImage = true;
                    }
                    else
                    {
                        model.UseAttributeImage = false;
                    }
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT AttributeId,AttributeName,DisplaySequence,TypeId,UsageMode,UseAttributeImage ");
            strSql.Append(" FROM PMS_Attributes ");
            if (!string.IsNullOrWhiteSpace(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT ");
            if (Top > 0)
            {
                strSql.Append(" TOP " + Top.ToString());
            }
            strSql.Append(" AttributeId,AttributeName,DisplaySequence,TypeId,UsageMode,UseAttributeImage ");
            strSql.Append(" FROM PMS_Attributes ");
            if (!string.IsNullOrWhiteSpace(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ORDER BY " + filedOrder);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(1) FROM PMS_Attributes ");
            if (!string.IsNullOrWhiteSpace(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
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
            if (!string.IsNullOrWhiteSpace(orderby.Trim()))
            {
                strSql.Append("ORDER BY T." + orderby);
            }
            else
            {
                strSql.Append("ORDER BY T.AttributeId desc");
            }
            strSql.Append(")AS Row, T.*  FROM PMS_Attributes T ");
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
            parameters[0].Value = "PMS_Attributes";
            parameters[1].Value = "AttributeId";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DBHelper.DefaultDBHelper.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  Method

        public bool AttributeManage(Model.Shop.Products.AttributeInfo model, Model.Shop.Products.DataProviderAction Action)
        {
            int rows = 0;
            SqlParameter[] param = { 
                                   new SqlParameter("@Action",SqlDbType.Int),
                                   new SqlParameter("@AttributeId",SqlDbType.BigInt),
                                   new SqlParameter("@AttributeName",SqlDbType.NVarChar),
                                   new SqlParameter("@TypeId",SqlDbType.Int),
                                   new SqlParameter("@UsageMode",SqlDbType.Int),
                                   new SqlParameter("@UseAttributeImage",SqlDbType.Bit),
                                   new SqlParameter("@UserDefinedPic",SqlDbType.Bit),
                                   new SqlParameter("@AttributeIdOutPut",SqlDbType.BigInt)
                                   };
            param[0].Value = (int)Action;
            param[1].Value = model.AttributeId;
            param[2].Value = model.AttributeName;
            param[3].Value = model.TypeId;
            param[4].Value = model.UsageMode;
            param[5].Value = model.UseAttributeImage;
            param[6].Value = model.UserDefinedPic;
            param[7].Direction = ParameterDirection.Output;
            DBHelper.DefaultDBHelper.RunProcedure("sp_PMS_AttributesCreateEditDelete", param, out rows);
            long attId = 0;
            if (Action == Model.Shop.Products.DataProviderAction.Create)
            {
                attId = Convert.ToInt64(param[7].Value);
            }
            else
            {
                attId = model.AttributeId;
            }
            if (rows > 0)
            {
                AttributeValue attInfo = new AttributeValue();
                if (Action == Model.Shop.Products.DataProviderAction.Update)
                {
                    attInfo.Delete(attId);
                }
                if (model.ValueStr != null)
                {
                    foreach (string varStr in model.ValueStr)
                    {
                        Model.Shop.Products.AttributeValue attModel = new Model.Shop.Products.AttributeValue();
                        attModel.AttributeId = attId;
                        attModel.ValueStr = varStr;
                        attInfo.AttributeValueManage(attModel, Model.Shop.Products.DataProviderAction.Create);
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AttributePMSManage(Model.Shop.Products.AttributeInfo model, Model.Shop.Products.DataProviderAction Action)
        {
            int rows = 0;
            SqlParameter[] param = {
                                   new SqlParameter("@Action",SqlDbType.Int),
                                   new SqlParameter("@AttributeId",SqlDbType.BigInt),
                                   new SqlParameter("@AttributeName",SqlDbType.NVarChar),
                                   new SqlParameter("@TypeId",SqlDbType.Int),
                                   new SqlParameter("@UsageMode",SqlDbType.Int),
                                   new SqlParameter("@UseAttributeImage",SqlDbType.Bit),
                                   new SqlParameter("@UserDefinedPic",SqlDbType.Bit),
                                   new SqlParameter("@AttributeIdOutPut",SqlDbType.BigInt)
                                   };
            param[0].Value = (int)Action;
            param[1].Value = model.AttributeId;
            param[2].Value = model.AttributeName;
            param[3].Value = model.TypeId;
            param[4].Value = model.UsageMode;
            param[5].Value = model.UseAttributeImage;
            param[6].Value = model.UserDefinedPic;
            param[7].Direction = ParameterDirection.Output;
            DBHelper.DefaultDBHelper.RunProcedure("sp_PMS_AttributesCreateEditDelete", param, out rows);
            long attId = 0;
            if (Action == Model.Shop.Products.DataProviderAction.Create)
            {
                attId = Convert.ToInt64(param[7].Value);
            }
            else
            {
                attId = model.AttributeId;
            }
         
            if (rows > 0)
            {
                AttributeValue attInfo = new AttributeValue();
                //if (Action == Model.Shop.Products.DataProviderAction.Update)
                //{
                //    attInfo.Delete(attId);
                //}
                if (model.AttributeValues != null)
                    {
                        foreach (var item in model.AttributeValues)
                        {
                            item.AttributeId = attId;
                            attInfo.SyncValues(item);
                        }
                    }
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(long? Typeid, Model.Shop.Products.SearchType searchType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT AttributeId,AttributeName,DisplaySequence,TypeId,UsageMode,UseAttributeImage ");
            strSql.Append(" FROM PMS_Attributes ");
            StringBuilder strWhere = new StringBuilder();

            if (searchType == Model.Shop.Products.SearchType.ExtAttribute)
            {
                if (!String.IsNullOrWhiteSpace(strWhere.ToString()))
                {
                    strWhere.Append(" AND ");
                }
                strWhere.Append("   UsageMode  <>3 ");
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(strWhere.ToString()))
                {
                    strWhere.Append(" AND ");
                }
                strWhere.Append("   UsageMode =3 ");
            }

            if (Typeid.HasValue)
            {
                if (!String.IsNullOrWhiteSpace(strWhere.ToString()))
                {
                    strWhere.Append(" AND ");
                }
                strWhere.Append("  TypeId=@TypeId");
                strSql.Append(" WHERE  " + strWhere.ToString());
                strSql.Append(" ORDER BY DisplaySequence ");
                SqlParameter[] param = { 
                                       new SqlParameter("@TypeId",SqlDbType.BigInt)
                                       };
                param[0].Value = Typeid.Value;
                return DBHelper.DefaultDBHelper.Query(strSql.ToString(), param);
            }
            else
            {
                strSql.Append(" WHERE  " + strWhere.ToString());
                strSql.Append(" ORDER BY DisplaySequence ");
                return DBHelper.DefaultDBHelper.Query(strSql.ToString());
            }
        }

        public bool ChangeImageStatue(long AttributeId, Model.Shop.Products.ProductAttributeModel status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE PMS_Attributes ");
            strSql.Append("SET UsageMode=@Status ");
            strSql.Append("WHERE AttributeId=@AttributeId ");
            SqlParameter[] parameter = { 
                                       new SqlParameter("@AttributeId",SqlDbType.BigInt),
                                       new SqlParameter("@Status",SqlDbType.Int)
                                       };
            parameter[0].Value = AttributeId;
            parameter[1].Value = (int)status;
            int resultRows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameter);
            return resultRows > 0;
        }

        public List<Model.Shop.Products.AttributeInfo> GetAttributeInfoList(int? typeId, Model.Shop.Products.SearchType searchType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  A.AttributeId ,
                                A.AttributeName ,
                                A.DisplaySequence AS AttributeDisplaySequence ,
                                A.TypeId ,
                                A.UsageMode ,
                                A.UseAttributeImage ,
                                A.UserDefinedPic ,
                                V.ValueId,
                                V.DisplaySequence AS ValueDisplaySequence ,
                                V.ValueStr ,
                                V.ImageUrl
                            FROM PMS_Attributes A
                                LEFT JOIN PMS_AttributeValues V ON A.AttributeId = V.AttributeId
                            WHERE ");
            //Tip: 此处不排除自定义属性的值, 正常解析, 添加商品处已在json解析处过滤 避免添加商品时出现其他商品填写的自定义属性 BEN ADD 2012-07-01
            //LEFT JOIN ... AND ((A.UsageMode<>2) OR (A.UsageMode=2 AND V.DisplaySequence IS NULL)) //排除自定义属性值条件
            List<SqlParameter> param = new List<SqlParameter>();
            if (typeId.HasValue)
            {
                strSql.Append(" A.TypeId = @TypeId");
                param.Add(new SqlParameter("@TypeId", SqlDbType.Int));
                param[0].Value = typeId;
            }
            switch (searchType)
            {
                case SearchType.ExtAttribute:
                    if (typeId.HasValue)
                    {
                        strSql.Append(" AND ");
                    }
                    strSql.Append(" A.UsageMode <>3");
                    break;
                case SearchType.Specification:
                    if (typeId.HasValue)
                    {
                        strSql.Append(" AND ");
                    }
                    strSql.Append(" A.UsageMode = 3");
                    break;
                default:
                    break;
            }
            strSql.Append(" ORDER BY A.DisplaySequence,V.AttributeId,V.DisplaySequence");

            DataSet ds = param.Count > 0 ?
                DBHelper.DefaultDBHelper.Query(strSql.ToString(), param.ToArray()) :
                DBHelper.DefaultDBHelper.Query(strSql.ToString());
            if (ds == null || ds.Tables[0].Rows.Count < 0) return null;

            //Fill And Return List
            return FillAttributeInfos(ds);
        }

        private List<Model.Shop.Products.AttributeInfo> FillAttributeInfos(DataSet ds)
        {
            List<Model.Shop.Products.AttributeInfo> list = new List<Model.Shop.Products.AttributeInfo>();
            Model.Shop.Products.AttributeInfo attributeInfo;
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {
                //判断是否存在
                attributeInfo = list.Find(info =>
                                          info.AttributeId.ToString(CultureInfo.InvariantCulture) ==
                                          dataRow["AttributeId"].ToString()
                    );
                // 第一次
                if (attributeInfo == null)
                {
                    int usageMode = (int)dataRow["UsageMode"];

                    //非自填写属性 无AttributeValue情况 (*)保护 忽略此数据
                    if (usageMode != 2)
                    {
                        if (dataRow["ValueId"] == DBNull.Value) continue;
                    }

                    attributeInfo = new Model.Shop.Products.AttributeInfo();
                    attributeInfo.AttributeId = (long)dataRow["AttributeId"];
                    attributeInfo.AttributeName = dataRow["AttributeName"].ToString();

                    attributeInfo.DisplaySequence = (int)dataRow["AttributeDisplaySequence"];
                    attributeInfo.TypeId = (int)dataRow["TypeId"];
                    attributeInfo.UsageMode = usageMode;
                    attributeInfo.UseAttributeImage = (bool)dataRow["UseAttributeImage"];
                    attributeInfo.UserDefinedPic = (bool)dataRow["UserDefinedPic"];
                    //Add to List
                    list.Add(attributeInfo);
                }

                if (dataRow["ValueId"] == DBNull.Value) continue;
                // 追加/填充 AttributeValue
                attributeInfo.AttributeValues.Add(new Model.Shop.Products.AttributeValue
                {
                    ValueId = (long)dataRow["ValueId"],
                    AttributeId = (long)dataRow["AttributeId"],
                    DisplaySequence = (int)dataRow["ValueDisplaySequence"],
                    ValueStr =
                        dataRow["ValueStr"] != DBNull.Value
                            ? dataRow["ValueStr"].ToString()
                            : string.Empty,
                    ImageUrl =
                        dataRow["ImageUrl"] != DBNull.Value
                            ? dataRow["ImageUrl"].ToString()
                            : string.Empty
                });
                attributeInfo.ValueStr.Add(dataRow.Field<string>("ValueStr"));
            }
            return list;
        }


        public List<Model.Shop.Products.AttributeInfo> GetAttributeInfoListByProductId(long productId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"
SELECT  A.AttributeId
      , A.AttributeName
      , A.DisplaySequence AS AttributeDisplaySequence
      , A.TypeId
      , A.UsageMode
      , A.UseAttributeImage
      , A.UserDefinedPic
      , V.ValueId
      , V.DisplaySequence AS ValueDisplaySequence
      , V.ValueStr
      , V.ImageUrl
FROM  PMS_ProductAttributes PA LEFT JOIN  PMS_Attributes A ON PA.AttributeId = A.AttributeId
        LEFT JOIN PMS_AttributeValues V ON PA.ValueId = V.ValueId
WHERE PA.ProductId = @ProductId");
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@ProductId", SqlDbType.Int));
            param[0].Value = productId;
            strSql.Append(" ORDER BY A.DisplaySequence,V.AttributeId,V.DisplaySequence");

            DataSet ds = param.Count > 0 ?
                DBHelper.DefaultDBHelper.Query(strSql.ToString(), param.ToArray()) :
                DBHelper.DefaultDBHelper.Query(strSql.ToString());
            if (ds == null || ds.Tables[0].Rows.Count < 0) return null;

            //Fill And Return List
            return FillAttributeInfos(ds);
        }

        public DataSet GetAttribute(int? cateID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT TOP(6)*  ");
            strSql.Append("FROM PMS_Attributes ");
            strSql.Append("WHERE AttributeId IN(SELECT DISTINCT AttributeId FROM PMS_ProductAttributes ");
            strSql.Append("WHERE ProductId IN(SELECT ProductId FROM PMS_Products ");
            if (cateID.HasValue)
            {
                strSql.AppendFormat("WHERE CategoryId ={0} ", cateID.Value);
            }
            strSql.Append(")) ");
            strSql.Append("ORDER BY PMS_Attributes.DisplaySequence ASC ");
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        public bool IsExistDefinedAttribute(int typeId, long? attId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(1) FROM PMS_Attributes ");
            strSql.AppendFormat("WHERE UsageMode=3 AND TypeId={0} AND UserDefinedPic=1 ", typeId);
            if (attId.HasValue)
            {
                strSql.AppendFormat("  AND AttributeId={0} ", attId.Value);
            }
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return false;
            }
            else
            {
                return Convert.ToInt32(obj) > 0;
            }
        }

        public DataSet GetProductAttributes(long productId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT A.ValueId,C.ValueStr,B.* FROM  ");
            strSql.Append("PMS_ProductAttributes A ");
            strSql.Append("LEFT JOIN PMS_Attributes B ON A.AttributeId = B.AttributeId ");
            strSql.Append("LEFT JOIN PMS_AttributeValues C ON C.ValueId= A.ValueId AND A.AttributeId = B.AttributeId ");
            strSql.Append("WHERE ProductId=@ProductId ");
            SqlParameter[] parameters ={
                                          new SqlParameter("@ProductId",SqlDbType.BigInt,8)
                                      };
            parameters[0].Value = productId;
            return DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 根据分类获取属性  
        /// </summary>
        /// <param name="cateID"></param>
        /// <param name="IsChild"></param>
        /// <returns></returns>
        public DataSet GetAttributesByCate(int cateID, bool IsChild)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  * FROM    PMS_Attributes  where  UsageMode <2 ");
            if (cateID > 0)
            {
                strSql.Append("  and    EXISTS ( SELECT * FROM   PMS_Products ");
                strSql.Append("  WHERE  SaleStatus=1 and  TypeId = PMS_Attributes.TypeId ");
                strSql.Append(" AND EXISTS ( SELECT * FROM   PMS_ProductCategories  ");
                strSql.Append(" WHERE  ProductId = PMS_Products.ProductId  ");
                if (IsChild)
                {
                    strSql.AppendFormat(
                        "   AND ( CategoryPath LIKE ( SELECT Path FROM PMS_Categories WHERE CategoryId = {0}  ) + '|%' ",
                        cateID);
                    strSql.AppendFormat(" OR PMS_ProductCategories.CategoryId = {0})", cateID);
                }
                else
                {
                    strSql.AppendFormat("   AND   PMS_ProductCategories.CategoryId = {0}", cateID);
                }
                strSql.Append(")) ");
            }
            strSql.Append("ORDER BY PMS_Attributes.DisplaySequence ASC ");
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }


        public bool IsExistName(int typeId, string name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(1) FROM PMS_Attributes");
            strSql.Append(" WHERE TypeId=@TypeId and AttributeName=@AttributeName");
            SqlParameter[] parameters = {
                    new SqlParameter("@TypeId", SqlDbType.Int,4),
                       new SqlParameter("@AttributeName", SqlDbType.NVarChar,200)
            };
            parameters[0].Value = typeId;
            parameters[1].Value = name;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }

      public string GetAttrValue(string keyName,long productId)
      {
          StringBuilder stringBuilder=new StringBuilder();
          stringBuilder.Append("select ValueStr from PMS_AttributeValues where ValueId in(");
          stringBuilder.Append(
              "select ValueId from PMS_ProductAttributes where ProductId=@productId and AttributeId in  (");
          stringBuilder.Append("select AttributeId from PMS_Attributes where AttributeName=@AttributeName))");
          SqlParameter[] parameters = {
                    new SqlParameter("@productId", SqlDbType.BigInt),
                       new SqlParameter("@AttributeName", SqlDbType.NVarChar,200)
            };
          parameters[0].Value = productId;
          parameters[1].Value = keyName;
          object obj = DBHelper.DefaultDBHelper.GetSingle(stringBuilder.ToString(),parameters);

          if (obj != null)
          {
              return obj.ToString();
          }
          else
          {
              return null;
          }
      }


        /// <summary>
        /// 重置属性表
        /// </summary>
        /// <returns></returns>
        public bool ResetTable()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("TRUNCATE TABLE PMS_AttributeValues ");
            strSql.Append("TRUNCATE TABLE PMS_Attributes ");
            return DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString()) > 0;
        }



        public bool AttributeManage(Model.Shop.Products.AttributeInfo model)
        {

            SqlParameter[] param = {
                                   new SqlParameter("@AttributeId",SqlDbType.BigInt),
                                   new SqlParameter("@AttributeName",SqlDbType.NVarChar),
                                   new SqlParameter("@TypeId",SqlDbType.Int),
                                   new SqlParameter("@UsageMode",SqlDbType.Int),
                                   new SqlParameter("@UseAttributeImage",SqlDbType.Bit),
                                   new SqlParameter("@UserDefinedPic",SqlDbType.Bit),
                                   };
            param[0].Value = model.AttributeId;
            param[1].Value = model.AttributeName;
            param[2].Value = model.TypeId;
            param[3].Value = model.UsageMode;
            param[4].Value = model.UseAttributeImage;
            param[5].Value = model.UserDefinedPic;
            string sql = string.Empty;
            long attId = model.AttributeId;
            if (model.UsageMode == 3)
            {
                sql = @"delete from PMS_Attributes where AttributeId=@AttributeId;SET IDENTITY_INSERT [PMS_Attributes] ON  
INSERT  INTO PMS_Attributes(AttributeId, AttributeName ,DisplaySequence ,TypeId ,UsageMode ,UseAttributeImage,UserDefinedPic)
 SELECT @AttributeId,@AttributeName , ISNULL(MAX(DisplaySequence)+1,1), 
@TypeId ,@UsageMode , @UseAttributeImage ,@UserDefinedPic
FROM PMS_Attributes  WHERE TypeId = @TypeId AND UsageMode = 3;
 SET IDENTITY_INSERT [PMS_Attributes] OFF 
";

            }
            else
            {
                sql = @" delete from PMS_Attributes where AttributeId=@AttributeId;SET IDENTITY_INSERT [PMS_Attributes] ON  INSERT  INTO PMS_Attributes( AttributeId,AttributeName ,DisplaySequence ,TypeId ,UsageMode ,UseAttributeImage,UserDefinedPic)
 SELECT @AttributeId,@AttributeName , ISNULL(MAX(DisplaySequence)+1,1), 
@TypeId ,@UsageMode , @UseAttributeImage ,@UserDefinedPic
FROM PMS_Attributes  WHERE TypeId = @TypeId ;
 SET IDENTITY_INSERT [PMS_Attributes] OFF ";
            }
            DBHelper.DefaultDBHelper.ExecuteSql(sql, param);

            if (attId > 0)
            {
                AttributeValue attInfo = new AttributeValue();
                if (model.AttributeValues != null)
                {
                    foreach (var item in model.AttributeValues)
                    {
                        attInfo.Add(item);
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

