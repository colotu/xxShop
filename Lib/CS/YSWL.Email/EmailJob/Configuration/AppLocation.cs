using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Xml;
using System.Text.RegularExpressions;
using System.Collections;

namespace YSWL.Email.EmailJob.Configuration
{

    public class AppLocation
    {
        private string defaultName;
        private Hashtable ht = new Hashtable();
        private const string HttpContextAppLocation = "AppLocation";
        private IList<string> keys = new List<string>();
        private string pattern;
        private Regex regex;

        internal void Add(MsApplication app)
        {
            if (this.ht.Contains(app.Name))
            {
                throw new Exception(string.Format(CultureInfo.InvariantCulture, "The Application.Name ({0}) was not unique", new object[] { app.Name }));
            }
            this.ht.Add(app.Name, app);
            this.keys.Add(app.Name);
        }

        //public static AppLocation Create(XmlNode node)
        //{
        //    return null;
        //}

        internal MsApplication CurrentMsApplication()
        {
            HttpContext current = HttpContext.Current;
            if (current == null)
            {
                return null;
            }
            MsApplication application = current.Items["AppLocation"] as MsApplication;
            if (application == null)
            {
                application = this.LookUp(current.Request.Path);
                if (application != null)
                {
                    current.Items.Add("AppLocation", application);
                }
            }
            return application;
        }

        public static AppLocation Default()
        {
            AppLocation location = new AppLocation();
            location.Add(new MsApplication("/", "All", ApplicationType.All));
            return location;
        }

        public bool IsName(string name)
        {
            return (string.Compare(name, this.CurrentName, true, CultureInfo.InvariantCulture) == 0);
        }

        internal MsApplication LookUp(string url)
        {
            if ((this.Pattern == null) || this.regex.IsMatch(url))
            {
                for (int i = 0; i < this.keys.Count; i++)
                {
                    MsApplication application = this.ht[this.keys[i]] as MsApplication;
                    if (application.IsMatch(url))
                    {
                        return application;
                    }
                }
                if (this.DefaultName != null)
                {
                    return (this.ht[this.DefaultName] as MsApplication);
                }
            }
            return null;
        }

        public ApplicationType CurrentApplicationType
        {
            get
            {
                MsApplication application = this.CurrentMsApplication();
                if (application != null)
                {
                    return application.ApplicationType;
                }
                return ApplicationType.Unknown;
            }
        }

        public string CurrentName
        {
            get
            {
                MsApplication application = this.CurrentMsApplication();
                if (application != null)
                {
                    return application.Name;
                }
                return null;
            }
        }

        public string DefaultName
        {
            get
            {
                return this.defaultName;
            }
            set
            {
                this.defaultName = value;
            }
        }

        public bool IsKnownApplication
        {
            get
            {
                return (this.CurrentApplicationType != ApplicationType.Unknown);
            }
        }

        public string Pattern
        {
            get
            {
                return this.pattern;
            }
            set
            {
                this.pattern = value;
                if (this.pattern != null)
                {
                    this.regex = new Regex(this.pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                }
            }
        }
    }
}