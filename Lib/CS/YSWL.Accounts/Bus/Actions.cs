using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml;
using YSWL.Accounts.IData;
using System.Xml.Linq;

namespace YSWL.Accounts.Bus
{
    /// <summary>
    /// 功能行为
    /// </summary>
    public class Actions
    {
        private IData.IActions dal = PubConstant.IsSQLServer ? (IActions)new Data.Actions() : new MySqlData.Actions();

        #region 属性
        private int actionID;
        private string description;
        private int permissionID;

        /// <summary>
        ///功能行为ID
        /// </summary>
        public int ActionID
        {
            get
            {
                return actionID;
            }
            set
            {
                actionID = value;
            }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        /// <summary>
        /// 权限
        /// </summary>
        public int PermissionID
        {
            get
            {
                return permissionID;
            }
            set
            {
                permissionID = value;
            }
        }
        #endregion

        public bool Exists(string Description)
        {
            return dal.Exists(Description);
        }
        /// <summary>
        /// Add a record
        /// </summary> 
        public int Add(string Description)
        {
            return dal.Add(Description);
        }

        /// <summary>
        /// Add a record,include perimission
        /// </summary>
        public int Add(string Description, int PermissionID)
        {
            return dal.Add(Description, PermissionID);
        }

        /// <summary>
        /// Update a record
        /// </summary>
        public void Update(int ActionID, string Description)
        {
            dal.Update(ActionID, Description);
        }

        /// <summary>
        /// Update a record, include permission
        /// </summary>
        public void Update(int ActionID, string Description, int PermissionID)
        {
            dal.Update(ActionID, Description, PermissionID);
        }

        /// <summary>
        /// Delete a record
        /// </summary>
        public void Delete(int ActionID)
        {
            dal.Delete(ActionID);
        }

        /// <summary>
        /// Get Description
        /// </summary>
        public string GetDescription(int ActionID)
        {
            return dal.GetDescription(ActionID);
        }

        /// <summary>
        /// Query data list
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }

        #region expand

        /// <summary>
        /// Relevance between ActionID and PermissionID
        /// </summary>
        public void AddPermission(int ActionID, int PermissionID)
        {
            dal.AddPermission(ActionID, PermissionID);
        }
        /// <summary>
        /// 批量增加权限设置
        /// </summary>
        /// <param name="ActionIDs"></param>
        /// <param name="PermissionID"></param>
        public void AddPermission(string ActionIDs, int PermissionID)
        {
            dal.AddPermission(ActionIDs, PermissionID);
        }
        /// <summary>
        /// Clear relevance
        /// </summary>        
        public void ClearPermissions(int ActionID)
        {
            dal.ClearPermissions(ActionID);
        }

