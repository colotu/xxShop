/*----------------------------------------------------------------
// Copyright (C) 2012 云商未来 版权所有。 
//
// 文件名：WebMenuConfig.cs
// 文件功能描述：网站菜单导航业务逻辑层
// 
// 创建标识：2012年5月23日 16:30:09
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/
using System;
using System.Data;
using System.Collections.Generic;
using YSWL.Common;
using YSWL.MALL.Model.Settings;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Settings;
namespace YSWL.MALL.BLL.Settings
{
	/// <summary>
	/// 导航菜单
	/// </summary>
	public partial class MainMenus
	{
        private readonly IMainMenus dal = DASettings.CreateMainMenus();
        public MainMenus()
        { }
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
        public bool Exists(int MenuID)
        {
            return dal.Exists(MenuID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.MALL.Model.Settings.MainMenus model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.Settings.MainMenus model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int MenuID)
        {

            return dal.Delete(MenuID);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string MenuIDlist)
        {
            return dal.DeleteList(Common.Globals.SafeLongFilter(MenuIDlist,0) );
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Settings.MainMenus GetModel(int MenuID)
        {

            return dal.GetModel(MenuID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public YSWL.MALL.Model.Settings.MainMenus GetModelByCache(int MenuID)
        {

            string CacheKey = "WebMenuConfigModel-" + MenuID;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(MenuID);
                    if (objModel != null)
                    {
                    int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.Settings.MainMenus)objModel;
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
        public List<YSWL.MALL.Model.Settings.MainMenus> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Settings.MainMenus> DataTableToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Settings.MainMenus> modelList = new List<YSWL.MALL.Model.Settings.MainMenus>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Settings.MainMenus model;
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

        #region 扩展方法
        public List<YSWL.MALL.Model.Settings.MainMenus> GetMenusByArea(YSWL.MALL.Model.Ms.EnumHelper.AreaType area,string theme="",int top=0)
        {
            string strWhere = " IsUsed=1 and NavArea=" + (int) area;
            if (!String.IsNullOrWhiteSpace(theme))
            {
                strWhere += " and (NavTheme='" + Common.InjectionFilter.SqlFilter(theme) + "' or NavTheme='')";
            }
            return GetModelList(top,strWhere," Sequence ");
        }

        public List<YSWL.MALL.Model.Settings.MainMenus> GetMenusByAreaByCacle(YSWL.MALL.Model.Ms.EnumHelper.AreaType area, string theme = "",int top=0)
        {
            string CacheKey = "GetMenusByAreaByCacle-" + area + theme;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = GetMenusByArea(area,theme,top);
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (List<YSWL.MALL.Model.Settings.MainMenus>)objModel;
      
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Settings.MainMenus> GetModelList(int Top, string strWhere, string filedOrder)
        {
            DataSet ds = dal.GetList(Top, strWhere, filedOrder);
            return DataTableToList(ds.Tables[0]);
        }
        #endregion 
    }
}

