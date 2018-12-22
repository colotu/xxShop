using System;
using System.Data;
using YSWL.MALL.Model.SysManage;
using YSWL.MALL.DALFactory;
using YSWL.Common;
using System.Collections.Generic;
using YSWL.MALL.IDAL.SysManage;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace YSWL.MALL.BLL.SysManage
{
    /// <summary>
    /// 系统菜单管理
    /// </summary>
    public class SysTree
    {
        private readonly ISysTree dal = DASysManage.CreateSysTree();


        public int GetPermissionCatalogID(int permissionID)
        {
            return dal.GetPermissionCatalogID(permissionID);
        }
        public SysTree()
        {
        }

        public int AddTreeNode(SysNode node)
        {
            return dal.AddTreeNode(node);
        }
        public void UpdateNode(SysNode node)
        {
            dal.UpdateNode(node);
        }
        public void DelTreeNode(int nodeid)
        {
            dal.DelTreeNode(nodeid);
        }
        public void DelTreeNodes(string nodeidlist)
        {
            dal.DelTreeNodes(nodeidlist);
        }
        public void MoveNodes(string nodeidlist, int ParentID)
        {
            dal.MoveNodes(nodeidlist, ParentID);
        }

        public DataSet GetTreeList(string strWhere)
        {
            return dal.GetTreeList(strWhere);
        }

        /// <summary>
        /// 获取全部菜单数据
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllTree()
        {
            return dal.GetTreeList("");
        }

        /// <summary>
        /// 根据菜单类型获取对应菜单数据
        /// </summary>
        /// <param name="treeType">菜单类型 0:admin后台 1:企业后台  2:代理商后台 3:用户后台</param>
        /// <returns></returns>
        public DataSet GetAllTreeByType(int treeType)
        {
            return dal.GetTreeList("TreeType=" + treeType);
        }

        /// <summary>
        /// 根据菜单类型获取启用的菜单数据
        /// </summary>
        /// <param name="treeType">菜单类型 0:admin后台 1:企业后台  2:代理商后台 3:用户后台</param>
        /// <returns></returns>
        public DataSet GetAllEnabledTreeByType(int treeType)
        {
            return GetAllEnabledTreeByType(treeType, true);
        }

        /// <summary>
        /// 根据菜单类型获取对应菜单数据
        /// </summary>
        /// <param name="treeType">菜单类型 0:admin后台 1:企业后台  2:代理商后台 3:用户后台</param>
        /// <param name="Enabled">是否启用</param>
        /// <returns></returns>
        public DataSet GetAllEnabledTreeByType(int treeType, bool Enabled)
        {
            return dal.GetTreeList("TreeType=" + treeType + " AND Enabled = " + (Enabled ? "1" : "0"));
        }

        /// <summary>
        /// 根据菜单类型获取对应菜单数据
        /// </summary>
        /// <param name="parentID">父ID</param>
        /// <param name="treeType">菜单类型 0:admin后台 1:企业后台  2:代理商后台 3:用户后台</param>
        /// <param name="Enabled">是否启用</param>
        /// <returns></returns>
        public DataSet GetEnabledTreeByParentId(int parentID, int treeType, bool Enabled)
        {
            return dal.GetTreeList("ParentID=" + parentID + " AND TreeType=" + treeType + " AND Enabled = " + (Enabled ? "1" : "0"));
        }

        /// <summary>
        /// Get an object list，From the cache
        /// <param name="treeType">菜单类型 0:admin后台 1:企业后台  2:代理商后台 3:用户后台</param>
        /// </summary>
        public DataSet GetAllEnabledTreeByType4Cache(int treeType)
        {
            string CacheKey = "GetAllEnabledTreeByType4Cache" + treeType;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = GetAllEnabledTreeByType(treeType);
                    if (objModel != null)
                    {
                        int CacheTime = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("CacheTime"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(CacheTime), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (DataSet)objModel;
        }


        public DataSet GetTreeSonList(int NodeID, int treeType, List<int> UserPermissions)
        {
            string strWhere = " Enabled=1 and TreeType=" + treeType;
            if (NodeID > -1)
            {
                strWhere += " and parentid=" + NodeID;
            }
            if (UserPermissions.Count > 0)
            {
                strWhere += " and (PermissionID=-1 or PermissionID in (" + StringPlus.GetArrayStr(UserPermissions) + "))";
            }
            return dal.GetTreeList(strWhere);
        }

        public SysNode GetNode(int NodeID)
        {
            return dal.GetNode(NodeID);
        }

        /// <summary>
        /// Get an object entity，From the cache
        /// </summary>
        public SysNode GetModelByCache(int NodeID)
        {

            string CacheKey = "SysManageModel-" + NodeID;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetNode(NodeID);
                    if (objModel != null)
                    {
                        int CacheTime = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("CacheTime"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(CacheTime), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (SysNode)objModel;
        }


        /// <summary>
        /// 修改启用状态
        /// </summary>
        /// <param name="nodeid"></param>
        public void UpdateEnabled(int nodeid)
        {
           dal.UpdateEnabled(nodeid);
        }


        public List<YSWL.MALL.Model.SysManage.SysNode> GetTreeListByType(int treeType,bool Enabled)
        {
            //根据配置加载对应的菜单
            bool IsXMLTree = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("SAAS_Menu_IsXML");
            if (IsXMLTree)
            {
                return GetAllTreeListXmlCache(treeType);
            }

            DataSet ds = GetAllEnabledTreeByType(treeType, Enabled);
            List<YSWL.MALL.Model.SysManage.SysNode> NodeList= DataTableToList(ds.Tables[0]);
            foreach (var sysNode in NodeList)
            {
                int count = NodeList.Where(c => c.ParentID == sysNode.NodeID).Count();
                if (count == 0)
                    sysNode.hasChildren = false;
            }
            return NodeList;
        }


        public List<YSWL.MALL.Model.SysManage.SysNode> GetTreeListByTypeCache(int treeType, bool Enabled)
        {
            //根据配置加载对应的菜单
            bool IsXMLTree = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("SAAS_Menu_IsXML");
            if (IsXMLTree)
            {
                return GetAllTreeListXmlCache(treeType);
            }

            string CacheKey = "GetTreeListByTypeCache-" + treeType + Enabled;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = GetTreeListByType(treeType, Enabled);
                    if (objModel != null)
                    {
                        int CacheTime = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("CacheTime"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(CacheTime), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (List<YSWL.MALL.Model.SysManage.SysNode>)objModel;
        }
      
	
		/// <summary>
		/// 获得数据列表
		/// </summary>
        public List<YSWL.MALL.Model.SysManage.SysNode> DataTableToList(DataTable dt)
		{
            List<YSWL.MALL.Model.SysManage.SysNode> modelList = new List<YSWL.MALL.Model.SysManage.SysNode>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
                YSWL.MALL.Model.SysManage.SysNode model;
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

        #region 日志管理
        public void AddLog(string time, string loginfo, string Particular)
        {
            dal.AddLog(time, loginfo, Particular);
        }
        public void DelOverdueLog(int days)
        {
            dal.DelOverdueLog(days);
        }
        public void DeleteLog(string Idlist)
        {
            string str = "";
            if (Idlist.Trim() != "")
            {
                str = " ID in (" + Idlist + ")";
            }
            dal.DeleteLog(str);
        }
        public void DeleteLog(string timestart, string timeend)
        {
            string str = " datetime>'" + timestart + "' and datetime<'" + timeend + "'";
            dal.DeleteLog(str);
        }
        public DataSet GetLogs(string strWhere)
        {
            return dal.GetLogs(strWhere);
        }
        public DataRow GetLog(string ID)
        {
            return dal.GetLog(ID);
        }

        #endregion

        #region  菜单管理 XML
        /// <summary>
        /// 获取所有的菜单
        /// </summary>
        /// <returns></returns>
        public static List<SysNode> GetAllTreeXml()
        {
            List<SysNode> menuList = new List<SysNode>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath("/Config/Menu.config"));
            //取指定的结点的集合
            XmlNodeList nodes = xmlDoc.SelectNodes("menus/menu");
            if (nodes != null)
            {
                SysNode model = null;
                foreach (var item in nodes)
                {
                    XmlElement node = (XmlElement)item;
                    model = new SysNode();
                    model.NodeID = Common.Globals.SafeInt(node.GetAttribute("NodeID"), 0);
                    model.TreeText = node.GetAttribute("TreeText");
                    model.OrderID = Common.Globals.SafeInt(node.GetAttribute("OrderID"), 0);
                    model.ParentID = Common.Globals.SafeInt(node.GetAttribute("ParentID"), 0);
                    model.PermissionID = Common.Globals.SafeInt(node.GetAttribute("PermissionID"), 0);
                    model.ImageUrl = node.GetAttribute("ImageUrl");
                    model.TreeType = Common.Globals.SafeInt(node.GetAttribute("TreeType"), 0);
                    model.Url = node.GetAttribute("Url");
                    model.Enabled = Common.Globals.SafeBool(node.GetAttribute("Enabled"), false);
                    menuList.Add(model);
                }
            }
            return menuList;
        }
        /// <summary>
        /// 获取可用的XML菜单
        /// </summary>
        /// <returns></returns>
        public List<YSWL.MALL.Model.SysManage.SysNode> GetAllTreeListXmlCache(int treeType)
        {
            string CacheKey = "GetAllTreeListXmlCache_"+ treeType;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    List<SysNode> allList = GetAllTreeXml();
                    objModel = allList.Where(c => c.Enabled&&c.TreeType== treeType).ToList();
                    if (objModel != null)
                    {
                        int CacheTime = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("CacheTime"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(CacheTime), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (List<YSWL.MALL.Model.SysManage.SysNode>)objModel;
        }
        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool AddMenu(SysNode model)
        {
            try
            {
                string xmlFile = HttpContext.Current.Server.MapPath("/Config/Menu.config");
                XDocument xmlDoc = XDocument.Load(xmlFile);
                XElement newElement = new XElement("menu",
                    new XAttribute("NodeID", model.NodeID),
                    new XAttribute("TreeText", model.TreeText),
                    new XAttribute("OrderID", model.OrderID.HasValue ? model.OrderID.Value : 0),
                    new XAttribute("ParentID", model.ParentID),
                    new XAttribute("PermissionID", model.NodeID),
                    new XAttribute("ImageUrl", model.ImageUrl),
                    new XAttribute("TreeType", model.TreeType),
                    new XAttribute("Url", model.Url),
                    new XAttribute("Enabled", model.Enabled)
                    );
                XElement root = xmlDoc.Element("menus");
                if (root != null)
                {
                    //添加的节点是否存在，如果存在就先移除然后再添加。
                    XElement menu = root.Elements().FirstOrDefault(c => Common.Globals.SafeInt(c.Attribute("NodeID").Value, 0) == model.NodeID);
                    if (menu != null)
                    {
                        menu.Remove();
                    }
                    root.Add(newElement);
                }
                xmlDoc.Save(xmlFile);
                return true;
            }
            catch (Exception ex)
            {
                Log.LogHelper.AddErrorLog("添加XML菜单失败：" + ex.Message, ex.StackTrace);
                throw;
            }

        }
        /// <summary>
        /// 同步所有的数据库菜单至XML菜单 （初始化一次性调用）
        /// </summary>
        /// <returns></returns>
        public static bool SyncMenu()
        {
           // YSWL.MALL.BLL.SysManage.SysTree treeBll = new SysTree();
            //int treeType = 4; //Common.ConfigHelper.GetConfigInt("TreeType");
            List<SysNode> allList = GetAllTreeXml();//treeBll.GetTreeListByType(treeType, true);
            foreach (var item in allList)
            {
                AddMenu(item);
            }
            return true;
        }
        #endregion 
    }
}
