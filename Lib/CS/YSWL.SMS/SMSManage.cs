using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Xml;

namespace YSWL.SMS
{
    public static class SMSManage
    {
        private static readonly string SN;
        private static readonly string PASSWORD;

        static SMSManage()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(GetConfigPath());
                SN = doc.DocumentElement.SelectSingleNode("SN").InnerText;
                PASSWORD = doc.DocumentElement.SelectSingleNode("Password").InnerText;
            }
            catch (Exception)
            {
                throw new NullReferenceException("THE YSWL.SMS KEY-[SN] IS NOT NULL! PLEASE CHECK THE SMSSETTING.CONFIG!");
            }

            if (string.IsNullOrEmpty(SN))
            {
                throw new NullReferenceException("THE YSWL.SMS KEY-[SN] IS NOT NULL! PLEASE CHECK THE SMSSETTING.CONFIG!");
            }
        }

        private static string GetConfigPath()
        {
            string configPath = ConfigurationManager.AppSettings["SmsConfigPath"];
            if (string.IsNullOrEmpty(configPath) || configPath.Trim().Length == 0)
            {
                configPath = HttpContext.Current.Request.MapPath("/Config/SmsSetting.config");
            }
            else
            {
                if (!Path.IsPathRooted(configPath))
                    configPath = HttpContext.Current.Request.MapPath(Path.Combine(configPath, "SmsSetting.config"));
                else
                    configPath = Path.Combine(configPath, "SmsSetting.config");
            }
            return configPath;
        }

        /*
         * 注册企业信息Register（软件序列号,密码（6位）,企业名称(最多60字节)，联系人姓名(最多20字节),
         * 联系电话(最多20字节)，联系手机(最多15字节)，电子邮件(最多60字节)，联系传真(最多20字节)，
         * 公司地址(最多60字节)，邮政编码(最多6字节)）
         * 
         * 参数说明：
         * 注册需要的序列号,请通过亿美销售人员获取
         * 注册需要的密码,请通过亿美销售人员获取
         * 
         * 在注册序列号时，需注意不需要每次发送短信时都注册一遍，  一个序列号在一台机器上只需要注册一次就够了
         * 即便是关闭应用程序或重启电脑也不需要重新注册，除非重装电脑或注销序列号之后才需要重新注册。
         */
        public static string Register()
        {
            int result = SMSDllImport.Register(SN, PASSWORD, "1", "1", "1", "1", "1", "1", "1", "1");
            if (result == 1)
                return "注册成功";
            else if (result == 101)
                return "网络故障";
            else if (result == 102)
                return "其它故障";
            else if (result == 0)
                return "失败";
            else if (result == 100)
                return "序列号码为空或无效";
            else if (result == 103)
                return "注册企业基本信息失败，当软件注册号码注册成功,但整体还是失败，要重新注册";
            else if (result == 104)
                return "注册信息填写不完整";
            else if (result == 114)
                return "得到标识错误";
            else
            {
                return "其他故障值：" + result.ToString();
            }
        }


        /*
         * 充值序列号ChargeUp(软件序列号,充值卡卡号,充值卡密码)
         * 
         * 参数说明：
         * 软件序列号为注册时使用的序列号
         * 充值卡卡号请通过亿美销售人员获取
         * 充值卡密码请通过亿美销售人员获取
         */
        public static string ChargeUp(string cardNO, string cardPwd)
        {
            //充值               注册号          卡号         密码
            int result = SMSDllImport.ChargeUp(SN, cardNO, cardPwd);

            if (result == 1)
                return "充值成功";
            else if (result == 101)
                return "网络故障";
            else if (result == 102)
                return "其它故障";
            else if (result == 100)
                return "序列号码为空或无效";
            else if (result == 106)
                return "卡号或密码为空或无效";
            else if (result == 0)
                return "失败";
            else
                return "其他故障值：" + result.ToString();
        }


        /*
         * 查询余额GetBalance(软件序列号,返回的余额)
         * 
         * 参数说明：
         * 软件序列号为注册时使用的序列号
         * 返回的余额(10个字节)使用时必须先分配内存
         */
        public static string GetBalance()
        {
            System.Text.StringBuilder balance = new System.Text.StringBuilder(0, 20);
            //得到余额            注册号         余额
            int result = SMSDllImport.GetBalance(SN, balance);
            string mybalance = balance.ToString(0, balance.Length - 1);
            if (result == 1)
                return mybalance + " 元";
            else if (result == 101)
                return "网络故障";
            else if (result == 102)
                return "其它故障";
            else if (result == 100)
                return "序列号码为空或无效";
            else if (result == 105)
                return "参数balance指针为空";
            else if (result == 0)
                return "失败:";
            else
                return "其他故障值:" + result.ToString();

        }

        /*
         * 查询单价GetPrice(软件序列号,单价金额)
         * 
         * 参数说明：
         * 软件序列号为注册时使用的序列号
         * 单价金额(5个字节)调用时必须创建
         */
        public static string GetPrice()
        {
            System.Text.StringBuilder myprice = new System.Text.StringBuilder(10);
            //得到单价           注册号         单价
            int result = SMSDllImport.GetPrice(SN, myprice);
            if (result == 1)
                return myprice.ToString() + "元:1";
            else if (result == 0)
                return "失败";
            else if (result == 101)
                return "网络故障";
            else if (result == 102)
                return "其它故障";
            else if (result == 100)
                return "序列号码为空或无效";
            else if (result == 105)
                return "参数balance指针为空";
            else
                return "其他故障值:" + result.ToString();
        }



        //注销序列号UnRegister(注册序列号) 
        /*软件的序列号只能在序列号注册的机器上注销，并且只需要注销一次
         * 
         * 参数说明：
         * 软件序列号为注册时使用的序列号
         */
        public static string UnRegister()
        {
            //注销序列号            注册号
            int result = SMSDllImport.UnRegister(SN);
            if (result == 1)
                return "注销成功";
            else if (result == 101)
                return "网络故障";
            else if (result == 102)
                return "其它故障";
            else if (result == 0)
                return "失败";
            else if (result == 100)
                return "序列号码为空或无效";
            else
                return "其他故障值：" + result.ToString();
        }


        /*设置代理服务器信息，要求代理服务器必须支持SOCK5协议
         设置代理SetProxy(代理服务器IP,代理服务器端口,用户名,密码)
         * 
         * 参数说明：
         * 代理服务器IP
         * 端口号
         * 用户名
         * 密码
         */
        public static string SetProxy(string ip, string port, string name, string pwd)
        {
            //设置代理    这里是 代理服务器IP    端口号        用户名         密码
            int result = SMSDllImport.SetProxy(ip, port, name, pwd);
            if (result == 1)
                return "设置成功";
            else
                return "设置失败" + result.ToString();
        }


        /*
         * 发送即时短信 SendSMS(这里是软件序列号, 手机号码,短信内容, 优先级)
         * 
         * 参数说明：
         * 软件序列号即注册序列号
         * 手机号码(最多一次发送200个手机号码,号码间用逗号分隔，逗号必须是半角状态的)
         * 短信内容(最多500个汉字或1000个纯英文，emay服务器程序能够自动分割；
         *  亿美有多个通道为客户提供服务，所以分割原则采用最短字数的通道为分割短信长度的规则，
         *  请客户应用程序不要自己分割短信以免造成混乱).亿美推荐短信长度70字以内 [扩展号]默认必须为空
         * 优先级代表优先级，范围1~5，数值越高优先级越高，当亿美通道的短信量特别大的时候，
         * 短信会在通道队列上排队，如果优先级越高，提交短信的速度会越快。
         */
        public static string SendSMS(string phone, string message, string priority)
        {
            //即时发送      这里是软件序列号    手机号       短信内容     优先级
            int result = SMSDllImport.SendSMS(SN, phone, message, priority);
            if (result == 1)
                return "发送成功";
            else if (result == 101)
                return "网络故障";
            else if (result == 102)
                return "其它故障";
            else if (result == 0)
                return "失败";
            else if (result == 100)
                return "序列号码为空或无效";
            else if (result == 107)
                return "手机号码为空或者超过1000个";
            else if (result == 108)
                return "手机号码分割符号不正确";
            else if (result == 109)
                return "部分手机号码不正确，已删除，其余手机号码被发送";
            else if (result == 110)
                return "短信内容为空或超长（70个汉字）";
            else if (result == 201)
                return "计费失败，请充值";
            else
                return "其他故障值：" + result.ToString();
        }
        public static string SendSMS(string phone, string message)
        {
            return SendSMS(phone, message, "5");
        }


        /*即时短信(带扩展号)SendSMSEx(软件序列号,手机号码,短信内容,附加号,优先级)
         * 
         * 参数说明：
         * 软件序列号即注册序列号
         * 手机号码(最多一次发送200个手机号码,号码间用逗号分隔，逗号必须是半角状态的)
         * 短信内容(最多500个汉字或1000个纯英文，emay服务器程序能够自动分割；
         *  亿美有多个通道为客户提供服务，所以分割原则采用最短字数的通道为分割短信长度的规则，
         *  请客户应用程序不要自己分割短信以免造成混乱).亿美推荐短信长度70字以内 [扩展号]默认必须为空
         * 附加号即扩展号，附加号有用户设置。
         * 优先级代表优先级，范围1~5，数值越高优先级越高，当亿美通道的短信量特别大的时候，
         * 短信会在通道队列上排队，如果优先级越高，提交短信的速度会越快。
         */
        public static string SendSMSEx(string phone, string message, string extensionNum, string priority)
        {
            //即时发送(扩展)   这里是软件序列号  手机号       短信内容      附加号        优先级
            int result = SMSDllImport.SendSMSEx(SN, phone, message, extensionNum, priority);
            if (result == 1)
                return "发送成功";
            else if (result == 101)
                return "网络故障";
            else if (result == 102)
                return "其它故障";
            else if (result == 0)
                return "失败";
            else if (result == 100)
                return "序列号码为空或无效";
            else if (result == 107)
                return "手机号码为空或者超过1000个";
            else if (result == 108)
                return "手机号码分割符号不正确";
            else if (result == 109)
                return "部分手机号码不正确，已删除，其余手机号码发送成功";
            else if (result == 110)
                return "短信内容为空或超长（70个汉字）";
            else if (result == 111)
                return "附加号码过长（8位）";
            else if (result == 201)
                return "计费失败，请充值";
            else
                return "其他故障值：" + result.ToString();
        }
        public static string SendSMSEx(string phone, string message, string extensionNum)
        {
            return SendSMSEx(phone, message, extensionNum, "5");
        }


        //定时发送
        /*
         * 定时发送SendScheSMS(软件序列号,手机号,短信内容,发送时间,优先级)
         * 
         * 参数具体说明：
         * 软件序列号即为注册序列号
         * 手机号码(最多一次发送200个手机号码,号码间用逗号分隔，逗号必须是半角状态的)
         * 短信内容(最多500个汉字或1000个纯英文，emay服务器程序能够自动分割；
         *  亿美有多个通道为客户提供服务，所以分割原则采用最短字数的通道为分割短信长度的规则，
         *  请客户应用程序不要自己分割短信以免造成混乱).亿美推荐短信长度70字以内 [扩展号]默认必须为空。
         * 发送时间：预定发送时间(格式为：年年年年月月日日时时分分秒秒)
         *  例如：20090817153041，代表2009年8月17日15点30分41秒。
         * 优先级代表优先级，范围1~5，数值越高优先级越高，当亿美通道的短信量特别大的时候，
         *  短信会在通道队列上排队，如果优先级越高，提交短信的速度会越快。
         *  
         * 在定时发送短信时需注意：发送时间必须大于当前时间，并且小于半年时间。定时发送失败。
         */
        public static string SendScheSMS(string phone, string message, string sendTime, string priority)
        {
            //定时发送        这里是软件序列号      手机号      短信内容　     发送时间     优先级
            int result = SMSDllImport.SendScheSMS(SN, phone, message, sendTime, priority);
            if (result == 1)
                return "发送成功";
            else if (result == 101)
                return "网络故障";
            else if (result == 102)
                return "其它故障";
            else if (result == 0)
                return "失败";
            else if (result == 100)
                return "序列号码为空或无效";
            else if (result == 107)
                return "手机号码为空或者超过1000个";
            else if (result == 108)
                return "手机号码分割符号不正确";
            else if (result == 109)
                return "部分手机号码不正确，已删除，其余手机号码发送成功";
            else if (result == 110)
                return "短信内容为空或超长（70个汉字）";
            else if (result == 112)
                return "定时时间为空或格式不正确";
            else if (result == 201)
                return "计费失败，请充值";
            else
                return "其他故障值：" + result.ToString();
        }


        /*定时发送SendScheSMSEx（软件序列号，手机号码，短信内容，发送时间， 附加号，优先级）
         * 具体参数说明：
         * 软件序列号即为注册序列号
         * 手机号码(最多一次发送200个手机号码,号码间用逗号分隔，逗号必须是半角状态的)
         * 短信内容(最多500个汉字或1000个纯英文，emay服务器程序能够自动分割；
         *  亿美有多个通道为客户提供服务，所以分割原则采用最短字数的通道为分割短信长度的规则，
         *  请客户应用程序不要自己分割短信以免造成混乱).亿美推荐短信长度70字以内 [扩展号]默认必须为空。
         * 发送时间：预定发送时间(格式为：年年年年月月日日时时分分秒秒)
         *  例如：20090817153041，代表2009年8月17日15点30分41秒。
         * 附加号即扩展号，附加号有用户设置。
         * 优先级代表优先级，范围1~5，数值越高优先级越高，当亿美通道的短信量特别大的时候，
         *  短信会在通道队列上排队，如果优先级越高，提交短信的速度会越快。
         *  
         * 
         *在定时发送短信时需注意：发送时间必须大于当前时间，并且小于半年时间。定时发送失败。
         */
        public static string SendScheSMSEx(string phone, string message, string sendTime, string extensionNum, string priority)
        {
            //定时发送(扩展)      这里是软件序列号      手机号      短信内容　     发送时间        附加号   优先级
            int result = SMSDllImport.SendScheSMSEx(SN, phone, message, sendTime, extensionNum, priority);
            if (result == 1)
                return "发送成功";
            else if (result == 101)
                return "网络故障";
            else if (result == 102)
                return "其它故障";
            else if (result == 0)
                return "失败";
            else if (result == 100)
                return "序列号码为空或无效";
            else if (result == 107)
                return "手机号码为空或者超过1000个";
            else if (result == 108)
                return "手机号码分割符号不正确";
            else if (result == 109)
                return "部分手机号码不正确，已删除，其余手机号码发送成功";
            else if (result == 110)
                return "短信内容为空或超长（70个汉字）";
            else if (result == 111)
                return "附加号码过长（8位）";
            else if (result == 112)
                return "定时时间为空或格式不正确";
            else if (result == 201)
                return "计费失败";
            else
                return "其他故障值：" + result.ToString();
        }
        /*注册转接RegistryTransfer(软件序列号,手机号)
         *
         * 参数说明：
         *  软件序列号即为注册序列号
         *  手机号(号码间用逗号分割，最多10个手机号码)
         *  
         * 注册转接功能，最多可以注册10个手机号码或上行服务代码
         * 手机用户回复的短信可转发到 注册的手机号上
         */
        public static string RegistryTransfer(string phone)
        {
            //申请转接服务         这里是软件序列号  手机号
            int result = SMSDllImport.RegistryTransfer(SN, phone);
            if (result == 1)
                return "申请成功";
            else if (result == 101)
                return "网络故障";
            else if (result == 102)
                return "其它故障";
            else if (result == 0)
                return "失败";
            else if (result == 100)
                return "序列号码为空或无效";
            else if (result == 107)
                return "手机号码为空或分割无效";
            else if (result == 108)
                return "手机号码分割符号不正确";
            else
                return "其他故障值：" + result.ToString();
        }


        /*注销转接功能CancelTransfer(软件序列号)
         * 
         * 参数说明：
         * 软件序列号即为注册序列号
         */
        public static string CancelTransfer()
        {
            //注销转接服务         这里是软件序列号
            int result = SMSDllImport.CancelTransfer(SN);
            if (result == 1)
                return "注销成功";
            else if (result == 101)
                return "网络故障";
            else if (result == 102)
                return "其它故障";
            else if (result == 0)
                return "失败";
            else if (result == 100)
                return "序列号码为空或无效";
            else
                return "其他故障值：" + result.ToString();
        }


        //接受短信：从EUCP平台接收手机用户上行的短信，以指针函数的形式返回结果集。
        /*接收短信ReceiveSMS(软件序列号,回调函数指针)
         * 
         * 参数说明：
         * 软件序列号即为注册序列号
         * 回调函数指针(接收到的短信通过回调函数返回)
         */
        public static string ReceiveSMS()
        {

            SMSDllImport.deleSQF mySmsContent = new SMSDllImport.deleSQF(SMSDllImport.getSMSContent);
            //接收短信                序列号       函数指针
            int result = 2;
            while (result == 2)  //当result = SMSDllImport.2 时，说明还有下一批短信等待接收，这时需重新再调用一次ReceiveSMS方法
            {
                result = SMSDllImport.ReceiveSMS(SN, mySmsContent);
                if (result == 1)
                    return "接收短信成功";
                else if (result == 101)
                    return "网络故障";
                else if (result == 102)
                    return "其它故障";
                else if (result == 105)
                    return "参数指针为空";
                else if (result == 0)
                    return "失败";
                else
                    return "其他故障值：" + result.ToString();
            }
            return string.Empty;
        }


        /*接收短信(扩展)ReceiveSMSEx(软件序列号，回调函数指针)
         * 
         * 参数说明：
         * 软件序列号即为注册序列号
         * 回调函数指针(接收到的短信通过回调函数返回)
         */
        public static string ReceiveSMSEx()
        {
            SMSDllImport.deleSQF mySmsContent = new SMSDllImport.deleSQF(SMSDllImport.getSMSContent);
            //接收短信(扩展)             序列号     函数指针
            int result = 2;
            while (result == 2) //当result = SMSDllImport.2 时，说明还有下一批短信等待接收，这时需重新再调用一次ReceiveSMSEx方法
            {
                result = SMSDllImport.ReceiveSMSEx(SN, mySmsContent);
                if (result == 1)
                    return "接收短信成功";
                else if (result == 101)
                    return "网络故障";
                else if (result == 102)
                    return "其它故障";
                else if (result == 105)
                    return "参数指针为空";
                else if (result == 0)
                    return "失败";
                else
                    return "其他故障值：" + result.ToString();
            }
            return string.Empty;
        }



        /*修改序列号的密码RegistryPwdUpd(软件序列号,原始密码,新密码)
         * 
         * 参数说明：
         * 软件序列号即为注册序列号
         * 原始密码用户设置
         * 新密码用户输入
         */
        internal static string RegistryPwdUpdata(string sn, string oldPwd, string newPwd)
        {
            //修改序列号密码         这里是软件序列号  
            int result = SMSDllImport.RegistryPwdUpd(sn, oldPwd, newPwd);
            if (result == 1)
                return "修改成功";
            else if (result == 0)
                return "失败";
            else if (result == 101)
                return "网络故障";
            else if (result == 102)
                return "其它故障";
            else if (result == 0)
                return "失败";
            else if (result == 100)
                return "序列号码为空或无效";
            else if (result == 113)
                return "旧密码或新密码为空";
            else
                return "其他故障值：" + result.ToString();
        }
    }
}
