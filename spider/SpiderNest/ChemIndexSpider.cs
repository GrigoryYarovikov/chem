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
using Spider.Helpers;
using Chem.Managers;
using System.Text.RegularExpressions;

namespace Spider
{
    class ChemIndexSpider : BaseSpider
    {
        SubstanceManager _substances;

        public ChemIndexSpider()
        {
            _substances = new SubstanceManager();
        }

        public override string Crawl()
        {
            string ans = "";

            var catList = new List<SpiderSubstanceCI>();

            var addr = "http://www.chemindex.com/";

            foreach (var substance in _substances.GetAll())
            {
                if (!substance.CAS.HasValue())
                    continue;

                var crawler = GetCrawler();

                var uri = new Uri(addr + substance.CAS + "-cas.html");
                var cToken = new CancellationTokenSource();

                crawler.CrawlBag.elements = new ConcurrentBag<SpiderSubstanceCI>();
                var result = crawler.Crawl(uri, cToken);

                var element = (crawler.CrawlBag.elements as ConcurrentBag<SpiderSubstanceCI>).First();
                if (element.Name.Length == 0)
                    element.Name = "sas";
                element.CatId = i;
                catList.Add(element);

                if (result.ErrorOccurred)
                {
                    ans += String.Format("Crawl of {0} completed with error: {1}\n",
                        result.RootUri.AbsoluteUri,
                        result.ErrorException.Message);
                }
            }
            var trueCatList = new List<Category>();
            foreach (var item in catList)
            {
                trueCatList.Add(new Category
                {
                    Name = item.Name
                });
            }
            foreach (var item in catList)
            {
                var cat = trueCatList.First(x => x.Name == item.Name);
                if (item.Parents != null)
                    cat.Parents = trueCatList.Where(x => item.Parents.Contains(x.Name)).ToList();
            }

            //_categories.AddMany(trueCatList);
            return ans;
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

            var element = ParsePage(crawledPage.Content.Text);
            (e.CrawlContext.CrawlBag.elements as ConcurrentBag<SpiderSubstanceCI>).Add(element);

            e.CrawlContext.CancellationTokenSource.Cancel();
        }

        SpiderSubstanceCI ParsePage(string page)
        {
            CQ dom = page;
            var items = dom[".subst-cat-owners-box"];
            if (items.Any())
            {
                var elem = items.Select(x =>
                {
                    CQ elemDom = x.InnerHTML;
                    return new SpiderSubstanceCI
                    {
                        Name = elemDom["b:contains('test')"].First().Text().CutStar(),
                        Parents = elemDom["a"].Elements.Select(y => y.InnerText.HtmlDecode().CutStar()).ToList()
                    };
                }).Aggregate((first, second) => { first.Parents.AddRange(second.Parents); return first; });
                return elem;
            }
            else
            {
                items = dom["h1"];
                var elem = new SpiderSubstanceCI
                {
                    Name = items.First().Text().Split(',').First().CutStar()
                };
                return elem;
            }
        }
    }
}
