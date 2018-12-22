/**
* UserHandler.cs
*
* 功 能： 商城 API
* 类 名： UserHandler
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/12/24 17:04:23  Ben    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using YSWL.Accounts.Bus;
using YSWL.MALL.BLL.Members;
using YSWL.MALL.BLL.SysManage;
using YSWL.Components.Handlers.API;
using YSWL.Json;
using YSWL.Json.RPC;
using YSWL.MALL.Model.Members;
using System.Text;
using YSWL.MALL.Model.Shop;
using System.Collections.Generic;
using Webdiyer.WebControls.Mvc;
using System.Web.Security;
using YSWL.Common;
using YSWL.Components;
using YSWL.MALL.Model.SysManage;
using YSWL.MALL.ViewModel.Shop;

namespace YSWL.MALL.API.Shop.v2
{
    public partial class ShopHandler
    {
        #region 用户登录

        [JsonRpcMethod("LoginV2", Idempotent = false)]
        [JsonRpcHelp("用户登录")]
        public  JsonObject LoginV2(string UserName, string Password,string enterprisestr)
        {
            if (string.IsNullOrWhiteSpace(UserName))
                return new Result(ResultStatus.Failed,
                    Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));

            try
            {
                AccountsPrincipal userPrincipal = AccountsPrincipal.ValidateLogin(UserName, Password);
                //登录失败，请确认用户名或密码是否正确。
                if (userPrincipal == null)
                {
                    LogHelp.AddUserLog(UserName, "", "登录失败!", Request);
                    return new Result(ResultStatus.Failed, Result.FormatFailed("40", "登录失败，请确认用户名或密码是否正确。"));
                }

                User currentUser = new YSWL.Accounts.Bus.User(userPrincipal);
                //您非普通用户，您没有权限使用接口系统！
                if (currentUser.UserType != "UU")
                {
                    return new Result(ResultStatus.Failed,
                        Result.FormatFailed(ERROR_CODE_UNAUTHORIZED, ERROR_MSG_UNAUTHORIZED));
                }

                Context.User = userPrincipal;
                //密码错误！
                if (((SiteIdentity)User.Identity).TestPassword(Password) == 0)
                {
                    YSWL.MALL.BLL.SysManage.LogHelp.AddUserLog(UserName, "", "密码错误！", Request);
                    return new Result(ResultStatus.Failed, Result.FormatFailed("42", "密码错误！"));
                }
                //对不起，该帐号已被冻结，请联系管理员！
                if (!currentUser.Activity)
                {
                    return new Result(ResultStatus.Failed, Result.FormatFailed("44", "对不起，该帐号已被冻结，请联系管理员！"));
                }
                //登录成功
                LogHelp.AddUserLog(currentUser.UserName, currentUser.UserType, "登录成功", Request);
                return new Result(ResultStatus.Success, GetUserInfo4Json(currentUser));
            }
            catch (Exception ex)
            {
                YSWL.MALL.BLL.SysManage.LogHelp.AddErrorLog(string.Format(ERROR_MSG_LOG, Request.Headers[REQUEST_HEADER_METHOD], ex.Message),
                    ex.StackTrace, Request);
                return new Result(ResultStatus.Error, ex);
            }
        }

        #endregion

        #region 获取个人信息

        private JsonObject GetUserInfo4Json(User userInfo)
        {
            YSWL.MALL.BLL.Members.UsersExp BLL = new YSWL.MALL.BLL.Members.UsersExp();
            UsersExpModel user = BLL.GetUsersModel(userInfo.UserID);
            if (userInfo == null || string.IsNullOrWhiteSpace(userInfo.UserType)) return null;
            JsonObject json = new JsonObject();
            json.Put("userId", user.UserID);
            json.Put("userName", user.UserName);
            json.Put("trueName", user.TrueName);
            json.Put("phone", user.Phone);
            json.Put("level", user.UserType);
            //json.Put("departmentID", userInfo.DepartmentID);
            json.Put("nickName", user.NickName);
        
            json.Put("isOpenMultiDepot", YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot());

            #region  企业字符串
            string enterpriseStr = "";
            string enterpriseId = "";
            json.Put("enterpriseStr", enterpriseStr);
            json.Put("enterpriseId", enterpriseId); 
            #endregion 

            json.Put("headImage", "/Upload/User/Gravatar/" + (user.UserID) + ".jpg?id=" + DateTime.Now);
            json.Put("point", user.Points);
            return json;
        }
        #endregion


        #region 注册用户
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="Password">密码</param>
        /// <param name="NickName">昵称</param>
        /// <param name="TrueName">真实姓名</param>
        /// <param name="Email">邮箱</param>
        /// <param name="Phone">手机</param>
        /// <param name="MobileDI">设备信息标识</param>
        /// <param name="MobileDMID">识设备型号及平台ID</param>
        /// <returns>新用户ID</returns>
        [JsonRpcMethod("RegisterV2", Idempotent = false)]
        [JsonRpcHelp("用户注册")]
        public JsonObject Register(string UserName, string Password, 
            string TrueName, string Email, string Phone, string MobileDI, string MobileDMID)
        {
            JsonObject jsonObject = new JsonObject();
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
            {
                return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
            }
            YSWL.Accounts.Bus.User user = new User();
            user.UserName = UserName;
            user.TrueName = TrueName;
            user.Email = Email;
            user.Phone = String.IsNullOrWhiteSpace(Phone)? UserName: Phone;
            user.NickName = TrueName;

            user.Sex = "1";
            user.EmployeeID = 0;
            user.DepartmentID = "-1";
            user.Activity = true;
            user.UserType = "UU";
            user.Style = 1;
            user.User_dateCreate = DateTime.Now;
            user.User_dateValid = DateTime.Now;
            user.User_cLang = "zh-CN";

            try
            {

                long enterpriseId = Common.Globals.SafeLong(YSWL.Common.CallContextHelper.GetAutoTag(), 0);
                user.Password = AccountsPrincipal.EncryptPassword(Password);
                user.UserID = user.Create();
                if (user.UserID == -100)
                {
                    //用户已存在
                    return new Result(ResultStatus.Failed, Result.FormatFailed("101", "用户已存在!"));
                }

                #region  清空客户缓存
                YSWL.DBUtility.SAASInfo.ClearCacheCusts(enterpriseId);
                #endregion
                //添加用户角色
                user.AddToRole(YSWL.MALL.BLL.SysManage.ConfigSystem.GetIntValueByCache("DefaultEmpRoleID"));
                YSWL.MALL.BLL.Members.UsersExp BLLUsersExp = new YSWL.MALL.BLL.Members.UsersExp();
                BLLUsersExp.Add(new UsersExpModel
                {
                    SourceType = (int)YSWL.MALL.Model.Members.Enum.SourceType.Ding,
                    UserID = user.UserID,
                    LastAccessTime = DateTime.Now,
                    LastLoginTime = DateTime.Now,
                    LastPostTime = DateTime.Now
                });
                YSWL.MALL.BLL.Members.PointsDetail pointBLL = new YSWL.MALL.BLL.Members.PointsDetail();
                pointBLL.AddPoints(2, user.UserID, "注册成功");
                YSWL.MALL.BLL.Members.RankDetail.AddScore(2, user.UserID, "注册成功");

            }
            catch (Exception ex)
            {
                YSWL.MALL.BLL.SysManage.LogHelp.AddErrorLog(string.Format(ERROR_MSG_LOG, Request.Headers[REQUEST_HEADER_METHOD], ex.Message), ex.StackTrace, Request);
                return new Result(ResultStatus.Error, ex);
            }
            YSWL.MALL.BLL.Members.UsersExp BLL = new YSWL.MALL.BLL.Members.UsersExp();
            UsersExpModel usersExp = BLL.GetUsersModel(user.UserID);
            jsonObject.Put("userId", user.UserID);
            jsonObject.Put("userName", user.UserName);
            jsonObject.Put("phone", user.Phone);
            jsonObject.Put("level", user.UserType);
            jsonObject.Put("trueName", user.TrueName);

            if (null != usersExp)
            {
                jsonObject.Put("point", usersExp.Points);
            }
            else
            {
                jsonObject.Put("point", "0");
            }
            jsonObject.Put("userName", user.UserName);
            return new Result(ResultStatus.Success, jsonObject);
        }
        #endregion
    }
}