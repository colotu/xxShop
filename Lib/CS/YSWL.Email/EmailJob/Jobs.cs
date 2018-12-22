using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Threading;
using System.Xml;
using System.Globalization;
using YSWL.Email.EmailJob.Configuration;

namespace YSWL.Email.EmailJob
{
    public class Jobs : IDisposable
    {
        private DateTime _completed;
        private DateTime _created = DateTime.Now;
        private static int _instancesOfParent;
        //private bool _isRunning;
        private static readonly Jobs _jobs = new Jobs();
        private DateTime _started;
        private bool disposed;
        private int Interval = 0xdbba0;
        private Hashtable jobList = new Hashtable();
        private Timer singleTimer;

        private Jobs()
        {
        }

        private void call_back(object state)
        {
            //this._isRunning = true;
            this._started = DateTime.Now;
            this.singleTimer.Change(-1, -1);
            foreach (Job job in this.jobList.Values)
            {
                if (job.Enabled && job.SingleThreaded)
                {
                    job.ExecuteJob();
                }
            }
            this.singleTimer.Change(this.Interval, this.Interval);
            //this._isRunning = false;
            this._completed = DateTime.Now;
        }

        public void Dispose()
        {
            if ((this.singleTimer != null) && !this.disposed)
            {
                lock (this)
                {
                    this.singleTimer.Dispose();
                    this.singleTimer = null;
                    this.disposed = true;
                }
            }
        }

        public static Jobs Instance()
        {
            return _jobs;
        }

        public bool IsJobEnabled(string jobName)
        {
            if (!this.jobList.Contains(jobName))
            {
                return false;
            }
            return ((Job) this.jobList[jobName]).Enabled;
        }

        public void Start()
        {
            Interlocked.Increment(ref _instancesOfParent);
            lock (this.jobList.SyncRoot)
            {
                if (this.jobList.Count == 0)
                {
                    XmlNode configSection = MsConfiguration.GetConfig().GetConfigSection("YSWL/Jobs");
                    bool flag = true;
                    XmlAttribute attribute = configSection.Attributes["singleThread"];
                    if (((attribute != null) && !string.IsNullOrWhiteSpace(attribute.Value)) && (string.Compare(attribute.Value, "false", true, CultureInfo.InvariantCulture) == 0))
                    {
                        flag = false;
                    }
                    else
                    {
                        flag = true;
                        XmlAttribute attribute2 = configSection.Attributes["minutes"];
                        if ((attribute2 != null) && !string.IsNullOrWhiteSpace(attribute2.Value))
                        {
                            try
                            {
                                this.Interval = int.Parse(attribute2.Value, CultureInfo.InvariantCulture) * 0xea60;
                            }
                            catch
                            {
                                this.Interval = 0xdbba0;
                            }
                        }
                    }
                    foreach (XmlNode node2 in configSection.ChildNodes)
                    {
                        if ((configSection.NodeType != XmlNodeType.Comment) && (node2.NodeType != XmlNodeType.Comment))
                        {
                            XmlAttribute attribute3 = node2.Attributes["type"];
                            XmlAttribute attribute4 = node2.Attributes["name"];
                            Type ijob = Type.GetType(attribute3.Value);
                            if ((ijob != null) && !this.jobList.Contains(attribute4.Value))
                            {
                                Job job = new Job(ijob, node2);
                                this.jobList[attribute4.Value] = job;
                                if (!flag || !job.SingleThreaded)
                                {
                                    job.InitializeTimer();
                                }
                            }
                        }
                    }
                    if (flag)
                    {
                        this.singleTimer = new Timer(new TimerCallback(this.call_back), null, this.Interval, this.Interval);
                    }
                }
            }
        }

        public void Stop()
        {
            Interlocked.Decrement(ref _instancesOfParent);
            if ((_instancesOfParent <= 0) && (this.jobList != null))
            {
                lock (this.jobList.SyncRoot)
                {
                    foreach (Job job in this.jobList.Values)
                    {
                        job.Dispose();
                    }
                    this.jobList.Clear();
                    if (this.singleTimer != null)
                    {
                        this.singleTimer.Dispose();
                        this.singleTimer = null;
                    }
                }
            }
        }

        public Hashtable CurrentJobs
        {
            get
            {
                return this.jobList;
            }
        }
    }
}