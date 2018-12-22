/**  版本信息模板在安装目录下，可自行修改。
* RankLimit.cs
*
* 功 能： N/A
* 类 名： RankLimit
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/12/17 16:54:41   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using YSWL.Common;
using YSWL.MALL.Model.Members;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Members;
namespace YSWL.MALL.BLL.Members
{
	/// <summary>
	/// RankLimit
	/// </summary>
	public partial class RankLimit
	{
        private readonly IRankLimit dal = DAMembers.CreateRankLimit();
		public RankLimit()
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
		public bool Exists(int LimitID)
		{
			return dal.Exists(LimitID);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(YSWL.MALL.Model.Members.RankLimit model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(YSWL.MALL.Model.Members.RankLimit model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int LimitID)
		{
			
			return dal.Delete(LimitID);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string LimitIDlist )
		{
			return dal.DeleteList(LimitIDlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public YSWL.MALL.Model.Members.RankLimit GetModel(int LimitID)
		{
			
			return dal.GetModel(LimitID);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public YSWL.MALL.Model.Members.RankLimit GetModelByCache(int LimitID)
		{
			
			string CacheKey = "RankLimitModel-" + LimitID;
			object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(LimitID);
					if (objModel != null)
					{
						int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("CacheTime");
						YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (YSWL.MALL.Model.Members.RankLimit)objModel;
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
		public List<YSWL.MALL.Model.Members.RankLimit> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<YSWL.MALL.Model.Members.RankLimit> DataTableToList(DataTable dt)
		{
			List<YSWL.MALL.Model.Members.RankLimit> modelList = new List<YSWL.MALL.Model.Members.RankLimit>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				YSWL.MALL.Model.Members.RankLimit model;
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
        #region  扩展方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string name)
        {
            int count = GetRecordCount("Name='" + Common.InjectionFilter.SqlFilter(name) + "'");
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <returns></returns>
        public List<YSWL.MALL.Model.Members.RankLimit> GetAllLimitList()
        {
            DataSet ds = dal.GetList("");
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 是否存在该记录(更新操作是调用)
        /// </summary>
        public bool Exists(string name, int limitid)
        {
            int count = GetRecordCount("Name='" + Common.InjectionFilter.SqlFilter(name) + "' and  LimitID!=" + limitid);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据(事务删除，更新积分规则引用的该限制)
        /// </summary>
        public bool DeleteEX(int PointsLimitID)
        {
            return dal.DeleteEX(PointsLimitID);
        }

        public bool ExistsLimit(int limitid)
        {
            return dal.ExistsLimit(limitid);
        }

        public List<YSWL.MALL.Model.Members.RankLimit> GetListByPageExt(string strWhere, string orderby, int startIndex, int endIndex)
        {
            DataSet ds = dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
            return DataTableToList(ds.Tables[0]);
        }

        public bool ExistsName(string name)
        {
            return dal.ExistsName(name);
        }

        public DataSet GetListByKeyWord(string keyword)
        {
            StringBuilder strWhere = new StringBuilder();
            if (!String.IsNullOrWhiteSpace(keyword))
            {
                strWhere.AppendFormat("  name like '%{0}%'", Common.InjectionFilter.SqlFilter(keyword));
                return dal.GetList(strWhere.ToString());
            }
            else
            {
                return GetAllList();
            }

        }
        #endregion
    }
}

