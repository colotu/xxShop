/**  版本信息模板在安装目录下，可自行修改。
* Location.cs
*
* 功 能： N/A
* 类 名： Location
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/3/24 14:00:03   N/A    初版
*
* Copyright (c) 2012 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Collections.Generic;
using YSWL.Common;
using YSWL.WeChat.Model.Core;
using YSWL.WeChat.IDAL.Core;
namespace YSWL.WeChat.BLL.Core
{
	/// <summary>
	/// Location
	/// </summary>
	public partial class Location
	{
        private readonly ILocation dal = YSWL.DBUtility.PubConstant.IsSQLServer ? (ILocation)new YSWL.WeChat.SQLServerDAL.Core.Location() : (ILocation)new YSWL.WeChat.MySqlDAL.Core.Location();//暂时预留
		public Location()
		{}
		#region  BasicMethod
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(long LocationId)
		{
			return dal.Exists(LocationId);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public long Add(YSWL.WeChat.Model.Core.Location model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(YSWL.WeChat.Model.Core.Location model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(long LocationId)
		{
			
			return dal.Delete(LocationId);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string LocationIdlist )
		{
			return dal.DeleteList(LocationIdlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public YSWL.WeChat.Model.Core.Location GetModel(long LocationId)
		{
			
			return dal.GetModel(LocationId);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public YSWL.WeChat.Model.Core.Location GetModelByCache(long LocationId)
		{
			
			string CacheKey = "LocationModel-" + LocationId;
			object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(LocationId);
					if (objModel != null)
					{
						int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("ModelCache");
						YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (YSWL.WeChat.Model.Core.Location)objModel;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<YSWL.WeChat.Model.Core.Location> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<YSWL.WeChat.Model.Core.Location> DataTableToList(DataTable dt)
		{
			List<YSWL.WeChat.Model.Core.Location> modelList = new List<YSWL.WeChat.Model.Core.Location>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				YSWL.WeChat.Model.Core.Location model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = dal.DataRowToModel(dt.Rows[n]);
					if (model != null)
					{
						modelList.Add(model);
					}
				}
			}
			return modelList;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			return dal.GetRecordCount(strWhere);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			return dal.GetListByPage( strWhere,  orderby,  startIndex,  endIndex);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  BasicMethod
		#region  ExtensionMethod
      
		#endregion  ExtensionMethod
	}
}

