using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.IDAL
{
    public interface IMailConfig
    {
        int Add(YSWL.MALL.Model.MailConfig model);
        void Delete(int ID);
        bool Exists(int UserID, string Mailaddress);
        System.Data.DataSet GetList(int Top, string strWhere, string filedOrder);
        System.Data.DataSet GetList(string strWhere);
        YSWL.MALL.Model.MailConfig GetModel(int ID);
        YSWL.MALL.Model.MailConfig GetModel();
        YSWL.MALL.Model.MailConfig GetModel(int? userId);
        void Update(YSWL.MALL.Model.MailConfig model);
    }
}
