using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YSWL.MALL.Web.SMSService;

namespace YSWL.MALL.Web.Components
{
    public class SMSHelper
    {
        /// <summary>
        /// 发送短信接口
        /// </summary>
        /// <param name="content"></param>
        /// <param name="numbers"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public static bool SendSMS(string content, string[] numbers,int priority=5)
        {
            //获取短信接口的序列号，和自定义Key
            string SerialNo = BLL.SysManage.ConfigSystem.GetValueByCache("Emay_SMS_SerialNo");
            string Key = BLL.SysManage.ConfigSystem.GetValueByCache("Emay_SMS_Key");
            if (String.IsNullOrWhiteSpace(SerialNo) || String.IsNullOrWhiteSpace(Key))
            {
                LogHelp.AddErrorLog("亿美短信接口缺少企业序列号或者自定义关键字Key", "亿美短信接口调用失败",HttpContext.Current.Request);
                return false;
            }
            SMSService.SDKClient sdkClient = new SDKClientClient();
            sendSMSRequest smsRequest = new sendSMSRequest();
            smsRequest.arg0 = SerialNo;
            smsRequest.arg1 = Key;
            smsRequest.arg3 = numbers;
            smsRequest.arg4 = Common.Globals.HtmlEncode(content);
            smsRequest.arg7 = priority;
            sendSMSResponse smsResponse = sdkClient.sendSMS(smsRequest);
            if (smsResponse.@return == 0)
            {
                return true;
            }
            string msg= SendSMSException(smsResponse.@return);
            LogHelp.AddErrorLog("亿美短信接口发送短信出现异常，【" + msg + "】，错误码【"+ smsResponse .@return+ "】", "亿美短信接口调用失败", HttpContext.Current.Request);
            return false;
        }
        /// <summary>
        /// 	注册序列号
        /// </summary>
        /// <returns></returns>
        public static bool RegistEx()
        {
            string SerialNo = BLL.SysManage.ConfigSystem.GetValueByCache("Emay_SMS_SerialNo");
            string Key = BLL.SysManage.ConfigSystem.GetValueByCache("Emay_SMS_Key");
            string Pwd = BLL.SysManage.ConfigSystem.GetValueByCache("Emay_SMS_Pwd");
            if (String.IsNullOrWhiteSpace(SerialNo) || String.IsNullOrWhiteSpace(Key))
            {
                LogHelp.AddErrorLog("亿美短信接口缺少企业序列号或者自定义关键字Key", "亿美短信接口调用失败", HttpContext.Current.Request);
                return false;
            }
            SMSService.SDKClient sdkClient = new SDKClientClient();
            registExRequest request = new registExRequest(SerialNo, Key, Pwd);
            registExResponse response = sdkClient.registEx(request);
            if (response.@return == 0)
            {
                return true;
            }
            string msg = RegistExException(response.@return);
            LogHelp.AddErrorLog("亿美短信注册序列号出现异常，【" + msg + "】", "亿美短信接口调用失败", HttpContext.Current.Request);
            return false;

        }

         /// <summary>
        /// 	注册序列号
        /// </summary>
        /// <returns></returns>
        public static bool Logout()
        {
            string SerialNo = BLL.SysManage.ConfigSystem.GetValueByCache("Emay_SMS_SerialNo");
            string Key = BLL.SysManage.ConfigSystem.GetValueByCache("Emay_SMS_Key");
            string Pwd = BLL.SysManage.ConfigSystem.GetValueByCache("Emay_SMS_Pwd");
            if (String.IsNullOrWhiteSpace(SerialNo) || String.IsNullOrWhiteSpace(Key))
            {
                LogHelp.AddErrorLog("亿美短信接口缺少企业序列号或者自定义关键字Key", "亿美短信接口调用失败", HttpContext.Current.Request);
                return false;
            }
            SMSService.SDKClient sdkClient = new SDKClientClient();
            logoutRequest request = new logoutRequest(SerialNo, Key);
            logoutResponse response = sdkClient.logout(request);
            if (response.@return == 0)
            {
                return true;
            }
            string msg = LogoutException(response.@return);
            LogHelp.AddErrorLog("亿美短信注销序列号出现异常，【" + msg + "】", "亿美短信接口调用失败", HttpContext.Current.Request);
            return false;

        }


