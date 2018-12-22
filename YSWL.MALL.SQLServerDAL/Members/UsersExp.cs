using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using YSWL.DBUtility;
using YSWL.MALL.IDAL.Members;

namespace YSWL.MALL.SQLServerDAL.Members
{
    /// <summary>
    /// 用户扩展类
    /// </summary>
    public partial class UsersExp : IUsersExp
    {
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DBHelper.DefaultDBHelper.GetMaxID("UserID", "Accounts_UsersExp");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int UserID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Accounts_UsersExp");
            strSql.Append(" where UserID=@UserID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)			};
            parameters[0].Value = UserID;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(YSWL.MALL.Model.Members.UsersExpModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Accounts_UsersExp(");
            strSql.Append("UserID,Gravatar,Singature,TelPhone,QQ,MSN,HomePage,Birthday,BirthdayVisible,BirthdayIndexVisible,Constellation,ConstellationVisible,ConstellationIndexVisible,NativePlace,NativePlaceVisible,NativePlaceIndexVisible,RegionId,Address,AddressVisible,AddressIndexVisible,BodilyForm,BodilyFormVisible,BodilyFormIndexVisible,BloodType,BloodTypeVisible,BloodTypeIndexVisible,Marriaged,MarriagedVisible,MarriagedIndexVisible,PersonalStatus,PersonalStatusVisible,PersonalStatusIndexVisible,Grade,Balance,BalanceCredit,Points,RankScore,TopicCount,ReplyTopicCount,FavTopicCount,PvCount,FansCount,FellowCount,AblumsCount,FavouritesCount,FavoritedCount,ShareCount,ProductsCount,PersonalDomain,LastAccessTime,LastAccessIP,LastPostTime,LastLoginTime,Remark,IsUserDPI,PayAccount,UserCardCode,UserCardType,SourceType,SalesId)");
            strSql.Append(" values (");
            strSql.Append("@UserID,@Gravatar,@Singature,@TelPhone,@QQ,@MSN,@HomePage,@Birthday,@BirthdayVisible,@BirthdayIndexVisible,@Constellation,@ConstellationVisible,@ConstellationIndexVisible,@NativePlace,@NativePlaceVisible,@NativePlaceIndexVisible,@RegionId,@Address,@AddressVisible,@AddressIndexVisible,@BodilyForm,@BodilyFormVisible,@BodilyFormIndexVisible,@BloodType,@BloodTypeVisible,@BloodTypeIndexVisible,@Marriaged,@MarriagedVisible,@MarriagedIndexVisible,@PersonalStatus,@PersonalStatusVisible,@PersonalStatusIndexVisible,@Grade,@Balance,@BalanceCredit,@Points,@RankScore,@TopicCount,@ReplyTopicCount,@FavTopicCount,@PvCount,@FansCount,@FellowCount,@AblumsCount,@FavouritesCount,@FavoritedCount,@ShareCount,@ProductsCount,@PersonalDomain,@LastAccessTime,@LastAccessIP,@LastPostTime,@LastLoginTime,@Remark,@IsUserDPI,@PayAccount,@UserCardCode,@UserCardType,@SourceType,@SalesId)");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@Gravatar", SqlDbType.NVarChar,200),
					new SqlParameter("@Singature", SqlDbType.NVarChar,200),
					new SqlParameter("@TelPhone", SqlDbType.NVarChar,50),
					new SqlParameter("@QQ", SqlDbType.NVarChar,50),
					new SqlParameter("@MSN", SqlDbType.NVarChar,50),
					new SqlParameter("@HomePage", SqlDbType.NVarChar,50),
					new SqlParameter("@Birthday", SqlDbType.DateTime),
					new SqlParameter("@BirthdayVisible", SqlDbType.SmallInt,2),
					new SqlParameter("@BirthdayIndexVisible", SqlDbType.Bit,1),
					new SqlParameter("@Constellation", SqlDbType.NVarChar,50),
					new SqlParameter("@ConstellationVisible", SqlDbType.SmallInt,2),
					new SqlParameter("@ConstellationIndexVisible", SqlDbType.Bit,1),
					new SqlParameter("@NativePlace", SqlDbType.NVarChar,300),
					new SqlParameter("@NativePlaceVisible", SqlDbType.SmallInt,2),
					new SqlParameter("@NativePlaceIndexVisible", SqlDbType.Bit,1),
					new SqlParameter("@RegionId", SqlDbType.Int,4),
					new SqlParameter("@Address", SqlDbType.NVarChar,300),
					new SqlParameter("@AddressVisible", SqlDbType.SmallInt,2),
					new SqlParameter("@AddressIndexVisible", SqlDbType.Bit,1),
					new SqlParameter("@BodilyForm", SqlDbType.NVarChar,10),
					new SqlParameter("@BodilyFormVisible", SqlDbType.SmallInt,2),
					new SqlParameter("@BodilyFormIndexVisible", SqlDbType.Bit,1),
					new SqlParameter("@BloodType", SqlDbType.NVarChar,10),
					new SqlParameter("@BloodTypeVisible", SqlDbType.SmallInt,2),
					new SqlParameter("@BloodTypeIndexVisible", SqlDbType.Bit,1),
					new SqlParameter("@Marriaged", SqlDbType.NVarChar,10),
					new SqlParameter("@MarriagedVisible", SqlDbType.SmallInt,2),
					new SqlParameter("@MarriagedIndexVisible", SqlDbType.Bit,1),
					new SqlParameter("@PersonalStatus", SqlDbType.NVarChar,10),
					new SqlParameter("@PersonalStatusVisible", SqlDbType.SmallInt,2),
					new SqlParameter("@PersonalStatusIndexVisible", SqlDbType.Bit,1),
					new SqlParameter("@Grade", SqlDbType.Int,4),
					new SqlParameter("@Balance", SqlDbType.Money,8),
					new SqlParameter("@BalanceCredit", SqlDbType.Money,8),
					new SqlParameter("@Points", SqlDbType.Int,4),
					new SqlParameter("@RankScore", SqlDbType.Int,4),
					new SqlParameter("@TopicCount", SqlDbType.Int,4),
					new SqlParameter("@ReplyTopicCount", SqlDbType.Int,4),
					new SqlParameter("@FavTopicCount", SqlDbType.Int,4),
					new SqlParameter("@PvCount", SqlDbType.Int,4),
					new SqlParameter("@FansCount", SqlDbType.Int,4),
					new SqlParameter("@FellowCount", SqlDbType.Int,4),
					new SqlParameter("@AblumsCount", SqlDbType.Int,4),
					new SqlParameter("@FavouritesCount", SqlDbType.Int,4),
					new SqlParameter("@FavoritedCount", SqlDbType.Int,4),
					new SqlParameter("@ShareCount", SqlDbType.Int,4),
					new SqlParameter("@ProductsCount", SqlDbType.Int,4),
					new SqlParameter("@PersonalDomain", SqlDbType.NVarChar,50),
					new SqlParameter("@LastAccessTime", SqlDbType.DateTime),
					new SqlParameter("@LastAccessIP", SqlDbType.NVarChar,50),
					new SqlParameter("@LastPostTime", SqlDbType.DateTime),
					new SqlParameter("@LastLoginTime", SqlDbType.DateTime),
					new SqlParameter("@Remark", SqlDbType.NVarChar,-1),
					new SqlParameter("@IsUserDPI", SqlDbType.Bit,1),
					new SqlParameter("@PayAccount", SqlDbType.NVarChar,200),
					new SqlParameter("@UserCardCode", SqlDbType.NVarChar,50),
					new SqlParameter("@UserCardType", SqlDbType.SmallInt,2),
					new SqlParameter("@SourceType", SqlDbType.Int,4),
					new SqlParameter("@SalesId", SqlDbType.Int,4)};
            parameters[0].Value = model.UserID;
            parameters[1].Value = model.Gravatar;
            parameters[2].Value = model.Singature;
            parameters[3].Value = model.TelPhone;
            parameters[4].Value = model.QQ;
            parameters[5].Value = model.MSN;
            parameters[6].Value = model.HomePage;
            parameters[7].Value = model.Birthday;
            parameters[8].Value = model.BirthdayVisible;
            parameters[9].Value = model.BirthdayIndexVisible;
            parameters[10].Value = model.Constellation;
            parameters[11].Value = model.ConstellationVisible;
            parameters[12].Value = model.ConstellationIndexVisible;
            parameters[13].Value = model.NativePlace;
            parameters[14].Value = model.NativePlaceVisible;
            parameters[15].Value = model.NativePlaceIndexVisible;
            parameters[16].Value = model.RegionId;
            parameters[17].Value = model.Address;
            parameters[18].Value = model.AddressVisible;
            parameters[19].Value = model.AddressIndexVisible;
            parameters[20].Value = model.BodilyForm;
            parameters[21].Value = model.BodilyFormVisible;
            parameters[22].Value = model.BodilyFormIndexVisible;
            parameters[23].Value = model.BloodType;
            parameters[24].Value = model.BloodTypeVisible;
            parameters[25].Value = model.BloodTypeIndexVisible;
            parameters[26].Value = model.Marriaged;
            parameters[27].Value = model.MarriagedVisible;
            parameters[28].Value = model.MarriagedIndexVisible;
            parameters[29].Value = model.PersonalStatus;
            parameters[30].Value = model.PersonalStatusVisible;
            parameters[31].Value = model.PersonalStatusIndexVisible;
            parameters[32].Value = model.Grade;
            parameters[33].Value = model.Balance;
            parameters[34].Value = model.BalanceCredit;
            parameters[35].Value = model.Points;
            parameters[36].Value = model.RankScore;
            parameters[37].Value = model.TopicCount;
            parameters[38].Value = model.ReplyTopicCount;
            parameters[39].Value = model.FavTopicCount;
            parameters[40].Value = model.PvCount;
            parameters[41].Value = model.FansCount;
            parameters[42].Value = model.FellowCount;
            parameters[43].Value = model.AblumsCount;
            parameters[44].Value = model.FavouritesCount;
            parameters[45].Value = model.FavoritedCount;
            parameters[46].Value = model.ShareCount;
            parameters[47].Value = model.ProductsCount;
            parameters[48].Value = model.PersonalDomain;
            parameters[49].Value = model.LastAccessTime;
            parameters[50].Value = model.LastAccessIP;
            parameters[51].Value = model.LastPostTime;
            parameters[52].Value = model.LastLoginTime;
            parameters[53].Value = model.Remark;
            parameters[54].Value = model.IsUserDPI;
            parameters[55].Value = model.PayAccount;
            parameters[56].Value = model.UserCardCode;
            parameters[57].Value = model.UserCardType;
            parameters[58].Value = model.SourceType;
            parameters[59].Value = model.SalesId;

            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.Members.UsersExpModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Accounts_UsersExp set ");
            strSql.Append("Gravatar=@Gravatar,");
            strSql.Append("Singature=@Singature,");
            strSql.Append("TelPhone=@TelPhone,");
            strSql.Append("QQ=@QQ,");
            strSql.Append("MSN=@MSN,");
            strSql.Append("HomePage=@HomePage,");
            strSql.Append("Birthday=@Birthday,");
            strSql.Append("BirthdayVisible=@BirthdayVisible,");
            strSql.Append("BirthdayIndexVisible=@BirthdayIndexVisible,");
            strSql.Append("Constellation=@Constellation,");
            strSql.Append("ConstellationVisible=@ConstellationVisible,");
            strSql.Append("ConstellationIndexVisible=@ConstellationIndexVisible,");
            strSql.Append("NativePlace=@NativePlace,");
            strSql.Append("NativePlaceVisible=@NativePlaceVisible,");
            strSql.Append("NativePlaceIndexVisible=@NativePlaceIndexVisible,");
            strSql.Append("RegionId=@RegionId,");
            strSql.Append("Address=@Address,");
            strSql.Append("AddressVisible=@AddressVisible,");
            strSql.Append("AddressIndexVisible=@AddressIndexVisible,");
            strSql.Append("BodilyForm=@BodilyForm,");
            strSql.Append("BodilyFormVisible=@BodilyFormVisible,");
            strSql.Append("BodilyFormIndexVisible=@BodilyFormIndexVisible,");
            strSql.Append("BloodType=@BloodType,");
            strSql.Append("BloodTypeVisible=@BloodTypeVisible,");
            strSql.Append("BloodTypeIndexVisible=@BloodTypeIndexVisible,");
            strSql.Append("Marriaged=@Marriaged,");
            strSql.Append("MarriagedVisible=@MarriagedVisible,");
            strSql.Append("MarriagedIndexVisible=@MarriagedIndexVisible,");
            strSql.Append("PersonalStatus=@PersonalStatus,");
            strSql.Append("PersonalStatusVisible=@PersonalStatusVisible,");
            strSql.Append("PersonalStatusIndexVisible=@PersonalStatusIndexVisible,");
            strSql.Append("Grade=@Grade,");
            strSql.Append("Balance=@Balance,");
            strSql.Append("BalanceCredit=@BalanceCredit,");
            strSql.Append("Points=@Points,");
            strSql.Append("RankScore=@RankScore,");
            strSql.Append("TopicCount=@TopicCount,");
            strSql.Append("ReplyTopicCount=@ReplyTopicCount,");
            strSql.Append("FavTopicCount=@FavTopicCount,");
            strSql.Append("PvCount=@PvCount,");
            strSql.Append("FansCount=@FansCount,");
            strSql.Append("FellowCount=@FellowCount,");
            strSql.Append("AblumsCount=@AblumsCount,");
            strSql.Append("FavouritesCount=@FavouritesCount,");
            strSql.Append("FavoritedCount=@FavoritedCount,");
            strSql.Append("ShareCount=@ShareCount,");
            strSql.Append("ProductsCount=@ProductsCount,");
            strSql.Append("PersonalDomain=@PersonalDomain,");
            strSql.Append("LastAccessTime=@LastAccessTime,");
            strSql.Append("LastAccessIP=@LastAccessIP,");
            strSql.Append("LastPostTime=@LastPostTime,");
            strSql.Append("LastLoginTime=@LastLoginTime,");
            strSql.Append("Remark=@Remark,");
            strSql.Append("IsUserDPI=@IsUserDPI,");
            strSql.Append("PayAccount=@PayAccount,");
            strSql.Append("UserCardCode=@UserCardCode,");
            strSql.Append("UserCardType=@UserCardType,");
            strSql.Append("SourceType=@SourceType,");
            strSql.Append("SalesId=@SalesId");
            strSql.Append(" where UserID=@UserID ");
            SqlParameter[] parameters = {
					new SqlParameter("@Gravatar", SqlDbType.NVarChar,200),
					new SqlParameter("@Singature", SqlDbType.NVarChar,200),
					new SqlParameter("@TelPhone", SqlDbType.NVarChar,50),
					new SqlParameter("@QQ", SqlDbType.NVarChar,50),
					new SqlParameter("@MSN", SqlDbType.NVarChar,50),
					new SqlParameter("@HomePage", SqlDbType.NVarChar,50),
					new SqlParameter("@Birthday", SqlDbType.DateTime),
					new SqlParameter("@BirthdayVisible", SqlDbType.SmallInt,2),
					new SqlParameter("@BirthdayIndexVisible", SqlDbType.Bit,1),
					new SqlParameter("@Constellation", SqlDbType.NVarChar,50),
					new SqlParameter("@ConstellationVisible", SqlDbType.SmallInt,2),
					new SqlParameter("@ConstellationIndexVisible", SqlDbType.Bit,1),
					new SqlParameter("@NativePlace", SqlDbType.NVarChar,300),
					new SqlParameter("@NativePlaceVisible", SqlDbType.SmallInt,2),
					new SqlParameter("@NativePlaceIndexVisible", SqlDbType.Bit,1),
					new SqlParameter("@RegionId", SqlDbType.Int,4),
					new SqlParameter("@Address", SqlDbType.NVarChar,300),
					new SqlParameter("@AddressVisible", SqlDbType.SmallInt,2),
					new SqlParameter("@AddressIndexVisible", SqlDbType.Bit,1),
					new SqlParameter("@BodilyForm", SqlDbType.NVarChar,10),
					new SqlParameter("@BodilyFormVisible", SqlDbType.SmallInt,2),
					new SqlParameter("@BodilyFormIndexVisible", SqlDbType.Bit,1),
					new SqlParameter("@BloodType", SqlDbType.NVarChar,10),
					new SqlParameter("@BloodTypeVisible", SqlDbType.SmallInt,2),
					new SqlParameter("@BloodTypeIndexVisible", SqlDbType.Bit,1),
					new SqlParameter("@Marriaged", SqlDbType.NVarChar,10),
					new SqlParameter("@MarriagedVisible", SqlDbType.SmallInt,2),
					new SqlParameter("@MarriagedIndexVisible", SqlDbType.Bit,1),
					new SqlParameter("@PersonalStatus", SqlDbType.NVarChar,10),
					new SqlParameter("@PersonalStatusVisible", SqlDbType.SmallInt,2),
					new SqlParameter("@PersonalStatusIndexVisible", SqlDbType.Bit,1),
					new SqlParameter("@Grade", SqlDbType.Int,4),
					new SqlParameter("@Balance", SqlDbType.Money,8),
					new SqlParameter("@BalanceCredit", SqlDbType.Money,8),
					new SqlParameter("@Points", SqlDbType.Int,4),
					new SqlParameter("@RankScore", SqlDbType.Int,4),
					new SqlParameter("@TopicCount", SqlDbType.Int,4),
					new SqlParameter("@ReplyTopicCount", SqlDbType.Int,4),
					new SqlParameter("@FavTopicCount", SqlDbType.Int,4),
					new SqlParameter("@PvCount", SqlDbType.Int,4),
					new SqlParameter("@FansCount", SqlDbType.Int,4),
					new SqlParameter("@FellowCount", SqlDbType.Int,4),
					new SqlParameter("@AblumsCount", SqlDbType.Int,4),
					new SqlParameter("@FavouritesCount", SqlDbType.Int,4),
					new SqlParameter("@FavoritedCount", SqlDbType.Int,4),
					new SqlParameter("@ShareCount", SqlDbType.Int,4),
					new SqlParameter("@ProductsCount", SqlDbType.Int,4),
					new SqlParameter("@PersonalDomain", SqlDbType.NVarChar,50),
					new SqlParameter("@LastAccessTime", SqlDbType.DateTime),
					new SqlParameter("@LastAccessIP", SqlDbType.NVarChar,50),
					new SqlParameter("@LastPostTime", SqlDbType.DateTime),
					new SqlParameter("@LastLoginTime", SqlDbType.DateTime),
					new SqlParameter("@Remark", SqlDbType.NVarChar,-1),
					new SqlParameter("@IsUserDPI", SqlDbType.Bit,1),
					new SqlParameter("@PayAccount", SqlDbType.NVarChar,200),
					new SqlParameter("@UserCardCode", SqlDbType.NVarChar,50),
					new SqlParameter("@UserCardType", SqlDbType.SmallInt,2),
					new SqlParameter("@SourceType", SqlDbType.Int,4),
					new SqlParameter("@SalesId", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4)};
            parameters[0].Value = model.Gravatar;
            parameters[1].Value = model.Singature;
            parameters[2].Value = model.TelPhone;
            parameters[3].Value = model.QQ;
            parameters[4].Value = model.MSN;
            parameters[5].Value = model.HomePage;
            parameters[6].Value = model.Birthday;
            parameters[7].Value = model.BirthdayVisible;
            parameters[8].Value = model.BirthdayIndexVisible;
            parameters[9].Value = model.Constellation;
            parameters[10].Value = model.ConstellationVisible;
            parameters[11].Value = model.ConstellationIndexVisible;
            parameters[12].Value = model.NativePlace;
            parameters[13].Value = model.NativePlaceVisible;
            parameters[14].Value = model.NativePlaceIndexVisible;
            parameters[15].Value = model.RegionId;
            parameters[16].Value = model.Address;
            parameters[17].Value = model.AddressVisible;
            parameters[18].Value = model.AddressIndexVisible;
            parameters[19].Value = model.BodilyForm;
            parameters[20].Value = model.BodilyFormVisible;
            parameters[21].Value = model.BodilyFormIndexVisible;
            parameters[22].Value = model.BloodType;
            parameters[23].Value = model.BloodTypeVisible;
            parameters[24].Value = model.BloodTypeIndexVisible;
            parameters[25].Value = model.Marriaged;
            parameters[26].Value = model.MarriagedVisible;
            parameters[27].Value = model.MarriagedIndexVisible;
            parameters[28].Value = model.PersonalStatus;
            parameters[29].Value = model.PersonalStatusVisible;
            parameters[30].Value = model.PersonalStatusIndexVisible;
            parameters[31].Value = model.Grade;
            parameters[32].Value = model.Balance;
            parameters[33].Value = model.BalanceCredit;
            parameters[34].Value = model.Points;
            parameters[35].Value = model.RankScore;
            parameters[36].Value = model.TopicCount;
            parameters[37].Value = model.ReplyTopicCount;
            parameters[38].Value = model.FavTopicCount;
            parameters[39].Value = model.PvCount;
            parameters[40].Value = model.FansCount;
            parameters[41].Value = model.FellowCount;
            parameters[42].Value = model.AblumsCount;
            parameters[43].Value = model.FavouritesCount;
            parameters[44].Value = model.FavoritedCount;
            parameters[45].Value = model.ShareCount;
            parameters[46].Value = model.ProductsCount;
            parameters[47].Value = model.PersonalDomain;
            parameters[48].Value = model.LastAccessTime;
            parameters[49].Value = model.LastAccessIP;
            parameters[50].Value = model.LastPostTime;
            parameters[51].Value = model.LastLoginTime;
            parameters[52].Value = model.Remark;
            parameters[53].Value = model.IsUserDPI;
            parameters[54].Value = model.PayAccount;
            parameters[55].Value = model.UserCardCode;
            parameters[56].Value = model.UserCardType;
            parameters[57].Value = model.SourceType;
            parameters[58].Value = model.SalesId;
            parameters[59].Value = model.UserID;
           // int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
            try
            {
                int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int UserID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Accounts_UsersExp ");
            strSql.Append(" where UserID=@UserID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)			};
            parameters[0].Value = UserID;

            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string UserIDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Accounts_UsersExp ");
            strSql.Append(" where UserID in (" + UserIDlist + ")  ");
            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Members.UsersExpModel GetModel(int UserID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 UserID,Gravatar,Singature,TelPhone,QQ,MSN,HomePage,Birthday,BirthdayVisible,BirthdayIndexVisible,Constellation,ConstellationVisible,ConstellationIndexVisible,NativePlace,NativePlaceVisible,NativePlaceIndexVisible,RegionId,Address,AddressVisible,AddressIndexVisible,BodilyForm,BodilyFormVisible,BodilyFormIndexVisible,BloodType,BloodTypeVisible,BloodTypeIndexVisible,Marriaged,MarriagedVisible,MarriagedIndexVisible,PersonalStatus,PersonalStatusVisible,PersonalStatusIndexVisible,Grade,Balance,BalanceCredit,Points,RankScore,TopicCount,ReplyTopicCount,FavTopicCount,PvCount,FansCount,FellowCount,AblumsCount,FavouritesCount,FavoritedCount,ShareCount,ProductsCount,PersonalDomain,LastAccessTime,LastAccessIP,LastPostTime,LastLoginTime,Remark,IsUserDPI,PayAccount,UserCardCode,UserCardType,SourceType,SalesId from Accounts_UsersExp ");
            strSql.Append(" where UserID=@UserID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)			};
            parameters[0].Value = UserID;

            DataSet ds = DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Members.UsersExpModel DataRowToModel(DataRow row)
        {
            YSWL.MALL.Model.Members.UsersExpModel model = new YSWL.MALL.Model.Members.UsersExpModel();
            if (row != null)
            {
                if (row["UserID"] != null && row["UserID"].ToString() != "")
                {
                    model.UserID = int.Parse(row["UserID"].ToString());
                }
                if (row["Gravatar"] != null)
                {
                    model.Gravatar = row["Gravatar"].ToString();
                }
                if (row["Singature"] != null)
                {
                    model.Singature = row["Singature"].ToString();
                }
                if (row["TelPhone"] != null)
                {
                    model.TelPhone = row["TelPhone"].ToString();
                }
                if (row["QQ"] != null)
                {
                    model.QQ = row["QQ"].ToString();
                }
                if (row["MSN"] != null)
                {
                    model.MSN = row["MSN"].ToString();
                }
                if (row["HomePage"] != null)
                {
                    model.HomePage = row["HomePage"].ToString();
                }
                if (row["Birthday"] != null && row["Birthday"].ToString() != "")
                {
                    model.Birthday = DateTime.Parse(row["Birthday"].ToString());
                }
                if (row["BirthdayVisible"] != null && row["BirthdayVisible"].ToString() != "")
                {
                    model.BirthdayVisible = int.Parse(row["BirthdayVisible"].ToString());
                }
                if (row["BirthdayIndexVisible"] != null && row["BirthdayIndexVisible"].ToString() != "")
                {
                    if ((row["BirthdayIndexVisible"].ToString() == "1") || (row["BirthdayIndexVisible"].ToString().ToLower() == "true"))
                    {
                        model.BirthdayIndexVisible = true;
                    }
                    else
                    {
                        model.BirthdayIndexVisible = false;
                    }
                }
                if (row["Constellation"] != null)
                {
                    model.Constellation = row["Constellation"].ToString();
                }
                if (row["ConstellationVisible"] != null && row["ConstellationVisible"].ToString() != "")
                {
                    model.ConstellationVisible = int.Parse(row["ConstellationVisible"].ToString());
                }
                if (row["ConstellationIndexVisible"] != null && row["ConstellationIndexVisible"].ToString() != "")
                {
                    if ((row["ConstellationIndexVisible"].ToString() == "1") || (row["ConstellationIndexVisible"].ToString().ToLower() == "true"))
                    {
                        model.ConstellationIndexVisible = true;
                    }
                    else
                    {
                        model.ConstellationIndexVisible = false;
                    }
                }
                if (row["NativePlace"] != null)
                {
                    model.NativePlace = row["NativePlace"].ToString();
                }
                if (row["NativePlaceVisible"] != null && row["NativePlaceVisible"].ToString() != "")
                {
                    model.NativePlaceVisible = int.Parse(row["NativePlaceVisible"].ToString());
                }
                if (row["NativePlaceIndexVisible"] != null && row["NativePlaceIndexVisible"].ToString() != "")
                {
                    if ((row["NativePlaceIndexVisible"].ToString() == "1") || (row["NativePlaceIndexVisible"].ToString().ToLower() == "true"))
                    {
                        model.NativePlaceIndexVisible = true;
                    }
                    else
                    {
                        model.NativePlaceIndexVisible = false;
                    }
                }
                if (row["RegionId"] != null && row["RegionId"].ToString() != "")
                {
                    model.RegionId = int.Parse(row["RegionId"].ToString());
                }
                if (row["Address"] != null)
                {
                    model.Address = row["Address"].ToString();
                }
                if (row["AddressVisible"] != null && row["AddressVisible"].ToString() != "")
                {
                    model.AddressVisible = int.Parse(row["AddressVisible"].ToString());
                }
                if (row["AddressIndexVisible"] != null && row["AddressIndexVisible"].ToString() != "")
                {
                    if ((row["AddressIndexVisible"].ToString() == "1") || (row["AddressIndexVisible"].ToString().ToLower() == "true"))
                    {
                        model.AddressIndexVisible = true;
                    }
                    else
                    {
                        model.AddressIndexVisible = false;
                    }
                }
                if (row["BodilyForm"] != null)
                {
                    model.BodilyForm = row["BodilyForm"].ToString();
                }
                if (row["BodilyFormVisible"] != null && row["BodilyFormVisible"].ToString() != "")
                {
                    model.BodilyFormVisible = int.Parse(row["BodilyFormVisible"].ToString());
                }
                if (row["BodilyFormIndexVisible"] != null && row["BodilyFormIndexVisible"].ToString() != "")
                {
                    if ((row["BodilyFormIndexVisible"].ToString() == "1") || (row["BodilyFormIndexVisible"].ToString().ToLower() == "true"))
                    {
                        model.BodilyFormIndexVisible = true;
                    }
                    else
                    {
                        model.BodilyFormIndexVisible = false;
                    }
                }
                if (row["BloodType"] != null)
                {
                    model.BloodType = row["BloodType"].ToString();
                }
                if (row["BloodTypeVisible"] != null && row["BloodTypeVisible"].ToString() != "")
                {
                    model.BloodTypeVisible = int.Parse(row["BloodTypeVisible"].ToString());
                }
                if (row["BloodTypeIndexVisible"] != null && row["BloodTypeIndexVisible"].ToString() != "")
                {
                    if ((row["BloodTypeIndexVisible"].ToString() == "1") || (row["BloodTypeIndexVisible"].ToString().ToLower() == "true"))
                    {
                        model.BloodTypeIndexVisible = true;
                    }
                    else
                    {
                        model.BloodTypeIndexVisible = false;
                    }
                }
                if (row["Marriaged"] != null)
                {
                    model.Marriaged = row["Marriaged"].ToString();
                }
                if (row["MarriagedVisible"] != null && row["MarriagedVisible"].ToString() != "")
                {
                    model.MarriagedVisible = int.Parse(row["MarriagedVisible"].ToString());
                }
                if (row["MarriagedIndexVisible"] != null && row["MarriagedIndexVisible"].ToString() != "")
                {
                    if ((row["MarriagedIndexVisible"].ToString() == "1") || (row["MarriagedIndexVisible"].ToString().ToLower() == "true"))
                    {
                        model.MarriagedIndexVisible = true;
                    }
                    else
                    {
                        model.MarriagedIndexVisible = false;
                    }
                }
                if (row["PersonalStatus"] != null)
                {
                    model.PersonalStatus = row["PersonalStatus"].ToString();
                }
                if (row["PersonalStatusVisible"] != null && row["PersonalStatusVisible"].ToString() != "")
                {
                    model.PersonalStatusVisible = int.Parse(row["PersonalStatusVisible"].ToString());
                }
                if (row["PersonalStatusIndexVisible"] != null && row["PersonalStatusIndexVisible"].ToString() != "")
                {
                    if ((row["PersonalStatusIndexVisible"].ToString() == "1") || (row["PersonalStatusIndexVisible"].ToString().ToLower() == "true"))
                    {
                        model.PersonalStatusIndexVisible = true;
                    }
                    else
                    {
                        model.PersonalStatusIndexVisible = false;
                    }
                }
                if (row["Grade"] != null && row["Grade"].ToString() != "")
                {
                    model.Grade = int.Parse(row["Grade"].ToString());
                }
                if (row["Balance"] != null && row["Balance"].ToString() != "")
                {
                    model.Balance = decimal.Parse(row["Balance"].ToString());
                }
                if (row["BalanceCredit"] != null && row["BalanceCredit"].ToString() != "")
                {
                    model.BalanceCredit = decimal.Parse(row["BalanceCredit"].ToString());
                }
                if (row["Points"] != null && row["Points"].ToString() != "")
                {
                    model.Points = int.Parse(row["Points"].ToString());
                }
                if (row["RankScore"] != null && row["RankScore"].ToString() != "")
                {
                    model.RankScore = int.Parse(row["RankScore"].ToString());
                }
                if (row["TopicCount"] != null && row["TopicCount"].ToString() != "")
                {
                    model.TopicCount = int.Parse(row["TopicCount"].ToString());
                }
                if (row["ReplyTopicCount"] != null && row["ReplyTopicCount"].ToString() != "")
                {
                    model.ReplyTopicCount = int.Parse(row["ReplyTopicCount"].ToString());
                }
                if (row["FavTopicCount"] != null && row["FavTopicCount"].ToString() != "")
                {
                    model.FavTopicCount = int.Parse(row["FavTopicCount"].ToString());
                }
                if (row["PvCount"] != null && row["PvCount"].ToString() != "")
                {
                    model.PvCount = int.Parse(row["PvCount"].ToString());
                }
                if (row["FansCount"] != null && row["FansCount"].ToString() != "")
                {
                    model.FansCount = int.Parse(row["FansCount"].ToString());
                }
                if (row["FellowCount"] != null && row["FellowCount"].ToString() != "")
                {
                    model.FellowCount = int.Parse(row["FellowCount"].ToString());
                }
                if (row["AblumsCount"] != null && row["AblumsCount"].ToString() != "")
                {
                    model.AblumsCount = int.Parse(row["AblumsCount"].ToString());
                }
                if (row["FavouritesCount"] != null && row["FavouritesCount"].ToString() != "")
                {
                    model.FavouritesCount = int.Parse(row["FavouritesCount"].ToString());
                }
                if (row["FavoritedCount"] != null && row["FavoritedCount"].ToString() != "")
                {
                    model.FavoritedCount = int.Parse(row["FavoritedCount"].ToString());
                }
                if (row["ShareCount"] != null && row["ShareCount"].ToString() != "")
                {
                    model.ShareCount = int.Parse(row["ShareCount"].ToString());
                }
                if (row["ProductsCount"] != null && row["ProductsCount"].ToString() != "")
                {
                    model.ProductsCount = int.Parse(row["ProductsCount"].ToString());
                }
                if (row["PersonalDomain"] != null)
                {
                    model.PersonalDomain = row["PersonalDomain"].ToString();
                }
                if (row["LastAccessTime"] != null && row["LastAccessTime"].ToString() != "")
                {
                    model.LastAccessTime = DateTime.Parse(row["LastAccessTime"].ToString());
                }
                if (row["LastAccessIP"] != null)
                {
                    model.LastAccessIP = row["LastAccessIP"].ToString();
                }
                if (row["LastPostTime"] != null && row["LastPostTime"].ToString() != "")
                {
                    model.LastPostTime = DateTime.Parse(row["LastPostTime"].ToString());
                }
                if (row["LastLoginTime"] != null && row["LastLoginTime"].ToString() != "")
                {
                    model.LastLoginTime = DateTime.Parse(row["LastLoginTime"].ToString());
                }
                if (row["Remark"] != null)
                {
                    model.Remark = row["Remark"].ToString();
                }
                if (row["IsUserDPI"] != null && row["IsUserDPI"].ToString() != "")
                {
                    if ((row["IsUserDPI"].ToString() == "1") || (row["IsUserDPI"].ToString().ToLower() == "true"))
                    {
                        model.IsUserDPI = true;
                    }
                    else
                    {
                        model.IsUserDPI = false;
                    }
                }
                if (row["PayAccount"] != null)
                {
                    model.PayAccount = row["PayAccount"].ToString();
                }
                if (row["UserCardCode"] != null)
                {
                    model.UserCardCode = row["UserCardCode"].ToString();
                }
                if (row["UserCardType"] != null && row["UserCardType"].ToString() != "")
                {
                    model.UserCardType = int.Parse(row["UserCardType"].ToString());
                }
                if (row["SourceType"] != null && row["SourceType"].ToString() != "")
                {
                    model.SourceType = int.Parse(row["SourceType"].ToString());
                }
                if (row["SalesId"] != null && row["SalesId"].ToString() != "")
                {
                    model.SalesId = int.Parse(row["SalesId"].ToString());
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UserID,Gravatar,Singature,TelPhone,QQ,MSN,HomePage,Birthday,BirthdayVisible,BirthdayIndexVisible,Constellation,ConstellationVisible,ConstellationIndexVisible,NativePlace,NativePlaceVisible,NativePlaceIndexVisible,RegionId,Address,AddressVisible,AddressIndexVisible,BodilyForm,BodilyFormVisible,BodilyFormIndexVisible,BloodType,BloodTypeVisible,BloodTypeIndexVisible,Marriaged,MarriagedVisible,MarriagedIndexVisible,PersonalStatus,PersonalStatusVisible,PersonalStatusIndexVisible,Grade,Balance,BalanceCredit,Points,RankScore,TopicCount,ReplyTopicCount,FavTopicCount,PvCount,FansCount,FellowCount,AblumsCount,FavouritesCount,FavoritedCount,ShareCount,ProductsCount,PersonalDomain,LastAccessTime,LastAccessIP,LastPostTime,LastLoginTime,Remark,IsUserDPI,PayAccount,UserCardCode,UserCardType,SourceType,SalesId ");
            strSql.Append(" FROM Accounts_UsersExp ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" UserID,Gravatar,Singature,TelPhone,QQ,MSN,HomePage,Birthday,BirthdayVisible,BirthdayIndexVisible,Constellation,ConstellationVisible,ConstellationIndexVisible,NativePlace,NativePlaceVisible,NativePlaceIndexVisible,RegionId,Address,AddressVisible,AddressIndexVisible,BodilyForm,BodilyFormVisible,BodilyFormIndexVisible,BloodType,BloodTypeVisible,BloodTypeIndexVisible,Marriaged,MarriagedVisible,MarriagedIndexVisible,PersonalStatus,PersonalStatusVisible,PersonalStatusIndexVisible,Grade,Balance,BalanceCredit,Points,RankScore,TopicCount,ReplyTopicCount,FavTopicCount,PvCount,FansCount,FellowCount,AblumsCount,FavouritesCount,FavoritedCount,ShareCount,ProductsCount,PersonalDomain,LastAccessTime,LastAccessIP,LastPostTime,LastLoginTime,Remark,IsUserDPI,PayAccount,UserCardCode,UserCardType,SourceType,SalesId ");
            strSql.Append(" FROM Accounts_UsersExp ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM Accounts_UsersExp ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.UserID desc");
            }
            strSql.Append(")AS Row, T.*  from Accounts_UsersExp T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /*
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@tblName", SqlDbType.VarChar, 255),
                    new SqlParameter("@fldName", SqlDbType.VarChar, 255),
                    new SqlParameter("@PageSize", SqlDbType.Int),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@IsReCount", SqlDbType.Bit),
                    new SqlParameter("@OrderType", SqlDbType.Bit),
                    new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
                    };
            parameters[0].Value = "Accounts_UsersExp";
            parameters[1].Value = "UserID";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DBHelper.DefaultDBHelper.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  BasicMethod

        #region 扩展方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(int userID)
        {
            int rowsAffected = 0;
            SqlParameter[] param = {
                                   new SqlParameter("@UserID",SqlDbType.Int)
                                   };
            param[0].Value = userID;
            DBHelper.DefaultDBHelper.RunProcedure("sp_Accounts_CreateUserExp", param, out rowsAffected);
            if (rowsAffected > 0)
            {
                return true;
            } return false;
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetUserList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" * ");
            strSql.Append(" FROM Accounts_Users ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        public bool UpdateFavouritesCount()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Accounts_UsersExp SET ");
            strSql.Append("FavouritesCount=( select COUNT(1) from SNS_UserFavourite where CreatedUserID=Accounts_UsersExp.UserID)");
            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateProductCount()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Accounts_UsersExp SET ");
            strSql.Append("ProductsCount=(select COUNT(1) from SNS_Products where CreateUserID=Accounts_UsersExp.UserID)");
            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateShareCount()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Accounts_UsersExp SET ");
            strSql.Append("ShareCount=ProductsCount+(select COUNT(1) from SNS_Photos where CreatedUserID=Accounts_UsersExp.UserID)");
            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateAblumsCount()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Accounts_UsersExp SET ");
            strSql.Append("AblumsCount=(select COUNT(1) from SNS_UserAlbums where CreatedUserID=Accounts_UsersExp.UserID)");
            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetUserCountByKeyWord(string NickName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(1) FROM  Accounts_Users au inner JOIN Accounts_UsersExp uea ON au.UserID=uea.UserID  ");
            if (!string.IsNullOrEmpty(NickName))
            {
                strSql.Append("AND NickName LIKE '%" + Common.InjectionFilter.SqlFilter(NickName) + "%'");
            }
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetUserListByKeyWord(string NickName, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("ORDER BY T." + orderby);
            }
            else
            {
                strSql.Append("ORDER BY T.UserID desc");
            }
            strSql.Append(")AS Row, T.*  FROM (SELECT uea.*,au.NickName FROM Accounts_Users au inner JOIN Accounts_UsersExp uea ON au.UserID=uea.UserID  ");
            if (!string.IsNullOrEmpty(NickName))
            {
                strSql.Append(" AND NickName LIKE @NickName");
            }
            strSql.Append(" ) T) TT");
            strSql.AppendFormat(" WHERE TT.Row BETWEEN {0} AND {1}", startIndex, endIndex);

            SqlParameter[] parameters = {
                    new SqlParameter("@NickName", SqlDbType.NVarChar)};
            parameters[0].Value = "%" + NickName + "%";
            return DBHelper.DefaultDBHelper.Query(strSql.ToString(),parameters);
        }

        /// <summary>
        /// 是否通过实名认证
        /// </summary>
        public bool UpdateIsDPI(string userIds, int status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("UPDATE UE SET IsUserDPI={0} FROM Accounts_UsersExp UE, ", status);
            strSql.AppendFormat("(SELECT UserID FROM Accounts_UsersApprove WHERE ApproveID IN ({0}))AP ", userIds);
            strSql.Append("WHERE UE.UserID=AP.UserID ");

            return DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString()) > 0;
        }

        public bool UpdatePhoneAndPay(int  userId,string account, string phone)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Accounts_UsersExp set ");
            strSql.Append("TelPhone=@TelPhone,");
            strSql.Append("PayAccount=@PayAccount");
            strSql.Append(" where UserID=@UserID ");
            SqlParameter[] parameters = {
					new SqlParameter("@TelPhone", SqlDbType.NVarChar,20),
					new SqlParameter("@PayAccount", SqlDbType.NVarChar,200),
					new SqlParameter("@UserID", SqlDbType.Int,4)};
            parameters[0].Value = phone;
            parameters[1].Value = account;
            parameters[2].Value = userId;

            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetUserRankId(int UserId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(1)AS Count,SourceType  FROM  Accounts_UsersExp ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)};
            parameters[0].Value = UserId;
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 获取用户余额
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public decimal GetUserBalance(int UserId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  Balance FROM Accounts_UsersExp WHERE UserId=@UserId");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)};
            parameters[0].Value = UserId;
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToDecimal(obj);
            }
        }
        /// <summary>
        /// 获得指定用户ID的全部下属用户
        /// </summary>
        public DataSet GetAllEmpByUserId(int userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"
WITH    CTEGetChild
          AS ( SELECT   *
               FROM     Accounts_Users
               WHERE    EmployeeID = {0}
               UNION ALL
               ( SELECT a.*
                 FROM   Accounts_Users AS a
                        INNER JOIN CTEGetChild AS b ON a.EmployeeID = b.UserID
               )
             )
    SELECT  *
    FROM    CTEGetChild ORDER BY EmployeeID, UserID
", userId);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
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
        public bool AddEx(Model.Members.UsersExpModel model, int inviteID, string inviteNick, int pointScore, int rankScore)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Accounts_UsersExp(");
            strSql.Append("UserID,Gravatar,Singature,TelPhone,QQ,MSN,HomePage,Birthday,BirthdayVisible,BirthdayIndexVisible,Constellation,ConstellationVisible,ConstellationIndexVisible,NativePlace,NativePlaceVisible,NativePlaceIndexVisible,RegionId,Address,AddressVisible,AddressIndexVisible,BodilyForm,BodilyFormVisible,BodilyFormIndexVisible,BloodType,BloodTypeVisible,BloodTypeIndexVisible,Marriaged,MarriagedVisible,MarriagedIndexVisible,PersonalStatus,PersonalStatusVisible,PersonalStatusIndexVisible,Grade,Balance,Points,TopicCount,ReplyTopicCount,FavTopicCount,PvCount,FansCount,FellowCount,AblumsCount,FavouritesCount,FavoritedCount,ShareCount,ProductsCount,PersonalDomain,LastAccessTime,LastAccessIP,LastPostTime,LastLoginTime,Remark,IsUserDPI,PayAccount)");
            strSql.Append(" values (");
            strSql.Append("@UserID,@Gravatar,@Singature,@TelPhone,@QQ,@MSN,@HomePage,@Birthday,@BirthdayVisible,@BirthdayIndexVisible,@Constellation,@ConstellationVisible,@ConstellationIndexVisible,@NativePlace,@NativePlaceVisible,@NativePlaceIndexVisible,@RegionId,@Address,@AddressVisible,@AddressIndexVisible,@BodilyForm,@BodilyFormVisible,@BodilyFormIndexVisible,@BloodType,@BloodTypeVisible,@BloodTypeIndexVisible,@Marriaged,@MarriagedVisible,@MarriagedIndexVisible,@PersonalStatus,@PersonalStatusVisible,@PersonalStatusIndexVisible,@Grade,@Balance,@Points,@TopicCount,@ReplyTopicCount,@FavTopicCount,@PvCount,@FansCount,@FellowCount,@AblumsCount,@FavouritesCount,@FavoritedCount,@ShareCount,@ProductsCount,@PersonalDomain,@LastAccessTime,@LastAccessIP,@LastPostTime,@LastLoginTime,@Remark,@IsUserDPI,@PayAccount)");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@Gravatar", SqlDbType.NVarChar,200),
					new SqlParameter("@Singature", SqlDbType.NVarChar,200),
					new SqlParameter("@TelPhone", SqlDbType.NVarChar,20),
					new SqlParameter("@QQ", SqlDbType.NVarChar,30),
					new SqlParameter("@MSN", SqlDbType.NVarChar,30),
					new SqlParameter("@HomePage", SqlDbType.NVarChar,50),
					new SqlParameter("@Birthday", SqlDbType.DateTime),
					new SqlParameter("@BirthdayVisible", SqlDbType.SmallInt,2),
					new SqlParameter("@BirthdayIndexVisible", SqlDbType.Bit,1),
					new SqlParameter("@Constellation", SqlDbType.NVarChar,10),
					new SqlParameter("@ConstellationVisible", SqlDbType.SmallInt,2),
					new SqlParameter("@ConstellationIndexVisible", SqlDbType.Bit,1),
					new SqlParameter("@NativePlace", SqlDbType.NVarChar,300),
					new SqlParameter("@NativePlaceVisible", SqlDbType.SmallInt,2),
					new SqlParameter("@NativePlaceIndexVisible", SqlDbType.Bit,1),
					new SqlParameter("@RegionId", SqlDbType.Int,4),
					new SqlParameter("@Address", SqlDbType.NVarChar,300),
					new SqlParameter("@AddressVisible", SqlDbType.SmallInt,2),
					new SqlParameter("@AddressIndexVisible", SqlDbType.Bit,1),
					new SqlParameter("@BodilyForm", SqlDbType.NVarChar,10),
					new SqlParameter("@BodilyFormVisible", SqlDbType.SmallInt,2),
					new SqlParameter("@BodilyFormIndexVisible", SqlDbType.Bit,1),
					new SqlParameter("@BloodType", SqlDbType.NVarChar,10),
					new SqlParameter("@BloodTypeVisible", SqlDbType.SmallInt,2),
					new SqlParameter("@BloodTypeIndexVisible", SqlDbType.Bit,1),
					new SqlParameter("@Marriaged", SqlDbType.NVarChar,10),
					new SqlParameter("@MarriagedVisible", SqlDbType.SmallInt,2),
					new SqlParameter("@MarriagedIndexVisible", SqlDbType.Bit,1),
					new SqlParameter("@PersonalStatus", SqlDbType.NVarChar,10),
					new SqlParameter("@PersonalStatusVisible", SqlDbType.SmallInt,2),
					new SqlParameter("@PersonalStatusIndexVisible", SqlDbType.Bit,1),
					new SqlParameter("@Grade", SqlDbType.Int,4),
					new SqlParameter("@Balance", SqlDbType.Money,8),
					new SqlParameter("@Points", SqlDbType.Int,4),
					new SqlParameter("@TopicCount", SqlDbType.Int,4),
					new SqlParameter("@ReplyTopicCount", SqlDbType.Int,4),
					new SqlParameter("@FavTopicCount", SqlDbType.Int,4),
					new SqlParameter("@PvCount", SqlDbType.Int,4),
					new SqlParameter("@FansCount", SqlDbType.Int,4),
					new SqlParameter("@FellowCount", SqlDbType.Int,4),
					new SqlParameter("@AblumsCount", SqlDbType.Int,4),
					new SqlParameter("@FavouritesCount", SqlDbType.Int,4),
					new SqlParameter("@FavoritedCount", SqlDbType.Int,4),
					new SqlParameter("@ShareCount", SqlDbType.Int,4),
					new SqlParameter("@ProductsCount", SqlDbType.Int,4),
					new SqlParameter("@PersonalDomain", SqlDbType.NVarChar,50),
					new SqlParameter("@LastAccessTime", SqlDbType.DateTime),
					new SqlParameter("@LastAccessIP", SqlDbType.NVarChar,50),
					new SqlParameter("@LastPostTime", SqlDbType.DateTime),
					new SqlParameter("@LastLoginTime", SqlDbType.DateTime),
					new SqlParameter("@Remark", SqlDbType.NVarChar),
					new SqlParameter("@IsUserDPI", SqlDbType.Bit,1),
					new SqlParameter("@PayAccount", SqlDbType.NVarChar,200)};
            parameters[0].Value = model.UserID;
            parameters[1].Value = model.Gravatar;
            parameters[2].Value = model.Singature;
            parameters[3].Value = model.TelPhone;
            parameters[4].Value = model.QQ;
            parameters[5].Value = model.MSN;
            parameters[6].Value = model.HomePage;
            parameters[7].Value = model.Birthday;
            parameters[8].Value = model.BirthdayVisible;
            parameters[9].Value = model.BirthdayIndexVisible;
            parameters[10].Value = model.Constellation;
            parameters[11].Value = model.ConstellationVisible;
            parameters[12].Value = model.ConstellationIndexVisible;
            parameters[13].Value = model.NativePlace;
            parameters[14].Value = model.NativePlaceVisible;
            parameters[15].Value = model.NativePlaceIndexVisible;
            parameters[16].Value = model.RegionId;
            parameters[17].Value = model.Address;
            parameters[18].Value = model.AddressVisible;
            parameters[19].Value = model.AddressIndexVisible;
            parameters[20].Value = model.BodilyForm;
            parameters[21].Value = model.BodilyFormVisible;
            parameters[22].Value = model.BodilyFormIndexVisible;
            parameters[23].Value = model.BloodType;
            parameters[24].Value = model.BloodTypeVisible;
            parameters[25].Value = model.BloodTypeIndexVisible;
            parameters[26].Value = model.Marriaged;
            parameters[27].Value = model.MarriagedVisible;
            parameters[28].Value = model.MarriagedIndexVisible;
            parameters[29].Value = model.PersonalStatus;
            parameters[30].Value = model.PersonalStatusVisible;
            parameters[31].Value = model.PersonalStatusIndexVisible;
            parameters[32].Value = model.Grade;
            parameters[33].Value = model.Balance;
            parameters[34].Value = model.Points;
            parameters[35].Value = model.TopicCount;
            parameters[36].Value = model.ReplyTopicCount;
            parameters[37].Value = model.FavTopicCount;
            parameters[38].Value = model.PvCount;
            parameters[39].Value = model.FansCount;
            parameters[40].Value = model.FellowCount;
            parameters[41].Value = model.AblumsCount;
            parameters[42].Value = model.FavouritesCount;
            parameters[43].Value = model.FavoritedCount;
            parameters[44].Value = model.ShareCount;
            parameters[45].Value = model.ProductsCount;
            parameters[46].Value = model.PersonalDomain;
            parameters[47].Value = model.LastAccessTime;
            parameters[48].Value = model.LastAccessIP;
            parameters[49].Value = model.LastPostTime;
            parameters[50].Value = model.LastLoginTime;
            parameters[51].Value = model.Remark;
            parameters[52].Value = model.IsUserDPI;
            parameters[53].Value = model.PayAccount;
            List<CommandInfo> sqllist = new List<CommandInfo>();
            CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
            sqllist.Add(cmd);

            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("insert into Accounts_UserInvite(");
            strSql2.Append("UserId,UserNick,InviteUserId,InviteNick,Depth,Path,Status,IsRebate,IsNew,CreatedDate,Remark,RebateDesc)");
            strSql2.Append(" values (");
            strSql2.Append("@UserId,@UserNick,@InviteUserId,@InviteNick,@Depth,@Path,@Status,@IsRebate,@IsNew,@CreatedDate,@Remark,@RebateDesc)");
            strSql2.Append(";select @@IDENTITY");
            SqlParameter[] parameters2 = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@UserNick", SqlDbType.NVarChar,200),
					new SqlParameter("@InviteUserId", SqlDbType.Int,4),
					new SqlParameter("@InviteNick", SqlDbType.NVarChar,200),
					new SqlParameter("@Depth", SqlDbType.Int,4),
					new SqlParameter("@Path", SqlDbType.NVarChar,4000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@IsRebate", SqlDbType.Bit,1),
					new SqlParameter("@IsNew", SqlDbType.Bit,1),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime),
					new SqlParameter("@Remark", SqlDbType.NVarChar,-1),
					new SqlParameter("@RebateDesc", SqlDbType.NVarChar,200)};
            parameters2[0].Value = model.UserID;
            parameters2[1].Value = model.NickName;
            parameters2[2].Value = inviteID;
            parameters2[3].Value = inviteNick;
            parameters2[4].Value = 1;
            parameters2[5].Value = "";
            parameters2[6].Value = 1;
            parameters2[7].Value = true;
            parameters2[8].Value = true;
            parameters2[9].Value = DateTime.Now;
            parameters2[10].Value = "";
            parameters2[11].Value = string.Format("邀请用户+{0}积分,{1}成长值", pointScore, rankScore);


            cmd = new CommandInfo(strSql2.ToString(), parameters2);
            sqllist.Add(cmd);

            int rows = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 更新客户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateCustom(Model.Members.UsersExpModel model)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();

            #region 更新动作
            StringBuilder strSql = new StringBuilder();
            //更新自己的分享数据和商品数量
            strSql.Append("update Accounts_Users set ");
            strSql.Append("UserName=@UserName,");
            strSql.Append("Password=@Password,");
            strSql.Append("TrueName=@TrueName,");
            strSql.Append("Phone=@Phone,");
            strSql.Append("Email=@Email,");
            strSql.Append("EmployeeID=@EmployeeID");
            strSql.Append(" where UserID=@UserID");
            SqlParameter[] parameters = {
					new SqlParameter("@UserName", SqlDbType.NVarChar,50),
					new SqlParameter("@Password", SqlDbType.Binary,20),
					new SqlParameter("@TrueName", SqlDbType.NVarChar,50),
					new SqlParameter("@Phone", SqlDbType.NVarChar,20),
					new SqlParameter("@Email", SqlDbType.NVarChar,100),
                    new SqlParameter("@EmployeeID", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4)};
            parameters[0].Value = model.UserName;
            parameters[1].Value = model.Password;
            parameters[2].Value = model.TrueName;
            parameters[3].Value = model.Phone;
            parameters[4].Value = model.Email;
            parameters[5].Value = model.EmployeeID;
            parameters[6].Value = model.UserID;
            CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
            sqllist.Add(cmd);

            //更新用户扩展信息
            StringBuilder strSql8 = new StringBuilder();
            strSql8.Append("Update Accounts_UsersExp set  Address=@Address  , RegionId=@RegionId,PersonalStatusIndexVisible=@PersonalStatusIndexVisible   ");
            strSql8.Append("  where  UserID=@UserID");
            SqlParameter[] parameters8 = {
                                             	new SqlParameter("@Address", SqlDbType.NVarChar,300),
                                                new SqlParameter("@RegionId", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4),
                     new SqlParameter("@PersonalStatusIndexVisible",SqlDbType.Bit,1)
                                         };
            parameters8[0].Value = model.Address;
            parameters8[1].Value = model.RegionId;
            parameters8[2].Value = model.UserID;
            parameters8[3].Value = model.PersonalStatusIndexVisible;
            cmd = new CommandInfo(strSql8.ToString(), parameters8);
            sqllist.Add(cmd);

            #endregion 更新动作

            int rowsAffected = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 更新业务员信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateSales(Model.Members.UsersExpModel model)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();

            #region 更新动作
            StringBuilder strSql = new StringBuilder();
            //更新自己的分享数据和商品数量
            strSql.Append("update Accounts_Users set ");
            strSql.Append("UserName=@UserName,");
            strSql.Append("Password=@Password,");
            strSql.Append("TrueName=@TrueName,");
            strSql.Append("Phone=@Phone,");
            strSql.Append("Email=@Email,");
            strSql.Append("EmployeeID=@EmployeeID");
            strSql.Append(" where UserID=@UserID");
            SqlParameter[] parameters = {
					new SqlParameter("@UserName", SqlDbType.NVarChar,50),
					new SqlParameter("@Password", SqlDbType.Binary,20),
					new SqlParameter("@TrueName", SqlDbType.NVarChar,50),
					new SqlParameter("@Phone", SqlDbType.NVarChar,20),
					new SqlParameter("@Email", SqlDbType.NVarChar,100),
                    new SqlParameter("@EmployeeID", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4)};
            parameters[0].Value = model.UserName;
            parameters[1].Value = model.Password;
            parameters[2].Value = model.TrueName;
            parameters[3].Value = model.Phone;
            parameters[4].Value = model.Email;
            parameters[5].Value = model.EmployeeID;
            parameters[6].Value = model.UserID;
            CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
            sqllist.Add(cmd);

            //更新用户扩展信息
            StringBuilder strSql8 = new StringBuilder();
            strSql8.Append("Update Accounts_UsersExp set  SalesId=@SalesId ");
            strSql8.Append("  where  UserID=@UserID");
            SqlParameter[] parameters8 = {
                                             	new SqlParameter("@SalesId", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4)
                                         };
            parameters8[0].Value = model.SalesId;
            parameters8[1].Value = model.UserID;
            cmd = new CommandInfo(strSql8.ToString(), parameters8);
            sqllist.Add(cmd);

            #endregion 更新动作

            int rowsAffected = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否有该业务员
        /// </summary>
        /// <param name="SaleId"></param>
        /// <returns></returns>
        public  bool HasSales(int SaleId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Accounts_UsersExp");
            strSql.Append(" where SalesId=@SalesId ");
            SqlParameter[] parameters = {
					new SqlParameter("@SalesId", SqlDbType.Int,4)			};
            parameters[0].Value = SaleId;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 根据业务员编号 （原ERP 系统中的编号）
        /// </summary>
        /// <param name="SaleId"></param>
        /// <returns></returns>
        public YSWL.MALL.Model.Members.UsersExpModel GetSalesModel(int SaleId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from Accounts_UsersExp ");
            strSql.Append(" where SalesId=@SalesId ");
            SqlParameter[] parameters = {
					new SqlParameter("@SalesId", SqlDbType.Int,4)			};
            parameters[0].Value = SaleId;

            YSWL.MALL.Model.Members.UsersExpModel model = new YSWL.MALL.Model.Members.UsersExpModel();
            DataSet ds = DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 用户注册来源统计
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
       public DataSet SourceCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(1)AS Count,SourceType FROM  Accounts_Users U INNER JOIN   Accounts_UsersExp Ue ON U.UserID=Ue.UserID ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append("  GROUP BY SourceType ");
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 增加一条数据 (用户扩展表和收货地址表)事物执行
        /// </summary>
        /// <returns></returns>
        public bool AddEx(Model.Members.UsersExpModel model, Model.Shop.Shipping.ShippingAddress addressModel)
        {
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("insert into Accounts_UsersExp(");
            strSql2.Append("UserID,Gravatar,Singature,TelPhone,QQ,MSN,HomePage,Birthday,BirthdayVisible,BirthdayIndexVisible,Constellation,ConstellationVisible,ConstellationIndexVisible,NativePlace,NativePlaceVisible,NativePlaceIndexVisible,RegionId,Address,AddressVisible,AddressIndexVisible,BodilyForm,BodilyFormVisible,BodilyFormIndexVisible,BloodType,BloodTypeVisible,BloodTypeIndexVisible,Marriaged,MarriagedVisible,MarriagedIndexVisible,PersonalStatus,PersonalStatusVisible,PersonalStatusIndexVisible,Grade,Balance,Points,TopicCount,ReplyTopicCount,FavTopicCount,PvCount,FansCount,FellowCount,AblumsCount,FavouritesCount,FavoritedCount,ShareCount,ProductsCount,PersonalDomain,LastAccessTime,LastAccessIP,LastPostTime,LastLoginTime,Remark,IsUserDPI,PayAccount)");
            strSql2.Append(" values (");
            strSql2.Append("@UserID,@Gravatar,@Singature,@TelPhone,@QQ,@MSN,@HomePage,@Birthday,@BirthdayVisible,@BirthdayIndexVisible,@Constellation,@ConstellationVisible,@ConstellationIndexVisible,@NativePlace,@NativePlaceVisible,@NativePlaceIndexVisible,@RegionId,@Address,@AddressVisible,@AddressIndexVisible,@BodilyForm,@BodilyFormVisible,@BodilyFormIndexVisible,@BloodType,@BloodTypeVisible,@BloodTypeIndexVisible,@Marriaged,@MarriagedVisible,@MarriagedIndexVisible,@PersonalStatus,@PersonalStatusVisible,@PersonalStatusIndexVisible,@Grade,@Balance,@Points,@TopicCount,@ReplyTopicCount,@FavTopicCount,@PvCount,@FansCount,@FellowCount,@AblumsCount,@FavouritesCount,@FavoritedCount,@ShareCount,@ProductsCount,@PersonalDomain,@LastAccessTime,@LastAccessIP,@LastPostTime,@LastLoginTime,@Remark,@IsUserDPI,@PayAccount)");
            SqlParameter[] parameters2 = {
                    new SqlParameter("@UserID", SqlDbType.Int,4),
                    new SqlParameter("@Gravatar", SqlDbType.NVarChar,200),
                    new SqlParameter("@Singature", SqlDbType.NVarChar,200),
                    new SqlParameter("@TelPhone", SqlDbType.NVarChar,20),
                    new SqlParameter("@QQ", SqlDbType.NVarChar,30),
                    new SqlParameter("@MSN", SqlDbType.NVarChar,30),
                    new SqlParameter("@HomePage", SqlDbType.NVarChar,50),
                    new SqlParameter("@Birthday", SqlDbType.DateTime),
                    new SqlParameter("@BirthdayVisible", SqlDbType.SmallInt,2),
                    new SqlParameter("@BirthdayIndexVisible", SqlDbType.Bit,1),
                    new SqlParameter("@Constellation", SqlDbType.NVarChar,10),
                    new SqlParameter("@ConstellationVisible", SqlDbType.SmallInt,2),
                    new SqlParameter("@ConstellationIndexVisible", SqlDbType.Bit,1),
                    new SqlParameter("@NativePlace", SqlDbType.NVarChar,300),
                    new SqlParameter("@NativePlaceVisible", SqlDbType.SmallInt,2),
                    new SqlParameter("@NativePlaceIndexVisible", SqlDbType.Bit,1),
                    new SqlParameter("@RegionId", SqlDbType.Int,4),
                    new SqlParameter("@Address", SqlDbType.NVarChar,300),
                    new SqlParameter("@AddressVisible", SqlDbType.SmallInt,2),
                    new SqlParameter("@AddressIndexVisible", SqlDbType.Bit,1),
                    new SqlParameter("@BodilyForm", SqlDbType.NVarChar,10),
                    new SqlParameter("@BodilyFormVisible", SqlDbType.SmallInt,2),
                    new SqlParameter("@BodilyFormIndexVisible", SqlDbType.Bit,1),
                    new SqlParameter("@BloodType", SqlDbType.NVarChar,10),
                    new SqlParameter("@BloodTypeVisible", SqlDbType.SmallInt,2),
                    new SqlParameter("@BloodTypeIndexVisible", SqlDbType.Bit,1),
                    new SqlParameter("@Marriaged", SqlDbType.NVarChar,10),
                    new SqlParameter("@MarriagedVisible", SqlDbType.SmallInt,2),
                    new SqlParameter("@MarriagedIndexVisible", SqlDbType.Bit,1),
                    new SqlParameter("@PersonalStatus", SqlDbType.NVarChar,10),
                    new SqlParameter("@PersonalStatusVisible", SqlDbType.SmallInt,2),
                    new SqlParameter("@PersonalStatusIndexVisible", SqlDbType.Bit,1),
                    new SqlParameter("@Grade", SqlDbType.Int,4),
                    new SqlParameter("@Balance", SqlDbType.Money,8),
                    new SqlParameter("@Points", SqlDbType.Int,4),
                    new SqlParameter("@TopicCount", SqlDbType.Int,4),
                    new SqlParameter("@ReplyTopicCount", SqlDbType.Int,4),
                    new SqlParameter("@FavTopicCount", SqlDbType.Int,4),
                    new SqlParameter("@PvCount", SqlDbType.Int,4),
                    new SqlParameter("@FansCount", SqlDbType.Int,4),
                    new SqlParameter("@FellowCount", SqlDbType.Int,4),
                    new SqlParameter("@AblumsCount", SqlDbType.Int,4),
                    new SqlParameter("@FavouritesCount", SqlDbType.Int,4),
                    new SqlParameter("@FavoritedCount", SqlDbType.Int,4),
                    new SqlParameter("@ShareCount", SqlDbType.Int,4),
                    new SqlParameter("@ProductsCount", SqlDbType.Int,4),
                    new SqlParameter("@PersonalDomain", SqlDbType.NVarChar,50),
                    new SqlParameter("@LastAccessTime", SqlDbType.DateTime),
                    new SqlParameter("@LastAccessIP", SqlDbType.NVarChar,50),
                    new SqlParameter("@LastPostTime", SqlDbType.DateTime),
                    new SqlParameter("@LastLoginTime", SqlDbType.DateTime),
                    new SqlParameter("@Remark", SqlDbType.NVarChar),
                    new SqlParameter("@IsUserDPI", SqlDbType.Bit,1),
                    new SqlParameter("@PayAccount", SqlDbType.NVarChar,200)};
            parameters2[0].Value = model.UserID;
            parameters2[1].Value = model.Gravatar;
            parameters2[2].Value = model.Singature;
            parameters2[3].Value = model.TelPhone;
            parameters2[4].Value = model.QQ;
            parameters2[5].Value = model.MSN;
            parameters2[6].Value = model.HomePage;
            parameters2[7].Value = model.Birthday;
            parameters2[8].Value = model.BirthdayVisible;
            parameters2[9].Value = model.BirthdayIndexVisible;
            parameters2[10].Value = model.Constellation;
            parameters2[11].Value = model.ConstellationVisible;
            parameters2[12].Value = model.ConstellationIndexVisible;
            parameters2[13].Value = model.NativePlace;
            parameters2[14].Value = model.NativePlaceVisible;
            parameters2[15].Value = model.NativePlaceIndexVisible;
            parameters2[16].Value = model.RegionId;
            parameters2[17].Value = model.Address;
            parameters2[18].Value = model.AddressVisible;
            parameters2[19].Value = model.AddressIndexVisible;
            parameters2[20].Value = model.BodilyForm;
            parameters2[21].Value = model.BodilyFormVisible;
            parameters2[22].Value = model.BodilyFormIndexVisible;
            parameters2[23].Value = model.BloodType;
            parameters2[24].Value = model.BloodTypeVisible;
            parameters2[25].Value = model.BloodTypeIndexVisible;
            parameters2[26].Value = model.Marriaged;
            parameters2[27].Value = model.MarriagedVisible;
            parameters2[28].Value = model.MarriagedIndexVisible;
            parameters2[29].Value = model.PersonalStatus;
            parameters2[30].Value = model.PersonalStatusVisible;
            parameters2[31].Value = model.PersonalStatusIndexVisible;
            parameters2[32].Value = model.Grade;
            parameters2[33].Value = model.Balance;
            parameters2[34].Value = model.Points;
            parameters2[35].Value = model.TopicCount;
            parameters2[36].Value = model.ReplyTopicCount;
            parameters2[37].Value = model.FavTopicCount;
            parameters2[38].Value = model.PvCount;
            parameters2[39].Value = model.FansCount;
            parameters2[40].Value = model.FellowCount;
            parameters2[41].Value = model.AblumsCount;
            parameters2[42].Value = model.FavouritesCount;
            parameters2[43].Value = model.FavoritedCount;
            parameters2[44].Value = model.ShareCount;
            parameters2[45].Value = model.ProductsCount;
            parameters2[46].Value = model.PersonalDomain;
            parameters2[47].Value = model.LastAccessTime;
            parameters2[48].Value = model.LastAccessIP;
            parameters2[49].Value = model.LastPostTime;
            parameters2[50].Value = model.LastLoginTime;
            parameters2[51].Value = model.Remark;
            parameters2[52].Value = model.IsUserDPI;
            parameters2[53].Value = model.PayAccount;
            List<CommandInfo> sqllist = new List<CommandInfo>();
            CommandInfo cmd = new CommandInfo(strSql2.ToString(), parameters2);
            sqllist.Add(cmd);

            StringBuilder strSql3 = new StringBuilder();
            strSql3.Append("insert into Shop_ShippingAddress(");
            strSql3.Append("RegionId,UserId,ShipName,Address,Zipcode,EmailAddress,TelPhone,CelPhone,IsDefault,Aliases,Latitude,Longitude,LineId,CircleId,DepotId,Remark)");
            strSql3.Append(" values (");
            strSql3.Append("@RegionId,@UserId,@ShipName,@Address,@Zipcode,@EmailAddress,@TelPhone,@CelPhone,@IsDefault,@Aliases,@Latitude,@Longitude,@LineId,@CircleId,@DepotId,@Remark)");
            strSql3.Append(";select @@IDENTITY");
            SqlParameter[] parameters3 = {
                    new SqlParameter("@RegionId", SqlDbType.Int,4),
                    new SqlParameter("@UserId", SqlDbType.Int,4),
                    new SqlParameter("@ShipName", SqlDbType.NVarChar,50),
                    new SqlParameter("@Address", SqlDbType.NVarChar,300),
                    new SqlParameter("@Zipcode", SqlDbType.NVarChar,20),
                    new SqlParameter("@EmailAddress", SqlDbType.NVarChar,100),
                    new SqlParameter("@TelPhone", SqlDbType.NVarChar,50),
                    new SqlParameter("@CelPhone", SqlDbType.NVarChar,50),
                    new SqlParameter("@IsDefault", SqlDbType.Bit,1),
                    new SqlParameter("@Aliases", SqlDbType.NVarChar,100),
                    new SqlParameter("@Latitude", SqlDbType.Decimal,9),
                    new SqlParameter("@Longitude", SqlDbType.Decimal,9),
                    new SqlParameter("@LineId", SqlDbType.Int,4),
                    new SqlParameter("@CircleId", SqlDbType.Int,4),
                    new SqlParameter("@DepotId", SqlDbType.Int,4),
                    new SqlParameter("@Remark", SqlDbType.NChar,1000)};
            parameters3[0].Value = addressModel.RegionId;
            parameters3[1].Value = addressModel.UserId;
            parameters3[2].Value = addressModel.ShipName;
            parameters3[3].Value = addressModel.Address;
            parameters3[4].Value = addressModel.Zipcode;
            parameters3[5].Value = addressModel.EmailAddress;
            parameters3[6].Value = addressModel.TelPhone;
            parameters3[7].Value = addressModel.CelPhone;
            parameters3[8].Value = addressModel.IsDefault;
            parameters3[9].Value = addressModel.Aliases;
            parameters3[10].Value = addressModel.Latitude;
            parameters3[11].Value = addressModel.Longitude;
            parameters3[12].Value = addressModel.LineId;
            parameters3[13].Value = addressModel.CircleId;
            parameters3[14].Value = addressModel.DepotId;
            parameters3[15].Value = addressModel.Remark;
            cmd = new CommandInfo(strSql3.ToString(), parameters3);
            sqllist.Add(cmd);

            int rows = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool UpdateQQ(int userId, string qq)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Accounts_UsersExp set ");
            strSql.Append("QQ=@QQ ");
            strSql.Append(" where UserID=@UserID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@QQ", SqlDbType.NVarChar,50),
                    new SqlParameter("@UserID", SqlDbType.Int,4)};
            parameters[0].Value = qq;
            parameters[1].Value = userId;

            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion 扩展方法
    }
}