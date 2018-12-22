/**
* ShippingAddress.cs
*
* 功 能： N/A
* 类 名： ShippingAddress
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/4/27 10:24:44   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Collections.Generic;
using YSWL.Common;
using YSWL.MALL.Model.Shop.Shipping;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Shop.Shipping;
namespace YSWL.MALL.BLL.Shop.Shipping
{
    /// <summary>
    /// ShippingAddress
    /// </summary>
    public partial class ShippingAddress
    {
        private readonly IShippingAddress dal = DAShopShipping.CreateShippingAddress();
        public ShippingAddress()
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
        public bool Exists(int ShippingId)
        {
            return dal.Exists(ShippingId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.MALL.Model.Shop.Shipping.ShippingAddress model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.Shop.Shipping.ShippingAddress model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ShippingId)
        {

            return dal.Delete(ShippingId);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string ShippingIdlist)
        {
            return dal.DeleteList(Common.Globals.SafeLongFilter(ShippingIdlist,0) );
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public YSWL.MALL.Model.Shop.Shipping.ShippingAddress GetModelByCache(int ShippingId)
        {

            string CacheKey = "ShippingAddressModel-" + ShippingId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(ShippingId);
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.Shop.Shipping.ShippingAddress)objModel;
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
        public List<YSWL.MALL.Model.Shop.Shipping.ShippingAddress>  GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
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

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.Shipping.ShippingAddress GetModel(int ShippingId)
        {
            BLL.Ms.Regions regionsManage = new Ms.Regions();
            YSWL.MALL.Model.Shop.Shipping.ShippingAddress model = dal.GetModel(ShippingId);
            if (model != null)
            {
                //加载区域完整名
                model.RegionFullName = regionsManage.GetFullNameById4Cache(model.RegionId);
            }
            return model;
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Shipping.ShippingAddress> DataTableToList(DataTable dt)
        {
            BLL.Ms.Regions regionsManage = new Ms.Regions();
            List<YSWL.MALL.Model.Shop.Shipping.ShippingAddress> modelList = new List<YSWL.MALL.Model.Shop.Shipping.ShippingAddress>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Shop.Shipping.ShippingAddress model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = dal.DataRowToModel(dt.Rows[n]);
                    if (model != null)
                    {
                        //加载区域完整名
                        model.RegionFullName = regionsManage.GetFullNameById4Cache(model.RegionId);
                        modelList.Add(model);
                    }
                }
            }
            return modelList;
        }

        public List<YSWL.MALL.Model.Shop.Shipping.ShippingAddress> GetListByPageEx(string strWhere, string orderby, int startIndex, int endIndex)
        {
            DataSet ds = GetListByPage(strWhere, orderby, startIndex, endIndex);
            return DataTableToList(ds.Tables[0]);
        }


       public List<YSWL.MALL.Model.Shop.Shipping.ShippingAddress> GetAddressBySupplier(int supplierId)
       {
           if (supplierId < 1)
           {
               return null;
           }
           DataSet ds = dal.GetAddressBySupplier(supplierId);
           if (!DataSetTools.DataSetIsNull(ds))
           {
               return DataTableToList(ds.Tables[0]);
           }
           return null;
       }
        /// <summary>
        /// 设置默认收货地址
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ShippingId"></param>
        /// <returns></returns>
        public bool SetDefaultShipAddress(int UserId, int ShippingId)
        {
            return dal.SetDefaultShipAddress(UserId, ShippingId);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Shipping.ShippingAddress> GetModelList(int Top, string strWhere, string filedOrder)
        {
            DataSet ds = dal.GetList(Top, strWhere, filedOrder);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.Shipping.ShippingAddress GetModelByUserId(int userId)
        {
            YSWL.MALL.Model.Shop.Shipping.ShippingAddress model  =dal.GetModelByUserId(userId);
            if (model != null)
            {
                BLL.Ms.Regions regionsManage = new Ms.Regions();
                //加载区域完整名
                model.RegionFullName = regionsManage.GetFullNameById4Cache(model.RegionId);
            }
            return model;
        }

        public bool UpdateMapInfo(int userId, decimal latitude, decimal longitude, string address)
        {
            return dal.UpdateMapInfo(userId, latitude, longitude, address);
        }

        /// <summary>
        /// 更新一条数据(如果收货地址id小于0则添加数据)
        /// </summary>
        public bool UpdateEx(YSWL.MALL.Model.Shop.Shipping.ShippingAddress model)
        {
            bool result;
            if (model.ShippingId > 0)
            {
                result = dal.Update(model);
                Common.DataCache.DeleteCache("ShippingAddressModel-" + model.ShippingId);
            }
            else
            {
                result = dal.Add(model) > 0;
            }
            return result;
        }

        #region 获取默认的收货地址RegionID
        public int GetDefaultRegionId(int userId)
        {
            string CacheKey = "GetDefaultRegionId-" + userId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    YSWL.MALL.Model.Shop.Shipping.ShippingAddress model = GetModelByUserId(userId);
                    objModel = model == null ? 0 : model.RegionId;
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30000);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return Common.Globals.SafeInt(objModel,0);
        }
        #endregion 

        /// <summary>
        /// 默认地址在列表最前面
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<YSWL.MALL.Model.Shop.Shipping.ShippingAddress> GetDefaultModelList(int userId)
        {   
            DataSet ds = dal.GetList("UserId="+ userId + " order by IsDefault desc");
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 是否存在默认收货地址
        /// </summary>
        public bool ExistsDefaultAddress(int userId)
        {
            return dal.ExistsDefaultAddress(userId);
        }
        #endregion  ExtensionMethod
    }
}

