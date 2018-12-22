namespace YSWL.Web.Validator
{
    using System;
    using System.Globalization;

    public class InputStringClientValidator : ClientValidator
    {
        private int lowerBound;
        private string regex;
        private int upperBound;

        internal override ValidateRenderControl GenerateAppendScript()
        {
            return new ValidateRenderControl();
        }

        internal override ValidateRenderControl GenerateInitScript()
        {
            ValidateRenderControl control = new ValidateRenderControl {
                Text = string.Format(CultureInfo.InvariantCulture, "initValid(new InputValidator('{0}', {1}, {2}, {3}, {4}, '{5}', '{6}'", new object[] { base.Owner.TargetClientId, this.LowerBound, this.UpperBound, base.Owner.Nullable ? "true" : "false", string.IsNullOrEmpty(this.Regex) ? "null" : ("'" + this.Regex + "'"), base.Owner.Nullable ? base.Owner.FocusMessage : (base.Owner.RequiredAlt + "<br/>" + base.Owner.FocusMessage), base.Owner.Nullable ? this.ErrorMessage : (base.Owner.RequiredMessage + "<br/>" + this.ErrorMessage) })
            };
            if (base.Owner.Nullable)
            {
                control.Text = control.Text + string.Format(CultureInfo.InvariantCulture, ", '{0}'", new object[] { base.Owner.NullMessage });
            }
            control.Text = control.Text + "));";
            return control;
        }

        public int LowerBound
        {
            get
            {
                return this.lowerBound;
            }
            set
            {
                this.lowerBound = value;
            }
        }

        public string Regex
        {
            get
            {
                return this.regex;
            }
            set
            {
                this.regex = value;
            }
        }

        public int UpperBound
        {
            get
            {
                return this.upperBound;
            }
            set
            {
                this.upperBound = value;
            }
        }
    }
}

