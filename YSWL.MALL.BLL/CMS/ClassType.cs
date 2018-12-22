using System;
using System.Data;

using System.Collections.Generic;
using YSWL.Common;
using YSWL.MALL.Model.CMS;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.CMS;

namespace YSWL.MALL.BLL.CMS
{
	/// <summary>
	/// 栏目类型
	/// </summary>
	public partial class ClassType
	{        
        private readonly IClassType dal = DataAccess<IClassType>.Create("CMS.ClassType");
		
		#region  Method

        #region 得到最大ID
        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return dal.GetMaxId();
        } 
        #endregion

        #region 是否存在该记录
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ClassTypeID)
        {
            return dal.Exists(ClassTypeID);
        } 
        #endregion

        #region 增加一条数据
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(YSWL.MALL.Model.CMS.ClassType model)
        {
            return dal.Add(model);
        } 
        #endregion

        #region 更新一条数据
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.CMS.ClassType model)
        {
            return dal.Update(model);
        } 
        #endregion

        #region 删除一条数据
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ClassTypeID)
        {

            return dal.Delete(ClassTypeID);
        } 
        #endregion

        #region 批量删除数据
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string ClassTypeIDlist)
        {
            return dal.DeleteList(Common.Globals.SafeLongFilter(ClassTypeIDlist,0) );
        } 
        #endregion

        #region 得到一个对象实体
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.CMS.ClassType GetModel(int ClassTypeID)
        {
            return dal.GetModel(ClassTypeID);
        } 
        #endregion

        #region 从缓存中得到一个对象实体
        /// <summary>
        /// 从缓存中得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.CMS.ClassType GetModelByCache(int ClassTypeID)
        {
            string CacheKey = "ClassTypeModel-" + ClassTypeID;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(ClassTypeID);
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.CMS.ClassType)objModel;
        } 
        #endregion

        #region 获得数据列表

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        } 
        #endregion

        #region 获得数据列表
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        } 
        #endregion

        #region 获得前几行数据
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        } 
        #endregion

        #region 获得数据列表
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.CMS.ClassType> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            if (DataSetTools.DataSetIsNull(ds))
            {
                return null;
            }
            return dal.DataTableToList(ds.Tables[0]);
        } 
        #endregion

       

        #region 分页获取数据列表
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //return dal.GetList(PageSize,PageIndex,strWhere);
        //} 
        #endregion

		#endregion  Method
	}
}

