using System;
using System.Data;
using System.Collections.Generic;

namespace YSWL.Map.BLL
{
    /// <summary>
    /// BaiduMap
    /// </summary>
    public partial class MapInfoManage
    {
        private const int ModelCache = 30;
        private readonly YSWL.Map.DAL.MapInfoService dal = new YSWL.Map.DAL.MapInfoService();
        public MapInfoManage()
        { }
        #region  Method

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
        public bool Exists(int MapId)
        {
            return dal.Exists(MapId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.Map.Model.MapInfo model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.Map.Model.MapInfo model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int MapId)
        {

            return dal.Delete(MapId);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string MapIdlist)
        {
            return dal.DeleteList(MapIdlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.Map.Model.MapInfo GetModel(int MapId)
        {

            return dal.GetModel(MapId);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public YSWL.Map.Model.MapInfo GetModelByCache(int MapId)
        {

            string CacheKey = "MapsModel-" + MapId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(MapId);
                    if (objModel != null)
                    {
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.Map.Model.MapInfo)objModel;
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
        public List<YSWL.Map.Model.MapInfo> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.Map.Model.MapInfo> DataTableToList(DataTable dt)
        {
            List<YSWL.Map.Model.MapInfo> modelList = new List<YSWL.Map.Model.MapInfo>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.Map.Model.MapInfo model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new YSWL.Map.Model.MapInfo();
                    if (dt.Rows[n]["MapId"] != null && dt.Rows[n]["MapId"].ToString() != "")
                    {
                        model.MapId = int.Parse(dt.Rows[n]["MapId"].ToString());
                    }
                    if (dt.Rows[n]["UserId"] != null && dt.Rows[n]["UserId"].ToString() != "")
                    {
                        model.UserId = int.Parse(dt.Rows[n]["UserId"].ToString());
                    }
                    if (dt.Rows[n]["DepartmentId"] != null && dt.Rows[n]["DepartmentId"].ToString() != "")
                    {
                        model.DepartmentId = int.Parse(dt.Rows[n]["DepartmentId"].ToString());
                    }
                    if (dt.Rows[n]["PointerLongitude"] != null && dt.Rows[n]["PointerLongitude"].ToString() != "")
                    {
                        model.PointerLongitude = dt.Rows[n]["PointerLongitude"].ToString();
                    }
                    if (dt.Rows[n]["PointDimension"] != null && dt.Rows[n]["PointDimension"].ToString() != "")
                    {
                        model.PointDimension = dt.Rows[n]["PointDimension"].ToString();
                    }
                    if (dt.Rows[n]["PointClass"] != null && dt.Rows[n]["PointClass"].ToString() != "")
                    {
                        model.PointClass = dt.Rows[n]["PointClass"].ToString();
                    }
                    if (dt.Rows[n]["PointerType"] != null && dt.Rows[n]["PointerType"].ToString() != "")
                    {
                        model.PointerType = dt.Rows[n]["PointerType"].ToString();
                    }
                    if (dt.Rows[n]["PointerTitle"] != null && dt.Rows[n]["PointerTitle"].ToString() != "")
                    {
                        model.PointerTitle = dt.Rows[n]["PointerTitle"].ToString();
                    }
                    if (dt.Rows[n]["PointImg"] != null && dt.Rows[n]["PointImg"].ToString() != "")
                    {
                        model.PointImg = dt.Rows[n]["PointImg"].ToString();
                    }
                    if (dt.Rows[n]["PointerContent"] != null && dt.Rows[n]["PointerContent"].ToString() != "")
                    {
                        model.PointerContent = dt.Rows[n]["PointerContent"].ToString();
                    }
                    if (dt.Rows[n]["SearchCity"] != null && dt.Rows[n]["SearchCity"].ToString() != "")
                    {
                        model.SearchCity = dt.Rows[n]["SearchCity"].ToString();
                    }
                    if (dt.Rows[n]["searchArea"] != null && dt.Rows[n]["searchArea"].ToString() != "")
                    {
                        model.searchArea = dt.Rows[n]["searchArea"].ToString();
                    }
                    if (dt.Rows[n]["Level"] != null && dt.Rows[n]["Level"].ToString() != "")
                    {
                        model.Level = int.Parse(dt.Rows[n]["Level"].ToString());
                    }
                    if (dt.Rows[n]["enableKeyboard"] != null && dt.Rows[n]["enableKeyboard"].ToString() != "")
                    {
                        if ((dt.Rows[n]["enableKeyboard"].ToString() == "1") || (dt.Rows[n]["enableKeyboard"].ToString().ToLower() == "true"))
                        {
                            model.enableKeyboard = true;
                        }
                        else
                        {
                            model.enableKeyboard = false;
                        }
                    }
                    if (dt.Rows[n]["enableScrollWheelZoom"] != null && dt.Rows[n]["enableScrollWheelZoom"].ToString() != "")
                    {
                        if ((dt.Rows[n]["enableScrollWheelZoom"].ToString() == "1") || (dt.Rows[n]["enableScrollWheelZoom"].ToString().ToLower() == "true"))
                        {
                            model.enableScrollWheelZoom = true;
                        }
                        else
                        {
                            model.enableScrollWheelZoom = false;
                        }
                    }
                    if (dt.Rows[n]["NavigationControl"] != null && dt.Rows[n]["NavigationControl"].ToString() != "")
                    {
                        if ((dt.Rows[n]["NavigationControl"].ToString() == "1") || (dt.Rows[n]["NavigationControl"].ToString().ToLower() == "true"))
                        {
                            model.NavigationControl = true;
                        }
                        else
                        {
                            model.NavigationControl = false;
                        }
                    }
                    if (dt.Rows[n]["ScaleControl"] != null && dt.Rows[n]["ScaleControl"].ToString() != "")
                    {
                        if ((dt.Rows[n]["ScaleControl"].ToString() == "1") || (dt.Rows[n]["ScaleControl"].ToString().ToLower() == "true"))
                        {
                            model.ScaleControl = true;
                        }
                        else
                        {
                            model.ScaleControl = false;
                        }
                    }
                    if (dt.Rows[n]["MapTypeControl"] != null && dt.Rows[n]["MapTypeControl"].ToString() != "")
                    {
                        if ((dt.Rows[n]["MapTypeControl"].ToString() == "1") || (dt.Rows[n]["MapTypeControl"].ToString().ToLower() == "true"))
                        {
                            model.MapTypeControl = true;
                        }
                        else
                        {
                            model.MapTypeControl = false;
                        }
                    }
                    if (dt.Rows[n]["MarkersLongitude"] != null && dt.Rows[n]["MarkersLongitude"].ToString() != "")
                    {
                        model.MarkersLongitude = dt.Rows[n]["MarkersLongitude"].ToString();
                    }
                    if (dt.Rows[n]["Markersdimension"] != null && dt.Rows[n]["Markersdimension"].ToString() != "")
                    {
                        model.MarkersDimension = dt.Rows[n]["Markersdimension"].ToString();
                    }
                    if (dt.Rows[n]["setAnimation"] != null && dt.Rows[n]["setAnimation"].ToString() != "")
                    {
                        model.setAnimation = dt.Rows[n]["setAnimation"].ToString();
                    }
                    if (dt.Rows[n]["LoadEvent"] != null && dt.Rows[n]["LoadEvent"].ToString() != "")
                    {
                        model.LoadEvent = dt.Rows[n]["LoadEvent"].ToString();
                    }
                    if (dt.Rows[n]["MenuItemzoomIn"] != null && dt.Rows[n]["MenuItemzoomIn"].ToString() != "")
                    {
                        if ((dt.Rows[n]["MenuItemzoomIn"].ToString() == "1") || (dt.Rows[n]["MenuItemzoomIn"].ToString().ToLower() == "true"))
                        {
                            model.MenuItemzoomIn = true;
                        }
                        else
                        {
                            model.MenuItemzoomIn = false;
                        }
                    }
                    if (dt.Rows[n]["MenuItemzoomOut"] != null && dt.Rows[n]["MenuItemzoomOut"].ToString() != "")
                    {
                        if ((dt.Rows[n]["MenuItemzoomOut"].ToString() == "1") || (dt.Rows[n]["MenuItemzoomOut"].ToString().ToLower() == "true"))
                        {
                            model.MenuItemzoomOut = true;
                        }
                        else
                        {
                            model.MenuItemzoomOut = false;
                        }
                    }
                    if (dt.Rows[n]["MenuItemsetZoomTop"] != null && dt.Rows[n]["MenuItemsetZoomTop"].ToString() != "")
                    {
                        if ((dt.Rows[n]["MenuItemsetZoomTop"].ToString() == "1") || (dt.Rows[n]["MenuItemsetZoomTop"].ToString().ToLower() == "true"))
                        {
                            model.MenuItemsetZoomTop = true;
                        }
                        else
                        {
                            model.MenuItemsetZoomTop = false;
                        }
                    }
                    if (dt.Rows[n]["MenuItemsetPoint"] != null && dt.Rows[n]["MenuItemsetPoint"].ToString() != "")
                    {
                        if ((dt.Rows[n]["MenuItemsetPoint"].ToString() == "1") || (dt.Rows[n]["MenuItemsetPoint"].ToString().ToLower() == "true"))
                        {
                            model.MenuItemsetPoint = true;
                        }
                        else
                        {
                            model.MenuItemsetPoint = false;
                        }
                    }
                    if (dt.Rows[n]["MapType"] != null && dt.Rows[n]["MapType"].ToString() != "")
                    {
                        model.MapType = int.Parse(dt.Rows[n]["MapType"].ToString());
                    }
                    if (dt.Rows[n]["Other1"] != null && dt.Rows[n]["Other1"].ToString() != "")
                    {
                        model.Other1 = dt.Rows[n]["Other1"].ToString();
                    }
                    if (dt.Rows[n]["Other2"] != null && dt.Rows[n]["Other2"].ToString() != "")
                    {
                        model.Other2 = dt.Rows[n]["Other2"].ToString();
                    }
                    if (dt.Rows[n]["Other3"] != null && dt.Rows[n]["Other3"].ToString() != "")
                    {
                        model.Other3 = dt.Rows[n]["Other3"].ToString();
                    }
                    modelList.Add(model);
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

        #endregion  Method
    }
}

