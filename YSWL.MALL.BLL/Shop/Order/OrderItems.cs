using System;
using System.Data;
using System.Collections.Generic;
using YSWL.Common;
using YSWL.MALL.Model.Shop.Order;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Shop.Order;
namespace YSWL.MALL.BLL.Shop.Order
{
    /// <summary>
    /// OrderItem
    /// </summary>
    public partial class OrderItems
    {
        private readonly IOrderItems dal = DAShopOrder.CreateOrderItem();
        public OrderItems()
        { }
        #region  BasicMethod
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long ItemId)
        {
            return dal.Exists(ItemId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public long Add(YSWL.MALL.Model.Shop.Order.OrderItems model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.Shop.Order.OrderItems model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(long ItemId)
        {

            return dal.Delete(ItemId);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string ItemIdlist)
        {
            return dal.DeleteList(ItemIdlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.Order.OrderItems GetModel(long ItemId)
        {

            return dal.GetModel(ItemId);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public YSWL.MALL.Model.Shop.Order.OrderItems GetModelByCache(long ItemId)
        {

            string CacheKey = "OrderItemsModel-" + ItemId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(ItemId);
                    if (objModel != null)
                    {
                        int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("CacheTime");
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.Shop.Order.OrderItems)objModel;
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
        public List<YSWL.MALL.Model.Shop.Order.OrderItems> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Order.OrderItems> DataTableToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Shop.Order.OrderItems> modelList = new List<YSWL.MALL.Model.Shop.Order.OrderItems>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Shop.Order.OrderItems model;
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
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Order.OrderItems> GetModelListByCache(string strWhere)
        {
            DataSet ds = GetListByCache(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetListByCache(string strWhere)
        {
            string CacheKey = "GetListByCache-" + strWhere;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetList(strWhere);
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (DataSet)objModel;
        }

        /// <summary>
        /// 获取销售记录
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="orderby"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public List<YSWL.MALL.ViewModel.Shop.SaleRecord> GetSaleRecordByPage(long productId, string orderby,
                                                                             int startIndex, int endIndex)
        {
            DataSet ds = dal.GetSaleRecordByPage(productId, orderby, startIndex, endIndex);
            return SaleRecordToList(ds.Tables[0]);
        }

        public List<YSWL.MALL.ViewModel.Shop.SaleRecord> SaleRecordToList(DataTable dt)
        {
            List<YSWL.MALL.ViewModel.Shop.SaleRecord> modelList = new List<YSWL.MALL.ViewModel.Shop.SaleRecord>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.ViewModel.Shop.SaleRecord model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new YSWL.MALL.ViewModel.Shop.SaleRecord();
                    if (dt.Rows[n]["BuyerName"] != null && dt.Rows[n]["BuyerName"].ToString() != "")
                    {
                        model.BuyName = dt.Rows[n]["BuyerName"].ToString();
                    }
                    if (dt.Rows[n]["ShipmentQuantity"] != null && dt.Rows[n]["ShipmentQuantity"].ToString() != "")
                    {
                        model.BuyCount = Common.Globals.SafeInt(dt.Rows[n]["ShipmentQuantity"].ToString(), 0);
                    }
                    if (dt.Rows[n]["CreatedDate"] != null && dt.Rows[n]["CreatedDate"].ToString() != "")
                    {
                        model.BuyDate = Common.Globals.SafeDateTime(dt.Rows[n]["CreatedDate"].ToString(), DateTime.Now);
                    }
                    if (dt.Rows[n]["SellPrice"] != null && dt.Rows[n]["SellPrice"].ToString() != "")
                    {
                        model.BuyPrice = Common.Globals.SafeDecimal(dt.Rows[n]["SellPrice"].ToString(), 0);
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }


        public int GetSaleRecordCount(long productId)
        {
            return dal.GetSaleRecordCount(productId);
        }

        public int GetOrderItemCountByOrderId(long orderId)
        {
            return dal.GetRecordCount(" OrderId=" + orderId);
        }

        public int GetOrderItemSumByOrderId(long orderId)
        {
            return dal.GetRecordSum(" OrderId=" + orderId);
        }

        public DataSet GetCommission(decimal DErate, decimal CPrate)
        {
            return dal.GetCommission(DErate, CPrate);
        }

        /// <summary>
        /// 根据商家Id获取售出商品总数
        /// </summary>
        public int GetRecordSum(int supplierId)
        {
            return dal.GetRecordSum(supplierId);
        }

        #endregion  ExtensionMethod

        //TODO: 暂时抓取实时数据,后期待优化
        #region  本月商品销售数量

        /// <summary>
        /// 本月商品销售数量
        /// </summary>
        /// <returns></returns>
        public List<YSWL.MALL.Model.Shop.BaseInfo> GetSaleResult()
      {
        List<YSWL.MALL.Model.Shop.BaseInfo> modelList = new List<YSWL.MALL.Model.Shop.BaseInfo>();
        DataSet ds = dal.GetSaleResult();
        if (ds != null && ds.Tables.Count > 0)
        {
          DataTable dt = ds.Tables[0];
        YSWL.MALL.Model.Shop.BaseInfo model = null;
          for (int i = 0; i < dt.Rows.Count; i++)
          {
            model = new YSWL.MALL.Model.Shop.BaseInfo();
            model.Name = dt.Rows[i]["Name"].ToString();
            model.Count = int.Parse(dt.Rows[i]["Count"].ToString());
            modelList.Add(model);
          }
        }
        return modelList;
      }

    #endregion

    #region  本月商品销售额排行

    /// <summary>
    /// 本月商品销售额排行
    /// </summary>
    /// <returns></returns>
    public List<YSWL.MALL.Model.Shop.BaseInfo> GetSaleAmountResult()
    {
      List<YSWL.MALL.Model.Shop.BaseInfo> modelList = new List<YSWL.MALL.Model.Shop.BaseInfo>();
      DataSet ds = dal.GetSaleAmountResult();
      if (ds != null && ds.Tables.Count > 0)
      {
        DataTable dt = ds.Tables[0];
        YSWL.MALL.Model.Shop.BaseInfo model = null;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
          model = new YSWL.MALL.Model.Shop.BaseInfo();
          model.Name = dt.Rows[i]["Name"].ToString();
          model.Amount = Math.Round(decimal.Parse(dt.Rows[i]["Amount"].ToString()),2);
          modelList.Add(model);
        }
      }
      return modelList;
    }

    #endregion
  }
}

