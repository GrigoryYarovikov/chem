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
using System.Diagnostics;

namespace Spider
{
    class CharChemSubstSpider : BaseSpider
    {
        SubstanceManager _substances;
        CategoryManager _categories;

        public CharChemSubstSpider()
        {
            _substances = new SubstanceManager();
            _categories = new CategoryManager(_substances.GetContext());
        }

        public override string Crawl()
        {
            var timer = new Stopwatch();
            timer.Start();

            string ans = "";

            var substSet = new SortedSet<SpiderSubstance>();
          
            var addr = "http://easychem.org/ru/subst-ref/?cat0=";
            for (int i = 1; i < 129; ++i)
            {
                var crawler = GetCrawler();
                var uri = new Uri(addr + i + "&pg=1");
                var cToken = new CancellationTokenSource();

                crawler.CrawlBag.elements = new ConcurrentBag<SpiderSubstance>();
                var result = crawler.Crawl(uri, cToken);

                var elements = (crawler.CrawlBag.elements as ConcurrentBag<SpiderSubstance>);
                
                foreach (var item in elements)
                    substSet.Add(item);

                if (result.ErrorOccurred)
                {
                    ans += String.Format("Crawl of {0} completed with error: {1}\n",
                        result.RootUri.AbsoluteUri,
                        result.ErrorException.Message);
                }
            }

            Console.WriteLine(String.Format("Time elapsed : {0}, PARSED", timer.Elapsed.TotalMinutes));

            var contextSet = new SortedSet<Substance>( _substances.GetAll());
           
            Console.WriteLine(String.Format("Time elapsed : {0}, SET_CREATED", timer.Elapsed.TotalMinutes));
           
            var trueSubstList = substSet.Select(x =>
                {
                    return new Substance
                    {
                        CAS = x.CAS,
                        Formula = x.BruttoFormula,
                        Names = x.Names.Select(n => { return new SubstanceName(n); }).ToList(),
                        Scheme = x.Formulas.Select(f => { return new SubstanceScheme(f); }).ToList(),
                        Categories = x.Categories.Select(c =>
                        {
                            return _categories.GetAll().FirstOrDefault(z => z.Name == c);
                        }).Where(v => v != null).ToList()
                    };
                });

            Console.WriteLine(String.Format("Time elapsed : {0}, TRUE_LIST_Q", timer.Elapsed.TotalMinutes));
            var listToAdd = new List<Substance>();
            foreach (var item in trueSubstList)
            {
                if (contextSet.Add(item))
                    listToAdd.Add(item);
            }
            Console.WriteLine(String.Format("Time elapsed : {0}, ALL_ADD", timer.Elapsed.TotalMinutes));

            _substances.AddMany(listToAdd);
            Console.WriteLine(String.Format("Time elapsed : {0}, elements found: {1}", timer.Elapsed.TotalMinutes, listToAdd.Count()));
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

            var elements = ParseSubstance(crawledPage.Content.Text);
            foreach (var item in elements)
                (e.CrawlContext.CrawlBag.elements as ConcurrentBag<SpiderSubstance>).Add(item);

            //e.CrawlContext.CancellationTokenSource.Cancel();
        }

        IEnumerable<SpiderSubstance> ParseSubstance(string page)
        {
            CQ dom = page;
            var items = dom["div[itemscope]"];
            if (items.Length != 0)
            {
                var elem = items.Select(x =>
                {
                    CQ elemDom = x.InnerHTML;
                    return new SpiderSubstance
                    {
                        Names = elemDom[".names dl span"].Elements.Select(y => y.InnerText.HtmlDecode().CutStar()).ToArray(),
                        Categories = elemDom[".categ a"].Elements.Select(y => y.InnerText.HtmlDecode().CutStar()).ToArray(),
                        BruttoFormula = elemDom[".subst-brutto"].First().Text(),
                        CAS = elemDom[".cas-rn"].First().Text(),
                        Formulas = elemDom[".formula-text"].Elements.Select(y => y.InnerText).ToArray()
                    };
                });
                return elem;
            }
            return new List<SpiderSubstance>();
        }

        protected override PoliteWebCrawler SetRules(PoliteWebCrawler crawler)
        {
            crawler.ShouldCrawlPage((pageToCrawl, crawlContext) =>
            {
                Regex regex = new Regex(@"http:\/\/easychem\.org\/ru\/subst-ref\/\?cat0=\d+&pg=\d");
                if (!regex.IsMatch(pageToCrawl.Uri.AbsoluteUri))
                    return new CrawlDecision { Allow = false, Reason = "Нужно парсить только списки выдачи" };

                return new CrawlDecision { Allow = true };
            });

            return crawler;
        }
    }
}
