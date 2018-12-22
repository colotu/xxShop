namespace YSWL.Controls
{
    using System;
    using System.Web;
    using System.Web.UI.WebControls;

    /// <summary>
    /// Yes or No 单选按钮 基于RadioButtonList
    /// </summary>
    public class YesNoRadioButtonList : RadioButtonList
    {
        public YesNoRadioButtonList()
        {
            this.Items.Clear();
            this.Items.Add(new ListItem((string) HttpContext.GetGlobalResourceObject("Resources", "Yes"), "True"));
            this.Items.Add(new ListItem((string) HttpContext.GetGlobalResourceObject("Resources", "No"), "False"));
            this.RepeatDirection = RepeatDirection.Horizontal;
            this.SelectedValue = true;
        }

        public new bool SelectedValue
        {
            get
            {
                return bool.Parse(base.SelectedValue);
            }
            set
            {
                base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value ? "True" : "False"));
            }
        }
    }
}

