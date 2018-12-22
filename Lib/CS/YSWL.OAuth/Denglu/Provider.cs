using System;

namespace YSWL.OAuth
{
    public class Provider
    {
        public Provider(String authUrl, String bindUrl, int mediaID, String mediaName)
        {
            this.authUrl = authUrl;
            this.bindUrl = bindUrl;
            this.mediaID = mediaID;
            this.mediaName = mediaName;
        }

        private String authUrl;

        public String AuthUrl
        {
            get { return authUrl; }
            set { authUrl = value; }
        }

        private String bindUrl;

        public String BindUrl
        {
            get { return bindUrl; }
            set { bindUrl = value; }
        }

        private int mediaID;

        public int MediaID
        {
            get { return mediaID; }
            set { mediaID = value; }
        }

        private String mediaName;

        public String MediaName
        {
            get { return mediaName; }
            set { mediaName = value; }
        }
    }
}