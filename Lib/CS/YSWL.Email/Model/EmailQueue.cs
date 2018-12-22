using System;
namespace YSWL.Email.Model
{
    /// <summary>
    /// EmailQueue:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class EmailQueue
    {
        public EmailQueue()
        { }
        #region Model
        private int _emailid;
        private int _emailpriority;
        private bool _isbodyhtml;
        private string _emailto;
        private string _emailcc;
        private string _emailbcc;
        private string _emailfrom;
        private string _emailsubject;
        private string _emailbody;
        private DateTime _nexttrytime;
        private int _numberoftries = 0;
        /// <summary>
        /// 邮件ID
        /// </summary>
        public int EmailId
        {
            set { _emailid = value; }
            get { return _emailid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int EmailPriority
        {
            set { _emailpriority = value; }
            get { return _emailpriority; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsBodyHtml
        {
            set { _isbodyhtml = value; }
            get { return _isbodyhtml; }
        }
        /// <summary>
        /// 收件人
        /// </summary>
        public string EmailTo
        {
            set { _emailto = value; }
            get { return _emailto; }
        }
        /// <summary>
        /// 抄送人
        /// </summary>
        public string EmailCc
        {
            set { _emailcc = value; }
            get { return _emailcc; }
        }
        /// <summary>
        /// 秘送人
        /// </summary>
        public string EmailBcc
        {
            set { _emailbcc = value; }
            get { return _emailbcc; }
        }
        /// <summary>
        /// 发件人
        /// </summary>
        public string EmailFrom
        {
            set { _emailfrom = value; }
            get { return _emailfrom; }
        }
        /// <summary>
        /// 邮件标题
        /// </summary>
        public string EmailSubject
        {
            set { _emailsubject = value; }
            get { return _emailsubject; }
        }
        /// <summary>
        /// 邮件内容
        /// </summary>
        public string EmailBody
        {
            set { _emailbody = value; }
            get { return _emailbody; }
        }
        /// <summary>
        /// 发送失败下次发送时间
        /// </summary>
        public DateTime NextTryTime
        {
            set { _nexttrytime = value; }
            get { return _nexttrytime; }
        }
        /// <summary>
        /// 发送失败尝试次数
        /// </summary>
        public int NumberOfTries
        {
            set { _numberoftries = value; }
            get { return _numberoftries; }
        }
        #endregion Model

    }
}

