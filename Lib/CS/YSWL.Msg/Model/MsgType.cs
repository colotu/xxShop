using System;

namespace YSWL.Msg.Model
{
    /// <summary>
    /// MsgType:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class MsgType
    {
        public MsgType()
        {}
        #region Model
        private int _id;
        private string _title;
        private string _usertype;
        private string _remark;
        private string _other;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set{ _id=value;}
            get{return _id;}
        }
        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            set{ _title=value;}
            get{return _title;}
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserType
        {
            set{ _usertype=value;}
            get{return _usertype;}
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            set{ _remark=value;}
            get{return _remark;}
        }
        /// <summary>
        /// 
        /// </summary>
        public string Other
        {
            set{ _other=value;}
            get{return _other;}
        }
        #endregion Model

    }
}

