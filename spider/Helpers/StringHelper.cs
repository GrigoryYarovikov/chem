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

        static public int ToNaturalOrZero(this string s)
        {
            var i = 0;
            Int32.TryParse(s, out i);
            return i;
        }

        static public double ToMassNumber(this string s)
        {
            var i = .0;
            Double.TryParse(s.CleanNumberString(), out i);
            return i;
        }

        static public string CleanNumberString(this string s)
        {
            var cArr = s.Select(x =>
            {
                if (x >= '0' && x <= '9' || x == ',')
                    return x;
                if (x == '.')
                    return ',';
                return ' ';
            }).Where(x => x != ' ').ToArray();
            return new String(cArr);
        }
    }
}
