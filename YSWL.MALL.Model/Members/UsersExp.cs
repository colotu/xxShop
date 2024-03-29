﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.Model.Members
{
    /// <summary>
    /// UsersExp
    /// </summary>
    [Serializable]
    public partial class UsersExpModel : YSWL.Accounts.Bus.User
    {
        #region Model
        private string _gravatar;
        private string _singature;
        private string _telphone;
        private string _qq;
        private string _msn;
        private string _homepage;
        private DateTime? _birthday;
        private int _birthdayvisible;
        private bool _birthdayindexvisible;
        private string _constellation;
        private int _constellationvisible;
        private bool _constellationindexvisible;
        private string _nativeplace;
        private int _nativeplacevisible;
        private bool _nativeplaceindexvisible;
        private int? _regionid;
        private string _address;
        private int _addressvisible;
        private bool _addressindexvisible;
        private string _bodilyform;
        private int _bodilyformvisible;
        private bool _bodilyformindexvisible;
        private string _bloodtype;
        private int _bloodtypevisible;
        private bool _bloodtypeindexvisible;
        private string _marriaged;
        private int _marriagedvisible;
        private bool _marriagedindexvisible;
        private string _personalstatus;
        private int _personalstatusvisible;
        private bool _personalstatusindexvisible;
        private int? _grade;
        private decimal? _balance = 0M;
        private decimal? _balancecredit = 0M;
        private int? _points = 0;
        private int _rankscore=0;
        private int? _topiccount = 0;
        private int? _replytopiccount = 0;
        private int? _favtopiccount = 0;
        private int? _pvcount = 0;
        private int? _fanscount = 0;
        private int? _fellowcount = 0;
        private int? _ablumscount = 0;
        private int? _favouritescount = 0;
        private int? _favoritedcount = 0;
        private int? _sharecount = 0;
        private int? _productscount = 0;
        private string _personaldomain;
        private DateTime? _lastaccesstime;
        private string _lastaccessip;
        private DateTime? _lastposttime;
        private DateTime _lastlogintime;
        private string _remark;
        private bool _isuserdpi = false;
        private string _payaccount;
        private string _usercardcode;
        private int? _usercardtype;
        private int? _sourcetype;
        private int? _salesid;
    
        /// <summary>
        /// 用户头像
        /// </summary>
        public string Gravatar
        {
            set { _gravatar = value; }
            get { return _gravatar; }
        }
        /// <summary>
        /// 个性签名
        /// </summary>
        public string Singature
        {
            set { _singature = value; }
            get { return _singature; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TelPhone
        {
            set { _telphone = value; }
            get { return _telphone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string QQ
        {
            set { _qq = value; }
            get { return _qq; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MSN
        {
            set { _msn = value; }
            get { return _msn; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HomePage
        {
            set { _homepage = value; }
            get { return _homepage; }
        }
        /// <summary>
        /// 用户生日
        /// </summary>
        public DateTime? Birthday
        {
            set { _birthday = value; }
            get { return _birthday; }
        }
        /// <summary>
        /// 对谁可见
        /// </summary>
        public int BirthdayVisible
        {
            set { _birthdayvisible = value; }
            get { return _birthdayvisible; }
        }
        /// <summary>
        /// 首页是否显示
        /// </summary>
        public bool BirthdayIndexVisible
        {
            set { _birthdayindexvisible = value; }
            get { return _birthdayindexvisible; }
        }
        /// <summary>
        /// 星座
        /// </summary>
        public string Constellation
        {
            set { _constellation = value; }
            get { return _constellation; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ConstellationVisible
        {
            set { _constellationvisible = value; }
            get { return _constellationvisible; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool ConstellationIndexVisible
        {
            set { _constellationindexvisible = value; }
            get { return _constellationindexvisible; }
        }
        /// <summary>
        /// 籍贯
        /// </summary>
        public string NativePlace
        {
            set { _nativeplace = value; }
            get { return _nativeplace; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int NativePlaceVisible
        {
            set { _nativeplacevisible = value; }
            get { return _nativeplacevisible; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool NativePlaceIndexVisible
        {
            set { _nativeplaceindexvisible = value; }
            get { return _nativeplaceindexvisible; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? RegionId
        {
            set { _regionid = value; }
            get { return _regionid; }
        }
        /// <summary>
        /// 住址
        /// </summary>
        public string Address
        {
            set { _address = value; }
            get { return _address; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int AddressVisible
        {
            set { _addressvisible = value; }
            get { return _addressvisible; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool AddressIndexVisible
        {
            set { _addressindexvisible = value; }
            get { return _addressindexvisible; }
        }
        /// <summary>
        /// 体型
        /// </summary>
        public string BodilyForm
        {
            set { _bodilyform = value; }
            get { return _bodilyform; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int BodilyFormVisible
        {
            set { _bodilyformvisible = value; }
            get { return _bodilyformvisible; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool BodilyFormIndexVisible
        {
            set { _bodilyformindexvisible = value; }
            get { return _bodilyformindexvisible; }
        }
        /// <summary>
        /// 血型
        /// </summary>
        public string BloodType
        {
            set { _bloodtype = value; }
            get { return _bloodtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int BloodTypeVisible
        {
            set { _bloodtypevisible = value; }
            get { return _bloodtypevisible; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool BloodTypeIndexVisible
        {
            set { _bloodtypeindexvisible = value; }
            get { return _bloodtypeindexvisible; }
        }
        /// <summary>
        /// 婚姻
        /// </summary>
        public string Marriaged
        {
            set { _marriaged = value; }
            get { return _marriaged; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int MarriagedVisible
        {
            set { _marriagedvisible = value; }
            get { return _marriagedvisible; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool MarriagedIndexVisible
        {
            set { _marriagedindexvisible = value; }
            get { return _marriagedindexvisible; }
        }
        /// <summary>
        /// 身份
        /// </summary>
        public string PersonalStatus
        {
            set { _personalstatus = value; }
            get { return _personalstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int PersonalStatusVisible
        {
            set { _personalstatusvisible = value; }
            get { return _personalstatusvisible; }
        }
        /// <summary>
        ///  暂时用为 是否已对接
        /// </summary>
        public bool PersonalStatusIndexVisible
        {
            set { _personalstatusindexvisible = value; }
            get { return _personalstatusindexvisible; }
        }
        /// <summary>
        /// 等级
        /// </summary>
        public int? Grade
        {
            set { _grade = value; }
            get { return _grade; }
        }
        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal? Balance
        {
            set { _balance = value; }
            get { return _balance; }
        }
        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal? BalanceCredit
        {
            set { _balancecredit = value; }
            get { return _balancecredit; }
        }
        /// <summary>
        /// 积分
        /// </summary>
        public int? Points
        {
            set { _points = value; }
            get { return _points; }
        }
        /// <summary>
        /// 等级成长值
        /// </summary>
        public int RankScore
        {
            set { _rankscore = value; }
            get { return _rankscore; }
        }
        /// <summary>
        /// 发表小组话题的个数
        /// </summary>
        public int? TopicCount
        {
            set { _topiccount = value; }
            get { return _topiccount; }
        }
        /// <summary>
        /// 回复主题的个数
        /// </summary>
        public int? ReplyTopicCount
        {
            set { _replytopiccount = value; }
            get { return _replytopiccount; }
        }
        /// <summary>
        /// 收藏主题个数
        /// </summary>
        public int? FavTopicCount
        {
            set { _favtopiccount = value; }
            get { return _favtopiccount; }
        }
        /// <summary>
        /// 访问量
        /// </summary>
        public int? PvCount
        {
            set { _pvcount = value; }
            get { return _pvcount; }
        }
        /// <summary>
        /// 粉丝数
        /// </summary>
        public int? FansCount
        {
            set { _fanscount = value; }
            get { return _fanscount; }
        }
        /// <summary>
        /// 关注数
        /// </summary>
        public int? FellowCount
        {
            set { _fellowcount = value; }
            get { return _fellowcount; }
        }
        /// <summary>
        /// 专辑的个数
        /// </summary>
        public int? AblumsCount
        {
            set { _ablumscount = value; }
            get { return _ablumscount; }
        }
        /// <summary>
        /// 喜欢的数量
        /// </summary>
        public int? FavouritesCount
        {
            set { _favouritescount = value; }
            get { return _favouritescount; }
        }
        /// <summary>
        /// 被喜欢的个数
        /// </summary>
        public int? FavoritedCount
        {
            set { _favoritedcount = value; }
            get { return _favoritedcount; }
        }
        /// <summary>
        /// 分享的数量
        /// </summary>
        public int? ShareCount
        {
            set { _sharecount = value; }
            get { return _sharecount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ProductsCount
        {
            set { _productscount = value; }
            get { return _productscount; }
        }
        /// <summary>
        /// 个人域名
        /// </summary>
        public string PersonalDomain
        {
            set { _personaldomain = value; }
            get { return _personaldomain; }
        }
        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime? LastAccessTime
        {
            set { _lastaccesstime = value; }
            get { return _lastaccesstime; }
        }
        /// <summary>
        /// 最后访问IP
        /// </summary>
        public string LastAccessIP
        {
            set { _lastaccessip = value; }
            get { return _lastaccessip; }
        }
        /// <summary>
        /// 最后发表时间
        /// </summary>
        public DateTime? LastPostTime
        {
            set { _lastposttime = value; }
            get { return _lastposttime; }
        }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastLoginTime
        {
            set { _lastlogintime = value; }
            get { return _lastlogintime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 用户是否通过实名认证
        /// </summary>
        public bool IsUserDPI
        {
            set { _isuserdpi = value; }
            get { return _isuserdpi; }
        }
        /// <summary>
        /// 支付宝帐号
        /// </summary>
        public string PayAccount
        {
            set { _payaccount = value; }
            get { return _payaccount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserCardCode
        {
            set { _usercardcode = value; }
            get { return _usercardcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? UserCardType
        {
            set { _usercardtype = value; }
            get { return _usercardtype; }
        }
        /// <summary>
        /// 1:PC 注册 2:微信注册 3：订货注册 4：400注册  5:后台代注册 6:业务员代注册
        /// </summary>
        public int? SourceType
        {
            set { _sourcetype = value; }
            get { return _sourcetype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? SalesId
        {
            set { _salesid = value; }
            get { return _salesid; }
        }
        #endregion Model


        #region 扩展属性
        private bool _isfellow = false;

        public bool IsFellow
        {
            set { _isfellow = value; }
            get { return _isfellow; }
        }

        private YSWL.MALL.Model.Members.UserRank _userRank;
        /// <summary>
        /// 会员等级
        /// </summary>
        public YSWL.MALL.Model.Members.UserRank UserRank
        {
            set { _userRank = value; }
            get { return _userRank; }
        }
        #endregion

    }
}

