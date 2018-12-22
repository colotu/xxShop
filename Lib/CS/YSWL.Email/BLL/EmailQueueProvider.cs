using System;
using System.Collections.Generic;
using YSWL.Email.IDAL;

namespace YSWL.Email.BLL
{
    public static class EmailQueueProvider
    {
        private static YSWL.Email.IDAL.IEmailQueueProvider dal = YSWL.DBUtility.PubConstant.IsSQLServer ? (IEmailQueueProvider)new Email.SQLServerDAL.EmailQueueProvider() : new Email.MySqlDAL.EmailQueueProvider();

        public static int DeleteQueuedEmail(int emailId)
        {
            return dal.DeleteQueuedEmail(emailId);
        }

        public static IList<YSWL.Email.Model.EmailTemplate> DequeueEmail()
        {
            return dal.DequeueEmail();
        }

        public static int QueueSendingFailure(IList<int> list, int failureInterval, int maxNumberOfTries)
        {
            return dal.QueueSendingFailure(list, failureInterval, maxNumberOfTries);
        }
    }
}