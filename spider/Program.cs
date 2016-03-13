using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abot.Crawler;
using Abot.Poco;
using System.Net;
using Abot.Core;
using CsQuery;
using Spider.Helpers;

namespace Spider
{
    class Program
    {
        static void Main(string[] args)
        {
            var spider = new MendeleevSpider();
            var result = spider.Crawl();
            if (result != null)
            {
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine("Mendeleev crawl success");
            }

//            var html = @"<h3>
//            <div id='lib_presta'>
//                Chambre standard 1 pers du <span class=''>03/03/2014</span>  au <span class=''>05/03/2014 </span>
//            </div>
//            <div id='prix_presta'>
//                127.76 &euro;
//            </div>
//        </h3><h3>
//            <div id='lib_presta'>
//                Chambre standard 2 pers du <span class=''>03/03/2014</span>  au <span class=''>05/03/2014 </span>
//            </div>
//            <div id='prix_presta'>
//                227.76 &euro;
//            </div>
//<div class='subst-cat-owners-box'>
//		  <a title='Органическое вещество, Органические соединения' href='/ru/subst-ref/?cat=1'>Органическое вещество*</a> » <a href='/ru/subst-ref/?cat=9'>Карбоновые кислоты</a> » <b>Оксикислоты</b>		
//		  <a title='Органическое вещество, Органические соединения' href='/ru/subst-ref/?cat=1'>Органическое вещество*</a> » <a href='/ru/subst-ref/?cat=9'>Карбоновые кислоты</a> » <b>Оксикислоты</b>		</div>
//
//</div>
//        </h3>";
//            CQ dom = html;

//            var libs = dom[".subst-cat-owners-box a,b"];
//            //var prixs = dom["#prix_presta"];
//            var ttt = libs.Select(x => x.InnerText.HtmlDecode()).Aggregate((x,y) => x + ' ' + y);

//            //var list = libs.Zip(prixs, (k, v) => new { k, v })
//            //  .Select(h => new { HotelName = h.k.InnerText.Trim(), Price = h.v.InnerText.Trim() });
        }
        
    }
}
