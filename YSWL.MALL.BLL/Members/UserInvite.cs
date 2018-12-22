/**
* UserInvite.cs
*
* 功 能： N/A
* 类 名： UserInvite
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/7/9 13:45:11   N/A    初版
*
* Copyright (c) 2012-2013 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Collections.Generic;
using YSWL.MALL.BLL.SysManage;
using YSWL.Common;
using YSWL.MALL.Model.Members;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Members;
using YSWL.WeChat.BLL.Core;

namespace YSWL.MALL.BLL.Members
{
	/// <summary>
	/// UserInvite
	/// </summary>
	public partial class UserInvite
	{
		private readonly IUserInvite dal=DAMembers.CreateUserInvite();
		public UserInvite()
		{}
        #region  BasicMethod

        /// <summary>
        /// 获取会员推荐人ID,输入用户ID
        /// </summary> 
        /// <returns></returns>
        public int GetInvUIdByUserid(int userid)
        {
            return dal.GetInvUIdByUserid(userid);//获取会员推荐人ID
        }


        /// <summary>
        ///获取会员推荐人ID,输入用户名
        /// </summary> 
        /// <returns></returns>
        public int GetInvUIdByUsername(string Username)
        {
            return dal.GetInvUIdByUsername(Username);//获取当前积分
        }

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
        public bool Exists(int InviteId)
        {
            return dal.Exists(InviteId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.MALL.Model.Members.UserInvite model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.Members.UserInvite model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int InviteId)
        {

            return dal.Delete(InviteId);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string InviteIdlist)
        {
            return dal.DeleteList(InviteIdlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Members.UserInvite GetModel(int InviteId)
        {

            return dal.GetModel(InviteId);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public YSWL.MALL.Model.Members.UserInvite GetModelByCache(int InviteId)
        {

            string CacheKey = "UserInviteModel-" + InviteId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(InviteId);
                    if (objModel != null)
                    {
                        int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("CacheTime");
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.Members.UserInvite)objModel;
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
        public List<YSWL.MALL.Model.Members.UserInvite> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Members.UserInvite> DataTableToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Members.UserInvite> modelList = new List<YSWL.MALL.Model.Members.UserInvite>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Members.UserInvite model;
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
        public List<Model.Members.UserInvite> GetListByUserId(int userId)
        {
            return GetModelList("InviteUserId=" + userId);
        }
        /// <summary>
        /// 获得分页数据列表
        /// </summary>
        public List<Model.Members.UserInvite> GetListByPage(string strWhere,int startIndex, int endIndex)
        {
            DataSet ds = dal.GetListByPage(strWhere, "  InviteId desc ", startIndex, endIndex);
            return  DataTableToList(ds.Tables[0]);
        }

        public bool UpdateStatus(int InviteId, int Status)
        {
            return dal.UpdateStatus(InviteId, Status);
        }
        /// <summary>
        /// 更新用户层级
        /// </summary>
        /// <param name="InviteUserId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
	    public bool UpdateDepthAndPath(int InviteUserId, int UserId)
	    {
            //获取上级用户
            YSWL.MALL.Model.Members.UserInvite parentModel = GetModelEx(InviteUserId);
            int depth = 2;
            string path = InviteUserId+"|"+UserId;
            if (parentModel != null)
            {
                depth =parentModel.Depth+1;
                path = parentModel.Path + "|" + UserId;
            }
            return dal.UpdateDepthAndPath(UserId, depth, path);
	    }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public YSWL.MALL.Model.Members.UserInvite GetModelEx(int UserId)
        {
            return dal.GetModelEx(UserId);
        }
        /// <summary>
        /// 是否存在被邀请了
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
	    public bool IsExist(int userId)
        {
            return dal.IsExist(userId);
        }

        /// <summary>
        /// 添加邀请信息
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="userOpen"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool AddInvite(string openId, string userOpen, int userId,string userName,string nickName)
	    {
           
            //判断是否已经存在被邀请了
	        if (IsExist(userId))
	        {
                return false;
	        }
            //获取邀请详情信息
            YSWL.WeChat.BLL.Core.SceneDetail detailBll=new SceneDetail();
	        YSWL.WeChat.Model.Core.SceneDetail detailModel= detailBll.GetSceneDetail(openId, userOpen);
            if (detailModel == null || detailModel.ReferUserId <= 0)
            {
                return false;
	        }
            YSWL.MALL.BLL.Members.Users userBll=new Users();
	        YSWL.MALL.Model.Members.Users userModel = userBll.GetModel(detailModel.ReferUserId);
	        if (userModel == null)
	        {
                return false;
            }
            if (userModel.UserID == userId)//避免自己成为自己的下级
            {
                return false;
            }
            //邀请加积分
            PointsDetail pointBll = new PointsDetail();
            int pointScore = pointBll.AddPoints(6, userModel.UserID, "邀请用户");//影响分数
            int rankScore = RankDetail.AddScore(6, userModel.UserID, "邀请用户");//影响值
            YSWL.MALL.Model.Members.UserInvite model=new Model.Members.UserInvite();
	        model.CreatedDate = DateTime.Now;
	        model.Depth = 1;
	        model.InviteNick = userModel.NickName;
	        model.InviteUserId = userModel.UserID;
	        model.IsNew = true;
	        model.IsRebate = true;
	        model.Path = ""; 
            model.Status = 1;
	        model.UserId = userId;
	        model.UserNick = userName;
            model.RebateDesc = string.Format("邀请用户+{0}积分,{1}成长值", pointScore, rankScore);
            bool isSuccess= Add(model) > 0;
            if (isSuccess)
            {
                UpdateDepthAndPath(userModel.UserID, model.UserId);

                #region 发送模板消息
                string AppId = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_AppId", -1, "AA");
                string AppSecret = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_AppSercet", -1, "AA");
                string result = YSWL.WeChat.BLL.Core.Utils.GetToken(AppId, AppSecret);
                YSWL.WeChat.Model.Core.User userInfo = YSWL.WeChat.BLL.Core.User.GetUserInfo(openId, detailModel.ReferUserId);
                string templateId = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("WeChat_TemplateId");
                if (!String.IsNullOrWhiteSpace(templateId))
                {
                    //string returnUrl = "/MShop/u/MyAlly";
                    string requestUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state={2}#wechat_redirect";
                    string returnUrl = "http://" + Common.Globals.DomainFullName + "/wcreturn.aspx";
                    string baseUrl = "/MShop/u/MyAlly";
                    string  url= String.Format(requestUrl, AppId, Common.Globals.UrlEncode(returnUrl), openId + "|" + Common.Globals.UrlEncode(baseUrl));


                    YSWL.WeChat.BLL.Core.Utils.SendTemplate(result, userInfo.UserName, templateId,
                        userName, nickName, url);
                }
                #endregion
            }
            return isSuccess;
	    }

	    #endregion  ExtensionMethod
	}
}

