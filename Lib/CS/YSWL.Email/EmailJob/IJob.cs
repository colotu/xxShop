using System;
using System.Collections.Generic;
using System.Xml;

namespace YSWL.Email.EmailJob
{
    public interface IJob
    {
        void Execute(XmlNode node);
    }
}