        /// <summary>
        /// 检验发送次数
        /// </summary>
        /// <param name="requestIP"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static bool CheckSendNum(string requestIP, string phoneNumber)
        {
            //验证方式
            string verifiedManner = BLL.SysManage.ConfigSystem.GetValueByCache("Emay_SMS_FrequentVerifiedManner");
            //文件
            if (!String.IsNullOrWhiteSpace(verifiedManner) && verifiedManner.ToLower() == "file")
            {
                string strText = ReadText();
                if(String.IsNullOrWhiteSpace(strText)){
                    return true;
                }
               string[] lines =strText.Split(new  string[] {"\r\n" },StringSplitOptions.RemoveEmptyEntries);
               if (lines ==null || lines.Length == 0)
               {
                   return true;
               }
               int phonecount = lines.Where(o => o.Contains(phoneNumber)).Count();
               if (phonecount >= 5) {
                   return false;
               }
               int ipcount = lines.Where(o => o.Contains(requestIP)).Count();
               if (ipcount > 20)
               {
                   return false;
               }  
            }//缓存
            else
            {
                //一个手机号码24小时内最多获取短信验证码5次
                string PhoneCacheKey = "YSWL.GetMsgCode_Phone" + phoneNumber;
                int count = YSWL.Common.Globals.SafeInt(YSWL.Common.DataCache.GetCache(PhoneCacheKey), 0);
                if (count >= 5)
                {
                    return false;
                }
                
                //一个IP24小时最多获取20次验证码
                string IPCacheKey = "YSWL.GetMsgCode_IP" + requestIP;
                count = YSWL.Common.Globals.SafeInt(YSWL.Common.DataCache.GetCache(IPCacheKey), 0);
                if (count >= 20) { return false; }
            }
            return true;
        }
       
