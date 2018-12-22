using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using YSWL.Common;
using YSWL.MALL.Model;
using YSWL.MALL.DALFactory;
namespace YSWL.MALL.BLL
{
    /// <summary>
    /// 用户邮箱配置
    /// </summary>
    public class MailConfig
    {
        private readonly YSWL.MALL.IDAL.IMailConfig dal = DAMembers.CreateMailConfig();

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
        public int Add(YSWL.MALL.Model.MailConfig model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update(YSWL.MALL.Model.MailConfig model)
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
        public YSWL.MALL.Model.MailConfig GetModel(int ID)
        {

            return dal.GetModel(ID);
        }


        /// <summary>
        /// 
        /// </summary>
        public YSWL.MALL.Model.MailConfig GetModel()
        {

            return dal.GetModel();
        }
        /// <summary>
        /// 
        /// </summary>
        public YSWL.MALL.Model.MailConfig GetModel(int? userId)
        {

            return dal.GetModel(userId);
        }

        /// <summary>
        /// 
        /// </summary>
        public YSWL.MALL.Model.MailConfig GetModelByCache(int ID)
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
                        int CacheTime = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("CacheTime"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(CacheTime), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.MailConfig)objModel;
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
        public List<YSWL.MALL.Model.MailConfig> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 
        /// </summary>
        public List<YSWL.MALL.Model.MailConfig> DataTableToList(DataTable dt)
        {
            List<YSWL.MALL.Model.MailConfig> modelList = new List<YSWL.MALL.Model.MailConfig>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.MailConfig model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new YSWL.MALL.Model.MailConfig();
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
