using System;
using System.Collections.Generic;
using System.Data;
using YSWL.Common;
using YSWL.Email.IDAL;

namespace YSWL.Email.BLL
{
    public class MailConfig
    {
        private readonly YSWL.Email.IDAL.IMailConfig dal = YSWL.DBUtility.PubConstant.IsSQLServer ? (IMailConfig)new Email.SQLServerDAL.MailConfig() : new Email.MySqlDAL.MailConfig();

        public MailConfig()
        { }

        #region

        /// <summary>
        ///
        /// </summary>
        public bool Exists(int UserID, string Mailaddress)
        {
            return dal.Exists(UserID, Mailaddress);
        }

        /// <summary>
        ///
        /// </summary>
        public int Add(YSWL.Email.Model.MailConfig model)
        {
            return dal.Add(model);
        }

        /// <summary>
        ///
        /// </summary>
        public void Update(YSWL.Email.Model.MailConfig model)
        {
            dal.Update(model);
        }

        /// <summary>
        ///
        /// </summary>
        public void Delete(int ID)
        {
            dal.Delete(ID);
        }

        /// <summary>
        ///
        /// </summary>
        public YSWL.Email.Model.MailConfig GetModel(int ID)
        {
            return dal.GetModel(ID);
        }

        /// <summary>
        ///
        /// </summary>
        public YSWL.Email.Model.MailConfig GetModel()
        {
            return dal.GetModel();
        }

        /// <summary>
        ///
        /// </summary>
        public YSWL.Email.Model.MailConfig GetModelByCache(int ID)
        {
            string CacheKey = "MailConfigModel-" + ID;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(ID);
                    if (objModel != null)
                    {
                        int CacheTime = 30;
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(CacheTime), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.Email.Model.MailConfig)objModel;
        }

        /// <summary>
        ///
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }

        /// <summary>
        ///
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }

        /// <summary>
        ///
        /// </summary>
        public List<YSWL.Email.Model.MailConfig> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        ///
        /// </summary>
        public List<YSWL.Email.Model.MailConfig> DataTableToList(DataTable dt)
        {
            List<YSWL.Email.Model.MailConfig> modelList = new List<YSWL.Email.Model.MailConfig>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.Email.Model.MailConfig model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new YSWL.Email.Model.MailConfig();
                    if (dt.Rows[n]["ID"].ToString() != "")
                    {
                        model.ID = int.Parse(dt.Rows[n]["ID"].ToString());
                    }
                    if (dt.Rows[n]["UserID"].ToString() != "")
                    {
                        model.UserID = int.Parse(dt.Rows[n]["UserID"].ToString());
                    }
                    model.Mailaddress = dt.Rows[n]["Mailaddress"].ToString();
                    model.Username = dt.Rows[n]["Username"].ToString();
                    model.Password = dt.Rows[n]["Password"].ToString();
                    model.SMTPServer = dt.Rows[n]["SMTPServer"].ToString();
                    if (dt.Rows[n]["SMTPPort"].ToString() != "")
                    {
                        model.SMTPPort = int.Parse(dt.Rows[n]["SMTPPort"].ToString());
                    }
                    if (dt.Rows[n]["SMTPSSL"].ToString() != "")
                    {
                        model.SMTPSSL = dt.Rows[n]["SMTPSSL"].ToString() == "1" ? true : false;
                    }
                    model.POPServer = dt.Rows[n]["POPServer"].ToString();
                    if (dt.Rows[n]["POPPort"].ToString() != "")
                    {
                        model.POPPort = int.Parse(dt.Rows[n]["POPPort"].ToString());
                    }
                    if (dt.Rows[n]["POPSSL"].ToString() != "")
                    {
                        model.POPSSL = dt.Rows[n]["POPSSL"].ToString() == "1" ? true : false;
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        /// <summary>
        ///
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        public DataSet GetListByUser(int UserID)
        {
            return GetList(" UserID=" + UserID);
        }

        #endregion
    }
}