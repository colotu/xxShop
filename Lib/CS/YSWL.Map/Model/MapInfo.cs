using System;
namespace YSWL.Map.Model
{
    /// <summary>
    /// MapInfo:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class MapInfo
    {
        public MapInfo()
        { }
        #region Model
        private int _mapid;
        private int _userid;
        private int _departmentid;
        private string _pointerlongitude;
        private string _pointdimension;
        private string _pointclass;
        private string _pointertype;
        private string _pointertitle;
        private string _pointimg;
        private string _pointercontent;
        private string _searchcity;
        private string _searcharea;
        private int? _level;
        private bool? _enablekeyboard;
        private bool? _enablescrollwheelzoom;
        private bool? _navigationcontrol;
        private bool? _scalecontrol;
        private bool? _maptypecontrol;
        private string _markerslongitude;
        private string _markersDimension;
        private string _setanimation;
        private string _loadevent;
        private bool? _menuitemzoomin;
        private bool? _menuitemzoomout;
        private bool? _menuitemsetzoomtop;
        private bool? _menuitemsetpoint;
        private int _maptype;
        private string _other1;
        private string _other2;
        private string _other3;
        /// <summary>
        /// 
        /// </summary>
        public int MapId
        {
            set { _mapid = value; }
            get { return _mapid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int UserId
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int DepartmentId
        {
            set { _departmentid = value; }
            get { return _departmentid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PointerLongitude
        {
            set { _pointerlongitude = value; }
            get { return _pointerlongitude; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PointDimension
        {
            set { _pointdimension = value; }
            get { return _pointdimension; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PointClass
        {
            set { _pointclass = value; }
            get { return _pointclass; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PointerType
        {
            set { _pointertype = value; }
            get { return _pointertype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PointerTitle
        {
            set { _pointertitle = value; }
            get { return _pointertitle; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PointImg
        {
            set { _pointimg = value; }
            get { return _pointimg; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PointerContent
        {
            set { _pointercontent = value; }
            get { return _pointercontent; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SearchCity
        {
            set { _searchcity = value; }
            get { return _searchcity; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string searchArea
        {
            set { _searcharea = value; }
            get { return _searcharea; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Level
        {
            set { _level = value; }
            get { return _level; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? enableKeyboard
        {
            set { _enablekeyboard = value; }
            get { return _enablekeyboard; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? enableScrollWheelZoom
        {
            set { _enablescrollwheelzoom = value; }
            get { return _enablescrollwheelzoom; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? NavigationControl
        {
            set { _navigationcontrol = value; }
            get { return _navigationcontrol; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? ScaleControl
        {
            set { _scalecontrol = value; }
            get { return _scalecontrol; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? MapTypeControl
        {
            set { _maptypecontrol = value; }
            get { return _maptypecontrol; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MarkersLongitude
        {
            set { _markerslongitude = value; }
            get { return _markerslongitude; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MarkersDimension
        {
            set { _markersDimension = value; }
            get { return _markersDimension; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string setAnimation
        {
            set { _setanimation = value; }
            get { return _setanimation; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LoadEvent
        {
            set { _loadevent = value; }
            get { return _loadevent; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? MenuItemzoomIn
        {
            set { _menuitemzoomin = value; }
            get { return _menuitemzoomin; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? MenuItemzoomOut
        {
            set { _menuitemzoomout = value; }
            get { return _menuitemzoomout; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? MenuItemsetZoomTop
        {
            set { _menuitemsetzoomtop = value; }
            get { return _menuitemsetzoomtop; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? MenuItemsetPoint
        {
            set { _menuitemsetpoint = value; }
            get { return _menuitemsetpoint; }
        }
        /// <summary>
        /// 地图类型 0:百度地图 1:Google地图
        /// </summary>
        public int MapType
        {
            set { _maptype = value; }
            get { return _maptype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Other1
        {
            set { _other1 = value; }
            get { return _other1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Other2
        {
            set { _other2 = value; }
            get { return _other2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Other3
        {
            set { _other3 = value; }
            get { return _other3; }
        }
        #endregion Model

    }
}