        private static string ReadText()
        {
            string text = "";
            string path = YSWL.Common.FileManage.GetWebAssemblyPath();
            string dir = string.Format(@"{0}\log\", path);
            string  fileName = string.Format("{0}{1}_{2}.txt", dir, "SMS", DateTime.Now.ToString("yyyyMMdd"));     
            try
            {
                if (System.IO.File.Exists(fileName))
                {            
                    text = System.IO.File.ReadAllText(fileName);
                }
            }
            catch
            {
            }
            return text;
        }
    
        /// <summary>
        /// 增加发送次数
        /// </summary>
        /// <param name="requestIP"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static bool AddSendNum(string requestIP, string phoneNumber)
        {
            //验证方式
            string  verifiedManner = BLL.SysManage.ConfigSystem.GetValueByCache("Emay_SMS_FrequentVerifiedManner");
            //文件
            if (!String.IsNullOrWhiteSpace(verifiedManner)  &&  verifiedManner.ToLower() == "file")
            {
                System.Text.StringBuilder log = new System.Text.StringBuilder();
                log.AppendFormat("SMS|IP:{0}|Phone:{1}", requestIP, phoneNumber);
                YSWL.Common.FileManage.WriteText(log, "SMS");
            }
            else {//缓存
                int ModelCache = Common.Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                //手机号
                string PhoneCacheKey = "YSWL.GetMsgCode_Phone" + phoneNumber;
                int phonecount = YSWL.Common.Globals.SafeInt(YSWL.Common.DataCache.GetCache(PhoneCacheKey), 0) + 1;

                // Ip
                string IPCacheKey = "YSWL.GetMsgCode_IP" + requestIP;
                int ipcount = YSWL.Common.Globals.SafeInt(YSWL.Common.DataCache.GetCache(IPCacheKey), 0) + 1;
                try
                {
                    YSWL.Common.DataCache.SetCache(PhoneCacheKey, phonecount, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    YSWL.Common.DataCache.SetCache(IPCacheKey, ipcount, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                }
                catch (Exception ex)
                {
                    YSWL.Common.FileManage.WriteText(new System.Text.StringBuilder(ex.Message), "SMS");
                }
            }
            return true;
        }


        /// <summary>
        /// 查询单价
        /// </summary>
        public double GetEachFee()
        {
            string SerialNo = BLL.SysManage.ConfigSystem.GetValueByCache("Emay_SMS_SerialNo");
            string Key = BLL.SysManage.ConfigSystem.GetValueByCache("Emay_SMS_Key");
            if (String.IsNullOrWhiteSpace(SerialNo) || String.IsNullOrWhiteSpace(Key))
            {
                LogHelp.AddErrorLog("亿美短信接口缺少企业序列号或者自定义关键字Key", "亿美短信接口调用失败", HttpContext.Current.Request);
                return 0;
            }
            try
            {
                SMSService.SDKClient sdkClient = new SDKClientClient();
                getEachFeeRequest request = new getEachFeeRequest(SerialNo, Key);
                getEachFeeResponse response = sdkClient.getEachFee(request);
                return response.@return;
            }
            catch (Exception ex)
            {
                LogHelp.AddErrorLog("查询短信单价异常："+ex.Message, ex.StackTrace,HttpContext.Current.Request);
                throw;
            }
           
        }


        /// <summary>
        /// 查询余额
        /// </summary>
        public static double GetBalance()
        {
            string SerialNo = BLL.SysManage.ConfigSystem.GetValueByCache("Emay_SMS_SerialNo");
            string Key = BLL.SysManage.ConfigSystem.GetValueByCache("Emay_SMS_Key");
            if (String.IsNullOrWhiteSpace(SerialNo) || String.IsNullOrWhiteSpace(Key))
            {
                LogHelp.AddErrorLog("亿美短信接口缺少企业序列号或者自定义关键字Key", "亿美短信接口调用失败", HttpContext.Current.Request);
                return 0;
            }
            try
            {
                SMSService.SDKClient sdkClient = new SDKClientClient();
                getBalanceRequest request = new getBalanceRequest(SerialNo, Key);
                getBalanceResponse response = sdkClient.getBalance(request);
                return response.@return;
            }
            catch (Exception ex)
            {
                LogHelp.AddErrorLog("查询短信余额异常：" + ex.Message, ex.StackTrace, HttpContext.Current.Request);
                throw;
            }

        }

        /// <summary>  
        /// 发送短信异常信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private static string SendSMSException(int code)
        {
            string msg = "";
            switch (code)
            {
                case 305:
                    msg = "服务器端返回错误，错误的返回值";
                    break;
                case 101 :
                case 103:
                    msg = "客户端网络故障";
                    break;
                case 307:
                    msg = "目标电话号码不符合规则，电话号码必须是以0、1开头";
                    break;
                case 997:
                    msg = "平台返回找不到超时的短信，该信息是否成功无法确定";
                    break;
                case 998:
                    msg = "由于客户端网络问题导致信息发送超时，该信息是否成功下发无法确定";
                    break;

                case -1:
                    msg = "系统异常";
                    break;
                case -2:
                    msg = "客户端异常";
                    break;
                case -101:
                    msg = "命令不被支持";
                    break;
                case -104:
                    msg = "请求超过限制";
                    break;
                case -117:
                    msg = "发送短信失败";
                    break;

                case -1104:
                    msg = "路由失败，请联系系统管理员";
                    break;
                case -9016:
                    msg = "发送短信包大小超出范围";
                    break;
                case -9017:
                    msg = "发送短信内容格式错误";
                    break;
                case -9018:
                    msg = "发送短信扩展号格式错误";
                    break;
                case -9019:
                    msg = "发送短信优先级格式错误";
                    break;
                case -9020:
                    msg = "发送短信手机号格式错误";
                    break;
                case -9021:
                    msg = "发送短信定时时间格式错误";
                    break;
                case -9022:
                    msg = "发送短信唯一序列值错误";
                    break;

                case -9001:
                    msg = "序列号格式错误";
                    break;
                case -9002:
                    msg = "密码格式错误";
                    break;
                case -9003:
                    msg = "客户端Key格式错误";
                    break;
                case -9025:
                    msg = "客户端请求sdk5超时";
                    break;
                default:
                    msg = "未知错误";
                    break;

            }
            return msg;
        }
        /// <summary>
        ///注册序列号异常信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private static string RegistExException(int code)
        {
            string msg = "";
            switch (code)
            {
                case 305:
                    msg = "服务器端返回错误，错误的返回值";
                    break;
                case 101:
                case 103:
                    msg = "客户端网络故障";
                    break;
                case 999:
                    msg = "操作频繁";
                    break;

                case -1:
                    msg = "系统异常";
                    break;
                case -2:
                    msg = "客户端异常";
                    break;
                case -101:
                    msg = "命令不被支持";
                    break;
                case -104:
                    msg = "请求超过限制";
                    break;
                case -110:
                    msg = "号码注册激活失败";
                    break;

                case -126:
                    msg = "路由信息失败";
                    break;
                case -190:
                    msg = "数据操作失败";
                    break;
                case -1100:
                    msg = "序列号错误，序列号不存在内存中，或尝试攻击的用户";
                    break;
                case -1103:
                    msg = "序列号Key错误";
                    break;
                case -1102:
                    msg = "序列号密码错误";
                    break;
                case -1104:
                    msg = "路由失败，请联系系统管理员";
                    break;
                case -1105:
                    msg = "注册号状态异常, 未用 1";
                    break;
                case -1107:
                    msg = "注册号状态异常, 停用 3";
                    break;

                case -1108:
                    msg = "注册号状态异常, 停止 5";
                    break;
                case -1901:
                    msg = "数据库插入操作失败";
                    break;

                case -9001:
                    msg = "序列号格式错误";
                    break;
                case -9002:
                    msg = "密码格式错误";
                    break;
                case -9003:
                    msg = "客户端Key格式错误";
                    break;
                case -9025:
                    msg = "客户端请求sdk5超时";
                    break;
                default:
                    msg = "未知错误";
                    break;

            }
            return msg;
        }
        /// <summary>
        /// 注销序列号异常信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private static string LogoutException(int code)
        {
            string msg = "";
            switch (code)
            {
                case 305:
                    msg = "服务器端返回错误，错误的返回值";
                    break;
                case 101:
                case 103:
                    msg = "客户端网络故障";
                    break;
                case 999:
                    msg = "操作频繁";
                    break;

                case -1:
                    msg = "系统异常";
                    break;
                case -2:
                    msg = "客户端异常";
                    break;
                case -101:
                    msg = "命令不被支持";
                    break;
                case -104:
                    msg = "请求超过限制";
                    break;
                case -122:
                    msg = "号码注销激活失败";
                    break;

                case -126:
                    msg = "路由信息失败";
                    break;
                case -190:
                    msg = "数据操作失败";
                    break;
                case -1100:
                    msg = "序列号错误，序列号不存在内存中，或尝试攻击的用户";
                    break;
                case -1103:
                    msg = "序列号Key错误";
                    break;
                case -1102:
                    msg = "序列号密码错误";
                    break;
                case -1104:
                    msg = "路由失败，请联系系统管理员";
                    break;
        
                case -1902:
                    msg = "数据库更新操作失败";
                    break;

                case -9001:
                    msg = "序列号格式错误";
                    break;
                case -9002:
                    msg = "密码格式错误";
                    break;
                case -9003:
                    msg = "客户端Key格式错误";
                    break;
                case -9025:
                    msg = "客户端请求sdk5超时";
                    break;
                default:
                    msg = "未知错误";
                    break;

            }
            return msg;
        }

 
    }
}