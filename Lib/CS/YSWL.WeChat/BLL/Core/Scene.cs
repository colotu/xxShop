/**  版本信息模板在安装目录下，可自行修改。
* Scene.cs
*
* 功 能： N/A
* 类 名： Scene
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/2/20 12:32:07   N/A    初版
*
* Copyright (c) 2012 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Collections.Generic;
using YSWL.Common;
using YSWL.WeChat.Model.Core;
using YSWL.WeChat.IDAL.Core;
namespace YSWL.WeChat.BLL.Core
{
	/// <summary>
	/// Scene
	/// </summary>
	public partial class Scene
	{
        private readonly IScene dal = YSWL.DBUtility.PubConstant.IsSQLServer ? (IScene)new WeChat.SQLServerDAL.Core.Scene() : (IScene)new WeChat.MySqlDAL.Core.Scene();
		public Scene()
		{}
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
		public bool Exists(int SceneId)
		{
			return dal.Exists(SceneId);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(YSWL.WeChat.Model.Core.Scene model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(YSWL.WeChat.Model.Core.Scene model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int SceneId)
		{
			
			return dal.Delete(SceneId);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string SceneIdlist )
		{
			return dal.DeleteList(SceneIdlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public YSWL.WeChat.Model.Core.Scene GetModel(int SceneId)
		{
			
			return dal.GetModel(SceneId);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public YSWL.WeChat.Model.Core.Scene GetModelByCache(int SceneId)
		{
			
			string CacheKey = "SceneModel-" + SceneId;
			object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(SceneId);
					if (objModel != null)
					{
						int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("ModelCache");
						YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (YSWL.WeChat.Model.Core.Scene)objModel;
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
		public List<YSWL.WeChat.Model.Core.Scene> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<YSWL.WeChat.Model.Core.Scene> DataTableToList(DataTable dt)
		{
			List<YSWL.WeChat.Model.Core.Scene> modelList = new List<YSWL.WeChat.Model.Core.Scene>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				YSWL.WeChat.Model.Core.Scene model;
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
			return dal.GetListByPage( strWhere,  orderby,  startIndex,  endIndex);
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
        #region  根据用户获取用户的推广二维码图片
        public static string GetUserQRImage(string token, YSWL.WeChat.Model.Core.User userInfo)
        {

            if (userInfo == null)
            {
                return "";
            }
            YSWL.WeChat.BLL.Core.Scene sceneBll = new Scene();
            YSWL.WeChat.Model.Core.Scene sceneInfo = sceneBll.GetSceneInfo(userInfo.UserId);
            string ticket = "";
            if (sceneInfo != null)
            {
                if (String.IsNullOrWhiteSpace(sceneInfo.ImageUrl))
                {
                    ticket = YSWL.WeChat.BLL.Core.Utils.GetTicket(token, sceneInfo.SceneId);
                    if (!String.IsNullOrWhiteSpace(ticket))
                    {

                        string ImageBase = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}";
                        string qrImage = String.Format(ImageBase, ticket);//直接用远程路径，没必要保存在本地
                        if (!String.IsNullOrWhiteSpace(qrImage))
                        {
                            sceneInfo.ImageUrl = qrImage;
                            sceneBll.Update(sceneInfo);
                        }
                        return qrImage;
                    }
                }
                else
                {
                    return sceneInfo.ImageUrl;
                }
            }
            
            //添加用户的场景信息
            sceneInfo=new Model.Core.Scene();
            sceneInfo.CreateTime = DateTime.Now;
            sceneInfo.CreatedUserId = userInfo.UserId;
            sceneInfo.Name = userInfo.NickName;
            sceneInfo.OpenId = userInfo.OpenId;
            sceneInfo.Remark = "微信用户二维码推广场景";
            sceneInfo.SceneId = sceneBll.Add(sceneInfo);
            if (sceneInfo.SceneId == 0)
            {
                return "";
            }
             ticket = YSWL.WeChat.BLL.Core.Utils.GetTicket(token, sceneInfo.SceneId);
            if (!String.IsNullOrWhiteSpace(ticket))
            {
   
                string ImageBase = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}";
                string qrImage= String.Format(ImageBase, ticket);//直接用远程路径，没必要保存在本地
                if (!String.IsNullOrWhiteSpace(qrImage))
                {
                    sceneInfo.ImageUrl = qrImage;
                    sceneBll.Update(sceneInfo); 
                }
                return qrImage;
            }
            return "";

        }

        
        #endregion
        /// <summary>
        /// 获取场景信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
	    public YSWL.WeChat.Model.Core.Scene GetSceneInfo(int userId)
	    {
	        return dal.GetSceneInfo(userId);
	    }

	    #endregion  ExtensionMethod
    }
}

