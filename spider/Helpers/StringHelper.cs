using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Helpers
{
    public static class StringHelper
    {
        static public string HtmlDecode(this string s)
        {
            return System.Net.WebUtility.HtmlDecode(s);
        }
        
    }
}
