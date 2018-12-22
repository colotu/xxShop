namespace YSWL.Web.Validator
{
    using System;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class DropDownListClientValidator : ClientValidator
    {
        internal override ValidateRenderControl GenerateAppendScript()
        {
            return new ValidateRenderControl();
        }

        internal override ValidateRenderControl GenerateInitScript()
        {
            ValidateRenderControl control = new ValidateRenderControl();
            WebControl control2 = (WebControl) base.Owner.NamingContainer.FindControl(base.Owner.ControlToValidate);
            if (control2 != null)
            {
                control2.Attributes.Add("groupname", base.Owner.TargetClientId);
                control.Text = string.Format(CultureInfo.InvariantCulture, "initValid(new SelectValidator('{0}', {1}, '{2}'", new object[] { base.Owner.TargetClientId, base.Owner.Nullable ? "true" : "false", base.Owner.Nullable ? this.ErrorMessage : (base.Owner.RequiredMessage + "<br/>" + this.ErrorMessage) });
                if (base.Owner.Nullable)
                {
                    control.Text = control.Text + string.Format(CultureInfo.InvariantCulture, ", '{0}'", new object[] { base.Owner.NullMessage });
                }
                control.Text = control.Text + "));";
            }
            return control;
        }
    }
}

