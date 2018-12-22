/**
* Comment.cs
*
* 功 能： N/A
* 类 名： Comment
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/1/30 18:33:35   N/A    初版
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
using YSWL.MALL.Model.CMS;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.CMS;
namespace YSWL.MALL.BLL.CMS
{
	/// <summary>
	/// Comment
	/// </summary>
	public partial class Comment
	{
		private readonly IComment dal = DataAccess<IComment>.Create("CMS.Comment");
		public Comment()
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
		public bool Exists(int ID)
		{
			return dal.Exists(ID);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(YSWL.MALL.Model.CMS.Comment model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(YSWL.MALL.Model.CMS.Comment model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int ID)
		{
			
			return dal.Delete(ID);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string IDlist )
		{
			return dal.DeleteList(Common.Globals.SafeLongFilter(IDlist ,0) );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public YSWL.MALL.Model.CMS.Comment GetModel(int ID)
		{
			
			return dal.GetModel(ID);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public YSWL.MALL.Model.CMS.Comment GetModelByCache(int ID)
		{
			
			string CacheKey = "CommentModel-" + ID;
			object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(ID);
					if (objModel != null)
					{
						 int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
						YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (YSWL.MALL.Model.CMS.Comment)objModel;
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
		public List<YSWL.MALL.Model.CMS.Comment> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<YSWL.MALL.Model.CMS.Comment> DataTableToList(DataTable dt)
		{
			List<YSWL.MALL.Model.CMS.Comment> modelList = new List<YSWL.MALL.Model.CMS.Comment>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				YSWL.MALL.Model.CMS.Comment model;
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
		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public List<YSWL.MALL.Model.CMS.Comment> GetModelList(int Top, string strWhere, string filedOrder)
		{
			return DataTableToList(dal.GetListEx(Top, strWhere, filedOrder).Tables[0]);
		}
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int AddEx(YSWL.MALL.Model.CMS.Comment model)
        {
            if (YSWL.MALL.BLL.Settings.FilterWords.ContainsModWords(model.Description))
            {
                model.State = false;
            }
            return dal.AddEx(model);
        }

        public List<YSWL.MALL.Model.CMS.Comment> GetComments(int ContentId, int StartIndex, int EndIndex)
              {
                  DataSet ds = GetListByPage(" State=1 and ContentId=" + ContentId, " CreatedDate desc ", StartIndex,
                                             EndIndex);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.CMS.Comment> GetModelList(int top,int id,int typeid)
        {
            StringBuilder strBuider = new StringBuilder();
            strBuider.AppendFormat(" State=1 and ContentId={0} and TypeId ={1}",  id, typeid);
            DataSet ds = dal.GetList(top, strBuider.ToString(), " ID desc  ");
            return DataTableToList(ds.Tables[0]);
        }
        ///// <summary>
        ///// 获得数据列表
        ///// </summary>
        //public List<YSWL.MALL.Model.CMS.Comment> GetCommentsByPage(int cid, int typeid, int StartIndex, int EndIndex, out int toalCount)
        //{
        //    StringBuilder strBuider = new StringBuilder();
        //    strBuider.AppendFormat(" State=1 and ContentId={0} and TypeId ={1}",cid,typeid);
        //    toalCount = GetRecordCount(strBuider.ToString());
        //    DataSet ds = GetListByPage(strBuider.ToString(), " ID desc ", StartIndex, EndIndex);
        //    return DataTableToList(ds.Tables[0]);
        //}

	    public int AddTran(YSWL.MALL.Model.CMS.Comment model)
	    {
	        return dal.AddTran(model);
	    }

	    /// <summary>
	    /// 批量更新状态
	    /// </summary>
	    /// <param name="IDlist">id列表</param>
	    /// <param name="state">状态</param>
	    /// <returns></returns>
	    public bool UpdateList(string IDlist, int  state)
	    {
	        return dal.UpdateList(IDlist, state);
	    }

	    #endregion  ExtensionMethod
	}
}

