using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteProperty.Common.Extentions
{
    public static class StringHelper
    {
        public static string ToShortString(this string value)
        {
            value = value.Trim();
            if (value.Length > 20)
            {
                value = value.Substring(0, 15) + "...";
            }
            return value;
        }
    }
}
