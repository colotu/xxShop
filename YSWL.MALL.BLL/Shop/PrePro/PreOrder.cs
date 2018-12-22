/**  版本信息模板在安装目录下，可自行修改。
* PreOrder.cs
*
* 功 能： N/A
* 类 名： PreOrder
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/8/24 16:08:39   N/A    初版
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
using YSWL.MALL.Model.Shop.PrePro;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Shop.PrePro;
namespace YSWL.MALL.BLL.Shop.PrePro
{
	/// <summary>
	/// 预定订单
	/// </summary>
	public partial class PreOrder
	{
        private readonly IPreOrder dal = DAShopPrePro.CreatePreOrder();
		public PreOrder()
		{}
        #region  BasicMethod
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long PreOrderId)
        {
            return dal.Exists(PreOrderId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public long Add(YSWL.MALL.Model.Shop.PrePro.PreOrder model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.Shop.PrePro.PreOrder model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(long PreOrderId)
        {

            return dal.Delete(PreOrderId);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string PreOrderIdlist)
        {
            return dal.DeleteList(PreOrderIdlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.PrePro.PreOrder GetModel(long PreOrderId)
        {

            return dal.GetModel(PreOrderId);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public YSWL.MALL.Model.Shop.PrePro.PreOrder GetModelByCache(long PreOrderId)
        {

            string CacheKey = "PreOrderModel-" + PreOrderId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(PreOrderId);
                    if (objModel != null)
                    {
                        int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("CacheTime");
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.Shop.PrePro.PreOrder)objModel;
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
        public List<YSWL.MALL.Model.Shop.PrePro.PreOrder> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.PrePro.PreOrder> DataTableToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Shop.PrePro.PreOrder> modelList = new List<YSWL.MALL.Model.Shop.PrePro.PreOrder>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Shop.PrePro.PreOrder model;
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
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int userId, string sku, int status)
        {
            return dal.Exists(userId, sku, status);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.PrePro.PreOrder GetModel(int userId, string sku, int status)
        {
            return dal.GetModel(userId, sku, status);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(long PreOrderId, int count, string deliveryTip)
        {
            return dal.Update(PreOrderId, count, deliveryTip);
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.PrePro.PreOrder> GetModelList(int userId, int startIndex, int endIndex, out int toalCount)
        {
            string where = " UserId=" + userId;
            toalCount = GetRecordCount(where);
            if (toalCount <= 0)
                return null;
            DataSet ds = dal.GetListByPage(where, " PreOrderId desc", startIndex, endIndex);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        ///  更新状态 
        /// </summary>
        /// <param name="PreOrderId">Id</param>
        /// <param name="Status">状态</param>
        /// <param name="HandleUserId">处理人Id</param>
        /// <returns></returns>
        public bool UpdateStatus(long PreOrderId, int Status, int HandleUserId)
        {
            return dal.UpdateStatus(PreOrderId, Status, HandleUserId);
        }
        /// <summary>
        ///  批量修改状态
        /// </summary>
        /// <param name="IDlist"></param>
        /// <param name="Status"></param>
        /// <param name="HandleUserId"></param>
        /// <returns></returns>
        public bool UpdateList(string IDlist, int Status, int HandleUserId)
        {
            return dal.UpdateList(IDlist, Status, HandleUserId);
        }
        #endregion  ExtensionMethod
	}
}

