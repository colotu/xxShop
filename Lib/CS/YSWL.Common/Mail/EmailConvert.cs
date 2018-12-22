using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Net.Mail;
using System.Globalization;

namespace YSWL.Common.Mail
{
   public static class EmailConvert
    {
       public static string ToDelimitedString(ICollection collection, string delimiter)
       {
           if (collection == null)
           {
               return string.Empty;
           }
           StringBuilder builder = new StringBuilder();
           if (collection is Hashtable)
           {
               foreach (object obj2 in ((Hashtable)collection).Keys)
               {
                   builder.Append(obj2.ToString() + delimiter);
               }
           }
           if (collection is ArrayList)
           {
               foreach (object obj3 in (ArrayList)collection)
               {
                   builder.Append(obj3.ToString() + delimiter);
               }
           }
           if (collection is string[])
           {
               foreach (string str in (string[])collection)
               {
                   builder.Append(str + delimiter);
               }
           }
           if (collection is MailAddressCollection)
           {
               foreach (MailAddress address in (MailAddressCollection)collection)
               {
                   builder.Append(address.Address + delimiter);
               }
           }
           return builder.ToString().TrimEnd(new char[] { Convert.ToChar(delimiter, CultureInfo.InvariantCulture) });
       }
    }
}
