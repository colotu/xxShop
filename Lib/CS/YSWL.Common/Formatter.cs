using System.Globalization;

namespace YSWL.Common
{
    public sealed class Formatter
    {
        private Formatter()
        {
        }

        public static string FormatErrorMessage(string msg)
        {
            return string.Format(CultureInfo.InvariantCulture, "<li>{0}", new object[] { msg });
        }

        public static string Whitespace(int height, int width, bool preBreak, bool postBreak)
        {
            string str = string.Format(CultureInfo.InvariantCulture, "<img width=\"{1}\" height=\"{0}\" src=\"" + Globals.ApplicationPath + "/Utility/images/1x1.gif\" />", new object[] { height, width });
            if (preBreak)
            {
                str = "<br />" + str;
            }
            if (postBreak)
            {
                str = str + "<br />";
            }
            return str;
        }
    }
}