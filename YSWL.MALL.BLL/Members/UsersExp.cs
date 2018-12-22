using System;
using System.Collections.Generic;
using System.Data;
using YSWL.Common;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Members;
using YSWL.MALL.Model.Members;

namespace YSWL.MALL.BLL.Members
{
    /// <summary>
    /// 用户扩展类，继承了USER类相关属性和方法
    /// </summary>
    public class UsersExp : YSWL.MALL.Model.Members.UsersExpModel
    {
        private readonly IUsersExp dal = DAMembers.CreateUsersExp();

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
        public bool Exists(int UserID)
        {
            return dal.Exists(UserID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(YSWL.MALL.Model.Members.UsersExpModel model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.Members.UsersExpModel model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int UserID)
        {

            return dal.Delete(UserID);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string UserIDlist)
        {
            return dal.DeleteList(UserIDlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Members.UsersExpModel GetUsersExpModel(int UserID)
        {

            return dal.GetModel(UserID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public YSWL.MALL.Model.Members.UsersExpModel GetModelByCache(int UserID)
        {

            string CacheKey = "UsersExpModel-" + UserID;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(UserID);
                    if (objModel != null)
                    {
                        int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("CacheTime");
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.Members.UsersExpModel)objModel;
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
        public List<YSWL.MALL.Model.Members.UsersExpModel> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Members.UsersExpModel> DataTableToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Members.UsersExpModel> modelList = new List<YSWL.MALL.Model.Members.UsersExpModel>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Members.UsersExpModel model;
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

        #region 扩展方法

        public bool Add(int userId)
        {
            return dal.Add(userId);
        }

        public DataSet GetUserName(string strUName, int iCount)
        {
            string strWhere = "UserName like '" + strUName + "%' AND Activity=1";
            return dal.GetUserList(iCount, strWhere, "UserName");
        }

        //更新用户的分享数据
        public bool UpdateShareCount()
        {
            return dal.UpdateShareCount();
        }

        //更新用户的商品数据
        public bool UpdateProductCount()
        {
            return dal.UpdateProductCount();
        }

        //更新用户的喜欢数据
        public bool UpdateFavouritesCount()
        {
            return dal.UpdateFavouritesCount();
        }

        //更新用户的专辑数据
        public bool UpdateAblumsCount()
        {
            return dal.UpdateAblumsCount();
        }

        /// <summary>
        /// 获取用户全部属性信息实体
        /// </summary>
        public YSWL.MALL.Model.Members.UsersExpModel GetUsersModel(int UserID)
        {
            //Users
            YSWL.MALL.Model.Members.UsersExpModel model = dal.GetModel(UserID);
            if (model == null)
            {
                model = new UsersExpModel();
            }
            YSWL.Accounts.Bus.User user = new Accounts.Bus.User(UserID);

            model.Activity = user.Activity;
            model.DepartmentID = user.DepartmentID;
            model.Email = user.Email;
            model.EmployeeID = user.EmployeeID;
            model.Phone = user.Phone;
            model.Password = user.Password;
            if (user.Sex != null)
            {
                model.Sex = user.Sex.Trim();
            }
            model.Style = user.Style;
            model.TrueName = user.TrueName;
            model.NickName = user.NickName;
            model.User_cLang = user.User_cLang;
            model.User_dateApprove = user.User_dateApprove;
            model.User_dateCreate = user.User_dateCreate;
            model.User_dateExpire = user.User_dateExpire;
            model.User_dateValid = user.User_dateValid;
            model.User_iApprover = user.User_iApprover;
            model.User_iApproveState = user.User_iApproveState;
            model.User_iCreator = user.User_iCreator;
            model.UserID = user.UserID;
            model.UserName = user.UserName;
            model.UserType = user.UserType;
            return model;
        }

        /// <summary>
        /// 获取用户全部属性信息实体，从缓存中
        /// </summary>
        public YSWL.MALL.Model.Members.UsersExpModel GetUsersExpModelByCache(int UserID)
        {
            string CacheKey = "UsersExpModel-" + UserID;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = GetUsersModel(UserID);
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.Members.UsersExpModel)objModel;
        }

        /// <summary>
        /// 获得用户扩展数据列表
        /// </summary>
        public DataSet GetUsersExpList(string strWhere)
        {
            return dal.GetList(strWhere);
        }

        #endregion 扩展方法

        /// <summary>
        /// 搜索个数
        /// </summary>
        public int GetUserCountByKeyWord(string NickName)
        {
            return dal.GetUserCountByKeyWord(NickName);
        }

        /// <summary>
        /// 搜索分页
        /// </summary>
        /// <param name="NickName"></param>
        public List<YSWL.MALL.Model.Members.UsersExpModel> GetUserListByKeyWord(string NickName, string orderby, int startIndex, int endIndex)
        {
            return DataTableToList(dal.GetUserListByKeyWord(NickName, orderby, startIndex, endIndex).Tables[0]);
        }

        /// <summary>
        /// 是否通过实名认证
        /// </summary>
        public bool UpdateIsDPI(string userIds, int status)
        {
            return dal.UpdateIsDPI(userIds, status);
        }

        public bool UpdatePhoneAndPay(int userid,string account, string phone)
        {
            return dal.UpdatePhoneAndPay(userid,account, phone);
        }
     
        /// <summary>
        /// 获得指定用户ID的全部下属用户
        /// </summary>
        public DataSet GetAllEmpByUserId(int userId)
        {
            return dal.GetAllEmpByUserId(userId);
        }
        /// <summary>
        /// 获取用户余额
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public decimal GetUserBalance(int UserId)
        {
            return dal.GetUserBalance(UserId);
        }


        /// <summary>
        /// 增加一条数据 (用户表和邀请表)事物执行
        /// </summary>
        /// <param name="model"></param>
        /// <param name="inviteID">邀请者UserID</param>
        /// <param name="inviteNick">邀请者昵称</param>
        /// <param name="pointScore">影响积分</param>
        /// <param name="rankScore">影响成长值</param>
        /// <returns></returns>
        public bool AddEx(UsersExpModel model, int inviteID, string inviteNick, int pointScore, int rankScore)
        {
              bool isSuccess= dal.AddEx(model, inviteID, inviteNick, pointScore, rankScore);
            if (isSuccess)//更新层级关系
            {
                YSWL.MALL.BLL.Members.UserInvite inviteBll = new UserInvite();
                inviteBll.UpdateDepthAndPath(inviteID, model.UserID);
            }
            return isSuccess;
        }
      /// <summary>
        /// 增加用户扩展数据
      /// </summary>
      /// <param name="model"></param>
        /// <param name="inviteuid">邀请用户UserID</param>
      /// <returns></returns>
        public bool AddExp(UsersExpModel model,int inviteuid)
        {
            //如果是被邀请用户则执行 AddEx()方法
            if (inviteuid > 0)
            {
                string InviteNick = new Users().GetNickName(inviteuid);
                if (!String.IsNullOrWhiteSpace(InviteNick))
                {
                    //邀请加积分
                    PointsDetail pointBll = new PointsDetail();
                    int pointScore = pointBll.AddPoints(6, inviteuid, "邀请用户");//影响分数
                    int rankScore = RankDetail.AddScore(6, inviteuid, "邀请用户");//影响值
                    bool isSuccess= AddEx(model, inviteuid, InviteNick, pointScore, rankScore);
                    return isSuccess;
                }
            }
            return dal.Add(model);
        }
        /// <summary>
        /// 更新客户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateCustom(UsersExpModel model)
        {
            return dal.UpdateCustom(model);
        }
        /// <summary>
        /// 更新业务员帐号
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateSales(UsersExpModel model)
        {
            return dal.UpdateSales(model);
        }
        /// <summary>
        /// 是否存在业务员
        /// </summary>
        /// <param name="SaleId"></param>
        /// <returns></returns>
        public bool HasSales(int SaleId)
        {
            return dal.HasSales(SaleId);
        }
        /// <summary>
        /// 获取业务员帐号信息
        /// </summary>
        /// <param name="SaleId"></param>
        /// <returns></returns>
        public YSWL.MALL.Model.Members.UsersExpModel GetSalesModel(int SaleId)
        {
            return dal.GetSalesModel(SaleId);
        }
        /// <summary>
        /// 统计用户注册来源
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet SourceCount(string strWhere)
        {
            return dal.SourceCount(strWhere);
        }
        /// <summary>
        /// 增加一条数据 (用户扩展表和收货地址表)事物执行
        /// </summary>
        /// <returns></returns>
        public bool AddEx(Model.Members.UsersExpModel model, Model.Shop.Shipping.ShippingAddress addressModel)
        {
            return dal.AddEx(model, addressModel);
        }

        public bool UpdateEx(Model.Members.UsersExpModel model)
        {
            if (Exists(model.UserID))
            {
                return Update(model);
            }
            else
            {
                model.UserID = model.UserID;
                model.BirthdayVisible = 0;
                model.BirthdayIndexVisible = false;
                model.ConstellationVisible = 0;
                model.ConstellationIndexVisible = false;
                model.NativePlaceVisible = 0;
                model.NativePlaceIndexVisible = false;
                model.RegionId = 0;
                model.AddressVisible = 0;
                model.AddressIndexVisible = false;
                model.BodilyFormVisible = 0;
                model.BodilyFormIndexVisible = false;
                model.BloodTypeVisible = 0;
                model.BloodTypeIndexVisible = false;
                model.MarriagedVisible = 0;
                model.MarriagedIndexVisible = false;
                model.PersonalStatusVisible = 0;
                model.PersonalStatusIndexVisible = false;
                model.LastAccessIP = "";
                model.LastAccessTime = DateTime.Now;
                model.LastLoginTime = DateTime.Now;
                model.LastPostTime = DateTime.Now;
                //注册来源
                model.SourceType = (int)YSWL.MALL.Model.Members.Enum.SourceType.WeChat;
            
                return Add(model);
            } 
    }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool UpdateQQ(int userId, string qq)
        {
            return dal.UpdateQQ(userId, qq);
        }
    }
}