        /// <summary>
        /// Get an object list
        /// </summary>
        /// <returns></returns>
        public Hashtable GetHashList()
        {
            #region  是否是XML 功能行为
            bool IsXMLAction = YSWL.Common.ConfigHelper.GetConfigBool("SAAS_Action_IsXML");
            if (IsXMLAction)
            {
                return GetXmlHashList();
            }
            #endregion 

            DataSet ds = dal.GetList("");
            Hashtable ht = new Hashtable();
            if ((ds.Tables.Count > 0) && (ds.Tables[0] != null))
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string Keyname = dr["ActionID"].ToString();
                    string Value = dr["PermissionID"].ToString();
                    ht.Add(Keyname, Value);
                }
            }
            return ht;
        }

        /// <summary>
        /// Get an object list，From the cache
        /// </summary>
        public Hashtable GetHashListByCache()
        {
            #region  是否是XML 功能行为
            bool IsXMLAction = YSWL.Common.ConfigHelper.GetConfigBool("SAAS_Action_IsXML");
            if (IsXMLAction)
            {
                return GetXmlHashListCache();
            }
            #endregion 

            string CacheKey = "ActionsPermissionHashList";
            object objModel = DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = GetHashList();
                    if (objModel != null)
                    {
                        int CacheTime = ConfigHelper.GetConfigInt("CacheTime");
                        DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(CacheTime), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (Hashtable)objModel;
        }

        #endregion

        #region XML  Action 

        /// <summary>
        /// 获取所有的功能行为(XML  Hash 列表)
        /// </summary>
        /// <returns></returns>
        public static Hashtable GetXmlHashList()
        {
            Hashtable ht = new Hashtable();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath("/Config/Action.config"));
            //取指定的结点的集合
            XmlNodeList nodes = xmlDoc.SelectNodes("actions/action");
            if (nodes != null)
            {
                foreach (var item in nodes)
                {
                    XmlElement node = (XmlElement)item;
                    string Keyname = node.GetAttribute("ActionID");
                    string Value = node.GetAttribute("PermissionID");
                    ht.Add(Keyname, Value);
                }
            }
            return ht;
        }
        /// <summary>
        /// 获取所有的缓存功能行为 (XML  Hash 列表)
        /// </summary>
        /// <returns></returns>
        public static Hashtable GetXmlHashListCache()
        {
            string CacheKey = "GetXmlHashListCache";
            object objModel = DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = GetXmlHashList();
                    if (objModel != null)
                    {
                        int CacheTime = ConfigHelper.GetConfigInt("CacheTime");
                        DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(CacheTime), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (Hashtable)objModel;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<YSWL.Accounts.Bus.Actions> GetAllActionXml()
        {
            List<Actions> actionList = new List<Actions>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath("/Action.config"));
            //取指定的结点的集合
            XmlNodeList nodes = xmlDoc.SelectNodes("actions/action");
            if (nodes != null)
            {
                YSWL.Accounts.Bus.Actions model = null;
                foreach (var item in nodes)
                {
                    XmlElement node = (XmlElement)item;
                    model = new Actions();
                    model.ActionID = Common.Globals.SafeInt(node.GetAttribute("ActionID"), 0);
                    model.Description = node.GetAttribute("Description");
                    model.PermissionID = Common.Globals.SafeInt(node.GetAttribute("PermissionID"), 0);
                    actionList.Add(model);
                }
            }
            return actionList;
        }
        /// <summary>
        /// 获取可用的XML功能行为
        /// </summary>
        /// <returns></returns>
        public List<Actions> GetAllActionListXmlCache()
        {
            string CacheKey = "GetAllActionListXmlCache";
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = GetAllActionXml();
                    if (objModel != null)
                    {
                        int CacheTime = ConfigHelper.GetConfigInt("CacheTime");
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(CacheTime), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (List<Actions>)objModel;
        }
        /// <summary>
        /// 添加功能行为
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool AddAction(Actions model)
        {
            try
            {
                string xmlFile = HttpContext.Current.Server.MapPath("/Action.config");
                XDocument xmlDoc = XDocument.Load(xmlFile);
                XElement newElement = new XElement("action",
                    new XAttribute("ActionID", model.ActionID),
                    new XAttribute("Description", model.Description),
                    new XAttribute("PermissionID", model.PermissionID)
                    );
                XElement root = xmlDoc.Element("actions");
                if (root != null)
                {
                    //添加的节点是否存在，如果存在就先移除然后再添加。
                    XElement action = root.Elements().FirstOrDefault(c => Common.Globals.SafeInt(c.Attribute("ActionID").Value, 0) == model.ActionID);
                    if (action != null)
                    {
                        action.Remove();
                    }
                    root.Add(newElement);
                }
                xmlDoc.Save(xmlFile);
                return true;
            }
            catch (Exception ex)
            {
                Log.LogHelper.AddErrorLog("添加XML功能行为失败：" + ex.Message, ex.StackTrace);
                throw;
            }

        }
        /// <summary>
        /// 同步所有的数据库功能行为至XML功能行为数据 （初始化一次性调用）
        /// </summary>
        /// <returns></returns>
        public  bool SyncActions()
        {
            DataSet ds = dal.GetList("");
            if ((ds.Tables.Count > 0) && (ds.Tables[0] != null))
            {
                Actions model = null;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    model=new Actions();
                    model.ActionID=Common.Globals.SafeInt(dr["ActionID"],0);
                    model.Description= dr["Description"].ToString();
                    model.PermissionID = Common.Globals.SafeInt(dr["PermissionID"], 0);
                    AddAction(model);
                }
            }
            return true;
        }

        #endregion 

    }
}
