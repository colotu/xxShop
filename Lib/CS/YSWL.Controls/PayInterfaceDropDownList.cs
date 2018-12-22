using System.Web.UI.WebControls;
using YSWL.Payment.Configuration;

namespace YSWL.Controls
{
    
    /// <summary>
    /// 支付模块 支付接口下拉列表扩展类
    /// </summary>
    public class PayInterfaceDropDownList : DropDownList
    {
        private bool allowNull = true;
        private string nullToDisplay = "";

        public override void DataBind()
        {
            PayConfiguration config = PayConfiguration.GetConfig();
            GatewayProvider provider = null;
            for (int i = 0; i < config.Keys.Count; i++)
            {
                provider = config.Providers[config.Keys[i]] as GatewayProvider;
                this.Items.Add(new ListItem(provider.DisplayName, provider.Name));
            }
            if (this.AllowNull)
            {
                this.Items.Insert(0, new ListItem(this.NullToDisplay, ""));
            }
        }

        public bool AllowNull
        {
            get
            {
                return this.allowNull;
            }
            set
            {
                this.allowNull = value;
            }
        }

        public string NullToDisplay
        {
            get
            {
                return this.nullToDisplay;
            }
            set
            {
                this.nullToDisplay = value;
            }
        }
    }
}

