using System;
using System.Data;

namespace YSWL.MALL.IDAL.Members
{
    /// <summary>
    /// 接口层UsersExp
    /// </summary>
    public interface IUsersExp
    {
        #region  成员方法
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
        bool Add(YSWL.MALL.Model.Members.UsersExpModel model);
        /// <summary>
        /// 更新一条数据
        /// </summary>
        bool Update(YSWL.MALL.Model.Members.UsersExpModel model);
        /// <summary>
        /// 删除一条数据
        /// </summary>
        bool Delete(int UserID);
        bool DeleteList(string UserIDlist);
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        YSWL.MALL.Model.Members.UsersExpModel GetModel(int UserID);
        YSWL.MALL.Model.Members.UsersExpModel DataRowToModel(DataRow row);
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
        #endregion  成员方法
        #region 扩展方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        bool Add(int userId);

        bool UpdateFavouritesCount();

        bool UpdateProductCount();

        bool UpdateShareCount();

        bool UpdateAblumsCount();

        DataSet GetUserList(int Top, string strWhere, string filedOrder);

        #endregion

        /// <summary>
        /// 搜索个数
        /// </summary>
        int GetUserCountByKeyWord(string NickName);
        /// <summary>
        /// 搜索分页
        /// </summary>
        /// <param name="NickName"></param>
        DataSet GetUserListByKeyWord(string NickName, string orderby, int startIndex, int endIndex);



        /// <summary>
        /// 是否通过实名认证
        /// </summary>
        bool UpdateIsDPI(string userIds, int status);

        bool UpdatePhoneAndPay(int userid,string account, string phone);

        int GetUserRankId(int UserId);
        decimal GetUserBalance(int UserId);
        
        DataSet GetAllEmpByUserId(int userId);

        /// <summary>
        /// 增加一条数据 (用户表和邀请表)事物执行
        /// </summary>
        /// <param name="model"></param>
        /// <param name="inviteuid">邀请者UserID</param>
        /// <param name="inviteNick">邀请者昵称</param>
        /// <param name="pointScore">影响积分</param>
        /// <param name="rankScore">影响成长值</param>
        /// <returns></returns>
        bool AddEx(Model.Members.UsersExpModel model, int inviteuid, string inviteNick, int pointScore, int  rankScore);

        bool UpdateCustom(Model.Members.UsersExpModel model);

        bool UpdateSales(Model.Members.UsersExpModel model);

         bool HasSales(int SaleId);

         YSWL.MALL.Model.Members.UsersExpModel GetSalesModel(int UserID);

         DataSet SourceCount(string strWhere);
         bool AddEx(Model.Members.UsersExpModel model, Model.Shop.Shipping.ShippingAddress addressModel);
        bool UpdateQQ(int userId, string qq);
 
        } 

}
