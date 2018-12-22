/**
* ShippingType.cs
*
* 功 能： N/A
* 类 名： ShippingType
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/4/27 10:24:45   N/A    初版
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
using System.Text;
namespace YSWL.MALL.BLL.Shop.Shipping
{
    /// <summary>
    /// ShippingType
    /// </summary>
    public partial class ShippingType
    {
        private readonly IShippingType dal = DAShopShipping.CreateShippingType();
        public ShippingType()
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
        public bool Exists(int ModeId)
        {
            return dal.Exists(ModeId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.MALL.Model.Shop.Shipping.ShippingType model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.Shop.Shipping.ShippingType model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ModeId)
        {

            return dal.Delete(ModeId);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string ModeIdlist)
        {
            return dal.DeleteList(Common.Globals.SafeLongFilter(ModeIdlist, 0));
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.Shipping.ShippingType GetModel(int ModeId)
        {

            return dal.GetModel(ModeId);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public YSWL.MALL.Model.Shop.Shipping.ShippingType GetModelByCache(int ModeId)
        {

            string CacheKey = "ShippingTypeModel-" + ModeId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(ModeId);
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.Shop.Shipping.ShippingType)objModel;
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
        public List<YSWL.MALL.Model.Shop.Shipping.ShippingType> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Shipping.ShippingType> DataTableToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Shop.Shipping.ShippingType> modelList = new List<YSWL.MALL.Model.Shop.Shipping.ShippingType>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Shop.Shipping.ShippingType model;
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
     
        /// <summary>
        /// 根据支付方式获取对应物流
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Shipping.ShippingType> GetListByPay(int paymentModeId)
        {
            System.Text.StringBuilder sql = new System.Text.StringBuilder();
            sql.AppendFormat(@"EXISTS ( SELECT ShippingModeId
                 FROM   Shop_ShippingPayment
                 WHERE  ShippingModeId = Shop_ShippingType.ModeId
                        AND PaymentModeId = {0} )", paymentModeId);
            return GetModelList(sql.ToString());
        }
        /// <summary>
        /// 根据支付方式获取对应物流
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Shipping.ShippingType> GetListByPay(int paymentModeId,int suppId)
        {
            string suppStr = "";
            if (suppId > 0)
            {
                suppStr = string.Format(" SupplierId = {0} ", suppId);
            }
            else {
                suppStr = " SupplierId <=0 ";
            }
            System.Text.StringBuilder sql = new System.Text.StringBuilder();
            sql.AppendFormat(@"EXISTS ( SELECT ShippingModeId
                 FROM   Shop_ShippingPayment
                 WHERE  ShippingModeId = Shop_ShippingType.ModeId
                        AND PaymentModeId = {0} AND {1} )", paymentModeId, suppStr);
            return GetModelList(sql.ToString());
        }
     

        /// <summary>
        /// 获取运费
        /// </summary>
        public decimal GetFreight(YSWL.MALL.Model.Shop.Shipping.ShippingType typeModel, int weight)
        {
            if (weight <= typeModel.Weight)
            {
                return typeModel.Price;
            }
            else
            {
                if (!typeModel.AddWeight.HasValue || typeModel.AddWeight.Value <= 0 || !typeModel.AddPrice.HasValue ||
                    typeModel.AddPrice.Value < 0)
                {
                    return typeModel.Price;
                }
                int addWeight = weight - typeModel.Weight;
                int addStep = addWeight % typeModel.AddWeight == 0 ? addWeight / typeModel.AddWeight.Value : (addWeight / typeModel.AddWeight.Value) + 1;
                return typeModel.Price + addStep * typeModel.AddPrice.Value;
            }
        }

        public YSWL.MALL.Model.Shop.Shipping.ShippingType GetCacgeModelByShipId(int shippingId)
        {
            if (shippingId < 1) return null;
            string CacheKey = "ShippingTypeModel-" + shippingId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetShippingTypeByAddress(shippingId);
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.Shop.Shipping.ShippingType)objModel;
        }

        /// <summary>
        /// 根据用户获取配送类型
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public YSWL.MALL.Model.Shop.Shipping.ShippingType GetModelByUser(int userId)
        {
            return dal.GetModelByUser(userId);
        }
       /// <summary>
        /// 获得前几行数据
       /// </summary>
       /// <param name="Top"></param>
       /// <param name="name">配送方式名称</param>
       /// <param name="supplierId">商家Id</param>
       /// <param name="paymentId">支付方式Id</param>
       /// <param name="filedOrder">排序方式</param>
       /// <returns></returns>
        public DataSet GetList(int Top, string name,int supplierId,int paymentId, string filedOrder)
        {
            StringBuilder strWhere = new StringBuilder();
            #region 配送方式名称
            if (!String.IsNullOrWhiteSpace(name))
            {
                if (strWhere.Length > 0)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" Name  like  '%{0}%' ", name);
            }
            #endregion
           #region 商家
            if (supplierId==0) {
                if (strWhere.Length > 0)
                {
                    strWhere.Append(" and ");
                }
                strWhere.Append(" supplierId  <= 0 ");
            }
            if (supplierId>0)
            {
                if (strWhere.Length > 0)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" supplierId  = {0} ", supplierId);
            }
            #endregion

            #region 支付方式
            if (paymentId > 0)
            {
                if (strWhere.Length > 0)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" EXISTS ( SELECT * FROM  Shop_ShippingPayment AS shippay WHERE shippay.PaymentModeId={0} and shippay.ShippingModeId =ship.ModeId ) ", paymentId);     
            }
            #endregion
               
            return dal.GetList(Top, strWhere.ToString(), filedOrder);
        }

        /// <summary>
        /// 获得配送信息
        /// </summary>
        /// <param name="supplierId">商家id</param>
        /// <returns></returns>
        public YSWL.MALL.Model.Shop.Shipping.ShippingType GetModelBySupplied(int supplierId)
        {
            return dal.GetModelBySupplied(supplierId);
        }

        /// <summary>
        /// 从缓存中根据supplierId获取model
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public YSWL.MALL.Model.Shop.Shipping.ShippingType GetCacgeModelSupplied(int supplierId)
        {
            if (supplierId < 1)
            {
                supplierId = 0;
            }
            string CacheKey = "ShippingTypeModelSupplier-" + supplierId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModelBySupplied(supplierId);
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.Shop.Shipping.ShippingType)objModel;
        }

        /// <summary>
        /// 更新model并删除缓存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateModelBySupplied(YSWL.MALL.Model.Shop.Shipping.ShippingType model)
        {
            string CacheKey = "ShippingTypeModelSupplier-" + model.SupplierId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel != null)
            {
                YSWL.Common.DataCache.DeleteCache(CacheKey);
            }
            return dal.Update(model);
        }

        /// <summary>
        /// 从缓存中根据supplierId获取配送model(未设置商家配送则为平台配送)
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public YSWL.MALL.Model.Shop.Shipping.ShippingType GetCacgeModelBySupplied(int supplierId)
        {
            if (supplierId > 0)
            {
                bool IsSuppShip = YSWL.MALL.BLL.Shop.Supplier.SupplierConfig.GetIntValueByCache(supplierId, "ShipType") == 1;//是否开启商家配送
                if (!IsSuppShip)//未设置配送方式默认为平台
                {
                    supplierId = 0;
                }
            }
           
            string CacheKey = "ShippingTypeModelSupplier-" + supplierId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {

                    objModel = dal.GetModelBySupplied(supplierId);
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.Shop.Shipping.ShippingType)objModel;
        }


        public List<YSWL.MALL.Model.Shop.Shipping.ShippingType> GetListBySupplied(int supplierId)
        {
            if (supplierId > 0)
            {
                bool IsSuppShip = YSWL.MALL.BLL.Shop.Supplier.SupplierConfig.GetIntValueByCache(supplierId, "ShipType") == 1;//是否开启商家配送
                if (!IsSuppShip)//未设置配送方式默认为平台
                {
                    supplierId = 0;
                }
            }

            string CacheKey = "GetListBySupplied-" + supplierId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {

                    DataSet ds = dal.GetListBySupplied(supplierId);
                    objModel=DataTableToList(ds.Tables[0]);
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (List<YSWL.MALL.Model.Shop.Shipping.ShippingType>)objModel;
        }

        #endregion  ExtensionMethod
    }
}

