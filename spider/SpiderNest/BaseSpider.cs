using Abot.Crawler;
using Abot.Poco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Spider
{
    public class BaseSpider
    {
        Uri _uri;

        public BaseSpider(string uri)
        {
            _uri = new Uri(uri);
        }

        public BaseSpider()
        {
        }

        public virtual string  Crawl()
        {            
            var crawler = GetCrawler();

            var result =  crawler.Crawl(_uri);

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

        protected PoliteWebCrawler GetCrawler()
        {
            var crawler = new PoliteWebCrawler();
            crawler.PageCrawlStartingAsync += crawler_ProcessPageCrawlStarting;
            crawler.PageCrawlCompletedAsync += crawler_ProcessPageCrawlCompleted;
            crawler.PageCrawlDisallowedAsync += crawler_PageCrawlDisallowed;
            crawler.PageLinksCrawlDisallowedAsync += crawler_PageLinksCrawlDisallowed;
            crawler = SetRules(crawler);
            return crawler;
        }

        protected virtual void crawler_ProcessPageCrawlStarting(object sender, PageCrawlStartingArgs e)
        {
            var pageToCrawl = e.PageToCrawl;
            Console.WriteLine("About to crawl link {0} which was found on page {1}", pageToCrawl.Uri.AbsoluteUri, pageToCrawl.ParentUri.AbsoluteUri);
        }

        protected virtual void crawler_ProcessPageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {
            var crawledPage = e.CrawledPage;

            if (crawledPage.WebException != null || crawledPage.HttpWebResponse.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Crawl of page failed {0}", crawledPage.Uri.AbsoluteUri);
            else
                Console.WriteLine("Crawl of page succeeded {0}", crawledPage.Uri.AbsoluteUri);

            if (string.IsNullOrEmpty(crawledPage.Content.Text))
                Console.WriteLine("Page had no content {0}", crawledPage.Uri.AbsoluteUri);

        }

        protected virtual void crawler_PageLinksCrawlDisallowed(object sender, PageLinksCrawlDisallowedArgs e)
        {
            var crawledPage = e.CrawledPage;
            //Console.WriteLine("Did not crawl the links on page {0} due to {1}", crawledPage.Uri.AbsoluteUri, e.DisallowedReason);
        }

        protected virtual void crawler_PageCrawlDisallowed(object sender, PageCrawlDisallowedArgs e)
        {
            var pageToCrawl = e.PageToCrawl;
           // Console.WriteLine("Did not crawl page {0} due to {1}", pageToCrawl.Uri.AbsoluteUri, e.DisallowedReason);
        }

        protected virtual PoliteWebCrawler SetRules(PoliteWebCrawler crawler) 
        {
            return crawler;
        }

    }
}
