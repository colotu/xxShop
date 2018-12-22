using System;
namespace YSWL.MALL.Model.Members
{
	/// <summary>
	/// UserRank:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class UserRank
	{
		public UserRank()
		{}
        #region Model
        private int _rankid;
        private string _name;
        private int _ranklevel = 0;
        private int _scoremax;
        private int _scoremin;
        private bool _isdefault;
        private int _ranktype;
        private string _description;
        private int? _createduserid;
        /// <summary>
        /// 
        /// </summary>
        public int RankId
        {
            set { _rankid = value; }
            get { return _rankid; }
        }
        /// <summary>
        /// 等级名称
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 等级排名
        /// </summary>
        public int RankLevel
        {
            set { _ranklevel = value; }
            get { return _ranklevel; }
        }
        /// <summary>
        /// 等级所需最高成长值
        /// </summary>
        public int ScoreMax
        {
            set { _scoremax = value; }
            get { return _scoremax; }
        }
        /// <summary>
        /// 等级所需最低成长值
        /// </summary>
        public int ScoreMin
        {
            set { _scoremin = value; }
            get { return _scoremin; }
        }
        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault
        {
            set { _isdefault = value; }
            get { return _isdefault; }
        }
        /// <summary>
        /// 等级类型  0:普通会员  1：代理商 
        /// </summary>
        public int RankType
        {
            set { _ranktype = value; }
            get { return _ranktype; }
        }
        /// <summary>
        /// 等级描述
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 创建者
        /// </summary>
        public int? CreatedUserId
        {
            set { _createduserid = value; }
            get { return _createduserid; }
        }
        #endregion Model

	}
}

