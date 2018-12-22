/**
* Comment.cs
*
* 功 能： [N/A]
* 类 名： Comment
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/5/23 15:36:00  Administrator    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

namespace YSWL.MALL.DALFactory
{
    public class DAMembers : DataAccessBase
    {
       
        /// <summary>
        /// 创建Guestbook数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Members.IGuestbook CreateGuestbook()
        {
            string ClassNamespace = AssemblyPath + ".Guestbook.Guestbook";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Members.IGuestbook)objType;
        }


        /// <summary>
        /// 创建EntryForm数据层接口。报名表
        /// </summary>
        public static YSWL.MALL.IDAL.Ms.IEntryForm CreateEntryForm()
        {
            string ClassNamespace = AssemblyPath + ".Ms.EntryForm";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Ms.IEntryForm)objType;
        }

        /// <summary>
        /// 创建ConfigSystem数据层接口
        /// </summary>
        public static YSWL.MALL.IDAL.IMailConfig CreateMailConfig()
        {
            string ClassNamespace = AssemblyPath + ".MailConfig";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.IMailConfig)objType;
        }

        /// <summary>
        /// 创建SiteMessage数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Members.ISiteMessage CreateSiteMessage()
        {
            string ClassNamespace = AssemblyPath + ".Members.SiteMessage";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Members.ISiteMessage)objType;
        }

        /// <summary>
        /// 创建SiteMessageLog数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Members.ISiteMessageLog CreateSiteMessageLog()
        {
            string ClassNamespace = AssemblyPath + ".Members.SiteMessageLog";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Members.ISiteMessageLog)objType;
        }

        /// <summary>
        /// 创建PointsDetail数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Members.IPointsDetail CreatePointsDetail()
        {
            string ClassNamespace = AssemblyPath + ".Members.PointsDetail";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Members.IPointsDetail)objType;
        }

        /// <summary>
        /// 创建PointsLimit数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Members.IPointsLimit CreatePointsLimit()
        {
            string ClassNamespace = AssemblyPath + ".Members.PointsLimit";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Members.IPointsLimit)objType;
        }

        /// <summary>
        /// 创建PointsRule数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Members.IPointsRule CreatePointsRule()
        {
            string ClassNamespace = AssemblyPath + ".Members.PointsRule";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Members.IPointsRule)objType;
        }

         /// <summary>
        /// 创建PointsAction数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Members.IPointsAction CreatePointsAction()
        {
            string ClassNamespace = AssemblyPath + ".Members.PointsAction";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Members.IPointsAction)objType;
        }
        

        /// <summary>
        /// 创建UsersApprove数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Members.IUsersApprove CreateUsersApprove()
        {
            string ClassNamespace = AssemblyPath + ".Members.UsersApprove";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Members.IUsersApprove)objType;
        }

        /// <summary>
        /// 创建Feedback数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Members.IFeedback CreateFeedback()
        {
            string ClassNamespace = AssemblyPath + ".Members.Feedback";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Members.IFeedback)objType;
        }
        /// <summary>
        /// 创建Feedback数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Members.IFeedbackType CreateFeedbackType()
        {
            string ClassNamespace = AssemblyPath + ".Members.FeedbackType";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Members.IFeedbackType)objType;
        }


        /// <summary>
        /// 创建UsersExp数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Members.IUserBind CreateUserBind()
        {
            string ClassNamespace = AssemblyPath + ".Members.UserBind";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Members.IUserBind)objType;
        }

        /// <summary>
        /// 创建UsersExp数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Members.IUsersExp CreateUsersExp()
        {
            string ClassNamespace = AssemblyPath + ".Members.UsersExp";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Members.IUsersExp)objType;
        }

        /// <summary>
        /// 创建Users数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Members.IUsers CreateUsers()
        {
            string ClassNamespace = AssemblyPath + ".Members.Users";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Members.IUsers)objType;
        }

        #region 用户等级相关表

        /// <summary>
        /// 创建RankDetail数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Members.IRankDetail CreateRankDetail()
        {
            string ClassNamespace = AssemblyPath + ".Members.RankDetail";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Members.IRankDetail)objType;
        }

        /// <summary>
        /// 创建RankLimit数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Members.IRankLimit CreateRankLimit()
        {
            string ClassNamespace = AssemblyPath + ".Members.RankLimit";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Members.IRankLimit)objType;
        }

        /// <summary>
        /// 创建RankRule数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Members.IRankRule CreateRankRule()
        {
            string ClassNamespace = AssemblyPath + ".Members.RankRule";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Members.IRankRule)objType;
        }

        /// <summary>
        /// 创建RankAction数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Members.IRankAction CreateRankAction()
        {
            string ClassNamespace = AssemblyPath + ".Members.RankAction";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Members.IRankAction)objType;
        }

        /// <summary>
        /// 创建UserRank数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Members.IUserRank CreateUserRank()
        {
            string ClassNamespace = AssemblyPath + ".Members.UserRank";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Members.IUserRank)objType;
        }

        #endregion
        /// <summary>
        /// 创建UserInvite数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Members.IUserInvite CreateUserInvite()
        {
            string ClassNamespace = AssemblyPath + ".Members.UserInvite";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Members.IUserInvite)objType;
        }

           /// <summary>
        /// 创建UserCard数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Members.IUserCard CreateUserCard()
        {
            string ClassNamespace = AssemblyPath + ".Members.UserCard";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Members.IUserCard)objType;
        }


        /// <summary>
        /// 创建UserDistribution数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Members.IUserDistribution CreateUserDistribution()
        {
            string ClassNamespace = AssemblyPath + ".Members.UserDistribution";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Members.IUserDistribution)objType;
        }
    }
}