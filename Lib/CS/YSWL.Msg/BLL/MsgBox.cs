using System;
using System.Data;
using System.Collections.Generic;
using YSWL.Msg.DAL;
using YSWL.Msg.Model;

namespace YSWL.Msg.BLL
{
    /// <summary>
    /// MsgBox
    /// </summary>
    public partial class MsgBox
    {
        private readonly YSWL.Msg.DAL.MsgBox dal = new YSWL.Msg.DAL.MsgBox();
        public MsgBox()
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
        public int Add(YSWL.Msg.Model.MsgBox model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.Msg.Model.MsgBox model)
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
        public bool DeleteList(string IDlist)
        {
            return dal.DeleteList(IDlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.Msg.Model.MsgBox GetModel(int ID)
        {

            return dal.GetModel(ID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public YSWL.Msg.Model.MsgBox GetModelByCache(int ID)
        {

            string CacheKey = "YSWL.Msg.Model.MsgBox-" + ID;
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
                catch { }
            }
            return (YSWL.Msg.Model.MsgBox)objModel;
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
        public List<YSWL.Msg.Model.MsgBox> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.Msg.Model.MsgBox> DataTableToList(DataTable dt)
        {
            List<YSWL.Msg.Model.MsgBox> modelList = new List<YSWL.Msg.Model.MsgBox>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.Msg.Model.MsgBox model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new YSWL.Msg.Model.MsgBox();
                    if (dt.Rows[n]["ID"] != null && dt.Rows[n]["ID"].ToString() != "")
                    {
                        model.ID = int.Parse(dt.Rows[n]["ID"].ToString());
                    }
                    if (dt.Rows[n]["SenderID"] != null && dt.Rows[n]["SenderID"].ToString() != "")
                    {
                        model.SenderID = dt.Rows[n]["SenderID"].ToString();
                    }
                    if (dt.Rows[n]["ReceiverID"] != null && dt.Rows[n]["ReceiverID"].ToString() != "")
                    {
                        model.ReceiverID = dt.Rows[n]["ReceiverID"].ToString();
                    }
                    if (dt.Rows[n]["Title"] != null && dt.Rows[n]["Title"].ToString() != "")
                    {
                        model.Title = dt.Rows[n]["Title"].ToString();
                    }
                    if (dt.Rows[n]["Content"] != null && dt.Rows[n]["Content"].ToString() != "")
                    {
                        model.Content = dt.Rows[n]["Content"].ToString();
                    }
                    if (dt.Rows[n]["MsgType"] != null && dt.Rows[n]["MsgType"].ToString() != "")
                    {
                        model.MsgType = dt.Rows[n]["MsgType"].ToString();
                    }
                    if (dt.Rows[n]["SendTime"] != null && dt.Rows[n]["SendTime"].ToString() != "")
                    {
                        model.SendTime = DateTime.Parse(dt.Rows[n]["SendTime"].ToString());
                    }
                    if (dt.Rows[n]["ReadTime"] != null && dt.Rows[n]["ReadTime"].ToString() != "")
                    {
                        model.ReadTime = DateTime.Parse(dt.Rows[n]["ReadTime"].ToString());
                    }
                    if (dt.Rows[n]["ReMark"] != null && dt.Rows[n]["ReMark"].ToString() != "")
                    {
                        model.ReMark = dt.Rows[n]["ReMark"].ToString();
                    }
                    if (dt.Rows[n]["Other"] != null && dt.Rows[n]["Other"].ToString() != "")
                    {
                        model.Other = dt.Rows[n]["Other"].ToString();
                    }
                    if (dt.Rows[n]["ReceiverIsRead"] != null && dt.Rows[n]["ReceiverIsRead"].ToString() != "")
                    {
                        if ((dt.Rows[n]["ReceiverIsRead"].ToString() == "1") || (dt.Rows[n]["ReceiverIsRead"].ToString().ToLower() == "true"))
                        {
                            model.ReceiverIsRead = true;
                        }
                        else
                        {
                            model.ReceiverIsRead = false;
                        }
                    }
                    if (dt.Rows[n]["SenderIsDel"] != null && dt.Rows[n]["SenderIsDel"].ToString() != "")
                    {
                        if ((dt.Rows[n]["SenderIsDel"].ToString() == "1") || (dt.Rows[n]["SenderIsDel"].ToString().ToLower() == "true"))
                        {
                            model.SenderIsDel = true;
                        }
                        else
                        {
                            model.SenderIsDel = false;
                        }
                    }
                    if (dt.Rows[n]["ReaderIsDel"] != null && dt.Rows[n]["ReaderIsDel"].ToString() != "")
                    {
                        if ((dt.Rows[n]["ReaderIsDel"].ToString() == "1") || (dt.Rows[n]["ReaderIsDel"].ToString().ToLower() == "true"))
                        {
                            model.ReaderIsDel = true;
                        }
                        else
                        {
                            model.ReaderIsDel = false;
                        }
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
        /// 根据存储过程和参数填入相应的值，如果存储过程中不存在某个参数，如果是string类型则赋null 如果是int 则赋0
        /// </summary>
        /// <param name="procedureName">存储过程的名称</param>
        /// <param name="AdminId">管理员的id</param>
        /// <param name="UserType">用户的类型</param>
        /// <param name="ReceiverID">接受者的id</param>
        /// <param name="SenderID">发送者的id</param>
        /// <param name="ID">id（删除和修改的时候用到 主键）</param>
        /// <returns>返回值</returns>
        public int ReturnDataCountByProcedure(string procedureName, string AdminId, string UserType, string ReceiverID, string SenderID, int ID)
        {
           return this.dal.ReturnDataCountByProcedure( procedureName,  AdminId,  UserType,  ReceiverID,  SenderID,  ID);

        }


        /// <summary>
        /// 根据存储过程和参数填入相应的值，如果存储过程中不存在某个参数，如果是string类型则赋null 如果是int 则赋0
        /// </summary>
        /// <param name="procedureName">存储过程的名称</param>
        /// <param name="AdminId">管理员的id</param>
        /// <param name="UserType">用户的类型</param>
        /// <param name="ReceiverID">接受者的id</param>
        /// <param name="SenderID">发送者的id</param>
        /// <returns>返回值</returns>
        public int ReturnDataCountByProcedure(string procedureName, string AdminId, string UserType, string ReceiverID, string SenderID)
        {

            return this.ReturnDataCountByProcedure(procedureName, AdminId, UserType, ReceiverID, SenderID, 0);


        }
        /// <summary>
        /// 根据存储过程和参数填入相应的值，如果存储过程中不存在某个参数，如果是string类型则赋null 如果是int 则赋0
        /// </summary>
        /// <param name="procedureName">存储过程的名称</param>
        /// <param name="AdminId">管理员的id</param>
        /// <param name="UserType">用户的类型</param>
        /// <param name="ReceiverID">接受者的id</param>
        /// <param name="SenderID">发送者的id</param>
        /// <param name="ID">id（删除和修改的时候用到 主键）</param>
        /// <param name="StartIndex">数据集起始的index。用于分页</param>
        /// <param name="EndIndex">i数据集结束的index，用于分页</param>
        /// <returns>返回值</returns>
        public DataSet ReturnDataListByProcedure(string procedureName, string AdminId, string UserType, string ReceiverID, string SenderID, int StartIndex, int EndIndex, int ID)
        {
            return this.dal.ReturnDataListByProcedure(procedureName, AdminId, UserType, ReceiverID, SenderID, StartIndex, EndIndex, ID);
        }

        /// <summary>
        /// 根据存储过程和参数填入相应的值，如果存储过程中不存在某个参数，如果是string类型则赋null 如果是int 则赋0
        /// </summary>
        /// <param name="procedureName">存储过程的名称</param>
        /// <param name="AdminId">管理员的id</param>
        /// <param name="UserType">用户的类型</param>
        /// <param name="ReceiverID">接受者的id</param>
        /// <param name="SenderID">发送者的id</param>
        /// <param name="ID">id（删除和修改的时候用到 主键）</param>
        /// <param name="StartIndex">数据集起始的index。用于分页</param>
        /// <param name="EndIndex">i数据集结束的index，用于分页</param>
        /// <returns>返回值</returns>
  
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
    }
}

