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
        public MapInfoService()
        { }
        #region  Method

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperSQL.GetMaxID("MapId", "Ms_Maps");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int MapId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Ms_Maps");
            strSql.Append(" where MapId=@MapId");
            SqlParameter[] parameters = {
                    new SqlParameter("@MapId", SqlDbType.Int,4)
};
            parameters[0].Value = MapId;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.Map.Model.MapInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Ms_Maps(");
            strSql.Append("UserId,DepartmentId,PointerLongitude,PointDimension,PointClass,PointerType,PointerTitle,PointImg,PointerContent,SearchCity,searchArea,Level,enableKeyboard,enableScrollWheelZoom,NavigationControl,ScaleControl,MapTypeControl,MarkersLongitude,MarkersDimension,setAnimation,LoadEvent,MenuItemzoomIn,MenuItemzoomOut,MenuItemsetZoomTop,MenuItemsetPoint,MapType,Other1,Other2,Other3)");
            strSql.Append(" values (");
            strSql.Append("@UserId,@DepartmentId,@PointerLongitude,@PointDimension,@PointClass,@PointerType,@PointerTitle,@PointImg,@PointerContent,@SearchCity,@searchArea,@Level,@enableKeyboard,@enableScrollWheelZoom,@NavigationControl,@ScaleControl,@MapTypeControl,@MarkersLongitude,@MarkersDimension,@setAnimation,@LoadEvent,@MenuItemzoomIn,@MenuItemzoomOut,@MenuItemsetZoomTop,@MenuItemsetPoint,@MapType,@Other1,@Other2,@Other3)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserId", SqlDbType.Int,4),
                    new SqlParameter("@DepartmentId", SqlDbType.Int,4),
                    new SqlParameter("@PointerLongitude", SqlDbType.NVarChar,100),
                    new SqlParameter("@PointDimension", SqlDbType.NVarChar,100),
                    new SqlParameter("@PointClass", SqlDbType.NVarChar,100),
                    new SqlParameter("@PointerType", SqlDbType.NVarChar,100),
                    new SqlParameter("@PointerTitle", SqlDbType.NVarChar,100),
                    new SqlParameter("@PointImg", SqlDbType.NVarChar,100),
                    new SqlParameter("@PointerContent", SqlDbType.NVarChar,300),
                    new SqlParameter("@SearchCity", SqlDbType.NVarChar,50),
                    new SqlParameter("@searchArea", SqlDbType.NVarChar,50),
                    new SqlParameter("@Level", SqlDbType.Int,4),
                    new SqlParameter("@enableKeyboard", SqlDbType.Bit,1),
                    new SqlParameter("@enableScrollWheelZoom", SqlDbType.Bit,1),
                    new SqlParameter("@NavigationControl", SqlDbType.Bit,1),
                    new SqlParameter("@ScaleControl", SqlDbType.Bit,1),
                    new SqlParameter("@MapTypeControl", SqlDbType.Bit,1),
                    new SqlParameter("@MarkersLongitude", SqlDbType.NVarChar,100),
                    new SqlParameter("@MarkersDimension", SqlDbType.NVarChar,100),
                    new SqlParameter("@setAnimation", SqlDbType.NVarChar,100),
                    new SqlParameter("@LoadEvent", SqlDbType.NVarChar,50),
                    new SqlParameter("@MenuItemzoomIn", SqlDbType.Bit,1),
                    new SqlParameter("@MenuItemzoomOut", SqlDbType.Bit,1),
                    new SqlParameter("@MenuItemsetZoomTop", SqlDbType.Bit,1),
                    new SqlParameter("@MenuItemsetPoint", SqlDbType.Bit,1),
                    new SqlParameter("@MapType", SqlDbType.SmallInt,2),
                    new SqlParameter("@Other1", SqlDbType.NVarChar,100),
                    new SqlParameter("@Other2", SqlDbType.NVarChar,100),
                    new SqlParameter("@Other3", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.UserId;
            parameters[1].Value = model.DepartmentId;
            parameters[2].Value = model.PointerLongitude;
            parameters[3].Value = model.PointDimension;
            parameters[4].Value = model.PointClass;
            parameters[5].Value = model.PointerType;
            parameters[6].Value = model.PointerTitle;
            parameters[7].Value = model.PointImg;
            parameters[8].Value = model.PointerContent;
            parameters[9].Value = model.SearchCity;
            parameters[10].Value = model.searchArea;
            parameters[11].Value = model.Level;
            parameters[12].Value = model.enableKeyboard;
            parameters[13].Value = model.enableScrollWheelZoom;
            parameters[14].Value = model.NavigationControl;
            parameters[15].Value = model.ScaleControl;
            parameters[16].Value = model.MapTypeControl;
            parameters[17].Value = model.MarkersLongitude;
            parameters[18].Value = model.MarkersDimension;
            parameters[19].Value = model.setAnimation;
            parameters[20].Value = model.LoadEvent;
            parameters[21].Value = model.MenuItemzoomIn;
            parameters[22].Value = model.MenuItemzoomOut;
            parameters[23].Value = model.MenuItemsetZoomTop;
            parameters[24].Value = model.MenuItemsetPoint;
            parameters[25].Value = model.MapType;
            parameters[26].Value = model.Other1;
            parameters[27].Value = model.Other2;
            parameters[28].Value = model.Other3;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.Map.Model.MapInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Ms_Maps set ");
            strSql.Append("UserId=@UserId,");
            strSql.Append("DepartmentId=@DepartmentId,");
            strSql.Append("PointerLongitude=@PointerLongitude,");
            strSql.Append("PointDimension=@PointDimension,");
            strSql.Append("PointClass=@PointClass,");
            strSql.Append("PointerType=@PointerType,");
            strSql.Append("PointerTitle=@PointerTitle,");
            strSql.Append("PointImg=@PointImg,");
            strSql.Append("PointerContent=@PointerContent,");
            strSql.Append("SearchCity=@SearchCity,");
            strSql.Append("searchArea=@searchArea,");
            strSql.Append("Level=@Level,");
            strSql.Append("enableKeyboard=@enableKeyboard,");
            strSql.Append("enableScrollWheelZoom=@enableScrollWheelZoom,");
            strSql.Append("NavigationControl=@NavigationControl,");
            strSql.Append("ScaleControl=@ScaleControl,");
            strSql.Append("MapTypeControl=@MapTypeControl,");
            strSql.Append("MarkersLongitude=@MarkersLongitude,");
            strSql.Append("MarkersDimension=@MarkersDimension,");
            strSql.Append("setAnimation=@setAnimation,");
            strSql.Append("LoadEvent=@LoadEvent,");
            strSql.Append("MenuItemzoomIn=@MenuItemzoomIn,");
            strSql.Append("MenuItemzoomOut=@MenuItemzoomOut,");
            strSql.Append("MenuItemsetZoomTop=@MenuItemsetZoomTop,");
            strSql.Append("MenuItemsetPoint=@MenuItemsetPoint,");
            strSql.Append("MapType=@MapType,");
            strSql.Append("Other1=@Other1,");
            strSql.Append("Other2=@Other2,");
            strSql.Append("Other3=@Other3");
            strSql.Append(" where MapId=@MapId");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserId", SqlDbType.Int,4),
                    new SqlParameter("@DepartmentId", SqlDbType.Int,4),
                    new SqlParameter("@PointerLongitude", SqlDbType.NVarChar,100),
                    new SqlParameter("@PointDimension", SqlDbType.NVarChar,100),
                    new SqlParameter("@PointClass", SqlDbType.NVarChar,100),
                    new SqlParameter("@PointerType", SqlDbType.NVarChar,100),
                    new SqlParameter("@PointerTitle", SqlDbType.NVarChar,100),
                    new SqlParameter("@PointImg", SqlDbType.NVarChar,100),
                    new SqlParameter("@PointerContent", SqlDbType.NVarChar,300),
                    new SqlParameter("@SearchCity", SqlDbType.NVarChar,50),
                    new SqlParameter("@searchArea", SqlDbType.NVarChar,50),
                    new SqlParameter("@Level", SqlDbType.Int,4),
                    new SqlParameter("@enableKeyboard", SqlDbType.Bit,1),
                    new SqlParameter("@enableScrollWheelZoom", SqlDbType.Bit,1),
                    new SqlParameter("@NavigationControl", SqlDbType.Bit,1),
                    new SqlParameter("@ScaleControl", SqlDbType.Bit,1),
                    new SqlParameter("@MapTypeControl", SqlDbType.Bit,1),
                    new SqlParameter("@MarkersLongitude", SqlDbType.NVarChar,100),
                    new SqlParameter("@MarkersDimension", SqlDbType.NVarChar,100),
                    new SqlParameter("@setAnimation", SqlDbType.NVarChar,100),
                    new SqlParameter("@LoadEvent", SqlDbType.NVarChar,50),
                    new SqlParameter("@MenuItemzoomIn", SqlDbType.Bit,1),
                    new SqlParameter("@MenuItemzoomOut", SqlDbType.Bit,1),
                    new SqlParameter("@MenuItemsetZoomTop", SqlDbType.Bit,1),
                    new SqlParameter("@MenuItemsetPoint", SqlDbType.Bit,1),
                    new SqlParameter("@MapType", SqlDbType.SmallInt,2),
                    new SqlParameter("@Other1", SqlDbType.NVarChar,100),
                    new SqlParameter("@Other2", SqlDbType.NVarChar,100),
                    new SqlParameter("@Other3", SqlDbType.NVarChar,50),
                    new SqlParameter("@MapId", SqlDbType.Int,4)};
            parameters[0].Value = model.UserId;
            parameters[1].Value = model.DepartmentId;
            parameters[2].Value = model.PointerLongitude;
            parameters[3].Value = model.PointDimension;
            parameters[4].Value = model.PointClass;
            parameters[5].Value = model.PointerType;
            parameters[6].Value = model.PointerTitle;
            parameters[7].Value = model.PointImg;
            parameters[8].Value = model.PointerContent;
            parameters[9].Value = model.SearchCity;
            parameters[10].Value = model.searchArea;
            parameters[11].Value = model.Level;
            parameters[12].Value = model.enableKeyboard;
            parameters[13].Value = model.enableScrollWheelZoom;
            parameters[14].Value = model.NavigationControl;
            parameters[15].Value = model.ScaleControl;
            parameters[16].Value = model.MapTypeControl;
            parameters[17].Value = model.MarkersLongitude;
            parameters[18].Value = model.MarkersDimension;
            parameters[19].Value = model.setAnimation;
            parameters[20].Value = model.LoadEvent;
            parameters[21].Value = model.MenuItemzoomIn;
            parameters[22].Value = model.MenuItemzoomOut;
            parameters[23].Value = model.MenuItemsetZoomTop;
            parameters[24].Value = model.MenuItemsetPoint;
            parameters[25].Value = model.MapType;
            parameters[26].Value = model.Other1;
            parameters[27].Value = model.Other2;
            parameters[28].Value = model.Other3;
            parameters[29].Value = model.MapId;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int MapId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Ms_Maps ");
            strSql.Append(" where MapId=@MapId");
            SqlParameter[] parameters = {
                    new SqlParameter("@MapId", SqlDbType.Int,4)
};
            parameters[0].Value = MapId;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string MapIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Ms_Maps ");
            strSql.Append(" where MapId in (" + MapIdlist + ")  ");
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.Map.Model.MapInfo GetModel(int MapId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 MapId,UserId,DepartmentId,PointerLongitude,PointDimension,PointClass,PointerType,PointerTitle,PointImg,PointerContent,SearchCity,searchArea,Level,enableKeyboard,enableScrollWheelZoom,NavigationControl,ScaleControl,MapTypeControl,MarkersLongitude,MarkersDimension,setAnimation,LoadEvent,MenuItemzoomIn,MenuItemzoomOut,MenuItemsetZoomTop,MenuItemsetPoint,MapType,Other1,Other2,Other3 from Ms_Maps ");
            strSql.Append(" where MapId=@MapId");
            SqlParameter[] parameters = {
                    new SqlParameter("@MapId", SqlDbType.Int,4)
};
            parameters[0].Value = MapId;
            return GetMapInfo(strSql, parameters);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select MapId,UserId,DepartmentId,PointerLongitude,PointDimension,PointClass,PointerType,PointerTitle,PointImg,PointerContent,SearchCity,searchArea,Level,enableKeyboard,enableScrollWheelZoom,NavigationControl,ScaleControl,MapTypeControl,MarkersLongitude,MarkersDimension,setAnimation,LoadEvent,MenuItemzoomIn,MenuItemzoomOut,MenuItemsetZoomTop,MenuItemsetPoint,MapType,Other1,Other2,Other3 ");
            strSql.Append(" FROM Ms_Maps ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" MapId,UserId,DepartmentId,PointerLongitude,PointDimension,PointClass,PointerType,PointerTitle,PointImg,PointerContent,SearchCity,searchArea,Level,enableKeyboard,enableScrollWheelZoom,NavigationControl,ScaleControl,MapTypeControl,MarkersLongitude,MarkersDimension,setAnimation,LoadEvent,MenuItemzoomIn,MenuItemzoomOut,MenuItemsetZoomTop,MenuItemsetPoint,MapType,Other1,Other2,Other3 ");
            strSql.Append(" FROM Ms_Maps ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM Ms_Maps ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperSQL.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.MapId desc");
            }
            strSql.Append(")AS Row, T.*  from Ms_Maps T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.Query(strSql.ToString());
        }

        #endregion  Method
    }
}

