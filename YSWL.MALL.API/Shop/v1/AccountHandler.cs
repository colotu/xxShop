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
using YSWL.MALL.Model.SysManage;

namespace YSWL.MALL.API.Shop.v1
{
    public partial class ShopHandler
    {
        #region 用户登录

        [JsonRpcMethod("Login", Idempotent = false)]
        [JsonRpcHelp("用户登录")]
        public JsonObject Login(string UserName, string Password)
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

        #region 用户名是否存在
        [JsonRpcMethod("HasUserByUserName", Idempotent = true)]
        [JsonRpcHelp("用户名是否存在")]
        public JsonObject HasUserByUserName(string UserName)
        {
            if (string.IsNullOrWhiteSpace(UserName)) return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
            YSWL.Accounts.Bus.User BLLUser = new User();
            return Result.HasResult(BLLUser.HasUserByUserName(UserName));
        }
        #endregion

        #region 昵称是否存在
        [JsonRpcMethod("HasUserByNickName", Idempotent = true)]
        [JsonRpcHelp("昵称是否存在")]
        public JsonObject HasUserByNickName(string NickName)
        {
            if (string.IsNullOrWhiteSpace(NickName)) return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
            YSWL.Accounts.Bus.User BLLUser = new User();
            return Result.HasResult(BLLUser.HasUserByNickName(NickName));
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
            json.Put("headImage", "/Upload/User/Gravatar/" + (user.UserID) + ".jpg?id=" + DateTime.Now);
            json.Put("point", user.Points);

            return json;
        }
        #endregion

        #region 更新个人信息
        [JsonRpcMethod("UpdateUserInfo", Idempotent = false)]
        [JsonRpcHelp("更新个人信息")]
        public JsonObject UpdateUserInfo(int UserId, string Password, string NewPassword,
            string Phone, string Email, string NickName)
        {
            if (UserId < 2) return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
            try
            {
                YSWL.Accounts.Bus.User user = new User(UserId);
                //NO DATA
                if (string.IsNullOrWhiteSpace(user.UserType)) return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_NODATA, ERROR_MSG_NODATA));
                //修改密码
                if (!string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(NewPassword))
                {
                    //验证旧密码
                    SiteIdentity siteIdentity = new SiteIdentity(UserId);
                    if (siteIdentity.TestPassword(Password) == 0) 
                    {
                        return new Result(ResultStatus.Failed, Result.FormatFailed("101", "当前密码不正确, 更新个人信息失败!"));
                    }
                    //非普通用户禁用接口
                    if (user.UserType != "UU")
                    {
                        return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_UNAUTHORIZED, ERROR_MSG_UNAUTHORIZED));
                    }
                    user.Password = AccountsPrincipal.EncryptPassword(NewPassword);
                }
                //user.TrueName = TrueName;
                //if (Sex.HasValue) user.Sex = Sex.ToString();
                user.Email = Email;
                user.Phone = Phone;
                user.NickName = NickName;

                return new Result(user.Update());
            }
            catch (Exception ex)
            {
                YSWL.MALL.BLL.SysManage.LogHelp.AddErrorLog(string.Format(ERROR_MSG_LOG, Request.Headers[REQUEST_HEADER_METHOD], ex.Message), ex.StackTrace, Request);
                return new Result(ResultStatus.Error, ex);
            }
        }
        #endregion

        #region 退出
        [JsonRpcMethod("LogOut", Idempotent = false)]
        [JsonRpcHelp("退出")]
        public JsonObject LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Remove(Globals.SESSIONKEY_USER);
            Session.Clear();
            Session.Abandon();
            return new Result(ResultStatus.Success,"Success");
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
        [JsonRpcMethod("RegisterUser", Idempotent = false)]
        [JsonRpcHelp("用户注册")]
        public  JsonObject Register(string UserName, string Password, string NickName,
            string TrueName, string Email, string Phone, string MobileDI, string MobileDMID)
        {
            JsonObject jsonObject = new JsonObject();
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password)) return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
            YSWL.Accounts.Bus.User user = new User();
            user.UserName = UserName;
            user.TrueName = TrueName;
            user.Email = Email;
            user.Phone = Phone;
            user.NickName = NickName;

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
                user.Password = AccountsPrincipal.EncryptPassword(Password);
                user.UserID = user.Create();
                if (user.UserID == -100)
                {
                    //用户已存在
                    return new Result(ResultStatus.Failed, Result.FormatFailed("101", "用户已存在!"));
                }
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
            jsonObject.Put("nickName", user.NickName);
            jsonObject.Put("headImage", "/Upload/User/Gravatar/" + (user.UserID) + ".jpg?id=" + DateTime.Now);

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

        #region 注册协议
        [JsonRpcMethod("registerStatement", Idempotent = false)]
        [JsonRpcHelp("注册协议")]
        public JsonObject GetProtocal()
        {
            YSWL.MALL.BLL.SysManage.WebSiteSet WebSiteSet = new YSWL.MALL.BLL.SysManage.WebSiteSet(ApplicationKeyType.System);
            JsonObject jsonObject = new JsonObject();
            string registerStatement = WebSiteSet.RegistStatement;
            return new Result(ResultStatus.Success, registerStatement);
        }
        #endregion
 
    }
}