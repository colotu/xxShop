using System;

namespace YSWL.Msg.Model
{
    /// <summary>
    /// MsgRecord:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class MsgRecord
    {
        public MsgRecord()
        {}
        #region Model
        private int _id;
        private int? _msgboxid;
        private string _msgtype;
        private string _other;
        private string _msgstate;
        private string _receiverId;
        /// <summary>
        /// ID
        /// </summary>
        public int ID
        {
            set{ _id=value;}
            get{return _id;}
        }
        /// <summary>
        /// 系统消息的ID
        /// </summary>
        public int? MsgBoxId
        {
            set{ _msgboxid=value;}
            get{return _msgboxid;}
        }
        public string  ReceiverID
        {
            set { _receiverId = value; }
            get { return _receiverId; }
        }
        /// <summary>
        /// 类型 0：已读 1：已删除
        /// </summary>
        public string MsgType
        {
            set{ _msgtype=value;}
            get{return _msgtype;}
        }
        /// <summary>
        /// 预留字段
        /// </summary>
        public string Other
        {
            set{ _other=value;}
            get{return _other;}
        }
        /// <summary>
        /// 
        /// </summary>
        public string MsgState
        {
            set{ _msgstate=value;}
            get{return _msgstate;}
        }
        #endregion Model

    }
}

