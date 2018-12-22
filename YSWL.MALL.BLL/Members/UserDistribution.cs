/**  版本信息模板在安装目录下，可自行修改。
* UserDistribution.cs
*
* 功 能： N/A
* 类 名： UserDistribution
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2017/1/14 12:07:50   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Collections.Generic;
using YSWL.Common;
using YSWL.DBUtility;
using YSWL.MALL.Model.Members;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Members;
namespace YSWL.MALL.BLL.Members
{
	/// <summary>
	/// UserDistribution
	/// </summary>
	public partial class UserDistribution
	{
		private readonly IUserDistribution dal= DAMembers.CreateUserDistribution();

        private static DataCacheCore dataCache = new DataCacheCore(new CacheOption
        {
            CacheType = SAASInfo.GetSystemBoolValue("RedisCacheUse") ? CacheType.Redis : CacheType.IIS,
            ReadWriteHosts = SAASInfo.GetSystemValue("RedisCacheReadWriteHosts"),
            ReadOnlyHosts = SAASInfo.GetSystemValue("RedisCacheReadOnlyHosts"),
            CancelProductKey = true,
            DefaultDb = 6
        });

        public UserDistribution()
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
		public bool Exists(int UserId,int DistributionId)
		{
			return dal.Exists(UserId,DistributionId);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(YSWL.MALL.Model.Members.UserDistribution model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(YSWL.MALL.Model.Members.UserDistribution model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int UserId,int DistributionId)
		{
			
			return dal.Delete(UserId,DistributionId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public YSWL.MALL.Model.Members.UserDistribution GetModel(int UserId,int DistributionId)
		{
			
			return dal.GetModel(UserId,DistributionId);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public YSWL.MALL.Model.Members.UserDistribution GetModelByCache(int UserId,int DistributionId)
		{
			
			string CacheKey = "UserDistributionModel-" + UserId+DistributionId;
			object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(UserId,DistributionId);
					if (objModel != null)
					{
						int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("ModelCache");
						YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (YSWL.MALL.Model.Members.UserDistribution)objModel;
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
		public List<YSWL.MALL.Model.Members.UserDistribution> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<YSWL.MALL.Model.Members.UserDistribution> DataTableToList(DataTable dt)
		{
			List<YSWL.MALL.Model.Members.UserDistribution> modelList = new List<YSWL.MALL.Model.Members.UserDistribution>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				YSWL.MALL.Model.Members.UserDistribution model;
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
        ///根据用户获取线上分销ID
	    public  int GetDistributionId(int userId)
	    {
	        return dal.GetDistributionId(userId);
	    }

	    public static int GetUserDistrId(int userId)
	    {
            string CacheKey = "MDM_UserDistributionInfo_" + userId;
             int   objModel = Common.Globals.SafeInt(dataCache.GetCache<string>(CacheKey),0);
            if (objModel ==0)
            {
                try
                {
                    YSWL.MALL.BLL.Members.UserDistribution distrBll = new YSWL.MALL.BLL.Members.UserDistribution();
                    objModel = distrBll.GetDistributionId(userId);
                    if (objModel !=0)
                    {
                        dataCache.SetCache(CacheKey, objModel, DateTime.MaxValue, TimeSpan.Zero);
                    }
                }
                catch (Exception ex)
                {
                    Log.LogHelper.AddErrorLog(ex.Message, ex.StackTrace);
                }
            }
            return objModel;
        }


	    #endregion  ExtensionMethod
    }
}

