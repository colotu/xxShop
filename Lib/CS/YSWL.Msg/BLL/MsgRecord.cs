using System;
using System.Data;
using System.Collections.Generic;
using YSWL.Msg.DAL;
using YSWL.Msg.Model;

namespace YSWL.Msg.BLL
{
    /// <summary>
    /// MsgRecord
    /// </summary>
    public partial class MsgRecord
    {
        private readonly YSWL.Msg.DAL.MsgRecord dal = new YSWL.Msg.DAL.MsgRecord();
        public MsgRecord()
        {}
        #region  Method

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
        public bool Exists(int ID)
        {
            return dal.Exists(ID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int  Add(YSWL.Msg.Model.MsgRecord model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.Msg.Model.MsgRecord model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            
            return dal.Delete(ID);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string IDlist )
        {
            return dal.DeleteList(IDlist );
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.Msg.Model.MsgRecord GetModel(int ID)
        {
            
            return dal.GetModel(ID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public YSWL.Msg.Model.MsgRecord GetModelByCache(int ID)
        {
            
            string CacheKey = "YSWL.Msg.Model.MsgRecord-" + ID;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(ID);
                    if (objModel != null)
                    {
                        int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("ModelCache");
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch{}
            }
            return (YSWL.Msg.Model.MsgRecord)objModel;
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
        public DataSet GetList(int Top,string strWhere,string filedOrder)
        {
            return dal.GetList(Top,strWhere,filedOrder);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.Msg.Model.MsgRecord> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.Msg.Model.MsgRecord> DataTableToList(DataTable dt)
        {
            List<YSWL.Msg.Model.MsgRecord> modelList = new List<YSWL.Msg.Model.MsgRecord>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.Msg.Model.MsgRecord model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new YSWL.Msg.Model.MsgRecord();
                    if(dt.Rows[n]["ID"]!=null && dt.Rows[n]["ID"].ToString()!="")
                    {
                        model.ID=int.Parse(dt.Rows[n]["ID"].ToString());
                    }
                    if(dt.Rows[n]["MsgBoxId"]!=null && dt.Rows[n]["MsgBoxId"].ToString()!="")
                    {
                        model.MsgBoxId=int.Parse(dt.Rows[n]["MsgBoxId"].ToString());
                    }
                    if(dt.Rows[n]["MsgType"]!=null && dt.Rows[n]["MsgType"].ToString()!="")
                    {
                    model.MsgType=dt.Rows[n]["MsgType"].ToString();
                    }
                    if(dt.Rows[n]["Other"]!=null && dt.Rows[n]["Other"].ToString()!="")
                    {
                    model.Other=dt.Rows[n]["Other"].ToString();
                    }
                    if(dt.Rows[n]["MsgState"]!=null && dt.Rows[n]["MsgState"].ToString()!="")
                    {
                    model.MsgState=dt.Rows[n]["MsgState"].ToString();
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
            return dal.GetListByPage( strWhere,  orderby,  startIndex,  endIndex);
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
            //return dal.GetList(PageSize,PageIndex,strWhere);
        //}

        #endregion  Method
    }
}

