using System;
using System.Data;
using System.Collections.Generic;
using YSWL.Common;
using YSWL.MALL.Model.Ms;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Ms;
using System.Text;
namespace YSWL.MALL.BLL.Ms
{
    /// <summary>
    /// Regions
    /// </summary>
    public partial class Regions
    {
        private readonly IRegions dal = DAMs.CreateRegions();

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
        public bool Exists(int RegionId)
        {
            return dal.Exists(RegionId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.MALL.Model.Ms.Regions model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.Ms.Regions model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int RegionId)
        {

            return dal.Delete(RegionId);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string RegionIdlist)
        {
            return dal.DeleteList(Common.Globals.SafeLongFilter(RegionIdlist,0) );
        }


        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Ms.Regions> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Ms.Regions> DataTableToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Ms.Regions> modelList = new List<YSWL.MALL.Model.Ms.Regions>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Ms.Regions model;
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

        #region NewMethod
        /// <summary>
        /// 获取省份数据
        /// </summary>
        /// <returns></returns>
        public DataSet GetProvinces()
        {
            return dal.GetProvinces();
        }

        /// <summary>
        /// 获取城市数据
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        public DataSet GetCitys(int parentID)
        {
            return dal.GetCitys(parentID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Ms.Regions GetModel(int RegionId)
        {
            return dal.GetModel(RegionId);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public YSWL.MALL.Model.Ms.Regions GetModelByCache(int RegionId)
        {
            string CacheKey = "RegionsModel-" + RegionId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(RegionId);
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.Ms.Regions)objModel;
        }

        /// <summary>
        /// 获取省份信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetPrivoces()
        {
            return dal.GetPrivoces();

        }

        /// <summary>
        /// 获取省份信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetPrivoceName()
        {
            return dal.GetPrivoceName();
        }
        /// <summary>
        /// 根据省份获取城市
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        public DataSet GetRegionName(string parentID)
        {
            return dal.GetRegionName(parentID);
        }

        /// <summary>
        /// 获取读取父Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetParentId(int id)
        {
            return dal.GetParentID(id);
        }

        public DataTable GetDistrictByParentId(int iParentId)
        {
            return dal.GetDistrictByParentId(iParentId).Tables[0];
        }

        public DataSet GetDisByParentId(int iParentId)
        {
            return dal.GetDistrictByParentId(iParentId);
        }

        public DataSet GetAllCityList()
        {
            DataSet ds = DataCache.GetCache("Maticsoft_CityList") as DataSet;
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                ds = dal.GetAllCityList();
                DataCache.SetCache("Maticsoft_CityList", ds);
            }
            return ds;
        }

        public int GetRegPath(int? regid)
        {
            return dal.GetRegPath(regid);
        }

        /// <summary>
        /// 获取完整省市区名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetNameListById4Cache(int id)
        {
            string cacheKey = "GetFullNameById4Cache-" + id;
            object objModel = YSWL.Common.DataCache.GetCache(cacheKey);
            if (objModel == null)
            {
                try
                {
                    List<string> tmp = dal.GetRegionNameByRID(id);
                    if (tmp != null && tmp.Count > 0)
                    {
                        objModel = tmp;
                        int modelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(cacheKey, tmp, DateTime.Now.AddMinutes(modelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return objModel as List<string>;
        }

        /// <summary>
        /// 获取完整省市区名称
        /// </summary>
        /// <returns></returns>
        public string GetFullNameById4Cache(int id)
        {
            List<string> tmp = GetNameListById4Cache(id);
            if (tmp != null && tmp.Count > 0)
            {
                return string.Join("", tmp);
            }
            return string.Empty;
        }

        /// <summary>
        /// 根据RID获取地域全名
        /// </summary>
        public string GetRegionNameByRID(int RID)
        {
            return string.Join("", dal.GetRegionNameByRID(RID));
        }


        public DataSet GetParentIDs(int regID, out int Count)
        {
            return dal.GetParentIDs(regID, out Count);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }
        public List<YSWL.MALL.Model.Ms.Regions> GetListInfo(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(int RegionId)
        {
            return dal.GetList("RegionId=" + RegionId + " or Path like '0," + RegionId + "%'");
        }
        /// <summary>
        /// 获取省份数据
        /// </summary>
        /// <returns></returns>
        public List<YSWL.MALL.Model.Ms.Regions> GetProvinceList()
        {
            string CacheKey = "GetProvinceList-ProvinceList";
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    DataSet ds = dal.GetPrivoces();
                    objModel = DataTableToList(ds.Tables[0]);
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (List<YSWL.MALL.Model.Ms.Regions>)objModel;

        }
     
        /// <summary>
        /// 更新多条数据的AreaID
        /// </summary>
        public bool UpdateAreaID(string regionlist, int AreaId)
        {
            return dal.UpdateAreaID(regionlist, AreaId);
        }

        /// <summary>
        /// 根据AreaID获取到Regionids
        /// </summary>
        /// <param name="areaid"></param>
        /// <returns></returns>
        public string GetRegionIDsByAreaId(int areaid)
        {
            //string s = dal.GetRegionIDsByAreaId(areaid);
            //if (s.Length > 0)
            //{
            //    s = "[" + s + "]";
            //}
            return dal.GetRegionIDsByAreaId(areaid);
        }

        /// <summary>
        /// 获取地域全名并将去除重复的词 
        /// </summary>
        /// <param name="address">userModel.Address</param>
        /// <returns></returns>
        public string GetAddress(int? RID)
        {
            if (!RID.HasValue)
            {
                return "暂未设置";
            }
            string strAddress = GetFullNameById4Cache(RID.Value);
            if (String.IsNullOrWhiteSpace(strAddress))
            {
                return "暂未设置";
            }
            if (strAddress.Contains("北京北京"))
            {
                return strAddress.Replace("北京北京", "北京");
            }
            else if (strAddress.Contains("上海上海"))
            {
                return strAddress.Replace("上海上海", "上海");
            }
            else if (strAddress.Contains("重庆重庆"))
            {
                return strAddress.Replace("重庆重庆", "重庆");
            }
            else if (strAddress.Contains("天津天津"))
            {
                return strAddress.Replace("天津天津", "天津");
            }
            else
            {
                return strAddress;
            }
        }
        /// <summary>
        /// 获取地域全名并将去除重复的词 
        /// </summary>
        /// <param name="address">userModel.Address</param>
        /// <returns></returns>
        public string GetRegionFullName(int? RID)
        {
            if (!RID.HasValue)
            {
                return "";
            }
            string strAddress = GetFullNameById4Cache(RID.Value);
            if (String.IsNullOrWhiteSpace(strAddress))
            {
                return "";
            }
            if (strAddress.Contains("北京北京"))
            {
                return strAddress.Replace("北京北京", "北京");
            }
            else if (strAddress.Contains("上海上海"))
            {
                return strAddress.Replace("上海上海", "上海");
            }
            else if (strAddress.Contains("重庆重庆"))
            {
                return strAddress.Replace("重庆重庆", "重庆");
            }
            else if (strAddress.Contains("天津天津"))
            {
                return strAddress.Replace("天津天津", "天津");
            }
            else
            {
                return strAddress;
            }
        }
        #endregion

        public List<Model.Ms.Regions> GetListDistrictByParentId(int parentId)
        {
            DataTable table = dal.GetDistrictByParentId(parentId).Tables[0];
            return DataTableToList(table);
        }
    
  public List<Model.Ms.Regions> GetSamePathArea(int reginoId)
        {
            DataTable table = dal.GetSamePathArea(reginoId).Tables[0];
            return DataTableToList(table);
        }
        public int GetCurrentParentId(int regionId)
        {
            return dal.GetCurrentParentId(regionId);
        }
        public bool IsParentRegion(int regionId)
        {
            return dal.IsParentRegion(regionId);
        }
        /// <summary>
        /// 判断收货地区是否包含在团购地区内
        /// </summary>
        /// <param name="gregoinId">团购地区regoinId</param>
        /// <param name="shippaddressrid">收货地址regoinId</param>
        /// <returns></returns>
        public bool Exists(int gregoinId,int shippaddressrid)
        { 
            //未指定团购地区，或者团购地区与收货地区相等
            if(gregoinId<=0 ||  gregoinId==shippaddressrid){
                return true;
            }
            YSWL.MALL.Model.Ms.Regions grModel=GetModelByCache(gregoinId);
            YSWL.MALL.Model.Ms.Regions shippaddModel=GetModelByCache(shippaddressrid);
            if(grModel==null && shippaddModel==null){
                return false;
            }
            if(grModel.Depth>=shippaddModel.Depth || grModel.Depth==3){
               return false;
            }
            string path="";
            path=string.Format("{0}{1},",grModel.Path,grModel.RegionId) ;
            //判断收货地区是否是团购地区的子节点
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("  path like  '{0}%' ",path);
            strSql.AppendFormat("  and  regionid ={0} ",shippaddressrid);
            return dal.Exists(strSql.ToString());
        }

    }
}

