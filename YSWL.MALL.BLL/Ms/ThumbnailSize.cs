using System;
using System.Data;
using System.Collections.Generic;
using YSWL.Common;
using YSWL.MALL.Model.Ms;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Ms;
namespace YSWL.MALL.BLL.Ms
{
	/// <summary>
	/// ThumbnailSize
	/// </summary>
	public partial class ThumbnailSize
	{
		private readonly IThumbnailSize dal=DAMs.CreateThumbnailSize();
		public ThumbnailSize()
		{}
        #region  BasicMethod
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string ThumName)
        {
            return dal.Exists(ThumName);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(YSWL.MALL.Model.Ms.ThumbnailSize model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.Ms.ThumbnailSize model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string ThumName)
        {

            return dal.Delete(ThumName);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string ThumNamelist)
        {
            return dal.DeleteList(Common.Globals.SafeLongFilter(ThumNamelist,0) );
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Ms.ThumbnailSize GetModel(string ThumName)
        {

            return dal.GetModel(ThumName);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public YSWL.MALL.Model.Ms.ThumbnailSize GetModelByCache(string ThumName)
        {

            string CacheKey = "ThumbnailSizeModel-" + ThumName;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(ThumName);
                    if (objModel != null)
                    {
                    int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.Ms.ThumbnailSize)objModel;
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
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Ms.ThumbnailSize> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Ms.ThumbnailSize> DataTableToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Ms.ThumbnailSize> modelList = new List<YSWL.MALL.Model.Ms.ThumbnailSize>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Ms.ThumbnailSize model;
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
            return dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
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

	    public static List<YSWL.MALL.Model.Ms.ThumbnailSize> GetThumSizeList(YSWL.MALL.Model.Ms.EnumHelper.AreaType type,string Theme="")
	    {
            string CacheKey = "GetThumSizeList-" + (int)type+Theme;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                YSWL.MALL.BLL.Ms.ThumbnailSize thumBll = new ThumbnailSize();
                try
                {
                    string strWhere = " Type=" + (int) type;
                    if (!String.IsNullOrWhiteSpace(Theme))
                    {
                        strWhere += " and (Theme='" + Common.InjectionFilter.SqlFilter(Theme) + "' or Theme='')";
                    }
                    objModel = thumBll.GetModelList(strWhere);
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (List<YSWL.MALL.Model.Ms.ThumbnailSize>)objModel;
          

	    }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public static string GetCloudName(string ThumName)
        {
            YSWL.MALL.BLL.Ms.ThumbnailSize thumbBll=new ThumbnailSize();
            YSWL.MALL.Model.Ms.ThumbnailSize model = thumbBll.GetModelByCache(ThumName);
            if (model != null)
            {
                return model.CloudSizeName;
            }
            return "";
        }

	    /// <summary>
	    /// 是否存在该记录
	    /// </summary>
	    /// <param name="ThumName">ThumName</param>
	    /// <param name="type">区域</param>
	    /// <param name="Theme">模版名称</param>
	    /// <returns></returns>
	    public bool Exists(string ThumName, int type, string Theme)
	    {
	        return dal.Exists(ThumName, type, Theme);
	    }

	    #endregion  ExtensionMethod
	}
}

