using Abot.Crawler;
using Abot.Poco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider
{
    class CharChemSpider : BaseSpider
    {
        public CharChemSpider()
            : base("http://www.stolet.ru")
        {

        }

        protected override void crawler_ProcessPageCrawlStarting(object sender, PageCrawlStartingArgs e)
        {
            Console.WriteLine("SINA");
        }

        protected override PoliteWebCrawler SetRules(PoliteWebCrawler crawler)
        {
            crawler.ShouldCrawlPage((pageToCrawl, crawlContext) =>
            {
                if (pageToCrawl.Uri.AbsoluteUri.Contains("ghost"))
                    return new CrawlDecision { Allow = false, Reason = "Scared of ghosts" };

                return new CrawlDecision { Allow = true };
            });

            //crawler.ShouldDownloadPageContent((crawledPage, crawlContext) =>
            //{
            //    if (crawlContext.CrawledCount >= 5)
            //        return new CrawlDecision { Allow = false, Reason = "We already downloaded the raw page content for 5 pages" };

            //    return new CrawlDecision { Allow = true };
            //});

            //crawler.ShouldCrawlPageLinks((crawledPage, crawlContext) =>
            //{
            //    if (!crawledPage.IsInternal)
            //        return new CrawlDecision { Allow = false, Reason = "We dont crawl links of external pages" };

            //    return new CrawlDecision { Allow = true };
            //});

            return crawler;
        }
    }
}
