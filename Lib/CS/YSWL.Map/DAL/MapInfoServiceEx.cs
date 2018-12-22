using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.DBUtility;

namespace YSWL.Map.DAL
{
    /// <summary>
    /// 数据访问类:MapService
    /// </summary>
    public partial class MapInfoService
    {
        /// <summary>
        /// 是否存在该企业记录
        /// </summary>
        public bool ExistsByDepartmentId(int department)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Ms_Maps");
            strSql.Append(" where DepartmentId=@DepartmentId");
            SqlParameter[] parameters = {
                    new SqlParameter("@DepartmentId", SqlDbType.Int,4)
                    };
            parameters[0].Value = department;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.Map.Model.MapInfo GetModelByDepartmentId(int departmentId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 MapId,UserId,DepartmentId,PointerLongitude,PointDimension,PointClass,PointerType,PointerTitle,PointImg,PointerContent,SearchCity,searchArea,Level,enableKeyboard,enableScrollWheelZoom,NavigationControl,ScaleControl,MapTypeControl,MarkersLongitude,MarkersDimension,setAnimation,LoadEvent,MenuItemzoomIn,MenuItemzoomOut,MenuItemsetZoomTop,MenuItemsetPoint,MapType,Other1,Other2,Other3 from Ms_Maps ");
            strSql.Append(" where DepartmentId=@DepartmentId");
            SqlParameter[] parameters = {
                    new SqlParameter("@DepartmentId", SqlDbType.Int,4)
};
            parameters[0].Value = departmentId;
            return GetMapInfo(strSql, parameters);
        }

