/**
* TrialReports.cs
*
* 功 能： N/A
* 类 名： TrialReports
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/5/22 18:12:41   N/A    初版
*
* Copyright (c) 2012-2013 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Collections.Generic;
using YSWL.Common;
using YSWL.MALL.Model.Shop.Trial;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Shop.Trial;
namespace YSWL.MALL.BLL.Shop.Trial
{
	/// <summary>
	/// TrialReports
	/// </summary>
	public partial class TrialReports
	{
        private readonly ITrialReports dal = DAShopTrial.CreateTrialReports();
		public TrialReports()
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
		public bool Exists(int ReportId)
		{
			return dal.Exists(ReportId);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(YSWL.MALL.Model.Shop.Trial.TrialReports model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(YSWL.MALL.Model.Shop.Trial.TrialReports model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int ReportId)
		{
			
			return dal.Delete(ReportId);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string ReportIdlist )
		{
			return dal.DeleteList(Common.Globals.SafeLongFilter(ReportIdlist ,0) );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public YSWL.MALL.Model.Shop.Trial.TrialReports GetModel(int ReportId)
		{
			
			return dal.GetModel(ReportId);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public YSWL.MALL.Model.Shop.Trial.TrialReports GetModelByCache(int ReportId)
		{
			
			string CacheKey = "TrialReportsModel-" + ReportId;
			object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(ReportId);
					if (objModel != null)
					{
						int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("CacheTime");
						YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (YSWL.MALL.Model.Shop.Trial.TrialReports)objModel;
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
		public List<YSWL.MALL.Model.Shop.Trial.TrialReports> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<YSWL.MALL.Model.Shop.Trial.TrialReports> DataTableToList(DataTable dt)
		{
			List<YSWL.MALL.Model.Shop.Trial.TrialReports> modelList = new List<YSWL.MALL.Model.Shop.Trial.TrialReports>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				YSWL.MALL.Model.Shop.Trial.TrialReports model;
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
        public List<YSWL.MALL.Model.Shop.Trial.TrialReports> GetTopList(int Top,string strWhere,string orderby)
        {
            DataSet ds = dal.GetList(Top, strWhere, orderby);
            return DataTableToList(ds.Tables[0]);
        }
		#endregion  ExtensionMethod
	}
}

