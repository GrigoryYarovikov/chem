using Abot.Crawler;
using Abot.Poco;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using CsQuery;
using System.Net;
using Chem.Models;
using Common.Helpers;
using Chem.Managers;

namespace Spider
{
    class MendeleevSpider : BaseSpider
    {
        ElementManager _manager;

        public MendeleevSpider(): base()
        {
            _manager = new ElementManager();
        }

        public override string Crawl()
        {
            var crawler = GetCrawler();

            var uri = new Uri("http://himik.pro/tables/mendeleev-table.html");
            var cToken = new CancellationTokenSource();

            crawler.CrawlBag.elements = new ConcurrentBag<Element>();
            var result = crawler.Crawl(uri, cToken);

            var elements = crawler.CrawlBag.elements as ConcurrentBag<Element>;
            _manager.AddMany(elements.ToList());

            if (result.ErrorOccurred)
            {
                return String.Format("Crawl of {0} completed with error: {1}",
                    result.RootUri.AbsoluteUri,
                    result.ErrorException.Message);
            }
            else
            {
                return null;
            }
        }

        protected override void crawler_ProcessPageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {
            var crawledPage = e.CrawledPage;

            if (crawledPage.WebException != null || crawledPage.HttpWebResponse.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Crawl of page failed {0}", crawledPage.Uri.AbsoluteUri);
            else
                Console.WriteLine("Crawl of page succeeded {0}", crawledPage.Uri.AbsoluteUri);

            if (string.IsNullOrEmpty(crawledPage.Content.Text))
            {
                Console.WriteLine("Page had no content {0}", crawledPage.Uri.AbsoluteUri);
                return;
            }

            CQ dom = crawledPage.Content.Text;
            var cells = dom[".cell"];
            var elemList = cells.Select(x =>
            {
                CQ elemDom = x.InnerHTML;
                return new Element
                {
                    Sign = elemDom[".elem1, .elem2, .elem3, .elem4"].First().Text(),
                    Name = elemDom[".capt a"].First().Text(),
                    Weight = elemDom[".massa"].First().Text().ToMassNumber()
                };
            }).Where(x => x.Weight != 0).ToList();

            foreach (var item in elemList)
                (e.CrawlContext.CrawlBag.elements as ConcurrentBag<Element>).Add(item);

            e.CrawlContext.CancellationTokenSource.Cancel();
        }
    }
}