        private static Model.MapInfo GetMapInfo(StringBuilder strSql, SqlParameter[] parameters)
        {
            YSWL.Map.Model.MapInfo model = new YSWL.Map.Model.MapInfo();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["MapId"] != null && ds.Tables[0].Rows[0]["MapId"].ToString() != "")
                {
                    model.MapId = int.Parse(ds.Tables[0].Rows[0]["MapId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["UserId"] != null && ds.Tables[0].Rows[0]["UserId"].ToString() != "")
                {
                    model.UserId = int.Parse(ds.Tables[0].Rows[0]["UserId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DepartmentId"] != null && ds.Tables[0].Rows[0]["DepartmentId"].ToString() != "")
                {
                    model.DepartmentId = int.Parse(ds.Tables[0].Rows[0]["DepartmentId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["PointerLongitude"] != null && ds.Tables[0].Rows[0]["PointerLongitude"].ToString() != "")
                {
                    model.PointerLongitude = ds.Tables[0].Rows[0]["PointerLongitude"].ToString();
                }
                if (ds.Tables[0].Rows[0]["PointDimension"] != null && ds.Tables[0].Rows[0]["PointDimension"].ToString() != "")
                {
                    model.PointDimension = ds.Tables[0].Rows[0]["PointDimension"].ToString();
                }
                if (ds.Tables[0].Rows[0]["PointClass"] != null && ds.Tables[0].Rows[0]["PointClass"].ToString() != "")
                {
                    model.PointClass = ds.Tables[0].Rows[0]["PointClass"].ToString();
                }
                if (ds.Tables[0].Rows[0]["PointerType"] != null && ds.Tables[0].Rows[0]["PointerType"].ToString() != "")
                {
                    model.PointerType = ds.Tables[0].Rows[0]["PointerType"].ToString();
                }
                if (ds.Tables[0].Rows[0]["PointerTitle"] != null && ds.Tables[0].Rows[0]["PointerTitle"].ToString() != "")
                {
                    model.PointerTitle = ds.Tables[0].Rows[0]["PointerTitle"].ToString();
                }
                if (ds.Tables[0].Rows[0]["PointImg"] != null && ds.Tables[0].Rows[0]["PointImg"].ToString() != "")
                {
                    model.PointImg = ds.Tables[0].Rows[0]["PointImg"].ToString();
                }
                if (ds.Tables[0].Rows[0]["PointerContent"] != null && ds.Tables[0].Rows[0]["PointerContent"].ToString() != "")
                {
                    model.PointerContent = ds.Tables[0].Rows[0]["PointerContent"].ToString();
                }
                if (ds.Tables[0].Rows[0]["SearchCity"] != null && ds.Tables[0].Rows[0]["SearchCity"].ToString() != "")
                {
                    model.SearchCity = ds.Tables[0].Rows[0]["SearchCity"].ToString();
                }
                if (ds.Tables[0].Rows[0]["searchArea"] != null && ds.Tables[0].Rows[0]["searchArea"].ToString() != "")
                {
                    model.searchArea = ds.Tables[0].Rows[0]["searchArea"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Level"] != null && ds.Tables[0].Rows[0]["Level"].ToString() != "")
                {
                    model.Level = int.Parse(ds.Tables[0].Rows[0]["Level"].ToString());
                }
                if (ds.Tables[0].Rows[0]["enableKeyboard"] != null && ds.Tables[0].Rows[0]["enableKeyboard"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["enableKeyboard"].ToString() == "1") || (ds.Tables[0].Rows[0]["enableKeyboard"].ToString().ToLower() == "true"))
                    {
                        model.enableKeyboard = true;
                    }
                    else
                    {
                        model.enableKeyboard = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["enableScrollWheelZoom"] != null && ds.Tables[0].Rows[0]["enableScrollWheelZoom"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["enableScrollWheelZoom"].ToString() == "1") || (ds.Tables[0].Rows[0]["enableScrollWheelZoom"].ToString().ToLower() == "true"))
                    {
                        model.enableScrollWheelZoom = true;
                    }
                    else
                    {
                        model.enableScrollWheelZoom = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["NavigationControl"] != null && ds.Tables[0].Rows[0]["NavigationControl"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["NavigationControl"].ToString() == "1") || (ds.Tables[0].Rows[0]["NavigationControl"].ToString().ToLower() == "true"))
                    {
                        model.NavigationControl = true;
                    }
                    else
                    {
                        model.NavigationControl = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["ScaleControl"] != null && ds.Tables[0].Rows[0]["ScaleControl"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["ScaleControl"].ToString() == "1") || (ds.Tables[0].Rows[0]["ScaleControl"].ToString().ToLower() == "true"))
                    {
                        model.ScaleControl = true;
                    }
                    else
                    {
                        model.ScaleControl = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["MapTypeControl"] != null && ds.Tables[0].Rows[0]["MapTypeControl"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["MapTypeControl"].ToString() == "1") || (ds.Tables[0].Rows[0]["MapTypeControl"].ToString().ToLower() == "true"))
                    {
                        model.MapTypeControl = true;
                    }
                    else
                    {
                        model.MapTypeControl = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["MarkersLongitude"] != null && ds.Tables[0].Rows[0]["MarkersLongitude"].ToString() != "")
                {
                    model.MarkersLongitude = ds.Tables[0].Rows[0]["MarkersLongitude"].ToString();
                }
                if (ds.Tables[0].Rows[0]["MarkersDimension"] != null && ds.Tables[0].Rows[0]["MarkersDimension"].ToString() != "")
                {
                    model.MarkersDimension = ds.Tables[0].Rows[0]["MarkersDimension"].ToString();
                }
                if (ds.Tables[0].Rows[0]["setAnimation"] != null && ds.Tables[0].Rows[0]["setAnimation"].ToString() != "")
                {
                    model.setAnimation = ds.Tables[0].Rows[0]["setAnimation"].ToString();
                }
                if (ds.Tables[0].Rows[0]["LoadEvent"] != null && ds.Tables[0].Rows[0]["LoadEvent"].ToString() != "")
                {
                    model.LoadEvent = ds.Tables[0].Rows[0]["LoadEvent"].ToString();
                }
                if (ds.Tables[0].Rows[0]["MenuItemzoomIn"] != null && ds.Tables[0].Rows[0]["MenuItemzoomIn"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["MenuItemzoomIn"].ToString() == "1") || (ds.Tables[0].Rows[0]["MenuItemzoomIn"].ToString().ToLower() == "true"))
                    {
                        model.MenuItemzoomIn = true;
                    }
                    else
                    {
                        model.MenuItemzoomIn = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["MenuItemzoomOut"] != null && ds.Tables[0].Rows[0]["MenuItemzoomOut"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["MenuItemzoomOut"].ToString() == "1") || (ds.Tables[0].Rows[0]["MenuItemzoomOut"].ToString().ToLower() == "true"))
                    {
                        model.MenuItemzoomOut = true;
                    }
                    else
                    {
                        model.MenuItemzoomOut = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["MenuItemsetZoomTop"] != null && ds.Tables[0].Rows[0]["MenuItemsetZoomTop"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["MenuItemsetZoomTop"].ToString() == "1") || (ds.Tables[0].Rows[0]["MenuItemsetZoomTop"].ToString().ToLower() == "true"))
                    {
                        model.MenuItemsetZoomTop = true;
                    }
                    else
                    {
                        model.MenuItemsetZoomTop = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["MenuItemsetPoint"] != null && ds.Tables[0].Rows[0]["MenuItemsetPoint"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["MenuItemsetPoint"].ToString() == "1") || (ds.Tables[0].Rows[0]["MenuItemsetPoint"].ToString().ToLower() == "true"))
                    {
                        model.MenuItemsetPoint = true;
                    }
                    else
                    {
                        model.MenuItemsetPoint = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["MapType"] != null && ds.Tables[0].Rows[0]["MapType"].ToString() != "")
                {
                    model.MapType = int.Parse(ds.Tables[0].Rows[0]["MapType"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Other1"] != null && ds.Tables[0].Rows[0]["Other1"].ToString() != "")
                {
                    model.Other1 = ds.Tables[0].Rows[0]["Other1"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Other2"] != null && ds.Tables[0].Rows[0]["Other2"].ToString() != "")
                {
                    model.Other2 = ds.Tables[0].Rows[0]["Other2"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Other3"] != null && ds.Tables[0].Rows[0]["Other3"].ToString() != "")
                {
                    model.Other3 = ds.Tables[0].Rows[0]["Other3"].ToString();
                }
                return model;
            }
            else
            {
                return null;
            }
        }
    }
}

