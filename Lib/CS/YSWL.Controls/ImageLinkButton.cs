
namespace YSWL.Controls
{
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// 图片按钮扩展类 基于LinkButton
    /// </summary>
    public class ImageLinkButton : LinkButton
    {
        private string alt;
        private string imageFormat = "<img border=\"0\" src=\"{0}\" alt=\"{1}\" />";
        private YSWL.Controls.ImagePosition position;
        private bool showText = true;

        private string GetImageTag()
        {
            if (string.IsNullOrWhiteSpace(this.ImageUrl))
            {
                return string.Empty;
            }
            return string.Format(CultureInfo.InvariantCulture, this.imageFormat, new object[] { this.ImageUrl, this.Alt });
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Attributes.Add("name", this.NamingContainer.UniqueID + "$" + this.ID);
            string imageTag = this.GetImageTag();
            if (!this.ShowText)
            {
                base.Text = "";
            }
            if (this.ImagePosition == YSWL.Controls.ImagePosition.Right)
            {
                base.Text = base.Text + imageTag;
            }
            else
            {
                base.Text = imageTag + base.Text;
            }
            base.Render(writer);
        }

        public string Alt
        {
            get
            {
                return this.alt;
            }
            set
            {
                this.alt = value;
            }
        }

        public YSWL.Controls.ImagePosition ImagePosition
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;
            }
        }

        public string ImageUrl
        {
            get
            {
                if (this.ViewState["Src"] != null)
                {
                    return (string) this.ViewState["Src"];
                }
                return null;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    string relativeUrl = value;
                    if (relativeUrl.StartsWith("~"))
                    {
                        relativeUrl = base.ResolveUrl(relativeUrl);
                    }
                    this.ViewState["Src"] = relativeUrl;
                }
                else
                {
                    this.ViewState["Src"] = null;
                }
            }
        }

        public bool ShowText
        {
            get
            {
                return this.showText;
            }
            set
            {
                this.showText = value;
            }
        }
    }

    /// <summary>
    /// 图片位置
    /// </summary>
    public enum ImagePosition
    {
        Left,
        Right
    }
}

