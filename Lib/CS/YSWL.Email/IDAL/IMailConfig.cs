/**
* IMailConfig.cs
*
* 功 能： [N/A]
* 类 名： IMailConfig
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/3/4 19:45:13  Ben    初版
*
* Copyright (c) 2013 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System.Data;

namespace YSWL.Email.IDAL
{
    public interface IMailConfig
    {
        /// <summary>
        ///
        /// </summary>
        bool Exists(int UserID, string Mailaddress);

        /// <summary>
        ///
        /// </summary>
        int Add(YSWL.Email.Model.MailConfig model);

        /// <summary>
        ///
        /// </summary>
        void Update(YSWL.Email.Model.MailConfig model);

        /// <summary>
        /// 删除一条数据
        /// </summary>
        void Delete(int ID);

        /// <summary>
        ///
        /// </summary>
        YSWL.Email.Model.MailConfig GetModel(int ID);

        /// <summary>
        ///
        /// </summary>
        YSWL.Email.Model.MailConfig GetModel();

        /// <summary>
        ///
        /// </summary>
        DataSet GetList(string strWhere);

        /// <summary>
        ///
        /// </summary>
        DataSet GetList(int Top, string strWhere, string filedOrder);
    }

}
