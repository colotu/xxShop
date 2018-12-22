using System;

namespace YSWL.Msg.Model
{
    /// <summary>
    /// MsgBox:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class MsgBox
    {
        public MsgBox()
        {}
        #region Model
        private int _id;
        private string _senderid;
        private string _receiverid;
        private string _title;
        private string _content;
        private string _msgtype;
        private DateTime? _sendtime;
        private DateTime? _readtime;
        private string _remark = "0";
        private string _other;
        private bool? _receiverisread;
        private bool? _senderisdel;
        private bool? _readerisdel;
        /// <summary>
        /// ID
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 发送者ID
        /// </summary>
        public string SenderID
        {
            set { _senderid = value; }
            get { return _senderid; }
        }
        /// <summary>
        /// 接受者ID
        /// </summary>
        public string ReceiverID
        {
            set { _receiverid = value; }
            get { return _receiverid; }
        }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content
        {
            set { _content = value; }
            get { return _content; }
        }
        /// <summary>
        /// msg类型，0为不同用户发的信息，1为系统消息
        /// </summary>
        public string MsgType
        {
            set { _msgtype = value; }
            get { return _msgtype; }
        }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime? SendTime
        {
            set { _sendtime = value; }
            get { return _sendtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ReadTime
        {
            set { _readtime = value; }
            get { return _readtime; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string ReMark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 预留字段
        /// </summary>
        public string Other
        {
            set { _other = value; }
            get { return _other; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? ReceiverIsRead
        {
            set { _receiverisread = value; }
            get { return _receiverisread; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? SenderIsDel
        {
            set { _senderisdel = value; }
            get { return _senderisdel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? ReaderIsDel
        {
            set { _readerisdel = value; }
            get { return _readerisdel; }
        }
        #endregion Model

    }
}

