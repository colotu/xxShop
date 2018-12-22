namespace YSWL.Web.Validator
{
    using System;
    using System.Globalization;

    public class InputMoneyClientValidator : ClientValidator
    {
        internal override ValidateRenderControl GenerateAppendScript()
        {
            return new ValidateRenderControl();
        }

        internal override ValidateRenderControl GenerateInitScript()
        {
            ValidateRenderControl control = new ValidateRenderControl {
                Text = string.Format(CultureInfo.InvariantCulture, "initValid(new InputValidator('{0}', 1, 10, {1}, '{2}', '{3}', '{4}'", new object[] { base.Owner.TargetClientId, base.Owner.Nullable ? "true" : "false", @"(0|-?[0-9]\\d*(\\.\\d{1,2})?)", base.Owner.Nullable ? base.Owner.FocusMessage : (base.Owner.RequiredAlt + "<br/>" + base.Owner.FocusMessage), base.Owner.Nullable ? this.ErrorMessage : (base.Owner.RequiredMessage + "<br/>" + this.ErrorMessage) })
            };
            if (base.Owner.Nullable)
            {
                control.Text = control.Text + string.Format(CultureInfo.InvariantCulture, ", '{0}'", new object[] { base.Owner.NullMessage });
            }
            control.Text = control.Text + "));";
            return control;
        }
    }
}

