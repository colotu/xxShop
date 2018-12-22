using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Text.RegularExpressions;

namespace YSWL.Email.EmailJob.Configuration
{
    internal class MsApplication
    {
        private ApplicationType _appType = ApplicationType.All;
        private string _name;
        private Regex _regex;

        internal MsApplication(string pattern, string name, ApplicationType appType)
        {
            this._name = name.ToLower(CultureInfo.InvariantCulture);
            this._appType = appType;
            this._regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public bool IsMatch(string url)
        {
            return this._regex.IsMatch(url);
        }

        public ApplicationType ApplicationType
        {
            get
            {
                return this._appType;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
        }
    }
}