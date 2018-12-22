using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YSWL.Email.EmailJob;

namespace YSWL.Email.EmailJob
{
    public class EmailJobSevice:IHttpModule
    {
        public void Dispose()
        {
            Jobs.Instance().Stop();
        }

        public void Init(HttpApplication context)
        {
            Jobs.Instance().Start();
        }
    }
}