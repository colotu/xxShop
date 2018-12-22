using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Script.Serialization;
using YSWL.MALL.BLL.CMS;
using YSWL.MALL.BLL.Ms;
using YSWL.MALL.BLL.Shop.Coupon;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.Common;
using YSWL.Email.Model;
using YSWL.Json;
using YSWL.Json.Conversion;
using YSWL.MALL.Model.SysManage;
using YSWL.WeChat.Model.Core;
using ConfigSystem = YSWL.MALL.BLL.SysManage.ConfigSystem;
using MailConfig = YSWL.MALL.BLL.MailConfig;
using MsgItem = YSWL.WeChat.Model.Core.MsgItem;
using PostMsg = YSWL.WeChat.Model.Core.PostMsg;
using User = YSWL.WeChat.BLL.Core.User;
using YSWL.Common.DEncrypt;
using YSWL.MALL.BLL.Shop.Supplier;
using YSWL.Web;

namespace YSWL.MALL.Web.Components
{
    /// <summary>
    /// 处理微信用户消息回复
    /// </summary>
    public class PostMsgHelper
    {

        #region 商品内容
        /// <summary>
        /// 获取商品数据
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static YSWL.WeChat.Model.Core.PostMsg GetProductMsg(YSWL.WeChat.Model.Core.RequestMsg msg, YSWL.WeChat.Model.Core.Command command, int step = 1)
        {

            YSWL.WeChat.Model.Core.PostMsg postMsg = new PostMsg();
            YSWL.MALL.BLL.Shop.Products.ProductInfo productInfoBll = new ProductInfo();
            //获取指令关键字
            string keyWords = step == 1 ? YSWL.WeChat.BLL.Core.Command.GetKeyWord(command, msg.Description) : msg.Description;
            postMsg.CreateTime = DateTime.Now;
            postMsg.OpenId = msg.OpenId;
            postMsg.UserName = msg.UserName;
            postMsg.MsgType = "text";
            if (String.IsNullOrWhiteSpace(keyWords))
            {
                SetActionCache(msg.UserName, command.ActionId);
                postMsg.Description = "请输入关键字内容信息！";
                return postMsg;
            }
            #region 记录用户点击日志
            YSWL.WeChat.BLL.Core.OPLog.AddClickLog(msg.UserName, msg.OpenId, 2, keyWords);
            #endregion
            int top = Common.Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("MShop_ProductMsg_TopCount"), 5);
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> productInfos = productInfoBll.GetKeyWordList(top, keyWords);
            if (productInfos != null && productInfos.Count > 0)
            {
                postMsg.ArticleCount = productInfos.Count;
                postMsg.MsgType = "news";
                //string MobileUrl = BLL.SysManage.ConfigSystem.GetValueByCache("QR_URL_WEBSITE", ApplicationKeyType.Mobile);
                string HostUrl = "http://" + Common.Globals.DomainFullName;
                int i = 0;
                foreach (var productInfo in productInfos)
                {
                    YSWL.WeChat.Model.Core.MsgItem item = new MsgItem();
                    if (i == 0)
                    {
                        item.PicUrl = String.IsNullOrWhiteSpace(productInfo.ImageUrl) ? HostUrl + "/Images/NoImage.png" : HostUrl + productInfo.ImageUrl;
                    }
                    else
                    {
                        item.PicUrl = String.IsNullOrWhiteSpace(productInfo.ImageUrl) ? HostUrl + "/Images/NoImage2.png" : HostUrl + productInfo.ImageUrl;
                    }
                    //item.PicUrl = MobileUrl.Contains(HostUrl) ? HostUrl : MobileUrl + Components.FileHelper.GeThumbImage(productInfo.ThumbnailUrl1, "T175X228_");
                    //item.Url = MobileUrl.Contains(HostUrl) ? YSWL.WeChat.BLL.Core.Utils.GetWCUrl(msg, HostUrl + YSWL.Components.MvcApplication.GetCurrentRoutePath(YSWL.Web.AreaRoute.MShop) + "p/d/" + productInfo.ProductId) : YSWL.WeChat.BLL.Core.Utils.GetWCUrl(msg, MobileUrl + YSWL.Components.MvcApplication.GetCurrentRoutePath(YSWL.Web.AreaRoute.MShop)  +"p/d/" + productInfo.ProductId);
                    item.Url = YSWL.WeChat.BLL.Core.Utils.GetWCUrl(msg, YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.MShop) + "p/d/" + productInfo.ProductId);
                    item.Title = productInfo.ProductName;
                    item.Description = productInfo.ShortDescription;
                    postMsg.MsgItems.Add(item);
                }
            }
            else
            {
                postMsg.Description = "对不起，您查询的指令关键字没有返回内容！";
            }
            ClearCache(msg.UserName);
            return postMsg;
        }
        #endregion

        #region 更新昵称
        /// <summary>
        /// 更新微信用户昵称
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private static YSWL.WeChat.Model.Core.PostMsg UpdateNickMsg(YSWL.WeChat.Model.Core.RequestMsg msg,
                                                                    YSWL.WeChat.Model.Core.Command command, int step = 1)
        {
            YSWL.WeChat.Model.Core.PostMsg postMsg = new PostMsg();

            YSWL.WeChat.BLL.Core.User userBll = new User();
            //获取指令关键字
            postMsg.CreateTime = DateTime.Now;
            postMsg.OpenId = msg.OpenId;
            postMsg.UserName = msg.UserName;
            postMsg.MsgType = "text";
            string keyWords = step == 1 ? YSWL.WeChat.BLL.Core.Command.GetKeyWord(command, msg.Description) : msg.Description;
            if (String.IsNullOrWhiteSpace(keyWords))
            {
                SetActionCache(msg.UserName, command.ActionId);
                postMsg.Description = "请输入您的昵称！";
                return postMsg;
            }
            #region 记录用户点击日志
            YSWL.WeChat.BLL.Core.OPLog.AddClickLog(msg.UserName, msg.OpenId, 3, keyWords);
            #endregion
            bool isSuccess = userBll.UpdateNick(msg.UserName, msg.OpenId, keyWords);
            if (isSuccess)
            {
                ClearCache(msg.UserName);
                postMsg.Description = "更新昵称成功！";
            }
            else
            {
                postMsg.Description = "服务器繁忙，请稍候再试！";
            }
            return postMsg;
        }
        #endregion

        #region 用户绑定功能
        private static YSWL.WeChat.Model.Core.PostMsg GetUserBindMsg(YSWL.WeChat.Model.Core.RequestMsg msg)
        {
            YSWL.WeChat.Model.Core.PostMsg postMsg = new PostMsg();
            postMsg.CreateTime = DateTime.Now;
            postMsg.OpenId = msg.OpenId;
            postMsg.UserName = msg.UserName;
            //获取指令关键字
            string MobileUrl = BLL.SysManage.ConfigSystem.GetValueByCache("QR_URL_WEBSITE", ApplicationKeyType.Mobile);
            string HostUrl = "http://" + Common.Globals.DomainFullName;
            //返回图文信息
            postMsg.ArticleCount = 1;
            postMsg.MsgType = "news";
            YSWL.WeChat.Model.Core.MsgItem item = new MsgItem();
            item.Description = String.Format("如果您有帐号，请点击进行用户绑定");
            item.PicUrl = "";
            item.Title = "用户帐号绑定";
            item.Url = String.Format("{2}/Account/userbind?user={0}&open={1}", msg.UserName, msg.OpenId, MobileUrl.Contains(HostUrl) ? HostUrl : MobileUrl);
            postMsg.MsgItems.Add(item);
            return postMsg;
        }
        #endregion

        #region 查询栏目文章
        /// <summary>
        /// 查询栏目文章
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private static YSWL.WeChat.Model.Core.PostMsg GetArticles(YSWL.WeChat.Model.Core.RequestMsg msg,
                                                                    YSWL.WeChat.Model.Core.Command command, int step = 1)
        {
            YSWL.WeChat.Model.Core.PostMsg postMsg = new PostMsg();
            postMsg.CreateTime = DateTime.Now;
            postMsg.OpenId = msg.OpenId;
            postMsg.UserName = msg.UserName;
            postMsg.MsgType = "text";
            YSWL.MALL.BLL.CMS.Content contentBll = new Content();
            //获取指令关键字
            string keyWords = step == 1 ? YSWL.WeChat.BLL.Core.Command.GetKeyWord(command, msg.Description) : msg.Description;
            //if (String.IsNullOrWhiteSpace(keyWords))
            //{
            //    SetActionCache(msg.UserName, command.ActionId);
            //    postMsg.Description = "请输入关键字内容信息！";
            //    return postMsg;
            //}
            #region 记录用户点击日志
            YSWL.WeChat.BLL.Core.OPLog.AddClickLog(msg.UserName, msg.OpenId, 1, keyWords);
            #endregion
            int articleCount = Common.Globals.SafeInt(YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("WeChat_Article_Count"), 8);
            List<YSWL.MALL.Model.CMS.Content> contentList = contentBll.GetWeChatList(command.TargetId, keyWords, articleCount);
            if (contentList != null && contentList.Count > 0)
            {
                string HostUrl = "http://" + Common.Globals.DomainFullName;
                postMsg.ArticleCount = contentList.Count;
                postMsg.MsgType = "news";
                string basePath = YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.COM);
                int i = 0;
                foreach (var content in contentList)
                {
                    YSWL.WeChat.Model.Core.MsgItem item = new MsgItem();
                    if (i == 0)
                    {
                        item.PicUrl = String.IsNullOrWhiteSpace(content.ImageUrl) ? HostUrl + "/Images/NoImage.png" : HostUrl + content.ImageUrl;
                    }
                    else
                    {
                        item.PicUrl = String.IsNullOrWhiteSpace(content.ImageUrl) ? HostUrl + "/Images/NoImage2.png" : HostUrl + content.ImageUrl;
                    }
                    item.Url = YSWL.WeChat.BLL.Core.Utils.GetWCUrl(msg, basePath + "article/detail/" + content.ContentID);
                    item.Title = content.Title;
                    item.Description = content.Summary;
                    postMsg.MsgItems.Add(item);
                    i++;
                }

            }
            else
            {
                postMsg.Description = "对不起，您查询的指令关键字没有返回内容！";
            }
            ClearCache(msg.UserName);
            return postMsg;
        }
        #endregion
        
        #region 获取优惠券
        /// <summary>
        /// 微信优惠券分配
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private static YSWL.WeChat.Model.Core.PostMsg GetCouponInfo(YSWL.WeChat.Model.Core.RequestMsg msg)
        {
            YSWL.WeChat.Model.Core.PostMsg postMsg = new PostMsg();
            postMsg.CreateTime = DateTime.Now;
            postMsg.OpenId = msg.OpenId;
            postMsg.UserName = msg.UserName;
            postMsg.MsgType = "text";
            YSWL.MALL.BLL.Shop.Coupon.CouponInfo infoBll = new CouponInfo();
            bool IsNeedBind = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("SyStem_WeChat_UserBind");
            //获取绑定用户信息
            int ruleId = 0;
            if (IsNeedBind)
            {
                YSWL.WeChat.BLL.Core.User wUserBll = new User();
                YSWL.WeChat.Model.Core.User wUserModel = wUserBll.GetUser(msg.OpenId, msg.UserName);
                if (wUserModel == null || wUserModel.UserId <= 0)
                {
                    string tag = string.Empty;
                    string returnUrl = YSWL.WeChat.BLL.Core.Utils.GetWCUrl(msg, "/COM/WeChat/UserBind?"+tag, false);
                    postMsg.Description = String.Format("您还未绑定微信商城帐号，为了方便给您更好的服务，请您先绑定商城帐号，<a href=\"{0}\">点击绑定</a>！", returnUrl);
                    return postMsg;
                }
                ruleId=infoBll.GetRuleId(wUserModel.UserId);
                if (ruleId==0)
                {
                    postMsg.Description = "亲，每个优惠券活动每人只有一次机会哦！";
                    return postMsg;
                }
            }
            else
            {
                ruleId = infoBll.GetRuleId(msg.UserName);
                if (ruleId==0)
                {
                    postMsg.Description = "亲，每个优惠券活动每人只有一次机会哦！";
                    return postMsg;
                }
            }

            YSWL.MALL.Model.Shop.Coupon.CouponInfo infoModel = infoBll.GetCoupon(msg.OpenId, msg.UserName, ruleId);
            if (infoModel == null)
            {
                postMsg.Description = "亲，您来晚了！优惠券已经被抢没了，下次记得早点哦，谢谢您的参与！";
                return postMsg;
            }
            YSWL.MALL.BLL.Shop.Coupon.CouponRule rubleBll = new CouponRule();
            YSWL.MALL.Model.Shop.Coupon.CouponRule ruleModel = rubleBll.GetModelByCache(infoModel.RuleId);
            postMsg.ArticleCount = 1;
            postMsg.MsgType = "news";
            YSWL.WeChat.Model.Core.MsgItem item = new MsgItem();
            item.PicUrl = "http://" + Common.Globals.DomainFullName + "/Images/d-02.jpg";
            item.Url = "";
            item.Title = ruleModel == null ? "优惠券抢购活动" : ruleModel.Name;
            item.Description = "亲，恭喜你已经获得一张体验优惠券，请尽快使用，每天关注，惊喜不断^_^, 您获得的优惠券号码：" + infoModel.CouponCode + "，面值为：" + infoModel.CouponPrice.ToString("F") + "元，" +
                               "有效期" + infoModel.StartDate.ToString("yyyy-MM-dd") + " 至 " + infoModel.EndDate.ToString("yyyy-MM-dd");
            postMsg.MsgItems.Add(item);
            return postMsg;
        }
        #endregion

        #region 物流消息
        private static YSWL.WeChat.Model.Core.PostMsg GetExpressInfo(YSWL.WeChat.Model.Core.RequestMsg msg, YSWL.WeChat.Model.Core.Command command, int step = 1)
        {
            YSWL.WeChat.Model.Core.PostMsg postMsg = new PostMsg();
            postMsg.CreateTime = DateTime.Now;
            postMsg.OpenId = msg.OpenId;
            postMsg.UserName = msg.UserName;
            postMsg.MsgType = "text";
            //获取指令关键字
            string keyWords = step == 1 ? YSWL.WeChat.BLL.Core.Command.GetKeyWord(command, msg.Description) : msg.Description;
            postMsg.CreateTime = DateTime.Now;
            postMsg.OpenId = msg.OpenId;
            postMsg.UserName = msg.UserName;
            postMsg.MsgType = "text";
            if (String.IsNullOrWhiteSpace(keyWords))
            {
                SetActionCache(msg.UserName, command.ActionId);
                postMsg.Description = "请输入您的订单号！";
                return postMsg;
            }
            #region 记录用户点击日志
            YSWL.WeChat.BLL.Core.OPLog.AddClickLog(msg.UserName, msg.OpenId, 11, keyWords);
            #endregion
            List<YSWL.MALL.ViewModel.Shop.Express> expressList = YSWL.MALL.Web.Components.ExpressHelper.GetExpress(keyWords);
            if (expressList == null || expressList.Count == 0)
            {
                postMsg.Description = "您输入的订单号暂无物流信息！";
            }
            else
            {
                foreach (var item in expressList)
                {
                    postMsg.Description += "【" + item.Date + "】" + item.Content + "\n";
                }
            }

            return postMsg;
        }
        #endregion

        #region 微信活动消息
        private static YSWL.WeChat.Model.Core.PostMsg GetActivityInfo(YSWL.WeChat.Model.Core.RequestMsg msg,
            YSWL.WeChat.Model.Core.Command command, int type = 0, int step = 1)
        {
            YSWL.WeChat.Model.Core.PostMsg postMsg = new PostMsg();
            postMsg.CreateTime = DateTime.Now;
            postMsg.OpenId = msg.OpenId;
            postMsg.UserName = msg.UserName;
            postMsg.MsgType = "text";

            YSWL.WeChat.BLL.Activity.ActivityInfo infoBll = new WeChat.BLL.Activity.ActivityInfo();

            YSWL.WeChat.Model.Activity.ActivityInfo infoModel = infoBll.GetActivity(0, type);
            if (infoModel == null)
            {
                postMsg.Description = "亲，您来晚了！该活动已经结束，下次记得早点哦，谢谢您的参与！";
                return postMsg;
            }
            if (infoModel.AwardType == 1)
            {
                bool IsNeedBind = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("SyStem_WeChat_UserBind");
                if (IsNeedBind)
                {
                    YSWL.WeChat.BLL.Core.User wUserBll = new User();
                    YSWL.WeChat.Model.Core.User wUserModel = wUserBll.GetUser(msg.OpenId, msg.UserName);
                    if (wUserModel == null || wUserModel.UserId <= 0)
                    {
                        string tag = string.Empty;
                        string returnUrl = YSWL.WeChat.BLL.Core.Utils.GetWCUrl(msg, "/COM/WeChat/UserBind?"+ tag, false);
                        postMsg.Description = String.Format("您还未绑定微信商城帐号，为了方便给您更好的服务，请您先绑定商城帐号，<a href=\"{0}\">点击绑定</a>！", returnUrl);
                        return postMsg;
                    }
                }
            }
            postMsg.ArticleCount = 1;
            postMsg.MsgType = "news";
            YSWL.WeChat.Model.Core.MsgItem item = new MsgItem();
            item.PicUrl = String.IsNullOrWhiteSpace(infoModel.ImageUrl) ? "" : "http://" + Common.Globals.DomainFullName + String.Format(infoModel.ImageUrl, "N_");
            string basePath = YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.COM);
            switch (type)
            {
                case 0:
                    item.Url = YSWL.WeChat.BLL.Core.Utils.GetWCUrl(msg, basePath + "WeChat/Scratch/" + infoModel.ActivityId, false);
                    break;
                case 1:
                    item.Url = YSWL.WeChat.BLL.Core.Utils.GetWCUrl(msg, basePath + "WeChat/BigWheel/" + infoModel.ActivityId, false);
                    break;
                default:
                    item.Url = YSWL.WeChat.BLL.Core.Utils.GetWCUrl(msg, basePath + "WeChat/Scratch/" + infoModel.ActivityId, false);
                    break;
            }
            item.Title = infoModel.Name;
            item.Description = infoModel.Summary;
            postMsg.MsgItems.Add(item);
            return postMsg;
        }
        #endregion

        #region 获取活动SN码
        private static YSWL.WeChat.Model.Core.PostMsg GetSNCode(YSWL.WeChat.Model.Core.RequestMsg msg)
        {
            YSWL.WeChat.Model.Core.PostMsg postMsg = new PostMsg();
            postMsg.CreateTime = DateTime.Now;
            postMsg.OpenId = msg.OpenId;
            postMsg.UserName = msg.UserName;
            postMsg.MsgType = "text";
            YSWL.WeChat.BLL.Activity.ActivityCode codeBll = new WeChat.BLL.Activity.ActivityCode();
            List<YSWL.WeChat.Model.Activity.ActivityCode> codeList = codeBll.GetUserCodes(msg.UserName, 5);
            if (codeList == null || codeList.Count == 0)
            {
                postMsg.Description = "亲，您暂无奖品SN码，继续努力哦";
                return postMsg;
            }
            foreach (var item in codeList)
            {
                postMsg.Description += item.ActivityName + "中,获得 " + item.AwardName
                    + ",奖品SN码为：" + item.CodeName + ",状态为：" + (item.Status == 1 ? "未使用" : "已使用") + "\n";
            }
            return postMsg;
        }
        #endregion

        #region 处理消息
        /// <summary>
        /// 处理相关指令数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetCommandMsg(YSWL.WeChat.Model.Core.Command command, YSWL.WeChat.Model.Core.RequestMsg msg)
        {
            //清除连续指令 
            ClearCache(msg.UserName);
            string xmlStr = "";
            //设置指令缓存 方便连续指令使用
            YSWL.MALL.Web.Components.PostMsgHelper.SetCommandCache(command, msg.UserName);
            //匹配指令
            YSWL.WeChat.Model.Core.PostMsg msgModel = GetActionMsg(msg, command.ActionId, command);
            if (msgModel != null && !String.IsNullOrWhiteSpace(msgModel.OpenId))
            {
                xmlStr = YSWL.WeChat.BLL.Core.PostMsg.GetPostMsgXML(msgModel);
            }
            return xmlStr;
        }

        /// <summary>
        /// 获取事件返回消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string GetEventMsg(YSWL.WeChat.Model.Core.RequestMsg msg)
        {
            string xmlStr = "";
            //获取事件指定的关键字
            YSWL.WeChat.BLL.Core.Menu menuBll = new WeChat.BLL.Core.Menu();
            int actionId = YSWL.WeChat.BLL.Core.Action.GetActionId(msg.EventKey);
            YSWL.WeChat.Model.Core.Menu menuModel = menuBll.GetMenu(msg.EventKey);
            YSWL.WeChat.Model.Core.Command command = new Command();
            msg.Description = menuModel == null ? "" : menuModel.Remark;
            if (actionId == 1)
            {
                command.TargetId = menuModel == null ? 0 : Common.Globals.SafeInt(menuModel.Remark, 0);
                msg.Description = "";
            }
            //匹配指令
            YSWL.WeChat.Model.Core.PostMsg msgModel = GetActionMsg(msg, actionId, command, 2);
            if (msgModel != null && !String.IsNullOrWhiteSpace(msgModel.OpenId))
            {
                xmlStr = YSWL.WeChat.BLL.Core.PostMsg.GetPostMsgXML(msgModel);
            }
            return xmlStr;
        }

        #region 处理地理位置信息
        /// <summary>
        /// 获取地理位置处理信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string GetLocationMsg(YSWL.WeChat.Model.Core.RequestMsg msg)
        {
            string xmlStr = "";
            //获取开关设置
            int LocationType = Common.Globals.SafeInt(YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_LocationMsg", msg.OpenId), 0);
            switch (LocationType)
            {
                case 0:
                    return xmlStr;
                case 1:
                    YSWL.WeChat.Model.Core.PostMsg msgModel = YSWL.WeChat.BLL.Push.TaskMsg.GetPushMsg(msg.OpenId, msg.UserName);
                    if (msgModel != null && !String.IsNullOrWhiteSpace(msgModel.OpenId))
                    {
                        xmlStr = YSWL.WeChat.BLL.Core.PostMsg.GetPostMsgXML(msgModel);
                    }
                    break;
                default:
        
                    return xmlStr;
            }
            return xmlStr;
        }
        #endregion

        public static YSWL.WeChat.Model.Core.PostMsg GetActionMsg(YSWL.WeChat.Model.Core.RequestMsg msg, int actionId, YSWL.WeChat.Model.Core.Command command, int step = 1)
        {
            YSWL.WeChat.Model.Core.PostMsg msgModel = new YSWL.WeChat.Model.Core.PostMsg();
            command.ActionId = actionId;
            switch (actionId)
            {
                
                //获取CMS文章数据
                case 1:
                    msgModel = GetArticles(msg, command, step);
                    break;
                // 获取Shop商品数据
                case 2:
                    msgModel = GetProductMsg(msg, command, step);
                    break;
                //修改昵称
                case 3:
                    msgModel = UpdateNickMsg(msg, command, step);
                    break;
                //查询栏目
                case 4:
                    break;
                //获取优惠券
                case 8:
                    #region 记录用户点击日志
                    YSWL.WeChat.BLL.Core.OPLog.AddClickLog(msg.UserName, msg.OpenId, 8);
                    #endregion
                    msgModel = GetCouponInfo(msg);
                    break;
                //用户绑定
                case 9:
                    #region 记录用户点击日志
                    YSWL.WeChat.BLL.Core.OPLog.AddClickLog(msg.UserName, msg.OpenId, 9);
                    #endregion
                    msgModel = GetUserBindMsg(msg);
                    break;
                //物流消息
                case 11:
                    msgModel = GetExpressInfo(msg, command, step);
                    break;
                //刮刮卡
                case 12:
                    #region 记录用户点击日志
                    YSWL.WeChat.BLL.Core.OPLog.AddClickLog(msg.UserName, msg.OpenId, 12);
                    #endregion
                    msgModel = GetActivityInfo(msg, command, 0, step);
                    break;
                //大转盘
                case 13:
                    #region 记录用户点击日志
                    YSWL.WeChat.BLL.Core.OPLog.AddClickLog(msg.UserName, msg.OpenId, 13);
                    #endregion
                    msgModel = GetActivityInfo(msg, command, 1, step);
                    break;
                //获取SN码
                case 14:
                    msgModel = GetSNCode(msg);
                    break;
                //推广二维码
                case 15:
                    msgModel = GetQRPostMsg(msg, command);
                    break;
                default:
                    break;
            }
            return msgModel;
        }

        /// <summary>
        /// 处理连续指令数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetContinueMsg(int actionId, YSWL.WeChat.Model.Core.RequestMsg msg)
        {
            YSWL.WeChat.Model.Core.PostMsg msgModel = new YSWL.WeChat.Model.Core.PostMsg();
            string xmlStr = "";
            //匹配指令
            SetTipCount(msg.UserName);
            YSWL.WeChat.Model.Core.Command command = GetCommandCache(msg.UserName);
            switch (actionId)
            {
                //获取CMS文章数据
                case 1:
                    msgModel = GetArticles(msg, command, 2);
                    break;
                // 获取Shop商品数据
                case 2:
                    msgModel = GetProductMsg(msg, command, 2);
                    break;
                //修改昵称
                case 3:
                    msgModel = UpdateNickMsg(msg, command, 2);
                    break;
                //物流消息
                case 11:
                    msgModel = GetExpressInfo(msg, command, 2);
                    break;
                default:
                    break;
            }
            if (msgModel != null && !String.IsNullOrWhiteSpace(msgModel.OpenId))
            {
                xmlStr = YSWL.WeChat.BLL.Core.PostMsg.GetPostMsgXML(msgModel);
            }
            return xmlStr;
        }

        /// <summary>
        /// 获取自动回复消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string GetAutoMsg(YSWL.WeChat.Model.Core.RequestMsg msg, bool IsTransfer=false)
        {
            YSWL.WeChat.Model.Core.PostMsg msgModel = new YSWL.WeChat.Model.Core.PostMsg();
            //获取自动回复消息
            msgModel = YSWL.WeChat.BLL.Core.PostMsg.GetAutoMsg(msg, IsTransfer);
            msgModel.OpenId = msg.OpenId;
            msgModel.UserName = msg.UserName;
            //是否发送邮件
            bool isSendEmail = YSWL.WeChat.BLL.Core.Config.isSendEmail(msgModel.OpenId, "WeChat_ChkNoMsg");
            if (isSendEmail)
            {
                SendWeChatEmail(msg);
            }
            return YSWL.WeChat.BLL.Core.PostMsg.GetPostMsgXML(msgModel);
        }
        #endregion

        #region 设置缓存
        /// <summary>
        /// 写入缓存连续指令(写入指令缓存)
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static int SetActionCache(string userName, int actionId)
        {
            string CacheKey = "ActionCache-" + userName;
            object objModel = null;
            try
            {
                objModel = actionId;
                if (objModel != null)
                {
                    int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ActionCache"), 3);
                    YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                }
            }
            catch { }
            return (int)objModel;
        }


        /// <summary>
        /// 获取缓存连续指令(获取指令缓存)
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static int GetActionCache(string userName, int tipCount = 2)
        {
            int count = GetTipCount(userName);
            if (count < tipCount)
            {
                string CacheKey = "ActionCache-" + userName;
                object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
                return objModel == null ? 0 : (int)objModel;
            }
            return 0;
        }
        /// <summary>
        /// 设置提示次数缓存
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static int SetTipCount(string userName)
        {
            string CacheKey = "TipCount-" + userName;
            object objModel = null;
            try
            {
                objModel = GetTipCount(userName) + 1;
                int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("CommandCache"), 5);
                YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
            }
            catch { }
            return objModel == null ? 0 : (int)objModel;
        }
        /// <summary>
        /// 获得提示次数
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static int GetTipCount(string userName)
        {
            string CacheKey = "TipCount-" + userName;

            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);

            return objModel == null ? 0 : (int)objModel;
        }
        /// <summary>
        /// 清除相关缓存
        /// </summary>
        /// <param name="userName"></param>
        public static void ClearCache(string userName)
        {
            //清除连续指令缓存
            HttpContext.Current.Cache.Remove("ActionCache-" + userName);
            //清除提示次数缓存
            HttpContext.Current.Cache.Remove("TipCount-" + userName);
            //清除指令缓存
            HttpContext.Current.Cache.Remove("CommandCache-" + userName);
        }
        /// <summary>
        /// 设置指令缓存
        /// </summary>
        /// <param name="command"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static YSWL.WeChat.Model.Core.Command SetCommandCache(YSWL.WeChat.Model.Core.Command command, string userName)
        {
            string CacheKey = "CommandCache-" + userName;
            object objModel = null;
            try
            {
                objModel = command;
                if (objModel != null)
                {
                    int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("CommandCache"), 3);
                    YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                }
            }
            catch { }
            return (YSWL.WeChat.Model.Core.Command)objModel;
        }
        /// <summary>
        /// 获取指令缓存
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static YSWL.WeChat.Model.Core.Command GetCommandCache(string userName)
        {
            string CacheKey = "CommandCache-" + userName;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            return objModel == null ? null : (YSWL.WeChat.Model.Core.Command)objModel;
        }

        #endregion

        #region 发送邮件
        /// <summary>
        /// 发送微信邮件
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool SendWeChatEmail(YSWL.WeChat.Model.Core.RequestMsg msg)
        {
            string emailBody = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_NoMsgEmailDesc", msg.OpenId);
            string emailTitle = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_NoMsgEmailTitle", msg.OpenId);
            string email = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_Email", msg.OpenId);

            if (!String.IsNullOrWhiteSpace(emailBody) && !String.IsNullOrWhiteSpace(email))
            {
                emailBody = ReplaceTag(emailBody,
                    new[] { "{Domain}", System.Web.HttpContext.Current.Request.Url.Authority },
                    new[] { "{CreatedDate}", DateTime.Now.ToString("yyyy-MM-dd") },
                       new[] { "{UserName}", msg.UserName },
                    new[] { "{Description}", msg.Description });
                try
                {
                    YSWL.MALL.BLL.MailConfig config = new MailConfig();
                    YSWL.MALL.Model.MailConfig mailModel = config.GetModel();
                    if (!String.IsNullOrWhiteSpace(mailModel.Mailaddress))
                    {
                        //单封邮件Model
                        YSWL.Email.Model.EmailQueue queueModel = new EmailQueue();
                        queueModel.EmailBody = emailBody;
                        queueModel.EmailFrom = mailModel.Mailaddress;
                        queueModel.EmailTo = email;
                        queueModel.EmailSubject = emailTitle;
                        queueModel.EmailPriority = 0;
                        queueModel.IsBodyHtml = false;
                        queueModel.NumberOfTries = 3;
                        queueModel.NextTryTime = DateTime.Now.AddMinutes(3);
                        YSWL.Email.EmailManage.PushQueue(queueModel);
                        return true;
                    }
                    // MailSender.Send(EmailUrl, emailModel.EmailSubject, emailBody);
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                    throw (e);
                }
            }
            return false;
        }

        /// <summary>
        /// 发送微信用户关注信息邮件
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool SendSubscribeEmail(YSWL.WeChat.Model.Core.RequestMsg msg)
        {
            string emailBody = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_SubMsgEmailDesc", msg.OpenId);
            string emailTitle = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_SubMsgEmailTitle", msg.OpenId);
            string email = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_Email", msg.OpenId);

            if (!String.IsNullOrWhiteSpace(emailBody) && !String.IsNullOrWhiteSpace(email))
            {
                emailBody = ReplaceTag(emailBody,
                    new[] { "{Domain}", System.Web.HttpContext.Current.Request.Url.Authority },
                    new[] { "{CreatedDate}", DateTime.Now.ToString("yyyy-MM-dd") },
                       new[] { "{UserName}", msg.UserName },
                    new[] { "{Description}", msg.Description });
                try
                {
                    YSWL.MALL.BLL.MailConfig config = new MailConfig();
                    YSWL.MALL.Model.MailConfig mailModel = config.GetModel();
                    if (!String.IsNullOrWhiteSpace(mailModel.Mailaddress))
                    {
                        YSWL.Email.Model.EmailQueue queueModel = new EmailQueue();
                        queueModel.EmailBody = emailBody;
                        queueModel.EmailFrom = mailModel.Mailaddress;
                        queueModel.EmailTo = email;
                        queueModel.EmailSubject = emailTitle;
                        queueModel.EmailPriority = 0;
                        queueModel.IsBodyHtml = false;
                        queueModel.NumberOfTries = 3;
                        queueModel.NextTryTime = DateTime.Now.AddMinutes(3);
                        YSWL.Email.EmailManage.PushQueue(queueModel);
                        return true;
                    }
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                    throw (e);
                }
            }
            return false;
        }
        #endregion

        #region 创建菜单
        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="AppSecret"></param>
        /// <returns></returns>
        public static bool CreateMenu(string access_token, string openId, bool IsAuto)
        {
            string posturl = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + access_token;
            try
            {
                int code = YSWL.WeChat.BLL.Core.Menu.CreateMenu(access_token, openId, IsAuto);
                if (code == 0)
                {
                    return true;
                }
                else
                {
                    YSWL.MALL.Model.SysManage.ErrorLog model = new YSWL.MALL.Model.SysManage.ErrorLog();
                    model.OPTime = DateTime.Now;
                    model.Loginfo = "创建微信菜单失败，失败信息错误码为：" + code;
                    model.Url = posturl;
                    YSWL.MALL.BLL.SysManage.ErrorLog.Add(model);
                    return false;
                }
            }
            catch (Exception ex)
            {
                YSWL.MALL.Model.SysManage.ErrorLog model = new YSWL.MALL.Model.SysManage.ErrorLog();
                model.OPTime = DateTime.Now;
                model.Loginfo = "创建微信菜单失败";
                model.Url = posturl;
                model.StackTrace = ex.Message;
                YSWL.MALL.BLL.SysManage.ErrorLog.Add(model);
                throw;
            }

        }
        #endregion

        #region 获取用户分组
        public static bool GetGroups(string access_token, string openId, bool isCover)
        {
            try
            {
                YSWL.WeChat.BLL.Core.Group groupBll = new WeChat.BLL.Core.Group();
                return groupBll.GetGroups(access_token, openId, isCover);
            }
            catch (Exception ex)
            {
                YSWL.MALL.Model.SysManage.ErrorLog model = new YSWL.MALL.Model.SysManage.ErrorLog();
                model.OPTime = DateTime.Now;
                model.Loginfo = "获取微信用户分组失败";
                model.Url = "";
                model.StackTrace = ex.Message;
                YSWL.MALL.BLL.SysManage.ErrorLog.Add(model);
                throw;
            }
        }
        #endregion

        #region  辅助方法
        /// <summary>
        /// 获取授权码
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="AppSecret"></param>
        /// <returns></returns>
        public static string GetToken(string AppId, string AppSecret)
        {
            try
            {
                string result = YSWL.WeChat.BLL.Core.Utils.GetToken(AppId, AppSecret);
                int code = Common.Globals.SafeInt(result, 0);
                if (code > 0)
                {
                    YSWL.MALL.Model.SysManage.ErrorLog model = new YSWL.MALL.Model.SysManage.ErrorLog();
                    model.OPTime = DateTime.Now;
                    model.Loginfo = "获取微信access_token失败, 错误码为" + code;
                    model.Url = "";
                    YSWL.MALL.BLL.SysManage.ErrorLog.Add(model);
                    return "";
                }
                return result;
            }
            catch (Exception ex)
            {
                YSWL.MALL.Model.SysManage.ErrorLog model = new YSWL.MALL.Model.SysManage.ErrorLog();
                model.OPTime = DateTime.Now;
                model.Loginfo = "获取微信access_token失败";
                model.Url = "";
                model.StackTrace = ex.Message;
                YSWL.MALL.BLL.SysManage.ErrorLog.Add(model);
                throw ex;
            }
        }

        public static string GetToken()
        {
            string AppId = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_AppId", -1, "AA");
            string AppSecret = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_AppSercet", -1, "AA");
            return GetToken(AppId, AppSecret);
        }
        /// <summary>
        /// 获取 渠道推广图片
        /// </summary>
        /// <param name="token"></param>
        /// <param name="sceneId"></param>
        /// <returns></returns>
        public static string GetQRImage(string token, int sceneId)
        {
            string ticket = YSWL.WeChat.BLL.Core.Utils.GetTicket(token, sceneId);
            if (!String.IsNullOrWhiteSpace(ticket))
            {
                string ImageBase = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}";
                return String.Format(ImageBase, ticket);//直接用远程路径，没必要
                //try
                //{
                //    using (System.Net.WebClient webclient = new System.Net.WebClient())
                //    {
                //        string savePath = "/Upload/WeChat/QRImage/";
                //        if (!Directory.Exists(HttpContext.Current.Server.MapPath(savePath)))
                //        {
                //            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(savePath));
                //        }
                //        string QRImage = savePath + "QRImage_" + sceneId + ".jpg";
                //        webclient.DownloadFile(String.Format(ImageBase, ticket), HttpContext.Current.Server.MapPath(QRImage));
                //        return QRImage;
                //    }
                //}
                //catch (Exception ex)
                //{
                //    LogHelp.AddErrorLog(ex.Message, ex.StackTrace);
                //    throw ex;
                //}
            }
            return "";

        }

        private static void ReGenImage(string origialStr)
        {
            //判断原始文件是否存在或者是否是云存储
            if (origialStr.StartsWith("http://") || !File.Exists(HttpContext.Current.Server.MapPath(origialStr)))
            {
                return;
            }

            FileInfo fileInfo = new FileInfo(HttpContext.Current.Server.MapPath(origialStr));
            //重新生成缩略图
            MakeThumbnailMode ThumbnailMode = MakeThumbnailMode.W;
            List<YSWL.MALL.Model.Ms.ThumbnailSize> thumSizeList =
                YSWL.MALL.BLL.Ms.ThumbnailSize.GetThumSizeList(YSWL.MALL.Model.Ms.EnumHelper.AreaType.SNS);
            //原始文件名
            string fileName = origialStr.Substring(origialStr.LastIndexOf('/') + 1, origialStr.Length - origialStr.LastIndexOf('/') - 1);

            string origialPath = origialStr.Substring(0, origialStr.LastIndexOf('/') + 1);

            string dir = "/Upload/SNS/Images/PhotosThumbs/" + DateTime.Now.ToString("yyyyMMdd");
            if (Directory.Exists(HttpContext.Current.Server.MapPath(dir)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(dir));
            }
            string thumbImageUrl = dir + "/{0}" + fileName;
            try
            {
                bool isAddWater = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("System_ThumbImage_AddWater");
                //原图水印保存地址
                string imagePath = origialPath;
                if (isAddWater)
                {
                    imagePath = origialPath + "W_";
                    //生成临时原图水印图
                    FileHelper.MakeWater(HttpContext.Current.Server.MapPath(origialStr), HttpContext.Current.Server.MapPath(imagePath + fileName));
                }

                //重新生成缩略图
                if (thumSizeList != null && thumSizeList.Count > 0)
                {
                    foreach (var thumSize in thumSizeList)
                    {
                        ImageTools.MakeThumbnail(HttpContext.Current.Server.MapPath(imagePath + fileName), HttpContext.Current.Server.MapPath(String.Format(thumbImageUrl, thumSize.ThumName)),
                            thumSize.ThumWidth, thumSize.ThumHeight, ThumbnailMode, ImageFormat.Jpeg);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelp.AddErrorLog(string.Format("SNS：{0}重新生成缩略图时发生异常:{1}", origialStr, ex.StackTrace), "", "重新生成缩略图时发生异常");
            }
            try
            {

            }
            catch (Exception ex)
            {
                LogHelp.AddErrorLog(string.Format("SNS：{0}重新生成缩略图更新到数据库时发生异常:{1}", origialStr, ex.StackTrace), "", "重新生成缩略图时发生异常");
            }

        }
        /// <summary>
        /// 替换标签
        /// </summary>
        /// <param name="body"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string ReplaceTag(string body, params string[][] values)
        {
            if (values == null || values.Length < 1) return body;
            foreach (string[] keyValue in values)
            {
                if (keyValue.Length != 2) continue;
                body = body.Replace(
                    keyValue[0], //Key
                        YSWL.Common.Globals.HtmlEncode(
                            keyValue[1]) //Value
                    );
            }
            return body;
        }
        
        #endregion

        #region 根据地理位置查询附近店铺
        /// <summary>
        /// 根据位置获取商家列表
        /// </summary>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="range">范围</param>
        /// <returns></returns>
        public static List<YSWL.MALL.Model.Shop.Supplier.SupplierInfo> GetShopListByPosition(double longitude,
                                                                                             double latitude,
                                                                                             double range)
        {
            YSWL.MALL.BLL.Shop.Supplier.SupplierInfo supplierInfoBll = new SupplierInfo();
            List<YSWL.MALL.Model.Shop.Supplier.SupplierInfo> supplierList = null;
            StreamReader reader = null;
            string posturl = "http://api.map.baidu.com/ag/coord/convert?from=0&to=4&x=" + longitude + "&y=" + latitude;
            //创建菜单
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(posturl);

                HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
                reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string content = reader.ReadToEnd();//得到结果
                YSWL.Json.JsonObject jsonObject = JsonConvert.Import<JsonObject>(content);
                double baiduX = double.Parse(Base64.Decode(jsonObject["x"].ToString()));
                double baiduY = double.Parse(Base64.Decode(jsonObject["y"].ToString()));
                Position.Degree degree = new Position.Degree(baiduX, baiduY);
                Position.Degree[] degrees = Position.CoordDispose.GetDegreeCoordinates(degree, range);
                if (degrees.Length != 4)//获取数据有误
                {
                    supplierList = new List<Model.Shop.Supplier.SupplierInfo>();
                }
                else
                {
                    double longitudeLow = degrees[3].X;
                    double latitudeLow = degrees[3].Y;
                    double longitudeHigh = degrees[0].X;
                    double latitudeHigh = degrees[0].Y;
                    supplierList =
                        supplierInfoBll.GetSupplierByPosition(latitudeLow, longitudeLow, latitudeHigh, longitudeHigh,
                                                              range);
                }
                return supplierList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                reader.Close();
            }
        }
        #endregion



        #region 处理推广二维码信息
        private static YSWL.WeChat.Model.Core.PostMsg GetQRPostMsg(YSWL.WeChat.Model.Core.RequestMsg msg,
                                                              YSWL.WeChat.Model.Core.Command command)
        {

            string token = YSWL.MALL.Web.Components.PostMsgHelper.GetToken();
            YSWL.WeChat.Model.Core.PostMsg postMsg = new PostMsg();
            YSWL.WeChat.BLL.Core.User userBll = new User();
            YSWL.WeChat.Model.Core.User userModel = userBll.GetUser(msg.OpenId, msg.UserName);
            postMsg.CreateTime = DateTime.Now;
            postMsg.OpenId = msg.OpenId;
            postMsg.UserName = msg.UserName;
            postMsg.MsgType = "text";
            if (userModel == null || userModel.UserId <= 0)
            {
                string tag = string.Empty;
                string returnUrl = YSWL.WeChat.BLL.Core.Utils.GetWCUrl(msg, "/COM/WeChat/UserBind?"+ tag, false);
                postMsg.Description = String.Format("您还未绑定微信商城帐号，为了方便给您更好的服务，请您先绑定商城帐号，<a href=\"{0}\">点击绑定</a>！", returnUrl);
                return postMsg;
            }

            //获取指令关键字
            string reply = BLL.SysManage.ConfigSystem.GetValueByCache("WeChat_MShop_Reply");

            if (string.IsNullOrEmpty(reply))
            {
                postMsg.Description = "亲,好友扫码就成为您的盟友,盟友下单即获得奖金!";
            }
            else
            {
                postMsg.Description = BLL.SysManage.ConfigSystem.GetValueByCache("WeChat_MShop_Reply");
            }



            postMsg.Description= BLL.SysManage.ConfigSystem.GetValueByCache("WeChat_MShop_Reply");

            long enterpriseId = 0;

            //调用委托方法创建并发送二维码
            DeleMethod deleMethod = CreateQrCode;

            IAsyncResult re = deleMethod.BeginInvoke(context, userModel, msg, enterpriseId, null, null);
           // deleMethod.EndInvoke(re);
          
            return postMsg;
        }

        public static HttpContext context
        {
            get { return HttpContext.Current; }
            set { value = context; }
        }

        public delegate void DeleMethod(HttpContext context,YSWL.WeChat.Model.Core.User userModel, YSWL.WeChat.Model.Core.RequestMsg msg, long enterpriseId);

        public static void CreateQrCode(HttpContext context,YSWL.WeChat.Model.Core.User userModel, YSWL.WeChat.Model.Core.RequestMsg msg, long enterpriseId)
        {
            
            try
            {
                string token = YSWL.MALL.Web.Components.PostMsgHelper.GetToken();
                string imagePath = String.Format(userModel.Headimgurl, "_132"); //头像
                string nickName = userModel.NickName;
                string QRUrl = YSWL.WeChat.BLL.Core.Scene.GetUserQRImage(token, userModel);//远程url //场景二维码
                QRUrl = System.Web.HttpUtility.UrlDecode(QRUrl);
                string savaPath = "/Upload/QRcode/QR_" + userModel.UserId + ".jpg";
                YSWL.WeChat.Model.Core.CustomerMsg qrCodeImage = new YSWL.WeChat.Model.Core.CustomerMsg();
                qrCodeImage.OpenId = msg.OpenId;
                qrCodeImage.CreateTime = DateTime.Now;
                qrCodeImage.MsgType = "image";
                imagePath = context.Server.MapPath(imagePath);
                savaPath = context.Server.MapPath(savaPath);
                List<string> userList = new List<string>();
                userList.Add(msg.UserName);

                if (File.Exists(savaPath))
                {
                    try
                    {
                        qrCodeImage.MediaId = YSWL.WeChat.BLL.Core.Utils.GetMediaId(token, savaPath, "image");
                        System.Threading.Thread.Sleep(4000);
                        YSWL.WeChat.BLL.Core.CustomerMsg.SendCustomMsg(qrCodeImage, token, userList);
                    }
                    catch(Exception ex)
                    {
                        LogHelp.AddErrorLog(ex.Message, ex.StackTrace);
                    }
                }
                else
                {
                    try
                    {
                        try
                        {
                            FileHelper.GetQRCode(imagePath, QRUrl, savaPath,nickName);
                        }
                        catch (Exception ex)
                        {
                            LogHelp.AddErrorLog(ex.Message, ex.StackTrace);
                        }
                        try
                        {
                            qrCodeImage.MediaId = YSWL.WeChat.BLL.Core.Utils.GetMediaId(token, savaPath, "image");
                        }
                        catch (Exception ex)
                        {
                            LogHelp.AddErrorLog(ex.Message, ex.StackTrace);
                        }
                        try
                        {
                            System.Threading.Thread.Sleep(4000);
                            YSWL.WeChat.BLL.Core.CustomerMsg.SendCustomMsg(qrCodeImage, token, userList);
                        }
                        catch (Exception ex)
                        {
                            LogHelp.AddErrorLog(ex.Message, ex.StackTrace);
                        }
                        
                    }
                    catch(Exception ex)
                    {
                        LogHelp.AddErrorLog(ex.Message, ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelp.AddErrorLog(ex.Message, ex.StackTrace);
            }
        }
        #endregion
    }
}
