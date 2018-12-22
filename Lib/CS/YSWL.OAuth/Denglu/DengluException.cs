namespace YSWL.OAuth
{
    /// <summary>
    /// 灯鹭接口可能返回的错误类型对照表
    /// Code Description
    /// 1 	参数错误，请参考API文档
    /// 2 	站点不存在
    /// 3 	时间戳有误
    /// 4 	只支持md5签名
    /// 5 	签名不正确
    /// 6 	token已过期
    /// 7 	媒体用户不存在
    /// 8 	媒体用户已绑定其他用户
    /// 9 	媒体用户已解绑
    /// 10 	未知错误
    /// </summary>
    public class DengluException : System.Exception
    {
        private int errorCode;

        public int ErrorCode
        {
            get { return errorCode; }
            set { errorCode = value; }
        }

        private string errorDescription;

        public string ErrorDescription
        {
            get { return errorDescription; }
            set { errorDescription = value; }
        }

        public DengluException(int errorCode, string errorDescription)
        {
            this.errorCode = errorCode;
            this.errorDescription = errorDescription;
        }
    }

//end DengluException
}