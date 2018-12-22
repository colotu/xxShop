using System;
using System.Collections.Generic;
using System.Data;
using YSWL.Email.IDAL;

namespace YSWL.Email.BLL
{
    /// <summary>
    /// EmailQueue
    /// </summary>
    public partial class EmailQueue
    {
        private readonly YSWL.Email.IDAL.IEmailQueue dal = YSWL.DBUtility.PubConstant.IsSQLServer ? (IEmailQueue)new Email.SQLServerDAL.EmailQueue() : new Email.MySqlDAL.EmailQueue();
      

        public EmailQueue()
        { }

        #region Method

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(YSWL.Email.Model.EmailQueue model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.Email.Model.EmailQueue model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete()
        {
            //该表无主键信息，请自定义主键/条件字段
            return dal.Delete();
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.Email.Model.EmailQueue GetModel()
        {
            //该表无主键信息，请自定义主键/条件字段
            return dal.GetModel();
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public YSWL.Email.Model.EmailQueue GetModelByCache()
        {
            //该表无主键信息，请自定义主键/条件字段
            string CacheKey = "EmailQueueModel-";
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel();
                    if (objModel != null)
                    {
                        int ModelCache = 30;// Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.Email.Model.EmailQueue)objModel;
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
        public List<YSWL.Email.Model.EmailQueue> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.Email.Model.EmailQueue> DataTableToList(DataTable dt)
        {
            List<YSWL.Email.Model.EmailQueue> modelList = new List<YSWL.Email.Model.EmailQueue>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.Email.Model.EmailQueue model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new YSWL.Email.Model.EmailQueue();
                    if (dt.Rows[n]["EmailId"] != null && dt.Rows[n]["EmailId"].ToString() != "")
                    {
                        model.EmailId = int.Parse(dt.Rows[n]["EmailId"].ToString());
                    }
                    if (dt.Rows[n]["EmailPriority"] != null && dt.Rows[n]["EmailPriority"].ToString() != "")
                    {
                        model.EmailPriority = int.Parse(dt.Rows[n]["EmailPriority"].ToString());
                    }
                    if (dt.Rows[n]["IsBodyHtml"] != null && dt.Rows[n]["IsBodyHtml"].ToString() != "")
                    {
                        if ((dt.Rows[n]["IsBodyHtml"].ToString() == "1") || (dt.Rows[n]["IsBodyHtml"].ToString().ToLower() == "true"))
                        {
                            model.IsBodyHtml = true;
                        }
                        else
                        {
                            model.IsBodyHtml = false;
                        }
                    }
                    if (dt.Rows[n]["EmailTo"] != null && dt.Rows[n]["EmailTo"].ToString() != "")
                    {
                        model.EmailTo = dt.Rows[n]["EmailTo"].ToString();
                    }
                    if (dt.Rows[n]["EmailCc"] != null && dt.Rows[n]["EmailCc"].ToString() != "")
                    {
                        model.EmailCc = dt.Rows[n]["EmailCc"].ToString();
                    }
                    if (dt.Rows[n]["EmailBcc"] != null && dt.Rows[n]["EmailBcc"].ToString() != "")
                    {
                        model.EmailBcc = dt.Rows[n]["EmailBcc"].ToString();
                    }
                    if (dt.Rows[n]["EmailFrom"] != null && dt.Rows[n]["EmailFrom"].ToString() != "")
                    {
                        model.EmailFrom = dt.Rows[n]["EmailFrom"].ToString();
                    }
                    if (dt.Rows[n]["EmailSubject"] != null && dt.Rows[n]["EmailSubject"].ToString() != "")
                    {
                        model.EmailSubject = dt.Rows[n]["EmailSubject"].ToString();
                    }
                    if (dt.Rows[n]["EmailBody"] != null && dt.Rows[n]["EmailBody"].ToString() != "")
                    {
                        model.EmailBody = dt.Rows[n]["EmailBody"].ToString();
                    }
                    if (dt.Rows[n]["NextTryTime"] != null && dt.Rows[n]["NextTryTime"].ToString() != "")
                    {
                        model.NextTryTime = DateTime.Parse(dt.Rows[n]["NextTryTime"].ToString());
                    }
                    if (dt.Rows[n]["NumberOfTries"] != null && dt.Rows[n]["NumberOfTries"].ToString() != "")
                    {
                        model.NumberOfTries = int.Parse(dt.Rows[n]["NumberOfTries"].ToString());
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

        #endregion Method

        /// <summary>
        /// 将邮件加入邮件发送队列
        /// </summary>
        public bool PushEmailQueur(string uType, string uName, string EmailSubject, string EmailBody, string EmailFrom)
        {
            return dal.PushEmailQueur(uType, uName, EmailSubject, EmailBody, EmailFrom);
        }

        public bool Exists(string emailSubject)
        {
            return dal.Exists(emailSubject);
        }
    }
}