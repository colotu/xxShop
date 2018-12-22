using System.Runtime.InteropServices;

namespace YSWL.SMS
{
    internal class SMSDllImport
    {
        //调用dll方法
        [DllImport("EUCPComm.dll", EntryPoint = "SendSMS")] //即时发送
        internal static extern int SendSMS(string sn, string mn, string ct, string priority);

        [DllImport("EUCPComm.dll", EntryPoint = "SendSMSEx")] //即时发送(扩展)
        internal static extern int SendSMSEx(string sn, string mn, string ct, string addi, string priority);

        [DllImport("EUCPComm.dll", EntryPoint = "SendScheSMS")] // 定时发送
        internal static extern int SendScheSMS(string sn, string mn, string ct, string ti, string priority);

        [DllImport("EUCPComm.dll", EntryPoint = "SendScheSMSEx")] // 定时发送(扩展)
        internal static extern int SendScheSMSEx(string sn, string mn, string ct, string ti, string addi,
                                                 string priority);

        [DllImport("EUCPComm.dll", EntryPoint = "ReceiveSMS")] // 接收短信
        internal static extern int ReceiveSMS(string sn, deleSQF mySmsContent);

        [DllImport("EUCPComm.dll", EntryPoint = "ReceiveSMSEx")] // 接收短信
        internal static extern int ReceiveSMSEx(string sn, deleSQF mySmsContent);

        //[DllImport("EUCPComm.dll",EntryPoint="ReceiveStatusReport")]  // 接收短信报告
        //internal static extern int ReceiveStatusReport(string sn,delegSMSReport mySmsReport);  

        //[DllImport("EUCPComm.dll",EntryPoint="ReceiveStatusReportEx")]  // 接收短信报告(带批量ID)
        //internal static extern int ReceiveStatusReportEx(string sn,delegSMSReportEx mySmsReportEx);  

        [DllImport("EUCPComm.dll", EntryPoint = "Register")] // 注册 
        internal static extern int Register(string sn, string pwd, string EntName, string LinkMan, string Phone,
                                            string Mobile, string Email, string Fax, string sAddress, string Postcode);

        [DllImport("EUCPComm.dll", EntryPoint = "GetBalance", CallingConvention = CallingConvention.Winapi)] // 余额 
        internal static extern int GetBalance(string m, System.Text.StringBuilder balance);

        [DllImport("EUCPComm.dll", EntryPoint = "ChargeUp")] // 存值
        internal static extern int ChargeUp(string sn, string acco, string pass);

        [DllImport("EUCPComm.dll", EntryPoint = "GetPrice")] // 价格
        internal static extern int GetPrice(string m, System.Text.StringBuilder balance);

        [DllImport("EUCPComm.dll", EntryPoint = "RegistryTransfer")] //申请转接
        internal static extern int RegistryTransfer(string sn, string mn);

        [DllImport("EUCPComm.dll", EntryPoint = "CancelTransfer")] // 注销转接
        internal static extern int CancelTransfer(string sn);

        [DllImport("EUCPComm.dll", EntryPoint = "UnRegister")] // 注销
        internal static extern int UnRegister(string sn);

        [DllImport("EUCPComm.dll", EntryPoint = "SetProxy")] // 设置代理服务器 
        internal static extern int SetProxy(string IP, string Port, string UserName, string PWD);

        [DllImport("EUCPComm.dll", EntryPoint = "RegistryPwdUpd")] // 修改序列号密码
        internal static extern int RegistryPwdUpd(string sn, string oldPWD, string newPWD);

        //回调(接收短信)
        /*回调函数参数说明(收到上行短信的各参数值)
            mobile：手机号码（当falg=1时有内容）
            senderaddi：发送者附加号码（当falg=1时有内容），无此项
            recvaddi：接收者附加号码（当falg=1时有内容），无此项
            ct：短信内容（当falg=1时有内容）
            sd：接收时间（当falg=1时有内容，格式：yyyymmddhhnnss）
            flag：1表示有短信，0表示无短信（不用在处理信息了）
         */
        internal static string getSMSContent(string mobile, string senderaddi, string recvaddi, string ct, string sd, ref int flag)
        {
            string mob = mobile;
            string content = ct;
            int myflag = flag;
            return (mob + "----" + content);
        }

        //声明委托，对回调函数进行封装。
        internal delegate string deleSQF(string mobile, string senderaddi, string recvaddi, string ct, string sd, ref int flag);
        internal deleSQF mySmsContent = new deleSQF(getSMSContent);

        //回调(接收状态报告)
        internal static string getSMSReport(string mobile, string errorCode, string serviceCodeAdd, string reportType, ref int flag)
        {
            //未完善
            string mob = mobile;
            int myflag = flag;
            return (mob + "----" + myflag);
        }
        internal delegate string delegSMSReport(string mobile, string errorCode, string serviceCodeAdd, string reportType, ref int flag);
        internal delegSMSReport mySmsReport = new delegSMSReport(getSMSReport);

        //回调(接收状态报告)带批量ID
        internal static string getSMSReportEx(ref long seq, string mobile, string errorCode, string serviceCodeAdd, string reportType, ref int flag)
        {
            //未完善
            string mob = mobile;
            int myflag = flag;
            return (mob + "----" + myflag);
        }
        internal delegate string delegSMSReportEx(ref long seq, string mobile, string errorCode, string serviceCodeAdd, string reportType, ref int flag);
        internal delegSMSReportEx mySmsReportEx = new delegSMSReportEx(getSMSReportEx);
    }
}
