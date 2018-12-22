using System.Web;

namespace YSWL.TimerTask
{
    public class TaskHttpModule : IHttpModule
    {

        #region IHttpModule 成员

        public void Dispose()
        {
            Task.Instance().Dispose();
        }

        public void Init(HttpApplication context)
        {
            Task.Instance().Start();
        }

        #endregion
    }
}
