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
            var spider = new CharChemSpider();
            var result = spider.Crawl();
        }
    }
}
