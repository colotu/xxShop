/*----------------------------------------------------------------
// Copyright (C) 2012 云商未来 版权所有。
//
// 文件名：AdvertisePosition.cs
// 文件功能描述：
// 
// 创建标识： [孙鹏]  2012/05/31 13:22:19
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/
using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using YSWL.Common;
using YSWL.MALL.Model.Settings;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Settings;
namespace YSWL.MALL.BLL.Settings
{
    /// <summary>
    /// 广告位管理
    /// </summary>
    public partial class AdvertisePosition
    {
        private readonly IAdvertisePosition dal = DASettings.CreateAdvertisePosition();
        public AdvertisePosition()
        { }
        #region  Method

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.MALL.Model.Settings.AdvertisePosition model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int AdvPositionId)
        {
            return dal.Exists(AdvPositionId);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.Settings.AdvertisePosition model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int AdvPositionId)
        {

            return dal.Delete(AdvPositionId);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string AdvPositionIdlist)
        {
            return dal.DeleteList(Common.Globals.SafeLongFilter(AdvPositionIdlist, 0));
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Settings.AdvertisePosition GetModel(int AdvPositionId)
        {

            return dal.GetModel(AdvPositionId);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public YSWL.MALL.Model.Settings.AdvertisePosition GetModelByCache(int AdvPositionId)
        {

            string CacheKey = "AdvertisePositionModel-" + AdvPositionId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(AdvPositionId);
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.Settings.AdvertisePosition)objModel;
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
        public List<YSWL.MALL.Model.Settings.AdvertisePosition> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Settings.AdvertisePosition> DataTableToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Settings.AdvertisePosition> modelList = new List<YSWL.MALL.Model.Settings.AdvertisePosition>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Settings.AdvertisePosition model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new YSWL.MALL.Model.Settings.AdvertisePosition();
                    if (dt.Rows[n]["AdvPositionId"] != null && dt.Rows[n]["AdvPositionId"].ToString() != "")
                    {
                        model.AdvPositionId = int.Parse(dt.Rows[n]["AdvPositionId"].ToString());
                    }
                    if (dt.Rows[n]["AdvPositionName"] != null && dt.Rows[n]["AdvPositionName"].ToString() != "")
                    {
                        model.AdvPositionName = dt.Rows[n]["AdvPositionName"].ToString();
                    }
                    if (dt.Rows[n]["ShowType"] != null && dt.Rows[n]["ShowType"].ToString() != "")
                    {
                        model.ShowType = int.Parse(dt.Rows[n]["ShowType"].ToString());
                    }
                    if (dt.Rows[n]["RepeatColumns"] != null && dt.Rows[n]["RepeatColumns"].ToString() != "")
                    {
                        model.RepeatColumns = int.Parse(dt.Rows[n]["RepeatColumns"].ToString());
                    }
                    if (dt.Rows[n]["Width"] != null && dt.Rows[n]["Width"].ToString() != "")
                    {
                        model.Width = int.Parse(dt.Rows[n]["Width"].ToString());
                    }
                    if (dt.Rows[n]["Height"] != null && dt.Rows[n]["Height"].ToString() != "")
                    {
                        model.Height = int.Parse(dt.Rows[n]["Height"].ToString());
                    }
                    if (dt.Rows[n]["AdvHtml"] != null && dt.Rows[n]["AdvHtml"].ToString() != "")
                    {
                        model.AdvHtml = dt.Rows[n]["AdvHtml"].ToString();
                    }
                    if (dt.Rows[n]["IsOne"] != null && dt.Rows[n]["IsOne"].ToString() != "")
                    {
                        if ((dt.Rows[n]["IsOne"].ToString() == "1") || (dt.Rows[n]["IsOne"].ToString().ToLower() == "true"))
                        {
                            model.IsOne = true;
                        }
                        else
                        {
                            model.IsOne = false;
                        }
                    }
                    if (dt.Rows[n]["TimeInterval"] != null && dt.Rows[n]["TimeInterval"].ToString() != "")
                    {
                        model.TimeInterval = int.Parse(dt.Rows[n]["TimeInterval"].ToString());
                    }
                    if (dt.Rows[n]["CreatedDate"] != null && dt.Rows[n]["CreatedDate"].ToString() != "")
                    {
                        model.CreatedDate = DateTime.Parse(dt.Rows[n]["CreatedDate"].ToString());
                    }
                    if (dt.Rows[n]["CreatedUserID"] != null && dt.Rows[n]["CreatedUserID"].ToString() != "")
                    {
                        model.CreatedUserID = int.Parse(dt.Rows[n]["CreatedUserID"].ToString());
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

        #region saas app接口方法
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Settings.AdvertisePosition> GetListByPageApp(string avdName, int? page = 1, int pageNum = 30)
        {
            if (!page.HasValue || page <= 0)
            {
                page = 1;
            }
            int startIndex = (page.Value - 1) * pageNum + 1;
            int endIndex = startIndex + pageNum - 1;
            StringBuilder strWhere = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(avdName))
            {
                strWhere.AppendFormat("AdvPositionName like '%{0}%'", Common.InjectionFilter.SqlFilter(avdName));
            }
            DataSet ds = dal.GetListByPage(strWhere.ToString(), " AdvPositionId desc", startIndex, endIndex);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 编辑广告位信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateAdvPostion(ViewModel.AdvertisePositionVm model)
        {
            return dal.UpdateAdvPostion(model);
        }
        #endregion
    }
}

