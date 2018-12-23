﻿using System;
using System.Data;
using System.Collections.Generic;
using YSWL.Common;
using YSWL.MALL.Model.Members;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Members;
namespace YSWL.MALL.BLL.Members
{
	/// <summary>
	/// 反馈类型
	/// </summary>
	public partial class FeedbackType
	{
		private readonly IFeedbackType dal=DAMembers.CreateFeedbackType();
		public FeedbackType()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int TypeId)
		{
			return dal.Exists(TypeId);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(YSWL.MALL.Model.Members.FeedbackType model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(YSWL.MALL.Model.Members.FeedbackType model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int TypeId)
		{
			
			return dal.Delete(TypeId);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string TypeIdlist )
		{
			return dal.DeleteList(Common.Globals.SafeLongFilter(TypeIdlist ,0) );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public YSWL.MALL.Model.Members.FeedbackType GetModel(int TypeId)
		{
			
			return dal.GetModel(TypeId);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public YSWL.MALL.Model.Members.FeedbackType GetModelByCache(int TypeId)
		{
			
			string CacheKey = "FeedbackTypeModel-" + TypeId;
			object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(TypeId);
					if (objModel != null)
					{
						 int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
						YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (YSWL.MALL.Model.Members.FeedbackType)objModel;
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
		public List<YSWL.MALL.Model.Members.FeedbackType> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<YSWL.MALL.Model.Members.FeedbackType> DataTableToList(DataTable dt)
		{
			List<YSWL.MALL.Model.Members.FeedbackType> modelList = new List<YSWL.MALL.Model.Members.FeedbackType>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				YSWL.MALL.Model.Members.FeedbackType model;
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
