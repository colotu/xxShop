using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Xml;
using System.IO;
using System.Web.Caching;
using System.Globalization;

namespace YSWL.Email.EmailJob.Configuration
{
    public class MsConfiguration
    {
        private string[] _defaultRoles = new string[] { "Everyone", "Registered Users" };
        private Dictionary<string, string> _integratedApplications = new Dictionary<string, string>();
        //private SSLSettings _ssl;
        private Dictionary<string, string> _supportedLanguages = new Dictionary<string, string>();
        //private AppLocation app;
        public const string CacheKey = "YSWL_Configuration";
        private string filesPath = "/";
        private string passwordEncodingFormat = "Unicode";
        private Hashtable providers = new Hashtable();
        private short smtpServerConnectionLimit = -1;
        private int threadCount = 2;
        private XmlDocument XmlDoc;

        public MsConfiguration(XmlDocument doc)
        {
            this.XmlDoc = doc;
            this.LoadValuesFromConfigurationXml();
        }

        //internal void GetAppLocation(XmlNode node)
        //{
        //    this.app = AppLocation.Create(node);
        //}

        public static MsConfiguration GetConfig()
        {
            MsConfiguration configuration = DataCache.Get("YSWL_Configuration") as MsConfiguration;
            if (configuration == null)
            {
                string filename = null;
                HttpContext current = HttpContext.Current;
                if (current != null)
                {
                    filename = current.Request.MapPath("~/YSWL.config");
                }
                else
                {
                    filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "YSWL.config");
                }
                XmlDocument doc = new XmlDocument();
                doc.Load(filename);
                configuration = new MsConfiguration(doc);
                DataCache.Max("YSWL_Configuration", configuration, new CacheDependency(filename));
                DataCache.ReSetFactor(configuration.CacheFactor);
            }
            return configuration;
        }

        public XmlNode GetConfigSection(string nodePath)
        {
            return this.XmlDoc.SelectSingleNode(nodePath);
        }

        internal void GetIntegratedApplications(XmlNode node)
        {
            foreach (XmlNode node2 in node.ChildNodes)
            {
                if (!this._integratedApplications.ContainsKey(node2.Attributes["applicationName"].Value))
                {
                    this._integratedApplications.Add(node2.Attributes["applicationName"].Value, node2.Attributes["implement"].Value);
                }
            }
        }

        internal void GetLanguages(XmlNode node)
        {
            foreach (XmlNode node2 in node.ChildNodes)
            {
                if ((string.Compare(node2.Attributes["enabled"].Value, "true", false, CultureInfo.InvariantCulture) == 0) && !this._supportedLanguages.ContainsKey(node2.Attributes["key"].Value))
                {
                    this._supportedLanguages.Add(node2.Attributes["key"].Value, node2.Attributes["name"].Value);
                }
            }
        }

        internal void GetProviders(XmlNode node, Hashtable table)
        {
            foreach (XmlNode node2 in node.ChildNodes)
            {
                string name = node2.Name;
                if (name != null)
                {
                    if (!(name == "add"))
                    {
                        if (name == "remove")
                        {
                            goto Label_0078;
                        }
                        if (name == "clear")
                        {
                            goto Label_0095;
                        }
                    }
                    else
                    {
                        table.Add(node2.Attributes["name"].Value, new Provider(node2.Attributes));
                    }
                }
                continue;
            Label_0078:
                table.Remove(node2.Attributes["name"].Value);
                continue;
            Label_0095:
                table.Clear();
            }
        }

        internal void LoadValuesFromConfigurationXml()
        {
        }

        //public AppLocation AppLocation
        //{
        //    get
        //    {
        //        return this.app;
        //    }
        //}

        public int CacheFactor
        {
            get
            {
                return 5;
            }
        }

        public string[] DefaultRoles
        {
            get
            {
                return this._defaultRoles;
            }
        }

        public string FilesPath
        {
            get
            {
                return this.filesPath;
            }
        }

        public Dictionary<string, string> IntegratedApplications
        {
            get
            {
                return this._integratedApplications;
            }
        }

        public string PasswordEncodingFormat
        {
            get
            {
                return this.passwordEncodingFormat;
            }
        }

        public Hashtable Providers
        {
            get
            {
                return this.providers;
            }
        }

        public int QueuedThreads
        {
            get
            {
                return this.threadCount;
            }
        }

        public short SmtpServerConnectionLimit
        {
            get
            {
                return this.smtpServerConnectionLimit;
            }
        }

        public Dictionary<string, string> SupportedLanguages
        {
            get
            {
                return this._supportedLanguages;
            }
        }
    }
}