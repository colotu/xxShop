using System.Data;
using YSWL.MALL.Model.Shop.Order;
using System;

namespace YSWL.MALL.IDAL.Members
{
    /// <summary>
    /// 接口层Users
    /// </summary>
    public interface IUsers
    {
        #region 成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        int GetMaxId();

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        bool Exists(int UserID);

        /// <summary>
        /// 增加一条数据
        /// </summary>
        int Add(YSWL.MALL.Model.Members.Users model);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        bool Update(YSWL.MALL.Model.Members.Users model);

        /// <summary>
        /// 删除一条数据
        /// </summary>
        bool Delete(int UserID);

        bool DeleteList(string UserIDlist);

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        YSWL.MALL.Model.Members.Users GetModel(int UserID);

        /// <summary>
        /// 获得数据列表
        /// </summary>
        DataSet GetList(string strWhere);

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        DataSet GetList(int Top, string strWhere, string filedOrder);

        int GetRecordCount(string strWhere);

        DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex);

        /// <summary>
        /// 根据分页获得数据列表
        /// </summary>
        //DataSet GetList(int PageSize,int PageIndex,string strWhere);

        #endregion 成员方法

        #region 扩展的成员方法

        /// <summary>
        /// 根据DepartmentID删除一条数据
        /// </summary>
        bool DeleteByDepartmentID(int DepartmentID);

        /// <summary>
        /// 根据DepartmentID批量删除数据
        /// </summary>
        bool DeleteListByDepartmentID(string DepartmentIDlist);

        bool DeleteUserListByUserID(string serInuserid);

        bool ExistByPhone(string Phone);

        bool ExistsByEmail(string Email);

        /// <summary>
        ///根据用户ID判断昵称是否已被其他用户使用
        /// </summary>
        bool ExistsNickName(string nickname);

        /// <summary>
        ///根据用户ID判断昵称是否已被其他用户使用
        /// </summary>
        bool ExistsNickName(int userid, string nickname);

        #endregion 扩展的成员方法

        DataSet GetList(string type, string keyWord);

        DataSet GetListEX(string keyWord);

        DataSet GetListEXByType(string type, string keyWord = "");
        /// <summary>
        /// 搜索用户
        /// </summary>
        DataSet GetSearchList(string type, string StrWhere = "");
        /// <summary>
        /// 一键更新用户的粉丝数和关注数
        /// </summary>
        bool UpdateFansAndFellowCount();

        /// <summary>
        /// 通过昵称获得用户的userid @某人的时候用到
        /// </summary>
        /// <param name="NickName">昵称</param>
        /// <returns></returns>
        int GetUserIdByNickName(string NickName);

        string GetUserName(int userId);

        int GetUserIdByDepartmentID(string DepartmentID);
        
        /// <summary>
        /// 批量冻结或解冻
        /// </summary>
        bool UpdateActiveStatus(string Ids, int ActiveType);


        int GetDefaultUserId();
        string GetNickName(int userId);

       bool DeleteEx(int userId);

        YSWL.MALL.Model.Members.Users GetModel(string userName);

        DataSet GetUserCount(StatisticMode mode, DateTime startDate, DateTime endDate);

        bool ExistsUserName(int userid, string username);


        bool UpdateSales(string userIds, int salesId);

        /// <summary>
        /// 用户注册统计--日期
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        DataSet GetDayRegCount(DateTime startDate, DateTime endDate, StatisticMode mode);


        DataSet GetCustList(int userId, int IsAct = -1, string KeyWord = "", string startDate = "");
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        bool Exists(int EmployeeID, int UserID);

        DataSet SalesRegisters(DateTime startDay, DateTime endDay);

        int GetSalesRegs(int SalesId, string startDay, string endDay);

        DataSet GetSalesRegList(int SalesId, string startDay, string endDay, int dateType);
        DataSet GetShopByRegion(int regionId, int supplierId);

        YSWL.MALL.Model.Members.Users GetInviteUser(int userId);

        bool SetSalesInfo(int salesId, string idlist);

       
        /// <summary>
        /// 通过用户名获得用户的真实姓名 zhou20160104xiugai
        /// </summary>
        /// <param name="NickName">用户名</param>
        /// <returns></returns>
        string GetUserTrueNameByUsername(string UserName);


        int GetUserIdByUserName(string UserName);

        int GetEmployeeIDByUserid(string Userid);

        //zhou20180104xiugai
        bool SetEmpidByUserid(string userId, string EmployeeID);

        //zhou20160104xiugai
        bool ExistsUserName(string username);

        string ExistsUserVIP(string username);

        //zhou20160104xiugai
        bool UpvipUserName(string username);
    }
}